Imports System.Windows.Forms
Imports System.Drawing

Public Module DataGridViewStyles
    Public Sub ApplyLightGrayHeaderStyle(dgv As DataGridView)
        ' Set font and color for the DataGridView header
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240) ' Light gray color
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        
        ' Apply header styles
        dgv.EnableHeadersVisualStyles = False ' Allow custom header styles to take effect
    End Sub
End Module 