//*****************************************************************************
// Copyright (C) 2023 Cognex Corporation
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
// CogSegmentTool. Specifically, this application demos the following:
// - Loading a previously configured CogSegmentTool from a .VPP file.
// - Creating and interacting with a SegmentEdit control.
// - Using the CogBlobTool to postprocess the CogSegmentTool's output
//   heatmap.
//
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.ViDiEL;
using Cognex.VisionProUI.ViDiEL.Segment.Controls;


namespace SegmentApp
{
  public partial class SegmentApp : Form
  {
    private CogImageFileTool mImageFileTool;
    private CogSegmentTool mSegmentTool;
    private CogBlobTool mBlobTool;

    public SegmentApp()
    {
      InitializeComponent();

      mImageFileTool = new CogImageFileTool();
      mImageFileCtrl.Subject = mImageFileTool;

      mBlobTool = new CogBlobTool();
      mBlobEditCtrl.Subject = mBlobTool;

      mSegmentTool = new CogSegmentTool();

      SegmentEdit aSegmentEdit = new SegmentEdit();

      // NOTE: SegmentEdit is a WPF control, so we will reference it in
      // our segment edit form's ElementHost.
      mElementHost.Child = aSegmentEdit;

      // NOTE: the subject of the SegmentEdit is the VI segment
      // tool available through CogSegmentTool's VisionInteropTool
      // property.
      aSegmentEdit.Subject = mSegmentTool.VisionInteropTool;

      // NOTE: add delegates to get the SegmentEdit to behave
      // appropriately for our VPro 9x application.
      aSegmentEdit.ViewModel.OpenDelegate = SegmentOpen;
      aSegmentEdit.ViewModel.SaveDelegate = SegmentSave;
      aSegmentEdit.ViewModel.SaveAsDelegate = SegmentSaveAs;
      aSegmentEdit.ViewModel.SaveMinimumToolDelegate = SegmentSaveMinimum;
      aSegmentEdit.ViewModel.SaveMinimumToolAsDelegate = SegmentSaveMinimumAs;
      aSegmentEdit.ViewModel.ResetDelegate = SegmentReset;
      aSegmentEdit.ViewModel.RunDelegate = SegmentRun;

    } // ctor


    private void OnMainFormLoad(object sender, EventArgs e)
    {
      String sVProRoot = Environment.GetEnvironmentVariable("VPRO_ROOT");
      if (String.IsNullOrEmpty(sVProRoot))
      {
        MessageBox.Show("VPRO_ROOT environment variable not defined!");
        return;
      }

      LoadInitialImageFile(sVProRoot);

      LoadInitialCogSegmentTool(sVProRoot);

      ConfigureInitialCogBlobTool();

    } // OnMainFormLoad(...)


    private void LoadInitialImageFile(String vproRoot)
    {
      String sImagePath = Path.Combine(vproRoot, "Images\\SegmentApp.cdb");

      if (!File.Exists(sImagePath))
      {
        MessageBox.Show("Image file " + sImagePath + " not found.");
        return;
      }

      try
      {
        mImageFileTool.Operator.Open(sImagePath, CogImageFileModeConstants.Read);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }

    } // LoadInitialImageFile(...)


    private void LoadInitialCogSegmentTool(String vproRoot)
    {
      String sSegmentToolPath = Path.Combine(vproRoot,
        "Samples\\Programming\\ViDiEL\\SegmentApp\\CogSegmentTool.vpp");

      if (!File.Exists(sSegmentToolPath))
      {
        MessageBox.Show("CogSegmentTool file " + sSegmentToolPath + " not found.");
        return;
      }

      try
      {
        var aT = CogSerializer.LoadObjectFromFile(
          sSegmentToolPath) as CogSegmentTool;
        mSegmentTool = aT;
        SegmentEdit aSegmentEdit = mElementHost.Child as SegmentEdit;
        aSegmentEdit.Subject = mSegmentTool.VisionInteropTool;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }

    } // LoadInitialCogSegmentTool(...)


    private void ConfigureInitialCogBlobTool()
    {
      // Treat light colored regions of heatmap as foreground ...
      mBlobTool.RunParams.SegmentationParams.Polarity =
        CogBlobSegmentationPolarityConstants.LightBlobs;

      // To process heatmap, a simple fixed threshold will suffice ...
      mBlobTool.RunParams.SegmentationParams.Mode =
        CogBlobSegmentationModeConstants.HardFixedThreshold;
      mBlobTool.RunParams.SegmentationParams.HardFixedThreshold = 128;

      // Compute areas of connected regions and filter out regions
      // that are too small ...;
      CogBlobMeasure aMeasure = new CogBlobMeasure();
      aMeasure.Measure = CogBlobMeasureConstants.Area;
      aMeasure.Mode = CogBlobMeasureModeConstants.Filter;
      aMeasure.FilterMode = CogBlobFilterModeConstants.ExcludeBlobsInRange;
      aMeasure.FilterRangeLow = 0.0;
      aMeasure.FilterRangeHigh = 5000.0; // for example
      mBlobTool.RunParams.RunTimeMeasures.Add(aMeasure);

    } // ConfigureInitialCogBlobTool()


    private void OnRunBtnClick(object sender, EventArgs e)
    {
      // Initialize result label ...
      mSegmentedRegionsLbl.Text = "Segmented Regions: 0";

      // Run CogImageFileTool ...
      mImageFileTool.Run();
      if (mImageFileTool.RunStatus.Result != CogToolResultConstants.Accept)
      {
        MessageBox.Show(mImageFileTool.RunStatus.Message);
        return;
      }

      // Consume image file tool output and run CogSegmentTool ...
      mSegmentTool.InputImage = mImageFileTool.OutputImage as ICogVisionData;
      mSegmentTool.Run();
      if (mSegmentTool.RunStatus.Result != CogToolResultConstants.Accept)
      {
        MessageBox.Show(mSegmentTool.RunStatus.Message);
        return;
      }

      // Verify that CogSegmentTool produced a result ...
      int nSegRes = mSegmentTool.Results.Count;
      if (nSegRes < 1)
      {
        MessageBox.Show("CogSegmentTool produced no results.");
        return;
      }

      // Extract heatmap and class name from CogSegmentTool results ...
      String sName = mSegmentTool.Results[0].Class;
      CogImage8Grey aHeatMap = mSegmentTool.Results[0].Heatmap;

      // Consume heatmap and run CogBlobTool for post processing ...
      mBlobTool.InputImage = aHeatMap;
      mBlobTool.Run();

      // Extract count of segmented regions from CogBlobTool ...
      CogBlobResultCollection aBlobs = mBlobTool.Results.GetBlobs();
      int nBlobResults = aBlobs.Count;
      mSegmentedRegionsLbl.Text = sName + " Regions: " + nBlobResults.ToString();

      // Extract CogBlobTool last run record for main tab display ...
      cogRecordsDisplay1.Subject = mBlobTool.CreateLastRunRecord();

    } // OnRunBtnClick(...)


    private void OnBlobEditCtrlSubjectChanged(object sender, EventArgs e)
    {
      // Because the edit control's "Reset" button will replace the
      // control subject with a new instance of CogBlobTool, we
      // must update our cached reference to the blob edit control
      // subject.

      mBlobTool = mBlobEditCtrl.Subject;

    } // OnBlobEditCtrlSubjectChanged(...)


    private void OnImageFileCtrlSubjectChanged(object sender, EventArgs e)
    {
      // Because the edit control's "Reset" button will replace the
      // control subject with a new instance of CogImageFileTool, we
      // must update our cached reference to the image file edit control
      // subject.

      mImageFileTool = mImageFileCtrl.Subject;

    } // OnImageFileCtrlSubjectChanged(...)


    private String PromptForNameOfVppToLoad()
    {
      // Prompt the user for the name of a file holding a persisted
      // CogSegmentTool. Return an empty string if they cancel.

      var aFileDialog = new OpenFileDialog();
      aFileDialog.Filter = "Files (.vpp)|*.vpp";
      aFileDialog.CheckFileExists = true;
      aFileDialog.Title = "Load CogSegmentTool";
      String sFileName = "";
      if (aFileDialog.ShowDialog() == DialogResult.OK)
        sFileName = aFileDialog.FileName;
      return sFileName;

    } // PromptForNameOfVppToLoad(...)


    private String PromptForNameOfVppToSave()
    {
      // Prompt the user for the name of a file to which we will save our
      // CogSegmentTool. Return an empty string if they cancel.

      var aFileDialog = new SaveFileDialog();
      aFileDialog.Filter = "Files (.vpp)|*.vpp";
      aFileDialog.Title = "Save CogSegmentTool";
      String sFileName = "";
      if (aFileDialog.ShowDialog() == DialogResult.OK)
        sFileName = aFileDialog.FileName;
      return sFileName;

    } // PromptForNameOfVppToSave()


    // NOTE: Here are delegates that override the behavior
    // of the SegmentEdit control. These cause the
    // control to interact with the CogSegmentTool
    // rather than directly act upon the contained
    // VI SegmentTool. That means, for example, that the
    // control's various Open and Save buttons will
    // correctly open or save VisionPro objects rather
    // than VI objects.

    private void SegmentOpen()
    {
      String sFileName = PromptForNameOfVppToLoad();
      if (sFileName == "") return;

      try
      {
        var aT =
          CogSerializer.LoadObjectFromFile(sFileName) as CogSegmentTool;

        mSegmentTool = aT;

        SegmentEdit aSegmentEdit = mElementHost.Child as SegmentEdit;

        aSegmentEdit.Subject = mSegmentTool.VisionInteropTool;
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message);
      }

    } // SegmentOpen()


    private void SegmentSave()
    {
      if (mSegmentTool == null) return;

      const String sFileName = "CogSegmentTool.vpp";
      try
      {
        CogSerializer.SaveObjectToFile(mSegmentTool, sFileName);
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message);
      }      

    } // SegmentSave()


    private void SegmentSaveAs()
    {
      if (mSegmentTool == null) return;

      String sFileName = PromptForNameOfVppToSave();
      if (sFileName == "") return;
      
      try
      {
        CogSerializer.SaveObjectToFile(mSegmentTool, sFileName);
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message);
      }      

    } // SegmentSaveAs()


    private void SegmentSaveMinimum()
    {
      if (mSegmentTool == null) return;

      const String sFileName = "CogSegmentTool.vpp";
      try
      {
        CogSerializer.SaveObjectToFile(mSegmentTool, sFileName,
          typeof(BinaryFormatter), CogSerializationOptionsConstants.Minimum);
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message);
      }      

    } // SegmentSaveMinimum()


    private void SegmentSaveMinimumAs()
    {
      if (mSegmentTool == null) return;

      String sFileName = PromptForNameOfVppToSave();
      if (sFileName == "") return;
      
      try
      {
        CogSerializer.SaveObjectToFile(mSegmentTool, sFileName,
          typeof(BinaryFormatter), CogSerializationOptionsConstants.Minimum);
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message);
      }      

    } // SegmentSaveMinimumAs()


    private void SegmentReset()
    {
      mSegmentTool = new CogSegmentTool();

      SegmentEdit aSegmentEdit = mElementHost.Child as SegmentEdit;

      aSegmentEdit.Subject = mSegmentTool.VisionInteropTool;

    } // SegmentReset()


    private void SegmentRun()
    {
      if (mSegmentTool != null)
      {
        mSegmentTool.Run(); // Never throws
      }
    } // SegmentRun()


  }  // public partial class SegmentApp : Form


} // namespace SegmentApp
