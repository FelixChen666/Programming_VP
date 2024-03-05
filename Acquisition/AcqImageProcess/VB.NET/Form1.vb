Option Strict On
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


' The AcqImageProcess sample program is intended to demonstrate how one
' might use VisionPro(R) to achieve a high level of application throughput
' by overlapping image acquisition and image processing. This is
' accomplished within the context of a single threaded VB.NET.

' The general approach taken here is to use the acquisition complete
' event to initiate image processing as soon as an image is
' available. In addition, the same event handler can queue up a manual
' trigger mode acquisition request prior to actually processing the
' current image so that acquisition of the next image can overlap
' processing of the current image.

' In addition to overlapping acquisition and image processing, this
' sample program demonstrates the impact on throughput of:
'   * updating a display control for every acquisition,
' This is accomplished by providing GUI controls that permit the
' toggling of these characteristics (i.e. do or do not update the
' display at run time).

' For the sake of this sample program, the image-processing task has
' been defined to be the location of the largest blob in the image using
' the blob tool. For every image acquired the blob tool is run, the
' largest blob is identified, and the center of mass of that blob is
' reported back to the GUI as well as displayed graphically on the
' image. The selection of this image-processing task is arbitrary - it
' could be any image-processing task.


' TIMING ANALYSIS

' As an aid to evaluating the impact of various implementation choices
' on throughput, the application computes and reports a cycle time in
' milliseconds for each image that is acquired and processed. This
' reported time corresponds to the interval from the delivery to the GUI
' of the previous results to (just before) the delivery to the GUI of
' the current results.

' Please note that it is possible to observe a reported cycle time of
' less than normal acquisition time (33 ms for RS170 cameras). This can
' happen when more than one completed acquisition becomes available for
' consumption at any given instant. In this case, the cycle time does
' not incorporate the usual amount of acquisition time.


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


' OF SPECIAL INTEREST

' A few techniques used in this sample program warrant some extra
' attention.

' 1. When using manual acquisition triggering, you should "prime" the
'    acquisition pipeline with two (or more, up to 32) acquisition start
'    requests. This will help prevent the acquisition engine from
'    becoming stalled for want of an acquisition request.

' SUPPORT FOR DIFFERENT ACQUISITION TRIGGER MODELS

' This sample program supports the use of manual, semi, and automatic
' acquisition trigger models. This is done by specifically checking the
' acq fifo's current trigger model and only invoking StartAcquire when
' it is allowed (i.e. when the trigger model is either manual or semi,
' but not when it is auto or slave). While this program explicitly
' demonstrates how to programmatically set the trigger model to manual,
' you can try out other trigger models by modifying the appropriate
' setting in the acq fifo edit control.

'*/

Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Collections
Imports System.Windows.Forms
Imports System.Data
Imports System.Threading
' Cognex namespace
Imports Cognex.VisionPro
' Needed for Blob tool
Imports Cognex.VisionPro.Blob
' Needed for CogException
Imports Cognex.VisionPro.Exceptions
Namespace AcqImageProcess
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
    Friend WithEvents AcqProcessingTabControl As System.Windows.Forms.TabControl
    Friend WithEvents ControlTab As System.Windows.Forms.TabPage
    Friend WithEvents cogDisplay1 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents StartButton As System.Windows.Forms.Button
    Friend WithEvents group1box As System.Windows.Forms.GroupBox
    Friend WithEvents TotalAcqText As System.Windows.Forms.TextBox
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents CycleTimeText As System.Windows.Forms.TextBox
    Friend WithEvents BlobYText As System.Windows.Forms.TextBox
    Friend WithEvents BlobXText As System.Windows.Forms.TextBox
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents AcqFifoConnectCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents DisplayUpdateCheckBox As System.Windows.Forms.CheckBox
        Friend WithEvents AcqFifoTab As System.Windows.Forms.TabPage
        Friend WithEvents CogAcqFifoEditV21 As Cognex.VisionPro.CogAcqFifoEditV2
        Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
            Me.AcqProcessingTabControl = New System.Windows.Forms.TabControl()
            Me.ControlTab = New System.Windows.Forms.TabPage()
            Me.cogDisplay1 = New Cognex.VisionPro.Display.CogDisplay()
            Me.StartButton = New System.Windows.Forms.Button()
            Me.group1box = New System.Windows.Forms.GroupBox()
            Me.TotalAcqText = New System.Windows.Forms.TextBox()
            Me.label4 = New System.Windows.Forms.Label()
            Me.CycleTimeText = New System.Windows.Forms.TextBox()
            Me.BlobYText = New System.Windows.Forms.TextBox()
            Me.BlobXText = New System.Windows.Forms.TextBox()
            Me.label3 = New System.Windows.Forms.Label()
            Me.label2 = New System.Windows.Forms.Label()
            Me.label1 = New System.Windows.Forms.Label()
            Me.AcqFifoConnectCheckBox = New System.Windows.Forms.CheckBox()
            Me.DisplayUpdateCheckBox = New System.Windows.Forms.CheckBox()
            Me.AcqFifoTab = New System.Windows.Forms.TabPage()
            Me.CogAcqFifoEditV21 = New Cognex.VisionPro.CogAcqFifoEditV2()
            Me.txtDescription = New System.Windows.Forms.TextBox()
            Me.AcqProcessingTabControl.SuspendLayout()
            Me.ControlTab.SuspendLayout()
            CType(Me.cogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.group1box.SuspendLayout()
            Me.AcqFifoTab.SuspendLayout()
            CType(Me.CogAcqFifoEditV21, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'AcqProcessingTabControl
            '
            Me.AcqProcessingTabControl.Controls.Add(Me.ControlTab)
            Me.AcqProcessingTabControl.Controls.Add(Me.AcqFifoTab)
            Me.AcqProcessingTabControl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.AcqProcessingTabControl.ItemSize = New System.Drawing.Size(120, 30)
            Me.AcqProcessingTabControl.Location = New System.Drawing.Point(0, 0)
            Me.AcqProcessingTabControl.Name = "AcqProcessingTabControl"
            Me.AcqProcessingTabControl.SelectedIndex = 0
            Me.AcqProcessingTabControl.Size = New System.Drawing.Size(856, 510)
            Me.AcqProcessingTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
            Me.AcqProcessingTabControl.TabIndex = 3
            '
            'ControlTab
            '
            Me.ControlTab.Controls.Add(Me.cogDisplay1)
            Me.ControlTab.Controls.Add(Me.StartButton)
            Me.ControlTab.Controls.Add(Me.group1box)
            Me.ControlTab.Controls.Add(Me.AcqFifoConnectCheckBox)
            Me.ControlTab.Controls.Add(Me.DisplayUpdateCheckBox)
            Me.ControlTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.ControlTab.Location = New System.Drawing.Point(4, 34)
            Me.ControlTab.Name = "ControlTab"
            Me.ControlTab.Size = New System.Drawing.Size(848, 472)
            Me.ControlTab.TabIndex = 0
            Me.ControlTab.Text = "Control"
            '
            'cogDisplay1
            '
            Me.cogDisplay1.ColorMapLowerClipColor = System.Drawing.Color.Black
            Me.cogDisplay1.ColorMapLowerRoiLimit = 0.0R
            Me.cogDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None
            Me.cogDisplay1.ColorMapUpperClipColor = System.Drawing.Color.Black
            Me.cogDisplay1.ColorMapUpperRoiLimit = 1.0R
            Me.cogDisplay1.Location = New System.Drawing.Point(424, 32)
            Me.cogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
            Me.cogDisplay1.MouseWheelSensitivity = 1.0R
            Me.cogDisplay1.Name = "cogDisplay1"
            Me.cogDisplay1.OcxState = CType(resources.GetObject("cogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
            Me.cogDisplay1.Size = New System.Drawing.Size(368, 360)
            Me.cogDisplay1.TabIndex = 4
            '
            'StartButton
            '
            Me.StartButton.Location = New System.Drawing.Point(128, 352)
            Me.StartButton.Name = "StartButton"
            Me.StartButton.Size = New System.Drawing.Size(120, 40)
            Me.StartButton.TabIndex = 3
            Me.StartButton.Text = "Start"
            '
            'group1box
            '
            Me.group1box.Controls.Add(Me.TotalAcqText)
            Me.group1box.Controls.Add(Me.label4)
            Me.group1box.Controls.Add(Me.CycleTimeText)
            Me.group1box.Controls.Add(Me.BlobYText)
            Me.group1box.Controls.Add(Me.BlobXText)
            Me.group1box.Controls.Add(Me.label3)
            Me.group1box.Controls.Add(Me.label2)
            Me.group1box.Controls.Add(Me.label1)
            Me.group1box.Location = New System.Drawing.Point(32, 120)
            Me.group1box.Name = "group1box"
            Me.group1box.Size = New System.Drawing.Size(344, 208)
            Me.group1box.TabIndex = 2
            Me.group1box.TabStop = False
            Me.group1box.Text = "Results"
            '
            'TotalAcqText
            '
            Me.TotalAcqText.Location = New System.Drawing.Point(184, 160)
            Me.TotalAcqText.Name = "TotalAcqText"
            Me.TotalAcqText.Size = New System.Drawing.Size(88, 26)
            Me.TotalAcqText.TabIndex = 7
            '
            'label4
            '
            Me.label4.Location = New System.Drawing.Point(40, 160)
            Me.label4.Name = "label4"
            Me.label4.Size = New System.Drawing.Size(120, 24)
            Me.label4.TabIndex = 6
            Me.label4.Text = "Total Acquires:"
            '
            'CycleTimeText
            '
            Me.CycleTimeText.Location = New System.Drawing.Point(184, 120)
            Me.CycleTimeText.Name = "CycleTimeText"
            Me.CycleTimeText.Size = New System.Drawing.Size(88, 26)
            Me.CycleTimeText.TabIndex = 5
            '
            'BlobYText
            '
            Me.BlobYText.Location = New System.Drawing.Point(184, 80)
            Me.BlobYText.Name = "BlobYText"
            Me.BlobYText.Size = New System.Drawing.Size(88, 26)
            Me.BlobYText.TabIndex = 4
            '
            'BlobXText
            '
            Me.BlobXText.Location = New System.Drawing.Point(184, 48)
            Me.BlobXText.Name = "BlobXText"
            Me.BlobXText.Size = New System.Drawing.Size(88, 26)
            Me.BlobXText.TabIndex = 3
            '
            'label3
            '
            Me.label3.Location = New System.Drawing.Point(32, 120)
            Me.label3.Name = "label3"
            Me.label3.Size = New System.Drawing.Size(136, 24)
            Me.label3.TabIndex = 2
            Me.label3.Text = "Cycle Time (ms):"
            '
            'label2
            '
            Me.label2.Location = New System.Drawing.Point(136, 80)
            Me.label2.Name = "label2"
            Me.label2.Size = New System.Drawing.Size(24, 24)
            Me.label2.TabIndex = 1
            Me.label2.Text = "Y:"
            '
            'label1
            '
            Me.label1.Location = New System.Drawing.Point(40, 48)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(120, 32)
            Me.label1.TabIndex = 0
            Me.label1.Text = "Largest Blob X:"
            '
            'AcqFifoConnectCheckBox
            '
            Me.AcqFifoConnectCheckBox.Location = New System.Drawing.Point(32, 72)
            Me.AcqFifoConnectCheckBox.Name = "AcqFifoConnectCheckBox"
            Me.AcqFifoConnectCheckBox.Size = New System.Drawing.Size(240, 24)
            Me.AcqFifoConnectCheckBox.TabIndex = 1
            Me.AcqFifoConnectCheckBox.Text = "Connect Acq Fifo Control"
            '
            'DisplayUpdateCheckBox
            '
            Me.DisplayUpdateCheckBox.Location = New System.Drawing.Point(32, 24)
            Me.DisplayUpdateCheckBox.Name = "DisplayUpdateCheckBox"
            Me.DisplayUpdateCheckBox.Size = New System.Drawing.Size(184, 24)
            Me.DisplayUpdateCheckBox.TabIndex = 0
            Me.DisplayUpdateCheckBox.Text = "Update Display"
            '
            'AcqFifoTab
            '
            Me.AcqFifoTab.Controls.Add(Me.CogAcqFifoEditV21)
            Me.AcqFifoTab.Location = New System.Drawing.Point(4, 34)
            Me.AcqFifoTab.Name = "AcqFifoTab"
            Me.AcqFifoTab.Size = New System.Drawing.Size(848, 472)
            Me.AcqFifoTab.TabIndex = 1
            Me.AcqFifoTab.Text = "AcqFifo"
            Me.AcqFifoTab.Visible = False
            '
            'CogAcqFifoEditV21
            '
            Me.CogAcqFifoEditV21.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CogAcqFifoEditV21.Location = New System.Drawing.Point(0, 0)
            Me.CogAcqFifoEditV21.MinimumSize = New System.Drawing.Size(489, 0)
            Me.CogAcqFifoEditV21.Name = "CogAcqFifoEditV21"
            Me.CogAcqFifoEditV21.Size = New System.Drawing.Size(848, 472)
            Me.CogAcqFifoEditV21.SuspendElectricRuns = False
            Me.CogAcqFifoEditV21.TabIndex = 0
            '
            'txtDescription
            '
            Me.txtDescription.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.txtDescription.Location = New System.Drawing.Point(0, 510)
            Me.txtDescription.Multiline = True
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.ReadOnly = True
            Me.txtDescription.Size = New System.Drawing.Size(856, 40)
            Me.txtDescription.TabIndex = 4
            Me.txtDescription.Text = "This sample demonstrates how to improve throughput by overlapping acquisition and" & _
        " image processing. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " Click the Start button to begin acquiring and processing i" & _
        "mages."
            '
            'Form1
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(856, 550)
            Me.Controls.Add(Me.AcqProcessingTabControl)
            Me.Controls.Add(Me.txtDescription)
            Me.Name = "Form1"
            Me.Text = "VisionPro Acquisition and Processing Sample"
            Me.AcqProcessingTabControl.ResumeLayout(False)
            Me.ControlTab.ResumeLayout(False)
            CType(Me.cogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.group1box.ResumeLayout(False)
            Me.group1box.PerformLayout()
            Me.AcqFifoTab.ResumeLayout(False)
            CType(Me.CogAcqFifoEditV21, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region
#Region " Private vars"
    Private AcqFifoTool As CogAcqFifoTool
    Private BlobTool As CogBlobTool
    Private numAcqs As Integer = 0
    Private StopWatch As CogStopwatch
    Private Processing As Boolean = False
    Private StopAcquire As Boolean
    Private TriggerOperator As Cognex.VisionPro.ICogAcqTrigger
    Private Rect As CogRectangle
    Private totalAcqs As Integer = 0
#End Region
#Region " Initialization"
    ' creates a new CogAcqFifoTool,CogBlobTool. Prepares acquisition and image processing,hooks acqusition 
    ' complete event
    Private Sub InitializeAcquisition()
      Try

        AcqFifoTool = New CogAcqFifoTool

        ' Check if the tool was able to create a default acqfifo.
        If AcqFifoTool.[Operator] Is Nothing Then
          Throw New CogAcqNoFrameGrabberException("A board might be missing or not be functioning properly.")

        End If
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

        TriggerOperator = AcqFifoTool.[Operator].OwnedTriggerParams '
        TriggerOperator.TriggerEnabled = False '
        TriggerOperator.TriggerModel = CogAcqTriggerModelConstants.Manual '
        TriggerOperator.TriggerEnabled = True '

        '/ Connect acqfifo edit control with actual tool
        CogAcqFifoEditV21.Subject = AcqFifoTool '

      Catch ex As Exception
        MessageBox.Show(ex.Message)
        StartButton.Enabled = False
        DisplayUpdateCheckBox.Enabled = False
        AcqFifoConnectCheckBox.Enabled = False
        AcqProcessingTabControl.Controls.Remove(AcqFifoTab)

      End Try
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

      Dim numReadyVal, numPendingVal As Integer
      Dim busyVal As Boolean
	  Dim info As New CogAcqInfo
      Dim CurrentImage As CogImage8Grey
      If StopAcquire Then
        Exit Sub
      End If
      Try

        AcqFifoTool.[Operator].GetFifoState(numPendingVal, numReadyVal, busyVal)
        If numReadyVal > 0 Then
          CurrentImage = _
              CType(AcqFifoTool.[Operator].CompleteAcquireEx(info), _
                      CogImage8Grey)
        Else
          Throw New CogAcqAbnormalException("Ready count is not greater than 0.")
        End If
        numAcqs += 1
        totalAcqs += 1
        TotalAcqText.Text = totalAcqs.ToString()
        ' We need to run the garbage collector on occasion to cleanup
        ' images that are no longer being used.
        If numAcqs > 4 Then
          GC.Collect()
          numAcqs = 0
        End If
        ' Issue another acquisition request if we are in manual trigger mode.
        If AcqCanStart() Then
          AcqFifoTool.[Operator].StartAcquire() ' ' request another acquisition
        End If
        '/ Do some processing while acquiring next image
        If Not CurrentImage Is Nothing Then
          AnalyzeImage(CurrentImage)
        End If

      Catch ce As CogException
        MessageBox.Show("The following error has occured:" & vbCrLf & ce.Message)
      End Try
    End Sub

#End Region
#Region " Image processing"
    ' Analyzes image, processes and displays results
    '
    Private Sub AnalyzeImage(ByVal objImage As CogImage8Grey)

      ' Miscellaneous local vars
      Dim Gotit As Boolean
      Dim BlobX As Double
      Dim BlobY As Double
      Dim MaxBlobArea As Double
      Dim BlobArea As Double
      Dim FilteredBlobs As CogBlobResultCollection
      Dim Marker As CogPointMarker

      Gotit = False
      BlobX = 0
      BlobY = 0

      ' Set up blob tool
      BlobTool.InputImage = objImage

      ' Run the blob tool
      BlobTool.Run() '

      ' Extract biggest blob results if available
      If (BlobTool.RunStatus.Result = CogToolResultConstants.Error) Then
        Gotit = False '
      ElseIf (BlobTool.Results.GetBlobs(True).Count < 1) Then
        Gotit = False '
      Else

        MaxBlobArea = -1
        FilteredBlobs = BlobTool.Results.GetBlobs(True)
        Dim ObjBlobResult As CogBlobResult
        For Each ObjBlobResult In FilteredBlobs

          BlobArea = ObjBlobResult.Area
          If BlobArea > MaxBlobArea Then
            Gotit = True
            MaxBlobArea = BlobArea
            BlobX = ObjBlobResult.CenterOfMassX
            BlobY = ObjBlobResult.CenterOfMassY
          End If
        Next
      End If

      ' Now do something with processing results
      If DisplayUpdateCheckBox.Checked Then

        cogDisplay1.DrawingEnabled = False
        cogDisplay1.Image = objImage
        If Gotit Then

          cogDisplay1.StaticGraphics.Clear()
          cogDisplay1.StaticGraphics.Add(Rect, "main")
          Marker = New CogPointMarker
          Marker.Color = CogColorConstants.Red
          Marker.X = BlobX
          Marker.Y = BlobY
          cogDisplay1.StaticGraphics.Add(Marker, "main")
        End If
        cogDisplay1.DrawingEnabled = True
      End If
      StopWatch.Stop()
      If Gotit Then

        BlobXText.Text = BlobX.ToString()
        BlobYText.Text = BlobY.ToString()

      Else

        BlobXText.Text = "N/A"
        BlobYText.Text = "N/A"
      End If
      ' Update cycle time, then reset clock
      CycleTimeText.Text = StopWatch.Milliseconds.ToString()
      StopWatch.Reset()
      StopWatch.Start()
    End Sub

#End Region
#Region " Axilliary subs"
    ' Before starting an acquisition checks that it is possible to start
    '
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

#End Region
#Region " Connect check box and start button handlers"
    ' connects / disconnects acqfifo control
    '
    Private Sub AcqFifoConnectCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AcqFifoConnectCheckBox.CheckedChanged
      If AcqFifoConnectCheckBox.Checked Then
        CogAcqFifoEditV21.Subject = AcqFifoTool
      Else
        CogAcqFifoEditV21.Subject = Nothing
      End If

    End Sub
    ' starts and stops an image acqusition
    '
    Private Sub StartButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StartButton.Click
      ' check if AcqFifoTool Operator Is null; if it is output an error and return
      If AcqFifoTool.Operator Is Nothing Then
        MessageBox.Show("AcqFifo not initialized. Open the AcqFifo tab, select a video format, and initialize acquisition. Then try again.")
        Return
      End If
	  
      If (Processing) Then ' we should stop

        ' Connect the complete event handler
        RemoveHandler AcqFifoTool.[Operator].Complete, _
                            New CogCompleteEventHandler(AddressOf Operator_Complete)
        StopAcquire = True
        ' Flush all outstanding acquisition requests and stop.
        AcqFifoTool.[Operator].Flush()

        Processing = False
        StartButton.Text = "Start"
        DisplayUpdateCheckBox.Enabled = True
        AcqFifoConnectCheckBox.Enabled = True
        AcqProcessingTabControl.TabPages(1).Enabled = True
      Else '  we should start
        TriggerOperator = AcqFifoTool.[Operator].OwnedTriggerParams '

        ' We'll be running greyscale blob on the acquired image, so configure the fifo
        ' to produce greyscale images.  If a color camera is used, this setting will cause
        ' the image to be converted to greyscale when CompleteAcquireEx is called.
        AcqFifoTool.Operator.OutputPixelFormat = CogImagePixelFormatConstants.Grey8

        ' Connect the complete event handler
        AddHandler AcqFifoTool.[Operator].Complete, _
                            New CogCompleteEventHandler(AddressOf Operator_Complete)
        Processing = True
        StopAcquire = False
        'Flush all outstanding acquisitions since they are not part of new acquisitions.
        AcqFifoTool.[Operator].Flush()

        StopWatch.Reset()
        StopWatch.Start()
        StartButton.Text = "Stop"
        DisplayUpdateCheckBox.Enabled = False
        AcqFifoConnectCheckBox.Enabled = False
        AcqProcessingTabControl.TabPages(1).Enabled = False
        If AcqCanStart() Then

          ' This is sort of subtle. For manual
          ' triggering, prime the acquisition engine
          ' with two (or more, up to 32) start requests
          ' to ensure optimal throughput

          AcqFifoTool.[Operator].StartAcquire()
          AcqFifoTool.[Operator].StartAcquire()
        End If
      End If

    End Sub

#End Region

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Try
        InitializeAcquisition()
      Catch ex As CogException
        MessageBox.Show(ex.Message)
        AcqProcessingTabControl.Controls.Remove(AcqFifoTab)
      End Try

    End Sub



    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
      CogAcqFifoEditV21.Dispose()
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
      StopAcquire = True
      If (Not AcqFifoTool.[Operator] Is Nothing) Then
        AcqFifoTool.[Operator].Flush()
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
end namespace
