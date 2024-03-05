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
  partial class ClassifyEditForm
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
      this.mElementHost = new System.Windows.Forms.Integration.ElementHost();
      this.SuspendLayout();
      // 
      // mElementHost
      // 
      this.mElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
      this.mElementHost.Location = new System.Drawing.Point(0, 0);
      this.mElementHost.Name = "mElementHost";
      this.mElementHost.Size = new System.Drawing.Size(1450, 725);
      this.mElementHost.TabIndex = 0;
      this.mElementHost.Text = "mElementHost";
      this.mElementHost.Child = null;
      // 
      // ClassifyEditForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1450, 725);
      this.Controls.Add(this.mElementHost);
      this.Name = "ClassifyEditForm";
      this.Text = "Classify Edit";
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.Integration.ElementHost mElementHost;
  }
}
