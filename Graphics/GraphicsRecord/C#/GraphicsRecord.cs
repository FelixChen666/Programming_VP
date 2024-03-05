//*******************************************************************************
// Copyright (C) 2004 Cognex Corporation
//
// Subject to Cognex Corporation's terms and conditions and license agreement,
// you are authorized to use and modify this source code in any way you find
// useful, provided the Software and/or the modified Software is used solely in
// conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
// and agree that Cognex has no warranty, obligations or liability for your use
// of the Software.
//*******************************************************************************
// This sample program is designed to illustrate certain VisionPro features or 
// techniques in the simplest way possible. It is not intended as the framework 
// for a complete application. In particular, the sample program may not provide
// proper error handling, event handling, cleanup, repeatability, and other 
// mechanisms that a commercial quality application requires.

// This sample shows how to display a Blob Tool's LastRunRecord graphics
// on a form using a CogRecordDisplay.  It also demonstrates how to
// selectively enable or disable different graphic features using
// the LastRunRecordEnable property of the BlobTool.
// This same pattern can be applied to all VisionPro tools.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Cognex.VisionPro;                  //used for basic VisionPro functionality
using Cognex.VisionPro.Implementation;   //used for CogRecord
using Cognex.VisionPro.Exceptions;       //used for VisionPro exceptions
using Cognex.VisionPro.Blob;             //used for CogBlobTool
using Cognex.VisionPro.ImageFile;        //used for CogImageFileTool

namespace GraphicsRecord
{
  /// <summary>
  /// Summary description for GraphicsRecordForm.
  /// </summary>
  public class GraphicsRecordForm : System.Windows.Forms.Form
  {
    #region Private Fields
    private System.Windows.Forms.TabControl tabGraphicsRecord;
    private System.Windows.Forms.TabPage tabSample;
    private System.Windows.Forms.TabPage tabImageFile;
    private System.Windows.Forms.TabPage tabTool;
    private Cognex.VisionPro.ImageFile.CogImageFileEditV2 mImageFileEdit;
    private Cognex.VisionPro.Blob.CogBlobEditV2 mBlobToolEdit;
    private Cognex.VisionPro.CogRecordDisplay mDisplay;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.Button btnRun;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.GroupBox grpToolGraphics;
    private System.Windows.Forms.CheckBox chkShowBoundary;
    private System.Windows.Forms.CheckBox chkShowBoundingBox;
    private System.Windows.Forms.CheckBox chkShowCOM;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    private CogImageFileTool mImageFileTool = null;
    private CogBlobTool      mBlobTool = null;

    #endregion

    #region Constructors & Dispose
    public GraphicsRecordForm()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      // Open the image file.  If an error occurs when opening the image file
      // (not found, for example), report it and return failure, using try...catch...  
      string sImageFile = Environment.GetEnvironmentVariable ("VPRO_ROOT");
      try
      {
        if (sImageFile == "")
          throw new Exception ("Required environment variable VPRO_ROOT is not set");
        sImageFile += @"\images\bracket_std.idb";

        // sink the events we are interested in
        mImageFileTool = mImageFileEdit.Subject;
        mImageFileTool.Operator.Open (sImageFile, CogImageFileModeConstants.Read);

        mBlobTool = mBlobToolEdit.Subject;
        mBlobTool.Changed += new CogChangedEventHandler(mBlobTool_Changed);
        mBlobTool.Ran += new EventHandler(mBlobTool_Ran);

        // use DataBinding property to link the output of ImageFileTool to input of BlobTool
        mBlobTool.DataBindings.Add ("InputImage", mImageFileTool, "OutputImage");

        // run the image file tool and blob tool once
        mImageFileTool.Run ();
        mBlobTool.Run ();

        // synchronize the controls with tool settings
        SyncGraphicsControls ();

        // setup txtDescription
        txtDescription.Text = "";
        txtDescription.AppendText ("Sample description: shows how to display CogRecord graphics in a CogRecordDisplay.");
        txtDescription.AppendText (Environment.NewLine);
        txtDescription.AppendText ("Sample usage: configure the graphics shown using the Vision Tool Graphics checkboxes.  ");
        txtDescription.AppendText ("The graphics shown can also be configured using the Graphics tab of the tool edit control.  ");
        txtDescription.AppendText ("The tool edit control can be found on the Vision Tool tab of this Form.");
      }
      catch (CogFileOpenException ex)
      {
        DisplayErrorAndExit (ex.GetType ().ToString (), "Caught exception: " + ex.Message + 
                             Environment.NewLine + "File " + sImageFile + " is not found");
        this.Dispose (true);  // dispose the instance
      }
      catch (CogException ex)
      {
        DisplayErrorAndExit (ex.GetType ().ToString (), "Caught Cognex exception: " + ex.Message);
        this.Dispose (true);  // dispose the instance
      }
      catch (Exception ex)
      {
        DisplayErrorAndExit (ex.GetType ().ToString (), "Caught System exception: " + ex.Message);
        this.Dispose (true);
      }    
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        // unsink events
        if (mBlobTool != null)
        {
          mBlobTool.Ran -= new EventHandler(mBlobTool_Ran);
        }
        if (components != null) 
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }

    #endregion

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GraphicsRecordForm));
      this.tabGraphicsRecord = new System.Windows.Forms.TabControl();
      this.tabSample = new System.Windows.Forms.TabPage();
      this.grpToolGraphics = new System.Windows.Forms.GroupBox();
      this.chkShowCOM = new System.Windows.Forms.CheckBox();
      this.chkShowBoundary = new System.Windows.Forms.CheckBox();
      this.chkShowBoundingBox = new System.Windows.Forms.CheckBox();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnRun = new System.Windows.Forms.Button();
      this.mDisplay = new Cognex.VisionPro.CogRecordDisplay();
      this.tabImageFile = new System.Windows.Forms.TabPage();
      this.mImageFileEdit = new Cognex.VisionPro.ImageFile.CogImageFileEditV2();
      this.tabTool = new System.Windows.Forms.TabPage();
      this.mBlobToolEdit = new Cognex.VisionPro.Blob.CogBlobEditV2();
      this.txtDescription = new System.Windows.Forms.TextBox();
      this.tabGraphicsRecord.SuspendLayout();
      this.tabSample.SuspendLayout();
      this.grpToolGraphics.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.mDisplay)).BeginInit();
      this.tabImageFile.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.mImageFileEdit)).BeginInit();
      this.tabTool.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.mBlobToolEdit)).BeginInit();
      this.SuspendLayout();
      // 
      // tabGraphicsRecord
      // 
      this.tabGraphicsRecord.Controls.Add(this.tabSample);
      this.tabGraphicsRecord.Controls.Add(this.tabImageFile);
      this.tabGraphicsRecord.Controls.Add(this.tabTool);
      this.tabGraphicsRecord.Dock = System.Windows.Forms.DockStyle.Top;
      this.tabGraphicsRecord.Location = new System.Drawing.Point(0, 0);
      this.tabGraphicsRecord.Name = "tabGraphicsRecord";
      this.tabGraphicsRecord.SelectedIndex = 0;
      this.tabGraphicsRecord.Size = new System.Drawing.Size(780, 408);
      this.tabGraphicsRecord.TabIndex = 0;
      // 
      // tabSample
      // 
      this.tabSample.Controls.Add(this.grpToolGraphics);
      this.tabSample.Controls.Add(this.btnClose);
      this.tabSample.Controls.Add(this.btnRun);
      this.tabSample.Controls.Add(this.mDisplay);
      this.tabSample.Location = new System.Drawing.Point(4, 22);
      this.tabSample.Name = "tabSample";
      this.tabSample.Size = new System.Drawing.Size(772, 382);
      this.tabSample.TabIndex = 0;
      this.tabSample.Text = "Graphics Record Sample";
      // 
      // grpToolGraphics
      // 
      this.grpToolGraphics.Controls.Add(this.chkShowCOM);
      this.grpToolGraphics.Controls.Add(this.chkShowBoundary);
      this.grpToolGraphics.Controls.Add(this.chkShowBoundingBox);
      this.grpToolGraphics.Location = new System.Drawing.Point(540, 88);
      this.grpToolGraphics.Name = "grpToolGraphics";
      this.grpToolGraphics.Size = new System.Drawing.Size(216, 108);
      this.grpToolGraphics.TabIndex = 3;
      this.grpToolGraphics.TabStop = false;
      this.grpToolGraphics.Text = "Vision Tool Graphics";
      // 
      // chkShowCOM
      // 
      this.chkShowCOM.Location = new System.Drawing.Point(12, 76);
      this.chkShowCOM.Name = "chkShowCOM";
      this.chkShowCOM.Size = new System.Drawing.Size(196, 24);
      this.chkShowCOM.TabIndex = 2;
      this.chkShowCOM.Text = "Show Center Of Mass";
      this.chkShowCOM.CheckedChanged += new System.EventHandler(this.chkShowCOM_CheckedChanged);
      // 
      // chkShowBoundary
      // 
      this.chkShowBoundary.Location = new System.Drawing.Point(12, 46);
      this.chkShowBoundary.Name = "chkShowBoundary";
      this.chkShowBoundary.Size = new System.Drawing.Size(196, 24);
      this.chkShowBoundary.TabIndex = 1;
      this.chkShowBoundary.Text = "Show Boundary";
      this.chkShowBoundary.CheckedChanged += new System.EventHandler(this.chkShowBoundary_CheckedChanged);
      // 
      // chkShowBoundingBox
      // 
      this.chkShowBoundingBox.Location = new System.Drawing.Point(12, 20);
      this.chkShowBoundingBox.Name = "chkShowBoundingBox";
      this.chkShowBoundingBox.Size = new System.Drawing.Size(196, 24);
      this.chkShowBoundingBox.TabIndex = 0;
      this.chkShowBoundingBox.Text = "Show Bounding Box";
      this.chkShowBoundingBox.CheckedChanged += new System.EventHandler(this.chkShowBoundingBox_CheckedChanged);
      // 
      // btnClose
      // 
      this.btnClose.Location = new System.Drawing.Point(664, 32);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(88, 36);
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnRun
      // 
      this.btnRun.Location = new System.Drawing.Point(540, 32);
      this.btnRun.Name = "btnRun";
      this.btnRun.Size = new System.Drawing.Size(88, 36);
      this.btnRun.TabIndex = 1;
      this.btnRun.Text = "Run";
      this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
      // 
      // mDisplay
      // 
      this.mDisplay.ContainingControl = this;
      this.mDisplay.Dock = System.Windows.Forms.DockStyle.Left;
      this.mDisplay.Location = new System.Drawing.Point(0, 0);
      this.mDisplay.Name = "mDisplay";
      this.mDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mDisplay.OcxState")));
      this.mDisplay.Size = new System.Drawing.Size(520, 382);
      this.mDisplay.TabIndex = 0;
      // 
      // tabImageFile
      // 
      this.tabImageFile.Controls.Add(this.mImageFileEdit);
      this.tabImageFile.Location = new System.Drawing.Point(4, 22);
      this.tabImageFile.Name = "tabImageFile";
      this.tabImageFile.Size = new System.Drawing.Size(772, 382);
      this.tabImageFile.TabIndex = 1;
      this.tabImageFile.Text = "Image File";
      // 
      // mImageFileEdit
      // 
      this.mImageFileEdit.Dock = System.Windows.Forms.DockStyle.Fill;
      this.mImageFileEdit.Location = new System.Drawing.Point(0, 0);
      this.mImageFileEdit.MinimumSize = new System.Drawing.Size(489, 0);
      this.mImageFileEdit.Name = "mImageFileEdit";
      this.mImageFileEdit.Size = new System.Drawing.Size(772, 382);
      this.mImageFileEdit.SuspendElectricRuns = false;
      this.mImageFileEdit.TabIndex = 0;
      // 
      // tabTool
      // 
      this.tabTool.Controls.Add(this.mBlobToolEdit);
      this.tabTool.Location = new System.Drawing.Point(4, 22);
      this.tabTool.Name = "tabTool";
      this.tabTool.Size = new System.Drawing.Size(772, 382);
      this.tabTool.TabIndex = 2;
      this.tabTool.Text = "Vision Tool";
      // 
      // mBlobToolEdit
      // 
      this.mBlobToolEdit.Dock = System.Windows.Forms.DockStyle.Fill;
      this.mBlobToolEdit.Location = new System.Drawing.Point(0, 0);
      this.mBlobToolEdit.MinimumSize = new System.Drawing.Size(489, 0);
      this.mBlobToolEdit.Name = "mBlobToolEdit";
      this.mBlobToolEdit.Size = new System.Drawing.Size(772, 382);
      this.mBlobToolEdit.SuspendElectricRuns = false;
      this.mBlobToolEdit.TabIndex = 0;
      // 
      // txtDescription
      // 
      this.txtDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.txtDescription.Location = new System.Drawing.Point(0, 409);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtDescription.Size = new System.Drawing.Size(780, 96);
      this.txtDescription.TabIndex = 1;
      this.txtDescription.Text = "";
      // 
      // GraphicsRecordForm
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(780, 505);
      this.Controls.Add(this.txtDescription);
      this.Controls.Add(this.tabGraphicsRecord);
      this.Name = "GraphicsRecordForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Graphics Record Sample Application";
      this.tabGraphicsRecord.ResumeLayout(false);
      this.tabSample.ResumeLayout(false);
      this.grpToolGraphics.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.mDisplay)).EndInit();
      this.tabImageFile.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.mImageFileEdit)).EndInit();
      this.tabTool.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.mBlobToolEdit)).EndInit();
      this.ResumeLayout(false);

    }
    #endregion

    #region Private Methods
    private void SyncGraphicsControls ()
    {
      chkShowBoundingBox.Checked = (mBlobTool.LastRunRecordEnable & CogBlobLastRunRecordConstants.ResultsBoundingBoxExtremaAngle) != 0;
      chkShowBoundary.Checked = (mBlobTool.LastRunRecordEnable & CogBlobLastRunRecordConstants.ResultsBoundary) != 0;
      chkShowCOM.Checked = (mBlobTool.LastRunRecordEnable & CogBlobLastRunRecordConstants.ResultsCenterOfMass) != 0;
    }
    private void SetLastRunRecordEnableFlag (bool enabled, CogBlobLastRunRecordConstants flag)
    {
      if (enabled)
        mBlobTool.LastRunRecordEnable |= flag;
      else
        mBlobTool.LastRunRecordEnable &= ~flag;
    }

    private void DisplayErrorAndExit (string errorType, string message)
    {
      MessageBox.Show (message, "GraphicsRecord", MessageBoxButtons.OK);
      this.Close ();
    }
    #endregion

    #region Private Event Handlers
    /// <summary>
    /// BlobTool Changed event handler, checking if tool result has changed or the tool result
    /// graphics flags have been changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void mBlobTool_Changed(object sender, CogChangedEventArgs e)
    {
      bool GraphicsRecordUpdated = false;
  
      // If the graphics record's enabled flags have changed, sync the graphics
      // control values with the graphics record's flags and set updated flag.
      if ((e.StateFlags & CogBlobTool.SfLastRunRecordEnable) != 0)
      {
        SyncGraphicsControls ();
        GraphicsRecordUpdated = true;
      }
      // If the last run record has new data, set updated flag.
      if ((e.StateFlags & CogBlobTool.SfCreateLastRunRecord) != 0)
        GraphicsRecordUpdated = true;
      
      // create and redraw graphics records if they have changed      
      if (GraphicsRecordUpdated)
      {
        // Get the last run recrod from the blob tool.
        ICogRecord lastRunRecord = mBlobTool.CreateLastRunRecord();

        if (lastRunRecord != null &&
          lastRunRecord.SubRecords.ContainsKey("InputImage"))
        {
          // Display the InputImage sub-record from the blob tool's last run 
          // records.
          mDisplay.Record = lastRunRecord.SubRecords["InputImage"];
          mDisplay.Fit(true);
        }
        else
        {
          // clear the display.
          mDisplay.Record = null;
        }
      }
    }

    /// <summary>
    /// BlobTool Ran event handler, check the tool result. If tool fails, display the error message
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void mBlobTool_Ran(object sender, EventArgs e)
    {
      if (mBlobTool.RunStatus.Result != CogToolResultConstants.Accept)
        MessageBox.Show (mBlobTool.RunStatus.Message, "GraphicsRecord");
    }
    #endregion

    #region Private Control Event Handlers
    /// <summary>
    /// Run button event handler - run the sample application
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRun_Click(object sender, System.EventArgs e)
    {
      if (mImageFileTool != null) mImageFileTool.Run ();
      if (mBlobTool != null) mBlobTool.Run ();
    }

    /// <summary>
    /// Close button event handler - Close the application
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnClose_Click(object sender, System.EventArgs e)
    {
      this.Close ();
    }
    /// <summary>
    /// ShowBoundingBox - enable/disable result graphics
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void chkShowBoundingBox_CheckedChanged(object sender, System.EventArgs e)
    {
      SetLastRunRecordEnableFlag (chkShowBoundingBox.Checked, CogBlobLastRunRecordConstants.ResultsBoundingBoxExtremaAngle);
    }

    /// <summary>
    /// ShowBoundary - enable/disable result graphics
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void chkShowBoundary_CheckedChanged(object sender, System.EventArgs e)
    {
      SetLastRunRecordEnableFlag (chkShowBoundary.Checked, CogBlobLastRunRecordConstants.ResultsBoundary);    
    }

    /// <summary>
    /// ShowCOM - enable/disable result graphics
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void chkShowCOM_CheckedChanged(object sender, System.EventArgs e)
    {
      SetLastRunRecordEnableFlag (chkShowCOM.Checked, CogBlobLastRunRecordConstants.ResultsCenterOfMass);
    }
    #endregion

    #region Main
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() 
    {
      try
      {
        GraphicsRecordForm sample = new GraphicsRecordForm ();
        if (sample.Disposing == false && sample.IsDisposed == false)
          Application.Run (sample);
      }
      catch (Exception ex)
      {
        MessageBox.Show (ex.Message, "GraphicsRecord", MessageBoxButtons.OK);
        Application.Exit ();
      }
    }
    #endregion
  }
}
