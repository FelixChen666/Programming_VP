//*****************************************************************************
// Copyright (C) 2016 Cognex Corporation
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
// ID Tool to compute process control metrics.
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
using Cognex.VisionPro.ID;
using Cognex.VisionPro.Implementation.Internal; // CogWaitCursor

namespace IDProcessControlMetrics
  {

  public partial class Form1 : Form
    {
    private CogIDTool mTool;


    public Form1()
      {
      InitializeComponent();

      } // public Form1()


    private void Form1_Load(
      object sender,
      EventArgs e)
      {
      // Some minor initialization of the application form ...
      txtDescription.Text = "";
      txtDescription.AppendText("Sample Description: ");
      txtDescription.AppendText("This application demonstrates the ");
      txtDescription.AppendText("programmatic use of the Cognex VisionPro ");
      txtDescription.AppendText("ID Tool to compute process control metrics.");
      txtDescription.AppendText(Environment.NewLine);
      txtDescription.AppendText(Environment.NewLine);
      txtDescription.AppendText("Sample Usage: click on Run to compute the ");
      txtDescription.AppendText("specified process control metrics.");


      // Find our test image and load it ...
      String sPath = Environment.GetEnvironmentVariable("VPRO_ROOT");
      if (sPath == null)
        {
        ShowErrorAndExit("Could not read VPRO_ROOT environment variable.");
        return;
        }
      sPath += "\\images\\IDProcessControlMetrics.cdb";

      CogImageFile aImageFile = new CogImageFile();
      try
        {
        aImageFile.Open(sPath, CogImageFileModeConstants.Read);
        }
      catch (Exception)
        {
        ShowErrorAndExit("Could not load image file " + sPath);
        return;
        }

      CogImage8Grey aImage = (CogImage8Grey)aImageFile[0];

      aImageFile.Close();

      // Create a CogOIDTool ...
      mTool = new CogIDTool();

      // The CogIDTool needs an input image ...
      mTool.InputImage = aImage;

      // and we need to enable (just) DataMatrix symbols ...
      mTool.RunParams.DisableAllCodes();
      mTool.RunParams.DataMatrix.Enabled = true;

      // To start, set DataMatrix.ProcessControlMetrics to None ...
      mTool.RunParams.DataMatrix.ProcessControlMetrics =
        CogIDDataMatrixProcessControlMetricsConstants.None;
 
      ctrlToolDisplay.Tool = mTool;

      radAIMDPM.Checked = true;
      radISO15415.Checked = false;
      radSEMIT10.Checked = false;

      dgvResults.Rows.Clear();      

      } // private void Form1_Load(...)


    private void btnRun_Click(
      object sender,
      EventArgs e)
      {
      // No inputs while we're running ...
      InputsEnable(false);

      // Clear any previous results
      dgvResults.Rows.Clear();

      // Set metrics based upon radio buttons ...
      if (radAIMDPM.Checked)
        mTool.RunParams.DataMatrix.ProcessControlMetrics =
          CogIDDataMatrixProcessControlMetricsConstants.AIMDPM;

      else if (radISO15415.Checked)
        mTool.RunParams.DataMatrix.ProcessControlMetrics =
          CogIDDataMatrixProcessControlMetricsConstants.ISO15415;

      else if (radSEMIT10.Checked)
        mTool.RunParams.DataMatrix.ProcessControlMetrics =
          CogIDDataMatrixProcessControlMetricsConstants.SEMIT10;

      using (new CogWaitCursor())
        {

        // Run the CogIDTool ...
        mTool.Run(); // won't ever throw

        ICogRunStatus aRunStatus = mTool.RunStatus;

        // Check tool run status ...
        if (aRunStatus.Result == CogToolResultConstants.Error)
          {
          ShowError(aRunStatus.Message);
          InputsEnable(true);
          return;
          } // if (aRunStatus.Result == CogToolResultConstants.Error)

        // Make sure there is at least one result ...
        if (mTool.Results.Count < 1)
          {
          ShowError("Symbol not found.");
          InputsEnable(true);
          return;
          }

        CogIDResultDecoded aRD = mTool.Results[0].DecodedData;

        // Make sure the result was decoded ...
        if (aRD == null)
          {
          ShowError("Symbol not decoded.");
          InputsEnable(true);
          return;
          }

        // Fetch references to process control metrics. May be null,
        // and at most one will be non-null ...
        CogIDProcessControlMetricsAIMDPM aAIMDPM =
          aRD.ProcessControlMetricsAIMDPM;

        CogIDProcessControlMetricsISO15415 aISO15415 =
          aRD.ProcessControlMetricsISO15415;

        CogIDProcessControlMetricsSEMIT10 aSEMIT10 =
          aRD.ProcessControlMetricsSEMIT10;

        // Populate results grid ...

        if (aAIMDPM != null)
          PopulateResultsAIMDPM(aAIMDPM);

        else if (aISO15415 != null)
          PopulateResultsISO15415(aISO15415);

        else if (aSEMIT10 != null)
          PopulateResultsSEMIT10(aSEMIT10);

        } // using (new CogWaitCursor)

      InputsEnable(true);

      } // private void btnRun_Click(...)


    private void PopulateResultsAIMDPM(
      CogIDProcessControlMetricsAIMDPM aAIMDPM)
      {
      if (aAIMDPM == null)
        return;

      dgvResults.Rows.Clear();
      dgvResults.Rows.Add(8);

      dgvResults.Rows[0].Cells[0].Value = "Overall";
      dgvResults.Rows[0].Cells[1].Value = aAIMDPM.OverallGrade;

      dgvResults.Rows[1].Cells[0].Value = "Modulation";
      dgvResults.Rows[1].Cells[1].Value = aAIMDPM.ModulationGrade;

      dgvResults.Rows[2].Cells[0].Value = "AxialNonUniformity";
      dgvResults.Rows[2].Cells[1].Value = aAIMDPM.AxialNonUniformityGrade;
      dgvResults.Rows[2].Cells[2].Value = 
        aAIMDPM.AxialNonUniformity.ToString("F4");

      dgvResults.Rows[3].Cells[0].Value = "PrintGrowth";
      dgvResults.Rows[3].Cells[2].Value =
        aAIMDPM.PrintGrowth.ToString("F4");

      dgvResults.Rows[4].Cells[0].Value = "UEC";
      dgvResults.Rows[4].Cells[1].Value = aAIMDPM.UECGrade;
      dgvResults.Rows[4].Cells[2].Value = 
        aAIMDPM.UEC.ToString("F4");

      dgvResults.Rows[5].Cells[0].Value = "SymbolContrast";
      dgvResults.Rows[5].Cells[1].Value = aAIMDPM.SymbolContrastGrade;
      dgvResults.Rows[5].Cells[2].Value = 
        aAIMDPM.SymbolContrast.ToString("F4");

      dgvResults.Rows[6].Cells[0].Value = "FixedPatternDamage";
      dgvResults.Rows[6].Cells[1].Value = aAIMDPM.FixedPatternDamageGrade;

      dgvResults.Rows[7].Cells[0].Value = "GridNonUniformity";
      dgvResults.Rows[7].Cells[1].Value = aAIMDPM.GridNonUniformityGrade;
      dgvResults.Rows[7].Cells[2].Value = 
        aAIMDPM.GridNonUniformity.ToString("F4");

      } // private void PopulateResultsAIMDPM(...)


    private void PopulateResultsISO15415(
      CogIDProcessControlMetricsISO15415 aISO15415)
      {
      if (aISO15415 == null)
        return;

      dgvResults.Rows.Clear();
      dgvResults.Rows.Add(9);

      dgvResults.Rows[0].Cells[0].Value = "Overall";
      dgvResults.Rows[0].Cells[1].Value = aISO15415.OverallGrade;

      dgvResults.Rows[1].Cells[0].Value = "Modulation";
      dgvResults.Rows[1].Cells[1].Value = aISO15415.ModulationGrade;

      dgvResults.Rows[2].Cells[0].Value = "AxialNonUniformity";
      dgvResults.Rows[2].Cells[1].Value = aISO15415.AxialNonUniformityGrade;
      dgvResults.Rows[2].Cells[2].Value = 
        aISO15415.AxialNonUniformity.ToString("F4");

      dgvResults.Rows[3].Cells[0].Value = "PrintGrowth";
      dgvResults.Rows[3].Cells[2].Value =
        aISO15415.PrintGrowth.ToString("F4");

      dgvResults.Rows[4].Cells[0].Value = "UEC";
      dgvResults.Rows[4].Cells[1].Value = aISO15415.UECGrade;
      dgvResults.Rows[4].Cells[2].Value = 
        aISO15415.UEC.ToString("F4");

      dgvResults.Rows[5].Cells[0].Value = "SymbolContrast";
      dgvResults.Rows[5].Cells[1].Value = aISO15415.SymbolContrastGrade;
      dgvResults.Rows[5].Cells[2].Value = 
        aISO15415.SymbolContrast.ToString("F4");

      dgvResults.Rows[6].Cells[0].Value = "FixedPatternDamage";
      dgvResults.Rows[6].Cells[1].Value = aISO15415.FixedPatternDamageGrade;

      dgvResults.Rows[7].Cells[0].Value = "GridNonUniformity";
      dgvResults.Rows[7].Cells[1].Value = aISO15415.GridNonUniformityGrade;
      dgvResults.Rows[7].Cells[2].Value = 
        aISO15415.GridNonUniformity.ToString("F4");

      dgvResults.Rows[8].Cells[0].Value = "ExtremeReflectance";
      dgvResults.Rows[8].Cells[1].Value = aISO15415.ExtremeReflectanceGrade;

      } // private void PopulateResultsISO15415(...)


    private void PopulateResultsSEMIT10(
      CogIDProcessControlMetricsSEMIT10 aSEMIT10)
      {
      if (aSEMIT10 == null)
        return;

      dgvResults.Rows.Clear();
      dgvResults.Rows.Add(13);

      dgvResults.Rows[0].Cells[0].Value = "AxialNonUniformity";
      dgvResults.Rows[0].Cells[2].Value =
        aSEMIT10.AxialNonUniformity.ToString("F4");

      dgvResults.Rows[1].Cells[0].Value = "PrintGrowth";
      dgvResults.Rows[1].Cells[2].Value =
        aSEMIT10.PrintGrowth.ToString("F4");

      dgvResults.Rows[2].Cells[0].Value = "UEC";
      dgvResults.Rows[2].Cells[2].Value =
        aSEMIT10.UEC.ToString("F4");

      dgvResults.Rows[3].Cells[0].Value = "SymbolContrast";
      dgvResults.Rows[3].Cells[2].Value =
        aSEMIT10.SymbolContrast.ToString("F4");

      dgvResults.Rows[4].Cells[0].Value = "SignalToNoiseRatio";
      dgvResults.Rows[4].Cells[2].Value =
        aSEMIT10.SignalToNoiseRatio.ToString("F4");

      dgvResults.Rows[5].Cells[0].Value = "HorizontalMarkGrowth";
      dgvResults.Rows[5].Cells[2].Value =
        aSEMIT10.HorizontalMarkGrowth.ToString("F4");

      dgvResults.Rows[6].Cells[0].Value = "VerticalMarkGrowth";
      dgvResults.Rows[6].Cells[2].Value =
        aSEMIT10.VerticalMarkGrowth.ToString("F4");

      dgvResults.Rows[7].Cells[0].Value = "DataMatrixCellWidth";
      dgvResults.Rows[7].Cells[2].Value =
        aSEMIT10.DataMatrixCellWidth.ToString("F4");

      dgvResults.Rows[8].Cells[0].Value = "DataMatrixCellHeight";
      dgvResults.Rows[8].Cells[2].Value =
        aSEMIT10.DataMatrixCellHeight.ToString("F4");

      dgvResults.Rows[9].Cells[0].Value = "HorizontalMarkMisplacement";
      dgvResults.Rows[9].Cells[2].Value =
        aSEMIT10.HorizontalMarkMisplacement.ToString("F4");

      dgvResults.Rows[10].Cells[0].Value = "VerticalMarkMisplacement";
      dgvResults.Rows[10].Cells[2].Value =
        aSEMIT10.VerticalMarkMisplacement.ToString("F4");

      dgvResults.Rows[11].Cells[0].Value = "CellDefects";
      dgvResults.Rows[11].Cells[2].Value =
        aSEMIT10.CellDefects.ToString("F4");

      dgvResults.Rows[12].Cells[0].Value = "FinderPatternDefects";
      dgvResults.Rows[12].Cells[2].Value =
        aSEMIT10.FinderPatternDefects.ToString("F4");

      } // private void PopulateResultsSEMIT10(...)


    private void ShowError(
      String sErrTxt)
      {
      String sMsg = "Error running CogIDTool.";
      if (sErrTxt != null)
        if (sErrTxt != "")
          sMsg = sErrTxt;

      MessageBox.Show(sMsg, "ID Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error);

      } // private void ShowError(String sErrTxt)


    private void ShowErrorAndExit(
      String sMsg)
      {
      ShowError(sMsg);

      Application.Exit();

      } // private void ShowErrorAndExit(...)


    private void InputsEnable(
      Boolean bEnable)
      {
      radAIMDPM.Enabled = bEnable;
      radISO15415.Enabled = bEnable;
      radSEMIT10.Enabled = bEnable;
      btnRun.Enabled = bEnable;

      } // private void InputsEnable(...)


    private void radAIMDPM_CheckedChanged(
      object sender,
      EventArgs e)
      {
      dgvResults.Rows.Clear();
      } // private void radAIMDPM_CheckedChanged(...)


    private void radISO15415_CheckedChanged(
      object sender,
      EventArgs e)
      {
      dgvResults.Rows.Clear();
      } // private void radISO15415_CheckedChanged(...)


    private void radSEMIT10_CheckedChanged(
      object sender,
      EventArgs e)
      {
      dgvResults.Rows.Clear();
      } // private void radSEMIT10_CheckedChanged(...)


    } // public partial class Form1 : Form


  } // namespace IDProcessControlMetrics
