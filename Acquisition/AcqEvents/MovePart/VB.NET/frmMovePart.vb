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

' This sample demonstrates how to catch and handle the move-part events.

' The move-part event fires after the camera has integrated the image, but (possibly)
' before it is available in video memory. When this event fires, it is safe to change
' the scene visible to the camera. This event is fired for all trigger mode.
' This sample will use manual triggering (also called software triggering) to
' acquire images. See StrobedAcq sample for detailed manual trigger setup procedures.

' A Cognex frame grabber must be present in order to run this sample program.
' If no Cognex board is present, the program displays an error and exits.

' Note that the .NET garbage collector is called on a regular basis to free
' image memory that is no longer referenced. 

' This program assumes that you have some knowledge of C# and VisionPro
' programming.

' Follow the next steps in order to catch the move-part event.
' Step 1) Create a  CogAcqFifoTool.
' Step 2) Retrieve the CogAcqMovePartEvent.
' Step 3) Display the number of times this event is called.
'*/
Imports System
Imports Microsoft.VisualBasic
Imports System.Drawing
Imports System.Collections
Imports System.Windows.Forms
Imports System.Data
Imports System.Threading
'Cognex namespace
Imports Cognex.VisionPro
' Needed for CogException
Imports Cognex.VisionPro.Exceptions
Namespace MovePart
  Public Class frmMovePart
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
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents lblBoardType As System.Windows.Forms.Label
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents lblVideoFormat As System.Windows.Forms.Label
    Friend WithEvents acqButton As System.Windows.Forms.Button
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents lblMovePartCount As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMovePart))
      Me.cogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
      Me.txtDescription = New System.Windows.Forms.TextBox
      Me.label1 = New System.Windows.Forms.Label
      Me.lblBoardType = New System.Windows.Forms.Label
      Me.label4 = New System.Windows.Forms.Label
      Me.lblVideoFormat = New System.Windows.Forms.Label
      Me.acqButton = New System.Windows.Forms.Button
      Me.label2 = New System.Windows.Forms.Label
      Me.lblMovePartCount = New System.Windows.Forms.Label
      CType(Me.cogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'cogDisplay1
      '
      Me.cogDisplay1.Location = New System.Drawing.Point(256, 8)
      Me.cogDisplay1.Name = "cogDisplay1"
      Me.cogDisplay1.OcxState = CType(resources.GetObject("cogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
      Me.cogDisplay1.Size = New System.Drawing.Size(288, 320)
      Me.cogDisplay1.TabIndex = 1
      '
      'txtDescription
      '
      Me.txtDescription.Location = New System.Drawing.Point(8, 336)
      Me.txtDescription.Multiline = True
      Me.txtDescription.Name = "txtDescription"
      Me.txtDescription.ReadOnly = True
      Me.txtDescription.Size = New System.Drawing.Size(528, 48)
      Me.txtDescription.TabIndex = 3
      Me.txtDescription.Text = "This sample demonstrates how to handle the move-part event.  A Cognex frame grabb" & _
      "er board must be present in order to run this sample program. When the Run butto" & _
      "n is pressed, the program first flushes all outstanding acquisitions since they " & _
      "are not part of new acquisitions, and acquires images automatically."
      '
      'label1
      '
      Me.label1.Location = New System.Drawing.Point(16, 32)
      Me.label1.Name = "label1"
      Me.label1.Size = New System.Drawing.Size(72, 16)
      Me.label1.TabIndex = 4
      Me.label1.Text = "Board Type:"
      '
      'lblBoardType
      '
      Me.lblBoardType.Location = New System.Drawing.Point(112, 32)
      Me.lblBoardType.Name = "lblBoardType"
      Me.lblBoardType.Size = New System.Drawing.Size(128, 16)
      Me.lblBoardType.TabIndex = 5
      Me.lblBoardType.Text = "Unknown"
      '
      'label4
      '
      Me.label4.Location = New System.Drawing.Point(16, 56)
      Me.label4.Name = "label4"
      Me.label4.Size = New System.Drawing.Size(128, 16)
      Me.label4.TabIndex = 12
      Me.label4.Text = "Selected Video Format:"
      '
      'lblVideoFormat
      '
      Me.lblVideoFormat.Location = New System.Drawing.Point(16, 80)
      Me.lblVideoFormat.Name = "lblVideoFormat"
      Me.lblVideoFormat.Size = New System.Drawing.Size(224, 40)
      Me.lblVideoFormat.TabIndex = 13
      Me.lblVideoFormat.Text = "Unknown"
      '
      'acqButton
      '
      Me.acqButton.Location = New System.Drawing.Point(56, 160)
      Me.acqButton.Name = "acqButton"
      Me.acqButton.Size = New System.Drawing.Size(104, 32)
      Me.acqButton.TabIndex = 14
      Me.acqButton.Text = "Run"
      '
      'label2
      '
      Me.label2.Location = New System.Drawing.Point(24, 232)
      Me.label2.Name = "label2"
      Me.label2.Size = New System.Drawing.Size(120, 16)
      Me.label2.TabIndex = 15
      Me.label2.Text = "Move part event count:"
      '
      'lblMovePartCount
      '
      Me.lblMovePartCount.Location = New System.Drawing.Point(168, 232)
      Me.lblMovePartCount.Name = "lblMovePartCount"
      Me.lblMovePartCount.Size = New System.Drawing.Size(64, 16)
      Me.lblMovePartCount.TabIndex = 16
      Me.lblMovePartCount.Text = "0"
      '
      'frmMovePart
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(544, 390)
      Me.Controls.Add(Me.lblMovePartCount)
      Me.Controls.Add(Me.label2)
      Me.Controls.Add(Me.acqButton)
      Me.Controls.Add(Me.lblVideoFormat)
      Me.Controls.Add(Me.label4)
      Me.Controls.Add(Me.lblBoardType)
      Me.Controls.Add(Me.label1)
      Me.Controls.Add(Me.txtDescription)
      Me.Controls.Add(Me.cogDisplay1)
      Me.Name = "frmMovePart"
      Me.Text = "Show how to capture the move part event"
      CType(Me.cogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region " Private vars"
    Private mTool As CogAcqFifoTool
    Private numAcqs As Integer = 0
    Private stopAcquire As Boolean
    Private mAcqFifo As Cognex.VisionPro.ICogAcqFifo

#End Region
#Region "Initialization"
    ' Creates a new CogAcqFifoTool, hooks acquisition events, initializes acquisition parameters
    '
    Private Sub InitializeAcquisition()
      ' Step 1 - Create an acquisition tool which creates an CogAcqFifo with a default
      '           video format for that frame grabber.
      mTool = New CogAcqFifoTool

      ' Check if the tool was able to create a default acqfifo.
      If mTool.[Operator] Is Nothing Then
        Throw New CogAcqNoFrameGrabberException("A board might be missing or not be functioning properly.")
      End If
      ' First, Get the ICogAcqFifo.
      mAcqFifo = mTool.[Operator]
      ' Display the video format
      lblVideoFormat.Text = mAcqFifo.VideoFormat
      ' Display the board type
      lblBoardType.Text = mAcqFifo.FrameGrabber.Name

      ' Hook up the acquisition completion event. Each time a tool acquires
      ' an image, it fires the acquisition completion event.
      AddHandler mAcqFifo.Complete, New CogCompleteEventHandler(AddressOf Operator_Complete)

      ' Hook up the acquisition move part event. 
      AddHandler mAcqFifo.MovePart, New CogMovePartEventHandler(AddressOf Operator_MovePart)

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
#Region "Complete and move part events"
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

        ' When you click the Stop button, the program calls the Flush method. However, the
        ' acquisition completion event might have been fired prior to calling Flush. If this
        ' happens mAcqFifo.Complete() will fail. To prevent this problem,
        ' the sample uses the StopAcquire boolean flag.
        If stopAcquire Then
          Return
        End If

        mAcqFifo.GetFifoState(numPendingVal, numReadyVal, busyVal)
        If numReadyVal > 0 Then
          cogDisplay1.Image = mAcqFifo.CompleteAcquireEx(info)
        End If
        numAcqs += 1
        ' We need to run the garbage collector on occasion to cleanup
        ' images that are no longer being used.
        If numAcqs > 4 Then

          GC.Collect()
          numAcqs = 0
        End If
        mAcqFifo.StartAcquire() ' request another acquisition

      Catch ce As CogException

        MessageBox.Show("The following error has occured" & vbCrLf & ce.Message)
      Catch ge As Exception

        MessageBox.Show("The following error has occured" & vbCrLf & ge.Message)

      End Try
    End Sub
    ' This is the movepart event handler for acquisition.   
    ' this next bit of code converts a string to an int32, adds 1,
    ' and then converts it back to a string for displaying as a label
    Private Sub Operator_MovePart(ByVal sender As Object, ByVal e As CogMovePartEventArgs)
      If InvokeRequired Then
        Dim eventArgs() As Object = {sender, e}
        Invoke(New CogMovePartEventHandler(AddressOf Operator_MovePart), _
          eventArgs)
        Return
      End If

      lblMovePartCount.Text = ((System.Int32.Parse(lblMovePartCount.Text, _
                                System.Globalization.NumberStyles.Integer, Nothing) + 1)).ToString()

    End Sub
#End Region
#Region "Run button click event"
    ' This method is changed event handler for the acqButton control
    ' on form.  It starts image acquisition.
    Private Sub acqButton_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles acqButton.Click

      Try

        If acqButton.Text = "Run" Then

          stopAcquire = False
          lblMovePartCount.Text = "0"
          ' Flush all outstanding acquisitions since they are not part of new acquisitions.
          mAcqFifo.Flush()
          ' Call StartAcquire to issue an acquisition request and wait for
          ' the acquisition complete event
          ' Note: For manual triggering, prime the acquisition engine
          ' with two (or more, up to 32) start requests to ensure optimal throughput.
          mAcqFifo.StartAcquire() '
          mAcqFifo.StartAcquire() '
          acqButton.Text = "Stop" '

        Else

          stopAcquire = True
          ' Flush all outstanding acquisition requests and stop.
          mAcqFifo.Flush()
          acqButton.Text = "Run"

        End If
      Catch ce As CogException

        MessageBox.Show("The following error has occured" & vbCrLf & ce.Message)
      Catch ge As Exception

        MessageBox.Show("The following error has occured" & vbCrLf & ge.Message)

      End Try
    End Sub
#End Region
    ' performs initialization
    Private Sub frmMovePart_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
      stopAcquire = True
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
