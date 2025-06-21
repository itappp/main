Imports System.Data
Imports System.Data.SqlClient

Public Class LiveProductionForm
    Private refreshTimer As New System.Windows.Forms.Timer()


    Public Sub New()
        InitializeComponent()

        ' Set up the timer to refresh live data every 5 seconds
        refreshTimer.Interval = 5000
        AddHandler refreshTimer.Tick, AddressOf RefreshLiveData
        refreshTimer.Start()

        ' Initialize FlowLayoutPanel if not added through designer
        flowLayoutPanel1 = New FlowLayoutPanel()
        flowLayoutPanel1.Dock = DockStyle.Fill
        Controls.Add(flowLayoutPanel1)

        ' Load initial data
        LoadLiveProductionData()
    End Sub

    ' Method to load live production data
    Private Sub LoadLiveProductionData()
        Dim connectionString As String = "Server=localhost;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"
        Dim query As String = "SELECT machine.name_arab, WorderID, name_ar, starttime, Status FROM LiveProduction left join machine on LiveProduction.MachineID = machine.id left join machine_steps on LiveProduction.processID = machine_steps.id"

        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        flowLayoutPanel1.Controls.Clear()

                        While reader.Read()
                            Dim machineID As String = reader("name_arab")
                            Dim processID As String = reader("name_ar").ToString()
                            Dim starttime As String = reader("starttime").ToString()
                            Dim worderID As String = reader("WorderID").ToString()
                            Dim status As String = reader("Status").ToString()

                            ' Create a panel for each machine
                            Dim machinePanel As New Panel With {
                                .Size = New Size(280, 130),
                                .BackColor = GetStatusColor(status)
                            }

                            ' Add machine name (centered at the top)
                            Dim lblMachine As New Label With {
                                .Text = machineID,
                                .AutoSize = True,
                                .TextAlign = ContentAlignment.MiddleCenter,
                                .Font = New Font("Calibri", 20, FontStyle.Bold),
                                .Dock = DockStyle.Top} ' Ensures it's placed at the top of the panel


                            ' Add WorderID (left-aligned below machineID)
                            Dim lblWorder As New Label With {
                                .Text = "Worder: " & worderID,
                                .AutoSize = True,
                                .TextAlign = ContentAlignment.TopRight,
                                .Font = New Font("Calibri", 15, FontStyle.Bold)
                            }
                            ' Add processID (left-aligned below worderID)
                            Dim lblprocessid As New Label With {
                                .Text = "Process: " & processID,
                                .AutoSize = True,
                                .TextAlign = ContentAlignment.TopRight,
                                .Font = New Font("Calibri", 15, FontStyle.Bold)
                            }
                            ' Add starttime (left-aligned below processID)
                            Dim lblstarttime As New Label With {
                                .Text = " Start: " & starttime,
                                .AutoSize = True,
                                .TextAlign = ContentAlignment.TopRight,
                                .Font = New Font("Calibri", 15, FontStyle.Bold)
                            }

                            ' Use a TableLayoutPanel for better control of layout
                            Dim layoutPanel As New TableLayoutPanel With {
                                .ColumnCount = 1,
                                .RowCount = 4,
                                .Dock = DockStyle.Fill
                            }

                            ' Optional: Set margin/padding for better spacing
                            lblWorder.Padding = New Padding(10, 0, 0, 0) ' Adds left padding for better alignment

                            ' Add the labels to the layout panel (lblMachine on top, lblWorder below)
                            layoutPanel.Controls.Add(lblMachine, 0, 0) ' Row 0 for machine
                            layoutPanel.Controls.Add(lblWorder, 0, 1) ' Row 1 for worder
                            layoutPanel.Controls.Add(lblprocessid, 0, 2)
                            layoutPanel.Controls.Add(lblstarttime, 0, 3)
                            ' Add the layout panel to the machine panel
                            machinePanel.Controls.Add(layoutPanel)

                            ' Add the panel to the flow layout
                            FlowLayoutPanel1.Controls.Add(machinePanel)


                        End While
                    End Using
                End Using
            Catch ex As SqlException
                MessageBox.Show("Error loading live data: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub
    ' Function to determine the color based on the status
    Private Function GetStatusColor(ByVal status As String) As Color
        Select Case status
            Case "Running"
                Return Color.Green
            Case "Pausing"
                Return Color.Yellow
            Case "Stopped"
                Return Color.Red
            Case Else
                Return Color.Gray ' Default color for unknown status
        End Select
    End Function

    ' Method to refresh live production data every 5 seconds
    Private Sub RefreshLiveData(ByVal sender As Object, ByVal e As EventArgs)
        LoadLiveProductionData()
    End Sub

    Private Sub LiveProductionForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
