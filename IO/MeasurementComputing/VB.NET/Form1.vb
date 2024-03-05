'*******************************************************************************
' Copyright (C) 2010 Cognex Corporation
'
' Subject to Cognex Corporations terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
'*******************************************************************************/
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.
'
' This application requires a Measurement Computing board to be installed.
'
' The intention of this sample program is to demonstrate how to program
' both input and output lines of an Measurement Computing board (MCB)
' using both script and a standalone application.
'
' This application loads QuickBuildMCB_Script.vpp, which contains 
' a single job called MCB_RaiseOutputs_OnAcqComplete_JobScript. Port CL 
' is programmed to be used as an output port. The main purpose of the script 
' is to show how to program output lines of the MCB.
'
' This application also configures port A as an input port and waits for a signal on line 0.
' When the line 0 is triggered by the external device, the GUI will display the message
' 'Input signal received count' on the output textbox.
' 
' You can also directly load QuickBuildMCB_Script.vpp from the QuickBuild 
' application and run it. The Online button on the QuickBuild toolbar works 
' the same way as the Online button in this sample program.

Option Explicit On
Imports System.IO
Imports Cognex.VisionPro
Imports Cognex.VisionPro.QuickBuild
Imports Cognex.VisionPro.QuickBuild.IO

Public Class Form1
  
  Dim _manager As CogJobManager
  Dim _mcb As CogIOMCB
  Dim offLine As String = "Offline"
  Dim onLine As String = "Online"
  Dim run As String = "Run"
  Dim stopmsg As String = "Stop"

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.

    btnOnline.Text = onLine
    btnOnline.Enabled = LoadQBApplication()
    btnRun.Enabled = btnOnline.Enabled
    Text1.Select(0, 0) ' unhighlight the description text
  End Sub

#Region "Initialization"
' This routine loads QuickBuildMCB_Script.vpp shipped with VisionPro
' This file must reside in samples\Programming\IO\MeasurementComputing
Private Function LoadQBApplication() As Boolean
  Dim expectedPath As String = "MeasurementComputing"
  Dim sampleName As String = "QuickBuildMCB_Script.vpp"
  Dim path As String = Environment.GetEnvironmentVariable("VPRO_ROOT")

    If path Is Nothing Then
      textBox1.Text = "Missing " & expectedPath & _
          " directory. Fix the problem and try it again."
      Return False
    End If

  ' Make sure QuickBuildMCB_Script.vpp can be found.
    path = path & "\\samples\\Programming\\IO\\MeasurementComputing\\" & sampleName
    If Not File.Exists(path) Then
      textBox1.Text = "Unable to locate " & expectedPath & _
          ". Fix the problem and try it again."
      Return False
    End If

  Try
    ' Let's load the sample program.
    _manager = CType(CogSerializer.LoadObjectFromFile(path), CogJobManager)
      If _manager Is Nothing Then
        textBox1.Text = path & " is corrupted. Unable to continue."
        Return False
      End If

    If (GetMCB()) Then
      ' Wait for an input signal.
        InitializeInput()

      ' Let's monitor the job manager stop event.
        ' We need to do this so that we can change the button state.
        AddHandler _manager.Stopped, AddressOf Manager_Stopped
      Else
        textBox1.Text = _
        "The system must have a Measurement Computing board to run this application."
        btnOnline.Enabled = False
        Return False
    End If
  Catch ex As Exception
      textBox1.Text = "Unexpected error: " & ex.Message
      Return False
    End Try
    LoadQBApplication = True
  End Function

  Private Function GetMCB() As Boolean
    ' CogJobManager always creates an instance of MCB found in the system.
    ' Hence, we must use the one that is created by CogJobManager.
    ' This program finds the first MCB found by CogJobManager and uses it.
    For Each device As CogIODiscreteDevice In _manager.IODeviceList.DiscreteDevices
      ' Make sure the device type if CogIOMCB
      If TypeOf device Is CogIOMCB Then

        _mcb = CType(device, CogIOMCB)
        Exit For
      End If
    Next

    If _mcb Is Nothing Then
      ' The system does not have an MCB board.
      Return False
    End If

    ' If you have more than one MCB board, you can use
    ' DeviceIndex to identify a specific board.
    textBox1.Text = String.Format("Found {0} with an index {1}", _
                                  _mcb.DeviceName, _mcb.DeviceIndex)
    GetMCB = True
  End Function

#End Region

#Region "Input Signal Handler"
  '
  ' A bank of the first eight lines of a Measurement Computing "USB-1024LS" or "PCI-DIO24/S"
  ' board is configured as input lines; however, it only uses the first line in this sample.
  ' The sample runs When it receives an input signal  
  ' It overrides the Initialize, PostAcquisitionRefInfo to acheive this.
  '
  Dim _inputLine As Integer = 0      ' Wait for an input signal from line 0

  'Perform any initialization required by your script here.
  Private Sub InitializeInput()
    ' Use CogIOMCBPortNumberConstant to define a port direction.
    ' Set the the port direction to handle input
    _mcb.SetPortDirection(CogIOMCBPortNumberConstants.PortA, _
      CogIOMCBPortDirectionConstants.Input)
    _mcb.SetLineEnabled(_inputLine, True)

    ' Assign a delegate that will handle an input signal
    _mcb.InputDelegate.Item(_inputLine) = AddressOf InputSignalReceived

    ' Let's wait for the input signal
  End Sub

  Dim _signalReceived As Integer = 0
  Private Sub InputSignalReceived(ByVal asserted As Boolean)
    Try
      If textBox1.InvokeRequired Then
        'It's not safe to update the form from the input signal thread, so we use Invoke to
        'get onto the GUI thread.
        Dim eventArgs() As Object = {asserted}
        textBox1.Invoke(New CogIODiscreteDevice.InputDelegateFn(AddressOf InputSignalReceived), eventArgs)
      Else
        'Increment our count and update the text box
        _signalReceived += 1
        textBox1.Text = _
          String.Format("Input signal received count = {0}", _signalReceived)

        ' Do something else...
        '
      End If
    Catch
      'Do not rethrow the exception. VisionPro does not expect exceptions to occur
      'in event handlers
    End Try
  End Sub

#End Region

#Region "Manager Stopped Event"
  ' We get this event when CogJobManger stops
  Private Sub Manager_Stopped(ByVal sender As Object, ByVal e As CogJobManagerActionEventArgs)
    If (btnRun.InvokeRequired) Then
      'It's not safe to update the form from the job manager stopped thread, so we use Invoke to
      'get onto the GUI thread.
      Dim eventArgs() As Object = {sender, e}
      btnRun.Invoke(New CogJobManager.CogJobManagerStoppedEventHandler(AddressOf Manager_Stopped), eventArgs)
      Return
    End If
    ' Show "Run" on the button
    btnRun.Text = run

    ' Allow the user to change the IOEnable state.
    btnOnline.Enabled = True

    ' You might want to consider going Offline; otherwise,
    ' the program will respond to the incoming input signal.
  End Sub
#End Region

#Region "Form Closing"
  Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    If Not _manager Is Nothing Then
      If Not _mcb Is Nothing Then
        _mcb.MonitorInputs = False
        _mcb.InputDelegate.Item(_inputLine) = Nothing
      End If

      ' Must shutdown the job manager before existing
      _manager.Shutdown()
      _manager = Nothing
    End If
  End Sub
#End Region

#Region "Button Handlers"
  ' Do not change the IOEnable state when CogJobManager is running.
  ' This routine performs the same task as pressing the Online button
  ' from QuickBuild.
  Private Sub btnOnline_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOnline.Click
    If btnOnline.Text = onLine Then
      ' Let's go online which enables both input and output lines
      btnOnline.Text = offLine
      If Not _manager.IOEnable Then
        _manager.IOEnable = True
      End If
    Else
      btnOnline.Text = onLine
      ' Display both input and output lines.
      _manager.IOEnable = False
    End If
  End Sub

  Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
    If btnRun.Text = run Then
      ' Let's run the QuickBuild application.
      btnRun.Text = stopmsg
      _manager.Run()

      ' disable the other button to avoid any contention.
      btnOnline.Enabled = False
    Else
      ' Let's stop the job manager
      _manager.Stop()

      ' Now let's wait for the stopped event
      ' See Manager_Stopped().
    End If
  End Sub

#End Region

End Class

