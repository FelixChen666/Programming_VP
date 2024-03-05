namespace QuickBuildClientSample
{
  partial class QBClientSampleForm
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
      this.ConnectButton = new System.Windows.Forms.Button();
      this.TotalBytesLabel = new System.Windows.Forms.Label();
      this.TotalLabel = new System.Windows.Forms.Label();
      this.ClearButton = new System.Windows.Forms.Button();
      this.txtDescription = new System.Windows.Forms.TextBox();
      this.PortNumberBox = new System.Windows.Forms.NumericUpDown();
      this.lblHostName = new System.Windows.Forms.Label();
      this.lblPortNumber = new System.Windows.Forms.Label();
      this.HostNameTextBox = new System.Windows.Forms.TextBox();
      this.OutputTextBox = new System.Windows.Forms.TextBox();
      this.LoggingCheckBox = new System.Windows.Forms.CheckBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.FileSaveButton = new System.Windows.Forms.Button();
      this.FileNameLabel = new System.Windows.Forms.Label();
      this.FileNameTextBox = new System.Windows.Forms.TextBox();
      this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
      ((System.ComponentModel.ISupportInitialize)(this.PortNumberBox)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // ConnectButton
      // 
      this.ConnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ConnectButton.Location = new System.Drawing.Point(428, 2);
      this.ConnectButton.Name = "ConnectButton";
      this.ConnectButton.Size = new System.Drawing.Size(100, 32);
      this.ConnectButton.TabIndex = 66;
      this.ConnectButton.Text = "Connect";
      this.ConnectButton.Click += new System.EventHandler(this.btnConnect_Click);
      // 
      // TotalBytesLabel
      // 
      this.TotalBytesLabel.Location = new System.Drawing.Point(132, 129);
      this.TotalBytesLabel.Name = "TotalBytesLabel";
      this.TotalBytesLabel.Size = new System.Drawing.Size(226, 18);
      this.TotalBytesLabel.TabIndex = 65;
      this.TotalBytesLabel.Text = "0";
      // 
      // TotalLabel
      // 
      this.TotalLabel.Location = new System.Drawing.Point(4, 129);
      this.TotalLabel.Name = "TotalLabel";
      this.TotalLabel.Size = new System.Drawing.Size(122, 22);
      this.TotalLabel.TabIndex = 64;
      this.TotalLabel.Text = "Total Bytes Received:";
      // 
      // ClearButton
      // 
      this.ClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ClearButton.Location = new System.Drawing.Point(428, 123);
      this.ClearButton.Name = "ClearButton";
      this.ClearButton.Size = new System.Drawing.Size(100, 30);
      this.ClearButton.TabIndex = 63;
      this.ClearButton.Text = "Clear";
      this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
      // 
      // txtDescription
      // 
      this.txtDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.txtDescription.Location = new System.Drawing.Point(0, 428);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      this.txtDescription.Size = new System.Drawing.Size(533, 85);
      this.txtDescription.TabIndex = 55;
      // 
      // PortNumberBox
      // 
      this.PortNumberBox.Location = new System.Drawing.Point(92, 32);
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
      this.PortNumberBox.TabIndex = 58;
      this.PortNumberBox.Value = new decimal(new int[] {
            5001,
            0,
            0,
            0});
      // 
      // lblHostName
      // 
      this.lblHostName.AutoSize = true;
      this.lblHostName.Location = new System.Drawing.Point(6, 5);
      this.lblHostName.Name = "lblHostName";
      this.lblHostName.Size = new System.Drawing.Size(58, 13);
      this.lblHostName.TabIndex = 60;
      this.lblHostName.Text = "Hostname:";
      // 
      // lblPortNumber
      // 
      this.lblPortNumber.AutoSize = true;
      this.lblPortNumber.Location = new System.Drawing.Point(6, 35);
      this.lblPortNumber.Name = "lblPortNumber";
      this.lblPortNumber.Size = new System.Drawing.Size(69, 13);
      this.lblPortNumber.TabIndex = 59;
      this.lblPortNumber.Text = "Port Number:";
      // 
      // HostNameTextBox
      // 
      this.HostNameTextBox.Location = new System.Drawing.Point(92, 5);
      this.HostNameTextBox.Name = "HostNameTextBox";
      this.HostNameTextBox.ReadOnly = true;
      this.HostNameTextBox.Size = new System.Drawing.Size(223, 20);
      this.HostNameTextBox.TabIndex = 57;
      // 
      // OutputTextBox
      // 
      this.OutputTextBox.AcceptsReturn = true;
      this.OutputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.OutputTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
      this.OutputTextBox.Location = new System.Drawing.Point(4, 159);
      this.OutputTextBox.MinimumSize = new System.Drawing.Size(525, 150);
      this.OutputTextBox.Multiline = true;
      this.OutputTextBox.Name = "OutputTextBox";
      this.OutputTextBox.ReadOnly = true;
      this.OutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.OutputTextBox.Size = new System.Drawing.Size(525, 263);
      this.OutputTextBox.TabIndex = 54;
      // 
      // LoggingCheckBox
      // 
      this.LoggingCheckBox.AutoSize = true;
      this.LoggingCheckBox.Checked = true;
      this.LoggingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.LoggingCheckBox.Location = new System.Drawing.Point(12, 70);
      this.LoggingCheckBox.Name = "LoggingCheckBox";
      this.LoggingCheckBox.Size = new System.Drawing.Size(64, 17);
      this.LoggingCheckBox.TabIndex = 67;
      this.LoggingCheckBox.Text = "Logging";
      this.LoggingCheckBox.UseVisualStyleBackColor = true;
      this.LoggingCheckBox.CheckStateChanged += new System.EventHandler(this.LoggingCheckBox_CheckedChanged);
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.FileSaveButton);
      this.groupBox1.Controls.Add(this.FileNameLabel);
      this.groupBox1.Controls.Add(this.FileNameTextBox);
      this.groupBox1.Location = new System.Drawing.Point(4, 72);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(418, 49);
      this.groupBox1.TabIndex = 68;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "groupBox1";
      // 
      // FileSaveButton
      // 
      this.FileSaveButton.Dock = System.Windows.Forms.DockStyle.Right;
      this.FileSaveButton.Enabled = false;
      this.FileSaveButton.Location = new System.Drawing.Point(375, 16);
      this.FileSaveButton.Name = "FileSaveButton";
      this.FileSaveButton.Size = new System.Drawing.Size(40, 30);
      this.FileSaveButton.TabIndex = 31;
      this.FileSaveButton.Text = "...";
      this.FileSaveButton.Click += new System.EventHandler(this.FileSaveButton_Click);
      // 
      // FileNameLabel
      // 
      this.FileNameLabel.Enabled = false;
      this.FileNameLabel.Location = new System.Drawing.Point(2, 20);
      this.FileNameLabel.Name = "FileNameLabel";
      this.FileNameLabel.Size = new System.Drawing.Size(63, 18);
      this.FileNameLabel.TabIndex = 30;
      this.FileNameLabel.Text = "Filename:";
      // 
      // FileNameTextBox
      // 
      this.FileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.FileNameTextBox.Enabled = false;
      this.FileNameTextBox.Location = new System.Drawing.Point(71, 20);
      this.FileNameTextBox.Name = "FileNameTextBox";
      this.FileNameTextBox.Size = new System.Drawing.Size(295, 20);
      this.FileNameTextBox.TabIndex = 29;
      this.FileNameTextBox.Text = "c:\\ClientSampleOutput.txt";
      // 
      // QBClientSampleForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(533, 513);
      this.Controls.Add(this.LoggingCheckBox);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.ConnectButton);
      this.Controls.Add(this.TotalBytesLabel);
      this.Controls.Add(this.TotalLabel);
      this.Controls.Add(this.ClearButton);
      this.Controls.Add(this.txtDescription);
      this.Controls.Add(this.PortNumberBox);
      this.Controls.Add(this.lblHostName);
      this.Controls.Add(this.lblPortNumber);
      this.Controls.Add(this.HostNameTextBox);
      this.Controls.Add(this.OutputTextBox);
      this.MinimumSize = new System.Drawing.Size(541, 518);
      this.Name = "QBClientSampleForm";
      this.Text = "QuickBuild TCP/IP Client Sample";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QBClientSampleForm_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this.PortNumberBox)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button ConnectButton;
    private System.Windows.Forms.Label TotalBytesLabel;
    private System.Windows.Forms.Label TotalLabel;
    private System.Windows.Forms.Button ClearButton;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.NumericUpDown PortNumberBox;
    private System.Windows.Forms.Label lblHostName;
    private System.Windows.Forms.Label lblPortNumber;
    private System.Windows.Forms.TextBox HostNameTextBox;
    private System.Windows.Forms.TextBox OutputTextBox;
    private System.Windows.Forms.CheckBox LoggingCheckBox;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button FileSaveButton;
    private System.Windows.Forms.Label FileNameLabel;
    private System.Windows.Forms.TextBox FileNameTextBox;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
  }
}

