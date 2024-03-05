//*****************************************************************************
// Copyright (C) 2007 Cognex Corporation
//
// Subject to Cognex Corporation's terms and conditions and license
// agreement, you are authorized to use and modify this source code in
// any way you find useful, provided the Software and/or the modified
// Software is used solely in conjunction with a Cognex Machine Vision
// System.  Furthermore you acknowledge and agree that Cognex has no
// warranty, obligations or liability for your use of the Software.
//*****************************************************************************
//
// This sample program is designed to illustrate certain VisionPro
// features or techniques in the simplest way possible. It is not
// intended as the framework for a complete application. In particular,
// the sample program may not provide proper error handling, event
// handling, cleanup, repeatability, and other mechanisms that a
// commercial quality application requires.
//
// This sample program demonstrates the programmatic use of the VisionPro
// CogOCVMaxTool.
//
// This program assumes that you have some knowledge of C# and VisionPro
// programming.
//
//*****************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.OCVMax;

namespace OCVMax
{
  public partial class Form1 : Form
    {
    private string mVProRoot;
    private CogImageFileTool mImageFileTool;
    private CogOCVMaxTool mOCVMaxTool;
    private CogOCVMaxPoseHelper mPoseHelper;
    private CogCompositeShape mTextGraphic;


    public Form1()
      {
      InitializeComponent();

      mDescriptionTextBox.Clear();
      mDescriptionTextBox.AppendText("Sample Description: This application " +
        "demonstrates programmatic use of the CogOCVMaxTool.");
      mDescriptionTextBox.AppendText(Environment.NewLine);
      mDescriptionTextBox.AppendText("Sample Usage: To begin, click on the " +
        "Start Setup button and then use the mouse to adjust the postion, " +
        "rotation, and scaling of the text graphics so that they match " +
        "the input image.");

      mVProRoot = null;
      mImageFileTool = null;
      mOCVMaxTool = null;
      mPoseHelper = null;
      mTextGraphic = null;

      } // public Form1()


    private void InitVProRoot()
      {
      mVProRoot = null;

      try
        {
        mVProRoot = System.Environment.GetEnvironmentVariable("VPRO_ROOT");
        if (mVProRoot == null)
          throw new Exception("Could not find VPRO_ROOT " +
            "environment variable.");
        }
      catch(Exception exc)
        {
        DisplayErrorThenExit(exc.Message);
        }

      } // private void InitVProRoot()


    private void InitImageFileTool()
      {
      string sImageFile = mVProRoot + "\\Images\\OCVSample.idb";

      try
        {
        mImageFileTool = new CogImageFileTool();

        mImageFileTool.Operator.Open(sImageFile,
          CogImageFileModeConstants.Read);

        mImageFileTool.Run();
        }
      catch (Exception exc)
        {
        DisplayErrorThenExit(exc.Message);
        }

      } //private void InitImageFileTool()


    private CogOCVMaxFont InitFont()
      {
      string fontPath = mVProRoot + "\\fonts\\1LS-Arial.cst";
      CogOCVMaxFont aFont = null;

      try
        {
        aFont = new CogOCVMaxFont();
        aFont.Import(fontPath);
        if (!aFont.Imported)
          throw new Exception("Could not import font " + fontPath);
        }
      catch (Exception exc)
        {
        DisplayErrorThenExit(exc.Message);
        }

      return aFont;

      } // private CogOCVMaxFont InitFont()


    private void Form1_Load(object sender, EventArgs e)
      {
      InitVProRoot();

      InitImageFileTool();

      mOCVMaxTool = new CogOCVMaxTool();

      mPoseHelper = new CogOCVMaxPoseHelper();

      // Use DataBindings to connect the image file tool's output image
      // to the ocvmax tool's input image. This automatically updates
      // every time the image file tool has a new output image.
      mOCVMaxTool.DataBindings.Add("InputImage", mImageFileTool,
        "OutputImage");

      // Point the tool display at the ocvmax tool and select
      // the current inspection record.
      mToolDisplay.Tool = mOCVMaxTool;
      mToolDisplay.SelectedRecordKey = "Current.InputImage";

      // Point display status bar at the display.
      mDisplayStatusBar.Display = mToolDisplay.Display;

      mStartSetupButton.Enabled = true;

      } // private void Form1_Load(...)


    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
      {
      if (mImageFileTool != null)
        if (mImageFileTool.Operator != null)
          mImageFileTool.Operator.Close();

      } // private void Form1_FormClosing(...)


    private void mStartSetupButton_Click(object sender, EventArgs e)
      {
      try
        {
        // We're going to put graphics directly on the tool display,
        // so temporarily prevent user from selecting other than
        // the current input image.
        mToolDisplay.SelectedRecordKey = "Current.InputImage";
        mToolDisplay.Controls["cboDisplayedImage"].Enabled = false;

        CogOCVMaxFont aFont = InitFont();

        CogOCVMaxPattern aPattern = mOCVMaxTool.Pattern;

        aPattern.AddText("476-582");

        CogOCVMaxParagraphIterator aPI = aPattern.CreateParagraphIterator(-1);
        aPI.MoveToPosition(0);

        aPI.Font = aFont;
        aPI.SetAlphabet(" 0123456789-");
        aPI.Polarity = CogOCVMaxPolarityConstants.DarkOnLight;
        aPI.FontRenderParams.SpotSizeScale = 1.28;
        aPI.FontRenderParams.SpotSpacingScaleX = 1.0;
        aPI.FontRenderParams.SpotSpacingScaleY = 1.08;
        aPI.FontRenderParams.StrokeWidthAdded = 0;
        aPI.FontRenderParams.CharacterSpacingAddedX = -3276;
        aPI.FontRenderParams.CharacterSpacingAddedY = 0;

        // Noise Level == Low : ScoreUsingOptimizedClutter
        // Noise Level == High : ScoreUsingClutter
        // Noise Level == Extreme : ScoreWithoutClutter
        aPattern.ScoreMode =
          CogOCVMaxScoreModeConstants.ScoreUsingOptimizedClutter;

        // Registration Mode Clean Print checked : Correlation
        // Registration Mode Clean Print *not* checked: Standard
        aPattern.CharacterRegistration =
          CogOCVMaxCharacterRegistrationConstants.Standard;

        // aPattern.Origin: no need to set for this example
        // aPattern.TrainTrainsform: ditto
        //aPattern.ExpectedDeformationRate: ditto
        // aPattern.DOFEnable: default angle + uniform scale ok this example

        aPattern.TrainTimeout = 30000.0; // 30,000 milliseconds = 30 seconds
        aPattern.TrainTimeoutEnabled = false;

        // The following is mostly about figuring reasonable
        // nominal scaling and rotation values for this paragraph ...
        mPoseHelper.Font = aPI.Font;
        mPoseHelper.FontRenderParams = aPI.FontRenderParams;
        mPoseHelper.SetText("476-582");
        mPoseHelper.InitialPoseEstimate(mOCVMaxTool.InputImage);
        mTextGraphic = mPoseHelper.FinalPosePrepare();
        mTextGraphic.Interactive = true;

        mToolDisplay.Display.InteractiveGraphics.Add(
          mTextGraphic, "TextGraphic", true);

        }
      catch (Exception exc)
        {
        DisplayErrorThenExit(exc.Message);
        }

      mDescriptionTextBox.Clear();
      mDescriptionTextBox.AppendText("Sample Description: This application " +
        "demonstrates programmatic use of the CogOCVMaxTool.");
      mDescriptionTextBox.AppendText(Environment.NewLine);
      mDescriptionTextBox.AppendText("Sample Usage: After adjusting the " +
        "position, rotation, and scaling of the text graphics so that " +
        "they match the input image, click on the Finish Setup button " +
        "to train and finish configuring the tool.");

      mFinishSetupButton.Enabled = true;
      mStartSetupButton.Enabled = false;
      
      } // private void mStartSetupButton_Click(...)


    private void mFinishSetupButton_Click(object sender, EventArgs e)
      {
      Cursor = Cursors.WaitCursor;  // this may take a while ...

      try
        {
        CogOCVMaxPattern aPattern = mOCVMaxTool.Pattern;

        CogOCVMaxParagraphIterator aPI = aPattern.CreateParagraphIterator(-1);
        aPI.MoveToPosition(0);

        // Now, we can utilize the user adjusted pose of the text
        // graphic to set the paragraph pose.
        aPI.Pose = mPoseHelper.FinalPoseComplete(mTextGraphic);

        mToolDisplay.Display.InteractiveGraphics.Clear();
        mTextGraphic = null;

        aPattern.Train();

        if (! aPattern.Trained)
          throw new Exception("CogOCVMaxTool failed to train!");

        // Now set up run params ...
        CogOCVMaxRunParams aRunParams = mOCVMaxTool.RunParams;

        aRunParams.ComputePoses |= CogOCVMaxComputePosesConstants.Paragraph;

        aRunParams.SetArrangementFromPattern(aPattern);

        // for this example, search entire image ...
        aRunParams.RunPose = null; // do not search around run pose
        mOCVMaxTool.Region = null; // do not search image within region

        const CogOCVMaxDOFAttributeConstants eNominal =
          CogOCVMaxDOFAttributeConstants.Nominal;
        const CogOCVMaxDOFAttributeConstants eLowerLimit =
          CogOCVMaxDOFAttributeConstants.LowerLimit;
        const CogOCVMaxDOFAttributeConstants eUpperLimit =
          CogOCVMaxDOFAttributeConstants.UpperLimit;

        // set up image search params ...
        CogOCVMaxSearchParams aImageSearchParams =
          aRunParams.ImageSearchParams; // not StartPoseSearchParams

        aImageSearchParams.UncertaintyX = 5;
        aImageSearchParams.UncertaintyY = 5;

        aImageSearchParams.AcceptThreshold = 0.40;

        aImageSearchParams.DOFEnable =
          CogOCVMaxDOFConstants.Rotation |
          CogOCVMaxDOFConstants.Scale;

        aImageSearchParams.SetDOFRotation(eNominal, CogMisc.DegToRad(0.0));
        aImageSearchParams.SetDOFRotation(eLowerLimit, CogMisc.DegToRad(-2.5));
        aImageSearchParams.SetDOFRotation(eUpperLimit, CogMisc.DegToRad(7.4));

        aImageSearchParams.SetDOFScale(eNominal, 1.00);
        aImageSearchParams.SetDOFScale(eLowerLimit, 0.85);
        aImageSearchParams.SetDOFScale(eUpperLimit, 1.10);

        aImageSearchParams.SetDOFScaleX(eNominal, 1.00);
        aImageSearchParams.SetDOFScaleX(eLowerLimit, 0.95);
        aImageSearchParams.SetDOFScaleX(eUpperLimit, 1.05);

        aImageSearchParams.SetDOFScaleY(eNominal, 1.00);
        aImageSearchParams.SetDOFScaleY(eLowerLimit, 0.95);
        aImageSearchParams.SetDOFScaleY(eUpperLimit, 1.05);

        aImageSearchParams.SetDOFShear(eNominal, CogMisc.DegToRad(0.0));
        aImageSearchParams.SetDOFShear(eLowerLimit, CogMisc.DegToRad(0.0));
        aImageSearchParams.SetDOFShear(eUpperLimit, CogMisc.DegToRad(0.0));


        // set up character search params ...
        CogOCVMaxSearchParams aCharacterSearchParams =
          aRunParams.KeySearchParams;

        aCharacterSearchParams.UncertaintyX = 9.6;
        aCharacterSearchParams.UncertaintyY = 6.0;

        aCharacterSearchParams.AcceptThreshold = 0.30;

        aCharacterSearchParams.DOFEnable =
          CogOCVMaxDOFConstants.Rotation |
          CogOCVMaxDOFConstants.Scale;

        aCharacterSearchParams.SetDOFRotation(eUpperLimit,
          CogMisc.DegToRad(8.5));
        aCharacterSearchParams.SetDOFScale(eUpperLimit, 1.07);
        aCharacterSearchParams.SetDOFScaleX(eUpperLimit, 1.05);
        aCharacterSearchParams.SetDOFScaleY(eUpperLimit, 1.05);
        aCharacterSearchParams.SetDOFShear(eUpperLimit,
          CogMisc.DegToRad(0.0));

        // confusion params for paragraph zero ...
        aRunParams.SetConfusionThreshold(0, 0.5);
        aRunParams.SetConfidenceThreshold(0, 0.0);

        // set up advanced params ...
        aRunParams.TimeoutEnabled = false;
        aRunParams.Timeout = 250.0; // milliseconds
        aRunParams.EarlyAcceptThreshold = 0.75;
        aRunParams.EarlyFailThreshold = 1.0;
        aImageSearchParams.ContrastThreshold = 10;
        aImageSearchParams.XYOverlap = 0.8;

        // enable graphics ...
        mOCVMaxTool.LastRunRecordEnable =
          CogOCVMaxLastRunRecordConstants.ParagraphBounds |
          CogOCVMaxLastRunRecordConstants.ParagraphCharactersBounds |
          CogOCVMaxLastRunRecordConstants.ParagraphCharactersLabel;

        mOCVMaxTool.LastRunRecordDiagEnable =
          CogOCVMaxLastRunRecordDiagConstants.InputImageByReference |
          CogOCVMaxLastRunRecordDiagConstants.Region;

        }
      catch (Exception exc)
        {
        DisplayErrorThenExit(exc.Message);
        }
      finally
        {
        Cursor = Cursors.Default;
        }

      // Restore the user's ability to switch among images ...
      mToolDisplay.Controls["cboDisplayedImage"].Enabled = true;

      mDescriptionTextBox.Clear();
      mDescriptionTextBox.AppendText("Sample Description: This application " +
        "demonstrates programmatic use of the CogOCVMaxTool.");
      mDescriptionTextBox.AppendText(Environment.NewLine);
      mDescriptionTextBox.AppendText("Sample Usage: Click on the " +
        "Run button to process the next image in the database.");

      mFinishSetupButton.Enabled = false;
      mRunButton.Enabled = true;

      } // private void mFinishSetupButton_Click(object sender, EventArgs e)


    private void mRunButton_Click(object sender, EventArgs e)
      {
      Cursor = Cursors.WaitCursor; // this may take a while ...
      try
        {
        mResultsTextBox.Clear();
        mImageFileTool.Run();
        mOCVMaxTool.Run();
        mToolDisplay.SelectedRecordKey = "LastRun.InputImage";
        DisplayResults();
        }
      catch (Exception exc)
        {
        DisplayErrorThenExit(exc.Message);
        }
      finally
        {
        Cursor = Cursors.Default;
        }

      } // private void mRunButton_Click(...)


    private bool IsRenderableUnicode(
      char aC)
      {
      bool isRenderable;

      if (System.Char.IsLetterOrDigit(aC))
        isRenderable = true;
      else if (System.Char.IsPunctuation(aC))
        isRenderable = true;
      else if (System.Char.IsSymbol(aC))
        isRenderable = true;
      else if (aC == ' ')
        isRenderable = true;
      else
        isRenderable = false;

      return isRenderable;

      } // private bool IsRenderableUnicode(...)


    private void DisplayResults()
      {
      string sNL = Environment.NewLine;

      CogToolResultConstants eStatus = mOCVMaxTool.RunStatus.Result;

      if (eStatus != CogToolResultConstants.Accept)
        {
        mResultsTextBox.AppendText("CogOCVMaxTool Run Status: " +
          eStatus.ToString() + sNL);
        return;
        }

      CogOCVMaxResult aRes = mOCVMaxTool.Result;

      if (aRes.TimedOut)
        {
        mResultsTextBox.AppendText("CogOCVMaxTool timed out!" + sNL);
        return;
        }

      mResultsTextBox.AppendText("Pattern Verified: " +
        aRes.Verified.ToString() + sNL);

      mResultsTextBox.AppendText("Pattern Verification Score: " +
        aRes.VerificationScore.ToString() + sNL);

      mResultsTextBox.AppendText("Num Paragraphs Verified: " +
        aRes.NumParagraphsVerified.ToString() + sNL);

      CogOCVMaxParagraphResult[] aPRs = aRes.ParagraphResults;

      int nP = aPRs.Length;
      for(int iP = 0; iP < nP; iP++)
        {
        mResultsTextBox.AppendText("Paragraph Number " + (iP + 1).ToString() +
          " of " + nP.ToString() + ":" + sNL);

        CogOCVMaxParagraphResult aPR = aPRs[iP];

        mResultsTextBox.AppendText("Paragraph Verified: " +
          aPR.Verified.ToString() + sNL);

        mResultsTextBox.AppendText("Paragraph Verification Score: " +
          aPR.VerificationScore.ToString() + sNL);

        mResultsTextBox.AppendText("Num Lines Verified: " +
          aPR.NumLinesVerified.ToString() + sNL);

        CogOCVMaxLineResult[] aLRs = aPR.LineResults;

        int nL = aLRs.Length;
        for(int iL = 0; iL < nL; iL++)
          {
          mResultsTextBox.AppendText("Line Number " + (iL + 1).ToString() +
            " of " + nL.ToString() + ":" + sNL);

          CogOCVMaxLineResult aLR = aLRs[iL];

          mResultsTextBox.AppendText("Line Verified: " +
            aLR.Verified.ToString() + sNL);

          mResultsTextBox.AppendText("Line Verification Score: " +
            aLR.VerificationScore.ToString() + sNL);

          CogOCVMaxCharacterResult[] aCRs = aLR.CharacterResults;

          int nC = aCRs.Length;
          for(int iC = 0; iC < nC; iC++)
            {
            mResultsTextBox.AppendText("Character Number " + 
              (iC + 1).ToString() + " of " + nC.ToString() + ":" + sNL);

            CogOCVMaxCharacterResult aCR = aCRs[iC];

            mResultsTextBox.AppendText("Character Verification Status: " +
              aCR.VerificationStatus.ToString() + sNL);

            mResultsTextBox.AppendText("Character Verification Score: " +
              aCR.VerificationScore.ToString() + sNL);

            mResultsTextBox.AppendText("Character Verification Match Score: " +
              aCR.VerificationMatchScore.ToString() + sNL);

            mResultsTextBox.AppendText("Character Confidence Score: " +
              aCR.ConfidenceScore.ToString() + sNL);

            int iKey = aCR.CharacterKey;
            if (iKey < 0)
              continue;
            if (iKey > 65535)
              iKey = iKey / 65536;

            mResultsTextBox.AppendText("Character Key: " + 
              iKey.ToString() + sNL);

            char aC = aCR.Character;

            if (IsRenderableUnicode(aC))
              mResultsTextBox.AppendText("Character: " + aC.ToString() + sNL);

            } // for(int iC = 0; iC < nC; iC++)


          } // for(int iL = 0; iL < nL; iL++)


        } // for(int iP = 0; iP < nP; iP++)

      } // private void DisplayResults()


    private void DisplayErrorThenExit(string sMsg)
      {
      MessageBox.Show(sMsg, "OCVMax", MessageBoxButtons.OK);
      this.Close();
      }  // private void DisplayErrorThenExit(string sMsg)


    } // public partial class Form1 : Form


} // namespace OCVMax
