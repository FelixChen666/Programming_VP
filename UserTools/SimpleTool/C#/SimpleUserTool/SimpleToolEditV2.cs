/******************************************************************************
Copyright (C) 2008 Cognex Corporation

Subject to Cognex Corporations terms and conditions and license agreement,
you are authorized to use and modify this source code in any way you find
useful, provided the Software and/or the modified Software is used solely in
conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
and agree that Cognex has no warranty, obligations or liability for your use
of the Software.
******************************************************************************/
// This sample program is designed to illustrate certain VisionPro
// features or techniques in the simplest way possible. It is not
// intended as the framework for a complete application. In
// particular, the sample program may not provide proper error
// handling, event handling, cleanup, repeatability, and other
// mechanisms that a commercial quality application requires.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Cognex.VisionPro;

// This file contains the source code for SimpleToolEditV2.
// The user should do the following things to create a Edit Control:
// 1. Create a new Inherited User Control.
//    Choose "Cognex.VisionPro.Controls.dll" and "CogToolEditControlBaseV2".
// 2. Drag and drop required controls to the SimpleToolEditV2.Designer.
// 3. Create a helper function to set the values of associated with
//      these subcontrols. See SetupSettingsTab() below.
// 4. Create a constructor.
// 5. Create a Subject getter and setter.
// 6. Create an override of InitializeFromSubject().
// 7. Create an override of SubjectValuesChanged().
// 8. Create an override of SubjectInUseChanged().
// 9. Add code to react to user inputs.


namespace SimpleTool
  {
  [System.ComponentModel.ToolboxItem(true)]
  public partial class SimpleToolEditV2 : CogToolEditControlBaseV2
    {
    public SimpleToolEditV2()
      : base(false) // base(bool hasFloatingResults)
      {
      InitializeComponent();

      // No electric behavior in this sample ...
      tbbElectric.Visible = false;

      } // public SimpleToolEditV2()


    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SimpleTool Subject
      {
      get { return base.GetSubject() as SimpleTool; }

      set { base.SetSubject(value); }

      } // public SimpleTool Subject


    // This (override) method is called at the first thread-safe
    // opportunity after the Subject has been replaced. The control
    // should use this method to (re)initialize itself using values
    // from the new subject.
    protected override void InitializeFromSubject()
      {
      base.InitializeFromSubject();

      SetupSettingsTab();

      } // protected override void InitializeFromSubject()


    // This (override) method is called whenever the Subject raises
    // a Changed event. Note that this derived class override must
    // call the base method. This function is always executed on
    // the GUI thread: InvokeRequired will always return false.
    // This is a good place to update any subcontrols whose value
    // may have changed (as indicated by the state flags).
    protected override void SubjectValuesChanged(
      object sender,
      CogChangedEventArgs e)
      {
      base.SubjectValuesChanged(sender, e);

      if ((e.StateFlags & SimpleTool.SfCopyTwice) != 0)
        {
        SetupSettingsTab();

        } // if ((e.StateFlags & SimpleTool.SfCopyTwice) != 0)

      } // protected override void SubjectValuesChanged(...)


    // This override provides a notification that the SubjectInUse
    // property has changed. This is a good place to enable/disable
    // subcontrols that (a) do not support queuing and (b) are not
    // implemented via a property provider.
    protected override void SubjectInUseChanged()
      {
      base.SubjectInUseChanged();

      bool bEnabled = false;
      if (Subject != null)
        if (! SubjectInUse)
          bEnabled = true;
      chkCopyTwice.Enabled = bEnabled;

      } // protected override void SubjectInUseChanged()


    // SetupSettingsTab() accesses Subject.xxx, and so should only
    // be called at safe times (e.g. when SubjectInUse is false).
    private void SetupSettingsTab()
      {
      AssertSafe();

      bool bChecked = false;
      if (Subject != null)
        if (Subject.CopyTwice)
          bChecked = true;
      chkCopyTwice.Checked = bChecked;

      bool bEnabled = false;
      if (Subject != null)
        bEnabled = true;
      chkCopyTwice.Enabled = bEnabled;

      } // private void SetupSettingsTab()


    // React to user input. Note that, because we disable input
    // from this control during unsafe times (when the tool is running),
    // this should be a safe time to access the tool.
    private void chkCopyTwice_CheckedChanged(
      object sender,
      EventArgs e)
      {
      // It should be safe, but we'll check anyway ...
      if (Subject == null)
        return;
      if (SubjectInUse)
        return;

      Subject.CopyTwice = chkCopyTwice.Checked;

      } // private void chkCopyTwice_CheckedChanged(...)


    } // public partial class SimpleToolEditV2 : CogToolEditControlBaseV2


  } // namespace SimpleTool

