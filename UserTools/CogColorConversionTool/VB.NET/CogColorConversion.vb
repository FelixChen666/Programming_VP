' *******************************************************************************
'Copyright (C) 2008 Cognex Corporation

'Subject to Cognex Corporations terms and conditions and license agreement,
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

'This sample demonstrates how a user can create their own Vision Tools using 
'base classes the Cognex provides.  The tool created in this class converts 3-plane
'RGB images to individual R, G, and B planes, or converts a 3-plane RGB image to 
'individual H, S, and I planes.

'The tool created is called the CogColorConversionTool.  It makes use of an operator, which
'does all the real work, called CogColorConversion.

'* 

'  In this sample, we are adding this new tool to the Cognex.VisionPro.ImageProcessing namespace
'  You can add your tool to any Cognex.VisionPro namespace, or create your own namespace.
Option Explicit On
Imports System
Imports System.ComponentModel      ' needed for Editor attribute
Imports System.Windows.Forms        'needed for Editor attribute
Imports Cognex.VisionPro
Imports Cognex.VisionPro.Implementation
Imports Cognex.VisionPro.ImageProcessing
Imports System.Runtime.Serialization
Imports Cognex.VisionPro.Exceptions

Namespace CogColorConversionTool
    ' <class summary>
    ' CogColorConversionResults:
    ' The results provided by the CogColorConversion operator.  The results consist of 
    ' three planes, representing R, G, and B or H, S, and I.  We mark the class with the
    ' .NET Serializable attribute so it can use persistence.  It is derived from ICloneable so
    ' that we can use the Clone() method to make a copy of this object.  Results should also
    ' derive from CogSerializableObject base so they can be saved/restored properly.
    ' </summary>

    <Serializable()> _
    Public Class CogColorConversionResults
        Inherits CogSerializableObjectBase
        Implements ICloneable
#Region " Constructors"
        Public Sub New()

        End Sub

        Public Sub New(ByVal other As CogColorConversionResults)

            _plane0 = other.Plane0
            _plane1 = other.Plane1
            _plane2 = other.Plane2
        End Sub
        Public Sub New(ByVal plane0 As CogImage8Grey, _
         ByVal plane1 As CogImage8Grey, _
         ByVal plane2 As CogImage8Grey)
            _plane0 = plane0
            _plane1 = plane1
            _plane2 = plane2
        End Sub
        ' Serialization Constructor - necessary for save/restore.
        Private Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.new(info, context)
        End Sub
#End Region
#Region " Public properties"
        Public ReadOnly Property Plane0() As CogImage8Grey
            Get
                Return _plane0
            End Get
        End Property
        Public ReadOnly Property Plane1() As CogImage8Grey
            Get
                Return _plane1
            End Get
        End Property
        Public ReadOnly Property Plane2() As CogImage8Grey
            Get
                Return _plane2
            End Get
        End Property
#End Region
#Region " Private Fields"
        Private _plane0 As CogImage8Grey
        Private _plane1 As CogImage8Grey
        Private _plane2 As CogImage8Grey
#End Region
#Region " ICloneable Implementation"
        ' returns a new copy of the results object
        Public Function Clone() As Object Implements System.ICloneable.Clone
            Return New CogColorConversionResults
        End Function
#End Region
    End Class
    ' <class summary>
    ' CogColorConversion:
    ' An operator used by the CogColorConversionTool to generate
    ' individual color planes from an RGB Image.  It is derived from CogSerializableChangedEventBase
    ' so that it will generate changed events and so that it can be serialized.  It is derived from ICloneable so
    ' that we can use the Clone() method to make a copy of this object. We mark the class with the
    ' .NET Serializable attribute so it can use persistence.  
    ' 
    ' When an object derives from CogSerializableChangedEventBase, it is able to generate changed events.
    ' When a changed event is generated, it is the responsiblity of the object to indicate
    ' which element of the class has changed.  To do this, StateFlags are used.  StateFlags
    ' are a bitfield where each bit represents a particular element that has changed.
    ' In this object, the only thing that can change is the _convertToHSI private field.
    ' When this element changes, a changed event is issued with SfConvertToHSI state flag
    ' asserted.   
    ' </summary>
    <Serializable()> _
    Public Class CogColorConversion
        Inherits CogSerializableChangedEventBase
        Implements ICloneable
#Region " Constractors"
        Public Sub New()
            _convertToHSI = False
        End Sub
        Public Sub New(ByVal other As CogColorConversion)
            _convertToHSI = other._convertToHSI
        End Sub
        ' Serialization Constructor - necessary for save/restore.
        Private Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.new(info, context)
        End Sub
#End Region
#Region "Private Fields"
        Private _convertToHSI As Boolean
#End Region
#Region "State Flags"
        ' <summary>
        ' Note that one always uses the next StateFlag from the base class
        ' as the first StateFlag for the new class.  One should always assign SfNextSf as the
        ' next unassigned StateFlag
        ' so that derived classes can determine where they should begin assigning 
        ' their StateFlags.
        ' </summary>

        Private Const Sf0 As Long = CogSerializableChangedEventBase.SfNextSf
        Public Const SfConvertToHSI As Long = Sf0 << 0
        Protected Shadows Const SfNextSf As Long = Sf0 << 1
#End Region
#Region "Public properties"
        ' <summary>
        ' These properties get/set the _convertToHSI value. When _convertToHSI value is 
        ' changed, a changed event is fired (OnChanged) with the appropriate StateFlag
        ' (SfConvertToHSI).
        ' </summary>
        Public Property ConvertToHSI() As Boolean
            Get
                Return _convertToHSI
            End Get
            Set(ByVal Value As Boolean)
                _convertToHSI = Value
                OnChanged(SfConvertToHSI)
            End Set
        End Property

#End Region
#Region "Public Functions"
        ' <summary>
        ' Execute
        ' The execute method converts the 3-plane RGB input image to the planar
        ' output image in the appropriate color format.
        ' Note: The Execute method fails to handle conversion of the region to 
        ' pixel space.  It assumes that the Region is already in Pixel space.
        ' </summary>
        ' <param name="InputImage"></param>
        ' <param name="Region"></param>
        ' <returns></returns>
        Public Function Execute(ByVal InputImage As CogImage24PlanarColor, _
        ByVal Region As Cognex.VisionPro.ICogRegion) As CogColorConversionResults
            Dim runImage As CogImage24PlanarColor
            Dim myCopyRegion As CogCopyRegion = New CogCopyRegion
            Dim plane0, plane1, plane2 As CogImage8Grey
            Dim regionAsShape As Cognex.VisionPro.ICogShape = Region
            Dim AOI As CogRectangle
            Dim X As Double = 0
            Dim y As Double = 0
            Dim Width As Double = 0
            Dim Height As Double = 0
            Dim sourceClipped, destClipped As Boolean
            Dim outRegion As Cognex.VisionPro.ICogRegion
            outRegion = Nothing

            If (Not Region Is Nothing) Then
                AOI = regionAsShape.EnclosingRectangle(CogCopyShapeConstants.GeometryOnly)
                AOI.GetXYWidthHeight(X, y, Width, Height)
            Else
                AOI = Nothing
            End If
            Try
                If _convertToHSI Then

                    If Not AOI Is Nothing Then
                        runImage = CType(CogImageConvert.GetHSIImage( _
                                 InputImage, _
                                 System.Convert.ToInt32(X), _
                                 System.Convert.ToInt32(y), _
                                 System.Convert.ToInt32(Width), _
                                 System.Convert.ToInt32(Height)), CogImage24PlanarColor)
                    Else
                        runImage = CType(CogImageConvert.GetHSIImage( _
                         InputImage, 0, 0, _
                         System.Convert.ToInt32(InputImage.Width), _
                         System.Convert.ToInt32(InputImage.Height)), CogImage24PlanarColor)
                    End If

                    Return New CogColorConversionResults(runImage.GetPlane(CogImagePlaneConstants.Red), _
                                 runImage.GetPlane(CogImagePlaneConstants.Green), _
                                runImage.GetPlane(CogImagePlaneConstants.Blue))
                Else

                    If Not AOI Is Nothing Then

                        myCopyRegion.RegionMode = CogRegionModeConstants.PixelAlignedBoundingBox
                        myCopyRegion.FillBoundingBox = False
                        plane0 = CType(myCopyRegion.Execute( _
                                      InputImage.GetPlane(CogImagePlaneConstants.Red), AOI, Nothing, sourceClipped, _
                                      destClipped, outRegion), CogImage8Grey)
                        plane1 = CType(myCopyRegion.Execute( _
                                    InputImage.GetPlane(CogImagePlaneConstants.Green), AOI, Nothing, sourceClipped, _
                                    destClipped, outRegion), CogImage8Grey)
                        plane2 = CType(myCopyRegion.Execute( _
                                    InputImage.GetPlane(CogImagePlaneConstants.Blue), AOI, Nothing, sourceClipped, _
                                    destClipped, outRegion), CogImage8Grey)

                    Else

                        plane0 = InputImage.GetPlane(CogImagePlaneConstants.Red).Copy(CogImageCopyModeConstants.CopyPixels)
                        plane1 = InputImage.GetPlane(CogImagePlaneConstants.Green).Copy(CogImageCopyModeConstants.CopyPixels)
                        plane2 = InputImage.GetPlane(CogImagePlaneConstants.Blue).Copy(CogImageCopyModeConstants.CopyPixels)
                    End If

                    Return New CogColorConversionResults(plane0, plane1, plane2)
                End If
            Catch ex As CogException
                Throw ex
            Catch ex As Exception
                Throw New CogColorConversionToolException("CogColorConversion Execution failure")
            End Try
        End Function
#End Region
#Region "Enumerations"
        '  <summary>
        ' These enumerations allow the Tool user to select what data they want to appear in 
        ' the CurrentRunRecord and the LastRunRecord.  The Flags attributes causes these
        ' enumerations to be treated as bitfields.
        ' </summary>

        <Flags()> _
        Public Enum CogColorConversionLastRunRecordConstants
            None = &H0
            OutputImages = &H1
            All = OutputImages
        End Enum

        <Flags()> _
        Public Enum CogColorConversionCurrentRunRecordConstants
            None = &H0
            InputImage = &H1
            Region = &H2
            All = InputImage Or Region
        End Enum
#End Region
#Region "ICloneable Implementation"
        ' returns a new copy of the CogColorConversion object
        Public Function Clone() As Object Implements System.ICloneable.Clone
            Return New CogColorConversion
        End Function
#End Region
    End Class
    ' <summary>
    ' CogColorConversionTool
    ' This class represents the actual tool that converts images.  To associate this tool with
    ' a tool edit control, we use the Editor attribute as shown below.  We mark the class with the
    ' Serializable attribute so it can use persistence.  
    ' The Tool derives from CogToolBase, which provides a lot of implicit functionality.  It
    ' is the user's responsibilty to override the following methods from CogToolBase in order
    ' to supply user specific functionality:
    ' InternalRun(): This method is executed when "Run" is called.  Fill this in with whatever
    ' your tool should be doing when it is run.
    ' InternalCreateRunRecord(): This method creates the CurrentRunRecord for the Tool.
    ' The CurrentRunRecord contains a set of CogRecords that appear in the Tool's Edit Control
    ' when Current.InputImage is selected in the display.  In this case, the input image and
    ' the input region are placed in the CurrentRunRecord.
    ' InternalCreateLastRunRecord(): This method creates the LastRunRecord for the Tool.  The
    ' LastRunRecord() contains a set of CogRecords that are used to show results in the
    ' Tool Edit Control's display.  In this case, the three output planes are placed in the
    ' LastRunRecord.
    ' </summary>
    <Serializable(), Editor(GetType(CogColorConversionToolEditor), GetType(Control)), CogDefaultToolInputTerminal(0, "InputImage", "InputImage"), CogDefaultToolOutputTerminal(0, "OutputImage0", "OutputImage0"), CogDefaultToolOutputTerminal(1, "OutputImage1", "OutputImage1"), CogDefaultToolOutputTerminal(2, "OutputImage2", "OutputImage2")> _
  Public Class CogColorConversionTool
        Inherits CogToolBase

#Region "Private Fields"
        Private _operator As CogColorConversion = New CogColorConversion
        Private _OutputImage0 As CogImage8Grey
        Private _OutputImage1 As CogImage8Grey
        Private _OutputImage2 As CogImage8Grey
        Private _InputImage As CogImage24PlanarColor
        Private _Region As Cognex.VisionPro.ICogRegion
        Private _HSI As Boolean

#End Region
#Region "State flags"
        ' <summary>
        ' Note that one always uses the next StateFlag from the base class
        ' as the first StateFlag for the new class.  One should always assign SfNextSf as the
        ' next unassigned StateFlag
        ' so that derived classes can determine where they should begin assigning 
        ' their StateFlags.  In this sample, we are assigning a unique StateFlag to each
        ' private field for this class.  We are also assigning StateFlags to the Current
        ' and LastRun record enable variables.
        ' </summary>

        Private Const Sf0 As Long = CogToolBase.SfNextSf
        Public Const SfInputImage As Long = Sf0 << 0
        Public Const SfHSI As Long = Sf0 << 1
        Public Const SfRegion As Long = Sf0 << 2
        Public Const SfOutputImage0 As Long = Sf0 << 3
        Public Const SfOutputImage1 As Long = Sf0 << 4
        Public Const SfOutputImage2 As Long = Sf0 << 5
        Public Const SfCurrentRunRecordEnable As Long = Sf0 << 6
        Public Const SfLastRunRecordEnable As Long = Sf0 << 7
        Protected Shadows Const SfNextSf As Long = Sf0 << 8
#End Region
#Region "Public Properties"
        'Note that we fire changed events (OnChanged) whenever a property changes.
        Public Property LastRunRecordEnable() As CogColorConversion.CogColorConversionLastRunRecordConstants
            Get
                Return CType(LastRunRecordEnable_, CogColorConversion.CogColorConversionLastRunRecordConstants)
            End Get
            Set(ByVal Value As CogColorConversion.CogColorConversionLastRunRecordConstants)
                LastRunRecordEnable_ = Value
                OnChanged(SfLastRunRecordEnable)
            End Set
        End Property
        Public Property CurrentRunRecordEnable() As CogColorConversion.CogColorConversionCurrentRunRecordConstants
            Get
                Return CType(CurrentRecordEnable_, CogColorConversion.CogColorConversionCurrentRunRecordConstants)
            End Get
            Set(ByVal Value As CogColorConversion.CogColorConversionCurrentRunRecordConstants)
                CurrentRecordEnable_ = Value
                OnChanged(SfCurrentRunRecordEnable)
            End Set
        End Property
        ' <summary>
        ' When the InputImage changes, we need to fire two changed events.  One changed
        ' event indicates that the InputImage has changed.  The other (SfCreateCurrentRecord)
        ' is used to update the CurrentRecord, of which the InputImage is part.
        ' </summary>	
        Public Property InputImage() As CogImage24PlanarColor
            Get
                Return _InputImage
            End Get
            Set(ByVal Value As CogImage24PlanarColor)
                If ((_InputImage Is Nothing) OrElse (Not _InputImage.Equals(Value))) Then        ' simplest test - ref equivalence.

                    _InputImage = Value
                    OnChanged(SfInputImage Or SfCreateCurrentRecord)
                End If

            End Set
        End Property
        Public ReadOnly Property OutputImage0() As CogImage8Grey
            Get
                Return _OutputImage0
            End Get
        End Property
        Public ReadOnly Property OutputImage1() As CogImage8Grey
            Get
                Return _OutputImage1
            End Get
        End Property
        Public ReadOnly Property OutputImage2() As CogImage8Grey
            Get
                Return _OutputImage2
            End Get
        End Property
        Public Property HSI() As Boolean
            Get
                Return _HSI
            End Get
            Set(ByVal Value As Boolean)
                If _HSI <> Value Then
                    _HSI = Value
                    OnChanged(SfHSI)
                End If
            End Set
        End Property
        Public Property Region() As Cognex.VisionPro.ICogRegion
            Get
                Return _Region
            End Get
            Set(ByVal Value As Cognex.VisionPro.ICogRegion)
                _Region = Value
                OnChanged(SfRegion Or SfCreateCurrentRecord)
            End Set
        End Property
#End Region
#Region "Constructors"
        ' <summary>
        ' The CurrentRecordEnable_ and LastRunRecordEnable_ member variables are provided
        ' by the CogToolBase class. Set these members to indicate which records should appear
        ' in the CurrentRunRecord and the LastRunRecord.  In this case, we are defaulting
        ' to indicate that all produced data should appear.
        ' </summary>
        Public Sub New()

            CurrentRecordEnable_ = CType(CogColorConversion.CogColorConversionCurrentRunRecordConstants.All, Integer)
            LastRunRecordEnable_ = CType(CogColorConversion.CogColorConversionLastRunRecordConstants.All, Integer)
            _HSI = False
            _operator.ConvertToHSI = _HSI
        End Sub

        Public Sub New(ByVal other As CogColorConversionTool)

            If Not other._OutputImage0 Is Nothing Then
                _OutputImage0 = CType(other.Clone, CogColorConversionTool)._OutputImage0
            End If
            If Not other._OutputImage1 Is Nothing Then
                _OutputImage1 = CType(other.Clone, CogColorConversionTool)._OutputImage1
            End If
            If Not other._OutputImage2 Is Nothing Then
                _OutputImage2 = CType(other.Clone, CogColorConversionTool)._OutputImage2
            End If
            If Not other._Region Is Nothing Then
                _Region = CType(other.Clone, CogColorConversionTool)._Region
            End If
            _HSI = other._HSI
            _operator.ConvertToHSI = _HSI
        End Sub
        ' Serialization Constructor - necessary for save/restore.
        Private Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.new(info, context)
            _operator.ConvertToHSI = _HSI
        End Sub

#End Region
#Region "Protected overides"
        ' returns a new copy of the CogColorConversionTool object
        Protected Overrides Function Clone() As Object
            Return New CogColorConversionTool
        End Function

        ' <summary>
        ' InternalCreateRunRecord
        ' This method creates the CurrentRunRecord.  Run records consist of
        ' one or more CogRecords.  If a CogRecord contains an ICogImage, it will be shown
        ' on the display of the Tool's edit control.  It the ICogImage record contains a
        ' graphics subrecord, the graphics will appear on the ICogImage.  For this tool,
        ' the CurrentRunRecord can include the InputImage and the Region.  Note that 
        ' these records are conditionally added based on the values stored in 
        ' CurrentRunRecordEnable.
        ' </summary>
        ' <param name="newRecord"></param>
        ' <param name="mycurrentRecordEnable"></param>
        Protected Overrides Sub InternalCreateCurrentRecord(ByVal newRecord As Cognex.VisionPro.ICogRecord, ByVal currentRecordEnable As Integer)
            Dim InputImageRecord As CogRecord
            If (CurrentRunRecordEnable And CogColorConversion.CogColorConversionCurrentRunRecordConstants.InputImage) > 0 Then

                InputImageRecord = New CogRecord("InputImage", _
                                                  GetType(CogImage24PlanarColor), _
                                                 CogRecordUsageConstants.Input, False, _InputImage, "InputImage")
                newRecord.SubRecords.Add(InputImageRecord)
                If (CurrentRunRecordEnable And CogColorConversion.CogColorConversionCurrentRunRecordConstants.Region) > 0 Then
                    InputImageRecord.SubRecords.Add(New CogRecord("Region", _
                                                     GetType(Cognex.VisionPro.ICogGraphicInteractive), _
                     CogRecordUsageConstants.Configuration, False, _Region, "Region"))

                End If
            End If




        End Sub
        ' <summary>
        ' InternalCreateLastRunRecord
        ' This method creates the LastRunRecord.  Run records consist of
        ' one or more CogRecords.  If a CogRecord contains an ICogImage, it will be shown
        ' on the display of the Tool's edit control.  It the ICogImage record contains a
        ' graphics subrecord, the graphics will appear on the ICogImage. For this tool,
        ' the LastRunRecord can include the OutputImages (i.e. individual planes).  Note that 
        ' this record is conditionally added based on the values stored in 
        ' LastRunRecordEnable.
        ' </summary>
        ' <param name="newRecord"></param>
        ' <param name="lastRunRecordEnable"></param>
        ' <param name="lastRunRecordDiagEnable"></param>

        Protected Overrides Sub InternalCreateLastRunRecord(ByVal newRecord As Cognex.VisionPro.ICogRecord, _
                                ByVal lastRunRecordEnable As Integer, ByVal lastRunRecordDiagEnable As Integer)
            If (lastRunRecordEnable And CogColorConversion.CogColorConversionLastRunRecordConstants.OutputImages) > 0 Then
                newRecord.SubRecords.Add(New CogRecord("OutputImage0", GetType(CogImage8Grey), _
                 CogRecordUsageConstants.Result, False, _OutputImage0, "OutputImage0"))
                newRecord.SubRecords.Add(New CogRecord("OutputImage1", GetType(CogImage8Grey), _
          CogRecordUsageConstants.Result, False, _OutputImage1, "OutputImage1"))
                newRecord.SubRecords.Add(New CogRecord("OutputImage2", GetType(CogImage8Grey), _
          CogRecordUsageConstants.Result, False, _OutputImage2, "OutputImage2"))
            End If

        End Sub

        ' <summary>
        ' InternalRun
        ' This method is the "Run" method for the tool.  It calls the Execute() method
        ' on the CogColorConversion operator.  It generates changed events for OutputImages
        ' and CreateLastRunRecord.  
        ' </summary>
        ' <param name="message"></param>
        ' <returns></returns>
        Protected Overrides Function InternalRun(ByRef message As String) As CogToolResultConstants
            Dim opResults As CogColorConversionResults
            _operator.ConvertToHSI = _HSI
            If _InputImage Is Nothing Then

                Throw New CogOperatorNoInputImageException
            End If
            opResults = _operator.Execute(_InputImage, _Region)
            _OutputImage0 = opResults.Plane0
            _OutputImage1 = opResults.Plane1
            _OutputImage2 = opResults.Plane2
            OnChanged(SfOutputImage0 Or SfOutputImage1 Or _
                      SfOutputImage2 Or SfCreateLastRunRecord)
            Return CogToolResultConstants.Accept

        End Function


#End Region
    End Class

    Public Class CogColorConversionToolException
        Inherits CogException
        Public msg As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal srcMsg As String)
            msg = srcMsg
        End Sub

        ' Serialization Constructor - necessary for save/restore.
        Private Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.new(info, context)
        End Sub

        Public Overrides ReadOnly Property Message() As String
            Get
                Return msg
            End Get
        End Property
    End Class

End Namespace

