namespace IDProcessControlMetrics
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
            this.ctrlToolDisplay = new Cognex.VisionPro.CogToolDisplay();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.radAIMDPM = new System.Windows.Forms.RadioButton();
            this.radISO15415 = new System.Windows.Forms.RadioButton();
            this.radSEMIT10 = new System.Windows.Forms.RadioButton();
            this.grpResults = new System.Windows.Forms.GroupBox();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.Metric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Score = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.SuspendLayout();
            // 
            // ctrlToolDisplay
            // 
            this.ctrlToolDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlToolDisplay.Location = new System.Drawing.Point(418, 12);
            this.ctrlToolDisplay.Name = "ctrlToolDisplay";
            this.ctrlToolDisplay.SelectedRecordKey = null;
            this.ctrlToolDisplay.ShowRecordsDropDown = true;
            this.ctrlToolDisplay.Size = new System.Drawing.Size(385, 418);
            this.ctrlToolDisplay.TabIndex = 0;
            this.ctrlToolDisplay.Tool = null;
            this.ctrlToolDisplay.ToolSyncObject = null;
            this.ctrlToolDisplay.UserRecord = null;
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(12, 442);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDescription.Size = new System.Drawing.Size(791, 82);
            this.txtDescription.TabIndex = 1;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(283, 12);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(100, 32);
            this.btnRun.TabIndex = 4;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // radAIMDPM
            // 
            this.radAIMDPM.AutoSize = true;
            this.radAIMDPM.Location = new System.Drawing.Point(22, 12);
            this.radAIMDPM.Name = "radAIMDPM";
            this.radAIMDPM.Size = new System.Drawing.Size(68, 17);
            this.radAIMDPM.TabIndex = 5;
            this.radAIMDPM.TabStop = true;
            this.radAIMDPM.Text = "AIMDPM";
            this.radAIMDPM.UseVisualStyleBackColor = true;
            this.radAIMDPM.CheckedChanged += new System.EventHandler(this.radAIMDPM_CheckedChanged);
            // 
            // radISO15415
            // 
            this.radISO15415.AutoSize = true;
            this.radISO15415.Location = new System.Drawing.Point(22, 35);
            this.radISO15415.Name = "radISO15415";
            this.radISO15415.Size = new System.Drawing.Size(73, 17);
            this.radISO15415.TabIndex = 6;
            this.radISO15415.TabStop = true;
            this.radISO15415.Text = "ISO15415";
            this.radISO15415.UseVisualStyleBackColor = true;
            this.radISO15415.CheckedChanged += new System.EventHandler(this.radISO15415_CheckedChanged);
            // 
            // radSEMIT10
            // 
            this.radSEMIT10.AutoSize = true;
            this.radSEMIT10.Location = new System.Drawing.Point(22, 58);
            this.radSEMIT10.Name = "radSEMIT10";
            this.radSEMIT10.Size = new System.Drawing.Size(70, 17);
            this.radSEMIT10.TabIndex = 7;
            this.radSEMIT10.TabStop = true;
            this.radSEMIT10.Text = "SEMIT10";
            this.radSEMIT10.UseVisualStyleBackColor = true;
            this.radSEMIT10.CheckedChanged += new System.EventHandler(this.radSEMIT10_CheckedChanged);
            // 
            // grpResults
            // 
            this.grpResults.Controls.Add(this.dgvResults);
            this.grpResults.Location = new System.Drawing.Point(12, 81);
            this.grpResults.Name = "grpResults";
            this.grpResults.Size = new System.Drawing.Size(391, 355);
            this.grpResults.TabIndex = 8;
            this.grpResults.TabStop = false;
            this.grpResults.Text = "Results";
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.AllowUserToResizeRows = false;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Metric,
            this.Grade,
            this.Score});
            this.dgvResults.Location = new System.Drawing.Point(6, 19);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.Size = new System.Drawing.Size(378, 308);
            this.dgvResults.TabIndex = 0;
            // 
            // Metric
            // 
            this.Metric.HeaderText = "Metric";
            this.Metric.Name = "Metric";
            this.Metric.ReadOnly = true;
            this.Metric.Width = 225;
            // 
            // Grade
            // 
            this.Grade.HeaderText = "Grade";
            this.Grade.Name = "Grade";
            this.Grade.ReadOnly = true;
            this.Grade.Width = 50;
            // 
            // Score
            // 
            this.Score.HeaderText = "Score";
            this.Score.Name = "Score";
            this.Score.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 540);
            this.Controls.Add(this.grpResults);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.radSEMIT10);
            this.Controls.Add(this.radISO15415);
            this.Controls.Add(this.radAIMDPM);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.ctrlToolDisplay);
            this.MinimumSize = new System.Drawing.Size(677, 482);
            this.Name = "Form1";
            this.Text = "ID Process Control Metrics";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Cognex.VisionPro.CogToolDisplay ctrlToolDisplay;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.RadioButton radAIMDPM;
        private System.Windows.Forms.RadioButton radISO15415;
        private System.Windows.Forms.RadioButton radSEMIT10;
        private System.Windows.Forms.GroupBox grpResults;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn Metric;
        private System.Windows.Forms.DataGridViewTextBoxColumn Grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Score;
    }
}

