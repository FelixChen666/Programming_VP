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

' This sample demonstrates how to Fixture a Blob Tool using a PMAlign Tool and a
' Fixture Tool.  The sample code will detect the presence of the upper right hole
' in the bracket.  When the Run button is clicked a new image is read.  The PMAlign
' Tool is run and creates a POSE as an output.  The POSE is a 6 degree of
' freedom transform that describes the transformation from runtime coordinate
' space to training time coordinate space.   A POSE consists of TranslationX,
' TranslationY, rotation, Scaling, ScalingX, and ScalingY.
' The output POSE of the PMAlign Tool is then given to the Fixture Tool.
' The Fixture Tool takes the POSE information from the PMAlign Tool and the
' new image and creates a new output image.  The output image from the Fixture Tool
' is then given to the Blob Tool.
'
' The three tools are created externally to this application and are loaded in during
' initialization(Form_Load).  The PMAlign Tool is train to use the whole bracket as
' the Train Image.  After the PMAilgn Tool runs the resulting POSE is given to the
' Fixture Tool.  The Fixture Tool takes the POSE information from PMAlign and the
' image from the Image File Tool and creates a new image then is passed to the Blob
' Tool.  The new image from the Fixture Tool has been corrected for the new location
' of the bracket.  The Blob Tool has its region of interest around the top right hole.
' When the Blob Tool runs the results are checked to see if a blob was detected.  If
' you want to check out the individual tool designs including setup parameter.
' go up one directory and load fixture_pmalign_tool_group.vpp in QuickStart.
'
' This program assumes that you have some knowledge of Visual Basic programming.
'
' Intialization(Form_Load):
'   Step 1) Load the tools, PMAlign, Fixture and Blob
'
' Run Button (runCmd_Click):
'   Step 1) Clear the Static graphics from the previous run
'   Step 2) Get a new image
'   Step 3) Run the PMAlign Tool and check it's results.  Plot result Graphics.
'   Step 4) Run the Fixture Tool and check it's results.
'   Step 5) Run the Blob Tool and check it's results.  Plot result Graphics.
'   Step 6) Display new image
'   Step 7) Display result graphics

Option Explicit On 
' needed for VisionPro
Imports Cognex.VisionPro
' needed for displaying
Imports Cognex.VisionPro.Display
' needed for image processing
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.ImageProcessing
Imports Cognex.VisionPro.PMAlign
Imports Cognex.VisionPro.Blob
Imports Cognex.VisionPro.CalibFix
Imports Cognex.VisionPro.Caliper
' needed for VisionPro exceptions
Imports Cognex.VisionPro.Exceptions

Namespace FixtureAndPMAlign

  Public Class FixtureAndPMAlign
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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FixtureAndPMAlign))
      Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
      Me.SampleDescription = New System.Windows.Forms.TextBox
      Me.runCmd = New System.Windows.Forms.Button
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
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
      Me.SampleDescription.Size = New System.Drawing.Size(568, 56)
      Me.SampleDescription.TabIndex = 1
      Me.SampleDescription.Text = "Sample Usage: demonstrate how to Fixture a Blob tool with a PMAlign Tool to detec" & _
      "t a hole. " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Sample Usage: Click the Run Button                " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "and the applicat" & _
      "ion will find the upper right hole on the bracket."
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
      'FixtureAndPMAlign
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(856, 462)
      Me.Controls.Add(Me.runCmd)
      Me.Controls.Add(Me.SampleDescription)
      Me.Controls.Add(Me.CogDisplay1)
      Me.Name = "FixtureAndPMAlign"
      Me.Text = "PMAlign and Fixture Sample Code"
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region " Private Vars "
    Private imageFileTool As CogImageFileTool
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
        Dim FixtureCaliperToolsVPPFiles As String
        FixtureCaliperToolsVPPFiles = strBaseDir & "/Samples/Programming/Fixture/FixtureAndPMAlign/"
        pmAlignTool = CType(CogSerializer.LoadObjectFromFile(FixtureCaliperToolsVPPFiles & "pmalign_tool.vpp"), CogPMAlignTool)
        fixtureTool = CType(CogSerializer.LoadObjectFromFile(FixtureCaliperToolsVPPFiles & "fixture_tool.vpp"), CogFixtureTool)
        blobTool = CType(CogSerializer.LoadObjectFromFile(FixtureCaliperToolsVPPFiles & "blob_tool.vpp"), CogBlobTool)


    End Sub
#End Region
#Region " 'Run' command click handler"
    Private Sub runCmd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles runCmd.Click

      Try
        ' Step 1) Clear Static Graphics
        CogDisplay1.StaticGraphics.Clear()

        ' Step 2) Get another image from data base
        imageFileTool.Run()

        ' Step 3) Run PMAlign Tool and check its results.
        pmAlignTool.InputImage = CType(imageFileTool.OutputImage, CogImage8Grey)
        pmAlignTool.Run()
        If pmAlignTool.RunStatus.Result <> CogToolResultConstants.Accept Then
          Throw pmAlignTool.RunStatus.Exception
        End If
        If pmAlignTool.Results.Count = 0 Then _
          Throw New Exception("No PatMax Results were found.")

        ' Step 4) Run Fixture Tool and check its results.
        fixtureTool.InputImage = imageFileTool.OutputImage
        ' Set entire transform (including scale and skew)
        fixtureTool.RunParams.UnfixturedFromFixturedTransform = pmAlignTool.Results(0).GetPose
        fixtureTool.Run()
        If fixtureTool.RunStatus.Result <> CogToolResultConstants.Accept Then
          Throw fixtureTool.RunStatus.Exception
        End If
        ' Step 5) Run Blob Tool and check its results.
        blobTool.InputImage = CType(fixtureTool.OutputImage, CogImage8Grey)
        blobTool.Run()
        If blobTool.RunStatus.Result <> CogToolResultConstants.Accept Then
          Throw blobTool.RunStatus.Exception
        End If
        If blobTool.Results.GetBlobs.Count = 0 Then
          Throw New Exception("No Blobs Results were found.")
        End If
        ' Step 6) Display new image
        CogDisplay1.Image = imageFileTool.OutputImage

        ' Step 7) Display result graphics
        CogDisplay1.StaticGraphics.Add _
         (blobTool.Results.GetBlobs.Item(0).CreateResultGraphics(CogBlobResultGraphicConstants.Boundary Or CogBlobResultGraphicConstants.CenterOfMass), "")
        CogDisplay1.StaticGraphics.Add(pmAlignTool.Results(0).CreateResultGraphics(CogPMAlignResultGraphicConstants.CoordinateAxes), "")
      Catch ex As CogException
        DisplayErrorAndExit("Tool Load Error: " & ex.Message)
      Catch gex As Exception
        DisplayErrorAndExit("Tool Load Error: " & gex.Message)
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
      MsgBox(ErrorMsg & vbCr & "Press OK to exit.")
      Application.Exit()
    End Sub
#End Region

    Private Sub FixtureAndPMAlign_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Try
        Initialization()
      Catch ex As CogException
        DisplayErrorAndExit("Tool Load Error: " & ex.Message)
      Catch gex As Exception
        DisplayErrorAndExit("Tool Load Error: " & gex.Message)
      End Try

    End Sub
  End Class
End Namespace
