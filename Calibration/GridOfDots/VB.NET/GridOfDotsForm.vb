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

' This sample provides the means to use a grid-of-dots in order to calibrate
' an image from image coordinates to physical coordinates. Each time the Calibrate
' button is hit, the next grid-of-dots image is loaded from the image file
' gridofdots.idb and calibration occurs.
' Calibration employs the following parameters:
' Number of Columns: The number of columns in the grid.
' Number of Rows: The number of rows in the grid.
' Row Origin: The 1-based index of the row element whose center of mass will be
'             used for the Y coordinate of the grid origin.
' Column Origin: The 1-based index of the column element whose center of mass
'                will be used for the X coordinate of the grid origin.
' Row Pitch: The physical distance between dots in a row.
' Column Pitch: The physical distance between dots in a column.
' Min Area: The minimum area in pixels for a black object to be considered a dot.
' Max RMS Error: The maximum allowable RMS error for calibration.
' Calibration will fail if either the number of required dots is not found or
' the RMS error exceeds Max RMS Error.
'

Option Explicit On
Imports Cognex.VisionPro
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.Blob
Imports Cognex.VisionPro.CalibFix
Imports System.Math
Imports Cognex.VisionPro.Exceptions
Imports System.Globalization

Namespace SampleGridOfDots
  Public Class GridOfDotsForm
    Inherits System.Windows.Forms.Form
#Region "Module Level vars"
    Private numColumns As Integer
    Private numRows As Integer
    Private rowOrigin As Integer
    Private columnOrigin As Integer
    Private rowPitch As Double
    Private columnPitch As Double
    Private minArea As Double
    Private maxRMSError As Double
    Private myImageFileTool As CogImageFileTool
    Private myCalTool As CogCalibNPointToNPointTool
    Private myBlobTool As CogBlobTool
#End Region
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
    Friend WithEvents numColumnsEdit As System.Windows.Forms.TextBox
    Friend WithEvents numRowsEdit As System.Windows.Forms.TextBox
    Friend WithEvents centerDotRowEdit As System.Windows.Forms.TextBox
    Friend WithEvents centerDotColumnEdit As System.Windows.Forms.TextBox
    Friend WithEvents rowPitchEdit As System.Windows.Forms.TextBox
    Friend WithEvents columnPitchEdit As System.Windows.Forms.TextBox
    Friend WithEvents minAreaEdit As System.Windows.Forms.TextBox
    Friend WithEvents maxRMSErrorEdit As System.Windows.Forms.TextBox
    Friend WithEvents myDisplay As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents calibrateCmd As System.Windows.Forms.Button
    Friend WithEvents computedRMSText As System.Windows.Forms.TextBox
    Friend WithEvents myDisplayToolbar As Cognex.VisionPro.CogDisplayToolbarV2
    Friend WithEvents myDisplayStatusBar As Cognex.VisionPro.CogDisplayStatusBarV2
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(GridOfDotsForm))
      Me.numColumnsEdit = New System.Windows.Forms.TextBox
      Me.numRowsEdit = New System.Windows.Forms.TextBox
      Me.centerDotRowEdit = New System.Windows.Forms.TextBox
      Me.centerDotColumnEdit = New System.Windows.Forms.TextBox
      Me.rowPitchEdit = New System.Windows.Forms.TextBox
      Me.columnPitchEdit = New System.Windows.Forms.TextBox
      Me.minAreaEdit = New System.Windows.Forms.TextBox
      Me.maxRMSErrorEdit = New System.Windows.Forms.TextBox
      Me.myDisplay = New Cognex.VisionPro.Display.CogDisplay
      Me.calibrateCmd = New System.Windows.Forms.Button
      Me.computedRMSText = New System.Windows.Forms.TextBox
      Me.myDisplayToolbar = New Cognex.VisionPro.CogDisplayToolbarV2
      Me.myDisplayStatusBar = New Cognex.VisionPro.CogDisplayStatusBarV2
      Me.Label1 = New System.Windows.Forms.Label
      Me.Label2 = New System.Windows.Forms.Label
      Me.Label3 = New System.Windows.Forms.Label
      Me.Label4 = New System.Windows.Forms.Label
      Me.Label5 = New System.Windows.Forms.Label
      Me.Label6 = New System.Windows.Forms.Label
      Me.Label7 = New System.Windows.Forms.Label
      Me.Label8 = New System.Windows.Forms.Label
      Me.Label9 = New System.Windows.Forms.Label
      Me.TextBox1 = New System.Windows.Forms.TextBox
      CType(Me.myDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'numColumnsEdit
      '
      Me.numColumnsEdit.Location = New System.Drawing.Point(120, 0)
      Me.numColumnsEdit.Name = "numColumnsEdit"
      Me.numColumnsEdit.TabIndex = 0
      Me.numColumnsEdit.Text = "5"
      '
      'numRowsEdit
      '
      Me.numRowsEdit.Location = New System.Drawing.Point(120, 40)
      Me.numRowsEdit.Name = "numRowsEdit"
      Me.numRowsEdit.TabIndex = 1
      Me.numRowsEdit.Text = "4"
      '
      'centerDotRowEdit
      '
      Me.centerDotRowEdit.Location = New System.Drawing.Point(120, 80)
      Me.centerDotRowEdit.Name = "centerDotRowEdit"
      Me.centerDotRowEdit.TabIndex = 2
      Me.centerDotRowEdit.Text = "2"
      '
      'centerDotColumnEdit
      '
      Me.centerDotColumnEdit.Location = New System.Drawing.Point(120, 120)
      Me.centerDotColumnEdit.Name = "centerDotColumnEdit"
      Me.centerDotColumnEdit.TabIndex = 3
      Me.centerDotColumnEdit.Text = "2"
      '
      'rowPitchEdit
      '
      Me.rowPitchEdit.Location = New System.Drawing.Point(392, 0)
      Me.rowPitchEdit.Name = "rowPitchEdit"
      Me.rowPitchEdit.TabIndex = 4
      Me.rowPitchEdit.Text = "1"
      '
      'columnPitchEdit
      '
      Me.columnPitchEdit.Location = New System.Drawing.Point(392, 40)
      Me.columnPitchEdit.Name = "columnPitchEdit"
      Me.columnPitchEdit.TabIndex = 5
      Me.columnPitchEdit.Text = "1"
      '
      'minAreaEdit
      '
      Me.minAreaEdit.Location = New System.Drawing.Point(392, 80)
      Me.minAreaEdit.Name = "minAreaEdit"
      Me.minAreaEdit.TabIndex = 6
      Me.minAreaEdit.Text = "250"
      '
      'maxRMSErrorEdit
      '
      Me.maxRMSErrorEdit.Location = New System.Drawing.Point(392, 120)
      Me.maxRMSErrorEdit.Name = "maxRMSErrorEdit"
      Me.maxRMSErrorEdit.TabIndex = 7
      Me.maxRMSErrorEdit.Text = "0.5"
      '
      'myDisplay
      '
      Me.myDisplay.Location = New System.Drawing.Point(32, 184)
      Me.myDisplay.Name = "myDisplay"
      Me.myDisplay.OcxState = CType(resources.GetObject("myDisplay.OcxState"), System.Windows.Forms.AxHost.State)
      Me.myDisplay.Size = New System.Drawing.Size(664, 272)
      Me.myDisplay.TabIndex = 8
      '
      'calibrateCmd
      '
      Me.calibrateCmd.Location = New System.Drawing.Point(544, 0)
      Me.calibrateCmd.Name = "calibrateCmd"
      Me.calibrateCmd.Size = New System.Drawing.Size(88, 48)
      Me.calibrateCmd.TabIndex = 9
      Me.calibrateCmd.Text = "Calibrate"
      '
      'computedRMSText
      '
      Me.computedRMSText.BackColor = System.Drawing.Color.Silver
      Me.computedRMSText.Location = New System.Drawing.Point(544, 120)
      Me.computedRMSText.Name = "computedRMSText"
      Me.computedRMSText.Size = New System.Drawing.Size(144, 20)
      Me.computedRMSText.TabIndex = 10
      Me.computedRMSText.Text = "0"
      '
      'myDisplayToolbar
      '
      Me.myDisplayToolbar.Enabled = True
      Me.myDisplayToolbar.Location = New System.Drawing.Point(40, 152)
      Me.myDisplayToolbar.Name = "myDisplayToolbar"
      Me.myDisplayToolbar.Size = New System.Drawing.Size(592, 26)
      Me.myDisplayToolbar.TabIndex = 11
      '
      'myDisplayStatusBar
      '
      Me.myDisplayStatusBar.Enabled = True
      Me.myDisplayStatusBar.Location = New System.Drawing.Point(32, 464)
      Me.myDisplayStatusBar.Name = "myDisplayStatusBar"
      Me.myDisplayStatusBar.Size = New System.Drawing.Size(600, 21)
      Me.myDisplayStatusBar.TabIndex = 12
      '
      'Label1
      '
      Me.Label1.Location = New System.Drawing.Point(0, 0)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(112, 23)
      Me.Label1.TabIndex = 14
      Me.Label1.Text = "Number of Columns"
      '
      'Label2
      '
      Me.Label2.Location = New System.Drawing.Point(0, 40)
      Me.Label2.Name = "Label2"
      Me.Label2.TabIndex = 15
      Me.Label2.Text = "Number of Rows"
      '
      'Label3
      '
      Me.Label3.Location = New System.Drawing.Point(0, 80)
      Me.Label3.Name = "Label3"
      Me.Label3.TabIndex = 16
      Me.Label3.Text = "Row Origin"
      '
      'Label4
      '
      Me.Label4.Location = New System.Drawing.Point(0, 120)
      Me.Label4.Name = "Label4"
      Me.Label4.TabIndex = 17
      Me.Label4.Text = "Column Origin"
      '
      'Label5
      '
      Me.Label5.Location = New System.Drawing.Point(272, 8)
      Me.Label5.Name = "Label5"
      Me.Label5.TabIndex = 18
      Me.Label5.Text = "Row Pitch"
      '
      'Label6
      '
      Me.Label6.Location = New System.Drawing.Point(272, 48)
      Me.Label6.Name = "Label6"
      Me.Label6.TabIndex = 19
      Me.Label6.Text = "Column Pitch"
      '
      'Label7
      '
      Me.Label7.Location = New System.Drawing.Point(280, 80)
      Me.Label7.Name = "Label7"
      Me.Label7.TabIndex = 20
      Me.Label7.Text = "Min Area"
      '
      'Label8
      '
      Me.Label8.Location = New System.Drawing.Point(280, 120)
      Me.Label8.Name = "Label8"
      Me.Label8.TabIndex = 21
      Me.Label8.Text = "Max RMS Error"
      '
      'Label9
      '
      Me.Label9.Location = New System.Drawing.Point(544, 88)
      Me.Label9.Name = "Label9"
      Me.Label9.Size = New System.Drawing.Size(136, 23)
      Me.Label9.TabIndex = 22
      Me.Label9.Text = "Computed RMS Error"
      '
      'TextBox1
      '
      Me.TextBox1.AcceptsReturn = True
      Me.TextBox1.AcceptsTab = True
      Me.TextBox1.Location = New System.Drawing.Point(32, 496)
      Me.TextBox1.Multiline = True
      Me.TextBox1.Name = "TextBox1"
      Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
      Me.TextBox1.Size = New System.Drawing.Size(672, 104)
      Me.TextBox1.TabIndex = 23
      Me.TextBox1.Text = "This sample provides the means to use a grid-of-dots in order to calibrate an ima" & _
      "ge from image coordinates to physical coordinates. Each time the Calibrate butto" & _
      "n is hit, the next grid-of-dots image is loaded from the image file gridofdots.i" & _
      "db and calibration occurs.  Calibration employs the following parameters:  " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Num" & _
      "ber of Columns: The number of columns in the grid. " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Number of Rows: The number " & _
      "of rows in the grid. " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Row Origin: The 1-based index of the row element whose ce" & _
      "nter of mass will be used for the Y coordinate of the grid origin. " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Column Orig" & _
      "in: The 1-based index of the column element whose center of mass will be used fo" & _
      "r the X coordinate of the grid origin. " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Row Pitch: The physical distance betwee" & _
      "n dots in a row. " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Column Pitch: The physical distance between dots in a column." & _
      " " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Min Area: The minimum area in pixels for a black object to be considered a do" & _
      "t. " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Max RMS Error: The maximum allowable RMS error for calibration. " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Calibrati" & _
      "on will fail if either the number of required dots is not found or the RMS error" & _
      " exceeds Max RMS Error."
      '
      'GridOfDotsForm
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(768, 622)
      Me.Controls.Add(Me.TextBox1)
      Me.Controls.Add(Me.Label9)
      Me.Controls.Add(Me.Label8)
      Me.Controls.Add(Me.Label7)
      Me.Controls.Add(Me.Label6)
      Me.Controls.Add(Me.Label5)
      Me.Controls.Add(Me.Label4)
      Me.Controls.Add(Me.Label3)
      Me.Controls.Add(Me.Label2)
      Me.Controls.Add(Me.Label1)
      Me.Controls.Add(Me.myDisplayStatusBar)
      Me.Controls.Add(Me.myDisplayToolbar)
      Me.Controls.Add(Me.computedRMSText)
      Me.Controls.Add(Me.calibrateCmd)
      Me.Controls.Add(Me.myDisplay)
      Me.Controls.Add(Me.maxRMSErrorEdit)
      Me.Controls.Add(Me.minAreaEdit)
      Me.Controls.Add(Me.columnPitchEdit)
      Me.Controls.Add(Me.rowPitchEdit)
      Me.Controls.Add(Me.centerDotColumnEdit)
      Me.Controls.Add(Me.centerDotRowEdit)
      Me.Controls.Add(Me.numRowsEdit)
      Me.Controls.Add(Me.numColumnsEdit)
      Me.Name = "GridOfDotsForm"
      Me.Text = "Grid of Dots Calibration"
      CType(Me.myDisplay, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region "Form and Controls Events"
    Private Sub GridOfDotsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Dim VPRO_PATH As String
      VPRO_PATH = Environment.GetEnvironmentVariable("VPRO_ROOT")
      If VPRO_PATH = "" Then _
        DisplayErrorAndExit("Required environment variable VPRO_ROOT not set.")

      myImageFileTool = New CogImageFileTool
      myCalTool = New CogCalibNPointToNPointTool
      myBlobTool = New CogBlobTool

      Try
        myImageFileTool.[Operator].Open(VPRO_PATH & "/images/gridofdots.idb", CogImageFileModeConstants.Read)

        numColumns = numColumnsEdit.Text
        numRows = numRowsEdit.Text
        rowOrigin = centerDotRowEdit.Text
        columnOrigin = centerDotColumnEdit.Text
        rowPitch = Double.Parse(rowPitchEdit.Text, CultureInfo.InvariantCulture)
        columnPitch = Double.Parse(columnPitchEdit.Text, CultureInfo.InvariantCulture)
        minArea = Double.Parse(minAreaEdit.Text, CultureInfo.InvariantCulture)
        maxRMSError = Double.Parse(maxRMSErrorEdit.Text, CultureInfo.InvariantCulture)
      Catch cogex As CogException
        DisplayErrorAndExit("Following Specific Cognex Error Occured:" & cogex.Message)
      Catch ex As Exception

        DisplayErrorAndExit(VPRO_PATH & "\images\gridofdots.idb not found")
      End Try
    End Sub
    Private Sub GridOfDotsForm_closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed

      If Not myImageFileTool Is Nothing Then
        myImageFileTool.[Operator].Close()
        myImageFileTool.Dispose()
      End If
      If Not myCalTool Is Nothing Then myCalTool.Dispose()
      If Not myBlobTool Is Nothing Then myBlobTool.Dispose()
    End Sub

    Private Sub calibrateCmd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calibrateCmd.Click
      Dim Bx(100) As Double, By(100) As Double, Barea(100) As Double
      Dim BID(100) As Integer
      Dim i As Integer, j As Integer
      Dim centerPointX As Double, centerPointY As Double
      Windows.Forms.Cursor.Current = Cursors.WaitCursor()

      myDisplayStatusBar.CoordinateSpaceName = "#"

      While myCalTool.Calibration.NumPoints > 0
        myCalTool.Calibration.DeletePointPair(0)
      End While

      ' Get the next image
      myImageFileTool.Run()

      myDisplayToolbar.Display = myDisplay
      myDisplayStatusBar.Display = myDisplay
      myDisplay.StaticGraphics.Clear()

      myDisplay.Image = myImageFileTool.OutputImage
      myDisplay.Fit()

      ' Get Blob tool image
      myBlobTool.InputImage = myImageFileTool.OutputImage
      ' Look for dark blobs
      myBlobTool.RunParams.SegmentationParams.Polarity = CogBlobSegmentationPolarityConstants.DarkBlobs
      myBlobTool.RunParams.SortAscending = True
      myBlobTool.RunParams.SortEnabled = True
      myBlobTool.RunParams.SortMeasure = CogBlobMeasureConstants.CenterMassX
      ' reject blobs that are smaller than minArea
      myBlobTool.RunParams.ConnectivityMinPixels = minArea
      myBlobTool.Run()

      ' Confirm the number of blobs located is the number that was expected
      If (numRows * numColumns) = myBlobTool.Results.GetBlobs.Count Then

        myCalTool.InputImage = myImageFileTool.OutputImage

        ' Sort the points
        Dim sortedPoints As New ArrayList
        SortDots(numColumns, numRows, myBlobTool.Results, sortedPoints)

        'Add graphics
        Dim myTextGraphic As CogGraphicLabel
        Dim myPointMarker As CogPointMarker
        Dim currentResult As CogBlobResult
        For i = 0 To sortedPoints.Count - 1
          myTextGraphic = New CogGraphicLabel
          'Dim myFont As New Font("Times", 14)

          'myTextGraphic.Font = myFont
          myTextGraphic.Text = Str(i)
          currentResult = sortedPoints(i)
          myTextGraphic.X = currentResult.CenterOfMassX
          myTextGraphic.Y = currentResult.CenterOfMassY - 10
          myDisplay.StaticGraphics.Add(myTextGraphic, "test")
          myPointMarker = New CogPointMarker
          myPointMarker.Color = CogColorConstants.Green
          myPointMarker.X = currentResult.CenterOfMassX
          myPointMarker.Y = currentResult.CenterOfMassY
          myDisplay.StaticGraphics.Add(myPointMarker, "test")
        Next

        ' Add Calibration points to NPoint Calibration Tool
        For i = 0 To numColumns - 1
          For j = 0 To numRows - 1
            currentResult = sortedPoints(numRows * i + j)
            myCalTool.Calibration.AddPointPair(currentResult.CenterOfMassX, _
                                               currentResult.CenterOfMassY, _
                                               i * rowPitch, _
                                               j * columnPitch)

          Next
        Next

        'Determine the origin
        currentResult = sortedPoints((columnOrigin - 1) * numRows + rowOrigin - 1)
        centerPointX = currentResult.CenterOfMassX
        centerPointY = currentResult.CenterOfMassY

        ' Config Calibration tool and execute
        ' Enable all degrees of freedom
        myCalTool.Calibration.DOFsToCompute = CogNPointToNPointDOFConstants.ScalingAspectRotationSkewAndTranslation
        ' Set the origin in calibrated space
        myCalTool.Calibration.CalibratedOriginX = centerPointX
        myCalTool.Calibration.CalibratedOriginY = centerPointY
        ' Don't rotate the origin of the calibrated image.
        myCalTool.Calibration.CalibratedXAxisRotation = 0
        myCalTool.RunParams.CalibratedSpaceName = "Calibrated"
        ' The origin is specified in pixels, so it's in uncalibrated space
        myCalTool.Calibration.CalibratedOriginSpace = CogCalibNPointAdjustmentSpaceConstants.Uncalibrated
        ' We want no origin rotation with respect to raw calibrated space.
        myCalTool.Calibration.CalibratedXAxisRotationSpace = CogCalibNPointAdjustmentSpaceConstants.RawCalibrated
        myCalTool.Calibration.Calibrate()
        myCalTool.Run()

        ' Draw the origin
        Dim myCogCoordinateAxes As New CogCoordinateAxes

        ' The transform to calibrated space from pixel space (# space) is the relationshp between
        ' the origin of the two spaces.  Applying the transform to the (0,0) position in pixel space
        ' yields the calibrated space origin.

        myCogCoordinateAxes.Transform = myCalTool.OutputImage.GetTransform("#", "Calibrated")
        myDisplay.StaticGraphics.Add(myCogCoordinateAxes, "test")

        myDisplayStatusBar.CoordinateSpaceName = "Calibrated"
        Windows.Forms.Cursor.Current = Cursors.Default
        ' Check the error tolerance and tool failure
        computedRMSText.Text = Format(myCalTool.Calibration.ComputedRMSError, "0.000")
        If myCalTool.Calibration.Calibrated <> True Or _
           myCalTool.Calibration.ComputedRMSError > maxRMSError Then
          MessageBox.Show("Calibration Failed")
        End If
      Else
        Windows.Forms.Cursor.Current = Cursors.Default
        MessageBox.Show("Wrong number of dots found")

      End If
    End Sub
    Private Sub rowPitchEdit_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles rowPitchEdit.Validating
      If rowPitchEdit.Text <= 0 Then
        rowPitchEdit.Text = rowPitch.ToString(CultureInfo.InvariantCulture)
        MessageBox.Show("Row Pitch must be > 0")
      End If
      rowPitch = Double.Parse(rowPitchEdit.Text(), CultureInfo.InvariantCulture)
    End Sub

    Private Sub numColumnsEdit_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles numColumnsEdit.Validating
      If numColumnsEdit.Text < 2 Then
        numColumnsEdit.Text = numColumns
        MessageBox.Show("Number of columns must be > 1")
      End If
      numColumns = numColumnsEdit.Text
    End Sub

    Private Sub numRowsEdit_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles numRowsEdit.Validating
      If numRowsEdit.Text < 2 Then
        numRowsEdit.Text = numRows
        MessageBox.Show("Number of rows must be > 1")
      End If
      numRows = numRowsEdit.Text
    End Sub

    Private Sub centerDotRowEdit_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles centerDotRowEdit.Validating
      If centerDotRowEdit.Text < 1 Then
        centerDotRowEdit.Text = rowOrigin
        MessageBox.Show("Row Origin must be >= 1")
      End If
      rowOrigin = centerDotRowEdit.Text
    End Sub

    Private Sub centerDotColumnEdit_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles centerDotColumnEdit.Validating
      If centerDotColumnEdit.Text < 1 Then
        centerDotColumnEdit.Text = columnOrigin
        MessageBox.Show("Column Origin must be >= 1")
      End If
      columnOrigin = centerDotColumnEdit.Text
    End Sub

    Private Sub columnPitchEdit_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles columnPitchEdit.Validating
      If columnPitchEdit.Text <= 0 Then
        columnPitchEdit.Text = columnPitch.ToString(CultureInfo.InvariantCulture)
        MessageBox.Show("Column Pitch must be > 0")
      End If
      columnPitch = Double.Parse(columnPitchEdit.Text, CultureInfo.InvariantCulture)
    End Sub

    Private Sub minAreaEdit_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles minAreaEdit.Validating
      If minAreaEdit.Text < 1 Then
        minAreaEdit.Text = minArea.ToString(CultureInfo.InvariantCulture)
        MessageBox.Show("Min area must be >= 1")
      End If
      minArea = Double.Parse(minAreaEdit.Text, CultureInfo.InvariantCulture)
    End Sub

    Private Sub maxRMSErrorEdit_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles maxRMSErrorEdit.Validating
      If maxRMSErrorEdit.Text <= 0 Then
        maxRMSErrorEdit.Text = maxRMSError.ToString(CultureInfo.InvariantCulture)
        MessageBox.Show("Max RMS Error must be > 0")
      End If
      maxRMSError = Double.Parse(maxRMSErrorEdit.Text, CultureInfo.InvariantCulture)
    End Sub
#End Region
#Region "Module Level Routines"
    ' Displays an error message and then exits the program.
    ' Call this when an unrecoverable error has occurred.
    Private Sub DisplayErrorAndExit(ByVal ErrorMsg As String)
      MessageBox.Show(ErrorMsg & Environment.NewLine & "Press OK to exit.")
      Me.Close()
      End      ' Quit if it is called from Form_Load
    End Sub

    'This method sorts the dots found.  The upper left hand corner is the 0th dot.  Numbering proceeds in column
    ' then row order
    Private Sub SortDots(ByRef rowSize As Integer, ByRef columnSize As Integer, _
    ByRef inputDots As CogBlobResults, ByRef sortedDots As ArrayList)
      Dim columnDots As New ArrayList
      Dim resultDots As New ArrayList
      Dim i As Integer, j As Integer


      For i = 0 To inputDots.GetBlobs.Count - 1
        columnDots.Add(inputDots.GetBlobs.Item(i))
      Next

      For i = 0 To rowSize - 1
        GenerateSortedColumn(columnSize, columnDots, resultDots)
        For j = 0 To resultDots.Count - 1
          sortedDots.Add(resultDots.Item(0))
          resultDots.RemoveAt(0)
        Next
      Next
    End Sub

    'This method finds the dot representing the top/leftmost dot.  It assumes the inputDots are already
    'sorted in ascending order of X position (pixel coordinates).
    Private Function FindTopLeftColumnDot(ByRef inputDots As ArrayList) As Integer

      Dim i As Integer
      Dim bestDotIndex As Integer
      Dim bestDot As CogBlobResult
      Dim currentDot As CogBlobResult

      bestDotIndex = 0

      bestDot = inputDots.Item(bestDotIndex)
      For i = bestDotIndex + 1 To inputDots.Count - 1
        currentDot = inputDots.Item(i)
        ' note that 1.5 * sideX length is used because the minimum horiz & vertical distance
        ' between 2 dots is 2 * side length and the center of mass comparison needs to be less
        ' than this.
        If currentDot.CenterOfMassX < _
             bestDot.CenterOfMassX + _
                1.5 * bestDot.GetBoundingBox(CogBlobAxisConstants.Principal).SideXLength And _
           currentDot.CenterOfMassY < bestDot.CenterOfMassY Then
          bestDot = currentDot
          bestDotIndex = i
        End If
      Next
      FindTopLeftColumnDot = bestDotIndex
    End Function

    'This method generates the left most column of dots, sorted from image top to image bottom in pixel space
    Private Sub GenerateSortedColumn(ByRef columnSize As Integer, ByRef inputDots As ArrayList, ByRef columnDots As ArrayList)

      Dim topLeftIndex As Integer, nextIndex As Integer, i As Integer
      Dim currentTopDot As CogBlobResult

      topLeftIndex = FindTopLeftColumnDot(inputDots)
      currentTopDot = inputDots.Item(topLeftIndex)
      columnDots.Add(currentTopDot)
      inputDots.RemoveAt(topLeftIndex)
      For i = 1 To columnSize - 1
        nextIndex = FindNextColumnDot(currentTopDot, inputDots)
        currentTopDot = inputDots.Item(nextIndex)
        columnDots.Add(currentTopDot)
        inputDots.RemoveAt(nextIndex)
      Next
    End Sub

    'This method finds the dot below the topDot in the current column
    Private Function FindNextColumnDot(ByRef topDot As CogBlobResult, ByRef inputDots As ArrayList) As Integer
      Dim bestDotIndex As Integer
      Dim bestDot As CogBlobResult, currentDot As CogBlobResult
      Dim currentDistance As Double
      Dim i As Integer
      Dim bestDistance As Double
      bestDotIndex = 0
      bestDot = inputDots.Item(bestDotIndex)
      bestDistance = bestDot.CenterOfMassY - topDot.CenterOfMassY
      If bestDistance < 0 Then
        bestDistance = 1000000
      End If
      For i = bestDotIndex + 1 To inputDots.Count - 1
        currentDot = inputDots.Item(i)
        currentDistance = currentDot.CenterOfMassY - topDot.CenterOfMassY
        If currentDistance > 0 And _
           currentDistance < bestDistance And _
           Abs(bestDot.CenterOfMassX - currentDot.CenterOfMassX) < _
            1.5 * topDot.GetBoundingBox(CogBlobAxisConstants.Principal).SideXLength Then
          bestDistance = currentDistance
          bestDotIndex = i
          bestDot = inputDots.Item(bestDotIndex)
        End If
      Next

      FindNextColumnDot = bestDotIndex

    End Function

#End Region


  End Class
End Namespace