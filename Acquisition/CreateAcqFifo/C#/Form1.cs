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

 This sample demonstrates how to create a new ICogAcqFifo operator for a given
 video format. The CogAcqFifoTool does not acquire images by itself. Instead it
 uses the ICogAcqFifo to acquire images.

 When the user wants to change the video format, a new ICogAcqFifo must be created.
 The sample displays an error and exits if it cannot locate a frame grabber. This
 is because the frame grabber creates the ICogAcqFifo.

 The sample will acquire an image when the Acquire button is pressed and display
 on the CogDisplay.

 This program assumes that you have some knowledge of C# and VisionPro
 programming.

 The following steps show how to create a new CogAcqFifo operator.
 Step 1) Create the CogFrameGrabbers. Make sure there is at least one Cognex
         frame grabber on the system.
 Step 2) Select the first frame grabber.
 Step 3) Create an ICogAcqFifo operator with the selected video format.
 Step 4) Acquire an image and display it when the Acquire button is pressed.
 
 Note that the .NET garbage collector is called every 5th image to free up images that
 are being held on the heap.

*/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;

namespace CreateAcqFifo
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Form1 : System.Windows.Forms.Form
    {
        private Cognex.VisionPro.Display.CogDisplay cogDisplay1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label BoardTypeLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox VidFormatComboBox;
        private System.Windows.Forms.Button AcquireButton;
        private System.Windows.Forms.TextBox textBox1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private ICogAcqFifo mAcqFifo = null;
        private ICogFrameGrabber mFrameGrabber = null;
        private int numAcqs = 0;

        public Form1()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            InitializeAcquisition();
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

        private void InitializeAcquisition()
        {
            try
            {
                // Step 1 - Create the CogFrameGrabbers
                CogFrameGrabbers mFrameGrabbers = new CogFrameGrabbers();
                if (mFrameGrabbers.Count < 1)
                    throw new CogAcqNoFrameGrabberException("No frame grabbers found");

                // Step 2 - Select the first frame grabber even if there is more than one.
                mFrameGrabber = mFrameGrabbers[0];
                // Display the board type
                BoardTypeLabel.Text = mFrameGrabber.Name;

                // Fill in video formats so that the user can choose one later.
                VidFormatComboBox.Items.Clear();
                for (int i = 0; i < mFrameGrabber.AvailableVideoFormats.Count; i++)
                    VidFormatComboBox.Items.Add(mFrameGrabber.AvailableVideoFormats[i]);

                // Add click handler for Video Format Combo Box
                VidFormatComboBox.SelectedIndexChanged += new EventHandler(VidFormatComboBox_SelectedIndexChanged);
                AcquireButton.Enabled = false;
            }
            catch (CogAcqException ex)
            {
                MessageBox.Show("No camera is connected " + ex.Message);
            }
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
            this.cogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
            this.label1 = new System.Windows.Forms.Label();
            this.BoardTypeLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.VidFormatComboBox = new System.Windows.Forms.ComboBox();
            this.AcquireButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).BeginInit();
            this.SuspendLayout();
            // 
            // cogDisplay1
            // 
            this.cogDisplay1.Location = new System.Drawing.Point(280, 24);
            this.cogDisplay1.Name = "cogDisplay1";
            this.cogDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay1.OcxState")));
            this.cogDisplay1.Size = new System.Drawing.Size(336, 296);
            this.cogDisplay1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Board Type:";
            // 
            // BoardTypeLabel
            // 
            this.BoardTypeLabel.Location = new System.Drawing.Point(104, 32);
            this.BoardTypeLabel.Name = "BoardTypeLabel";
            this.BoardTypeLabel.Size = new System.Drawing.Size(160, 24);
            this.BoardTypeLabel.TabIndex = 2;
            this.BoardTypeLabel.Text = "Unknown";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select a Video Format:";
            // 
            // VidFormatComboBox
            // 
            this.VidFormatComboBox.Location = new System.Drawing.Point(16, 104);
            this.VidFormatComboBox.Name = "VidFormatComboBox";
            this.VidFormatComboBox.Size = new System.Drawing.Size(224, 21);
            this.VidFormatComboBox.TabIndex = 4;
            this.VidFormatComboBox.Text = "Video Format";
            // 
            // AcquireButton
            // 
            this.AcquireButton.Location = new System.Drawing.Point(64, 176);
            this.AcquireButton.Name = "AcquireButton";
            this.AcquireButton.Size = new System.Drawing.Size(88, 40);
            this.AcquireButton.TabIndex = 5;
            this.AcquireButton.Text = "Acquire";
            this.AcquireButton.Click += new System.EventHandler(this.AcquireButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 336);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(648, 40);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "This sample demonstrates how to create an acquisition fifo for a selected video f" +
                "ormat.  Select a video format and click Acquire to grab and display an image.";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(648, 382);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.AcquireButton);
            this.Controls.Add(this.VidFormatComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BoardTypeLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cogDisplay1);
            this.Name = "Form1";
            this.Text = "Create Acqusition Fifo Sample";
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
                Application.Run(new Form1());
            }
            catch (CogException ce)
            {	
                MessageBox.Show("The following error has occured\n" + ce.Message);
                Application.Exit();
            }

        }
        // Handler for Video Format Combo Box
        private void VidFormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Step 3: Create the acq fifo with the selected video format.
            String videoFormat = VidFormatComboBox.SelectedItem.ToString();
            // note that CreateAcqFifo will throw an exception if it cannot create the
            // acq fifo with the specified video format.
            mAcqFifo = mFrameGrabber.CreateAcqFifo(videoFormat,
                CogAcqFifoPixelFormatConstants.Format8Grey,0,true);
            AcquireButton.Enabled = true;
        }

        // Note that we are calling the .NET garbage collector every 5th acquisition to cleanup
        // objects on the heap.
        private void AcquireButton_Click(object sender, System.EventArgs e)
        {
            int trignum;
            if (mAcqFifo != null)
            {
                // Step 4: Acquire an image
                cogDisplay1.Image = mAcqFifo.Acquire(out trignum);
                numAcqs++;
                if (numAcqs > 4)
                {
                    GC.Collect();
                    numAcqs = 0;
                }
            }

        }
    }
}
