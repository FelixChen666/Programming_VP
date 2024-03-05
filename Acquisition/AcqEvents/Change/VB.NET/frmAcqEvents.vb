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
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This sample demonstates how to catch and handle various acquisition events.
'
' The most common type of event fired by a VisionPro object is a change event.
' Change events are used by VisionPro objects to indicate that the state of
' the object has changed. For example, when you change the brightness of
' an acquisition FIFO it fires a change event that you can handle if you want
' to do something in response to the change or ignore if you do not. In this sample,
' we will show you how to capture the brightness, contrast, strobe enabled, and
' trigger enabled events.
'
' A Cognex frame grabber must be present in order to run this sample program.
' If no Cognex board is present, the program displays an error and exits.
'
' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
'
' Follow the next steps in order to catch the acquisition events.
' Step 1) Create the CogAcqFifoTool
' Step 2) Assign the CogAcqFifo, CogAcqBrightness, CogAcqContrast, CogAcqTrigger, and
'         CogAcqStrobe operators.
' Step 3) Add the mAcqFifo_Change event handler.
'
Option Strict On
Imports System
' Needed for VisionPro
Imports Cognex.VisionPro
' Needed for VisionPro Exceptions
Imports Cognex.VisionPro.Exceptions
Namespace Change
  Public Class frmAcqEvents
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
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblVideoFormat As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents brightnessUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents contrastUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkTriggerEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents chkStrobeEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents acqButton As System.Windows.Forms.Button
        Friend WithEvents label5 As System.Windows.Forms.Label
        Friend WithEvents lblNoBrightness As System.Windows.Forms.Label
        Friend WithEvents lblNoContrast As System.Windows.Forms.Label
        Friend WithEvents lblNoStrobe As System.Windows.Forms.Label
        Friend WithEvents lblNoTrigger As System.Windows.Forms.Label
    Friend WithEvents EventLabel As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAcqEvents))
            Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
            Me.txtDescription = New System.Windows.Forms.TextBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.lblBoardType = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.lblVideoFormat = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.brightnessUpDown = New System.Windows.Forms.NumericUpDown
            Me.Label4 = New System.Windows.Forms.Label
            Me.contrastUpDown = New System.Windows.Forms.NumericUpDown
            Me.chkTriggerEnabled = New System.Windows.Forms.CheckBox
            Me.chkStrobeEnabled = New System.Windows.Forms.CheckBox
            Me.acqButton = New System.Windows.Forms.Button
            Me.label5 = New System.Windows.Forms.Label
            Me.EventLabel = New System.Windows.Forms.Label
            Me.lblNoBrightness = New System.Windows.Forms.Label
            Me.lblNoContrast = New System.Windows.Forms.Label
            Me.lblNoStrobe = New System.Windows.Forms.Label
            Me.lblNoTrigger = New System.Windows.Forms.Label
            CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.brightnessUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.contrastUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CogDisplay1
            '
            Me.CogDisplay1.Location = New System.Drawing.Point(272, 16)
            Me.CogDisplay1.Name = "CogDisplay1"
            Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
            Me.CogDisplay1.Size = New System.Drawing.Size(328, 344)
            Me.CogDisplay1.TabIndex = 0
            '
            'txtDescription
            '
            Me.txtDescription.Location = New System.Drawing.Point(8, 376)
            Me.txtDescription.Multiline = True
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Size = New System.Drawing.Size(600, 40)
            Me.txtDescription.TabIndex = 1
            Me.txtDescription.Text = " This sample demonstates how to catch and handle various acquisition events. If t" & _
                "he trigger is disabled, " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "a new image will not be acquired. Increase the brightn" & _
                "ess if you see a dark image."
            Me.txtDescription.WordWrap = False
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(8, 16)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(80, 23)
            Me.Label1.TabIndex = 2
            Me.Label1.Text = "Board Type"
            '
            'lblBoardType
            '
            Me.lblBoardType.Location = New System.Drawing.Point(112, 16)
            Me.lblBoardType.Name = "lblBoardType"
            Me.lblBoardType.Size = New System.Drawing.Size(152, 24)
            Me.lblBoardType.TabIndex = 3
            Me.lblBoardType.Text = "Unknown"
            '
            'Label2
            '
            Me.Label2.Location = New System.Drawing.Point(8, 48)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(176, 16)
            Me.Label2.TabIndex = 4
            Me.Label2.Text = "Selected Video Format"
            '
            'lblVideoFormat
            '
            Me.lblVideoFormat.Location = New System.Drawing.Point(8, 73)
            Me.lblVideoFormat.Name = "lblVideoFormat"
            Me.lblVideoFormat.Size = New System.Drawing.Size(258, 66)
            Me.lblVideoFormat.TabIndex = 5
            Me.lblVideoFormat.Text = "Unknown"
            '
            'Label3
            '
            Me.Label3.Location = New System.Drawing.Point(16, 144)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(107, 20)
            Me.Label3.TabIndex = 6
            Me.Label3.Text = "Brightness"
            '
            'brightnessUpDown
            '
            Me.brightnessUpDown.DecimalPlaces = 1
            Me.brightnessUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
            Me.brightnessUpDown.Location = New System.Drawing.Point(120, 142)
            Me.brightnessUpDown.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.brightnessUpDown.Name = "brightnessUpDown"
            Me.brightnessUpDown.Size = New System.Drawing.Size(82, 20)
            Me.brightnessUpDown.TabIndex = 9
            '
            'Label4
            '
            Me.Label4.Location = New System.Drawing.Point(16, 184)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(107, 20)
            Me.Label4.TabIndex = 10
            Me.Label4.Text = "Contrast"
            '
            'contrastUpDown
            '
            Me.contrastUpDown.DecimalPlaces = 1
            Me.contrastUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
            Me.contrastUpDown.Location = New System.Drawing.Point(120, 184)
            Me.contrastUpDown.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.contrastUpDown.Name = "contrastUpDown"
            Me.contrastUpDown.Size = New System.Drawing.Size(82, 20)
            Me.contrastUpDown.TabIndex = 11
            '
            'chkTriggerEnabled
            '
            Me.chkTriggerEnabled.Location = New System.Drawing.Point(16, 256)
            Me.chkTriggerEnabled.Name = "chkTriggerEnabled"
            Me.chkTriggerEnabled.Size = New System.Drawing.Size(186, 16)
            Me.chkTriggerEnabled.TabIndex = 12
            Me.chkTriggerEnabled.Text = "Trigger Enabled"
            '
            'chkStrobeEnabled
            '
            Me.chkStrobeEnabled.Location = New System.Drawing.Point(16, 224)
            Me.chkStrobeEnabled.Name = "chkStrobeEnabled"
            Me.chkStrobeEnabled.Size = New System.Drawing.Size(186, 26)
            Me.chkStrobeEnabled.TabIndex = 13
            Me.chkStrobeEnabled.Text = "Strobe Enabled"
            '
            'acqButton
            '
            Me.acqButton.Location = New System.Drawing.Point(80, 288)
            Me.acqButton.Name = "acqButton"
            Me.acqButton.Size = New System.Drawing.Size(104, 32)
            Me.acqButton.TabIndex = 14
            Me.acqButton.Text = "Acquire Image"
            '
            'label5
            '
            Me.label5.Location = New System.Drawing.Point(16, 336)
            Me.label5.Name = "label5"
            Me.label5.Size = New System.Drawing.Size(48, 24)
            Me.label5.TabIndex = 15
            Me.label5.Text = "Event:"
            '
            'EventLabel
            '
            Me.EventLabel.Location = New System.Drawing.Point(80, 336)
            Me.EventLabel.Name = "EventLabel"
            Me.EventLabel.Size = New System.Drawing.Size(72, 24)
            Me.EventLabel.TabIndex = 16
            Me.EventLabel.Text = "None"
            '
            'lblNoBrightness
            '
            Me.lblNoBrightness.AutoSize = True
            Me.lblNoBrightness.Location = New System.Drawing.Point(13, 144)
            Me.lblNoBrightness.Name = "lblNoBrightness"
            Me.lblNoBrightness.Size = New System.Drawing.Size(189, 13)
            Me.lblNoBrightness.TabIndex = 17
            Me.lblNoBrightness.Text = "CogAcqFifo doesn't support Brightness"
            Me.lblNoBrightness.Visible = False
            '
            'lblNoContrast
            '
            Me.lblNoContrast.AutoSize = True
            Me.lblNoContrast.Location = New System.Drawing.Point(12, 184)
            Me.lblNoContrast.Name = "lblNoContrast"
            Me.lblNoContrast.Size = New System.Drawing.Size(179, 13)
            Me.lblNoContrast.TabIndex = 18
            Me.lblNoContrast.Text = "CogAcqFifo doesn't support Contrast"
            Me.lblNoContrast.Visible = False
            '
            'lblNoStrobe
            '
            Me.lblNoStrobe.AutoSize = True
            Me.lblNoStrobe.Location = New System.Drawing.Point(12, 225)
            Me.lblNoStrobe.Name = "lblNoStrobe"
            Me.lblNoStrobe.Size = New System.Drawing.Size(171, 13)
            Me.lblNoStrobe.TabIndex = 19
            Me.lblNoStrobe.Text = "CogAcqFifo doesn't support Strobe"
            Me.lblNoStrobe.Visible = False
            '
            'lblNoTrigger
            '
            Me.lblNoTrigger.AutoSize = True
            Me.lblNoTrigger.Location = New System.Drawing.Point(12, 259)
            Me.lblNoTrigger.Name = "lblNoTrigger"
            Me.lblNoTrigger.Size = New System.Drawing.Size(173, 13)
            Me.lblNoTrigger.TabIndex = 20
            Me.lblNoTrigger.Text = "CogAcqFifo doesn't support Trigger"
            Me.lblNoTrigger.Visible = False
            '
            'frmAcqEvents
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(616, 422)
            Me.Controls.Add(Me.EventLabel)
            Me.Controls.Add(Me.label5)
            Me.Controls.Add(Me.acqButton)
            Me.Controls.Add(Me.chkStrobeEnabled)
            Me.Controls.Add(Me.chkTriggerEnabled)
            Me.Controls.Add(Me.contrastUpDown)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.brightnessUpDown)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.lblVideoFormat)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.lblBoardType)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.txtDescription)
            Me.Controls.Add(Me.CogDisplay1)
            Me.Controls.Add(Me.lblNoBrightness)
            Me.Controls.Add(Me.lblNoContrast)
            Me.Controls.Add(Me.lblNoStrobe)
            Me.Controls.Add(Me.lblNoTrigger)
            Me.Name = "frmAcqEvents"
            Me.Text = "Shows how to capture certain acquisition events"
            CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.brightnessUpDown, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.contrastUpDown, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region
#Region "Private vars"
    Private mTool As CogAcqFifoTool
    Private mAcqFifo As Cognex.VisionPro.ICogAcqFifo
    Private mBrightness As Cognex.VisionPro.ICogAcqBrightness
    Private mContrast As Cognex.VisionPro.ICogAcqContrast
    Private mTrigger As Cognex.VisionPro.ICogAcqTrigger
    Private mStrobe As Cognex.VisionPro.ICogAcqStrobe
    Private numAcqs As Integer

#End Region
#Region " Form events"
    ' This method creates a new CogAcqFifoTool, assigns operators, hooks AcqFifo complete and change events
    '
    Private Sub frmAcqEvents_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Try
        ' Step 1 - Create an acquisition tool which creates an CogAcqFifo with a default
        '          video format (Sony XC75 - 640 x 480).
        mTool = New CogAcqFifoTool

        ' Check if the tool was able to create a default acqfifo.
        If mTool.[Operator] Is Nothing Then
          Throw New CogAcqNoFrameGrabberException("A board might be missing or not be functioning properly.")
        End If
        ' See samples\Programming\Acquisition\Operators sample for obtaining each operator.
        ' Step 2 - Assign the CogAcqFifo, CogAcqBrightness, CogAcqContrast, CogAcqTrigger, and
        '          CogAcqStrobe operators.
        ' First, Get the ICogAcqFifo.
        mAcqFifo = mTool.[Operator]

        ' Display the video format.
        lblVideoFormat.Text = mAcqFifo.VideoFormat
        ' Display the board type
        lblBoardType.Text = mAcqFifo.FrameGrabber.Name
        ' Let's have a small timeout period.
        mAcqFifo.Timeout = 300  ' in ms.

        ' Get the CogAcqBrightness
        ' Controls brightness levels of an acquired image
        mBrightness = mTool.[Operator].OwnedBrightnessParams
                ' Show the initial brightness
                If Not mBrightness Is Nothing Then
                    brightnessUpDown.Value = CType(mBrightness.Brightness, Decimal)
                Else
                    brightnessUpDown.Visible = False
                    lblNoBrightness.Visible = True
                End If

                ' Get the CogAcqContrast
                ' Controls contrast levels of an acquired image
                mContrast = mTool.[Operator].OwnedContrastParams
                ' Show the initial contrast
                If Not mContrast Is Nothing Then
                    contrastUpDown.Value = CType(mContrast.Contrast, Decimal)
                Else
                    contrastUpDown.Visible = False
                    lblNoContrast.Visible = True
                End If
                ' Get the CogAcqTrigger
                ' Controls an acquisition FIFO's trigger model.
                mTrigger = mTool.[Operator].OwnedTriggerParams
                ' Show the initial trigger enabled state.
                If Not mTrigger Is Nothing Then
                    chkTriggerEnabled.Checked = mTrigger.TriggerEnabled
                Else
                    chkTriggerEnabled.Visible = False
                    lblNoTrigger.Visible = True
                End If
                ' Get the CogAcqStrobe
                ' Controls a strobe light associated with an acquisition FIFO.
                mStrobe = mTool.[Operator].OwnedStrobeParams
                ' Show the initial state of StrobeEnabled
                If Not mStrobe Is Nothing Then
                    chkStrobeEnabled.Checked = mStrobe.StrobeEnabled
                Else
                    chkStrobeEnabled.Visible = False
                    lblNoStrobe.Visible = True
                End If
                ' Hook up the acquisition completion event. Each time a tool acquires
                ' an image, it fires the acquisition completion event.
                AddHandler mAcqFifo.Complete, AddressOf macqfifo_complete
                ' Hook up the operator changed event handler. Each time an operator
                ' (i.e. acquisition property) of ICogAcqFifo changes, it fires an
                ' operator changed event.
                AddHandler mAcqFifo.Changed, AddressOf macqfifo_changed

                ' See samples\Programming\Acquisition\Operators sample for obtaining other operators.
            Catch ex As CogAcqException
                DisplayErrorAndExit("Unexpected Error: " & ex.Message)
            Catch gex As Exception
                DisplayErrorAndExit("Unexpected Error: " & gex.Message)
            End Try
    End Sub
#End Region
#Region " Helper function"
    ' Displays an error message and then exits the program.
    ' Call this when an unrecoverable error has occurred.
    Private Sub DisplayErrorAndExit(ByVal ErrorMsg As String)
      MsgBox(ErrorMsg & vbCrLf & "Press OK to exit.")
      Me.Cursor = Cursors.Arrow
      Application.Exit()
    End Sub
#End Region
#Region "Acquisition  events"
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

        mTool.[Operator].GetFifoState(numPendingVal, numReadyVal, busyVal)
        If (numReadyVal > 0) Then
          CogDisplay1.Image = mTool.[Operator].CompleteAcquireEx(info)
          numAcqs += 1
        End If
        ' We need to run the garbage collector on occasion to cleanup
        ' images that are no longer being used.
        If numAcqs > 4 Then
          GC.Collect()
          numAcqs = 0
        End If

      Catch ex As CogException
        MessageBox.Show("The following error has occured:" & vbCrLf & ex.Message)
      Catch gex As Exception
        MessageBox.Show("The following error has occured:" & vbCrLf & gex.Message)
      End Try

    End Sub
    ' Whenever the state of a VisionPro object changes, that object fires a Changed event.
    ' The first argument to the Changed event is the object that fired the event.  The
    ' second argument contains a StateFlags property, which is a bitfield that indicates 
    ' what has changed on the sender object.  Each property that has changed on the object
    ' corresponds to a 1 in the StateFlag bitfield.  By interrogating the StateFlags bitfield, one
    ' can ascertain what has changed.

    ' Each time one of the properties changes, the EventLabel on the form updates the name
    ' of which property changed, and flips the color (red->green or green->red).
        Private Sub macqfifo_changed(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogChangedEventArgs)
          If InvokeRequired Then
            Dim eventArgs() As Object = {sender, e}
            Invoke(New CogChangedEventHandler(AddressOf macqfifo_changed), _
              eventArgs)
            Return
          End If

            Dim CurrentStatusColor As Color = EventLabel.ForeColor
            Dim found As Boolean = False
            EventLabel.Text = ""
            If (e.StateFlags And CogAcqFifoStateFlags.SfBrightness) > 0 Then

                EventLabel.Text += "Brightness "
                found = True
            End If
            If (e.StateFlags And CogAcqFifoStateFlags.SfContrast) > 0 Then

                EventLabel.Text += "Contrast "
                found = True
            End If
            If (e.StateFlags And CogAcqFifoStateFlags.SfTriggerEnabled) > 0 Then

                EventLabel.Text += "Trigger Enabled "
                found = True
            End If
            If (e.StateFlags And CogAcqFifoStateFlags.SfStrobeEnabled) > 0 Then

                EventLabel.Text += "Strobe Enabled "
                found = True
            End If
            If found Then
                If EventLabel.ForeColor.Equals(Color.Green) Then
                    EventLabel.ForeColor = Color.Red
                Else
                    EventLabel.ForeColor = Color.Green
                End If
            End If

        End Sub
        ' This method is changed event handler for the acqButton control
        ' on form.  It starts an acquisition.
        Private Sub acqButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles acqButton.Click
            Try
                If mAcqFifo Is Nothing Then
                    Exit Sub
                End If
                mAcqFifo.StartAcquire()
            Catch ex As CogException
                MessageBox.Show("The following error has occured:" & vbCrLf & ex.Message)
            Catch gex As Exception
                MessageBox.Show("The following error has occured:" & vbCrLf & gex.Message)
            End Try

        End Sub

#End Region
#Region "auxiliary controls handlers"
        ' this private method handles changes in brightness introduced by user 
        '
        Private Sub brightnessUpDown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles brightnessUpDown.ValueChanged
            If mBrightness Is Nothing Then
                Return
            End If

            If brightnessUpDown.Value >= 0 And brightnessUpDown.Value <= 1 Then
                mBrightness.Brightness = CType(brightnessUpDown.Value, Double)
            Else
                brightnessUpDown.Value = CType(0.5, Decimal)
                mBrightness.Brightness = 0.5
            End If

        End Sub
        ' this private method handles changes in contrast introduced by user 
        '
        Private Sub contrastUpDown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles contrastUpDown.ValueChanged
            If mContrast Is Nothing Then
                Return
            End If

            If contrastUpDown.Value >= 0 And contrastUpDown.Value <= 1 Then
                mContrast.Contrast = CType(contrastUpDown.Value, Double)
            Else
                contrastUpDown.Value = CType(0.5, Decimal)
                mContrast.Contrast = 0.5
            End If

        End Sub
        ' this private method handles changes in strobe enabled introduced by user 
        '
        Private Sub chkStrobeEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkStrobeEnabled.CheckedChanged
            If mStrobe Is Nothing Then
                Return
            End If

            mStrobe.StrobeEnabled = chkStrobeEnabled.Checked
        End Sub
        ' this private method handles changes in trigger enabled introduced by user 
        '
        Private Sub chkTriggerEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTriggerEnabled.CheckedChanged
            If mTrigger Is Nothing Then
                Return
            End If

            mTrigger.TriggerEnabled = chkTriggerEnabled.Checked
        End Sub
#End Region
    End Class
End Namespace