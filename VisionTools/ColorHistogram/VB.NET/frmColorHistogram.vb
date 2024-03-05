
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

' This sample demonstrates a way to compute a color histogram given three planes
' of a color image. In this sample, the Hue, Saturation and Intensity planes are
' used, but this can also apply to any 3-plane color image.

' The application provides the histogram for selected pixels of a chosen plane.
' The pixels are selected by specifying the min/max grey range allowed for each plane.
' Those pixels in each plane that fall within the specified range are AND'ed together
' to form a mask.  The mask is provided to the histogram along with a plane of choice.
' The histogram of the chosen plane is computed only on unmasked pixels (values > 0).

' In this sample, three synthetic images are created to represent each of the planes
' of a 3-plane color image. These synthetic images can be replaced by a 3 planes of
' a color image acquired from the user's choice of sources (acquisition, image file, etc.),
' if so desired.
'
' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
'
Option Explicit On 
Imports Cognex.VisionPro
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.ImageProcessing
Namespace SampleColorHistogram
  Public Class frmColorHistogram
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
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents HuePlaneOption As System.Windows.Forms.RadioButton
    Friend WithEvents SaturationPlaneOption As System.Windows.Forms.RadioButton
    Friend WithEvents IntensityPlaneOption As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents HueMinCtl As System.Windows.Forms.TextBox
    Friend WithEvents HueMaxCtl As System.Windows.Forms.TextBox
    Friend WithEvents SatMinCtl As System.Windows.Forms.TextBox
    Friend WithEvents SatMaxCtl As System.Windows.Forms.TextBox
    Friend WithEvents IntMinCtl As System.Windows.Forms.TextBox
    Friend WithEvents IntMaxCtl As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents myDisplay As Cognex.VisionPro.Display.CogDisplay
        Friend WithEvents myDisplayStatusBar As Cognex.VisionPro.CogDisplayStatusBarV2
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmColorHistogram))
      Me.myDisplay = New Cognex.VisionPro.Display.CogDisplay
      Me.btnRun = New System.Windows.Forms.Button
      Me.GroupBox1 = New System.Windows.Forms.GroupBox
      Me.IntensityPlaneOption = New System.Windows.Forms.RadioButton
      Me.SaturationPlaneOption = New System.Windows.Forms.RadioButton
      Me.HuePlaneOption = New System.Windows.Forms.RadioButton
      Me.Label1 = New System.Windows.Forms.Label
      Me.Label2 = New System.Windows.Forms.Label
      Me.Label3 = New System.Windows.Forms.Label
      Me.Label4 = New System.Windows.Forms.Label
      Me.HueMinCtl = New System.Windows.Forms.TextBox
      Me.HueMaxCtl = New System.Windows.Forms.TextBox
      Me.SatMinCtl = New System.Windows.Forms.TextBox
      Me.SatMaxCtl = New System.Windows.Forms.TextBox
      Me.IntMinCtl = New System.Windows.Forms.TextBox
      Me.IntMaxCtl = New System.Windows.Forms.TextBox
      Me.Label5 = New System.Windows.Forms.Label
      Me.Label6 = New System.Windows.Forms.Label
            Me.myDisplayStatusBar = New Cognex.VisionPro.CogDisplayStatusBarV2
      Me.TextBox1 = New System.Windows.Forms.TextBox
      CType(Me.myDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.GroupBox1.SuspendLayout()
      Me.SuspendLayout()
      '
      'myDisplay
      '
      Me.myDisplay.Location = New System.Drawing.Point(272, 16)
      Me.myDisplay.Name = "myDisplay"
      Me.myDisplay.OcxState = CType(resources.GetObject("myDisplay.OcxState"), System.Windows.Forms.AxHost.State)
      Me.myDisplay.Size = New System.Drawing.Size(384, 320)
      Me.myDisplay.TabIndex = 0
      '
      'btnRun
      '
      Me.btnRun.Location = New System.Drawing.Point(72, 336)
      Me.btnRun.Name = "btnRun"
      Me.btnRun.Size = New System.Drawing.Size(104, 40)
      Me.btnRun.TabIndex = 1
      Me.btnRun.Text = "Run"
      '
      'GroupBox1
      '
      Me.GroupBox1.Controls.Add(Me.IntensityPlaneOption)
      Me.GroupBox1.Controls.Add(Me.SaturationPlaneOption)
      Me.GroupBox1.Controls.Add(Me.HuePlaneOption)
      Me.GroupBox1.Location = New System.Drawing.Point(16, 16)
      Me.GroupBox1.Name = "GroupBox1"
      Me.GroupBox1.Size = New System.Drawing.Size(192, 120)
      Me.GroupBox1.TabIndex = 2
      Me.GroupBox1.TabStop = False
      Me.GroupBox1.Text = "Select a plane to Process"
      '
      'IntensityPlaneOption
      '
      Me.IntensityPlaneOption.Location = New System.Drawing.Point(64, 80)
      Me.IntensityPlaneOption.Name = "IntensityPlaneOption"
      Me.IntensityPlaneOption.TabIndex = 2
      Me.IntensityPlaneOption.Text = "Intensity Plane"
      '
      'SaturationPlaneOption
      '
      Me.SaturationPlaneOption.Location = New System.Drawing.Point(64, 48)
      Me.SaturationPlaneOption.Name = "SaturationPlaneOption"
      Me.SaturationPlaneOption.Size = New System.Drawing.Size(112, 24)
      Me.SaturationPlaneOption.TabIndex = 1
      Me.SaturationPlaneOption.Text = "Saturation Plane"
      '
      'HuePlaneOption
      '
      Me.HuePlaneOption.Location = New System.Drawing.Point(64, 24)
      Me.HuePlaneOption.Name = "HuePlaneOption"
      Me.HuePlaneOption.TabIndex = 0
      Me.HuePlaneOption.Text = "Hue Plane"
      '
      'Label1
      '
      Me.Label1.Location = New System.Drawing.Point(48, 160)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(120, 40)
      Me.Label1.TabIndex = 3
      Me.Label1.Text = "Allowable Grey Value Range for Each Plane"
      '
      'Label2
      '
      Me.Label2.Location = New System.Drawing.Point(32, 232)
      Me.Label2.Name = "Label2"
      Me.Label2.Size = New System.Drawing.Size(64, 23)
      Me.Label2.TabIndex = 4
      Me.Label2.Text = "Hue:"
      '
      'Label3
      '
      Me.Label3.Location = New System.Drawing.Point(32, 256)
      Me.Label3.Name = "Label3"
      Me.Label3.Size = New System.Drawing.Size(64, 23)
      Me.Label3.TabIndex = 5
      Me.Label3.Text = "Saturation:"
      '
      'Label4
      '
      Me.Label4.Location = New System.Drawing.Point(32, 288)
      Me.Label4.Name = "Label4"
      Me.Label4.Size = New System.Drawing.Size(64, 23)
      Me.Label4.TabIndex = 6
      Me.Label4.Text = "Intensity:"
      '
      'HueMinCtl
      '
      Me.HueMinCtl.Location = New System.Drawing.Point(104, 232)
      Me.HueMinCtl.Name = "HueMinCtl"
      Me.HueMinCtl.Size = New System.Drawing.Size(40, 20)
      Me.HueMinCtl.TabIndex = 7
      Me.HueMinCtl.Text = "50"
      '
      'HueMaxCtl
      '
      Me.HueMaxCtl.Location = New System.Drawing.Point(168, 232)
      Me.HueMaxCtl.Name = "HueMaxCtl"
      Me.HueMaxCtl.Size = New System.Drawing.Size(40, 20)
      Me.HueMaxCtl.TabIndex = 8
      Me.HueMaxCtl.Text = "254"
      '
      'SatMinCtl
      '
      Me.SatMinCtl.Location = New System.Drawing.Point(104, 256)
      Me.SatMinCtl.Name = "SatMinCtl"
      Me.SatMinCtl.Size = New System.Drawing.Size(40, 20)
      Me.SatMinCtl.TabIndex = 9
      Me.SatMinCtl.Text = "50"
      '
      'SatMaxCtl
      '
      Me.SatMaxCtl.Location = New System.Drawing.Point(168, 256)
      Me.SatMaxCtl.Name = "SatMaxCtl"
      Me.SatMaxCtl.Size = New System.Drawing.Size(40, 20)
      Me.SatMaxCtl.TabIndex = 10
      Me.SatMaxCtl.Text = "254"
      '
      'IntMinCtl
      '
      Me.IntMinCtl.Location = New System.Drawing.Point(104, 280)
      Me.IntMinCtl.Name = "IntMinCtl"
      Me.IntMinCtl.Size = New System.Drawing.Size(40, 20)
      Me.IntMinCtl.TabIndex = 11
      Me.IntMinCtl.Text = "50"
      '
      'IntMaxCtl
      '
      Me.IntMaxCtl.Location = New System.Drawing.Point(168, 280)
      Me.IntMaxCtl.Name = "IntMaxCtl"
      Me.IntMaxCtl.Size = New System.Drawing.Size(40, 20)
      Me.IntMaxCtl.TabIndex = 12
      Me.IntMaxCtl.Text = "254"
      '
      'Label5
      '
      Me.Label5.Location = New System.Drawing.Point(104, 200)
      Me.Label5.Name = "Label5"
      Me.Label5.Size = New System.Drawing.Size(56, 23)
      Me.Label5.TabIndex = 13
      Me.Label5.Text = "Min"
      '
      'Label6
      '
      Me.Label6.Location = New System.Drawing.Point(168, 200)
      Me.Label6.Name = "Label6"
      Me.Label6.Size = New System.Drawing.Size(40, 23)
      Me.Label6.TabIndex = 14
      Me.Label6.Text = "Max"
      '
      'myDisplayStatusBar
      '
      Me.myDisplayStatusBar.Enabled = True
      Me.myDisplayStatusBar.Location = New System.Drawing.Point(280, 352)
      Me.myDisplayStatusBar.Name = "myDisplayStatusBar"
            Me.myDisplayStatusBar.Size = New System.Drawing.Size(384, 21)
      Me.myDisplayStatusBar.TabIndex = 15
      '
      'TextBox1
      '
      Me.TextBox1.Location = New System.Drawing.Point(16, 392)
      Me.TextBox1.Multiline = True
      Me.TextBox1.Name = "TextBox1"
      Me.TextBox1.Size = New System.Drawing.Size(640, 40)
      Me.TextBox1.TabIndex = 16
      Me.TextBox1.Text = "Sample description: demonstrates a way to compute a color histogram given three p" & _
      "lanes of a color image.  In this " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "sample, the Hue, Saturation and Intensity pla" & _
      "nes are used, but this can also apply to any 3-plane color image."
      '
      'frmColorHistogram
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(688, 446)
      Me.Controls.Add(Me.TextBox1)
      Me.Controls.Add(Me.myDisplayStatusBar)
      Me.Controls.Add(Me.Label6)
      Me.Controls.Add(Me.Label5)
      Me.Controls.Add(Me.IntMaxCtl)
      Me.Controls.Add(Me.IntMinCtl)
      Me.Controls.Add(Me.SatMaxCtl)
      Me.Controls.Add(Me.SatMinCtl)
      Me.Controls.Add(Me.HueMaxCtl)
      Me.Controls.Add(Me.HueMinCtl)
      Me.Controls.Add(Me.Label4)
      Me.Controls.Add(Me.Label3)
      Me.Controls.Add(Me.Label2)
      Me.Controls.Add(Me.Label1)
      Me.Controls.Add(Me.GroupBox1)
      Me.Controls.Add(Me.btnRun)
      Me.Controls.Add(Me.myDisplay)
      Me.Name = "frmColorHistogram"
      Me.Text = "Color Histogram Computation Demo"
      CType(Me.myDisplay, System.ComponentModel.ISupportInitialize).EndInit()
      Me.GroupBox1.ResumeLayout(False)
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region "Module Level vars"
    Private mImageFileTool As CogImageFileTool
    Private mFirstMinMaxTool As CogIPTwoImageMinMaxTool
    Private mMinMaxTool As CogIPTwoImageMinMaxTool
    Private CopyRegion As CogCopyRegionTool
    Private myHistogramTool As CogHistogramTool
    Private HueImage As CogImage8Grey
    Private SaturationImage As CogImage8Grey
    Private IntensityImage As CogImage8Grey
#End Region
#Region "Form and Controls events"
    Private Sub frmColorHistogram_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Dim VPRO_PATH As String
      VPRO_PATH = Environ("VPRO_ROOT")
      If VPRO_PATH = "" Then _
        DisplayErrorAndExit("Required environment variable VPRO_ROOT not set.")

      ' Create tools
      mImageFileTool = New CogImageFileTool
      Try
        mImageFileTool.[Operator].Open(VPRO_PATH & "/images/smiley.bmp", CogImageFileModeConstants.Read)

        mFirstMinMaxTool = New CogIPTwoImageMinMaxTool
        mMinMaxTool = New CogIPTwoImageMinMaxTool
        myHistogramTool = New CogHistogramTool
        ' The CogDisplayStatusBar will be in sync with the CogDisplay
        myDisplayStatusBar.Display = myDisplay
        ' Three planes of a color image
        HueImage = New CogImage8Grey
        SaturationImage = New CogImage8Grey
        IntensityImage = New CogImage8Grey
        HuePlaneOption.Checked = True

      Catch ex As Exception
        DisplayErrorAndExit(VPRO_PATH & "\images\smiley.bmp not found")
      End Try
    End Sub
    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
      ' This method creates the synthetic images, displays them, and executes the histogram computation based on
      ' user-provided parameters.
      myDisplay.StaticGraphics.Clear()
      mImageFileTool.Run()
      myDisplay.Image = mImageFileTool.OutputImage
      MessageBox.Show("Original image displayed")
      Dim myHSIImage As CogImage24PlanarColor
      myHSIImage = CogImageConvert.GetHSIImage(mImageFileTool.OutputImage, 0, 0, 0, 0)
      Dim HueImage As CogImage8Grey
      Dim SaturationImage As CogImage8Grey
      Dim IntensityImage As CogImage8Grey
      HueImage = myHSIImage.GetPlane(CogImagePlaneConstants.Hue)
      SaturationImage = myHSIImage.GetPlane(CogImagePlaneConstants.Saturation)
      IntensityImage = myHSIImage.GetPlane(CogImagePlaneConstants.Intensity)

      If HuePlaneOption.Checked = True Then
        ComputeHistogram(myHSIImage, CogImagePlaneConstants.Hue, HueMinCtl.Text, HueMaxCtl.Text, _
        SatMinCtl.Text, SatMaxCtl.Text, _
        IntMinCtl.Text, IntMaxCtl.Text)
      ElseIf SaturationPlaneOption.Checked = True Then
        ComputeHistogram(myHSIImage, CogImagePlaneConstants.Saturation, HueMinCtl.Text, HueMaxCtl.Text, _
        SatMinCtl.Text, SatMaxCtl.Text, _
        IntMinCtl.Text, IntMaxCtl.Text)
      Else
        ComputeHistogram(myHSIImage, CogImagePlaneConstants.Intensity, HueMinCtl.Text, HueMaxCtl.Text, _
        SatMinCtl.Text, SatMaxCtl.Text, _
        IntMinCtl.Text, IntMaxCtl.Text)
      End If

    End Sub
    Private Sub frmColorHistogram_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
      If Not mImageFileTool Is Nothing Then
        mImageFileTool.[Operator].Close()
        mImageFileTool.Dispose()
      End If
      If Not mFirstMinMaxTool Is Nothing Then mFirstMinMaxTool.Dispose()
      If Not mMinMaxTool Is Nothing Then mMinMaxTool.Dispose()
      If Not myHistogramTool Is Nothing Then myHistogramTool.Dispose()
    End Sub
#End Region
#Region "Module Level Routines"
    'This method does the following:
    ' 1. Computes a PixelMap on each plane to determine which pixels should contribute to the histogram.
    '    This utilizes the user-specified grey-value ranges for each plane.
    ' 2. Uses CogIPTwoImageMinMax to compute the intersetion of the three images to form an image mask.
    ' 3. Computes a histogram of the supplied ImagePlane for the unmasked pixels.
    ' This method also displays result images and histogram graphics during processing.

    Private Sub ComputeHistogram(ByVal ColorImage As CogImage24PlanarColor, ByVal ImagePlane As CogImagePlaneConstants, _
    ByVal HueMin As Byte, ByVal HueMax As Byte, _
    ByVal SaturationMin As Byte, ByVal SaturationMax As Byte, _
    ByVal IntensityMin As Byte, ByVal IntensityMax As Byte)

      ' When working with a single IPOneImage step, it is often easier to just use the operator directly,
      ' as shown below, instead of using the IPOneImageTool.
      Dim myPixelMapOperator As CogIPOneImagePixelMap
      Dim mappedHueImage As CogImage8Grey
      Dim mappedSaturationImage As CogImage8Grey
      Dim mappedIntensityImage As CogImage8Grey
      Dim myMap(255) As Byte

      myPixelMapOperator = New CogIPOneImagePixelMap

      'Hue image first
      InitializePixelMap(myMap, HueMin, HueMax)
      myPixelMapOperator.SetMap(myMap)
            Dim r As Cognex.VisionPro.ICogRegion
            r = Nothing
      mappedHueImage = myPixelMapOperator.Execute(ColorImage.GetPlane(CogImagePlaneConstants.Hue), CogRegionModeConstants.PixelAlignedBoundingBox, r)
      myDisplay.Image = mappedHueImage
      MessageBox.Show("Mapped Hue Image Displayed")

      'Saturation image next
      InitializePixelMap(myMap, SaturationMin, SaturationMax)
      myPixelMapOperator.SetMap(myMap)
      mappedSaturationImage = myPixelMapOperator.Execute(ColorImage.GetPlane(CogImagePlaneConstants.Saturation), CogRegionModeConstants.PixelAlignedBoundingBox, r)
      myDisplay.Image = mappedSaturationImage
      MessageBox.Show("Mapped Saturation Image Displayed")

      'Intensity image last
      InitializePixelMap(myMap, IntensityMin, IntensityMax)
      myPixelMapOperator.SetMap(myMap)
      mappedIntensityImage = myPixelMapOperator.Execute(ColorImage.GetPlane(CogImagePlaneConstants.Intensity), CogRegionModeConstants.PixelAlignedBoundingBox, r)
      myDisplay.Image = mappedIntensityImage
      MessageBox.Show("Mapped Intensity Image Displayed")

      'Find the intersection of their mapped values
      mMinMaxTool.InputImageA = mappedHueImage
      mMinMaxTool.InputImageB = mappedSaturationImage
      mMinMaxTool.RunParams.Operation = CogIPTwoImageMinMaxOperationConstants.Min Or CogIPTwoImageMinMaxOperationConstants.Max
      mMinMaxTool.Run()
      mMinMaxTool.InputImageA = mMinMaxTool.OutputImage
      mMinMaxTool.InputImageB = mappedIntensityImage
      mMinMaxTool.Run()

      myDisplay.Image = mMinMaxTool.OutputImage
      MessageBox.Show("Intersection of Mapped Images Displayed")

      ' check to see if mask is empty
      myHistogramTool.InputImage = mMinMaxTool.OutputImage
      myHistogramTool.Run()
      If Not myHistogramTool.Result Is Nothing Then
        If myHistogramTool.Result.Maximum = 0 Then
          MessageBox.Show("No pixels available to histogram")
        Else
          'Compute Histogram of selected image plane
          myHistogramTool.RunParams.InputImageMask = mMinMaxTool.OutputImage
          myHistogramTool.InputImage = ColorImage.GetPlane(ImagePlane)
          myHistogramTool.Run()
          Dim myHistogramGraphics As CogCompositeShape
          myHistogramGraphics = myHistogramTool.Result.CreateResultGraphics(CogHistogramResultGraphicConstants.All)
          myDisplay.StaticGraphics.Add(myHistogramGraphics, "test")
          myDisplay.Fit()
          MessageBox.Show("Histogram of filtered selected plane displayed")
        End If
      Else
        MessageBox.Show("Histogram tool failed")
      End If
    End Sub

    'This method is used to initialize a PixelMap array as a mask.
    Private Sub InitializePixelMap(ByVal mapArray() As Byte, ByVal MinVal As Byte, ByVal MaxVal As Byte)
      Dim i As Integer

      For i = 0 To 255
        If i >= MinVal And i <= MaxVal Then
          mapArray(i) = 255
        Else
          mapArray(i) = 0
        End If
      Next i
    End Sub
    Private Sub DisplayErrorAndExit(ByVal ErrorMsg As String)
      MessageBox.Show(ErrorMsg & Environment.NewLine & "Press OK to exit.")
      Me.Close()
      End      ' Quit if it is called from Form_Load
    End Sub
#End Region


  End Class
End Namespace