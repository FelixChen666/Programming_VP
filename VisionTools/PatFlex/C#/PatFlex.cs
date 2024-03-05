//*******************************************************************************
//Copyright (C) 2003 Cognex Corporation
//
//Subject to Cognex Corporation's terms and conditions and license agreement,
//you are authorized to use and modify this source code in any way you find
//useful, provided the Software and/or the modified Software is used solely in
//conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
//and agree that Cognex has no warranty, obligations or liability for your use
//of the Software.

// This sample program is designed to illustrate certain VisionPro features or 
// techniques in the simplest way possible. It is not intended as the framework 
// for a complete application. In particular, the sample program may not provide
// proper error handling, event handling, cleanup, repeatability, and other 
// mechanisms that a commercial quality application requires.

// This sample demonstrates the use of PatFlex to train and locate a pattern 
// in a user provided image. The user should execute the following steps:
// 1. Select an image source: either an image file or a frame grabber 
//    Something like PatFlex.idb in the Images directory will work. 
// 2. Grab an image from the image source
// 3. Click 'Setup'.  Select a training region. Hit 'OK'.
// 4. Click 'Run' to see the location result and score.
// 5. Click 'Next Image' followed by 'Run' to locate the pattern on a subsequent image
//
// Note that execution parameters can be changed by selecting the appropriate tab and
// modifying the provided values.
//*******************************************************************************
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Cognex.VisionPro;                 // used for basic VisionPro functionality such as CogAcquireTool
using Cognex.VisionPro.Display;         // used for CogDisplay
using Cognex.VisionPro.Exceptions;      // used for VisionPro Exceptions
using Cognex.VisionPro.ImageFile;       // used for CogImageFileTool
using Cognex.VisionPro.ImageProcessing; // used for CogCopyRegionTool
using Cognex.VisionPro.PMAlign;         // used for CogPMAlignTool

namespace PatFlex
{
	/// <summary>
	/// Summary description for PatFlexForm.
	/// </summary>
  public class PatFlexForm : System.Windows.Forms.Form
  {
    #region Private Fields
    private System.Windows.Forms.TabControl tabSamples;
    private System.Windows.Forms.TabPage tabDemo;
    private System.Windows.Forms.TabPage tabFrameGrabber;
    private System.Windows.Forms.TabPage tabImageFile;
    private System.Windows.Forms.TabPage tabPatFlex;
    private System.Windows.Forms.TabPage tabValidRegion;
    private System.Windows.Forms.GroupBox grpImageSource;
    private System.Windows.Forms.Button btnNextImage;
    private System.Windows.Forms.Button btnOpenFile;
    private System.Windows.Forms.GroupBox grpFlexDemo;
    private System.Windows.Forms.Button btnSetup;
    private System.Windows.Forms.Button btnRun;
    private System.Windows.Forms.Label llbScoreTitle;
    private System.Windows.Forms.Label lblScore;
    private System.Windows.Forms.Label lblPatFlexResultDisplay;
    private System.Windows.Forms.Label lblUnwarpedResultDisplay;
    private Cognex.VisionPro.Display.CogDisplay mDisplayDistortionResult;
    private Cognex.VisionPro.Display.CogDisplay mDisplayUnwarpedResult;
    private Cognex.VisionPro.ImageFile.CogImageFileEditV2 mImageFileEdit;
    private Cognex.VisionPro.PMAlign.CogPMAlignEditV2 mPMAlignEdit;
    private Cognex.VisionPro.ImageProcessing.CogCopyRegionEditV2 mCopyRegionEdit;
    private System.Windows.Forms.RadioButton optImageFile;
    private System.Windows.Forms.RadioButton optFrameGrabber;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.OpenFileDialog mOpenFileDialog;
    private IContainer components = null;

    //Declare references for the 4 tools in this sample application. 
    //We will make use of the _Change and _Ran synchronous event handlers.
    private CogImageFileTool  mImageFileTool = null;
    private CogAcqFifoTool    mFifoTool = null;
    private CogCopyRegionTool mRegionTool = null;
    private CogPMAlignTool    mPMAlignTool = null;

    //Flag for "VisionPro Demo" tab indicating that user is currently setting up a
    //tool.  Also used to indicate in live video mode.  If user selects "Setup"
    //then the GUI controls are disabled except for the interactive graphics
    //required for setup as well as the "OK" button used to complete the Setup.
    private bool mDoneSetup = false;
    private CogAcqFifoEditV2 mAcqFifoEdit;
	private int numacqs = 0;

    #endregion

    #region Private Enum
    //Enumeration values passed to EnableAll & DisableAll subroutines which
    //indicates what is being setup thus determining which Buttons on the GUI
    //should be left enabled.
    private enum SetupConstants
    {
      SetupLiveDisplay, SetupPatFlex
    }
    #endregion

    #region Constructor & Dispose
    public PatFlexForm()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      //
      // initialize tools we use in the sample
      //
      txtDescription.Text = "This sample demonstrates the use of PatFlex to train " + 
                "and locate a pattern in a user provided image." + Environment.NewLine + Environment.NewLine + 
                "The user should execute the following steps:" + Environment.NewLine + 
                "1. Select an image source: either an image file or a frame grabber." + Environment.NewLine + 
                "   Something like PatFlex.idb in the Images directory will work." + Environment.NewLine + 
                "2. Grab an image from the image source." + Environment.NewLine + 
                "3. Click 'Setup'.  Select a training region. Hit 'OK'." + Environment.NewLine + 
                "4. Click 'Run' to see the location result and score." + Environment.NewLine + 
                "5. Click 'Next Image' followed by 'Run' to locate the pattern on a subsequent image." + Environment.NewLine + Environment.NewLine + 
                "Note that execution parameters can be changed by selecting the appropriate tab and " + 
                "modifying the provided values.";

      //Set reference to CogImageFileTool created by Edit Control
      //The Image File Edit Control creates its subject when its AutoCreateTool property is true
      mImageFileTool = mImageFileEdit.Subject;
      mImageFileTool.Ran += new EventHandler(mImageFileTool_Ran);
      
      //Initialize the Dialog box for the "Open File" button on the "VisionPro Demo" tab.
      mOpenFileDialog.Filter = mImageFileTool.Operator.FilterText;
      mOpenFileDialog.CheckFileExists = true;

      //Set reference to CogmFifoTool created by Edit Control
      //The Acq Fifo Edit Control creates its subject when its AutoCreateTool property is true
      mFifoTool = mAcqFifoEdit.Subject;
      mFifoTool.Ran += new EventHandler(mFifoTool_Ran);
      
      //Operator will be null if no Frame Grabber is available.  Disable the Frame Grabber
      //option on the "VisionPro Demo" tab if no frame grabber available.
      if (mFifoTool.Operator == null) optFrameGrabber.Enabled = false;
      
      // default option of image source is ImageFile
      optImageFile.Checked = true;

      //AutoCreateTool for the PMAlign edit control is False, therefore, we must create
      //a PMAlign tool and set the subject of the control to reference the new tool.
      mPMAlignTool = new CogPMAlignTool ();
      mPMAlignEdit.Subject = mPMAlignTool;
      
      //Change the default Train Region to center of a 640x480 image & change the DOFs
      //so that Skew is not enabled.  Note - TrainRegion is of type ICogRegion, therefore,
      //we must use a CogRectangleAffine reference in order to call CogRectangleAffine
      //properties.
      CogRectangleAffine mPMTrainRegion = mPMAlignTool.Pattern.TrainRegion as CogRectangleAffine;
      if (mPMTrainRegion != null)
      {
        mPMTrainRegion.SetCenterLengthsRotationSkew (320, 240, 100, 100, 0, 0);
        mPMTrainRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position | 
          CogRectangleAffineDOFConstants.Rotation | CogRectangleAffineDOFConstants.Size;
      }      
      //Create a SearchRegion that uses the entire image (assumes 640x480)
      //Note that by default the SearchRegion is Nothing and PMAlign will search the entire
      //image anyway.  This is added for sample code purposes & to graphically show that the
      //entire image is being used.
      CogRectangle mPMSearchRegion = new CogRectangle ();
      //Establish an Region Of Interest (ROI) to let the user manipulate during training.
      mPMSearchRegion.SetCenterWidthHeight (320, 240, 640, 480);
      mPMSearchRegion.GraphicDOFEnable = CogRectangleDOFConstants.Size | CogRectangleDOFConstants.Position;
      mPMSearchRegion.Interactive = true;
      mPMAlignTool.SearchRegion = mPMSearchRegion;
      
      //Set up the mPMAlignTool to use the PatFlex algorithm.
      mPMAlignTool.Pattern.TrainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatFlex;  
    
      // Sink to mPMAlignTool//s Change event
      mPMAlignTool.Changed += new CogChangedEventHandler(mPMAlignTool_Changed);
      
      //Add a cog Copy Region tool so that we can display only the region that was
      //unwarped by the PatFex algorithm.  This is really the only region that
      //uses the unwarp transform in a valid fashion.  The rest of the image appears
      //unwarped, but it's just borrowing the unwarp transform and is not really
      //correct in a mathematical sense.
      mRegionTool = new CogCopyRegionTool ();
      mCopyRegionEdit.Subject = mRegionTool;
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        // we created mRegionTool and PMAlign Tool, so we need to dispose them too
        if (mRegionTool != null) mRegionTool.Dispose ();
        if (mPMAlignTool != null) 
        {
          // unsink the change event before we dispose the tool
          mPMAlignTool.Changed -= new CogChangedEventHandler(mPMAlignTool_Changed);
          mPMAlignTool.Dispose ();
        }
        // mFifoTool and mImageFileTool are from the control. We leave them up to the
        // control to release them
        if (mImageFileTool != null) 
        {
          mImageFileTool.Operator.Close (); //close opened image file
          mImageFileTool.Ran -= new EventHandler(mImageFileTool_Ran); //unsink the event
        }
        if (mFifoTool != null)
        {
          mFifoTool.Ran -= new EventHandler(mFifoTool_Ran); //unsink the Ran event
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatFlexForm));
            this.tabSamples = new System.Windows.Forms.TabControl();
            this.tabDemo = new System.Windows.Forms.TabPage();
            this.mDisplayUnwarpedResult = new Cognex.VisionPro.Display.CogDisplay();
            this.mDisplayDistortionResult = new Cognex.VisionPro.Display.CogDisplay();
            this.lblUnwarpedResultDisplay = new System.Windows.Forms.Label();
            this.lblPatFlexResultDisplay = new System.Windows.Forms.Label();
            this.grpFlexDemo = new System.Windows.Forms.GroupBox();
            this.lblScore = new System.Windows.Forms.Label();
            this.llbScoreTitle = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnSetup = new System.Windows.Forms.Button();
            this.grpImageSource = new System.Windows.Forms.GroupBox();
            this.btnNextImage = new System.Windows.Forms.Button();
            this.optImageFile = new System.Windows.Forms.RadioButton();
            this.optFrameGrabber = new System.Windows.Forms.RadioButton();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.tabFrameGrabber = new System.Windows.Forms.TabPage();
            this.mAcqFifoEdit = new Cognex.VisionPro.CogAcqFifoEditV2();
            this.tabImageFile = new System.Windows.Forms.TabPage();
            this.mImageFileEdit = new Cognex.VisionPro.ImageFile.CogImageFileEditV2();
            this.tabPatFlex = new System.Windows.Forms.TabPage();
            this.mPMAlignEdit = new Cognex.VisionPro.PMAlign.CogPMAlignEditV2();
            this.tabValidRegion = new System.Windows.Forms.TabPage();
            this.mCopyRegionEdit = new Cognex.VisionPro.ImageProcessing.CogCopyRegionEditV2();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.mOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabSamples.SuspendLayout();
            this.tabDemo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mDisplayUnwarpedResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mDisplayDistortionResult)).BeginInit();
            this.grpFlexDemo.SuspendLayout();
            this.grpImageSource.SuspendLayout();
            this.tabFrameGrabber.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mAcqFifoEdit)).BeginInit();
            this.tabImageFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mImageFileEdit)).BeginInit();
            this.tabPatFlex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mPMAlignEdit)).BeginInit();
            this.tabValidRegion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mCopyRegionEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // tabSamples
            // 
            this.tabSamples.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSamples.Controls.Add(this.tabDemo);
            this.tabSamples.Controls.Add(this.tabFrameGrabber);
            this.tabSamples.Controls.Add(this.tabImageFile);
            this.tabSamples.Controls.Add(this.tabPatFlex);
            this.tabSamples.Controls.Add(this.tabValidRegion);
            this.tabSamples.Location = new System.Drawing.Point(0, 0);
            this.tabSamples.Name = "tabSamples";
            this.tabSamples.SelectedIndex = 0;
            this.tabSamples.Size = new System.Drawing.Size(864, 536);
            this.tabSamples.TabIndex = 0;
            // 
            // tabDemo
            // 
            this.tabDemo.Controls.Add(this.mDisplayUnwarpedResult);
            this.tabDemo.Controls.Add(this.mDisplayDistortionResult);
            this.tabDemo.Controls.Add(this.lblUnwarpedResultDisplay);
            this.tabDemo.Controls.Add(this.lblPatFlexResultDisplay);
            this.tabDemo.Controls.Add(this.grpFlexDemo);
            this.tabDemo.Controls.Add(this.grpImageSource);
            this.tabDemo.Location = new System.Drawing.Point(4, 22);
            this.tabDemo.Name = "tabDemo";
            this.tabDemo.Size = new System.Drawing.Size(856, 510);
            this.tabDemo.TabIndex = 0;
            this.tabDemo.Text = "VisionPro Demo";
            // 
            // mDisplayUnwarpedResult
            // 
            this.mDisplayUnwarpedResult.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.mDisplayUnwarpedResult.ColorMapLowerRoiLimit = 0D;
            this.mDisplayUnwarpedResult.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.mDisplayUnwarpedResult.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.mDisplayUnwarpedResult.ColorMapUpperRoiLimit = 1D;
            this.mDisplayUnwarpedResult.Location = new System.Drawing.Point(444, 120);
            this.mDisplayUnwarpedResult.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.mDisplayUnwarpedResult.MouseWheelSensitivity = 1D;
            this.mDisplayUnwarpedResult.Name = "mDisplayUnwarpedResult";
            this.mDisplayUnwarpedResult.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mDisplayUnwarpedResult.OcxState")));
            this.mDisplayUnwarpedResult.Size = new System.Drawing.Size(408, 384);
            this.mDisplayUnwarpedResult.TabIndex = 5;
            // 
            // mDisplayDistortionResult
            // 
            this.mDisplayDistortionResult.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.mDisplayDistortionResult.ColorMapLowerRoiLimit = 0D;
            this.mDisplayDistortionResult.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.mDisplayDistortionResult.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.mDisplayDistortionResult.ColorMapUpperRoiLimit = 1D;
            this.mDisplayDistortionResult.Location = new System.Drawing.Point(12, 120);
            this.mDisplayDistortionResult.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.mDisplayDistortionResult.MouseWheelSensitivity = 1D;
            this.mDisplayDistortionResult.Name = "mDisplayDistortionResult";
            this.mDisplayDistortionResult.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mDisplayDistortionResult.OcxState")));
            this.mDisplayDistortionResult.Size = new System.Drawing.Size(408, 384);
            this.mDisplayDistortionResult.TabIndex = 4;
            // 
            // lblUnwarpedResultDisplay
            // 
            this.lblUnwarpedResultDisplay.Location = new System.Drawing.Point(500, 92);
            this.lblUnwarpedResultDisplay.Name = "lblUnwarpedResultDisplay";
            this.lblUnwarpedResultDisplay.Size = new System.Drawing.Size(288, 23);
            this.lblUnwarpedResultDisplay.TabIndex = 3;
            this.lblUnwarpedResultDisplay.Text = "Unwarped PatFlex Result Displayed";
            this.lblUnwarpedResultDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPatFlexResultDisplay
            // 
            this.lblPatFlexResultDisplay.Location = new System.Drawing.Point(72, 92);
            this.lblPatFlexResultDisplay.Name = "lblPatFlexResultDisplay";
            this.lblPatFlexResultDisplay.Size = new System.Drawing.Size(288, 23);
            this.lblPatFlexResultDisplay.TabIndex = 2;
            this.lblPatFlexResultDisplay.Text = "PatFlex Result with Distortion Field Displayed";
            this.lblPatFlexResultDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpFlexDemo
            // 
            this.grpFlexDemo.Controls.Add(this.lblScore);
            this.grpFlexDemo.Controls.Add(this.llbScoreTitle);
            this.grpFlexDemo.Controls.Add(this.btnRun);
            this.grpFlexDemo.Controls.Add(this.btnSetup);
            this.grpFlexDemo.Location = new System.Drawing.Point(440, 4);
            this.grpFlexDemo.Name = "grpFlexDemo";
            this.grpFlexDemo.Size = new System.Drawing.Size(376, 76);
            this.grpFlexDemo.TabIndex = 1;
            this.grpFlexDemo.TabStop = false;
            this.grpFlexDemo.Text = "PatFlex Demo";
            // 
            // lblScore
            // 
            this.lblScore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblScore.Location = new System.Drawing.Point(280, 32);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(64, 24);
            this.lblScore.TabIndex = 3;
            this.lblScore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // llbScoreTitle
            // 
            this.llbScoreTitle.Location = new System.Drawing.Point(220, 32);
            this.llbScoreTitle.Name = "llbScoreTitle";
            this.llbScoreTitle.Size = new System.Drawing.Size(52, 23);
            this.llbScoreTitle.TabIndex = 2;
            this.llbScoreTitle.Text = "Score";
            this.llbScoreTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(116, 28);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(72, 32);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "Run";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(24, 28);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(76, 32);
            this.btnSetup.TabIndex = 0;
            this.btnSetup.Text = "Setup";
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // grpImageSource
            // 
            this.grpImageSource.Controls.Add(this.btnNextImage);
            this.grpImageSource.Controls.Add(this.optImageFile);
            this.grpImageSource.Controls.Add(this.optFrameGrabber);
            this.grpImageSource.Controls.Add(this.btnOpenFile);
            this.grpImageSource.Location = new System.Drawing.Point(40, 4);
            this.grpImageSource.Name = "grpImageSource";
            this.grpImageSource.Size = new System.Drawing.Size(376, 76);
            this.grpImageSource.TabIndex = 0;
            this.grpImageSource.TabStop = false;
            this.grpImageSource.Text = "Image Acquisition";
            // 
            // btnNextImage
            // 
            this.btnNextImage.Location = new System.Drawing.Point(264, 28);
            this.btnNextImage.Name = "btnNextImage";
            this.btnNextImage.Size = new System.Drawing.Size(88, 32);
            this.btnNextImage.TabIndex = 2;
            this.btnNextImage.Text = "Next Image";
            this.btnNextImage.Click += new System.EventHandler(this.btnNextImage_Click);
            // 
            // optImageFile
            // 
            this.optImageFile.Location = new System.Drawing.Point(16, 44);
            this.optImageFile.Name = "optImageFile";
            this.optImageFile.Size = new System.Drawing.Size(128, 24);
            this.optImageFile.TabIndex = 1;
            this.optImageFile.Text = "Image File";
            this.optImageFile.CheckedChanged += new System.EventHandler(this.optImageFile_CheckedChanged);
            // 
            // optFrameGrabber
            // 
            this.optFrameGrabber.Location = new System.Drawing.Point(16, 20);
            this.optFrameGrabber.Name = "optFrameGrabber";
            this.optFrameGrabber.Size = new System.Drawing.Size(128, 24);
            this.optFrameGrabber.TabIndex = 0;
            this.optFrameGrabber.Text = "Frame Grabber";
            this.optFrameGrabber.CheckedChanged += new System.EventHandler(this.optFrameGrabber_CheckedChanged);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(160, 28);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(88, 32);
            this.btnOpenFile.TabIndex = 3;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // tabFrameGrabber
            // 
            this.tabFrameGrabber.Controls.Add(this.mAcqFifoEdit);
            this.tabFrameGrabber.Location = new System.Drawing.Point(4, 22);
            this.tabFrameGrabber.Name = "tabFrameGrabber";
            this.tabFrameGrabber.Size = new System.Drawing.Size(856, 510);
            this.tabFrameGrabber.TabIndex = 1;
            this.tabFrameGrabber.Text = "Frame Grabber";
            // 
            // mAcqFifoEdit
            // 
            this.mAcqFifoEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mAcqFifoEdit.Location = new System.Drawing.Point(0, 0);
            this.mAcqFifoEdit.MinimumSize = new System.Drawing.Size(489, 0);
            this.mAcqFifoEdit.Name = "mAcqFifoEdit";
            this.mAcqFifoEdit.Size = new System.Drawing.Size(856, 510);
            this.mAcqFifoEdit.SuspendElectricRuns = false;
            this.mAcqFifoEdit.TabIndex = 0;
            // 
            // tabImageFile
            // 
            this.tabImageFile.Controls.Add(this.mImageFileEdit);
            this.tabImageFile.Location = new System.Drawing.Point(4, 22);
            this.tabImageFile.Name = "tabImageFile";
            this.tabImageFile.Size = new System.Drawing.Size(856, 510);
            this.tabImageFile.TabIndex = 2;
            this.tabImageFile.Text = "Image File";
            // 
            // mImageFileEdit
            // 
            this.mImageFileEdit.AllowDrop = true;
            this.mImageFileEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mImageFileEdit.Location = new System.Drawing.Point(0, 0);
            this.mImageFileEdit.MinimumSize = new System.Drawing.Size(489, 0);
            this.mImageFileEdit.Name = "mImageFileEdit";
            this.mImageFileEdit.OutputHighLight = System.Drawing.Color.Lime;
            this.mImageFileEdit.Size = new System.Drawing.Size(856, 510);
            this.mImageFileEdit.SuspendElectricRuns = false;
            this.mImageFileEdit.TabIndex = 0;
            // 
            // tabPatFlex
            // 
            this.tabPatFlex.Controls.Add(this.mPMAlignEdit);
            this.tabPatFlex.Location = new System.Drawing.Point(4, 22);
            this.tabPatFlex.Name = "tabPatFlex";
            this.tabPatFlex.Size = new System.Drawing.Size(856, 510);
            this.tabPatFlex.TabIndex = 3;
            this.tabPatFlex.Text = "PatFlex Tool";
            // 
            // mPMAlignEdit
            // 
            this.mPMAlignEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mPMAlignEdit.Location = new System.Drawing.Point(0, 0);
            this.mPMAlignEdit.MinimumSize = new System.Drawing.Size(489, 0);
            this.mPMAlignEdit.Name = "mPMAlignEdit";
            this.mPMAlignEdit.Size = new System.Drawing.Size(856, 510);
            this.mPMAlignEdit.SuspendElectricRuns = false;
            this.mPMAlignEdit.TabIndex = 0;
            // 
            // tabValidRegion
            // 
            this.tabValidRegion.Controls.Add(this.mCopyRegionEdit);
            this.tabValidRegion.Location = new System.Drawing.Point(4, 22);
            this.tabValidRegion.Name = "tabValidRegion";
            this.tabValidRegion.Size = new System.Drawing.Size(856, 510);
            this.tabValidRegion.TabIndex = 4;
            this.tabValidRegion.Text = "Valid Region";
            // 
            // mCopyRegionEdit
            // 
            this.mCopyRegionEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mCopyRegionEdit.Location = new System.Drawing.Point(0, 0);
            this.mCopyRegionEdit.MinimumSize = new System.Drawing.Size(489, 0);
            this.mCopyRegionEdit.Name = "mCopyRegionEdit";
            this.mCopyRegionEdit.Size = new System.Drawing.Size(856, 510);
            this.mCopyRegionEdit.SuspendElectricRuns = false;
            this.mCopyRegionEdit.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtDescription.Location = new System.Drawing.Point(0, 541);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDescription.Size = new System.Drawing.Size(864, 116);
            this.txtDescription.TabIndex = 6;
            // 
            // PatFlexForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(864, 657);
            this.Controls.Add(this.tabSamples);
            this.Controls.Add(this.txtDescription);
            this.Name = "PatFlexForm";
            this.Text = "PatFlex Sample Application";
            this.tabSamples.ResumeLayout(false);
            this.tabDemo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mDisplayUnwarpedResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mDisplayDistortionResult)).EndInit();
            this.grpFlexDemo.ResumeLayout(false);
            this.grpImageSource.ResumeLayout(false);
            this.tabFrameGrabber.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mAcqFifoEdit)).EndInit();
            this.tabImageFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mImageFileEdit)).EndInit();
            this.tabPatFlex.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mPMAlignEdit)).EndInit();
            this.tabValidRegion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mCopyRegionEdit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }
    #endregion

    #region Private Control Event Handler
    /// <summary>
    /// Open Image File or Start Live Display
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnOpenFile_Click(object sender, System.EventArgs e)
    {
      //Clear graphics, assuming a new image will be in the display once user
      //completes either Live Video or Open File operation, therefore, graphics
      //will be out of sync.
      mDisplayDistortionResult.StaticGraphics.Clear ();
      mDisplayDistortionResult.InteractiveGraphics.Clear ();
  
      //"Live Video" & "Stop Live" button when Frame Grabber option is selected.
      //Using our EnableAll & DisableAll subroutine to force the user stop live
      //video before doing anything else.
      if (optFrameGrabber.Checked == true)
      {
        if (mDisplayDistortionResult.LiveDisplayRunning)
        {
          mDisplayDistortionResult.StopLiveDisplay ();
          EnableAll (SetupConstants.SetupLiveDisplay);
          //Run the mFifoTool so that all of the sample app images get the last
          //image from Live Video (see mFifoTool_Ran)
          mFifoTool.Run ();
        }
        else if (mFifoTool.Operator != null)
        {
          mDisplayDistortionResult.StartLiveDisplay (mFifoTool.Operator, true);
          DisableAll (SetupConstants.SetupLiveDisplay);
        }
      }
      //"Open File" button when image file option is selected
      //DrawingEnabled is used to simply hide the image while the Fit is performed.
      //This prevents the image from being diplayed at the initial zoom factor
      //prior to fit being called.
      else
      {
        try
        {
          DialogResult result = mOpenFileDialog.ShowDialog ();
          if (result != DialogResult.Cancel)
          {
            mImageFileTool.Operator.Open (mOpenFileDialog.FileName, CogImageFileModeConstants.Read);
            mDisplayDistortionResult.DrawingEnabled = false;
            mImageFileTool.Run ();
            mDisplayDistortionResult.Fit (true);
            mDisplayDistortionResult.DrawingEnabled = true;
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show (ex.Message);
        }
        finally
        {
          // Add code that needs to run no matter an exception is thrown or not
        }
      }
    }

    //"New Image" / "Next Image" button.  Simply call Run for the approriate tool.
    //The tool's _Ran will handle passing its OutputImage to the desired
    //destinations.  By using the _Ran instead of the placing the code this
    //_Click routine, any Run, regardless of how initiated, will have the new
    //OutputImage passed to the desired locations.
    private void btnNextImage_Click(object sender, System.EventArgs e)
    {
      if (optFrameGrabber.Checked)
      {
        mFifoTool.Run ();
      }
      else
      {
        mImageFileTool.Run ();
      }
    }

    /// <summary>
    /// Setup PatFlex
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSetup_Click(object sender, System.EventArgs e)
    {
      //PatMax Setup button has been pressed, Entering mDoneSetup mode.
      if (!mDoneSetup)
      {
        //Copy InputImage to TrainImage, if no ImputImage then display an
        //error message
        if (mPMAlignTool.InputImage == null) 
        {
          MessageBox.Show ("No InputImage available for setup.");
          return;
        }
        mPMAlignTool.Pattern.TrainImage = mPMAlignTool.InputImage;
        //While setting up PMAlign, disable other GUI controls.
        mDoneSetup = true;
        DisableAll (SetupConstants.SetupPatFlex);
        //Add TrainRegion to display's interactive graphics
        //Add SearchRegion to display's static graphics for display only.
        mDisplayDistortionResult.InteractiveGraphics.Clear ();
        mDisplayDistortionResult.StaticGraphics.Clear ();
        mDisplayDistortionResult.InteractiveGraphics.Add (mPMAlignTool.Pattern.TrainRegion as ICogGraphicInteractive, "TrainRegion", false);
        if (mPMAlignTool.SearchRegion != null)
          mDisplayDistortionResult.StaticGraphics.Add (mPMAlignTool.SearchRegion as ICogGraphicInteractive, "SearchRegion");
      }
      //OK has been pressed, completing Setup.
      else
      {
        mDoneSetup = false;
        mDisplayDistortionResult.InteractiveGraphics.Clear ();
        mDisplayDistortionResult.StaticGraphics.Clear ();
        //Make sure we catch errors from Train, since they are likely.  For example,
        //No InputImage, No Pattern Features, etc.
        try
        {
          mPMAlignTool.Pattern.Origin.TranslationX = mPMAlignTool.Pattern.TrainRegion.EnclosingRectangle (CogCopyShapeConstants.All).CenterX;
          mPMAlignTool.Pattern.Origin.TranslationY = mPMAlignTool.Pattern.TrainRegion.EnclosingRectangle (CogCopyShapeConstants.All).CenterY;
          mPMAlignTool.Pattern.Train ();
          EnableAll (SetupConstants.SetupPatFlex);
        }
        catch (Exception ex)
        {
          MessageBox.Show (ex.Message);
        }
      }
    }
    /// <summary>
    /// PMAlign Run button _Click...Clear graphics, Run the tool, & display a
    /// message box if RunStatus does not indicate a successful run.
    ///Updating of GUI & Display with PMAlign results is handled by the _Change
    /// event handler below.  This allows the user to run PMAlign from the control
    /// and the "VisionPro Demo" will still be updated properly.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRun_Click(object sender, System.EventArgs e)
    {
      mDisplayDistortionResult.InteractiveGraphics.Clear ();
      mDisplayDistortionResult.StaticGraphics.Clear ();
      
      //Set up the mPMAlignTool runtime algorithm to be PatFlex.
      mPMAlignTool.RunParams.RunAlgorithm = CogPMAlignRunAlgorithmConstants.PatFlex;
      mPMAlignTool.RunParams.OwnedFlexParams.SaveDeformationInfo = CogPMAlignFlexDeformationInfoConstants.TransformAndUnwarpData;
      mPMAlignTool.Run ();
      if (mPMAlignTool.RunStatus.Exception != null)
        MessageBox.Show (mPMAlignTool.RunStatus.Message);
    }
    /// <summary>
    /// Toggle FrameGrabber / ImageFile option.  Simply changes some of the
    /// captions on buttons for clarity.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void optFrameGrabber_CheckedChanged(object sender, System.EventArgs e)
    {
      UpdateImageOption ();
    }
    /// <summary>
    /// Toggle FrameGrabber / ImageFile option.  Simply changes some of the
    /// captions on buttons for clarity.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void optImageFile_CheckedChanged(object sender, System.EventArgs e)
    {
      UpdateImageOption ();    
    }
    #endregion

    #region Private Methods
    private void UpdateImageOption ()
    {
      if (optFrameGrabber.Checked)
      {
        btnOpenFile.Text = "Live Video";
        btnNextImage.Text = "New Image";
      }
      else
      {
        btnOpenFile.Text = "Open File";
        btnNextImage.Text = "Next Image";
      }
    }
    /// <summary>
    /// Disable GUI controls when forcing the user to complete a task before moving on
    /// to something new.  Example, Setting up PMAlign.
    /// </summary>
    /// <param name="butThis"></param>
    private void DisableAll(SetupConstants butThis)
    {
      //Disable all of the frames (Disables controls within frame)
      grpImageSource.Enabled = false;
      grpFlexDemo.Enabled = false;
      //Disable all of the tabs except "VisionPro Demo" tab.
      for (int iPage = 1; iPage < tabSamples.TabPages.Count; iPage++)
      {
        ((Control)tabSamples.TabPages [iPage]).Enabled = false;
      }                                                                                                              //Based on what the user is doing, Re-enable appropriate frame and disable
      //specific controls within the frame.
      if (butThis == SetupConstants.SetupPatFlex)
      {
        grpFlexDemo.Enabled = true;
        btnSetup.Text = "OK";
        btnRun.Enabled = false;
      }
      else if (butThis == SetupConstants.SetupLiveDisplay)
      {
        grpImageSource.Enabled = true;
        btnOpenFile.Text = "Stop Live";
        btnNextImage.Enabled = false;
        optFrameGrabber.Enabled = false;
        optImageFile.Enabled = false;
      }
    }                                      
    /// <summary>
    /// Enable all of the GUI controls when done a task.  Example, done setting up PMAlign.
    /// </summary>
    /// <param name="butThis"></param>
    private void EnableAll(SetupConstants butThis)
    {
      grpImageSource.Enabled = true;
      grpFlexDemo.Enabled = true;
      for (int iPage = 1; iPage < tabSamples.TabPages.Count; iPage++)
      {
        ((Control)tabSamples.TabPages [iPage]).Enabled = true;
      }
      if (butThis == SetupConstants.SetupPatFlex)
      {
        btnSetup.Text = "Setup";
        btnRun.Enabled = true;
      }
      else if (butThis == SetupConstants.SetupLiveDisplay)
      {
        btnOpenFile.Text = "Live Video";
        btnNextImage.Enabled = true;
        optFrameGrabber.Enabled = true;
        optImageFile.Enabled = true;
      }
    }
    #endregion

    #region Private Tool Event Handler
    /// <summary>
    /// Pass ImageFile OutputImage to PatMax tool & the Display on "VisionPro Demo" tab.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void mImageFileTool_Ran(object sender, EventArgs e)
    {
      mDisplayDistortionResult.InteractiveGraphics.Clear ();
      mDisplayDistortionResult.StaticGraphics.Clear ();
      mDisplayDistortionResult.Image = mImageFileTool.OutputImage;
      mPMAlignTool.InputImage = mImageFileTool.OutputImage as CogImage8Grey;
    }
    /// <summary>
    /// Pass AcqFifo OutputImage to the PatMax tool & the Display on "VisionPro" tab.
    /// Also, pass OutputImage to the InputImage of ImageFile tool so that this
    /// sample application can be used to Record an image file from frame grabber.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void mFifoTool_Ran(object sender, EventArgs e)
    {

      mDisplayDistortionResult.InteractiveGraphics.Clear ();
      mDisplayDistortionResult.StaticGraphics.Clear ();
      mDisplayDistortionResult.Image = mFifoTool.OutputImage;
      mPMAlignTool.InputImage = mFifoTool.OutputImage as CogImage8Grey;
      mImageFileTool.InputImage = mFifoTool.OutputImage;
	  numacqs++;
		// Call the garbage collector to free up unused images
		if (numacqs > 4)
		{
			GC.Collect();
			numacqs = 0;
		}
    }
    /// <summary>
    /// If PMAlign results have changed then update the Score & Region graphic.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void mPMAlignTool_Changed(object sender, CogChangedEventArgs e)
    {
      if ((e.StateFlags & CogPMAlignTool.SfResults) != 0)
      {
        mDisplayDistortionResult.StaticGraphics.Clear ();
        //Note, Results will be nothing if Run failed.
        if (mPMAlignTool.Results == null) 
          lblScore.Text = "N/A";
        //Passing result does not imply Pattern is found, must check count.
        else if (mPMAlignTool.Results.Count > 0)
        {
          lblScore.Text = mPMAlignTool.Results[0].Score.ToString ("F3");
          //Allow the tool to display the deformation grid so the users get
          //a feel for how much work the tool is really doing.
          CogCompositeShape resultGraphics = mPMAlignTool.Results[0].CreateResultGraphics (CogPMAlignResultGraphicConstants.FlexDeformationGrid | CogPMAlignResultGraphicConstants.MatchRegion);
          mDisplayDistortionResult.InteractiveGraphics.Add (resultGraphics, "PatFlexResults", false);

          //Copy only the region that was trained from the Unwarped Image and display
          //that in the mRegionTool.
          mRegionTool.InputImage = mPMAlignTool.Results[0].UnwarpedInputImage;
          mRegionTool.Region = mPMAlignTool.Pattern.TrainRegion;
          mRegionTool.Run ();

          //Take the output from the mRegionTool and put it in the right hand window of the GUI.
          mDisplayUnwarpedResult.Image = mRegionTool.OutputImage;
          mDisplayUnwarpedResult.Fit (true);
        }
        else
          lblScore.Text = "N/A";
      }
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
        Application.Run(new PatFlexForm());
      }
      catch (Exception ex)
      {
        MessageBox.Show ("Caught exception: " + ex.Message);
      }
      finally
      {
        //Releasing framegrabbers
        CogFrameGrabbers frameGrabbers = new CogFrameGrabbers();
        for (int i = 0; i < frameGrabbers.Count; i++)
        {
          frameGrabbers[i].Disconnect(false);
        }
      }
    }
    #endregion
  }
}
