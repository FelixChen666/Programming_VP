' Copyright (C) 2004-2010 Cognex Corporation
'
' Subject to Cognex Corporation's terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
'*******************************************************************************'
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This sample code was written explicitly for the 8504, but it also works with the 8514 and 8514e as well.  In particular this sample
' code demostates the use of the External I/O Module which can supports 16
' I/O lines, configurated as 8 inputs and 8 outputs.  However this sample is using
' 8 outputs and 4 inputs.
'
' Note: Even though there are 4 Bi-Directional lines they
'       have to be configured as 4 outputs because of the
'       I/O Box.
'
' This sample code uses the 800-5818-1 Terminal Block connected
' to a External I/O Board using a  300-0390 cable.  The External
' I/O Board is then connected to the 8504. For more
' information about PIO for the 8504 refer to VisionPro
'
' This sample code demonstrates the different features of the output and input I/O
' lines.  For the output lines in this sample code will show how to toggle an output
' using a timer, turn an output on and off, pulse an output and turn on and off
' a group of outputs lines.  For the inputs lines this sample code shows how to create
' event handlers.
'
' Intialization(InitializeAcquisition):
'   Step 1) Get a frame grabber
'   Step 2) Create the output lines and set their mode
'   Step 3) Create the input lines and set their mode, create input event handlers
'   Step 4) Setup timer
'
'   Timer Output Line 0
'     Start the timer.  In the timer event handler toggle the state of output #0.  The
'     timer interval is set for 1 Sec.
'
'   Output #1
'     Set the Output #1 on and off.  The state of the line will stay until the
'     button is clicked.
'
'   Pulse Output #2
'     It will change state for 500 mSec. and then return to its initial state.
'
'   Input Event Handlers
'     When ever an input voltage is applied to any of the four inputs, an event is generated and
'     counted.  The input event counts are displayed.
'
' The setup used when running the sample code was the following
'  BI0 -> BI4   pin14-Pin18
'  BI1 -> BI5   pin15-pin19
'  BI2 -> BI6   pin16-pin20
'  BI3 -> BI7   pin17-pin21

Option Explicit On 
' Needed for VisionPro
Imports Cognex.VisionPro
' Needed for VisionPro Exceptions
Imports Cognex.VisionPro.Exceptions
' Needed for VisionPro 8504 Framegrabber
Imports Cognex.VisionPro.FG8504

Namespace PIOAnd8504
  Public Class Form1
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
      If disposing Then
        If Not (components Is Nothing) Then
          components.Dispose()
        End If
        Dim frameGrabbers As New CogFrameGrabbers
        For Each fg As ICogFrameGrabber In frameGrabbers
          fg.Disconnect(False)
        Next
      End If
      MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Text1 As System.Windows.Forms.TextBox
    Friend WithEvents out0TimerCmd As System.Windows.Forms.Button
    Friend WithEvents out1Cmd As System.Windows.Forms.Button
    Friend WithEvents out2PulseCmd As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Me.Text1 = New System.Windows.Forms.TextBox
      Me.out0TimerCmd = New System.Windows.Forms.Button
      Me.out1Cmd = New System.Windows.Forms.Button
      Me.out2PulseCmd = New System.Windows.Forms.Button
      Me.Label1 = New System.Windows.Forms.Label
      Me.Label2 = New System.Windows.Forms.Label
      Me.Label3 = New System.Windows.Forms.Label
      Me.Label4 = New System.Windows.Forms.Label
      Me.Label5 = New System.Windows.Forms.Label
      Me.Label6 = New System.Windows.Forms.Label
      Me.Label7 = New System.Windows.Forms.Label
      Me.Label8 = New System.Windows.Forms.Label
      Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
      Me.SuspendLayout()
      '
      'Text1
      '
      Me.Text1.Location = New System.Drawing.Point(8, 280)
      Me.Text1.Multiline = True
      Me.Text1.Name = "Text1"
      Me.Text1.ReadOnly = True
      Me.Text1.Size = New System.Drawing.Size(560, 96)
      Me.Text1.TabIndex = 0
      Me.Text1.Text = "Sample description: demonstrate the different features that output and input line" & _
      "s exhibit." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Sample usage:" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Click Enable Timer Output 0 Button to have output 0 t" & _
      "oggle on and off once a second" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Click Output 1 On Button to turn output 1 on and" & _
      " off" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Click Pulse Output 2 Button to pulse output 2 for 500 mSec." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "When ever an " & _
      "input voltage is applied to any of the four inputs, an event is generated and co" & _
      "unted"
      '
      'out0TimerCmd
      '
      Me.out0TimerCmd.Location = New System.Drawing.Point(24, 24)
      Me.out0TimerCmd.Name = "out0TimerCmd"
      Me.out0TimerCmd.Size = New System.Drawing.Size(144, 48)
      Me.out0TimerCmd.TabIndex = 1
      Me.out0TimerCmd.Text = "Enable Timer Output 0"
      '
      'out1Cmd
      '
      Me.out1Cmd.Location = New System.Drawing.Point(208, 24)
      Me.out1Cmd.Name = "out1Cmd"
      Me.out1Cmd.Size = New System.Drawing.Size(144, 48)
      Me.out1Cmd.TabIndex = 2
      Me.out1Cmd.Text = "Output 1 On"
      '
      'out2PulseCmd
      '
      Me.out2PulseCmd.Location = New System.Drawing.Point(392, 24)
      Me.out2PulseCmd.Name = "out2PulseCmd"
      Me.out2PulseCmd.Size = New System.Drawing.Size(144, 48)
      Me.out2PulseCmd.TabIndex = 3
      Me.out2PulseCmd.Text = "Pulse Output 2"
      '
      'Label1
      '
      Me.Label1.Location = New System.Drawing.Point(152, 120)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(48, 16)
      Me.Label1.TabIndex = 4
      Me.Label1.Text = "Input 0"
      Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'Label2
      '
      Me.Label2.Location = New System.Drawing.Point(152, 152)
      Me.Label2.Name = "Label2"
      Me.Label2.Size = New System.Drawing.Size(48, 16)
      Me.Label2.TabIndex = 5
      Me.Label2.Text = "Input 1"
      Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'Label3
      '
      Me.Label3.Location = New System.Drawing.Point(240, 120)
      Me.Label3.Name = "Label3"
      Me.Label3.Size = New System.Drawing.Size(100, 16)
      Me.Label3.TabIndex = 6
      Me.Label3.Text = "0"
      Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
      '
      'Label4
      '
      Me.Label4.Location = New System.Drawing.Point(240, 152)
      Me.Label4.Name = "Label4"
      Me.Label4.Size = New System.Drawing.Size(100, 16)
      Me.Label4.TabIndex = 7
      Me.Label4.Text = "0"
      Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
      '
      'Label5
      '
      Me.Label5.Location = New System.Drawing.Point(152, 184)
      Me.Label5.Name = "Label5"
      Me.Label5.Size = New System.Drawing.Size(48, 16)
      Me.Label5.TabIndex = 8
      Me.Label5.Text = "Input 2"
      Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'Label6
      '
      Me.Label6.Location = New System.Drawing.Point(152, 216)
      Me.Label6.Name = "Label6"
      Me.Label6.Size = New System.Drawing.Size(48, 16)
      Me.Label6.TabIndex = 9
      Me.Label6.Text = "Input 3"
      Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      '
      'Label7
      '
      Me.Label7.Location = New System.Drawing.Point(240, 183)
      Me.Label7.Name = "Label7"
      Me.Label7.Size = New System.Drawing.Size(100, 16)
      Me.Label7.TabIndex = 10
      Me.Label7.Text = "0"
      Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
      '
      'Label8
      '
      Me.Label8.Location = New System.Drawing.Point(240, 216)
      Me.Label8.Name = "Label8"
      Me.Label8.Size = New System.Drawing.Size(100, 16)
      Me.Label8.TabIndex = 11
      Me.Label8.Text = "0"
      Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
      '
      'Timer1
      '
      Me.Timer1.Interval = 1000
      '
      'Form1
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(576, 382)
      Me.Controls.Add(Me.Label8)
      Me.Controls.Add(Me.Label7)
      Me.Controls.Add(Me.Label6)
      Me.Controls.Add(Me.Label5)
      Me.Controls.Add(Me.Label4)
      Me.Controls.Add(Me.Label3)
      Me.Controls.Add(Me.Label2)
      Me.Controls.Add(Me.Label1)
      Me.Controls.Add(Me.out2PulseCmd)
      Me.Controls.Add(Me.out1Cmd)
      Me.Controls.Add(Me.out0TimerCmd)
      Me.Controls.Add(Me.Text1)
      Me.Name = "Form1"
      Me.Text = "Form1"
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region " Private vars"
    Private DiscreteIO As ICogFrameGrabber

    Private OutPutLines As CogOutputLines
    Private OutputLine0 As CogOutputLine
    Private OutputLine1 As CogOutputLine
    Private OutputLine2 As CogOutputLine
    Private OutputLine3 As CogOutputLine
    Private OutputLine4 As CogOutputLine
    Private OutputLine5 As CogOutputLine
    Private OutputLine6 As CogOutputLine
    Private OutputLine7 As CogOutputLine

    Private WithEvents Input0Event As CogInputLine
    Private WithEvents Input1Event As CogInputLine
    Private WithEvents Input2Event As CogInputLine
    Private WithEvents Input3Event As CogInputLine

    Private eventCount0 As Long
    Private eventCount1 As Long
    Private eventCount2 As Long
    Private eventCount3 As Long

#End Region
#Region " Initialization"
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Try
        InitializeAcquisition()
      Catch ex As CogException
        MessageBox.Show(ex.Message)
        Application.Exit()
      Catch gex As Exception
        MessageBox.Show(gex.Message)
        Application.Exit()
      End Try

    End Sub


    Private Sub InitializeAcquisition()
      ' Step 1) Get a Frame Grabber
      Dim FrameGrabbers As CogFrameGrabbers
      Dim FrameGrabber As ICogFrameGrabber

      FrameGrabbers = New CogFrameGrabbers
      ' Look for 8504. Terminate if it cannot find the 8504
      Dim FG8504 As ICogFrameGrabber = Nothing
      For Each FrameGrabber In FrameGrabbers
                If (InStr(FrameGrabber.Name, "8504") > 0) Or (InStr(FrameGrabber.Name, "8514") > 0) Or (InStr(FrameGrabber.Name, "8514e") > 0) Then
                    FG8504 = FrameGrabber
                    Exit For
                End If
      Next FrameGrabber

      If FG8504 Is Nothing Then
        Throw New CogAcqNoFrameGrabberException("No 8504 FrameGrabber is found.")
      End If
      DiscreteIO = FG8504

      ' Step 2) Create output lines and set its mode
      CreateOutputLines()

      ' Step 3) Create input lines
      CreateInputLines()

    End Sub
#End Region
#Region " Auxilliary routines"
    ' This routine is used to create all the output lines.
    Private Sub CreateOutputLines()
            Const OutputSWLine0 As Integer = 4
            Const OutputSWLine1 As Integer = 5
            Const OutputSWLine2 As Integer = 6

      OutPutLines = DiscreteIO.OutputLines
      OutputLine0 = OutPutLines(OutputSWLine0)
      OutputLine1 = OutPutLines(OutputSWLine1)
      OutputLine2 = OutPutLines(OutputSWLine2)

      If Not SetupOutputLine(OutputLine0, False) Then  ' Check for pulse mode off
        Throw New CogAcqAbnormalException("Error on setup for Output Line 0.")
      End If

      If Not SetupOutputLine(OutputLine1, False) Then
        Throw New CogAcqAbnormalException("Error on setup for Output Line 1.")
      End If
      If Not SetupOutputLine(OutputLine2, True) Then
        Throw New CogAcqAbnormalException("Error on setup for Output Line 2.")
      End If

    End Sub

    ' This function is used to setup an output.
    Private Function SetupOutputLine(ByVal outputLine As CogOutputLine, ByVal enablePulseMode As Boolean) As Boolean
      If outputLine.CanBeEnabled Then
        outputLine.Enabled = True
        outputLine.Value = True
        If enablePulseMode Then
          If outputLine.GetPulseModeSupported(CogOutputLinePulseModeConstants.Low) Then
            outputLine.PulseMode = CogOutputLinePulseModeConstants.Low
            outputLine.PulseDuration = 500 '.5 Sec. Pulse Duration
          Else
            SetupOutputLine = False
            Exit Function
          End If
        End If
      Else
        SetupOutputLine = False
        Exit Function
      End If
      SetupOutputLine = True
    End Function

    ' This routine is used to create all the input lines.
    Private Sub CreateInputLines()
      Dim InputLines As CogInputLines
      Dim InputLine0 As CogInputLine
      Dim InputLine1 As CogInputLine
      Dim InputLine2 As CogInputLine
      Dim InputLine3 As CogInputLine

            Const InputSWLine0 As Integer = 0
            Const InputSWLine1 As Integer = 1
            Const InputSWLine2 As Integer = 2
            Const InputSWLine3 As Integer = 3

      InputLines = DiscreteIO.InputLines
      InputLine0 = InputLines(InputSWLine0)
      InputLine1 = InputLines(InputSWLine1)
      InputLine2 = InputLines(InputSWLine2)
      InputLine3 = InputLines(InputSWLine3)

      If SetupInputLine(InputLine0) Then
        eventCount0 = 0
        Input0Event = InputLine0
      Else
        Throw New CogAcqAbnormalException("Error on setup for Input Line 0.")
      End If

      If SetupInputLine(InputLine1) Then
        eventCount1 = 0
        Input1Event = InputLine1
      Else
        Throw New CogAcqAbnormalException("Error on setup for Input Line 1.")
      End If

      If SetupInputLine(InputLine2) Then
        eventCount2 = 0
        Input2Event = InputLine2
      Else
        Throw New CogAcqAbnormalException("Error on setup for Input Line 2.")
      End If

      If SetupInputLine(InputLine3) Then
        eventCount3 = 0
        Input3Event = InputLine3
      Else
        Throw New CogAcqAbnormalException("Error on setup for Input Line 3.")
      End If
    End Sub

    ' This function is used to setup an input.
    Private Function SetupInputLine(ByVal inputLine As CogInputLine) As Boolean
      If inputLine.CanBeEnabled Then
        inputLine.Enabled = True
        inputLine.TriggerMode = CogInputLineTriggerModeConstants.LowToHigh
      Else
        SetupInputLine = False
      End If
      SetupInputLine = True
    End Function

#End Region
#Region " Input lines event handlers"
    ' Input #0 Event Handler
        Private Sub Input0Event_LowToHigh(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogInputLineEventArgs) Handles Input0Event.LowToHigh
          If InvokeRequired Then
            Dim eventArgs() As Object = {sender, e}
            Invoke(New CogLowToHighEventHandler(AddressOf Input0Event_LowToHigh), _
              eventArgs)
            Return
          End If
          eventCount0 = eventCount0 + 1
          Label3.Text = eventCount0.ToString
        End Sub
    ' Input #1 Event Handler
        Private Sub Input1Event_LowToHigh(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogInputLineEventArgs) Handles Input1Event.LowToHigh
          If InvokeRequired Then
            Dim eventArgs() As Object = {sender, e}
            Invoke(New CogLowToHighEventHandler(AddressOf Input1Event_LowToHigh), _
              eventArgs)
            Return
          End If
          eventCount1 = eventCount1 + 1
          Label4.Text = eventCount1.ToString
        End Sub
    ' Input #2 Event Handler
        Private Sub Input2Event_LowToHigh(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogInputLineEventArgs) Handles Input2Event.LowToHigh
          If InvokeRequired Then
            Dim eventArgs() As Object = {sender, e}
            Invoke(New CogLowToHighEventHandler(AddressOf Input2Event_LowToHigh), _
              eventArgs)
            Return
          End If
          eventCount2 = eventCount2 + 1
          Label7.Text = eventCount2.ToString
        End Sub
    ' Input #3 Event Handler
        Private Sub Input3Event_LowToHigh(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogInputLineEventArgs) Handles Input3Event.LowToHigh
          If InvokeRequired Then
            Dim eventArgs() As Object = {sender, e}
            Invoke(New CogLowToHighEventHandler(AddressOf Input3Event_LowToHigh), _
              eventArgs)
            Return
          End If
          eventCount3 = eventCount3 + 1
          Label8.Text = eventCount3.ToString
        End Sub

#End Region
#Region " Timer and command buttons event handlers"
    ' Timer Event Handler
    ' More comments about why you want to do this
        Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
            If OutputLine0.Value Then
                OutputLine0.Value = False
            Else
                OutputLine0.Value = True
            End If
        End Sub

    Private Sub out0TimerCmd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles out0TimerCmd.Click
      If Timer1.Enabled = False Then
        Timer1.Enabled = True
        out0TimerCmd.Text = "Disable Timer Output 0"
      Else
        Timer1.Enabled = False
        out0TimerCmd.Text = "Enable Timer Output 0"
      End If

    End Sub

        Private Sub out1Cmd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles out1Cmd.Click
          If OutputLine1.Value Then
            OutputLine1.Value = False
            out1Cmd.Text = "Output 1 Off"
          Else
            OutputLine1.Value = True
            out1Cmd.Text = "Output 1 On"
          End If

        End Sub

    Private Sub out2PulseCmd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles out2PulseCmd.Click
      ' This will cause Output Line #2 to pulse on for 500 mSec.
      OutputLine2.Value = False
    End Sub
#End Region

  End Class

End Namespace