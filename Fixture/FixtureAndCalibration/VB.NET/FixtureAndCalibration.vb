'*******************************************************************************
' Copyright (C) 2004 Cognex Corporation
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

' This sample demonstrates how to Fixture a Blob Tool using a PMAlign Tool, a
' Fixture Tool and a Calibration Tool.  The sample code will detect the presence
' of the upper right hole in the bracket.  When the Run button is clicked a new
' image is read. The Calibration Tool is run. The output of the Calibration Tool
' is given to the PMAlign Tool.  The PMAlignTool is run.  The output POSE of the
' PMAlign Tool is given to the Fixture Tool.  The Fixture Tool takes the POSE
' information from the PMAlign Tool and the new image and creates a new (fixtured)
' ouput image.  The output image from the Fixture Tool is then given to the Blob Tool,
' and helps the Blob Tool to easily locate the upper right hole in the bracket.
' The results will now be displayed in the calibrated space, that is in real world
' coordinates that were given to the nPointToNPoint Calibration Tool.
'
' Note: The actual units are not required by the NPointToNPoint Calibration Tool. The
'       tool only cares about the values of the real world positions and how good
'       of a fit it can calculate based on those positions.
'
' The four tools are created externally to this application and are loaded in during
' initialization(Form_Load).  The individual tools designs can be examine using QuickStart
' and are located one directory above.
'
' Calibration Tool
' The Calibration Tool is a NPointToNPoint Calibration Tool.  It uses three holes
' in the bracket for calibration.  A set of real world dimensions are then given to the
' tool.  The tool is calibrated once during the QuickStart phase.  The archive for the
' calibration tool is calibration_tool.vpp.
'
' PMAlign Tool
' The PMAlign Tool is trained to use the whole bracket as the Train Image. After the
' Calibration Tool is run the output is given to the PMAlign Tool.  The PMAlign Tool
' is set for rotation but no scale.  The archive for the calibration tool is
' pmalign_tool.vpp.
'
' Fixture Tool
' The Fixture Tool takes the POSE information from PMAlign Tool and the image from the Image
' File Tool and creates a new (fixtured) image that is then passed to the BlobTool.  The new
' image from the Fixture Tool has been corrected for the new location of the bracket.
' The archive for the Fixture Tool is fixture_tool.vpp.
'
' Blob Tool
' The Blob Tool has its region of interest around the top right hole of the bracket.  The
' Blob Tool is setup for a 10 pixels minimum.  The threshold is set for Hard Dynamic and
' dark blobs on a light background.  The archive of the Blob Tool is blob_tool.vpp.
' and load.
'
' If you want to run the complete tool design from QuickStart, then load
' fixture_pmalign_tool_group.vpp.
'
' This program assumes that you have some knowledge of Visual Basic programming.
'
' Intialization(Form_Load):
'   Step 1) Load the tools, Calibration, PMAlign, Fixture and Blob
'
' Run Button (runCmd_Click):
'   Step 1) Clear the Static graphics from the previous run
'   Step 2) Get a new image
'   Step 3) Run the Calibration Tool and check its results
'   Step 4) Run the PMAlign Tool and check its results.  Plot result Graphics.
'   Step 5) Run the Fixture Tool and check its results.
'   Step 6) Run the Blob Tool and check its results.  Plot result Graphics.
Option Explicit On 
' Needed for VisionPro
Imports Cognex.VisionPro
' Needed for VisionPro Image Processing
Imports Cognex.VisionPro.ImageFile
' Needed for VisionPro Image Processing
Imports Cognex.VisionPro.ImageProcessing
' Needed for VisionPro Image Processing
Imports Cognex.VisionPro.PMAlign
' Needed for VisionPro Image Processing
Imports Cognex.VisionPro.Blob
' Needed for VisionPro Image Processing
Imports Cognex.VisionPro.CalibFix
' Needed for displaying
Imports Cognex.VisionPro.Display
' Needed for VisionPro Exceptions
Imports Cognex.VisionPro.Exceptions

Namespace FixtureAndCalibration

 Public Class FixtureAndCalibration
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
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Friend WithEvents CogDisplay1 As Cognex.VisionPro.Display.CogDisplay
  Friend WithEvents SampleDescription As System.Windows.Forms.TextBox
  Friend WithEvents runCmd As System.Windows.Forms.Button
  Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents Label4 As System.Windows.Forms.Label
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FixtureAndCalibration))
    Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
    Me.SampleDescription = New System.Windows.Forms.TextBox
    Me.runCmd = New System.Windows.Forms.Button
    Me.GroupBox1 = New System.Windows.Forms.GroupBox
    Me.Label4 = New System.Windows.Forms.Label
    Me.Label3 = New System.Windows.Forms.Label
    Me.Label2 = New System.Windows.Forms.Label
    Me.Label1 = New System.Windows.Forms.Label
    CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupBox1.SuspendLayout()
    Me.SuspendLayout()
    '
    'CogDisplay1
    '
    Me.CogDisplay1.Location = New System.Drawing.Point(16, 16)
    Me.CogDisplay1.Name = "CogDisplay1"
    Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
    Me.CogDisplay1.Size = New System.Drawing.Size(568, 360)
    Me.CogDisplay1.TabIndex = 0
    '
    'SampleDescription
    '
    Me.SampleDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.SampleDescription.Location = New System.Drawing.Point(16, 392)
    Me.SampleDescription.Multiline = True
    Me.SampleDescription.Name = "SampleDescription"
    Me.SampleDescription.Size = New System.Drawing.Size(568, 40)
    Me.SampleDescription.TabIndex = 1
    Me.SampleDescription.Text = "Sample description: demonstrate how to Fixture a Blob tool with a PMAlign Tool to" & _
    " detect a hole with calibration." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Sample usage: Click the Run Button"
    Me.SampleDescription.WordWrap = False
    '
    'runCmd
    '
    Me.runCmd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.runCmd.Location = New System.Drawing.Point(640, 56)
    Me.runCmd.Name = "runCmd"
    Me.runCmd.Size = New System.Drawing.Size(128, 40)
    Me.runCmd.TabIndex = 2
    Me.runCmd.Text = "Run"
    '
    'GroupBox1
    '
    Me.GroupBox1.Controls.Add(Me.Label4)
    Me.GroupBox1.Controls.Add(Me.Label3)
    Me.GroupBox1.Controls.Add(Me.Label2)
    Me.GroupBox1.Controls.Add(Me.Label1)
    Me.GroupBox1.Location = New System.Drawing.Point(616, 152)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(208, 128)
    Me.GroupBox1.TabIndex = 4
    Me.GroupBox1.TabStop = False
    Me.GroupBox1.Text = "Hole Position"
    '
    'Label4
    '
    Me.Label4.Location = New System.Drawing.Point(72, 80)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(80, 23)
    Me.Label4.TabIndex = 4
    Me.Label4.Text = "0"
    '
    'Label3
    '
    Me.Label3.Location = New System.Drawing.Point(72, 32)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(80, 23)
    Me.Label3.TabIndex = 3
    Me.Label3.Text = "0"
    '
    'Label2
    '
    Me.Label2.Location = New System.Drawing.Point(24, 80)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(24, 23)
    Me.Label2.TabIndex = 1
    Me.Label2.Text = "Y:"
    '
    'Label1
    '
    Me.Label1.Location = New System.Drawing.Point(24, 32)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(24, 23)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "X:"
    '
    'FixtureAndCalibration
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(856, 446)
    Me.Controls.Add(Me.GroupBox1)
    Me.Controls.Add(Me.runCmd)
    Me.Controls.Add(Me.SampleDescription)
    Me.Controls.Add(Me.CogDisplay1)
    Me.Name = "FixtureAndCalibration"
    Me.Text = "FixtureAndCalibration"
    CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupBox1.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region
#Region " Private Vars "
  Private imageFileTool As CogImageFileTool
  Private calibrationTool As CogCalibNPointToNPointTool
  Private pmAlignTool As CogPMAlignTool
  Private fixtureTool As CogFixtureTool
  Private blobTool As CogBlobTool
#End Region
#Region " Initialization"
  Private Sub Initialization()
    ' Get VPRO_ROOT from environment which is needed to locate bracket_std.idb.
    Const ImageFileName As String = "/Images/bracket_std.idb"
    Dim strBaseDir As String

      strBaseDir = Environ("VPRO_ROOT")
      If strBaseDir = "" Then
        Throw New Exception("Required environment variable VPRO_ROOT not set.")
      End If
      imageFileTool = New CogImageFileTool
      imageFileTool.[Operator].Open(strBaseDir & ImageFileName, CogImageFileModeConstants.Read)
      imageFileTool.Run()

      ' Step 1) Load the tools
      Dim CalibrationFixtureVPPFiles As String
      CalibrationFixtureVPPFiles = strBaseDir & "/Samples/Programming/Fixture/FixtureAndCalibration/"
      calibrationTool = CType(CogSerializer.LoadObjectFromFile(CalibrationFixtureVPPFiles & "calibration_tool.vpp"), CogCalibNPointToNPointTool)
      pmAlignTool = CType(CogSerializer.LoadObjectFromFile(CalibrationFixtureVPPFiles + "pmalign_tool.vpp"), CogPMAlignTool)
      fixtureTool = CType(CogSerializer.LoadObjectFromFile(CalibrationFixtureVPPFiles + "fixture_tool.vpp"), CogFixtureTool)
      blobTool = CType(CogSerializer.LoadObjectFromFile(CalibrationFixtureVPPFiles + "blob_tool.vpp"), CogBlobTool)

  End Sub
#End Region
#Region " 'Run' command button click handler"
    Private Sub runCmd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles runCmd.Click

      Dim tempImage As CogImage8Grey
      Try
        ' Step 1) Clear Static Graphics
        CogDisplay1.StaticGraphics.Clear()

        ' Step 2) Get another image from data base
        imageFileTool.Run()
        tempImage = CType(imageFileTool.OutputImage, CogImage8Grey)
        CogDisplay1.Image = tempImage

        ' Step 3) Run Calibration Tool and check its results
        calibrationTool.InputImage = tempImage
        calibrationTool.Run()
        If calibrationTool.RunStatus.Result <> CogToolResultConstants.Accept Then
          Throw New Exception(calibrationTool.RunStatus.Message)
        End If
        ' Step 4) Run the PMAlign Tool and check its results.  Plot result Graphics.
        pmAlignTool.InputImage = CType(calibrationTool.OutputImage, CogImage8Grey)
        pmAlignTool.Run()
        If pmAlignTool.RunStatus.Result <> CogToolResultConstants.Accept Then
          Throw pmAlignTool.RunStatus.Exception
        End If
        If pmAlignTool.Results.Count = 0 Then
          Throw New Exception("No PMAlign Results were found.")
        End If
        CogDisplay1.StaticGraphics.Add(pmAlignTool.Results(0).CreateResultGraphics(CogPMAlignResultGraphicConstants.CoordinateAxes), "")
        ' Step 5) Run the Fixture Tool and check its results.
        fixtureTool.InputImage = calibrationTool.OutputImage
        ' Set entire transform (including scale and skew)
        fixtureTool.RunParams.UnfixturedFromFixturedTransform = pmAlignTool.Results(0).GetPose
        fixtureTool.Run()
        If fixtureTool.RunStatus.Result <> CogToolResultConstants.Accept Then
          Throw fixtureTool.RunStatus.Exception
        End If
        ' Step 6) Run the Blob Tool and check its results.  Plot result Graphics.
        blobTool.InputImage = CType(fixtureTool.OutputImage, CogImage8Grey)
        blobTool.Run()
        If blobTool.RunStatus.Result <> CogToolResultConstants.Accept Then
          Throw blobTool.RunStatus.Exception
        End If
        If blobTool.Results.GetBlobs.Count = 0 Then
          Throw New Exception("No Blob Results were found.")
        End If
        Label3.Text = Format(blobTool.Results.GetBlobs.Item(0).CenterOfMassX, "##.##")
        Label4.Text = Format(blobTool.Results.GetBlobs.Item(0).CenterOfMassY, "##.##")
        CogDisplay1.StaticGraphics.Add(blobTool.Results.GetBlobs.Item(0).CreateResultGraphics _
          (CogBlobResultGraphicConstants.Boundary Or CogBlobResultGraphicConstants.CenterOfMass), "")
      Catch ex As CogException
        DisplayErrorAndExit("Tool run Error: " & ex.Message)
      Catch gex As Exception
        DisplayErrorAndExit("Tool run Error: " & gex.Message)
      End Try
    End Sub

#End Region
#Region " Auxiliary subs and functions"
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Helper function.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Displays an error message and then exits the program.
    ' Call this when an unrecoverable error has occurred.
    Private Sub DisplayErrorAndExit(ByVal ErrorMsg As String)
      MessageBox.Show(ErrorMsg & vbCrLf & "Press OK to exit.")
      Application.Exit()
    End Sub
#End Region

    Private Sub FixtureAndCalibration_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Try
        Initialization()
      Catch ex As CogException
        DisplayErrorAndExit("Tool Load Error: " & ex.Message)
      Catch gex As Exception
        DisplayErrorAndExit("Tool Load Error: " & gex.Message)
      End Try
    End Sub
  End Class
end namespace