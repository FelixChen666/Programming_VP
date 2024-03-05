/*******************************************************************************
 Copyright (C) 2014 Cognex Corporation

 Subject to Cognex Corporations terms and conditions and license agreement,
 you are authorized to use and modify this source code in any way you find
 useful, provided the Software and/or the modified Software is used solely in
 conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
 and agree that Cognex has no warranty, obligations or liability for your use
 of the Software.
*******************************************************************************
 This sample program is designed to illustrate certain VisionPro features or 
 techniques in the simplest way possible. It is not intended as the framework 
 for a complete application. In particular, the sample program may not provide
 proper error handling, event handling, cleanup, repeatability, and other 
 mechanisms that a commercial quality application requires.
 * 
 */
namespace BasicDiscreteIO
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
      this.chkOutput0 = new System.Windows.Forms.CheckBox();
      this.chkOutput1 = new System.Windows.Forms.CheckBox();
      this.chkOutput2 = new System.Windows.Forms.CheckBox();
      this.chkOutput3 = new System.Windows.Forms.CheckBox();
      this.chkOutput7 = new System.Windows.Forms.CheckBox();
      this.chkOutput6 = new System.Windows.Forms.CheckBox();
      this.chkOutput5 = new System.Windows.Forms.CheckBox();
      this.chkOutput4 = new System.Windows.Forms.CheckBox();
      this.chkOutput11 = new System.Windows.Forms.CheckBox();
      this.chkOutput10 = new System.Windows.Forms.CheckBox();
      this.chkOutput9 = new System.Windows.Forms.CheckBox();
      this.chkOutput8 = new System.Windows.Forms.CheckBox();
      this.chkOutput15 = new System.Windows.Forms.CheckBox();
      this.chkOutput14 = new System.Windows.Forms.CheckBox();
      this.chkOutput13 = new System.Windows.Forms.CheckBox();
      this.chkOutput12 = new System.Windows.Forms.CheckBox();
      this.chkDsOutput1 = new System.Windows.Forms.CheckBox();
      this.chkDsOutput0 = new System.Windows.Forms.CheckBox();
      this.chkInput7 = new System.Windows.Forms.CheckBox();
      this.chkInput6 = new System.Windows.Forms.CheckBox();
      this.chkInput5 = new System.Windows.Forms.CheckBox();
      this.chkInput4 = new System.Windows.Forms.CheckBox();
      this.chkInput3 = new System.Windows.Forms.CheckBox();
      this.chkInput2 = new System.Windows.Forms.CheckBox();
      this.chkInput1 = new System.Windows.Forms.CheckBox();
      this.chkInput0 = new System.Windows.Forms.CheckBox();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.lblLog = new System.Windows.Forms.Label();
      this.grpInputBank0 = new System.Windows.Forms.GroupBox();
      this.grpOutputBank0 = new System.Windows.Forms.GroupBox();
      this.btnPulseOutput = new System.Windows.Forms.Button();
      this.numLineToPulse = new System.Windows.Forms.NumericUpDown();
      this.grpDSOutputBank0 = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.chkDsOutput7 = new System.Windows.Forms.CheckBox();
      this.chkDsOutput6 = new System.Windows.Forms.CheckBox();
      this.chkDsOutput5 = new System.Windows.Forms.CheckBox();
      this.chkDsOutput4 = new System.Windows.Forms.CheckBox();
      this.chkDsOutput3 = new System.Windows.Forms.CheckBox();
      this.chkDsOutput2 = new System.Windows.Forms.CheckBox();
      this.btnReadState = new System.Windows.Forms.Button();
      this.btnClearLog = new System.Windows.Forms.Button();
      this.grpInputBank0.SuspendLayout();
      this.grpOutputBank0.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numLineToPulse)).BeginInit();
      this.grpDSOutputBank0.SuspendLayout();
      this.SuspendLayout();
      // 
      // chkOutput0
      // 
      this.chkOutput0.AutoSize = true;
      this.chkOutput0.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput0.Location = new System.Drawing.Point(16, 16);
      this.chkOutput0.Name = "chkOutput0";
      this.chkOutput0.Size = new System.Drawing.Size(17, 31);
      this.chkOutput0.TabIndex = 0;
      this.chkOutput0.Tag = "0";
      this.chkOutput0.Text = "0";
      this.chkOutput0.UseVisualStyleBackColor = true;
      this.chkOutput0.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput1
      // 
      this.chkOutput1.AutoSize = true;
      this.chkOutput1.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput1.Location = new System.Drawing.Point(36, 16);
      this.chkOutput1.Name = "chkOutput1";
      this.chkOutput1.Size = new System.Drawing.Size(17, 31);
      this.chkOutput1.TabIndex = 1;
      this.chkOutput1.Tag = "1";
      this.chkOutput1.Text = "1";
      this.chkOutput1.UseVisualStyleBackColor = true;
      this.chkOutput1.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput2
      // 
      this.chkOutput2.AutoSize = true;
      this.chkOutput2.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput2.Location = new System.Drawing.Point(59, 16);
      this.chkOutput2.Name = "chkOutput2";
      this.chkOutput2.Size = new System.Drawing.Size(17, 31);
      this.chkOutput2.TabIndex = 2;
      this.chkOutput2.Tag = "2";
      this.chkOutput2.Text = "2";
      this.chkOutput2.UseVisualStyleBackColor = true;
      this.chkOutput2.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput3
      // 
      this.chkOutput3.AutoSize = true;
      this.chkOutput3.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput3.Location = new System.Drawing.Point(79, 16);
      this.chkOutput3.Name = "chkOutput3";
      this.chkOutput3.Size = new System.Drawing.Size(17, 31);
      this.chkOutput3.TabIndex = 3;
      this.chkOutput3.Tag = "3";
      this.chkOutput3.Text = "3";
      this.chkOutput3.UseVisualStyleBackColor = true;
      this.chkOutput3.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput7
      // 
      this.chkOutput7.AutoSize = true;
      this.chkOutput7.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput7.Location = new System.Drawing.Point(155, 16);
      this.chkOutput7.Name = "chkOutput7";
      this.chkOutput7.Size = new System.Drawing.Size(17, 31);
      this.chkOutput7.TabIndex = 7;
      this.chkOutput7.Tag = "7";
      this.chkOutput7.Text = "7";
      this.chkOutput7.UseVisualStyleBackColor = true;
      this.chkOutput7.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput6
      // 
      this.chkOutput6.AutoSize = true;
      this.chkOutput6.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput6.Location = new System.Drawing.Point(139, 16);
      this.chkOutput6.Name = "chkOutput6";
      this.chkOutput6.Size = new System.Drawing.Size(17, 31);
      this.chkOutput6.TabIndex = 6;
      this.chkOutput6.Tag = "6";
      this.chkOutput6.Text = "6";
      this.chkOutput6.UseVisualStyleBackColor = true;
      this.chkOutput6.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput5
      // 
      this.chkOutput5.AutoSize = true;
      this.chkOutput5.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput5.Location = new System.Drawing.Point(119, 16);
      this.chkOutput5.Name = "chkOutput5";
      this.chkOutput5.Size = new System.Drawing.Size(17, 31);
      this.chkOutput5.TabIndex = 5;
      this.chkOutput5.Tag = "5";
      this.chkOutput5.Text = "5";
      this.chkOutput5.UseVisualStyleBackColor = true;
      this.chkOutput5.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput4
      // 
      this.chkOutput4.AutoSize = true;
      this.chkOutput4.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput4.Location = new System.Drawing.Point(99, 16);
      this.chkOutput4.Name = "chkOutput4";
      this.chkOutput4.Size = new System.Drawing.Size(17, 31);
      this.chkOutput4.TabIndex = 4;
      this.chkOutput4.Tag = "4";
      this.chkOutput4.Text = "4";
      this.chkOutput4.UseVisualStyleBackColor = true;
      this.chkOutput4.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput11
      // 
      this.chkOutput11.AutoSize = true;
      this.chkOutput11.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput11.Location = new System.Drawing.Point(76, 53);
      this.chkOutput11.Name = "chkOutput11";
      this.chkOutput11.Size = new System.Drawing.Size(23, 31);
      this.chkOutput11.TabIndex = 11;
      this.chkOutput11.Tag = "11";
      this.chkOutput11.Text = "11";
      this.chkOutput11.UseVisualStyleBackColor = true;
      this.chkOutput11.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput10
      // 
      this.chkOutput10.AutoSize = true;
      this.chkOutput10.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput10.Location = new System.Drawing.Point(56, 53);
      this.chkOutput10.Name = "chkOutput10";
      this.chkOutput10.Size = new System.Drawing.Size(23, 31);
      this.chkOutput10.TabIndex = 10;
      this.chkOutput10.Tag = "10";
      this.chkOutput10.Text = "10";
      this.chkOutput10.UseVisualStyleBackColor = true;
      this.chkOutput10.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput9
      // 
      this.chkOutput9.AutoSize = true;
      this.chkOutput9.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput9.Location = new System.Drawing.Point(36, 53);
      this.chkOutput9.Name = "chkOutput9";
      this.chkOutput9.Size = new System.Drawing.Size(17, 31);
      this.chkOutput9.TabIndex = 9;
      this.chkOutput9.Tag = "9";
      this.chkOutput9.Text = "9";
      this.chkOutput9.UseVisualStyleBackColor = true;
      this.chkOutput9.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput8
      // 
      this.chkOutput8.AutoSize = true;
      this.chkOutput8.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput8.Location = new System.Drawing.Point(16, 53);
      this.chkOutput8.Name = "chkOutput8";
      this.chkOutput8.Size = new System.Drawing.Size(17, 31);
      this.chkOutput8.TabIndex = 8;
      this.chkOutput8.Tag = "8";
      this.chkOutput8.Text = "8";
      this.chkOutput8.UseVisualStyleBackColor = true;
      this.chkOutput8.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput15
      // 
      this.chkOutput15.AutoSize = true;
      this.chkOutput15.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput15.Location = new System.Drawing.Point(155, 53);
      this.chkOutput15.Name = "chkOutput15";
      this.chkOutput15.Size = new System.Drawing.Size(23, 31);
      this.chkOutput15.TabIndex = 15;
      this.chkOutput15.Tag = "15";
      this.chkOutput15.Text = "15";
      this.chkOutput15.UseVisualStyleBackColor = true;
      this.chkOutput15.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput14
      // 
      this.chkOutput14.AutoSize = true;
      this.chkOutput14.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput14.Location = new System.Drawing.Point(136, 53);
      this.chkOutput14.Name = "chkOutput14";
      this.chkOutput14.Size = new System.Drawing.Size(23, 31);
      this.chkOutput14.TabIndex = 14;
      this.chkOutput14.Tag = "14";
      this.chkOutput14.Text = "14";
      this.chkOutput14.UseVisualStyleBackColor = true;
      this.chkOutput14.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput13
      // 
      this.chkOutput13.AutoSize = true;
      this.chkOutput13.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput13.Location = new System.Drawing.Point(116, 53);
      this.chkOutput13.Name = "chkOutput13";
      this.chkOutput13.Size = new System.Drawing.Size(23, 31);
      this.chkOutput13.TabIndex = 13;
      this.chkOutput13.Tag = "13";
      this.chkOutput13.Text = "13";
      this.chkOutput13.UseVisualStyleBackColor = true;
      this.chkOutput13.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkOutput12
      // 
      this.chkOutput12.AutoSize = true;
      this.chkOutput12.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkOutput12.Location = new System.Drawing.Point(96, 53);
      this.chkOutput12.Name = "chkOutput12";
      this.chkOutput12.Size = new System.Drawing.Size(23, 31);
      this.chkOutput12.TabIndex = 12;
      this.chkOutput12.Tag = "12";
      this.chkOutput12.Text = "12";
      this.chkOutput12.UseVisualStyleBackColor = true;
      this.chkOutput12.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkDsOutput1
      // 
      this.chkDsOutput1.AutoSize = true;
      this.chkDsOutput1.Location = new System.Drawing.Point(80, 37);
      this.chkDsOutput1.Name = "chkDsOutput1";
      this.chkDsOutput1.Size = new System.Drawing.Size(74, 17);
      this.chkDsOutput1.TabIndex = 17;
      this.chkDsOutput1.Tag = "1";
      this.chkDsOutput1.Text = "1 (Enable)";
      this.chkDsOutput1.UseVisualStyleBackColor = true;
      this.chkDsOutput1.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkDsOutput0
      // 
      this.chkDsOutput0.AutoSize = true;
      this.chkDsOutput0.Location = new System.Drawing.Point(80, 19);
      this.chkDsOutput0.Name = "chkDsOutput0";
      this.chkDsOutput0.Size = new System.Drawing.Size(74, 17);
      this.chkDsOutput0.TabIndex = 16;
      this.chkDsOutput0.Tag = "0";
      this.chkDsOutput0.Text = "0 (Enable)";
      this.chkDsOutput0.UseVisualStyleBackColor = true;
      this.chkDsOutput0.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkInput7
      // 
      this.chkInput7.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkInput7.AutoSize = true;
      this.chkInput7.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkInput7.Enabled = false;
      this.chkInput7.FlatAppearance.CheckedBackColor = System.Drawing.Color.LimeGreen;
      this.chkInput7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.chkInput7.Location = new System.Drawing.Point(178, 22);
      this.chkInput7.Name = "chkInput7";
      this.chkInput7.Size = new System.Drawing.Size(23, 23);
      this.chkInput7.TabIndex = 33;
      this.chkInput7.Text = "7";
      this.chkInput7.UseVisualStyleBackColor = true;
      // 
      // chkInput6
      // 
      this.chkInput6.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkInput6.AutoSize = true;
      this.chkInput6.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkInput6.Enabled = false;
      this.chkInput6.FlatAppearance.CheckedBackColor = System.Drawing.Color.LimeGreen;
      this.chkInput6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.chkInput6.Location = new System.Drawing.Point(154, 22);
      this.chkInput6.Name = "chkInput6";
      this.chkInput6.Size = new System.Drawing.Size(23, 23);
      this.chkInput6.TabIndex = 32;
      this.chkInput6.Text = "6";
      this.chkInput6.UseVisualStyleBackColor = true;
      // 
      // chkInput5
      // 
      this.chkInput5.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkInput5.AutoSize = true;
      this.chkInput5.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkInput5.Enabled = false;
      this.chkInput5.FlatAppearance.CheckedBackColor = System.Drawing.Color.LimeGreen;
      this.chkInput5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.chkInput5.Location = new System.Drawing.Point(130, 22);
      this.chkInput5.Name = "chkInput5";
      this.chkInput5.Size = new System.Drawing.Size(23, 23);
      this.chkInput5.TabIndex = 31;
      this.chkInput5.Text = "5";
      this.chkInput5.UseVisualStyleBackColor = true;
      // 
      // chkInput4
      // 
      this.chkInput4.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkInput4.AutoSize = true;
      this.chkInput4.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkInput4.Enabled = false;
      this.chkInput4.FlatAppearance.CheckedBackColor = System.Drawing.Color.LimeGreen;
      this.chkInput4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.chkInput4.Location = new System.Drawing.Point(106, 22);
      this.chkInput4.Name = "chkInput4";
      this.chkInput4.Size = new System.Drawing.Size(23, 23);
      this.chkInput4.TabIndex = 30;
      this.chkInput4.Text = "4";
      this.chkInput4.UseVisualStyleBackColor = true;
      // 
      // chkInput3
      // 
      this.chkInput3.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkInput3.AutoSize = true;
      this.chkInput3.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkInput3.Enabled = false;
      this.chkInput3.FlatAppearance.CheckedBackColor = System.Drawing.Color.LimeGreen;
      this.chkInput3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.chkInput3.Location = new System.Drawing.Point(82, 22);
      this.chkInput3.Name = "chkInput3";
      this.chkInput3.Size = new System.Drawing.Size(23, 23);
      this.chkInput3.TabIndex = 29;
      this.chkInput3.Text = "3";
      this.chkInput3.UseVisualStyleBackColor = true;
      // 
      // chkInput2
      // 
      this.chkInput2.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkInput2.AutoSize = true;
      this.chkInput2.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkInput2.Enabled = false;
      this.chkInput2.FlatAppearance.CheckedBackColor = System.Drawing.Color.LimeGreen;
      this.chkInput2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.chkInput2.Location = new System.Drawing.Point(58, 22);
      this.chkInput2.Name = "chkInput2";
      this.chkInput2.Size = new System.Drawing.Size(23, 23);
      this.chkInput2.TabIndex = 28;
      this.chkInput2.Text = "2";
      this.chkInput2.UseVisualStyleBackColor = true;
      // 
      // chkInput1
      // 
      this.chkInput1.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkInput1.AutoSize = true;
      this.chkInput1.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkInput1.Enabled = false;
      this.chkInput1.FlatAppearance.CheckedBackColor = System.Drawing.Color.LimeGreen;
      this.chkInput1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.chkInput1.Location = new System.Drawing.Point(34, 22);
      this.chkInput1.Name = "chkInput1";
      this.chkInput1.Size = new System.Drawing.Size(23, 23);
      this.chkInput1.TabIndex = 27;
      this.chkInput1.Text = "1";
      this.chkInput1.UseVisualStyleBackColor = true;
      // 
      // chkInput0
      // 
      this.chkInput0.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkInput0.AutoSize = true;
      this.chkInput0.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.chkInput0.Enabled = false;
      this.chkInput0.FlatAppearance.CheckedBackColor = System.Drawing.Color.LimeGreen;
      this.chkInput0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.chkInput0.Location = new System.Drawing.Point(10, 22);
      this.chkInput0.Name = "chkInput0";
      this.chkInput0.Size = new System.Drawing.Size(23, 23);
      this.chkInput0.TabIndex = 26;
      this.chkInput0.Text = "0";
      this.chkInput0.UseVisualStyleBackColor = true;
      // 
      // textBox1
      // 
      this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox1.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBox1.Location = new System.Drawing.Point(2, 225);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBox1.Size = new System.Drawing.Size(548, 291);
      this.textBox1.TabIndex = 35;
      // 
      // lblLog
      // 
      this.lblLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblLog.AutoSize = true;
      this.lblLog.Location = new System.Drawing.Point(9, 206);
      this.lblLog.Name = "lblLog";
      this.lblLog.Size = new System.Drawing.Size(28, 13);
      this.lblLog.TabIndex = 36;
      this.lblLog.Text = "Log:";
      // 
      // grpInputBank0
      // 
      this.grpInputBank0.Controls.Add(this.chkInput6);
      this.grpInputBank0.Controls.Add(this.chkInput0);
      this.grpInputBank0.Controls.Add(this.chkInput1);
      this.grpInputBank0.Controls.Add(this.chkInput2);
      this.grpInputBank0.Controls.Add(this.chkInput7);
      this.grpInputBank0.Controls.Add(this.chkInput3);
      this.grpInputBank0.Controls.Add(this.chkInput4);
      this.grpInputBank0.Controls.Add(this.chkInput5);
      this.grpInputBank0.Location = new System.Drawing.Point(12, 12);
      this.grpInputBank0.Name = "grpInputBank0";
      this.grpInputBank0.Size = new System.Drawing.Size(210, 59);
      this.grpInputBank0.TabIndex = 37;
      this.grpInputBank0.TabStop = false;
      this.grpInputBank0.Text = "Input Bank 0";
      this.grpInputBank0.Visible = false;
      // 
      // grpOutputBank0
      // 
      this.grpOutputBank0.Controls.Add(this.btnPulseOutput);
      this.grpOutputBank0.Controls.Add(this.numLineToPulse);
      this.grpOutputBank0.Controls.Add(this.chkOutput7);
      this.grpOutputBank0.Controls.Add(this.chkOutput0);
      this.grpOutputBank0.Controls.Add(this.chkOutput1);
      this.grpOutputBank0.Controls.Add(this.chkOutput2);
      this.grpOutputBank0.Controls.Add(this.chkOutput3);
      this.grpOutputBank0.Controls.Add(this.chkOutput4);
      this.grpOutputBank0.Controls.Add(this.chkOutput5);
      this.grpOutputBank0.Controls.Add(this.chkOutput15);
      this.grpOutputBank0.Controls.Add(this.chkOutput6);
      this.grpOutputBank0.Controls.Add(this.chkOutput14);
      this.grpOutputBank0.Controls.Add(this.chkOutput11);
      this.grpOutputBank0.Controls.Add(this.chkOutput13);
      this.grpOutputBank0.Controls.Add(this.chkOutput12);
      this.grpOutputBank0.Controls.Add(this.chkOutput8);
      this.grpOutputBank0.Controls.Add(this.chkOutput9);
      this.grpOutputBank0.Controls.Add(this.chkOutput10);
      this.grpOutputBank0.Location = new System.Drawing.Point(243, 12);
      this.grpOutputBank0.Name = "grpOutputBank0";
      this.grpOutputBank0.Size = new System.Drawing.Size(298, 101);
      this.grpOutputBank0.TabIndex = 38;
      this.grpOutputBank0.TabStop = false;
      this.grpOutputBank0.Text = "Output Bank 0";
      this.grpOutputBank0.Visible = false;
      // 
      // btnPulseOutput
      // 
      this.btnPulseOutput.Location = new System.Drawing.Point(205, 30);
      this.btnPulseOutput.Name = "btnPulseOutput";
      this.btnPulseOutput.Size = new System.Drawing.Size(87, 23);
      this.btnPulseOutput.TabIndex = 42;
      this.btnPulseOutput.Text = "Pulse Output";
      this.btnPulseOutput.UseVisualStyleBackColor = true;
      this.btnPulseOutput.Click += new System.EventHandler(this.btnPulseOutput_Click);
      // 
      // numLineToPulse
      // 
      this.numLineToPulse.Location = new System.Drawing.Point(222, 59);
      this.numLineToPulse.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
      this.numLineToPulse.Name = "numLineToPulse";
      this.numLineToPulse.Size = new System.Drawing.Size(49, 20);
      this.numLineToPulse.TabIndex = 43;
      // 
      // grpDSOutputBank0
      // 
      this.grpDSOutputBank0.Controls.Add(this.label2);
      this.grpDSOutputBank0.Controls.Add(this.label1);
      this.grpDSOutputBank0.Controls.Add(this.chkDsOutput7);
      this.grpDSOutputBank0.Controls.Add(this.chkDsOutput6);
      this.grpDSOutputBank0.Controls.Add(this.chkDsOutput5);
      this.grpDSOutputBank0.Controls.Add(this.chkDsOutput4);
      this.grpDSOutputBank0.Controls.Add(this.chkDsOutput3);
      this.grpDSOutputBank0.Controls.Add(this.chkDsOutput2);
      this.grpDSOutputBank0.Controls.Add(this.chkDsOutput0);
      this.grpDSOutputBank0.Controls.Add(this.chkDsOutput1);
      this.grpDSOutputBank0.Location = new System.Drawing.Point(12, 119);
      this.grpDSOutputBank0.Name = "grpDSOutputBank0";
      this.grpDSOutputBank0.Size = new System.Drawing.Size(529, 60);
      this.grpDSOutputBank0.TabIndex = 39;
      this.grpDSOutputBank0.TabStop = false;
      this.grpDSOutputBank0.Text = "DS1000 Output Bank 0";
      this.grpDSOutputBank0.Visible = false;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(7, 38);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(55, 13);
      this.label2.TabIndex = 25;
      this.label2.Text = "Camera 2:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(7, 19);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(55, 13);
      this.label1.TabIndex = 24;
      this.label1.Text = "Camera 1:";
      // 
      // chkDsOutput7
      // 
      this.chkDsOutput7.AutoSize = true;
      this.chkDsOutput7.Location = new System.Drawing.Point(425, 38);
      this.chkDsOutput7.Name = "chkDsOutput7";
      this.chkDsOutput7.Size = new System.Drawing.Size(71, 17);
      this.chkDsOutput7.TabIndex = 23;
      this.chkDsOutput7.Tag = "7";
      this.chkDsOutput7.Text = "7 (Power)";
      this.chkDsOutput7.UseVisualStyleBackColor = true;
      this.chkDsOutput7.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkDsOutput6
      // 
      this.chkDsOutput6.AutoSize = true;
      this.chkDsOutput6.Location = new System.Drawing.Point(425, 19);
      this.chkDsOutput6.Name = "chkDsOutput6";
      this.chkDsOutput6.Size = new System.Drawing.Size(71, 17);
      this.chkDsOutput6.TabIndex = 22;
      this.chkDsOutput6.Tag = "6";
      this.chkDsOutput6.Text = "6 (Power)";
      this.chkDsOutput6.UseVisualStyleBackColor = true;
      this.chkDsOutput6.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkDsOutput5
      // 
      this.chkDsOutput5.AutoSize = true;
      this.chkDsOutput5.Location = new System.Drawing.Point(306, 37);
      this.chkDsOutput5.Name = "chkDsOutput5";
      this.chkDsOutput5.Size = new System.Drawing.Size(74, 17);
      this.chkDsOutput5.TabIndex = 21;
      this.chkDsOutput5.Tag = "5";
      this.chkDsOutput5.Text = "5 (Control)";
      this.chkDsOutput5.UseVisualStyleBackColor = true;
      this.chkDsOutput5.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkDsOutput4
      // 
      this.chkDsOutput4.AutoSize = true;
      this.chkDsOutput4.Location = new System.Drawing.Point(306, 19);
      this.chkDsOutput4.Name = "chkDsOutput4";
      this.chkDsOutput4.Size = new System.Drawing.Size(74, 17);
      this.chkDsOutput4.TabIndex = 20;
      this.chkDsOutput4.Tag = "4";
      this.chkDsOutput4.Text = "4 (Control)";
      this.chkDsOutput4.UseVisualStyleBackColor = true;
      this.chkDsOutput4.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkDsOutput3
      // 
      this.chkDsOutput3.AutoSize = true;
      this.chkDsOutput3.Location = new System.Drawing.Point(186, 37);
      this.chkDsOutput3.Name = "chkDsOutput3";
      this.chkDsOutput3.Size = new System.Drawing.Size(74, 17);
      this.chkDsOutput3.TabIndex = 19;
      this.chkDsOutput3.Tag = "3";
      this.chkDsOutput3.Text = "3 (Trigger)";
      this.chkDsOutput3.UseVisualStyleBackColor = true;
      this.chkDsOutput3.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // chkDsOutput2
      // 
      this.chkDsOutput2.AutoSize = true;
      this.chkDsOutput2.Location = new System.Drawing.Point(186, 19);
      this.chkDsOutput2.Name = "chkDsOutput2";
      this.chkDsOutput2.Size = new System.Drawing.Size(74, 17);
      this.chkDsOutput2.TabIndex = 18;
      this.chkDsOutput2.Tag = "2";
      this.chkDsOutput2.Text = "2 (Trigger)";
      this.chkDsOutput2.UseVisualStyleBackColor = true;
      this.chkDsOutput2.CheckedChanged += new System.EventHandler(this.chkOutput_CheckedChanged);
      // 
      // btnReadState
      // 
      this.btnReadState.Location = new System.Drawing.Point(364, 196);
      this.btnReadState.Name = "btnReadState";
      this.btnReadState.Size = new System.Drawing.Size(89, 23);
      this.btnReadState.TabIndex = 40;
      this.btnReadState.Text = "Read State";
      this.btnReadState.UseVisualStyleBackColor = true;
      this.btnReadState.Visible = false;
      this.btnReadState.Click += new System.EventHandler(this.btnReadState_Click);
      // 
      // btnClearLog
      // 
      this.btnClearLog.Location = new System.Drawing.Point(461, 196);
      this.btnClearLog.Name = "btnClearLog";
      this.btnClearLog.Size = new System.Drawing.Size(89, 23);
      this.btnClearLog.TabIndex = 41;
      this.btnClearLog.Text = "Clear Log";
      this.btnClearLog.UseVisualStyleBackColor = true;
      this.btnClearLog.Visible = false;
      this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(553, 520);
      this.Controls.Add(this.btnClearLog);
      this.Controls.Add(this.btnReadState);
      this.Controls.Add(this.grpDSOutputBank0);
      this.Controls.Add(this.grpOutputBank0);
      this.Controls.Add(this.lblLog);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.grpInputBank0);
      this.Name = "Form1";
      this.Text = "BasicDiscreteIO";
      this.grpInputBank0.ResumeLayout(false);
      this.grpInputBank0.PerformLayout();
      this.grpOutputBank0.ResumeLayout(false);
      this.grpOutputBank0.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numLineToPulse)).EndInit();
      this.grpDSOutputBank0.ResumeLayout(false);
      this.grpDSOutputBank0.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkOutput0;
        private System.Windows.Forms.CheckBox chkOutput1;
        private System.Windows.Forms.CheckBox chkOutput2;
        private System.Windows.Forms.CheckBox chkOutput3;
        private System.Windows.Forms.CheckBox chkOutput7;
        private System.Windows.Forms.CheckBox chkOutput6;
        private System.Windows.Forms.CheckBox chkOutput5;
        private System.Windows.Forms.CheckBox chkOutput4;
        private System.Windows.Forms.CheckBox chkOutput11;
        private System.Windows.Forms.CheckBox chkOutput10;
        private System.Windows.Forms.CheckBox chkOutput9;
        private System.Windows.Forms.CheckBox chkOutput8;
        private System.Windows.Forms.CheckBox chkOutput15;
        private System.Windows.Forms.CheckBox chkOutput14;
        private System.Windows.Forms.CheckBox chkOutput13;
        private System.Windows.Forms.CheckBox chkOutput12;
        private System.Windows.Forms.CheckBox chkDsOutput1;
        private System.Windows.Forms.CheckBox chkDsOutput0;
        private System.Windows.Forms.CheckBox chkInput7;
        private System.Windows.Forms.CheckBox chkInput6;
        private System.Windows.Forms.CheckBox chkInput5;
        private System.Windows.Forms.CheckBox chkInput4;
        private System.Windows.Forms.CheckBox chkInput3;
        private System.Windows.Forms.CheckBox chkInput2;
        private System.Windows.Forms.CheckBox chkInput1;
        private System.Windows.Forms.CheckBox chkInput0;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.GroupBox grpInputBank0;
        private System.Windows.Forms.GroupBox grpOutputBank0;
        private System.Windows.Forms.GroupBox grpDSOutputBank0;
        private System.Windows.Forms.CheckBox chkDsOutput7;
        private System.Windows.Forms.CheckBox chkDsOutput6;
        private System.Windows.Forms.CheckBox chkDsOutput5;
        private System.Windows.Forms.CheckBox chkDsOutput4;
        private System.Windows.Forms.CheckBox chkDsOutput3;
        private System.Windows.Forms.CheckBox chkDsOutput2;
        private System.Windows.Forms.Button btnReadState;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPulseOutput;
        private System.Windows.Forms.NumericUpDown numLineToPulse;
    }
}

