' ***************************************************************************
' Copyright (C) 2007 Cognex Corporation
'
' Subject to Cognex Corporation's terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely
' in conjunction with a Cognex Machine Vision System.  Furthermore you
' acknowledge and agree that Cognex has no warranty, obligations or liability
' for your use of the Software.
' ***************************************************************************
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This application makes use of the .NET method "Invoke" to move data between 
' the Job Thread (worker thread) and the GUI thread.  Invoke() is a .NET 
' construct that allows thread safe access to objects on the Forms main GUI 
' thread (i.e. controls) from other threads.  It is forbidden to access GUI 
' objects directly from a thread that is not the Forms main GUI thread.  
'
' Invoke() is an important technique to understand when working with the 
' CogJobManager because under the hood the CogJobManager uses many worker 
' threads to acquire images and run the jobs.  This means that often when the
' CogJobManager fires an event the listening event handler function will be
' called on the worker thread from which the event was fired or raised.  For 
' instance, the UserResultAvailable event will not call its handler function on
' the on the main GUI thread.  A popular thing to do is to display user results 
' to the GUI.  This obviously requires accessing controls (GUI thread objects),
' therefore this is an example of a situation that requires Invoke().
'
' Using the Invoke mechanism has three main parts.
'
'    1.  The Delegate - The delegate is a .NET name for a function pointer.  We
'        need to declare a delegate and then instantiate an instance of this 
'        delegate.  The delegate is then passed by the Invoke() method to tell 
'	     Invoke() which method to call on the GUI thread.
'
'    2.  The InvokeRequired() call - This call simply returns true if the code
'        that is currently executing is not on the main GUI thread.  If 
'	     InvokeRequired is true, the current code is not running on the GUI 
'	     thread, so we must call Invoke to access GUI objects.
'
'    3.  The Invoke() call - This call will block the current thread and wait
'        for the GUI thread to execute the method whose delegate we have passed
'        to the Invoke() call.
'
'Below is an example of how we use Invoke in this program, notice that we use
'InvokeRequired to see if we are on the correct thread, if not we create a 
'delegate to the same function and Invoke it on the GUI thread, when
'myJobManager_UserResultAvailable then executes on the GUI thread
'InvokeRequired() will is false and we continue on to update the GUI elements.
'
'       ' Define delegate whose signature matches CJM events.
'       Delegate Sub myJobManagerDelegate(ByVal sender As Object, _
'         ByVal e As CogJobManagerActionEventArgs)
'
'       Private Sub myJobManager_UserResultAvailable(ByVal sender As Object, _
'         ByVal e As CogJobManagerActionEventArgs)
'           If InvokeRequired Then
'
'               ' Create a pointer to this function
'               Dim myDel As New myJobManagerDelegate( _
'                 AddressOf myJobManager_UserResultAvailable)
'        
'               ' Call this same function on the correct thread
'               Dim eventArgs() As Object = {sender, e}
'               Invoke(myDel, eventArgs)
'
'               Return
'           End If
'
'           ' CODE THAT UPDATES THE GUI GOES HERE
'
'       End Sub
'
'  Note:  This sample is the same application that is generated as part of the
'         VisionPro training course; more insight into its parts can be gained 
'	      from reviewing the course material for the Application Development
'         section of this course.  If you do not have access to this material
'         please contact Cognex Technical Support (508)650-3100 for more
'         information.

Imports Cognex.VisionPro
Imports Cognex.VisionPro.QuickBuild
Imports Cognex.VisionPro.ToolGroup
Imports Cognex.VisionPro.PMAlign



Public Class Form1

#Region "Variable Declarations"

    Private myJobManager As CogJobManager
    Private myJob As CogJob
    Private myIndependentJob As CogJobIndependent

    ' Delegate whose signature matches CJM events.
    Delegate Sub myJobManagerDelegate(ByVal sender As Object, _
                       ByVal e As CogJobManagerActionEventArgs)

#End Region

#Region "Code the Executes when the Form first loads"

    ''' <summary>
    ''' This function is responsible for the initial 
    ''' setup of the app.  It loads and prepares
    ''' the saved QuickBuild app into a CogJobManager object,
    ''' attaches event handlers to to interesting CogJobManager
    ''' events, and sets up the CogDisplayStatusBar to reflect the
    ''' status of the CogDisplay we are using.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form1_Load(ByVal sender As System.Object, _
      ByVal e As System.EventArgs) Handles MyBase.Load

        ' Depersist the CogJobManager saved via QuickBuild
        myJobManager = CType(CogSerializer.LoadObjectFromFile( _
          Environment.GetEnvironmentVariable("VPRO_ROOT") + _
          "\\Samples\\Programming\\QuickBuild\\advancedAppOne.vpp"), CogJobManager)

        ' Initialize Variables
        myJob = myJobManager.Job(0)
        myIndependentJob = myJob.OwnedIndependent

        ' Flush queues
        myJobManager.UserQueueFlush()
        myJobManager.FailureQueueFlush()
        myJob.ImageQueueFlush()
        myIndependentJob.RealTimeQueueFlush()

        ' Register handler for Stopped event
        AddHandler myJobManager.Stopped, _
          AddressOf myJobManager_Stopped

        ' Register handler for UserResultAvailable event
        AddHandler myJobManager.UserResultAvailable, _
             AddressOf myJobManager_UserResultAvailable

        ' Connect the status bar
        CogDisplayStatusBar1.Display = CogRecordDisplay1

    End Sub

#End Region

#Region "Logic to Handle Button clicks"

    ''' <summary>
    ''' This function handles the click event for the RunOnce button
    ''' notice that it disables some other functionality that prevents
    ''' the user from trying to do other things while the job is 
    ''' allready running.  The functionality is re-enabled when the 
    ''' CogJobManager raises or fires its "stopped" event.    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RunOnceButton_Click(ByVal sender As System.Object, _
      ByVal e As System.EventArgs) Handles RunOnceButton.Click

        Try
            RunOnceButton.Enabled = False
            RunContCheckBox.Enabled = False ' Disable if running
            ShowTrainCheckBox.Enabled = False

            myJobManager.Run()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' This method handles a RunContiuus click similarly to the 
    ''' RunOnce handler.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RunContCheckBox_CheckedChanged(ByVal sender As System.Object, _
      ByVal e As System.EventArgs) Handles RunContCheckBox.CheckedChanged

        If (RunContCheckBox.Checked) Then
            Try
                RunOnceButton.Enabled = False
                ShowTrainCheckBox.Enabled = False
                myJobManager.RunContinuous()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Else
            Try
                RunContCheckBox.Enabled = False
                ShowTrainCheckBox.Enabled = True
                myJobManager.Stop()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If

    End Sub

    ''' <summary>
    ''' This handles a click to the Show Train Image button.
    ''' It allows the user to view train image record the pattern was 
    ''' trained off of and also enable the Retrain button. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ShowTrainCheckBox_CheckedChanged(ByVal sender As System.Object, _
      ByVal e As System.EventArgs) Handles ShowTrainCheckBox.CheckedChanged
        If (ShowTrainCheckBox.Checked) Then
            RunOnceButton.Enabled = False
            RunContCheckBox.Enabled = False
            RetrainButton.Enabled = True

            Dim myTG As CogToolGroup = myJob.VisionTool
            Dim myPMTool As CogPMAlignTool = myTG.Tools("CogPMAlignTool1")
            Dim tmpRecord As Cognex.VisionPro.ICogRecord
            tmpRecord = myPMTool.CreateCurrentRecord()
            tmpRecord = tmpRecord.SubRecords.Item("TrainImage")
            CogRecordDisplay1.Record = tmpRecord
            CogRecordDisplay1.Fit(True)
            RunStatusTextBox.Text = ""

        Else
            RunOnceButton.Enabled = True
            RunContCheckBox.Enabled = True
            RetrainButton.Enabled = False
            CogRecordDisplay1.Record = Nothing
        End If

    End Sub

    ''' <summary>
    ''' Handles a click to the Retrain Button, when clicked the PMAlignTool
    ''' will retrain its pattern.  The new trained pattern will reflect any
    ''' changes made to the train image record in the CogRecordDisplay.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RetrainButton_Click(ByVal sender As System.Object, _
      ByVal e As System.EventArgs) Handles RetrainButton.Click

        Dim myTG As CogToolGroup = myJob.VisionTool
        Dim myPMTool As CogPMAlignTool = myTG.Tools("CogPMAlignTool1")
        myPMTool.Pattern.Train()

    End Sub

#End Region

#Region "CogJobManager event handlers"

    ''' <summary>
    ''' This function handles the stopped event from the CogJobManager
    ''' When the Job is stopped it re-enables the Run Buttons. Note that
    ''' this function uses Invoke() as described in the biginning of this document. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub myJobManager_Stopped(ByVal sender As Object, _
      ByVal e As CogJobManagerActionEventArgs)

        If InvokeRequired Then

            ' Create a pointer to this function
            Dim myDel As New myJobManagerDelegate( _
                                AddressOf myJobManager_Stopped)

            ' Call this same function on the correct thread
            Dim eventArgs() As Object = {sender, e}
            Invoke(myDel, eventArgs)

            Return
        End If

        RunOnceButton.Enabled = True
        RunContCheckBox.Enabled = True  ' Enable when stopped

    End Sub


    ''' <summary>
    ''' This function demonstrates how to use the UserResultAvailable
    ''' event of the CogJobManager to retireve information from the 
    ''' running CogJobs.  Any object contained within a CogJob
    ''' can be added to the User Results (a.k.a. Posted Items) and accessed
    ''' in this way.
    ''' 
    ''' In this example we retireve some info about the Job's run status as
    ''' well as the Record that we want to display.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Note that this app updates the GUI every time a new result is
    ''' available.  Processor time is used every time the GUI is updated.
    ''' For applications that require very high throughput it might not
    ''' make sense to update the GUI for each run of the job as it will not
    ''' be noticeable to a user, and can slow down your overall throughput.
    ''' 
    ''' For high-throughput applications it will often make sense to 
    ''' consider options like only updating the GUI for every other result
    ''' for example.</remarks>
    Private Sub myJobManager_UserResultAvailable( _
                   ByVal sender As Object, _
                   ByVal e As CogJobManagerActionEventArgs)
        If InvokeRequired Then
            ' Create a pointer to this function
            Dim myDel As New myJobManagerDelegate( _
                       AddressOf myJobManager_UserResultAvailable)
            ' Call this same function on the correct thread
            Dim eventArgs() As Object = {sender, e}
            Invoke(myDel, eventArgs)
            Return
        End If

        Dim topRecord As Cognex.VisionPro.ICogRecord = _
                                      myJobManager.UserResult
        RunStatusTextBox.Text = _
           topRecord.SubRecords("UserResultTag").Content & ": " _
         & topRecord.SubRecords("JobName").Content & " --> " _
         & topRecord.SubRecords("RunStatus").Content.ToString


        Dim tmpRecord As Cognex.VisionPro.ICogRecord
        ' Assume the required record is present and get it.
        tmpRecord = topRecord.SubRecords("ShowLastRunRecordForUserQueue")
        tmpRecord = tmpRecord.SubRecords("LastRun")
        tmpRecord = tmpRecord.SubRecords("CogFixtureTool1.OutputImage")
        CogRecordDisplay1.Record = tmpRecord
        CogRecordDisplay1.Fit(True)

    End Sub

#End Region

#Region "Logic to gracefully close the app"

    ''' <summary>
    ''' This function is called as the application is closing.  It 
    ''' contains some important details about properly cleaning up
    ''' an applicaiton that utilizes the CogJobManager.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form1_FormClosing(ByVal sender As Object, _
      ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        ' UNregister handler for Stopped event
        RemoveHandler myJobManager.Stopped, _
          AddressOf myJobManager_Stopped

        ' UNregister handler for UserResultAvailable event
        RemoveHandler myJobManager.UserResultAvailable, _
                AddressOf myJobManager_UserResultAvailable

        ' This line prevents a deadlock that can occur
        ' if a UserResult becomes available after
        ' Form1_FormClosing() is called but before we unregister
        ' the UserResultAvailable event.
        Application.DoEvents()

        ' Be sure to shutdown the CogJobManager!!
        myJobManager.Shutdown()

        ' Explictly Dispose of all VisionPro controls
        CogDisplayStatusBar1.Dispose()
        CogRecordDisplay1.Dispose()

    End Sub

#End Region

End Class
