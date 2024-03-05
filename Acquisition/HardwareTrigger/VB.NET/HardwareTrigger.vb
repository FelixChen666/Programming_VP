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

' This sample demonstrates how to set up hardware trigger mode (a.k.a automatic
' triggering) and to acquire images. A Cognex frame grabber board must be present
' in order to run this sample program. If no Cognex board is present, the program
' displays an error and exits.
'
' In auto-trigger mode, the CogAcqFifoTool automatically acquires an image
' and fires an acquisition completion event when a pulse is received on the
' appropriate input line. Otherwise, no images will be displayed.

' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
'
' Follow the next steps in order to set up hardware trigger mode.
' Step 1) Create a  CogAcqFifoTool (see Form_Load).
' Step 2) Set TriggerEnabled property to False (See Setup).
' Step 3) Select hardware auto trigger mode (see Form_Load).
' Step 4) When a single acquisition is completed, the CogAcqFifo will fire
'         the acquisition completion event. The acquisition completion event handler
'         object   must be hooked to catch this event
'         (see Form_Load).
' Step 5) Set TriggerEnabled property to True and wait for external triggers (See cmdRun_Click).
' Step 6) Display an image when an acquisition is completed (see mCompleteEvent_Complete).

Option Explicit On
Imports System.Threading
' Needed for VisionPro
Imports Cognex.VisionPro
' Needed for VisionPro Exceptions
Imports Cognex.VisionPro.Exceptions
Namespace HardwareTrigger
    Public Class HardwareTrigger
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
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblBoardType As System.Windows.Forms.Label
        Friend WithEvents chkEnbStrobe As System.Windows.Forms.CheckBox
        Friend WithEvents cmdRun As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(HardwareTrigger))
            Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
            Me.txtDescription = New System.Windows.Forms.TextBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.lblBoardType = New System.Windows.Forms.Label
            Me.cmdRun = New System.Windows.Forms.Button
            Me.chkEnbStrobe = New System.Windows.Forms.CheckBox
            CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CogDisplay1
            '
            Me.CogDisplay1.Location = New System.Drawing.Point(288, 8)
            Me.CogDisplay1.Name = "CogDisplay1"
            Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
            Me.CogDisplay1.Size = New System.Drawing.Size(416, 368)
            Me.CogDisplay1.TabIndex = 0
            '
            'txtDescription
            '
            Me.txtDescription.Location = New System.Drawing.Point(8, 384)
            Me.txtDescription.Multiline = True
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Size = New System.Drawing.Size(696, 56)
            Me.txtDescription.TabIndex = 2
            Me.txtDescription.Text = resources.GetString("txtDescription.Text")
            Me.txtDescription.WordWrap = False
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(8, 16)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(80, 23)
            Me.Label1.TabIndex = 3
            Me.Label1.Text = "Board Type"
            '
            'lblBoardType
            '
            Me.lblBoardType.Location = New System.Drawing.Point(8, 48)
            Me.lblBoardType.Name = "lblBoardType"
            Me.lblBoardType.Size = New System.Drawing.Size(224, 40)
            Me.lblBoardType.TabIndex = 4
            Me.lblBoardType.Text = "Unknown"
            '
            'cmdRun
            '
            Me.cmdRun.Location = New System.Drawing.Point(64, 168)
            Me.cmdRun.Name = "cmdRun"
            Me.cmdRun.Size = New System.Drawing.Size(136, 40)
            Me.cmdRun.TabIndex = 15
            Me.cmdRun.Text = "Run"
            '
            'chkEnbStrobe
            '
            Me.chkEnbStrobe.AutoSize = True
            Me.chkEnbStrobe.Location = New System.Drawing.Point(11, 82)
            Me.chkEnbStrobe.Name = "chkEnbStrobe"
            Me.chkEnbStrobe.Size = New System.Drawing.Size(93, 17)
            Me.chkEnbStrobe.TabIndex = 16
            Me.chkEnbStrobe.Text = "Enable Strobe"
            Me.chkEnbStrobe.UseVisualStyleBackColor = True
            '
            'HardwareTrigger
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(712, 446)
            Me.Controls.Add(Me.chkEnbStrobe)
            Me.Controls.Add(Me.cmdRun)
            Me.Controls.Add(Me.lblBoardType)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.txtDescription)
            Me.Controls.Add(Me.CogDisplay1)
            Me.Name = "HardwareTrigger"
            Me.Text = "Hardware Trigger Sample Application"
            CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private mAcqFifo As Cognex.VisionPro.ICogAcqFifo = Nothing
        Private mTrigger As Cognex.VisionPro.ICogAcqTrigger
        Private numacqs As Integer

        ' This is the complete event handler for acquisition.  When an image is acquired,
        ' it fires a complete event.  This handler verifies the state of the acquisition
        ' fifo, and then calls Complete(), which gets the image from the fifo.

        ' Note that it is necessary to call the .NET garbarge collector on a regular
        ' basis so large images that are no longer used will be released back to the
        ' heap.  In this sample, it is called every 5th acqusition.
        Private Sub macqfifo_complete(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogCompleteEventArgs)
            If InvokeRequired Then
                Dim eventArgs() As Object = {sender, e}
                Invoke(New CogCompleteEventHandler(AddressOf macqfifo_complete), _
                  eventArgs)
                Return
            End If

            Dim numReadyVal, numPendingVal As Integer
            Dim busyVal As Boolean
			Dim info As New CogAcqInfo
			
            Try

                mAcqFifo.GetFifoState(numPendingVal, numReadyVal, busyVal)
                If (numReadyVal > 0) Then
                    CogDisplay1.Image = mAcqFifo.CompleteAcquireEx(info)
                    numacqs += 1
                End If
                txtDescription.Text = "Running..."  'Clear the text
                ' We need to run the garbage collector on occasion to cleanup
                ' images that are no longer being used.
                If numacqs > 4 Then
                    GC.Collect()
                    numacqs = 0
                End If

            Catch ex As CogException
                MessageBox.Show("The following error has occured:" & vbCrLf & ex.Message)
            Catch gex As Exception
                MessageBox.Show("The following error has occured:" & vbCrLf & gex.Message)
            End Try

        End Sub
        ' "Run" command button handler.
        ' Starts tool run
        Private Sub cmdRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRun.Click
            Try
                ' Now change the button caption.
                If cmdRun.Text = "Run" Then
                    ' Flush all outstanding acquisitions since they are not part of new acquisitions.
                    mAcqFifo.Flush()

                    ' If strobing is supported, set enable based on the checkbox state.
                    If Not mAcqFifo.OwnedStrobeParams Is Nothing Then
                        mAcqFifo.OwnedStrobeParams.StrobeEnabled = chkEnbStrobe.Checked
                        chkEnbStrobe.Enabled = False
                    End If

                    ' Step 5 - The trigger enabled bit must be set to true in order to acquire images.
                    mTrigger.TriggerEnabled = True

                    ' Tell the user that it is now waiting for external triggers.
                    txtDescription.Text = "Waiting for external triggers..."
                    cmdRun.Text = "Stop"
                Else
                    ' No images will be acquired when the trigger is disabled.
                    mTrigger.TriggerEnabled = False

                    chkEnbStrobe.Enabled = True

                    txtDescription.Text = _
                      "Sample description: demonstrates how to setup hardware trigger mode. A Cognex frame grabber board must be present " & vbCrLf & _
                      "in order to run this sample program. When the Run button is pressed, the program first flushes all outstanding acquisitions " & vbCrLf & _
                      "since they are not part of new acquisitions. It then enables the trigger enabled bit and waits for the external trigger. " & vbCrLf & _
                      "No new image is displayed if no external trigger comes in."

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
        ' This subroutine does all the initialization work 
        ' 
        Private Sub InitializeAcquisition()
            ' Step 1 - Create an acquisition fifo using the first avaiable frame grabber and
            '          video format.
            Dim fgs As CogFrameGrabbers = New CogFrameGrabbers

            If fgs.Count > 0 Then
                mAcqFifo = fgs(0).CreateAcqFifo( _
                  fgs(0).AvailableVideoFormats(0), _
                  CogAcqFifoPixelFormatConstants.Format8Grey, _
                  0, _
                  True)
            Else
                Throw New CogAcqNoFrameGrabberException("A board might be missing or not be functioning properly.")
            End If

            ' Check if a fifo was successfully created
            If mAcqFifo Is Nothing Then
                Throw New CogAcqCannotCreateFifoException("Unable to create Acquisition FIFO.")
            End If

            ' Display the board type
            lblBoardType.Text = mAcqFifo.FrameGrabber.Name

            ' See samples\Programming\Acquisition\Operators sample for obtaining each operator.
            ' Step 2 - Assign the CogAcqFifo CogAcqTrigger 

            ' Get the CogAcqTrigger
            ' Controls an acquisition FIFO's trigger model.
            mTrigger = mAcqFifo.OwnedTriggerParams
            If mTrigger Is Nothing Then
                Throw New CogAcqNoFrameGrabberException("This board type " & mAcqFifo.FrameGrabber.Name & _
                                    "does not support trigger mode.")
            End If

            ' Do not enable the trigger until the Run button is pressed.
            If mTrigger.TriggerEnabled Then
                mTrigger.TriggerEnabled = False
            End If

            ' Step 3 - Select hardware auto trigger mode.
            mTrigger.TriggerModel = CogAcqTriggerModelConstants.Auto

            ' NOTE: Either the exposure or brightness may need adjustment to clearly see
            '       the acquired image. Both exposure and brightness are set to high values
            '       in case sufficient lighting is unavailable.
            Dim Exposure As Cognex.VisionPro.ICogAcqExposure
            Dim Brightness As Cognex.VisionPro.ICogAcqBrightness
            Exposure = mAcqFifo.OwnedExposureParams
            Brightness = mAcqFifo.OwnedBrightnessParams
            'check to make sure the properties are supported and then set them
            If Not Exposure Is Nothing Then
                Exposure.Exposure = 50     ' in milli-seconds (ms)
            End If
            If Not Brightness Is Nothing Then
                Brightness.Brightness = 0.9
            End If

            ' Show the strobe check box if strobing is supported.
            If mAcqFifo.OwnedStrobeParams Is Nothing Then
                chkEnbStrobe.Visible = False
            Else
                chkEnbStrobe.Visible = True
            End If

            ' Hook up the acquisition completion event. Each time a tool acquires
            ' an image, it fires the acquisition completion event.
            AddHandler mAcqFifo.Complete, AddressOf macqfifo_complete

        End Sub

        Private Sub HardwareTrigger_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
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