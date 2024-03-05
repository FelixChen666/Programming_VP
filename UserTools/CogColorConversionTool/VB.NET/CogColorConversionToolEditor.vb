'******************************************************************************
'Copyright (C) 2008 Cognex Corporation
'
'Subject to Cognex Corporations terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.
'*****************************************************************************/
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.


Option Explicit On

Imports System
Imports System.ComponentModel      ' needed for Editor attribute
Imports System.Windows.Forms        'needed for Editor attribute
Imports Cognex.VisionPro            ' needed for VisionPro
Imports Cognex.VisionPro.ImageProcessing ' needed for image processing
Imports Cognex.VisionPro.Exceptions     ' needed for VisionPro Exceptions


' This file contains the source code for CogColorConversionToolEditor.
' The user should do the following things to create a Edit Control:
' 1. Create a new Inherited User Control.
'    Choose "Cognex.VisionPro.Controls.dll" and "CogToolEditControlBaseV2".
' 2. Drag and drop required controls to the SimpleToolEditV2.Designer.
' 3. Create a helper function to set the values of associated with
'      these subcontrols. See SetupSettingsTab() below.
' 4. Create a constructor.
' 5. Create a Subject getter and setter.
' 6. Create an override of InitializeFromSubject().
' 7. Create an override of SubjectValuesChanged().
' 8. Create an override of SubjectInUseChanged().
' 9. Add code to react to user inputs.


Namespace CogColorConversionTool

  <System.ComponentModel.ToolboxItem(True)> _
  Public Class CogColorConversionToolEditor


    Public Sub New()
      MyBase.New()

      InitializeComponent()

      ' No electric behavior in this sample ...
      tbbElectric.Visible = False

    End Sub ' Public Sub New()


    <System.ComponentModel.Browsable(False), _
      System.ComponentModel.DesignerSerializationVisibility( _
      System.ComponentModel.DesignerSerializationVisibility.Hidden)> _
    Public Property Subject() As CogColorConversionTool

      Get
        Return MyBase.GetSubject()
      End Get

      Set( _
        ByVal Value As CogColorConversionTool)
        MyBase.SetSubject(Value)
      End Set

    End Property ' Public Property Subject() As CogColorConversionTool


    ' This (override) method is called at the first thread-safe
    ' opportunity after the Subject has been replaced. The control
    ' should use this method to (re)initialize itself using values
    ' from the new subject.
    Protected Overrides Sub InitializeFromSubject()
      MyBase.InitializeFromSubject()

      SetupSettingsTab()
      SetupRegionTab()

    End Sub ' Protected Overrides Sub InitializeFromSubject()


    ' This (override) method is called whenever the Subject raises
    ' a Changed event. Note that this derived class override must
    ' call the base method. This function is always executed on
    ' the GUI thread: InvokeRequired will always return false.
    ' This is a good place to update any subcontrols whose value
    ' may have changed (as indicated by the state flags).
    Protected Overrides Sub SubjectValuesChanged( _
      ByVal sender As Object, _
      ByVal e As Cognex.VisionPro.CogChangedEventArgs)
      MyBase.SubjectValuesChanged(sender, e)

      If e.StateFlags And CogColorConversionTool.SfHSI Then
        SetupSettingsTab()
      End If

      If e.StateFlags And CogColorConversionTool.SfRegion Then
        SetupRegionTab()
      End If

    End Sub ' Protected Overrides Sub SubjectValuesChanged( _


    ' This override provides a notification that the SubjectInUse
    ' property has changed. This is a good place to enable/disable
    ' subcontrols that (a) do not support queuing and (b) are not
    ' implemented via a property provider.
    Protected Overrides Sub SubjectInUseChanged()
      MyBase.SubjectInUseChanged()

      Dim bEnabled As Boolean = False
      If Not Subject Is Nothing Then
        If Not SubjectInUse Then
          bEnabled = True
        End If
      End If
      chkConvertToHSI.Enabled = bEnabled
      cboRegionShape.Enabled = bEnabled

    End Sub ' Protected Overrides Sub SubjectInUseChanged()


    ' SetupSettingsTab() accesses Subject.xxx, and so should only
    ' be called at safe times (e.g. when SubjectInUse is false).
    Private Sub SetupSettingsTab()
      AssertSafe()

      Dim bChecked As Boolean = False
      If Not Subject Is Nothing Then
        If Subject.HSI Then
          bChecked = True
        End If
      End If
      chkConvertToHSI.Checked = bChecked

      Dim bEnabled As Boolean = False
      If Not Subject Is Nothing Then
        bEnabled = True
      End If
      chkConvertToHSI.Enabled = bEnabled

    End Sub ' Private Sub SetupSettingsTab()


    ' SetupRegionTab() accesses Subject.xxx, and so should only
    ' be called at safe times (e.g. when SubjectInUse is false).
    Private Sub SetupRegionTab()
      AssertSafe()

      ' Note that the following simplified approach does
      ' not deal well with the case in which the tool's
      ' Region property is set from outside the control to
      ' a shape other than circle, rectangle, or none.
      Dim iSelectedIndex As Integer = -1
      If Not Subject Is Nothing Then
        If Subject.Region Is Nothing Then
          iSelectedIndex = 2
        Elseif TypeOf Subject.Region Is CogCircle Then
          iSelectedIndex = 0
        Elseif TypeOf Subject.Region Is CogRectangle Then
          iSelectedIndex = 1
        End If
      End If
      cboRegionShape.SelectedIndex = iSelectedIndex

      Dim bEnabled As Boolean = False
      If Not Subject Is Nothing Then
        bEnabled = True
      End If
      cboRegionShape.Enabled = bEnabled

        End Sub ' Private Sub SetupRegionTab()


    ' React to user input on the settings tab. Note that, because
    ' we disable input from this control during unsafe times (when
    ' the tool is running), this should be a safe time to
    ' access the tool.
    Private Sub chkConvertToHSI_CheckedChanged( _
      ByVal sender As Object, _
      ByVal e As EventArgs) Handles chkConvertToHSI.CheckedChanged

      ' It should be safe, but we'll check anyway ...
      If Subject Is Nothing Then Return
      If SubjectInUse Then Return

      Subject.HSI = chkConvertToHSI.Checked

    End Sub


    ' React to user input on the region tab. Note that, because
    ' we disable input from this control during unsafe times (when
    ' the tool is running), this should be a safe time to
    ' access the tool.
    Private Sub RegionComboBox_SelectedIndexChanged( _
      ByVal sender As Object, _
      ByVal e As EventArgs) Handles cboRegionShape.SelectedIndexChanged

      ' It should be safe, but we'll check anyway ...
      If Subject Is Nothing Then Return
      If SubjectInUse Then Return

      Dim tempRegion As Cognex.VisionPro.ICogRegion

      Select Case cboRegionShape.SelectedItem.ToString
        Case "CogCircle"
          tempRegion = New CogCircle
          CType(tempRegion, CogCircle).SetCenterRadius(320, 240, 50)
          CType(tempRegion, CogCircle).Interactive = True
          CType(tempRegion, CogCircle).GraphicDOFEnable = _
            CogCircleDOFConstants.All
          Subject.Region = tempRegion

        Case "CogRectangle"
          tempRegion = New CogRectangle
          CType(tempRegion, CogRectangle).SetCenterWidthHeight(320,240,100,100)
          CType(tempRegion, CogRectangle).Interactive = True
          CType(tempRegion, CogRectangle).GraphicDOFEnable = _
            CogRectangleDOFConstants.Position Or CogRectangleDOFConstants.Size
          Subject.Region = tempRegion

        Case Else
          Subject.Region = Nothing
      End Select

    End Sub ' Private Sub RegionComboBox_SelectedIndexChanged(...)


  End Class ' CogColorConversionToolEditor


End Namespace ' CogColorConversionTool
