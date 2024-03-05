namespace AdvancedAppOne
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
            this.RunOnceButton = new System.Windows.Forms.Button();
            this.RunContCheckBox = new System.Windows.Forms.CheckBox();
            this.ShowTrainCheckBox = new System.Windows.Forms.CheckBox();
            this.RetrainButton = new System.Windows.Forms.Button();
            this.cogRecordDisplay1 = new Cognex.VisionPro.CogRecordDisplay();
            this.cogDisplayStatusBar1 = new Cognex.VisionPro.CogDisplayStatusBarV2();
            this.RunStatusTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).BeginInit();
            this.SuspendLayout();
            // 
            // RunOnceButton
            // 
            this.RunOnceButton.Location = new System.Drawing.Point(15, 23);
            this.RunOnceButton.Name = "RunOnceButton";
            this.RunOnceButton.Size = new System.Drawing.Size(93, 25);
            this.RunOnceButton.TabIndex = 0;
            this.RunOnceButton.Text = "Run Once";
            this.RunOnceButton.UseVisualStyleBackColor = true;
            this.RunOnceButton.Click += new System.EventHandler(this.RunOnceButton_Click);
            // 
            // RunContCheckBox
            // 
            this.RunContCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.RunContCheckBox.AutoSize = true;
            this.RunContCheckBox.Location = new System.Drawing.Point(15, 63);
            this.RunContCheckBox.Name = "RunContCheckBox";
            this.RunContCheckBox.Size = new System.Drawing.Size(93, 23);
            this.RunContCheckBox.TabIndex = 1;
            this.RunContCheckBox.Text = "Run Continuous";
            this.RunContCheckBox.UseVisualStyleBackColor = true;
            this.RunContCheckBox.CheckedChanged += new System.EventHandler(this.RunContCheckBox_CheckedChanged);
            // 
            // ShowTrainCheckBox
            // 
            this.ShowTrainCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.ShowTrainCheckBox.AutoSize = true;
            this.ShowTrainCheckBox.Location = new System.Drawing.Point(19, 285);
            this.ShowTrainCheckBox.Name = "ShowTrainCheckBox";
            this.ShowTrainCheckBox.Size = new System.Drawing.Size(117, 23);
            this.ShowTrainCheckBox.TabIndex = 2;
            this.ShowTrainCheckBox.Text = "Show Training Image";
            this.ShowTrainCheckBox.UseVisualStyleBackColor = true;
            this.ShowTrainCheckBox.CheckedChanged += new System.EventHandler(this.ShowTrainCheckBox_CheckedChanged);
            // 
            // RetrainButton
            // 
            this.RetrainButton.Enabled = false;
            this.RetrainButton.Location = new System.Drawing.Point(20, 314);
            this.RetrainButton.Name = "RetrainButton";
            this.RetrainButton.Size = new System.Drawing.Size(116, 23);
            this.RetrainButton.TabIndex = 3;
            this.RetrainButton.Text = "Retrain";
            this.RetrainButton.UseVisualStyleBackColor = true;
            this.RetrainButton.Click += new System.EventHandler(this.RetrainButton_Click);
            // 
            // cogRecordDisplay1
            // 
            this.cogRecordDisplay1.Location = new System.Drawing.Point(161, 28);
            this.cogRecordDisplay1.Name = "cogRecordDisplay1";
            this.cogRecordDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogRecordDisplay1.OcxState")));
            this.cogRecordDisplay1.Size = new System.Drawing.Size(316, 308);
            this.cogRecordDisplay1.TabIndex = 4;
            // 
            // cogDisplayStatusBar1
            // 
            this.cogDisplayStatusBar1.Enabled = true;
            this.cogDisplayStatusBar1.Location = new System.Drawing.Point(161, 345);
            this.cogDisplayStatusBar1.Name = "cogDisplayStatusBar1";
            this.cogDisplayStatusBar1.Size = new System.Drawing.Size(315, 19);
            this.cogDisplayStatusBar1.TabIndex = 5;
            // 
            // RunStatusTextBox
            // 
            this.RunStatusTextBox.Location = new System.Drawing.Point(12, 383);
            this.RunStatusTextBox.Name = "RunStatusTextBox";
            this.RunStatusTextBox.Size = new System.Drawing.Size(464, 20);
            this.RunStatusTextBox.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 418);
            this.Controls.Add(this.RunStatusTextBox);
            this.Controls.Add(this.cogDisplayStatusBar1);
            this.Controls.Add(this.cogRecordDisplay1);
            this.Controls.Add(this.RetrainButton);
            this.Controls.Add(this.ShowTrainCheckBox);
            this.Controls.Add(this.RunContCheckBox);
            this.Controls.Add(this.RunOnceButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RunOnceButton;
        private System.Windows.Forms.CheckBox RunContCheckBox;
        private System.Windows.Forms.CheckBox ShowTrainCheckBox;
        private System.Windows.Forms.Button RetrainButton;
        private Cognex.VisionPro.CogRecordDisplay cogRecordDisplay1;
        private Cognex.VisionPro.CogDisplayStatusBarV2 cogDisplayStatusBar1;
        private System.Windows.Forms.TextBox RunStatusTextBox;
    }
}

