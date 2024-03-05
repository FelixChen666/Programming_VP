/*******************************************************************************
Copyright (C) 2004 Cognex Corporation

Subject to Cognex Corporation's terms and conditions and license agreement,
you are authorized to use and modify this source code in any way you find
useful, provided the Software and/or the modified Software is used solely in
conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
and agree that Cognex has no warranty, obligations or liability for your use
of the Software.
*******************************************************************************
 This sample program is designed to illustrate certain VisionPro features or 
 techniques in the simplest way possible. It is not intended as the framework 
 for a complete application. In particular, the sample program may not provide
 proper error handling, event handling, cleanup, repeatability, and other 
 mechanisms that a commercial quality application requires.
 
 This application demonstrates using a collection of PMAlign tools to find object
 points for N-Point calibration, perform calibration, and then measure blob
 features in calibrated space. The user chooses the number of calibration points
 (between 3 and 10) and a separate PMAlign tool is constructed to find each
 calibration point. The N-Point calibration tool uses these points, along with
 USER SUPPLIED calibration measurements to compute the calibrated space.
 Since the demo can't possibly guess what calibrated
 coordinates a user might wish to assign to these points, they will have to be entered
 manually in the grid on the calibration tab.  Typically, these calibrated points will
 be determined by measuring the features of an object, having its schematics, or by
 using an object with a regular pattern (such as a grid) with known dimensions.

 Here is some physical information about the bracket in the database file (bracket_std.idb)
 in the images directory.  The bracket has four holes, two small, two large.
 The two large holes are 4.94 cm apart, the small holes are 10.88 cm
 apart, and the midpoint of the small holes to the midpoint of the large holes
 is 3.712 cm. This information should help pick reasonable raw calibrated
 coordinates to calibrate on those images.
 
 Blob is then run on the calibrated image and measurements for each found blob are
 returned.
*/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
// add using statements for Cognex namespaces
using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.PMAlign;


namespace CalibNPoint
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class CalibNPointForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl CalibNPointTabControl;
		private System.Windows.Forms.TabPage DemoTabPage;
		private System.Windows.Forms.TabPage AcqTabPage;
		private System.Windows.Forms.TabPage ImageFileTabPage;
		private System.Windows.Forms.TabPage PMAlignTabPage;
		private System.Windows.Forms.TabPage CalibNPtTabPage;
		private System.Windows.Forms.TabPage BlobTabPage;
		private System.Windows.Forms.RadioButton FrameGrabberRadio;
		private System.Windows.Forms.RadioButton ImageFileRadio;
		private System.Windows.Forms.Button OpenFileButton;
		private System.Windows.Forms.Button NextImageButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button PMAlignSetupButton;
		private System.Windows.Forms.Button PMAlignRunButton;
		private System.Windows.Forms.Button CalibRunButton;
		private System.Windows.Forms.Button CalibSetupButton;
		private System.Windows.Forms.Button BlobRunButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button RunAllButton;
		private Cognex.VisionPro.Display.CogDisplay cogDisplay1;
		private Cognex.VisionPro.ImageFile.CogImageFileEditV2 cogImageFileEdit1;
        private Cognex.VisionPro.PMAlign.CogPMAlignEditV2 cogPMAlignEdit1;
        private Cognex.VisionPro.CalibFix.CogCalibNPointToNPointEditV2 cogCalibNPointEdit1;
        private Cognex.VisionPro.Blob.CogBlobEditV2 cogBlobEdit1;
		private System.Windows.Forms.TextBox DescriptionText;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox PMAlignComboBox;
    private System.Windows.Forms.NumericUpDown NumPMAlignToolsUpDown;
    private IContainer components;

		// Define Cognex Tool variables and other non-GUI stuff.

		// This arraylist holds the PMAlign tools, one for each calibration point.
		private System.Collections.ArrayList PMAlignTools = new System.Collections.ArrayList();
		// This is the acquisition tool
		private CogAcqFifoTool AcqFifoTool;
		// This is the image file tool.
		private CogImageFileTool ImageFileTool;
		// This is the calibration tool.
		private CogCalibNPointToNPointTool CalibNPointTool;
		// This object holds calibration operator.
		private CogCalibNPointToNPoint Calib;
		// This is the blob tool.
		private CogBlobTool BlobTool;

		// Flag for "VisionPro Demo" tab indicating that user is currently setting up a
		// tool.  Also used to indicate in live video mode.  If user selects "Setup"
		// then the GUI controls are disabled except for the interactive graphics
		// required for setup as well as the "OK" button used to complete the Setup.
		private bool SettingUp;
		private bool RunningPMAlignTools;
		private bool RunningAll;
		private int PMAlignSelected = 0;
		private int NumPMAlignTools;
		private System.Windows.Forms.Label CalibratedLabel;
		private System.Windows.Forms.TextBox BlobCountText;
		private System.Windows.Forms.GroupBox GroupCalib;
		private System.Windows.Forms.GroupBox GroupPMAlign;
		private System.Windows.Forms.GroupBox GroupAcq;
    private System.Windows.Forms.GroupBox GroupBlob;
		private int numAcqs = 0;
    private CogAcqFifoEditV2 CogAcqFifoEditV21;

		// Enumeration values which
		// indicates what is being setup thus determining which buttons on the GUI
		// should be left enabled.
		private enum SettingUpConstants
		{
			SettingUpPMAlign = 0,
			SettingUpCalib = 1,
			SettingUpBlob = 2,
			SettingLiveVideo = 99
		}

		const double PI = 3.141592653589;

		public CalibNPointForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			NumPMAlignTools = (int)NumPMAlignToolsUpDown.Value;
			InitializeTools();
		}

		// This method creates the Nth PMAlign Tool
		public void CreateNewPMAlignTool(int ToolNumber)
		{
			int ulx,uly;
			CogPMAlignTool NewTool = new CogPMAlignTool();
			if (FrameGrabberRadio.Checked)
				NewTool.InputImage = (CogImage8Grey)AcqFifoTool.OutputImage;
			else
				NewTool.InputImage = (CogImage8Grey)ImageFileTool.OutputImage;

			// Setup PMAlign tool.  Add tip text to train region to distinguish
			CogRectangleAffine ar = (CogRectangleAffine)NewTool.Pattern.TrainRegion;
			ar.TipText = "PMAlign Pattern Region " + ToolNumber;
			ar.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position |
				CogRectangleAffineDOFConstants.Rotation |
				CogRectangleAffineDOFConstants.Size;
  			
			// Stagger train regions so they aren't all piled up on one another
			ulx = 25 + 20 * (ToolNumber % 20);
			uly = 25 + 20 * (ToolNumber % 20);
			ar.SetOriginLengthsRotationSkew(uly,uly,100,100,0,0);

			//Setup a cursor indicating region is manipulable.
                        ar.MouseCursor = CogStandardCursorConstants.ManipulableGraphic;

			// Default origin to center of region
			NewTool.Pattern.Origin.TranslationX = ar.CenterX;
			NewTool.Pattern.Origin.TranslationY = ar.CenterY;
  
			// Set it up to find one instance of pattern at any angle.
			NewTool.RunParams.ApproximateNumberToFind = 1;
			NewTool.RunParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;
			NewTool.RunParams.ZoneAngle.Low = -PI;
			NewTool.RunParams.ZoneAngle.High = PI;

			// Add Ran event handler
			NewTool.Ran += new EventHandler(PMAlign_Ran);

			// Add a selection to the PMAlignComboBox selector
			PMAlignComboBox.Items.Add("PMAlign " + ToolNumber.ToString());
			PMAlignTools.Add(NewTool);

		}

		private void InitializeTools()
		{
			CogBlobMeasure myMeasure;

			// Get the auto-created tools
			ImageFileTool = cogImageFileEdit1.Subject;
			AcqFifoTool = CogAcqFifoEditV21.Subject;
			CalibNPointTool = cogCalibNPointEdit1.Subject;
			Calib = CalibNPointTool.Calibration;
			BlobTool = cogBlobEdit1.Subject;

			// AcqFifoToo operator will be Nothing if no Frame Grabber is available.  Disable the Frame
			// Grabber option on the "VisionPro Demo" tab if no frame grabber available.
			if (AcqFifoTool.Operator == null)
				FrameGrabberRadio.Enabled = false;

			// Initialize the Open File Dialog box for the "Open File" button on the "VisionPro Demo" tab.
			openFileDialog1.Filter = ImageFileTool.Operator.FilterText;
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.ReadOnlyChecked = true;

			// AutoCreateTool for the PMAlign edit control is False, therefore, we must set the
			// subject of the control.
			
			for (int i = 0; i < NumPMAlignTools; i++)
				CreateNewPMAlignTool(i);
			cogPMAlignEdit1.Subject = (CogPMAlignTool)PMAlignTools[0];
			PMAlignComboBox.SelectedIndex = 0;

			// Initialize calibration tool
			Calib.AddPointPair(0,0,0,0);
			Calib.AddPointPair(1,0,50,0);
			Calib.AddPointPair(0,1,0,50);
			
			// Add appropriate measurements to Blob Tool
			myMeasure = new CogBlobMeasure();
			myMeasure.Measure = CogBlobMeasureConstants.Area;
			BlobTool.RunParams.RunTimeMeasures.Add(myMeasure);

			myMeasure = new CogBlobMeasure();
			myMeasure.Measure = CogBlobMeasureConstants.CenterMassX;
			BlobTool.RunParams.RunTimeMeasures.Add(myMeasure);
	
			myMeasure = new CogBlobMeasure();
			myMeasure.Measure = CogBlobMeasureConstants.CenterMassY;
			BlobTool.RunParams.RunTimeMeasures.Add(myMeasure);
	
			myMeasure = new CogBlobMeasure();
			myMeasure.Measure = CogBlobMeasureConstants.Perimeter;
			BlobTool.RunParams.RunTimeMeasures.Add(myMeasure);

			RunningPMAlignTools = false;
			RunningAll = false;

			// Add event handler to indicate when the ImageFile Tool,
			// AcqFifo Tool, and Blob Tool have run.
			ImageFileTool.Ran +=new EventHandler(ImageFileTool_Ran);
			AcqFifoTool.Ran +=new EventHandler(AcqFifoTool_Ran);
			BlobTool.Ran +=new EventHandler(BlobTool_Ran);
			
			// Add a changed event handler for the CalibNPoint Tool and Calibration operator
			CalibNPointTool.Ran+=new EventHandler(CalibNPointTool_Ran);
			Calib.Changed +=new CogChangedEventHandler(Calib_Changed);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalibNPointForm));
      this.CalibNPointTabControl = new System.Windows.Forms.TabControl();
      this.DemoTabPage = new System.Windows.Forms.TabPage();
      this.cogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
      this.RunAllButton = new System.Windows.Forms.Button();
      this.GroupCalib = new System.Windows.Forms.GroupBox();
      this.CalibratedLabel = new System.Windows.Forms.Label();
      this.CalibRunButton = new System.Windows.Forms.Button();
      this.CalibSetupButton = new System.Windows.Forms.Button();
      this.GroupPMAlign = new System.Windows.Forms.GroupBox();
      this.PMAlignRunButton = new System.Windows.Forms.Button();
      this.PMAlignSetupButton = new System.Windows.Forms.Button();
      this.NumPMAlignToolsUpDown = new System.Windows.Forms.NumericUpDown();
      this.label1 = new System.Windows.Forms.Label();
      this.GroupAcq = new System.Windows.Forms.GroupBox();
      this.NextImageButton = new System.Windows.Forms.Button();
      this.OpenFileButton = new System.Windows.Forms.Button();
      this.ImageFileRadio = new System.Windows.Forms.RadioButton();
      this.FrameGrabberRadio = new System.Windows.Forms.RadioButton();
      this.GroupBlob = new System.Windows.Forms.GroupBox();
      this.BlobCountText = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.BlobRunButton = new System.Windows.Forms.Button();
      this.AcqTabPage = new System.Windows.Forms.TabPage();
      this.ImageFileTabPage = new System.Windows.Forms.TabPage();
      this.cogImageFileEdit1 = new Cognex.VisionPro.ImageFile.CogImageFileEditV2();
      this.PMAlignTabPage = new System.Windows.Forms.TabPage();
      this.PMAlignComboBox = new System.Windows.Forms.ComboBox();
      this.label4 = new System.Windows.Forms.Label();
      this.cogPMAlignEdit1 = new Cognex.VisionPro.PMAlign.CogPMAlignEditV2();
      this.CalibNPtTabPage = new System.Windows.Forms.TabPage();
      this.cogCalibNPointEdit1 = new Cognex.VisionPro.CalibFix.CogCalibNPointToNPointEditV2();
      this.BlobTabPage = new System.Windows.Forms.TabPage();
      this.cogBlobEdit1 = new Cognex.VisionPro.Blob.CogBlobEditV2();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.DescriptionText = new System.Windows.Forms.TextBox();
      this.CogAcqFifoEditV21 = new Cognex.VisionPro.CogAcqFifoEditV2();
      this.CalibNPointTabControl.SuspendLayout();
      this.DemoTabPage.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).BeginInit();
      this.GroupCalib.SuspendLayout();
      this.GroupPMAlign.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.NumPMAlignToolsUpDown)).BeginInit();
      this.GroupAcq.SuspendLayout();
      this.GroupBlob.SuspendLayout();
      this.AcqTabPage.SuspendLayout();
      this.ImageFileTabPage.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.cogImageFileEdit1)).BeginInit();
      this.PMAlignTabPage.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.cogPMAlignEdit1)).BeginInit();
      this.CalibNPtTabPage.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.cogCalibNPointEdit1)).BeginInit();
      this.BlobTabPage.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.cogBlobEdit1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.CogAcqFifoEditV21)).BeginInit();
      this.SuspendLayout();
      // 
      // CalibNPointTabControl
      // 
      this.CalibNPointTabControl.Controls.Add(this.DemoTabPage);
      this.CalibNPointTabControl.Controls.Add(this.AcqTabPage);
      this.CalibNPointTabControl.Controls.Add(this.ImageFileTabPage);
      this.CalibNPointTabControl.Controls.Add(this.PMAlignTabPage);
      this.CalibNPointTabControl.Controls.Add(this.CalibNPtTabPage);
      this.CalibNPointTabControl.Controls.Add(this.BlobTabPage);
      this.CalibNPointTabControl.Location = new System.Drawing.Point(8, 8);
      this.CalibNPointTabControl.Name = "CalibNPointTabControl";
      this.CalibNPointTabControl.SelectedIndex = 0;
      this.CalibNPointTabControl.Size = new System.Drawing.Size(872, 536);
      this.CalibNPointTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
      this.CalibNPointTabControl.TabIndex = 0;
      // 
      // DemoTabPage
      // 
      this.DemoTabPage.Controls.Add(this.cogDisplay1);
      this.DemoTabPage.Controls.Add(this.RunAllButton);
      this.DemoTabPage.Controls.Add(this.GroupCalib);
      this.DemoTabPage.Controls.Add(this.GroupPMAlign);
      this.DemoTabPage.Controls.Add(this.NumPMAlignToolsUpDown);
      this.DemoTabPage.Controls.Add(this.label1);
      this.DemoTabPage.Controls.Add(this.GroupAcq);
      this.DemoTabPage.Controls.Add(this.GroupBlob);
      this.DemoTabPage.Location = new System.Drawing.Point(4, 22);
      this.DemoTabPage.Name = "DemoTabPage";
      this.DemoTabPage.Size = new System.Drawing.Size(864, 510);
      this.DemoTabPage.TabIndex = 0;
      this.DemoTabPage.Text = "VisionPro Demo";
      // 
      // cogDisplay1
      // 
      this.cogDisplay1.Location = new System.Drawing.Point(408, 40);
      this.cogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
      this.cogDisplay1.MouseWheelSensitivity = 1;
      this.cogDisplay1.Name = "cogDisplay1";
      this.cogDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay1.OcxState")));
      this.cogDisplay1.Size = new System.Drawing.Size(432, 416);
      this.cogDisplay1.TabIndex = 8;
      // 
      // RunAllButton
      // 
      this.RunAllButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RunAllButton.Location = new System.Drawing.Point(24, 440);
      this.RunAllButton.Name = "RunAllButton";
      this.RunAllButton.Size = new System.Drawing.Size(352, 32);
      this.RunAllButton.TabIndex = 7;
      this.RunAllButton.Text = "Next Image and Run Blob";
      this.RunAllButton.Click += new System.EventHandler(this.RunAllButton_Click);
      // 
      // GroupCalib
      // 
      this.GroupCalib.Controls.Add(this.CalibratedLabel);
      this.GroupCalib.Controls.Add(this.CalibRunButton);
      this.GroupCalib.Controls.Add(this.CalibSetupButton);
      this.GroupCalib.Location = new System.Drawing.Point(24, 256);
      this.GroupCalib.Name = "GroupCalib";
      this.GroupCalib.Size = new System.Drawing.Size(352, 72);
      this.GroupCalib.TabIndex = 5;
      this.GroupCalib.TabStop = false;
      this.GroupCalib.Text = "CalibNPoint";
      // 
      // CalibratedLabel
      // 
      this.CalibratedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CalibratedLabel.ForeColor = System.Drawing.Color.Red;
      this.CalibratedLabel.Location = new System.Drawing.Point(248, 32);
      this.CalibratedLabel.Name = "CalibratedLabel";
      this.CalibratedLabel.Size = new System.Drawing.Size(88, 16);
      this.CalibratedLabel.TabIndex = 2;
      this.CalibratedLabel.Text = "Uncalibrated";
      // 
      // CalibRunButton
      // 
      this.CalibRunButton.Location = new System.Drawing.Point(144, 24);
      this.CalibRunButton.Name = "CalibRunButton";
      this.CalibRunButton.Size = new System.Drawing.Size(80, 32);
      this.CalibRunButton.TabIndex = 1;
      this.CalibRunButton.Text = "Run";
      this.CalibRunButton.Click += new System.EventHandler(this.CalibRunButton_Click);
      // 
      // CalibSetupButton
      // 
      this.CalibSetupButton.Location = new System.Drawing.Point(32, 24);
      this.CalibSetupButton.Name = "CalibSetupButton";
      this.CalibSetupButton.Size = new System.Drawing.Size(80, 32);
      this.CalibSetupButton.TabIndex = 0;
      this.CalibSetupButton.Text = "Calibrate";
      this.CalibSetupButton.Click += new System.EventHandler(this.CalibSetupButton_Click);
      // 
      // GroupPMAlign
      // 
      this.GroupPMAlign.Controls.Add(this.PMAlignRunButton);
      this.GroupPMAlign.Controls.Add(this.PMAlignSetupButton);
      this.GroupPMAlign.Location = new System.Drawing.Point(24, 176);
      this.GroupPMAlign.Name = "GroupPMAlign";
      this.GroupPMAlign.Size = new System.Drawing.Size(352, 72);
      this.GroupPMAlign.TabIndex = 3;
      this.GroupPMAlign.TabStop = false;
      this.GroupPMAlign.Text = "PMAlign";
      // 
      // PMAlignRunButton
      // 
      this.PMAlignRunButton.Location = new System.Drawing.Point(144, 24);
      this.PMAlignRunButton.Name = "PMAlignRunButton";
      this.PMAlignRunButton.Size = new System.Drawing.Size(80, 32);
      this.PMAlignRunButton.TabIndex = 1;
      this.PMAlignRunButton.Text = "Run All";
      this.PMAlignRunButton.Click += new System.EventHandler(this.PMAlignRunButton_Click);
      // 
      // PMAlignSetupButton
      // 
      this.PMAlignSetupButton.Location = new System.Drawing.Point(32, 24);
      this.PMAlignSetupButton.Name = "PMAlignSetupButton";
      this.PMAlignSetupButton.Size = new System.Drawing.Size(80, 32);
      this.PMAlignSetupButton.TabIndex = 0;
      this.PMAlignSetupButton.Text = "Setup All";
      this.PMAlignSetupButton.Click += new System.EventHandler(this.PMAlignSetupButton_Click);
      // 
      // NumPMAlignToolsUpDown
      // 
      this.NumPMAlignToolsUpDown.Location = new System.Drawing.Point(224, 144);
      this.NumPMAlignToolsUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.NumPMAlignToolsUpDown.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
      this.NumPMAlignToolsUpDown.Name = "NumPMAlignToolsUpDown";
      this.NumPMAlignToolsUpDown.Size = new System.Drawing.Size(64, 20);
      this.NumPMAlignToolsUpDown.TabIndex = 2;
      this.NumPMAlignToolsUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
      this.NumPMAlignToolsUpDown.ValueChanged += new System.EventHandler(this.NumPMAlignToolsUpDown_ValueChanged);
      // 
      // label1
      // 
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(40, 144);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(176, 16);
      this.label1.TabIndex = 1;
      this.label1.Text = "Number of Calibration Points:";
      // 
      // GroupAcq
      // 
      this.GroupAcq.Controls.Add(this.NextImageButton);
      this.GroupAcq.Controls.Add(this.OpenFileButton);
      this.GroupAcq.Controls.Add(this.ImageFileRadio);
      this.GroupAcq.Controls.Add(this.FrameGrabberRadio);
      this.GroupAcq.Location = new System.Drawing.Point(24, 24);
      this.GroupAcq.Name = "GroupAcq";
      this.GroupAcq.Size = new System.Drawing.Size(352, 96);
      this.GroupAcq.TabIndex = 0;
      this.GroupAcq.TabStop = false;
      this.GroupAcq.Text = "Image Acquisition";
      // 
      // NextImageButton
      // 
      this.NextImageButton.Location = new System.Drawing.Point(240, 32);
      this.NextImageButton.Name = "NextImageButton";
      this.NextImageButton.Size = new System.Drawing.Size(88, 32);
      this.NextImageButton.TabIndex = 3;
      this.NextImageButton.Text = "Next Image";
      this.NextImageButton.Click += new System.EventHandler(this.NextImageButton_Click);
      // 
      // OpenFileButton
      // 
      this.OpenFileButton.Location = new System.Drawing.Point(136, 32);
      this.OpenFileButton.Name = "OpenFileButton";
      this.OpenFileButton.Size = new System.Drawing.Size(88, 32);
      this.OpenFileButton.TabIndex = 2;
      this.OpenFileButton.Text = "Open File";
      this.OpenFileButton.Click += new System.EventHandler(this.OpenFileButton_Click);
      // 
      // ImageFileRadio
      // 
      this.ImageFileRadio.Checked = true;
      this.ImageFileRadio.Location = new System.Drawing.Point(16, 56);
      this.ImageFileRadio.Name = "ImageFileRadio";
      this.ImageFileRadio.Size = new System.Drawing.Size(104, 24);
      this.ImageFileRadio.TabIndex = 1;
      this.ImageFileRadio.TabStop = true;
      this.ImageFileRadio.Text = "Image File";
      this.ImageFileRadio.CheckedChanged += new System.EventHandler(this.ImageFileRadio_CheckedChanged);
      // 
      // FrameGrabberRadio
      // 
      this.FrameGrabberRadio.Location = new System.Drawing.Point(16, 24);
      this.FrameGrabberRadio.Name = "FrameGrabberRadio";
      this.FrameGrabberRadio.Size = new System.Drawing.Size(104, 24);
      this.FrameGrabberRadio.TabIndex = 0;
      this.FrameGrabberRadio.Text = "Frame Grabber";
      this.FrameGrabberRadio.CheckedChanged += new System.EventHandler(this.FrameGrabberRadio_CheckedChanged);
      // 
      // GroupBlob
      // 
      this.GroupBlob.Controls.Add(this.BlobCountText);
      this.GroupBlob.Controls.Add(this.label3);
      this.GroupBlob.Controls.Add(this.BlobRunButton);
      this.GroupBlob.Location = new System.Drawing.Point(24, 344);
      this.GroupBlob.Name = "GroupBlob";
      this.GroupBlob.Size = new System.Drawing.Size(352, 72);
      this.GroupBlob.TabIndex = 6;
      this.GroupBlob.TabStop = false;
      this.GroupBlob.Text = "Blob";
      // 
      // BlobCountText
      // 
      this.BlobCountText.Location = new System.Drawing.Point(240, 32);
      this.BlobCountText.Name = "BlobCountText";
      this.BlobCountText.ReadOnly = true;
      this.BlobCountText.Size = new System.Drawing.Size(72, 20);
      this.BlobCountText.TabIndex = 2;
      this.BlobCountText.Text = "0";
      // 
      // label3
      // 
      this.label3.Location = new System.Drawing.Point(168, 32);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(64, 16);
      this.label3.TabIndex = 1;
      this.label3.Text = "Blob Count";
      // 
      // BlobRunButton
      // 
      this.BlobRunButton.Location = new System.Drawing.Point(32, 24);
      this.BlobRunButton.Name = "BlobRunButton";
      this.BlobRunButton.Size = new System.Drawing.Size(104, 32);
      this.BlobRunButton.TabIndex = 0;
      this.BlobRunButton.Text = "Run";
      this.BlobRunButton.Click += new System.EventHandler(this.BlobRunButton_Click);
      // 
      // AcqTabPage
      // 
      this.AcqTabPage.Controls.Add(this.CogAcqFifoEditV21);
      this.AcqTabPage.Location = new System.Drawing.Point(4, 22);
      this.AcqTabPage.Name = "AcqTabPage";
      this.AcqTabPage.Size = new System.Drawing.Size(864, 510);
      this.AcqTabPage.TabIndex = 1;
      this.AcqTabPage.Text = "Acquisition";
      // 
      // ImageFileTabPage
      // 
      this.ImageFileTabPage.Controls.Add(this.cogImageFileEdit1);
      this.ImageFileTabPage.Location = new System.Drawing.Point(4, 22);
      this.ImageFileTabPage.Name = "ImageFileTabPage";
      this.ImageFileTabPage.Size = new System.Drawing.Size(864, 510);
      this.ImageFileTabPage.TabIndex = 2;
      this.ImageFileTabPage.Text = "Image File";
      // 
      // cogImageFileEdit1
      // 
      this.cogImageFileEdit1.AllowDrop = true;
      this.cogImageFileEdit1.Location = new System.Drawing.Point(8, 8);
      this.cogImageFileEdit1.MinimumSize = new System.Drawing.Size(489, 0);
      this.cogImageFileEdit1.Name = "cogImageFileEdit1";
      this.cogImageFileEdit1.OutputHighLight = System.Drawing.Color.Lime;
      this.cogImageFileEdit1.Size = new System.Drawing.Size(848, 472);
      this.cogImageFileEdit1.SuspendElectricRuns = false;
      this.cogImageFileEdit1.TabIndex = 0;
      // 
      // PMAlignTabPage
      // 
      this.PMAlignTabPage.Controls.Add(this.PMAlignComboBox);
      this.PMAlignTabPage.Controls.Add(this.label4);
      this.PMAlignTabPage.Controls.Add(this.cogPMAlignEdit1);
      this.PMAlignTabPage.Location = new System.Drawing.Point(4, 22);
      this.PMAlignTabPage.Name = "PMAlignTabPage";
      this.PMAlignTabPage.Size = new System.Drawing.Size(864, 510);
      this.PMAlignTabPage.TabIndex = 3;
      this.PMAlignTabPage.Text = "PMAlign";
      // 
      // PMAlignComboBox
      // 
      this.PMAlignComboBox.Location = new System.Drawing.Point(152, 24);
      this.PMAlignComboBox.Name = "PMAlignComboBox";
      this.PMAlignComboBox.Size = new System.Drawing.Size(168, 21);
      this.PMAlignComboBox.TabIndex = 2;
      this.PMAlignComboBox.SelectedIndexChanged += new System.EventHandler(this.PMAlignComboBox_SelectedIndexChanged);
      // 
      // label4
      // 
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(24, 24);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(120, 24);
      this.label4.TabIndex = 1;
      this.label4.Text = "PMAlign Tool:";
      // 
      // cogPMAlignEdit1
      // 
      this.cogPMAlignEdit1.Location = new System.Drawing.Point(16, 72);
      this.cogPMAlignEdit1.MinimumSize = new System.Drawing.Size(489, 0);
      this.cogPMAlignEdit1.Name = "cogPMAlignEdit1";
      this.cogPMAlignEdit1.Size = new System.Drawing.Size(832, 440);
      this.cogPMAlignEdit1.SuspendElectricRuns = false;
      this.cogPMAlignEdit1.TabIndex = 0;
      // 
      // CalibNPtTabPage
      // 
      this.CalibNPtTabPage.Controls.Add(this.cogCalibNPointEdit1);
      this.CalibNPtTabPage.Location = new System.Drawing.Point(4, 22);
      this.CalibNPtTabPage.Name = "CalibNPtTabPage";
      this.CalibNPtTabPage.Size = new System.Drawing.Size(864, 510);
      this.CalibNPtTabPage.TabIndex = 4;
      this.CalibNPtTabPage.Text = "CalibNPoint";
      // 
      // cogCalibNPointEdit1
      // 
      this.cogCalibNPointEdit1.Location = new System.Drawing.Point(8, 8);
      this.cogCalibNPointEdit1.MinimumSize = new System.Drawing.Size(489, 0);
      this.cogCalibNPointEdit1.Name = "cogCalibNPointEdit1";
      this.cogCalibNPointEdit1.Size = new System.Drawing.Size(848, 472);
      this.cogCalibNPointEdit1.SuspendElectricRuns = false;
      this.cogCalibNPointEdit1.TabIndex = 0;
      // 
      // BlobTabPage
      // 
      this.BlobTabPage.Controls.Add(this.cogBlobEdit1);
      this.BlobTabPage.Location = new System.Drawing.Point(4, 22);
      this.BlobTabPage.Name = "BlobTabPage";
      this.BlobTabPage.Size = new System.Drawing.Size(864, 510);
      this.BlobTabPage.TabIndex = 5;
      this.BlobTabPage.Text = "Blob";
      // 
      // cogBlobEdit1
      // 
      this.cogBlobEdit1.Location = new System.Drawing.Point(8, 8);
      this.cogBlobEdit1.MinimumSize = new System.Drawing.Size(489, 0);
      this.cogBlobEdit1.Name = "cogBlobEdit1";
      this.cogBlobEdit1.Size = new System.Drawing.Size(848, 472);
      this.cogBlobEdit1.SuspendElectricRuns = false;
      this.cogBlobEdit1.TabIndex = 0;
      // 
      // DescriptionText
      // 
      this.DescriptionText.Location = new System.Drawing.Point(16, 552);
      this.DescriptionText.Multiline = true;
      this.DescriptionText.Name = "DescriptionText";
      this.DescriptionText.ReadOnly = true;
      this.DescriptionText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.DescriptionText.Size = new System.Drawing.Size(864, 104);
      this.DescriptionText.TabIndex = 1;
      this.DescriptionText.Text = resources.GetString("DescriptionText.Text");
      // 
      // CogAcqFifoEditV21
      // 
      this.CogAcqFifoEditV21.Location = new System.Drawing.Point(0, 0);
      this.CogAcqFifoEditV21.MinimumSize = new System.Drawing.Size(489, 0);
      this.CogAcqFifoEditV21.Name = "CogAcqFifoEditV21";
      this.CogAcqFifoEditV21.Size = new System.Drawing.Size(861, 495);
      this.CogAcqFifoEditV21.SuspendElectricRuns = false;
      this.CogAcqFifoEditV21.TabIndex = 0;
      // 
      // CalibNPointForm
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(892, 670);
      this.Controls.Add(this.DescriptionText);
      this.Controls.Add(this.CalibNPointTabControl);
      this.Name = "CalibNPointForm";
      this.Text = "N-Point Calibration Sample Application";
      this.CalibNPointTabControl.ResumeLayout(false);
      this.DemoTabPage.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).EndInit();
      this.GroupCalib.ResumeLayout(false);
      this.GroupPMAlign.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.NumPMAlignToolsUpDown)).EndInit();
      this.GroupAcq.ResumeLayout(false);
      this.GroupBlob.ResumeLayout(false);
      this.GroupBlob.PerformLayout();
      this.AcqTabPage.ResumeLayout(false);
      this.ImageFileTabPage.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.cogImageFileEdit1)).EndInit();
      this.PMAlignTabPage.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.cogPMAlignEdit1)).EndInit();
      this.CalibNPtTabPage.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.cogCalibNPointEdit1)).EndInit();
      this.BlobTabPage.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.cogBlobEdit1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.CogAcqFifoEditV21)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.Run(new CalibNPointForm());

            //Releasing framegrabbers
            CogFrameGrabbers frameGrabbers = new CogFrameGrabbers();
            for (int i = 0; i < frameGrabbers.Count; i++)
            {
                frameGrabbers[i].Disconnect(false);
            }
		}

		private void OpenFileButton_Click(object sender, System.EventArgs e)
		{
			// Clear graphics, assuming a new image will be in the display once user
			// completes either Live Video or Open File operation, therefore, graphics
			// will be out of sync.
			cogDisplay1.InteractiveGraphics.Clear();
			cogDisplay1.StaticGraphics.Clear();

			// "Live Video"  & "Stop Live" button when Frame Grabber option is selected.
			// Using our EnableAll & DisableAll subroutine to force the user stop live
			// video before doing anything else.

			if (FrameGrabberRadio.Checked)
			{
				if (cogDisplay1.LiveDisplayRunning)
				{
					cogDisplay1.StopLiveDisplay();
					EnableAll(SettingUpConstants.SettingLiveVideo);
					
					// Run the AcqFifoTool so that all of the sample app images get the last
					// image from Live Video
					AcqFifoTool.Run();
				}
				else if (AcqFifoTool.Operator != null)
				{
					cogDisplay1.StartLiveDisplay(AcqFifoTool.Operator,false);
					DisableAll(SettingUpConstants.SettingLiveVideo);
				}
			}

			// "Open File" button when image file option is selected
			// DrawingEnabled is used to simply hide the image while the Fit is performed.
			// This prevents the image from being diplayed at the initial zoom factor prior
			// to fit being called.
			else
			{
				openFileDialog1.ShowDialog();
				if (openFileDialog1.FileName != "")
				{
					ImageFileTool.Operator.Open(openFileDialog1.FileName,
						CogImageFileModeConstants.Read);
					cogDisplay1.DrawingEnabled = true;
					ImageFileTool.Run();
					cogDisplay1.Fit(true);
					cogDisplay1.DrawingEnabled = true;
				}
			}
		}
		//  Enable all of the GUI controls when done a task.  Example, done setting up PMAlign.
		private void EnableAll(SettingUpConstants Settings)
		{
			RunAllButton.Enabled = true;
			// Enable all of the groups (Enables controls within group)
			GroupAcq.Enabled = true;
			GroupPMAlign.Enabled = true;
			GroupCalib.Enabled = true;
			GroupBlob.Enabled = true;
			// Enable all of the tabs except "VisionPro Demo" tab.
			for (int i = 1; i < 6;  i++)
			{
				foreach (Control c in CalibNPointTabControl.TabPages[i].Controls)
					c.Enabled = true;
			}
			// Based on what the user is doing, Re-enable appropriate groups and disable
			// specific controls within the group.
			if (Settings == SettingUpConstants.SettingUpPMAlign) 
			{
				GroupPMAlign.Enabled = true;
				PMAlignSetupButton.Text = "Setup";
				PMAlignRunButton.Enabled = true;
			}
			else if (Settings == SettingUpConstants.SettingLiveVideo)
			{
				GroupAcq.Enabled = true;
				OpenFileButton.Text = "Live Video";
				NextImageButton.Enabled = true;
				FrameGrabberRadio.Enabled = true;
				ImageFileRadio.Enabled = true;
			}
		}

		// Disable GUI controls when forcing the user to complete a task before moving on
		// to something new.  Example, Setting up PMAlign.
		private void DisableAll(SettingUpConstants Settings)
		{
			RunAllButton.Enabled = false;
			// Disable all of the groups (Disables controls within group)
			GroupAcq.Enabled = false;
			GroupPMAlign.Enabled = false;
			GroupCalib.Enabled = false;
			GroupBlob.Enabled = false;
			// Disable all of the tabs except "VisionPro Demo" tab.
			for (int i = 1; i < 6;  i++)
			{
				foreach (Control c in CalibNPointTabControl.TabPages[i].Controls)
					c.Enabled = false;
			}
			// Based on what the user is doing, Re-enable appropriate groups and disable
			// specific controls within the group.
			if (Settings == SettingUpConstants.SettingUpPMAlign) 
			{
				GroupPMAlign.Enabled = true;
				PMAlignSetupButton.Text = "OK";
				PMAlignRunButton.Enabled = false;
			}
			else if (Settings == SettingUpConstants.SettingLiveVideo)
			{
				GroupAcq.Enabled = true;
				OpenFileButton.Text = "Stop Live";
				NextImageButton.Enabled = false;
				FrameGrabberRadio.Enabled = false;
				ImageFileRadio.Enabled = false;
			}
		}

		// "New Image" / "Next Image" button.  Simply call Run for the approriate tool.
		// The tool's Ran will handle passing its OutputImage to the desired
		// destinations.  By using the Ran instead of the placing the code this
		// _Click routine, any Run, regardless of how initiated, will have the new
		// OutputImage passed to the desired locations.
		private void NextImageButton_Click(object sender, System.EventArgs e)
		{
			if (FrameGrabberRadio.Checked)
				AcqFifoTool.Run();
			else
				ImageFileTool.Run();
		}

		private void PMAlignSetupButton_Click(object sender, System.EventArgs e)
		{
			bool DrawingWasEnabled;
			CogCoordinateAxes axes;
			CogPMAlignTool CurrentTool;
			int i;
	
			cogDisplay1.StaticGraphics.Clear();
			cogDisplay1.InteractiveGraphics.Clear();
			// PMAlign Setup button has been pressed, Entering SettingUp mode.
			if (!SettingUp)
			{
				// If no image then display error and exit
				if (cogDisplay1.Image == null)
				{
					MessageBox.Show("No Image For Training Features");
					return;
				}
				// While setting up PMAlign tools, disable other GUI controls.
				SettingUp = true;
				DisableAll(SettingUpConstants.SettingUpPMAlign);
				DrawingWasEnabled = cogDisplay1.DrawingEnabled;
				cogDisplay1.DrawingEnabled = false;

				// Add PMAlign Pattern regions & origins to display's interactive graphics
				for (i = 0; i < NumPMAlignTools; i++)
				{
					CurrentTool = (CogPMAlignTool)PMAlignTools[i];
					cogDisplay1.InteractiveGraphics.Add((ICogGraphicInteractive)CurrentTool.Pattern.TrainRegion,
						"main",false);
					// Add an origin graphic with tip text to distinguish them
					axes = new CogCoordinateAxes();
					axes.Transform = CurrentTool.Pattern.Origin;
					axes.TipText = "PatMax Pattern Origin " + i.ToString();
					axes.GraphicDOFEnable = CogCoordinateAxesDOFConstants.All &
						~CogCoordinateAxesDOFConstants.Skew;
					axes.Interactive = true;
					// Add a standard VisionPro "manipulable" mouse cursor.
					axes.MouseCursor = CogStandardCursorConstants.ManipulableGraphic;
					axes.XAxisLabel.MouseCursor = CogStandardCursorConstants.ManipulableGraphic;
					axes.YAxisLabel.MouseCursor = CogStandardCursorConstants.ManipulableGraphic;
					cogDisplay1.InteractiveGraphics.Add((ICogGraphicInteractive)axes,"main",false);
				}
				// Re-enable drawing
				cogDisplay1.DrawingEnabled = DrawingWasEnabled;
			}
			else  // OK button has been pressed, completing setup
			{
				SettingUp = false;
				cogDisplay1.InteractiveGraphics.Clear();
				cogDisplay1.StaticGraphics.Clear();

				try
				{
					for (i = 0; i < NumPMAlignTools; i++)
					{
						CurrentTool = (CogPMAlignTool)PMAlignTools[i];
						CurrentTool.Pattern.TrainImage = CurrentTool.InputImage;
						CurrentTool.Pattern.Train();
					}
					PMAlignRun();
					EnableAll(SettingUpConstants.SettingUpPMAlign);
				}
				catch (CogException ce)
				{
					MessageBox.Show("Encountered exception: " + ce.Message);
				}
			}
		}

		private void CalibRunButton_Click(object sender, System.EventArgs e)
		{
			cogDisplay1.StaticGraphics.Clear();
			cogDisplay1.InteractiveGraphics.Clear();
			CalibRun();
		}

		private void BlobRunButton_Click(object sender, System.EventArgs e)
		{
			cogDisplay1.InteractiveGraphics.Clear();
			cogDisplay1.StaticGraphics.Clear();
			BlobRun();
		}

		private void RunAllButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				cogDisplay1.DrawingEnabled = false;
				RunningAll = true;

				if (FrameGrabberRadio.Checked)
					AcqFifoTool.Run();
				else
					ImageFileTool.Run();
				CalibRun();
				BlobRun();
			}
			catch (CogException ce)
			{
				MessageBox.Show("Encountered exception: " + ce.Message);
			}
			finally
			{
				RunningAll = false;
				cogDisplay1.DrawingEnabled = true;
			}
		}

		private void CalibRun()
		{
			// Run the Calib tool to stuff the calibrated space into the image
			try
			{
				CalibNPointTool.Run();
				if (CalibNPointTool.RunStatus.Result == CogToolResultConstants.Error)
					MessageBox.Show(CalibNPointTool.RunStatus.Message);
			}
			catch (CogException ce)
			{
				MessageBox.Show("Encountered exception: " + ce.Message);
			}

		}
		// Runs the Blob tool
		private void BlobRun()
		{
			BlobTool.Run();
			if (BlobTool.RunStatus.Result == CogToolResultConstants.Error)
				MessageBox.Show(BlobTool.RunStatus.Message);
		}


		// Handler for when user selects a new PMAlign tool to edit.
		private void PMAlignComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PMAlignSelected = ((ComboBox)sender).SelectedIndex;
			cogPMAlignEdit1.Subject = (CogPMAlignTool)PMAlignTools[PMAlignSelected];
		}

		// Handler when the user changes the number of PMAlign tools used for 
		// calibration points.
		private void NumPMAlignToolsUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int CurrentPMAlignToolCount = (int)NumPMAlignToolsUpDown.Value;
			if (CurrentPMAlignToolCount == NumPMAlignTools)
				return;

			// remove display graphics
			cogDisplay1.InteractiveGraphics.Clear();
			cogDisplay1.StaticGraphics.Clear();

			// update CalibNPoint Tool to have the correct number of points
			CalibNPointTool.Calibration.NumPoints = CurrentPMAlignToolCount;

			// Add new PMAlign tools as needed
			while (CurrentPMAlignToolCount > NumPMAlignTools)
				CreateNewPMAlignTool(NumPMAlignTools++);

			// Remove PMAlign tools if not needed
			while (CurrentPMAlignToolCount < NumPMAlignTools)
			{
				// remove ran event handler
				((CogPMAlignTool)PMAlignTools[NumPMAlignTools-1]).Ran -= new EventHandler(PMAlign_Ran);
				// remove tool from array
				PMAlignTools.RemoveAt(NumPMAlignTools -1);
				NumPMAlignTools--;
			}

			// Handle case where we removed the currently selected
			// PMAlign tool in the edit control

			if (PMAlignSelected >= NumPMAlignTools)
			{
				PMAlignSelected = NumPMAlignTools -1;
				cogPMAlignEdit1.Subject = (CogPMAlignTool)PMAlignTools[PMAlignSelected];
			}
		}
		
		// Handle a change to the Frame Grabber radio button.
		private void FrameGrabberRadio_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FrameGrabberRadio.Checked)
			{
				OpenFileButton.Text = "Live Video";
				NextImageButton.Text = "New Image";
			}
			else
			{
				OpenFileButton.Text = "Open File";
				NextImageButton.Text = "Next Image";
			}

		}

		// Handle a change to the Frame Grabber radio button.
		private void ImageFileRadio_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!ImageFileRadio.Checked)
			{
				OpenFileButton.Text = "Live Video";
				NextImageButton.Text = "New Image";
			}
			else
			{
				OpenFileButton.Text = "Open File";
				NextImageButton.Text = "Next Image";
			}

		
		}
		// This handler executes after the Image File tool is run.
		private void ImageFileTool_Ran(object sender, EventArgs e)
		{
			cogDisplay1.StaticGraphics.Clear();
			cogDisplay1.InteractiveGraphics.Clear();
			cogDisplay1.Image = ImageFileTool.OutputImage;
			cogDisplay1.Fit(true);
			for (int i = 0; i < NumPMAlignTools; i++)
				((CogPMAlignTool)PMAlignTools[i]).InputImage = (CogImage8Grey)ImageFileTool.OutputImage;
			CalibNPointTool.InputImage = ImageFileTool.OutputImage;
		    // Note: Set blob's input image when calibration tool runs.
		}

		// This handler executes after the Image File tool is run.
		// Note that we invoke the garbage collector every 5th acquisition.
		private void AcqFifoTool_Ran(object sender, EventArgs e)
		{
			numAcqs++;
			if (numAcqs > 4)
			{
				GC.Collect();
				numAcqs = 0;
			}

			cogDisplay1.StaticGraphics.Clear();
			cogDisplay1.InteractiveGraphics.Clear();
			cogDisplay1.Image = AcqFifoTool.OutputImage;
			cogDisplay1.Fit(true);
			for (int i = 0; i < NumPMAlignTools; i++)
				((CogPMAlignTool)PMAlignTools[i]).InputImage = (CogImage8Grey)AcqFifoTool.OutputImage;
			CalibNPointTool.InputImage = AcqFifoTool.OutputImage;
			
			// Note: Set blob's input image when calibration tool runs.
		}

		// Each PMAlign tool supplies one uncalibrated point to the N-Point calibration
		// tool.  For more information, see comments at top of file.
		void PMAlignRun()
		{
			CogPMAlignTool CurrentTool;
			CogPMAlignResult Result;
			bool DrawingWasEnabled;
			DrawingWasEnabled = cogDisplay1.DrawingEnabled;
			try
			{
				cogDisplay1.DrawingEnabled = false;
				RunningPMAlignTools = true;
				for (int i = 0; i < NumPMAlignTools; i++)
				{
					CurrentTool = (CogPMAlignTool)PMAlignTools[i];
					CurrentTool.Run();
					if (CurrentTool.RunStatus.Result == CogToolResultConstants.Error)
						MessageBox.Show(CurrentTool.RunStatus.Message);
					else
					{
						if (CurrentTool.Results.Count == 0)
							MessageBox.Show("Feature number " + i + " not found.");
						else if (CurrentTool.Results.Count > 1)
							MessageBox.Show("Feature number " + i + " found too many times.");
						else
						{
							// update corresponding point in calibration tool.
							// NOTE: THIS WILL UNCALIBRATE CALIBRATION TOOL.
							Result = CurrentTool.Results[0];
							CalibNPointTool.Calibration.SetUncalibratedPointX(i,
								Result.GetPose().TranslationX);
							CalibNPointTool.Calibration.SetUncalibratedPointY(i,
								Result.GetPose().TranslationY);
							cogDisplay1.InteractiveGraphics.Add(Result.CreateResultGraphics(CogPMAlignResultGraphicConstants.Origin),
								"main",false);
						}
					}
				}
			}
			catch (CogException ce)
			{
				MessageBox.Show("Encountered exception: " + ce.Message);
			}
			finally
			{
				RunningPMAlignTools = false;
				cogDisplay1.DrawingEnabled = DrawingWasEnabled;
			}
		}

		// This handler is used to take care of the case where the user runs a
		// PMAlign tool directly from the PMAlign Tool edit control.
		private void PMAlign_Ran(object sender, EventArgs e)
		{
			CogPMAlignTool RanTool = (CogPMAlignTool)sender;

			if (RunningPMAlignTools) return;

			cogDisplay1.InteractiveGraphics.Clear();
			cogDisplay1.StaticGraphics.Clear();
			if (RanTool.Results != null)
			{
				if (RanTool.Results.Count == 1)
				{
					// update corresponding point in calibration tool.
					// NOTE: THIS WILL UNCALIBRATE CALIBRATION TOOL.
					CalibNPointTool.Calibration.SetUncalibratedPointX(PMAlignSelected,
						RanTool.Results[0].GetPose().TranslationX);
					CalibNPointTool.Calibration.SetUncalibratedPointY(PMAlignSelected,
						RanTool.Results[0].GetPose().TranslationY);
				}
			}
		}

		// Handler for the PMAlign Run button.
		private void PMAlignRunButton_Click(object sender, System.EventArgs e)
		{
			cogDisplay1.InteractiveGraphics.Clear();
			cogDisplay1.StaticGraphics.Clear();
			PMAlignRun();
		}

		// Handler for the Calib Setup button.
		private void CalibSetupButton_Click(object sender, System.EventArgs e)
		{
			cogDisplay1.InteractiveGraphics.Clear();
			cogDisplay1.StaticGraphics.Clear();
    
			CalibNPointTool.CalibrationImage = CalibNPointTool.InputImage;
    
			// Unfortunately, there is no way for the demo to guess calibrated points.
			// The user will have to enter calibrated points in the point grid on the
			// N-Point calibration tab before clicking calibrate.
			// See comment at top of file for geometric information for bracket if you are
			// running on that image.

			try
			{
				CalibNPointTool.Calibration.Calibrate();
			}
			catch (CogException ce)
			{
				MessageBox.Show("Encountered exception: " + ce.Message);
			}
		}

		// This is the change event handler for the Calibration operator.  It is 
		// executed whenever calibration goes from trained to untrained (or vice versa).
		private void Calib_Changed(object sender, CogChangedEventArgs e)
		{
			CogCoordinateAxes axes = new CogCoordinateAxes();
			if ((e.StateFlags & CogCalibNPointToNPoint.SfCalibrated) > 0)
			{
				cogDisplay1.InteractiveGraphics.Clear();
				cogDisplay1.StaticGraphics.Clear();
				// Set application state based on calibrated state (label, graphics, etc)

				if (CalibNPointTool.Calibration.Calibrated)
				{
					CalibratedLabel.Text = "Calibrated";
					CalibratedLabel.ForeColor = System.Drawing.Color.Green;
					
					// When going calibrated, show calibration axes in display on demo tab
					axes.Transform = (CogTransform2DLinear)CalibNPointTool.Calibration.GetComputedUncalibratedFromCalibratedTransform();
					axes.Color = CogColorConstants.Green;
					axes.XAxisLabel.Color = CogColorConstants.Green;
					axes.YAxisLabel.Color = CogColorConstants.Green;
					axes.DisplayedXAxisLength = 50;
					cogDisplay1.InteractiveGraphics.Add((ICogGraphicInteractive)axes,"main",false);
				}
				else
				{
					// When uncalibrating, set blob's input to null to prevent it from running
					// on old calibration/image data
					BlobTool.InputImage = null;
					CalibratedLabel.Text = "Uncalibrated";
					CalibratedLabel.ForeColor = System.Drawing.Color.Red;
				}
			}

		}

		// Handler for Blob Tool ran event. If Blob results have changed then update the 
		// Count & Region graphic.
		private void BlobTool_Ran(object sender, EventArgs e)
		{
			CogGraphicInteractiveCollection ResultGraphics = new CogGraphicInteractiveCollection();
			if (!RunningAll)
			{
				cogDisplay1.InteractiveGraphics.Clear(); 
				cogDisplay1.StaticGraphics.Clear();
			}
			if (BlobTool.Results == null)
				BlobCountText.Text = "N/A";
			else
			{
				BlobCountText.Text = BlobTool.Results.GetBlobs(true).Count.ToString();
				BlobCountText.Refresh();
				foreach (CogBlobResult BlobResult in BlobTool.Results.GetBlobs(true))
					ResultGraphics.Add(BlobResult.CreateResultGraphics(CogBlobResultGraphicConstants.Boundary | 
						CogBlobResultGraphicConstants.TipText));
				cogDisplay1.InteractiveGraphics.AddList(ResultGraphics,"main",false);
			}
		}

		private void CalibNPointTool_Ran(object sender, EventArgs e)
		{
			// Make blob run in calibrated space. Running the calibration tool will
			// add the calibrated space to the image for blob to run.
			// NOTE: its input region, if any, will need to setup in calibrated space AFTER calibrating
    
			CogCoordinateAxes axes;
			BlobTool.InputImage = (CogImage8Grey)CalibNPointTool.OutputImage;
    
			if (!RunningAll) 
			{
				cogDisplay1.StaticGraphics.Clear();
				cogDisplay1.InteractiveGraphics.Clear();
			}
            
			// if we are calibrated, then show the calibrated space on run
			if (CalibNPointTool.Calibration.Calibrated) 
			{
				axes = new CogCoordinateAxes();
				axes.Transform = (CogTransform2DLinear)CalibNPointTool.Calibration.GetComputedUncalibratedFromCalibratedTransform();
				axes.Color = CogColorConstants.Green;
				axes.XAxisLabel.Color = CogColorConstants.Green;
				axes.YAxisLabel.Color = CogColorConstants.Green;
				axes.DisplayedXAxisLength = 50;
            
				cogDisplay1.InteractiveGraphics.Add(axes,"main",false);
			}

		}
	}
}
