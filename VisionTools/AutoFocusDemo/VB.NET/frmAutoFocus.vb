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

' This program demonstrate the use of the CogImageSharpnessTool and
' CogMaximizer component to perform a simulated auto-focus operation.
'
' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
Option Explicit On 
Imports Cognex.VisionPro
Imports Cognex.VisionPro.ImageProcessing
Imports Cognex.VisionPro.ImageFile
Namespace SampleAutoFocus
  Public Class frmAutoFocus
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
    ' Friend WithEvents CogImageSharpnessToolEdit1 As Cognex.VisionPro.ImageProcessing.CogImageSharpnessToolEdit
    Friend WithEvents btnFocus As System.Windows.Forms.Button
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents GraphDisplay As Cognex.VisionPro.Display.CogDisplay
        Friend WithEvents CogImageSharpnessEdit1 As Cognex.VisionPro.ImageProcessing.CogImageSharpnessEditV2
    Friend WithEvents btnGraph As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkSlow As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAutoFocus))
      Me.GraphDisplay = New Cognex.VisionPro.Display.CogDisplay
      Me.btnFocus = New System.Windows.Forms.Button
      Me.btnLoad = New System.Windows.Forms.Button
      Me.CogImageSharpnessEdit1 = New Cognex.VisionPro.ImageProcessing.CogImageSharpnessEditV2
      Me.btnGraph = New System.Windows.Forms.Button
      Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
      Me.chkSlow = New System.Windows.Forms.CheckBox
      Me.Label1 = New System.Windows.Forms.Label
      CType(Me.GraphDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.CogImageSharpnessEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'GraphDisplay
      '
      Me.GraphDisplay.Location = New System.Drawing.Point(136, 445)
      Me.GraphDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
      Me.GraphDisplay.MouseWheelSensitivity = 1
      Me.GraphDisplay.Name = "GraphDisplay"
      Me.GraphDisplay.OcxState = CType(resources.GetObject("GraphDisplay.OcxState"), System.Windows.Forms.AxHost.State)
      Me.GraphDisplay.Size = New System.Drawing.Size(736, 192)
      Me.GraphDisplay.TabIndex = 0
      '
      'btnFocus
      '
      Me.btnFocus.Location = New System.Drawing.Point(8, 498)
      Me.btnFocus.Name = "btnFocus"
      Me.btnFocus.Size = New System.Drawing.Size(88, 40)
      Me.btnFocus.TabIndex = 4
      Me.btnFocus.Text = "Auto Focus"
      '
      'btnLoad
      '
      Me.btnLoad.Location = New System.Drawing.Point(8, 588)
      Me.btnLoad.Name = "btnLoad"
      Me.btnLoad.Size = New System.Drawing.Size(88, 40)
      Me.btnLoad.TabIndex = 5
      Me.btnLoad.Text = "Load New File"
      '
      'CogImageSharpnessEdit1
      '
      Me.CogImageSharpnessEdit1.Location = New System.Drawing.Point(8, 8)
      Me.CogImageSharpnessEdit1.MinimumSize = New System.Drawing.Size(489, 0)
      Me.CogImageSharpnessEdit1.Name = "CogImageSharpnessEdit1"
      Me.CogImageSharpnessEdit1.Size = New System.Drawing.Size(864, 410)
      Me.CogImageSharpnessEdit1.SuspendElectricRuns = False
      Me.CogImageSharpnessEdit1.TabIndex = 6
      '
      'btnGraph
      '
      Me.btnGraph.Location = New System.Drawing.Point(8, 436)
      Me.btnGraph.Name = "btnGraph"
      Me.btnGraph.Size = New System.Drawing.Size(88, 40)
      Me.btnGraph.TabIndex = 7
      Me.btnGraph.Text = "Create Graph"
      '
      'chkSlow
      '
      Me.chkSlow.Location = New System.Drawing.Point(12, 544)
      Me.chkSlow.Name = "chkSlow"
      Me.chkSlow.Size = New System.Drawing.Size(104, 24)
      Me.chkSlow.TabIndex = 8
      Me.chkSlow.Text = "Slow Down"
      '
      'Label1
      '
      Me.Label1.AutoSize = True
      Me.Label1.Location = New System.Drawing.Point(133, 429)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(486, 13)
      Me.Label1.TabIndex = 9
      Me.Label1.Text = "Click and drag the yellow line to load images of the same scene taken with differ" & _
          "ent focal adjustments."
      '
      'frmAutoFocus
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(876, 642)
      Me.Controls.Add(Me.Label1)
      Me.Controls.Add(Me.chkSlow)
      Me.Controls.Add(Me.btnGraph)
      Me.Controls.Add(Me.CogImageSharpnessEdit1)
      Me.Controls.Add(Me.btnLoad)
      Me.Controls.Add(Me.btnFocus)
      Me.Controls.Add(Me.GraphDisplay)
      Me.Name = "frmAutoFocus"
      Me.Text = "Create Graph"
      CType(Me.GraphDisplay, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.CogImageSharpnessEdit1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)
      Me.PerformLayout()

    End Sub

#End Region
#Region "Module Level Vars"
    '
    Private mSharpnessTool As CogImageSharpnessTool
    Private mSharpness As CogImageSharpness         ' RunParams of the tool
    Private mMaximizer As CogMaximizer              ' Performs a maximization search
    Private mRegion As Cognex.VisionPro.ICogRegion                    ' Processing region of the tool
    Dim GraphData(,) As Long
    ' The following are used to load images from an image database file, in
    ' order to simulate moving a focus mechanism and acquiring images. The
    ' image database must contain images in order of increasing or decreasing
    ' focus position.
    Private mImageFile As CogImageFileTool                     ' Loads images from the image database

    ' The following are used to generate the focus graph
    Private GraphHeight As Long, GraphWidth As Long            ' Pixel dimensions of the graph
    Private mGraph(3) As CogGeneralContour         ' Used to draw the sharpness plots
    Private mSlider As CogLineSegment                          ' Vertical slider on the graph
    Private Slider As Cognex.VisionPro.ICogGraphicInteractive          ' Sinks events from the slider graphic
    Private mLastPosition As Long                              ' Last slider position
    Private Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
#End Region

#Region "Form and Controls Events"
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      ' First create an image file tool
      mImageFile = New CogImageFileTool

      mSlider = New CogLineSegment

      ' Initialization

      GraphHeight = GraphDisplay.Height
      GraphWidth = GraphDisplay.Width
      mMaximizer = New CogMaximizer
      AddHandler mMaximizer.Evaluate, AddressOf mMaximizer_Evaluate
      mSharpnessTool = New CogImageSharpnessTool
      AddHandler mSharpnessTool.Changed, AddressOf mSharpnessTool_Change
      mSharpness = mSharpnessTool.RunParams
      AddHandler mSharpness.Changed, AddressOf mSharpness_Change
      CogImageSharpnessEdit1.Subject = mSharpnessTool

      ' Initialize the graphics
      InitGraph(0, CogColorConstants.Red, "Mode=Band Pass")
      InitGraph(1, CogColorConstants.Green, "Mode=Autocorrelation")
      InitGraph(2, CogColorConstants.Cyan, "Mode=Absolute Difference")
      InitGraph(3, CogColorConstants.Orange, "Mode=Gradient Energy")

      Slider = mSlider     ' Sinks dragging events
      AddHandler Slider.Dragging, AddressOf Slider_Dragging
      AddHandler Slider.DraggingStopped, AddressOf Slider_DraggingStopped
      Slider.Color = CogColorConstants.Yellow
      Slider.SelectedColor = CogColorConstants.Yellow
      Slider.Interactive = True
      mSlider.GraphicDOFEnable = CogLineSegmentDOFConstants.Position
      mSlider.SelectedSpaceName = "*" ' Display coordinates
      mSlider.TipText = "Click and drag to show different images"
      GraphDisplay.InteractiveGraphics.Add(mSlider, "Slider", False)

      ' Open the image database containing focus images
      Dim VproRoot As String
      VproRoot = Environment.GetEnvironmentVariable("VPRO_ROOT") ' This should be set to the install directory
      If VproRoot = "" Then
        OpenFileDialog1.FileName = "Focus.idb"
        LoadFile()
      Else
        OpenFileDialog1.FileName = VproRoot & "\Images\Focus.idb"
        OpenImageFile()
      End If
      If mImageFile.[Operator].OpenMode = CogImageFileModeConstants.Closed Then End ' Exit if user cancelled

      ' Give the tool a region
      Dim Rect As New CogRectangle
      Rect.Interactive = True
      Rect.GraphicDOFEnable = CogRectangleDOFConstants.All
      Rect.SetXYWidthHeight(230, 45, 40, 40)
      mSharpnessTool.Region = Rect
    End Sub
    Private Sub btnFocus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFocus.Click
      ' Call the Execute method of the CogMaximizer to iteratively find the
      ' best focus position. This method fires the Evaluate event at each
      ' position until it finds a maximum.
      Dim BestFocus As Long
      BestFocus = mMaximizer.Execute(0, mImageFile.[Operator].Count - 1, 1)

      ' The last position evaluated by mMaximizer.Exeute is not
      ' necessarily the best focus position, so move to the best
      ' position, acquire and process the image, and update the display
      MoveAcquireAndProcess(BestFocus)
    End Sub
    Private Sub btnGraph2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGraph.Click

            Windows.Forms.Cursor.Current = Cursors.WaitCursor
      InvalidateGraph()
      Dim OldPosition As Long
      OldPosition = SliderPosition(mSlider)

      ' Suspend events on the CogImageSharpness object so that the control
      ' does not refresh every time we change the mode.
      Dim EventConfig As ICogChangedEvent
      EventConfig = mSharpness
      EventConfig.SuspendChangedEvent()

      ' Get the data for the graphs

      GraphData = GenerateGraphData()

      ' Update the graph contours
      Dim nImages As Long = mImageFile.Operator.Count
      Dim Axis As Long, Position As Long
      Dim endPosition As Long = nImages - 1
      For Axis = 0 To 3
        mGraph(Axis).RemoveSegments(mGraph(Axis).CreateSegmentIterator(0), _
          Nothing)

        For Position = 1 To endPosition
          Dim eStartFlags As CogGeneralContourVertexConstants
          Dim eEndFlags As CogGeneralContourVertexConstants

          eStartFlags = IIf(Position = 1, _
            CogGeneralContourVertexConstants.FlagNone, _
            CogGeneralContourVertexConstants.Connected)
          eEndFlags = CogGeneralContourVertexConstants.FlagNone

          Dim dStartX As Double, dStartY As Double
          Dim dEndX As Double, dEndY As Double

          dStartX = IIf(Position = 1, PositionToGraphX(0), 0.0)
          dStartY = IIf(Position = 1, GraphData(0, Axis), 0.0)

          dEndX = PositionToGraphX(Position)
          dEndY = GraphData(Position, Axis)

          mGraph(Axis).AddLineSegment(Nothing, eStartFlags, _
            dStartX, dStartY, eEndFlags, dEndX, dEndY)

        Next Position
        mGraph(Axis).Visible = True
      Next Axis

      ' Restore things & select the active graph
      MoveAcquireAndProcess(OldPosition)
            Windows.Forms.Cursor.Current = Cursors.Default
      EventConfig.ResumeAndRaiseChangedEvent()
      SelectActiveGraph()
    End Sub
#End Region
#Region "Module Level Helper Routines"
    '******************************************************************************
    ' Code in the following section is related to acquiring images and measuring
    ' the sharpness.
    '******************************************************************************

    ' This subroutine simulates moving a focus mechanism to a specified
    ' position and acquiring an image. It does this using an image
    ' database containing images acquired over a range of focus positions.
    ' After the image is acquired, it runs the tool to measure the
    ' sharpness at the current position
    Private Function MoveAcquireAndProcess(ByVal Position As Long, Optional ByVal MoveSlider As Boolean = True) As Long
      ' Acquire an image at the specified position
      mSharpnessTool.InputImage = mImageFile.[Operator].Item(Position)

      ' Run the tool
      mSharpnessTool.Run()

      ' If it fails (typically because the ROI is outside the image) then
      ' just return zero.
      If Not mSharpnessTool.RunStatus.Exception Is Nothing Then
        MoveAcquireAndProcess = 0
      Else
        ' Need to scale the sharpness measurement into a reasonable range for
        ' the CogMaximizer's Fitness parameter (a 32 bit integer)
        Select Case mSharpness.Mode
          Case CogImageSharpnessModeConstants.AbsDiff, CogImageSharpnessModeConstants.AutoCorrelation, CogImageSharpnessModeConstants.GradientEnergy
            MoveAcquireAndProcess = mSharpnessTool.Score * 1000000
          Case Else
            MoveAcquireAndProcess = mSharpnessTool.Score
        End Select
      End If

      ' If requested, move the slider to the corresponding position on the graph
      If MoveSlider Then
        Dim X As Double
        X = PositionToGraphX(Position)
        mSlider.SetStartEnd(X, 0, X, GraphHeight)
      End If
      mLastPosition = Position
    End Function
    Private Sub DisplayErrorAndExit(ByVal ErrorMsg As String)
      MsgBox(ErrorMsg & Environment.NewLine & "Press OK to exit.")
      Me.Close()
      End      ' quit if it called from Form_Load
    End Sub
    ' This computes the position (image number) corresponding to the
    ' current location of the slider graphic.
    Private Function SliderPosition(ByVal Seg As CogLineSegment) As Long
      SliderPosition = Int(Seg.StartX * (mImageFile.[Operator].Count - 1) / GraphWidth + 0.5)
      If SliderPosition < 0 Then SliderPosition = 0
      If SliderPosition > mImageFile.[Operator].Count - 1 Then SliderPosition = mImageFile.[Operator].Count - 1
    End Function

    ' This computes the X coordinate on the graph corresponding to a given
    ' position (image number)
    Private Function PositionToGraphX(ByVal Position As Long) As Double
      PositionToGraphX = Position * GraphWidth / (mImageFile.[Operator].Count - 1)
    End Function

    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click

      LoadFile()
    End Sub
    ' Attempt to open an image file
    Private Sub OpenImageFile()
      Try
        ' Open the file
        Dim NewImageFile As CogImageFileTool
        NewImageFile = New CogImageFileTool
        NewImageFile.[Operator].Open(OpenFileDialog1.FileName, CogImageFileModeConstants.Read)

        ' Must have at least 2 images
        If NewImageFile.[Operator].Count < 2 Then
          Err.Raise(1, , "File must contain at least 2 images.")
        End If

        ' Close the old file & use the new one from now on
        mImageFile.[Operator].Close()
        mImageFile = NewImageFile
        InvalidateGraph()

        ' Show vertical grid lines on the graph based on the number of images
        GraphDisplay.StaticGraphics.Clear()
        Dim Position As Long, GridLine As New CogLineSegment
        GridLine.Color = &H606060
        GridLine.SelectedSpaceName = "*"
        For Position = 0 To mImageFile.[Operator].Count - 1
          Dim X As Double : X = PositionToGraphX(Position)
          GridLine.SetStartEnd(X, 0, X, GraphHeight)
          GraphDisplay.StaticGraphics.Add(GridLine, "test")
        Next Position

        ' Move to the middle position
        MoveAcquireAndProcess(mImageFile.[Operator].Count / 2)

      Catch cogex As Exceptions.CogException
        MsgBox("Following Specific Cognex Error Occured:" & cogex.Message)
        LoadFile()
      Catch ex As Exception
        DisplayErrorAndExit("Encountered the following error: " & Err.Description)
        LoadFile()

      End Try

    End Sub


    Sub LoadFile()
      OpenFileDialog1.CheckFileExists = True
      OpenFileDialog1.ReadOnlyChecked = True
      OpenFileDialog1.Title = "Open a database of focus images"
      OpenFileDialog1.Filter = "Image Dabtabase Files (*.cdb;*.idb)|*.cdb;*.idb|All Files (*.*)|*.*"
      Try
        OpenFileDialog1.ShowDialog()
        OpenImageFile()
      Catch cogex As Exceptions.CogException
        MsgBox("Following Specific Cognex Error Occured:" & cogex.Message)
      Catch ex As Exception
        DisplayErrorAndExit("Encountered the following error: " & Err.Description)
      End Try
    End Sub

    ' This event is fired by the CogMaximizer to evaluate the fitness
    ' measurement at a given position.


    '******************************************************************************
    ' Code in the following section is related to generating the sharpness
    ' graph at the bottom of the form, and to moving the vertical slider on it.
    '******************************************************************************

    ' This subroutine initializes one of the graph contours. There are three of them,
    ' one for each mode of the sharpness tool.
    Private Sub InitGraph(ByVal Axis As Integer, ByVal Color As CogColorConstants, _
        ByVal TipText As String)
      mGraph(Axis) = New CogGeneralContour
      mGraph(Axis).Color = Color
      mGraph(Axis).TipText = TipText
      mGraph(Axis).Interactive = True
      mGraph(Axis).SelectedSpaceName = "*" ' Display coordinates
      Dim GraphicInteractive As Cognex.VisionPro.ICogGraphicInteractive
      GraphicInteractive = mGraph(Axis)
      GraphicInteractive.SelectedColor = Color
      GraphDisplay.InteractiveGraphics.Add(mGraph(Axis), CStr(Axis), False)
    End Sub

    ' This moves the graph corresponding to the current Mode of the
    ' CogImageSharpness object to the front and makes its line solid,
    ' making the other lines dashed
    Private Sub SelectActiveGraph()
      Dim ActiveAxis As Integer
      Select Case mSharpness.Mode
        Case CogImageSharpnessModeConstants.BandPass
          ActiveAxis = 0
        Case CogImageSharpnessModeConstants.AutoCorrelation
          ActiveAxis = 1
        Case CogImageSharpnessModeConstants.AbsDiff
          ActiveAxis = 2
        Case CogImageSharpnessModeConstants.GradientEnergy
          ActiveAxis = 3
      End Select

      Dim Axis As Integer
      For Axis = 0 To 3
        Dim GraphicInteractive As Cognex.VisionPro.ICogGraphicInteractive
        GraphicInteractive = mGraph(Axis)
        GraphicInteractive.LineStyle = IIf(Axis = ActiveAxis, CogGraphicLineStyleConstants.Solid, Cognex.VisionPro.CogGraphicLineStyleConstants.Dot)
        GraphicInteractive.SelectedLineStyle = GraphicInteractive.LineStyle
      Next Axis
      GraphDisplay.InteractiveGraphics.MoveToFront(CStr(ActiveAxis))
      GraphDisplay.InteractiveGraphics.MoveToFront("Slider")
    End Sub


    ' This subroutine steps over all the images and generates graph data.
    ' The data are normalized to the range 0-GraphHeight
        Private Function GenerateGraphData() As Long(,)
      ReDim GraphData(mImageFile.[Operator].Count - 1, 3) 'As Long

            ' Save the old shaprness mode
            Dim OldMode As CogImageSharpnessModeConstants
            OldMode = mSharpness.Mode

            ' Step over all the images
      Dim Position As Long, MaxFitness(3) As Long
            For Position = 0 To mImageFile.[Operator].Count - 1
                ' Evaluate the sharpness using the 3 image sharpness modes
                mSharpness.Mode = CogImageSharpnessModeConstants.BandPass
                GraphData(Position, 0) = MoveAcquireAndProcess(Position, True)
                If GraphData(Position, 0) > MaxFitness(0) Then MaxFitness(0) = GraphData(Position, 0)

                mSharpness.Mode = CogImageSharpnessModeConstants.AutoCorrelation
                GraphData(Position, 1) = MoveAcquireAndProcess(Position, False)
                If GraphData(Position, 1) > MaxFitness(1) Then MaxFitness(1) = GraphData(Position, 1)

                mSharpness.Mode = CogImageSharpnessModeConstants.AbsDiff
                GraphData(Position, 2) = MoveAcquireAndProcess(Position, False)
        If GraphData(Position, 2) > MaxFitness(2) Then MaxFitness(2) = GraphData(Position, 2)

        mSharpness.Mode = CogImageSharpnessModeConstants.GradientEnergy
        GraphData(Position, 3) = MoveAcquireAndProcess(Position, False)
        If GraphData(Position, 3) > MaxFitness(3) Then MaxFitness(3) = GraphData(Position, 3)

            Next Position

            ' Normalize the data to fit in the graph
            Dim Axis As Integer
      For Axis = 0 To 3
        Dim YScale As Double
        If MaxFitness(Axis) = 0 Then
          YScale = 0
        Else
          YScale = GraphHeight / MaxFitness(Axis)
        End If
        For Position = 0 To mImageFile.[Operator].Count - 1
          GraphData(Position, Axis) = GraphHeight - GraphData(Position, Axis) * YScale
        Next Position
      Next Axis

            ' Restore the sharpness mode
            mSharpness.Mode = OldMode
            GenerateGraphData = GraphData
        End Function

    ' This subroutine hides the graph because it's no longer valid for the current
    ' settings of the tool and region.
    Private Sub InvalidateGraph()
      Dim i As Integer
      For i = 0 To 3
        mGraph(i).Visible = False
      Next i
    End Sub
#End Region
#Region "Cognex Objects Events"
    ' If the region geometry changes, hide the graph
    Private Sub mRegion_Change(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogChangedEventArgs)
      If e.StateFlags > 0 Then
        InvalidateGraph()
      End If
    End Sub

    ' If the CogImageSharpeness parameters change hide the graph
    Private Sub mSharpness_Change(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogChangedEventArgs) ' Handles mSharpness.Changed
      If e.StateFlags = CogImageSharpness.SfMode Then
        SelectActiveGraph()
      Else
        InvalidateGraph()
      End If
    End Sub

    ' This event fires whenever a property of the tool changes
    Private Sub mSharpnessTool_Change(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogChangedEventArgs) 'Handles mSharpnessTool.Changed
      ' If the tool's region changes to a different shape, hide the graph because it's no longer valid
      If Not mSharpnessTool.Region Is mRegion Then
        mRegion = mSharpnessTool.Region
        AddHandler mRegion.Changed, AddressOf mRegion_Change
        InvalidateGraph()
      End If

      ' If the tool's RunParams property has changed, need to update this form's internal reference to it
      If Not mSharpness Is mSharpnessTool.RunParams Then
        mSharpness = mSharpnessTool.RunParams
        InvalidateGraph()
      End If
    End Sub

    ' This event fires when the user is dragging the slider
    Private Sub Slider_Dragging(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogDraggingEventArgs)
      e.DragGraphic.Color = CogColorConstants.Orange
      ' Constrain the segment to be vertical and go from top to bottom of the display
      Dim Seg As CogLineSegment
      Seg = e.DragGraphic
      Seg.SetStartEnd(Seg.StartX, 0, Seg.StartX, GraphHeight)
      Dim Position As Long
      Position = SliderPosition(Seg)
      If Position <> mLastPosition Then MoveAcquireAndProcess(Position, False)
    End Sub

    ' This fires when the slider is dropped
    Private Sub Slider_DraggingStopped(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogDraggingEventArgs)
      ' Constrain the segment to be vertical and go from top to bottom of the display
      Dim Seg As CogLineSegment
      Seg = e.DragGraphic
      Dim Position As Long
      Position = SliderPosition(Seg)
      Dim X As Double
      X = PositionToGraphX(Position)
      Seg.SetStartEnd(X, 0, X, GraphHeight)

      MoveAcquireAndProcess(Position)
    End Sub
    Private Sub mMaximizer_Evaluate(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogMaximizerEvaluateEventArgs) 'Handles mMaximizer.Evaluate


      ' Move the focus mechanism, acquire a new image and measure the sharpness
      e.Fitness = MoveAcquireAndProcess(e.Position)

      If chkSlow.Checked Then
        ' Allow the user interface to refresh and pause
        System.Windows.Forms.Application.DoEvents()
        Sleep(300)
      End If

      ' Continue the maximization operation
      e.ContinueRunning = True
    End Sub
#End Region



  End Class
End Namespace
