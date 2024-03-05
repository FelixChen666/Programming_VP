'*******************************************************************************
'Copyright (C) 2004 Cognex Corporation
'
'Subject to Cognex Corporation's terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.
'************************************************************
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

Namespace SamplePatInsp
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
    Friend WithEvents PreInspectDisplay As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents PostInspectDisplay As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents btnInspect As System.Windows.Forms.Button
    Friend WithEvents InfoBox As System.Windows.Forms.TextBox
        Friend WithEvents PreInspectDispStatusBar As Cognex.VisionPro.CogDisplayStatusBarV2
        Friend WithEvents PostInspectDispStatusBar As Cognex.VisionPro.CogDisplayStatusBarV2
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmPatInspSamp))
      Me.PreInspectDisplay = New Cognex.VisionPro.Display.CogDisplay
      Me.PostInspectDisplay = New Cognex.VisionPro.Display.CogDisplay
      Me.btnInspect = New System.Windows.Forms.Button
      Me.InfoBox = New System.Windows.Forms.TextBox
            Me.PreInspectDispStatusBar = New Cognex.VisionPro.CogDisplayStatusBarV2
            Me.PostInspectDispStatusBar = New Cognex.VisionPro.CogDisplayStatusBarV2
      CType(Me.PreInspectDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.PostInspectDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'PreInspectDisplay
      '
      Me.PreInspectDisplay.Location = New System.Drawing.Point(24, 160)
      Me.PreInspectDisplay.Name = "PreInspectDisplay"
      Me.PreInspectDisplay.OcxState = CType(resources.GetObject("PreInspectDisplay.OcxState"), System.Windows.Forms.AxHost.State)
      Me.PreInspectDisplay.Size = New System.Drawing.Size(336, 192)
      Me.PreInspectDisplay.TabIndex = 0
      '
      'PostInspectDisplay
      '
      Me.PostInspectDisplay.Location = New System.Drawing.Point(400, 160)
      Me.PostInspectDisplay.Name = "PostInspectDisplay"
      Me.PostInspectDisplay.OcxState = CType(resources.GetObject("PostInspectDisplay.OcxState"), System.Windows.Forms.AxHost.State)
      Me.PostInspectDisplay.Size = New System.Drawing.Size(320, 192)
      Me.PostInspectDisplay.TabIndex = 1
      '
      'btnInspect
      '
      Me.btnInspect.Location = New System.Drawing.Point(64, 16)
      Me.btnInspect.Name = "btnInspect"
      Me.btnInspect.Size = New System.Drawing.Size(216, 72)
      Me.btnInspect.TabIndex = 2
      Me.btnInspect.Text = "Load Next Image and Inspect"
      '
      'InfoBox
      '
      Me.InfoBox.AcceptsReturn = True
      Me.InfoBox.Location = New System.Drawing.Point(24, 400)
      Me.InfoBox.Multiline = True
      Me.InfoBox.Name = "InfoBox"
      Me.InfoBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
      Me.InfoBox.Size = New System.Drawing.Size(696, 152)
      Me.InfoBox.TabIndex = 3
      Me.InfoBox.Text = ""
      '
      'PreInspectDispStatusBar
      '
      Me.PreInspectDispStatusBar.Enabled = True
      Me.PreInspectDispStatusBar.Location = New System.Drawing.Point(24, 360)
      Me.PreInspectDispStatusBar.Name = "PreInspectDispStatusBar"
            Me.PreInspectDispStatusBar.Size = New System.Drawing.Size(336, 21)
      Me.PreInspectDispStatusBar.TabIndex = 4
      '
      'PostInspectDispStatusBar
      '
      Me.PostInspectDispStatusBar.Enabled = True
      Me.PostInspectDispStatusBar.Location = New System.Drawing.Point(408, 360)
      Me.PostInspectDispStatusBar.Name = "PostInspectDispStatusBar"
            Me.PostInspectDispStatusBar.Size = New System.Drawing.Size(312, 21)
      Me.PostInspectDispStatusBar.TabIndex = 5
      '
      'frmPatInspSamp
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(768, 574)
      Me.Controls.Add(Me.PostInspectDispStatusBar)
      Me.Controls.Add(Me.PreInspectDispStatusBar)
      Me.Controls.Add(Me.InfoBox)
      Me.Controls.Add(Me.btnInspect)
      Me.Controls.Add(Me.PostInspectDisplay)
      Me.Controls.Add(Me.PreInspectDisplay)
      Me.Name = "frmPatInspSamp"
      Me.Text = "PatInspect Sample"
      CType(Me.PreInspectDisplay, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.PostInspectDisplay, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region "Module Level vars"
    Dim trainIDBTool As New CogImageFileTool
    Dim testIDBTool As New CogImageFileTool
    Dim myPMTool As New CogPMAlignTool
    Dim myPITool As New CogPatInspectTool
    Dim inspectRegion As New CogRectangle
    Dim trainOrigin As New CogTransform2DLinear
#End Region
#Region "Form and Controls events"
    Private Sub frmPatInspSamp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Dim VPRO_PATH As String
      VPRO_PATH = Environment.GetEnvironmentVariable("VPRO_ROOT")
      If VPRO_PATH = "" Then
                MessageBox.Show("Required environment variable VPRO_ROOT not set.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End
      End If

      InfoBox.Text = _
        "This example illustates the use of PatInspect to find defects in a set of " & _
        "shapes that move/rotate in an image." & Environment.NewLine & _
        "The following steps are performed: " & Environment.NewLine & _
        "1. A PMAlign Tool is trained to find a region in an image. " & Environment.NewLine & _
        "2. A PatInspect Tool is trained to inspect a second region in the image. " & Environment.NewLine & _
        "3. Images in a training database are aligned using the trained PMAlign Tool and then " & _
        "used to statistically train the PatInspect Tool." & Environment.NewLine & _
        "4. Images in a testing database are aligned using the trained PMAlignTool and then " & _
        "inspected using the trained PatInspect Tool." & Environment.NewLine & _
        "Graphics: Green indicates the alignment region. Magenta indicates the inspection region"

      PreInspectDispStatusBar.Display = PreInspectDisplay
      PostInspectDispStatusBar.Display = PostInspectDisplay

      ' Open training image file.
      trainIDBTool.[Operator].Open(VPRO_PATH & "/images/pmSample.idb", CogImageFileModeConstants.Read)
      ' Open testing image file.
      testIDBTool.[Operator].Open(VPRO_PATH & "/images/piSample.idb", CogImageFileModeConstants.Read)

      'train PMAlignTool on region in first image of trainIDB
      trainIDBTool.Run()
      myPMTool.Pattern.TrainImage = trainIDBTool.OutputImage
      Dim trainRegion As New CogRectangle
      trainRegion.SetXYWidthHeight(300, 200, 114, 123)
      trainRegion.SelectedSpaceName = trainIDBTool.OutputImage.SelectedSpaceName
      myPMTool.Pattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBox
      myPMTool.Pattern.TrainMode = CogPMAlignTrainModeConstants.Image
      myPMTool.Pattern.TrainRegion = trainRegion
      trainOrigin.TranslationX = 352
      trainOrigin.TranslationY = 260
      trainOrigin.Rotation = 0
      myPMTool.Pattern.Origin = trainOrigin
      myPMTool.Pattern.Train()

      ' train the PatInspectTool

      ' first get the training image and origin

      myPITool.Pattern.TrainImage = myPMTool.Pattern.TrainImage
      myPITool.Pattern.Origin = myPMTool.Pattern.Origin

      ' next set the region to inspect and train

      inspectRegion.SetXYWidthHeight(370, 113, 172, 138)
      inspectRegion.SelectedSpaceName = trainIDBTool.OutputImage.SelectedSpaceName
      myPITool.Pattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBox
      myPITool.Pattern.TrainRegion = inspectRegion

      myPITool.Pattern.Train()

      ' next statistically train the PatInspectTool.  This will require:
      ' 1. aligning the next image using the PMAlignTool to find the correct region on which to
      '    statistically train.
      ' 2. statistically training the PatInspectTool on that region.
      ' Note that statistical training is optional

      ' allow full rotation
      myPMTool.RunParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh
      myPMTool.RunParams.ZoneAngle.Low = -3.14159
      myPMTool.RunParams.ZoneAngle.High = 3.14159

      Dim i As Integer

      For i = 2 To trainIDBTool.[Operator].Count
        trainIDBTool.Run()
        myPMTool.InputImage = trainIDBTool.OutputImage
        myPMTool.Run()
        If myPMTool.Results.Count <> 1 Then
                    MessageBox.Show("Wrong number of PMAlign results found", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
          End
        End If
        myPITool.Pattern.StatisticalTrain(trainIDBTool.OutputImage, myPMTool.Results.Item(0).GetPose)
      Next
    End Sub

    Private Sub btnInspect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInspect.Click
      ' Get the next image from the test database
      testIDBTool.Run()

      ' align the image
      myPMTool.InputImage = testIDBTool.OutputImage
      myPMTool.Run()
      If myPMTool.Results.Count <> 1 Then
                MessageBox.Show("Wrong number of PMAlign results found", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End
      End If

      ' draw graphics showing the aligned training region
      Dim alignRegion As CogCompositeShape
      alignRegion = myPMTool.Results.Item(0).CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchRegion Or CogPMAlignResultGraphicConstants.CoordinateAxes)

      PreInspectDisplay.StaticGraphics.Clear()
      PreInspectDisplay.Image = testIDBTool.OutputImage
      PreInspectDisplay.Fit()
      PreInspectDisplay.StaticGraphics.Add(alignRegion, "test")

      ' inspect the image and draw graphics showing inspection region
      myPITool.InputImage = testIDBTool.OutputImage
      myPITool.Pose = myPMTool.Results.Item(0).GetPose
      myPITool.Run()
      PostInspectDisplay.Image = myPITool.Result.GetDifferenceImage(CogPatInspectDifferenceImageConstants.Absolute)
      PostInspectDisplay.Fit()
      Dim transformedRect As CogRectangleAffine

      ' The following line of code maps the train-time inspection region to a region that is properly
      ' aligned on the run-time image.
      ' - It assumes inspectRegion is in the selected space of the train image.
      ' - myPITool.Pose is a Run-Time Selected Space from Pattern Space Transform
      ' - trainOrigin is a Train-Time selected Space from Pattern Space Transform
      ' - Thus, The Transform passed to MapLinear is a Run-Time Selected Space from
      '   Train-Time Selected Space Transform.
      transformedRect = inspectRegion.MapLinear(myPITool.Pose.Compose(trainOrigin.Invert), CogCopyShapeConstants.GeometryOnly)
      transformedRect.Color = CogColorConstants.Magenta
      PreInspectDisplay.StaticGraphics.Add(transformedRect, "test")
    End Sub
    Private Sub frmPatInspSamp_closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
      If Not trainIDBTool Is Nothing Then trainIDBTool.Dispose()
      If Not testIDBTool Is Nothing Then testIDBTool.Dispose()
      If Not myPMTool Is Nothing Then myPMTool.Dispose()
      If Not myPITool Is Nothing Then myPITool.Dispose()
    End Sub
#End Region

   
  End Class
End Namespace