namespace OCVMax
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
          if (disposing)
            {
            if (components != null)
              components.Dispose();

            mToolDisplay.Dispose();

            } // if (disposing)

          base.Dispose(disposing);

          } // protectd override void Dispose(bool disposing)


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.mStartSetupButton = new System.Windows.Forms.Button();
            this.mRunButton = new System.Windows.Forms.Button();
            this.mToolDisplay = new Cognex.VisionPro.CogToolDisplay();
            this.mResultsTextBox = new System.Windows.Forms.TextBox();
            this.mDisplayStatusBar = new Cognex.VisionPro.CogDisplayStatusBarV2();
            this.mFinishSetupButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mDescriptionTextBox
            // 
            this.mDescriptionTextBox.Location = new System.Drawing.Point(5, 431);
            this.mDescriptionTextBox.Multiline = true;
            this.mDescriptionTextBox.Name = "mDescriptionTextBox";
            this.mDescriptionTextBox.ReadOnly = true;
            this.mDescriptionTextBox.Size = new System.Drawing.Size(709, 60);
            this.mDescriptionTextBox.TabIndex = 0;
            // 
            // mStartSetupButton
            // 
            this.mStartSetupButton.Enabled = false;
            this.mStartSetupButton.Location = new System.Drawing.Point(5, 356);
            this.mStartSetupButton.Name = "mStartSetupButton";
            this.mStartSetupButton.Size = new System.Drawing.Size(90, 54);
            this.mStartSetupButton.TabIndex = 1;
            this.mStartSetupButton.Text = "Start Setup";
            this.mStartSetupButton.UseVisualStyleBackColor = true;
            this.mStartSetupButton.Click += new System.EventHandler(this.mStartSetupButton_Click);
            // 
            // mRunButton
            // 
            this.mRunButton.Enabled = false;
            this.mRunButton.Location = new System.Drawing.Point(220, 355);
            this.mRunButton.Name = "mRunButton";
            this.mRunButton.Size = new System.Drawing.Size(90, 54);
            this.mRunButton.TabIndex = 2;
            this.mRunButton.Text = "Run";
            this.mRunButton.UseVisualStyleBackColor = true;
            this.mRunButton.Click += new System.EventHandler(this.mRunButton_Click);
            // 
            // mToolDisplay
            // 
            this.mToolDisplay.Location = new System.Drawing.Point(342, 10);
            this.mToolDisplay.Name = "mToolDisplay";
            this.mToolDisplay.SelectedRecordKey = null;
            this.mToolDisplay.Size = new System.Drawing.Size(371, 376);
            this.mToolDisplay.TabIndex = 3;
            this.mToolDisplay.Tool = null;
            this.mToolDisplay.ToolSyncObject = null;
            this.mToolDisplay.UserRecord = null;
            // 
            // mResultsTextBox
            // 
            this.mResultsTextBox.Location = new System.Drawing.Point(5, 10);
            this.mResultsTextBox.Multiline = true;
            this.mResultsTextBox.Name = "mResultsTextBox";
            this.mResultsTextBox.ReadOnly = true;
            this.mResultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mResultsTextBox.Size = new System.Drawing.Size(305, 319);
            this.mResultsTextBox.TabIndex = 4;
            // 
            // mDisplayStatusBar
            // 
            this.mDisplayStatusBar.Enabled = true;
            this.mDisplayStatusBar.Location = new System.Drawing.Point(342, 385);
            this.mDisplayStatusBar.Name = "mDisplayStatusBar";
            this.mDisplayStatusBar.Size = new System.Drawing.Size(370, 24);
            this.mDisplayStatusBar.TabIndex = 5;
            // 
            // mFinishSetupButton
            // 
            this.mFinishSetupButton.Enabled = false;
            this.mFinishSetupButton.Location = new System.Drawing.Point(112, 356);
            this.mFinishSetupButton.Name = "mFinishSetupButton";
            this.mFinishSetupButton.Size = new System.Drawing.Size(90, 54);
            this.mFinishSetupButton.TabIndex = 6;
            this.mFinishSetupButton.Text = "Finish Setup";
            this.mFinishSetupButton.UseVisualStyleBackColor = true;
            this.mFinishSetupButton.Click += new System.EventHandler(this.mFinishSetupButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 497);
            this.Controls.Add(this.mFinishSetupButton);
            this.Controls.Add(this.mDisplayStatusBar);
            this.Controls.Add(this.mResultsTextBox);
            this.Controls.Add(this.mToolDisplay);
            this.Controls.Add(this.mRunButton);
            this.Controls.Add(this.mStartSetupButton);
            this.Controls.Add(this.mDescriptionTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "OCVMax Sample";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox mDescriptionTextBox;
        private System.Windows.Forms.Button mStartSetupButton;
        private System.Windows.Forms.Button mRunButton;
        private Cognex.VisionPro.CogToolDisplay mToolDisplay;
        private System.Windows.Forms.TextBox mResultsTextBox;
        private Cognex.VisionPro.CogDisplayStatusBarV2 mDisplayStatusBar;
        private System.Windows.Forms.Button mFinishSetupButton;
    }
}

