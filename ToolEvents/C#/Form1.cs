/*******************************************************************************
 Copyright (C) 2004 Cognex Corporation

 Subject to Cognex Corporations terms and conditions and license agreement,
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

 This sample demonstrates how to create a tool, assign it to a tool edit control
 and capture the tool Changed, Ran and Running events. The CogBlobTool and
 the CogBlobEdit control are used for this purpose. The CogImageFileTool will
 automatically load bracket_std.idb that is located in the images directory
 at the start up. The program retrieves an image from the CogImageFileCDB
 and assigns it as an input image of the CogBlobTool. This is done inside
 of the CogBlobTool’s Running event handler.

 This program assumes that you have some knowledge of C# and VisionPro
 programming.

*/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

// Cognex using statements
using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.ImageFile;

namespace ToolEvents
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ToolEventsForm : System.Windows.Forms.Form
	{
		private Cognex.VisionPro.Blob.CogBlobEditV2 cogBlobEdit1;
		private System.Windows.Forms.TextBox DescriptionText;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private CogBlobTool VisionProTool;
		private CogImageFileTool ImageSource;

		public ToolEventsForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			InitializeTools();
		}

		private void InitializeTools()
		{
			String VProLocation;
			ImageSource = new CogImageFileTool();
			// Locate the image file using the environment variable that indicates
			// where VisionPro is installed.  If the environment variable is not set,
			// throw an exception that will be caught by the caller.
			VProLocation = Environment.GetEnvironmentVariable("VPRO_ROOT");
			if ((VProLocation == null) || (VProLocation == ""))
				throw new VPRORootNotSetException();

			VProLocation = VProLocation + "/Images/bracket_std.idb";

			// Open the image file.  If an error occurs when opening the image file
			// it will throw an exception that will be caught by the caller.
			ImageSource.Operator.Open(VProLocation,CogImageFileModeConstants.Read);

			// Create a new Blob Tool
			VisionProTool = new CogBlobTool();
			cogBlobEdit1.Subject = VisionProTool;

			// Connect the tool's event handlers

			// Changed Event handler: Executes whenever the tool has changed.
			VisionProTool.Changed +=new CogChangedEventHandler(VisionProTool_Changed);
			// Running Event handler:  Executes just before the tool runs.
			VisionProTool.Running +=new EventHandler(VisionProTool_Running);
			// Ran Event handler: Executes just after the tool runs.
			VisionProTool.Ran +=new EventHandler(VisionProTool_Ran);

			// Run once at startup
			VisionProTool.Run();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ToolEventsForm));
			this.cogBlobEdit1 = new Cognex.VisionPro.Blob.CogBlobEditV2();
			this.DescriptionText = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.cogBlobEdit1)).BeginInit();
			this.SuspendLayout();
			// 
			// cogBlobEdit1
			// 
			this.cogBlobEdit1.Location = new System.Drawing.Point(8, 8);
            this.cogBlobEdit1.MinimumSize = new System.Drawing.Size(489, 0);
			this.cogBlobEdit1.Name = "cogBlobEdit1";
			this.cogBlobEdit1.Size = new System.Drawing.Size(832, 376);
            this.cogBlobEdit1.SuspendElectricRuns = false;
			this.cogBlobEdit1.TabIndex = 0;
			// 
			// DescriptionText
			// 
			this.DescriptionText.Location = new System.Drawing.Point(8, 400);
			this.DescriptionText.Multiline = true;
			this.DescriptionText.Name = "DescriptionText";
			this.DescriptionText.ReadOnly = true;
			this.DescriptionText.Size = new System.Drawing.Size(832, 48);
			this.DescriptionText.TabIndex = 1;
			this.DescriptionText.Text = @"Sample Description: Shows how tool event handlers can be implemented to do things at various stages of the tool's run.
Sample usage: Switch to the last run input image display, then press the run button on the Blob Edit Control. Note that a new image is used each time the tool is run.";
			// 
			// ToolEventsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(848, 462);
			this.Controls.Add(this.DescriptionText);
			this.Controls.Add(this.cogBlobEdit1);
			this.Name = "ToolEventsForm";
			this.Text = "Tool Events Sample Application";
			((System.ComponentModel.ISupportInitialize)(this.cogBlobEdit1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			// Catch any CogExceptions, display the message, and exit the program.
			try 
			{
				Application.Run(new ToolEventsForm());
			}
			catch (CogException ce)
			{
				MessageBox.Show("Error encountered: " + ce.Message);
			}

		}

		private void VisionProTool_Changed(object sender, CogChangedEventArgs e)
		{
			// To see what has changed, look at the state flags that are contained in 
			// the CogChangedEventArgs object.  The StateFlags property is a bitfield,
			// where each bit represents a single element of the tool that may have changed.  If a bit
			// is asserted, that element in the tool has changed.  Multiple bits may be asserted
			// simultaneously.  To see what has changed, bitwise AND the StateFlags with the static
			// Sf**** members in the tool class.  If it returns a value greater than 0, then that
			// particular element has changed.  

			// In the example below, we are testing to see if the RunStatus property has changed
			// by bitwise AND'ing StateFlags with SfRunStatus.

			// Report error conditions, if any.
			if ((e.StateFlags & CogBlobTool.SfRunStatus) > 0)
			{
				if (VisionProTool.RunStatus.Result == CogToolResultConstants.Error)
					MessageBox.Show(VisionProTool.RunStatus.Message);
			}
		}

		// This routine is called before the VisionPro tool runs
		private void VisionProTool_Running(object sender, EventArgs e)
		{
			// Get input image.
			ImageSource.Run();
			VisionProTool.InputImage = (CogImage8Grey)ImageSource.OutputImage;
		}

		// This routine is called after the VisionPro tool has run
		private void VisionProTool_Ran(object sender, EventArgs e)
		{
			// Perform other tasks like reporting results, running other tools, drawing graphics, etc.
			MessageBox.Show("Tool has run");
		}
	}

	// Create a custom exception that will be thrown when the VPRO_ROOT isn't set.
	class VPRORootNotSetException :  CogException
	{
		public VPRORootNotSetException():base("VPRO_ROOT not set")
		{
		}
		public VPRORootNotSetException(String msg) : base(msg)
		{
		}
	}
}
