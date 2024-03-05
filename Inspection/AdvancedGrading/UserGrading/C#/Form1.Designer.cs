namespace UserGrading
{
    partial class UserGradingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserGradingForm));
            this.mInputDatabaseLabel = new System.Windows.Forms.Label();
            this.mVerificationResultLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.cogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboGrades = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnMeasure = new System.Windows.Forms.Button();
            this.nudCOMY = new System.Windows.Forms.NumericUpDown();
            this.nudRCOMY = new System.Windows.Forms.NumericUpDown();
            this.nudCOMX = new System.Windows.Forms.NumericUpDown();
            this.nudRCOMX = new System.Windows.Forms.NumericUpDown();
            this.nudArea = new System.Windows.Forms.NumericUpDown();
            this.nudRArea = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblRecordName = new System.Windows.Forms.Label();
            this.scrlImages = new System.Windows.Forms.HScrollBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCOMY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRCOMY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCOMX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRCOMX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRArea)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // mInputDatabaseLabel
            // 
            this.mInputDatabaseLabel.Location = new System.Drawing.Point(6, 21);
            this.mInputDatabaseLabel.Name = "mInputDatabaseLabel";
            this.mInputDatabaseLabel.Size = new System.Drawing.Size(119, 18);
            this.mInputDatabaseLabel.TabIndex = 1;
            this.mInputDatabaseLabel.Text = "Input Database: ";
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
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.mInputDatabaseLabel);
            this.groupBox1.Location = new System.Drawing.Point(357, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 72);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Database";
            // 
            // btnClear
            // 
            this.btnClear.Enabled = false;
            this.btnClear.Location = new System.Drawing.Point(9, 42);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(272, 23);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Restore database";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cogDisplay1
            // 
            this.cogDisplay1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cogDisplay1.Location = new System.Drawing.Point(12, 12);
            this.cogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay1.MouseWheelSensitivity = 1;
            this.cogDisplay1.Name = "cogDisplay1";
            this.cogDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay1.OcxState")));
            this.cogDisplay1.Size = new System.Drawing.Size(339, 315);
            this.cogDisplay1.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cboGrades);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.btnMeasure);
            this.groupBox2.Controls.Add(this.nudCOMY);
            this.groupBox2.Controls.Add(this.nudRCOMY);
            this.groupBox2.Controls.Add(this.nudCOMX);
            this.groupBox2.Controls.Add(this.nudRCOMX);
            this.groupBox2.Controls.Add(this.nudArea);
            this.groupBox2.Controls.Add(this.nudRArea);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lblRecordName);
            this.groupBox2.Controls.Add(this.scrlImages);
            this.groupBox2.Location = new System.Drawing.Point(357, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(289, 237);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Record controls";
            // 
            // cboGrades
            // 
            this.cboGrades.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGrades.FormattingEnabled = true;
            this.cboGrades.Location = new System.Drawing.Point(120, 179);
            this.cboGrades.Name = "cboGrades";
            this.cboGrades.Size = new System.Drawing.Size(162, 21);
            this.cboGrades.TabIndex = 33;
            this.cboGrades.SelectedIndexChanged += new System.EventHandler(this.cboGrades_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 182);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 13);
            this.label10.TabIndex = 32;
            this.label10.Text = "Grade:";
            // 
            // btnMeasure
            // 
            this.btnMeasure.Enabled = false;
            this.btnMeasure.Location = new System.Drawing.Point(9, 206);
            this.btnMeasure.Name = "btnMeasure";
            this.btnMeasure.Size = new System.Drawing.Size(272, 25);
            this.btnMeasure.TabIndex = 25;
            this.btnMeasure.Text = "Measure image";
            this.btnMeasure.UseVisualStyleBackColor = true;
            this.btnMeasure.Click += new System.EventHandler(this.btnMeasure_Click);
            // 
            // nudCOMY
            // 
            this.nudCOMY.DecimalPlaces = 2;
            this.nudCOMY.Location = new System.Drawing.Point(120, 149);
            this.nudCOMY.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudCOMY.Name = "nudCOMY";
            this.nudCOMY.Size = new System.Drawing.Size(93, 20);
            this.nudCOMY.TabIndex = 24;
            this.nudCOMY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudCOMY.ThousandsSeparator = true;
            this.nudCOMY.ValueChanged += new System.EventHandler(this.nudCOMY_ValueChanged);
            // 
            // nudRCOMY
            // 
            this.nudRCOMY.DecimalPlaces = 2;
            this.nudRCOMY.Location = new System.Drawing.Point(233, 149);
            this.nudRCOMY.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudRCOMY.Name = "nudRCOMY";
            this.nudRCOMY.Size = new System.Drawing.Size(49, 20);
            this.nudRCOMY.TabIndex = 23;
            this.nudRCOMY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudRCOMY.ThousandsSeparator = true;
            this.nudRCOMY.ValueChanged += new System.EventHandler(this.nudRCOMY_ValueChanged);
            // 
            // nudCOMX
            // 
            this.nudCOMX.DecimalPlaces = 2;
            this.nudCOMX.Location = new System.Drawing.Point(120, 117);
            this.nudCOMX.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudCOMX.Name = "nudCOMX";
            this.nudCOMX.Size = new System.Drawing.Size(93, 20);
            this.nudCOMX.TabIndex = 22;
            this.nudCOMX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudCOMX.ThousandsSeparator = true;
            this.nudCOMX.ValueChanged += new System.EventHandler(this.nudCOMX_ValueChanged);
            // 
            // nudRCOMX
            // 
            this.nudRCOMX.DecimalPlaces = 2;
            this.nudRCOMX.Location = new System.Drawing.Point(233, 117);
            this.nudRCOMX.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudRCOMX.Name = "nudRCOMX";
            this.nudRCOMX.Size = new System.Drawing.Size(49, 20);
            this.nudRCOMX.TabIndex = 21;
            this.nudRCOMX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudRCOMX.ThousandsSeparator = true;
            this.nudRCOMX.ValueChanged += new System.EventHandler(this.nudRCOMX_ValueChanged);
            // 
            // nudArea
            // 
            this.nudArea.DecimalPlaces = 2;
            this.nudArea.Location = new System.Drawing.Point(120, 85);
            this.nudArea.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudArea.Name = "nudArea";
            this.nudArea.Size = new System.Drawing.Size(93, 20);
            this.nudArea.TabIndex = 20;
            this.nudArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudArea.ThousandsSeparator = true;
            this.nudArea.ValueChanged += new System.EventHandler(this.nudArea_ValueChanged);
            // 
            // nudRArea
            // 
            this.nudRArea.DecimalPlaces = 2;
            this.nudRArea.Location = new System.Drawing.Point(232, 85);
            this.nudRArea.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudRArea.Name = "nudRArea";
            this.nudRArea.Size = new System.Drawing.Size(49, 20);
            this.nudRArea.TabIndex = 19;
            this.nudRArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudRArea.ThousandsSeparator = true;
            this.nudRArea.ValueChanged += new System.EventHandler(this.nudRArea_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(222, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "±";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(222, 119);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "±";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(221, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "±";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Blob center of mass Y:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Blob center of mass X:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Blob area:";
            // 
            // lblRecordName
            // 
            this.lblRecordName.AutoSize = true;
            this.lblRecordName.Location = new System.Drawing.Point(6, 62);
            this.lblRecordName.Name = "lblRecordName";
            this.lblRecordName.Size = new System.Drawing.Size(10, 13);
            this.lblRecordName.TabIndex = 14;
            this.lblRecordName.Text = "-";
            // 
            // scrlImages
            // 
            this.scrlImages.Enabled = false;
            this.scrlImages.LargeChange = 1;
            this.scrlImages.Location = new System.Drawing.Point(9, 21);
            this.scrlImages.Maximum = 10;
            this.scrlImages.Minimum = 1;
            this.scrlImages.Name = "scrlImages";
            this.scrlImages.Size = new System.Drawing.Size(273, 29);
            this.scrlImages.TabIndex = 13;
            this.scrlImages.Value = 1;
            this.scrlImages.ValueChanged += new System.EventHandler(this.scrlImages_ValueChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Location = new System.Drawing.Point(652, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(172, 314);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Usage";
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(3, 16);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(166, 295);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // UserGradingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 339);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cogDisplay1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mVerificationResultLabel);
            this.Name = "UserGradingForm";
            this.Text = "User Grading Sample Code";
            this.Load += new System.EventHandler(this.UserGradingForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserGradingForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCOMY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRCOMY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCOMX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRCOMX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRArea)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label mInputDatabaseLabel;
        private System.Windows.Forms.Label mVerificationResultLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.HScrollBar scrlImages;
        private System.Windows.Forms.Label lblRecordName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMeasure;
        private System.Windows.Forms.NumericUpDown nudCOMY;
        private System.Windows.Forms.NumericUpDown nudRCOMY;
        private System.Windows.Forms.NumericUpDown nudCOMX;
        private System.Windows.Forms.NumericUpDown nudRCOMX;
        private System.Windows.Forms.NumericUpDown nudArea;
        private System.Windows.Forms.NumericUpDown nudRArea;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboGrades;
        private System.Windows.Forms.TextBox textBox1;
    }
}

