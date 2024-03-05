namespace GenTLUSB3Basler
{
  partial class FeatureNodeTreeViewer
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
      this.featureNodeTreeView = new System.Windows.Forms.TreeView();
      this.SuspendLayout();
      // 
      // featureNodeTreeView
      // 
      this.featureNodeTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.featureNodeTreeView.Location = new System.Drawing.Point(0, 0);
      this.featureNodeTreeView.Name = "featureNodeTreeView";
      this.featureNodeTreeView.Size = new System.Drawing.Size(1084, 561);
      this.featureNodeTreeView.TabIndex = 0;
      // 
      // FeatureNodeTreeViewer
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1084, 561);
      this.Controls.Add(this.featureNodeTreeView);
      this.Name = "FeatureNodeTreeViewer";
      this.Text = "Feature Node Tree Viewer";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView featureNodeTreeView;
  }
}