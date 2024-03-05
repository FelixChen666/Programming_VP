//*****************************************************************************
// Copyright (C) 2014 Cognex Corporation
//
// Subject to Cognex Corporation's terms and conditions and license
// agreement, you are authorized to use and modify this source code in
// any way you find useful, provided the Software and/or the modified
// Software is used solely in conjunction with a Cognex Machine Vision
// System.  Furthermore you acknowledge and agree that Cognex has no
// warranty, obligations or liability for your use of the Software.
//*****************************************************************************
// This sample program is designed to illustrate certain VisionPro
// features or techniques in the simplest way possible. It is not
// intended as the framework for a complete application. In particular,
// the sample program may not provide proper error handling, event
// handling, cleanup, repeatability, and other mechanisms that a
// commercial quality application requires.

namespace CrossSection
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.btStartProcessing = new System.Windows.Forms.Button();
      this.btnNextImage = new System.Windows.Forms.Button();
      this.chkFreezeOnDefect = new System.Windows.Forms.CheckBox();
      this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
      this.lblStepSize = new System.Windows.Forms.Label();
      this.btnShowControl = new System.Windows.Forms.Button();
      this.DefectDisplay = new Cognex.VisionPro.CogRecordDisplay();
      this.RegionDisplay = new Cognex.VisionPro.CogRecordDisplay();
      this.ProfileDisplay = new Cognex.VisionPro.CogRecordDisplay();
      this.lblDisplayDefects = new System.Windows.Forms.Label();
      this.lblDisplayProcessingRegion = new System.Windows.Forms.Label();
      this.lblDisplayProfile = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.DefectDisplay)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.RegionDisplay)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ProfileDisplay)).BeginInit();
      this.SuspendLayout();
      // 
      // btStartProcessing
      // 
      this.btStartProcessing.Location = new System.Drawing.Point(606, 10);
      this.btStartProcessing.Name = "btStartProcessing";
      this.btStartProcessing.Size = new System.Drawing.Size(142, 23);
      this.btStartProcessing.TabIndex = 0;
      this.btStartProcessing.Text = "Start Processing";
      this.btStartProcessing.UseVisualStyleBackColor = true;
      this.btStartProcessing.Click += new System.EventHandler(this.btStartProcessing_Click);
      // 
      // btnNextImage
      // 
      this.btnNextImage.Location = new System.Drawing.Point(766, 9);
      this.btnNextImage.Name = "btnNextImage";
      this.btnNextImage.Size = new System.Drawing.Size(142, 23);
      this.btnNextImage.TabIndex = 1;
      this.btnNextImage.Text = "Next Image";
      this.btnNextImage.UseVisualStyleBackColor = true;
      this.btnNextImage.Click += new System.EventHandler(this.btnNextImage_Click);
      // 
      // chkFreezeOnDefect
      // 
      this.chkFreezeOnDefect.AutoSize = true;
      this.chkFreezeOnDefect.Checked = true;
      this.chkFreezeOnDefect.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkFreezeOnDefect.Location = new System.Drawing.Point(12, 12);
      this.chkFreezeOnDefect.Name = "chkFreezeOnDefect";
      this.chkFreezeOnDefect.Size = new System.Drawing.Size(106, 17);
      this.chkFreezeOnDefect.TabIndex = 14;
      this.chkFreezeOnDefect.Text = "Freeze on defect";
      this.chkFreezeOnDefect.UseVisualStyleBackColor = true;
      // 
      // numericUpDown1
      // 
      this.numericUpDown1.Location = new System.Drawing.Point(258, 10);
      this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDown1.Name = "numericUpDown1";
      this.numericUpDown1.Size = new System.Drawing.Size(68, 20);
      this.numericUpDown1.TabIndex = 15;
      this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // lblStepSize
      // 
      this.lblStepSize.AutoSize = true;
      this.lblStepSize.Location = new System.Drawing.Point(156, 13);
      this.lblStepSize.Name = "lblStepSize";
      this.lblStepSize.Size = new System.Drawing.Size(96, 13);
      this.lblStepSize.TabIndex = 16;
      this.lblStepSize.Text = "Step Size in Pixels:";
      // 
      // btnShowControl
      // 
      this.btnShowControl.Location = new System.Drawing.Point(923, 11);
      this.btnShowControl.Name = "btnShowControl";
      this.btnShowControl.Size = new System.Drawing.Size(142, 23);
      this.btnShowControl.TabIndex = 17;
      this.btnShowControl.Text = "Show Control";
      this.btnShowControl.UseVisualStyleBackColor = true;
      this.btnShowControl.Click += new System.EventHandler(this.btnShowControl_Click);
      // 
      // DefectDisplay
      // 
      this.DefectDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
      this.DefectDisplay.ColorMapLowerRoiLimit = 0D;
      this.DefectDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
      this.DefectDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
      this.DefectDisplay.ColorMapUpperRoiLimit = 1D;
      this.DefectDisplay.Location = new System.Drawing.Point(12, 66);
      this.DefectDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
      this.DefectDisplay.MouseWheelSensitivity = 1D;
      this.DefectDisplay.Name = "DefectDisplay";
      this.DefectDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("DefectDisplay.OcxState")));
      this.DefectDisplay.Size = new System.Drawing.Size(344, 268);
      this.DefectDisplay.TabIndex = 18;
      // 
      // RegionDisplay
      // 
      this.RegionDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
      this.RegionDisplay.ColorMapLowerRoiLimit = 0D;
      this.RegionDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
      this.RegionDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
      this.RegionDisplay.ColorMapUpperRoiLimit = 1D;
      this.RegionDisplay.Location = new System.Drawing.Point(362, 66);
      this.RegionDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
      this.RegionDisplay.MouseWheelSensitivity = 1D;
      this.RegionDisplay.Name = "RegionDisplay";
      this.RegionDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("RegionDisplay.OcxState")));
      this.RegionDisplay.Size = new System.Drawing.Size(344, 268);
      this.RegionDisplay.TabIndex = 19;
      // 
      // ProfileDisplay
      // 
      this.ProfileDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
      this.ProfileDisplay.ColorMapLowerRoiLimit = 0D;
      this.ProfileDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
      this.ProfileDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
      this.ProfileDisplay.ColorMapUpperRoiLimit = 1D;
      this.ProfileDisplay.Location = new System.Drawing.Point(712, 66);
      this.ProfileDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
      this.ProfileDisplay.MouseWheelSensitivity = 1D;
      this.ProfileDisplay.Name = "ProfileDisplay";
      this.ProfileDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ProfileDisplay.OcxState")));
      this.ProfileDisplay.Size = new System.Drawing.Size(344, 268);
      this.ProfileDisplay.TabIndex = 20;
      // 
      // lblDisplayDefects
      // 
      this.lblDisplayDefects.AutoSize = true;
      this.lblDisplayDefects.Location = new System.Drawing.Point(116, 50);
      this.lblDisplayDefects.Name = "lblDisplayDefects";
      this.lblDisplayDefects.Size = new System.Drawing.Size(120, 13);
      this.lblDisplayDefects.TabIndex = 21;
      this.lblDisplayDefects.Text = "Defects in Scan Region";
      // 
      // lblDisplayProcessingRegion
      // 
      this.lblDisplayProcessingRegion.AutoSize = true;
      this.lblDisplayProcessingRegion.Location = new System.Drawing.Point(478, 50);
      this.lblDisplayProcessingRegion.Name = "lblDisplayProcessingRegion";
      this.lblDisplayProcessingRegion.Size = new System.Drawing.Size(96, 13);
      this.lblDisplayProcessingRegion.TabIndex = 22;
      this.lblDisplayProcessingRegion.Text = "Processing Region";
      // 
      // lblDisplayProfile
      // 
      this.lblDisplayProfile.AutoSize = true;
      this.lblDisplayProfile.Location = new System.Drawing.Point(825, 50);
      this.lblDisplayProfile.Name = "lblDisplayProfile";
      this.lblDisplayProfile.Size = new System.Drawing.Size(81, 13);
      this.lblDisplayProfile.TabIndex = 23;
      this.lblDisplayProfile.Text = "Profile Graphics";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1108, 358);
      this.Controls.Add(this.lblDisplayProfile);
      this.Controls.Add(this.lblDisplayProcessingRegion);
      this.Controls.Add(this.lblDisplayDefects);
      this.Controls.Add(this.ProfileDisplay);
      this.Controls.Add(this.RegionDisplay);
      this.Controls.Add(this.DefectDisplay);
      this.Controls.Add(this.btnShowControl);
      this.Controls.Add(this.lblStepSize);
      this.Controls.Add(this.numericUpDown1);
      this.Controls.Add(this.chkFreezeOnDefect);
      this.Controls.Add(this.btnNextImage);
      this.Controls.Add(this.btStartProcessing);
      this.Name = "Form1";
      this.Text = "Form1";
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.DefectDisplay)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.RegionDisplay)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ProfileDisplay)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btStartProcessing;
    private System.Windows.Forms.Button btnNextImage;
    private System.Windows.Forms.CheckBox chkFreezeOnDefect;
    private System.Windows.Forms.NumericUpDown numericUpDown1;
    private System.Windows.Forms.Label lblStepSize;
    private System.Windows.Forms.Button btnShowControl;
    private Cognex.VisionPro.CogRecordDisplay DefectDisplay;
    private Cognex.VisionPro.CogRecordDisplay RegionDisplay;
    private Cognex.VisionPro.CogRecordDisplay ProfileDisplay;
    private System.Windows.Forms.Label lblDisplayDefects;
    private System.Windows.Forms.Label lblDisplayProcessingRegion;
    private System.Windows.Forms.Label lblDisplayProfile;
  }
}

