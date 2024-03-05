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
  partial class ControlForm
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
      this.cog3DRangeImageCrossSectionEditV21 = new Cognex.VisionPro3D.Cog3DRangeImageCrossSectionEditV2();
      ((System.ComponentModel.ISupportInitialize)(this.cog3DRangeImageCrossSectionEditV21)).BeginInit();
      this.SuspendLayout();
      // 
      // cog3DRangeImageCrossSectionEditV21
      // 
      this.cog3DRangeImageCrossSectionEditV21.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cog3DRangeImageCrossSectionEditV21.Location = new System.Drawing.Point(0, 0);
      this.cog3DRangeImageCrossSectionEditV21.MinimumSize = new System.Drawing.Size(489, 0);
      this.cog3DRangeImageCrossSectionEditV21.Name = "cog3DRangeImageCrossSectionEditV21";
      this.cog3DRangeImageCrossSectionEditV21.Size = new System.Drawing.Size(730, 409);
      this.cog3DRangeImageCrossSectionEditV21.SuspendElectricRuns = false;
      this.cog3DRangeImageCrossSectionEditV21.TabIndex = 0;
      // 
      // ControlForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(730, 409);
      this.Controls.Add(this.cog3DRangeImageCrossSectionEditV21);
      this.Name = "ControlForm";
      this.Text = "ControlForm";
      ((System.ComponentModel.ISupportInitialize)(this.cog3DRangeImageCrossSectionEditV21)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private Cognex.VisionPro3D.Cog3DRangeImageCrossSectionEditV2 cog3DRangeImageCrossSectionEditV21;
  }
}