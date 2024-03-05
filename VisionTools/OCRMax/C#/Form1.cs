//*****************************************************************************
// Copyright (C) 2011 Cognex Corporation
//
// Subject to Cognex Corporation's terms and conditions and license
// agreement, you are authorized to use and modify this source code in
// any way you find useful, provided the Software and/or the modified
// Software is used solely in conjunction with a Cognex Machine Vision
// System.  Furthermore you acknowledge and agree that Cognex has no
// warranty, obligations or liability for your use of the Software.
//*****************************************************************************
// This sample program is designed to illustrate certain VisionPro
// features or techniques in the simplest way possible. It is not
// intended as the framework for a complete application. In particular,
// the sample program may not provide proper error handling, event
// handling, cleanup, repeatability, and other mechanisms that a
// commercial quality application requires.
//
// This program assumes that you have some knowledge of C# and VisionPro
// programming.
//
// This sample program demonstrates the programmatic use of the VisionPro
// OCRMax Tool.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.OCRMax;

namespace OCRMaxSample
  {

  public partial class Form1 : Form
    {

    private CogOCRMaxTool mTool;


    public Form1()
      {
      InitializeComponent();

      } // public Form1()


    private void DisplayErrorAndExit(
      String sMsg)
      {
      MessageBox.Show(sMsg, "OCRMax Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
      Application.Exit();

      } // private void DisplayErrorAndExit(...)


    private void Form1_Load(
      object sender,
      EventArgs e)
      {
      // Some minor initialization of the application form ...
      txtDescription.Text = "";
      txtDescription.AppendText("Sample Description: ");
      txtDescription.AppendText("This application demonstrates the ");
      txtDescription.AppendText("programmatic use of the Cognex VisionPro ");
      txtDescription.AppendText("OCRMax Tool.");
      txtDescription.AppendText(Environment.NewLine);
      txtDescription.AppendText("Sample Usage: click on Run to ");
      txtDescription.AppendText("perform OCRMax with the specified fielding.");


      // Find our test image and load it ...
      String sPath = Environment.GetEnvironmentVariable("VPRO_ROOT");
      if (sPath == null)
        {
        DisplayErrorAndExit("Could not read VPRO_ROOT environment variable.");
        return;
        }
      sPath += "\\images\\alphanumbers.bmp";

      CogImageFile aImageFile = new CogImageFile();
      try
        {
        aImageFile.Open(sPath, CogImageFileModeConstants.Read);
        }
      catch (Exception)
        {
        DisplayErrorAndExit("Could not load image file " + sPath);
        return;
        }

      CogImage8Grey aImage = (CogImage8Grey)aImageFile[0];

      aImageFile.Close();

      // Define an appropriate region of interest ...
      CogRectangleAffine aROI = new CogRectangleAffine();
      aROI.SetOriginLengthsRotationSkew(340, 748, 1010, 93, 0, 0);

      // Create a CogOCRMaxTool ...
      mTool = new CogOCRMaxTool();

      // Start to configure the tool ...
      mTool.InputImage = aImage;
      mTool.Region = aROI;

      // Initially, run just the tool's segmenter. We'll use
      // segmentation results to populate a font object for
      // use by the classifier ...
      CogOCRMaxSegmenterParagraphResult aSegmenterParagraphResult = null;
      try
        {
        aSegmenterParagraphResult = mTool.Segmenter.Execute(aImage, aROI);
        }
      catch (Exception aX)
        {
        DisplayErrorAndExit(aX.Message);
        return;
        }

      CogOCRMaxSegmenterLineResult aSegmenterLineResult =
        aSegmenterParagraphResult[0];

      // We know apriori that this image with this ROI will yield
      // segmented images of the alphabet A through Z ...
      int[] aAlphabet = new int[] {
        (int)'A', (int)'B', (int)'C', (int)'D', (int)'E', (int)'F',
        (int)'G', (int)'H', (int)'I', (int)'J', (int)'K', (int)'L',
        (int)'M', (int)'N', (int)'O', (int)'P', (int)'Q', (int)'R',
        (int)'S', (int)'T', (int)'U', (int)'V', (int)'W', (int)'X',
        (int)'Y', (int)'Z'};

      int iC = 0;
      foreach(CogOCRMaxSegmenterPositionResult aSegmenterPositionResult in
        aSegmenterLineResult)
        {
        CogOCRMaxChar aC = aSegmenterPositionResult.Character;
        aC.CharacterCode = aAlphabet[iC];
        mTool.Classifier.Font.Add(aC);
        iC++;
        } // for each(CogOCRMaxSegmenterPositionResult ...

      // Now we can train the classifier ...
      try
        {
        mTool.Classifier.Train();
        }
      catch (Exception aX)
        {
        DisplayErrorAndExit(aX.Message);
        return;
        }
      if (!mTool.Classifier.Trained)
        {
        DisplayErrorAndExit("Could not train classifier.");
        return;
        }

      // Enable the fielding
      mTool.FieldingEnabled = true;

      // Now some final form setup from the tool ...
      txtFieldString.Text = mTool.Fielding.FieldString;
      chkFieldingAliasAlpha.Checked =
        mTool.Fielding.FieldingDefinitions['A'].Enabled;
      chkFieldingAliasNumeric.Checked =
        mTool.Fielding.FieldingDefinitions['N'].Enabled;
      chkFieldingAliasAny.Checked =
        mTool.Fielding.FieldingDefinitions['*'].Enabled;
      ctrlToolDisplay.Tool = mTool;

      } // private void Form1_Load(...)


    private void txtFieldString_TextChanged(
      object sender,
      EventArgs e)
      {
      mTool.Fielding.FieldString = txtFieldString.Text;

      } // private void txtFieldString_TextChanged(...)


    private void chkFieldingAliasAlpha_CheckedChanged(
      object sender,
      EventArgs e)
      {
      CogOCRMaxFieldingDefinition aFD =
        mTool.Fielding.FieldingDefinitions['A'];
      if (aFD == null)
        return;

      aFD.Enabled = chkFieldingAliasAlpha.Checked;

      } // private void chkFieldingAliasAlpha_CheckedChanged(...)


    private void chkFieldingAliasNumeric_CheckedChanged(
      object sender,
      EventArgs e)
      {
      CogOCRMaxFieldingDefinition aFD =
        mTool.Fielding.FieldingDefinitions['N'];
      if (aFD == null)
        return;

      aFD.Enabled = chkFieldingAliasNumeric.Checked;

      } // private void chkFieldingAliasNumeric_CheckedChanged(...)


    private void chkFieldingAliasAny_CheckedChanged(
      object sender,
      EventArgs e)
      {
      CogOCRMaxFieldingDefinition aFD =
        mTool.Fielding.FieldingDefinitions['*'];
      if (aFD == null)
        return;

      aFD.Enabled = chkFieldingAliasAny.Enabled;

      } // private void chkFieldingAliasAny_CheckedChanged(...)


    private void btnRun_Click(
      object sender,
      EventArgs e)
      {
      txtResult.Text = "";

      txtFieldString.Enabled = false;
      chkFieldingAliasAlpha.Enabled = false;
      chkFieldingAliasNumeric.Enabled = false;
      chkFieldingAliasAny.Enabled = false;

      mTool.Run(); // won't ever throw

      ICogRunStatus aRunStatus = mTool.RunStatus;

      if (aRunStatus.Result == CogToolResultConstants.Error)
        {
        String sMsg = "Error running CogOCRMaxTool.";
        if (aRunStatus.Message != null)
          sMsg += Environment.NewLine + aRunStatus.Message;
        MessageBox.Show(sMsg, "OCRMax Error",
          MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      else
        txtResult.Text = mTool.LineResult.ResultString;

      txtFieldString.Enabled = true;
      chkFieldingAliasAlpha.Enabled = true;
      chkFieldingAliasNumeric.Enabled = true;
      chkFieldingAliasAny.Enabled = true;

      } // private void btnRun_Click(...)


    } // public partial class Form1 : Form


  } // namespace OCRMaxSample
