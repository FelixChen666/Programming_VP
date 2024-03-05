namespace SimpleTool
{
    partial class SimpleToolEditV2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleToolEditV2));
            this.chkCopyTwice = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.sbpIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpStatusCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpStatusMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpProcessingTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpTotalTime)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tpgSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tbrButtons
            // 
            this.tbrButtons.Size = new System.Drawing.Size(748, 28);
            // 
            // sbpStatusMessage
            // 
            this.sbpStatusMessage.Width = 502;
            // 
            // imlButtons
            // 
            this.imlButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlButtons.ImageStream")));
            this.imlButtons.Images.SetKeyName(0, "");
            this.imlButtons.Images.SetKeyName(1, "");
            this.imlButtons.Images.SetKeyName(2, "");
            this.imlButtons.Images.SetKeyName(3, "");
            this.imlButtons.Images.SetKeyName(4, "");
            this.imlButtons.Images.SetKeyName(5, "");
            this.imlButtons.Images.SetKeyName(6, "");
            this.imlButtons.Images.SetKeyName(7, "");
            this.imlButtons.Images.SetKeyName(8, "");
            this.imlButtons.Images.SetKeyName(9, "");
            this.imlButtons.Images.SetKeyName(10, "");
            // 
            // tabControl
            // 
            this.tabControl.Size = new System.Drawing.Size(489, 484);
            // 
            // tpgSettings
            // 
            this.tpgSettings.Controls.Add(this.chkCopyTwice);
            this.tpgSettings.Size = new System.Drawing.Size(481, 458);
            // 
            // lblControlState
            // 
            this.lblControlState.Location = new System.Drawing.Point(8, 487);
            // 
            // chkCopyTwice
            // 
            this.chkCopyTwice.AutoSize = true;
            this.chkCopyTwice.Location = new System.Drawing.Point(28, 27);
            this.chkCopyTwice.Name = "chkCopyTwice";
            this.chkCopyTwice.Size = new System.Drawing.Size(88, 17);
            this.chkCopyTwice.TabIndex = 0;
            this.chkCopyTwice.Text = "Copy Twice?";
            this.chkCopyTwice.UseVisualStyleBackColor = true;
            this.chkCopyTwice.CheckedChanged += new System.EventHandler(this.chkCopyTwice_CheckedChanged);
            // 
            // SimpleToolEditV2
            // 
            this.Name = "SimpleToolEditV2";
            ((System.ComponentModel.ISupportInitialize)(this.sbpIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpStatusCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpStatusMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpProcessingTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpTotalTime)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tpgSettings.ResumeLayout(false);
            this.tpgSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkCopyTwice;
    }
}
