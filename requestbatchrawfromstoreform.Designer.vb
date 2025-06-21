<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class requestbatchrawfromstoreform
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
        Me.cmbWorder = New System.Windows.Forms.ComboBox()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.WebBrowser = New System.Windows.Forms.WebBrowser()
        Me.lblSelectedWorkOrders = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cmbWorder
        '
        Me.cmbWorder.Font = New System.Drawing.Font("Tahoma", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbWorder.FormattingEnabled = True
        Me.cmbWorder.Location = New System.Drawing.Point(550, 80)
        Me.cmbWorder.Name = "cmbWorder"
        Me.cmbWorder.Size = New System.Drawing.Size(240, 30)
        Me.cmbWorder.TabIndex = 0
        '
        'btnGenerate
        '
        Me.btnGenerate.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGenerate.Location = New System.Drawing.Point(615, 667)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(108, 51)
        Me.btnGenerate.TabIndex = 1
        Me.btnGenerate.Text = "طباعه"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'WebBrowser
        '
        Me.WebBrowser.Location = New System.Drawing.Point(68, 147)
        Me.WebBrowser.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser.Name = "WebBrowser"
        Me.WebBrowser.Size = New System.Drawing.Size(1157, 482)
        Me.WebBrowser.TabIndex = 2
        '
        'lblSelectedWorkOrders
        '
        Me.lblSelectedWorkOrders.AutoSize = True
        Me.lblSelectedWorkOrders.Location = New System.Drawing.Point(121, 43)
        Me.lblSelectedWorkOrders.Name = "lblSelectedWorkOrders"
        Me.lblSelectedWorkOrders.Size = New System.Drawing.Size(47, 17)
        Me.lblSelectedWorkOrders.TabIndex = 3
        Me.lblSelectedWorkOrders.Text = "Label1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(611, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(93, 21)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "أمر الشغل"
        '
        'requestbatchrawfromstoreform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1336, 900)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblSelectedWorkOrders)
        Me.Controls.Add(Me.WebBrowser)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.cmbWorder)
        Me.Name = "requestbatchrawfromstoreform"
        Me.Text = "requestbatchrawfromstoreform"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbWorder As ComboBox
    Friend WithEvents btnGenerate As Button
    Friend WithEvents WebBrowser As WebBrowser
    Friend WithEvents lblSelectedWorkOrders As Label
    Friend WithEvents Label1 As Label
End Class
