Imports System.Data.SqlClient

Public Class MainITForm
    Private connectionString As String = "Server=180.1.1.6;Database=wmg-po;User Id=sa;Password=admin@WMG2024;"



    ' Assume this function is called during login to set the UserAccessLevel
    Private Sub SetUserAccessLevel(ByVal username As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT acc_level, form_name FROM dep_users WHERE username = @username"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@username", username)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            ' Assuming acc_level is the first column and form_name is the second
                            Dim accLevel As Integer = Convert.ToInt32(reader("acc_level"))
                            Dim allowedForms As String = reader("form_name").ToString()

                            ' Call a method to manage menu visibility based on allowed forms
                            ManageFormAccess(allowedForms)

                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error retrieving user access level: " & ex.Message)
        End Try
    End Sub
    Private Sub ManageFormAccess(ByVal allowedForms As String)
        ' Split the allowed forms into an array
        Dim formsArray As String() = allowedForms.Split(","c)

        ' Disable all menu items initially
        For Each item As ToolStripMenuItem In MenuStrip1.Items
            item.Enabled = False
        Next
        ' Enable the menu items based on the allowed forms
        For Each formName As String In formsArray
            Select Case formName.Trim().ToLower() ' Adjust for case sensitivity

                Case "contractform"
                    ToolStripMenuItem1.Enabled = True
                Case "salesprintpackingform"
                    ToolStripMenuItem1.Enabled = True
                Case "technicallibqconpoForm"
                    ToolStripMenuItem2.Enabled = True
                Case "addnewcodeform"
                    ToolStripMenuItem2.Enabled = True
                Case "techviewform"
                    ToolStripMenuItem2.Enabled = True
                Case "printworkorderform"
                    ToolStripMenuItem2.Enabled = True
                Case "AIAnalysisForm"
                    ToolStripMenuItem3.Enabled = True

                Case "uploadform"
                    ToolStripMenuItem3.Enabled = True


                Case "reporttechnicalform"
                    ToolStripMenuItem4.Enabled = True
                Case "reportform"
                    ToolStripMenuItem4.Enabled = True
                Case "reportstop"
                    ToolStripMenuItem4.Enabled = True
                Case "liveproductionform"
                    ToolStripMenuItem4.Enabled = True
                Case "searchrowfinishform"
                    ToolStripMenuItem4.Enabled = True
                Case "storefinishviewform"
                    ToolStripMenuItem4.Enabled = True
                Case "pickpackform"
                    ToolStripMenuItem4.Enabled = True
                Case "reportchangesform"
                    ToolStripMenuItem4.Enabled = True
                Case "reportsfinishinspectform"
                    ToolStripMenuItem4.Enabled = True
                Case "FabricInOutForm"
                    ToolStripMenuItem4.Enabled = True
                Case "finishdisbursmentviewform"
                    ToolStripMenuItem4.Enabled = True
                Case "qcrawtestreportform"
                    ToolStripMenuItem4.Enabled = True
                Case "reporttrackform"
                    ToolStripMenuItem4.Enabled = True
                Case "rawstorerawform"
                    ToolStripMenuItem4.Enabled = True
                Case "reportrowinspect2form"
                    ToolStripMenuItem4.Enabled = True
                Case "samplelotrawreport2form"
                    ToolStripMenuItem4.Enabled = True
                Case "reportlivebatchform"
                    ToolStripMenuItem4.Enabled = True
                Case "colorsreportform"
                    ToolStripMenuItem4.Enabled = True



                Case "storefinishform"
                    ToolStripMenuItem5.Enabled = True
                Case "finishDisbursementform"
                    ToolStripMenuItem5.Enabled = True
                Case "pickpackform"
                    ToolStripMenuItem5.Enabled = True
                Case "addhulkfinishForm"
                    ToolStripMenuItem5.Enabled = True
                Case "porowtoinspectionForm"
                    ToolStripMenuItem5.Enabled = True
                Case "storefinishreturn"
                    ToolStripMenuItem5.Enabled = True


                Case "UpdateLibraryCodeForm"
                    ToolStripMenuItem6.Enabled = True
                Case "updateqtyworderform"
                    ToolStripMenuItem6.Enabled = True
                Case "techdataform"
                    ToolStripMenuItem6.Enabled = True
                Case "planningform"
                    ToolStripMenuItem6.Enabled = True
                Case "trackingform"
                    ToolStripMenuItem6.Enabled = True
                Case "updatedeliverydateform"
                    ToolStripMenuItem6.Enabled = True
                Case "requestbatchrawfromstoreform"
                    ToolStripMenuItem6.Enabled = True


                Case "qcfinishrollform"
                    ToolStripMenuItem7.Enabled = True
                Case "meterrowinspectForm"
                    ToolStripMenuItem7.Enabled = True
                Case "mainqcrawtestform"
                    ToolStripMenuItem7.Enabled = True

                Case "devcodelibform"
                    ToolStripMenuItem8.Enabled = True


                Case "dentryform"
                    ToolStripMenuItem9.Enabled = True


                Case "mainfinishinspectform"
                    ToolStripMenuItem10.Enabled = True
                Case "reportfinishinspectform"
                    ToolStripMenuItem10.Enabled = True
                Case "finishinspectadddefectsform"
                    ToolStripMenuItem10.Enabled = True


                Case "mainrowinspectform"
                    ToolStripMenuItem12.Enabled = True
                Case "reportrowinspectform"
                    ToolStripMenuItem12.Enabled = True
                Case "rawinspectadddefectsform"
                    ToolStripMenuItem12.Enabled = True
                Case "mainrowsampleform"
                    ToolStripMenuItem12.Enabled = True
                Case "samplelotrawreportform"
                    ToolStripMenuItem12.Enabled = True
                Case "updatetonewbatchlotform"
                    ToolStripMenuItem12.Enabled = True


                Case "rawreturnform"
                    ToolStripMenuItem13.Enabled = True
                Case "rawdisbursementform"
                    ToolStripMenuItem13.Enabled = True
                Case "recievedrawform"
                    ToolStripMenuItem13.Enabled = True
                Case "rawstorereporform"
                    ToolStripMenuItem13.Enabled = True
                Case "mainrawstoreform"
                    ToolStripMenuItem13.Enabled = True
                Case "rawtoinspectform"
                    ToolStripMenuItem13.Enabled = True
                Case "printbarcoderawform"
                    ToolStripMenuItem13.Enabled = True
                Case "updaterollocationform"
                    ToolStripMenuItem13.Enabled = True
                Case "rawfrominspectreturnform"
                    ToolStripMenuItem13.Enabled = True



                Case "accaddclientform"
                    ToolStripMenuItem14.Enabled = True

                Case "powerpiform"
                    PowerPiToolStripMenuItem.Enabled = True
                Case "powerpihulkform"
                    PowerPiToolStripMenuItem.Enabled = True
                Case "testrepfollowproddataentryform"
                    PowerPiToolStripMenuItem.Enabled = True
                Case "networkcamerasearchform"
                    ToolStripMenuItem3.Enabled = True

                Case "salarynotificationform"
                    NetworkToolsToolStripMenuItemhr.Enabled = True

                Case "insertcolorsform"
                    ToolStripMenuItem15.Enabled = True



            End Select
        Next
    End Sub
    Private Sub MainITForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        ' Access the logged-in username from the global variable
        lblUsername.Text = "Logged in as: " & LoggedInUsername
        ' Set the user access level based on logged-in username
        SetUserAccessLevel(LoggedInUsername)
    End Sub
    ' Event handler for Sales menu item click
    Private Sub SalesDataToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SalesDataToolStripMenuItem.Click
        Dim contractform As New ContractForm
        contractform.Show()
    End Sub

    ' Event handler for add code menu item click
    Private Sub AddNewCodeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AddNewCodeToolStripMenuItem.Click
        Dim addnewcodeform As New addnewcodeform
        addnewcodeform.Show()
    End Sub

    ' Event handler for tech data menu item click
    Private Sub TechnicalDataToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TechnicalDataToolStripMenuItem.Click
        Dim technicallibqconpoForm As New technicallibqconpoForm
        technicallibqconpoForm.Show()
    End Sub

    Private Sub ReportTechnicalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ReportTechnicalToolStripMenuItem.Click
        Dim reporttechnicalform As New reporttechnicalform
        reporttechnicalform.Show()
    End Sub
    Private Sub UploadDataToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles UploadDataToolStripMenuItem.Click
        Dim uploadform As New UploadForm
        uploadform.Show()
    End Sub
    Private Sub ReportProductionToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ReportProductionToolStripMenuItem.Click
        Dim reportform As New reportform
        reportform.Show()
    End Sub
    Private Sub ReportStopToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ReportStopToolStripMenuItem.Click
        Dim reportstop As New reportstop
        reportstop.Show()
    End Sub
    Private Sub LiveProductionToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LiveProductionToolStripMenuItem.Click
        Dim liveproductionform As New LiveProductionForm
        liveproductionform.Show()
    End Sub
    Private Sub InspectionFabricToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim searchrowfinishform As New SearchRowFinishForm
        searchrowfinishform.Show()
    End Sub
    Private Sub UpdateCodeLibraryToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles UpdateCodeLibraryToolStripMenuItem.Click
        Dim UpdateLibraryCodeForm As New UpdateLibraryCodeForm
        UpdateLibraryCodeForm.Show()
    End Sub
    Private Sub ReceivingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReceivingToolStripMenuItem.Click
        Dim storefinishform As New storefinishform
        storefinishform.Show()
    End Sub
    Private Sub FinishReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FinishReportToolStripMenuItem.Click
        Dim storefinishviewform As New storefinishviewform
        storefinishviewform.Show()
    End Sub
    Private Sub PackingToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PackingToolStripMenuItem5.Click
        Dim pickpackform As New pickpackform
        pickpackform.Show()
    End Sub
    Private Sub requestPackingreqToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles requestPackingreqToolStripMenuItem.Click
        Dim salesprintpackingform As New salesprintpackingform
        salesprintpackingform.Show()
    End Sub
    Private Sub SampleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SampleToolStripMenuItem.Click
        Dim finishDisbursementform As New finishDisbursementtform
        finishDisbursementform.Show()
    End Sub
    Private Sub UpdateQTYToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateQTYToolStripMenuItem.Click
        Dim updateqtyworderform As New updateqtyworderform
        updateqtyworderform.Show()
    End Sub
    Private Sub ChangesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangesToolStripMenuItem.Click
        Dim reportchangesform As New reportchangesform
        reportchangesform.Show()
    End Sub
    Private Sub PackingfToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PackingfToolStripMenuItem.Click
        Dim packingviewform As New packingviewform
        packingviewform.Show()
    End Sub

    Private Sub SampleFinishToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SampleFinishToolStripMenuItem.Click
        Dim finishdisbursmentviewform As New finishdisbursmentviewform
        finishdisbursmentviewform.Show()
    End Sub
    Private Sub AddHulkToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddHulkToolStripMenuItem5.Click
        Dim addhulkfinishForm As New addhulkfinishForm
        addhulkfinishForm.Show()
    End Sub
    Private Sub CreateWorderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateWorderToolStripMenuItem.Click
        Dim techdataform As New techdataform
        techdataform.Show()
    End Sub
    Private Sub PrintWorderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintWorderToolStripMenuItem.Click
        Dim techviewform As New techviewform
        techviewform.Show()
    End Sub
    Private Sub RecordsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecordsToolStripMenuItem.Click
        Dim planningform As New planningForm
        planningform.Show()
    End Sub
    Private Sub FinishRollToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FinishRollToolStripMenuItem.Click
        Dim Qcfinishrollform As New Qcfinishrollform
        Qcfinishrollform.Show()
    End Sub
    Private Sub RowFinishToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RowFinishToolStripMenuItem.Click
        Dim FabricInOutForm As New FabricInOutForm
        FabricInOutForm.Show()
    End Sub
    Private Sub WorderWeightToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim porowtoinspectionForm As New porowtoinspectionForm
        porowtoinspectionForm.Show()
    End Sub
    Private Sub WorderMetersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WorderMetersToolStripMenuItem.Click
        Dim meterrowinspectForm As New meterrowinspectForm
        meterrowinspectForm.Show()
    End Sub
    Private Sub CreateLibraryCodeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateLibraryCodeToolStripMenuItem.Click
        Dim devcodelibform As New devcodelibform
        devcodelibform.Show()
    End Sub
    Private Sub FollowPoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FollowPoToolStripMenuItem.Click
        Dim dentryform As New dentryform
        dentryform.Show()
    End Sub
    Private Sub RiiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RiiToolStripMenuItem.Click
        Dim mainfinishinspectform As New mainfinishinspectform
        mainfinishinspectform.Show()
    End Sub
    Private Sub ReportFinishInspectionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportFinishInspectionToolStripMenuItem.Click
        Dim reportfinishinspectform As New reportfinishinspectform
        reportfinishinspectform.Show()
    End Sub

    Private Sub rowwToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles rowwToolStripMenuItem1.Click
        Dim mainrowinspectform As New mainrowinspectform
        mainrowinspectform.Show()
    End Sub
    Private Sub ReportrowInspectionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportrowInspectionToolStripMenuItem.Click
        Dim reportrowinspectform As New reportrowinspectform
        reportrowinspectform.Show()
    End Sub

    Private Sub reprtfinish2ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles reprtfinish2ToolStripMenuItem.Click
        Dim reportsfinishinspectform As New reportsfinishinspectform
        reportsfinishinspectform.Show()
    End Sub
    Private Sub createbatchToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles createbatchToolStripMenuItem.Click
        Dim createbatchform As New createbatchform
        createbatchform.Show()
    End Sub

    Private Sub recievedrawToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles recievedrawToolStripMenuItem.Click
        Dim recievedrawform As New recievedrawform
        recievedrawform.Show()
    End Sub

    Private Sub rawdistributeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles rawdistributeToolStripMenuItem.Click
        Dim rawdisbursementform As New rawdisbursementform
        rawdisbursementform.Show()
    End Sub

    Private Sub rawreturnToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles rawreturnToolStripMenuItem.Click
        Dim rawreturnform As New rawreturnform
        rawreturnform.Show()
    End Sub

    Private Sub rawreportsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles rawreportsToolStripMenuItem.Click
        Dim rawstorereporform As New rawstorereporform
        rawstorereporform.Show()
    End Sub
    Private Sub AddClientToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddClientToolStripMenuItem.Click
        Dim accaddclientform As New accaddclientform
        accaddclientform.Show()
    End Sub
    Private Sub AddDefectsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddDefectsToolStripMenuItem.Click
        Dim finishinspectadddefectsform As New finishinspectadddefectsform
        finishinspectadddefectsform.Show()
    End Sub

    Private Sub AddDefectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddDefectToolStripMenuItem.Click
        Dim rawinspectadddefectsform As New rawinspectadddefectsform
        rawinspectadddefectsform.Show()
    End Sub

    Private Sub inspectrowsampToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles inspectrowsampToolStripMenuItem.Click
        Dim mainrowsampleform As New mainrowsampleform
        mainrowsampleform.Show()
    End Sub

    Private Sub Reportsample10ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Reportsample10ToolStripMenuItem.Click
        Dim samplelotrawreportform As New samplelotrawreportform
        samplelotrawreportform.Show()
    End Sub
    Private Sub QctestrawToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QctestrawToolStripMenuItem.Click
        Dim mainqcrawtestform As New mainqcrawtestform
        mainqcrawtestform.Show()
    End Sub
    Private Sub UpdateRawToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateRawToolStripMenuItem.Click
        Dim updatetonewbatchlotform As New updatetonewbatchlotform
        updatetonewbatchlotform.Show()
    End Sub
    Private Sub FinishReturnToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FinishReturnToolStripMenuItem.Click
        Dim storefinishreturn As New storefinishreturn
        storefinishreturn.Show()
    End Sub
    Private Sub QclabToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles QclabToolStripMenuItem1.Click
        Dim qcrawtestreportform As New qcrawtestreportform
        qcrawtestreportform.Show()
    End Sub
    Private Sub RunningStatusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunningStatusToolStripMenuItem.Click
        Dim trackingform As New trackingform
        trackingform.Show()
    End Sub
    Private Sub ReportstatusprodToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportstatusprodToolStripMenuItem.Click
        Dim reporttrackform As New reporttrackform
        reporttrackform.Show()
    End Sub
    Private Sub UpdateDeliverydateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateDeliverydateToolStripMenuItem.Click
        Dim updatedeliverydateform As New UpdateDeliveryDateForm
        updatedeliverydateform.Show()
    End Sub
    Private Sub RequestrawToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RequestrawToolStripMenuItem.Click
        Dim requestbatchrawfromstoreform As New requestbatchrawfromstoreform
        requestbatchrawfromstoreform.Show()
    End Sub
    Private Sub RawstorerepToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RawstorerepToolStripMenuItem.Click
        Dim rawstorerawform As New rawstorerawform
        rawstorerawform.Show()
    End Sub
    Private Sub PrintPoReadyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintPoReadyToolStripMenuItem.Click
        Dim printworkorderform As New PrintWorkOrderForm
        printworkorderform.Show()
    End Sub
    Private Sub RawreportinsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RawreportinsToolStripMenuItem.Click
        Dim reportrowinspect2form As New reportrowinspect2form
        reportrowinspect2form.Show()
    End Sub
    Private Sub RawsampinspToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RawsampinspToolStripMenuItem.Click
        Dim samplelotrawreport2form As New samplelotrawreport2form
        samplelotrawreport2form.Show()
    End Sub
    Private Sub BatchstatusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BatchstatusToolStripMenuItem.Click
        Dim reportlivebatchform As New reportlivebatchform
        reportlivebatchform.Show()
    End Sub
    Private Sub PiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PiToolStripMenuItem.Click
        Dim powerpiform As New powerpiform
        powerpiform.Show()
    End Sub
    Private Sub HulkrawfinishpoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HulkrawfinishpoToolStripMenuItem.Click
        Dim powerpihulkform As New powerpihulkform
        powerpihulkform.Show()
    End Sub
    Private Sub ProditdataentryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProditdataentryToolStripMenuItem.Click
        Dim testrepfollowproddataentryform As New testrepfollowproddataentryform
        testrepfollowproddataentryform.Show()
    End Sub
    Private Sub WordercardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WordercardToolStripMenuItem.Click
        Dim printworkorderform As New PrintWorkOrderForm
        printworkorderform.Show()
    End Sub
    Private Sub ContractlotToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ContractlotToolStripMenuItem.Click
        Dim contractlotform As New ContractLotForm()
        contractlotform.Show()
    End Sub

    Private Sub AIAnalysisToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles AIAnalysisToolStripMenuItem.Click
        Dim aiForm As New AIAnalysisForm()
        aiForm.Show()
    End Sub
    Private Sub MainrawstoreToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MainrawstoreToolStripMenuItem.Click
        Dim mainrawstoreform As New mainrawstoreform
        mainrawstoreform.Show()
    End Sub
    Private Sub CameraToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CameraToolStripMenuItem.Click
        Dim networkCameraSearchForm As New NetworkCameraSearchForm
        networkCameraSearchForm.Show()
    End Sub

    Private Sub SendSalaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SendSalaryToolStripMenuItem.Click
        Dim salarynotificationform As New salarynotificationform
        salarynotificationform.Show()
    End Sub

    Private Sub RawstorenewtoinspectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RawstorenewtoinspectToolStripMenuItem.Click
        Dim rawtoinspectform As New rawtoinspectform
        rawtoinspectform.Show()
    End Sub

    Private Sub PrintrawbarcodeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintrawbarcodeToolStripMenuItem.Click
        Dim printbarcoderawform As New printbarcoderawform
        printbarcoderawform.Show()
    End Sub

    Private Sub ChangerawlocationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangerawlocationToolStripMenuItem.Click
        Dim updaterollocationform As New updaterollocationform
        updaterollocationform.Show()
    End Sub

    Private Sub RawfrominspectreturnToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RawfrominspectreturnToolStripMenuItem.Click
        Dim rawfrominspectreturnform As New rawfrominspectreturnform
        rawfrominspectreturnform.Show()
    End Sub
    Private Sub btnLogout_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnlogout.Click
        Me.Hide() ' Optionally hide the form instead of closing
        Dim login As New LoginForm ' Assuming your login form is named LoginForm
        login.Show()
    End Sub

    Private Sub InsertDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InsertDataToolStripMenuItem.Click
        Dim frmMachineidletimes As New frmMachineIdleTimes
        frmMachineidletimes.Show()
    End Sub

    Private Sub InsertColorsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InsertColorsToolStripMenuItem.Click
        Dim insertcolorsform As New insertcolorsform
        insertcolorsform.Show()
    End Sub

    Private Sub ColorsreportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ColorsreportToolStripMenuItem.Click
        Dim colorsreportform As New colorsreportform
        colorsreportform.Show()
    End Sub

    Private Sub DeletefinishrollsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeletefinishrollsToolStripMenuItem.Click
        Dim moveinspectiondataform As New moveinspectiondataform
        moveinspectiondataform.Show()
    End Sub

    Private Sub InsertcustprobToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InsertcustprobToolStripMenuItem.Click
        Dim CustomerProblemsForm As New CustomerProblemsForm
        CustomerProblemsForm.Show()
    End Sub

    Private Sub QcreplyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QcreplyToolStripMenuItem.Click
        Dim qcreplyform As New QCReplyForm
        qcreplyform.Show()
    End Sub

    Private Sub CustomersproreportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CustomersproreportToolStripMenuItem.Click
        Dim CustomerProblemsReportForm As New CustomerProblemsReportForm
        CustomerProblemsReportForm.Show()
    End Sub

    Private Sub ToolStripMenuItemstochstorefinish_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemstochstorefinish.Click
        Dim stockfinishform As New stockfinishform
        stockfinishform.Show()
    End Sub
End Class

