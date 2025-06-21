Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices.RuntimeHelpers
Imports DocumentFormat.OpenXml.Wordprocessing

Public Class recievedchimicform
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub recievedchimicform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Populate cmbCodeType with code types
        cmbCodeType.Items.AddRange(New String() {"DY", "LQ", "PW"})

        ' Populate cmbMovementType with movement types
        cmbMovementType.Items.AddRange(New String() {"إضافة", "صرف", "مرتجع"})

        ' Populate cmbDisributOrder with disribut orders
        PopulateDisributOrders()

        ' Hide cmbDisributOrder initially
        cmbDisributOrder.Visible = False
    End Sub


    Private Sub PopulateDisributOrders()
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Dim query As String = "SELECT DISTINCT addition_order FROM chimic_log WHERE addition_order LIKE 'chimicdisribut-%'"
            Using cmd As New SqlCommand(query, conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        cmbDisributOrder.Items.Add(reader("addition_order").ToString())
                    End While
                End Using
            End Using
        End Using
    End Sub
    Private Sub cmbDisributOrder_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDisributOrder.SelectedIndexChanged
        If cmbMovementType.SelectedItem Is Nothing Then
            MessageBox.Show("يرجى اختيار نوع الحركة أولاً.")
            Return
        End If

        Dim disributOrder As String = cmbDisributOrder.SelectedItem.ToString()
        LoadDisributData(disributOrder)
    End Sub


    Private Sub LoadDisributData(disributOrder As String)
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Dim query As String = "SELECT * FROM chimic_log WHERE addition_order = @disributOrder AND addition_order LIKE 'chimicdisribut-%'"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@disributOrder", disributOrder)
                Using adapter As New SqlDataAdapter(cmd)
                    Dim table As New DataTable()
                    adapter.Fill(table)
                    dgv.DataSource = table
                End Using
            End Using
        End Using
    End Sub

    Private Sub cmbMovementType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMovementType.SelectedIndexChanged
        Dim selectedMovementType As String = cmbMovementType.SelectedItem.ToString()

        If selectedMovementType = "مرتجع" Then
            cmbDisributOrder.Visible = True
            cmbCodeType.Visible = False
            cmbCode.Visible = False
            Label5.Visible = False
            Label1.Visible = False
        Else
            cmbDisributOrder.Visible = False
            cmbCodeType.Visible = True
            cmbCode.Visible = True
            Label5.Visible = True
            Label1.Visible = True
        End If
    End Sub


    Private Sub cmbCodeType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCodeType.SelectedIndexChanged
        If cmbMovementType.SelectedItem Is Nothing Then
            MessageBox.Show("يرجى اختيار نوع الحركة أولاً.")
            Return
        End If

        Dim selectedCodeType As String = cmbCodeType.SelectedItem.ToString()
        LoadCodes(selectedCodeType)
    End Sub


    Private Sub LoadCodes(codeType As String)
        cmbCode.Items.Clear()
        cmbCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbCode.AutoCompleteSource = AutoCompleteSource.CustomSource
        Dim autoCompleteCollection As New AutoCompleteStringCollection()

        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Dim query As String = "SELECT code FROM chimic_items WHERE code LIKE @codeType + '%'"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@codeType", codeType)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim code As String = reader("code").ToString()
                        cmbCode.Items.Add(code)
                        autoCompleteCollection.Add(code)
                    End While
                End Using
            End Using
        End Using

        cmbCode.AutoCompleteCustomSource = autoCompleteCollection
    End Sub

    Private Sub cmbCode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCode.SelectedIndexChanged
        If cmbMovementType.SelectedItem Is Nothing Then
            MessageBox.Show("يرجى اختيار نوع الحركة أولاً.")
            Return
        End If

        Dim selectedCode As String = cmbCode.SelectedItem.ToString()
        LoadBalanceData(selectedCode)
    End Sub


    Private Sub LoadBalanceData(code As String)
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Dim query As String = "SELECT * FROM chimic_balance WHERE code = @code"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@code", code)
                Using adapter As New SqlDataAdapter(cmd)
                    Dim table As New DataTable()
                    adapter.Fill(table)
                    dgv.DataSource = table
                End Using
            End Using
        End Using
    End Sub

    Private Sub btnAddBalance_Click(sender As Object, e As EventArgs) Handles btnAddBalance.Click
        If cmbCode.SelectedItem Is Nothing Then
            MessageBox.Show("يرجى اختيار كود أولاً.")
            Return
        End If

        Dim code As String = cmbCode.SelectedItem.ToString()
        Dim qty As Decimal = Convert.ToDecimal(txtQty.Text)
        Dim numberUnit As Decimal = Convert.ToDecimal(txtNumberUnit.Text)
        Dim weightUnit As Decimal = Convert.ToDecimal(txtWeightUnit.Text)
        Dim movementType As String = cmbMovementType.SelectedItem.ToString()

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            ' Load available quantity from the balance table
            Dim availableQty As Decimal = 0
            Dim loadQuery As String = "SELECT qty FROM chimic_balance WHERE code = @code"
            Using loadCmd As New SqlCommand(loadQuery, conn)
                loadCmd.Parameters.AddWithValue("@code", code)
                Dim result As Object = loadCmd.ExecuteScalar()
                If result IsNot Nothing Then
                    availableQty = Convert.ToDecimal(result)
                End If
            End Using

            ' Check if the quantity is sufficient for the movement type
            If movementType = "صرف" AndAlso qty > availableQty Then
                MessageBox.Show("الكمية المتاحة غير كافية للصرف.")
                Return
            ElseIf movementType = "مرتجع" AndAlso qty > availableQty Then
                MessageBox.Show("الكمية المتاحة غير كافية للمرتجع.")
                Return
            End If

            ' Check if the code already exists in the balance table
            Dim checkQuery As String = "SELECT COUNT(*) FROM chimic_balance WHERE code = @code"
            Using checkCmd As New SqlCommand(checkQuery, conn)
                checkCmd.Parameters.AddWithValue("@code", code)
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                If count > 0 Then
                    ' Update existing record
                    Dim updateQuery As String
                    If movementType = "مرتجع" Then
                        updateQuery = "UPDATE chimic_balance SET qty = qty - @qty, number_unit = number_unit - @numberUnit, weight_unit = weight_unit - @weightUnit WHERE code = @code"
                    Else
                        updateQuery = "UPDATE chimic_balance SET qty = qty + @qty, number_unit = number_unit + @numberUnit, weight_unit = weight_unit + @weightUnit WHERE code = @code"
                    End If
                    Using updateCmd As New SqlCommand(updateQuery, conn)
                        updateCmd.Parameters.AddWithValue("@qty", qty)
                        updateCmd.Parameters.AddWithValue("@numberUnit", numberUnit)
                        updateCmd.Parameters.AddWithValue("@weightUnit", weightUnit)
                        updateCmd.Parameters.AddWithValue("@code", code)
                        updateCmd.ExecuteNonQuery()
                    End Using
                Else
                    ' Insert new record
                    Dim insertQuery As String = "INSERT INTO chimic_balance (code, qty, number_unit, weight_unit) VALUES (@code, @qty, @numberUnit, @weightUnit)"
                    Using insertCmd As New SqlCommand(insertQuery, conn)
                        insertCmd.Parameters.AddWithValue("@code", code)
                        insertCmd.Parameters.AddWithValue("@qty", qty)
                        insertCmd.Parameters.AddWithValue("@numberUnit", numberUnit)
                        insertCmd.Parameters.AddWithValue("@weightUnit", weightUnit)
                        insertCmd.ExecuteNonQuery()
                    End Using
                End If
            End Using

            ' Insert into the log table with appropriate order
            Dim order As String = GetNextOrder(movementType)
            Dim logQuery As String = "INSERT INTO chimic_log (addition_order, code, qty, number_unit, weight_unit) VALUES (@order, @code, @qty, @numberUnit, @weightUnit)"
            Using logCmd As New SqlCommand(logQuery, conn)
                logCmd.Parameters.AddWithValue("@order", order)
                logCmd.Parameters.AddWithValue("@code", code)
                logCmd.Parameters.AddWithValue("@qty", qty)
                logCmd.Parameters.AddWithValue("@numberUnit", numberUnit)
                logCmd.Parameters.AddWithValue("@weightUnit", weightUnit)
                logCmd.ExecuteNonQuery()
            End Using

            MessageBox.Show("Balance updated successfully.")
        End Using
    End Sub

    Private Function GetNextOrder(movementType As String) As String
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Dim query As String
            Dim prefix As String

            Select Case movementType
                Case "إضافة"
                    query = "SELECT MAX(CAST(SUBSTRING(addition_order, 10, LEN(addition_order) - 9) AS INT)) FROM chimic_log WHERE addition_order LIKE 'chimicadd-%'"
                    prefix = "chimicadd-"
                Case "صرف"
                    query = "SELECT MAX(CAST(SUBSTRING(addition_order, 17, LEN(addition_order) - 16) AS INT)) FROM chimic_log WHERE addition_order LIKE 'chimicdisribut-%'"
                    prefix = "chimicdisribut-"
                Case "مرتجع"
                    query = "SELECT MAX(CAST(SUBSTRING(addition_order, 15, LEN(addition_order) - 14) AS INT)) FROM chimic_log WHERE addition_order LIKE 'chimicreturn-%'"
                    prefix = "chimicreturn-"
                Case Else
                    Throw New ArgumentException("Invalid movement type")
            End Select

            Using cmd As New SqlCommand(query, conn)
                Dim maxNumber As Object = cmd.ExecuteScalar()
                Dim nextNumber As Integer = If(IsDBNull(maxNumber), 1, Convert.ToInt32(maxNumber) + 1)
                Return $"{prefix}{nextNumber:D6}"
            End Using
        End Using
    End Function

End Class
