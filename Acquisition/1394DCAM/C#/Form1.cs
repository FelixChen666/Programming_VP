/*******************************************************************************
Copyright (C) 2005-2010 Cognex Corporation

Subject to Cognex Corporations terms and conditions and license agreement,
you are authorized to use and modify this source code in any way you find
useful, provided the Software and/or the modified Software is used solely in
conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
and agree that Cognex has no warranty, obligations or liability for your use
of the Software.
*******************************************************************************/
//This sample program is designed to illustrate certain VisionPro features or 
//techniques in the simplest way possible. It is not intended as the framework 
//for a complete application. In particular, the sample program may not provide 
//proper error handling, event handling, cleanup, repeatability, and other 
//mechanisms that a commercial quality application requires.
//
// This sample demonstrates acquisition from a color 1394DCAM camera, in particular:
//   * Methods for converting raw bayer data to a more usable format
//   * Manually adjusting the white balance setting of the camera (if supported by the camera)
//   * Using the "One-Push" white balance feature of the camera (if supported by the camera)
// 
// Note that this sample performs acquisition in the GUI thread in response to a
// timer event.  This avoids issues associated with multiple threads, which simplifies
// the sample but frequently causes poor GUI response.  In a real vision application
// you would use a worker thread to perform acquisition.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Cognex.VisionPro;
using Cognex.VisionPro.FG1394DCAM;

namespace Acq1394DCAM
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
  public class Form1 : System.Windows.Forms.Form
  {
    private Cognex.VisionPro.Display.CogDisplay _display;
    private Cognex.VisionPro.CogDisplayStatusBarV2 _displayStatusBar;
    private IContainer components;

    private Cognex.VisionPro.ICogAcqFifo _acqFifo = null;
    private bool _changed;
    private int _UGain;
    private int _VGain;
    private int _exposure = -1;

    internal Timer Timer1;
    internal ToolTip ToolTip1;
    internal GroupBox gbProperties;
    internal Label Label3;
    internal Label Label2;
    internal Label Label1;
    internal Button btnAuto;
    internal TrackBar _blueTB;
    internal TrackBar _redTB;
    internal TrackBar _exposureTB;
    internal GroupBox gbFifo;
    internal ComboBox _fgList;
    internal ComboBox _vfList;
    internal GroupBox gbConversion;
    internal RadioButton rbConvertNone;
    internal RadioButton rbConvertHSI;
    internal RadioButton rbConvertGrey;
    internal RadioButton rbConvertColor;
    internal GroupBox gbPixelFormat;
    internal RadioButton rbPFRaw;
    internal RadioButton rbPFGrey;
    internal RadioButton rbPFColor;

    public Form1()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      //
      // TODO: Add any constructor code after InitializeComponent call
      //

      // associate the status bar with the display
      _displayStatusBar.Display = _display;

      CogFrameGrabber1394DCAMs fgs = new CogFrameGrabber1394DCAMs();
      foreach (Cognex.VisionPro.ICogFrameGrabber fg in fgs)
        _fgList.Items.Add(fg.Name);
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
        CogFrameGrabbers frameGrabbers = new CogFrameGrabbers();
        foreach (ICogFrameGrabber fg in frameGrabbers)
          fg.Disconnect(false);
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
      this._display = new Cognex.VisionPro.Display.CogDisplay();
      this._displayStatusBar = new Cognex.VisionPro.CogDisplayStatusBarV2();
      this.Timer1 = new System.Windows.Forms.Timer(this.components);
      this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.btnAuto = new System.Windows.Forms.Button();
      this._blueTB = new System.Windows.Forms.TrackBar();
      this._redTB = new System.Windows.Forms.TrackBar();
      this._exposureTB = new System.Windows.Forms.TrackBar();
      this._fgList = new System.Windows.Forms.ComboBox();
      this._vfList = new System.Windows.Forms.ComboBox();
      this.rbConvertNone = new System.Windows.Forms.RadioButton();
      this.rbConvertHSI = new System.Windows.Forms.RadioButton();
      this.rbConvertGrey = new System.Windows.Forms.RadioButton();
      this.rbConvertColor = new System.Windows.Forms.RadioButton();
      this.rbPFRaw = new System.Windows.Forms.RadioButton();
      this.rbPFGrey = new System.Windows.Forms.RadioButton();
      this.rbPFColor = new System.Windows.Forms.RadioButton();
      this.gbProperties = new System.Windows.Forms.GroupBox();
      this.Label3 = new System.Windows.Forms.Label();
      this.Label2 = new System.Windows.Forms.Label();
      this.Label1 = new System.Windows.Forms.Label();
      this.gbFifo = new System.Windows.Forms.GroupBox();
      this.gbConversion = new System.Windows.Forms.GroupBox();
      this.gbPixelFormat = new System.Windows.Forms.GroupBox();
      ((System.ComponentModel.ISupportInitialize)(this._display)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._blueTB)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._redTB)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this._exposureTB)).BeginInit();
      this.gbProperties.SuspendLayout();
      this.gbFifo.SuspendLayout();
      this.gbConversion.SuspendLayout();
      this.gbPixelFormat.SuspendLayout();
      this.SuspendLayout();
      // 
      // _display
      // 
      this._display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._display.Location = new System.Drawing.Point(288, 8);
      this._display.Name = "_display";
      this._display.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("_display.OcxState")));
      this._display.Size = new System.Drawing.Size(593, 467);
      this._display.TabIndex = 4;
      // 
      // _displayStatusBar
      // 
      this._displayStatusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._displayStatusBar.Enabled = true;
      this._displayStatusBar.Location = new System.Drawing.Point(288, 475);
      this._displayStatusBar.Name = "_displayStatusBar";
      this._displayStatusBar.Size = new System.Drawing.Size(593, 24);
      this._displayStatusBar.TabIndex = 5;
      // 
      // Timer1
      // 
      this.Timer1.Enabled = true;
      this.Timer1.Interval = 50;
      this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
      // 
      // btnAuto
      // 
      this.btnAuto.Location = new System.Drawing.Point(9, 112);
      this.btnAuto.Name = "btnAuto";
      this.btnAuto.Size = new System.Drawing.Size(122, 23);
      this.btnAuto.TabIndex = 1;
      this.btnAuto.Text = "Auto White Balance";
      this.ToolTip1.SetToolTip(this.btnAuto, "Uses the camera\'s \"one push\" feature to set red and blue gain.  Use with a white " +
              "scene for best results.");
      this.btnAuto.UseVisualStyleBackColor = true;
      this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
      // 
      // _blueTB
      // 
      this._blueTB.AutoSize = false;
      this._blueTB.Location = new System.Drawing.Point(70, 81);
      this._blueTB.Maximum = 100;
      this._blueTB.Name = "_blueTB";
      this._blueTB.Size = new System.Drawing.Size(198, 25);
      this._blueTB.TabIndex = 0;
      this._blueTB.TickFrequency = 0;
      this._blueTB.TickStyle = System.Windows.Forms.TickStyle.None;
      this.ToolTip1.SetToolTip(this._blueTB, "Adjust the amount of blue data in the image");
      // 
      // _redTB
      // 
      this._redTB.AutoSize = false;
      this._redTB.Location = new System.Drawing.Point(70, 50);
      this._redTB.Maximum = 100;
      this._redTB.Name = "_redTB";
      this._redTB.Size = new System.Drawing.Size(198, 25);
      this._redTB.TabIndex = 0;
      this._redTB.TickFrequency = 0;
      this._redTB.TickStyle = System.Windows.Forms.TickStyle.None;
      this.ToolTip1.SetToolTip(this._redTB, "Adjust the amount of red data in the image");
      // 
      // _exposureTB
      // 
      this._exposureTB.AutoSize = false;
      this._exposureTB.Location = new System.Drawing.Point(70, 19);
      this._exposureTB.Maximum = 75;
      this._exposureTB.Name = "_exposureTB";
      this._exposureTB.Size = new System.Drawing.Size(198, 25);
      this._exposureTB.TabIndex = 0;
      this._exposureTB.TickFrequency = 0;
      this._exposureTB.TickStyle = System.Windows.Forms.TickStyle.None;
      this.ToolTip1.SetToolTip(this._exposureTB, "Adjust the exposure to get a good looking image.");
      // 
      // _fgList
      // 
      this._fgList.DropDownWidth = 500;
      this._fgList.Location = new System.Drawing.Point(6, 19);
      this._fgList.Name = "_fgList";
      this._fgList.Size = new System.Drawing.Size(262, 21);
      this._fgList.TabIndex = 3;
      this.ToolTip1.SetToolTip(this._fgList, "Select the camera to acquire from");
      this._fgList.SelectedIndexChanged += new System.EventHandler(this._fgList_SelectedIndexChanged);
      // 
      // _vfList
      // 
      this._vfList.DropDownWidth = 500;
      this._vfList.Location = new System.Drawing.Point(6, 46);
      this._vfList.Name = "_vfList";
      this._vfList.Size = new System.Drawing.Size(262, 21);
      this._vfList.TabIndex = 2;
      this.ToolTip1.SetToolTip(this._vfList, "Select the video format to use.  Pseudo-live display will start automatically.");
      this._vfList.SelectedIndexChanged += new System.EventHandler(this._vfList_SelectedIndexChanged);
      // 
      // rbConvertNone
      // 
      this.rbConvertNone.AutoSize = true;
      this.rbConvertNone.Checked = true;
      this.rbConvertNone.Location = new System.Drawing.Point(6, 88);
      this.rbConvertNone.Name = "rbConvertNone";
      this.rbConvertNone.Size = new System.Drawing.Size(99, 17);
      this.rbConvertNone.TabIndex = 2;
      this.rbConvertNone.TabStop = true;
      this.rbConvertNone.Text = "Do Not Convert";
      this.ToolTip1.SetToolTip(this.rbConvertNone, "Display raw data without conversion.");
      this.rbConvertNone.UseVisualStyleBackColor = true;
      // 
      // rbConvertHSI
      // 
      this.rbConvertHSI.AutoSize = true;
      this.rbConvertHSI.Location = new System.Drawing.Point(6, 65);
      this.rbConvertHSI.Name = "rbConvertHSI";
      this.rbConvertHSI.Size = new System.Drawing.Size(95, 17);
      this.rbConvertHSI.TabIndex = 1;
      this.rbConvertHSI.Text = "Convert to HSI";
      this.ToolTip1.SetToolTip(this.rbConvertHSI, "Use CogImageConvert to creata an HSI image.");
      this.rbConvertHSI.UseVisualStyleBackColor = true;
      // 
      // rbConvertGrey
      // 
      this.rbConvertGrey.AutoSize = true;
      this.rbConvertGrey.Location = new System.Drawing.Point(6, 42);
      this.rbConvertGrey.Name = "rbConvertGrey";
      this.rbConvertGrey.Size = new System.Drawing.Size(124, 17);
      this.rbConvertGrey.TabIndex = 1;
      this.rbConvertGrey.Text = "Convert to Greyscale";
      this.ToolTip1.SetToolTip(this.rbConvertGrey, "Use CogImageConvert to creata a greyscale image.");
      this.rbConvertGrey.UseVisualStyleBackColor = true;
      // 
      // rbConvertColor
      // 
      this.rbConvertColor.AutoSize = true;
      this.rbConvertColor.Location = new System.Drawing.Point(6, 19);
      this.rbConvertColor.Name = "rbConvertColor";
      this.rbConvertColor.Size = new System.Drawing.Size(101, 17);
      this.rbConvertColor.TabIndex = 0;
      this.rbConvertColor.Text = "Convert to Color";
      this.ToolTip1.SetToolTip(this.rbConvertColor, "Use CogImageConvert to creata a planar color image.");
      this.rbConvertColor.UseVisualStyleBackColor = true;
      // 
      // rbPFRaw
      // 
      this.rbPFRaw.AutoSize = true;
      this.rbPFRaw.Location = new System.Drawing.Point(6, 65);
      this.rbPFRaw.Name = "rbPFRaw";
      this.rbPFRaw.Size = new System.Drawing.Size(112, 17);
      this.rbPFRaw.TabIndex = 0;
      this.rbPFRaw.TabStop = true;
      this.rbPFRaw.Text = "Acquire Raw Data";
      this.ToolTip1.SetToolTip(this.rbPFRaw, "Acquire raw bayer data, then convert using CogImageConvert");
      this.rbPFRaw.UseVisualStyleBackColor = true;
      this.rbPFRaw.CheckedChanged += new System.EventHandler(this.rbPFRaw_CheckedChanged);
      // 
      // rbPFGrey
      // 
      this.rbPFGrey.AutoSize = true;
      this.rbPFGrey.Location = new System.Drawing.Point(6, 42);
      this.rbPFGrey.Name = "rbPFGrey";
      this.rbPFGrey.Size = new System.Drawing.Size(122, 17);
      this.rbPFGrey.TabIndex = 0;
      this.rbPFGrey.TabStop = true;
      this.rbPFGrey.Text = "Acquire in Greyscale";
      this.ToolTip1.SetToolTip(this.rbPFGrey, "Use OutputPixelFormat to create a greyscale image.");
      this.rbPFGrey.UseVisualStyleBackColor = true;
      // 
      // rbPFColor
      // 
      this.rbPFColor.AutoSize = true;
      this.rbPFColor.Location = new System.Drawing.Point(6, 19);
      this.rbPFColor.Name = "rbPFColor";
      this.rbPFColor.Size = new System.Drawing.Size(100, 17);
      this.rbPFColor.TabIndex = 0;
      this.rbPFColor.TabStop = true;
      this.rbPFColor.Text = "Acquire In Color";
      this.ToolTip1.SetToolTip(this.rbPFColor, "Use OutputPixelFormat to create a planar image.");
      this.rbPFColor.UseVisualStyleBackColor = true;
      // 
      // gbProperties
      // 
      this.gbProperties.Controls.Add(this.Label3);
      this.gbProperties.Controls.Add(this.Label2);
      this.gbProperties.Controls.Add(this.Label1);
      this.gbProperties.Controls.Add(this.btnAuto);
      this.gbProperties.Controls.Add(this._blueTB);
      this.gbProperties.Controls.Add(this._redTB);
      this.gbProperties.Controls.Add(this._exposureTB);
      this.gbProperties.Location = new System.Drawing.Point(8, 92);
      this.gbProperties.Name = "gbProperties";
      this.gbProperties.Size = new System.Drawing.Size(274, 146);
      this.gbProperties.TabIndex = 17;
      this.gbProperties.TabStop = false;
      this.gbProperties.Text = "Set Image Parameters";
      // 
      // Label3
      // 
      this.Label3.AutoSize = true;
      this.Label3.Location = new System.Drawing.Point(6, 87);
      this.Label3.Name = "Label3";
      this.Label3.Size = new System.Drawing.Size(56, 13);
      this.Label3.TabIndex = 3;
      this.Label3.Text = "Blue Gain:";
      // 
      // Label2
      // 
      this.Label2.AutoSize = true;
      this.Label2.Location = new System.Drawing.Point(6, 56);
      this.Label2.Name = "Label2";
      this.Label2.Size = new System.Drawing.Size(55, 13);
      this.Label2.TabIndex = 3;
      this.Label2.Text = "Red Gain:";
      // 
      // Label1
      // 
      this.Label1.AutoSize = true;
      this.Label1.Location = new System.Drawing.Point(6, 25);
      this.Label1.Name = "Label1";
      this.Label1.Size = new System.Drawing.Size(54, 13);
      this.Label1.TabIndex = 2;
      this.Label1.Text = "Exposure:";
      // 
      // gbFifo
      // 
      this.gbFifo.Controls.Add(this._fgList);
      this.gbFifo.Controls.Add(this._vfList);
      this.gbFifo.Location = new System.Drawing.Point(8, 13);
      this.gbFifo.Name = "gbFifo";
      this.gbFifo.Size = new System.Drawing.Size(274, 73);
      this.gbFifo.TabIndex = 18;
      this.gbFifo.TabStop = false;
      this.gbFifo.Text = "Select Camera and Video Format";
      // 
      // gbConversion
      // 
      this.gbConversion.Controls.Add(this.rbConvertNone);
      this.gbConversion.Controls.Add(this.rbConvertHSI);
      this.gbConversion.Controls.Add(this.rbConvertGrey);
      this.gbConversion.Controls.Add(this.rbConvertColor);
      this.gbConversion.Location = new System.Drawing.Point(8, 338);
      this.gbConversion.Name = "gbConversion";
      this.gbConversion.Size = new System.Drawing.Size(274, 119);
      this.gbConversion.TabIndex = 20;
      this.gbConversion.TabStop = false;
      this.gbConversion.Text = "Raw Data Conversion";
      // 
      // gbPixelFormat
      // 
      this.gbPixelFormat.Controls.Add(this.rbPFRaw);
      this.gbPixelFormat.Controls.Add(this.rbPFGrey);
      this.gbPixelFormat.Controls.Add(this.rbPFColor);
      this.gbPixelFormat.Location = new System.Drawing.Point(8, 244);
      this.gbPixelFormat.Name = "gbPixelFormat";
      this.gbPixelFormat.Size = new System.Drawing.Size(274, 88);
      this.gbPixelFormat.TabIndex = 19;
      this.gbPixelFormat.TabStop = false;
      this.gbPixelFormat.Text = "Pixel Format";
      // 
      // Form1
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(889, 505);
      this.Controls.Add(this.gbConversion);
      this.Controls.Add(this.gbPixelFormat);
      this.Controls.Add(this.gbFifo);
      this.Controls.Add(this.gbProperties);
      this.Controls.Add(this._displayStatusBar);
      this.Controls.Add(this._display);
      this.MinimumSize = new System.Drawing.Size(544, 376);
      this.Name = "Form1";
      this.Text = "1394 DCAM Capture and Display";
      ((System.ComponentModel.ISupportInitialize)(this._display)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._blueTB)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._redTB)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this._exposureTB)).EndInit();
      this.gbProperties.ResumeLayout(false);
      this.gbProperties.PerformLayout();
      this.gbFifo.ResumeLayout(false);
      this.gbConversion.ResumeLayout(false);
      this.gbConversion.PerformLayout();
      this.gbPixelFormat.ResumeLayout(false);
      this.gbPixelFormat.PerformLayout();
      this.ResumeLayout(false);

    }
    #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.Run(new Form1());
    }

    // Acquisition is performed in response to a timer event.  This is not the way to write a real application,
    // but is a simple way to avoid threading issues in this sample.
    void Timer1_Tick(System.Object sender, System.EventArgs e)
    {
      if (_acqFifo == null)
        return;

      // After an automatic white balance occurs, read the new settings and update the
      // sliders to match.
      if (_changed)
      {
        UpdateSliders();
        _changed = false;
      }

      // Set the white balance property (if supported) based on the sliders.
      // Important: The fifo's values are set only if the desired setting has changed.  This avoids
      // overwriting the settings determined via automatic white balance before they can be read by
      // the code above.
      if (_acqFifo.OwnedWhiteBalanceParams != null)
      {
        if (_UGain != _redTB.Value)
        {
          _UGain = _redTB.Value;
          _acqFifo.OwnedWhiteBalanceParams.UGain = _UGain / 100.0;
        }
        if (_VGain != _blueTB.Value)
        {
          _VGain = _blueTB.Value;
          _acqFifo.OwnedWhiteBalanceParams.VGain = _VGain / 100.0;
        }
      }

      if (_exposure != _exposureTB.Value)
      {
        _exposure = _exposureTB.Value;
        _acqFifo.OwnedExposureParams.Exposure = System.Math.Pow(1.4, _exposure);
      }

      // A few words about pixel formats:
      //
      // The "pixel format" defines how each pixel's data is stored in memory.  There is
      // a wide variety of pixel formats that can be acquired from a camera, but VisionPro
      // can only process a few of them.  The fifo's OutputPixelFormat property allows you
      // to specify the desired pixel format for processing, and the fifo will perform
      // a conversion if needed to produce an image in the desired pixel format.
      //
      // PlanarRGB8 is the default setting for color cameras, and is compatible with color
      // vision tools.
      //
      // Grey8 is the default setting for greyscale cameras, and is compatible with greyscale
      // vision tools.
      //
      // All fifos support PlanarRGB8 and Grey8 settings for OutputPixelFormat.
      //
      // Bayer8 is only available for bayer color cameras, and is intended for backwards
      // compatibility or if you need more control over the image conversion process.
      if (rbPFColor.Checked)
        _acqFifo.OutputPixelFormat = CogImagePixelFormatConstants.PlanarRGB8;
      else if (rbPFGrey.Checked)
        _acqFifo.OutputPixelFormat = CogImagePixelFormatConstants.Grey8;
      else if (rbPFRaw.Checked)
        _acqFifo.OutputPixelFormat = CogImagePixelFormatConstants.Bayer8;

      // Acquire an image
      int trigNum;
      Cognex.VisionPro.ICogImage image;
      image = _acqFifo.Acquire(out trigNum);

      // If the image was acquired in raw bayer format, use CogImageConvert to convert it to
      // a different pixel format.
      if (rbPFRaw.Checked)
      {
        if (rbConvertGrey.Checked)
          image = Cognex.VisionPro.CogImageConvert.
            GetIntensityImageFromBayer(image, 0, 0, 0, 0,
            0.299, 0.587, 0.114);

        if (rbConvertColor.Checked)
          image = Cognex.VisionPro.CogImageConvert.
            GetRGBImageFromBayer(image, 0, 0, 0, 0, 1.0, 1.0, 1.0);

        if (rbConvertHSI.Checked)
          image = Cognex.VisionPro.CogImageConvert.
            GetHSIImageFromBayer(image, 0, 0, 0, 0, 1.0, 1.0, 1.0);

        // There is no code associated with the "no conversion" option.
      }

      _display.Image = image;
      GC.Collect();
    }

    // When a camera is selected
    void _fgList_SelectedIndexChanged(System.Object sender, System.EventArgs e)
    {
      _vfList.Items.Clear();
      CogFrameGrabber1394DCAMs fgs = new CogFrameGrabber1394DCAMs();
      ComboBox cb = sender as ComboBox;

      // Populate the list of available video formats, then set selectedIndex to 0, which as a side effect will
      // cause the first format in the list to automatically take effect.
      foreach (String vf in fgs[cb.SelectedIndex].AvailableVideoFormats)
        _vfList.Items.Add(vf);
      _vfList.SelectedIndex = 0;
    }

    // When a video format is selected
    void _vfList_SelectedIndexChanged(System.Object sender, System.EventArgs e)
    {
      // Remove any handler associated with a previous fifo.
      if (_acqFifo != null)
        _acqFifo.Changed -= new CogChangedEventHandler(Operator_Changed);

      // Construct a new fifo using the selected format and camera.
      CogFrameGrabber1394DCAMs fgs = new CogFrameGrabber1394DCAMs();
      _acqFifo = fgs[_fgList.SelectedIndex].CreateAcqFifo(
        fgs[_fgList.SelectedIndex].AvailableVideoFormats[_vfList.SelectedIndex],
        CogAcqFifoPixelFormatConstants.Format8Grey, 0, false);

      // Add a handler to detect changes to the white balance settings.
      _acqFifo.Changed += new CogChangedEventHandler(Operator_Changed);

      // Bayer8 output is only available with bayer cameras.  This is enforced in the sample code by
      // disabling the "Raw" if the acquired format is not Bayer8.
      rbPFRaw.Enabled = _acqFifo.AcquiredPixelFormat() == CogImagePixelFormatConstants.Bayer8;

      // Update radio button state to match default OutputPixelFormat for the fifo.
      if (_acqFifo.OutputPixelFormat == CogImagePixelFormatConstants.PlanarRGB8)
        rbPFColor.Checked = true;
      else
        rbPFGrey.Checked = true;

      gbConversion.Enabled = false;

      // Reset slider controls
      _exposureTB.Value = 10;
      _changed = true;
    }

    // Fifo changed event handler.  Set _changed to true when white balance has changed, so that the updated
    // values can be read during the next acquisition.
    void Operator_Changed(Object sender, CogChangedEventArgs e)
    {
      if (e.StateFlags == CogAcqFifoStateFlags.SfUGain ||
          e.StateFlags == CogAcqFifoStateFlags.SfVGain)
        _changed = true;
    }

    // Read the white balance values and set the sliders accordingly.
    void UpdateSliders()
    {
      Cognex.VisionPro.ICogAcqWhiteBalance wb;
      wb = _acqFifo.OwnedWhiteBalanceParams;
      if (wb != null)
      {
        _UGain = (int)(wb.UGain * 100.0);
        _VGain = (int)(wb.VGain * 100.0);
        _redTB.Value = _UGain;
        _blueTB.Value = _VGain;
      }
    }

    // Perform an automatic white balance using the camera's "one push" function.
    void btnAuto_Click(System.Object sender, System.EventArgs e)
    {
      if (_acqFifo != null)
      {
        Cognex.VisionPro.ICogAcqWhiteBalance wb;
        wb = _acqFifo.OwnedWhiteBalanceParams;

        if (wb != null)
        {
          // Calling AutoWhiteBalance does not immediately perform a white balance.  Instead, it requests
          // that an automatic white balance be performed prior to the next acquisition.  After the automatic
          // white balance occurs, the changed event will be fired and the new white balance values can be
          // read from the fifo's properties.
          wb.AutoWhiteBalance();

          // If the fifo is idle, prepare can be used to cause the requested automatic white balance
          // to occur.
          _acqFifo.Prepare();
        }
      }
    }
    // Conditionally enable the ImageConvert options based on whether raw bayer data was acquired or not.
    void rbPFRaw_CheckedChanged(System.Object sender, System.EventArgs e)
    {
      gbConversion.Enabled = rbPFRaw.Checked;
    }
  }
}
