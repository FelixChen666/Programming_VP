'*******************************************************************************
'Copyright (C) 2008 Cognex Corporation

'Subject to Cognex Corporations terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.
'*******************************************************************************/
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This file contains the source code for SimpleTool.
' The user must do the following things to create a Tool:
' 1. Create a new Class Library project.
' 2. Include project references to:
'    - Cognex.VisionPro
'    - Cognex.VisionPro.Core
' 3. Create your Tool class.  Derive it from CogToolBase.
' 4. Define Tool-Specific private fields. Examples:
'    - Input Image
'    - Output Image
'    - Region
' 5. Define Constructors and Clone method.
' 6. Make the tool serializable.
' 7. Define a 64-bit state flag constant for each property or method that
'    returns changeable data.
' 8. Create properties for accessing tool fields.
'    - Each "setter" must call OnChanged(appropriate_state_flags).
' 9. Create other methods, as needed.
' 10. Override the following CogToolBase methods:
'    - InternalRun() - Executed when tool is run programmatically, or from
'      the edit control.
'    - InternalCreateCurrentRecord() - Creates a "run record"
'      containing images and graphcs to be displayed in the edit 
'      control before the tool runs.  Usually includes:
'       * Input Image
'       * Region
'    - InternalCreateLastRunRecord() - Creates a "run record"
'      containing images and graphcs to be displayed in the edit 
'      control after the tool runs.  Usually includes:
'       * Output Image(s)

Option Explicit On

Imports System
Imports System.Runtime.Serialization
Imports System.ComponentModel  ' needed for Editor attribute
Imports System.Windows.Forms   ' needed for Editor attribute
Imports Cognex.VisionPro
Imports Cognex.VisionPro.Implementation

Namespace SimpleTool
    ' SimpleTool is the most basic example of a user-built tool.  When run,
    ' it copies the input image to the output image.  If CopyTwice is true,
    ' it copies the image twice.
    <Serializable(), _
     Editor(GetType(SimpleToolEditV2), GetType(Control)), _
     CogDefaultToolInputTerminal(0, "InputImage", "InputImage"), _
     CogDefaultToolOutputTerminal(0, "OutputImage", "OutputImage"), _
     DesignerCategory("")> _
    Public Class SimpleTool
        Inherits CogToolBase

#Region " Private Fields"
        ' This attribute identifies the following field as an input image.
        ' (Users can choose not to serialize VisionPro input images.)
        <CogSerializationOptionsAttribute(CogSerializationOptionsConstants.InputImages)> _
        Private _InputImage As CogImage8Grey

        ' This attribute identifies the following field as an output image.
        ' (Users can choose not to serialize VisionPro output images.)
        <CogSerializationOptionsAttribute(CogSerializationOptionsConstants.OutputImages)> _
        Private _OutputImage As CogImage8Grey

        ' CopyTwice will be modifiable in the edit control.
        Private _CopyTwice As Boolean   ' defaults to false
#End Region

#Region " Constructors and Clone"
        ' Construct this tool in a default state.
        Sub New()
        End Sub

        ' Construct this tool as a deep copy of the given one.
        Sub New(ByVal otherTool As SimpleTool)
            MyBase.New(otherTool)
            ' Make a deep copy of the input image
            If (Not otherTool._InputImage Is Nothing) Then
                _InputImage = otherTool._InputImage.Copy()
            End If

            ' Make a deep copy of the output image, but only if it's not
            ' already the same object as the input image.
            If (Not otherTool._OutputImage Is Nothing) Then
                If (Object.ReferenceEquals(otherTool._InputImage, _
                                           otherTool._OutputImage)) Then
                    _OutputImage = _InputImage  ' Same object
                Else  ' Deep copy
                    _OutputImage = otherTool._OutputImage.Copy()
                End If
            End If

            _CopyTwice = otherTool._CopyTwice
        End Sub

        ' Construct this tool from the given serialization info.
        ' The call to the base class will initialize all tool fields.
        Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.new(info, context)
        End Sub

        ' Return a deep copy of this object via ICloneable.Clone()
        Protected Overrides Function Clone() As Object
            Return New SimpleTool(Me)
        End Function
#End Region

#Region " State Flags"
        ' Define a 64-bit state flag constant for each property or method
        ' that returns changeable data.  The name of the constant must be
        ' "Sf" followed by the name of the property or method.  Make sure
        ' that the bit constants defined here do not overlap with the ones
        ' from our base class: The first state flag should be set to
        ' "CogToolBase.SfNextSf".
        Private Const Sf0 As Long = CogToolBase.SfNextSf
        Public Const SfInputImage As Long = Sf0 << 0
        Public Const SfOutputImage As Long = Sf0 << 1
        Public Const SfCopyTwice As Long = Sf0 << 2
        Protected Shadows Const SfNextSf As Long = Sf0 << 3  ' for derived classes
#End Region

#Region " Public Properties"
        Public Property InputImage() As CogImage8Grey
            Get
                Return _InputImage
            End Get
            Set(ByVal Value As CogImage8Grey)
                If (Not Object.ReferenceEquals(_InputImage, Value)) Then
                    _InputImage = Value
                    ' The state flags in the Changed event below indicate that the
                    ' current run record has changed (along with the input image).
                    ' This is done because the input image is contained inside
                    ' the current run record.
                    ' Note that SfCreateCurrentRecord is provided by CogToolBase.
                    OnChanged(SfInputImage Or SfCreateCurrentRecord)
                End If
            End Set
        End Property

        Public ReadOnly Property OutputImage() As CogImage8Grey
            ' This property has no setter since it is an output of the tool.
            Get
                Return _OutputImage
            End Get
        End Property

        Public Property CopyTwice() As Boolean
            Get
                Return _CopyTwice
            End Get
            Set(ByVal Value As Boolean)
                If _CopyTwice = (Not Value) Then
                    _CopyTwice = Value
                    OnChanged(SfCopyTwice) ' Fire the CopyTwice Changed event
                End If
            End Set
        End Property
#End Region

#Region " CogToolBase overrides"
        ' InternalRun:
        ' This is the "Run" method for the SimpleTool.  It overwrites the 
        ' existing OutputImage with a deep copy of the InputImage.
        ' The "message" argument is a string that will be copied into the message
        ' property of the tool's RunStatus object (if no exceptions are thrown).
        ' This method returns an enumeration value describing the tool result.
        Protected Overrides Function InternalRun(ByRef message As String) _
                                                        As CogToolResultConstants
            ' It's an error to run the tool without an input image...
            If (_InputImage Is Nothing) Then
                Throw New Cognex.VisionPro.Exceptions.CogOperatorNoInputImageException
            End If

            ' Make a deep copy of the image.  Doing it twice isn't really 
            ' useful except to illustrate how tool properties can be used
            ' to alter tool execution (and waste CPU cycles).
            _OutputImage = _InputImage.Copy()
            If (_CopyTwice) Then
                _OutputImage = _InputImage.Copy()
                message = "Copied twice!"
            End If

            ' Fire a Changed event indicating that the output image -- and
            ' the LastRunRecord containing that image -- have changed.
            OnChanged(SfOutputImage Or SfCreateLastRunRecord)

            ' If the code reaches here, the tool has run successfully.
            Return CogToolResultConstants.Accept
        End Function

        ' The two methods below create "run records" for the SimpleTool.
        ' The "Current" run record contains images and graphics that represent
        ' inputs to the tool.  The "Last" run record holds images and graphics
        ' that were computed during the last run of the tool.
        '
        ' A run record is a hierarchical structure whose top-level node 
        ' contains a collection of image records.  Each image record can
        ' be selected in the dropdown list of the tool's edit control.
        ' The control will display the selected image record plus all 
        ' graphics contained in sub-records of that image record.
        '
        ' Each method below adds an appropriate image record to the (newly 
        ' created) top-level record called "newRecord".  Usually each routine
        ' would use the provided bit flags to decide what images and graphics
        ' to add to the top-level record, but for the sake of simplicity we
        ' ignore the bit flags (and add no graphics).


        ' InternalCreateCurrentRecord: Creates the CurrentRecord.  
        ' For this tool, the CurrentRecord includes the InputImage.
        ' The "newRecord" argument is a newly-created record to which
        ' image sub-records may be added.
        ' The "currentRecordEnable" argument is an integer bit pattern
        ' indicating what images and graphics to add.
        Protected Overrides Sub InternalCreateCurrentRecord( _
                                ByVal newRecord As Cognex.VisionPro.ICogRecord, _
                                ByVal currentRecordEnable As Integer)
            ' Add a new image record to the top-level "newRecord".
            newRecord.SubRecords.Add( _
              New CogRecord("InputImage", _
                            GetType(CogImage8Grey), _
                            CogRecordUsageConstants.Input, _
                            False, _InputImage, "InputImage"))
        End Sub

        ' InternalCreateLastRunRecord: Creates the LastRunRecord.
        ' For this tool, the LastRunRecord includes the OutputImage.
        ' The "newRecord" argument is a newly-created record to which
        ' image sub-records may be added.
        ' The "lastRunRecordEnable" argument is an integer bit pattern
        ' indicating what images and graphics to add.
        ' The "lastRunRecordDiagEnable" argument is an integer bit pattern
        ' indicating what 'Diag' images and graphics to add.  'Diag' images
        ' and graphics are specified using a separate set of bits because 
        ' they make the tool run significantly slower (or use significantly
        ' more memory).  Users must re-run the tool to see these items.
        Protected Overrides Sub InternalCreateLastRunRecord( _
                                ByVal newRecord As Cognex.VisionPro.ICogRecord, _
                                ByVal lastRunRecordEnable As Integer, _
                                ByVal lastRunRecordDiagEnable As Integer)
            ' Add a new image record to the top-level "newRecord".
            newRecord.SubRecords.Add( _
              New CogRecord("OutputImage", _
                            GetType(CogImage8Grey), _
                            CogRecordUsageConstants.Result, _
                            False, _OutputImage, "OutputImage"))
        End Sub
#End Region

    End Class
End Namespace