namespace ToolBlockLoad
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;


    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.cogToolBlockEditV21 = new Cognex.VisionPro.ToolBlock.CogToolBlockEditV2();
      this.cogRecordDisplay1 = new Cognex.VisionPro.CogRecordDisplay();
      this.btnRun = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.radImageFile = new System.Windows.Forms.RadioButton();
      this.radCamera = new System.Windows.Forms.RadioButton();
      this.label4 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.nAreaLow = new System.Windows.Forms.NumericUpDown();
      this.nAreaHigh = new System.Windows.Forms.NumericUpDown();
      this.label6 = new System.Windows.Forms.Label();
      this.nPass = new System.Windows.Forms.Label();
      this.nFail = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEditV21)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nAreaLow)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nAreaHigh)).BeginInit();
      this.SuspendLayout();
      // 
      // cogToolBlockEditV21
      // 
      this.cogToolBlockEditV21.AllowDrop = true;
      this.cogToolBlockEditV21.ContextMenuCustomizer = null;
      this.cogToolBlockEditV21.Location = new System.Drawing.Point(490, 0);
      this.cogToolBlockEditV21.MinimumSize = new System.Drawing.Size(489, 0);
      this.cogToolBlockEditV21.Name = "cogToolBlockEditV21";
      this.cogToolBlockEditV21.ShowNodeToolTips = true;
      this.cogToolBlockEditV21.Size = new System.Drawing.Size(489, 478);
      this.cogToolBlockEditV21.SuspendElectricRuns = false;
      this.cogToolBlockEditV21.TabIndex = 0;
      // 
      // cogRecordDisplay1
      // 
      this.cogRecordDisplay1.Location = new System.Drawing.Point(0, 0);
      this.cogRecordDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
      this.cogRecordDisplay1.MouseWheelSensitivity = 1;
      this.cogRecordDisplay1.Name = "cogRecordDisplay1";
      this.cogRecordDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogRecordDisplay1.OcxState")));
      this.cogRecordDisplay1.Size = new System.Drawing.Size(484, 478);
      this.cogRecordDisplay1.TabIndex = 1;
      // 
      // btnRun
      // 
      this.btnRun.Location = new System.Drawing.Point(445, 518);
      this.btnRun.Name = "btnRun";
      this.btnRun.Size = new System.Drawing.Size(97, 24);
      this.btnRun.TabIndex = 3;
      this.btnRun.Text = "Run Once";
      this.btnRun.UseVisualStyleBackColor = true;
      this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(668, 512);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(33, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Pass:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(12, 492);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(42, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "Inputs";
      // 
      // radImageFile
      // 
      this.radImageFile.AutoSize = true;
      this.radImageFile.Checked = true;
      this.radImageFile.Location = new System.Drawing.Point(15, 518);
      this.radImageFile.Name = "radImageFile";
      this.radImageFile.Size = new System.Drawing.Size(73, 17);
      this.radImageFile.TabIndex = 8;
      this.radImageFile.TabStop = true;
      this.radImageFile.Text = "Image File";
      this.radImageFile.UseVisualStyleBackColor = true;
      // 
      // radCamera
      // 
      this.radCamera.AutoSize = true;
      this.radCamera.Location = new System.Drawing.Point(15, 541);
      this.radCamera.Name = "radCamera";
      this.radCamera.Size = new System.Drawing.Size(61, 17);
      this.radCamera.TabIndex = 9;
      this.radCamera.Text = "Camera";
      this.radCamera.UseVisualStyleBackColor = true;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(668, 533);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(26, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Fail:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(139, 518);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(110, 13);
      this.label1.TabIndex = 12;
      this.label1.Text = "Area Low Filter Value:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(139, 543);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(112, 13);
      this.label5.TabIndex = 13;
      this.label5.Text = "Area High Filter Value:";
      // 
      // nAreaLow
      // 
      this.nAreaLow.Location = new System.Drawing.Point(255, 512);
      this.nAreaLow.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
      this.nAreaLow.Name = "nAreaLow";
      this.nAreaLow.Size = new System.Drawing.Size(120, 20);
      this.nAreaLow.TabIndex = 14;
      this.nAreaLow.Value = new decimal(new int[] {
            5050,
            0,
            0,
            0});
      this.nAreaLow.ValueChanged += new System.EventHandler(this.nAreaLow_ValueChanged);
      // 
      // nAreaHigh
      // 
      this.nAreaHigh.Location = new System.Drawing.Point(255, 538);
      this.nAreaHigh.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
      this.nAreaHigh.Name = "nAreaHigh";
      this.nAreaHigh.Size = new System.Drawing.Size(120, 20);
      this.nAreaHigh.TabIndex = 15;
      this.nAreaHigh.Value = new decimal(new int[] {
            8050,
            0,
            0,
            0});
      this.nAreaHigh.ValueChanged += new System.EventHandler(this.nAreaHigh_ValueChanged);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label6.Location = new System.Drawing.Point(663, 490);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(52, 13);
      this.label6.TabIndex = 16;
      this.label6.Text = "OutPuts";
      // 
      // nPass
      // 
      this.nPass.AutoSize = true;
      this.nPass.Location = new System.Drawing.Point(730, 513);
      this.nPass.Name = "nPass";
      this.nPass.Size = new System.Drawing.Size(13, 13);
      this.nPass.TabIndex = 17;
      this.nPass.Text = "0";
      // 
      // nFail
      // 
      this.nFail.AutoSize = true;
      this.nFail.Location = new System.Drawing.Point(730, 533);
      this.nFail.Name = "nFail";
      this.nFail.Size = new System.Drawing.Size(13, 13);
      this.nFail.TabIndex = 18;
      this.nFail.Text = "0";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(880, 570);
      this.Controls.Add(this.nFail);
      this.Controls.Add(this.nPass);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.nAreaHigh);
      this.Controls.Add(this.nAreaLow);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.radCamera);
      this.Controls.Add(this.radImageFile);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.btnRun);
      this.Controls.Add(this.cogRecordDisplay1);
      this.Controls.Add(this.cogToolBlockEditV21);
      this.Name = "Form1";
      this.Text = "Form1";
      ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEditV21)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nAreaLow)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nAreaHigh)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Cognex.VisionPro.ToolBlock.CogToolBlockEditV2 cogToolBlockEditV21;
    private Cognex.VisionPro.CogRecordDisplay cogRecordDisplay1;
    private System.Windows.Forms.Button btnRun;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.RadioButton radImageFile;
    private System.Windows.Forms.RadioButton radCamera;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.NumericUpDown nAreaLow;
    private System.Windows.Forms.NumericUpDown nAreaHigh;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label nPass;
    private System.Windows.Forms.Label nFail;
  }
}

