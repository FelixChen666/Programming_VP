'*******************************************************************************
'Copyright (C) 2004 Cognex Corporation
'
'Subject to Cognex Corporation's terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.
'*******************************************************************************
'
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This application demonstrates using a collection of PatMax tools to find object
' points for N-Point calibration, perform calibration, and then measure blob
' features in calibrated space. The user chooses the number of calibration points
' (the default is 3) and a separate PatMax tool is constructed to find each
' calibration point. The N-Point calibration tool uses these points, along with
' USER SUPPLIED calibration measurements to compute the calibrated space.
' Blob is then run on the calibrated image and measurements for each found blob are
' returned.
'

'Declare references for the tools in this sample application.  Use WithEvents
'so that we can make use of the _Change and _PostRun synchronous event
'handlers.
Option Explicit On 
Imports Cognex.VisionPro.Blob
Imports Cognex.VisionPro.CalibFix
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.PMAlign
Imports Cognex.VisionPro
Imports Cognex.VisionPro.Exceptions
Imports System.Math
Imports System.ComponentModel
Public Class frmCalibNPoint
  Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()
       'This call is required by the Windows Form Designer.
    InitializeComponent()
      'Add any initialization after the InitializeComponent() call
    NumPMAlignTools = CInt(NumPMAlignToolsUpDown.Value)
    'InitializeTools()
    blnInitialized = True
  End Sub

  'Form overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then

      'Releasing framegrabbers
      Dim frameGrabbers As New CogFrameGrabbers()
      For i As Integer = 0 To frameGrabbers.Count - 1
        frameGrabbers(i).Disconnect(False)
      Next

      If Not (components Is Nothing) Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
  Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
  Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
  Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
  Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
  Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
  Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
  Private WithEvents GroupAcq As System.Windows.Forms.GroupBox
  Private WithEvents NextImageButton As System.Windows.Forms.Button
  Private WithEvents OpenFileButton As System.Windows.Forms.Button
  Private WithEvents ImageFileRadio As System.Windows.Forms.RadioButton
  Private WithEvents FrameGrabberRadio As System.Windows.Forms.RadioButton
  Friend WithEvents NumPMAlignToolsUpDown As System.Windows.Forms.NumericUpDown
  Friend WithEvents label1 As System.Windows.Forms.Label
  Friend WithEvents GroupPMAlign As System.Windows.Forms.GroupBox
  Friend WithEvents PMAlignRunButton As System.Windows.Forms.Button
  Friend WithEvents PMAlignSetupButton As System.Windows.Forms.Button
  Friend WithEvents GroupCalib As System.Windows.Forms.GroupBox
  Friend WithEvents CalibratedLabel As System.Windows.Forms.Label
  Friend WithEvents CalibRunButton As System.Windows.Forms.Button
  Friend WithEvents CalibSetupButton As System.Windows.Forms.Button
  Friend WithEvents GroupBlob As System.Windows.Forms.GroupBox
  Friend WithEvents BlobCountText As System.Windows.Forms.TextBox
  Friend WithEvents label3 As System.Windows.Forms.Label
  Friend WithEvents BlobRunButton As System.Windows.Forms.Button
  Friend WithEvents RunAllButton As System.Windows.Forms.Button
  Friend WithEvents DescriptionText As System.Windows.Forms.TextBox
  Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
  Friend WithEvents CogImageFileEdit1 As Cognex.VisionPro.ImageFile.CogImageFileEditV2
  Friend WithEvents CogPMAlignEdit1 As Cognex.VisionPro.PMAlign.CogPMAlignEditV2
  Friend WithEvents CogBlobEdit1 As Cognex.VisionPro.Blob.CogBlobEditV2
  Friend WithEvents cogCalibNPointEdit1 As Cognex.VisionPro.CalibFix.CogCalibNPointToNPointEditV2
  Friend WithEvents CogDisplay1 As Cognex.VisionPro.Display.CogDisplay
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents CogAcqFifoEditV21 As Cognex.VisionPro.CogAcqFifoEditV2
  Friend WithEvents PMAlignComboBox As System.Windows.Forms.ComboBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCalibNPoint))
    Me.TabControl1 = New System.Windows.Forms.TabControl
    Me.TabPage1 = New System.Windows.Forms.TabPage
    Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
    Me.RunAllButton = New System.Windows.Forms.Button
    Me.GroupBlob = New System.Windows.Forms.GroupBox
    Me.BlobCountText = New System.Windows.Forms.TextBox
    Me.label3 = New System.Windows.Forms.Label
    Me.BlobRunButton = New System.Windows.Forms.Button
    Me.GroupCalib = New System.Windows.Forms.GroupBox
    Me.CalibratedLabel = New System.Windows.Forms.Label
    Me.CalibRunButton = New System.Windows.Forms.Button
    Me.CalibSetupButton = New System.Windows.Forms.Button
    Me.GroupPMAlign = New System.Windows.Forms.GroupBox
    Me.PMAlignRunButton = New System.Windows.Forms.Button
    Me.PMAlignSetupButton = New System.Windows.Forms.Button
    Me.NumPMAlignToolsUpDown = New System.Windows.Forms.NumericUpDown
    Me.label1 = New System.Windows.Forms.Label
    Me.GroupAcq = New System.Windows.Forms.GroupBox
    Me.NextImageButton = New System.Windows.Forms.Button
    Me.OpenFileButton = New System.Windows.Forms.Button
    Me.ImageFileRadio = New System.Windows.Forms.RadioButton
    Me.FrameGrabberRadio = New System.Windows.Forms.RadioButton
    Me.TabPage4 = New System.Windows.Forms.TabPage
    Me.PMAlignComboBox = New System.Windows.Forms.ComboBox
    Me.Label2 = New System.Windows.Forms.Label
    Me.CogPMAlignEdit1 = New Cognex.VisionPro.PMAlign.CogPMAlignEditV2
    Me.TabPage3 = New System.Windows.Forms.TabPage
    Me.CogImageFileEdit1 = New Cognex.VisionPro.ImageFile.CogImageFileEditV2
    Me.TabPage2 = New System.Windows.Forms.TabPage
    Me.TabPage5 = New System.Windows.Forms.TabPage
    Me.cogCalibNPointEdit1 = New Cognex.VisionPro.CalibFix.CogCalibNPointToNPointEditV2
    Me.TabPage6 = New System.Windows.Forms.TabPage
    Me.CogBlobEdit1 = New Cognex.VisionPro.Blob.CogBlobEditV2
    Me.DescriptionText = New System.Windows.Forms.TextBox
    Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
    Me.CogAcqFifoEditV21 = New Cognex.VisionPro.CogAcqFifoEditV2
    Me.TabControl1.SuspendLayout()
    Me.TabPage1.SuspendLayout()
    CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupBlob.SuspendLayout()
    Me.GroupCalib.SuspendLayout()
    Me.GroupPMAlign.SuspendLayout()
    CType(Me.NumPMAlignToolsUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupAcq.SuspendLayout()
    Me.TabPage4.SuspendLayout()
    CType(Me.CogPMAlignEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.TabPage3.SuspendLayout()
    CType(Me.CogImageFileEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.TabPage2.SuspendLayout()
    Me.TabPage5.SuspendLayout()
    CType(Me.cogCalibNPointEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.TabPage6.SuspendLayout()
    CType(Me.CogBlobEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CogAcqFifoEditV21, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'TabControl1
    '
    Me.TabControl1.Controls.Add(Me.TabPage1)
    Me.TabControl1.Controls.Add(Me.TabPage4)
    Me.TabControl1.Controls.Add(Me.TabPage3)
    Me.TabControl1.Controls.Add(Me.TabPage2)
    Me.TabControl1.Controls.Add(Me.TabPage5)
    Me.TabControl1.Controls.Add(Me.TabPage6)
    Me.TabControl1.Location = New System.Drawing.Point(0, 0)
    Me.TabControl1.Name = "TabControl1"
    Me.TabControl1.SelectedIndex = 0
    Me.TabControl1.Size = New System.Drawing.Size(880, 512)
    Me.TabControl1.TabIndex = 0
    '
    'TabPage1
    '
    Me.TabPage1.Controls.Add(Me.CogDisplay1)
    Me.TabPage1.Controls.Add(Me.RunAllButton)
    Me.TabPage1.Controls.Add(Me.GroupBlob)
    Me.TabPage1.Controls.Add(Me.GroupCalib)
    Me.TabPage1.Controls.Add(Me.GroupPMAlign)
    Me.TabPage1.Controls.Add(Me.NumPMAlignToolsUpDown)
    Me.TabPage1.Controls.Add(Me.label1)
    Me.TabPage1.Controls.Add(Me.GroupAcq)
    Me.TabPage1.Location = New System.Drawing.Point(4, 22)
    Me.TabPage1.Name = "TabPage1"
    Me.TabPage1.Size = New System.Drawing.Size(872, 486)
    Me.TabPage1.TabIndex = 0
    Me.TabPage1.Text = "VisionPro Demo"
    '
    'CogDisplay1
    '
    Me.CogDisplay1.Location = New System.Drawing.Point(368, 24)
    Me.CogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
    Me.CogDisplay1.MouseWheelSensitivity = 1
    Me.CogDisplay1.Name = "CogDisplay1"
    Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
    Me.CogDisplay1.Size = New System.Drawing.Size(432, 336)
    Me.CogDisplay1.TabIndex = 9
    '
    'RunAllButton
    '
    Me.RunAllButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.RunAllButton.Location = New System.Drawing.Point(8, 384)
    Me.RunAllButton.Name = "RunAllButton"
    Me.RunAllButton.Size = New System.Drawing.Size(352, 32)
    Me.RunAllButton.TabIndex = 8
    Me.RunAllButton.Text = "Next Image and Run Blob"
    '
    'GroupBlob
    '
    Me.GroupBlob.Controls.Add(Me.BlobCountText)
    Me.GroupBlob.Controls.Add(Me.label3)
    Me.GroupBlob.Controls.Add(Me.BlobRunButton)
    Me.GroupBlob.Location = New System.Drawing.Point(8, 312)
    Me.GroupBlob.Name = "GroupBlob"
    Me.GroupBlob.Size = New System.Drawing.Size(352, 64)
    Me.GroupBlob.TabIndex = 7
    Me.GroupBlob.TabStop = False
    Me.GroupBlob.Text = "Blob"
    '
    'BlobCountText
    '
    Me.BlobCountText.Location = New System.Drawing.Point(240, 32)
    Me.BlobCountText.Name = "BlobCountText"
    Me.BlobCountText.ReadOnly = True
    Me.BlobCountText.Size = New System.Drawing.Size(72, 20)
    Me.BlobCountText.TabIndex = 2
    Me.BlobCountText.Text = "0"
    '
    'label3
    '
    Me.label3.Location = New System.Drawing.Point(168, 32)
    Me.label3.Name = "label3"
    Me.label3.Size = New System.Drawing.Size(64, 16)
    Me.label3.TabIndex = 1
    Me.label3.Text = "Blob Count"
    '
    'BlobRunButton
    '
    Me.BlobRunButton.Location = New System.Drawing.Point(32, 24)
    Me.BlobRunButton.Name = "BlobRunButton"
    Me.BlobRunButton.Size = New System.Drawing.Size(104, 32)
    Me.BlobRunButton.TabIndex = 0
    Me.BlobRunButton.Text = "Run"
    '
    'GroupCalib
    '
    Me.GroupCalib.Controls.Add(Me.CalibratedLabel)
    Me.GroupCalib.Controls.Add(Me.CalibRunButton)
    Me.GroupCalib.Controls.Add(Me.CalibSetupButton)
    Me.GroupCalib.Location = New System.Drawing.Point(8, 232)
    Me.GroupCalib.Name = "GroupCalib"
    Me.GroupCalib.Size = New System.Drawing.Size(352, 72)
    Me.GroupCalib.TabIndex = 6
    Me.GroupCalib.TabStop = False
    Me.GroupCalib.Text = "CalibNPoint"
    '
    'CalibratedLabel
    '
    Me.CalibratedLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.CalibratedLabel.ForeColor = System.Drawing.Color.Red
    Me.CalibratedLabel.Location = New System.Drawing.Point(248, 32)
    Me.CalibratedLabel.Name = "CalibratedLabel"
    Me.CalibratedLabel.Size = New System.Drawing.Size(88, 16)
    Me.CalibratedLabel.TabIndex = 2
    Me.CalibratedLabel.Text = "Uncalibrated"
    '
    'CalibRunButton
    '
    Me.CalibRunButton.Location = New System.Drawing.Point(144, 24)
    Me.CalibRunButton.Name = "CalibRunButton"
    Me.CalibRunButton.Size = New System.Drawing.Size(80, 32)
    Me.CalibRunButton.TabIndex = 1
    Me.CalibRunButton.Text = "Run"
    '
    'CalibSetupButton
    '
    Me.CalibSetupButton.Location = New System.Drawing.Point(32, 24)
    Me.CalibSetupButton.Name = "CalibSetupButton"
    Me.CalibSetupButton.Size = New System.Drawing.Size(80, 32)
    Me.CalibSetupButton.TabIndex = 0
    Me.CalibSetupButton.Text = "Calibrate"
    '
    'GroupPMAlign
    '
    Me.GroupPMAlign.Controls.Add(Me.PMAlignRunButton)
    Me.GroupPMAlign.Controls.Add(Me.PMAlignSetupButton)
    Me.GroupPMAlign.Location = New System.Drawing.Point(8, 152)
    Me.GroupPMAlign.Name = "GroupPMAlign"
    Me.GroupPMAlign.Size = New System.Drawing.Size(352, 72)
    Me.GroupPMAlign.TabIndex = 5
    Me.GroupPMAlign.TabStop = False
    Me.GroupPMAlign.Text = "PMAlign"
    '
    'PMAlignRunButton
    '
    Me.PMAlignRunButton.Location = New System.Drawing.Point(144, 24)
    Me.PMAlignRunButton.Name = "PMAlignRunButton"
    Me.PMAlignRunButton.Size = New System.Drawing.Size(80, 32)
    Me.PMAlignRunButton.TabIndex = 1
    Me.PMAlignRunButton.Text = "Run All"
    '
    'PMAlignSetupButton
    '
    Me.PMAlignSetupButton.Location = New System.Drawing.Point(32, 24)
    Me.PMAlignSetupButton.Name = "PMAlignSetupButton"
    Me.PMAlignSetupButton.Size = New System.Drawing.Size(80, 32)
    Me.PMAlignSetupButton.TabIndex = 0
    Me.PMAlignSetupButton.Text = "Setup All"
    '
    'NumPMAlignToolsUpDown
    '
    Me.NumPMAlignToolsUpDown.Location = New System.Drawing.Point(216, 120)
    Me.NumPMAlignToolsUpDown.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
    Me.NumPMAlignToolsUpDown.Minimum = New Decimal(New Integer() {3, 0, 0, 0})
    Me.NumPMAlignToolsUpDown.Name = "NumPMAlignToolsUpDown"
    Me.NumPMAlignToolsUpDown.Size = New System.Drawing.Size(64, 20)
    Me.NumPMAlignToolsUpDown.TabIndex = 4
    Me.NumPMAlignToolsUpDown.Value = New Decimal(New Integer() {3, 0, 0, 0})
    '
    'label1
    '
    Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.label1.Location = New System.Drawing.Point(24, 120)
    Me.label1.Name = "label1"
    Me.label1.Size = New System.Drawing.Size(176, 16)
    Me.label1.TabIndex = 3
    Me.label1.Text = "Number of Calibration Points:"
    '
    'GroupAcq
    '
    Me.GroupAcq.Controls.Add(Me.NextImageButton)
    Me.GroupAcq.Controls.Add(Me.OpenFileButton)
    Me.GroupAcq.Controls.Add(Me.ImageFileRadio)
    Me.GroupAcq.Controls.Add(Me.FrameGrabberRadio)
    Me.GroupAcq.Location = New System.Drawing.Point(8, 16)
    Me.GroupAcq.Name = "GroupAcq"
    Me.GroupAcq.Size = New System.Drawing.Size(352, 96)
    Me.GroupAcq.TabIndex = 1
    Me.GroupAcq.TabStop = False
    Me.GroupAcq.Text = "Image Acquisition"
    '
    'NextImageButton
    '
    Me.NextImageButton.Location = New System.Drawing.Point(240, 32)
    Me.NextImageButton.Name = "NextImageButton"
    Me.NextImageButton.Size = New System.Drawing.Size(88, 32)
    Me.NextImageButton.TabIndex = 3
    Me.NextImageButton.Text = "Next Image"
    '
    'OpenFileButton
    '
    Me.OpenFileButton.Location = New System.Drawing.Point(136, 32)
    Me.OpenFileButton.Name = "OpenFileButton"
    Me.OpenFileButton.Size = New System.Drawing.Size(88, 32)
    Me.OpenFileButton.TabIndex = 2
    Me.OpenFileButton.Text = "Open File"
    '
    'ImageFileRadio
    '
    Me.ImageFileRadio.Checked = True
    Me.ImageFileRadio.Location = New System.Drawing.Point(16, 56)
    Me.ImageFileRadio.Name = "ImageFileRadio"
    Me.ImageFileRadio.Size = New System.Drawing.Size(104, 24)
    Me.ImageFileRadio.TabIndex = 1
    Me.ImageFileRadio.TabStop = True
    Me.ImageFileRadio.Text = "Image File"
    '
    'FrameGrabberRadio
    '
    Me.FrameGrabberRadio.Location = New System.Drawing.Point(16, 24)
    Me.FrameGrabberRadio.Name = "FrameGrabberRadio"
    Me.FrameGrabberRadio.Size = New System.Drawing.Size(104, 24)
    Me.FrameGrabberRadio.TabIndex = 0
    Me.FrameGrabberRadio.Text = "Frame Grabber"
    '
    'TabPage4
    '
    Me.TabPage4.Controls.Add(Me.PMAlignComboBox)
    Me.TabPage4.Controls.Add(Me.Label2)
    Me.TabPage4.Controls.Add(Me.CogPMAlignEdit1)
    Me.TabPage4.Location = New System.Drawing.Point(4, 22)
    Me.TabPage4.Name = "TabPage4"
    Me.TabPage4.Size = New System.Drawing.Size(872, 486)
    Me.TabPage4.TabIndex = 3
    Me.TabPage4.Text = "PMalign"
    '
    'PMAlignComboBox
    '
    Me.PMAlignComboBox.Location = New System.Drawing.Point(136, 8)
    Me.PMAlignComboBox.Name = "PMAlignComboBox"
    Me.PMAlignComboBox.Size = New System.Drawing.Size(121, 21)
    Me.PMAlignComboBox.TabIndex = 2
    '
    'Label2
    '
    Me.Label2.Location = New System.Drawing.Point(16, 8)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(100, 23)
    Me.Label2.TabIndex = 1
    Me.Label2.Text = "PMAlign Tool:"
    '
    'CogPMAlignEdit1
    '
    Me.CogPMAlignEdit1.Location = New System.Drawing.Point(8, 32)
    Me.CogPMAlignEdit1.MinimumSize = New System.Drawing.Size(489, 0)
    Me.CogPMAlignEdit1.Name = "CogPMAlignEdit1"
    Me.CogPMAlignEdit1.Size = New System.Drawing.Size(760, 433)
    Me.CogPMAlignEdit1.SuspendElectricRuns = False
    Me.CogPMAlignEdit1.TabIndex = 0
    '
    'TabPage3
    '
    Me.TabPage3.Controls.Add(Me.CogImageFileEdit1)
    Me.TabPage3.Location = New System.Drawing.Point(4, 22)
    Me.TabPage3.Name = "TabPage3"
    Me.TabPage3.Size = New System.Drawing.Size(872, 486)
    Me.TabPage3.TabIndex = 2
    Me.TabPage3.Text = "Image File"
    '
    'CogImageFileEdit1
    '
    Me.CogImageFileEdit1.AllowDrop = True
    Me.CogImageFileEdit1.Location = New System.Drawing.Point(0, 0)
    Me.CogImageFileEdit1.MinimumSize = New System.Drawing.Size(489, 0)
    Me.CogImageFileEdit1.Name = "CogImageFileEdit1"
    Me.CogImageFileEdit1.OutputHighLight = System.Drawing.Color.Lime
    Me.CogImageFileEdit1.Size = New System.Drawing.Size(776, 320)
    Me.CogImageFileEdit1.SuspendElectricRuns = False
    Me.CogImageFileEdit1.TabIndex = 0
    '
    'TabPage2
    '
    Me.TabPage2.Controls.Add(Me.CogAcqFifoEditV21)
    Me.TabPage2.Location = New System.Drawing.Point(4, 22)
    Me.TabPage2.Name = "TabPage2"
    Me.TabPage2.Size = New System.Drawing.Size(872, 486)
    Me.TabPage2.TabIndex = 1
    Me.TabPage2.Text = "Acquisition"
    '
    'TabPage5
    '
    Me.TabPage5.Controls.Add(Me.cogCalibNPointEdit1)
    Me.TabPage5.Location = New System.Drawing.Point(4, 22)
    Me.TabPage5.Name = "TabPage5"
    Me.TabPage5.Size = New System.Drawing.Size(872, 486)
    Me.TabPage5.TabIndex = 4
    Me.TabPage5.Text = "CalibNPoint"
    '
    'cogCalibNPointEdit1
    '
    Me.cogCalibNPointEdit1.Location = New System.Drawing.Point(0, 16)
    Me.cogCalibNPointEdit1.MinimumSize = New System.Drawing.Size(489, 0)
    Me.cogCalibNPointEdit1.Name = "cogCalibNPointEdit1"
    Me.cogCalibNPointEdit1.Size = New System.Drawing.Size(720, 496)
    Me.cogCalibNPointEdit1.SuspendElectricRuns = False
    Me.cogCalibNPointEdit1.TabIndex = 0
    '
    'TabPage6
    '
    Me.TabPage6.Controls.Add(Me.CogBlobEdit1)
    Me.TabPage6.Location = New System.Drawing.Point(4, 22)
    Me.TabPage6.Name = "TabPage6"
    Me.TabPage6.Size = New System.Drawing.Size(872, 486)
    Me.TabPage6.TabIndex = 5
    Me.TabPage6.Text = "Blob"
    '
    'CogBlobEdit1
    '
    Me.CogBlobEdit1.Location = New System.Drawing.Point(16, 16)
    Me.CogBlobEdit1.MinimumSize = New System.Drawing.Size(489, 0)
    Me.CogBlobEdit1.Name = "CogBlobEdit1"
    Me.CogBlobEdit1.Size = New System.Drawing.Size(744, 408)
    Me.CogBlobEdit1.SuspendElectricRuns = False
    Me.CogBlobEdit1.TabIndex = 0
    '
    'DescriptionText
    '
    Me.DescriptionText.Location = New System.Drawing.Point(8, 512)
    Me.DescriptionText.Multiline = True
    Me.DescriptionText.Name = "DescriptionText"
    Me.DescriptionText.ReadOnly = True
    Me.DescriptionText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.DescriptionText.Size = New System.Drawing.Size(864, 104)
    Me.DescriptionText.TabIndex = 2
    Me.DescriptionText.Text = resources.GetString("DescriptionText.Text")
    '
    'CogAcqFifoEditV21
    '
    Me.CogAcqFifoEditV21.Location = New System.Drawing.Point(0, 0)
    Me.CogAcqFifoEditV21.MinimumSize = New System.Drawing.Size(489, 0)
    Me.CogAcqFifoEditV21.Name = "CogAcqFifoEditV21"
    Me.CogAcqFifoEditV21.Size = New System.Drawing.Size(869, 483)
    Me.CogAcqFifoEditV21.SuspendElectricRuns = False
    Me.CogAcqFifoEditV21.TabIndex = 0
    '
    'frmCalibNPoint
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(896, 630)
    Me.Controls.Add(Me.DescriptionText)
    Me.Controls.Add(Me.TabControl1)
    Me.Name = "frmCalibNPoint"
    Me.Text = "N-Point Calibration Sample Application"
    Me.TabControl1.ResumeLayout(False)
    Me.TabPage1.ResumeLayout(False)
    CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupBlob.ResumeLayout(False)
    Me.GroupBlob.PerformLayout()
    Me.GroupCalib.ResumeLayout(False)
    Me.GroupPMAlign.ResumeLayout(False)
    CType(Me.NumPMAlignToolsUpDown, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupAcq.ResumeLayout(False)
    Me.TabPage4.ResumeLayout(False)
    CType(Me.CogPMAlignEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.TabPage3.ResumeLayout(False)
    CType(Me.CogImageFileEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.TabPage2.ResumeLayout(False)
    Me.TabPage5.ResumeLayout(False)
    CType(Me.cogCalibNPointEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.TabPage6.ResumeLayout(False)
    CType(Me.CogBlobEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CogAcqFifoEditV21, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub

#End Region
#Region "Private vars"
  Private ImageFileTool As CogImageFileTool
  Private AcqFifoTool As CogAcqFifoTool
  Private PatMaxTool As CogPMAlignTool
  Private lngPatMaxSelected As Long
  ' Collection of PatMax tools to find object points for N-Point
  ' calibration. User chooses number of points, default is 3.
  ' Each PatMax tool is supposed to give us ONE object feature location
  ' (in uncalibrated space, of course). Its feature location will be used as
  ' an "Uncalibrated" point input to the N-Point calibration tool.  The N-Point calibration
  ' tool uses this information, along with USER SUPPLIED CALIBRATED COORDINATES for the point
  ' to compute the calibrated space.  Since the demo can't possibly guess what calibrated
  ' coordinates a user might wish to assign to these points, they will have to be entered
  ' manually in the grid on the calibration tab.  Typically, these calibrated points will
  ' be determined by measuring the features of an object, having its schematics, or by
  ' using an object with a regular pattern (such as a grid) with known dimensions.
  '
  ' Here is some physical information about the bracket in the database file
  ' in the images directory.  The bracket has four holes, two small, two large.
  ' The two large holes are 4.94 cm apart, the small holes are 10.88 cm
  ' apart, and the midpoint of the small holes to the midpoint of the large holes
  ' is 3.712 cm. This information should help pick reasonable raw calibrated
  ' coordinates to calibrate on those images.

  ' A CogCalibNPointToNPointTool for calibrating the points found by PatMax tools.
  ' Also sync events (via Calib) from Calibration sub-object of calibration tool
  ' to watch for changes in the "calibrated" state of the tool.
  Private CalibNPointTool As CogCalibNPointToNPointTool
  Private Calib As CogCalibNPointToNPoint  'CogCalibNPointToNPointParams

  ' A Blob tool to run in calibrated space.  Note that because blob runs in the
  ' calibrated space, its results such as area and perimeter are in calibrated
  ' units (e.g. centimeters, inches, mils, etc)
  Private BlobTool As CogBlobTool

  'Flag for "VisionPro Demo" tab indicating that user is currently setting up a
  'tool.  Also used to indicate in live video mode.  If user selects "Setup"
  'then the GUI controls are disabled except for the interactive graphics
  'required for setup as well as the "OK" button used to complete the Setup.
  Private blnSettingUp As Boolean
  Dim RunningAll As Boolean
  Dim RunningPMAlignTools As Boolean
  Private blnRunningPatMaxTools As Boolean
  Private blnRunningAll As Boolean
  Dim NumPMAlignTools As Integer
  Dim PMAlignSelected As Integer = 0
  Dim PMAlignTools As New ArrayList
  Dim blnInitialized As Boolean = False

  'Enumeration values passed to EnableAll & DisableAll subroutines which
  'indicates what is being setup thus determining which Buttons on the GUI
  'should be left enabled.
  Private Enum MySettingUpConstants
    SettingUpPMAlign = 0
    SettingUpCalib = 1
    SettingUpBlob = 2
    SettingLiveVideo = 99
  End Enum
  Dim SettingUpConstants As MySettingUpConstants
#End Region
#Region "Controls And Form Events"
  Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    InitializeTools()
  End Sub
  Private Sub frmCalibNPoint_closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
    If Not ImageFileTool Is Nothing Then ImageFileTool.Dispose()
    If Not AcqFifoTool Is Nothing Then AcqFifoTool.Dispose()
    If Not CalibNPointTool Is Nothing Then CalibNPointTool.Dispose()
    If Not BlobTool Is Nothing Then BlobTool.Dispose()
  End Sub


  Private Sub OpenFileButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenFileButton.Click
    ' Clear graphics, assuming a new image will be in the display once user
    ' completes either Live Video or Open File operation, therefore, graphics
    ' will be out of sync.
    CogDisplay1.InteractiveGraphics.Clear()
    CogDisplay1.StaticGraphics.Clear()

    ' "Live Video"  & "Stop Live" button when Frame Grabber option is selected.
    ' Using our EnableAll & DisableAll subroutine to force the user stop live
    ' video before doing anything else.

    If (FrameGrabberRadio.Checked) Then

      If (CogDisplay1.LiveDisplayRunning) Then

        CogDisplay1.StopLiveDisplay()
        EnableAll(MySettingUpConstants.SettingLiveVideo)

        ' Run the AcqFifoTool so that all of the sample app images get the last
        ' image from Live Video
        AcqFifoTool.Run()
      Else
        If (Not AcqFifoTool.[Operator] Is Nothing) Then

          CogDisplay1.StartLiveDisplay(AcqFifoTool.[Operator], False)
          DisableAll(MySettingUpConstants.SettingLiveVideo)

        End If

      End If
      Exit Sub
    End If
    ' "Open File" button when image file option is selected
    ' DrawingEnabled is used to simply hide the image while the Fit is performed.
    ' This prevents the image from being diplayed at the initial zoom factor prior
    ' to fit being called.


    OpenFileDialog1.ShowDialog()
    If (OpenFileDialog1.FileName <> "") Then

      ImageFileTool.[Operator].Open(OpenFileDialog1.FileName, _
      CogImageFileModeConstants.Read)
      CogDisplay1.DrawingEnabled = True
      ImageFileTool.Run()
      CogDisplay1.Fit(True)
      CogDisplay1.DrawingEnabled = True
    End If

  End Sub


  Private Sub NextImageButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NextImageButton.Click
    If (FrameGrabberRadio.Checked) Then
      AcqFifoTool.Run()
    Else
      ImageFileTool.Run()
    End If
  End Sub

  Private Sub FrameGrabberRadio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FrameGrabberRadio.CheckedChanged
    If (FrameGrabberRadio.Checked) Then

      OpenFileButton.Text = "Live Video"
      NextImageButton.Text = "New Image"

    Else

      OpenFileButton.Text = "Open File"
      NextImageButton.Text = "Next Image"
    End If
  End Sub

  Private Sub ImageFileRadio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImageFileRadio.CheckedChanged
    If (ImageFileRadio.Checked = False) Then

      OpenFileButton.Text = "Live Video"
      NextImageButton.Text = "New Image"

    Else

      OpenFileButton.Text = "Open File"
      NextImageButton.Text = "Next Image"
    End If
  End Sub

  Private Sub PMAlignSetupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PMAlignSetupButton.Click
    Dim DrawingWasEnabled As Boolean
    Dim axes As CogCoordinateAxes
    Dim CurrentTool As CogPMAlignTool
    Dim i As Integer

    CogDisplay1.StaticGraphics.Clear()
    CogDisplay1.InteractiveGraphics.Clear()
    ' PMAlign Setup button has been pressed, Entering SettingUp mode.
    If blnSettingUp = False Then

      ' If no image then display error and exit
      If (CogDisplay1.Image Is Nothing) Then

        MessageBox.Show("No Image For Training Features")
        Return
      End If

      ' While setting up PMAlign tools, disable other GUI controls.
      blnSettingUp = True
      DisableAll(MySettingUpConstants.SettingUpPMAlign)
      DrawingWasEnabled = CogDisplay1.DrawingEnabled
      CogDisplay1.DrawingEnabled = False

      ' Add PMAlign Pattern regions & origins to display's interactive graphics
      For i = 0 To NumPMAlignTools - 1


        CurrentTool = CType(PMAlignTools(i), CogPMAlignTool)
        CogDisplay1.InteractiveGraphics.Add(CType(CurrentTool.Pattern.TrainRegion, Cognex.VisionPro.ICogGraphicInteractive), _
      "main", False)
        ' Add an origin graphic with tip text to distinguish them
        axes = New CogCoordinateAxes
        axes.Transform = CurrentTool.Pattern.Origin
        axes.TipText = "PatMax Pattern Origin " + i.ToString()
        axes.GraphicDOFEnable = CogCoordinateAxesDOFConstants.All And Not _
         CogCoordinateAxesDOFConstants.Skew
        axes.Interactive = True
        ' Add a standard VisionPro "manipulable" mouse cursor.
        axes.MouseCursor = CogStandardCursorConstants.ManipulableGraphic
        axes.XAxisLabel.MouseCursor = CogStandardCursorConstants.ManipulableGraphic
        axes.YAxisLabel.MouseCursor = CogStandardCursorConstants.ManipulableGraphic
        CogDisplay1.InteractiveGraphics.Add(CType(axes, Cognex.VisionPro.ICogGraphicInteractive), "main", False)
      Next
      ' Re-enable drawing
      CogDisplay1.DrawingEnabled = DrawingWasEnabled
    Else
      ' OK button has been pressed, completing setup

      blnSettingUp = False
      CogDisplay1.InteractiveGraphics.Clear()
      CogDisplay1.StaticGraphics.Clear()

      Try

        For i = 0 To NumPMAlignTools - 1

          CurrentTool = CType(PMAlignTools(i), CogPMAlignTool)
          CurrentTool.Pattern.TrainImage = CurrentTool.InputImage
          CurrentTool.Pattern.Train()
        Next
        PMAlignRun()
        EnableAll(MySettingUpConstants.SettingUpPMAlign)

      Catch ce As Exception

        MessageBox.Show("Encountered exception: " + ce.Message)


      End Try
    End If

  End Sub

  Private Sub PMAlignRunButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PMAlignRunButton.Click
    CogDisplay1.InteractiveGraphics.Clear()
    CogDisplay1.StaticGraphics.Clear()
    PMAlignRun()
  End Sub

  Private Sub NumPMAlignToolsUpDown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumPMAlignToolsUpDown.ValueChanged
    If blnInitialized = False Then Exit Sub
    Dim CurrentPMAlignToolCount As Integer = CInt(NumPMAlignToolsUpDown.Value)
    If (CurrentPMAlignToolCount = NumPMAlignTools) Then
      Exit Sub
    End If
    ' remove display graphics
    CogDisplay1.InteractiveGraphics.Clear()
    CogDisplay1.StaticGraphics.Clear()

    ' update CalibNPoint Tool to have the correct number of points
    CalibNPointTool.Calibration.NumPoints = CurrentPMAlignToolCount

    ' Add new PMAlign tools as needed
    Do While (CurrentPMAlignToolCount > NumPMAlignTools)
      NumPMAlignTools = NumPMAlignTools + 1
      CreateNewPMAlignTool(NumPMAlignTools)

      ' Remove PMAlign tools if not needed
      Do While (CurrentPMAlignToolCount < NumPMAlignTools)


        ' remove tool from array
        PMAlignTools.RemoveAt(NumPMAlignTools - 1)
        NumPMAlignTools = NumPMAlignTools - 1
        ' remove ran event handler
        RemoveHandler CType(PMAlignTools(NumPMAlignTools - 1), CogPMAlignTool).Ran, AddressOf PMAlign_Ran
      Loop

      ' Handle case where we removed the currently selected
      ' PMAlign tool in the edit control

      If (PMAlignSelected >= NumPMAlignTools) Then

        PMAlignSelected = NumPMAlignTools - 1
        CogPMAlignEdit1.Subject = CType(PMAlignTools(PMAlignSelected), CogPMAlignTool)
      End If
    Loop
  End Sub

  Private Sub CalibSetupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalibSetupButton.Click

    CogDisplay1.InteractiveGraphics.Clear()
    CogDisplay1.StaticGraphics.Clear()

    CalibNPointTool.CalibrationImage = CalibNPointTool.InputImage

    ' Unfortunately, there is no way for the demo to guess calibrated points.
    ' The user will have to enter calibrated points in the point grid on the
    ' N-Point calibration tab before clicking calibrate.
    ' See comment at top of file for geometric information for bracket if you are
    ' running on that image.

    Try

      CalibNPointTool.Calibration.Calibrate()
    Catch ex As Exception

      MessageBox.Show("Encountered exception: " + ex.Message)

    End Try
  End Sub

  Private Sub CalibRunButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalibRunButton.Click

    CogDisplay1.StaticGraphics.Clear()
    CogDisplay1.InteractiveGraphics.Clear()
    CalibRun()
  End Sub



  Private Sub BlobRunButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlobRunButton.Click
    CogDisplay1.InteractiveGraphics.Clear()
    CogDisplay1.StaticGraphics.Clear()
    BlobRun()
  End Sub

  Private Sub RunAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunAllButton.Click
    Try

      CogDisplay1.DrawingEnabled = False
      RunningAll = True

      If (FrameGrabberRadio.Checked) Then
        AcqFifoTool.Run()
        CalibRun()
        BlobRun()
      Else
        ImageFileTool.Run()
        CalibRun()
        BlobRun()
      End If
    Catch ce As CogException

      MessageBox.Show("Encountered exception: " + ce.Message)

    Finally

      RunningAll = False
      CogDisplay1.DrawingEnabled = True

    End Try
  End Sub
  Private Sub PMAlignComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PMAlignComboBox.SelectedIndexChanged
    PMAlignSelected = PMAlignComboBox.SelectedIndex()
    CogPMAlignEdit1.Subject = PMAlignTools(PMAlignSelected)
  End Sub
#End Region
#Region "Module Level Helper Routines"
  ' This method creates the Nth PMAlign Tool
  Public Sub CreateNewPMAlignTool(ByVal ToolNumber As Integer)

        Dim ulx, uly As Integer
        Dim NewTool As New CogPMAlignTool
        If FrameGrabberRadio.Checked Then
            NewTool.InputImage = CType(AcqFifoTool.OutputImage, CogImage8Grey)
        Else
            NewTool.InputImage = CType(ImageFileTool.OutputImage, CogImage8Grey)
        End If

        ' Setup PMAlign tool.  Add tip text to train region to distinguish
        Dim ar As CogRectangleAffine
        ar = CType(NewTool.Pattern.TrainRegion, CogRectangleAffine)
        ar.TipText = "PMAlign Pattern Region " + ToolNumber.ToString
        ar.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position Or _
         CogRectangleAffineDOFConstants.Rotation Or _
         CogRectangleAffineDOFConstants.Size

        ' Stagger train regions so they aren't all piled up on one another
        ulx = 25 + 20 * (ToolNumber Mod 20)
        uly = 25 + 20 * (ToolNumber Mod 20)
        ar.SetOriginLengthsRotationSkew(uly, uly, 100, 100, 0, 0)

        'Setup a cursor indicating region is manipulable.
        ar.MouseCursor = CogStandardCursorConstants.ManipulableGraphic

        ' Default origin to center of region
        NewTool.Pattern.Origin.TranslationX = ar.CenterX
        NewTool.Pattern.Origin.TranslationY = ar.CenterY

        ' Set it up to find one instance of pattern at any angle.
        NewTool.RunParams.ApproximateNumberToFind = 1
        NewTool.RunParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh
        NewTool.RunParams.ZoneAngle.Low = -PI
        NewTool.RunParams.ZoneAngle.High = PI

        ' Add Ran event handler
        AddHandler NewTool.Ran, AddressOf PMAlign_Ran

        ' Add a selection to the PMAlignComboBox selector
        PMAlignComboBox.Items.Add("PMAlign " + ToolNumber.ToString())
        PMAlignTools.Add(NewTool)

  End Sub

  Private Sub InitializeTools()

    Dim myMeasure As CogBlobMeasure

    ' Get the auto-created tools
    ImageFileTool = CogImageFileEdit1.Subject
    AcqFifoTool = CogAcqFifoEditV21.Subject
    CalibNPointTool = cogCalibNPointEdit1.Subject
    Calib = CalibNPointTool.Calibration
    BlobTool = CogBlobEdit1.Subject

    ' AcqFifoToo operator will be Nothing if no Frame Grabber is available.  Disable the Frame
    ' Grabber option on the "VisionPro Demo" tab if no frame grabber available.
    If (AcqFifoTool.[Operator] Is Nothing) Then
      FrameGrabberRadio.Enabled = False
    End If
    ' Initialize the Open File Dialog box for the "Open File" button on the "VisionPro Demo" tab.
    OpenFileDialog1.Filter = ImageFileTool.[Operator].FilterText
    OpenFileDialog1.CheckFileExists = True
    OpenFileDialog1.ReadOnlyChecked = True

    ' AutoCreateTool for the PMAlign edit control is False, therefore, we must set the
    ' subject of the control.
    Dim i As Integer
        For i = 0 To NumPMAlignTools - 1
            CreateNewPMAlignTool(i)
        Next
        CogPMAlignEdit1.Subject = CType(PMAlignTools(0), CogPMAlignTool)
        PMAlignComboBox.SelectedIndex = 0

        ' Initialize calibration tool
        Calib.AddPointPair(0, 0, 0, 0)
        Calib.AddPointPair(1, 0, 50, 0)
        Calib.AddPointPair(0, 1, 0, 50)

        ' Add appropriate measurements to Blob Tool
        myMeasure = New CogBlobMeasure
        myMeasure.Measure = CogBlobMeasureConstants.Area
        BlobTool.RunParams.RunTimeMeasures.Add(myMeasure)

        myMeasure = New CogBlobMeasure
        myMeasure.Measure = CogBlobMeasureConstants.CenterMassX
        BlobTool.RunParams.RunTimeMeasures.Add(myMeasure)

        myMeasure = New CogBlobMeasure
        myMeasure.Measure = CogBlobMeasureConstants.CenterMassY
        BlobTool.RunParams.RunTimeMeasures.Add(myMeasure)

        myMeasure = New CogBlobMeasure
        myMeasure.Measure = CogBlobMeasureConstants.Perimeter
        BlobTool.RunParams.RunTimeMeasures.Add(myMeasure)

        RunningPMAlignTools = False
        RunningAll = False
        ' Add event handler to indicate when the ImageFile Tool,
        ' AcqFifo Tool, and Blob Tool have run.
        AddHandler ImageFileTool.Ran, AddressOf ImageFileTool_Ran
        AddHandler AcqFifoTool.Ran, AddressOf AcqFifoTool_Ran
        AddHandler BlobTool.Ran, AddressOf BlobTool_Ran

        ' Add a changed event handler for the CalibNPoint Tool and Calibration operator
        AddHandler CalibNPointTool.Ran, AddressOf CalibNPointTool_Ran
        AddHandler Calib.Changed, AddressOf Calib_Changed

  End Sub

  Private Sub EnableAll(ByVal Settings As MySettingUpConstants)

    RunAllButton.Enabled = True
    ' Enable all of the groups (Enables controls within group)
    GroupAcq.Enabled = True
    GroupPMAlign.Enabled = True
    GroupCalib.Enabled = True
    GroupBlob.Enabled = True
    ' Enable all of the tabs except "VisionPro Demo" tab.
    Dim i As Integer
    For i = 1 To 5
      Dim c As Control
      For Each c In TabControl1.TabPages(i).Controls
        c.Enabled = True
      Next
    Next
    ' Based on what the user is doing, Re-enable appropriate groups and disable
    ' specific controls within the group.
        If (Settings = MySettingUpConstants.SettingUpPMAlign) Then

            GroupPMAlign.Enabled = True
            PMAlignSetupButton.Text = "Setup"
            PMAlignRunButton.Enabled = True

        ElseIf (Settings = MySettingUpConstants.SettingLiveVideo) Then

            GroupAcq.Enabled = True
            OpenFileButton.Text = "Live Video"
            NextImageButton.Enabled = True
            FrameGrabberRadio.Enabled = True
            ImageFileRadio.Enabled = True
        End If

  End Sub

  ' Disable GUI controls when forcing the user to complete a task before moving on
  ' to something new.  Example, Setting up PMAlign.
  Private Sub DisableAll(ByVal Settings As MySettingUpConstants)

    RunAllButton.Enabled = False
    ' Disable all of the groups (Disables controls within group)
    GroupAcq.Enabled = False
    GroupPMAlign.Enabled = False
    GroupCalib.Enabled = False
    GroupBlob.Enabled = False
    ' Disable all of the tabs except "VisionPro Demo" tab.
    Dim i As Integer
    For i = 1 To 5
      Dim c As Control
      For Each c In TabControl1.TabPages(i).Controls
        c.Enabled = False
      Next
    Next
    ' Based on what the user is doing, Re-enable appropriate groups and disable
    ' specific controls within the group.
        If (Settings = MySettingUpConstants.SettingUpPMAlign) Then

            GroupPMAlign.Enabled = True
            PMAlignSetupButton.Text = "OK"
            PMAlignRunButton.Enabled = False

        ElseIf (Settings = MySettingUpConstants.SettingLiveVideo) Then

            GroupAcq.Enabled = True
            OpenFileButton.Text = "Stop Live"
            NextImageButton.Enabled = False
            FrameGrabberRadio.Enabled = False
            ImageFileRadio.Enabled = False
        End If

  End Sub
  Private Sub BlobRun()
    BlobTool.Run()
    If (BlobTool.RunStatus.Result = CogToolResultConstants.Error) Then
      MessageBox.Show(BlobTool.RunStatus.Message)
    End If
  End Sub
  Private Sub CalibRun()
    ' Run the Calib tool to stuff the calibrated space into the image
    Try

      CalibNPointTool.Run()
      If (CalibNPointTool.RunStatus.Result = CogToolResultConstants.Error) Then
        MessageBox.Show(CalibNPointTool.RunStatus.Message)
      End If
    Catch ce As Exception

      MessageBox.Show("Encountered exception: " + ce.Message)
    End Try
  End Sub
  Sub PMAlignRun()
    Dim CurrentTool As CogPMAlignTool
    Dim Result As CogPMAlignResult
    Dim DrawingWasEnabled As Boolean
    DrawingWasEnabled = CogDisplay1.DrawingEnabled
    Try

      CogDisplay1.DrawingEnabled = False
      RunningPMAlignTools = True
      Dim i As Integer
            For i = 0 To NumPMAlignTools - 1

                CurrentTool = CType(PMAlignTools(i), CogPMAlignTool)
                CurrentTool.Run()
                If (CurrentTool.RunStatus.Result = CogToolResultConstants.Error) Then
                    MessageBox.Show(CurrentTool.RunStatus.Message)
                Else

                    If (CurrentTool.Results.Count = 0) Then
                        MessageBox.Show("Feature number " + i.ToString() + " not found.")
                    ElseIf (CurrentTool.Results.Count > 1) Then
                        MessageBox.Show("Feature number " + i.ToString() + " found too many times.")
                    Else

                        ' update corresponding point in calibration tool.
                        ' NOTE: THIS WILL UNCALIBRATE CALIBRATION TOOL.
                        Result = CurrentTool.Results(0)
                        CalibNPointTool.Calibration.SetUncalibratedPointX(i, _
                    Result.GetPose().TranslationX)
                        CalibNPointTool.Calibration.SetUncalibratedPointY(i, _
                         Result.GetPose().TranslationY)
                        CogDisplay1.InteractiveGraphics.Add(Result.CreateResultGraphics(CogPMAlignResultGraphicConstants.Origin), _
                    "main", False)
                    End If
                End If

            Next
        Catch ce As Exception

      MessageBox.Show("Encountered exception: " + ce.Message)

    Finally

      RunningPMAlignTools = False
      CogDisplay1.DrawingEnabled = DrawingWasEnabled

    End Try
  End Sub

#End Region
#Region "Cognex Tools Events"

    Private Sub AcqFifoTool_Ran(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Note that we invoke the garbage collector every 5th acquisition.
        Static numAcqs As Integer
        numAcqs = numAcqs + 1
        If (numAcqs > 4) Then

            GC.Collect()
            numAcqs = 0
        End If

        CogDisplay1.StaticGraphics.Clear()
        CogDisplay1.InteractiveGraphics.Clear()
        CogDisplay1.Image = AcqFifoTool.OutputImage
        CogDisplay1.Fit(True)
        Dim i As Integer
        For i = 0 To NumPMAlignTools - 1
            CType(PMAlignTools(i), CogPMAlignTool).InputImage = CType(AcqFifoTool.OutputImage, CogImage8Grey)
            CalibNPointTool.InputImage = AcqFifoTool.OutputImage
        Next
    End Sub

    Private Sub PatMaxTool_Ran(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles PatMaxTool.Ran
        Dim RanTool As CogPMAlignTool
        RanTool = CType(sender, CogPMAlignTool)
        If RunningPMAlignTools Then Exit Sub

        CogDisplay1.InteractiveGraphics.Clear()
        CogDisplay1.StaticGraphics.Clear()
        If (Not RanTool.Results Is Nothing) Then

            If (RanTool.Results.Count = 1) Then

                ' update corresponding point in calibration tool.
                ' NOTE: THIS WILL UNCALIBRATE CALIBRATION TOOL.
                CalibNPointTool.Calibration.SetUncalibratedPointX(PMAlignSelected, _
                 RanTool.Results(0).GetPose().TranslationX)
                CalibNPointTool.Calibration.SetUncalibratedPointY(PMAlignSelected, _
                 RanTool.Results(0).GetPose().TranslationY)

            End If
        End If
    End Sub

    Private Sub ImageFileTool_Ran(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles ImageFileTool.Ran
        CogDisplay1.StaticGraphics.Clear()
        CogDisplay1.InteractiveGraphics.Clear()
        CogDisplay1.Image = ImageFileTool.OutputImage
        CogDisplay1.Fit(True)
        Dim i As Integer
        For i = 0 To NumPMAlignTools - 1
            CType(PMAlignTools(i), CogPMAlignTool).InputImage = CType(ImageFileTool.OutputImage, CogImage8Grey)
            CalibNPointTool.InputImage = ImageFileTool.OutputImage
        Next
    End Sub

    Private Sub Calib_Changed(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogChangedEventArgs) 'Handles Calib.Changed
        Dim axes As CogCoordinateAxes = New CogCoordinateAxes
        If ((e.StateFlags And CogCalibNPointToNPoint.SfCalibrated) > 0) Then

            CogDisplay1.InteractiveGraphics.Clear()
            CogDisplay1.StaticGraphics.Clear()
            ' Set application state based on calibrated state (label, graphics, etc)

            If (CalibNPointTool.Calibration.Calibrated) Then

                CalibratedLabel.Text = "Calibrated"
                CalibratedLabel.ForeColor = System.Drawing.Color.Green

                ' When going calibrated, show calibration axes in display on demo tab
                axes.Transform = CType(CalibNPointTool.Calibration.GetComputedUncalibratedFromCalibratedTransform, CogTransform2DLinear)
                axes.Color = CogColorConstants.Green
                axes.XAxisLabel.Color = CogColorConstants.Green
                axes.YAxisLabel.Color = CogColorConstants.Green
                axes.DisplayedXAxisLength = 50
                CogDisplay1.InteractiveGraphics.Add(CType(axes, Cognex.VisionPro.ICogGraphicInteractive), "main", False)

            Else

                ' When uncalibrating, set blob's input to null to prevent it from running
                ' on old calibration/image data
                BlobTool.InputImage = Nothing
                CalibratedLabel.Text = "Uncalibrated"
                CalibratedLabel.ForeColor = System.Drawing.Color.Red

            End If

        End If

    End Sub

    Private Sub BlobTool_Ran(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles BlobTool.Ran
        Dim ResultGraphics As CogGraphicInteractiveCollection = New CogGraphicInteractiveCollection
        If (RunningAll = False) Then

            CogDisplay1.InteractiveGraphics.Clear()
            CogDisplay1.StaticGraphics.Clear()
        End If
        If (BlobTool.Results Is Nothing) Then
            BlobCountText.Text = "N/A"
        Else

            BlobCountText.Text = BlobTool.Results.GetBlobs(True).Count.ToString()
            BlobCountText.Refresh()
            Dim BlobResult As CogBlobResult
            For Each BlobResult In BlobTool.Results.GetBlobs(True)
                ResultGraphics.Add(BlobResult.CreateResultGraphics(CogBlobResultGraphicConstants.Boundary Or _
              CogBlobResultGraphicConstants.TipText))
                CogDisplay1.InteractiveGraphics.AddList(ResultGraphics, "main", False)
            Next
        End If
    End Sub

    Private Sub CalibNPointTool_Ran(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles CalibNPointTool.Ran
        ' Make blob run in calibrated space. Running the calibration tool will
        'add the calibrated space to the image for blob to run.
        'NOTE: its input region, if any, will need to setup in calibrated space AFTER calibrating

        Dim axes As CogCoordinateAxes
        BlobTool.InputImage = CType(CalibNPointTool.OutputImage, CogImage8Grey)

        If (RunningAll = False) Then

            CogDisplay1.StaticGraphics.Clear()
            CogDisplay1.InteractiveGraphics.Clear()
        End If

        ' if we are calibrated, then show the calibrated space on run
        If (CalibNPointTool.Calibration.Calibrated) Then

            axes = New CogCoordinateAxes
            axes.Transform = CType(CalibNPointTool.Calibration.GetComputedUncalibratedFromCalibratedTransform, CogTransform2DLinear)
            axes.Color = CogColorConstants.Green
            axes.XAxisLabel.Color = CogColorConstants.Green
            axes.YAxisLabel.Color = CogColorConstants.Green
            axes.DisplayedXAxisLength = 50

            CogDisplay1.InteractiveGraphics.Add(axes, "main", False)
        End If

    End Sub

    Private Sub PMAlign_Ran(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim RanTool As CogPMAlignTool
        RanTool = sender
        If RunningPMAlignTools = True Then Exit Sub

        CogDisplay1.InteractiveGraphics.Clear()
        CogDisplay1.StaticGraphics.Clear()
        If (Not RanTool.Results Is Nothing) Then

            If (RanTool.Results.Count = 1) Then

                ' update corresponding point in calibration tool.
                ' NOTE: THIS WILL UNCALIBRATE CALIBRATION TOOL.
                CalibNPointTool.Calibration.SetUncalibratedPointX(PMAlignSelected, _
              RanTool.Results(0).GetPose().TranslationX)
                CalibNPointTool.Calibration.SetUncalibratedPointY(PMAlignSelected, _
                 RanTool.Results(0).GetPose().TranslationY)
            End If
        End If
    End Sub
#End Region

End Class