Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading.Tasks
Imports System.Threading

Public Class NetworkCameraSearchForm
    Private WithEvents btnSearch As Button
    Private WithEvents dgvResults As DataGridView
    Private WithEvents progressForm As Form
    Private WithEvents progressBar As ProgressBar
    Private WithEvents lblStatus As Label
    Private WithEvents btnCancel As Button
    Private isSearching As Boolean = False
    Private cancellationTokenSource As CancellationTokenSource

    Private Sub NetworkCameraSearchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Network Camera Search"
        Me.Size = New Size(800, 600)

        ' Create and configure the search button
        btnSearch = New Button()
        btnSearch.Text = "Search Cameras"
        btnSearch.Location = New Point(10, 10)
        btnSearch.Size = New Size(120, 30)
        AddHandler btnSearch.Click, AddressOf btnSearch_Click
        Me.Controls.Add(btnSearch)

        ' Create and configure the DataGridView
        dgvResults = New DataGridView()
        dgvResults.Location = New Point(10, 50)
        dgvResults.Size = New Size(760, 500)
        dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvResults.AllowUserToAddRows = False
        dgvResults.ReadOnly = True
        dgvResults.Columns.Add("Name", "Device Name")
        dgvResults.Columns.Add("IP", "IP Address")
        dgvResults.Columns.Add("MAC", "MAC Address")
        Me.Controls.Add(dgvResults)

        ' Create progress form
        progressForm = New Form()
        progressForm.Text = "Searching Cameras"
        progressForm.Size = New Size(300, 150)
        progressForm.FormBorderStyle = FormBorderStyle.FixedDialog
        progressForm.StartPosition = FormStartPosition.CenterParent
        progressForm.ControlBox = False
        progressForm.MaximizeBox = False
        progressForm.MinimizeBox = False

        progressBar = New ProgressBar()
        progressBar.Location = New Point(10, 10)
        progressBar.Size = New Size(260, 30)
        progressForm.Controls.Add(progressBar)

        lblStatus = New Label()
        lblStatus.Location = New Point(10, 50)
        lblStatus.Size = New Size(260, 20)
        lblStatus.Text = "Searching..."
        progressForm.Controls.Add(lblStatus)

        btnCancel = New Button()
        btnCancel.Text = "Cancel"
        btnCancel.Location = New Point(10, 80)
        btnCancel.Size = New Size(100, 30)
        AddHandler btnCancel.Click, AddressOf btnCancel_Click
        progressForm.Controls.Add(btnCancel)
    End Sub

    Private Async Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If isSearching Then Return
        isSearching = True
        btnSearch.Enabled = False
        dgvResults.Rows.Clear()
        cancellationTokenSource = New CancellationTokenSource()

        Try
            progressForm.Show()
            progressBar.Value = 0
            lblStatus.Text = "Searching VLAN4 (140.1.1.1 - 140.1.1.254)..."
            progressBar.Maximum = 254

            ' Create tasks for different search operations
            Dim tasks As New List(Of Task)
            tasks.Add(Task.Run(Function() SearchIPRange("140.1.1.1", "140.1.1.254", cancellationTokenSource.Token)))
            tasks.Add(Task.Run(Function() SearchIP("192.168.1.64", cancellationTokenSource.Token)))
            tasks.Add(Task.Run(Function() SearchMACRange("80:7c", cancellationTokenSource.Token)))

            ' Wait for all tasks to complete
            Await Task.WhenAll(tasks)
        Catch ex As Exception
            MessageBox.Show("Error during search: " & ex.Message)
        Finally
            isSearching = False
            btnSearch.Enabled = True
            progressForm.Hide()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If cancellationTokenSource IsNot Nothing Then
            cancellationTokenSource.Cancel()
        End If
        progressForm.Hide()
        isSearching = False
        btnSearch.Enabled = True
    End Sub

    Private Function SearchIPRange(startIP As String, endIP As String, cancellationToken As CancellationToken) As Task
        Return Task.Run(Sub()
                            Dim startBytes = IPAddress.Parse(startIP).GetAddressBytes()
                            Dim endBytes = IPAddress.Parse(endIP).GetAddressBytes()
                            Dim currentBytes = CType(startBytes.Clone(), Byte())

                            For i As Integer = 0 To 254
                                If cancellationToken.IsCancellationRequested Then Return

                                Dim ip = New IPAddress(currentBytes).ToString()
                                If IsCamera(ip) Then
                                    Dim hostname = GetHostname(ip)
                                    Dim mac = GetMACAddress(ip)
                                    Me.Invoke(Sub()
                                                  dgvResults.Rows.Add(hostname, ip, mac)
                                              End Sub)
                                End If

                                currentBytes(3) += 1
                                Dim progressValue = i + 1
                                Me.Invoke(Sub()
                                              progressBar.Value = progressValue
                                          End Sub)
                            Next
                        End Sub, cancellationToken)
    End Function

    Private Function SearchIP(ip As String, cancellationToken As CancellationToken) As Task
        Return Task.Run(Sub()
                            If cancellationToken.IsCancellationRequested Then Return

                            If IsCamera(ip) Then
                                Dim hostname = GetHostname(ip)
                                Dim mac = GetMACAddress(ip)
                                Me.Invoke(Sub()
                                              dgvResults.Rows.Add(hostname, ip, mac)
                                          End Sub)
                            End If
                        End Sub, cancellationToken)
    End Function

    Private Function SearchMACRange(macPrefix As String, cancellationToken As CancellationToken) As Task
        Return Task.Run(Sub()
                            Try
                                Dim process = New Process()
                                process.StartInfo.FileName = "arp"
                                process.StartInfo.Arguments = "-a"
                                process.StartInfo.UseShellExecute = False
                                process.StartInfo.RedirectStandardOutput = True
                                process.StartInfo.CreateNoWindow = True
                                process.Start()
                                Dim output = process.StandardOutput.ReadToEnd()
                                process.WaitForExit()

                                Dim lines = output.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                                For Each line In lines
                                    If cancellationToken.IsCancellationRequested Then Return

                                    If line.Contains(macPrefix) Then
                                        Dim parts = line.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                                        If parts.Length >= 2 Then
                                            Dim ip = parts(0)
                                            Dim mac = parts(1)
                                            If IsCamera(ip) Then
                                                Dim hostname = GetHostname(ip)
                                                Me.Invoke(Sub()
                                                              dgvResults.Rows.Add(hostname, ip, mac)
                                                          End Sub)
                                            End If
                                        End If
                                    End If
                                Next
                            Catch ex As Exception
                                ' Log error but continue with other searches
                            End Try
                        End Sub, cancellationToken)
    End Function

    Private Function IsCamera(ip As String) As Boolean
        Try
            ' First try to ping the device
            Using ping As New Ping()
                Dim reply = ping.Send(ip, 100)
                If reply.Status <> IPStatus.Success Then
                    Return False
                End If
            End Using

            ' Check common camera ports with shorter timeout
            Dim ports = {80, 554, 8000, 8080, 37777, 34567}
            For Each port In ports
                Using client As New TcpClient()
                    client.ConnectAsync(ip, port).Wait(100) ' 100ms timeout
                    If client.Connected Then
                        client.Close()
                        Return True
                    End If
                End Using
            Next

            ' Try to get device info using SNMP
            Try
                Dim process = New Process()
                process.StartInfo.FileName = "snmpwalk"
                process.StartInfo.Arguments = "-v2c -c public " & ip & " .1.3.6.1.2.1.1.1.0"
                process.StartInfo.UseShellExecute = False
                process.StartInfo.RedirectStandardOutput = True
                process.StartInfo.CreateNoWindow = True
                process.Start()
                Dim output = process.StandardOutput.ReadToEnd()
                process.WaitForExit()

                Dim outputLower = output.ToLower()
                If outputLower.Contains("camera") OrElse
                   outputLower.Contains("ipcam") OrElse
                   outputLower.Contains("dvr") OrElse
                   outputLower.Contains("nvr") Then
                    Return True
                End If
            Catch
                ' SNMP failed, continue with other checks
            End Try
        Catch
            ' Connection failed, continue checking other ports
        End Try
        Return False
    End Function

    Private Function GetMACAddress(ip As String) As String
        Try
            ' First try to ping the IP to update ARP table
            Try
                Using ping As New Ping()
                    ping.Send(ip, 100)
                End Using
            Catch
                ' Ping failed, continue anyway
            End Try

            ' Try to get MAC using ARP with specific IP
            Dim process = New Process()
            process.StartInfo.FileName = "arp"
            process.StartInfo.Arguments = "-a " & ip
            process.StartInfo.UseShellExecute = False
            process.StartInfo.RedirectStandardOutput = True
            process.StartInfo.CreateNoWindow = True
            process.Start()
            Dim output = process.StandardOutput.ReadToEnd()
            process.WaitForExit()

            ' Parse ARP output
            Dim lines = output.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
            For Each line In lines
                If line.Contains(ip) Then
                    ' Extract MAC address
                    Dim parts = line.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                    For Each part In parts
                        If part.Contains("-") OrElse part.Contains(":") Then
                            ' Format MAC address to use hyphens
                            Dim mac = part.Trim()
                            mac = mac.Replace(":", "-")
                            Return mac
                        End If
                    Next
                End If
            Next

            ' If ARP fails, try to get MAC using netsh
            Try
                process.StartInfo.FileName = "netsh"
                process.StartInfo.Arguments = "interface ipv4 show neighbors"
                process.Start()
                output = process.StandardOutput.ReadToEnd()
                process.WaitForExit()

                lines = output.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                For Each line In lines
                    If line.Contains(ip) Then
                        Dim parts = line.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                        For Each part In parts
                            If part.Contains("-") OrElse part.Contains(":") Then
                                Dim mac = part.Trim()
                                mac = mac.Replace(":", "-")
                                Return mac
                            End If
                        Next
                    End If
                Next
            Catch
                ' netsh failed, continue with other methods
            End Try

            ' Try to get MAC using getmac
            Try
                process.StartInfo.FileName = "getmac"
                process.StartInfo.Arguments = "/v /fo csv /nh"
                process.Start()
                output = process.StandardOutput.ReadToEnd()
                process.WaitForExit()

                lines = output.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                For Each line In lines
                    If line.Contains(ip) Then
                        Dim parts = line.Split(","c)
                        If parts.Length >= 2 Then
                            Dim mac = parts(0).Trim(""""c)
                            mac = mac.Replace(":", "-")
                            Return mac
                        End If
                    End If
                Next
            Catch
                ' getmac failed, continue with other methods
            End Try

            ' If all methods fail, try to get MAC from network interfaces
            Try
                Dim networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                For Each networkInterface In networkInterfaces
                    If networkInterface.OperationalStatus = OperationalStatus.Up Then
                        Dim properties = networkInterface.GetIPProperties()
                        For Each address In properties.UnicastAddresses
                            If address.Address.AddressFamily = AddressFamily.InterNetwork Then
                                Dim ipAddress = address.Address.ToString()
                                Dim subnetMask = address.IPv4Mask.ToString()

                                ' Check if the target IP is in the same subnet
                                If IsInSameSubnet(ip, ipAddress, subnetMask) Then
                                    ' Try to get MAC using netsh
                                    process.StartInfo.FileName = "netsh"
                                    process.StartInfo.Arguments = "interface ipv4 show neighbors"
                                    process.Start()
                                    output = process.StandardOutput.ReadToEnd()
                                    process.WaitForExit()

                                    lines = output.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                                    For Each line In lines
                                        If line.Contains(ip) Then
                                            Dim parts = line.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                                            For Each part In parts
                                                If part.Contains("-") OrElse part.Contains(":") Then
                                                    Dim mac = part.Trim()
                                                    mac = mac.Replace(":", "-")
                                                    Return mac
                                                End If
                                            Next
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                Next
            Catch
                ' Network interface method failed
            End Try

            ' If still no MAC, try to get it from the command line
            Try
                process.StartInfo.FileName = "cmd"
                process.StartInfo.Arguments = "/c arp -a | findstr " & ip
                process.Start()
                output = process.StandardOutput.ReadToEnd()
                process.WaitForExit()

                If Not String.IsNullOrEmpty(output) Then
                    Dim parts = output.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                    For Each part In parts
                        If part.Contains("-") OrElse part.Contains(":") Then
                            Dim mac = part.Trim()
                            mac = mac.Replace(":", "-")
                            Return mac
                        End If
                    Next
                End If
            Catch
                ' Command line method failed
            End Try

            ' If all methods fail, return Unknown
            Return "Unknown"
        Catch ex As Exception
            ' Log error but continue
        End Try
        Return "Unknown"
    End Function

    Private Function IsInSameSubnet(ip1 As String, ip2 As String, subnetMask As String) As Boolean
        Try
            Dim ip1Bytes = IPAddress.Parse(ip1).GetAddressBytes()
            Dim ip2Bytes = IPAddress.Parse(ip2).GetAddressBytes()
            Dim maskBytes = IPAddress.Parse(subnetMask).GetAddressBytes()

            For i As Integer = 0 To 3
                If (ip1Bytes(i) And maskBytes(i)) <> (ip2Bytes(i) And maskBytes(i)) Then
                    Return False
                End If
            Next

            Return True
        Catch
            Return False
        End Try
    End Function

    Private Function GetHostname(ip As String) As String
        Try
            ' First try to get hostname using DNS
            Try
                Dim hostEntry = Dns.GetHostEntry(ip)
                If hostEntry.HostName <> ip Then
                    Return hostEntry.HostName
                End If
            Catch
                ' DNS failed, continue with other methods
            End Try

            ' Try to get hostname using nbtstat
            Try
                Dim process = New Process()
                process.StartInfo.FileName = "nbtstat"
                process.StartInfo.Arguments = "-A " & ip
                process.StartInfo.UseShellExecute = False
                process.StartInfo.RedirectStandardOutput = True
                process.StartInfo.CreateNoWindow = True
                process.Start()
                Dim output = process.StandardOutput.ReadToEnd()
                process.WaitForExit()

                ' Parse nbtstat output to find hostname
                Dim lines = output.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                For Each line In lines
                    If line.Contains("<00>") AndAlso Not line.Contains("UNIQUE") Then
                        Dim parts = line.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                        If parts.Length >= 1 Then
                            Return parts(0).Trim()
                        End If
                    End If
                Next
            Catch
                ' nbtstat failed, continue with other methods
            End Try

            ' Try to get hostname using ping
            Try
                Dim process = New Process()
                process.StartInfo.FileName = "ping"
                process.StartInfo.Arguments = "-a " & ip & " -n 1"
                process.StartInfo.UseShellExecute = False
                process.StartInfo.RedirectStandardOutput = True
                process.StartInfo.CreateNoWindow = True
                process.Start()
                Dim output = process.StandardOutput.ReadToEnd()
                process.WaitForExit()

                ' Parse ping output to find hostname
                Dim lines = output.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                For Each line In lines
                    If line.Contains("Pinging ") AndAlso Not line.Contains("Pinging " & ip) Then
                        Dim startIndex = line.IndexOf("Pinging ") + 8
                        Dim endIndex = line.IndexOf(" [")
                        If endIndex > startIndex Then
                            Return line.Substring(startIndex, endIndex - startIndex)
                        End If
                    End If
                Next
            Catch
                ' ping failed, continue with other methods
            End Try

            ' If all methods fail, return a descriptive name based on IP
            Return "Camera-" & ip.Replace(".", "-")
        Catch ex As Exception
            ' Log error but continue
            Return "Camera-" & ip.Replace(".", "-")
        End Try
    End Function
End Class