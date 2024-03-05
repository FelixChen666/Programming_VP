namespace SegmentApp
{
  partial class SegmentApp
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
      this.mTabCtrl = new System.Windows.Forms.TabControl();
      this.mTabPageMain = new System.Windows.Forms.TabPage();
      this.mRunBtn = new System.Windows.Forms.Button();
      this.mSegmentedRegionsLbl = new System.Windows.Forms.Label();
      this.cogRecordsDisplay1 = new Cognex.VisionPro.CogRecordsDisplay();
      this.tabPageSegment = new System.Windows.Forms.TabPage();
      this.mElementHost = new System.Windows.Forms.Integration.ElementHost();
      this.tabPageOther = new System.Windows.Forms.TabPage();
      this.mPostProcessingLabel = new System.Windows.Forms.Label();
      this.mInputImageLbl = new System.Windows.Forms.Label();
      this.mBlobEditCtrl = new Cognex.VisionPro.Blob.CogBlobEditV2();
      this.mImageFileCtrl = new Cognex.VisionPro.ImageFile.CogImageFileEditV2();
      this.mTabCtrl.SuspendLayout();
      this.mTabPageMain.SuspendLayout();
      this.tabPageSegment.SuspendLayout();
      this.tabPageOther.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.mBlobEditCtrl)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.mImageFileCtrl)).BeginInit();
      this.SuspendLayout();
      // 
      // mTabCtrl
      // 
      this.mTabCtrl.Controls.Add(this.mTabPageMain);
      this.mTabCtrl.Controls.Add(this.tabPageSegment);
      this.mTabCtrl.Controls.Add(this.tabPageOther);
      this.mTabCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.mTabCtrl.Location = new System.Drawing.Point(0, 0);
      this.mTabCtrl.Name = "mTabCtrl";
      this.mTabCtrl.SelectedIndex = 0;
      this.mTabCtrl.Size = new System.Drawing.Size(1384, 811);
      this.mTabCtrl.TabIndex = 0;
      // 
      // mTabPageMain
      // 
      this.mTabPageMain.Controls.Add(this.mRunBtn);
      this.mTabPageMain.Controls.Add(this.mSegmentedRegionsLbl);
      this.mTabPageMain.Controls.Add(this.cogRecordsDisplay1);
      this.mTabPageMain.Location = new System.Drawing.Point(4, 22);
      this.mTabPageMain.Name = "mTabPageMain";
      this.mTabPageMain.Padding = new System.Windows.Forms.Padding(3);
      this.mTabPageMain.Size = new System.Drawing.Size(1376, 785);
      this.mTabPageMain.TabIndex = 0;
      this.mTabPageMain.Text = "Main";
      this.mTabPageMain.UseVisualStyleBackColor = true;
      // 
      // mRunBtn
      // 
      this.mRunBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.mRunBtn.Location = new System.Drawing.Point(50, 99);
      this.mRunBtn.Name = "mRunBtn";
      this.mRunBtn.Size = new System.Drawing.Size(350, 59);
      this.mRunBtn.TabIndex = 4;
      this.mRunBtn.Text = "Run";
      this.mRunBtn.UseVisualStyleBackColor = true;
      this.mRunBtn.Click += new System.EventHandler(this.OnRunBtnClick);
      // 
      // mSegmentedRegionsLbl
      // 
      this.mSegmentedRegionsLbl.AutoSize = true;
      this.mSegmentedRegionsLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.mSegmentedRegionsLbl.Location = new System.Drawing.Point(43, 265);
      this.mSegmentedRegionsLbl.Name = "mSegmentedRegionsLbl";
      this.mSegmentedRegionsLbl.Size = new System.Drawing.Size(398, 42);
      this.mSegmentedRegionsLbl.TabIndex = 3;
      this.mSegmentedRegionsLbl.Text = "Segmented Regions: 0";
      // 
      // cogRecordsDisplay1
      // 
      this.cogRecordsDisplay1.Location = new System.Drawing.Point(500, 16);
      this.cogRecordsDisplay1.Name = "cogRecordsDisplay1";
      this.cogRecordsDisplay1.SelectedRecordKey = null;
      this.cogRecordsDisplay1.ShowRecordsDropDown = false;
      this.cogRecordsDisplay1.Size = new System.Drawing.Size(804, 745);
      this.cogRecordsDisplay1.Subject = null;
      this.cogRecordsDisplay1.TabIndex = 0;
      // 
      // tabPageSegment
      // 
      this.tabPageSegment.Controls.Add(this.mElementHost);
      this.tabPageSegment.Location = new System.Drawing.Point(4, 22);
      this.tabPageSegment.Name = "tabPageSegment";
      this.tabPageSegment.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageSegment.Size = new System.Drawing.Size(1376, 785);
      this.tabPageSegment.TabIndex = 1;
      this.tabPageSegment.Text = "Segment";
      this.tabPageSegment.UseVisualStyleBackColor = true;
      // 
      // mElementHost
      // 
      this.mElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
      this.mElementHost.Location = new System.Drawing.Point(3, 3);
      this.mElementHost.Name = "mElementHost";
      this.mElementHost.Size = new System.Drawing.Size(1370, 779);
      this.mElementHost.TabIndex = 0;
      this.mElementHost.Text = "elementHost1";
      this.mElementHost.Child = null;
      // 
      // tabPageOther
      // 
      this.tabPageOther.Controls.Add(this.mPostProcessingLabel);
      this.tabPageOther.Controls.Add(this.mInputImageLbl);
      this.tabPageOther.Controls.Add(this.mBlobEditCtrl);
      this.tabPageOther.Controls.Add(this.mImageFileCtrl);
      this.tabPageOther.Location = new System.Drawing.Point(4, 22);
      this.tabPageOther.Name = "tabPageOther";
      this.tabPageOther.Size = new System.Drawing.Size(1376, 785);
      this.tabPageOther.TabIndex = 2;
      this.tabPageOther.Text = "Other";
      this.tabPageOther.UseVisualStyleBackColor = true;
      // 
      // mPostProcessingLabel
      // 
      this.mPostProcessingLabel.AutoSize = true;
      this.mPostProcessingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.mPostProcessingLabel.Location = new System.Drawing.Point(8, 519);
      this.mPostProcessingLabel.Name = "mPostProcessingLabel";
      this.mPostProcessingLabel.Size = new System.Drawing.Size(299, 42);
      this.mPostProcessingLabel.TabIndex = 3;
      this.mPostProcessingLabel.Text = "Post Processing:";
      // 
      // mInputImageLbl
      // 
      this.mInputImageLbl.AutoSize = true;
      this.mInputImageLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.mInputImageLbl.Location = new System.Drawing.Point(8, 151);
      this.mInputImageLbl.Name = "mInputImageLbl";
      this.mInputImageLbl.Size = new System.Drawing.Size(222, 42);
      this.mInputImageLbl.TabIndex = 2;
      this.mInputImageLbl.Text = "Input Image:";
      // 
      // mBlobEditCtrl
      // 
      this.mBlobEditCtrl.Location = new System.Drawing.Point(374, 375);
      this.mBlobEditCtrl.MinimumSize = new System.Drawing.Size(489, 0);
      this.mBlobEditCtrl.Name = "mBlobEditCtrl";
      this.mBlobEditCtrl.Size = new System.Drawing.Size(865, 402);
      this.mBlobEditCtrl.SuspendElectricRuns = false;
      this.mBlobEditCtrl.TabIndex = 1;
      this.mBlobEditCtrl.SubjectChanged += new System.EventHandler(this.OnBlobEditCtrlSubjectChanged);
      // 
      // mImageFileCtrl
      // 
      this.mImageFileCtrl.AllowDrop = true;
      this.mImageFileCtrl.Location = new System.Drawing.Point(374, 29);
      this.mImageFileCtrl.MinimumSize = new System.Drawing.Size(489, 0);
      this.mImageFileCtrl.Name = "mImageFileCtrl";
      this.mImageFileCtrl.OutputHighLight = System.Drawing.Color.Lime;
      this.mImageFileCtrl.Size = new System.Drawing.Size(865, 329);
      this.mImageFileCtrl.SuspendElectricRuns = false;
      this.mImageFileCtrl.TabIndex = 0;
      this.mImageFileCtrl.SubjectChanged += new System.EventHandler(this.OnImageFileCtrlSubjectChanged);
      // 
      // SegmentApp
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1384, 811);
      this.Controls.Add(this.mTabCtrl);
      this.Name = "SegmentApp";
      this.Text = "SegmentApp";
      this.Load += new System.EventHandler(this.OnMainFormLoad);
      this.mTabCtrl.ResumeLayout(false);
      this.mTabPageMain.ResumeLayout(false);
      this.mTabPageMain.PerformLayout();
      this.tabPageSegment.ResumeLayout(false);
      this.tabPageOther.ResumeLayout(false);
      this.tabPageOther.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.mBlobEditCtrl)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.mImageFileCtrl)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl mTabCtrl;
    private System.Windows.Forms.TabPage mTabPageMain;
    private Cognex.VisionPro.CogRecordsDisplay cogRecordsDisplay1;
    private System.Windows.Forms.TabPage tabPageSegment;
    private System.Windows.Forms.TabPage tabPageOther;
    private System.Windows.Forms.Integration.ElementHost mElementHost;
    private Cognex.VisionPro.Blob.CogBlobEditV2 mBlobEditCtrl;
    private Cognex.VisionPro.ImageFile.CogImageFileEditV2 mImageFileCtrl;
    private System.Windows.Forms.Label mPostProcessingLabel;
    private System.Windows.Forms.Label mInputImageLbl;
    private System.Windows.Forms.Button mRunBtn;
    private System.Windows.Forms.Label mSegmentedRegionsLbl;
  }
}

