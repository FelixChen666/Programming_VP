namespace QBServerSample
{
  partial class QuickBuildServerSampleForm
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
      this.TotalBytesLabel = new System.Windows.Forms.Label();
      this.TotalLabel = new System.Windows.Forms.Label();
      this.ClearButton = new System.Windows.Forms.Button();
      this.DescriptionTextBox = new System.Windows.Forms.TextBox();
      this.HostNameLabel = new System.Windows.Forms.Label();
      this.PortNumberLabel = new System.Windows.Forms.Label();
      this.HostNameTextBox = new System.Windows.Forms.TextBox();
      this.OutputTextBox = new System.Windows.Forms.TextBox();
      this.PortNumberBox = new System.Windows.Forms.NumericUpDown();
      this.ListenButton = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.LoggingCheckBox = new System.Windows.Forms.CheckBox();
      this.FileSaveButton = new System.Windows.Forms.Button();
      this.FileNameLabel = new System.Windows.Forms.Label();
      this.FileNameTextBox = new System.Windows.Forms.TextBox();
      this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
      ((System.ComponentModel.ISupportInitialize)(this.PortNumberBox)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // TotalBytesLabel
      // 
      this.TotalBytesLabel.Location = new System.Drawing.Point(136, 117);
      this.TotalBytesLabel.Name = "TotalBytesLabel";
      this.TotalBytesLabel.Size = new System.Drawing.Size(226, 18);
      this.TotalBytesLabel.TabIndex = 80;
      this.TotalBytesLabel.Text = "0";
      // 
      // TotalLabel
      // 
      this.TotalLabel.Location = new System.Drawing.Point(9, 117);
      this.TotalLabel.Name = "TotalLabel";
      this.TotalLabel.Size = new System.Drawing.Size(122, 20);
      this.TotalLabel.TabIndex = 79;
      this.TotalLabel.Text = "Total Bytes Received:";
      // 
      // ClearButton
      // 
      this.ClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ClearButton.Location = new System.Drawing.Point(428, 108);
      this.ClearButton.Name = "ClearButton";
      this.ClearButton.Size = new System.Drawing.Size(100, 30);
      this.ClearButton.TabIndex = 78;
      this.ClearButton.Text = "Clear";
      this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
      // 
      // DescriptionTextBox
      // 
      this.DescriptionTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.DescriptionTextBox.Location = new System.Drawing.Point(0, 453);
      this.DescriptionTextBox.Multiline = true;
      this.DescriptionTextBox.Name = "DescriptionTextBox";
      this.DescriptionTextBox.ReadOnly = true;
      this.DescriptionTextBox.Size = new System.Drawing.Size(534, 74);
      this.DescriptionTextBox.TabIndex = 77;
      // 
      // HostNameLabel
      // 
      this.HostNameLabel.AutoSize = true;
      this.HostNameLabel.Location = new System.Drawing.Point(8, 5);
      this.HostNameLabel.Name = "HostNameLabel";
      this.HostNameLabel.Size = new System.Drawing.Size(58, 13);
      this.HostNameLabel.TabIndex = 75;
      this.HostNameLabel.Text = "Hostname:";
      // 
      // PortNumberLabel
      // 
      this.PortNumberLabel.AutoSize = true;
      this.PortNumberLabel.Location = new System.Drawing.Point(8, 28);
      this.PortNumberLabel.Name = "PortNumberLabel";
      this.PortNumberLabel.Size = new System.Drawing.Size(69, 13);
      this.PortNumberLabel.TabIndex = 74;
      this.PortNumberLabel.Text = "Port Number:";
      // 
      // HostNameTextBox
      // 
      this.HostNameTextBox.Location = new System.Drawing.Point(94, 2);
      this.HostNameTextBox.Name = "HostNameTextBox";
      this.HostNameTextBox.ReadOnly = true;
      this.HostNameTextBox.Size = new System.Drawing.Size(327, 20);
      this.HostNameTextBox.TabIndex = 72;
      // 
      // OutputTextBox
      // 
      this.OutputTextBox.AcceptsReturn = true;
      this.OutputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.OutputTextBox.ImeMode = System.Windows.Forms.ImeMode.Hangul;
      this.OutputTextBox.Location = new System.Drawing.Point(6, 139);
      this.OutputTextBox.Multiline = true;
      this.OutputTextBox.Name = "OutputTextBox";
      this.OutputTextBox.ReadOnly = true;
      this.OutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.OutputTextBox.Size = new System.Drawing.Size(522, 308);
      this.OutputTextBox.TabIndex = 71;
      // 
      // PortNumberBox
      // 
      this.PortNumberBox.Location = new System.Drawing.Point(94, 28);
      this.PortNumberBox.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
      this.PortNumberBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.PortNumberBox.Name = "PortNumberBox";
      this.PortNumberBox.Size = new System.Drawing.Size(75, 20);
      this.PortNumberBox.TabIndex = 73;
      this.PortNumberBox.Value = new decimal(new int[] {
            5001,
            0,
            0,
            0});
      // 
      // ListenButton
      // 
      this.ListenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ListenButton.Location = new System.Drawing.Point(428, 9);
      this.ListenButton.Name = "ListenButton";
      this.ListenButton.Size = new System.Drawing.Size(100, 32);
      this.ListenButton.TabIndex = 76;
      this.ListenButton.Text = "Listen";
      this.ListenButton.Click += new System.EventHandler(this.ListenButton_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.LoggingCheckBox);
      this.groupBox1.Controls.Add(this.FileSaveButton);
      this.groupBox1.Controls.Add(this.FileNameLabel);
      this.groupBox1.Controls.Add(this.FileNameTextBox);
      this.groupBox1.Location = new System.Drawing.Point(6, 54);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(418, 47);
      this.groupBox1.TabIndex = 81;
      this.groupBox1.TabStop = false;
      // 
      // LoggingCheckBox
      // 
      this.LoggingCheckBox.AutoSize = true;
      this.LoggingCheckBox.Checked = true;
      this.LoggingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.LoggingCheckBox.Location = new System.Drawing.Point(6, 0);
      this.LoggingCheckBox.Name = "LoggingCheckBox";
      this.LoggingCheckBox.Size = new System.Drawing.Size(64, 17);
      this.LoggingCheckBox.TabIndex = 69;
      this.LoggingCheckBox.Text = "Logging";
      this.LoggingCheckBox.UseVisualStyleBackColor = true;
      this.LoggingCheckBox.CheckedChanged += new System.EventHandler(this.LoggingCheckBox_CheckedChanged);
      // 
      // FileSaveButton
      // 
      this.FileSaveButton.Dock = System.Windows.Forms.DockStyle.Right;
      this.FileSaveButton.Location = new System.Drawing.Point(375, 16);
      this.FileSaveButton.Name = "FileSaveButton";
      this.FileSaveButton.Size = new System.Drawing.Size(40, 28);
      this.FileSaveButton.TabIndex = 31;
      this.FileSaveButton.Text = "...";
      this.FileSaveButton.Click += new System.EventHandler(this.FileSaveButton_Click);
      // 
      // FileNameLabel
      // 
      this.FileNameLabel.Location = new System.Drawing.Point(3, 23);
      this.FileNameLabel.Name = "FileNameLabel";
      this.FileNameLabel.Size = new System.Drawing.Size(65, 15);
      this.FileNameLabel.TabIndex = 30;
      this.FileNameLabel.Text = "Filename:";
      // 
      // FileNameTextBox
      // 
      this.FileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.FileNameTextBox.Location = new System.Drawing.Point(68, 20);
      this.FileNameTextBox.Name = "FileNameTextBox";
      this.FileNameTextBox.Size = new System.Drawing.Size(298, 20);
      this.FileNameTextBox.TabIndex = 29;
      this.FileNameTextBox.Text = "c:\\ServerSampleOutput.txt";
      this.FileNameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FileNameTextBox_KeyDown);
      // 
      // QuickBuildServerSampleForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(534, 527);
      this.Controls.Add(this.TotalBytesLabel);
      this.Controls.Add(this.TotalLabel);
      this.Controls.Add(this.ClearButton);
      this.Controls.Add(this.DescriptionTextBox);
      this.Controls.Add(this.HostNameLabel);
      this.Controls.Add(this.PortNumberLabel);
      this.Controls.Add(this.HostNameTextBox);
      this.Controls.Add(this.OutputTextBox);
      this.Controls.Add(this.PortNumberBox);
      this.Controls.Add(this.ListenButton);
      this.Controls.Add(this.groupBox1);
      this.MinimumSize = new System.Drawing.Size(542, 553);
      this.Name = "QuickBuildServerSampleForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = "QuickBuild TCP/IP Server Sample";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QuickBuildServerSampleForm_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this.PortNumberBox)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label TotalBytesLabel;
    private System.Windows.Forms.Label TotalLabel;
    private System.Windows.Forms.Button ClearButton;
    private System.Windows.Forms.TextBox DescriptionTextBox;
    private System.Windows.Forms.Label HostNameLabel;
    private System.Windows.Forms.Label PortNumberLabel;
    private System.Windows.Forms.TextBox HostNameTextBox;
    private System.Windows.Forms.TextBox OutputTextBox;
    private System.Windows.Forms.NumericUpDown PortNumberBox;
    private System.Windows.Forms.Button ListenButton;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox LoggingCheckBox;
    private System.Windows.Forms.Button FileSaveButton;
    private System.Windows.Forms.Label FileNameLabel;
    private System.Windows.Forms.TextBox FileNameTextBox;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;

  }
}

