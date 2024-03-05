namespace MeasurementComputing
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.Text1 = new System.Windows.Forms.TextBox();
      this.btnRun = new System.Windows.Forms.Button();
      this.btnOnline = new System.Windows.Forms.Button();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // Text1
      // 
      this.Text1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.Text1.Location = new System.Drawing.Point(2, 143);
      this.Text1.Margin = new System.Windows.Forms.Padding(2);
      this.Text1.Multiline = true;
      this.Text1.Name = "Text1";
      this.Text1.ReadOnly = true;
      this.Text1.Size = new System.Drawing.Size(497, 171);
      this.Text1.TabIndex = 1;
      this.Text1.Text = resources.GetString("Text1.Text");
      // 
      // btnRun
      // 
      this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnRun.Location = new System.Drawing.Point(373, 51);
      this.btnRun.Margin = new System.Windows.Forms.Padding(2);
      this.btnRun.Name = "btnRun";
      this.btnRun.Size = new System.Drawing.Size(113, 29);
      this.btnRun.TabIndex = 5;
      this.btnRun.Text = "Run";
      this.btnRun.UseVisualStyleBackColor = true;
      this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
      // 
      // btnOnline
      // 
      this.btnOnline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOnline.Location = new System.Drawing.Point(373, 10);
      this.btnOnline.Margin = new System.Windows.Forms.Padding(2);
      this.btnOnline.Name = "btnOnline";
      this.btnOnline.Size = new System.Drawing.Size(113, 29);
      this.btnOnline.TabIndex = 6;
      this.btnOnline.Text = "Online";
      this.btnOnline.UseVisualStyleBackColor = true;
      this.btnOnline.Click += new System.EventHandler(this.btnOnline_Click);
      // 
      // toolTip1
      // 
      this.toolTip1.AutoPopDelay = 5000;
      this.toolTip1.InitialDelay = 500;
      this.toolTip1.IsBalloon = true;
      this.toolTip1.ReshowDelay = 500;
      this.toolTip1.ShowAlways = true;
      // 
      // textBox1
      // 
      this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox1.Location = new System.Drawing.Point(9, 10);
      this.textBox1.Margin = new System.Windows.Forms.Padding(2);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new System.Drawing.Size(348, 129);
      this.textBox1.TabIndex = 7;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(501, 315);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.btnOnline);
      this.Controls.Add(this.btnRun);
      this.Controls.Add(this.Text1);
      this.Margin = new System.Windows.Forms.Padding(2);
      this.Name = "Form1";
      this.Text = "MCB Sample Program";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    internal System.Windows.Forms.TextBox Text1;
    private System.Windows.Forms.Button btnRun;
    private System.Windows.Forms.Button btnOnline;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.TextBox textBox1;
  }
}

