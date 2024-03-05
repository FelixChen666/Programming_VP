namespace GenTLUSB3Basler
{
  partial class Main
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
      this.imageDisplay = new Cognex.VisionPro.Display.CogDisplay();
      this.cameraSelectionComboBox = new System.Windows.Forms.ComboBox();
      this.cameraSelectionLabel = new System.Windows.Forms.Label();
      this.getSnapshotButton = new System.Windows.Forms.Button();
      this.getFeatureNodeTreeButton = new System.Windows.Forms.Button();
      this.createFifoButton = new System.Windows.Forms.Button();
      this.acquireSinglebutton = new System.Windows.Forms.Button();
      this.featureNameTextBox = new System.Windows.Forms.TextBox();
      this.setFeatureButton = new System.Windows.Forms.Button();
      this.getFeatureButton = new System.Windows.Forms.Button();
      this.createCustomPropertyButton = new System.Windows.Forms.Button();
      this.featureValueTextBox = new System.Windows.Forms.TextBox();
      this.featureNameLabel = new System.Windows.Forms.Label();
      this.featureValueLabel = new System.Windows.Forms.Label();
      this.videoFormatSelectionComboBox = new System.Windows.Forms.ComboBox();
      this.selectVideoFormatLabel = new System.Windows.Forms.Label();
      this.acquireContinuousToggleButton = new System.Windows.Forms.CheckBox();
      this.clearCustomPropertiesButton = new System.Windows.Forms.Button();
      this.executeCommandButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.imageDisplay)).BeginInit();
      this.SuspendLayout();
      // 
      // imageDisplay
      // 
      this.imageDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.imageDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
      this.imageDisplay.ColorMapLowerRoiLimit = 0D;
      this.imageDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
      this.imageDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
      this.imageDisplay.ColorMapUpperRoiLimit = 1D;
      this.imageDisplay.DoubleTapZoomCycleLength = 2;
      this.imageDisplay.DoubleTapZoomSensitivity = 2.5D;
      this.imageDisplay.Location = new System.Drawing.Point(12, 12);
      this.imageDisplay.Margin = new System.Windows.Forms.Padding(2);
      this.imageDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
      this.imageDisplay.MouseWheelSensitivity = 1D;
      this.imageDisplay.Name = "imageDisplay";
      this.imageDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("imageDisplay.OcxState")));
      this.imageDisplay.Size = new System.Drawing.Size(650, 529);
      this.imageDisplay.TabIndex = 0;
      // 
      // cameraSelectionComboBox
      // 
      this.cameraSelectionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cameraSelectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cameraSelectionComboBox.FormattingEnabled = true;
      this.cameraSelectionComboBox.Location = new System.Drawing.Point(701, 34);
      this.cameraSelectionComboBox.Margin = new System.Windows.Forms.Padding(2);
      this.cameraSelectionComboBox.Name = "cameraSelectionComboBox";
      this.cameraSelectionComboBox.Size = new System.Drawing.Size(260, 21);
      this.cameraSelectionComboBox.TabIndex = 1;
      this.cameraSelectionComboBox.SelectedIndexChanged += new System.EventHandler(this.cameraSelectionComboBox_SelectedIndexChanged);
      // 
      // cameraSelectionLabel
      // 
      this.cameraSelectionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cameraSelectionLabel.AutoSize = true;
      this.cameraSelectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cameraSelectionLabel.Location = new System.Drawing.Point(701, 12);
      this.cameraSelectionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.cameraSelectionLabel.Name = "cameraSelectionLabel";
      this.cameraSelectionLabel.Size = new System.Drawing.Size(127, 20);
      this.cameraSelectionLabel.TabIndex = 30;
      this.cameraSelectionLabel.Text = "Select a Camera";
      // 
      // getSnapshotButton
      // 
      this.getSnapshotButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.getSnapshotButton.Enabled = false;
      this.getSnapshotButton.Location = new System.Drawing.Point(700, 127);
      this.getSnapshotButton.Margin = new System.Windows.Forms.Padding(2);
      this.getSnapshotButton.Name = "getSnapshotButton";
      this.getSnapshotButton.Size = new System.Drawing.Size(128, 32);
      this.getSnapshotButton.TabIndex = 3;
      this.getSnapshotButton.Text = "Get Feature Snapshot";
      this.getSnapshotButton.UseVisualStyleBackColor = true;
      this.getSnapshotButton.Click += new System.EventHandler(this.getSnapshotButton_Click);
      // 
      // getFeatureNodeTreeButton
      // 
      this.getFeatureNodeTreeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.getFeatureNodeTreeButton.Enabled = false;
      this.getFeatureNodeTreeButton.Location = new System.Drawing.Point(832, 127);
      this.getFeatureNodeTreeButton.Margin = new System.Windows.Forms.Padding(2);
      this.getFeatureNodeTreeButton.Name = "getFeatureNodeTreeButton";
      this.getFeatureNodeTreeButton.Size = new System.Drawing.Size(128, 32);
      this.getFeatureNodeTreeButton.TabIndex = 4;
      this.getFeatureNodeTreeButton.Text = "Get Feature Node Tree";
      this.getFeatureNodeTreeButton.UseVisualStyleBackColor = true;
      this.getFeatureNodeTreeButton.Click += new System.EventHandler(this.getFeatureNodeTreeButton_Click);
      // 
      // createFifoButton
      // 
      this.createFifoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.createFifoButton.Enabled = false;
      this.createFifoButton.Location = new System.Drawing.Point(766, 164);
      this.createFifoButton.Margin = new System.Windows.Forms.Padding(2);
      this.createFifoButton.Name = "createFifoButton";
      this.createFifoButton.Size = new System.Drawing.Size(128, 32);
      this.createFifoButton.TabIndex = 5;
      this.createFifoButton.Text = "Create FIFO for Camera";
      this.createFifoButton.UseVisualStyleBackColor = true;
      this.createFifoButton.Click += new System.EventHandler(this.createFifoButton_Click);
      // 
      // acquireSinglebutton
      // 
      this.acquireSinglebutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.acquireSinglebutton.Enabled = false;
      this.acquireSinglebutton.Location = new System.Drawing.Point(694, 510);
      this.acquireSinglebutton.Margin = new System.Windows.Forms.Padding(2);
      this.acquireSinglebutton.Name = "acquireSinglebutton";
      this.acquireSinglebutton.Size = new System.Drawing.Size(117, 32);
      this.acquireSinglebutton.TabIndex = 13;
      this.acquireSinglebutton.Text = "Acquire Single Image";
      this.acquireSinglebutton.UseVisualStyleBackColor = true;
      this.acquireSinglebutton.Click += new System.EventHandler(this.acquireSinglebutton_Click);
      // 
      // featureNameTextBox
      // 
      this.featureNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.featureNameTextBox.Location = new System.Drawing.Point(751, 236);
      this.featureNameTextBox.Margin = new System.Windows.Forms.Padding(2);
      this.featureNameTextBox.Name = "featureNameTextBox";
      this.featureNameTextBox.Size = new System.Drawing.Size(210, 20);
      this.featureNameTextBox.TabIndex = 6;
      // 
      // setFeatureButton
      // 
      this.setFeatureButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.setFeatureButton.Enabled = false;
      this.setFeatureButton.Location = new System.Drawing.Point(766, 339);
      this.setFeatureButton.Margin = new System.Windows.Forms.Padding(2);
      this.setFeatureButton.Name = "setFeatureButton";
      this.setFeatureButton.Size = new System.Drawing.Size(145, 32);
      this.setFeatureButton.TabIndex = 9;
      this.setFeatureButton.Text = "Set Feature";
      this.setFeatureButton.UseVisualStyleBackColor = true;
      this.setFeatureButton.Click += new System.EventHandler(this.setFeatureButton_Click);
      // 
      // getFeatureButton
      // 
      this.getFeatureButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.getFeatureButton.Enabled = false;
      this.getFeatureButton.Location = new System.Drawing.Point(766, 303);
      this.getFeatureButton.Margin = new System.Windows.Forms.Padding(2);
      this.getFeatureButton.Name = "getFeatureButton";
      this.getFeatureButton.Size = new System.Drawing.Size(145, 32);
      this.getFeatureButton.TabIndex = 8;
      this.getFeatureButton.Text = "Get Feature";
      this.getFeatureButton.UseVisualStyleBackColor = true;
      this.getFeatureButton.Click += new System.EventHandler(this.getFeatureButton_Click);
      // 
      // createCustomPropertyButton
      // 
      this.createCustomPropertyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.createCustomPropertyButton.Enabled = false;
      this.createCustomPropertyButton.Location = new System.Drawing.Point(766, 411);
      this.createCustomPropertyButton.Margin = new System.Windows.Forms.Padding(2);
      this.createCustomPropertyButton.Name = "createCustomPropertyButton";
      this.createCustomPropertyButton.Size = new System.Drawing.Size(145, 32);
      this.createCustomPropertyButton.TabIndex = 11;
      this.createCustomPropertyButton.Text = "Create Custom Property";
      this.createCustomPropertyButton.UseVisualStyleBackColor = true;
      this.createCustomPropertyButton.Click += new System.EventHandler(this.createCustomPropertyButton_Click);
      // 
      // featureValueTextBox
      // 
      this.featureValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.featureValueTextBox.Location = new System.Drawing.Point(751, 259);
      this.featureValueTextBox.Margin = new System.Windows.Forms.Padding(2);
      this.featureValueTextBox.Name = "featureValueTextBox";
      this.featureValueTextBox.Size = new System.Drawing.Size(210, 20);
      this.featureValueTextBox.TabIndex = 7;
      // 
      // featureNameLabel
      // 
      this.featureNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.featureNameLabel.AutoSize = true;
      this.featureNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.featureNameLabel.Location = new System.Drawing.Point(696, 239);
      this.featureNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.featureNameLabel.Name = "featureNameLabel";
      this.featureNameLabel.Size = new System.Drawing.Size(49, 17);
      this.featureNameLabel.TabIndex = 32;
      this.featureNameLabel.Text = "Name:";
      // 
      // featureValueLabel
      // 
      this.featureValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.featureValueLabel.AutoSize = true;
      this.featureValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.featureValueLabel.Location = new System.Drawing.Point(698, 261);
      this.featureValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.featureValueLabel.Name = "featureValueLabel";
      this.featureValueLabel.Size = new System.Drawing.Size(48, 17);
      this.featureValueLabel.TabIndex = 33;
      this.featureValueLabel.Text = "Value:";
      // 
      // videoFormatSelectionComboBox
      // 
      this.videoFormatSelectionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.videoFormatSelectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.videoFormatSelectionComboBox.Enabled = false;
      this.videoFormatSelectionComboBox.FormattingEnabled = true;
      this.videoFormatSelectionComboBox.Location = new System.Drawing.Point(700, 91);
      this.videoFormatSelectionComboBox.Margin = new System.Windows.Forms.Padding(2);
      this.videoFormatSelectionComboBox.Name = "videoFormatSelectionComboBox";
      this.videoFormatSelectionComboBox.Size = new System.Drawing.Size(260, 21);
      this.videoFormatSelectionComboBox.TabIndex = 2;
      this.videoFormatSelectionComboBox.SelectedIndexChanged += new System.EventHandler(this.videoFormatSelectionComboBox_SelectedIndexChanged);
      // 
      // selectVideoFormatLabel
      // 
      this.selectVideoFormatLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.selectVideoFormatLabel.AutoSize = true;
      this.selectVideoFormatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.selectVideoFormatLabel.Location = new System.Drawing.Point(701, 69);
      this.selectVideoFormatLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.selectVideoFormatLabel.Name = "selectVideoFormatLabel";
      this.selectVideoFormatLabel.Size = new System.Drawing.Size(167, 20);
      this.selectVideoFormatLabel.TabIndex = 31;
      this.selectVideoFormatLabel.Text = "Select a Video Format";
      // 
      // acquireContinuousToggleButton
      // 
      this.acquireContinuousToggleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.acquireContinuousToggleButton.Appearance = System.Windows.Forms.Appearance.Button;
      this.acquireContinuousToggleButton.Enabled = false;
      this.acquireContinuousToggleButton.Location = new System.Drawing.Point(816, 510);
      this.acquireContinuousToggleButton.Name = "acquireContinuousToggleButton";
      this.acquireContinuousToggleButton.Size = new System.Drawing.Size(156, 32);
      this.acquireContinuousToggleButton.TabIndex = 14;
      this.acquireContinuousToggleButton.Text = "Begin Continuous Acquisition";
      this.acquireContinuousToggleButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.acquireContinuousToggleButton.UseVisualStyleBackColor = true;
      this.acquireContinuousToggleButton.Click += new System.EventHandler(this.acquireContinuousToggleButton_Click);
      // 
      // clearCustomPropertiesButton
      // 
      this.clearCustomPropertiesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.clearCustomPropertiesButton.Enabled = false;
      this.clearCustomPropertiesButton.Location = new System.Drawing.Point(766, 447);
      this.clearCustomPropertiesButton.Margin = new System.Windows.Forms.Padding(2);
      this.clearCustomPropertiesButton.Name = "clearCustomPropertiesButton";
      this.clearCustomPropertiesButton.Size = new System.Drawing.Size(145, 32);
      this.clearCustomPropertiesButton.TabIndex = 12;
      this.clearCustomPropertiesButton.Text = "Clear Custom Properties";
      this.clearCustomPropertiesButton.UseVisualStyleBackColor = true;
      this.clearCustomPropertiesButton.Click += new System.EventHandler(this.clearCustomPropertiesButton_Click);
      // 
      // executeCommandButton
      // 
      this.executeCommandButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.executeCommandButton.Enabled = false;
      this.executeCommandButton.Location = new System.Drawing.Point(766, 375);
      this.executeCommandButton.Margin = new System.Windows.Forms.Padding(2);
      this.executeCommandButton.Name = "executeCommandButton";
      this.executeCommandButton.Size = new System.Drawing.Size(145, 32);
      this.executeCommandButton.TabIndex = 10;
      this.executeCommandButton.Text = "Execute Command Feature";
      this.executeCommandButton.UseVisualStyleBackColor = true;
      this.executeCommandButton.Click += new System.EventHandler(this.executeCommandButton_Click);
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(984, 553);
      this.Controls.Add(this.executeCommandButton);
      this.Controls.Add(this.clearCustomPropertiesButton);
      this.Controls.Add(this.acquireContinuousToggleButton);
      this.Controls.Add(this.selectVideoFormatLabel);
      this.Controls.Add(this.videoFormatSelectionComboBox);
      this.Controls.Add(this.featureValueLabel);
      this.Controls.Add(this.featureNameLabel);
      this.Controls.Add(this.featureValueTextBox);
      this.Controls.Add(this.createCustomPropertyButton);
      this.Controls.Add(this.getFeatureButton);
      this.Controls.Add(this.setFeatureButton);
      this.Controls.Add(this.featureNameTextBox);
      this.Controls.Add(this.acquireSinglebutton);
      this.Controls.Add(this.createFifoButton);
      this.Controls.Add(this.getFeatureNodeTreeButton);
      this.Controls.Add(this.getSnapshotButton);
      this.Controls.Add(this.cameraSelectionLabel);
      this.Controls.Add(this.cameraSelectionComboBox);
      this.Controls.Add(this.imageDisplay);
      this.Margin = new System.Windows.Forms.Padding(2);
      this.MinimumSize = new System.Drawing.Size(1000, 592);
      this.Name = "Main";
      this.Text = "GentL Basler USB3 Acquisition Sample";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
      this.Load += new System.EventHandler(this.Main_Load);
      ((System.ComponentModel.ISupportInitialize)(this.imageDisplay)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Cognex.VisionPro.Display.CogDisplay imageDisplay;
    private System.Windows.Forms.ComboBox cameraSelectionComboBox;
    private System.Windows.Forms.Label cameraSelectionLabel;
    private System.Windows.Forms.Button getSnapshotButton;
    private System.Windows.Forms.Button getFeatureNodeTreeButton;
    private System.Windows.Forms.Button createFifoButton;
    private System.Windows.Forms.Button acquireSinglebutton;
    private System.Windows.Forms.TextBox featureNameTextBox;
    private System.Windows.Forms.Button setFeatureButton;
    private System.Windows.Forms.Button getFeatureButton;
    private System.Windows.Forms.Button createCustomPropertyButton;
    private System.Windows.Forms.TextBox featureValueTextBox;
    private System.Windows.Forms.Label featureNameLabel;
    private System.Windows.Forms.Label featureValueLabel;
    private System.Windows.Forms.ComboBox videoFormatSelectionComboBox;
    private System.Windows.Forms.Label selectVideoFormatLabel;
    private System.Windows.Forms.CheckBox acquireContinuousToggleButton;
    private System.Windows.Forms.Button clearCustomPropertiesButton;
    private System.Windows.Forms.Button executeCommandButton;
  }
}

