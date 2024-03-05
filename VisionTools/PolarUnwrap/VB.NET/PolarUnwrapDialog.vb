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

' This example illustrates how to find a feature in a circular object which can
' be rotated by using CogPolarUnwrapTool and CogCNLSearchTool.
'
' The following steps are performed:
' 1. Find the circular object by using CogFindCircleTool.
' 2. Run CogPolarUnwrapTool.
' 3. Run CogCNLSearchTool on the unwrapped image.
' 4. Map the position of the feature back to the original image.
Option Explicit On 
Imports Cognex.VisionPro
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.ImageProcessing
Imports Cognex.VisionPro.CNLSearch
Imports Cognex.VisionPro.Caliper
Namespace samplePolarUnwrap
  Public Class PolarUnwrapDialog
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
    Friend WithEvents SourceDisplay As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents UnwrappedDisplay As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents ExplanationText As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(PolarUnwrapDialog))
      Me.SourceDisplay = New Cognex.VisionPro.Display.CogDisplay
      Me.UnwrappedDisplay = New Cognex.VisionPro.Display.CogDisplay
      Me.btnRun = New System.Windows.Forms.Button
      Me.ExplanationText = New System.Windows.Forms.TextBox
      Me.Label1 = New System.Windows.Forms.Label
      Me.Label2 = New System.Windows.Forms.Label
      CType(Me.SourceDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.UnwrappedDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'SourceDisplay
      '
      Me.SourceDisplay.Location = New System.Drawing.Point(24, 24)
      Me.SourceDisplay.Name = "SourceDisplay"
      Me.SourceDisplay.OcxState = CType(resources.GetObject("SourceDisplay.OcxState"), System.Windows.Forms.AxHost.State)
      Me.SourceDisplay.Size = New System.Drawing.Size(336, 288)
      Me.SourceDisplay.TabIndex = 0
      '
      'UnwrappedDisplay
      '
      Me.UnwrappedDisplay.Location = New System.Drawing.Point(24, 352)
      Me.UnwrappedDisplay.Name = "UnwrappedDisplay"
      Me.UnwrappedDisplay.OcxState = CType(resources.GetObject("UnwrappedDisplay.OcxState"), System.Windows.Forms.AxHost.State)
      Me.UnwrappedDisplay.Size = New System.Drawing.Size(720, 64)
      Me.UnwrappedDisplay.TabIndex = 1
      '
      'btnRun
      '
      Me.btnRun.Location = New System.Drawing.Point(472, 232)
      Me.btnRun.Name = "btnRun"
      Me.btnRun.Size = New System.Drawing.Size(216, 72)
      Me.btnRun.TabIndex = 2
      Me.btnRun.Text = "Run"
      '
      'ExplanationText
      '
      Me.ExplanationText.Location = New System.Drawing.Point(400, 24)
      Me.ExplanationText.Multiline = True
      Me.ExplanationText.Name = "ExplanationText"
      Me.ExplanationText.ReadOnly = True
      Me.ExplanationText.Size = New System.Drawing.Size(328, 184)
      Me.ExplanationText.TabIndex = 3
      Me.ExplanationText.Text = ""
      '
      'Label1
      '
      Me.Label1.Location = New System.Drawing.Point(304, 320)
      Me.Label1.Name = "Label1"
      Me.Label1.TabIndex = 4
      Me.Label1.Text = "Unwrapped Image"
      '
      'Label2
      '
      Me.Label2.Location = New System.Drawing.Point(112, 0)
      Me.Label2.Name = "Label2"
      Me.Label2.Size = New System.Drawing.Size(136, 23)
      Me.Label2.TabIndex = 5
      Me.Label2.Text = "Source Image"
      '
      'PolarUnwrapDialog
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(768, 534)
      Me.Controls.Add(Me.Label2)
      Me.Controls.Add(Me.Label1)
      Me.Controls.Add(Me.ExplanationText)
      Me.Controls.Add(Me.btnRun)
      Me.Controls.Add(Me.UnwrappedDisplay)
      Me.Controls.Add(Me.SourceDisplay)
      Me.Name = "PolarUnwrapDialog"
      Me.Text = "Polar Unwrap Sample Application"
      CType(Me.SourceDisplay, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.UnwrappedDisplay, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Module Level vars"
    Private ImageTool As New CogImageFileTool
    Private FindCircleTool As New CogFindCircleTool
    Private PolarUnwrapTool As New CogPolarUnwrapTool
    Private SearchTool As New CogCNLSearchTool
    Private CircularAnnulusSection As New CogCircularAnnulusSection
#End Region
#Region "Form and Controls Events"

    Private Sub frmPatInspSamp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Dim VPRO_PATH As String
      VPRO_PATH = Environment.GetEnvironmentVariable("VPRO_ROOT")
      If VPRO_PATH = "" Then
                MessageBox.Show("Required environment variable VPRO_ROOT not set.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End
      End If

      ExplanationText.Text = _
        "This example illustrates how to use CogPolarUnwrapTool and CogCNLSearchTool " & _
        "to find a feature in a circular object which can be rotated." & Environment.NewLine & _
        Environment.NewLine & _
        "The following steps are performed:" & Environment.NewLine & _
        "1. Find the circular object by using CogFindCircleTool." & Environment.NewLine & _
        "2. Run CogPolarUnwrapTool." & Environment.NewLine & _
        "3. Run CogCNLSearchTool on the unwrapped image." & Environment.NewLine & _
        "4. Map the position of the feature back to the original image.  The red " & _
        "marker in the image to the left indicates this position" & Environment.NewLine & _
        "Click on the Run button below to cycle through images."

      ' Open image file.
      Dim idb As New CogImageFile
      idb.Open(VPRO_PATH & "/images/polar.idb", CogImageFileModeConstants.Read)

      ' Initialize the image file tool
      ImageTool.[Operator] = idb

      Dim CircularArc As New CogCircularArc

      ' Set parameters for the circle finder.
      CircularArc.CenterX = 160
      CircularArc.CenterY = 110
      CircularArc.Radius = 65
      CircularArc.AngleStart = Cognex.VisionPro.CogMisc.DegToRad(0)
      CircularArc.AngleSpan = Cognex.VisionPro.CogMisc.DegToRad(360)
      FindCircleTool.RunParams.ExpectedCircularArc = CircularArc

      ' Set parameters for the polar unwrap tool.
      CircularAnnulusSection.Radius = 57
      CircularAnnulusSection.RadialScale = 0.7
      CircularAnnulusSection.AngleStart = Cognex.VisionPro.CogMisc.DegToRad(0)
      CircularAnnulusSection.AngleSpan = Cognex.VisionPro.CogMisc.DegToRad(720)
      CircularAnnulusSection.ArcDirectionAdornment = CogCircularAnnulusSectionDirectionAdornmentConstants.SolidArrow
      CircularAnnulusSection.RadialDirectionAdornment = CogCircularAnnulusSectionDirectionAdornmentConstants.Arrow

      ' Run once.
      btnInspect_Click(Me, New System.EventArgs)
      CircularAnnulusSection.Color = CogColorConstants.Yellow
      SourceDisplay.InteractiveGraphics.Add(CircularAnnulusSection, "test", False)
    End Sub

    Private Sub btnInspect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
      ' Clear all static graphics.
      SourceDisplay.StaticGraphics.Clear()
      UnwrappedDisplay.StaticGraphics.Clear()

      ImageTool.Run()

      FindCircleTool.InputImage = ImageTool.OutputImage
      FindCircleTool.Run()

      ' Use the circle finder to position the circular annulus section.
      CircularAnnulusSection.CenterX = FindCircleTool.Results.GetCircle.CenterX
      CircularAnnulusSection.CenterY = FindCircleTool.Results.GetCircle.CenterY

      ' Run polar unwrap.
      PolarUnwrapTool.InputImage = ImageTool.OutputImage
      PolarUnwrapTool.Region = CircularAnnulusSection
      PolarUnwrapTool.Run()

      ' Train the search tool if necessary.
      If Not SearchTool.Pattern.Trained Then
        Dim rect As New CogRectangle
        rect.X = 220
        rect.Y = 3
        rect.Width = 17
        rect.Height = 13
        SearchTool.Pattern.OriginX = rect.CenterX
        SearchTool.Pattern.OriginY = rect.CenterY

        SearchTool.Pattern.TrainImage = PolarUnwrapTool.OutputImage
        SearchTool.Pattern.TrainRegion = rect
        SearchTool.Pattern.Train()

        SearchTool.RunParams.AcceptThreshold = 0.8
      End If

      ' Run the search tool on the unwrapped image.
      SearchTool.InputImage = PolarUnwrapTool.OutputImage
      SearchTool.Run()

      ' Display images.
      SourceDisplay.Image = ImageTool.OutputImage
      UnwrappedDisplay.Image = PolarUnwrapTool.OutputImage

      ' Display edge regions and edge points found by the circle finder.
      Dim i As Long
      For i = 0 To FindCircleTool.Results.Count - 1
        SourceDisplay.StaticGraphics.Add(FindCircleTool.Results.Item(i).CreateResultGraphics(CogFindCircleResultGraphicConstants.DataPoint Or _
          CogFindCircleResultGraphicConstants.CaliperRegion), "test")
      Next i

      ' Display region and origin of found feature.
      UnwrappedDisplay.StaticGraphics.Add( _
        SearchTool.Results.Item(0).CreateResultGraphics(CogCNLSearchResultGraphicConstants.MatchRegion Or _
        CogCNLSearchResultGraphicConstants.Origin), "test")

      ' Display mapped point.
      Dim InputX As Double, InputY As Double
      Dim Point As New CogPointMarker
      PolarUnwrapTool.RunParams.GetInputPointFromOutputPoint( _
        PolarUnwrapTool.InputImage, CircularAnnulusSection, _
        SearchTool.Results.Item(0).LocationX, _
        SearchTool.Results.Item(0).LocationY, _
        InputX, InputY)
      Point.X = InputX
      Point.Y = InputY
      Point.Color = CogColorConstants.Red
      Point.GraphicType = CogPointMarkerGraphicTypeConstants.Crosshair
      SourceDisplay.StaticGraphics.Add(Point, "test")
    End Sub
    Private Sub PolarUnwrapDialog_closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
      If Not ImageTool Is Nothing Then ImageTool.Dispose()
      If Not PolarUnwrapTool Is Nothing Then PolarUnwrapTool.Dispose()
      If Not SearchTool Is Nothing Then SearchTool.Dispose()
    End Sub
#End Region


  End Class
End Namespace