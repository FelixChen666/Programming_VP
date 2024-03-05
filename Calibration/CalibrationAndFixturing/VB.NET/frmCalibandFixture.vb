'*******************************************************************************
'Copyright (C) 2005 Cognex Corporation
'
'Subject to Cognex Corporation's terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.
'*******************************************************************************
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' Calibration and Fixturing Sample App
' ------------------------------------
'
' The calibration and fixturing sample application measures the width of a
' bracket's tab and determines the coordinates of two of its four holes.  All
' results are reported in millimeter units.  The application's level of
' functionality can be configured using standard Windows controls.  This allows
' the user to weigh the benefits of a function against the time it takes to
' execute.
'
' To use the sample, get an image of the bracket you wish to calibrate on (the
' first image in the application's default idb is a good choice) by clicking
' <Get Image>.  The display window is updated to show the active image.  If you
' don't like the active image, continue getting new images until you have a
' good one.  To calibrate, choose <Calibrate>.  Now you are ready to run an
' inspection.  Choose <Run> to inspect the active image or choose
' <Get Image and Run> to capture a new active image and inspect it.  Once you
' are comfortable with running the inspection, try changing the image analysis
' level and enabling/disabling the tool controls and graphics to get a feel for
' the cost of the different operations.
'
' The sample code is intended to demonstrate the how a VisionPro application
' can incorporate calibration and fixturing.  It also provides examples of
' coding techniques and features common to most VisionPro applications.  Such
' features include setting properties of and responding to events of VisionPro
' tools and their sub-objects, grouping a focused set of operations into a
' single tool group and displaying a composite of tool graphics on a single
' display.  The sample also demonstrates a design model which, although not
' specifically a VisionPro issue, encapsulates the complex functionality of a
' vision inspection behind a high-level interface.
'
' The code was developed in accordance with standard VisionPro coding
' guidelines.  There are two levels of the sample application; the first level
' being a QuickStart prototype application, and the second being a Visual Basic
' application.  The QuickStart prototype was developed to determine the best
' tools (and best configurations) for providing the measurements needed by the
' application.  The Visual Basic application, in addition to implementing the
' prototype's functionality, implements a GUI and provides a level of runtime
' control not possible from a QuickStart application.
'
' The Visual Basic application imports a set of tool configurations from the
' QuickStart prototype.  The level of QuickStart configurations imported was
' chosen so as to strike the best balance between leveraging QuickStart work
' already done and maximizing the flexibility available to our application.
'
' The application runs in two basic modes: calibration and inspection.  The
' calibration mode has no configuration parameters or special error handling;
' it either works or it does not.  The inspection mode is highly configurable
' and its error handling is relatively elaborate.  Since the calibration mode
' is so rigid and focused, the application was able to import, from QuickStart,
' the entire procedure in two configurations: a tool group and a calibration
' tool.  The tool group includes all the tools needed for producing the
' calibration tool's input coordinates.  In contrast, the inspection mode is so
' flexible and complex that importing a tool group providing a useful level of
' functionality is not possible.
'
' The sample uses a mixture of coordinate spaces to generate meaningful
' results.  To understand when and why a coordinate space is used, we must
' consider the three types of spaces used by the sample.  The sample's spaces
' are pixel (# = Use Pixel Space), unfixtured millimeter (@\Millimeters) and
' fixtured millimeter (@\Millimeters\Fixture).  Pixel space values specify an
' offset in pixel units from the upper left corner of the runtime image.
' Unfixtured millimeter space values specify an offset in millimeter units from
' the center of the calibration image bracket's four holes.  Fixtured
' millimeter space values specify an offset in millimeter units from the center
' of the runtime image bracket's four holes.
'
' The input regions to both caliper and blob must run in the fixtured
' millimeter space.  Running in the fixtured millimeter space means that the
' region's coordinates are relative to the part, not the image.  This is how
' the sample is able to tolerate variations in the bracket's position in the
' runtime image.  Because the application reports its results in millimeter
' units, both caliper and blob must run in one of the two types of millimeter
' spaces.  Note that the space a tool runs in need not be the same as that of
' its region.  Because the caliper result is reported as a distance, the
' caliper tool will work equally well in either space.  However, because blob
' results are reported as coordinates relative to the image, not the bracket,
' the blob tool must run in the unfixtured millimeter space.  To satisfy this
' mix of coordinate spaces, both regions are explicitly set to use the fixtured
' millimeter space and the tools, as is always the case, run in their input
' image's space.  Since caliper will work in either type of millimeter space
' and blob requires the unfixtured millimeter space, the input image's space
' must be of type unfixtured millimeter.  For the input regions to have access
' to the fixtured millimeter space, the image must come from the fixture tool.
' For the tools to run in the unfixtured millimeter space, the fixture tool
' must specify the unfixtured millimeter space as its output image's space.

Imports Cognex.VisionPro.Display
Imports Cognex.VisionPro

Public Class CalibrationAndFixturingForm
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
  Friend WithEvents GeneralTabPage As System.Windows.Forms.TabPage
  Friend WithEvents ImageFileTabPage As System.Windows.Forms.TabPage
  Friend WithEvents AcqFifoTabPage As System.Windows.Forms.TabPage
  Friend WithEvents CalTabPage As System.Windows.Forms.TabPage
  Friend WithEvents PMAlignTabPage As System.Windows.Forms.TabPage
  Friend WithEvents FixTabPage As System.Windows.Forms.TabPage
  Friend WithEvents CaliperTabPage As System.Windows.Forms.TabPage
  Friend WithEvents BlobTabPage As System.Windows.Forms.TabPage
  Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
  Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents FileRadio As System.Windows.Forms.RadioButton
  Friend WithEvents CameraRadio As System.Windows.Forms.RadioButton
  Friend WithEvents LiveCheckBox As System.Windows.Forms.CheckBox
  Friend WithEvents GetImageButton As System.Windows.Forms.Button
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents AcqCountTextBox As System.Windows.Forms.TextBox
  Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
  Friend WithEvents CalibrateButton As System.Windows.Forms.Button
  Friend WithEvents ShowCalibImageButton As System.Windows.Forms.Button
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents CalibTextBox As System.Windows.Forms.TextBox
  Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
  Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
  Friend WithEvents ShowControlsCheck As System.Windows.Forms.CheckBox
  Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
  Friend WithEvents RunButton As System.Windows.Forms.Button
  Friend WithEvents Label7 As System.Windows.Forms.Label
  Friend WithEvents TimeTextBox As System.Windows.Forms.TextBox
  Friend WithEvents DescriptionTextBox As System.Windows.Forms.TextBox
  Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
  Friend WithEvents Label8 As System.Windows.Forms.Label
  Friend WithEvents TabWidthTextBox As System.Windows.Forms.TextBox
  Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
  Friend WithEvents Label9 As System.Windows.Forms.Label
  Friend WithEvents Label10 As System.Windows.Forms.Label
  Friend WithEvents CogImageFileEdit1 As Cognex.VisionPro.ImageFile.CogImageFileEditV2
  Friend WithEvents MainTabControl As System.Windows.Forms.TabControl
  Friend WithEvents CalTabControl As System.Windows.Forms.TabControl
  Friend WithEvents InputsTabPage As System.Windows.Forms.TabPage
  Friend WithEvents ParamsTabPage As System.Windows.Forms.TabPage
  Friend WithEvents CogToolGroupEdit1 As Cognex.VisionPro.ToolGroup.CogToolGroupEditV2
  Friend WithEvents CogCalibNPointToNPointEdit1 As Cognex.VisionPro.CalibFix.CogCalibNPointToNPointEditV2
  Friend WithEvents CogPMAlignEdit1 As Cognex.VisionPro.PMAlign.CogPMAlignEditV2
  Friend WithEvents CogFixtureEdit1 As Cognex.VisionPro.CalibFix.CogFixtureEditV2
  Friend WithEvents CogCaliperEdit1 As Cognex.VisionPro.Caliper.CogCaliperEditV2
  Friend WithEvents CogBlobEdit1 As Cognex.VisionPro.Blob.CogBlobEditV2
  Friend WithEvents mResultsDisplay As Cognex.VisionPro.Display.CogDisplay
  Friend WithEvents Hole1TextBox As System.Windows.Forms.TextBox
  Friend WithEvents Hole0TextBox As System.Windows.Forms.TextBox
  Friend WithEvents AnalysisLevel As System.Windows.Forms.TrackBar
  Friend WithEvents BlobLabel As System.Windows.Forms.Label
  Friend WithEvents CaliperLabel As System.Windows.Forms.Label
  Friend WithEvents FixtureLabel As System.Windows.Forms.Label
  Friend WithEvents PMAlignLabel As System.Windows.Forms.Label
  Friend WithEvents DisplayToolGraphics As System.Windows.Forms.CheckBox
  Friend WithEvents CogAcqFifoEditV21 As Cognex.VisionPro.CogAcqFifoEditV2
  Friend WithEvents GetImAndRunButton As System.Windows.Forms.Button
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CalibrationAndFixturingForm))
    Me.MainTabControl = New System.Windows.Forms.TabControl()
    Me.GeneralTabPage = New System.Windows.Forms.TabPage()
    Me.mResultsDisplay = New Cognex.VisionPro.Display.CogDisplay()
    Me.GroupBox7 = New System.Windows.Forms.GroupBox()
    Me.Hole1TextBox = New System.Windows.Forms.TextBox()
    Me.Label10 = New System.Windows.Forms.Label()
    Me.Label9 = New System.Windows.Forms.Label()
    Me.Hole0TextBox = New System.Windows.Forms.TextBox()
    Me.GroupBox6 = New System.Windows.Forms.GroupBox()
    Me.TabWidthTextBox = New System.Windows.Forms.TextBox()
    Me.Label8 = New System.Windows.Forms.Label()
    Me.GroupBox5 = New System.Windows.Forms.GroupBox()
    Me.TimeTextBox = New System.Windows.Forms.TextBox()
    Me.Label7 = New System.Windows.Forms.Label()
    Me.GetImAndRunButton = New System.Windows.Forms.Button()
    Me.RunButton = New System.Windows.Forms.Button()
    Me.BlobLabel = New System.Windows.Forms.Label()
    Me.CaliperLabel = New System.Windows.Forms.Label()
    Me.FixtureLabel = New System.Windows.Forms.Label()
    Me.PMAlignLabel = New System.Windows.Forms.Label()
    Me.AnalysisLevel = New System.Windows.Forms.TrackBar()
    Me.GroupBox4 = New System.Windows.Forms.GroupBox()
    Me.DisplayToolGraphics = New System.Windows.Forms.CheckBox()
    Me.GroupBox3 = New System.Windows.Forms.GroupBox()
    Me.ShowControlsCheck = New System.Windows.Forms.CheckBox()
    Me.GroupBox2 = New System.Windows.Forms.GroupBox()
    Me.CalibTextBox = New System.Windows.Forms.TextBox()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.ShowCalibImageButton = New System.Windows.Forms.Button()
    Me.CalibrateButton = New System.Windows.Forms.Button()
    Me.GroupBox1 = New System.Windows.Forms.GroupBox()
    Me.AcqCountTextBox = New System.Windows.Forms.TextBox()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.GetImageButton = New System.Windows.Forms.Button()
    Me.LiveCheckBox = New System.Windows.Forms.CheckBox()
    Me.CameraRadio = New System.Windows.Forms.RadioButton()
    Me.FileRadio = New System.Windows.Forms.RadioButton()
    Me.PictureBox1 = New System.Windows.Forms.PictureBox()
    Me.ImageFileTabPage = New System.Windows.Forms.TabPage()
    Me.CogImageFileEdit1 = New Cognex.VisionPro.ImageFile.CogImageFileEditV2()
    Me.AcqFifoTabPage = New System.Windows.Forms.TabPage()
    Me.CogAcqFifoEditV21 = New Cognex.VisionPro.CogAcqFifoEditV2()
    Me.CalTabPage = New System.Windows.Forms.TabPage()
    Me.CalTabControl = New System.Windows.Forms.TabControl()
    Me.InputsTabPage = New System.Windows.Forms.TabPage()
    Me.CogToolGroupEdit1 = New Cognex.VisionPro.ToolGroup.CogToolGroupEditV2()
    Me.ParamsTabPage = New System.Windows.Forms.TabPage()
    Me.CogCalibNPointToNPointEdit1 = New Cognex.VisionPro.CalibFix.CogCalibNPointToNPointEditV2()
    Me.PMAlignTabPage = New System.Windows.Forms.TabPage()
    Me.CogPMAlignEdit1 = New Cognex.VisionPro.PMAlign.CogPMAlignEditV2()
    Me.FixTabPage = New System.Windows.Forms.TabPage()
    Me.CogFixtureEdit1 = New Cognex.VisionPro.CalibFix.CogFixtureEditV2()
    Me.CaliperTabPage = New System.Windows.Forms.TabPage()
    Me.CogCaliperEdit1 = New Cognex.VisionPro.Caliper.CogCaliperEditV2()
    Me.BlobTabPage = New System.Windows.Forms.TabPage()
    Me.CogBlobEdit1 = New Cognex.VisionPro.Blob.CogBlobEditV2()
    Me.DescriptionTextBox = New System.Windows.Forms.TextBox()
    Me.MainTabControl.SuspendLayout()
    Me.GeneralTabPage.SuspendLayout()
    CType(Me.mResultsDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupBox7.SuspendLayout()
    Me.GroupBox6.SuspendLayout()
    Me.GroupBox5.SuspendLayout()
    CType(Me.AnalysisLevel, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupBox4.SuspendLayout()
    Me.GroupBox3.SuspendLayout()
    Me.GroupBox2.SuspendLayout()
    Me.GroupBox1.SuspendLayout()
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.ImageFileTabPage.SuspendLayout()
    CType(Me.CogImageFileEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.AcqFifoTabPage.SuspendLayout()
    CType(Me.CogAcqFifoEditV21, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.CalTabPage.SuspendLayout()
    Me.CalTabControl.SuspendLayout()
    Me.InputsTabPage.SuspendLayout()
    CType(Me.CogToolGroupEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.ParamsTabPage.SuspendLayout()
    CType(Me.CogCalibNPointToNPointEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.PMAlignTabPage.SuspendLayout()
    CType(Me.CogPMAlignEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.FixTabPage.SuspendLayout()
    CType(Me.CogFixtureEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.CaliperTabPage.SuspendLayout()
    CType(Me.CogCaliperEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.BlobTabPage.SuspendLayout()
    CType(Me.CogBlobEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'MainTabControl
    '
    Me.MainTabControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
        Or System.Windows.Forms.AnchorStyles.Left) _
        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.MainTabControl.Controls.Add(Me.GeneralTabPage)
    Me.MainTabControl.Controls.Add(Me.ImageFileTabPage)
    Me.MainTabControl.Controls.Add(Me.AcqFifoTabPage)
    Me.MainTabControl.Controls.Add(Me.CalTabPage)
    Me.MainTabControl.Controls.Add(Me.PMAlignTabPage)
    Me.MainTabControl.Controls.Add(Me.FixTabPage)
    Me.MainTabControl.Controls.Add(Me.CaliperTabPage)
    Me.MainTabControl.Controls.Add(Me.BlobTabPage)
    Me.MainTabControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.MainTabControl.Location = New System.Drawing.Point(0, 0)
    Me.MainTabControl.Name = "MainTabControl"
    Me.MainTabControl.SelectedIndex = 0
    Me.MainTabControl.Size = New System.Drawing.Size(856, 551)
    Me.MainTabControl.TabIndex = 0
    '
    'GeneralTabPage
    '
    Me.GeneralTabPage.Controls.Add(Me.mResultsDisplay)
    Me.GeneralTabPage.Controls.Add(Me.GroupBox7)
    Me.GeneralTabPage.Controls.Add(Me.GroupBox6)
    Me.GeneralTabPage.Controls.Add(Me.GroupBox5)
    Me.GeneralTabPage.Controls.Add(Me.GroupBox4)
    Me.GeneralTabPage.Controls.Add(Me.GroupBox3)
    Me.GeneralTabPage.Controls.Add(Me.GroupBox2)
    Me.GeneralTabPage.Controls.Add(Me.GroupBox1)
    Me.GeneralTabPage.Controls.Add(Me.PictureBox1)
    Me.GeneralTabPage.Location = New System.Drawing.Point(4, 22)
    Me.GeneralTabPage.Name = "GeneralTabPage"
    Me.GeneralTabPage.Size = New System.Drawing.Size(848, 525)
    Me.GeneralTabPage.TabIndex = 0
    Me.GeneralTabPage.Text = "General"
    '
    'mResultsDisplay
    '
    Me.mResultsDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black
    Me.mResultsDisplay.ColorMapLowerRoiLimit = 0.0R
    Me.mResultsDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None
    Me.mResultsDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black
    Me.mResultsDisplay.ColorMapUpperRoiLimit = 1.0R
    Me.mResultsDisplay.Location = New System.Drawing.Point(320, 24)
    Me.mResultsDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
    Me.mResultsDisplay.MouseWheelSensitivity = 1.0R
    Me.mResultsDisplay.Name = "mResultsDisplay"
    Me.mResultsDisplay.OcxState = CType(resources.GetObject("mResultsDisplay.OcxState"), System.Windows.Forms.AxHost.State)
    Me.mResultsDisplay.Size = New System.Drawing.Size(512, 360)
    Me.mResultsDisplay.TabIndex = 8
    '
    'GroupBox7
    '
    Me.GroupBox7.Controls.Add(Me.Hole1TextBox)
    Me.GroupBox7.Controls.Add(Me.Label10)
    Me.GroupBox7.Controls.Add(Me.Label9)
    Me.GroupBox7.Controls.Add(Me.Hole0TextBox)
    Me.GroupBox7.Location = New System.Drawing.Point(488, 408)
    Me.GroupBox7.Name = "GroupBox7"
    Me.GroupBox7.Size = New System.Drawing.Size(352, 80)
    Me.GroupBox7.TabIndex = 7
    Me.GroupBox7.TabStop = False
    Me.GroupBox7.Text = "Hole Coordinates"
    '
    'Hole1TextBox
    '
    Me.Hole1TextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Hole1TextBox.Location = New System.Drawing.Point(232, 32)
    Me.Hole1TextBox.Name = "Hole1TextBox"
    Me.Hole1TextBox.ReadOnly = True
    Me.Hole1TextBox.Size = New System.Drawing.Size(112, 20)
    Me.Hole1TextBox.TabIndex = 4
    '
    'Label10
    '
    Me.Label10.Location = New System.Drawing.Point(184, 32)
    Me.Label10.Name = "Label10"
    Me.Label10.Size = New System.Drawing.Size(48, 24)
    Me.Label10.TabIndex = 3
    Me.Label10.Text = "Hole 1:"
    '
    'Label9
    '
    Me.Label9.Location = New System.Drawing.Point(8, 32)
    Me.Label9.Name = "Label9"
    Me.Label9.Size = New System.Drawing.Size(48, 24)
    Me.Label9.TabIndex = 1
    Me.Label9.Text = "Hole 0:"
    '
    'Hole0TextBox
    '
    Me.Hole0TextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Hole0TextBox.Location = New System.Drawing.Point(64, 32)
    Me.Hole0TextBox.Name = "Hole0TextBox"
    Me.Hole0TextBox.ReadOnly = True
    Me.Hole0TextBox.Size = New System.Drawing.Size(112, 20)
    Me.Hole0TextBox.TabIndex = 2
    '
    'GroupBox6
    '
    Me.GroupBox6.Controls.Add(Me.TabWidthTextBox)
    Me.GroupBox6.Controls.Add(Me.Label8)
    Me.GroupBox6.Location = New System.Drawing.Point(304, 408)
    Me.GroupBox6.Name = "GroupBox6"
    Me.GroupBox6.Size = New System.Drawing.Size(168, 80)
    Me.GroupBox6.TabIndex = 6
    Me.GroupBox6.TabStop = False
    Me.GroupBox6.Text = "Tab Width"
    '
    'TabWidthTextBox
    '
    Me.TabWidthTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.TabWidthTextBox.Location = New System.Drawing.Point(64, 32)
    Me.TabWidthTextBox.Name = "TabWidthTextBox"
    Me.TabWidthTextBox.ReadOnly = True
    Me.TabWidthTextBox.Size = New System.Drawing.Size(88, 20)
    Me.TabWidthTextBox.TabIndex = 1
    '
    'Label8
    '
    Me.Label8.Location = New System.Drawing.Point(16, 32)
    Me.Label8.Name = "Label8"
    Me.Label8.Size = New System.Drawing.Size(48, 24)
    Me.Label8.TabIndex = 0
    Me.Label8.Text = "Width:"
    '
    'GroupBox5
    '
    Me.GroupBox5.Controls.Add(Me.TimeTextBox)
    Me.GroupBox5.Controls.Add(Me.Label7)
    Me.GroupBox5.Controls.Add(Me.GetImAndRunButton)
    Me.GroupBox5.Controls.Add(Me.RunButton)
    Me.GroupBox5.Controls.Add(Me.BlobLabel)
    Me.GroupBox5.Controls.Add(Me.CaliperLabel)
    Me.GroupBox5.Controls.Add(Me.FixtureLabel)
    Me.GroupBox5.Controls.Add(Me.PMAlignLabel)
    Me.GroupBox5.Controls.Add(Me.AnalysisLevel)
    Me.GroupBox5.Location = New System.Drawing.Point(24, 320)
    Me.GroupBox5.Name = "GroupBox5"
    Me.GroupBox5.Size = New System.Drawing.Size(272, 168)
    Me.GroupBox5.TabIndex = 5
    Me.GroupBox5.TabStop = False
    Me.GroupBox5.Text = "Image Analysis"
    '
    'TimeTextBox
    '
    Me.TimeTextBox.Location = New System.Drawing.Point(72, 136)
    Me.TimeTextBox.Name = "TimeTextBox"
    Me.TimeTextBox.ReadOnly = True
    Me.TimeTextBox.Size = New System.Drawing.Size(80, 20)
    Me.TimeTextBox.TabIndex = 8
    '
    'Label7
    '
    Me.Label7.Location = New System.Drawing.Point(16, 136)
    Me.Label7.Name = "Label7"
    Me.Label7.Size = New System.Drawing.Size(48, 24)
    Me.Label7.TabIndex = 7
    Me.Label7.Text = "Time:"
    '
    'GetImAndRunButton
    '
    Me.GetImAndRunButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.GetImAndRunButton.Location = New System.Drawing.Point(128, 96)
    Me.GetImAndRunButton.Name = "GetImAndRunButton"
    Me.GetImAndRunButton.Size = New System.Drawing.Size(112, 24)
    Me.GetImAndRunButton.TabIndex = 6
    Me.GetImAndRunButton.Text = "Get Image and Run"
    '
    'RunButton
    '
    Me.RunButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.RunButton.Location = New System.Drawing.Point(24, 96)
    Me.RunButton.Name = "RunButton"
    Me.RunButton.Size = New System.Drawing.Size(72, 24)
    Me.RunButton.TabIndex = 5
    Me.RunButton.Text = "Run"
    '
    'BlobLabel
    '
    Me.BlobLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.BlobLabel.Location = New System.Drawing.Point(216, 24)
    Me.BlobLabel.Name = "BlobLabel"
    Me.BlobLabel.Size = New System.Drawing.Size(32, 16)
    Me.BlobLabel.TabIndex = 4
    Me.BlobLabel.Text = "Blob"
    '
    'CaliperLabel
    '
    Me.CaliperLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.CaliperLabel.Location = New System.Drawing.Point(144, 24)
    Me.CaliperLabel.Name = "CaliperLabel"
    Me.CaliperLabel.Size = New System.Drawing.Size(56, 16)
    Me.CaliperLabel.TabIndex = 3
    Me.CaliperLabel.Text = "Caliper"
    '
    'FixtureLabel
    '
    Me.FixtureLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.FixtureLabel.Location = New System.Drawing.Point(80, 24)
    Me.FixtureLabel.Name = "FixtureLabel"
    Me.FixtureLabel.Size = New System.Drawing.Size(56, 16)
    Me.FixtureLabel.TabIndex = 2
    Me.FixtureLabel.Text = "Fixture"
    '
    'PMAlignLabel
    '
    Me.PMAlignLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.PMAlignLabel.Location = New System.Drawing.Point(8, 24)
    Me.PMAlignLabel.Name = "PMAlignLabel"
    Me.PMAlignLabel.Size = New System.Drawing.Size(56, 16)
    Me.PMAlignLabel.TabIndex = 1
    Me.PMAlignLabel.Text = "PMAlign"
    '
    'AnalysisLevel
    '
    Me.AnalysisLevel.Location = New System.Drawing.Point(16, 48)
    Me.AnalysisLevel.Maximum = 3
    Me.AnalysisLevel.Name = "AnalysisLevel"
    Me.AnalysisLevel.Size = New System.Drawing.Size(224, 45)
    Me.AnalysisLevel.TabIndex = 0
    '
    'GroupBox4
    '
    Me.GroupBox4.Controls.Add(Me.DisplayToolGraphics)
    Me.GroupBox4.Location = New System.Drawing.Point(192, 256)
    Me.GroupBox4.Name = "GroupBox4"
    Me.GroupBox4.Size = New System.Drawing.Size(104, 56)
    Me.GroupBox4.TabIndex = 4
    Me.GroupBox4.TabStop = False
    Me.GroupBox4.Text = "Tool Graphics"
    '
    'DisplayToolGraphics
    '
    Me.DisplayToolGraphics.Appearance = System.Windows.Forms.Appearance.Button
    Me.DisplayToolGraphics.Location = New System.Drawing.Point(8, 24)
    Me.DisplayToolGraphics.Name = "DisplayToolGraphics"
    Me.DisplayToolGraphics.Size = New System.Drawing.Size(88, 24)
    Me.DisplayToolGraphics.TabIndex = 1
    Me.DisplayToolGraphics.Text = "Display"
    Me.DisplayToolGraphics.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'GroupBox3
    '
    Me.GroupBox3.Controls.Add(Me.ShowControlsCheck)
    Me.GroupBox3.Location = New System.Drawing.Point(192, 192)
    Me.GroupBox3.Name = "GroupBox3"
    Me.GroupBox3.Size = New System.Drawing.Size(104, 56)
    Me.GroupBox3.TabIndex = 3
    Me.GroupBox3.TabStop = False
    Me.GroupBox3.Text = "Tool Controls"
    '
    'ShowControlsCheck
    '
    Me.ShowControlsCheck.Appearance = System.Windows.Forms.Appearance.Button
    Me.ShowControlsCheck.Location = New System.Drawing.Point(8, 24)
    Me.ShowControlsCheck.Name = "ShowControlsCheck"
    Me.ShowControlsCheck.Size = New System.Drawing.Size(88, 24)
    Me.ShowControlsCheck.TabIndex = 0
    Me.ShowControlsCheck.Text = "Show"
    Me.ShowControlsCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'GroupBox2
    '
    Me.GroupBox2.Controls.Add(Me.CalibTextBox)
    Me.GroupBox2.Controls.Add(Me.Label2)
    Me.GroupBox2.Controls.Add(Me.ShowCalibImageButton)
    Me.GroupBox2.Controls.Add(Me.CalibrateButton)
    Me.GroupBox2.Location = New System.Drawing.Point(24, 184)
    Me.GroupBox2.Name = "GroupBox2"
    Me.GroupBox2.Size = New System.Drawing.Size(160, 128)
    Me.GroupBox2.TabIndex = 2
    Me.GroupBox2.TabStop = False
    Me.GroupBox2.Text = "Calibration"
    '
    'CalibTextBox
    '
    Me.CalibTextBox.Location = New System.Drawing.Point(56, 24)
    Me.CalibTextBox.Name = "CalibTextBox"
    Me.CalibTextBox.ReadOnly = True
    Me.CalibTextBox.Size = New System.Drawing.Size(88, 20)
    Me.CalibTextBox.TabIndex = 3
    '
    'Label2
    '
    Me.Label2.Location = New System.Drawing.Point(8, 24)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(48, 16)
    Me.Label2.TabIndex = 2
    Me.Label2.Text = "Status:"
    '
    'ShowCalibImageButton
    '
    Me.ShowCalibImageButton.Location = New System.Drawing.Point(16, 88)
    Me.ShowCalibImageButton.Name = "ShowCalibImageButton"
    Me.ShowCalibImageButton.Size = New System.Drawing.Size(128, 24)
    Me.ShowCalibImageButton.TabIndex = 1
    Me.ShowCalibImageButton.Text = "Show Calib Image"
    '
    'CalibrateButton
    '
    Me.CalibrateButton.Location = New System.Drawing.Point(16, 56)
    Me.CalibrateButton.Name = "CalibrateButton"
    Me.CalibrateButton.Size = New System.Drawing.Size(128, 24)
    Me.CalibrateButton.TabIndex = 0
    Me.CalibrateButton.Text = "Calibrate"
    '
    'GroupBox1
    '
    Me.GroupBox1.Controls.Add(Me.AcqCountTextBox)
    Me.GroupBox1.Controls.Add(Me.Label1)
    Me.GroupBox1.Controls.Add(Me.GetImageButton)
    Me.GroupBox1.Controls.Add(Me.LiveCheckBox)
    Me.GroupBox1.Controls.Add(Me.CameraRadio)
    Me.GroupBox1.Controls.Add(Me.FileRadio)
    Me.GroupBox1.Location = New System.Drawing.Point(24, 80)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(272, 96)
    Me.GroupBox1.TabIndex = 1
    Me.GroupBox1.TabStop = False
    Me.GroupBox1.Text = "Image Source"
    '
    'AcqCountTextBox
    '
    Me.AcqCountTextBox.Location = New System.Drawing.Point(200, 64)
    Me.AcqCountTextBox.Name = "AcqCountTextBox"
    Me.AcqCountTextBox.ReadOnly = True
    Me.AcqCountTextBox.Size = New System.Drawing.Size(56, 20)
    Me.AcqCountTextBox.TabIndex = 5
    Me.AcqCountTextBox.Text = "0"
    '
    'Label1
    '
    Me.Label1.Location = New System.Drawing.Point(128, 64)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(72, 16)
    Me.Label1.TabIndex = 4
    Me.Label1.Text = "Acq Count:"
    '
    'GetImageButton
    '
    Me.GetImageButton.Location = New System.Drawing.Point(144, 24)
    Me.GetImageButton.Name = "GetImageButton"
    Me.GetImageButton.Size = New System.Drawing.Size(88, 24)
    Me.GetImageButton.TabIndex = 3
    Me.GetImageButton.Text = "Get Image"
    '
    'LiveCheckBox
    '
    Me.LiveCheckBox.Location = New System.Drawing.Point(32, 72)
    Me.LiveCheckBox.Name = "LiveCheckBox"
    Me.LiveCheckBox.Size = New System.Drawing.Size(88, 16)
    Me.LiveCheckBox.TabIndex = 2
    Me.LiveCheckBox.Text = "Live Video"
    '
    'CameraRadio
    '
    Me.CameraRadio.Location = New System.Drawing.Point(16, 48)
    Me.CameraRadio.Name = "CameraRadio"
    Me.CameraRadio.Size = New System.Drawing.Size(96, 16)
    Me.CameraRadio.TabIndex = 1
    Me.CameraRadio.Text = "Camera"
    '
    'FileRadio
    '
    Me.FileRadio.Location = New System.Drawing.Point(16, 24)
    Me.FileRadio.Name = "FileRadio"
    Me.FileRadio.Size = New System.Drawing.Size(96, 16)
    Me.FileRadio.TabIndex = 0
    Me.FileRadio.Text = "File"
    '
    'PictureBox1
    '
    Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
    Me.PictureBox1.Location = New System.Drawing.Point(24, 24)
    Me.PictureBox1.Name = "PictureBox1"
    Me.PictureBox1.Size = New System.Drawing.Size(192, 40)
    Me.PictureBox1.TabIndex = 0
    Me.PictureBox1.TabStop = False
    '
    'ImageFileTabPage
    '
    Me.ImageFileTabPage.Controls.Add(Me.CogImageFileEdit1)
    Me.ImageFileTabPage.Location = New System.Drawing.Point(4, 22)
    Me.ImageFileTabPage.Name = "ImageFileTabPage"
    Me.ImageFileTabPage.Size = New System.Drawing.Size(848, 525)
    Me.ImageFileTabPage.TabIndex = 1
    Me.ImageFileTabPage.Text = "ImageFile"
    '
    'CogImageFileEdit1
    '
    Me.CogImageFileEdit1.AllowDrop = True
    Me.CogImageFileEdit1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.CogImageFileEdit1.Location = New System.Drawing.Point(0, 0)
    Me.CogImageFileEdit1.MinimumSize = New System.Drawing.Size(489, 0)
    Me.CogImageFileEdit1.Name = "CogImageFileEdit1"
    Me.CogImageFileEdit1.OutputHighLight = System.Drawing.Color.Lime
    Me.CogImageFileEdit1.Size = New System.Drawing.Size(848, 525)
    Me.CogImageFileEdit1.SuspendElectricRuns = False
    Me.CogImageFileEdit1.TabIndex = 0
    '
    'AcqFifoTabPage
    '
    Me.AcqFifoTabPage.Controls.Add(Me.CogAcqFifoEditV21)
    Me.AcqFifoTabPage.Location = New System.Drawing.Point(4, 22)
    Me.AcqFifoTabPage.Name = "AcqFifoTabPage"
    Me.AcqFifoTabPage.Size = New System.Drawing.Size(848, 525)
    Me.AcqFifoTabPage.TabIndex = 2
    Me.AcqFifoTabPage.Text = "AcqFifo"
    '
    'CogAcqFifoEditV21
    '
    Me.CogAcqFifoEditV21.Dock = System.Windows.Forms.DockStyle.Fill
    Me.CogAcqFifoEditV21.Location = New System.Drawing.Point(0, 0)
    Me.CogAcqFifoEditV21.MinimumSize = New System.Drawing.Size(489, 0)
    Me.CogAcqFifoEditV21.Name = "CogAcqFifoEditV21"
    Me.CogAcqFifoEditV21.Size = New System.Drawing.Size(848, 525)
    Me.CogAcqFifoEditV21.SuspendElectricRuns = False
    Me.CogAcqFifoEditV21.TabIndex = 0
    '
    'CalTabPage
    '
    Me.CalTabPage.Controls.Add(Me.CalTabControl)
    Me.CalTabPage.Location = New System.Drawing.Point(4, 22)
    Me.CalTabPage.Name = "CalTabPage"
    Me.CalTabPage.Size = New System.Drawing.Size(848, 525)
    Me.CalTabPage.TabIndex = 3
    Me.CalTabPage.Text = "Calibration"
    '
    'CalTabControl
    '
    Me.CalTabControl.Controls.Add(Me.InputsTabPage)
    Me.CalTabControl.Controls.Add(Me.ParamsTabPage)
    Me.CalTabControl.Dock = System.Windows.Forms.DockStyle.Fill
    Me.CalTabControl.Location = New System.Drawing.Point(0, 0)
    Me.CalTabControl.Name = "CalTabControl"
    Me.CalTabControl.SelectedIndex = 0
    Me.CalTabControl.Size = New System.Drawing.Size(848, 525)
    Me.CalTabControl.TabIndex = 0
    '
    'InputsTabPage
    '
    Me.InputsTabPage.Controls.Add(Me.CogToolGroupEdit1)
    Me.InputsTabPage.Location = New System.Drawing.Point(4, 22)
    Me.InputsTabPage.Name = "InputsTabPage"
    Me.InputsTabPage.Size = New System.Drawing.Size(840, 499)
    Me.InputsTabPage.TabIndex = 0
    Me.InputsTabPage.Text = "Inputs"
    '
    'CogToolGroupEdit1
    '
    Me.CogToolGroupEdit1.AllowDrop = True
    Me.CogToolGroupEdit1.ContextMenuCustomizer = Nothing
    Me.CogToolGroupEdit1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.CogToolGroupEdit1.Location = New System.Drawing.Point(0, 0)
    Me.CogToolGroupEdit1.MinimumSize = New System.Drawing.Size(489, 0)
    Me.CogToolGroupEdit1.Name = "CogToolGroupEdit1"
    Me.CogToolGroupEdit1.ShowNodeToolTips = True
    Me.CogToolGroupEdit1.Size = New System.Drawing.Size(840, 499)
    Me.CogToolGroupEdit1.SuspendElectricRuns = False
    Me.CogToolGroupEdit1.TabIndex = 0
    '
    'ParamsTabPage
    '
    Me.ParamsTabPage.Controls.Add(Me.CogCalibNPointToNPointEdit1)
    Me.ParamsTabPage.Location = New System.Drawing.Point(4, 22)
    Me.ParamsTabPage.Name = "ParamsTabPage"
    Me.ParamsTabPage.Size = New System.Drawing.Size(840, 499)
    Me.ParamsTabPage.TabIndex = 1
    Me.ParamsTabPage.Text = "Parameters"
    '
    'CogCalibNPointToNPointEdit1
    '
    Me.CogCalibNPointToNPointEdit1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.CogCalibNPointToNPointEdit1.Location = New System.Drawing.Point(0, 0)
    Me.CogCalibNPointToNPointEdit1.MinimumSize = New System.Drawing.Size(489, 0)
    Me.CogCalibNPointToNPointEdit1.Name = "CogCalibNPointToNPointEdit1"
    Me.CogCalibNPointToNPointEdit1.Size = New System.Drawing.Size(840, 499)
    Me.CogCalibNPointToNPointEdit1.SuspendElectricRuns = False
    Me.CogCalibNPointToNPointEdit1.TabIndex = 0
    '
    'PMAlignTabPage
    '
    Me.PMAlignTabPage.Controls.Add(Me.CogPMAlignEdit1)
    Me.PMAlignTabPage.Location = New System.Drawing.Point(4, 22)
    Me.PMAlignTabPage.Name = "PMAlignTabPage"
    Me.PMAlignTabPage.Size = New System.Drawing.Size(848, 525)
    Me.PMAlignTabPage.TabIndex = 4
    Me.PMAlignTabPage.Text = "PMAlign"
    '
    'CogPMAlignEdit1
    '
    Me.CogPMAlignEdit1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.CogPMAlignEdit1.Location = New System.Drawing.Point(0, 0)
    Me.CogPMAlignEdit1.MinimumSize = New System.Drawing.Size(489, 0)
    Me.CogPMAlignEdit1.Name = "CogPMAlignEdit1"
    Me.CogPMAlignEdit1.Size = New System.Drawing.Size(848, 525)
    Me.CogPMAlignEdit1.SuspendElectricRuns = False
    Me.CogPMAlignEdit1.TabIndex = 0
    '
    'FixTabPage
    '
    Me.FixTabPage.Controls.Add(Me.CogFixtureEdit1)
    Me.FixTabPage.Location = New System.Drawing.Point(4, 22)
    Me.FixTabPage.Name = "FixTabPage"
    Me.FixTabPage.Size = New System.Drawing.Size(848, 525)
    Me.FixTabPage.TabIndex = 5
    Me.FixTabPage.Text = "Fixture"
    '
    'CogFixtureEdit1
    '
    Me.CogFixtureEdit1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.CogFixtureEdit1.Location = New System.Drawing.Point(0, 0)
    Me.CogFixtureEdit1.MinimumSize = New System.Drawing.Size(489, 0)
    Me.CogFixtureEdit1.Name = "CogFixtureEdit1"
    Me.CogFixtureEdit1.Size = New System.Drawing.Size(848, 525)
    Me.CogFixtureEdit1.SuspendElectricRuns = False
    Me.CogFixtureEdit1.TabIndex = 0
    '
    'CaliperTabPage
    '
    Me.CaliperTabPage.Controls.Add(Me.CogCaliperEdit1)
    Me.CaliperTabPage.Location = New System.Drawing.Point(4, 22)
    Me.CaliperTabPage.Name = "CaliperTabPage"
    Me.CaliperTabPage.Size = New System.Drawing.Size(848, 525)
    Me.CaliperTabPage.TabIndex = 6
    Me.CaliperTabPage.Text = "Caliper"
    '
    'CogCaliperEdit1
    '
    Me.CogCaliperEdit1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.CogCaliperEdit1.Location = New System.Drawing.Point(0, 0)
    Me.CogCaliperEdit1.MinimumSize = New System.Drawing.Size(489, 0)
    Me.CogCaliperEdit1.Name = "CogCaliperEdit1"
    Me.CogCaliperEdit1.Size = New System.Drawing.Size(848, 525)
    Me.CogCaliperEdit1.SuspendElectricRuns = False
    Me.CogCaliperEdit1.TabIndex = 0
    '
    'BlobTabPage
    '
    Me.BlobTabPage.Controls.Add(Me.CogBlobEdit1)
    Me.BlobTabPage.Location = New System.Drawing.Point(4, 22)
    Me.BlobTabPage.Name = "BlobTabPage"
    Me.BlobTabPage.Size = New System.Drawing.Size(848, 525)
    Me.BlobTabPage.TabIndex = 7
    Me.BlobTabPage.Text = "Blob"
    '
    'CogBlobEdit1
    '
    Me.CogBlobEdit1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.CogBlobEdit1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.CogBlobEdit1.Location = New System.Drawing.Point(0, 0)
    Me.CogBlobEdit1.MinimumSize = New System.Drawing.Size(489, 0)
    Me.CogBlobEdit1.Name = "CogBlobEdit1"
    Me.CogBlobEdit1.Size = New System.Drawing.Size(848, 525)
    Me.CogBlobEdit1.SuspendElectricRuns = False
    Me.CogBlobEdit1.TabIndex = 0
    '
    'DescriptionTextBox
    '
    Me.DescriptionTextBox.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.DescriptionTextBox.Location = New System.Drawing.Point(0, 550)
    Me.DescriptionTextBox.Multiline = True
    Me.DescriptionTextBox.Name = "DescriptionTextBox"
    Me.DescriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.DescriptionTextBox.Size = New System.Drawing.Size(855, 48)
    Me.DescriptionTextBox.TabIndex = 1
    '
    'CalibrationAndFixturingForm
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
    Me.ClientSize = New System.Drawing.Size(855, 598)
    Me.Controls.Add(Me.DescriptionTextBox)
    Me.Controls.Add(Me.MainTabControl)
    Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Name = "CalibrationAndFixturingForm"
    Me.Text = "Calibration and Fixturing Sample Application"
    Me.MainTabControl.ResumeLayout(False)
    Me.GeneralTabPage.ResumeLayout(False)
    CType(Me.mResultsDisplay, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupBox7.ResumeLayout(False)
    Me.GroupBox7.PerformLayout()
    Me.GroupBox6.ResumeLayout(False)
    Me.GroupBox6.PerformLayout()
    Me.GroupBox5.ResumeLayout(False)
    Me.GroupBox5.PerformLayout()
    CType(Me.AnalysisLevel, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupBox4.ResumeLayout(False)
    Me.GroupBox3.ResumeLayout(False)
    Me.GroupBox2.ResumeLayout(False)
    Me.GroupBox2.PerformLayout()
    Me.GroupBox1.ResumeLayout(False)
    Me.GroupBox1.PerformLayout()
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ImageFileTabPage.ResumeLayout(False)
    CType(Me.CogImageFileEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.AcqFifoTabPage.ResumeLayout(False)
    CType(Me.CogAcqFifoEditV21, System.ComponentModel.ISupportInitialize).EndInit()
    Me.CalTabPage.ResumeLayout(False)
    Me.CalTabControl.ResumeLayout(False)
    Me.InputsTabPage.ResumeLayout(False)
    CType(Me.CogToolGroupEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ParamsTabPage.ResumeLayout(False)
    CType(Me.CogCalibNPointToNPointEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.PMAlignTabPage.ResumeLayout(False)
    CType(Me.CogPMAlignEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.FixTabPage.ResumeLayout(False)
    CType(Me.CogFixtureEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.CaliperTabPage.ResumeLayout(False)
    CType(Me.CogCaliperEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.BlobTabPage.ResumeLayout(False)
    CType(Me.CogBlobEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub

#End Region

  ' Inspection class instance.
  Dim Inspection As InspectionClass

  ' Index of first tab that corresponds to the first image analysis
  ' level (i.e. eImageAnalysisLevelPMAlign).
  Const FirstAnalysisLevelTab As Integer = 4

  Private Sub AnalysisLevel_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AnalysisLevel.Scroll
    Inspection.AnalysisLevel = AnalysisLevel.Value
    ' Disable tool labels and tabs for tools that will not be run.
    Dim i As Integer
    For i = 5 To 7
      MainTabControl.TabPages(i).Enabled = (i <= AnalysisLevel.Value + 5)
    Next
  End Sub

  Private Sub CalibrateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalibrateButton.Click
    Inspection.Calibrate()

    ' Show the new calibration image
    mResultsDisplay.InteractiveGraphics.Clear()
    mResultsDisplay.Image = Inspection.CalibrationImage

  End Sub

  Private Sub DisplayToolGraphics_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayToolGraphics.CheckedChanged
    Inspection.ToolGraphicsVisible = DisplayToolGraphics.Checked

  End Sub

  Private Sub CalibrationAndFixturingForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Inspection = New InspectionClass
    If Not Inspection.AcqAvailable Then
      CameraRadio.Enabled = False
      LiveCheckBox.Enabled = False
    End If

    ' Display surface for images and result graphics.
    Inspection.ResultsDisplay = mResultsDisplay

    ' Inspection requires access to tool edit controls.
    Inspection.AttachedForm = Me

    ' Initialize inspection by using default control values.

    Dim f As System.EventArgs = New System.EventArgs
    AnalysisLevel_Scroll(Me, f)
    DisplayToolGraphics_CheckedChanged(Me, f)
    FileRadio.Checked = True
    LiveCheckBox_CheckedChanged(Me, f)
    ShowControlsCheck_CheckedChanged(Me, f)
    ' Add event handlers for inspection events
    AddHandler Inspection.AcquisitionCompleted, AddressOf Inspection_AcquisitionCompleted
    AddHandler Inspection.ResultsChanged, AddressOf Inspection_ResultsChanged
    AddHandler Inspection.CalibrationChanged, AddressOf Inspection_CalibrationChanged

    DescriptionTextBox.Text = "Sample description: shows how to measure and report results in a defined coordinate space." + vbCrLf + _
        "Sample usage: click <Get Image> to get an image for calibration; click <Calibrate> to define a coordinate space having " + _
        "units of millimeters; and click <Get Image and Run> to inspect the bracket.  Try running the inspection with various " + _
        "features disabled to see the effect they have on inspection speed."


  End Sub

  Private Sub GetImageButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetImageButton.Click
    Inspection.GetImage()
  End Sub

  Private Sub GetImAndRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetImAndRunButton.Click
    Inspection.GetImage()
    Inspection.Run()

  End Sub

  Private Sub FileRadio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileRadio.CheckedChanged
    If FileRadio.Checked Then
      Inspection.ImageSource = InspectionClass.ImageSourceConsts.eImageFileImageSource
    Else
      Inspection.ImageSource = InspectionClass.ImageSourceConsts.eAcqFifoImageSource

    End If
  End Sub

  Private Sub CameraRadio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CameraRadio.CheckedChanged
    If CameraRadio.Checked Then
      Inspection.ImageSource = InspectionClass.ImageSourceConsts.eAcqFifoImageSource
    Else
      Inspection.ImageSource = InspectionClass.ImageSourceConsts.eImageFileImageSource

    End If

  End Sub


  Private Sub Inspection_AcquisitionCompleted()
    Static numAcquires As Integer
    Static numAcqsForGC As Integer
    ' Note that we invoke the garbage collector every 5th acquisition.
    numAcquires = numAcquires + 1
    AcqCountTextBox.Text = CStr(numAcquires)
    numAcqsForGC = numAcqsForGC + 1
    If (numAcqsForGC > 4) Then

      GC.Collect()
      numAcqsForGC = 0
    End If

  End Sub

  Private Sub Inspection_CalibrationChanged(ByVal Calibrated As Boolean)
    If Calibrated Then
      CalibTextBox.Text = "Calibrated"
    Else
      CalibTextBox.Text = "Uncalibrated"
    End If
    ' Enable functionality that is only available if calibrated.
    ShowCalibImageButton.Enabled = Calibrated
    RunButton.Enabled = Calibrated
    GetImAndRunButton.Enabled = Calibrated
  End Sub

  Private Sub Inspection_ResultsChanged()
    ' Report time taken to compute results.
    TimeTextBox.Text = CStr(Math.Round(Inspection.Time, 2))

    ' Caliper results reporting.
    TabWidthTextBox.Text = "0.0"
    If Inspection.AnalysisLevel >= InspectionClass.ImageAnalysisLevelConsts.eImageAnalysisLevelCaliper And _
       Not Inspection.TabWidthResults Is Nothing Then
      If Inspection.TabWidthResults.Count >= 1 Then
        Dim tabWidth As Double
        tabWidth = Inspection.TabWidthResults(0).Edge1.Position - _
                   Inspection.TabWidthResults(0).Edge0.Position
        TabWidthTextBox.Text = CStr(Math.Round(tabWidth, 2))
      End If
    End If

    ' Blob results reporting.
    Hole0TextBox.Text = "(0.0, 0.0)"
    Hole1TextBox.Text = "(0.0, 0.0)"
    If Inspection.AnalysisLevel >= InspectionClass.ImageAnalysisLevelConsts.eImageAnalysisLevelBlob And _
       Not Inspection.LargeHoleResults Is Nothing Then
      Dim xPos As Double, yPos As Double
      If Inspection.LargeHoleResults.GetBlobs.Count >= 1 Then
        xPos = Inspection.LargeHoleResults.GetBlobs.Item(0).CenterOfMassX
        yPos = Inspection.LargeHoleResults.GetBlobs.Item(0).CenterOfMassY
        Hole0TextBox.Text = _
          "(" + CStr(Math.Round(xPos, 2)) + ", " + CStr(Math.Round(yPos, 2)) + ")"
      End If
      If Inspection.LargeHoleResults.GetBlobs.Count >= 2 Then
        xPos = Inspection.LargeHoleResults.GetBlobs.Item(1).CenterOfMassX
        yPos = Inspection.LargeHoleResults.GetBlobs.Item(1).CenterOfMassY
        Hole1TextBox.Text = _
          "(" + CStr(Math.Round(xPos, 2)) + ", " + CStr(Math.Round(yPos, 2)) + ")"
      End If
    End If
  End Sub

  Private Sub LiveCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LiveCheckBox.CheckedChanged
    Inspection.LiveVideo = LiveCheckBox.Checked
    ' Live video may not be legal given the current configuration.
    LiveCheckBox.Checked = IIf(Inspection.LiveVideo, True, False)
  End Sub

  Private Sub RunButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunButton.Click
    Inspection.Run()

  End Sub

  Private Sub ShowCalibImageButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowCalibImageButton.Click
    mResultsDisplay.InteractiveGraphics.Clear()
    mResultsDisplay.Image = Inspection.CalibrationImage

  End Sub


  Private Sub ShowControlsCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowControlsCheck.CheckedChanged
    Inspection.ToolControlsVisible = ShowControlsCheck.Checked
    ' Show/hide tabs.
    Dim i As Integer
    For i = 1 To MainTabControl.TabPages.Count - 1
      MainTabControl.TabPages(i).Visible = Inspection.ToolControlsVisible
    Next i
  End Sub

  Private Sub CalibrationAndFixturingForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

    ' Close our edit controls nicely!
    Inspection.RemoveHandlers()
    RemoveHandler Inspection.AcquisitionCompleted, AddressOf Inspection_AcquisitionCompleted
    RemoveHandler Inspection.ResultsChanged, AddressOf Inspection_ResultsChanged
    RemoveHandler Inspection.CalibrationChanged, AddressOf Inspection_CalibrationChanged

    CogImageFileEdit1.Dispose()
    CogAcqFifoEditV21.Dispose()
    CogCalibNPointToNPointEdit1.Dispose()
    CogPMAlignEdit1.Dispose()
    CogCaliperEdit1.Dispose()
    CogBlobEdit1.Dispose()
    CogToolGroupEdit1.Dispose()
    mResultsDisplay.Dispose()

  End Sub
End Class
