// ***************************************************************************
// Copyright (C) 2005 Cognex Corporation
//
// Subject to Cognex Corporation's terms and conditions and license agreement,
// you are authorized to use and modify this source code in any way you find
// useful, provided the Software and/or the modified Software is used solely
// in conjunction with a Cognex Machine Vision System.  Furthermore you
// acknowledge and agree that Cognex has no warranty, obligations or liability
// for your use of the Software.
// ***************************************************************************
// This sample program is designed to illustrate certain VisionPro features or 
// techniques in the simplest way possible. It is not intended as the framework 
// for a complete application. In particular, the sample program may not provide
// proper error handling, event handling, cleanup, repeatability, and other 
// mechanisms that a commercial quality application requires.

// This sample demonstrates how to load a persisted QuickBuild application
// and access the results provided in the user
// result queue. 
//
// The provided .vpp file is configured to use "VPRO_ROOT/Images/pmSample.idb" as the
// source of images and a blob tool.  
//
// To use: If necessary, reconfigure acquisition using QuickBuild.  Then run
// this sample code.  The number of blobs will be displayed in the count text
// box and the Blob tool input image will be displayed in the image display
// control.
//
// This application makes use of the .NET method "Invoke" to move data between
// the Job Thread (worker thread) and the GUI thread.  As described in the
// .NET documenation, Invoke allows a worker thread to tell the GUI thread to
// run a specified method with specified parameters.  This is the .NET
// recommended manner for getting worker threads to update the GUI.  Because we
// use Invoke in this sample, is efficient, but may be a bit complicated for
// those unfamiliar with threading.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using Cognex.VisionPro;
using Cognex.VisionPro.QuickBuild;

namespace EfficientApp
{
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public class Form1 : System.Windows.Forms.Form
  {
    internal System.Windows.Forms.Label Label1;
    internal System.Windows.Forms.TextBox myCountText;
    internal System.Windows.Forms.TextBox SampleTextBox;
    CogJobManager myJobManager;
    CogJob myJob;
    CogJobIndependent myIndependentJob;
    private System.Windows.Forms.Button RunOnceButton;
    private System.Windows.Forms.CheckBox RunContCheckBox;
    private CogRecordDisplay cogRecordDisplay1;

    //		C# is a multi-threaded language, unlike VB6. Because of this, one must be careful
    //		when worker threads interact with the GUI (which is on its own thread). One preferred
    //		way to do this in .NET is to use the InvokeRequired/BeginInvoke() mechanisms.  These 
    //		mechanisms allow worker threads to tell the GUI thread that a given method needs to be
    //		run on the GUI thread.  This insures thread safety on the GUI thread.  Otherwise, bad
    //		things (crashes, etc.) might occur if non-GUI threads tried to update the GUI.
    //
    //		When InvokeRequired is called from a Form's method, it determines if the calling thread
    //		is different from the Form's thread. If so, it returns true which indicates that 
    //		a worker thread wants to post something to the GUI.  Else, it returns false.
    //
    //		If InvokeRequired is true, then the caller is trying to tell the GUI thread to run a  
    //		particular method to post something on the GUI.  To do this, one employs BeginInvoke with
    //		a delegate that contains the address of the particular method to run along with parameters needed
    //		by that method.
    //

    //		Delegates which dictate the signature of the methods that will post to the GUI.
    delegate void UserResultDelegate(object sender, CogJobManagerActionEventArgs e);
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public Form1()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      InitializeJobManager();
      this.Closing += new CancelEventHandler(Form1_Closing);
    }

    private void InitializeJobManager()
    {
      SampleTextBox.Text =
        "This sample demonstrates how to load a persisted QuickBuild application and access " +
        "the results provided in the posted items (a.k.a. as user result queue)." +
        System.Environment.NewLine + System.Environment.NewLine +

        "The sample uses mySavedQB.vpp, which consists of a single Job that executes a " +
        "Blob tool with default parameters using a frame grabber provided image.  " +
        @"The provided .vpp file is configured to use ""VPRO_ROOT\images\pmSample.idb"" as the " +
        "source of images." +
        System.Environment.NewLine + System.Environment.NewLine +
        "To use:  Click the Run button or the Run Continuous button.  " +
        "The number of blobs will be displayed in the count text box " +
        "and the Blob tool input image will be displayed in the image display control.";

      //Depersist the QuickBuild session
      myJobManager = (CogJobManager)CogSerializer.LoadObjectFromFile(
          Environment.GetEnvironmentVariable("VPRO_ROOT") + 
          "\\Samples\\Programming\\QuickBuild\\mySavedQB.vpp");
      myJob = myJobManager.Job(0);
      myIndependentJob = myJob.OwnedIndependent;

      //flush queues
      myJobManager.UserQueueFlush();
      myJobManager.FailureQueueFlush();
      myJob.ImageQueueFlush();
      myIndependentJob.RealTimeQueueFlush();

      // setup event handlers.  These are called when a result packet is available on
      // the User Result Queue or the Real-Time Queue, respectively.
      myJobManager.UserResultAvailable += new CogJobManager.CogUserResultAvailableEventHandler(myJobManager_UserResultAvailable);
    }

    //	If it is called by a worker thread,
    //	InvokeRequired is true, as described above.  When this occurs, a delegate is constructed
    //	which is really a pointer to the method that the GUI thread should call.
    //	BeginInvoke is then called, with this delegate and the Image parameter.
    //	Notice that this subroutine tells the GUI thread to call the same subroutine!  
    //	When the GUI calls this method on its own thread, InvokeRequired will be false and the 
    //	CogRecordDisplay is updated with the info.
    // This method handles the UserResultAvailable Event. The user packet
    // has been configured to contain the blob tool input image, which we retrieve and display.
    private void myJobManager_UserResultAvailable(object sender, CogJobManagerActionEventArgs e)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new UserResultDelegate(myJobManager_UserResultAvailable), new object[] { sender, e });
        return;
      }
      Cognex.VisionPro.ICogRecord tmpRecord;
      Cognex.VisionPro.ICogRecord topRecord = myJobManager.UserResult();

      // check to be sure results are available
      if (topRecord == null) return;

      // Assume that the required "count" record is present, and go get it.
      tmpRecord = topRecord.SubRecords[@"Tools.Item[""CogBlobTool1""].CogBlobTool.Results.GetBlobs().Count"];
      int count = (int)tmpRecord.Content;
      myCountText.Text = count.ToString();

      // Assume that the required "image" record is present, and go get it.
      tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
      tmpRecord = tmpRecord.SubRecords["LastRun"];
      tmpRecord = tmpRecord.SubRecords["Image Source.OutputImage"];
      cogRecordDisplay1.Record = tmpRecord;
      cogRecordDisplay1.Fit(true);
    }


    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.Label1 = new System.Windows.Forms.Label();
      this.myCountText = new System.Windows.Forms.TextBox();
      this.SampleTextBox = new System.Windows.Forms.TextBox();
      this.RunOnceButton = new System.Windows.Forms.Button();
      this.RunContCheckBox = new System.Windows.Forms.CheckBox();
      this.cogRecordDisplay1 = new Cognex.VisionPro.CogRecordDisplay();
      ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).BeginInit();
      this.SuspendLayout();
      // 
      // Label1
      // 
      this.Label1.Location = new System.Drawing.Point(16, 128);
      this.Label1.Name = "Label1";
      this.Label1.Size = new System.Drawing.Size(40, 16);
      this.Label1.TabIndex = 4;
      this.Label1.Text = "Count:";
      // 
      // myCountText
      // 
      this.myCountText.Location = new System.Drawing.Point(40, 152);
      this.myCountText.Name = "myCountText";
      this.myCountText.ReadOnly = true;
      this.myCountText.Size = new System.Drawing.Size(72, 20);
      this.myCountText.TabIndex = 3;
      // 
      // SampleTextBox
      // 
      this.SampleTextBox.Location = new System.Drawing.Point(440, 8);
      this.SampleTextBox.Multiline = true;
      this.SampleTextBox.Name = "SampleTextBox";
      this.SampleTextBox.ReadOnly = true;
      this.SampleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.SampleTextBox.Size = new System.Drawing.Size(248, 192);
      this.SampleTextBox.TabIndex = 5;
      // 
      // RunOnceButton
      // 
      this.RunOnceButton.Location = new System.Drawing.Point(16, 16);
      this.RunOnceButton.Name = "RunOnceButton";
      this.RunOnceButton.Size = new System.Drawing.Size(96, 32);
      this.RunOnceButton.TabIndex = 1;
      this.RunOnceButton.Text = "Run Once";
      this.RunOnceButton.Click += new System.EventHandler(this.RunOnceButton_Click);
      // 
      // RunContCheckBox
      // 
      this.RunContCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
      this.RunContCheckBox.Location = new System.Drawing.Point(16, 56);
      this.RunContCheckBox.Name = "RunContCheckBox";
      this.RunContCheckBox.Size = new System.Drawing.Size(96, 32);
      this.RunContCheckBox.TabIndex = 2;
      this.RunContCheckBox.Text = "Run Continuous";
      this.RunContCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.RunContCheckBox.CheckedChanged += new System.EventHandler(this.RunContCheckBox_CheckedChanged);
      // 
      // cogRecordDisplay1
      // 
      this.cogRecordDisplay1.Location = new System.Drawing.Point(129, 11);
      this.cogRecordDisplay1.Name = "cogRecordDisplay1";
      this.cogRecordDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogRecordDisplay1.OcxState")));
      this.cogRecordDisplay1.Size = new System.Drawing.Size(305, 203);
      this.cogRecordDisplay1.TabIndex = 6;
      // 
      // Form1
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(720, 238);
      this.Controls.Add(this.cogRecordDisplay1);
      this.Controls.Add(this.RunContCheckBox);
      this.Controls.Add(this.RunOnceButton);
      this.Controls.Add(this.SampleTextBox);
      this.Controls.Add(this.myCountText);
      this.Controls.Add(this.Label1);
      this.Name = "Form1";
      this.Text = "QuickBuild Sample Application";
      ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).EndInit();
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
      Application.Run(new Form1());
    }

    private void Form1_Closing(object sender, CancelEventArgs e)
    {
      myJobManager.UserResultAvailable -= new CogJobManager.CogUserResultAvailableEventHandler(myJobManager_UserResultAvailable);
      cogRecordDisplay1.Dispose();
      // Be sure to shudown the CogJobManager!!
      myJobManager.Shutdown();
    }

    private void RunOnceButton_Click(object sender, System.EventArgs e)
    {
      try
      {
        myJobManager.Run();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void RunContCheckBox_CheckedChanged(object sender, System.EventArgs e)
    {
      if (RunContCheckBox.Checked)
      {
        try
        {
          myJobManager.RunContinuous();
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
        RunOnceButton.Enabled = false;
      }
      else
      {
        try
        {
          myJobManager.Stop();
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
        RunOnceButton.Enabled = true;
      }

    }
  }
}
