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

 This sample demonstates how to catch and handle various acquisition events.

 The most common type of event fired by a VisionPro object is a change event.
 Change events are used by VisionPro objects to indicate that the state of
 the object has changed. For example, when you change the brightness of
 an acquisition FIFO it fires a change event that you can handle if you want
 to do something in response to the change or ignore if you do not. In this sample,
 we will show you how to capture the brightness, contrast, strobe enabled, and
 trigger enabled events.

 A Cognex frame grabber must be present in order to run this sample program.
 If no Cognex board is present, the program displays an error and exits.

 This program assumes that you have some knowledge of C# and VisionPro
 programming.

 Follow the next steps in order to catch the acquisition events.
 Step 1) Create the CogAcqFifoTool
 Step 2) Assign the ICogAcqFifo, ICogAcqBrightness, ICogAcqContrast, ICogAcqTrigger, and
         ICogAcqStrobe operators.
 Step 3) Add the acq fifo changed and complete event handlers.
 
 Note that it is necessary to call the .NET garbarge collector on a regular
 basis so large images that are no longer used will be released back to the
 heap.  In this sample, it is called every 5th acqusition in the Operator_Complete() handler
 
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

namespace Change
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkStrobeEnabled;
        private System.Windows.Forms.CheckBox chkTriggerEnabled;
        private System.Windows.Forms.NumericUpDown brightnessUpDown;
        private System.Windows.Forms.NumericUpDown contrastUpDown;
        private System.Windows.Forms.Button acqButton;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private CogAcqFifoTool mTool;
        private ICogAcqFifo mAcqFifo;
        private ICogAcqBrightness mBrightness;
        private ICogAcqContrast mContrast;
        private ICogAcqTrigger mTrigger;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblVideoFormat;
        private ICogAcqStrobe mStrobe;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label EventLabel;
        private Label NoBrightnessLabel;
        private Label NoContrastLabel;
        private Label NoStrobeLabel;
        private Label NoTriggerLabel;
        private int numAcqs = 0;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAcqEvents));
            this.cogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblBoardType = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkStrobeEnabled = new System.Windows.Forms.CheckBox();
            this.chkTriggerEnabled = new System.Windows.Forms.CheckBox();
            this.brightnessUpDown = new System.Windows.Forms.NumericUpDown();
            this.contrastUpDown = new System.Windows.Forms.NumericUpDown();
            this.acqButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblVideoFormat = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.EventLabel = new System.Windows.Forms.Label();
            this.NoBrightnessLabel = new System.Windows.Forms.Label();
            this.NoContrastLabel = new System.Windows.Forms.Label();
            this.NoStrobeLabel = new System.Windows.Forms.Label();
            this.NoTriggerLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastUpDown)).BeginInit();
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
            this.txtDescription.Location = new System.Drawing.Point(0, 368);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(528, 48);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.Text = resources.GetString("txtDescription.Text");
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
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Brightness";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Contrast";
            // 
            // chkStrobeEnabled
            // 
            this.chkStrobeEnabled.Location = new System.Drawing.Point(19, 208);
            this.chkStrobeEnabled.Name = "chkStrobeEnabled";
            this.chkStrobeEnabled.Size = new System.Drawing.Size(185, 26);
            this.chkStrobeEnabled.TabIndex = 6;
            this.chkStrobeEnabled.Text = "Strobe Enabled";
            this.chkStrobeEnabled.CheckedChanged += new System.EventHandler(this.chkStrobeEnabled_CheckedChanged);
            // 
            // chkTriggerEnabled
            // 
            this.chkTriggerEnabled.Location = new System.Drawing.Point(19, 240);
            this.chkTriggerEnabled.Name = "chkTriggerEnabled";
            this.chkTriggerEnabled.Size = new System.Drawing.Size(189, 23);
            this.chkTriggerEnabled.TabIndex = 7;
            this.chkTriggerEnabled.Text = "Trigger Enabled";
            this.chkTriggerEnabled.CheckedChanged += new System.EventHandler(this.chkTriggerEnabled_CheckedChanged);
            // 
            // brightnessUpDown
            // 
            this.brightnessUpDown.DecimalPlaces = 1;
            this.brightnessUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.brightnessUpDown.Location = new System.Drawing.Point(91, 136);
            this.brightnessUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.brightnessUpDown.Name = "brightnessUpDown";
            this.brightnessUpDown.Size = new System.Drawing.Size(113, 20);
            this.brightnessUpDown.TabIndex = 8;
            this.brightnessUpDown.ValueChanged += new System.EventHandler(this.brightnessUpDown_ValueChanged);
            // 
            // contrastUpDown
            // 
            this.contrastUpDown.DecimalPlaces = 1;
            this.contrastUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.contrastUpDown.Location = new System.Drawing.Point(91, 168);
            this.contrastUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.contrastUpDown.Name = "contrastUpDown";
            this.contrastUpDown.Size = new System.Drawing.Size(113, 20);
            this.contrastUpDown.TabIndex = 9;
            this.contrastUpDown.ValueChanged += new System.EventHandler(this.contrastUpDown_ValueChanged);
            // 
            // acqButton
            // 
            this.acqButton.Location = new System.Drawing.Point(43, 280);
            this.acqButton.Name = "acqButton";
            this.acqButton.Size = new System.Drawing.Size(104, 32);
            this.acqButton.TabIndex = 10;
            this.acqButton.Text = "Acquire Image";
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
            this.lblVideoFormat.Size = new System.Drawing.Size(208, 61);
            this.lblVideoFormat.TabIndex = 12;
            this.lblVideoFormat.Text = "Unknown";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 320);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 24);
            this.label5.TabIndex = 13;
            this.label5.Text = "Event:";
            // 
            // EventLabel
            // 
            this.EventLabel.Location = new System.Drawing.Point(56, 320);
            this.EventLabel.Name = "EventLabel";
            this.EventLabel.Size = new System.Drawing.Size(72, 24);
            this.EventLabel.TabIndex = 14;
            this.EventLabel.Text = "None";
            // 
            // NoBrightnessLabel
            // 
            this.NoBrightnessLabel.AutoSize = true;
            this.NoBrightnessLabel.Location = new System.Drawing.Point(15, 136);
            this.NoBrightnessLabel.Name = "NoBrightnessLabel";
            this.NoBrightnessLabel.Size = new System.Drawing.Size(189, 13);
            this.NoBrightnessLabel.TabIndex = 15;
            this.NoBrightnessLabel.Text = "CogAcqFifo doesn\'t support Brightness";
            this.NoBrightnessLabel.Visible = false;
            // 
            // NoContrastLabel
            // 
            this.NoContrastLabel.AutoSize = true;
            this.NoContrastLabel.Location = new System.Drawing.Point(19, 171);
            this.NoContrastLabel.Name = "NoContrastLabel";
            this.NoContrastLabel.Size = new System.Drawing.Size(179, 13);
            this.NoContrastLabel.TabIndex = 16;
            this.NoContrastLabel.Text = "CogAcqFifo doesn\'t support Contrast";
            this.NoContrastLabel.Visible = false;
            // 
            // NoStrobeLabel
            // 
            this.NoStrobeLabel.AutoSize = true;
            this.NoStrobeLabel.Location = new System.Drawing.Point(19, 208);
            this.NoStrobeLabel.Name = "NoStrobeLabel";
            this.NoStrobeLabel.Size = new System.Drawing.Size(171, 13);
            this.NoStrobeLabel.TabIndex = 17;
            this.NoStrobeLabel.Text = "CogAcqFifo doesn\'t support Strobe";
            this.NoStrobeLabel.Visible = false;
            // 
            // NoTriggerLabel
            // 
            this.NoTriggerLabel.AutoSize = true;
            this.NoTriggerLabel.Location = new System.Drawing.Point(19, 241);
            this.NoTriggerLabel.Name = "NoTriggerLabel";
            this.NoTriggerLabel.Size = new System.Drawing.Size(178, 13);
            this.NoTriggerLabel.TabIndex = 18;
            this.NoTriggerLabel.Text = "CogAcqFifo doesn\'t support Triggers";
            this.NoTriggerLabel.Visible = false;
            // 
            // frmAcqEvents
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(536, 422);
            this.Controls.Add(this.EventLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblVideoFormat);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.acqButton);
            this.Controls.Add(this.contrastUpDown);
            this.Controls.Add(this.brightnessUpDown);
            this.Controls.Add(this.chkTriggerEnabled);
            this.Controls.Add(this.chkStrobeEnabled);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblBoardType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.cogDisplay1);
            this.Controls.Add(this.NoBrightnessLabel);
            this.Controls.Add(this.NoStrobeLabel);
            this.Controls.Add(this.NoTriggerLabel);
            this.Controls.Add(this.NoContrastLabel);
            this.Name = "frmAcqEvents";
            this.Text = "Show how to capture certain acquisition events";
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastUpDown)).EndInit();
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

                // See samples\Programming\Acquisition\Operators sample for obtaining each operator.
                // Step 2 - Assign the ICogAcqFifo, ICogAcqBrightness, ICogAcqContrast, ICogAcqTrigger, and
                //          ICogAcqStrobe operators.
                // First, Get the ICogAcqFifo.
                mAcqFifo = mTool.Operator;
                // Display the video format
                lblVideoFormat.Text = mAcqFifo.VideoFormat;
                // Display the board type
                lblBoardType.Text = mAcqFifo.FrameGrabber.Name;
                // Let's have a small timeout period.
                mAcqFifo.Timeout = 300; // in ms.

                // Get the ICogAcqBrightness
                mBrightness = mTool.Operator.OwnedBrightnessParams;
                // Show the brightness
                if (mBrightness != null)
                    brightnessUpDown.Value = (decimal) mBrightness.Brightness;
                else
                {
                    brightnessUpDown.Visible = false;
                    NoBrightnessLabel.Visible = true;
                }

                // Get the ICogAcqContrast
                mContrast = mTool.Operator.OwnedContrastParams;
                // Show the contrast
                if (mContrast != null)
                    contrastUpDown.Value = (decimal) mContrast.Contrast;
                else
                {
                    contrastUpDown.Visible = false;
                    NoContrastLabel.Visible = true;
                }

                // Get the ICogAcqTrigger
                mTrigger = mTool.Operator.OwnedTriggerParams;
                if (mTrigger != null)
                    chkTriggerEnabled.Checked = mTrigger.TriggerEnabled;
                else
                {
                    chkTriggerEnabled.Visible = false;
                    NoTriggerLabel.Visible = true;
                }

                // Get the ICogAcqStrobe
                mStrobe = mTool.Operator.OwnedStrobeParams;
                if (mStrobe != null)
                    chkStrobeEnabled.Checked = mStrobe.StrobeEnabled;
                else
                {
                    chkStrobeEnabled.Visible = false;
                    NoStrobeLabel.Visible = true;
                }

                // Hook up the acquisition completion event. Each time a tool acquires
                // an image, it fires the acquisition completion event.
                mTool.Operator.Complete += new CogCompleteEventHandler(Operator_Complete);

                // Hook up the operator changed event handler. Each time an operator
                // (i.e. acquisition property) of ICogAcqFifo changes, it fires an
                // operator changed event.
                mTool.Operator.Changed += new CogChangedEventHandler(Operator_Changed);
            }
            catch (CogAcqException ex)
            {
                MessageBox.Show("The following error has occured: \n" + ex.Message);
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
                mTool.Operator.GetFifoState(out numPendingVal,out numReadyVal,out busyVal);
                if (numReadyVal > 0)
                    cogDisplay1.Image = 
                        mTool.Operator.CompleteAcquireEx(info); 
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

        // Whenever the state of a VisionPro object changes, that object fires a Changed event.
        // The first argument to the Changed event is the object that fired the event.  The
        // second argument contains a StateFlags property, which is a bitfield that indicates 
        // what has changed on the sender object.  Each property that has changed on the object
        // corresponds to a 1 in the StateFlag bitfield.  By interrogating the StateFlags bitfield, one
        // can ascertain what has changed.

        // Each time one of the properties changes, the EventLabel on the form updates the name
        // of which property changed, and flips the color (red->green or green->red).
        private void Operator_Changed(object sender, CogChangedEventArgs e)
        {
                    if (InvokeRequired)
                    {
                        Invoke(new CogChangedEventHandler(Operator_Changed),
                            new object[] {sender, e});
                        return;
                    }

            Color CurrentStatusColor = EventLabel.ForeColor;
            bool found = false;
            EventLabel.Text = "";
            if ((e.StateFlags & CogAcqFifoStateFlags.SfBrightness) > 0)
            {
                EventLabel.Text += "Brightness ";
                found = true;
            }
            if ((e.StateFlags & CogAcqFifoStateFlags.SfContrast) > 0)
            {
                EventLabel.Text += "Contrast ";
                found = true;
            }
            if ((e.StateFlags & CogAcqFifoStateFlags.SfTriggerEnabled) > 0)
            {
                EventLabel.Text += "Trigger Enabled ";
                found = true;
            }
            if ((e.StateFlags & CogAcqFifoStateFlags.SfStrobeEnabled) > 0)
            {
                EventLabel.Text += "Strobe Enabled ";
                found = true;
            }
            if (found)
            {
                if (EventLabel.ForeColor == Color.Green)
                    EventLabel.ForeColor = Color.Red;
                else
                    EventLabel.ForeColor = Color.Green;
            }

        }

        // This method is changed event handler for the brightnessUpDown control
        // on form.
        private void brightnessUpDown_ValueChanged(object sender, System.EventArgs e)
        {
            if (mBrightness == null)
                return;

            if (brightnessUpDown.Value >= 0 && brightnessUpDown.Value <= 1)
                mBrightness.Brightness = (double)brightnessUpDown.Value;
            else
            {
                brightnessUpDown.Value = (decimal)0.5;
                mBrightness.Brightness = 0.5;
            }
        }

        // This method is changed event handler for the contrastUpDown control
        // on form.
        private void contrastUpDown_ValueChanged(object sender, System.EventArgs e)
        {
            if (mContrast == null)
                return;

            if (contrastUpDown.Value >= 0 && contrastUpDown.Value <= 1)
                mContrast.Contrast = (double)contrastUpDown.Value;
            else
            {
                contrastUpDown.Value = (decimal)0.5;
                mContrast.Contrast = 0.5;
            }
        }

        // This method is changed event handler for the chkStrobeEnabled control
        // on form.
        private void chkStrobeEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            if (mStrobe == null)
                return;

            mStrobe.StrobeEnabled = chkStrobeEnabled.Checked;
        }

        // This method is changed event handler for the chkTriggerEnabled control
        // on form.
        private void chkTriggerEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            if (mTrigger == null)
                return;

            mTrigger.TriggerEnabled = chkTriggerEnabled.Checked;
        }
        // This method is changed event handler for the acqButton control
        // on form.  It starts an acquisition.
        private void acqButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (mAcqFifo == null)
                    return;
                mAcqFifo.StartAcquire();
            }
            catch (CogException ce)
            {
                MessageBox.Show("The following error has occured\n" + ce.Message);
            }
        }
    }
}

