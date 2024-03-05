
'*******************************************************************************
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
Imports Cognex.VisionPro.ImageProcessing
Imports Cognex.VisionPro.PatInspect
Imports Cognex.VisionPro.PMAlign
Imports Cognex.VisionPro.Exceptions
Namespace SamplePMAlign
  Public Class frmPatInspSamp
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
    Friend WithEvents InfoTxt As System.Windows.Forms.TextBox
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents FrameGrabber As System.Windows.Forms.TabPage
    Friend WithEvents CogDisplay1 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents optImageAcquisitionOptionFrameGrabber As System.Windows.Forms.RadioButton
    Friend WithEvents optImageAcquisitionOptionImageFile As System.Windows.Forms.RadioButton
    Friend WithEvents cmdPatMaxSetupCommand As System.Windows.Forms.Button
    Friend WithEvents txtPatMaxScoreValue As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdImageAcquisitionLiveOrOpenCommand As System.Windows.Forms.Button
    Friend WithEvents cmdImageAcquisitionNewImageCommand As System.Windows.Forms.Button
    Friend WithEvents ImageAcquisitionCommonDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents CogImageFileEdit1 As Cognex.VisionPro.ImageFile.CogImageFileEditV2
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents CogPMAlignEdit1 As Cognex.VisionPro.PMAlign.CogPMAlignEditV2
    Friend WithEvents VProSampleAppTab As System.Windows.Forms.TabControl
    Friend WithEvents cmdPatMaxRunCommand As System.Windows.Forms.Button
    Friend WithEvents frmPatMax As System.Windows.Forms.GroupBox
    Friend WithEvents CogAcqFifoEdit1 As Cognex.VisionPro.CogAcqFifoEditV2
    Friend WithEvents frmImageAcquisitionFrame As System.Windows.Forms.GroupBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPatInspSamp))
            Me.InfoTxt = New System.Windows.Forms.TextBox()
            Me.VProSampleAppTab = New System.Windows.Forms.TabControl()
            Me.TabPage1 = New System.Windows.Forms.TabPage()
            Me.frmPatMax = New System.Windows.Forms.GroupBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.txtPatMaxScoreValue = New System.Windows.Forms.TextBox()
            Me.cmdPatMaxRunCommand = New System.Windows.Forms.Button()
            Me.cmdPatMaxSetupCommand = New System.Windows.Forms.Button()
            Me.frmImageAcquisitionFrame = New System.Windows.Forms.GroupBox()
            Me.cmdImageAcquisitionNewImageCommand = New System.Windows.Forms.Button()
            Me.cmdImageAcquisitionLiveOrOpenCommand = New System.Windows.Forms.Button()
            Me.optImageAcquisitionOptionImageFile = New System.Windows.Forms.RadioButton()
            Me.optImageAcquisitionOptionFrameGrabber = New System.Windows.Forms.RadioButton()
            Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay()
            Me.TabPage2 = New System.Windows.Forms.TabPage()
            Me.CogImageFileEdit1 = New Cognex.VisionPro.ImageFile.CogImageFileEditV2()
            Me.FrameGrabber = New System.Windows.Forms.TabPage()
            Me.CogAcqFifoEdit1 = New Cognex.VisionPro.CogAcqFifoEditV2()
            Me.TabPage3 = New System.Windows.Forms.TabPage()
            Me.CogPMAlignEdit1 = New Cognex.VisionPro.PMAlign.CogPMAlignEditV2()
            Me.ImageAcquisitionCommonDialog = New System.Windows.Forms.OpenFileDialog()
            Me.VProSampleAppTab.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            Me.frmPatMax.SuspendLayout()
            Me.frmImageAcquisitionFrame.SuspendLayout()
            CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabPage2.SuspendLayout()
            CType(Me.CogImageFileEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.FrameGrabber.SuspendLayout()
            CType(Me.CogAcqFifoEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabPage3.SuspendLayout()
            CType(Me.CogPMAlignEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'InfoTxt
            '
            Me.InfoTxt.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.InfoTxt.Location = New System.Drawing.Point(0, 502)
            Me.InfoTxt.Multiline = True
            Me.InfoTxt.Name = "InfoTxt"
            Me.InfoTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.InfoTxt.Size = New System.Drawing.Size(767, 112)
            Me.InfoTxt.TabIndex = 3
            '
            'VProSampleAppTab
            '
            Me.VProSampleAppTab.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.VProSampleAppTab.Controls.Add(Me.TabPage1)
            Me.VProSampleAppTab.Controls.Add(Me.TabPage2)
            Me.VProSampleAppTab.Controls.Add(Me.FrameGrabber)
            Me.VProSampleAppTab.Controls.Add(Me.TabPage3)
            Me.VProSampleAppTab.Location = New System.Drawing.Point(0, 0)
            Me.VProSampleAppTab.Name = "VProSampleAppTab"
            Me.VProSampleAppTab.SelectedIndex = 0
            Me.VProSampleAppTab.Size = New System.Drawing.Size(767, 496)
            Me.VProSampleAppTab.TabIndex = 4
            '
            'TabPage1
            '
            Me.TabPage1.Controls.Add(Me.frmPatMax)
            Me.TabPage1.Controls.Add(Me.frmImageAcquisitionFrame)
            Me.TabPage1.Controls.Add(Me.CogDisplay1)
            Me.TabPage1.Location = New System.Drawing.Point(4, 22)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Size = New System.Drawing.Size(759, 470)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "VisionPro Demo"
            '
            'frmPatMax
            '
            Me.frmPatMax.Controls.Add(Me.Label1)
            Me.frmPatMax.Controls.Add(Me.txtPatMaxScoreValue)
            Me.frmPatMax.Controls.Add(Me.cmdPatMaxRunCommand)
            Me.frmPatMax.Controls.Add(Me.cmdPatMaxSetupCommand)
            Me.frmPatMax.Location = New System.Drawing.Point(0, 168)
            Me.frmPatMax.Name = "frmPatMax"
            Me.frmPatMax.Size = New System.Drawing.Size(392, 128)
            Me.frmPatMax.TabIndex = 2
            Me.frmPatMax.TabStop = False
            Me.frmPatMax.Text = "Pat Max"
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(232, 56)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(48, 32)
            Me.Label1.TabIndex = 3
            Me.Label1.Text = "Score"
            '
            'txtPatMaxScoreValue
            '
            Me.txtPatMaxScoreValue.Location = New System.Drawing.Point(296, 40)
            Me.txtPatMaxScoreValue.Multiline = True
            Me.txtPatMaxScoreValue.Name = "txtPatMaxScoreValue"
            Me.txtPatMaxScoreValue.Size = New System.Drawing.Size(80, 40)
            Me.txtPatMaxScoreValue.TabIndex = 2
            '
            'cmdPatMaxRunCommand
            '
            Me.cmdPatMaxRunCommand.Location = New System.Drawing.Point(112, 40)
            Me.cmdPatMaxRunCommand.Name = "cmdPatMaxRunCommand"
            Me.cmdPatMaxRunCommand.Size = New System.Drawing.Size(104, 48)
            Me.cmdPatMaxRunCommand.TabIndex = 1
            Me.cmdPatMaxRunCommand.Text = "Run"
            '
            'cmdPatMaxSetupCommand
            '
            Me.cmdPatMaxSetupCommand.Location = New System.Drawing.Point(8, 40)
            Me.cmdPatMaxSetupCommand.Name = "cmdPatMaxSetupCommand"
            Me.cmdPatMaxSetupCommand.Size = New System.Drawing.Size(96, 48)
            Me.cmdPatMaxSetupCommand.TabIndex = 0
            Me.cmdPatMaxSetupCommand.Text = "Setup"
            '
            'frmImageAcquisitionFrame
            '
            Me.frmImageAcquisitionFrame.Controls.Add(Me.cmdImageAcquisitionNewImageCommand)
            Me.frmImageAcquisitionFrame.Controls.Add(Me.cmdImageAcquisitionLiveOrOpenCommand)
            Me.frmImageAcquisitionFrame.Controls.Add(Me.optImageAcquisitionOptionImageFile)
            Me.frmImageAcquisitionFrame.Controls.Add(Me.optImageAcquisitionOptionFrameGrabber)
            Me.frmImageAcquisitionFrame.Location = New System.Drawing.Point(8, 8)
            Me.frmImageAcquisitionFrame.Name = "frmImageAcquisitionFrame"
            Me.frmImageAcquisitionFrame.Size = New System.Drawing.Size(384, 136)
            Me.frmImageAcquisitionFrame.TabIndex = 1
            Me.frmImageAcquisitionFrame.TabStop = False
            Me.frmImageAcquisitionFrame.Text = "Image Acquisition"
            '
            'cmdImageAcquisitionNewImageCommand
            '
            Me.cmdImageAcquisitionNewImageCommand.Location = New System.Drawing.Point(280, 48)
            Me.cmdImageAcquisitionNewImageCommand.Name = "cmdImageAcquisitionNewImageCommand"
            Me.cmdImageAcquisitionNewImageCommand.Size = New System.Drawing.Size(75, 40)
            Me.cmdImageAcquisitionNewImageCommand.TabIndex = 3
            Me.cmdImageAcquisitionNewImageCommand.Text = "Next Image"
            '
            'cmdImageAcquisitionLiveOrOpenCommand
            '
            Me.cmdImageAcquisitionLiveOrOpenCommand.Location = New System.Drawing.Point(152, 48)
            Me.cmdImageAcquisitionLiveOrOpenCommand.Name = "cmdImageAcquisitionLiveOrOpenCommand"
            Me.cmdImageAcquisitionLiveOrOpenCommand.Size = New System.Drawing.Size(75, 40)
            Me.cmdImageAcquisitionLiveOrOpenCommand.TabIndex = 2
            Me.cmdImageAcquisitionLiveOrOpenCommand.Text = "Open File"
            '
            'optImageAcquisitionOptionImageFile
            '
            Me.optImageAcquisitionOptionImageFile.Checked = True
            Me.optImageAcquisitionOptionImageFile.Location = New System.Drawing.Point(24, 64)
            Me.optImageAcquisitionOptionImageFile.Name = "optImageAcquisitionOptionImageFile"
            Me.optImageAcquisitionOptionImageFile.Size = New System.Drawing.Size(104, 24)
            Me.optImageAcquisitionOptionImageFile.TabIndex = 1
            Me.optImageAcquisitionOptionImageFile.TabStop = True
            Me.optImageAcquisitionOptionImageFile.Text = "Image File"
            '
            'optImageAcquisitionOptionFrameGrabber
            '
            Me.optImageAcquisitionOptionFrameGrabber.Location = New System.Drawing.Point(24, 32)
            Me.optImageAcquisitionOptionFrameGrabber.Name = "optImageAcquisitionOptionFrameGrabber"
            Me.optImageAcquisitionOptionFrameGrabber.Size = New System.Drawing.Size(104, 24)
            Me.optImageAcquisitionOptionFrameGrabber.TabIndex = 0
            Me.optImageAcquisitionOptionFrameGrabber.Text = "Frame Grabber"
            '
            'CogDisplay1
            '
            Me.CogDisplay1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.CogDisplay1.ColorMapLowerClipColor = System.Drawing.Color.Black
            Me.CogDisplay1.ColorMapLowerRoiLimit = 0.0R
            Me.CogDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None
            Me.CogDisplay1.ColorMapUpperClipColor = System.Drawing.Color.Black
            Me.CogDisplay1.ColorMapUpperRoiLimit = 1.0R
            Me.CogDisplay1.Location = New System.Drawing.Point(400, 16)
            Me.CogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
            Me.CogDisplay1.MouseWheelSensitivity = 1.0R
            Me.CogDisplay1.Name = "CogDisplay1"
            Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
            Me.CogDisplay1.Size = New System.Drawing.Size(356, 454)
            Me.CogDisplay1.TabIndex = 0
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.CogImageFileEdit1)
            Me.TabPage2.Location = New System.Drawing.Point(4, 22)
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.Size = New System.Drawing.Size(759, 470)
            Me.TabPage2.TabIndex = 2
            Me.TabPage2.Text = "Image File"
            '
            'CogImageFileEdit1
            '
            Me.CogImageFileEdit1.AllowDrop = True
            Me.CogImageFileEdit1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CogImageFileEdit1.Location = New System.Drawing.Point(0, 0)
            Me.CogImageFileEdit1.MinimumSize = New System.Drawing.Size(489, 0)
            Me.CogImageFileEdit1.Name = "CogImageFileEdit1"
            Me.CogImageFileEdit1.OutputHighLight = System.Drawing.Color.Lime
            Me.CogImageFileEdit1.Size = New System.Drawing.Size(759, 470)
            Me.CogImageFileEdit1.SuspendElectricRuns = False
            Me.CogImageFileEdit1.TabIndex = 0
            '
            'FrameGrabber
            '
            Me.FrameGrabber.Controls.Add(Me.CogAcqFifoEdit1)
            Me.FrameGrabber.Location = New System.Drawing.Point(4, 22)
            Me.FrameGrabber.Name = "FrameGrabber"
            Me.FrameGrabber.Size = New System.Drawing.Size(759, 470)
            Me.FrameGrabber.TabIndex = 1
            Me.FrameGrabber.Text = "FrameGrabber"
            '
            'CogAcqFifoEdit1
            '
            Me.CogAcqFifoEdit1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CogAcqFifoEdit1.Location = New System.Drawing.Point(0, 0)
            Me.CogAcqFifoEdit1.MinimumSize = New System.Drawing.Size(489, 0)
            Me.CogAcqFifoEdit1.Name = "CogAcqFifoEdit1"
            Me.CogAcqFifoEdit1.Size = New System.Drawing.Size(759, 470)
            Me.CogAcqFifoEdit1.SuspendElectricRuns = False
            Me.CogAcqFifoEdit1.TabIndex = 0
            '
            'TabPage3
            '
            Me.TabPage3.Controls.Add(Me.CogPMAlignEdit1)
            Me.TabPage3.Location = New System.Drawing.Point(4, 22)
            Me.TabPage3.Name = "TabPage3"
            Me.TabPage3.Size = New System.Drawing.Size(759, 470)
            Me.TabPage3.TabIndex = 3
            Me.TabPage3.Text = "PatMax"
            '
            'CogPMAlignEdit1
            '
            Me.CogPMAlignEdit1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CogPMAlignEdit1.Location = New System.Drawing.Point(0, 0)
            Me.CogPMAlignEdit1.MinimumSize = New System.Drawing.Size(489, 0)
            Me.CogPMAlignEdit1.Name = "CogPMAlignEdit1"
            Me.CogPMAlignEdit1.Size = New System.Drawing.Size(759, 470)
            Me.CogPMAlignEdit1.SuspendElectricRuns = False
            Me.CogPMAlignEdit1.TabIndex = 0
            '
            'frmPatInspSamp
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(767, 614)
            Me.Controls.Add(Me.VProSampleAppTab)
            Me.Controls.Add(Me.InfoTxt)
            Me.Name = "frmPatInspSamp"
            Me.Text = "PMAlign  Sample"
            Me.VProSampleAppTab.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            Me.frmPatMax.ResumeLayout(False)
            Me.frmPatMax.PerformLayout()
            Me.frmImageAcquisitionFrame.ResumeLayout(False)
            CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabPage2.ResumeLayout(False)
            CType(Me.CogImageFileEdit1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.FrameGrabber.ResumeLayout(False)
            CType(Me.CogAcqFifoEdit1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabPage3.ResumeLayout(False)
            CType(Me.CogPMAlignEdit1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region
#Region "Module Level vars"

    Dim ImageFileTool As CogImageFileTool
    Dim PatMaxTool As CogPMAlignTool
    Dim AcqFifoTool As CogAcqFifoTool
    'Flag for "VisionPro Demo" tab indicating that user is currently setting up a
    'tool.  Also used to indicate in live video mode.  If user selects "Setup"
    'then the GUI controls are disabled except for the interactive graphics
    'required for setup as well as the "OK" button used to complete the Setup.
    Dim SettingUp As Boolean

    ' values passed to EnableAll & DisableAll subroutines which
    'indicates what is being setup thus determining which Buttons on the GUI
    'should be left enabled.
    Enum SettingUpConstants As Integer
      settingUpPatMax
      settingLiveVideo
    End Enum

    Dim settingUpPatMax As SettingUpConstants = SettingUpConstants.settingUpPatMax
    Dim settingLiveVideo As SettingUpConstants = SettingUpConstants.settingLiveVideo
#End Region
#Region "Form and Controls events"
    Private Sub frmPatInspSamp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


      SettingUp = False
      InfoTxt.Text = "This sample demonstrates the use of PMAlign to train and locate a " & _
      "pattern in a user provided image." & Environment.NewLine & _
      "The user should execute the following steps:" & Environment.NewLine & _
      "1. Select an image source: either an image file or a frame grabber." & Environment.NewLine & _
      "2. Grab an image from the image source." & Environment.NewLine & _
      "3. Click 'Setup' in the PatMax frame.  Select a training region. Hit 'OK'." & Environment.NewLine & _
      "4. Click 'Run' in the PatMax frame to see the location result and score." & Environment.NewLine & _
      "5. Click 'Next Image' followed by 'Run' to locate the pattern on a subsequent image." & Environment.NewLine & _
      "Note that execution parameters can be changed by selecting the appropriate tab and " & _
      "modifying the provided values."


      'Set reference to CogImageFileTool created by Edit Control
      'The Image File Edit Control creates its subject when its AutoCreateTool property is True
      ImageFileTool = CogImageFileEdit1.Subject
      AddHandler ImageFileTool.Ran, AddressOf ImageFileTool_Ran
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
      ImageAcquisitionCommonDialog.CheckFileExists = True
      ImageAcquisitionCommonDialog.ReadOnlyChecked = True

      'AutoCreateTool for the PMAlign edit control is False, therefore, we must create
      'a PMAlign tool and set the subject of the control to reference the new tool.
      PatMaxTool = New CogPMAlignTool
      AddHandler PatMaxTool.Changed, AddressOf PatMaxTool_Changed
      CogPMAlignEdit1.Subject = PatMaxTool

      'Change the default Train Region to center of a 640x480 image & change the DOFs
      'so that Skew is not enabled.  Note - TrainRegion is of type ICogRegion, therefore,
      'we must use a CogRectangleAffine reference in order to call CogRectangleAffine
      'properties.
      Dim PatMaxTrainRegion As CogRectangleAffine
      PatMaxTrainRegion = PatMaxTool.Pattern.TrainRegion
      If Not PatMaxTrainRegion Is Nothing Then
        PatMaxTrainRegion.SetCenterLengthsRotationSkew(320, 240, 100, 100, 0, 0)
        PatMaxTrainRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position Or _
         CogRectangleAffineDOFConstants.Rotation Or _
        CogRectangleAffineDOFConstants.Size
      End If

      'Create a SearchRegion that uses the entire image (assumes 640x480)
      'Note that by default the SearchRegion is Nothing and PMAlign will search the entire
      'image anyway.  This is added for sample code purposes & to graphically show that the
      'entire image is being used.
      Dim PatMaxSearchRegion As New CogRectangle
      PatMaxTool.SearchRegion = PatMaxSearchRegion
      PatMaxSearchRegion.SetCenterWidthHeight(320, 240, 640, 480)
      PatMaxSearchRegion.GraphicDOFEnable = CogRectangleDOFConstants.Position Or _
      CogRectangleDOFConstants.Size
      PatMaxSearchRegion.Interactive = True
    End Sub



    Private Sub cmdImageAcquisitionLiveOrOpenCommand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdImageAcquisitionLiveOrOpenCommand.Click
      'Clear graphics, assuming a new image will be in the display once user
      'completes either Live Video or Open File operation, therefore, graphics
      'will be out of sync.
      CogDisplay1.StaticGraphics.Clear()
      CogDisplay1.InteractiveGraphics.Clear()

      '"Live Video"  & "Stop Live" button when Frame Grabber option is selected.
      'Using our EnableAll & DisableAll subroutine to force the user stop live
      'video before doing anything else.
      If optImageAcquisitionOptionFrameGrabber.Checked = True Then
        If CogDisplay1.LiveDisplayRunning Then
          CogDisplay1.StopLiveDisplay()
          EnableAll(settingLiveVideo)
          'Run the AcqFifoTool so that all of the sample app images get the last
          'image from Live Video (see AcqFifoTool_PostRun)
          AcqFifoTool.Run()
        ElseIf Not AcqFifoTool.[Operator] Is Nothing Then
          CogDisplay1.StartLiveDisplay(AcqFifoTool.[Operator])
          DisableAll(settingLiveVideo)
        End If

      Else
        '"Open File" button when image file option is selected
        'DrawingEnabled is used to simply hide the image while the Fit is performed.
        'This prevents the image from being diplayed at the initial zoom factor
        'prior to fit being called.
        Try
          Dim result As DialogResult = ImageAcquisitionCommonDialog.ShowDialog()
          If result <> Windows.Forms.DialogResult.Cancel Then
            ImageFileTool.[Operator].Open(ImageAcquisitionCommonDialog.FileName, CogImageFileModeConstants.Read)
            CogDisplay1.DrawingEnabled = False
            ImageFileTool.Run()
            CogDisplay1.Fit()
            CogDisplay1.DrawingEnabled = True
          End If
        Catch cogex As CogException
          MessageBox.Show("Following Specific Cognex Error Occured:" & cogex.Message)
        Catch ex As Exception
          MessageBox.Show("Following Error Occured:" & ex.Message)
        End Try
      End If
    End Sub
    Private Sub cmdImageAcquisitionNewImageCommand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdImageAcquisitionNewImageCommand.Click
      If optImageAcquisitionOptionFrameGrabber.Checked = True Then
        AcqFifoTool.Run()
      Else
        ImageFileTool.Run()
      End If
    End Sub

    Private Sub cmdPatMaxRunCommand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPatMaxRunCommand.Click
      CogDisplay1.InteractiveGraphics.Clear()
      CogDisplay1.StaticGraphics.Clear()
      PatMaxTool.Run()
      If Not PatMaxTool.RunStatus.Exception Is Nothing Then
        MessageBox.Show(PatMaxTool.RunStatus.Exception.Message, "PatMax Run Error")
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
        'While setting up PMAlign, disable other GUI controls.
        SettingUp = True
        DisableAll(settingUpPatMax)
        'Add TrainRegion to display's interactive graphics
        'Add SearchRegion to display's static graphics for display only.
        CogDisplay1.InteractiveGraphics.Clear()
        CogDisplay1.StaticGraphics.Clear()
        CogDisplay1.InteractiveGraphics.Add(PatMaxTool.Pattern.TrainRegion, "test", False)
        If Not PatMaxTool.SearchRegion Is Nothing Then
          CogDisplay1.StaticGraphics.Add(PatMaxTool.SearchRegion, "test")
        End If

      Else  'OK has been pressed, completing Setup.
        SettingUp = False
        CogDisplay1.InteractiveGraphics.Clear()
        CogDisplay1.StaticGraphics.Clear()
        'Make sure we catch errors from Train, since they are likely.  For example,
        'No InputImage, No Pattern Features, etc.
        Try
          PatMaxTool.Pattern.Train()
        Catch cogex As CogException
          MessageBox.Show("Following Specific Cognex Error Occured:" & cogex.Message)
        Catch ex As Exception

          MessageBox.Show(ex.Message, "PatMax Setup Error")
        End Try
        EnableAll(settingUpPatMax)
      End If
    End Sub
    Private Sub frmPatInspSamp_closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed

      RemoveHandler PatMaxTool.Changed, AddressOf PatMaxTool_Changed
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
      If butThis = settingUpPatMax Then
        frmPatMax.Enabled = True
        cmdPatMaxSetupCommand.Text = "OK"
        cmdPatMaxRunCommand.Enabled = False
      ElseIf butThis = settingLiveVideo Then
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
      If butThis = settingUpPatMax Then
        cmdPatMaxSetupCommand.Text = "Setup"
        cmdPatMaxRunCommand.Enabled = True
      ElseIf butThis = settingLiveVideo Then
        cmdImageAcquisitionLiveOrOpenCommand.Text = "Live Video"
        cmdImageAcquisitionNewImageCommand.Enabled = True
        optImageAcquisitionOptionFrameGrabber.Enabled = True
        optImageAcquisitionOptionImageFile.Enabled = True
      End If
    End Sub
#End Region
#Region "Cognex Objects Events"


    'Pass AcqFifo OutputImage to the PatMax tool & the Display on "VisionPro" tab.
    'Also, pass OutputImage to the InputImage of ImageFile tool so that this
    'sample application can be used to Record a image file from frame grabber.
        Private Sub AcqFifoTool_Ran(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles AcqFifoTool.Ran

            Static numacqs As Integer
            CogDisplay1.InteractiveGraphics.Clear()
            CogDisplay1.StaticGraphics.Clear()
            CogDisplay1.Image = AcqFifoTool.OutputImage
            PatMaxTool.InputImage = TryCast(AcqFifoTool.OutputImage, CogImage8Grey)
            ImageFileTool.InputImage = AcqFifoTool.OutputImage
            ' Run the garbage collector to free unused images
            numacqs += 1
            If numacqs > 4 Then
                GC.Collect()
                numacqs = 0
            End If

        End Sub

        Private Sub ImageFileTool_Ran(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles ImageFileTool.Ran
            CogDisplay1.InteractiveGraphics.Clear()
            CogDisplay1.StaticGraphics.Clear()
            CogDisplay1.Image = ImageFileTool.OutputImage
            PatMaxTool.InputImage = ImageFileTool.OutputImage
        End Sub
        'If PMAlign results have changed then update the Score & Region graphic.
        Private Sub PatMaxTool_Changed(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogChangedEventArgs) 'Handles PatMaxTool.Changed
            'If FunctionalArea And cogFA_Tool_Results Then
            If Cognex.VisionPro.Implementation.CogToolBase.SfCreateLastRunRecord Or Cognex.VisionPro.Implementation.CogToolBase.SfRunStatus Then
                CogDisplay1.StaticGraphics.Clear()
                'Note, Results will be nothing if Run failed.
                If PatMaxTool.Results Is Nothing Then
                    txtPatMaxScoreValue.Text = "N/A"
                ElseIf PatMaxTool.Results.Count > 0 Then
                    'Passing result does not imply Pattern is found, must check count.
                    txtPatMaxScoreValue.Text = Math.Round(PatMaxTool.Results(0).Score, 3)
                    txtPatMaxScoreValue.Refresh()
                    Dim resultGraphics As CogCompositeShape
                    resultGraphics = PatMaxTool.Results(0).CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchRegion)
                    CogDisplay1.InteractiveGraphics.Add(resultGraphics, "test", False)
                Else
                    txtPatMaxScoreValue.Text = "N/A"
                End If
            End If
        End Sub
#End Region

  End Class
End Namespace