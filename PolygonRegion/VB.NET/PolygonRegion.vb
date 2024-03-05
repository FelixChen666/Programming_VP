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

' This sample program demonstrates the programmatic use of the Blob Tool and
' the CogCopyRegionTool. Blob outputs a polygon which is used as an input region
' of the CogCopyRegionTool. The CogCopyRegionTool creates an output image based on
' its input region which is displayed on the CogDisplay. The sample uses the objects
' and interfaces defined in the Cognex Core, Cognex Image, Cognex Blob and Cognex
' Image Processing type libraries.
'
' The CogCopyRegionTool is used to copy the pixels within a supplied region of
' an input image into a a new image.
'
' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
'
' The following steps show how to add the CogPolygon as an input region.
'
' Step 1) Create the CogCopyRegionTool
' Step 2) Configure the CogCopyRegion parameters.
' Step 3) Assign an input image and the region of the CogCopyRegionTool.
' Step 4) Run the CogCopyRegionTool
' Step 5) Display the output image of the CogCopyRegion tool.
'
Option Explicit On 
' needed for VisionPro
Imports Cognex.VisionPro
' needed for VisionPro image processing
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.ImageProcessing
Imports Cognex.VisionPro.Blob
' needed for VisionPro Exceptions
Imports Cognex.VisionPro.Exceptions

Namespace PolygonRegion
  Public Class PolygonRegion
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
    Friend WithEvents cmdRun As System.Windows.Forms.Button
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents ctrDisplay As Cognex.VisionPro.Display.CogDisplay
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(PolygonRegion))
      Me.ctrDisplay = New Cognex.VisionPro.Display.CogDisplay
      Me.cmdRun = New System.Windows.Forms.Button
      Me.txtDescription = New System.Windows.Forms.TextBox
      CType(Me.ctrDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'ctrDisplay
      '
      Me.ctrDisplay.Location = New System.Drawing.Point(168, 8)
      Me.ctrDisplay.Name = "ctrDisplay"
      Me.ctrDisplay.OcxState = CType(resources.GetObject("ctrDisplay.OcxState"), System.Windows.Forms.AxHost.State)
      Me.ctrDisplay.Size = New System.Drawing.Size(592, 424)
      Me.ctrDisplay.TabIndex = 0
      '
      'cmdRun
      '
      Me.cmdRun.Location = New System.Drawing.Point(16, 128)
      Me.cmdRun.Name = "cmdRun"
      Me.cmdRun.Size = New System.Drawing.Size(136, 40)
      Me.cmdRun.TabIndex = 1
      Me.cmdRun.Text = "Run"
      '
      'txtDescription
      '
      Me.txtDescription.Location = New System.Drawing.Point(8, 448)
      Me.txtDescription.Multiline = True
      Me.txtDescription.Name = "txtDescription"
      Me.txtDescription.Size = New System.Drawing.Size(744, 48)
      Me.txtDescription.TabIndex = 2
      Me.txtDescription.Text = "Sample Description: This application demonstrates the programmatic use of a tool-" & _
      "generated CogPolygon as the (input) region for another tool." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Sample Usage: Clic" & _
      "k on Run to see a CogPolygon region in use. Click on Reset to start over."
      Me.txtDescription.WordWrap = False
      '
      'PolygonRegion
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(760, 502)
      Me.Controls.Add(Me.txtDescription)
      Me.Controls.Add(Me.cmdRun)
      Me.Controls.Add(Me.ctrDisplay)
      Me.Name = "PolygonRegion"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
      Me.Text = "VisionPro Polygon Region"
      CType(Me.ctrDisplay, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region " Private Vars "
    Private imageTool As CogImageFileTool
    Private blobTool As CogBlobTool
    Private copyRegionTool As CogCopyRegionTool
    Private imageGrey As CogImage8Grey

#End Region
#Region " Initialization"
    Private Sub Initialization()
      ' Create all the tools needed to perform the sample operation.
        imageTool = New CogImageFileTool
        blobTool = New CogBlobTool
        ' Step 1 - Create the CogCopyRegionTool
        copyRegionTool = New CogCopyRegionTool

        ' Exit if we cannot create any one of the above tools.
        If imageTool Is Nothing Or blobTool Is Nothing Or copyRegionTool Is Nothing Then
          Throw New Exception("Can not create Cognex tools")
        End If
        ' Configure the blob tool
        blobTool.RunParams.RegionMode = CogRegionModeConstants.PixelAlignedBoundingBox
        blobTool.RunParams.ConnectivityMode = CogBlobConnectivityModeConstants.GreyScale
        blobTool.RunParams.ConnectivityMinPixels = 10
        blobTool.RunParams.ConnectivityCleanup = CogBlobConnectivityCleanupConstants.Fill
        blobTool.RunParams.SegmentationParams.SetSegmentationSoftRelativeThreshold( _
                          0, 0, 40, 60, 254, CogBlobSegmentationPolarityConstants.DarkBlobs)

        ' We need to create an CogBlobMeasure object to filter out the really small blobs.
        ' A CogBlobTool's operator can hold a collection of CogBlobMeasures to perform
        ' various blob measurements.
        Dim blobMeasure As CogBlobMeasure
        blobMeasure = New CogBlobMeasure
        blobMeasure.Measure = CogBlobMeasureConstants.Area
        blobMeasure.Mode = CogBlobMeasureModeConstants.Filter
        blobMeasure.FilterMode = CogBlobFilterModeConstants.ExcludeBlobsInRange
        blobMeasure.FilterRangeLow = 0
        blobMeasure.FilterRangeHigh = 100
        ' Add to the collection
        blobTool.RunParams.RunTimeMeasures.Add(blobMeasure)

        ' Step 2 - Configure the CogCopyRegion which is the same as CogCopyRegion.RunParams.
        copyRegionTool.RunParams.RegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask
        copyRegionTool.RunParams.FillBoundingBox = True       ' fill area around the polygon
        copyRegionTool.RunParams.FillBoundingBoxValue = 255
        copyRegionTool.RunParams.FillRegion = False

        ' Get VPRO_ROOT from environment which is needed to locate polygonRegion.cdb.
        Const strFileName As String = "/Images/polygonRegion.cdb"
        Dim strBaseDir As String
        strBaseDir = Environ("VPRO_ROOT")
        If strBaseDir = "" Then
          Throw New Exception("Required environment variable VPRO_ROOT not set.")
        End If

        ' Open polygonRegion.cdb and display the first image on the CogDisplay.
        imageTool.[Operator].Open(strBaseDir & strFileName, CogImageFileModeConstants.Read)
        imageGrey = CType(imageTool.[Operator].Item(0), CogImage8Grey)
        ctrDisplay.Image = imageGrey
        ctrDisplay.Fit(True)    ' make sure the image fits the CogDisplay.



    End Sub
#End Region
#Region " Helper Functions:"
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Helper function.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Displays an error message and then exits the program.
    ' Call this when an unrecoverable error has occurred.
    Private Sub DisplayErrorAndExit(ByVal ErrorMsg As String)
      MessageBox.Show(ErrorMsg & vbCr & "Press OK to exit.")
      Application.Exit()
    End Sub

#End Region
#Region " 'Run' command button click handler"
    Private Sub cmdRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRun.Click
      Try
        If cmdRun.Text = "Run" Then
          ' Run blob on the input image
          Dim blobResults As CogBlobResults
          blobTool.Region = Nothing     ' search for the entire region
          blobTool.InputImage = imageGrey
          blobTool.Run()
          blobResults = blobTool.Results
          ' Make sure the result is not empty.
          If Not blobResults Is Nothing Then
            If blobResults.GetBlobs.Count > 0 Then
              ' Extract the largest blob (operator is already configured to filter
              ' and sort so all we have to do is grab blob number zero).
              Dim blobResult As CogBlobResult
              blobResult = blobResults.GetBlobs.Item(0)

              ' GetBoundary returns CogPolygon which is used as a region for
              ' the CogCopyRegionTool.
              Dim region As Cognex.VisionPro.ICogRegion
              region = blobResult.GetBoundary

              ' Step 3 - Feed input image and polygon based region to the CogCopyRegionTool
              '          to create a new image. The CogCopyRegionTool is configured to copy
              '          only those pixels that are within the polygon region, and to fill
              '          with the value 255 those pixels outside the region but inside
              '          the polygons bounding box.
              copyRegionTool.Region = region
              copyRegionTool.InputImage = imageGrey
              ' Step 4 - Run the tool
              copyRegionTool.Run()
              ' Step 5 - Display the output image of the CogCopyRegion tool.
              ctrDisplay.Image = copyRegionTool.OutputImage

              ' Toggle our state by changing the button caption ...
              cmdRun.Text = "Reset"
            End If
          End If
        Else
          ' Reload the input image
          imageGrey = CType(imageTool.[Operator].Item(0), CogImage8Grey)

          ' Update the display
          ctrDisplay.Image = imageGrey
          ctrDisplay.Fit(True)

          ' Toggle our state by changing the button caption
          cmdRun.Text = "Run"
        End If
      Catch ex As CogException
        DisplayErrorAndExit("Unexpected error: " & ex.Message)
      Catch gex As Exception
        DisplayErrorAndExit("Unexpected error: " & gex.Message)
      End Try

    End Sub

#End Region

    Private Sub PolygonRegion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Try
        Initialization()
      Catch ex As CogException
        DisplayErrorAndExit("Unexpected error: " & ex.Message)
      Catch gex As Exception
        DisplayErrorAndExit("Unexpected error: " & gex.Message)
      End Try

    End Sub
  End Class

End Namespace
