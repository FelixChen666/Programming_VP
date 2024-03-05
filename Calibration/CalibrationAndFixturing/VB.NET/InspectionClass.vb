'*******************************************************************************
'Copyright (C) 2005 Cognex Corporation
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

Imports Cognex.VisionPro
Imports Cognex.VisionPro.Display
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.ToolGroup
Imports Cognex.VisionPro.CalibFix
Imports Cognex.VisionPro.Blob
Imports Cognex.VisionPro.Caliper
Imports Cognex.VisionPro.PMAlign
Public Class InspectionClass
    ' These members are to be used only by their corresponding property
    ' implementations.
    Private Display_ As CogDisplay
    Private Form_ As CalibrationAndFixturingForm
    Private AnalysisLevel_ As Integer
    Private ToolGraphicsVisible_ As Boolean
    Private ImageSource_ As Integer
    Private LiveVideo_ As Boolean
    Private ToolControlsVisible_ As Boolean
    Private AcqAvailable_ As Boolean = True

    ' Performance analysis.
    Private analysisTimer As New CogStopwatch

    ' Tools used.
    Dim InspectionImageFileTool As CogImageFileTool
    Dim InspectionImageAcqFifoTool As CogAcqFifoTool
    Dim CalibrationInputsToolGroup As CogToolGroup
    Dim CalibrateToMillimetersTool As CogCalibNPointToNPointTool
    Dim PMAlignInputToFixtureTool As CogPMAlignTool
    Dim NormalizationFixtureTool As CogFixtureTool
    Dim TabWidthCaliperTool As CogCaliperTool
    Dim LargeHoleBlobsTool As CogBlobTool

    ' Tool graphics displayed.
    Dim PMAlignInputToFixtureToolGraphic As New CogCompositeShape
    Dim NormalizationFixtureToolGraphic As New CogCoordinateAxes
    Dim TabWidthCaliperToolGraphic As New CogCompositeShape
    Dim LargeHoleBlob1ToolGraphic As New CogCompositeShape
    Dim LargeHoleBlob2ToolGraphic As New CogCompositeShape

    ' Sync calibration state change event.
    Dim CalibrateToMillimetersSubobject As CogCalibNPointToNPoint

    ' Sync acquisition complete event.
    'Dim WithEvents InspectionImageAcqFifoCompleteEvent As CogAcqCompleteEvent

    ' Image source enumerations.
    Public Enum ImageSourceConsts
        eImageFileImageSource = 0
        eAcqFifoImageSource
    End Enum

    ' Image analysis level enumerations.
    Public Enum ImageAnalysisLevelConsts
        eImageAnalysisLevelPMAlign = 0
        eImageAnalysisLevelFixture
        eImageAnalysisLevelCaliper
        eImageAnalysisLevelBlob
    End Enum

    ' Events sourced.
    Public Event AcquisitionCompleted()
    Public Event ResultsChanged()
    Public Event CalibrationChanged(ByVal Calibrated As Boolean)

    Private Sub CalibrateToMillimetersSubobject_Change(ByVal Sender As Object, ByVal e As CogChangedEventArgs)
        ' If the calibration state has changed, fire event indicating so.
        If (e.StateFlags & CogCalibNPointToNPoint.SfCalibrated) Then
            RaiseEvent CalibrationChanged(CalibrateToMillimetersSubobject.Calibrated)
        End If
    End Sub

    Private Sub InspectionImageAcqFifoCompleteEvent_Complete(ByVal Sender As Object, ByVal e As CogCompleteEventArgs)
        ' Fire event indicating an acquisition has completed.
        RaiseEvent AcquisitionCompleted()
    End Sub

    Public Sub New()

        ' Locate resource files using the environment variable that indicates
        ' where VisionPro is installed.  If the environment variable is not set,
        ' display the error and terminate the application.
        Dim baseDir As String
        baseDir = Environ("VPRO_ROOT")
        If baseDir = "" Then
            MsgBox("Required environment variable VPRO_ROOT not set.", vbCritical)
            End
        End If

        ' Setup the image sources.  If the default image file cannot be opened, ignore
        ' and let the user manually open it later.
        Try
            InspectionImageAcqFifoTool = New CogAcqFifoTool

        Catch ex As Exception
            AcqAvailable_ = False
        End Try

        If InspectionImageAcqFifoTool.[Operator] Is Nothing Then
            AcqAvailable_ = False
        End If

        InspectionImageFileTool = New CogImageFileTool
        Try
            InspectionImageFileTool.[Operator].Open(baseDir & "\images\bracket_std.idb", CogImageFileModeConstants.Read)

        Catch ex As Exception
            MsgBox("Unable to open image file: bracket_std.idb")
            Return
        End Try

        ' Load the VisionPro tools.
        Try
            CalibrationInputsToolGroup = CogSerializer.LoadObjectFromFile( _
              baseDir & "\Samples\Programming\Calibration\CalibrationAndFixturing\CalibrationInputs.NET.vpp")
            CalibrateToMillimetersTool = CogSerializer.LoadObjectFromFile( _
              baseDir & "\Samples\Programming\Calibration\CalibrationAndFixturing\CalibrateToMillimeters.vpp")
            PMAlignInputToFixtureTool = CogSerializer.LoadObjectFromFile( _
              baseDir & "\Samples\Programming\Calibration\CalibrationAndFixturing\PMAlignInputToFixture.vpp")
            NormalizationFixtureTool = CogSerializer.LoadObjectFromFile( _
              baseDir & "\Samples\Programming\Calibration\CalibrationAndFixturing\NormalizationFixture.vpp")
            TabWidthCaliperTool = CogSerializer.LoadObjectFromFile( _
              baseDir & "\Samples\Programming\Calibration\CalibrationAndFixturing\TabWidthCaliper.vpp")
            LargeHoleBlobsTool = CogSerializer.LoadObjectFromFile( _
              baseDir & "\Samples\Programming\Calibration\CalibrationAndFixturing\LargeHoleBlobs.vpp")

        Catch ex As Exception
            MsgBox("Unable to load persisted tool files")
            Return
        End Try
        CalibrateToMillimetersSubobject = CalibrateToMillimetersTool.Calibration
        AddHandler CalibrateToMillimetersSubobject.Changed, AddressOf CalibrateToMillimetersSubobject_Change
        If AcqAvailable_ Then
            AddHandler InspectionImageAcqFifoTool.[Operator].Complete, AddressOf InspectionImageAcqFifoCompleteEvent_Complete
        End If

    End Sub

    Public Sub RemoveHandlers()
        RemoveHandler CalibrateToMillimetersSubobject.Changed, AddressOf CalibrateToMillimetersSubobject_Change
        If AcqAvailable_ Then
            RemoveHandler InspectionImageAcqFifoTool.[Operator].Complete, AddressOf InspectionImageAcqFifoCompleteEvent_Complete
        End If

    End Sub

    Public ReadOnly Property AcqAvailable() As Boolean
        Get
            Return AcqAvailable_
        End Get
    End Property
    Public Property AnalysisLevel() As ImageAnalysisLevelConsts
        Get
            Return AnalysisLevel_
        End Get
        Set(ByVal Value As ImageAnalysisLevelConsts)
            AnalysisLevel_ = Value
        End Set
    End Property

    Public Sub Calibrate()
        ' Remove old calibration.
        CalibrateToMillimetersTool.Calibration.Uncalibrate()
        CalibrateToMillimetersTool.CalibrationImage = Nothing

        ' Get calibration image.
        Dim CalibImage As CogImage8Grey
        CalibImage = ResultsDisplay.Image

        ' Test for presence of required calibration image.
        If CalibImage Is Nothing Then
            MsgBox("No image available for calibration")
            Return
        End If
        ' Compute the calibration input points.
        Dim tempPMAlignTool As CogPMAlignTool
        Dim i As Integer
        For i = 0 To CalibrationInputsToolGroup.Tools.Count - 1
            tempPMAlignTool = CType(CalibrationInputsToolGroup.Tools(i), CogPMAlignTool)
            tempPMAlignTool.InputImage = CalibImage
        Next
        CalibrationInputsToolGroup.Run()

        ' If any calibration inputs tool group tool fails to produce its
        ' points, indicate failure.
        For i = 0 To CalibrationInputsToolGroup.Tools.Count - 1
            tempPMAlignTool = CType(CalibrationInputsToolGroup.Tools(i), CogPMAlignTool)
            If tempPMAlignTool.Results.Count <> 1 Then
                MsgBox("Calibration error")
                Return
            End If
        Next
        ' Upper-left hole coordinate is first uncalibrated point.

        For i = 0 To CalibrationInputsToolGroup.Tools.Count - 1
            tempPMAlignTool = CType(CalibrationInputsToolGroup.Tools(i), CogPMAlignTool)
            CalibrateToMillimetersTool.Calibration.SetUncalibratedPointX(i, tempPMAlignTool.Results(0).GetPose.TranslationX)
            CalibrateToMillimetersTool.Calibration.SetUncalibratedPointY(i, tempPMAlignTool.Results(0).GetPose.TranslationY)
        Next

        ' Calibrate.  This builds a coordinate space derived from the
        ' transform found that best maps the uncalibrated points configured
        ' above to the corresponding points configured in the calibration
        ' tool edit control.
        CalibrateToMillimetersTool.CalibrationImage = CalibImage
        CalibrateToMillimetersTool.Calibration.Calibrate()
    End Sub

    Public Property ToolGraphicsVisible() As Boolean
        Get
            Return ToolGraphicsVisible_
        End Get
        Set(ByVal Value As Boolean)
            ToolGraphicsVisible_ = Value
        End Set
    End Property

    Public Property ImageSource() As ImageSourceConsts
        Get
            Return ImageSource_
        End Get
        Set(ByVal Value As ImageSourceConsts)
            ImageSource_ = Value
        End Set
    End Property

    Public Property LiveVideo() As Boolean
        Get
            Return LiveVideo_
        End Get
        Set(ByVal Value As Boolean)
            ' Test for valid config
            Dim Live As Boolean = Value
            If Live And (ImageSource <> ImageSourceConsts.eAcqFifoImageSource) Then
                MsgBox("Live video requires camera image source")
                Live = False
            End If
            LiveVideo_ = Live

            'Start/stop live video.
            If Live Then
                ResultsDisplay.InteractiveGraphics.Clear()
                ResultsDisplay.StartLiveDisplay(InspectionImageAcqFifoTool.[Operator])
                MsgBox("Press OK to exit live video")
                LiveVideo_ = False
            End If
            ResultsDisplay.StopLiveDisplay()
        End Set
    End Property

    Public Sub Run()
        ' Time this routine.  Timing allows the user to determine the tool
        ' edit control and/or tool graphics overhead.
        analysisTimer.Stop()
        analysisTimer.Reset()
        analysisTimer.Start()

        ' Test for calibration.
        If CalibrateToMillimetersSubobject.Calibrated = False Then
            MsgBox("Calibrate before running")
            Exit Sub
        End If

        ' Apply calibration.
        CalibrateToMillimetersTool.InputImage = ResultsDisplay.Image
        CalibrateToMillimetersTool.Run()

        ' PMAlign is run as its result is a required input to fixture.
        If AnalysisLevel >= ImageAnalysisLevelConsts.eImageAnalysisLevelPMAlign Then
            PMAlignInputToFixtureTool.InputImage = CalibrateToMillimetersTool.OutputImage
            PMAlignInputToFixtureTool.Run()
        End If

        ' Fixture based on the result of PMAlign run above.  Must test PMAlign's
        ' result because fixture cannot run without it.
        If AnalysisLevel >= ImageAnalysisLevelConsts.eImageAnalysisLevelFixture Then
            NormalizationFixtureTool.InputImage = CalibrateToMillimetersTool.OutputImage
            Dim pmResultAvailable As Boolean
            If PMAlignInputToFixtureTool.Results Is Nothing Then
                pmResultAvailable = False
            Else
                pmResultAvailable = PMAlignInputToFixtureTool.Results.Count = 1
            End If
            If pmResultAvailable Then
                NormalizationFixtureTool.RunParams.UnfixturedFromFixturedTransform = _
                  PMAlignInputToFixtureTool.Results(0).GetPose
                NormalizationFixtureTool.Run()
            Else
                ' Set to error.  Call RunStatus Init passing an error code to indicate
                ' fixturing failure.
                MsgBox("Fixturing error")
                NormalizationFixtureTool.Run()
            End If
        End If

        ' Caliper runs on the fixture's output image.
        If AnalysisLevel >= ImageAnalysisLevelConsts.eImageAnalysisLevelCaliper Then
            TabWidthCaliperTool.InputImage = NormalizationFixtureTool.OutputImage
            TabWidthCaliperTool.Run()
        End If

        ' Blob runs on the fixture's output image.  While the blob tool's region
        ' uses the fixture output image's fixture transform, the blob tool does
        ' not.  The blob tool runs in the calibration tool's coordinate space.
        If AnalysisLevel >= ImageAnalysisLevelConsts.eImageAnalysisLevelBlob Then
            LargeHoleBlobsTool.InputImage = NormalizationFixtureTool.OutputImage
            LargeHoleBlobsTool.Run()
        End If

        ' Draw tool graphics.
        Draw()

        ' Subroutine execution time (including graphic rendering time, if
        ' enabled).
        analysisTimer.Stop()

        ' Inform clients that a new set of results is ready.
        RaiseEvent ResultsChanged()
    End Sub

    Public Property ToolControlsVisible() As Boolean
        Get
            Return ToolControlsVisible_
        End Get
        Set(ByVal Value As Boolean)
            ToolControlsVisible_ = Value
            ' Attach/detach tool control and its tool.
            If ToolControlsVisible_ Then
                AttachedForm.CogImageFileEdit1.Subject = InspectionImageFileTool
        AttachedForm.CogAcqFifoEditV21.Subject = InspectionImageAcqFifoTool
                AttachedForm.CogToolGroupEdit1.Subject = CalibrationInputsToolGroup
                AttachedForm.CogCalibNPointToNPointEdit1.Subject = CalibrateToMillimetersTool
                AttachedForm.CogPMAlignEdit1.Subject = PMAlignInputToFixtureTool
                AttachedForm.CogFixtureEdit1.Subject = NormalizationFixtureTool
                AttachedForm.CogCaliperEdit1.Subject = TabWidthCaliperTool
                AttachedForm.CogBlobEdit1.Subject = LargeHoleBlobsTool
            Else
                ' Leaving a tool attached to an invisible tool edit control will incur
                ' overhead associated with synchronizing edit control fields with tool
                ' properties.  Since the edit control is not visible, the synch-
                ' ronization and resulting overhead is unnecessary.  By detaching the
                ' tool when the control is not visible, we eliminate the extraneous
                ' overhead.
                AttachedForm.CogImageFileEdit1.Subject = Nothing
        AttachedForm.CogAcqFifoEditV21.Subject = Nothing
                AttachedForm.CogToolGroupEdit1.Subject = Nothing
                AttachedForm.CogCalibNPointToNPointEdit1.Subject = Nothing
                AttachedForm.CogPMAlignEdit1.Subject = Nothing
                AttachedForm.CogFixtureEdit1.Subject = Nothing
                AttachedForm.CogCaliperEdit1.Subject = Nothing
                AttachedForm.CogBlobEdit1.Subject = Nothing
            End If
        End Set
    End Property

    <CLSCompliant(False)> _
    Public Property ResultsDisplay() As CogDisplay
        Get
            If Display_ Is Nothing Then
                MsgBox("No Display Attached")
                Throw New Exception("No Display Attached")
            End If
            Return Display_
        End Get
        Set(ByVal Value As CogDisplay)
            Display_ = Value
        End Set
    End Property

    Public ReadOnly Property CalibrationImage() As CogImage8Grey
        Get
            Return CalibrateToMillimetersTool.CalibrationImage
        End Get
    End Property

    Public Property AttachedForm() As CalibrationAndFixturingForm
        Get
            If Form_ Is Nothing Then
                MsgBox("No Form Attached")
                Throw New Exception("No Form Attached")
            End If
            Return Form_
        End Get
        Set(ByVal Value As CalibrationAndFixturingForm)
            Form_ = Value
            RaiseEvent CalibrationChanged(CalibrateToMillimetersSubobject.Calibrated)
            RaiseEvent ResultsChanged()
        End Set
    End Property

    Private Sub Draw()
        ' Trash any existing graphics.
        ResultsDisplay.InteractiveGraphics.Clear()

        ' Do nothing if graphics are disabled.
        If Not ToolGraphicsVisible Then
            Exit Sub
        End If

        ' Temporarily disable graphics.
        ResultsDisplay.DrawingEnabled = False

        ' PMAlign graphics.
        If AnalysisLevel >= ImageAnalysisLevelConsts.eImageAnalysisLevelPMAlign Then
            If Not PMAlignInputToFixtureTool.Results Is Nothing Then
                If PMAlignInputToFixtureTool.Results.Count = 1 Then
                    PMAlignInputToFixtureToolGraphic = _
                      PMAlignInputToFixtureTool.Results(0).CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchRegion)
                    ResultsDisplay.InteractiveGraphics.Add( _
                      PMAlignInputToFixtureToolGraphic, "main", False)
                End If
            End If
        End If

        ' Fixture graphics.  This code manages a graphic object rather than
        ' relying on the tool to compose its graphics.
        If AnalysisLevel >= ImageAnalysisLevelConsts.eImageAnalysisLevelFixture Then
            NormalizationFixtureToolGraphic.Transform = _
              NormalizationFixtureTool.RunParams.UnfixturedFromFixturedTransform
            NormalizationFixtureToolGraphic.Color = _
              IIf(NormalizationFixtureTool.RunStatus.Result = CogToolResultConstants.Accept, _
                  CogColorConstants.Green, CogColorConstants.Red)
            ResultsDisplay.InteractiveGraphics.Add( _
              NormalizationFixtureToolGraphic, "main", False)
        End If

        ' Caliper graphics.
        If AnalysisLevel >= ImageAnalysisLevelConsts.eImageAnalysisLevelCaliper Then
            If Not TabWidthCaliperTool.Results Is Nothing Then
                If TabWidthCaliperTool.Results.Count >= 1 Then
                    TabWidthCaliperToolGraphic = _
                      TabWidthCaliperTool.Results(0). _
                      CreateResultGraphics(CogCaliperResultGraphicConstants.Edges)
                    ResultsDisplay.InteractiveGraphics.Add( _
                      TabWidthCaliperToolGraphic, "main", False)
                End If
            End If
        End If

        ' Blob graphics.
        If AnalysisLevel >= ImageAnalysisLevelConsts.eImageAnalysisLevelBlob Then
            If Not LargeHoleBlobsTool.Results Is Nothing Then
                If LargeHoleBlobsTool.Results.GetBlobs.Count >= 1 Then
                    LargeHoleBlob1ToolGraphic = _
                      LargeHoleBlobsTool.Results.GetBlobs.Item(0).CreateResultGraphics(CogBlobResultGraphicConstants.Boundary)
                    ResultsDisplay.InteractiveGraphics.Add( _
                      LargeHoleBlob1ToolGraphic, "main", False)
                End If
                If LargeHoleBlobsTool.Results.GetBlobs.Count >= 2 Then
                    LargeHoleBlob2ToolGraphic = _
                      LargeHoleBlobsTool.Results.GetBlobs.Item(1).CreateResultGraphics(CogBlobResultGraphicConstants.Boundary)
                    ResultsDisplay.InteractiveGraphics.Add( _
                      LargeHoleBlob2ToolGraphic, "main", False)
                End If
            End If
        End If

        ' Render graphics.
        ResultsDisplay.DrawingEnabled = True
    End Sub

    ' Extract the next image from our image source.
    Public Sub GetImage()
        Dim image As ICogImage

        If ImageSource = ImageSourceConsts.eImageFileImageSource Then
            InspectionImageFileTool.Run()
            image = InspectionImageFileTool.OutputImage
        Else
            InspectionImageAcqFifoTool.Run()
            image = InspectionImageAcqFifoTool.OutputImage
        End If

        ResultsDisplay.InteractiveGraphics.Clear()
        ResultsDisplay.Image = image
    End Sub

    Public ReadOnly Property Time() As Double
        Get
            Return analysisTimer.Milliseconds
        End Get
    End Property

    Public ReadOnly Property TabWidthResults() As CogCaliperResults
        Get
            Return TabWidthCaliperTool.Results
        End Get
    End Property

    Public ReadOnly Property LargeHoleResults() As CogBlobResults
        Get
            Return LargeHoleBlobsTool.Results
        End Get
    End Property
End Class




