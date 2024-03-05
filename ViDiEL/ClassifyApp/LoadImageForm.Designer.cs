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
  partial class LoadImageForm
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
      this.mImageFileEdit = new Cognex.VisionPro.ImageFile.CogImageFileEditV2();
      ((System.ComponentModel.ISupportInitialize)(this.mImageFileEdit)).BeginInit();
      this.SuspendLayout();
      // 
      // mImageFileEdit
      // 
      this.mImageFileEdit.AllowDrop = true;
      this.mImageFileEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.mImageFileEdit.Location = new System.Drawing.Point(2, 3);
      this.mImageFileEdit.MinimumSize = new System.Drawing.Size(665, 0);
      this.mImageFileEdit.Name = "mImageFileEdit";
      this.mImageFileEdit.OutputHighLight = System.Drawing.Color.Lime;
      this.mImageFileEdit.Size = new System.Drawing.Size(1022, 467);
      this.mImageFileEdit.SuspendElectricRuns = false;
      this.mImageFileEdit.TabIndex = 0;
      // 
      // LoadImageForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1028, 473);
      this.Controls.Add(this.mImageFileEdit);
      this.Name = "LoadImageForm";
      this.Text = "Load Image From File";
      ((System.ComponentModel.ISupportInitialize)(this.mImageFileEdit)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    public Cognex.VisionPro.ImageFile.CogImageFileEditV2 mImageFileEdit;
  }
}
