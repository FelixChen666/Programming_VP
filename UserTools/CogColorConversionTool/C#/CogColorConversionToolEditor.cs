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
// particular, the sample program may not provide  proper error
// handling, event handling, cleanup, repeatability, and other 
// mechanisms that a commercial quality application requires.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Cognex.VisionPro;

// This file contains the source code for CogColorConversionToolEditor.
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


namespace CogColorConversionTool
  {
  [System.ComponentModel.ToolboxItem(true)]
  public partial class CogColorConversionToolEditor :
    Cognex.VisionPro.CogToolEditControlBaseV2
    {
    public CogColorConversionToolEditor()
      : base(false) // base(bool hasFloatingResults)
      {
      InitializeComponent();

      // No electric behavior in this sample ...
      tbbElectric.Visible = false;

      } // public CogColorConversionToolEditor()


    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public CogColorConversionTool Subject
      {
      get { return base.GetSubject() as CogColorConversionTool; }

      set { base.SetSubject(value); }

      } // public CogColorConversionTool Subject


    // This (override) method is called at the first thread-safe
    // opportunity after the Subject has been replaced. The control
    // should use this method to (re)initialize itself using values
    // from the new subject.
    protected override void InitializeFromSubject()
      {
      base.InitializeFromSubject();

      SetupSettingsTab();
      SetupRegionTab();

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

      if ((e.StateFlags & CogColorConversionTool.SfHSI) != 0)
        {
        SetupSettingsTab();

        } // if ((e.StateFlags & CogColorConversionTool.SfHSI) != 0)

      if ((e.StateFlags & CogColorConversionTool.SfRegion) != 0)
        {
        SetupRegionTab();

        } // if ((e.StateFlags & CogColorConversionTool.SfRegion) != 0)

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
      chkConvertToHSI.Enabled = bEnabled;
      cboRegionShape.Enabled = bEnabled;

      } // protected override void SubjectInUseChanged()


    // SetupSettingsTab() accesses Subject.xxx, and so should only
    // be called at safe times (e.g. when SubjectInUse is false).
    private void SetupSettingsTab()
      {
      AssertSafe();

      bool bChecked = false;
      if (Subject != null)
        if (Subject.HSI)
          bChecked = true;
      chkConvertToHSI.Checked = bChecked;

      bool bEnabled = false;
      if (Subject != null)
        bEnabled = true;
      chkConvertToHSI.Enabled = bEnabled;

      } // private void SetupSettingsTab()


    // SetupRegionTab() accesses Subject.xxx, and so should only
    // be called at safe times (e.g. when SubjectInUse is false).
    private void SetupRegionTab()
      {
      AssertSafe();

      // Note that the following simplified approach does
      // not deal well with the case in which the tool's
      // Region property is set from outside the control to
      // a shape other than circle, rectangle, or none.
      int iSelectedIndex = -1;
      if (Subject != null)
        if (Subject.Region == null)
          iSelectedIndex = 2;
        else if (Subject.Region.GetType() == typeof(CogCircle))
          iSelectedIndex = 0;
        else if (Subject.Region.GetType() == typeof(CogRectangle))
          iSelectedIndex = 1;
      cboRegionShape.SelectedIndex = iSelectedIndex;

      bool bEnabled = false;
      if (Subject != null)
        bEnabled = true;
      cboRegionShape.Enabled = bEnabled;

      } // private void SetupSettingsTab()


    // React to user input on the settings tab. Note that, because
    // we disable input from this control during unsafe times (when
    // the tool is running), this should be a safe time to
    // access the tool.
    private void chkConvertToHSI_CheckedChanged(
      object sender,
      EventArgs e)
      {
      // It should be safe, but we'll check anyway ...
      if (Subject == null)
        return;
      if (SubjectInUse)
        return;

      Subject.HSI = chkConvertToHSI.Checked;

      } // private void chkConvertToHSI_CheckedChanged(...)


    // React to user input on the region tab. Note that, because
    // we disable input from this control during unsafe times (when
    // the tool is running), this should be a safe time to
    // access the tool.
    private void cboRegionShape_SelectedIndexChanged(
      object sender,
      EventArgs e)
      {
      // It should be safe, but we'll check anyway ...
      if (Subject == null)
        return;
      if (SubjectInUse)
        return;

      ICogRegion tempRegion;

      switch ((System.String)cboRegionShape.SelectedItem)
        {
        case "CogCircle":
          tempRegion = new CogCircle();
          ((CogCircle)tempRegion).SetCenterRadius(320, 240, 50);
          ((CogCircle)tempRegion).Interactive = true;
          ((CogCircle)tempRegion).GraphicDOFEnable = CogCircleDOFConstants.All;
          Subject.Region = tempRegion;
          break;

        case "CogRectangle":
          tempRegion = new CogRectangle();
          ((CogRectangle)tempRegion).SetCenterWidthHeight(320, 240, 100, 100);
          ((CogRectangle)tempRegion).Interactive = true;
          ((CogRectangle)tempRegion).GraphicDOFEnable =
            CogRectangleDOFConstants.Position | CogRectangleDOFConstants.Size;
          Subject.Region = tempRegion;
          break;

        default:
          Subject.Region = null;
          break;
        } // switch ((System.String)cboRegionShape.SelectedItem)

      } // private void cboRegionShape_SelectedIndexChanged(...)


    } // public partial class CogColorConversionToolEditor : ...


  } // namespace CogColorConversionTool
