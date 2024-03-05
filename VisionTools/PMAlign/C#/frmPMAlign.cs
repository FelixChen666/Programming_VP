using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

//*******************************************************************************
//*******************************************************************************
//Copyright (C) 2004 Cognex Corporation
//
//Subject to Cognex Corporation's terms and conditions and license agreement,
//you are authorized to use and modify this source code in any way you find
//useful, provided the Software and/or the modified Software is used solely in
//conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
//and agree that Cognex has no warranty, obligations or liability for your use
//of the Software.
//*******************************************************************************
// This sample program is designed to illustrate certain VisionPro features or 
// techniques in the simplest way possible. It is not intended as the framework 
// for a complete application. In particular, the sample program may not provide
// proper error handling, event handling, cleanup, repeatability, and other 
// mechanisms that a commercial quality application requires.

using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.PatInspect;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.Exceptions;
namespace SamplePMAlign
{
	public class frmPatInspSamp : System.Windows.Forms.Form
	{

		#region " Windows Form Designer generated code "

		public frmPatInspSamp() : base()
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}

		//Form overrides dispose to clean up the component list.
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if ((components != null)) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		internal System.Windows.Forms.TextBox InfoTxt;
		internal System.Windows.Forms.TabPage TabPage1;
		internal System.Windows.Forms.TabPage FrameGrabber;
		internal Cognex.VisionPro.Display.CogDisplay CogDisplay1;
		
        private System.Windows.Forms.RadioButton optImageAcquisitionOptionFrameGrabber;
		private System.Windows.Forms.RadioButton optImageAcquisitionOptionImageFile;
		
		private System.Windows.Forms.Button cmdPatMaxSetupCommand;

		private System.Windows.Forms.TextBox txtPatMaxScoreValue;
		private System.Windows.Forms.Label Label1;
		private System.Windows.Forms.Button cmdImageAcquisitionLiveOrOpenCommand;
		
		private System.Windows.Forms.Button cmdImageAcquisitionNewImageCommand;
        private System.Windows.Forms.OpenFileDialog ImageAcquisitionCommonDialog;
        private System.Windows.Forms.TabPage TabPage2;
        private Cognex.VisionPro.ImageFile.CogImageFileEditV2 CogImageFileEdit1;
        private System.Windows.Forms.TabPage TabPage3;
        private Cognex.VisionPro.PMAlign.CogPMAlignEditV2 CogPMAlignEdit1;
        private System.Windows.Forms.TabControl VProSampleAppTab;

        private System.Windows.Forms.Button cmdPatMaxRunCommand;
		internal System.Windows.Forms.GroupBox frmPatMax;
		internal Cognex.VisionPro.CogAcqFifoEditV2 CogAcqFifoEdit1;
		internal System.Windows.Forms.GroupBox frmImageAcquisitionFrame;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPatInspSamp));
            this.InfoTxt = new System.Windows.Forms.TextBox();
            this.VProSampleAppTab = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.frmPatMax = new System.Windows.Forms.GroupBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtPatMaxScoreValue = new System.Windows.Forms.TextBox();
            this.cmdPatMaxRunCommand = new System.Windows.Forms.Button();
            this.cmdPatMaxSetupCommand = new System.Windows.Forms.Button();
            this.frmImageAcquisitionFrame = new System.Windows.Forms.GroupBox();
            this.cmdImageAcquisitionNewImageCommand = new System.Windows.Forms.Button();
            this.cmdImageAcquisitionLiveOrOpenCommand = new System.Windows.Forms.Button();
            this.optImageAcquisitionOptionImageFile = new System.Windows.Forms.RadioButton();
            this.optImageAcquisitionOptionFrameGrabber = new System.Windows.Forms.RadioButton();
            this.CogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.CogImageFileEdit1 = new Cognex.VisionPro.ImageFile.CogImageFileEditV2();
            this.FrameGrabber = new System.Windows.Forms.TabPage();
            this.CogAcqFifoEdit1 = new Cognex.VisionPro.CogAcqFifoEditV2();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.CogPMAlignEdit1 = new Cognex.VisionPro.PMAlign.CogPMAlignEditV2();
            this.ImageAcquisitionCommonDialog = new System.Windows.Forms.OpenFileDialog();
            this.VProSampleAppTab.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.frmPatMax.SuspendLayout();
            this.frmImageAcquisitionFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CogDisplay1)).BeginInit();
            this.TabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CogImageFileEdit1)).BeginInit();
            this.FrameGrabber.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CogAcqFifoEdit1)).BeginInit();
            this.TabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CogPMAlignEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // InfoTxt
            // 
            this.InfoTxt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.InfoTxt.Location = new System.Drawing.Point(0, 466);
            this.InfoTxt.Multiline = true;
            this.InfoTxt.Name = "InfoTxt";
            this.InfoTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.InfoTxt.Size = new System.Drawing.Size(768, 112);
            this.InfoTxt.TabIndex = 3;
            // 
            // VProSampleAppTab
            // 
            this.VProSampleAppTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VProSampleAppTab.Controls.Add(this.TabPage1);
            this.VProSampleAppTab.Controls.Add(this.TabPage2);
            this.VProSampleAppTab.Controls.Add(this.FrameGrabber);
            this.VProSampleAppTab.Controls.Add(this.TabPage3);
            this.VProSampleAppTab.Location = new System.Drawing.Point(0, 0);
            this.VProSampleAppTab.Name = "VProSampleAppTab";
            this.VProSampleAppTab.SelectedIndex = 0;
            this.VProSampleAppTab.Size = new System.Drawing.Size(768, 466);
            this.VProSampleAppTab.TabIndex = 4;
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.frmPatMax);
            this.TabPage1.Controls.Add(this.frmImageAcquisitionFrame);
            this.TabPage1.Controls.Add(this.CogDisplay1);
            this.TabPage1.Location = new System.Drawing.Point(4, 22);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Size = new System.Drawing.Size(760, 440);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "VisionPro Demo";
            // 
            // frmPatMax
            // 
            this.frmPatMax.Controls.Add(this.Label1);
            this.frmPatMax.Controls.Add(this.txtPatMaxScoreValue);
            this.frmPatMax.Controls.Add(this.cmdPatMaxRunCommand);
            this.frmPatMax.Controls.Add(this.cmdPatMaxSetupCommand);
            this.frmPatMax.Location = new System.Drawing.Point(0, 168);
            this.frmPatMax.Name = "frmPatMax";
            this.frmPatMax.Size = new System.Drawing.Size(392, 128);
            this.frmPatMax.TabIndex = 2;
            this.frmPatMax.TabStop = false;
            this.frmPatMax.Text = "Pat Max";
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(232, 56);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(48, 32);
            this.Label1.TabIndex = 3;
            this.Label1.Text = "Score";
            // 
            // txtPatMaxScoreValue
            // 
            this.txtPatMaxScoreValue.Location = new System.Drawing.Point(296, 40);
            this.txtPatMaxScoreValue.Multiline = true;
            this.txtPatMaxScoreValue.Name = "txtPatMaxScoreValue";
            this.txtPatMaxScoreValue.Size = new System.Drawing.Size(80, 40);
            this.txtPatMaxScoreValue.TabIndex = 2;
            // 
            // cmdPatMaxRunCommand
            // 
            this.cmdPatMaxRunCommand.Location = new System.Drawing.Point(112, 40);
            this.cmdPatMaxRunCommand.Name = "cmdPatMaxRunCommand";
            this.cmdPatMaxRunCommand.Size = new System.Drawing.Size(104, 48);
            this.cmdPatMaxRunCommand.TabIndex = 1;
            this.cmdPatMaxRunCommand.Text = "Run";
            this.cmdPatMaxRunCommand.Click += new System.EventHandler(this.cmdPatMaxRunCommand_Click);
            // 
            // cmdPatMaxSetupCommand
            // 
            this.cmdPatMaxSetupCommand.Location = new System.Drawing.Point(8, 40);
            this.cmdPatMaxSetupCommand.Name = "cmdPatMaxSetupCommand";
            this.cmdPatMaxSetupCommand.Size = new System.Drawing.Size(96, 48);
            this.cmdPatMaxSetupCommand.TabIndex = 0;
            this.cmdPatMaxSetupCommand.Text = "Setup";
            this.cmdPatMaxSetupCommand.Click += new System.EventHandler(this.cmdPatMaxSetupCommand_Click);
            // 
            // frmImageAcquisitionFrame
            // 
            this.frmImageAcquisitionFrame.Controls.Add(this.cmdImageAcquisitionNewImageCommand);
            this.frmImageAcquisitionFrame.Controls.Add(this.cmdImageAcquisitionLiveOrOpenCommand);
            this.frmImageAcquisitionFrame.Controls.Add(this.optImageAcquisitionOptionImageFile);
            this.frmImageAcquisitionFrame.Controls.Add(this.optImageAcquisitionOptionFrameGrabber);
            this.frmImageAcquisitionFrame.Location = new System.Drawing.Point(8, 8);
            this.frmImageAcquisitionFrame.Name = "frmImageAcquisitionFrame";
            this.frmImageAcquisitionFrame.Size = new System.Drawing.Size(384, 136);
            this.frmImageAcquisitionFrame.TabIndex = 1;
            this.frmImageAcquisitionFrame.TabStop = false;
            this.frmImageAcquisitionFrame.Text = "Image Acquisition";
            // 
            // cmdImageAcquisitionNewImageCommand
            // 
            this.cmdImageAcquisitionNewImageCommand.Location = new System.Drawing.Point(280, 48);
            this.cmdImageAcquisitionNewImageCommand.Name = "cmdImageAcquisitionNewImageCommand";
            this.cmdImageAcquisitionNewImageCommand.Size = new System.Drawing.Size(75, 40);
            this.cmdImageAcquisitionNewImageCommand.TabIndex = 3;
            this.cmdImageAcquisitionNewImageCommand.Text = "Next Image";
            this.cmdImageAcquisitionNewImageCommand.Click += new System.EventHandler(this.cmdImageAcquisitionNewImageCommand_Click);
            // 
            // cmdImageAcquisitionLiveOrOpenCommand
            // 
            this.cmdImageAcquisitionLiveOrOpenCommand.Location = new System.Drawing.Point(152, 48);
            this.cmdImageAcquisitionLiveOrOpenCommand.Name = "cmdImageAcquisitionLiveOrOpenCommand";
            this.cmdImageAcquisitionLiveOrOpenCommand.Size = new System.Drawing.Size(75, 40);
            this.cmdImageAcquisitionLiveOrOpenCommand.TabIndex = 2;
            this.cmdImageAcquisitionLiveOrOpenCommand.Text = "Open File";
            this.cmdImageAcquisitionLiveOrOpenCommand.Click += new System.EventHandler(this.cmdImageAcquisitionLiveOrOpenCommand_Click);
            // 
            // optImageAcquisitionOptionImageFile
            // 
            this.optImageAcquisitionOptionImageFile.Checked = true;
            this.optImageAcquisitionOptionImageFile.Location = new System.Drawing.Point(24, 64);
            this.optImageAcquisitionOptionImageFile.Name = "optImageAcquisitionOptionImageFile";
            this.optImageAcquisitionOptionImageFile.Size = new System.Drawing.Size(104, 24);
            this.optImageAcquisitionOptionImageFile.TabIndex = 1;
            this.optImageAcquisitionOptionImageFile.TabStop = true;
            this.optImageAcquisitionOptionImageFile.Text = "Image File";
            this.optImageAcquisitionOptionImageFile.CheckedChanged += new System.EventHandler(this.optImageAcquisitionOptionImageFile_CheckedChanged);
            // 
            // optImageAcquisitionOptionFrameGrabber
            // 
            this.optImageAcquisitionOptionFrameGrabber.Location = new System.Drawing.Point(24, 32);
            this.optImageAcquisitionOptionFrameGrabber.Name = "optImageAcquisitionOptionFrameGrabber";
            this.optImageAcquisitionOptionFrameGrabber.Size = new System.Drawing.Size(104, 24);
            this.optImageAcquisitionOptionFrameGrabber.TabIndex = 0;
            this.optImageAcquisitionOptionFrameGrabber.Text = "Frame Grabber";
            this.optImageAcquisitionOptionFrameGrabber.CheckedChanged += new System.EventHandler(this.optImageAcquisitionOptionFrameGrabber_CheckedChanged);
            // 
            // CogDisplay1
            // 
            this.CogDisplay1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CogDisplay1.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.CogDisplay1.ColorMapLowerRoiLimit = 0D;
            this.CogDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.CogDisplay1.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.CogDisplay1.ColorMapUpperRoiLimit = 1D;
            this.CogDisplay1.Location = new System.Drawing.Point(400, 16);
            this.CogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.CogDisplay1.MouseWheelSensitivity = 1D;
            this.CogDisplay1.Name = "CogDisplay1";
            this.CogDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("CogDisplay1.OcxState")));
            this.CogDisplay1.Size = new System.Drawing.Size(357, 421);
            this.CogDisplay1.TabIndex = 0;
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.CogImageFileEdit1);
            this.TabPage2.Location = new System.Drawing.Point(4, 22);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Size = new System.Drawing.Size(760, 440);
            this.TabPage2.TabIndex = 2;
            this.TabPage2.Text = "Image File";
            // 
            // CogImageFileEdit1
            // 
            this.CogImageFileEdit1.AllowDrop = true;
            this.CogImageFileEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CogImageFileEdit1.Location = new System.Drawing.Point(0, 0);
            this.CogImageFileEdit1.MinimumSize = new System.Drawing.Size(489, 0);
            this.CogImageFileEdit1.Name = "CogImageFileEdit1";
            this.CogImageFileEdit1.OutputHighLight = System.Drawing.Color.Lime;
            this.CogImageFileEdit1.Size = new System.Drawing.Size(760, 440);
            this.CogImageFileEdit1.SuspendElectricRuns = false;
            this.CogImageFileEdit1.TabIndex = 0;
            // 
            // FrameGrabber
            // 
            this.FrameGrabber.Controls.Add(this.CogAcqFifoEdit1);
            this.FrameGrabber.Location = new System.Drawing.Point(4, 22);
            this.FrameGrabber.Name = "FrameGrabber";
            this.FrameGrabber.Size = new System.Drawing.Size(760, 440);
            this.FrameGrabber.TabIndex = 1;
            this.FrameGrabber.Text = "FrameGrabber";
            // 
            // CogAcqFifoEdit1
            // 
            this.CogAcqFifoEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CogAcqFifoEdit1.Location = new System.Drawing.Point(0, 0);
            this.CogAcqFifoEdit1.MinimumSize = new System.Drawing.Size(489, 0);
            this.CogAcqFifoEdit1.Name = "CogAcqFifoEdit1";
            this.CogAcqFifoEdit1.Size = new System.Drawing.Size(760, 440);
            this.CogAcqFifoEdit1.SuspendElectricRuns = false;
            this.CogAcqFifoEdit1.TabIndex = 0;
            // 
            // TabPage3
            // 
            this.TabPage3.Controls.Add(this.CogPMAlignEdit1);
            this.TabPage3.Location = new System.Drawing.Point(4, 22);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Size = new System.Drawing.Size(760, 440);
            this.TabPage3.TabIndex = 3;
            this.TabPage3.Text = "PatMax";
            // 
            // CogPMAlignEdit1
            // 
            this.CogPMAlignEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CogPMAlignEdit1.Location = new System.Drawing.Point(0, 0);
            this.CogPMAlignEdit1.MinimumSize = new System.Drawing.Size(489, 0);
            this.CogPMAlignEdit1.Name = "CogPMAlignEdit1";
            this.CogPMAlignEdit1.Size = new System.Drawing.Size(760, 440);
            this.CogPMAlignEdit1.SuspendElectricRuns = false;
            this.CogPMAlignEdit1.TabIndex = 0;
            // 
            // frmPatInspSamp
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(768, 578);
            this.Controls.Add(this.VProSampleAppTab);
            this.Controls.Add(this.InfoTxt);
            this.Name = "frmPatInspSamp";
            this.Text = "PMAlign  Sample";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPatInspSamp_FormClosing);
            this.Load += new System.EventHandler(this.frmPatInspSamp_Load);
            this.VProSampleAppTab.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.frmPatMax.ResumeLayout(false);
            this.frmPatMax.PerformLayout();
            this.frmImageAcquisitionFrame.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CogDisplay1)).EndInit();
            this.TabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CogImageFileEdit1)).EndInit();
            this.FrameGrabber.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CogAcqFifoEdit1)).EndInit();
            this.TabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CogPMAlignEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		#region "Module Level vars"

		CogImageFileTool ImageFileTool;
		CogPMAlignTool PatMaxTool;
		CogAcqFifoTool AcqFifoTool;
		//Flag for "VisionPro Demo" tab indicating that user is currently setting up a
		//tool.  Also used to indicate in live video mode.  If user selects "Setup"
		//then the GUI controls are disabled except for the interactive graphics
		//required for setup as well as the "OK" button used to complete the Setup.

		bool SettingUp;
		// values passed to EnableAll & DisableAll subroutines which
		//indicates what is being setup thus determining which Buttons on the GUI
		//should be left enabled.
		public enum SettingUpConstants : int
		{
			settingUpPatMax,
			settingLiveVideo
		}

		SettingUpConstants settingUpPatMax = SettingUpConstants.settingUpPatMax;
        SettingUpConstants settingLiveVideo = SettingUpConstants.settingLiveVideo;
			#endregion
		
		#region "Form and Controls events"

		private void frmPatInspSamp_Load(System.Object sender, System.EventArgs e)
		{

			SettingUp = false;
			InfoTxt.Text = "This sample demonstrates the use of PMAlign to train and locate a " + "pattern in a user provided image." + Environment.NewLine + "The user should execute the following steps:" + Environment.NewLine + "1. Select an image source: either an image file or a frame grabber." + Environment.NewLine + "2. Grab an image from the image source." + Environment.NewLine + "3. Click 'Setup' in the PatMax frame.  Select a training region. Hit 'OK'." + Environment.NewLine + "4. Click 'Run' in the PatMax frame to see the location result and score." + Environment.NewLine + "5. Click 'Next Image' followed by 'Run' to locate the pattern on a subsequent image." + Environment.NewLine + "Note that execution parameters can be changed by selecting the appropriate tab and " + "modifying the provided values.";


			//Set reference to CogImageFileTool created by Edit Control
			//The Image File Edit Control creates its subject when its AutoCreateTool property is True
			ImageFileTool = CogImageFileEdit1.Subject;
			ImageFileTool.Ran += ImageFileTool_Ran;
			//Set reference to CogAcqFifoTool created by Edit Control
			//The Acq Fifo Edit Control creates its subject when its AutoCreateTool property is True
			AcqFifoTool = CogAcqFifoEdit1.Subject;
			AcqFifoTool.Ran += AcqFifoTool_Ran;
			//Operator will be Nothing if no Frame Grabber is available.  Disable the Frame Grabber
			//option on the "VisionPro Demo" tab if no frame grabber available.
			if (AcqFifoTool.Operator == null) {
				optImageAcquisitionOptionFrameGrabber.Enabled = false;
			}

			//Initialize the Dialog box for the "Open File" button on the "VisionPro Demo" tab.
			ImageAcquisitionCommonDialog.Filter = ImageFileTool.Operator.FilterText;
			ImageAcquisitionCommonDialog.CheckFileExists = true;
			ImageAcquisitionCommonDialog.ReadOnlyChecked = true;

			//AutoCreateTool for the PMAlign edit control is False, therefore, we must create
			//a PMAlign tool and set the subject of the control to reference the new tool.
			PatMaxTool = new CogPMAlignTool();
			PatMaxTool.Changed += PatMaxTool_Changed;
			CogPMAlignEdit1.Subject = PatMaxTool;

			//Change the default Train Region to center of a 640x480 image & change the DOFs
			//so that Skew is not enabled.  Note - TrainRegion is of type ICogRegion, therefore,
			//we must use a CogRectangleAffine reference in order to call CogRectangleAffine
			//properties.
			CogRectangleAffine PatMaxTrainRegion = default(CogRectangleAffine);
			PatMaxTrainRegion = PatMaxTool.Pattern.TrainRegion as CogRectangleAffine;
			if ((PatMaxTrainRegion != null)) {
				PatMaxTrainRegion.SetCenterLengthsRotationSkew(320, 240, 100, 100, 0, 0);
				PatMaxTrainRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position | CogRectangleAffineDOFConstants.Rotation | CogRectangleAffineDOFConstants.Size;
			}

			//Create a SearchRegion that uses the entire image (assumes 640x480)
			//Note that by default the SearchRegion is Nothing and PMAlign will search the entire
			//image anyway.  This is added for sample code purposes & to graphically show that the
			//entire image is being used.
			CogRectangle PatMaxSearchRegion = new CogRectangle();
			PatMaxTool.SearchRegion = PatMaxSearchRegion;
			PatMaxSearchRegion.SetCenterWidthHeight(320, 240, 640, 480);
			PatMaxSearchRegion.GraphicDOFEnable = CogRectangleDOFConstants.Position | CogRectangleDOFConstants.Size;
			PatMaxSearchRegion.Interactive = true;
		}



		private void cmdImageAcquisitionLiveOrOpenCommand_Click(System.Object sender, System.EventArgs e)
		{
			//Clear graphics, assuming a new image will be in the display once user
			//completes either Live Video or Open File operation, therefore, graphics
			//will be out of sync.
			CogDisplay1.StaticGraphics.Clear();
			CogDisplay1.InteractiveGraphics.Clear();

			//"Live Video"  & "Stop Live" button when Frame Grabber option is selected.
			//Using our EnableAll & DisableAll subroutine to force the user stop live
			//video before doing anything else.
			if (optImageAcquisitionOptionFrameGrabber.Checked == true) {
				if (CogDisplay1.LiveDisplayRunning) {
					CogDisplay1.StopLiveDisplay();
					EnableAll(settingLiveVideo);
					//Run the AcqFifoTool so that all of the sample app images get the last
					//image from Live Video (see AcqFifoTool_PostRun)
					AcqFifoTool.Run();
				} else if ((AcqFifoTool.Operator != null)) {
					CogDisplay1.StartLiveDisplay(AcqFifoTool.Operator, false);
					DisableAll(settingLiveVideo);
				}

			} else {
				//"Open File" button when image file option is selected
				//DrawingEnabled is used to simply hide the image while the Fit is performed.
				//This prevents the image from being diplayed at the initial zoom factor
				//prior to fit being called.
				try {
					DialogResult result = ImageAcquisitionCommonDialog.ShowDialog();
					if (result != System.Windows.Forms.DialogResult.Cancel) {
						ImageFileTool.Operator.Open(ImageAcquisitionCommonDialog.FileName, CogImageFileModeConstants.Read);
						CogDisplay1.DrawingEnabled = false;
						ImageFileTool.Run();
						CogDisplay1.Fit(true);
						CogDisplay1.DrawingEnabled = true;
					}
				} catch (CogException cogex) {
					MessageBox.Show("Following Specific Cognex Error Occured:" + cogex.Message);
				} catch (Exception ex) {
					MessageBox.Show("Following Error Occured:" + ex.Message);
				}
			}
		}
		private void cmdImageAcquisitionNewImageCommand_Click(System.Object sender, System.EventArgs e)
		{
			if (optImageAcquisitionOptionFrameGrabber.Checked == true) {
				AcqFifoTool.Run();
			} else {
				ImageFileTool.Run();
			}
		}

		private void cmdPatMaxRunCommand_Click(System.Object sender, System.EventArgs e)
		{
			CogDisplay1.InteractiveGraphics.Clear();
			CogDisplay1.StaticGraphics.Clear();
			PatMaxTool.Run();
			if ((PatMaxTool.RunStatus.Exception != null)) {
				MessageBox.Show(PatMaxTool.RunStatus.Exception.Message, "PatMax Run Error");
			}
		}


		private void optImageAcquisitionOptionFrameGrabber_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			if (optImageAcquisitionOptionFrameGrabber.Checked == true) {
				cmdImageAcquisitionLiveOrOpenCommand.Text = "Live Video";
				cmdImageAcquisitionNewImageCommand.Text = "New Image";
			} else {
				cmdImageAcquisitionLiveOrOpenCommand.Text = "Open File";
				cmdImageAcquisitionNewImageCommand.Text = "Next Image";
			}
		}

		private void optImageAcquisitionOptionImageFile_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			if (optImageAcquisitionOptionImageFile.Checked == true) {
				cmdImageAcquisitionLiveOrOpenCommand.Text = "Open File";
				cmdImageAcquisitionNewImageCommand.Text = "Next Image";
			} else {
				cmdImageAcquisitionLiveOrOpenCommand.Text = "Live Video";
				cmdImageAcquisitionNewImageCommand.Text = "New Image";
			}
		}

		private void cmdPatMaxSetupCommand_Click(System.Object sender, System.EventArgs e)
		{
			//PatMax Setup button has been pressed, Entering SettingUp mode.
			if (!SettingUp) {
				//Copy InputImage to TrainImage, If no ImputImage then display an
				//error message
				if (PatMaxTool.InputImage == null) {
					MessageBox.Show("No InputImage available for setup.", "PatMax Setup Error");
					return;
				}
				PatMaxTool.Pattern.TrainImage = PatMaxTool.InputImage;
				//While setting up PMAlign, disable other GUI controls.
				SettingUp = true;
				DisableAll(settingUpPatMax);
				//Add TrainRegion to display's interactive graphics
				//Add SearchRegion to display's static graphics for display only.
				CogDisplay1.InteractiveGraphics.Clear();
				CogDisplay1.StaticGraphics.Clear();
				CogDisplay1.InteractiveGraphics.Add(PatMaxTool.Pattern.TrainRegion as ICogGraphicInteractive, "test", false);
				if ((PatMaxTool.SearchRegion != null)) {
					CogDisplay1.StaticGraphics.Add(PatMaxTool.SearchRegion as ICogGraphic, "test");
				}

			//OK has been pressed, completing Setup.
			} else {
				SettingUp = false;
				CogDisplay1.InteractiveGraphics.Clear();
				CogDisplay1.StaticGraphics.Clear();
				//Make sure we catch errors from Train, since they are likely.  For example,
				//No InputImage, No Pattern Features, etc.
				try {
					PatMaxTool.Pattern.Train();
				} catch (CogException cogex) {
					MessageBox.Show("Following Specific Cognex Error Occured:" + cogex.Message);

				} catch (Exception ex) {
					MessageBox.Show(ex.Message, "PatMax Setup Error");
				}
				EnableAll(settingUpPatMax);
			}
		}

		#endregion
		#region "Module Level Routines"
		//Disable GUI controls when forcing the user to complete a task before moving on
		//to something new.  Example, Setting up PMAlign.

		private void DisableAll(SettingUpConstants butThis)
		{
			//Disable all of the frames (Disables controls within frame)
			frmImageAcquisitionFrame.Enabled = false;
			frmPatMax.Enabled = false;
			//Disable all of the tabs except "VisionPro Demo" tab.
			VProSampleAppTab.TabPages[1].Enabled = false;
			VProSampleAppTab.TabPages[2].Enabled = false;
			VProSampleAppTab.TabPages[3].Enabled = false;
			//Based on what the user is doing, Re-enable appropriate frame and disable
			//specific controls within the frame.
			if (butThis == settingUpPatMax) {
				frmPatMax.Enabled = true;
				cmdPatMaxSetupCommand.Text = "OK";
				cmdPatMaxRunCommand.Enabled = false;
			} else if (butThis == settingLiveVideo) {
				frmImageAcquisitionFrame.Enabled = true;
				cmdImageAcquisitionLiveOrOpenCommand.Text = "Stop Live";
				cmdImageAcquisitionNewImageCommand.Enabled = false;
				optImageAcquisitionOptionFrameGrabber.Enabled = false;
				optImageAcquisitionOptionImageFile.Enabled = false;
			}
		}
		//Enable all of the GUI controls when done a task.  Example, done setting up PMAlign.
		private void EnableAll(SettingUpConstants butThis)
		{
			frmImageAcquisitionFrame.Enabled = true;
			frmPatMax.Enabled = true;
			VProSampleAppTab.TabPages[1].Enabled = true;
			VProSampleAppTab.TabPages[2].Enabled = true;
			VProSampleAppTab.TabPages[3].Enabled = true;
			if (butThis == settingUpPatMax) {
				cmdPatMaxSetupCommand.Text = "Setup";
				cmdPatMaxRunCommand.Enabled = true;
			} else if (butThis == settingLiveVideo) {
				cmdImageAcquisitionLiveOrOpenCommand.Text = "Live Video";
				cmdImageAcquisitionNewImageCommand.Enabled = true;
				optImageAcquisitionOptionFrameGrabber.Enabled = true;
				optImageAcquisitionOptionImageFile.Enabled = true;
			}
		}
		#endregion
		#region "Cognex Objects Events"


		//Pass AcqFifo OutputImage to the PatMax tool & the Display on "VisionPro" tab.
		//Also, pass OutputImage to the InputImage of ImageFile tool so that this
		//sample application can be used to Record a image file from frame grabber.
		// Handles AcqFifoTool.Ran

		int static_AcqFifoTool_Ran_numacqs;
		private void AcqFifoTool_Ran(object sender, System.EventArgs e)
		{
			CogDisplay1.InteractiveGraphics.Clear();
			CogDisplay1.StaticGraphics.Clear();
			CogDisplay1.Image = AcqFifoTool.OutputImage;
			PatMaxTool.InputImage = AcqFifoTool.OutputImage as CogImage8Grey;
			ImageFileTool.InputImage = AcqFifoTool.OutputImage;
			// Run the garbage collector to free unused images
			static_AcqFifoTool_Ran_numacqs += 1;
			if (static_AcqFifoTool_Ran_numacqs > 4) {
				GC.Collect();
				static_AcqFifoTool_Ran_numacqs = 0;
			}

		}

		//Handles ImageFileTool.Ran
		private void ImageFileTool_Ran(object sender, System.EventArgs e)
		{
			CogDisplay1.InteractiveGraphics.Clear();
			CogDisplay1.StaticGraphics.Clear();
			CogDisplay1.Image = ImageFileTool.OutputImage;
			PatMaxTool.InputImage = ImageFileTool.OutputImage as CogImage8Grey;
		}
		//If PMAlign results have changed then update the Score & Region graphic.
		//Handles PatMaxTool.Changed
		private void PatMaxTool_Changed(object sender, Cognex.VisionPro.CogChangedEventArgs e)
		{
			//If FunctionalArea And cogFA_Tool_Results Then
			if ((Cognex.VisionPro.Implementation.CogToolBase.SfCreateLastRunRecord 
           | Cognex.VisionPro.Implementation.CogToolBase.SfRunStatus) > 0) {
				CogDisplay1.StaticGraphics.Clear();
				//Note, Results will be nothing if Run failed.
				if (PatMaxTool.Results == null) {
					txtPatMaxScoreValue.Text = "N/A";
				} else if (PatMaxTool.Results.Count > 0) {
					//Passing result does not imply Pattern is found, must check count.
					txtPatMaxScoreValue.Text = PatMaxTool.Results[0].Score.ToString("g3");
					txtPatMaxScoreValue.Refresh();
					CogCompositeShape resultGraphics = default(CogCompositeShape);
					resultGraphics = PatMaxTool.Results[0].CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchRegion);
					CogDisplay1.InteractiveGraphics.Add(resultGraphics, "test", false);
				} else {
					txtPatMaxScoreValue.Text = "N/A";
				}
			}
		}
		#endregion

        private void frmPatInspSamp_FormClosing(object sender, FormClosingEventArgs e)
        {
            PatMaxTool.Changed -= PatMaxTool_Changed;
            if ((PatMaxTool != null))
                PatMaxTool.Dispose();
        }
	}
}
