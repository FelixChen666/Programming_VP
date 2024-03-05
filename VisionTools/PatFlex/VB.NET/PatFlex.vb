'*******************************************************************************
'Copyright (C) 2003 Cognex Corporation
'
'Subject to Cognex Corporation's terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.

' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This sample demonstrates the use of PatFlex to train and locate a pattern 
' in a user provided image. The user should execute the following steps:
' 1. Select an image source: either an image file or a frame grabber 
'    Something like PatFlex.idb in the Images directory will work. 
' 2. Grab an image from the image source
' 3. Click 'Setup'.  Select a training region. Hit 'OK'.
' 4. Click 'Run' to see the location result and score.
' 5. Click 'Next Image' followed by 'Run' to locate the pattern on a subsequent image
'
' Note that execution parameters can be changed by selecting the appropriate tab and
' modifying the provided values.
'*******************************************************************************
Option Explicit On 

Imports Cognex.VisionPro                   'used by VisionPro basic functionality
Imports Cognex.VisionPro.Exceptions        'used by VisionPro exceptions
Imports Cognex.VisionPro.Display           'used by CogDisplay
Imports Cognex.VisionPro.ImageFile         'used by CogImageFile
Imports Cognex.VisionPro.ImageProcessing   'used by CogCopyRegionTool
Imports Cognex.VisionPro.PMAlign           'used by CogPMAlignTool

Namespace PatFlexSampleCode
  Public Class PatFlexForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      '
      ' initialize tools we use in the sample
      '
      txtDescription.Text = "This sample demonstrates the use of PatFlex to train " & _
                "and locate a pattern in a user provided image." & Environment.NewLine & Environment.NewLine & _
                "The user should execute the following steps:" & Environment.NewLine & _
                "1. Select an image source: either an image file or a frame grabber." & Environment.NewLine & _
                "   Something like PatFlex.idb in the Images directory will work." & Environment.NewLine & _
                "2. Grab an image from the image source." & Environment.NewLine & _
                "3. Click 'Setup'.  Select a training region. Hit 'OK'." & Environment.NewLine & _
                "4. Click 'Run' to see the location result and score." & Environment.NewLine & _
                "5. Click 'Next Image' followed by 'Run' to locate the pattern on a subsequent image." & Environment.NewLine & Environment.NewLine & _
                "Note that execution parameters can be changed by selecting the appropriate tab and " & _
                "modifying the provided values."

      'Set reference to CogImageFileTool created by Edit Control
      'The Image File Edit Control creates its subject when its AutoCreateTool property is true
      mImageFileTool = mImageFileEdit.Subject
      AddHandler mImageFileTool.Ran, AddressOf mImageFileTool_Ran

      'Initialize the Dialog box for the "Open File" button on the "VisionPro Demo" tab.
      mOpenFileDialog.Filter = mImageFileTool.[Operator].FilterText
      mOpenFileDialog.CheckFileExists = True

      'Set reference to CogmFifoTool created by Edit Control
      'The Acq Fifo Edit Control creates its subject when its AutoCreateTool property is true
      mFifoTool = mAcqFifoEdit.Subject
      AddHandler mFifoTool.Ran, AddressOf mFifoTool_Ran

      'Operator will be null if no Frame Grabber is available.  Disable the Frame Grabber
      'option on the "VisionPro Demo" tab if no frame grabber available.
      If mFifoTool.[Operator] Is Nothing Then optFrameGrabber.Enabled = False

      ' default option of image source is ImageFile
      optImageFile.Checked = True

      'AutoCreateTool for the PMAlign edit control is False, therefore, we must create
      'a PMAlign tool and set the subject of the control to reference the new tool.
      mPMAlignTool = New CogPMAlignTool
      mPMAlignEdit.Subject = mPMAlignTool

      'Change the default Train Region to center of a 640x480 image & change the DOFs
      'so that Skew is not enabled.  Note - TrainRegion is of type ICogRegion, therefore,
      'we must use a CogRectangleAffine reference in order to call CogRectangleAffine
      'properties.
      Dim mPMTrainRegion As CogRectangleAffine = CType(mPMAlignTool.Pattern.TrainRegion, CogRectangleAffine)
      If Not mPMTrainRegion Is Nothing Then
        mPMTrainRegion.SetCenterLengthsRotationSkew(320, 240, 100, 100, 0, 0)
        mPMTrainRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position Or _
            CogRectangleAffineDOFConstants.Rotation Or CogRectangleAffineDOFConstants.Size
      End If
      'Create a SearchRegion that uses the entire image (assumes 640x480)
      'Note that by default the SearchRegion is Nothing and PMAlign will search the entire
      'image anyway.  This is added for sample code purposes & to graphically show that the
      'entire image is being used.
      Dim mPMSearchRegion As CogRectangle = New CogRectangle
      'Establish an Region Of Interest (ROI) to let the user manipulate during training.
      mPMSearchRegion.SetCenterWidthHeight(320, 240, 640, 480)
      mPMSearchRegion.GraphicDOFEnable = CogRectangleDOFConstants.Size Or CogRectangleDOFConstants.Position
      mPMSearchRegion.Interactive = True
      mPMAlignTool.SearchRegion = mPMSearchRegion

      'Set up the mPMAlignTool to use the PatFlex algorithm.
      mPMAlignTool.Pattern.TrainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatFlex

      ' Sink to mPMAlignTool's Change event
      AddHandler mPMAlignTool.Changed, AddressOf mPMAlignTool_Changed

      'Add a cog Copy Region tool so that we can display only the region that was
      'unwarped by the PatFex algorithm.  This is really the only region that
      'uses the unwarp transform in a valid fashion.  The rest of the image appears
      'unwarped, but it's just borrowing the unwarp transform and is not really
      'correct in a mathematical sense.
      mRegionTool = New CogCopyRegionTool
      mCopyRegionEdit.Subject = mRegionTool
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
      If disposing Then
        ' we created mRegionTool and PMAlign Tool, so we need to dispose them too
        If Not mRegionTool Is Nothing Then mRegionTool.Dispose()
        If Not mPMAlignTool Is Nothing Then
          ' unsink the change event before we dispose the tool
          RemoveHandler mPMAlignTool.Changed, AddressOf mPMAlignTool_Changed
          mPMAlignTool.Dispose()
        End If
        ' mFifoTool and mImageFileTool are from the control. We leave them up to the
        ' control to release them
        If Not mImageFileTool Is Nothing Then
          mImageFileTool.[Operator].Close()  'close opened image file
          RemoveHandler mImageFileTool.Ran, AddressOf mImageFileTool_Ran 'unsink the event
        End If
        If Not mFifoTool Is Nothing Then
          RemoveHandler mFifoTool.Ran, AddressOf mFifoTool_Ran 'unsink the event
        End If

        'Releasing framegrabbers
        If (disposing) Then

          Dim frameGrabbers As New CogFrameGrabbers()
          For i As Integer = 0 To frameGrabbers.Count - 1
            frameGrabbers(i).Disconnect(False)
          Next
        End If

        If Not (components Is Nothing) Then
          components.Dispose()
        End If
      End If
      MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified imports the Windows Form Designer.  
    'Do not modify it imports the code editor.
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents tabSamples As System.Windows.Forms.TabControl
    Friend WithEvents tabDemo As System.Windows.Forms.TabPage
    Friend WithEvents tabFrameGrabber As System.Windows.Forms.TabPage
    Friend WithEvents tabImageFile As System.Windows.Forms.TabPage
    Friend WithEvents tabPatFlex As System.Windows.Forms.TabPage
    Friend WithEvents tabValidRegion As System.Windows.Forms.TabPage
    Friend WithEvents grpImageSource As System.Windows.Forms.GroupBox
    Friend WithEvents btnNextImage As System.Windows.Forms.Button
    Friend WithEvents optImageFile As System.Windows.Forms.RadioButton
    Friend WithEvents optFrameGrabber As System.Windows.Forms.RadioButton
    Friend WithEvents btnOpenFile As System.Windows.Forms.Button
    Friend WithEvents grpFlexDemo As System.Windows.Forms.GroupBox
    Friend WithEvents lblScore As System.Windows.Forms.Label
    Friend WithEvents llbScoreTitle As System.Windows.Forms.Label
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents btnSetup As System.Windows.Forms.Button
    Friend WithEvents mDisplayUnwarpedResult As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents mDisplayDistortionResult As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents lblUnwarpedResultDisplay As System.Windows.Forms.Label
    Friend WithEvents lblPatFlexResultDisplay As System.Windows.Forms.Label
    Friend WithEvents mImageFileEdit As Cognex.VisionPro.ImageFile.CogImageFileEditV2
    Friend WithEvents mPMAlignEdit As Cognex.VisionPro.PMAlign.CogPMAlignEditV2
    Friend WithEvents mCopyRegionEdit As Cognex.VisionPro.ImageProcessing.CogCopyRegionEditV2
    Friend WithEvents mAcqFifoEdit As Cognex.VisionPro.CogAcqFifoEditV2
    Friend WithEvents mOpenFileDialog As System.Windows.Forms.OpenFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PatFlexForm))
            Me.txtDescription = New System.Windows.Forms.TextBox()
            Me.tabSamples = New System.Windows.Forms.TabControl()
            Me.tabDemo = New System.Windows.Forms.TabPage()
            Me.mDisplayUnwarpedResult = New Cognex.VisionPro.Display.CogDisplay()
            Me.mDisplayDistortionResult = New Cognex.VisionPro.Display.CogDisplay()
            Me.lblUnwarpedResultDisplay = New System.Windows.Forms.Label()
            Me.lblPatFlexResultDisplay = New System.Windows.Forms.Label()
            Me.grpFlexDemo = New System.Windows.Forms.GroupBox()
            Me.lblScore = New System.Windows.Forms.Label()
            Me.llbScoreTitle = New System.Windows.Forms.Label()
            Me.btnRun = New System.Windows.Forms.Button()
            Me.btnSetup = New System.Windows.Forms.Button()
            Me.grpImageSource = New System.Windows.Forms.GroupBox()
            Me.btnNextImage = New System.Windows.Forms.Button()
            Me.optImageFile = New System.Windows.Forms.RadioButton()
            Me.optFrameGrabber = New System.Windows.Forms.RadioButton()
            Me.btnOpenFile = New System.Windows.Forms.Button()
            Me.tabFrameGrabber = New System.Windows.Forms.TabPage()
            Me.mAcqFifoEdit = New Cognex.VisionPro.CogAcqFifoEditV2()
            Me.tabImageFile = New System.Windows.Forms.TabPage()
            Me.mImageFileEdit = New Cognex.VisionPro.ImageFile.CogImageFileEditV2()
            Me.tabPatFlex = New System.Windows.Forms.TabPage()
            Me.mPMAlignEdit = New Cognex.VisionPro.PMAlign.CogPMAlignEditV2()
            Me.tabValidRegion = New System.Windows.Forms.TabPage()
            Me.mCopyRegionEdit = New Cognex.VisionPro.ImageProcessing.CogCopyRegionEditV2()
            Me.mOpenFileDialog = New System.Windows.Forms.OpenFileDialog()
            Me.tabSamples.SuspendLayout()
            Me.tabDemo.SuspendLayout()
            CType(Me.mDisplayUnwarpedResult, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.mDisplayDistortionResult, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpFlexDemo.SuspendLayout()
            Me.grpImageSource.SuspendLayout()
            Me.tabFrameGrabber.SuspendLayout()
            CType(Me.mAcqFifoEdit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabImageFile.SuspendLayout()
            CType(Me.mImageFileEdit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabPatFlex.SuspendLayout()
            CType(Me.mPMAlignEdit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabValidRegion.SuspendLayout()
            CType(Me.mCopyRegionEdit, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'txtDescription
            '
            Me.txtDescription.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.txtDescription.Location = New System.Drawing.Point(0, 545)
            Me.txtDescription.Multiline = True
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.ReadOnly = True
            Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.txtDescription.Size = New System.Drawing.Size(898, 92)
            Me.txtDescription.TabIndex = 7
            '
            'tabSamples
            '
            Me.tabSamples.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tabSamples.Controls.Add(Me.tabDemo)
            Me.tabSamples.Controls.Add(Me.tabFrameGrabber)
            Me.tabSamples.Controls.Add(Me.tabImageFile)
            Me.tabSamples.Controls.Add(Me.tabPatFlex)
            Me.tabSamples.Controls.Add(Me.tabValidRegion)
            Me.tabSamples.Location = New System.Drawing.Point(0, 0)
            Me.tabSamples.Name = "tabSamples"
            Me.tabSamples.SelectedIndex = 0
            Me.tabSamples.Size = New System.Drawing.Size(896, 539)
            Me.tabSamples.TabIndex = 8
            '
            'tabDemo
            '
            Me.tabDemo.Controls.Add(Me.mDisplayUnwarpedResult)
            Me.tabDemo.Controls.Add(Me.mDisplayDistortionResult)
            Me.tabDemo.Controls.Add(Me.lblUnwarpedResultDisplay)
            Me.tabDemo.Controls.Add(Me.lblPatFlexResultDisplay)
            Me.tabDemo.Controls.Add(Me.grpFlexDemo)
            Me.tabDemo.Controls.Add(Me.grpImageSource)
            Me.tabDemo.Location = New System.Drawing.Point(4, 22)
            Me.tabDemo.Name = "tabDemo"
            Me.tabDemo.Size = New System.Drawing.Size(888, 513)
            Me.tabDemo.TabIndex = 0
            Me.tabDemo.Text = "VisionPro Demo"
            '
            'mDisplayUnwarpedResult
            '
            Me.mDisplayUnwarpedResult.ColorMapLowerClipColor = System.Drawing.Color.Black
            Me.mDisplayUnwarpedResult.ColorMapLowerRoiLimit = 0.0R
            Me.mDisplayUnwarpedResult.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None
            Me.mDisplayUnwarpedResult.ColorMapUpperClipColor = System.Drawing.Color.Black
            Me.mDisplayUnwarpedResult.ColorMapUpperRoiLimit = 1.0R
            Me.mDisplayUnwarpedResult.Location = New System.Drawing.Point(440, 120)
            Me.mDisplayUnwarpedResult.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
            Me.mDisplayUnwarpedResult.MouseWheelSensitivity = 1.0R
            Me.mDisplayUnwarpedResult.Name = "mDisplayUnwarpedResult"
            Me.mDisplayUnwarpedResult.OcxState = CType(resources.GetObject("mDisplayUnwarpedResult.OcxState"), System.Windows.Forms.AxHost.State)
            Me.mDisplayUnwarpedResult.Size = New System.Drawing.Size(408, 384)
            Me.mDisplayUnwarpedResult.TabIndex = 9
            '
            'mDisplayDistortionResult
            '
            Me.mDisplayDistortionResult.ColorMapLowerClipColor = System.Drawing.Color.Black
            Me.mDisplayDistortionResult.ColorMapLowerRoiLimit = 0.0R
            Me.mDisplayDistortionResult.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None
            Me.mDisplayDistortionResult.ColorMapUpperClipColor = System.Drawing.Color.Black
            Me.mDisplayDistortionResult.ColorMapUpperRoiLimit = 1.0R
            Me.mDisplayDistortionResult.Location = New System.Drawing.Point(8, 120)
            Me.mDisplayDistortionResult.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
            Me.mDisplayDistortionResult.MouseWheelSensitivity = 1.0R
            Me.mDisplayDistortionResult.Name = "mDisplayDistortionResult"
            Me.mDisplayDistortionResult.OcxState = CType(resources.GetObject("mDisplayDistortionResult.OcxState"), System.Windows.Forms.AxHost.State)
            Me.mDisplayDistortionResult.Size = New System.Drawing.Size(408, 384)
            Me.mDisplayDistortionResult.TabIndex = 8
            '
            'lblUnwarpedResultDisplay
            '
            Me.lblUnwarpedResultDisplay.Location = New System.Drawing.Point(496, 92)
            Me.lblUnwarpedResultDisplay.Name = "lblUnwarpedResultDisplay"
            Me.lblUnwarpedResultDisplay.Size = New System.Drawing.Size(288, 23)
            Me.lblUnwarpedResultDisplay.TabIndex = 7
            Me.lblUnwarpedResultDisplay.Text = "Unwarped PatFlex Result Displayed"
            Me.lblUnwarpedResultDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblPatFlexResultDisplay
            '
            Me.lblPatFlexResultDisplay.Location = New System.Drawing.Point(68, 92)
            Me.lblPatFlexResultDisplay.Name = "lblPatFlexResultDisplay"
            Me.lblPatFlexResultDisplay.Size = New System.Drawing.Size(288, 23)
            Me.lblPatFlexResultDisplay.TabIndex = 6
            Me.lblPatFlexResultDisplay.Text = "PatFlex Result with Distortion Field Displayed"
            Me.lblPatFlexResultDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'grpFlexDemo
            '
            Me.grpFlexDemo.Controls.Add(Me.lblScore)
            Me.grpFlexDemo.Controls.Add(Me.llbScoreTitle)
            Me.grpFlexDemo.Controls.Add(Me.btnRun)
            Me.grpFlexDemo.Controls.Add(Me.btnSetup)
            Me.grpFlexDemo.Location = New System.Drawing.Point(436, 8)
            Me.grpFlexDemo.Name = "grpFlexDemo"
            Me.grpFlexDemo.Size = New System.Drawing.Size(376, 76)
            Me.grpFlexDemo.TabIndex = 2
            Me.grpFlexDemo.TabStop = False
            Me.grpFlexDemo.Text = "PatFlex Demo"
            '
            'lblScore
            '
            Me.lblScore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblScore.Location = New System.Drawing.Point(280, 32)
            Me.lblScore.Name = "lblScore"
            Me.lblScore.Size = New System.Drawing.Size(64, 24)
            Me.lblScore.TabIndex = 3
            Me.lblScore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'llbScoreTitle
            '
            Me.llbScoreTitle.Location = New System.Drawing.Point(220, 32)
            Me.llbScoreTitle.Name = "llbScoreTitle"
            Me.llbScoreTitle.Size = New System.Drawing.Size(52, 23)
            Me.llbScoreTitle.TabIndex = 2
            Me.llbScoreTitle.Text = "Score"
            Me.llbScoreTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'btnRun
            '
            Me.btnRun.Location = New System.Drawing.Point(116, 28)
            Me.btnRun.Name = "btnRun"
            Me.btnRun.Size = New System.Drawing.Size(72, 32)
            Me.btnRun.TabIndex = 1
            Me.btnRun.Text = "Run"
            '
            'btnSetup
            '
            Me.btnSetup.Location = New System.Drawing.Point(24, 28)
            Me.btnSetup.Name = "btnSetup"
            Me.btnSetup.Size = New System.Drawing.Size(76, 32)
            Me.btnSetup.TabIndex = 0
            Me.btnSetup.Text = "Setup"
            '
            'grpImageSource
            '
            Me.grpImageSource.Controls.Add(Me.btnNextImage)
            Me.grpImageSource.Controls.Add(Me.optImageFile)
            Me.grpImageSource.Controls.Add(Me.optFrameGrabber)
            Me.grpImageSource.Controls.Add(Me.btnOpenFile)
            Me.grpImageSource.Location = New System.Drawing.Point(40, 8)
            Me.grpImageSource.Name = "grpImageSource"
            Me.grpImageSource.Size = New System.Drawing.Size(376, 76)
            Me.grpImageSource.TabIndex = 1
            Me.grpImageSource.TabStop = False
            Me.grpImageSource.Text = "Image Acquisition"
            '
            'btnNextImage
            '
            Me.btnNextImage.Location = New System.Drawing.Point(264, 28)
            Me.btnNextImage.Name = "btnNextImage"
            Me.btnNextImage.Size = New System.Drawing.Size(88, 32)
            Me.btnNextImage.TabIndex = 2
            Me.btnNextImage.Text = "Next Image"
            '
            'optImageFile
            '
            Me.optImageFile.Location = New System.Drawing.Point(16, 44)
            Me.optImageFile.Name = "optImageFile"
            Me.optImageFile.Size = New System.Drawing.Size(128, 24)
            Me.optImageFile.TabIndex = 1
            Me.optImageFile.Text = "Image File"
            '
            'optFrameGrabber
            '
            Me.optFrameGrabber.Location = New System.Drawing.Point(16, 20)
            Me.optFrameGrabber.Name = "optFrameGrabber"
            Me.optFrameGrabber.Size = New System.Drawing.Size(128, 24)
            Me.optFrameGrabber.TabIndex = 0
            Me.optFrameGrabber.Text = "Frame Grabber"
            '
            'btnOpenFile
            '
            Me.btnOpenFile.Location = New System.Drawing.Point(160, 28)
            Me.btnOpenFile.Name = "btnOpenFile"
            Me.btnOpenFile.Size = New System.Drawing.Size(88, 32)
            Me.btnOpenFile.TabIndex = 3
            Me.btnOpenFile.Text = "Open File"
            '
            'tabFrameGrabber
            '
            Me.tabFrameGrabber.Controls.Add(Me.mAcqFifoEdit)
            Me.tabFrameGrabber.Location = New System.Drawing.Point(4, 22)
            Me.tabFrameGrabber.Name = "tabFrameGrabber"
            Me.tabFrameGrabber.Size = New System.Drawing.Size(888, 513)
            Me.tabFrameGrabber.TabIndex = 1
            Me.tabFrameGrabber.Text = "Frame Grabber"
            '
            'mAcqFifoEdit
            '
            Me.mAcqFifoEdit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.mAcqFifoEdit.Location = New System.Drawing.Point(0, 0)
            Me.mAcqFifoEdit.MinimumSize = New System.Drawing.Size(489, 0)
            Me.mAcqFifoEdit.Name = "mAcqFifoEdit"
            Me.mAcqFifoEdit.Size = New System.Drawing.Size(888, 513)
            Me.mAcqFifoEdit.SuspendElectricRuns = False
            Me.mAcqFifoEdit.TabIndex = 0
            '
            'tabImageFile
            '
            Me.tabImageFile.Controls.Add(Me.mImageFileEdit)
            Me.tabImageFile.Location = New System.Drawing.Point(4, 22)
            Me.tabImageFile.Name = "tabImageFile"
            Me.tabImageFile.Size = New System.Drawing.Size(888, 513)
            Me.tabImageFile.TabIndex = 2
            Me.tabImageFile.Text = "Image File"
            '
            'mImageFileEdit
            '
            Me.mImageFileEdit.AllowDrop = True
            Me.mImageFileEdit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.mImageFileEdit.Location = New System.Drawing.Point(0, 0)
            Me.mImageFileEdit.MinimumSize = New System.Drawing.Size(489, 0)
            Me.mImageFileEdit.Name = "mImageFileEdit"
            Me.mImageFileEdit.OutputHighLight = System.Drawing.Color.Lime
            Me.mImageFileEdit.Size = New System.Drawing.Size(888, 513)
            Me.mImageFileEdit.SuspendElectricRuns = False
            Me.mImageFileEdit.TabIndex = 0
            '
            'tabPatFlex
            '
            Me.tabPatFlex.Controls.Add(Me.mPMAlignEdit)
            Me.tabPatFlex.Location = New System.Drawing.Point(4, 22)
            Me.tabPatFlex.Name = "tabPatFlex"
            Me.tabPatFlex.Size = New System.Drawing.Size(888, 513)
            Me.tabPatFlex.TabIndex = 3
            Me.tabPatFlex.Text = "PatFlex"
            '
            'mPMAlignEdit
            '
            Me.mPMAlignEdit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.mPMAlignEdit.Location = New System.Drawing.Point(0, 0)
            Me.mPMAlignEdit.MinimumSize = New System.Drawing.Size(489, 0)
            Me.mPMAlignEdit.Name = "mPMAlignEdit"
            Me.mPMAlignEdit.Size = New System.Drawing.Size(888, 513)
            Me.mPMAlignEdit.SuspendElectricRuns = False
            Me.mPMAlignEdit.TabIndex = 0
            '
            'tabValidRegion
            '
            Me.tabValidRegion.Controls.Add(Me.mCopyRegionEdit)
            Me.tabValidRegion.Location = New System.Drawing.Point(4, 22)
            Me.tabValidRegion.Name = "tabValidRegion"
            Me.tabValidRegion.Size = New System.Drawing.Size(888, 513)
            Me.tabValidRegion.TabIndex = 4
            Me.tabValidRegion.Text = "Valid Region"
            '
            'mCopyRegionEdit
            '
            Me.mCopyRegionEdit.Dock = System.Windows.Forms.DockStyle.Fill
            Me.mCopyRegionEdit.Location = New System.Drawing.Point(0, 0)
            Me.mCopyRegionEdit.MinimumSize = New System.Drawing.Size(489, 0)
            Me.mCopyRegionEdit.Name = "mCopyRegionEdit"
            Me.mCopyRegionEdit.Size = New System.Drawing.Size(888, 513)
            Me.mCopyRegionEdit.SuspendElectricRuns = False
            Me.mCopyRegionEdit.TabIndex = 0
            '
            'PatFlexForm
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(898, 637)
            Me.Controls.Add(Me.tabSamples)
            Me.Controls.Add(Me.txtDescription)
            Me.Name = "PatFlexForm"
            Me.Text = "PatFlex Sample Application"
            Me.tabSamples.ResumeLayout(False)
            Me.tabDemo.ResumeLayout(False)
            CType(Me.mDisplayUnwarpedResult, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.mDisplayDistortionResult, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpFlexDemo.ResumeLayout(False)
            Me.grpImageSource.ResumeLayout(False)
            Me.tabFrameGrabber.ResumeLayout(False)
            CType(Me.mAcqFifoEdit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabImageFile.ResumeLayout(False)
            CType(Me.mImageFileEdit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabPatFlex.ResumeLayout(False)
            CType(Me.mPMAlignEdit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabValidRegion.ResumeLayout(False)
            CType(Me.mCopyRegionEdit, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Private Fields"
    'Declare references for the 4 tools in this sample application. 
    'We will make use of the _Change and _Ran synchronous event handlers.
    Private mImageFileTool As CogImageFileTool = Nothing
    Private mFifoTool As CogAcqFifoTool = Nothing
    Private mRegionTool As CogCopyRegionTool = Nothing
    Private mPMAlignTool As CogPMAlignTool = Nothing

    'Flag for "VisionPro Demo" tab indicating that user is currently setting up a
    'tool.  Also used to indicate in live video mode.  If user selects "Setup"
    'then the GUI controls are disabled except for the interactive graphics
    'required for setup as well as the "OK" button used to complete the Setup.
    Private mDoneSetup As Boolean = False
#End Region

#Region "Private Enum"
    'Enumeration values passed to EnableAll & DisableAll subroutines which
    'indicates what is being setup thus determining which Buttons on the GUI
    'should be left enabled.
    Private Enum SetupConstants
      SetupLiveDisplay
      SetupPatFlex
    End Enum
#End Region

#Region "Private Control Event Handler"
    ''' <summary>
    ''' Open Image File or Start Live Display
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnOpenFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOpenFile.Click
      'Clear graphics, assuming a new image will be in the display once user
      'completes either Live Video or Open File operation, therefore, graphics
      'will be out of sync.
      mDisplayDistortionResult.StaticGraphics.Clear()
      mDisplayDistortionResult.InteractiveGraphics.Clear()

      '"Live Video" & "Stop Live" button when Frame Grabber option is selected.
      'Using our EnableAll & DisableAll subroutine to force the user stop live
      'video before doing anything else.
      If optFrameGrabber.Checked = True Then
        If mDisplayDistortionResult.LiveDisplayRunning Then
          mDisplayDistortionResult.StopLiveDisplay()
          EnableAll(SetupConstants.SetupLiveDisplay)
          'Run the mFifoTool so that all of the sample app images get the last
          'image from Live Video (see mFifoTool_PostRun)
          mFifoTool.Run()
        ElseIf Not mFifoTool.[Operator] Is Nothing Then
          mDisplayDistortionResult.StartLiveDisplay(mFifoTool.[Operator], True)
          DisableAll(SetupConstants.SetupLiveDisplay)
        End If
        '"Open File" button when image file option is selected
        'DrawingEnabled is used to simply hide the image while the Fit is performed.
        'This prevents the image from being diplayed at the initial zoom factor
        'prior to fit being called.
      Else
        Try
          Dim result As DialogResult = mOpenFileDialog.ShowDialog()
          If result <> Windows.Forms.DialogResult.Cancel Then
            mImageFileTool.[Operator].Open(mOpenFileDialog.FileName, CogImageFileModeConstants.Read)
            mDisplayDistortionResult.DrawingEnabled = False
            mImageFileTool.Run()
            mDisplayDistortionResult.Fit(True)
            mDisplayDistortionResult.DrawingEnabled = True
          End If
        Catch ex As CogFileOpenException
          MessageBox.Show(ex.Message)
        Catch ex As CogException
          MessageBox.Show(ex.Message)
        Catch ex As Exception
          MessageBox.Show(ex.Message)
        Finally
          'Add code that needs to run no matter an exception is thrown or not
        End Try
      End If
    End Sub

    '"New Image" / "Next Image" button.  Simply call Run for the approriate tool.
    'The tool's _Ran will handle passing its OutputImage to the desired
    'destinations.  By imports the _Ran instead of the placing the code this
    '_Click routine, any Run, regardless of how initiated, will have the new
    'OutputImage passed to the desired locations.
    Private Sub btnNextImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNextImage.Click
      If optFrameGrabber.Checked Then
        mFifoTool.Run()
      Else
        mImageFileTool.Run()
      End If
    End Sub

    ''' <summary>
    ''' Setup PatFlex
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSetup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetup.Click
      'PatMax Setup button has been pressed, Entering mDoneSetup mode.
      If Not mDoneSetup Then
        'Copy InputImage to TrainImage, if no ImputImage then display an
        'error message
        If mPMAlignTool.InputImage Is Nothing Then
          MessageBox.Show("No InputImage available for setup.")
          Exit Sub
        End If
        mPMAlignTool.Pattern.TrainImage = mPMAlignTool.InputImage
        'While setting up PMAlign, disable other GUI controls.
        mDoneSetup = True
        DisableAll(SetupConstants.SetupPatFlex)
        'Add TrainRegion to display's interactive graphics
        'Add SearchRegion to display's static graphics for display only.
        mDisplayDistortionResult.InteractiveGraphics.Clear()
        mDisplayDistortionResult.StaticGraphics.Clear()
        mDisplayDistortionResult.InteractiveGraphics.Add(CType(mPMAlignTool.Pattern.TrainRegion, ICogGraphicInteractive), "TrainRegion", False)
        If Not mPMAlignTool.SearchRegion Is Nothing Then
          mDisplayDistortionResult.StaticGraphics.Add(CType(mPMAlignTool.SearchRegion, ICogGraphicInteractive), "SearchRegion")
        End If
      Else 'OK has been pressed, completing Setup.
        mDoneSetup = False
        mDisplayDistortionResult.InteractiveGraphics.Clear()
        mDisplayDistortionResult.StaticGraphics.Clear()
        'Make sure we catch errors from Train, since they are likely.  For example,
        'No InputImage, No Pattern Features, etc.
        Try
          mPMAlignTool.Pattern.Origin.TranslationX = mPMAlignTool.Pattern.TrainRegion.EnclosingRectangle(CogCopyShapeConstants.All).CenterX
          mPMAlignTool.Pattern.Origin.TranslationY = mPMAlignTool.Pattern.TrainRegion.EnclosingRectangle(CogCopyShapeConstants.All).CenterY
          mPMAlignTool.Pattern.Train()
          EnableAll(SetupConstants.SetupPatFlex)
        Catch ex As CogPMAlignCanNotTrainException
          MessageBox.Show(ex.Message)
        Catch ex As CogException
          MessageBox.Show(ex.Message)
        Catch ex As Exception
          MessageBox.Show(ex.Message)
        End Try
      End If
    End Sub

    ''' <summary>
    ''' PMAlign Run button _Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
      mDisplayDistortionResult.InteractiveGraphics.Clear()
      mDisplayDistortionResult.StaticGraphics.Clear()

      'Set up the mPMAlignTool runtime algorithm to be PatFlex.
      mPMAlignTool.RunParams.RunAlgorithm = CogPMAlignRunAlgorithmConstants.PatFlex
      mPMAlignTool.RunParams.OwnedFlexParams.SaveDeformationInfo = CogPMAlignFlexDeformationInfoConstants.TransformAndUnwarpData
      mPMAlignTool.Run()
      If Not mPMAlignTool.RunStatus.Exception Is Nothing Then
        MessageBox.Show(mPMAlignTool.RunStatus.Message)
      End If
    End Sub
    ''' <summary>
    ''' Toggle FrameGrabber / ImageFile option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub optFrameGrabber_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optFrameGrabber.CheckedChanged
      UpdateImageOption()
    End Sub
    ''' <summary>
    ''' Toggle FrameGrabber / ImageFile option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub optImageFile_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optImageFile.CheckedChanged
      UpdateImageOption()
    End Sub
#End Region

#Region "Private Methods"
    Private Sub UpdateImageOption()
      If optFrameGrabber.Checked Then
        btnOpenFile.Text = "Live Video"
        btnNextImage.Text = "New Image"
      Else
        btnOpenFile.Text = "Open File"
        btnNextImage.Text = "Next Image"
      End If
    End Sub
    ''' <summary>
    ''' Disable GUI controls when forcing the user to complete a task before moving on to something new.
    ''' </summary>
    ''' <param name="butThis"></param>
    Private Sub DisableAll(ByVal butThis As SetupConstants)
      'Disable all of the frames (Disables controls within frame)
      grpImageSource.Enabled = False
      grpFlexDemo.Enabled = False
      'Disable all of the tabs except "VisionPro Demo" tab.
      For Each page As TabPage In tabSamples.TabPages
        If page.TabIndex > 0 Then
          page.Enabled = False
        End If
      Next page                                                                                                              'Based on what the user is doing, Re-enable appropriate frame and disable
      'specific controls within the frame.
      If butThis = SetupConstants.SetupPatFlex Then
        grpFlexDemo.Enabled = True
        btnSetup.Text = "OK"
        btnRun.Enabled = False
      ElseIf butThis = SetupConstants.SetupLiveDisplay Then
        grpImageSource.Enabled = True
        btnOpenFile.Text = "Stop Live"
        btnNextImage.Enabled = False
        optFrameGrabber.Enabled = False
        optImageFile.Enabled = False
      End If
    End Sub
    ''' <summary>
    ''' Enable all of the GUI controls when done a task.  Example, done setting up PMAlign.
    ''' </summary>
    ''' <param name="butThis"></param>
    Private Sub EnableAll(ByVal butThis As SetupConstants)
      grpImageSource.Enabled = True
      grpFlexDemo.Enabled = True
      For Each page As TabPage In tabSamples.TabPages
        If page.TabIndex > 0 Then
          page.Enabled = True
        End If
      Next page                                                                                                              'Based on what the user is doing, Re-enable appropriate frame and disable
      If butThis = SetupConstants.SetupPatFlex Then
        btnSetup.Text = "Setup"
        btnRun.Enabled = True
      ElseIf butThis = SetupConstants.SetupLiveDisplay Then
        btnOpenFile.Text = "Live Video"
        btnNextImage.Enabled = True
        optFrameGrabber.Enabled = True
        optImageFile.Enabled = True
      End If
    End Sub
#End Region

#Region "Private Tool Event Handler"
    ''' <summary>
    ''' Pass ImageFile OutputImage to PatMax tool and the Display on VisionPro Demo tab.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mImageFileTool_Ran(ByVal sender As Object, ByVal e As EventArgs)
      mDisplayDistortionResult.InteractiveGraphics.Clear()
      mDisplayDistortionResult.StaticGraphics.Clear()
      mDisplayDistortionResult.Image = mImageFileTool.OutputImage
      mPMAlignTool.InputImage = CType(mImageFileTool.OutputImage, CogImage8Grey)
    End Sub
    ''' <summary>
    ''' Pass AcqFifo OutputImage to the PatMax tool and the Display on VisionPro tab.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
        Private Sub mFifoTool_Ran(ByVal sender As Object, ByVal e As EventArgs)
            Static numacqs As Integer
            mDisplayDistortionResult.InteractiveGraphics.Clear()
            mDisplayDistortionResult.StaticGraphics.Clear()
            mDisplayDistortionResult.Image = mFifoTool.OutputImage
            mPMAlignTool.InputImage = CType(mFifoTool.OutputImage, CogImage8Grey)
            mImageFileTool.InputImage = mFifoTool.OutputImage
            ' Call the garbage collector to free up acquired images
            numacqs += 1
            If numacqs > 4 Then
                GC.Collect()
                numacqs = 0
            End If

        End Sub
        ''' <summary>
        ''' If PMAlign results have changed then update the Score and Region graphic.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub mPMAlignTool_Changed(ByVal sender As Object, ByVal e As CogChangedEventArgs)
            If (e.StateFlags And CogPMAlignTool.SfResults) <> 0 Then
                mDisplayDistortionResult.StaticGraphics.Clear()
                'Note, Results will be nothing if Run failed.
                If mPMAlignTool.Results Is Nothing Then
                    lblScore.Text = "N/A"
                    'Passing result does not imply Pattern is found, must check count.
                ElseIf mPMAlignTool.Results.Count > 0 Then
                    lblScore.Text = mPMAlignTool.Results(0).Score.ToString("F3")
                    'Allow the tool to display the deformation grid so the users get
                    'a feel for how much work the tool is really doing.
                    Dim resultGraphics As CogCompositeShape = mPMAlignTool.Results(0).CreateResultGraphics(CogPMAlignResultGraphicConstants.FlexDeformationGrid Or CogPMAlignResultGraphicConstants.MatchRegion)
                    mDisplayDistortionResult.InteractiveGraphics.Add(resultGraphics, "PatFlexResults", False)

                    'Copy only the region that was trained from the Unwarped Image and display
                    'that in the mRegionTool.
                    mRegionTool.InputImage = mPMAlignTool.Results(0).UnwarpedInputImage
                    mRegionTool.Region = mPMAlignTool.Pattern.TrainRegion
                    mRegionTool.Run()

                    'Take the output from the mRegionTool and put it in the right hand window of the GUI.
                    mDisplayUnwarpedResult.Image = mRegionTool.OutputImage
                    mDisplayUnwarpedResult.Fit(True)
                Else
                    lblScore.Text = "N/A"
                End If
            End If
        End Sub
#End Region

  End Class
End Namespace
