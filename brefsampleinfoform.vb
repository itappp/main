Imports System.Data.SqlClient

Public Class brefsampleinfoform
    Private sqlServerConnectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"

    Private Sub brefsampleinfoform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Populate cmbpva
        cmbpva.Items.AddRange(New String() {"no", "starch", "pva", "starch\pva"})

        ' Populate cmbSeparation
        cmbSeparation.Items.AddRange(New String() {"عالى - High", "متوسط - Medium", "لايوجد - None"})

        ' Populate cmbDurability
        cmbDurability.Items.AddRange(New String() {"جيده - Good", "متوسطه - Medium", "ضعيفه - Weak"})

        ' Populate fabric combo boxes
        PopulateFabricComboBox(cmbfabric1)
        PopulateFabricComboBox(cmbfabric2)
        PopulateFabricComboBox(cmbfabric3)

        ' Ensure no item is selected by default
        cmbfabric1.SelectedIndex = -1
        cmbfabric2.SelectedIndex = -1
        cmbfabric3.SelectedIndex = -1
    End Sub

    Public Sub SetBatchAndLot(batch As String, lot As String)
        lbllot.Text = lot
        lblbatch.Text = batch
    End Sub

    Private Sub PopulateFabricComboBox(comboBox As ComboBox)
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim command As New SqlCommand("SELECT id, name FROM fabric_material", connection)
            connection.Open()
            Dim reader As SqlDataReader = command.ExecuteReader()
            Dim fabricMaterials As New List(Of FabricMaterial)
            While reader.Read()
                fabricMaterials.Add(New FabricMaterial With {.Id = Convert.ToInt32(reader("id")), .Name = reader("name").ToString()})
            End While
            comboBox.DataSource = fabricMaterials
        End Using

        comboBox.DisplayMember = "Name"
        comboBox.ValueMember = "Id"
    End Sub

    Public Class FabricMaterial
        Public Property Id As Integer
        Public Property Name As String

        Public Overrides Function ToString() As String
            Return Name
        End Function
    End Class

    Private Function IsBatchAndLotAlreadyRecorded(batchId As String, lot As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM batch_lot_defect WHERE batch_id = @batch_id AND lot = @lot"
        Using connection As New SqlConnection(sqlServerConnectionString)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@batch_id", batchId)
                command.Parameters.AddWithValue("@lot", lot)
                connection.Open()
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Private Sub btninsert_Click(sender As Object, e As EventArgs) Handles btninsert.Click
        ' Check if required fields are filled
        If String.IsNullOrEmpty(txtrawmoisture.Text) OrElse String.IsNullOrEmpty(txtwidth.Text) OrElse cmbpva.SelectedItem Is Nothing OrElse cmbSeparation.SelectedItem Is Nothing OrElse cmbDurability.SelectedItem Is Nothing Then
            MessageBox.Show("Please fill in all required fields.")
            Return
        End If

        If cmbfabric1.SelectedValue Is Nothing Then
            MessageBox.Show("Please select a fabric for the first fabric combo box.")
            Return
        End If

        ' Check if batch and lot are already recorded
        If IsBatchAndLotAlreadyRecorded(lblbatch.Text, lbllot.Text) Then
            MessageBox.Show("This batch and lot have already been recorded.")
            Return
        End If

        ' Get the selected fabric name and percentage
        Dim fabric1Name As String = CType(cmbfabric1.SelectedItem, FabricMaterial).Name
        Dim mixRate As String = fabric1Name & " " & txtprecent1.Text

        ' Check if fabric2 and percentage2 are selected/entered
        If cmbfabric2.SelectedValue IsNot Nothing AndAlso Not String.IsNullOrEmpty(txtprecent2.Text) Then
            Dim fabric2Name As String = CType(cmbfabric2.SelectedItem, FabricMaterial).Name
            mixRate &= " \ " & fabric2Name & " " & txtprecent2.Text
        End If

        ' Check if fabric3 and percentage3 are selected/entered
        If cmbfabric3.SelectedValue IsNot Nothing AndAlso Not String.IsNullOrEmpty(txtprecent3.Text) Then
            Dim fabric3Name As String = CType(cmbfabric3.SelectedItem, FabricMaterial).Name
            mixRate &= " \ " & fabric3Name & " " & txtprecent3.Text
        End If

        Try
            Using connection As New SqlConnection(sqlServerConnectionString)
                Dim command As New SqlCommand("INSERT INTO batch_lot_defect (batch_id, lot, Raw_Moisture, raw_width, pva_Starch, style1, style2, style3, needle, Separation, Durability, notes, precent_style1, precent_style2, precent_style3, mix_rate, date) VALUES (@batch_id, @lot, @Raw_Moisture, @raw_width, @pva_Starch, @style1, @style2, @style3, @needle, @Separation, @Durability, @notes, @precent_style1, @precent_style2, @precent_style3, @mix_rate, @date)", connection)
                command.Parameters.AddWithValue("@batch_id", lblbatch.Text)
                command.Parameters.AddWithValue("@lot", lbllot.Text)
                command.Parameters.AddWithValue("@Raw_Moisture", txtrawmoisture.Text)
                command.Parameters.AddWithValue("@raw_width", txtwidth.Text)
                command.Parameters.AddWithValue("@pva_Starch", cmbpva.SelectedItem.ToString())
                command.Parameters.AddWithValue("@style1", cmbfabric1.SelectedValue)
                command.Parameters.AddWithValue("@style2", If(cmbfabric2.SelectedValue Is Nothing, DBNull.Value, cmbfabric2.SelectedValue))
                command.Parameters.AddWithValue("@style3", If(cmbfabric3.SelectedValue Is Nothing, DBNull.Value, cmbfabric3.SelectedValue))
                command.Parameters.AddWithValue("@needle", ckvneedle.Checked)
                command.Parameters.AddWithValue("@Separation", cmbSeparation.SelectedItem.ToString())
                command.Parameters.AddWithValue("@Durability", cmbDurability.SelectedItem.ToString())
                command.Parameters.AddWithValue("@notes", txtnotes.Text)
                command.Parameters.AddWithValue("@precent_style1", txtprecent1.Text)
                command.Parameters.AddWithValue("@precent_style2", If(String.IsNullOrEmpty(txtprecent2.Text), DBNull.Value, txtprecent2.Text))
                command.Parameters.AddWithValue("@precent_style3", If(String.IsNullOrEmpty(txtprecent3.Text), DBNull.Value, txtprecent3.Text))
                command.Parameters.AddWithValue("@mix_rate", mixRate)
                command.Parameters.AddWithValue("@date", DateTime.Now)

                connection.Open()
                command.ExecuteNonQuery()
                MessageBox.Show("Record inserted successfully.")
                ' Hide the current form and show the mainrowsampleform
                Me.Hide()
                Dim mainForm As New mainrowsampleform()
                mainForm.Show()
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub

End Class
