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
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

Option Explicit On 
Imports Cognex.VisionPro
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.PMAlign
Namespace SampleSynthPatMax
  Public Class ModelMakerForm
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
    Friend WithEvents VProSampleAppTab As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents CogDisplay1 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents cmdImageAcquisitionLiveOrOpenCommand As System.Windows.Forms.Button
    Friend WithEvents cmdImageAcquisitionNewImageCommand As System.Windows.Forms.Button
    Friend WithEvents optImageAcquisitionOptionFrameGrabber As System.Windows.Forms.RadioButton
    Friend WithEvents optImageAcquisitionOptionImageFile As System.Windows.Forms.RadioButton
    Friend WithEvents ImageAcquisitionCommonDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cmdPatMaxSetupCommand As System.Windows.Forms.Button
    Friend WithEvents NxtImg As System.Windows.Forms.Button
    Friend WithEvents cmdPatMaxRunCommand As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPatMaxScoreValue As System.Windows.Forms.TextBox
    Friend WithEvents CogImageFileEdit1 As Cognex.VisionPro.ImageFile.CogImageFileEditV2
        Friend WithEvents CogSynthModelEditor1 As Cognex.VisionPro.CogSynthModelEditorV2
    Friend WithEvents CogPMAlignEdit1 As Cognex.VisionPro.PMAlign.CogPMAlignEditV2
    Friend WithEvents InfoTxt As System.Windows.Forms.TextBox
    Friend WithEvents frmImageAcquisitionFrame As System.Windows.Forms.GroupBox
        Friend WithEvents CogAcqFifoEdit1 As Cognex.VisionPro.CogAcqFifoEditV2
        Friend WithEvents frmPatMax As System.Windows.Forms.GroupBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ModelMakerForm))
            Me.VProSampleAppTab = New System.Windows.Forms.TabControl()
            Me.TabPage1 = New System.Windows.Forms.TabPage()
            Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay()
            Me.frmPatMax = New System.Windows.Forms.GroupBox()
            Me.txtPatMaxScoreValue = New System.Windows.Forms.TextBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.cmdPatMaxRunCommand = New System.Windows.Forms.Button()
            Me.NxtImg = New System.Windows.Forms.Button()
            Me.cmdPatMaxSetupCommand = New System.Windows.Forms.Button()
            Me.frmImageAcquisitionFrame = New System.Windows.Forms.GroupBox()
            Me.optImageAcquisitionOptionImageFile = New System.Windows.Forms.RadioButton()
            Me.optImageAcquisitionOptionFrameGrabber = New System.Windows.Forms.RadioButton()
            Me.cmdImageAcquisitionNewImageCommand = New System.Windows.Forms.Button()
            Me.cmdImageAcquisitionLiveOrOpenCommand = New System.Windows.Forms.Button()
            Me.TabPage3 = New System.Windows.Forms.TabPage()
            Me.CogImageFileEdit1 = New Cognex.VisionPro.ImageFile.CogImageFileEditV2()
            Me.TabPage2 = New System.Windows.Forms.TabPage()
            Me.CogAcqFifoEdit1 = New Cognex.VisionPro.CogAcqFifoEditV2()
            Me.TabPage4 = New System.Windows.Forms.TabPage()
            Me.CogSynthModelEditor1 = New Cognex.VisionPro.CogSynthModelEditorV2()
            Me.TabPage5 = New System.Windows.Forms.TabPage()
            Me.CogPMAlignEdit1 = New Cognex.VisionPro.PMAlign.CogPMAlignEditV2()
            Me.ImageAcquisitionCommonDialog = New System.Windows.Forms.OpenFileDialog()
            Me.InfoTxt = New System.Windows.Forms.TextBox()
            Me.VProSampleAppTab.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.frmPatMax.SuspendLayout()
            Me.frmImageAcquisitionFrame.SuspendLayout()
            Me.TabPage3.SuspendLayout()
            CType(Me.CogImageFileEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabPage2.SuspendLayout()
            CType(Me.CogAcqFifoEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabPage4.SuspendLayout()
            Me.TabPage5.SuspendLayout()
            CType(Me.CogPMAlignEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'VProSampleAppTab
            '
            Me.VProSampleAppTab.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.VProSampleAppTab.Controls.Add(Me.TabPage1)
            Me.VProSampleAppTab.Controls.Add(Me.TabPage3)
            Me.VProSampleAppTab.Controls.Add(Me.TabPage2)
            Me.VProSampleAppTab.Controls.Add(Me.TabPage4)
            Me.VProSampleAppTab.Controls.Add(Me.TabPage5)
            Me.VProSampleAppTab.Location = New System.Drawing.Point(0, 0)
            Me.VProSampleAppTab.Name = "VProSampleAppTab"
            Me.VProSampleAppTab.SelectedIndex = 0
            Me.VProSampleAppTab.Size = New System.Drawing.Size(840, 544)
            Me.VProSampleAppTab.TabIndex = 0
            '
            'TabPage1
            '
            Me.TabPage1.Controls.Add(Me.CogDisplay1)
            Me.TabPage1.Controls.Add(Me.frmPatMax)
            Me.TabPage1.Controls.Add(Me.frmImageAcquisitionFrame)
            Me.TabPage1.Location = New System.Drawing.Point(4, 22)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Size = New System.Drawing.Size(832, 518)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "VisionPro Demo"
            '
            'CogDisplay1
            '
            Me.CogDisplay1.ColorMapLowerClipColor = System.Drawing.Color.Black
            Me.CogDisplay1.ColorMapLowerRoiLimit = 0.0R
            Me.CogDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None
            Me.CogDisplay1.ColorMapUpperClipColor = System.Drawing.Color.Black
            Me.CogDisplay1.ColorMapUpperRoiLimit = 1.0R
            Me.CogDisplay1.Location = New System.Drawing.Point(360, 24)
            Me.CogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
            Me.CogDisplay1.MouseWheelSensitivity = 1.0R
            Me.CogDisplay1.Name = "CogDisplay1"
            Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
            Me.CogDisplay1.Size = New System.Drawing.Size(336, 272)
            Me.CogDisplay1.TabIndex = 2
            '
            'frmPatMax
            '
            Me.frmPatMax.Controls.Add(Me.txtPatMaxScoreValue)
            Me.frmPatMax.Controls.Add(Me.Label1)
            Me.frmPatMax.Controls.Add(Me.cmdPatMaxRunCommand)
            Me.frmPatMax.Controls.Add(Me.NxtImg)
            Me.frmPatMax.Controls.Add(Me.cmdPatMaxSetupCommand)
            Me.frmPatMax.Location = New System.Drawing.Point(16, 152)
            Me.frmPatMax.Name = "frmPatMax"
            Me.frmPatMax.Size = New System.Drawing.Size(328, 144)
            Me.frmPatMax.TabIndex = 1
            Me.frmPatMax.TabStop = False
            Me.frmPatMax.Text = "ModelMaker Demo"
            '
            'txtPatMaxScoreValue
            '
            Me.txtPatMaxScoreValue.Location = New System.Drawing.Point(200, 88)
            Me.txtPatMaxScoreValue.Name = "txtPatMaxScoreValue"
            Me.txtPatMaxScoreValue.Size = New System.Drawing.Size(100, 20)
            Me.txtPatMaxScoreValue.TabIndex = 4
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(136, 88)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(48, 23)
            Me.Label1.TabIndex = 3
            Me.Label1.Text = "Score"
            '
            'cmdPatMaxRunCommand
            '
            Me.cmdPatMaxRunCommand.Location = New System.Drawing.Point(24, 112)
            Me.cmdPatMaxRunCommand.Name = "cmdPatMaxRunCommand"
            Me.cmdPatMaxRunCommand.Size = New System.Drawing.Size(88, 23)
            Me.cmdPatMaxRunCommand.TabIndex = 2
            Me.cmdPatMaxRunCommand.Text = "Run"
            '
            'NxtImg
            '
            Me.NxtImg.Location = New System.Drawing.Point(24, 72)
            Me.NxtImg.Name = "NxtImg"
            Me.NxtImg.Size = New System.Drawing.Size(88, 23)
            Me.NxtImg.TabIndex = 1
            Me.NxtImg.Text = "Next Image"
            '
            'cmdPatMaxSetupCommand
            '
            Me.cmdPatMaxSetupCommand.Location = New System.Drawing.Point(40, 32)
            Me.cmdPatMaxSetupCommand.Name = "cmdPatMaxSetupCommand"
            Me.cmdPatMaxSetupCommand.Size = New System.Drawing.Size(216, 23)
            Me.cmdPatMaxSetupCommand.TabIndex = 0
            Me.cmdPatMaxSetupCommand.Text = "Setup PatMax Region"
            '
            'frmImageAcquisitionFrame
            '
            Me.frmImageAcquisitionFrame.Controls.Add(Me.optImageAcquisitionOptionImageFile)
            Me.frmImageAcquisitionFrame.Controls.Add(Me.optImageAcquisitionOptionFrameGrabber)
            Me.frmImageAcquisitionFrame.Controls.Add(Me.cmdImageAcquisitionNewImageCommand)
            Me.frmImageAcquisitionFrame.Controls.Add(Me.cmdImageAcquisitionLiveOrOpenCommand)
            Me.frmImageAcquisitionFrame.Location = New System.Drawing.Point(16, 16)
            Me.frmImageAcquisitionFrame.Name = "frmImageAcquisitionFrame"
            Me.frmImageAcquisitionFrame.Size = New System.Drawing.Size(328, 120)
            Me.frmImageAcquisitionFrame.TabIndex = 0
            Me.frmImageAcquisitionFrame.TabStop = False
            Me.frmImageAcquisitionFrame.Text = "Image Acquisition"
            '
            'optImageAcquisitionOptionImageFile
            '
            Me.optImageAcquisitionOptionImageFile.Checked = True
            Me.optImageAcquisitionOptionImageFile.Location = New System.Drawing.Point(8, 64)
            Me.optImageAcquisitionOptionImageFile.Name = "optImageAcquisitionOptionImageFile"
            Me.optImageAcquisitionOptionImageFile.Size = New System.Drawing.Size(136, 24)
            Me.optImageAcquisitionOptionImageFile.TabIndex = 3
            Me.optImageAcquisitionOptionImageFile.TabStop = True
            Me.optImageAcquisitionOptionImageFile.Text = "Image Database File"
            '
            'optImageAcquisitionOptionFrameGrabber
            '
            Me.optImageAcquisitionOptionFrameGrabber.Location = New System.Drawing.Point(8, 24)
            Me.optImageAcquisitionOptionFrameGrabber.Name = "optImageAcquisitionOptionFrameGrabber"
            Me.optImageAcquisitionOptionFrameGrabber.Size = New System.Drawing.Size(104, 24)
            Me.optImageAcquisitionOptionFrameGrabber.TabIndex = 2
            Me.optImageAcquisitionOptionFrameGrabber.Text = "Frame Grabber"
            '
            'cmdImageAcquisitionNewImageCommand
            '
            Me.cmdImageAcquisitionNewImageCommand.Location = New System.Drawing.Point(216, 72)
            Me.cmdImageAcquisitionNewImageCommand.Name = "cmdImageAcquisitionNewImageCommand"
            Me.cmdImageAcquisitionNewImageCommand.Size = New System.Drawing.Size(104, 40)
            Me.cmdImageAcquisitionNewImageCommand.TabIndex = 1
            Me.cmdImageAcquisitionNewImageCommand.Text = "Next image"
            '
            'cmdImageAcquisitionLiveOrOpenCommand
            '
            Me.cmdImageAcquisitionLiveOrOpenCommand.Location = New System.Drawing.Point(216, 16)
            Me.cmdImageAcquisitionLiveOrOpenCommand.Name = "cmdImageAcquisitionLiveOrOpenCommand"
            Me.cmdImageAcquisitionLiveOrOpenCommand.Size = New System.Drawing.Size(104, 40)
            Me.cmdImageAcquisitionLiveOrOpenCommand.TabIndex = 0
            Me.cmdImageAcquisitionLiveOrOpenCommand.Text = "Open File"
            '
            'TabPage3
            '
            Me.TabPage3.Controls.Add(Me.CogImageFileEdit1)
            Me.TabPage3.Location = New System.Drawing.Point(4, 22)
            Me.TabPage3.Name = "TabPage3"
            Me.TabPage3.Size = New System.Drawing.Size(832, 628)
            Me.TabPage3.TabIndex = 2
            Me.TabPage3.Text = "Image File"
            '
            'CogImageFileEdit1
            '
            Me.CogImageFileEdit1.AllowDrop = True
            Me.CogImageFileEdit1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CogImageFileEdit1.Location = New System.Drawing.Point(0, 0)
            Me.CogImageFileEdit1.MinimumSize = New System.Drawing.Size(489, 0)
            Me.CogImageFileEdit1.Name = "CogImageFileEdit1"
            Me.CogImageFileEdit1.OutputHighLight = System.Drawing.Color.Lime
            Me.CogImageFileEdit1.Size = New System.Drawing.Size(832, 628)
            Me.CogImageFileEdit1.SuspendElectricRuns = False
            Me.CogImageFileEdit1.TabIndex = 0
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.CogAcqFifoEdit1)
            Me.TabPage2.Location = New System.Drawing.Point(4, 22)
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.Size = New System.Drawing.Size(832, 628)
            Me.TabPage2.TabIndex = 1
            Me.TabPage2.Text = "Frame Grabber"
            '
            'CogAcqFifoEdit1
            '
            Me.CogAcqFifoEdit1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CogAcqFifoEdit1.Location = New System.Drawing.Point(0, 0)
            Me.CogAcqFifoEdit1.MinimumSize = New System.Drawing.Size(489, 0)
            Me.CogAcqFifoEdit1.Name = "CogAcqFifoEdit1"
            Me.CogAcqFifoEdit1.Size = New System.Drawing.Size(832, 628)
            Me.CogAcqFifoEdit1.SuspendElectricRuns = False
            Me.CogAcqFifoEdit1.TabIndex = 0
            '
            'TabPage4
            '
            Me.TabPage4.Controls.Add(Me.CogSynthModelEditor1)
            Me.TabPage4.Location = New System.Drawing.Point(4, 22)
            Me.TabPage4.Name = "TabPage4"
            Me.TabPage4.Size = New System.Drawing.Size(832, 628)
            Me.TabPage4.TabIndex = 3
            Me.TabPage4.Text = "Model Maker"
            '
            'CogSynthModelEditor1
            '
            Me.CogSynthModelEditor1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CogSynthModelEditor1.Image = Nothing
            Me.CogSynthModelEditor1.Location = New System.Drawing.Point(0, 0)
            Me.CogSynthModelEditor1.Name = "CogSynthModelEditor1"
            Me.CogSynthModelEditor1.ShowToolTips = False
            Me.CogSynthModelEditor1.Size = New System.Drawing.Size(832, 628)
      Me.CogSynthModelEditor1.TabIndex = 0
            '
            'TabPage5
            '
            Me.TabPage5.Controls.Add(Me.CogPMAlignEdit1)
            Me.TabPage5.Location = New System.Drawing.Point(4, 22)
            Me.TabPage5.Name = "TabPage5"
            Me.TabPage5.Size = New System.Drawing.Size(832, 628)
            Me.TabPage5.TabIndex = 4
            Me.TabPage5.Text = "PatMax"
            '
            'CogPMAlignEdit1
            '
            Me.CogPMAlignEdit1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CogPMAlignEdit1.Location = New System.Drawing.Point(0, 0)
            Me.CogPMAlignEdit1.MinimumSize = New System.Drawing.Size(489, 0)
            Me.CogPMAlignEdit1.Name = "CogPMAlignEdit1"
            Me.CogPMAlignEdit1.Size = New System.Drawing.Size(832, 628)
            Me.CogPMAlignEdit1.SuspendElectricRuns = False
            Me.CogPMAlignEdit1.TabIndex = 0
            '
            'InfoTxt
            '
            Me.InfoTxt.AcceptsReturn = True
            Me.InfoTxt.AcceptsTab = True
            Me.InfoTxt.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.InfoTxt.Location = New System.Drawing.Point(0, 550)
            Me.InfoTxt.Multiline = True
            Me.InfoTxt.Name = "InfoTxt"
            Me.InfoTxt.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.InfoTxt.Size = New System.Drawing.Size(840, 104)
            Me.InfoTxt.TabIndex = 1
            Me.InfoTxt.Text = "InfoTxt"
            '
            'ModelMakerForm
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(840, 654)
            Me.Controls.Add(Me.InfoTxt)
            Me.Controls.Add(Me.VProSampleAppTab)
            Me.Name = "ModelMakerForm"
            Me.Text = "VisionPro Demo"
            Me.VProSampleAppTab.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.frmPatMax.ResumeLayout(False)
            Me.frmPatMax.PerformLayout()
            Me.frmImageAcquisitionFrame.ResumeLayout(False)
            Me.TabPage3.ResumeLayout(False)
            CType(Me.CogImageFileEdit1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabPage2.ResumeLayout(False)
            CType(Me.CogAcqFifoEdit1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabPage4.ResumeLayout(False)
            Me.TabPage5.ResumeLayout(False)
            CType(Me.CogPMAlignEdit1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Module Level vars"
        Private ImageFileTool As CogImageFileTool
        Private PatMaxTool As CogPMAlignTool
        Private AcqFifoTool As CogAcqFifoTool

        'Flag for "VisionPro Demo" tab indicating that user is currently setting up a
        'tool.  Also used to indicate in live video mode.  If user selects "Setup"
        'then the GUI controls are disabled except for the interactive graphics
        'required for setup as well as the "OK" button used to complete the Setup.
        Private SettingUp As Boolean

        'Enumeration values passed to EnableAll & DisableAll subroutines which
        'indicates what is being setup thus determining which Buttons on the GUI
        'should be left enabled.
        Private Enum SettingUpConstants
            settingUpPatMax = 0
            settingLiveVideo = 1
        End Enum
#End Region

#Region "Form and Controls Events"
        Private Sub ModelMakerForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If MessageBox.Show("Optimal screen resolution for this demo is 1024x768 or more.", "", _
                    MessageBoxButtons.OKCancel) = Windows.Forms.DialogResult.Cancel Then
                Me.Close()
                Exit Sub
            End If

            SettingUp = False

            ShowDemoInstructions()

            'Set reference to CogImageFileTool created by Edit Control
            'The Image File Edit Control creates its subject when its AutoCreateTool property is True
            ImageFileTool = CogImageFileEdit1.Subject
            AddHandler ImageFileTool.Ran, AddressOf imageFileTool_Ran
            'Set reference to CogAcqFifoTool created by Edit Control
            'The Acq Fifo Edit Control creates its subject when its AutoCreateTool property is True
            AcqFifoTool = CogAcqFifoEdit1.Subject
            AddHandler AcqFifoTool.Ran, AddressOf AcqFifoTool_Ran
            'Operator will be Nothing if no Frame Grabber is available.  Disable the Frame Grabber
            'option on the "VisionPro Demo" tab if no frame grabber available.
            If AcqFifoTool.[Operator] Is Nothing Then
                optImageAcquisitionOptionFrameGrabber.Enabled = False
            End If

            'Initialize the Dialog box for the "Open File" button on the "VisionPro Demo" tab.
            ImageAcquisitionCommonDialog.Filter = ImageFileTool.[Operator].FilterText
            ImageAcquisitionCommonDialog.CheckFileExists = True Or _
            ImageAcquisitionCommonDialog.ShowReadOnly = True


            'AutoCreateTool for the PMAlign edit control is False, therefore, we must create
            'a PMAlign tool and set the subject of the control to reference the new tool.
            PatMaxTool = New CogPMAlignTool
            AddHandler PatMaxTool.Changed, AddressOf patMaxTool_Changed
            CogPMAlignEdit1.Subject = PatMaxTool

            'Change the default Train Region to center of a 640x480 image & change the DOFs
            'so that Skew is not enabled.  Note - TrainRegion is of type ICogRegion, therefore,
            'we must use a CogRectangleAffine reference in order to call CogRectangleAffine
            'properties.
            Dim PatMaxTrainRegion As CogRectangleAffine
            PatMaxTrainRegion = PatMaxTool.Pattern.TrainRegion
            If Not PatMaxTrainRegion Is Nothing Then
                PatMaxTrainRegion.SetCenterLengthsRotationSkew(330, 245, 610, 130, 0, 0)
                PatMaxTrainRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position Or _
                 CogRectangleAffineDOFConstants.Rotation Or CogRectangleAffineDOFConstants.Size
            End If

            'Create a SearchRegion that uses the entire image (assumes 640x480)
            'Note that by default the SearchRegion is Nothing and PMAlign will search the entire
            'image anyway.  This is added for sample code purposes & to graphically show that the
            'entire image is being used.
            Dim PatMaxSearchRegion As New CogRectangle
            PatMaxTool.SearchRegion = PatMaxSearchRegion

            'Establish an Region Of Interest (ROI) to let the user manipulate during training.
            PatMaxSearchRegion.SetCenterWidthHeight(320, 240, 640, 480)
            PatMaxSearchRegion.GraphicDOFEnable = CogRectangleDOFConstants.Size Or CogRectangleDOFConstants.Position
            PatMaxSearchRegion.Interactive = True

            'Set up the PatMaxTool to Train using the PatMax algorithm.
            ' enable the angle and scale degrees of freedom for Patmax.
            PatMaxTool.RunParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh
            PatMaxTool.RunParams.ZoneAngle.Low = CogMisc.DegToRad(-45.0#)
            PatMaxTool.RunParams.ZoneAngle.High = CogMisc.DegToRad(45.0#)
            PatMaxTool.RunParams.ZoneScale.Configuration = CogPMAlignZoneConstants.LowHigh
            PatMaxTool.RunParams.ZoneScale.Low = 0.8
            PatMaxTool.RunParams.ZoneScale.High = 1.2
            PatMaxTool.Pattern.TrainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatMaxAndPatQuick
            PatMaxTool.Pattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBox
            PatMaxTool.Pattern.GrainLimitAutoSelect = False
            PatMaxTool.Pattern.GrainLimitCoarse = 2.0#
            PatMaxTool.Pattern.GrainLimitFine = 1.0#
            PatMaxTool.RunParams.ScoreUsingClutter = False
            PatMaxTool.RunParams.SaveMatchInfo = True
            PatMaxTool.RunParams.RunAlgorithm = CogPMAlignRunAlgorithmConstants.PatMax
            PatMaxTool.LastRunRecordDiagEnable = PatMaxTool.LastRunRecordDiagEnable Or CogPMAlignLastRunRecordDiagConstants.ResultsMatchFeatures
        End Sub

        Private Sub VProSampleAppTab_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VProSampleAppTab.SelectedIndexChanged
            ' current tab number is

            Dim curr_tab As Integer
            curr_tab = VProSampleAppTab.SelectedIndex

            ' display instructions for first tab (demo tab)
            If curr_tab = 0 Then
                ShowDemoInstructions()
            End If

            ' display instructions for second tab
            If curr_tab = 1 Then
                InfoTxt.Text = " "
            End If

            ' display instructions for third tab
            If curr_tab = 2 Then
                InfoTxt.Text = " "
            End If

            ' display instructions for fouth tab (Model Maker)
            If curr_tab = 3 Then
                InfoTxt.Text = "The user should execute the following steps to create a Shape Model (VPP file) From a CAD DXF File :" & Environment.NewLine & _
                 "1)   Click the File->Open icon" & Environment.NewLine & _
                 "2)   At the bottom of the File Selection dialog box, change the type of file to DXF" & Environment.NewLine & _
                 "3)   Select .../SynthPatMax/VB.NET/plate.dxf and click Open." & Environment.NewLine & _
                 "4)   Select the Layers radio-box, select layer named PNL for loading and click OK." & Environment.NewLine & _
                 "5)   Right Click on the backgound and select Fit Image & Graphics from the menu." & Environment.NewLine & _
                 "6)   Type CTRL-A to select all the shapes, then drag the bounding box into the middle of the image.  " & Environment.NewLine & _
                 "       See the selected object in the top left? That's your imported DXF information. " & Environment.NewLine & _
                 "7)   Manipulate the shapes by dragging, resizing and using the Flip/Rotate button " & Environment.NewLine & _
                 "8)   You may even remove shapes you think might be incorrect or not reliable for some reason." & Environment.NewLine & _
                 "9)   Get the shapes aligned, sized and rotated so that they fit nicely on top of the image. " & Environment.NewLine & _
                 "10)  Type CTRL-A to select all the shapes." & Environment.NewLine & _
                 "11)  Now it's time to Click the Polarize button to get the shapes to have the right polarity before we save them." & Environment.NewLine & _
                 "       Polarize brings up a small GUI box, just use the defaults by clicking OK. " & Environment.NewLine & _
                 "12)  Perform a Save As operation by clicking the icon in the GUI and selecting Save" & Environment.NewLine & _
                 "13)  Name the file something like plate_shapes_saved.vpp" & Environment.NewLine & _
                 "14)  This VPP file can now be loaded into any PatMax object and used for alignment purposes." & Environment.NewLine & _
                 "15)  Now click on the PatMax tab in the GUI and follow the instructions for PatMax. "
            End If

            ' display instructions for fifth tab (PatMax)
            If curr_tab = 4 Then
                InfoTxt.Text = "Here's how you can load this new model into PatMax and train it: " & Environment.NewLine & _
                 "Some of these steps are already being done programmatically in VB but for the purpose of learning we'll do it manually too." & Environment.NewLine & _
                 "1)   In the PatMax dialog, on the Train Params tab, select a Train Mode of Shape Models With Image." & Environment.NewLine & _
                 "2)   This will make the Model Maker toolbar button (the little blue triangle) selectable in the GUI. " & Environment.NewLine & _
                 "3)   Click the Model Maker Icon, then Open the VPP file you just saved (plate_shapes_saved.vpp)." & Environment.NewLine & _
                 "4)   Right Click on the image and select Fit Image & Graphics from the menu." & Environment.NewLine & _
                 "5)   When you close the Model Maker Interface, you'll almost be ready to train the pattern. " & Environment.NewLine & _
                 "6)   Now you need to select the Train Region & Origin tab on the PatMax GUI" & Environment.NewLine & _
                 "7)   Under the Region Mode, select Pixel Aligned Bounding Box" & Environment.NewLine & _
                 "8)   Now go back to the Train Params Tab of the PatMax GUI and click Train and you're ready to run." & Environment.NewLine & _
                 "9)   And by the way, you can save trained PatMax tool as a VPP file too and use it in another application " & Environment.NewLine & _
                 "10)  Go back to the VisionPro Demo tab, click the run button and watch the results"
            End If
        End Sub

        Private Sub cmdImageAcquisitionLiveOrOpenCommand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdImageAcquisitionLiveOrOpenCommand.Click
            'Clear graphics, assuming a new image will be in the display once user
            'completes either Live Video or Open File operation, therefore, graphics
            'will be out of sync.
            CogDisplay1.StaticGraphics.Clear()
            CogDisplay1.InteractiveGraphics.Clear()

            '"Live Video" & "Stop Live" button when Frame Grabber option is selected.
            'Using our EnableAll & DisableAll subroutine to force the user stop live
            'video before doing anything else.
            If optImageAcquisitionOptionFrameGrabber.Checked = True Then
                If CogDisplay1.LiveDisplayRunning Then
                    CogDisplay1.StopLiveDisplay()
                    EnableAll(SettingUpConstants.settingLiveVideo)
                    'Run the AcqFifoTool so that all of the sample app images get the last
                    'image from Live Video (see AcqFifoTool_PostRun)
                    AcqFifoTool.Run()
                ElseIf Not AcqFifoTool.[Operator] Is Nothing Then
                    CogDisplay1.StartLiveDisplay(AcqFifoTool.[Operator])
                    DisableAll(SettingUpConstants.settingLiveVideo)
                End If
                '"Open File" button when image file option is selected
                'DrawingEnabled is used to simply hide the image while the Fit is performed.
                'This prevents the image from being diplayed at the initial zoom factor
                'prior to fit being called.
            Else

                Try
                    ImageAcquisitionCommonDialog.InitialDirectory = Environment.GetEnvironmentVariable("VPRO_ROOT") & "\images"
                    Dim result As DialogResult = ImageAcquisitionCommonDialog.ShowDialog()
                    If result <> Windows.Forms.DialogResult.Cancel Then
                        ImageFileTool.[Operator].Open(ImageAcquisitionCommonDialog.FileName, CogImageFileModeConstants.Read)
                        CogDisplay1.DrawingEnabled = False
                        ImageFileTool.Run()
                        CogDisplay1.Fit()
                        CogDisplay1.DrawingEnabled = True
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex.Message)

                End Try
            End If
        End Sub

        Private Sub optImageAcquisitionOptionFrameGrabber_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optImageAcquisitionOptionFrameGrabber.CheckedChanged
            If optImageAcquisitionOptionFrameGrabber.Checked = True Then
                cmdImageAcquisitionLiveOrOpenCommand.Text = "Live Video"
                cmdImageAcquisitionNewImageCommand.Text = "New Image"
            Else
                cmdImageAcquisitionLiveOrOpenCommand.Text = "Open File"
                cmdImageAcquisitionNewImageCommand.Text = "Next Image"
            End If
        End Sub

        Private Sub optImageAcquisitionOptionImageFile_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optImageAcquisitionOptionImageFile.CheckedChanged
            If optImageAcquisitionOptionImageFile.Checked = True Then
                cmdImageAcquisitionLiveOrOpenCommand.Text = "Open File"
                cmdImageAcquisitionNewImageCommand.Text = "Next Image"
            Else
                cmdImageAcquisitionLiveOrOpenCommand.Text = "Live Video"
                cmdImageAcquisitionNewImageCommand.Text = "New Image"
            End If
        End Sub

        Private Sub cmdPatMaxSetupCommand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPatMaxSetupCommand.Click
            'PatMax Setup button has been pressed, Entering SettingUp mode.
            If Not SettingUp Then
                'Copy InputImage to TrainImage, If no ImputImage then display an
                'error message
                If PatMaxTool.InputImage Is Nothing Then
                    MessageBox.Show("No InputImage available for setup.", "PatMax Setup Error")
                    Exit Sub
                End If
                PatMaxTool.Pattern.TrainImage = PatMaxTool.InputImage
                CogSynthModelEditor1.Image = PatMaxTool.InputImage
                'While setting up PMAlign, disable other GUI controls.
                SettingUp = True
                DisableAll(SettingUpConstants.settingUpPatMax)
                'Add TrainRegion to display's interactive graphics
                'Add SearchRegion to display's static graphics for display only.
                CogDisplay1.InteractiveGraphics.Clear()
                CogDisplay1.StaticGraphics.Clear()
                CogDisplay1.InteractiveGraphics.Add(PatMaxTool.Pattern.TrainRegion, "test", False)
                If Not PatMaxTool.SearchRegion Is Nothing Then
                    CogDisplay1.StaticGraphics.Add(PatMaxTool.SearchRegion, "test")
                End If
                'OK has been pressed, completing Setup.
            Else
                SettingUp = False
                CogDisplay1.InteractiveGraphics.Clear()
                CogDisplay1.StaticGraphics.Clear()
                'Make sure we catch errors from Train, since they are likely.  For example,
                'No InputImage, No Pattern Features, etc.
                Try
                    PatMaxTool.Pattern.Origin.TranslationX = PatMaxTool.Pattern.TrainRegion.EnclosingRectangle(CogCopyShapeConstants.All).CenterX
                    PatMaxTool.Pattern.Origin.TranslationY = PatMaxTool.Pattern.TrainRegion.EnclosingRectangle(CogCopyShapeConstants.All).CenterY
                    PatMaxTool.Pattern.TrainMode = CogPMAlignTrainModeConstants.ShapeModelsWithImage
                    ' dont do training here, do it from the PatMax control at the end of this demo.
                    ' PatMaxTool.Pattern.Train
                Catch cogex As Exceptions.CogException

                    MessageBox.Show("Following Specific Cognex Error Occured:" & cogex.Message)
                Catch ex As Exception

                    MessageBox.Show(Err.Description, "PatMax Setup Error")
                End Try
                EnableAll(SettingUpConstants.settingUpPatMax)
            End If
        End Sub

        '"New Image" / "Next Image" button.  Simply call Run for the approriate tool.
        'The tool's _PostRun will handle passing its OutputImage to the desired
        'destinations.  By using the _PostRun instead of the placing the code this
        '_Click routine, any Run, regardless of how initiated, will have the new
        'OutputImage passed to the desired locations.
        Private Sub NewImageCommand()
            If optImageAcquisitionOptionFrameGrabber.Checked = True Then
                AcqFifoTool.Run()
            Else
                ImageFileTool.Run()
            End If
        End Sub


        Private Sub cmdImageAcquisitionNewImageCommand_Click( _
          ByVal sender As System.Object, _
          ByVal e As System.EventArgs) Handles cmdImageAcquisitionNewImageCommand.Click
            NewImageCommand()
        End Sub


        Private Sub NxtImg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NxtImg.Click
            NewImageCommand()
        End Sub

        Private Sub cmdPatMaxRunCommand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPatMaxRunCommand.Click
            CogDisplay1.InteractiveGraphics.Clear()
            CogDisplay1.StaticGraphics.Clear()

            'Set up the PatMaxTool runtime algorithm to be PatMax.
            PatMaxTool.Run()
            If Not PatMaxTool.RunStatus.Exception Is Nothing Then
                MessageBox.Show(PatMaxTool.RunStatus.Exception.Message, "PatMax Run Error")
            End If
        End Sub
        Private Sub ModelMakerForm_closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
            RemoveHandler ImageFileTool.Ran, AddressOf imageFileTool_Ran
            RemoveHandler AcqFifoTool.Ran, AddressOf AcqFifoTool_Ran
            RemoveHandler PatMaxTool.Changed, AddressOf patMaxTool_Changed
            If Not ImageFileTool Is Nothing Then ImageFileTool.Dispose()
            If Not AcqFifoTool Is Nothing Then AcqFifoTool.Dispose()
            If Not PatMaxTool Is Nothing Then PatMaxTool.Dispose()

        End Sub
#End Region
#Region "Module Level Routines"
        'Disable GUI controls when forcing the user to complete a task before moving on
        'to something new.  Example, Setting up PMAlign.
        Private Sub DisableAll(ByVal butThis As SettingUpConstants)
            'Disable all of the frames (Disables controls within frame)
            frmImageAcquisitionFrame.Enabled = False
            frmPatMax.Enabled = False
            'Disable all of the tabs except "VisionPro Demo" tab.
            VProSampleAppTab.TabPages(1).Enabled = False
            VProSampleAppTab.TabPages(2).Enabled = False
            VProSampleAppTab.TabPages(3).Enabled = False
            'Based on what the user is doing, Re-enable appropriate frame and disable
            'specific controls within the frame.
            If butThis = SettingUpConstants.settingUpPatMax Then
                frmPatMax.Enabled = True
                cmdPatMaxSetupCommand.Text = "OK"
                cmdPatMaxRunCommand.Enabled = False
            ElseIf butThis = SettingUpConstants.settingLiveVideo Then
                frmImageAcquisitionFrame.Enabled = True
                cmdImageAcquisitionLiveOrOpenCommand.Text = "Stop Live"
                cmdImageAcquisitionNewImageCommand.Enabled = False
                optImageAcquisitionOptionFrameGrabber.Enabled = False
                optImageAcquisitionOptionImageFile.Enabled = False
            End If
        End Sub
        'Enable all of the GUI controls when done a task.  Example, done setting up PMAlign.
        Private Sub EnableAll(ByVal butThis As SettingUpConstants)
            frmImageAcquisitionFrame.Enabled = True
            frmPatMax.Enabled = True
            VProSampleAppTab.TabPages(1).Enabled = True
            VProSampleAppTab.TabPages(2).Enabled = True
            VProSampleAppTab.TabPages(3).Enabled = True
            If butThis = SettingUpConstants.settingUpPatMax Then
                cmdPatMaxSetupCommand.Text = "Setup PatMax Region"
                cmdPatMaxRunCommand.Enabled = True
            ElseIf butThis = SettingUpConstants.settingLiveVideo Then
                cmdImageAcquisitionLiveOrOpenCommand.Text = "Live Video"
                cmdImageAcquisitionNewImageCommand.Enabled = True
                optImageAcquisitionOptionFrameGrabber.Enabled = True
                optImageAcquisitionOptionImageFile.Enabled = True
            End If
        End Sub
        Private Sub ShowDemoInstructions()
            InfoTxt.Text = "This sample application demonstrates the use of the Model Maker Control within the PatMax tool. " & Environment.NewLine & _
            "It allows the user to import and manipulate a CAD DXF file, save the new shapes to a VPP file and then " & Environment.NewLine & _
            "load and train PatMax using that saved VPP file data. " & Environment.NewLine & _
            "If you execute the tabs in a left to right fashion and follow the instructions at the bottom of each tab " & Environment.NewLine & _
            "you will be able to accomplish this in just minutes." & Environment.NewLine & _
            "1) The first thing you want to do is click the Open File button and select the .../images/plate.idb file. " & Environment.NewLine & _
            "2) Then click the Setup PatMax Region button and resize the training rectangle to fit around the part in the image. " & Environment.NewLine & _
            "3) When you are done resizing the window, click the OK button and this region will be saved as the PatMax training window. " & Environment.NewLine & _
            "4) Now, it's ok to move on to the Model Maker tab on the GUI and follow the instructions there. "
        End Sub

#End Region
#Region "Cognex Objects Events"
        'Pass ImageFile OutputImage to PatMax tool & the Display on "VisionPro Demo" tab.
        Private Sub imageFileTool_Ran(ByVal sender As Object, ByVal e As EventArgs)
            CogDisplay1.InteractiveGraphics.Clear()
            CogDisplay1.StaticGraphics.Clear()
            CogDisplay1.Image = ImageFileTool.OutputImage
            PatMaxTool.InputImage = ImageFileTool.OutputImage
        End Sub
        'Pass AcqFifo OutputImage to the PatMax tool & the Display on "VisionPro" tab.
        'Also, pass OutputImage to the InputImage of ImageFile tool so that this
        'sample application can be used to Record an image file from frame grabber.
        Private Sub AcqFifoTool_Ran(ByVal sender As Object, ByVal e As EventArgs)
            Static numacqs As Integer
            CogDisplay1.InteractiveGraphics.Clear()
            CogDisplay1.StaticGraphics.Clear()
            CogDisplay1.Image = AcqFifoTool.OutputImage
            PatMaxTool.InputImage = AcqFifoTool.OutputImage
            ImageFileTool.InputImage = AcqFifoTool.OutputImage
            ' Run the garbage collector to free images
            numacqs += 1
            If numacqs > 4 Then
                GC.Collect()
                numacqs = 0
            End If
        End Sub
        'If PMAlign results have changed then update the Score & Region graphic.
        Private Sub patMaxTool_Changed(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogChangedEventArgs)

            CogDisplay1.StaticGraphics.Clear()
            'Note, Results will be nothing if Run failed.
            If PatMaxTool.Results Is Nothing Then
                txtPatMaxScoreValue.Text = "N/A"
                'Passing result does not imply Pattern is found, must check count.
            ElseIf PatMaxTool.Results.Count > 0 Then
                txtPatMaxScoreValue.Text = Math.Round(PatMaxTool.Results(0).Score, 3)
                txtPatMaxScoreValue.Refresh()
                Dim resultGraphics As CogCompositeShape

                'Allow the tool to display the match features and match region.
                resultGraphics = PatMaxTool.Results(0).CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchFeatures Or CogPMAlignResultGraphicConstants.MatchRegion)
                CogDisplay1.InteractiveGraphics.Add(resultGraphics, "test", False)
            Else
                txtPatMaxScoreValue.Text = "N/A"
            End If

        End Sub
#End Region


    End Class
End Namespace