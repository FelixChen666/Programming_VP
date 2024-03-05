namespace GenTLUSB3Basler
{
  partial class SnapshotViewer
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
      this.snapshotContentRichTextBox = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // snapshotContentRichTextBox
      // 
      this.snapshotContentRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.snapshotContentRichTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.snapshotContentRichTextBox.Location = new System.Drawing.Point(0, 0);
      this.snapshotContentRichTextBox.Margin = new System.Windows.Forms.Padding(0);
      this.snapshotContentRichTextBox.Name = "snapshotContentRichTextBox";
      this.snapshotContentRichTextBox.ReadOnly = true;
      this.snapshotContentRichTextBox.Size = new System.Drawing.Size(1084, 561);
      this.snapshotContentRichTextBox.TabIndex = 0;
      this.snapshotContentRichTextBox.Text = "";
      // 
      // SnapshotViewer
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1084, 561);
      this.Controls.Add(this.snapshotContentRichTextBox);
      this.Name = "SnapshotViewer";
      this.Text = "Snapshot Viewer";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.RichTextBox snapshotContentRichTextBox;
  }
}