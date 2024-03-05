/*******************************************************************************
 Copyright (C) 2004-2010 Cognex Corporation

 Subject to Cognex Corporations terms and conditions and license agreement,
 you are authorized to use and modify this source code in any way you find
 useful, provided the Software and/or the modified Software is used solely in
 conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
 and agree that Cognex has no warranty, obligations or liability for your use
 of the Software.
*******************************************************************************

 This sample demonstrates how to catch and handle the overrun event.

 The overrun event fires when an acquisition fails because, even though
 the acquisition system was able to obtain the required resources, it was unable
 to start the acquisition. This is because the acquisition system received another
 trigger while it was still acquiring an image.

 Hardware trigger mode (a.k.a automatic triggering) is used to show this event.
 See HardwareTrigger sample for detailed setup procedures because this sample
 will not illustrate every step.

 A Cognex frame grabber must be present in order to run this sample program.
 If no Cognex board is present, the program displays an error and exits.
 
 Note that the .NET garbage collector is called on a regular basis to free
 image memory that is no longer referenced. 

 This program assumes that you have some knowledge of C# and VisionPro
 programming.

 Follow the next steps in order to catch and handle the overrun event.
 Step 1) Create a CogAcqFifoTool.
 Step 2) Set TriggerEnabled property to False.
 Step 3) Select hardware auto trigger mode.
 Step 4) When a single acquisition is completed, the ICogAcqFifo will fire
         the acquisition completion event. The acquisition completion event handler
          must be initialized to catch this event.
 Step 5) Hook up the overrun event.
 Step 6) Set TriggerEnabled property to True and wait for external triggers.
 Step 7) Display the overrun error count
*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
// Cognex namespace
using Cognex.VisionPro;
// Needed for CogException
using Cognex.VisionPro.Exceptions;

namespace Overrun
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class frmAcqEvents : System.Windows.Forms.Form
    {
        private Cognex.VisionPro.Display.CogDisplay cogDisplay1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblBoardType;
        private System.Windows.Forms.Button acqButton;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private CogAcqFifoTool mTool;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblVideoFormat;
        private System.Windows.Forms.Label label2;
        private int numAcqs = 0;
        private System.Windows.Forms.Label lblOverrunCount;
        ICogAcqFifo mAcqFifo;
        private System.Windows.Forms.Label triggerInfoText;
        ICogAcqTrigger mTrigger;

        public frmAcqEvents()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            InitializeAcquisition();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmAcqEvents));
            this.cogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblBoardType = new System.Windows.Forms.Label();
            this.acqButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblVideoFormat = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblOverrunCount = new System.Windows.Forms.Label();
            this.triggerInfoText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).BeginInit();
            this.SuspendLayout();
            // 
            // cogDisplay1
            // 
            this.cogDisplay1.Location = new System.Drawing.Point(232, 24);
            this.cogDisplay1.Name = "cogDisplay1";
            this.cogDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay1.OcxState")));
            this.cogDisplay1.Size = new System.Drawing.Size(304, 296);
            this.cogDisplay1.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(8, 328);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(528, 64);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.Text = @"This sample demonstrates how to handle the overrun event.  A Cognex frame grabber board must be present in order to run this sample program. When the Run button is pressed, the program first flushes all outstanding acquisitions since they are not part of new acquisitions.  It then enables the trigger enable property, and awaits hardware triggers.  No image will be captured without a hardware trigger.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Board Type:";
            // 
            // lblBoardType
            // 
            this.lblBoardType.Location = new System.Drawing.Point(96, 32);
            this.lblBoardType.Name = "lblBoardType";
            this.lblBoardType.Size = new System.Drawing.Size(128, 16);
            this.lblBoardType.TabIndex = 3;
            this.lblBoardType.Text = "Unknown";
            // 
            // acqButton
            // 
            this.acqButton.Location = new System.Drawing.Point(48, 104);
            this.acqButton.Name = "acqButton";
            this.acqButton.Size = new System.Drawing.Size(104, 32);
            this.acqButton.TabIndex = 10;
            this.acqButton.Text = "Run";
            this.acqButton.Click += new System.EventHandler(this.acqButton_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Selected Video Format:";
            // 
            // lblVideoFormat
            // 
            this.lblVideoFormat.Location = new System.Drawing.Point(16, 72);
            this.lblVideoFormat.Name = "lblVideoFormat";
            this.lblVideoFormat.Size = new System.Drawing.Size(208, 16);
            this.lblVideoFormat.TabIndex = 12;
            this.lblVideoFormat.Text = "Unknown";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(32, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "Overrun event count:";
            // 
            // lblOverrunCount
            // 
            this.lblOverrunCount.Location = new System.Drawing.Point(160, 200);
            this.lblOverrunCount.Name = "lblOverrunCount";
            this.lblOverrunCount.Size = new System.Drawing.Size(64, 16);
            this.lblOverrunCount.TabIndex = 14;
            this.lblOverrunCount.Text = "0";
            // 
            // triggerInfoText
            // 
            this.triggerInfoText.Location = new System.Drawing.Point(32, 160);
            this.triggerInfoText.Name = "triggerInfoText";
            this.triggerInfoText.Size = new System.Drawing.Size(176, 16);
            this.triggerInfoText.TabIndex = 15;
            this.triggerInfoText.Text = " Waiting to run...";
            // 
            // frmAcqEvents
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(536, 406);
            this.Controls.Add(this.triggerInfoText);
            this.Controls.Add(this.lblOverrunCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblVideoFormat);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.acqButton);
            this.Controls.Add(this.lblBoardType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.cogDisplay1);
            this.Name = "frmAcqEvents";
            this.Text = "Show how to capture the overrun event";
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            try 
            {
                Application.Run(new frmAcqEvents());
            }
            catch (CogException ce)
            {	
                MessageBox.Show("The following error has occured\n" + ce.Message);
                Application.Exit();
            }
                
        }

        public void InitializeAcquisition()
        {
            try
            {
                // Step 1 - Create an acquisition tool which creates an CogAcqFifo with a default
                //           video format for that frame grabber.
                mTool = new CogAcqFifoTool();

                // Check if the tool was able to create a default acqfifo.
                if (mTool.Operator == null)
                    throw new CogAcqNoFrameGrabberException("A board might be missing or not be functioning properly.");

                // First, Get the ICogAcqFifo.
                mAcqFifo = mTool.Operator;
                // Display the video format
                lblVideoFormat.Text = mAcqFifo.VideoFormat;
                // Display the board type
                lblBoardType.Text = mAcqFifo.FrameGrabber.Name;

                // Step 2: Get the trigger operator (aka trigger acquisition property)
                mTrigger = mAcqFifo.OwnedTriggerParams;
                if (mTrigger == null)
                    throw new CogAcqWrongTriggerModelException("Trigger model not supported");
                // Turn off triggers
                mTrigger.TriggerEnabled = false;
                // Step 3: Select hardware triggers
                mTrigger.TriggerModel = CogAcqTriggerModelConstants.Auto;

                // Step 4: Hook up the acquisition completion event. Each time a tool acquires
                // an image, it fires the acquisition completion event.
                mAcqFifo.Complete += new CogCompleteEventHandler(Operator_Complete);

                // Step 5: Hook up the acquisition overrun event
                mAcqFifo.Overrun += new CogOverrunEventHandler(mAcqFifo_Overrun);

                // NOTE: Either the exposure or brightness may need adjustment to clearly see
                // the acquired image. Both exposure and brightness are set to high values
                // in case sufficient lighting is unavailable.

                if (mAcqFifo.OwnedExposureParams != null)
                    mAcqFifo.OwnedExposureParams.Exposure = 50; //mSecs
                if (mAcqFifo.OwnedBrightnessParams != null)
                    mAcqFifo.OwnedBrightnessParams.Brightness = 0.9;
            }
            catch (CogAcqException ex)
            {
                MessageBox.Show(ex.Message);

            }
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
            try
            {
                mAcqFifo.GetFifoState(out numPendingVal,out numReadyVal,out busyVal);
                if (numReadyVal > 0)
                    cogDisplay1.Image = 
                        mAcqFifo.CompleteAcquireEx(info); 
                numAcqs++;
                // We need to run the garbage collector on occasion to cleanup
                // images that are no longer being used.
                if (numAcqs > 4)
                {
                    GC.Collect();
                    numAcqs = 0;
                }
            }
            catch (CogException ce)
            {
                MessageBox.Show("The following error has occured\n" + ce.Message);
            }
        }

        // This method is the clicked event handler for the acqButton control
        // on form.  It starts image acquisition.
        private void acqButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (mAcqFifo != null)
                {
                    if (acqButton.Text == "Run")
                    {
                        lblOverrunCount.Text = "0";
                        // Flush all outstanding acquisitions since they are not part of new acquisitions.
                        mAcqFifo.Flush();
                        mTrigger.TriggerEnabled = true;
                        triggerInfoText.Text = "Waiting for hardware triggers...";
                        acqButton.Text = "Stop";
                    }
                    else
                    {
                        mTrigger.TriggerEnabled = false;
                        triggerInfoText.Text = "Waiting to run...";
                        acqButton.Text = "Run";
                    }
                }

            }
            catch (CogException ce)
            {
                MessageBox.Show("The following error has occured\n" + ce.Message);
            }
        }
        // Overrun event handler
        private void mAcqFifo_Overrun(object sender, CogOverrunEventArgs e)
        {
            // this next bit of code converts a string to an int32, adds 1,
            // and then converts it back to a string for displaying as a label
                    if (InvokeRequired)
                    {
                        Invoke(new CogOverrunEventHandler(mAcqFifo_Overrun),
                            new object[] {sender, e});
                        return;
                    }


            lblOverrunCount.Text = ((System.Int32)(System.Int32.Parse(lblOverrunCount.Text,
                System.Globalization.NumberStyles.Integer,null) + 1)).ToString();

        }
    }
}

