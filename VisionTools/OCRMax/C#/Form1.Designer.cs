namespace OCRMaxSample
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
            this.grpFielding = new System.Windows.Forms.GroupBox();
            this.grpResult = new System.Windows.Forms.GroupBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.txtFieldString = new System.Windows.Forms.TextBox();
            this.chkFieldingAliasAlpha = new System.Windows.Forms.CheckBox();
            this.chkFieldingAliasNumeric = new System.Windows.Forms.CheckBox();
            this.chkFieldingAliasAny = new System.Windows.Forms.CheckBox();
            this.grpFielding.SuspendLayout();
            this.grpResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctrlToolDisplay
            // 
            this.ctrlToolDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlToolDisplay.Location = new System.Drawing.Point(345, 12);
            this.ctrlToolDisplay.Name = "ctrlToolDisplay";
            this.ctrlToolDisplay.SelectedRecordKey = null;
            this.ctrlToolDisplay.ShowRecordsDropDown = true;
            this.ctrlToolDisplay.Size = new System.Drawing.Size(312, 319);
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
            this.txtDescription.Location = new System.Drawing.Point(12, 356);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDescription.Size = new System.Drawing.Size(645, 82);
            this.txtDescription.TabIndex = 1;
            // 
            // grpFielding
            // 
            this.grpFielding.Controls.Add(this.chkFieldingAliasAny);
            this.grpFielding.Controls.Add(this.chkFieldingAliasNumeric);
            this.grpFielding.Controls.Add(this.chkFieldingAliasAlpha);
            this.grpFielding.Controls.Add(this.txtFieldString);
            this.grpFielding.Location = new System.Drawing.Point(12, 12);
            this.grpFielding.Name = "grpFielding";
            this.grpFielding.Size = new System.Drawing.Size(317, 154);
            this.grpFielding.TabIndex = 2;
            this.grpFielding.TabStop = false;
            this.grpFielding.Text = "Fielding";
            // 
            // grpResult
            // 
            this.grpResult.Controls.Add(this.txtResult);
            this.grpResult.Controls.Add(this.label1);
            this.grpResult.Location = new System.Drawing.Point(12, 188);
            this.grpResult.Name = "grpResult";
            this.grpResult.Size = new System.Drawing.Size(317, 73);
            this.grpResult.TabIndex = 3;
            this.grpResult.TabStop = false;
            this.grpResult.Text = "Result";
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(12, 285);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(124, 46);
            this.btnRun.TabIndex = 4;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 0;
            // 
            // txtResult
            // 
            this.txtResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResult.Location = new System.Drawing.Point(6, 29);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtResult.Size = new System.Drawing.Size(305, 22);
            this.txtResult.TabIndex = 1;
            // 
            // txtFieldString
            // 
            this.txtFieldString.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFieldString.Location = new System.Drawing.Point(6, 28);
            this.txtFieldString.Name = "txtFieldString";
            this.txtFieldString.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtFieldString.Size = new System.Drawing.Size(305, 22);
            this.txtFieldString.TabIndex = 0;
            this.txtFieldString.TextChanged += new System.EventHandler(this.txtFieldString_TextChanged);
            // 
            // chkFieldingAliasAlpha
            // 
            this.chkFieldingAliasAlpha.AutoSize = true;
            this.chkFieldingAliasAlpha.Location = new System.Drawing.Point(6, 70);
            this.chkFieldingAliasAlpha.Name = "chkFieldingAliasAlpha";
            this.chkFieldingAliasAlpha.Size = new System.Drawing.Size(233, 17);
            this.chkFieldingAliasAlpha.TabIndex = 1;
            this.chkFieldingAliasAlpha.Text = "A = ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            this.chkFieldingAliasAlpha.UseVisualStyleBackColor = true;
            this.chkFieldingAliasAlpha.CheckedChanged += new System.EventHandler(this.chkFieldingAliasAlpha_CheckedChanged);
            // 
            // chkFieldingAliasNumeric
            // 
            this.chkFieldingAliasNumeric.AutoSize = true;
            this.chkFieldingAliasNumeric.Location = new System.Drawing.Point(6, 93);
            this.chkFieldingAliasNumeric.Name = "chkFieldingAliasNumeric";
            this.chkFieldingAliasNumeric.Size = new System.Drawing.Size(106, 17);
            this.chkFieldingAliasNumeric.TabIndex = 2;
            this.chkFieldingAliasNumeric.Text = "N = 0123456789";
            this.chkFieldingAliasNumeric.UseVisualStyleBackColor = true;
            this.chkFieldingAliasNumeric.CheckedChanged += new System.EventHandler(this.chkFieldingAliasNumeric_CheckedChanged);
            // 
            // chkFieldingAliasAny
            // 
            this.chkFieldingAliasAny.AutoSize = true;
            this.chkFieldingAliasAny.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFieldingAliasAny.Location = new System.Drawing.Point(6, 116);
            this.chkFieldingAliasAny.Name = "chkFieldingAliasAny";
            this.chkFieldingAliasAny.Size = new System.Drawing.Size(136, 20);
            this.chkFieldingAliasAny.TabIndex = 3;
            this.chkFieldingAliasAny.Text = "* = {any character}";
            this.chkFieldingAliasAny.UseVisualStyleBackColor = true;
            this.chkFieldingAliasAny.CheckedChanged += new System.EventHandler(this.chkFieldingAliasAny_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 455);
            this.Controls.Add(this.grpResult);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.grpFielding);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.ctrlToolDisplay);
            this.MinimumSize = new System.Drawing.Size(677, 482);
            this.Name = "Form1";
            this.Text = "OCRMax Sample";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpFielding.ResumeLayout(false);
            this.grpFielding.PerformLayout();
            this.grpResult.ResumeLayout(false);
            this.grpResult.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Cognex.VisionPro.CogToolDisplay ctrlToolDisplay;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.GroupBox grpFielding;
        private System.Windows.Forms.GroupBox grpResult;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.CheckBox chkFieldingAliasAlpha;
        private System.Windows.Forms.TextBox txtFieldString;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkFieldingAliasAny;
        private System.Windows.Forms.CheckBox chkFieldingAliasNumeric;
    }
}

