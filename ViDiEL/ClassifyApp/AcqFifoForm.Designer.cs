//*****************************************************************************
// Copyright (C) 2023 Cognex Corporation
//
// Subject to Cognex Corporation's terms and conditions and license
// agreement, you are authorized to use and modify this source code in
// any way you find useful, provided the Software and/or the modified
// Software is used solely in conjunction with a Cognex Machine Vision
// System.  Furthermore you acknowledge and agree that Cognex has no
// warranty, obligations or liability for your use of the Software.
//*****************************************************************************

namespace ClassifyApp
{
  partial class AcqFifoForm
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
      this.mAcqFifoEdit = new Cognex.VisionPro.CogAcqFifoEditV2();
      ((System.ComponentModel.ISupportInitialize)(this.mAcqFifoEdit)).BeginInit();
      this.SuspendLayout();
      // 
      // mAcqFifoEdit
      // 
      this.mAcqFifoEdit.Location = new System.Drawing.Point(2, 12);
      this.mAcqFifoEdit.MinimumSize = new System.Drawing.Size(489, 0);
      this.mAcqFifoEdit.Name = "mAcqFifoEdit";
      this.mAcqFifoEdit.Size = new System.Drawing.Size(894, 421);
      this.mAcqFifoEdit.SuspendElectricRuns = false;
      this.mAcqFifoEdit.TabIndex = 0;
      // 
      // AcqFifoForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(910, 436);
      this.Controls.Add(this.mAcqFifoEdit);
      this.Name = "AcqFifoForm";
      this.Text = "Image Acquisition";
      ((System.ComponentModel.ISupportInitialize)(this.mAcqFifoEdit)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    public Cognex.VisionPro.CogAcqFifoEditV2 mAcqFifoEdit;
  }
}
