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

' The SyncSimulAcq sample program is intended to demonstrate how one
' might use VisionPro(R) to achieve a high level of application
' throughput by overlapping image processing with synchronous
' simultaneous image acquisition. This is accomplished within the
' context of a single threaded Visual Basic application.
'
' The general approach taken here is to use the acquisition complete
' event to initiate image processing as soon as a set of simultaneously
' acquired images become available.  In addition, the same event handler
' can queue up a manual trigger mode acquisition request prior to
' actually processing the current set of images so that acquisition of
' the next set of images can overlap processing of the current set.
'
' In addition to overlapping acquisition and image processing, this
' sample program demonstrates the impact on throughput of:
'   * updating display controls for every acquisition,
' This is accomplished by providing GUI controls that permit the
' toggling of these characteristics (i.e. do or do not update the
' displays at run time).
'
' For the sake of this sample program, the image-processing task has
' been defined to be the location of the largest blob in the set of
' images. This is done using the blob tool.  For every set of images
' acquired the blob tool is run on each individual image, the largest
' blob is identified, and the center of mass of that blob is reported
' back to the GUI as well as displayed graphically on the corresponding
' image.  The selection of this image-processing task is arbitrary - it
' could be any image-processing task.
'
'
' TIMING ANALYSIS
'
' As an aid to evaluating the impact of various implementation choices
' on throughput, the application computes and reports a cycle time in
' milliseconds for each set of images that is acquired and
' processed. This reported time corresponds to the interval from the
' delivery to the GUI of the previous results to (just before) the
' delivery to the GUI of the current results.
'
' Please note that for sufficiently small image processing tasks (such
' as when the blob tool ROI is very small), it is possible to observe a
' reported cycle time of less than normal acquisition time (33 ms for
' RS170 cameras). This can happen when more than one completed set of
' acquisitions becomes available for consumption at any given
' instant. In this case, the cycle time does not incorporate the usual
' amount of acquisition time.
'
' SINGLE THREADED APPLICATIONS

' This application is written as a single threaded application. To facilitate this,
' image analysis is performed within the complete event handler.  This will work as
' long as the image analysis time is less than the time between acquisitions.  If
' the time between acquisitions is shorter than the image analysis time, then
' acquisitions will not be serviced in a timely manner.  This is because the complete
' event handler will not be ready to respond to the next complete event when it
' is fired.  Instead, complete events will queue, and be handled when image
' processing finishes and the complete event handler exits.

' If two many images are awaiting the application to call complete, the acquisition
' system will start issuing exceptions indicating that no buffer space is available
' for acqusitions.

' Also note that with single threaded applications, acquisition, processing, and
' the GUI are all running in the same thread.  If acquisition and processing 
' consume most of the CPU, the GUI will become unresponsive.  Multi-threaded
' applications tend to be more GUI responsive, since they can be constructed such
' that the GUI occasionally gets a time slice of the CPU.


' GARBAGE COLLECTION

' This sample application explicitly calls the .NET garbage to free
' image memory that is no longer referenced. If the garbarge collector was not 
' called, the application may eventually run out of memory.

'
' OF SPECIAL INTEREST
'
' A few techniques used in this sample program warrant some extra
' attention.
'
' 1. When using manual acquisition triggering, you should "prime" the
'    acquisition pipeline with two (or more, up to 32) acquisition start
'    requests. This will help prevent the acquisition engine from
'    becoming stalled for want of an acquisition request.
'
' SUPPORT FOR DIFFERENT ACQUISITION TRIGGER MODELS
'
' This sample program supports the use of manual, semi, and automatic
' acquisition trigger models. This is done by specifically checking the
' master acq fifo's current trigger model and only invoking StartAcquire
' when it is allowed (i.e. when the trigger model is either manual or
' semi, but not when it is auto or slave). This program explicitly
' demonstrates how to programmatically set the trigger model to manual.
'
' This program assumes that you have good knowledge of Visual Basic and VisionPro
' programming. Studying the following samples will help you better understand this sample.
' samples\Programming\Acquisition\AcqEvents\Change
' samples\Programming\Acquisition\AcqToolEditCtl
' samples\Programming\Acquisition\CreateAcqFifo
' samples\Programming\Acquisition\StrobedAcq
' samples\Programming\Acquisition\AcqImageProcess
'
Option Explicit On 
Imports System.Threading
' Cognex namespace
Imports Cognex.VisionPro
' Needed for CogException
Imports Cognex.VisionPro.Exceptions
' Needed for image processing
Imports Cognex.VisionPro.Blob
Namespace SyncSimulAcq
  Public Class Form1
    Inherits System.Windows.Forms.Form
#Region " Private vars"
    Private AcqFifoToolMaster As CogAcqFifoTool
    Private m_MastAcqFifo As Cognex.VisionPro.ICogAcqFifo
    Private m_AcqFifoSlave As Cognex.VisionPro.ICogAcqFifo
    Private m_AcqSimul As Cognex.VisionPro.ICogAcqSimultaneous
    Private BlobTool As CogBlobTool
    Private numAcqs As Integer = 0
    Private StopWatch As CogStopwatch
    Private Processing As Boolean = False
    Private StopAcquire As Boolean
    Private TriggerOperator As Cognex.VisionPro.ICogAcqTrigger
    Private Rect As CogRectangle
    Private totalAcqs As Integer = 0
    Private mFrameGrabber As Cognex.VisionPro.ICogFrameGrabber
    Friend WithEvents CogAcqFifoEditV21 As Cognex.VisionPro.CogAcqFifoEditV2
    Private Const defaultVideoFrmt As String = "Sony XC-75 640x480 IntDrv CCF"

#End Region
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
        CogAcqFifoEditV21.Dispose()
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
    Friend WithEvents tabAll As System.Windows.Forms.TabControl
    Friend WithEvents AcqFifoTab As System.Windows.Forms.TabPage
    Friend WithEvents ControlTab As System.Windows.Forms.TabPage
    Friend WithEvents StartButton As System.Windows.Forms.Button
    Friend WithEvents group1box As System.Windows.Forms.GroupBox
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents AcqFifoConnectCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents DisplayUpdateCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ctrDisplayMaster As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents ctrDisplaySlave As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents txtResultsY As System.Windows.Forms.TextBox
    Friend WithEvents txtResultsX As System.Windows.Forms.TextBox
    Friend WithEvents txtResultsT As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
      Me.tabAll = New System.Windows.Forms.TabControl
      Me.ControlTab = New System.Windows.Forms.TabPage
      Me.Label5 = New System.Windows.Forms.Label
      Me.Label4 = New System.Windows.Forms.Label
      Me.ctrDisplaySlave = New Cognex.VisionPro.Display.CogDisplay
      Me.ctrDisplayMaster = New Cognex.VisionPro.Display.CogDisplay
      Me.StartButton = New System.Windows.Forms.Button
      Me.group1box = New System.Windows.Forms.GroupBox
      Me.txtResultsT = New System.Windows.Forms.TextBox
      Me.txtResultsY = New System.Windows.Forms.TextBox
      Me.txtResultsX = New System.Windows.Forms.TextBox
      Me.label3 = New System.Windows.Forms.Label
      Me.label2 = New System.Windows.Forms.Label
      Me.label1 = New System.Windows.Forms.Label
      Me.AcqFifoConnectCheckBox = New System.Windows.Forms.CheckBox
      Me.DisplayUpdateCheckBox = New System.Windows.Forms.CheckBox
      Me.AcqFifoTab = New System.Windows.Forms.TabPage
      Me.TextBox1 = New System.Windows.Forms.TextBox
      Me.CogAcqFifoEditV21 = New Cognex.VisionPro.CogAcqFifoEditV2
      Me.tabAll.SuspendLayout()
      Me.ControlTab.SuspendLayout()
      CType(Me.ctrDisplaySlave, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.ctrDisplayMaster, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.group1box.SuspendLayout()
      Me.AcqFifoTab.SuspendLayout()
      CType(Me.CogAcqFifoEditV21, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'tabAll
      '
      Me.tabAll.Controls.Add(Me.ControlTab)
      Me.tabAll.Controls.Add(Me.AcqFifoTab)
      Me.tabAll.ItemSize = New System.Drawing.Size(120, 30)
      Me.tabAll.Location = New System.Drawing.Point(8, 8)
      Me.tabAll.Name = "tabAll"
      Me.tabAll.SelectedIndex = 0
      Me.tabAll.Size = New System.Drawing.Size(920, 480)
      Me.tabAll.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
      Me.tabAll.TabIndex = 0
      '
      'ControlTab
      '
      Me.ControlTab.Controls.Add(Me.Label5)
      Me.ControlTab.Controls.Add(Me.Label4)
      Me.ControlTab.Controls.Add(Me.ctrDisplaySlave)
      Me.ControlTab.Controls.Add(Me.ctrDisplayMaster)
      Me.ControlTab.Controls.Add(Me.StartButton)
      Me.ControlTab.Controls.Add(Me.group1box)
      Me.ControlTab.Controls.Add(Me.AcqFifoConnectCheckBox)
      Me.ControlTab.Controls.Add(Me.DisplayUpdateCheckBox)
      Me.ControlTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.ControlTab.Location = New System.Drawing.Point(4, 34)
      Me.ControlTab.Name = "ControlTab"
      Me.ControlTab.Size = New System.Drawing.Size(912, 442)
      Me.ControlTab.TabIndex = 0
      Me.ControlTab.Text = "Control"
      '
      'Label5
      '
      Me.Label5.Location = New System.Drawing.Point(712, 392)
      Me.Label5.Name = "Label5"
      Me.Label5.Size = New System.Drawing.Size(88, 32)
      Me.Label5.TabIndex = 12
      Me.Label5.Text = "Slave"
      '
      'Label4
      '
      Me.Label4.Location = New System.Drawing.Point(432, 392)
      Me.Label4.Name = "Label4"
      Me.Label4.Size = New System.Drawing.Size(88, 32)
      Me.Label4.TabIndex = 11
      Me.Label4.Text = "Master"
      '
      'ctrDisplaySlave
      '
      Me.ctrDisplaySlave.Location = New System.Drawing.Point(624, 24)
      Me.ctrDisplaySlave.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
      Me.ctrDisplaySlave.MouseWheelSensitivity = 1
      Me.ctrDisplaySlave.Name = "ctrDisplaySlave"
      Me.ctrDisplaySlave.OcxState = CType(resources.GetObject("ctrDisplaySlave.OcxState"), System.Windows.Forms.AxHost.State)
      Me.ctrDisplaySlave.Size = New System.Drawing.Size(280, 352)
      Me.ctrDisplaySlave.TabIndex = 10
      '
      'ctrDisplayMaster
      '
      Me.ctrDisplayMaster.Location = New System.Drawing.Point(344, 24)
      Me.ctrDisplayMaster.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
      Me.ctrDisplayMaster.MouseWheelSensitivity = 1
      Me.ctrDisplayMaster.Name = "ctrDisplayMaster"
      Me.ctrDisplayMaster.OcxState = CType(resources.GetObject("ctrDisplayMaster.OcxState"), System.Windows.Forms.AxHost.State)
      Me.ctrDisplayMaster.Size = New System.Drawing.Size(272, 352)
      Me.ctrDisplayMaster.TabIndex = 9
      '
      'StartButton
      '
      Me.StartButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.StartButton.Location = New System.Drawing.Point(136, 352)
      Me.StartButton.Name = "StartButton"
      Me.StartButton.Size = New System.Drawing.Size(120, 40)
      Me.StartButton.TabIndex = 8
      Me.StartButton.Text = "Start"
      '
      'group1box
      '
      Me.group1box.Controls.Add(Me.txtResultsT)
      Me.group1box.Controls.Add(Me.txtResultsY)
      Me.group1box.Controls.Add(Me.txtResultsX)
      Me.group1box.Controls.Add(Me.label3)
      Me.group1box.Controls.Add(Me.label2)
      Me.group1box.Controls.Add(Me.label1)
      Me.group1box.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.group1box.Location = New System.Drawing.Point(8, 133)
      Me.group1box.Name = "group1box"
      Me.group1box.Size = New System.Drawing.Size(328, 179)
      Me.group1box.TabIndex = 7
      Me.group1box.TabStop = False
      Me.group1box.Text = "Results"
      '
      'txtResultsT
      '
      Me.txtResultsT.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.txtResultsT.Location = New System.Drawing.Point(184, 128)
      Me.txtResultsT.Name = "txtResultsT"
      Me.txtResultsT.Size = New System.Drawing.Size(136, 26)
      Me.txtResultsT.TabIndex = 5
      '
      'txtResultsY
      '
      Me.txtResultsY.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.txtResultsY.Location = New System.Drawing.Point(184, 88)
      Me.txtResultsY.Name = "txtResultsY"
      Me.txtResultsY.Size = New System.Drawing.Size(136, 26)
      Me.txtResultsY.TabIndex = 4
      '
      'txtResultsX
      '
      Me.txtResultsX.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.txtResultsX.Location = New System.Drawing.Point(184, 48)
      Me.txtResultsX.Name = "txtResultsX"
      Me.txtResultsX.Size = New System.Drawing.Size(136, 26)
      Me.txtResultsX.TabIndex = 3
      '
      'label3
      '
      Me.label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.label3.Location = New System.Drawing.Point(32, 128)
      Me.label3.Name = "label3"
      Me.label3.Size = New System.Drawing.Size(136, 24)
      Me.label3.TabIndex = 2
      Me.label3.Text = "Cycle Time (ms):"
      '
      'label2
      '
      Me.label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.label2.Location = New System.Drawing.Point(136, 88)
      Me.label2.Name = "label2"
      Me.label2.Size = New System.Drawing.Size(24, 24)
      Me.label2.TabIndex = 1
      Me.label2.Text = "Y:"
      '
      'label1
      '
      Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.label1.Location = New System.Drawing.Point(40, 48)
      Me.label1.Name = "label1"
      Me.label1.Size = New System.Drawing.Size(120, 32)
      Me.label1.TabIndex = 0
      Me.label1.Text = "Largest Blob X:"
      '
      'AcqFifoConnectCheckBox
      '
      Me.AcqFifoConnectCheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.AcqFifoConnectCheckBox.Location = New System.Drawing.Point(40, 85)
      Me.AcqFifoConnectCheckBox.Name = "AcqFifoConnectCheckBox"
      Me.AcqFifoConnectCheckBox.Size = New System.Drawing.Size(280, 24)
      Me.AcqFifoConnectCheckBox.TabIndex = 6
      Me.AcqFifoConnectCheckBox.Text = "Connect Master Acq Fifo Control"
      '
      'DisplayUpdateCheckBox
      '
      Me.DisplayUpdateCheckBox.Checked = True
      Me.DisplayUpdateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
      Me.DisplayUpdateCheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.DisplayUpdateCheckBox.Location = New System.Drawing.Point(40, 37)
      Me.DisplayUpdateCheckBox.Name = "DisplayUpdateCheckBox"
      Me.DisplayUpdateCheckBox.Size = New System.Drawing.Size(184, 24)
      Me.DisplayUpdateCheckBox.TabIndex = 5
      Me.DisplayUpdateCheckBox.Text = "Update Display"
      '
      'AcqFifoTab
      '
      Me.AcqFifoTab.Controls.Add(Me.CogAcqFifoEditV21)
      Me.AcqFifoTab.Location = New System.Drawing.Point(4, 34)
      Me.AcqFifoTab.Name = "AcqFifoTab"
      Me.AcqFifoTab.Size = New System.Drawing.Size(912, 442)
      Me.AcqFifoTab.TabIndex = 1
      Me.AcqFifoTab.Text = "Master AcqFifo"
      '
      'TextBox1
      '
      Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.TextBox1.Location = New System.Drawing.Point(8, 496)
      Me.TextBox1.Multiline = True
      Me.TextBox1.Name = "TextBox1"
      Me.TextBox1.Size = New System.Drawing.Size(840, 56)
      Me.TextBox1.TabIndex = 1
      Me.TextBox1.Text = "Sample description: shows how to overlap synchronous simultaneous acquisition wit" & _
          "h processing." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Sample usage: click Start to begin overlapped synchronous simulta" & _
          "neous acquisition and processing."
      '
      'CogAcqFifoEditV21
      '
      Me.CogAcqFifoEditV21.Location = New System.Drawing.Point(0, 0)
      Me.CogAcqFifoEditV21.MinimumSize = New System.Drawing.Size(489, 0)
      Me.CogAcqFifoEditV21.Name = "CogAcqFifoEditV21"
      Me.CogAcqFifoEditV21.Size = New System.Drawing.Size(909, 445)
      Me.CogAcqFifoEditV21.SuspendElectricRuns = False
      Me.CogAcqFifoEditV21.TabIndex = 0
      '
      'Form1
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(936, 558)
      Me.Controls.Add(Me.TextBox1)
      Me.Controls.Add(Me.tabAll)
      Me.Name = "Form1"
      Me.Text = "Form1"
      Me.tabAll.ResumeLayout(False)
      Me.ControlTab.ResumeLayout(False)
      CType(Me.ctrDisplaySlave, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.ctrDisplayMaster, System.ComponentModel.ISupportInitialize).EndInit()
      Me.group1box.ResumeLayout(False)
      Me.group1box.PerformLayout()
      Me.AcqFifoTab.ResumeLayout(False)
      CType(Me.CogAcqFifoEditV21, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)
      Me.PerformLayout()

    End Sub

#End Region
#Region " Axilliary subs"
    Private Function AcqCanStart() As Boolean

      ' Check trigger model to see if it's okay to call acq fifo's StartAcquire method
      If TriggerOperator Is Nothing Then
        Return False
      ElseIf TriggerOperator.TriggerModel = CogAcqTriggerModelConstants.Auto Then
        Return False
      ElseIf TriggerOperator.TriggerModel = CogAcqTriggerModelConstants.Slave Then
        Return False
      Else
        Return True
      End If
    End Function
    Private Function AcqSimulSupported(ByVal FrameGrabber As Cognex.VisionPro.ICogFrameGrabber, _
                                       ByVal strVidFrmt As String) As Boolean
      Try
        Dim objAcqFifo As Cognex.VisionPro.ICogAcqFifo
        objAcqFifo = FrameGrabber.CreateAcqFifo(strVidFrmt, CogAcqFifoPixelFormatConstants.Format8Grey, 0, True)
        If objAcqFifo.OwnedSimultaneousParams Is Nothing Then
          AcqSimulSupported = False
        Else
          AcqSimulSupported = True
        End If
      Catch ex As CogException
        AcqSimulSupported = False
      End Try
    End Function

#End Region
#Region " Initialization"
    Private Sub InitializeAcquisition()
      AcqFifoToolMaster = New CogAcqFifoTool
      ' Find a frame grabber which supports simultaneous acquisition
      Dim mFrameGrabbers As CogFrameGrabbers
      mFrameGrabbers = New CogFrameGrabbers
      Dim blnGotOne As Boolean
      blnGotOne = False
      For Each mFrameGrabber In mFrameGrabbers
        If AcqSimulSupported(mFrameGrabber, defaultVideoFrmt) Then
          blnGotOne = True
          Exit For
        End If
      Next mFrameGrabber

      If Not blnGotOne Then
        Throw New CogAcqNoFrameGrabberException("Could not find a simultaneous frame grabber!")
      End If


      ' Check if the tool was able to create a default acqfifo.
      If AcqFifoToolMaster.[Operator] Is Nothing Then
        Throw New CogAcqNoFrameGrabberException("A board might be missing or not be functioning properly.")
      End If
      ' Create the master acq fifo using the previously located simultaneous frame
      ' grabber and connect the tool to it.

      AcqFifoToolMaster.[Operator] = mFrameGrabber.CreateAcqFifo(defaultVideoFrmt, CogAcqFifoPixelFormatConstants.Format8Grey, 0, True)

      ' Associate the simultaneous acq object with the master acq fifo
      m_AcqSimul = AcqFifoToolMaster.[Operator].OwnedSimultaneousParams
      m_MastAcqFifo = AcqFifoToolMaster.[Operator]

      ' Create the slave acq fifo
      m_AcqFifoSlave = m_AcqSimul.CreateSlaveAcqFifo(True)

      ' Set acq fifo trigger model. Note that this
      ' may also be set via the AcqFifo edit control.
      TriggerOperator = AcqFifoToolMaster.[Operator].OwnedTriggerParams
      TriggerOperator.TriggerEnabled = False
      TriggerOperator.TriggerModel = CogAcqTriggerModelConstants.Manual
      TriggerOperator.TriggerEnabled = True

      ' Connect acq fifo edit control with tool
      AcqFifoConnectCheckBox.Checked = True



      BlobTool = New CogBlobTool

      ' NOTE: We use the cogBlobSegmentationModeHardFixedThreshold mode and
      ' set ConnectivityMinPixels to 1000 because we want the blob image processing
      ' to run faster than the time it takes to acquire an image. Otherwise, the program
      ' may not respond user input quickly enough or the screen may not update properly.
      ' Image processing time varies depending on the CPU speed. A faster CPU processes
      ' the image faster.

      BlobTool.RunParams.SegmentationParams.Mode = _
       CogBlobSegmentationModeConstants.HardFixedThreshold
      BlobTool.RunParams.ConnectivityMinPixels = 1000

      ' Limit the search region so that the processing can be done quickly.
      ' This region will be displayed on the CogDisplay later.

      Rect = New CogRectangle
      Rect.SetXYWidthHeight(150, 100, 350, 300)
      BlobTool.Region = Rect

      StopWatch = New CogStopwatch

      ' Set acq fifo trigger model. Note that this
      ' may also be set via the AcqFifo edit control.

      TriggerOperator = AcqFifoToolMaster.[Operator].OwnedTriggerParams '
      TriggerOperator.TriggerEnabled = False '
      TriggerOperator.TriggerModel = CogAcqTriggerModelConstants.Manual '
      TriggerOperator.TriggerEnabled = True '

      '/ Connect acqfifo edit control with actual tool
      CogAcqFifoEditV21.Subject = AcqFifoToolMaster '

      ' connect change event handlers
      AddHandler m_MastAcqFifo.Changed, _
                          New CogChangedEventHandler(AddressOf MastAcqFifo_Changed)
      AddHandler AcqFifoToolMaster.Changed, _
                          New CogChangedEventHandler(AddressOf AcqFifoToolMaster_Changed)
    End Sub

#End Region
#Region " Image processing"
    Private Sub AnalyzeImage(ByRef dblArea As Double, ByRef dblX As Double, _
                         ByRef dblY As Double, ByVal objImage As CogImage8Grey)
      Dim dblBlobAreaMax As Double
      Dim dblBlobX As Double
      Dim dblBlobY As Double
      dblBlobAreaMax = -1
      dblBlobX = 0
      dblBlobY = 0

      ' Set up blob tool
      BlobTool.InputImage = objImage

      ' Run the blob tool
      BlobTool.Run()

      ' Extract biggest blob results if available
      If Not BlobTool.RunStatus.Result = CogToolResultConstants.Accept Then
        ' nothing to process!
      ElseIf BlobTool.Results.GetBlobs(True).Count < 1 Then
        ' still nothing to process!
      Else
        Dim dblBlobArea As Double
        Dim objBlobResult As CogBlobResult

        For Each objBlobResult In BlobTool.Results.GetBlobs(True)
          dblBlobArea = objBlobResult.Area
          If (dblBlobArea > dblBlobAreaMax) Then
            dblBlobAreaMax = dblBlobArea
            dblBlobX = objBlobResult.CenterOfMassX
            dblBlobY = objBlobResult.CenterOfMassY
          End If
        Next objBlobResult
      End If

      dblArea = dblBlobAreaMax
      dblX = dblBlobX
      dblY = dblBlobY

    End Sub

    Private Sub AnalyzeImages(ByVal objMasterImage As CogImage8Grey, _
                              ByVal objSlaveImage As CogImage8Grey)
      Dim dblMA As Double, dblMX As Double, dblMY As Double
      Dim dblSA As Double, dblSX As Double, dblSY As Double
      Dim dblA As Double, dblX As Double, dblY As Double
      Dim blnGotit As Boolean, blnMaster As Boolean

      ' Analyze both images
      AnalyzeImage(dblMA, dblMX, dblMY, objMasterImage)
      AnalyzeImage(dblSA, dblSX, dblSY, objSlaveImage)

      ' Find the bigger of two (or fewer) blobs
      If (dblMA > dblSA) Then
        dblA = dblMA
        dblX = dblMX
        dblY = dblMY
        blnMaster = True
      Else
        dblA = dblSA
        dblX = dblSX
        dblY = dblSY
        blnMaster = False
      End If
      If dblA = -1 Then
        blnGotit = False
      Else
        blnGotit = True
      End If

      ' Now do something with processing results
      If DisplayUpdateCheckBox.Checked Then
        ctrDisplayMaster.Image = objMasterImage
        ctrDisplaySlave.Image = objSlaveImage
        If blnGotit Then
          Dim objMarker As New CogPointMarker
          objMarker.Color = CogColorConstants.Red
          objMarker.X = dblX
          objMarker.Y = dblY
          ctrDisplayMaster.StaticGraphics.Clear()
          ctrDisplaySlave.StaticGraphics.Clear()
          If blnMaster Then
            ctrDisplayMaster.StaticGraphics.Add(objMarker, "Master")
          Else
            ctrDisplaySlave.StaticGraphics.Add(objMarker, "Slave")
          End If
        End If
      End If

      Dim strX As String, strY As String
      If Not blnGotit Then
        strX = "N/A"
        strY = "N/A"
      ElseIf blnMaster Then
        strX = "M: " & dblX.ToString("0000")
        strY = "M: " & dblY.ToString("0000")
      Else
        strX = "S: " & dblX.ToString("0000")
        strY = "S: " & dblY.ToString("0000")
      End If
      txtResultsX.Text = strX
      txtResultsY.Text = strY

      ' Update cycle time, then reset clock
      txtResultsT.Text = StopWatch.Milliseconds.ToString("000.0")
      StopWatch.Reset()
      StopWatch.Start()

    End Sub

#End Region
#Region " Complete event handler"
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

      If Processing = False Then Return

      Dim numReadyVal, numPendingVal As Integer
	  Dim info As New CogAcqInfo
      Dim busyVal As Boolean
      Dim objImageMaster As CogImage8Grey
      objImageMaster = Nothing
      Dim objImageSlave As CogImage8Grey
      objImageSlave = Nothing
      Try

        AcqFifoToolMaster.[Operator].GetFifoState(numPendingVal, numReadyVal, busyVal)
        If numReadyVal > 0 Then
          objImageMaster = _
              CType(m_MastAcqFifo.CompleteAcquireEx(info), _
                      CogImage8Grey)
        Else
          '          Throw New CogAcqAbnormalException("Master ready count is not greater than 0.")
        End If
        numAcqs += 1
        totalAcqs += 1
        ' We need to run the garbage collector on occasion to cleanup
        ' images that are no longer being used.
        If numAcqs > 4 Then
          GC.Collect()
          numAcqs = 0
        End If

        ' Fetch the slave image
        If Not m_AcqFifoSlave Is Nothing Then
          m_AcqFifoSlave.GetFifoState(numPendingVal, numReadyVal, busyVal)
          If numReadyVal > 0 Then
            objImageSlave = CType(m_AcqFifoSlave.CompleteAcquireEx(info), _
                    CogImage8Grey)

          Else
            '            Throw New CogAcqAbnormalException("Slave ready count is not greater than 0.")
          End If

        End If

        ' Issue another acquisition request if we are in manual trigger mode.
        If Processing Then
          If AcqCanStart() Then
            If (Not objImageMaster Is Nothing) And (Not objImageSlave Is Nothing) Then
              m_MastAcqFifo.StartAcquire() ' ' request another acquisition
            End If
          End If
        End If
        '/ Do some processing while acquiring next image
        If Not objImageMaster Is Nothing And Not objImageSlave Is Nothing Then
          AnalyzeImages(objImageMaster, objImageSlave)
        End If


      Catch ex As CogException
        MessageBox.Show("The following error has occured:" & vbCrLf & ex.Message)
      End Try
    End Sub
#End Region
#Region " Change event handlers"
    ' Sink the master strobe enable with the slave. This allows the user to acquire
    ' images for both master and slave.
    Private Sub MastAcqFifo_Changed(ByVal sender As Object, ByVal e As CogChangedEventArgs)
      If m_AcqFifoSlave Is Nothing Or m_MastAcqFifo Is Nothing Then
        Exit Sub
      End If

      If (e.StateFlags And CogAcqFifoStateFlags.SfStrobeEnabled) <> 0 Then
        Dim MasterStrobe As Cognex.VisionPro.ICogAcqStrobe
        Dim SlaveStrobe As Cognex.VisionPro.ICogAcqStrobe
        MasterStrobe = m_MastAcqFifo.OwnedStrobeParams
        SlaveStrobe = m_AcqFifoSlave.OwnedStrobeParams
        SlaveStrobe.StrobeEnabled = MasterStrobe.StrobeEnabled
      End If

    End Sub
    Private Sub AcqFifoToolMaster_Changed(ByVal sender As Object, ByVal e As CogChangedEventArgs)

      If (e.StateFlags And CogAcqFifoTool.SfOperator) = 0 Then
        Exit Sub
      End If
      Try
        ' The board may not support trigger or simultaneous acquisition.


        If AcqFifoToolMaster.[Operator] Is Nothing Then
          m_AcqFifoSlave = Nothing
          m_AcqSimul = Nothing
          TriggerOperator = Nothing
          Exit Sub
        End If
        TriggerOperator = AcqFifoToolMaster.[Operator].OwnedTriggerParams
        m_AcqSimul = AcqFifoToolMaster.[Operator].OwnedSimultaneousParams
        m_MastAcqFifo = AcqFifoToolMaster.[Operator]


        ' If the board does not support simultaneous acquisition
        ' set mAcqFifoSlave to Nothing
        If m_AcqSimul Is Nothing Then
          m_AcqFifoSlave = Nothing
          Exit Sub
        End If

        ' Underlying Operator (may have) changed, so
        ' (re-) connect associated objects, and
        ' create new slave acq fifo ...
        m_AcqFifoSlave = m_AcqSimul.CreateSlaveAcqFifo(True)


        ' Need to call prepare after creating a slave acqfifo.
        If Not m_AcqFifoSlave Is Nothing Then

          m_AcqFifoSlave.Prepare()
        End If

        ' rehook complete/changed event handlers 

        AddHandler m_MastAcqFifo.Complete, _
                            New CogCompleteEventHandler(AddressOf Operator_Complete)

        AddHandler m_MastAcqFifo.Changed, _
                            New CogChangedEventHandler(AddressOf MastAcqFifo_Changed)

      Catch

      End Try


    End Sub
#End Region
#Region " Form controls handlers"
    Private Sub StartButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StartButton.Click
      If Processing Then   ' we should stop
        ' Connect the complete event handler
        StopAcquire = True
        TriggerOperator.TriggerEnabled = False '
        ' Flush all outstanding acquisition requests and stop.
        m_MastAcqFifo.Flush()
        m_AcqFifoSlave.Flush()
        Processing = False
        RemoveHandler m_MastAcqFifo.Complete, _
                            New CogCompleteEventHandler(AddressOf Operator_Complete)

        StartButton.Text = "Start"
        DisplayUpdateCheckBox.Enabled = True
        AcqFifoConnectCheckBox.Enabled = True
        AcqFifoTab.Enabled = True
      Else '  we should start

        Processing = True
        StopAcquire = False
        'Flush all outstanding acquisitions since they are not part of new acquisitions.
        m_MastAcqFifo.Flush()
        m_AcqFifoSlave.Flush()

        StopWatch.Reset()
        StopWatch.Start()
        StartButton.Text = "Stop"
        DisplayUpdateCheckBox.Enabled = False
        AcqFifoConnectCheckBox.Enabled = False
        AcqFifoTab.Enabled = False
        ' Connect the complete event handler
        AddHandler m_MastAcqFifo.Complete, _
                            New CogCompleteEventHandler(AddressOf Operator_Complete)
        TriggerOperator = AcqFifoToolMaster.[Operator].OwnedTriggerParams '
        TriggerOperator.TriggerEnabled = True '

        If AcqCanStart() Then

          ' This is sort of subtle. For manual
          ' triggering, prime the acquisition engine
          ' with two (or more, up to 32) start requests
          ' to ensure optimal throughput

          m_MastAcqFifo.StartAcquire()
          m_MastAcqFifo.StartAcquire()
        End If
      End If
    End Sub

    Private Sub AcqFifoConnectCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AcqFifoConnectCheckBox.CheckedChanged
      ' connect / disconnect acqfifo control
      If AcqFifoConnectCheckBox.Checked Then
        CogAcqFifoEditV21.Subject = AcqFifoToolMaster
      Else
        CogAcqFifoEditV21.Subject = Nothing
      End If
    End Sub

#End Region

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


    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
      Processing = False
      StopAcquire = True
      If (Not m_MastAcqFifo Is Nothing) Then
        m_MastAcqFifo.Flush()
        m_AcqFifoSlave.Flush()
      End If
      Dim counter As Integer
      counter = 0
      While (counter < 15)
        Application.DoEvents()
        Thread.Sleep(1)
        counter = counter + 1
      End While
    End Sub
  End Class
End Namespace