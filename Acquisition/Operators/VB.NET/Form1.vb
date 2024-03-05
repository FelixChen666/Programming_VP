'*******************************************************************************
'Copyright (C) 2004-2010 Cognex Corporation
'
'Subject to Cognex Corporation's terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.
'*******************************************************************************
'
' This sample demonstrates how to retrieve each operator of the CogAcqFifoTool.
' Operators perform most of the operations such as acquiring images, setting
' the brightness and exposure, enabling or disabling the strobe, etc. Some properties
' cannot be directly accessed without getting the actual operator object such
' as setting the brightness or constrast, etc.
'
' NOTE:
' What is an CogAcqFifoTool operator?
' An CogAcqFifoTool operator supports a group of properties that control the behavior
' of a specific type of frame grabber. These properties might include strobe, brightness,
' exposure, and so on.
'
' Why does the CogAcqFifoTool have many operators?
' The CogAcqFifoTool has many operators because not all frame grabbers support
' the same properties.
'
Option Explicit On 
'Needed for VisionPro
Imports Cognex.VisionPro
'Needed for VisionPro Exceptions
Imports Cognex.VisionPro.Exceptions
Namespace Operators
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
    Friend WithEvents lblMsg As System.Windows.Forms.Label
    Friend WithEvents cmdRun As System.Windows.Forms.Button
    Friend WithEvents textBox1 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.lblMsg = New System.Windows.Forms.Label
      Me.cmdRun = New System.Windows.Forms.Button
      Me.textBox1 = New System.Windows.Forms.TextBox
      Me.SuspendLayout()
      '
      'lblMsg
      '
      Me.lblMsg.Location = New System.Drawing.Point(24, 24)
      Me.lblMsg.Name = "lblMsg"
      Me.lblMsg.Size = New System.Drawing.Size(264, 312)
      Me.lblMsg.TabIndex = 0
      Me.lblMsg.Text = "Press the Run button to see the list of operators."
      '
      'cmdRun
      '
      Me.cmdRun.Location = New System.Drawing.Point(80, 360)
      Me.cmdRun.Name = "cmdRun"
      Me.cmdRun.Size = New System.Drawing.Size(128, 32)
      Me.cmdRun.TabIndex = 1
      Me.cmdRun.Text = "Run"
      '
      'textBox1
      '
      Me.textBox1.Location = New System.Drawing.Point(0, 408)
      Me.textBox1.Multiline = True
      Me.textBox1.Name = "textBox1"
      Me.textBox1.ReadOnly = True
      Me.textBox1.Size = New System.Drawing.Size(304, 48)
      Me.textBox1.TabIndex = 8
      Me.textBox1.Text = "Sample description: illustrates how to retrieve each operator of " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "a given acquis" & _
      "ition tool."
      Me.textBox1.WordWrap = False
      '
      'Form1
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(304, 462)
      Me.Controls.Add(Me.textBox1)
      Me.Controls.Add(Me.cmdRun)
      Me.Controls.Add(Me.lblMsg)
      Me.Name = "Form1"
      Me.Text = "Lists all the available acquisition operators"
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region " Private vars"
    Private mTool As CogAcqFifoTool        ' Tool being edited

    ' List of Operators that may apply to a frame grabber.
    ' Declare the following variables with WithEvents if you want to capture events
    ' from each operator
    Private mAcqFifo As Cognex.VisionPro.ICogAcqFifo    ' AcqFifo being edited
    Private mExposure As Cognex.VisionPro.ICogAcqExposure
    Private mBrightness As Cognex.VisionPro.ICogAcqBrightness
    Private mContrast As Cognex.VisionPro.ICogAcqContrast
    Private mTrigger As Cognex.VisionPro.ICogAcqTrigger
    Private mStrobe As Cognex.VisionPro.ICogAcqStrobe
    Private mStrobeDly As Cognex.VisionPro.ICogAcqStrobeDelay
    Private mStrobePulseDuration As Cognex.VisionPro.ICogAcqStrobePulseDuration
    Private mRegion As Cognex.VisionPro.ICogAcqROI
    Private mAcqLight As Cognex.VisionPro.ICogAcqLight
    Private mLtable As Cognex.VisionPro.ICogAcqLookupTable
    Private mCameraGain As Cognex.VisionPro.ICogAcqDigitalCameraGain
    Private mSyncModel As Cognex.VisionPro.ICogAcqSync
    Private mLineScan As Cognex.VisionPro.ICogAcqLineScan

#End Region

    ' Click handler for "Run" command button 
    '
    Private Sub cmdRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRun.Click
      Try
        Me.Cursor = Cursors.WaitCursor
        ' Create a tool
        mTool = New CogAcqFifoTool
        If mTool Is Nothing Then
          Throw New Exception("Cannot create the CogAcqFifoTool.")
        End If

        ' First, get the CogAcqFifo operator. If the CogAcqFifoTool cannot create
        ' the CogAcqFifo, it throws an error.
        ' Controls an acquisition FIFO.
        mAcqFifo = mTool.[Operator]      ' This shows how to obtain an operator.
        If mAcqFifo Is Nothing Then
          Throw New CogAcqNoFrameGrabberException("A board might be missing or not be functioning properly.")
        End If

        lblMsg.Text = "Board Type = " & mAcqFifo.FrameGrabber.Name & vbCrLf
        lblMsg.Text += "Supports CogAcqFifo" & vbCrLf

        ' Get the CogAcqExposure
        ' Controls exposure time for an acquisition FIFO. For strobed acquisitions,
        ' this value is the duration of the strobe pulse.
        mExposure = mTool.[Operator].OwnedExposureParams
        ' If it fails to create an instance of the CogAcqExposure operator then do not
        ' display its operator name
        If Not mExposure Is Nothing Then
          lblMsg.Text += "Supports CogAcqExposure" & vbCrLf
        End If
        ' Get the CogAcqBrightness
        ' Controls brightness levels of an acquired image
        mBrightness = mTool.[Operator].OwnedBrightnessParams
        If Not mBrightness Is Nothing Then
          lblMsg.Text += "Supports CogAcqBrightness" & vbCrLf
        End If
        ' Get the CogAcqContrast
        ' Controls contrast levels of an acquired image
        mContrast = mTool.[Operator].OwnedContrastParams
        If Not mContrast Is Nothing Then
          lblMsg.Text += "Supports CogAcqContrast" & vbCrLf
        End If

        ' Get the CogAcqTrigger
        ' Controls an acquisition FIFO's trigger model.
        mTrigger = mTool.[Operator].OwnedTriggerParams
        If Not mTrigger Is Nothing Then
          lblMsg.Text += "Supports CogAcqTrigger" & vbCrLf
        End If
        ' Get the CogAcqStrobe
        ' Controls a strobe light associated with an acquisition FIFO.
        mStrobe = mTool.[Operator].OwnedStrobeParams
        If Not mStrobe Is Nothing Then
          lblMsg.Text += "Supports CogAcqStrobe" & vbCrLf
        End If

        ' Get the CogAcqStrobeDelay. Controls strobe delay
        mStrobeDly = mTool.[Operator].OwnedStrobeDelayParams
        If Not mStrobeDly Is Nothing Then
          lblMsg.Text += "Supports CogAcqStrobeDelay" & vbCrLf
        End If

        ' Get the CogAcqStrobePulseDuration.
        ' Controls the strobe pulse duration.
        mStrobePulseDuration = mTool.[Operator].OwnedStrobePulseDurationParams
        If Not mStrobePulseDuration Is Nothing Then
          lblMsg.Text += "Supports CogAcqStrobePulseDuration" & vbCrLf
        End If

        ' Get the CogAcqLight
        ' Controls lighting devices, such as the Cognex acuLight, for an acquisition FIFO.
        mAcqLight = mTool.[Operator].OwnedLightParams
        If Not mAcqLight Is Nothing Then
          lblMsg.Text += "Supports CogAcqLight" & vbCrLf
        End If

        ' Get the CogAcqROI
        ' Used to specify the region of interest (ROI) for image acquisition.
        mRegion = mTool.[Operator].OwnedROIParams
        If Not mRegion Is Nothing Then
          lblMsg.Text += "Supports CogAcqROI" & vbCrLf
        End If

        ' Get the CogAcqLookupTable
        ' A lookup table for mapping pixel values of acquired images.
        mLtable = mTool.[Operator].OwnedLookupTableParams
        If Not mLtable Is Nothing Then
          lblMsg.Text += "Supports CogAcqLookupTable" & vbCrLf
        End If

        ' Get the CogAcqLineScan
        mLineScan = mTool.[Operator].OwnedLineScanParams
        If Not mLineScan Is Nothing Then
          lblMsg.Text += "Supports CogAcqLineScan" & vbCrLf
        End If

        ' Get the CogAcqDigitalCameraGain
        ' Controls the gain on a digital camera.
        mCameraGain = mTool.[Operator].OwnedDigitalCameraGainParams
        If Not mCameraGain Is Nothing Then
          lblMsg.Text += "Supports CogAcqDigitalCameraGain" & vbCrLf
        End If

      Catch ex As CogException
        MessageBox.Show(ex.Message)
        Application.Exit()
      Catch gex As Exception
        MessageBox.Show(gex.Message)
        Application.Exit()
      Finally
        Me.Cursor = Cursors.Arrow
      End Try

    End Sub
  End Class
End Namespace
