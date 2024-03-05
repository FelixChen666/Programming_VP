'/*******************************************************************************
' Copyright (C) 2004-2010 Cognex Corporation

' Subject to Cognex Corporations terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
'*******************************************************************************
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This sample demonstrates how to catch and handle the overrun event.

' The overrun event fires when an acquisition fails because, even though
' the acquisition system was able to obtain the required resources, it was unable
' to start the acquisition. This is because the acquisition system received another
' trigger while it was still acquiring an image.

' Hardware trigger mode (a.k.a automatic triggering) is used to show this event.
' See HardwareTrigger sample for detailed setup procedures because this sample
' will not illustrate every step.

' A Cognex frame grabber must be present in order to run this sample program.
' If no Cognex board is present, the program displays an error and exits.

' Note that the .NET garbage collector is called on a regular basis to free
' image memory that is no longer referenced. 

' This program assumes that you have some knowledge of C# and VisionPro
' programming.

' Follow the next steps in order to catch and handle the overrun event.
' Step 1) Create a CogAcqFifoTool.
' Step 2) Set TriggerEnabled property to False.
' Step 3) Select hardware auto trigger mode.
' Step 4) When a single acquisition is completed, the ICogAcqFifo will fire
'         the acquisition completion event. The acquisition completion event handler
'          must be initialized to catch this event.
' Step 5) Hook up the overrun event.
' Step 6) Set TriggerEnabled property to True and wait for external triggers.
' Step 7) Display the overrun error count
'*/

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.Windows.Forms
Imports System.Data
Imports System.Threading
' Cognex namespace
Imports Cognex.VisionPro
' Needed for CogException
Imports Cognex.VisionPro.Exceptions
Namespace Overrun
  Public Class frmOverrun
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
        For Each fg As Cognex.VisionPro.ICogFrameGrabber In frameGrabbers
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
    Friend WithEvents cogDisplay1 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents triggerInfoText As System.Windows.Forms.Label
    Friend WithEvents lblOverrunCount As System.Windows.Forms.Label
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents lblVideoFormat As System.Windows.Forms.Label
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents acqButton As System.Windows.Forms.Button
    Friend WithEvents lblBoardType As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmOverrun))
      Me.cogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
      Me.txtDescription = New System.Windows.Forms.TextBox
      Me.triggerInfoText = New System.Windows.Forms.Label
      Me.lblOverrunCount = New System.Windows.Forms.Label
      Me.label2 = New System.Windows.Forms.Label
      Me.lblVideoFormat = New System.Windows.Forms.Label
      Me.label4 = New System.Windows.Forms.Label
      Me.acqButton = New System.Windows.Forms.Button
      Me.lblBoardType = New System.Windows.Forms.Label
      Me.label1 = New System.Windows.Forms.Label
      CType(Me.cogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'cogDisplay1
      '
      Me.cogDisplay1.Location = New System.Drawing.Point(248, 8)
      Me.cogDisplay1.Name = "cogDisplay1"
      Me.cogDisplay1.OcxState = CType(resources.GetObject("cogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
      Me.cogDisplay1.Size = New System.Drawing.Size(304, 296)
      Me.cogDisplay1.TabIndex = 1
      '
      'txtDescription
      '
      Me.txtDescription.Location = New System.Drawing.Point(4, 320)
      Me.txtDescription.Multiline = True
      Me.txtDescription.Name = "txtDescription"
      Me.txtDescription.ReadOnly = True
      Me.txtDescription.Size = New System.Drawing.Size(528, 72)
      Me.txtDescription.TabIndex = 2
      Me.txtDescription.Text = "This sample demonstrates how to handle the overrun event.  A Cognex frame grabber" & _
      " board must be present in order to run this sample program. When the Run button " & _
      "is pressed, the program first flushes all outstanding acquisitions since they ar" & _
      "e not part of new acquisitions.  It then enables the trigger enable property, an" & _
      "d awaits hardware triggers.  No image will be captured without a hardware trigge" & _
      "r."
      '
      'triggerInfoText
      '
      Me.triggerInfoText.Location = New System.Drawing.Point(16, 208)
      Me.triggerInfoText.Name = "triggerInfoText"
      Me.triggerInfoText.Size = New System.Drawing.Size(176, 16)
      Me.triggerInfoText.TabIndex = 23
      Me.triggerInfoText.Text = " Waiting to run..."
      '
      'lblOverrunCount
      '
      Me.lblOverrunCount.Location = New System.Drawing.Point(144, 256)
      Me.lblOverrunCount.Name = "lblOverrunCount"
      Me.lblOverrunCount.Size = New System.Drawing.Size(64, 16)
      Me.lblOverrunCount.TabIndex = 22
      Me.lblOverrunCount.Text = "0"
      '
      'label2
      '
      Me.label2.Location = New System.Drawing.Point(8, 256)
      Me.label2.Name = "label2"
      Me.label2.Size = New System.Drawing.Size(120, 16)
      Me.label2.TabIndex = 21
      Me.label2.Text = "Overrun event count:"
      '
      'lblVideoFormat
      '
      Me.lblVideoFormat.Location = New System.Drawing.Point(16, 88)
      Me.lblVideoFormat.Name = "lblVideoFormat"
      Me.lblVideoFormat.Size = New System.Drawing.Size(216, 48)
      Me.lblVideoFormat.TabIndex = 20
      Me.lblVideoFormat.Text = "Unknown"
      '
      'label4
      '
      Me.label4.Location = New System.Drawing.Point(8, 56)
      Me.label4.Name = "label4"
      Me.label4.Size = New System.Drawing.Size(128, 16)
      Me.label4.TabIndex = 19
      Me.label4.Text = "Selected Video Format:"
      '
      'acqButton
      '
      Me.acqButton.Location = New System.Drawing.Point(32, 152)
      Me.acqButton.Name = "acqButton"
      Me.acqButton.Size = New System.Drawing.Size(104, 32)
      Me.acqButton.TabIndex = 18
      Me.acqButton.Text = "Run"
      '
      'lblBoardType
      '
      Me.lblBoardType.Location = New System.Drawing.Point(104, 24)
      Me.lblBoardType.Name = "lblBoardType"
      Me.lblBoardType.Size = New System.Drawing.Size(128, 16)
      Me.lblBoardType.TabIndex = 17
      Me.lblBoardType.Text = "Unknown"
      '
      'label1
      '
      Me.label1.Location = New System.Drawing.Point(16, 24)
      Me.label1.Name = "label1"
      Me.label1.Size = New System.Drawing.Size(72, 16)
      Me.label1.TabIndex = 16
      Me.label1.Text = "Board Type:"
      '
      'frmOverrun
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(552, 406)
      Me.Controls.Add(Me.triggerInfoText)
      Me.Controls.Add(Me.lblOverrunCount)
      Me.Controls.Add(Me.label2)
      Me.Controls.Add(Me.lblVideoFormat)
      Me.Controls.Add(Me.label4)
      Me.Controls.Add(Me.acqButton)
      Me.Controls.Add(Me.lblBoardType)
      Me.Controls.Add(Me.label1)
      Me.Controls.Add(Me.txtDescription)
      Me.Controls.Add(Me.cogDisplay1)
      Me.Name = "frmOverrun"
      Me.Text = "Show how to capture the overrun event"
      CType(Me.cogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region "Private vars"
    Private mTool As CogAcqFifoTool
    Private numAcqs As Integer = 0
    Private mAcqFifo As Cognex.VisionPro.ICogAcqFifo
    Private mTrigger As Cognex.VisionPro.ICogAcqTrigger

#End Region
#Region " Initialization"
    ' Creates a new CogAcqFifoTool,initializes acquitision,hooks acquisition events
    '
    Public Sub InitializeAcquisition()

      ' Step 1 - Create an acquisition tool which creates an CogAcqFifo with a default
      '           video format for that frame grabber.
      mTool = New CogAcqFifoTool  '

      ' Check if the tool was able to create a default acqfifo.
      If mTool.[Operator] Is Nothing Then
        Throw New CogAcqNoFrameGrabberException("A board might be missing or not be functioning properly.")
      End If
      ' First, Get the ICogAcqFifo.
      mAcqFifo = mTool.[Operator]
      ' Display the video format
      lblVideoFormat.Text = mAcqFifo.VideoFormat
      ' Display the board type
      lblBoardType.Text = mAcqFifo.FrameGrabber.Name '

      ' Step 2: Get the trigger operator (aka trigger acquisition property)
      mTrigger = mAcqFifo.OwnedTriggerParams
      If mTrigger Is Nothing Then
        Throw New CogAcqWrongTriggerModelException("Trigger model not supported")
      End If
      ' Turn off triggers
      mTrigger.TriggerEnabled = False
      ' Step 3: Select hardware triggers
      mTrigger.TriggerModel = CogAcqTriggerModelConstants.Auto

      ' Step 4: Hook up the acquisition completion event. Each time a tool acquires
      ' an image, it fires the acquisition completion event.
      AddHandler mAcqFifo.Complete, New CogCompleteEventHandler(AddressOf Operator_Complete)

      ' Step 5: Hook up the acquisition overrun event
      AddHandler mAcqFifo.Overrun, New CogOverrunEventHandler(AddressOf mAcqFifo_Overrun)

      ' NOTE: Either the exposure or brightness may need adjustment to clearly see
      ' the acquired image. Both exposure and brightness are set to high values
      ' in case sufficient lighting is unavailable.

            If Not mAcqFifo.OwnedExposureParams Is Nothing Then
                mAcqFifo.OwnedExposureParams.Exposure = 50  'mSecs
            End If
            If Not mAcqFifo.OwnedBrightnessParams Is Nothing Then
                mAcqFifo.OwnedBrightnessParams.Brightness = 0.9
            End If


        End Sub
#End Region
#Region " Event handlers"
    ' This is the complete event handler for acquisition.  When an image is acquired,
    ' it fires a complete event.  This handler verifies the state of the acquisition
    ' fifo, and then calls Complete(), which gets the image from the fifo.

    ' Note that it is necessary to call the .NET garbarge collector on a regular
    ' basis so large images that are no longer used will be released back to the
    ' heap.  In this sample, it is called every 5th acqusition.
    Private Sub Operator_Complete(ByVal sender As Object, ByVal e As CogCompleteEventArgs)

      If InvokeRequired Then
        Dim eventArgs() As Object = {sender, e}
        Invoke(New CogCompleteEventHandler(AddressOf Operator_Complete), _
          eventArgs)
        Return
      End If

      Dim numReadyVal, numPendingVal As Integer
      Dim busyVal As Boolean
	  Dim info As New CogAcqInfo
      Try
        mAcqFifo.GetFifoState(numPendingVal, numReadyVal, busyVal)
        If numReadyVal > 0 Then
          cogDisplay1.Image = _
                  mAcqFifo.CompleteAcquireEx(info)
        End If
        numAcqs += 1
        ' We need to run the garbage collector on occasion to cleanup
        ' images that are no longer being used.
        If numAcqs > 4 Then

          GC.Collect()
          numAcqs = 0
        End If

      Catch ce As CogException
        MessageBox.Show("The following error has occured " & vbCr & ce.Message)
      Catch ge As Exception
        MessageBox.Show("The following error has occured " & vbCr & ge.Message)
      End Try
    End Sub
    ' Overrun event handler
    ' this next bit of code converts a string to an int32, adds 1,
    ' and then converts it back to a string for displaying as a label
    Private Sub mAcqFifo_Overrun(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogOverrunEventArgs)

      If InvokeRequired Then
        Dim eventArgs() As Object = {sender, e}
        Invoke(New CogOverrunEventHandler(AddressOf mAcqFifo_Overrun), _
          eventArgs)
        Return
      End If

      lblOverrunCount.Text = ((System.Int32.Parse(lblOverrunCount.Text, _
                  System.Globalization.NumberStyles.Integer, Nothing) + 1)).ToString()

    End Sub
    ' This method is the clicked event handler for the acqButton control
    ' on form.  It starts image acquisition.
    Private Sub acqButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles acqButton.Click

      Try

        If acqButton.Text = "Run" Then

          lblOverrunCount.Text = "0" '
          ' Flush all outstanding acquisitions since they are not part of new acquisitions.
          mAcqFifo.Flush() '
          mTrigger.TriggerEnabled = True '
          triggerInfoText.Text = "Waiting for hardware triggers..." '
          acqButton.Text = "Stop" '

        Else

          mTrigger.TriggerEnabled = False '
          triggerInfoText.Text = "Waiting to run..." '
          acqButton.Text = "Run" '
        End If
      Catch ce As CogException
        MessageBox.Show("The following error has occured " & vbCr & ce.Message)
      Catch ge As Exception
        MessageBox.Show("The following error has occured " & vbCr & ge.Message)
      End Try

    End Sub

#End Region
    ' performs initialization
    Private Sub frmOverrun_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
      mTrigger.TriggerEnabled = False
      mAcqFifo.Flush()
      Dim counter As Integer
      counter = 0
      While (counter < 10)
        Application.DoEvents()
        Thread.Sleep(1)
        counter = counter + 1
      End While

    End Sub
  End Class
End Namespace