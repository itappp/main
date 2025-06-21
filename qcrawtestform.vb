Imports System.Data.SqlClient
Public Class qcrawtestform
    Private sqlServerConnectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"


    Private Sub qcrawtestform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Populate cmbpva
        cmbpva.Items.AddRange(New String() {"no", "starch", "pva", "starch\pva", "Copon", "مرتجع"})


        cmbtensile.Items.AddRange(New String() {"Accept", "Reject", "Copon", "مرتجع"})

        cmbtear.Items.AddRange(New String() {"Accept", "Reject", "Copon", "مرتجع"})
        cmbcolorwater.Items.AddRange(New String() {"Accept", "Reject", "Copon", "مرتجع"})
        cmbwashing.Items.AddRange(New String() {"Accept", "Reject", "Copon", "مرتجع"})
        cmbcolormercerize.Items.AddRange(New String() {"Accept", "Reject", "Copon", "مرتجع"})
        cmbopenclose.Items.AddRange(New String() {"مفتوح", "مقفول"})

        ' Populate fabric combo boxes
        PopulateFabricComboBox(cmbfabric1)
        PopulateFabricComboBox(cmbfabric2)
        PopulateFabricComboBox(cmbfabric3)

        ' Populate fabric type combo box
        PopulateFabricTypeComboBox()

        ' Ensure no item is selected by default
        cmbfabric1.SelectedIndex = -1
        cmbfabric2.SelectedIndex = -1
        cmbfabric3.SelectedIndex = -1
        cmbkindfab.SelectedIndex = -1
        cmbopenclose.SelectedIndex = -1

        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Retrieve the public_name for the logged-in user
        Dim publicName As String = GetPublicName(LoggedInUsername)
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

    Public Class FabricType
        Public Property Id As Integer
        Public Property FabricTypeAr As String

        Public Overrides Function ToString() As String
            Return FabricTypeAr
        End Function
    End Class

    Private Sub PopulateFabricTypeComboBox()
        Using connection As New SqlConnection(sqlServerConnectionString)
            Dim command As New SqlCommand("SELECT id, fabrictype_ar FROM fabric", connection)
            connection.Open()
            Dim reader As SqlDataReader = command.ExecuteReader()
            Dim fabricTypes As New List(Of FabricType)
            While reader.Read()
                fabricTypes.Add(New FabricType With {.Id = Convert.ToInt32(reader("id")), .FabricTypeAr = reader("fabrictype_ar").ToString()})
            End While
            cmbkindfab.DataSource = fabricTypes
        End Using

        cmbkindfab.DisplayMember = "FabricTypeAr"
        cmbkindfab.ValueMember = "Id"
    End Sub

    Private Function IsBatchAndLotAlreadyRecorded(batchId As String, lot As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM qc_raw_test WHERE batch_id = @batch_id AND lot = @lot"
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
        If String.IsNullOrEmpty(txtwidthbefor.Text) OrElse String.IsNullOrEmpty(txtwidthafter.Text) OrElse String.IsNullOrEmpty(txtweightbefor.Text) OrElse String.IsNullOrEmpty(txtweightafter.Text) OrElse cmbtensile.SelectedItem Is Nothing OrElse cmbtear.SelectedItem Is Nothing OrElse cmbpva.SelectedItem Is Nothing OrElse cmbcolorwater.SelectedItem Is Nothing OrElse cmbwashing.SelectedItem Is Nothing OrElse cmbcolormercerize.SelectedItem Is Nothing OrElse cmbkindfab.SelectedValue Is Nothing OrElse cmbopenclose.SelectedItem Is Nothing Then
            MessageBox.Show("Please fill in all required fields.")
            Return
        End If

        If cmbfabric1.SelectedValue Is Nothing Then
            MessageBox.Show("Please select a fabric for the first fabric combo box.")
            Return
        End If

        If cmbkindfab.SelectedValue Is Nothing Then
            MessageBox.Show("Please select a fabric type.")
            Return
        End If

        If cmbopenclose.SelectedItem Is Nothing Then
            MessageBox.Show("Please select fabric status (مفتوح/مقفول).")
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

        ' Check if fabric2 is selected
        If cmbfabric2.SelectedValue IsNot Nothing Then
            Dim fabric2Name As String = CType(cmbfabric2.SelectedItem, FabricMaterial).Name
            mixRate &= " \ " & fabric2Name
            If Not String.IsNullOrEmpty(txtprecent2.Text) Then
                mixRate &= " " & txtprecent2.Text
            End If
        End If

        ' Check if fabric3 is selected
        If cmbfabric3.SelectedValue IsNot Nothing Then
            Dim fabric3Name As String = CType(cmbfabric3.SelectedItem, FabricMaterial).Name
            mixRate &= " \ " & fabric3Name
            If Not String.IsNullOrEmpty(txtprecent3.Text) Then
                mixRate &= " " & txtprecent3.Text
            End If
        End If


        Try
            Using connection As New SqlConnection(sqlServerConnectionString)
                Dim command As New SqlCommand("INSERT INTO qc_raw_test (batch_id, lot, raw_befor_width, raw_after_width, raw_befor_weight, raw_after_weight, tensile_weft, tensile_warp, tensile_result, tear_weft, tear_warp, tear_result, pva_Starch, color_water, washing, color_mercerize, mix_rate, style1, precent_style1, style2, precent_style2, style3, precent_style3, kind_fab, fab_status, notes, date,username) VALUES (@batch_id, @lot, @raw_befor_width, @raw_after_width, @raw_befor_weight, @raw_after_weight, @tensile_weft, @tensile_warp, @tensile_result, @tear_weft, @tear_warp, @tear_result, @pva_Starch, @color_water, @washing, @color_mercerize, @mix_rate, @style1, @precent_style1, @style2, @precent_style2, @style3, @precent_style3, @kind_fab, @fab_status, @notes, @date,@username)", connection)
                command.Parameters.AddWithValue("@batch_id", lblbatch.Text)
                command.Parameters.AddWithValue("@lot", lbllot.Text)
                command.Parameters.AddWithValue("@raw_befor_width", txtwidthbefor.Text)
                command.Parameters.AddWithValue("@raw_after_width", txtwidthafter.Text)
                command.Parameters.AddWithValue("@raw_befor_weight", txtweightbefor.Text)
                command.Parameters.AddWithValue("@raw_after_weight", txtweightafter.Text)
                command.Parameters.AddWithValue("@tensile_weft", txttensileweft.Text)
                command.Parameters.AddWithValue("@tensile_warp", txttensilewarp.Text)
                command.Parameters.AddWithValue("@tensile_result", cmbtensile.SelectedItem.ToString())
                command.Parameters.AddWithValue("@tear_weft", txttearweft.Text)
                command.Parameters.AddWithValue("@tear_warp", txttearwarp.Text)
                command.Parameters.AddWithValue("@tear_result", cmbtear.SelectedItem.ToString())
                command.Parameters.AddWithValue("@pva_Starch", cmbpva.SelectedItem.ToString())
                command.Parameters.AddWithValue("@color_water", cmbcolorwater.SelectedItem.ToString())
                command.Parameters.AddWithValue("@washing", cmbwashing.SelectedItem.ToString())
                command.Parameters.AddWithValue("@color_mercerize", cmbcolormercerize.SelectedItem.ToString())
                command.Parameters.AddWithValue("@mix_rate", mixRate)
                command.Parameters.AddWithValue("@style1", cmbfabric1.SelectedValue)
                command.Parameters.AddWithValue("@precent_style1", txtprecent1.Text)
                command.Parameters.AddWithValue("@style2", If(cmbfabric2.SelectedValue Is Nothing, DBNull.Value, cmbfabric2.SelectedValue))
                command.Parameters.AddWithValue("@precent_style2", If(String.IsNullOrEmpty(txtprecent2.Text), DBNull.Value, txtprecent2.Text))
                command.Parameters.AddWithValue("@style3", If(cmbfabric3.SelectedValue Is Nothing, DBNull.Value, cmbfabric3.SelectedValue))
                command.Parameters.AddWithValue("@precent_style3", If(String.IsNullOrEmpty(txtprecent3.Text), DBNull.Value, txtprecent3.Text))
                command.Parameters.AddWithValue("@kind_fab", cmbkindfab.SelectedValue)
                command.Parameters.AddWithValue("@fab_status", cmbopenclose.SelectedItem.ToString())
                command.Parameters.AddWithValue("@notes", txtnotes.Text)
                command.Parameters.AddWithValue("@date", DateTime.Now)
                command.Parameters.AddWithValue("@username", LoggedInUsername)

                connection.Open()
                command.ExecuteNonQuery()
                MessageBox.Show("Record inserted successfully.")
                ' Hide the current form and show the mainrowsampleform
                Me.Hide()
                Dim mainForm As New mainqcrawtestform()
                mainForm.Show()
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub
    Private Function GetPublicName(ByVal username As String) As String
        Dim publicName As String = String.Empty
        Try
            Using conn As New SqlConnection(sqlServerConnectionString)
                ' SQL query to get the public_name from dep_users where username matches
                Dim query As String = "SELECT public_name FROM dep_users WHERE username = @username"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@username", username)

                conn.Open()
                ' Execute the query and retrieve the public_name
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    publicName = result.ToString()
                End If
                conn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error retrieving public name: " & ex.Message)
        End Try
        Return publicName
    End Function

    Private Sub cmbkindfab_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbkindfab.SelectedIndexChanged
        If cmbkindfab.SelectedItem IsNot Nothing Then
            Dim selectedFabricType As FabricType = CType(cmbkindfab.SelectedItem, FabricType)
            ' Check if the selected fabric type is "نسيج"
            If selectedFabricType.FabricTypeAr.Trim().ToLower() = "نسيج" Then
                ' Automatically select "مفتوح" for نسيج
                cmbopenclose.SelectedItem = "مفتوح"
            Else
                ' For other fabric types (like تريكو), clear the selection
                cmbopenclose.SelectedIndex = -1
            End If
        End If
    End Sub

End Class