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

' This sample demonstates how to start and stop a live display using the CogDisplay.
'
' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
'
' Follow the next steps in order to start and stop the live display
' Step 1) Create a  CogAcqFifoTool.
' Step 2) Stop the live display
' Step 3) Start the live display.
Option Explicit On 

Imports System
' Needed for VisionPro
Imports Cognex.VisionPro
' Needed for VisionPro Exceptions
Imports Cognex.VisionPro.Exceptions

Namespace LiveDisplay
  Public Class Form1
    Inherits System.Windows.Forms.Form
    Private WithEvents mTool As CogAcqFifoTool
    Private mAcqFifo As Cognex.VisionPro.ICogAcqFifo
        Private mBrightness As Cognex.VisionPro.ICogAcqBrightness
        Friend WithEvents lblNoBrightness As System.Windows.Forms.Label
        Friend WithEvents lblNoContrast As System.Windows.Forms.Label
    Private mContrast As Cognex.VisionPro.ICogAcqContrast

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
    Friend WithEvents contrastUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents brightnessUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblVideoFormat As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblBoardType As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents acqButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
            Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
            Me.txtDescription = New System.Windows.Forms.TextBox
            Me.contrastUpDown = New System.Windows.Forms.NumericUpDown
            Me.Label4 = New System.Windows.Forms.Label
            Me.brightnessUpDown = New System.Windows.Forms.NumericUpDown
            Me.Label3 = New System.Windows.Forms.Label
            Me.lblVideoFormat = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.lblBoardType = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.acqButton = New System.Windows.Forms.Button
            Me.lblNoBrightness = New System.Windows.Forms.Label
            Me.lblNoContrast = New System.Windows.Forms.Label
            CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.contrastUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.brightnessUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CogDisplay1
            '
            Me.CogDisplay1.Location = New System.Drawing.Point(288, 8)
            Me.CogDisplay1.Name = "CogDisplay1"
            Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
            Me.CogDisplay1.Size = New System.Drawing.Size(328, 344)
            Me.CogDisplay1.TabIndex = 1
            '
            'txtDescription
            '
            Me.txtDescription.Location = New System.Drawing.Point(8, 360)
            Me.txtDescription.Multiline = True
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Size = New System.Drawing.Size(600, 32)
            Me.txtDescription.TabIndex = 2
            Me.txtDescription.Text = " This sample demonstates how to start and stop a live display using the CogDispla" & _
                "y control."
            Me.txtDescription.WordWrap = False
            '
            'contrastUpDown
            '
            Me.contrastUpDown.DecimalPlaces = 1
            Me.contrastUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
            Me.contrastUpDown.Location = New System.Drawing.Point(128, 208)
            Me.contrastUpDown.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.contrastUpDown.Name = "contrastUpDown"
            Me.contrastUpDown.Size = New System.Drawing.Size(89, 20)
            Me.contrastUpDown.TabIndex = 19
            '
            'Label4
            '
            Me.Label4.Location = New System.Drawing.Point(24, 208)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(107, 16)
            Me.Label4.TabIndex = 18
            Me.Label4.Text = "Contrast"
            '
            'brightnessUpDown
            '
            Me.brightnessUpDown.DecimalPlaces = 1
            Me.brightnessUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
            Me.brightnessUpDown.Location = New System.Drawing.Point(128, 168)
            Me.brightnessUpDown.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.brightnessUpDown.Name = "brightnessUpDown"
            Me.brightnessUpDown.Size = New System.Drawing.Size(89, 20)
            Me.brightnessUpDown.TabIndex = 17
            '
            'Label3
            '
            Me.Label3.Location = New System.Drawing.Point(24, 168)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(107, 20)
            Me.Label3.TabIndex = 16
            Me.Label3.Text = "Brightness"
            '
            'lblVideoFormat
            '
            Me.lblVideoFormat.Location = New System.Drawing.Point(8, 96)
            Me.lblVideoFormat.Name = "lblVideoFormat"
            Me.lblVideoFormat.Size = New System.Drawing.Size(264, 48)
            Me.lblVideoFormat.TabIndex = 15
            Me.lblVideoFormat.Text = "Unknown"
            '
            'Label2
            '
            Me.Label2.Location = New System.Drawing.Point(16, 72)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(176, 16)
            Me.Label2.TabIndex = 14
            Me.Label2.Text = "Selected Video Format"
            '
            'lblBoardType
            '
            Me.lblBoardType.Location = New System.Drawing.Point(120, 40)
            Me.lblBoardType.Name = "lblBoardType"
            Me.lblBoardType.Size = New System.Drawing.Size(152, 24)
            Me.lblBoardType.TabIndex = 13
            Me.lblBoardType.Text = "Unknown"
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(16, 40)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(80, 23)
            Me.Label1.TabIndex = 12
            Me.Label1.Text = "Board Type"
            '
            'acqButton
            '
            Me.acqButton.Location = New System.Drawing.Point(80, 272)
            Me.acqButton.Name = "acqButton"
            Me.acqButton.Size = New System.Drawing.Size(104, 32)
            Me.acqButton.TabIndex = 20
            Me.acqButton.Text = "Start Live Display"
            '
            'lblNoBrightness
            '
            Me.lblNoBrightness.AutoSize = True
            Me.lblNoBrightness.Location = New System.Drawing.Point(24, 170)
            Me.lblNoBrightness.Name = "lblNoBrightness"
            Me.lblNoBrightness.Size = New System.Drawing.Size(193, 13)
            Me.lblNoBrightness.TabIndex = 21
            Me.lblNoBrightness.Text = "CogAcqFifo Doesn't Support Brightness"
            Me.lblNoBrightness.Visible = False
            '
            'lblNoContrast
            '
            Me.lblNoContrast.AutoSize = True
            Me.lblNoContrast.Location = New System.Drawing.Point(24, 211)
            Me.lblNoContrast.Name = "lblNoContrast"
            Me.lblNoContrast.Size = New System.Drawing.Size(183, 13)
            Me.lblNoContrast.TabIndex = 22
            Me.lblNoContrast.Text = "CogAcqFifo Doesn't Support Contrast"
            Me.lblNoContrast.Visible = False
            '
            'Form1
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(616, 398)
            Me.Controls.Add(Me.acqButton)
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
            Me.Controls.Add(Me.lblNoContrast)
            Me.Controls.Add(Me.lblNoBrightness)
            Me.Name = "Form1"
            Me.Text = "Form1"
            CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.contrastUpDown, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.brightnessUpDown, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region
#Region " Initialization"
    Private Sub InitializeAcquisition()

      Me.Visible = True

      ' Step 1 - Create an acquisition tool which creates an CogAcqFifo with a default
      '          video format (Sony XC75 - 640 x 480).
      mTool = New CogAcqFifoTool

      ' Check if the tool was able to create a default acqfifo.
      If mTool.[Operator] Is Nothing Then
        Throw New CogAcqNoFrameGrabberException("A board might be missing or not be functioning properly.")
      End If
      ' See samples\Programming\Acquisition\Operators sample for obtaining each operator.
      ' Step 2 - Assign the CogAcqFifo, CogAcqBrightness, CogAcqContrast 
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
            ' Show the initial brightness if supported
            If Not mBrightness Is Nothing Then
                brightnessUpDown.Value = CType(mBrightness.Brightness, Decimal)
            Else
                brightnessUpDown.Visible = False
                lblNoBrightness.Visible = True
            End If
            ' Get the CogAcqContrast
            ' Controls contrast levels of an acquired image
            mContrast = mTool.[Operator].OwnedContrastParams
            ' Show the initial brightness if supported
            If Not mContrast Is Nothing Then
                contrastUpDown.Value = CType(mContrast.Contrast, Decimal)
            Else
                contrastUpDown.Visible = False
                lblNoContrast.Visible = True
            End If



        End Sub
#End Region
#Region " Auxiliary controls handlers"
    ' Toggle between live display and stop live display
    '
    Private Sub acqButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles acqButton.Click
      If CogDisplay1.LiveDisplayRunning Then
        ' Step 2 - Stop the live display
        CogDisplay1.StopLiveDisplay()
        acqButton.Text = "Start Live Display"
      Else
        ' Step 3 - Start the live display
        CogDisplay1.StartLiveDisplay(mTool.[Operator])
        acqButton.Text = "Stop Live Display"
      End If

    End Sub

    Private Sub brightnessUpDown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles brightnessUpDown.ValueChanged

            If mBrightness Is Nothing Then Return

            If brightnessUpDown.Value >= 0 And brightnessUpDown.Value <= 1 Then
                mBrightness.Brightness = CType(brightnessUpDown.Value, Double)
            Else
                brightnessUpDown.Value = CType(0.5, Decimal)
                mBrightness.Brightness = 0.5
            End If

        End Sub
        Private Sub contrastUpDown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles contrastUpDown.ValueChanged

            If mContrast Is Nothing Then Return

            If contrastUpDown.Value >= 0 And contrastUpDown.Value <= 1 Then
                mContrast.Contrast = CType(contrastUpDown.Value, Double)
            Else
                contrastUpDown.Value = CType(0.5, Decimal)
                mContrast.Contrast = 0.5
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
  End Class
End Namespace