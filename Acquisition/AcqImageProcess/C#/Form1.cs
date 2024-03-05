/*******************************************************************************
 Copyright (C) 2004-2010 Cognex Corporation

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
 
 The AcqImageProcess sample program is intended to demonstrate how one
 might use VisionPro(R) to achieve a high level of application throughput
 by overlapping image acquisition and image processing. This is
 accomplished within the context of a single threaded C#.

 The general approach taken here is to use the acquisition complete
 event to initiate image processing as soon as an image is
 available. In addition, the same event handler can queue up a manual
 trigger mode acquisition request prior to actually processing the
 current image so that acquisition of the next image can overlap
 processing of the current image.

 In addition to overlapping acquisition and image processing, this
 sample program demonstrates the impact on throughput of:
   * updating a display control for every acquisition,
 This is accomplished by providing GUI controls that permit the
 toggling of these characteristics (i.e. do or do not update the
 display at run time).

 For the sake of this sample program, the image-processing task has
 been defined to be the location of the largest blob in the image using
 the blob tool. For every image acquired the blob tool is run, the
 largest blob is identified, and the center of mass of that blob is
 reported back to the GUI as well as displayed graphically on the
 image. The selection of this image-processing task is arbitrary - it
 could be any image-processing task.


 TIMING ANALYSIS

 As an aid to evaluating the impact of various implementation choices
 on throughput, the application computes and reports a cycle time in
 milliseconds for each image that is acquired and processed. This
 reported time corresponds to the interval from the delivery to the GUI
 of the previous results to (just before) the delivery to the GUI of
 the current results.

 Please note that it is possible to observe a reported cycle time of
 less than normal acquisition time (33 ms for RS170 cameras). This can
 happen when more than one completed acquisition becomes available for
 consumption at any given instant. In this case, the cycle time does
 not incorporate the usual amount of acquisition time.
 
 
 SINGLE THREADED APPLICATIONS
 
 This application is written as a single threaded application. To facilitate this,
 image analysis is performed within the complete event handler.  This will work as
 long as the image analysis time is less than the time between acquisitions.  If
 the time between acquisitions is shorter than the image analysis time, then
 acquisitions will not be serviced in a timely manner.  This is because the complete
 event handler will not be ready to respond to the next complete event when it
 is fired.  Instead, complete events will queue, and be handled when image
 processing finishes and the complete event handler exits.
 
 If two many images are awaiting the application to call complete, the acquisition
 system will start issuing exceptions indicating that no buffer space is available
 for acqusitions.
 
 Also note that with single threaded applications, acquisition, processing, and
 the GUI are all running in the same thread.  If acquisition and processing 
 consume most of the CPU, the GUI will become unresponsive.  Multi-threaded
 applications tend to be more GUI responsive, since they can be constructed such
 that the GUI occasionally gets a time slice of the CPU.
  
  
 GARBAGE COLLECTION
  
 This sample application explicitly calls the .NET garbage to free
 image memory that is no longer referenced. If the garbarge collector was not 
 called, the application may eventually run out of memory.
 

 OF SPECIAL INTEREST

 A few techniques used in this sample program warrant some extra
 attention.

 1. When using manual acquisition triggering, you should "prime" the
    acquisition pipeline with two (or more, up to 32) acquisition start
    requests. This will help prevent the acquisition engine from
    becoming stalled for want of an acquisition request.

 SUPPORT FOR DIFFERENT ACQUISITION TRIGGER MODELS

 This sample program supports the use of manual, semi, and automatic
 acquisition trigger models. This is done by specifically checking the
 acq fifo's current trigger model and only invoking StartAcquire when
 it is allowed (i.e. when the trigger model is either manual or semi,
 but not when it is auto or slave). While this program explicitly
 demonstrates how to programmatically set the trigger model to manual,
 you can try out other trigger models by modifying the appropriate
 setting in the acq fifo edit control.

*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
// Cognex namespace
using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
// Needed for CogException
using Cognex.VisionPro.Exceptions;

namespace AcqImageProcess
{
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public class frmAcqImageProcess : System.Windows.Forms.Form
  {
    private System.Windows.Forms.TextBox txtDescription;
    private IContainer components = null;
    private System.Windows.Forms.TabPage ControlTab;
    private System.Windows.Forms.TabPage AcqFifoTab;
    private System.Windows.Forms.CheckBox DisplayUpdateCheckBox;
    private System.Windows.Forms.CheckBox AcqFifoConnectCheckBox;
    private System.Windows.Forms.GroupBox group1box;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox BlobXText;
    private System.Windows.Forms.TextBox BlobYText;
    private System.Windows.Forms.TextBox CycleTimeText;
    private System.Windows.Forms.Button StartButton;
    private Cognex.VisionPro.Display.CogDisplay cogDisplay1;

    private CogAcqFifoTool AcqFifoTool;
    private CogBlobTool BlobTool;
    private int numAcqs = 0;
    private CogStopwatch StopWatch;
    private bool Processing = false;
    private bool StopAcquire;
    private ICogAcqTrigger TriggerOperator;
    private System.Windows.Forms.TabControl AcqProcessingTabControl;
    private CogRectangle Rect;
    private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TotalAcqText;
        private CogAcqFifoEditV2 cogAcqFifoEditV21;
    private int totalAcqs = 0;

    public frmAcqImageProcess()
    {
      //
      // Required for Windows Form Designer support
      //
        InitializeComponent();
        this.Load +=new EventHandler(frmAcqImageProcess_Load);
        this.Closing +=new CancelEventHandler(frmAcqImageProcess_Closing);
      //
      // TODO: Add any constructor code after InitializeComponent call
      //
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      StopAcquire = true;
      if (AcqFifoTool.Operator != null)
        AcqFifoTool.Operator.Flush();
      int counter = 0;
      while ( counter < 10)
      {
        Application.DoEvents();
        Thread.Sleep(1);
        counter++;
      }

      base.OnClosing (e);
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
        CogFrameGrabbers frameGrabbers = new CogFrameGrabbers();
        foreach (ICogFrameGrabber fg in frameGrabbers)
          fg.Disconnect(false);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAcqImageProcess));
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.AcqProcessingTabControl = new System.Windows.Forms.TabControl();
            this.ControlTab = new System.Windows.Forms.TabPage();
            this.cogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
            this.StartButton = new System.Windows.Forms.Button();
            this.group1box = new System.Windows.Forms.GroupBox();
            this.TotalAcqText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CycleTimeText = new System.Windows.Forms.TextBox();
            this.BlobYText = new System.Windows.Forms.TextBox();
            this.BlobXText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.AcqFifoConnectCheckBox = new System.Windows.Forms.CheckBox();
            this.DisplayUpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.AcqFifoTab = new System.Windows.Forms.TabPage();
            this.cogAcqFifoEditV21 = new Cognex.VisionPro.CogAcqFifoEditV2();
            this.AcqProcessingTabControl.SuspendLayout();
            this.ControlTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).BeginInit();
            this.group1box.SuspendLayout();
            this.AcqFifoTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogAcqFifoEditV21)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDescription
            // 
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtDescription.Location = new System.Drawing.Point(0, 502);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(832, 24);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.Text = "This sample demonstrates how to improve throughput by overlapping acquisition and" +
    " image processing.  Click the Start button to begin acquiring and processing ima" +
    "ges.";
            // 
            // AcqProcessingTabControl
            // 
            this.AcqProcessingTabControl.Controls.Add(this.ControlTab);
            this.AcqProcessingTabControl.Controls.Add(this.AcqFifoTab);
            this.AcqProcessingTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AcqProcessingTabControl.ItemSize = new System.Drawing.Size(120, 30);
            this.AcqProcessingTabControl.Location = new System.Drawing.Point(0, 0);
            this.AcqProcessingTabControl.Name = "AcqProcessingTabControl";
            this.AcqProcessingTabControl.SelectedIndex = 0;
            this.AcqProcessingTabControl.Size = new System.Drawing.Size(832, 502);
            this.AcqProcessingTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.AcqProcessingTabControl.TabIndex = 2;
            // 
            // ControlTab
            // 
            this.ControlTab.Controls.Add(this.cogDisplay1);
            this.ControlTab.Controls.Add(this.StartButton);
            this.ControlTab.Controls.Add(this.group1box);
            this.ControlTab.Controls.Add(this.AcqFifoConnectCheckBox);
            this.ControlTab.Controls.Add(this.DisplayUpdateCheckBox);
            this.ControlTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ControlTab.Location = new System.Drawing.Point(4, 34);
            this.ControlTab.Name = "ControlTab";
            this.ControlTab.Size = new System.Drawing.Size(816, 434);
            this.ControlTab.TabIndex = 0;
            this.ControlTab.Text = "Control";
            // 
            // cogDisplay1
            // 
            this.cogDisplay1.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay1.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay1.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay1.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay1.Location = new System.Drawing.Point(424, 32);
            this.cogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay1.MouseWheelSensitivity = 1D;
            this.cogDisplay1.Name = "cogDisplay1";
            this.cogDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay1.OcxState")));
            this.cogDisplay1.Size = new System.Drawing.Size(360, 360);
            this.cogDisplay1.TabIndex = 4;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(128, 352);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(120, 40);
            this.StartButton.TabIndex = 3;
            this.StartButton.Text = "Start";
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // group1box
            // 
            this.group1box.Controls.Add(this.TotalAcqText);
            this.group1box.Controls.Add(this.label4);
            this.group1box.Controls.Add(this.CycleTimeText);
            this.group1box.Controls.Add(this.BlobYText);
            this.group1box.Controls.Add(this.BlobXText);
            this.group1box.Controls.Add(this.label3);
            this.group1box.Controls.Add(this.label2);
            this.group1box.Controls.Add(this.label1);
            this.group1box.Location = new System.Drawing.Point(32, 120);
            this.group1box.Name = "group1box";
            this.group1box.Size = new System.Drawing.Size(344, 208);
            this.group1box.TabIndex = 2;
            this.group1box.TabStop = false;
            this.group1box.Text = "Results";
            // 
            // TotalAcqText
            // 
            this.TotalAcqText.Location = new System.Drawing.Point(184, 160);
            this.TotalAcqText.Name = "TotalAcqText";
            this.TotalAcqText.Size = new System.Drawing.Size(88, 26);
            this.TotalAcqText.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(40, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "Total Acquires:";
            // 
            // CycleTimeText
            // 
            this.CycleTimeText.Location = new System.Drawing.Point(184, 120);
            this.CycleTimeText.Name = "CycleTimeText";
            this.CycleTimeText.Size = new System.Drawing.Size(88, 26);
            this.CycleTimeText.TabIndex = 5;
            // 
            // BlobYText
            // 
            this.BlobYText.Location = new System.Drawing.Point(184, 80);
            this.BlobYText.Name = "BlobYText";
            this.BlobYText.Size = new System.Drawing.Size(88, 26);
            this.BlobYText.TabIndex = 4;
            // 
            // BlobXText
            // 
            this.BlobXText.Location = new System.Drawing.Point(184, 48);
            this.BlobXText.Name = "BlobXText";
            this.BlobXText.Size = new System.Drawing.Size(88, 26);
            this.BlobXText.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(32, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Cycle Time (ms):";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(136, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Y:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(40, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Largest Blob X:";
            // 
            // AcqFifoConnectCheckBox
            // 
            this.AcqFifoConnectCheckBox.Checked = true;
            this.AcqFifoConnectCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AcqFifoConnectCheckBox.Location = new System.Drawing.Point(32, 72);
            this.AcqFifoConnectCheckBox.Name = "AcqFifoConnectCheckBox";
            this.AcqFifoConnectCheckBox.Size = new System.Drawing.Size(240, 24);
            this.AcqFifoConnectCheckBox.TabIndex = 1;
            this.AcqFifoConnectCheckBox.Text = "Connect Acq Fifo Control";
            this.AcqFifoConnectCheckBox.CheckedChanged += new System.EventHandler(this.AcqFifoConnectCheckBox_CheckedChanged);
            // 
            // DisplayUpdateCheckBox
            // 
            this.DisplayUpdateCheckBox.Location = new System.Drawing.Point(32, 24);
            this.DisplayUpdateCheckBox.Name = "DisplayUpdateCheckBox";
            this.DisplayUpdateCheckBox.Size = new System.Drawing.Size(184, 24);
            this.DisplayUpdateCheckBox.TabIndex = 0;
            this.DisplayUpdateCheckBox.Text = "Update Display";
            // 
            // AcqFifoTab
            // 
            this.AcqFifoTab.Controls.Add(this.cogAcqFifoEditV21);
            this.AcqFifoTab.Location = new System.Drawing.Point(4, 34);
            this.AcqFifoTab.Name = "AcqFifoTab";
            this.AcqFifoTab.Size = new System.Drawing.Size(824, 464);
            this.AcqFifoTab.TabIndex = 1;
            this.AcqFifoTab.Text = "AcqFifo";
            // 
            // cogAcqFifoEditV21
            // 
            this.cogAcqFifoEditV21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogAcqFifoEditV21.Location = new System.Drawing.Point(0, 0);
            this.cogAcqFifoEditV21.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogAcqFifoEditV21.Name = "cogAcqFifoEditV21";
            this.cogAcqFifoEditV21.Size = new System.Drawing.Size(824, 464);
            this.cogAcqFifoEditV21.SuspendElectricRuns = false;
            this.cogAcqFifoEditV21.TabIndex = 0;
            // 
            // frmAcqImageProcess
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(832, 526);
            this.Controls.Add(this.AcqProcessingTabControl);
            this.Controls.Add(this.txtDescription);
            this.Name = "frmAcqImageProcess";
            this.Text = "VisionPro Acquisition and Processing Sample";
            this.AcqProcessingTabControl.ResumeLayout(false);
            this.ControlTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).EndInit();
            this.group1box.ResumeLayout(false);
            this.group1box.PerformLayout();
            this.AcqFifoTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogAcqFifoEditV21)).EndInit();
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
        Application.Run(new frmAcqImageProcess());
        
    }

    public void InitializeAcquisition()
    {
      try
      {
        AcqFifoTool = new CogAcqFifoTool();

        // Check if the tool was able to create a default acqfifo.
        if (AcqFifoTool.Operator == null)
          throw new CogAcqNoFrameGrabberException("A board might be missing or not be functioning properly.");

        BlobTool = new CogBlobTool();

        // NOTE: We use the cogBlobSegmentationModeHardFixedThreshold mode and
        // set ConnectivityMinPixels to 1000 because we want the blob image processing
        // to run faster than the time it takes to acquire an image. Otherwise, the program
        // may not respond user input quickly enough or the screen may not update properly.
        // Image processing time varies depending on the CPU speed. A faster CPU processes
        // the image faster.

        BlobTool.RunParams.SegmentationParams.Mode = CogBlobSegmentationModeConstants.HardFixedThreshold;
        BlobTool.RunParams.ConnectivityMinPixels = 1000;

        // Limit the search region so that the processing can be done quickly.
        // This region will be displayed on the CogDisplay later.

        Rect = new CogRectangle();
        Rect.SetXYWidthHeight(150, 100, 350, 300);
        BlobTool.Region = Rect;

        StopWatch = new CogStopwatch();

        // Set acq fifo trigger model. Note that this
        // may also be set via the AcqFifo edit control.

        TriggerOperator = AcqFifoTool.Operator.OwnedTriggerParams;
        TriggerOperator.TriggerEnabled = false;
        TriggerOperator.TriggerModel = CogAcqTriggerModelConstants.Manual;
        TriggerOperator.TriggerEnabled = true;

        // Connect acqfifo edit control with actual tool
        cogAcqFifoEditV21.Subject = AcqFifoTool;

      }
      catch (CogException ce)
      { 
        MessageBox.Show("The following error has occured\n" + ce.Message);
        AcqProcessingTabControl.Controls.Remove(AcqFifoTab);
        StartButton.Enabled = false;
        DisplayUpdateCheckBox.Enabled = false;
        AcqFifoConnectCheckBox.Enabled = false;
      }
    }

    private bool AcqCanStart()
    {
      // Check trigger model to see if it's okay to call acq fifo's StartAcquire method
      if (TriggerOperator == null)
        return false;
      else if (TriggerOperator.TriggerModel == CogAcqTriggerModelConstants.Auto)
        return false;
      else if (TriggerOperator.TriggerModel == CogAcqTriggerModelConstants.Slave)
        return false;
      else
        return true;
    }

    // This is the complete event handler for acquisition.  When an image is acquired,
    // it fires a complete event.  This handler verifies the state of the acquisition
    // fifo, and then calls Complete(), which gets the image from the fifo.
    
    // Note that it is necessary to call the .NET garbarge collector on a regular
    // basis so large images that are no longer used will be released back to the
    // heap.  In this sample, it is called every 5th acqusition.
    private void Operator_Complete(object sender, CogCompleteEventArgs e)
    {
      if (InvokeRequired)
      {
          Invoke(new CogCompleteEventHandler(Operator_Complete),
              new object[] {sender, e});
          return;
      }

      int numReadyVal, numPendingVal;
      bool busyVal;
      CogAcqInfo info = new CogAcqInfo();
      CogImage8Grey CurrentImage;
      if (StopAcquire)
        return;
      try
      {
        AcqFifoTool.Operator.GetFifoState(out numPendingVal,out numReadyVal,out busyVal);
        if (numReadyVal > 0)
          CurrentImage = (CogImage8Grey)AcqFifoTool.Operator.CompleteAcquireEx(info); 
        else
          throw new CogAcqAbnormalException("Ready count is not greater than 0.");
//        KeptImage = CurrentImage.Copy(CogImageCopyModeConstants.CopyPixels);
        numAcqs++;
        totalAcqs++;
        TotalAcqText.Text = totalAcqs.ToString();
        // We need to run the garbage collector on occasion to cleanup
        // images that are no longer being used.
        if (numAcqs > 4)
        {
          GC.Collect();
          numAcqs = 0;
        }
        // Issue another acquisition request if we are in manual trigger mode.
        if (AcqCanStart())
           AcqFifoTool.Operator.StartAcquire(); // request another acquisition

        // Do some processing while acquiring next image
        if (CurrentImage != null)
          AnalyzeImage(CurrentImage);
      }
      catch (CogException ce)
      {
        MessageBox.Show("The following error has occured\n" + ce.Message);
      }
    }

    private void StartButton_Click(object sender, System.EventArgs e)
    {
      // check if AcqFifoTool Operator is null; if it is output an error and return
      if (AcqFifoTool.Operator == null)
      {
        MessageBox.Show("AcqFifo not initialized. Open the AcqFifo tab, select a video format, and initialize acquisition. Then try again.");
        return;
      }
      
      if (Processing) // we should stop
      {
        // Connect the complete event handler
        AcqFifoTool.Operator.Complete -=new CogCompleteEventHandler(Operator_Complete);
        StopAcquire = true;
        // Flush all outstanding acquisition requests and stop.
        AcqFifoTool.Operator.Flush();

        Processing = false;
        StartButton.Text = "Start";
        DisplayUpdateCheckBox.Enabled = true;
        AcqFifoConnectCheckBox.Enabled = true;
      }
      else  // we should start
      {
        // Make sure to check in case the trigger model has changed
        TriggerOperator = AcqFifoTool.Operator.OwnedTriggerParams;

        // We'll be running greyscale blob on the acquired image, so configure the fifo
        // to produce greyscale images.  If a color camera is used, this setting will cause
        // the image to be converted to greyscale when CompleteAcquireEx is called.
        AcqFifoTool.Operator.OutputPixelFormat = CogImagePixelFormatConstants.Grey8;
        
        // Connect the complete event handler
        AcqFifoTool.Operator.Complete +=new CogCompleteEventHandler(Operator_Complete);
        Processing = true;
        StopAcquire = false;
        // Flush all outstanding acquisitions since they are not part of new acquisitions.
        AcqFifoTool.Operator.Flush();
    
        StopWatch.Reset();
        StopWatch.Start();
        StartButton.Text = "Stop";
        DisplayUpdateCheckBox.Enabled = false;
        AcqFifoConnectCheckBox.Enabled = false;
        if (AcqCanStart())
        {
          // This is sort of subtle. For manual
          // triggering, prime the acquisition engine
          // with two (or more, up to 32) start requests
          // to ensure optimal throughput
          
          AcqFifoTool.Operator.StartAcquire();
          AcqFifoTool.Operator.StartAcquire();
        }
      }

    }

    private void AnalyzeImage(CogImage8Grey objImage)
    {
      // Miscellaneous local vars
      bool Gotit;
      double BlobX;
      double BlobY;
      double MaxBlobArea;
      double BlobArea;
      CogBlobResultCollection FilteredBlobs;
      CogPointMarker Marker;

      Gotit = false;  
      BlobX = 0;
      BlobY = 0;
    
      // Set up blob tool
      BlobTool.InputImage = objImage;
  
      // Run the blob tool
      BlobTool.Run();
  
      // Extract biggest blob results if available
      if (BlobTool.RunStatus.Result == CogToolResultConstants.Error)
        Gotit = false;
      else if (BlobTool.Results.GetBlobs(true).Count < 1) 
        Gotit = false;
      else
      {
        MaxBlobArea = -1;
        FilteredBlobs = BlobTool.Results.GetBlobs(true);
        foreach (CogBlobResult ObjBlobResult in FilteredBlobs)
        {
          BlobArea = ObjBlobResult.Area;
          if (BlobArea > MaxBlobArea)
          {
            Gotit = true;
            MaxBlobArea = BlobArea;
            BlobX = ObjBlobResult.CenterOfMassX;
            BlobY = ObjBlobResult.CenterOfMassY;
          }
        }
      }
  
      // Now do something with processing results
      if (DisplayUpdateCheckBox.Checked)
      {
        cogDisplay1.DrawingEnabled = false;
        cogDisplay1.Image = objImage;
        if (Gotit)
        {
          cogDisplay1.StaticGraphics.Clear();
          cogDisplay1.StaticGraphics.Add(Rect,"main");
          Marker = new CogPointMarker();
          Marker.Color = CogColorConstants.Red;
          Marker.X = BlobX;
          Marker.Y = BlobY;
          cogDisplay1.StaticGraphics.Add(Marker,"main");
        }
        cogDisplay1.DrawingEnabled = true;
      }
      StopWatch.Stop();
      if (Gotit)
      {
        BlobXText.Text = BlobX.ToString();
        BlobYText.Text = BlobY.ToString();
      }
      else
      {
        BlobXText.Text = "N/A";
        BlobYText.Text = "N/A";
      }
      // Update cycle time, then reset clock
      CycleTimeText.Text = StopWatch.Milliseconds.ToString();
      StopWatch.Reset();
      StopWatch.Start();
    }

    private void AcqFifoConnectCheckBox_CheckedChanged(object sender, System.EventArgs e)
    {
      // connect / disconnect acqfifo control
      if (AcqFifoConnectCheckBox.Checked)
        cogAcqFifoEditV21.Subject = AcqFifoTool;
      else
        cogAcqFifoEditV21.Subject = null; 
    }

    private void frmAcqImageProcess_Load(object sender, EventArgs e)
    {
      InitializeAcquisition();

    }

    private void frmAcqImageProcess_Closing(object sender, CancelEventArgs e)
    {
      //Make sure to dispose of the acq fifo edit control or an error may occur!
      cogAcqFifoEditV21.Dispose();
    }

  }
}

