/*******************************************************************************
Copyright (C) 2008 Cognex Corporation

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

This sample demonstrates how a user can create their own Vision Tools using 
base classes the Cognex provides.  The tool created in this class converts 3-plane
RGB images to individual R, G, and B planes, or converts a 3-plane RGB image to 
individual H, S, and I planes.

The tool created is called the CogColorConversionTool.  It makes use of an operator, which
does all the real work, called CogColorConversion.

*/

namespace ColorConversionTest
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.origDisplay = new Cognex.VisionPro.Display.CogDisplay();
            this.convertedDisplay = new Cognex.VisionPro.Display.CogDisplay();
            this.lblOriginalImage = new System.Windows.Forms.Label();
            this.lblConvertedImagePlane = new System.Windows.Forms.Label();
            this.chkConvertToHSI = new System.Windows.Forms.CheckBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.grpDisplaySelector = new System.Windows.Forms.GroupBox();
            this.radioPlane2 = new System.Windows.Forms.RadioButton();
            this.radioPlane1 = new System.Windows.Forms.RadioButton();
            this.radioPlane0 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.origDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.convertedDisplay)).BeginInit();
            this.grpDisplaySelector.SuspendLayout();
            this.SuspendLayout();
            // 
            // origDisplay
            // 
            this.origDisplay.Location = new System.Drawing.Point(12, 41);
            this.origDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.origDisplay.MouseWheelSensitivity = 1;
            this.origDisplay.Name = "origDisplay";
            this.origDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("origDisplay.OcxState")));
            this.origDisplay.Size = new System.Drawing.Size(300, 300);
            this.origDisplay.TabIndex = 0;
            // 
            // convertedDisplay
            // 
            this.convertedDisplay.Location = new System.Drawing.Point(332, 41);
            this.convertedDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.convertedDisplay.MouseWheelSensitivity = 1;
            this.convertedDisplay.Name = "convertedDisplay";
            this.convertedDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("convertedDisplay.OcxState")));
            this.convertedDisplay.Size = new System.Drawing.Size(300, 300);
            this.convertedDisplay.TabIndex = 1;
            // 
            // lblOriginalImage
            // 
            this.lblOriginalImage.AutoSize = true;
            this.lblOriginalImage.Location = new System.Drawing.Point(12, 14);
            this.lblOriginalImage.Name = "lblOriginalImage";
            this.lblOriginalImage.Size = new System.Drawing.Size(74, 13);
            this.lblOriginalImage.TabIndex = 2;
            this.lblOriginalImage.Text = "Original Image";
            // 
            // lblConvertedImagePlane
            // 
            this.lblConvertedImagePlane.AutoSize = true;
            this.lblConvertedImagePlane.Location = new System.Drawing.Point(329, 14);
            this.lblConvertedImagePlane.Name = "lblConvertedImagePlane";
            this.lblConvertedImagePlane.Size = new System.Drawing.Size(118, 13);
            this.lblConvertedImagePlane.TabIndex = 3;
            this.lblConvertedImagePlane.Text = "Converted Image Plane";
            // 
            // chkConvertToHSI
            // 
            this.chkConvertToHSI.AutoSize = true;
            this.chkConvertToHSI.Location = new System.Drawing.Point(26, 370);
            this.chkConvertToHSI.Name = "chkConvertToHSI";
            this.chkConvertToHSI.Size = new System.Drawing.Size(102, 17);
            this.chkConvertToHSI.TabIndex = 4;
            this.chkConvertToHSI.Text = "Convert to HSI?";
            this.chkConvertToHSI.UseVisualStyleBackColor = true;
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(185, 370);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 5;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // grpDisplaySelector
            // 
            this.grpDisplaySelector.Controls.Add(this.radioPlane2);
            this.grpDisplaySelector.Controls.Add(this.radioPlane1);
            this.grpDisplaySelector.Controls.Add(this.radioPlane0);
            this.grpDisplaySelector.Location = new System.Drawing.Point(332, 347);
            this.grpDisplaySelector.Name = "grpDisplaySelector";
            this.grpDisplaySelector.Size = new System.Drawing.Size(300, 54);
            this.grpDisplaySelector.TabIndex = 6;
            this.grpDisplaySelector.TabStop = false;
            this.grpDisplaySelector.Text = "groupBox1";
            // 
            // radioPlane2
            // 
            this.radioPlane2.AutoSize = true;
            this.radioPlane2.Location = new System.Drawing.Point(218, 23);
            this.radioPlane2.Name = "radioPlane2";
            this.radioPlane2.Size = new System.Drawing.Size(61, 17);
            this.radioPlane2.TabIndex = 2;
            this.radioPlane2.TabStop = true;
            this.radioPlane2.Text = "Plane 2";
            this.radioPlane2.UseVisualStyleBackColor = true;
            this.radioPlane2.CheckedChanged += new System.EventHandler(this.radioPlane2_CheckedChanged);
            // 
            // radioPlane1
            // 
            this.radioPlane1.AutoSize = true;
            this.radioPlane1.Location = new System.Drawing.Point(118, 23);
            this.radioPlane1.Name = "radioPlane1";
            this.radioPlane1.Size = new System.Drawing.Size(61, 17);
            this.radioPlane1.TabIndex = 1;
            this.radioPlane1.TabStop = true;
            this.radioPlane1.Text = "Plane 1";
            this.radioPlane1.UseVisualStyleBackColor = true;
            this.radioPlane1.CheckedChanged += new System.EventHandler(this.radioPlane1_CheckedChanged);
            // 
            // radioPlane0
            // 
            this.radioPlane0.AutoSize = true;
            this.radioPlane0.Location = new System.Drawing.Point(18, 23);
            this.radioPlane0.Name = "radioPlane0";
            this.radioPlane0.Size = new System.Drawing.Size(61, 17);
            this.radioPlane0.TabIndex = 0;
            this.radioPlane0.TabStop = true;
            this.radioPlane0.Text = "Plane 0";
            this.radioPlane0.UseVisualStyleBackColor = true;
            this.radioPlane0.CheckedChanged += new System.EventHandler(this.radioPlane0_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 413);
            this.Controls.Add(this.grpDisplaySelector);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.chkConvertToHSI);
            this.Controls.Add(this.lblConvertedImagePlane);
            this.Controls.Add(this.lblOriginalImage);
            this.Controls.Add(this.convertedDisplay);
            this.Controls.Add(this.origDisplay);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.origDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.convertedDisplay)).EndInit();
            this.grpDisplaySelector.ResumeLayout(false);
            this.grpDisplaySelector.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Cognex.VisionPro.Display.CogDisplay origDisplay;
        private Cognex.VisionPro.Display.CogDisplay convertedDisplay;
        private System.Windows.Forms.Label lblOriginalImage;
        private System.Windows.Forms.Label lblConvertedImagePlane;
        private System.Windows.Forms.CheckBox chkConvertToHSI;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.GroupBox grpDisplaySelector;
        private System.Windows.Forms.RadioButton radioPlane0;
        private System.Windows.Forms.RadioButton radioPlane2;
        private System.Windows.Forms.RadioButton radioPlane1;
    }
}

