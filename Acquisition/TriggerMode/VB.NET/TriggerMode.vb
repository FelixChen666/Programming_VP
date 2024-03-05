'*******************************************************************************
' Copyright (C) 2004-2010 Cognex Corporation
'
' Subject to Cognex Corporation's terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
'*******************************************************************************
'
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This sample demonstrates how to switch between manual and hardware trigger mode.
'
' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
'
' Follow the next steps to set one of the trigger mode.
' Step 1) Create a CogAcqFifoTool (see Form_Load).
' Step 2) Get an instance of the CogAcqTrigger object.
' Step 3) Set manual trigger mode
' Step 4) Set hardware trigger mode
'
Option Explicit On 
Imports System.Threading
' Needed for VisionPro
Imports Cognex.VisionPro
' Needed for CogException
Imports Cognex.VisionPro.Exceptions
Namespace TriggerMode
  Public Class TriggerMode
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
    Friend WithEvents CogDisplay1 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents lblBoardType As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblVideoFormat As System.Windows.Forms.Label
    Friend WithEvents cmdRun As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents optManual As System.Windows.Forms.RadioButton
    Friend WithEvents optAuto As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(TriggerMode))
      Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
      Me.txtDescription = New System.Windows.Forms.TextBox
      Me.lblBoardType = New System.Windows.Forms.Label
      Me.Label1 = New System.Windows.Forms.Label
      Me.Label2 = New System.Windows.Forms.Label
      Me.lblVideoFormat = New System.Windows.Forms.Label
      Me.cmdRun = New System.Windows.Forms.Button
      Me.GroupBox1 = New System.Windows.Forms.GroupBox
      Me.optManual = New System.Windows.Forms.RadioButton
      Me.optAuto = New System.Windows.Forms.RadioButton
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.GroupBox1.SuspendLayout()
      Me.SuspendLayout()
      '
      'CogDisplay1
      '
      Me.CogDisplay1.Location = New System.Drawing.Point(272, 8)
      Me.CogDisplay1.Name = "CogDisplay1"
      Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
      Me.CogDisplay1.Size = New System.Drawing.Size(320, 384)
      Me.CogDisplay1.TabIndex = 0
      '
      'txtDescription
      '
      Me.txtDescription.Location = New System.Drawing.Point(8, 400)
      Me.txtDescription.Multiline = True
      Me.txtDescription.Name = "txtDescription"
      Me.txtDescription.Size = New System.Drawing.Size(592, 48)
      Me.txtDescription.TabIndex = 3
      Me.txtDescription.Text = "Sample description: demonstrates how to switch between manual and hardware auto t" & _
      "rigger mode. A Cognex frame grabber " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "must be present in order to run this sampl" & _
      "e. The sample will acquire images and display them when the Run button is " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "pres" & _
      "sed."
      Me.txtDescription.WordWrap = False
      '
      'lblBoardType
      '
      Me.lblBoardType.Location = New System.Drawing.Point(112, 8)
      Me.lblBoardType.Name = "lblBoardType"
      Me.lblBoardType.Size = New System.Drawing.Size(152, 24)
      Me.lblBoardType.TabIndex = 6
      Me.lblBoardType.Text = "Unknown"
      '
      'Label1
      '
      Me.Label1.Location = New System.Drawing.Point(16, 8)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(80, 16)
      Me.Label1.TabIndex = 5
      Me.Label1.Text = "Board Type"
      '
      'Label2
      '
      Me.Label2.Location = New System.Drawing.Point(8, 48)
      Me.Label2.Name = "Label2"
      Me.Label2.Size = New System.Drawing.Size(176, 16)
      Me.Label2.TabIndex = 7
      Me.Label2.Text = "Selected Video Format"
      '
      'lblVideoFormat
      '
      Me.lblVideoFormat.Location = New System.Drawing.Point(8, 72)
      Me.lblVideoFormat.Name = "lblVideoFormat"
      Me.lblVideoFormat.Size = New System.Drawing.Size(256, 48)
      Me.lblVideoFormat.TabIndex = 8
      Me.lblVideoFormat.Text = "Unknown"
      '
      'cmdRun
      '
      Me.cmdRun.Location = New System.Drawing.Point(72, 296)
      Me.cmdRun.Name = "cmdRun"
      Me.cmdRun.Size = New System.Drawing.Size(120, 40)
      Me.cmdRun.TabIndex = 9
      Me.cmdRun.Text = "Run"
      '
      'GroupBox1
      '
      Me.GroupBox1.Controls.Add(Me.optAuto)
      Me.GroupBox1.Controls.Add(Me.optManual)
      Me.GroupBox1.Location = New System.Drawing.Point(48, 144)
      Me.GroupBox1.Name = "GroupBox1"
      Me.GroupBox1.Size = New System.Drawing.Size(176, 112)
      Me.GroupBox1.TabIndex = 10
      Me.GroupBox1.TabStop = False
      Me.GroupBox1.Text = "Trigger Mode"
      '
      'optManual
      '
      Me.optManual.Location = New System.Drawing.Point(16, 32)
      Me.optManual.Name = "optManual"
      Me.optManual.Size = New System.Drawing.Size(120, 24)
      Me.optManual.TabIndex = 0
      Me.optManual.Text = "Manual"
      '
      'optAuto
      '
      Me.optAuto.Location = New System.Drawing.Point(16, 72)
      Me.optAuto.Name = "optAuto"
      Me.optAuto.Size = New System.Drawing.Size(120, 24)
      Me.optAuto.TabIndex = 1
      Me.optAuto.Text = "Hardware Auto"
      '
      'TriggerMode
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(600, 454)
      Me.Controls.Add(Me.GroupBox1)
      Me.Controls.Add(Me.cmdRun)
      Me.Controls.Add(Me.lblVideoFormat)
      Me.Controls.Add(Me.Label2)
      Me.Controls.Add(Me.lblBoardType)
      Me.Controls.Add(Me.Label1)
      Me.Controls.Add(Me.txtDescription)
      Me.Controls.Add(Me.CogDisplay1)
      Me.Name = "TriggerMode"
      Me.Text = "Form1"
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.GroupBox1.ResumeLayout(False)
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region "Private vars"
    Private mTool As CogAcqFifoTool
    Private mTrigger As Cognex.VisionPro.ICogAcqTrigger
    Private StopAcquire As Boolean
        Private numacqs As Integer

#End Region
#Region "Initialization"
    Private Sub InitializeAcquisition()
      ' Step 1 - Create an acquisition tool which creates an CogAcqFifo with a default
      '          video format
      mTool = New CogAcqFifoTool

      ' Check if the tool was able to create a default acqfifo.
      If mTool.[Operator] Is Nothing Then
        Throw New CogAcqNoFrameGrabberException("A board might be missing or not be functioning properly.")
      End If
      ' Get an instance of the CogAcqFifo object
      Dim mAcqFifo As Cognex.VisionPro.ICogAcqFifo
      mAcqFifo = mTool.[Operator]
      ' Display the video format.
      lblVideoFormat.Text = mAcqFifo.VideoFormat
      ' Display the board type
      lblBoardType.Text = mTool.[Operator].FrameGrabber.Name

      ' Ignore the error temporarily. When the next statement fails, mTrigger will
      ' still be set to Nothing, and the error will be caught later. This way a more
      ' comprehensive error message can be generated.


      ' Step 2 - Get an instance of the CogAcqTrigger object.
      mTrigger = mTool.[Operator].OwnedTriggerParams
      If mTrigger Is Nothing Then
        Throw New CogAcqNoFrameGrabberException("This board type " & mTool.[Operator].FrameGrabber.Name & _
                            "does not support trigger mode.")
      End If

      ' Hook up the acquisition completion event. Each time a tool acquires
      ' an image, it fires the acquisition completion event.
      AddHandler mTool.[Operator].Complete, _
                                  New CogCompleteEventHandler(AddressOf Acq_Complete)

      ' NOTE: Either the exposure or brightness may need adjustment to clearly see
      '       the acquired image. Both exposure and brightness are set to high values
      '       in case sufficient lighting is unavailable.
      Dim Exposure As Cognex.VisionPro.ICogAcqExposure
      Dim Brightness As Cognex.VisionPro.ICogAcqBrightness
      Exposure = mTool.[Operator].OwnedExposureParams
      Brightness = mTool.[Operator].OwnedBrightnessParams

            'check to make sure the properties are supported and then set them
            If Not Exposure Is Nothing Then
                Exposure.Exposure = 50     ' in milli-seconds (ms)
            End If
            If Not Brightness Is Nothing Then
                Brightness.Brightness = 0.9
            End If

      optManual.Checked = True   ' Initially we are in manual trigger mode

    End Sub
#End Region
#Region "Complete Event"
    Private Sub Acq_Complete(ByVal sender As Object, ByVal e As CogCompleteEventArgs)
      If InvokeRequired Then
        Dim eventArgs() As Object = {sender, e}
        Invoke(New CogCompleteEventHandler(AddressOf Acq_Complete), _
          eventArgs)
        Return
      End If

            Try

                ' When you click the Stop button, the program calls the Flush method. However, the
                ' acquisition completion event might have been fired prior to calling Flush. If this
                ' happens mTool.Operator.CompleteAcquireEx will fail. To prevent this problem,
                ' the sample uses the StopAcquire boolean flag.
                If StopAcquire Then

                    Exit Sub
                End If
                ' Retrieve an image if it is available.
                Dim numReadyVal As Integer, numPendingVal As Integer
                Dim busyVal As Boolean
				Dim info As New CogAcqInfo
								
                mTool.[Operator].GetFifoState(numPendingVal, numReadyVal, busyVal)
                If numReadyVal > 0 Then
                    CogDisplay1.Image = mTool.[Operator].CompleteAcquireEx(info)
                    numacqs += 1
                Else
                    ' TO DO: Add error handling code here.
                End If
                ' Queue another acquisition request if we are in manual trigger mode
                If optManual.Checked Then
                    mTool.[Operator].StartAcquire()
                End If
                ' We need to run the garbage collector on occasion to cleanup
                ' images that are no longer being used.
                If numacqs > 4 Then
                    GC.Collect()
                    numacqs = 0
                End If


            Catch ex As CogException
                MessageBox.Show(ex.Message)
            Catch gex As Exception
                MessageBox.Show(gex.Message)
            End Try

    End Sub
#End Region
#Region " Form controls handlers"
    Private Sub optManual_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optManual.CheckedChanged
      ' Switch to hardware trigger mode
      mTrigger.TriggerEnabled = False
      mTool.[Operator].Flush()
      ' Step 3 - Set manual trigger mode
      mTrigger.TriggerModel = CogAcqTriggerModelConstants.Manual
      mTrigger.TriggerEnabled = True

    End Sub

    Private Sub optAuto_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optAuto.CheckedChanged
      ' Switch to manual trigger mode
      mTrigger.TriggerEnabled = False
      mTool.[Operator].Flush()
      ' Step 3 - Set hardware trigger mode
      mTrigger.TriggerModel = CogAcqTriggerModelConstants.Auto
      mTrigger.TriggerEnabled = True

    End Sub

    Private Sub cmdRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRun.Click
      Try
        ' Now change the button caption.
        If cmdRun.Text = "Run" Then
          optManual.Enabled = False   ' the user cannot change trigger mode during run
          optAuto.Enabled = False
          mTrigger.TriggerEnabled = True  ' enable the trigger
          StopAcquire = False
          ' Flush all outstanding acquisitions since they are not part of new acquisitions.
          mTool.[Operator].Flush()
          ' Call StartAcquire to issue an acquisition request and wait for
          ' the acquisition complete event
          ' Note: For manual triggering, prime the acquisition engine
          '       with two (or more, up to 32) start requests to ensure optimal throughput
          If optManual.Checked Then
            mTool.[Operator].StartAcquire()
            mTool.[Operator].StartAcquire()
          End If
          cmdRun.Text = "Stop"
        Else
          mTrigger.TriggerEnabled = False   ' disable the trigger
          optManual.Enabled = True
          optAuto.Enabled = True
          StopAcquire = True
          ' Flush all outstanding acquisition requests and stop.
          mTool.[Operator].Flush()
          cmdRun.Text = "Run"
        End If
      Catch ex As CogException
        MessageBox.Show(ex.Message)
        Application.Exit()
      Catch gex As Exception
        MessageBox.Show(gex.Message)
        Application.Exit()
      End Try

    End Sub

#End Region

    Private Sub TriggerMode_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
      StopAcquire = True
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