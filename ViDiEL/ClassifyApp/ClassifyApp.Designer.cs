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
  partial class ClassifyApp
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassifyApp));
      this.StartClassifyEditBtn = new System.Windows.Forms.Button();
      this.AcquireImageBtn = new System.Windows.Forms.Button();
      this.ImageFileBtn = new System.Windows.Forms.Button();
      this.RunConfiguredToolBtn = new System.Windows.Forms.Button();
      this.RunOnceBtn = new System.Windows.Forms.Button();
      this.DisplaysCtnr = new System.Windows.Forms.SplitContainer();
      this.mDisplay = new Cognex.VisionPro.Display.CogDisplay();
      this.imageLabel = new System.Windows.Forms.Label();
      this.mRecordsDisplay = new Cognex.VisionPro.CogRecordsDisplay();
      this.recordLabel = new System.Windows.Forms.Label();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
      ((System.ComponentModel.ISupportInitialize)(this.DisplaysCtnr)).BeginInit();
      this.DisplaysCtnr.Panel1.SuspendLayout();
      this.DisplaysCtnr.Panel2.SuspendLayout();
      this.DisplaysCtnr.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.mDisplay)).BeginInit();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // StartClassifyEditBtn
      // 
      this.StartClassifyEditBtn.Location = new System.Drawing.Point(17, 25);
      this.StartClassifyEditBtn.Margin = new System.Windows.Forms.Padding(2);
      this.StartClassifyEditBtn.Name = "StartClassifyEditBtn";
      this.StartClassifyEditBtn.Size = new System.Drawing.Size(145, 28);
      this.StartClassifyEditBtn.TabIndex = 0;
      this.StartClassifyEditBtn.Text = "Start ClassifyEdit";
      this.StartClassifyEditBtn.UseVisualStyleBackColor = true;
      this.StartClassifyEditBtn.Click += new System.EventHandler(this.StartClassifyEditBtn_Click);
      // 
      // AcquireImageBtn
      // 
      this.AcquireImageBtn.Location = new System.Drawing.Point(17, 57);
      this.AcquireImageBtn.Margin = new System.Windows.Forms.Padding(2);
      this.AcquireImageBtn.Name = "AcquireImageBtn";
      this.AcquireImageBtn.Size = new System.Drawing.Size(145, 28);
      this.AcquireImageBtn.TabIndex = 1;
      this.AcquireImageBtn.Text = "Acquire Image";
      this.AcquireImageBtn.UseVisualStyleBackColor = true;
      this.AcquireImageBtn.Click += new System.EventHandler(this.AcquireImageBtn_Click);
      // 
      // ImageFileBtn
      // 
      this.ImageFileBtn.Location = new System.Drawing.Point(17, 89);
      this.ImageFileBtn.Margin = new System.Windows.Forms.Padding(2);
      this.ImageFileBtn.Name = "ImageFileBtn";
      this.ImageFileBtn.Size = new System.Drawing.Size(145, 28);
      this.ImageFileBtn.TabIndex = 2;
      this.ImageFileBtn.Text = "Image file";
      this.ImageFileBtn.UseVisualStyleBackColor = true;
      this.ImageFileBtn.Click += new System.EventHandler(this.ImageFileBtn_Click);
      // 
      // RunConfiguredToolBtn
      // 
      this.RunConfiguredToolBtn.Location = new System.Drawing.Point(17, 121);
      this.RunConfiguredToolBtn.Margin = new System.Windows.Forms.Padding(2);
      this.RunConfiguredToolBtn.Name = "RunConfiguredToolBtn";
      this.RunConfiguredToolBtn.Size = new System.Drawing.Size(145, 28);
      this.RunConfiguredToolBtn.TabIndex = 3;
      this.RunConfiguredToolBtn.Text = "Run Configured Tool";
      this.RunConfiguredToolBtn.UseVisualStyleBackColor = true;
      this.RunConfiguredToolBtn.Click += new System.EventHandler(this.RunConfiguredToolBtn_Click);
      // 
      // RunOnceBtn
      // 
      this.RunOnceBtn.Location = new System.Drawing.Point(17, 153);
      this.RunOnceBtn.Margin = new System.Windows.Forms.Padding(2);
      this.RunOnceBtn.Name = "RunOnceBtn";
      this.RunOnceBtn.Size = new System.Drawing.Size(145, 28);
      this.RunOnceBtn.TabIndex = 4;
      this.RunOnceBtn.Text = "Run Once";
      this.RunOnceBtn.UseVisualStyleBackColor = true;
      this.RunOnceBtn.Click += new System.EventHandler(this.RunOnceBtn_Click);
      // 
      // DisplaysCtnr
      // 
      this.DisplaysCtnr.Location = new System.Drawing.Point(195, 25);
      this.DisplaysCtnr.Name = "DisplaysCtnr";
      // 
      // DisplaysCtnr.Panel1
      // 
      this.DisplaysCtnr.Panel1.BackColor = System.Drawing.SystemColors.ControlLight;
      this.DisplaysCtnr.Panel1.Controls.Add(this.mDisplay);
      this.DisplaysCtnr.Panel1.Controls.Add(this.imageLabel);
      // 
      // DisplaysCtnr.Panel2
      // 
      this.DisplaysCtnr.Panel2.BackColor = System.Drawing.SystemColors.ControlLight;
      this.DisplaysCtnr.Panel2.Controls.Add(this.mRecordsDisplay);
      this.DisplaysCtnr.Panel2.Controls.Add(this.recordLabel);
      this.DisplaysCtnr.Size = new System.Drawing.Size(594, 242);
      this.DisplaysCtnr.SplitterDistance = 282;
      this.DisplaysCtnr.SplitterWidth = 3;
      this.DisplaysCtnr.TabIndex = 5;
      // 
      // mDisplay
      // 
      this.mDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
      this.mDisplay.ColorMapLowerRoiLimit = 0D;
      this.mDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
      this.mDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
      this.mDisplay.ColorMapUpperRoiLimit = 1D;
      this.mDisplay.DoubleTapZoomCycleLength = 2;
      this.mDisplay.DoubleTapZoomSensitivity = 2.5D;
      this.mDisplay.Location = new System.Drawing.Point(3, 29);
      this.mDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
      this.mDisplay.MouseWheelSensitivity = 1D;
      this.mDisplay.Name = "mDisplay";
      this.mDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mDisplay.OcxState")));
      this.mDisplay.Size = new System.Drawing.Size(278, 212);
      this.mDisplay.TabIndex = 1;
      // 
      // imageLabel
      // 
      this.imageLabel.AutoSize = true;
      this.imageLabel.Location = new System.Drawing.Point(13, 7);
      this.imageLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.imageLabel.Name = "imageLabel";
      this.imageLabel.Size = new System.Drawing.Size(71, 15);
      this.imageLabel.TabIndex = 0;
      this.imageLabel.Text = "Input Image";
      // 
      // mRecordsDisplay
      // 
      this.mRecordsDisplay.Location = new System.Drawing.Point(3, 29);
      this.mRecordsDisplay.Name = "mRecordsDisplay";
      this.mRecordsDisplay.SelectedRecordKey = null;
      this.mRecordsDisplay.ShowRecordsDropDown = true;
      this.mRecordsDisplay.Size = new System.Drawing.Size(302, 212);
      this.mRecordsDisplay.Subject = null;
      this.mRecordsDisplay.TabIndex = 2;
      // 
      // recordLabel
      // 
      this.recordLabel.AutoSize = true;
      this.recordLabel.Location = new System.Drawing.Point(11, 7);
      this.recordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.recordLabel.Name = "recordLabel";
      this.recordLabel.Size = new System.Drawing.Size(85, 15);
      this.recordLabel.TabIndex = 1;
      this.recordLabel.Text = "Record Display";
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
      this.statusStrip1.Location = new System.Drawing.Point(0, 284);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(812, 22);
      this.statusStrip1.TabIndex = 6;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
      // 
      // toolStripStatusLabel2
      // 
      this.toolStripStatusLabel2.ForeColor = System.Drawing.SystemColors.Highlight;
      this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
      this.toolStripStatusLabel2.Size = new System.Drawing.Size(48, 17);
      this.toolStripStatusLabel2.Text = "Status : ";
      // 
      // ClassifyApp
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(812, 306);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.DisplaysCtnr);
      this.Controls.Add(this.RunOnceBtn);
      this.Controls.Add(this.RunConfiguredToolBtn);
      this.Controls.Add(this.ImageFileBtn);
      this.Controls.Add(this.AcquireImageBtn);
      this.Controls.Add(this.StartClassifyEditBtn);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "ClassifyApp";
      this.Text = "ClassifyApp";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnMainFormClosing);
      this.Load += new System.EventHandler(this.OnMainFormLoad);
      this.DisplaysCtnr.Panel1.ResumeLayout(false);
      this.DisplaysCtnr.Panel1.PerformLayout();
      this.DisplaysCtnr.Panel2.ResumeLayout(false);
      this.DisplaysCtnr.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DisplaysCtnr)).EndInit();
      this.DisplaysCtnr.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.mDisplay)).EndInit();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button StartClassifyEditBtn;
    private System.Windows.Forms.Button AcquireImageBtn;
    private System.Windows.Forms.Button ImageFileBtn;
    private System.Windows.Forms.Button RunConfiguredToolBtn;
    private System.Windows.Forms.Button RunOnceBtn;
    private System.Windows.Forms.SplitContainer DisplaysCtnr;
    private Cognex.VisionPro.Display.CogDisplay mDisplay;
    private System.Windows.Forms.Label imageLabel;
    private Cognex.VisionPro.CogRecordsDisplay mRecordsDisplay;
    private System.Windows.Forms.Label recordLabel;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
  }
}

