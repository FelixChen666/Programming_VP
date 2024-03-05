namespace UserVerification
{
    partial class UserVerificationForm
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
            this.mVerifyButton = new System.Windows.Forms.Button();
            this.mInputDatabaseLabel = new System.Windows.Forms.Label();
            this.mOutputDatabaseLabel = new System.Windows.Forms.Label();
            this.mToolBlockLabel = new System.Windows.Forms.Label();
            this.mVerificationResultLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mBlobThresholdNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.mTotalLabel = new System.Windows.Forms.Label();
            this.mMatchedLabel = new System.Windows.Forms.Label();
            this.mMismatchedLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mBlobThresholdNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // mVerifyButton
            // 
            this.mVerifyButton.Location = new System.Drawing.Point(12, 212);
            this.mVerifyButton.Name = "mVerifyButton";
            this.mVerifyButton.Size = new System.Drawing.Size(75, 42);
            this.mVerifyButton.TabIndex = 0;
            this.mVerifyButton.Text = "Verify";
            this.mVerifyButton.UseVisualStyleBackColor = true;
            this.mVerifyButton.Click += new System.EventHandler(this.mVerifyButton_Click);
            // 
            // mInputDatabaseLabel
            // 
            this.mInputDatabaseLabel.AutoSize = true;
            this.mInputDatabaseLabel.Location = new System.Drawing.Point(6, 17);
            this.mInputDatabaseLabel.Name = "mInputDatabaseLabel";
            this.mInputDatabaseLabel.Size = new System.Drawing.Size(80, 13);
            this.mInputDatabaseLabel.TabIndex = 1;
            this.mInputDatabaseLabel.Text = "Input Database";
            // 
            // mOutputDatabaseLabel
            // 
            this.mOutputDatabaseLabel.AutoSize = true;
            this.mOutputDatabaseLabel.Location = new System.Drawing.Point(6, 16);
            this.mOutputDatabaseLabel.Name = "mOutputDatabaseLabel";
            this.mOutputDatabaseLabel.Size = new System.Drawing.Size(88, 13);
            this.mOutputDatabaseLabel.TabIndex = 2;
            this.mOutputDatabaseLabel.Text = "Output Database";
            // 
            // mToolBlockLabel
            // 
            this.mToolBlockLabel.AutoSize = true;
            this.mToolBlockLabel.Location = new System.Drawing.Point(6, 16);
            this.mToolBlockLabel.Name = "mToolBlockLabel";
            this.mToolBlockLabel.Size = new System.Drawing.Size(124, 13);
            this.mToolBlockLabel.TabIndex = 3;
            this.mToolBlockLabel.Text = "CogToolBlock under test";
            // 
            // mVerificationResultLabel
            // 
            this.mVerificationResultLabel.AutoSize = true;
            this.mVerificationResultLabel.BackColor = System.Drawing.SystemColors.Control;
            this.mVerificationResultLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mVerificationResultLabel.Location = new System.Drawing.Point(93, 212);
            this.mVerificationResultLabel.Name = "mVerificationResultLabel";
            this.mVerificationResultLabel.Size = new System.Drawing.Size(0, 42);
            this.mVerificationResultLabel.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mInputDatabaseLabel);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(614, 39);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Database";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mOutputDatabaseLabel);
            this.groupBox2.Location = new System.Drawing.Point(12, 57);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(614, 39);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output Database";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.mBlobThresholdNumericUpDown);
            this.groupBox3.Controls.Add(this.mToolBlockLabel);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(12, 102);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(614, 68);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "CogToolBlock under test";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(400, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Change this threshold to 10, 100, or 255 and press Verify to see a verification f" +
                "ailure";
            // 
            // mBlobThresholdNumericUpDown
            // 
            this.mBlobThresholdNumericUpDown.Location = new System.Drawing.Point(103, 42);
            this.mBlobThresholdNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.mBlobThresholdNumericUpDown.Name = "mBlobThresholdNumericUpDown";
            this.mBlobThresholdNumericUpDown.Size = new System.Drawing.Size(77, 20);
            this.mBlobThresholdNumericUpDown.TabIndex = 9;
            this.mBlobThresholdNumericUpDown.ValueChanged += new System.EventHandler(this.mNumBlobsNumericUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Blob Threshold:";
            // 
            // mTotalLabel
            // 
            this.mTotalLabel.AutoSize = true;
            this.mTotalLabel.Location = new System.Drawing.Point(12, 182);
            this.mTotalLabel.Name = "mTotalLabel";
            this.mTotalLabel.Size = new System.Drawing.Size(43, 13);
            this.mTotalLabel.TabIndex = 8;
            this.mTotalLabel.Text = "Total: 0";
            // 
            // mMatchedLabel
            // 
            this.mMatchedLabel.AutoSize = true;
            this.mMatchedLabel.Location = new System.Drawing.Point(299, 182);
            this.mMatchedLabel.Name = "mMatchedLabel";
            this.mMatchedLabel.Size = new System.Drawing.Size(49, 13);
            this.mMatchedLabel.TabIndex = 9;
            this.mMatchedLabel.Text = "Match: 0";
            // 
            // mMismatchedLabel
            // 
            this.mMismatchedLabel.AutoSize = true;
            this.mMismatchedLabel.Location = new System.Drawing.Point(562, 182);
            this.mMismatchedLabel.Name = "mMismatchedLabel";
            this.mMismatchedLabel.Size = new System.Drawing.Size(64, 13);
            this.mMismatchedLabel.TabIndex = 10;
            this.mMismatchedLabel.Text = "Mismatch: 0";
            // 
            // UserVerificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 266);
            this.Controls.Add(this.mMismatchedLabel);
            this.Controls.Add(this.mMatchedLabel);
            this.Controls.Add(this.mTotalLabel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mVerificationResultLabel);
            this.Controls.Add(this.mVerifyButton);
            this.Name = "UserVerificationForm";
            this.Text = "User Verification Sample Code";
            this.Load += new System.EventHandler(this.UserVerificationForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserVerificationForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mBlobThresholdNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button mVerifyButton;
        private System.Windows.Forms.Label mInputDatabaseLabel;
        private System.Windows.Forms.Label mOutputDatabaseLabel;
        private System.Windows.Forms.Label mToolBlockLabel;
        private System.Windows.Forms.Label mVerificationResultLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown mBlobThresholdNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label mTotalLabel;
        private System.Windows.Forms.Label mMatchedLabel;
        private System.Windows.Forms.Label mMismatchedLabel;
    }
}

