//*******************************************************************************
//Copyright (C) 2004 Cognex Corporation
//
//Subject to Cognex Corporation's terms and conditions and license agreement,
//you are authorized to use and modify this source code in any way you find
//useful, provided the Software and/or the modified Software is used solely in
//conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
//and agree that Cognex has no warranty, obligations or liability for your use
//of the Software.
//*******************************************************************************
// This sample program is designed to illustrate certain VisionPro features or 
// techniques in the simplest way possible. It is not intended as the framework 
// for a complete application. In particular, the sample program may not provide
// proper error handling, event handling, cleanup, repeatability, and other 
// mechanisms that a commercial quality application requires.

//*******************************************************************************
// Description:
//
// This sample demonstrates three ways to use OCV for applications in which the
// pattern to verify changes frequently.  Examples of this include:
//   - Serial number verification, where the serial number increases with each
//     presented image.
//   - Credit card verification, where the credit card number and the name change
//     with each presented image.
//
// Three different algorithms are provided for solving these applications.  All three
// algorithms assume that that the font has already been constructed (e.g. using
// the font editor, provided with the VPro OCV component).
//
// The algorithms are:
//   - Standard:  With each new image and associated pattern to verify, a new pattern
//                is trained.  The total time to process an image is the time to
//                train the new pattern plus the time to run verification of that pattern
//                on the provided image.
//
//   - Multiple Models: This algorithm uses limited OCR to solve the OCV problem.
//                During setup time, each position in the pattern is trained with
//                all the possible font models that might appear at that position.  For
//                example, if a particular pattern position is known to be numeric, the
//                position is trained with patterns 0-9.  At run time, when a new image
//                is presented for verification, OCV tests all the font models that have
//                been trained at each position and returns the font model with the
//                highest score at that position. The set of highest scoring font models
//                is compared to the verification string to determine if they match.  The
//                total time to process an image is the time to run verfication of the
//                provided pattern on the image.
//                Notes:
//                  1. The time to run verification is directly dependent on the number
//                     of models at each position.  Since OCV must test all models
//                     provided at each position, it will be slower for cases in which
//                     a large number of models are trained at each position.
//                  2. OCV does not check for confusion between models that are trained
//                     at a given position.  For example, if the user trains both an
//                     O (the letter) and a 0 (the number) at a particular position,
//                     then confusion will not be checked between the two.  OCV will
//                     simply return the highest scoring model.
//
//   - Smart Placement: This algorithm tries to efficiently verify only the expected
//                 model at each position.  At setup time, a separate pattern is
//                 trained for each model in the font.  The pattern consists of only
//                 a single model.  For example, if the font consists of the
//                 numbers 0-9, then 10 patterns are constructed: Pattern0 has
//                 only the 0 model, Pattern1 has only the 1 model, etc.  Each pattern
//                 pose is set to some default value. At run time, the verification
//                 string is used to select which patterns are tested on the image, as
//                 well as the location of each pattern.  For example, to verify the
//                 pattern "Thomas", the run-time pose for pattern containing the "T"
//                 model is placed at the expected pose for the overall string.
//                 That pattern is then verified.  Next, the run-time pose for the
//                 pattern containing the "h" is assigned by offsetting it's position
//                 from the "T".  The pattern "h" is then verified. The process of
//                 pose determination and verification continues until the entire string
//                 is verified.
//                 Notes:
//                 1. This algorithm is by far the faster of the three since it is only
//                    verifying one model per position.
//                 2. The run-time pose for each pattern must be carefully calculated
//                    by the application.  This works well for fixed width fonts, but
//                    poorly for proportional width fonts.
//
// This sample code uses two image sets:
//  - alphas.idb: a set of images consisting of alpha (non-numeric) strings in
//                courier 14pt bold.
//  - numbers.idb: a set of images consiting of numeric (non-alpha) strings in
//                courier 14pt bold.
//  There are also a set of text files containing ground truth information for
//  the images:
//   - alphas.txt and numbers.txt
//
// This example also includes two pre-constructed font files:
//   - courier14ptalpha.vpp: the font for alpha (non-numeric) courier 14pt bold chars.
//   - courier14ptnumeric.vpp: the font for numeric (non-alpha) courier 14pt bold chars.
//
// *** It is a requirement that the .txt files and the .vpp files described   ***
// *** are located in the same directory as this sample code.  The sample     ***
// *** code only looks in its current directory for these files.              ***

// *** The .idb files must be located in the <VPRO_ROOT>\images directory.    ***

// For reference, the image alphanumbers.bmp is included in the <VPRO_ROOT>\images
// directory.  This image contains the courier 14pt bold chars plus other
// sample font chars.
//*******************************************************************************

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Cognex.VisionPro;                  // used for basic VisionPro functionality
using Cognex.VisionPro.Display;          // used for CogDisplay
using Cognex.VisionPro.Exceptions;       // used for VisionPro exception
using Cognex.VisionPro.ImageFile;        // used for CogImageFileTool
using Cognex.VisionPro.OC;               // used for CogOCVTool and CogOCFont

namespace OCVPatternChange
{
  /// <summary>
  /// OCVPatternChange sample code form.
  /// </summary>
  public class OCVPatternChangeForm : System.Windows.Forms.Form
  {
    #region Private Fields
    private System.Windows.Forms.MainMenu mnuMain;
    private System.Windows.Forms.MenuItem mnuSamples;
    private System.Windows.Forms.MenuItem mnuMultipleModels;
    private System.Windows.Forms.MenuItem mnuSmartPlacement;
    private System.Windows.Forms.MenuItem mnuStandard;
    private System.Windows.Forms.MenuItem mnuExit;
    private System.Windows.Forms.Button btnProcess;
    private System.Windows.Forms.RadioButton optNumeric;
    private System.Windows.Forms.RadioButton optAlpha;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.TextBox txtInfo;
    private Cognex.VisionPro.Display.CogDisplay mDisplay;
    private System.Windows.Forms.Label lblResult;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtResult;
    private System.Windows.Forms.TextBox txtTimeTotal;
    private System.Windows.Forms.TextBox txtTimePerChar;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    private CogOCVTool mOCVTool = null;
    private CogOCFont  mOCFont  = null;
    private CogImageFile mImageFile = null;

    private int mCurrentImage = 0;
    private int mMaxStringLength = 0;
    private int mPatternOriginX = 0;
    private int mPatternOriginY = 0;
    private int mFontWidth = 0;
    private int mFontHeight = 0;

    private MenuOptionConstants mRunOption = MenuOptionConstants.None;
    private ArrayList mOCVStringsToVerify = null;
    private ArrayList mSmartPatterns = null;
    private ArrayList mOCVResults = null;

    #endregion

    #region Enums
    private enum MenuOptionConstants
    {
      None = -1, MultipleModels, SmartPlacement, Standard
    }
    #endregion

    #region Constructors & Dispose
    public OCVPatternChangeForm()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      mOCFont = new CogOCFont ();
      mImageFile = new CogImageFile ();
      mOCVResults = new ArrayList ();

      optNumeric.Checked = true;
      optAlpha.Checked   = false;
      btnProcess.Enabled = false;
      txtInfo.Text = "";

      mDisplay.AutoFit = true;  // turn on auto fit image of display
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        // release any resources we employed in the application
        if (mOCVTool != null) mOCVTool.Dispose ();
        if (mImageFile != null) mImageFile.Close ();
        if (mSmartPatterns != null) mSmartPatterns.Clear (); // release the reference to pattern objects
        if (mOCVResults != null) mOCVResults.Clear ();
        mOCFont = null;
        if (components != null) 
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }
    #endregion

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(OCVPatternChangeForm));
      this.mnuMain = new System.Windows.Forms.MainMenu();
      this.mnuSamples = new System.Windows.Forms.MenuItem();
      this.mnuMultipleModels = new System.Windows.Forms.MenuItem();
      this.mnuSmartPlacement = new System.Windows.Forms.MenuItem();
      this.mnuStandard = new System.Windows.Forms.MenuItem();
      this.mnuExit = new System.Windows.Forms.MenuItem();
      this.btnProcess = new System.Windows.Forms.Button();
      this.optNumeric = new System.Windows.Forms.RadioButton();
      this.optAlpha = new System.Windows.Forms.RadioButton();
      this.lblDescription = new System.Windows.Forms.Label();
      this.txtDescription = new System.Windows.Forms.TextBox();
      this.txtInfo = new System.Windows.Forms.TextBox();
      this.mDisplay = new Cognex.VisionPro.Display.CogDisplay();
      this.lblResult = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.txtResult = new System.Windows.Forms.TextBox();
      this.txtTimeTotal = new System.Windows.Forms.TextBox();
      this.txtTimePerChar = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.mDisplay)).BeginInit();
      this.SuspendLayout();
      // 
      // mnuMain
      // 
      this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                            this.mnuSamples});
      // 
      // mnuSamples
      // 
      this.mnuSamples.Index = 0;
      this.mnuSamples.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                               this.mnuMultipleModels,
                                                                               this.mnuSmartPlacement,
                                                                               this.mnuStandard,
                                                                               this.mnuExit});
      this.mnuSamples.Text = "Samples";
      // 
      // mnuMultipleModels
      // 
      this.mnuMultipleModels.Index = 0;
      this.mnuMultipleModels.Text = "Multiple models at each position";
      this.mnuMultipleModels.Click += new System.EventHandler(this.mnuMultipleModels_Click);
      // 
      // mnuSmartPlacement
      // 
      this.mnuSmartPlacement.Index = 1;
      this.mnuSmartPlacement.Text = "Smart placement of models";
      this.mnuSmartPlacement.Click += new System.EventHandler(this.mnuSmartPlacement_Click);
      // 
      // mnuStandard
      // 
      this.mnuStandard.Index = 2;
      this.mnuStandard.Text = "Standard";
      this.mnuStandard.Click += new System.EventHandler(this.mnuStandard_Click);
      // 
      // mnuExit
      // 
      this.mnuExit.Index = 3;
      this.mnuExit.Text = "Exit";
      this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
      // 
      // btnProcess
      // 
      this.btnProcess.Location = new System.Drawing.Point(24, 16);
      this.btnProcess.Name = "btnProcess";
      this.btnProcess.Size = new System.Drawing.Size(136, 48);
      this.btnProcess.TabIndex = 0;
      this.btnProcess.Text = "Process Next Image";
      this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
      // 
      // optNumeric
      // 
      this.optNumeric.Location = new System.Drawing.Point(32, 80);
      this.optNumeric.Name = "optNumeric";
      this.optNumeric.Size = new System.Drawing.Size(124, 20);
      this.optNumeric.TabIndex = 1;
      this.optNumeric.Text = "Numeric Images";
      this.optNumeric.CheckedChanged += new System.EventHandler(this.optNumeric_Checked);
      // 
      // optAlpha
      // 
      this.optAlpha.Location = new System.Drawing.Point(32, 112);
      this.optAlpha.Name = "optAlpha";
      this.optAlpha.Size = new System.Drawing.Size(124, 20);
      this.optAlpha.TabIndex = 2;
      this.optAlpha.Text = "Alpha Images";
      this.optAlpha.CheckedChanged += new System.EventHandler(this.optAlpha_Checked);
      // 
      // lblDescription
      // 
      this.lblDescription.Location = new System.Drawing.Point(236, 32);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new System.Drawing.Size(104, 16);
      this.lblDescription.TabIndex = 3;
      this.lblDescription.Text = "Sample Description";
      // 
      // txtDescription
      // 
      this.txtDescription.Location = new System.Drawing.Point(356, 28);
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      this.txtDescription.Size = new System.Drawing.Size(320, 20);
      this.txtDescription.TabIndex = 4;
      this.txtDescription.Text = "";
      // 
      // txtInfo
      // 
      this.txtInfo.Location = new System.Drawing.Point(236, 64);
      this.txtInfo.Multiline = true;
      this.txtInfo.Name = "txtInfo";
      this.txtInfo.ReadOnly = true;
      this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtInfo.Size = new System.Drawing.Size(440, 68);
      this.txtInfo.TabIndex = 5;
      this.txtInfo.Text = "";
      // 
      // mDisplay
      // 
      this.mDisplay.Location = new System.Drawing.Point(16, 212);
      this.mDisplay.Name = "mDisplay";
      this.mDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mDisplay.OcxState")));
      this.mDisplay.Size = new System.Drawing.Size(668, 300);
      this.mDisplay.TabIndex = 6;
      // 
      // lblResult
      // 
      this.lblResult.Location = new System.Drawing.Point(28, 168);
      this.lblResult.Name = "lblResult";
      this.lblResult.Size = new System.Drawing.Size(68, 20);
      this.lblResult.TabIndex = 7;
      this.lblResult.Text = "Results";
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(344, 168);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(56, 28);
      this.label1.TabIndex = 8;
      this.label1.Text = "Total Time (ms)";
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(504, 168);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(68, 32);
      this.label2.TabIndex = 9;
      this.label2.Text = "Per Char Time (ms)";
      // 
      // txtResult
      // 
      this.txtResult.Location = new System.Drawing.Point(100, 168);
      this.txtResult.Name = "txtResult";
      this.txtResult.ReadOnly = true;
      this.txtResult.Size = new System.Drawing.Size(236, 20);
      this.txtResult.TabIndex = 10;
      this.txtResult.Text = "";
      // 
      // txtTimeTotal
      // 
      this.txtTimeTotal.Location = new System.Drawing.Point(404, 172);
      this.txtTimeTotal.Name = "txtTimeTotal";
      this.txtTimeTotal.ReadOnly = true;
      this.txtTimeTotal.Size = new System.Drawing.Size(72, 20);
      this.txtTimeTotal.TabIndex = 11;
      this.txtTimeTotal.Text = "";
      // 
      // txtTimePerChar
      // 
      this.txtTimePerChar.Location = new System.Drawing.Point(580, 172);
      this.txtTimePerChar.Name = "txtTimePerChar";
      this.txtTimePerChar.ReadOnly = true;
      this.txtTimePerChar.Size = new System.Drawing.Size(72, 20);
      this.txtTimePerChar.TabIndex = 12;
      this.txtTimePerChar.Text = "";
      // 
      // OCVPatternChangeForm
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(696, 533);
      this.Controls.Add(this.txtTimePerChar);
      this.Controls.Add(this.txtTimeTotal);
      this.Controls.Add(this.txtResult);
      this.Controls.Add(this.txtInfo);
      this.Controls.Add(this.txtDescription);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.lblResult);
      this.Controls.Add(this.mDisplay);
      this.Controls.Add(this.lblDescription);
      this.Controls.Add(this.optAlpha);
      this.Controls.Add(this.optNumeric);
      this.Controls.Add(this.btnProcess);
      this.Menu = this.mnuMain;
      this.Name = "OCVPatternChangeForm";
      this.Text = "OCV Pattern Change Sample Application";
      ((System.ComponentModel.ISupportInitialize)(this.mDisplay)).EndInit();
      this.ResumeLayout(false);

    }
    #endregion

    #region Private Methods
    /// <summary>
    /// This subroutine opens the image database files (*.idb), the font files (*.vpp)
    /// And the text file that describes the strings in each image.
    /// </summary>
    private void OpenDB ()
    {
      try
      {
        this.Cursor = Cursors.WaitCursor;
        string sBaseDir = Environment.GetEnvironmentVariable ("VPRO_ROOT");
        if (sBaseDir == "")
        {
          MessageBox.Show ("Required environment variable VPRO_ROOT is not set", "OCVPatternChange", MessageBoxButtons.OK, MessageBoxIcon.Stop);
          return;
        }
        mImageFile.Close ();
        string sDBFile = sBaseDir + "/Samples/Programming/VisionTools/OCVPatternChange/";
        string sFontFile = sBaseDir + "/Samples/Programming/VisionTools/OCVPatternChange/";
        if (optNumeric.Checked)
        {
          mImageFile.Open (sBaseDir + @"\Images\numbers.idb", CogImageFileModeConstants.Read);
          sDBFile += "numbers.txt";  // numeric file 0-9
          sFontFile += "courier14ptnumeric.vpp";
        }
        else
        {
          mImageFile.Open (sBaseDir + @"\Images\alphas.idb", CogImageFileModeConstants.Read);
          sDBFile += "alphas.txt";   // Alpha font (letters A-Z)
          sFontFile += "courier14ptalpha.vpp";
        }
        mOCFont = CogSerializer.LoadObjectFromFile (sFontFile) as CogOCFont;
        // Note that the text file have header information consisting of:
        //   maxStringLength: The maximum string length for any string in the file.
        //   patternOriginX, patternOriginY: The rough expected position of the first character in each string.
        //   fontWidth: The average width of each character in the font.
        //   Note that this is for fixed-width fonts only.
        //   fontHeight: The average height of each character in the font.
        using (StreamReader reader = new StreamReader (sDBFile))
        {
          mMaxStringLength = System.Convert.ToInt32 (reader.ReadLine ());
          string origin = reader.ReadLine ();
          mPatternOriginX = Convert.ToInt32 (origin.Substring (0, origin.IndexOf (",")));
          mPatternOriginY = Convert.ToInt32 (origin.Substring (origin.Length - origin.IndexOf (",")).Trim ());
          string font = reader.ReadLine ();
          mFontWidth  = Convert.ToInt32 (font.Substring (0, font.IndexOf (",")));
          mFontHeight = Convert.ToInt32 (font.Substring (font.Length - font.IndexOf (",")).Trim ());
          mOCVStringsToVerify = new ArrayList ();
          string sNext = reader.ReadLine ();
          while (sNext != null)
          {
            mOCVStringsToVerify.Add (sNext);
            sNext = reader.ReadLine ();
          }
        }
        if (mSmartPatterns == null) mSmartPatterns = new ArrayList ();
        // if the count of patterns in the list is less than the count of font models, add more
        for (int iPattern = mSmartPatterns.Count; iPattern < mOCFont.FontModels.Count; iPattern++)
          mSmartPatterns.Add (new CogOCVPattern ());
        // if the count of patterns in the list is more than the count of font models, remove them
        while (mSmartPatterns.Count > mOCFont.FontModels.Count)
          mSmartPatterns.RemoveAt (0);
      }
      catch (Exception ex)
      {
        MessageBox.Show (ex.Message, "OCVPatternChange");
        btnProcess.Enabled = false;
      }
      finally
      {
        mCurrentImage = 0;
        this.ResetCursor ();
      }
    }
    /// <summary>
    /// This subroutine is the run-time processing for the multiple models method.
    /// </summary>
    private void ProcessMultipleModels ()
    {
      try
      {
        this.Cursor = Cursors.WaitCursor;
        btnProcess.Enabled = false;
        // clean-up the old result list
        mOCVResults.Clear ();
        // remove old graphics if they exist
        mDisplay.StaticGraphics.Clear ();

        // Get the image from the CogImageFile operator
        ICogImage mImage = mImageFile [mCurrentImage];
        // Display it on my control's CogDisplay
        mDisplay.Image = mImage;
        // Set run-time parameters for the OCV tool.
        mOCVTool.InputImage = mImage as CogImage8Grey;
        mOCVTool.PatternPosition.ExpectedPose.TranslationX = 0;
        mOCVTool.PatternPosition.ExpectedPose.TranslationY = 0;
        mOCVTool.PatternPosition.ExpectedPose.Rotation = 0;
        mOCVTool.PatternPosition.ExpectedPose.Scaling = 1;
        // make sure to set the selected space name or the positional information will
        // be incorrect.
        mOCVTool.PatternPosition.SelectedSpaceName = mImage.SelectedSpaceName;
        mOCVTool.PatternPosition.TranslationUncertainty = 4; //+/- 4 pixels in TransX, TransY
        mOCVTool.PatternPosition.RotationUncertainty = 0;
        mOCVTool.PatternPosition.ScalingUncertainty = 0;
        // All set, so run the tool
        mOCVTool.Run ();
        // Get the next record in the database
        string sPattern = mOCVStringsToVerify [mCurrentImage] as string;
        // Get the results out, one by one.
        if (mOCVTool.RunStatus.Exception == null)  //no exceptions
        {
          string resultString = "";
          for (int i = 0; i < sPattern.Length; i++)
          {
            mOCVResults.Add (mOCVTool.Results [i]);
            if (mOCVTool.Results [i].Status == CogOCVCharacterStatusConstants.Verified)
              resultString = resultString + ((CogOCVResult)mOCVTool.Results [i]).FontModelName;
            else
              resultString = resultString + "?";
          }
          // Post the results to the control.
          PostResults (mOCVTool.RunStatus.ProcessingTime, sPattern, resultString, sPattern.Length);
        }
        btnProcess.Enabled = true;
      }
      catch (Exception ex)
      {
        MessageBox.Show ("ProcessSmartPlacement caught exception: " + ex.Message);
      }
      finally
      {
        mCurrentImage = (mCurrentImage + 1 >= mImageFile.Count ? 0 : mCurrentImage + 1);
        this.ResetCursor ();
      }
    }
    /// <summary>
    /// This subroutine is the run-time processing for the smart placement method. 
    /// </summary>
    private void ProcessSmartPlacement()
    {
      try
      {
        this.Cursor = Cursors.WaitCursor;
        btnProcess.Enabled = false;

        // clear old result list
        mOCVResults.Clear ();

        // remove old graphics if they exist
        mDisplay.StaticGraphics.Clear ();

        // Get the image from the CogImageFile operator
        ICogImage mImage = mImageFile [mCurrentImage];
        // Display it on my control's CogDisplay
        mDisplay.Image = mImage;
        
        // Get the next record in the database
        string sPattern = mOCVStringsToVerify [mCurrentImage] as string;

        // Now we're going to use the characters in the verification string and the
        // model width & pattern origin info to lay out our string to verify.  We do this
        // dynamically.  After verifying the first character, we use it's position to
        // place the next character for verification.  We then verify it. We repeat
        // until the string is complete.
        // Note that in this sample code, we are making the assumption that the characters
        // are arranged horizontally.  If this is not true in your application, then
        // the position of the next character needs to be computed in a different manner.
        int numBlanks = 0;
        double totalTime = 0.0;
        string resultsString = "";
        for (int iPos = 0; iPos < sPattern.Length; iPos++)
        {
          if (mOCVTool != null) mOCVTool.Dispose ();
          mOCVTool = new CogOCVTool ();
          // Get the first character to verify
          string thisChar = sPattern.Substring (iPos, 1);
          if (thisChar != " ")
          {
            // find the pattern corresponding to this character in the mySmartPatterns array
            CogOCVPattern smartPattern = null;
            foreach (CogOCVPattern pattern in mSmartPatterns)
            {
              int iModelInstance;
              string sModelName;
              pattern.CharacterPositions [0].GetFontModel (0, out sModelName, out iModelInstance);
              if (sModelName == thisChar) 
              {
                smartPattern = pattern;
                break;
              }
            }
            if (smartPattern == null)
            {
              throw new Exception ("Illegal character: " + thisChar + " found");
            }
            // Set the tools pattern to the pattern associated with this character
            mOCVTool.Pattern = smartPattern;
            mOCVTool.InputImage = mImage as CogImage8Grey;
            // Use PatternPosition.ExpectedPose to place the expected location of
            // this pattern (consisting of a single character) in the image.
            // Shift the origin of the next character by the width of a character in the font.
            // Note that this assumes that the string is positioned horizontally.  Your
            // application my differ.
            mOCVTool.PatternPosition.ExpectedPose.TranslationX = mPatternOriginX + iPos * mFontWidth;
            mOCVTool.PatternPosition.ExpectedPose.TranslationY = mPatternOriginY;
            mOCVTool.PatternPosition.ExpectedPose.Rotation = 0;
            mOCVTool.PatternPosition.ExpectedPose.Scaling = 1;
            mOCVTool.PatternPosition.SelectedSpaceName = mImage.SelectedSpaceName;
            mOCVTool.PatternPosition.TranslationUncertainty = 2;
            mOCVTool.PatternPosition.RotationUncertainty = 0;
            mOCVTool.PatternPosition.ScalingUncertainty = 0;
            // All set, so run the tool
            mOCVTool.Run ();
            totalTime = totalTime + mOCVTool.RunStatus.ProcessingTime;
            if (mOCVTool.RunStatus.Exception == null)
            {
              mOCVResults.Add (mOCVTool.Results [0]);
              if (mOCVTool.Results [0].Status == CogOCVCharacterStatusConstants.Verified)
                resultsString = resultsString + mOCVTool.Results [0].FontModelName;
              else
                resultsString = resultsString + "?";
            }
          }
          else
          {
            mOCVResults.Add (null);  // space filler
            resultsString = resultsString + " ";
            numBlanks = numBlanks + 1;
          }
        }
        // Post the results to the control.
        PostResults (totalTime, sPattern, resultsString, sPattern.Length - numBlanks);
        btnProcess.Enabled = true;
      }
      catch (Exception ex)
      {
        MessageBox.Show ("ProcessSmartPlacement caught exception: " + ex.Message);
      }
      finally
      {
        mCurrentImage = (mCurrentImage + 1 >= mImageFile.Count ? 0 : mCurrentImage + 1);
        this.ResetCursor ();
      }
    } 
    /// <summary> 
    /// This subroutine is the run-time processing for the standard method.
    /// </summary>
    private void ProcessStandard()
    {
      try
      {
        this.Cursor = Cursors.WaitCursor;
        btnProcess.Enabled = false;

        // clear old result list
        mOCVResults.Clear ();

        // remove old graphics if they exist
        mDisplay.StaticGraphics.Clear ();

        // Get the image from the CogImageFile operator
        ICogImage mImage = mImageFile [mCurrentImage];
        // Display it on my control's CogDisplay
        mDisplay.Image = mImage;
        
        // Get the next record in the database
        string sPattern = mOCVStringsToVerify [mCurrentImage] as string;
        
        if (mOCVTool != null) mOCVTool.Dispose ();
        mOCVTool = new CogOCVTool ();     //first, make an OCV tool
        mOCVTool.Pattern.Font = mOCFont;  // assign the font
        mOCVTool.InputImage = mImage as CogImage8Grey;   // assign the image
        // For each position in the string get the character at that position
        // and add it to the pattern.
        for (int iPos = 0; iPos < sPattern.Length; iPos++)
        {
          CogOCVCharacterPosition mCharPosition = new CogOCVCharacterPosition ();
          string thisChar = sPattern.Substring (iPos, 1);
          mCharPosition.AddFontModel (thisChar, 0);
          // Set position, rotation, scaling and associated uncertainty parameters
          // Note that in this example the uncertainties are the same for each position,
          // so I'm setting UsePatternXXXX = true.
          mCharPosition.UsePatternCharacterUncertainties = true;
          mCharPosition.UsePatternRunTimeCharacterThresholds = true;
          mCharPosition.Rotation = 0;
          mCharPosition.Scaling = 1;
          mCharPosition.TranslationY = mPatternOriginY;
          // Note that this example assumes that the string to verify is positioned horizontally so
          // when laying out the pattern, the position of the next character is
          // fontWidth pixels away (in the X direction) from the current character. So
          // the first character is at patternOriginX, the second at
          // patternOriginX + 1 * fontWidth, the third at patternOriginX + 2 * fontWidth, etc.
          mCharPosition.TranslationX = mPatternOriginX + iPos * mFontWidth;
          // Add this position to the pattern.
          mOCVTool.Pattern.CharacterPositions.Add (mCharPosition);
        }
        // Set uncertainties for translation, rotation, and scale.
        mOCVTool.Pattern.CharacterTranslationUncertainty = 4; //pixels
        mOCVTool.Pattern.CharacterRotationUncertainty = 0.0;  //radians
        mOCVTool.Pattern.CharacterScalingUncertainty = 0.0;   //percentage
        // Set accept and confusion thresholds
        mOCVTool.Pattern.RunTimeCharacterAcceptThreshold = 0.5;
        mOCVTool.Pattern.RunTimeCharacterConfidenceThreshold = 0.0;
   
        // Get the time it takes to train the pattern. Note that we need to count this time
        // towards overall pattern verification time since we are changing the pattern with each
        // new pattern/image we verify.
        CogStopwatch mTimer = new CogStopwatch ();
        mTimer.Reset ();
        mTimer.Start ();
        mOCVTool.Pattern.Train ();
        mTimer.Stop ();
    
        // Now let's set up the runtime info
        mOCVTool.PatternPosition.ExpectedPose.TranslationX = 0;
        mOCVTool.PatternPosition.ExpectedPose.TranslationY = 0;
        mOCVTool.PatternPosition.ExpectedPose.Rotation = 0;
        mOCVTool.PatternPosition.ExpectedPose.Scaling = 1;
        mOCVTool.PatternPosition.SelectedSpaceName = mImage.SelectedSpaceName;
        mOCVTool.PatternPosition.TranslationUncertainty = 4;
        mOCVTool.PatternPosition.RotationUncertainty = 0;
        mOCVTool.PatternPosition.ScalingUncertainty = 0;
        // All set, so run the tool
        mOCVTool.Run ();
        string resultsString = "";
        if (mOCVTool.RunStatus.Exception == null)
        {
          for (int iPos = 0; iPos < sPattern.Length; iPos++)
          {
            if (mOCVTool.Results [iPos].Status == CogOCVCharacterStatusConstants.Verified)
              resultsString = resultsString + mOCVTool.Results [iPos].FontModelName;
            else
              resultsString = resultsString + "?";
            mOCVResults.Add (mOCVTool.Results [iPos]);
          } 
        }               
        // Post the results to the control.
        PostResults (mOCVTool.RunStatus.ProcessingTime + mTimer.Milliseconds, sPattern, resultsString, sPattern.Length);
        btnProcess.Enabled = true;
      }
      catch (Exception ex)
      {
        MessageBox.Show ("ProcessStandard caught exception: " + ex.Message);
      }
      finally
      {
        mCurrentImage = (mCurrentImage + 1 >= mImageFile.Count ? 0 : mCurrentImage + 1);
        this.ResetCursor ();
      }
    }
    /// <summary>
    /// This method called from menu item event handlers
    /// </summary>
    /// <param name="option"></param>
    private void SelectRunMode (MenuOptionConstants option)
    {
      mRunOption = option;
      mCurrentImage = 0;    // reset the image index to start from beginning
      switch (option)
      {
        case MenuOptionConstants.MultipleModels: SelectMultipleModels (); break;
        case MenuOptionConstants.SmartPlacement: SelectSmartPlacement (); break;
        case MenuOptionConstants.Standard: SelectStandard (); break;
        default: mRunOption = MenuOptionConstants.None; break;
      }
    }
    /// <summary>
    /// This method prepares MultipleModels tests
    /// </summary>
    private void SelectMultipleModels ()
    {
      try
      {
        // set cursor to be busy state
        this.Cursor = Cursors.WaitCursor;
        btnProcess.Enabled = false;
        // dispose the current OCV tool if there is one. We will create a new one for
        // each running mode
        if (mOCVTool != null) mOCVTool.Dispose ();
        // create a new OCV Tool
        mOCVTool = new CogOCVTool ();

        txtInfo.Text = "This example uses multiple models at each position. " + 
                      "It is useful for applications in which the number " +
                      "of characters and their positions are known, but each " + 
                      "position may take on a number of character values.  " +
                      "Examples include serial number or credit card validation " +
                      "where fixed-sized font models are employed.  It is slower " +
                      "than Smart Placement, because it has to evaluate many models " +
                      "at each position. However, it is usually faster than the Standard technique." +
                      "Note that numeric processing is much faster " +
                      "than alpha since there are fewer models in the numeric font than the alpha font.";
        txtDescription.Text = "Multiple Models";

        mOCVTool.Pattern.Font = mOCFont;
        // For each position in the string...
        for (int iPos = 0; iPos < mMaxStringLength; iPos++)
        {
          CogOCVCharacterPosition mCharPosition = new CogOCVCharacterPosition ();
          // Add every model to each position except don't add a blank model to the first
          // position due to a bug.
          for (int iModel = 0; iModel < mOCVTool.Pattern.Font.FontModels.Count; iModel++)
          {
            if (iPos != 0 || (iPos == 0 && mOCFont.FontModels [iModel].Type != CogOCFontModelTypeConstants.Blank))
              mCharPosition.AddFontModel (mOCFont.FontModels [iModel].Name,mOCFont.FontModels [iModel].Instance);
          }
          // Set position, rotation, scaling and associated uncertainty parameters
          // Note that in this example the uncertainties are the same for each position,
          // so I'm setting UsePatternXXXX = true.
          mCharPosition.UsePatternCharacterUncertainties = true;
          mCharPosition.UsePatternRunTimeCharacterThresholds = true;
          mCharPosition.Rotation = 0;
          mCharPosition.Scaling = 1;
          mCharPosition.TranslationY = mPatternOriginY;
          // Note that this example assumes that the string to verify is positioned horizontally.
          // Position the next character to be fontWidth away from my current character.
          mCharPosition.TranslationX = mPatternOriginX + iPos * mFontWidth;
          // Add this position to my pattern.
          mOCVTool.Pattern.CharacterPositions.Add (mCharPosition);
        }
        // Set overall pattern uncertainties for translation, rotation, and scale.
        mOCVTool.Pattern.CharacterTranslationUncertainty = 4; //pixels
        mOCVTool.Pattern.CharacterRotationUncertainty = 0.0;  // radians
        mOCVTool.Pattern.CharacterScalingUncertainty = 0.0;   //percentage
        // Set accept and confusion thresholds
        mOCVTool.Pattern.RunTimeCharacterAcceptThreshold = 0.5;
        mOCVTool.Pattern.RunTimeCharacterConfidenceThreshold = 0.0;
        mOCVTool.Pattern.Train ();

        btnProcess.Enabled = true;
      }
      catch (Exception ex)
      {
        MessageBox.Show (ex.Message, "OCVPatternChange");
      }
      finally
      {
        // reset cursor
        this.ResetCursor ();
      }
    }
    /// <summary>
    /// This method prepares SmartPlacement tests
    /// </summary>
    private void SelectSmartPlacement ()
    {
      try
      {
        // set cursor to be busy state
        this.Cursor = Cursors.WaitCursor;
        btnProcess.Enabled = false;

        txtInfo.Text = "This example uses a single model at each position. It employs a different pattern " +
                "at each position, which is dynamically placed at run-time based on the previous found model.  " +
                "It is useful for applications in which the number " +
                "of characters and their positions are known, but each " +
                "position may take on a number of character values.  " +
                "Examples include serial number or credit card validation " +
                "where fixed-sized font models are employed.  It is faster than " +
                "the Multiple Models or Standard techniques, because each pattern is pre-trained and " +
                "it has to evaluate only one model " +
                "at each position.  However, it is more prone to positioning errors.";
        txtDescription.Text = "Smart Placement";

        for (int iModel = 0, iPattern = 0; iModel < mOCFont.FontModels.Count; iModel++)
        {
          CogOCVCharacterPosition mCharPosition = new CogOCVCharacterPosition ();
          CogOCFontModel model = mOCFont.FontModels [iModel];
          // Note that we don't create a blank pattern - it's not allowed in OCV. Instead,
          // we will skip blank positions at run-time.
          if (model.Type != CogOCFontModelTypeConstants.Blank)
          {
            // mSmartPatterns holds all the trained patterns. There is one pattern for
            // each character model.  The only thing in a given pattern is a single
            // character model.
            CogOCVPattern pattern = mSmartPatterns [iPattern] as CogOCVPattern;
            pattern.Font = mOCFont;
            mCharPosition.AddFontModel (model.Name, model.Instance);
            // Set position, rotation, scaling and associated uncertainty parameters
            // Note that in this example the uncertainties are the same for each position,
            // so I'm setting UsePatternXXXX = true.
            mCharPosition.UsePatternCharacterUncertainties = true;
            mCharPosition.UsePatternRunTimeCharacterThresholds = true;
            mCharPosition.Rotation = 0;
            mCharPosition.Scaling = 1;
            mCharPosition.TranslationX = 0;
            mCharPosition.TranslationY = 0;
            // first, clean out the pattern
            pattern.CharacterPositions.Clear ();
            // Add the single character model.
            pattern.CharacterPositions.Add (mCharPosition);
            // Set uncertainties for translation, rotation, and scale.
            pattern.CharacterTranslationUncertainty = 2; //pixels
            pattern.CharacterRotationUncertainty = 0.0;  //radians
            pattern.CharacterScalingUncertainty = 0.0;   //percentage
            // Set accept and confusion thresholds
            pattern.RunTimeCharacterAcceptThreshold = 0.5;
            pattern.RunTimeCharacterConfidenceThreshold = 0.0;
            // Train this pattern.
            pattern.Train ();
            iPattern = iPattern + 1;
          }
        }
        btnProcess.Enabled = true;
      }
      catch (Exception ex)
      {
        MessageBox.Show (ex.Message, "OCVPatternChange");
      }
      finally
      {
        // reset cursor
        this.ResetCursor ();
      }
    }
    /// <summary>
    /// This method prepares Standard tests
    /// </summary>
    private void SelectStandard ()
    {
      try
      {
        // set cursor to be busy state
        this.Cursor = Cursors.WaitCursor;
        btnProcess.Enabled = false;
        // dispose the current OCV tool if there is one. We will create a new one for
        // each running mode
        if (mOCVTool != null) mOCVTool.Dispose ();
        // create a new OCV Tool
        mOCVTool = new CogOCVTool ();

        txtInfo.Text = "This example uses standard OCV for each pattern. " +
              "For each new pattern, an OCV tool is trained and run. " +
              "The total pattern time is the sum of the train time and " +
              "run time.  ";
        txtDescription.Text = "Standard";
        btnProcess.Enabled = true;
      }
      catch (Exception ex)
      {
        MessageBox.Show (ex.Message, "OCVPatternChange");
      }
      finally
      {
        // reset cursor
        this.ResetCursor ();
      }
    }
    /// <summary>
    /// This subroutine displays the graphics on the control
    /// </summary>
    /// <param name="resultString">result string</param>
    private void DisplayGraphics (string resultString)
    {
      mDisplay.DrawingEnabled = false;
      for (int iPos = 0; iPos < resultString.Length; iPos++)
      {
        // Get the center, width, height of the found model.
        // Also compare the font model to the model to verify.
        // If correct, display in a green rectangle, else display in red.
        if (resultString.Substring (iPos, 1) != " " || mRunOption != MenuOptionConstants.SmartPlacement)
        {
          CogOCVResult result = mOCVResults [iPos] as CogOCVResult;
          CogOCFontModel model = mOCFont.FontModels.GetFontModelByNameInstance (result.FontModelName, result.FontModelInstance);
          double centerX = result.GetPose ().TranslationX;
          double centerY = result.GetPose ().TranslationY;
          int width, height;
          if (model.Type == CogOCFontModelTypeConstants.Normal)
          {  // normal font
            width = model.Image.Width;
            height = model.Image.Height;
          }
          else
          { // blank font
            width = model.BlankWidth;
            height = model.BlankHeight;
          }
          CogRectangle rect = new CogRectangle ();
          rect.SetCenterWidthHeight (centerX, centerY, width, height);
          rect.SelectedSpaceName = mOCVTool.InputImage.SelectedSpaceName;
          rect.GraphicDOFEnable = CogRectangleDOFConstants.None;
          if (result.Status == CogOCVCharacterStatusConstants.Verified)
            rect.Color = CogColorConstants.Green;
          else
            rect.Color = CogColorConstants.Red;
          mDisplay.StaticGraphics.Add (rect, "Results");
        }
      }
      mDisplay.DrawingEnabled = true;
    }
    /// <summary>
    /// This method posts the results to the control and insures that the graphics are
    /// presented
    /// </summary>
    /// <param name="totalTime"></param>
    /// <param name="stringToVerify"></param>
    /// <param name="resultString"></param>
    /// <param name="stringLength"></param>
    private void PostResults (double totalTime, string stringToVerify, string resultString, int stringLength)
    {
      double perCharTime = totalTime / stringLength;
      // Post the processing time
      txtTimeTotal.Text = totalTime.ToString ("F2");
      txtTimePerChar.Text = perCharTime.ToString ("F2");
  
      // Post the results string
      txtResult.Text = resultString;
      // Draw graphics on screen
      DisplayGraphics (resultString);
    }
    #endregion

    #region Main
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() 
    {
      Application.Run(new OCVPatternChangeForm());
    }
    #endregion

    #region Private Control Event Handlers
    private void btnProcess_Click(object sender, System.EventArgs e)
    {
      switch (mRunOption)
      {
        case MenuOptionConstants.MultipleModels:
          ProcessMultipleModels (); break;
        case MenuOptionConstants.SmartPlacement:
          ProcessSmartPlacement (); break;
        case MenuOptionConstants.Standard:
          ProcessStandard (); break;
      };
    }

    private void mnuMultipleModels_Click (object sender, System.EventArgs e)
    {
      SelectRunMode (MenuOptionConstants.MultipleModels);
    }

    private void mnuStandard_Click (object sender, System.EventArgs e)
    {
      SelectRunMode (MenuOptionConstants.Standard);    
    }

    private void mnuSmartPlacement_Click (object sender, System.EventArgs e)
    {
      SelectRunMode (MenuOptionConstants.SmartPlacement);
    }

    private void mnuExit_Click (object sender, System.EventArgs e)
    {
      this.Close ();
    }
    private void optNumeric_Checked(object sender, System.EventArgs e)
    {
      if (optNumeric.Checked)
      {
        OpenDB (); // openDB since selection has changed
        SelectRunMode (mRunOption);    
      }
    }

    private void optAlpha_Checked(object sender, System.EventArgs e)
    {
      if (optAlpha.Checked)
      {
        OpenDB (); // openDB since selection has changed
        SelectRunMode (mRunOption);    
      }
    }
    #endregion
  }
}
