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


' This file contains the source code for SimpleToolEditV2.
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


Public Class SimpleToolEditV2


  Public Sub New()
    MyBase.New()

    InitializeComponent()

    ' No electric behavior in this sample ...
    tbbElectric.Visible = False

  End Sub 'Public Sub New()


  <System.ComponentModel.Browsable(False), _
    System.ComponentModel.DesignerSerializationVisibility( _
    System.ComponentModel.DesignerSerializationVisibility.Hidden)> _
  Public Property Subject() As SimpleTool.SimpleTool

    Get
      Return MyBase.GetSubject()
    End Get

    Set( _
      ByVal Value As SimpleTool.SimpleTool)
      MyBase.SetSubject(Value)
    End Set

  End Property ' Public Property Subject() As SimpleTool.SimpleTool


  ' This (override) method is called at the first thread-safe
  ' opportunity after the Subject has been replaced. The control
  ' should use this method to (re)initialize itself using values
  ' from the new subject.
  Protected Overrides Sub InitializeFromSubject()
    MyBase.InitializeFromSubject()

    SetupSettingsTab()

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

    If e.StateFlags And SimpleTool.SimpleTool.SfCopyTwice Then
      SetupSettingsTab()
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
    chkCopyTwice.Enabled = bEnabled

  End Sub ' Protected Overrides Sub SubjectInUseChanged()


  ' SetupSettingsTab() accesses Subject.xxx, and so should only
  ' be called at safe times (e.g. when SubjectInUse is false).
  Private Sub SetupSettingsTab()
    AssertSafe()

    Dim bChecked As Boolean = False
    If Not Subject Is Nothing Then
      If Subject.CopyTwice Then
        bChecked = True
      End If
    End If
    chkCopyTwice.Checked = bChecked

    Dim bEnabled As Boolean = False
    If Not Subject Is Nothing Then
      bEnabled = True
    End If
    chkCopyTwice.Enabled = bEnabled

  End Sub ' Private Sub SetupSettingsTab()


  ' This override provides a notification that the SubjectInUse
  ' property has changed. This is a good place to enable/disable
  ' subcontrols that (a) do not support queuing and (b) are not
  ' implemented via a property provider.
  Private Sub chkCopyTwice_CheckedChanged( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles chkCopyTwice.CheckedChanged

    If Subject Is Nothing Then Return
    If SubjectInUse Then Return

    Subject.CopyTwice = chkCopyTwice.Checked

  End Sub ' Private Sub chkCopyTwice_CheckedChanged( _


End Class ' Public Class SimpleToolEditV2

