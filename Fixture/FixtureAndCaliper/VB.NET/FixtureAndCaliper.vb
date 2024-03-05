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

' This sample demonstrates how to Fixture a Blob Tool using a Caliper Tool and a
' Fixture Tool.  The sample code will detect the presence of a small square in the
' upper area of the image.  When the Run button is clicked a new image is read.
' The Caliper Tool is run.  The output Y information from the Caliper Tool is given
' to the Fixture Tool.  The Fixture Tool takes Y information from the Caliper
' Tool and the new image and creates a new ouput image.  The output image from the
' Fixture Tool is then given to the Blob Tool.
'
' WARNING:
' This application will only work correctly if the images are only changing in the
' Y direction.  This is because the caliper tool is only measuring the top edge of the
' image and therefore can only detect Y direction change accurately enough.
'


' The three tools are created externally to this application and are loaded in during
' initialization(Form_Load).  The individual tools designs can be examine using QuickStart
' and are located one directory above.
'
' Caliper Tool
' The Caliper Tool has its search region set for the top edge of the rectangle that contains
' the small white square.  The tool is setup for one edge and is looking from light to
' dark.  The archive is for the Caliper Tool is caliper_tool.vpp.
'
' Fixture Tool
' The Fixture Too takes the POSE information from PMAlign and the image from the Image
' File Tool and creates a new image then is passed to the BlobTool.  The new image from
' the Fixture Tool has been corrected for the new location of the brakcet. The archive
' for the Fixture Tool is fixture_tool.vpp.
'
' Blob Tool
' The Blob Tool has its region of interest around the top right hole of the bracket.  The
' Blob Tool is setup for a 10 pixels minimum.  The threshold is set for Hard Dynamic and
' dark blobs on a light background.  The archive of the Blob Tool is blob_tool.vpp.
' and load.
'
' If you want to run the complete tool design for in QuickStart, then load
' caliper_fixture_group.vpp.
'
' This program assumes that you have some knowledge of Visual Basic programming.
'
' Intialization:
'   Step 1) Load the tools, PMAlign, Fixture and Blob
'
' Run Button (runCmd_Click):
'   Step 1) Clear the Static graphics from the previous run
'   Step 2) Get a new image
'   Step 3) Run the PMAlign Tool and check it's results.  Plot result Graphics.
'   Step 4) Run the Fixture Tool and check it's results.
'   Step 5) Run the Blob Tool and check it's results.  Plot result Graphics.
Option Explicit On 
' needed for VisionPro
Imports Cognex.VisionPro
' needed for displaying
Imports Cognex.VisionPro.Display
' needed for image processing
Imports Cognex.VisionPro.PMAlign
Imports Cognex.VisionPro.Blob
Imports Cognex.VisionPro.CalibFix
Imports Cognex.VisionPro.Caliper
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.ImageProcessing
' needed for VisionPro exceptions
Imports Cognex.VisionPro.Exceptions

Namespace FixtureAndCaliper

  Public Class FixtureAndCaliper
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
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FixtureAndCaliper))
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
      Me.SampleDescription.Text = "Sample description: demonstrate how to Fixture a Blob tool with a Caliper Tool to" & _
      " detect a small square. " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Sample usage: Click the Run Button                " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "an" & _
      "d the application will find the upper right hole on the bracket."
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
      'FixtureAndCaliper
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(856, 462)
      Me.Controls.Add(Me.runCmd)
      Me.Controls.Add(Me.SampleDescription)
      Me.Controls.Add(Me.CogDisplay1)
      Me.Name = "FixtureAndCaliper"
      Me.Text = "CaliperAndFixture"
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region " Private Vars "
    Private imageFileTool As CogImageFileTool
    Private caliperTool As CogCaliperTool
    Private fixtureTool As CogFixtureTool
    Private blobTool As CogBlobTool
#End Region
#Region " Initialization"
    Private Sub Initialization()
      ' Get VPRO_ROOT from environment which is needed to locate bracket_std.idb.
      Const ImageFileName As String = "/Images/square_images.idb"
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
        FixtureCaliperToolsVPPFiles = strBaseDir & "/Samples/Programming/Fixture/FixtureAndCaliper/"
        caliperTool = CType(CogSerializer.LoadObjectFromFile(FixtureCaliperToolsVPPFiles + "caliper_tool.vpp"), CogCaliperTool)
        fixtureTool = CType(CogSerializer.LoadObjectFromFile(FixtureCaliperToolsVPPFiles + "fixture_tool.vpp"), CogFixtureTool)
        blobTool = CType(CogSerializer.LoadObjectFromFile(FixtureCaliperToolsVPPFiles + "blob_tool.vpp"), CogBlobTool)


    End Sub
#End Region
#Region " 'Run' command button click handler"
    Private Sub runCmd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles runCmd.Click
            Dim LinXform As CogTransform2DLinear
            Dim tempImage As CogImage8Grey

      Try
        ' Step 1) Clear Static Graphics

        CogDisplay1.StaticGraphics.Clear()

        ' Step 2) Get another image from data base
        imageFileTool.Run()
        tempImage = CType(imageFileTool.OutputImage, CogImage8Grey)
        CogDisplay1.Image = tempImage

        ' Step 3) Run Caliper Tool and check its results.  Plot result Graphics.
        caliperTool.InputImage = tempImage
        caliperTool.Run()
        If caliperTool.RunStatus.Result <> CogToolResultConstants.Accept Then
          Throw caliperTool.RunStatus.Exception
        End If

        If caliperTool.Results.Count = 0 Then
          Throw New Exception("No edge was found.")
        End If
        CogDisplay1.StaticGraphics.Add(caliperTool.Results(0).CreateResultGraphics(CogCaliperResultGraphicConstants.All), "")

        ' Step 4) Run Fixture Tool and check its results.
        fixtureTool.InputImage = tempImage
        fixtureTool.Run()
        If fixtureTool.RunStatus.Result <> CogToolResultConstants.Accept Then
          Throw fixtureTool.RunStatus.Exception
        End If
        '
        ' Set only the Y (no X, scale or skew)
        '
        LinXform = CType(fixtureTool.RunParams.UnfixturedFromFixturedTransform, CogTransform2DLinear)
        LinXform.TranslationY = caliperTool.Results(0).Edge0.PositionY
        fixtureTool.Run()
        If fixtureTool.RunStatus.Result <> CogToolResultConstants.Accept Then
          Throw fixtureTool.RunStatus.Exception
        End If
        ' Step 5) Run Blob Tool and check its results.  Plot result Graphics.

        blobTool.InputImage = CType(fixtureTool.OutputImage, CogImage8Grey)
        blobTool.Run()
        If blobTool.RunStatus.Result <> CogToolResultConstants.Accept Then
          Throw blobTool.RunStatus.Exception
        End If
        If blobTool.Results.GetBlobs.Count = 0 Then
          Throw New Exception("No PatMax Results were found.")
        End If
        CogDisplay1.StaticGraphics.Add(blobTool.Results.GetBlobs.Item(0).CreateResultGraphics _
         (CogBlobResultGraphicConstants.Boundary Or CogBlobResultGraphicConstants.CenterOfMass), "")
      Catch ex As CogException
        DisplayErrorAndExit("Tool Run Error: " & ex.Message)
      Catch gex As Exception
        DisplayErrorAndExit("Tool Run Error: " & gex.Message)
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
      MsgBox(ErrorMsg & vbCrLf & "Press OK to exit.")
      Application.Exit()
    End Sub
#End Region


    Private Sub FixtureAndCaliper_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
