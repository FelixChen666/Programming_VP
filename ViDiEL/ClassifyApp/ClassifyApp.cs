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
// CogClassifyTool. Specifically, this application demos the following:
// - Configuring ClassifyTool with ClassifyToolEdit and save the configuration
//     to a .vpp file.
// - Provide input images
//   - Acquire image with a camera, or
//   - Use an image file.
// - Load saved .vpp file and run the classify tool.
// - The last run records for the classify tool are shown in the
//     CogRecordsDisplay.
//
// You can click "Run Once" to cycle through the images in the image database
// or images acquired with a camera.
//
//

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ViDiEL;

using Cognex.VisionProUI.ViDiEL.Classify.Controls;


namespace ClassifyApp
{
  public partial class ClassifyApp : Form
  {
    private CogImageFileTool mImageFileTool;
    private LoadImageForm mImageFileForm;

    private CogAcqFifoTool mAcqFifoTool;
    private AcqFifoForm mAcqFifoForm;

    private CogClassifyTool mClassifyTool;
    private ClassifyEditForm mClassifyEditForm;

    private CogClassifyTool mLoadedClassifyTool;

    private bool mUseImageFile;


    public ClassifyApp()
    {
      InitializeComponent();

      mImageFileTool = new CogImageFileTool();
      mImageFileTool.Ran += ImageFileTool_Ran;
      mImageFileForm = null;

      mAcqFifoTool = new CogAcqFifoTool();
      mAcqFifoTool.Ran += AcqFifoTool_Ran;
      mAcqFifoForm = null;

      mClassifyTool = new CogClassifyTool();
      mClassifyEditForm = null;

      mLoadedClassifyTool = null;

      mUseImageFile = false;

    } // ClassifyApp ctor


    private void StartClassifyEditBtn_Click(object sender, System.EventArgs e)
    {
      if (mClassifyEditForm != null)
        return;

      if (mUseImageFile)
      {
        mClassifyTool.InputImage = mImageFileTool.OutputImage as ICogVisionData;
      }
      else // use acquired image
      {
        mClassifyTool.InputImage = mAcqFifoTool.OutputImage as ICogVisionData;
      }

      mClassifyEditForm = new ClassifyEditForm();

      ClassifyEdit aClassifyEdit = new ClassifyEdit();

      // NOTE: ClassifyEdit is a WPF control, so we will reference it in
      // our classify edit form's ElementHost.
      mClassifyEditForm.mElementHost.Child = aClassifyEdit;

      // NOTE: the subject of the ClassifyEdit is the VI classify
      // tool available through CogClassifyTool's VisionInteropTool
      // property.
      aClassifyEdit.Subject = mClassifyTool.VisionInteropTool;

      // NOTE: add delegates to get the ClassifyEdit to behave
      // appropriately for our VPro 9x application.
      aClassifyEdit.ViewModel.OpenDelegate = ClassifyOpen;
      aClassifyEdit.ViewModel.SaveDelegate = ClassifySave;
      aClassifyEdit.ViewModel.SaveAsDelegate = ClassifySaveAs;
      aClassifyEdit.ViewModel.SaveMinimumToolDelegate = ClassifySaveMinimum;
      aClassifyEdit.ViewModel.SaveMinimumToolAsDelegate = ClassifySaveMinimumAs;
      aClassifyEdit.ViewModel.ResetDelegate = ClassifyReset;
      aClassifyEdit.ViewModel.RunDelegate = ClassifyRun;

      mClassifyEditForm.FormClosing += OnClassifyEditFormClosing;

      mClassifyEditForm.Show();

    } // StartClassifyEditBtn_Click(...)


    private void AcquireImageBtn_Click(object sender, System.EventArgs e)
    {
      if (mAcqFifoForm != null)
        return;

      mUseImageFile = false;

      mAcqFifoForm = new AcqFifoForm();
      mAcqFifoForm.mAcqFifoEdit.Subject = mAcqFifoTool;
      mAcqFifoForm.FormClosing += OnAcqFifoFormClosing;
      mAcqFifoForm.Show();

    } // AcquireImageBtn_Click(...)


    private void ImageFileBtn_Click(object sender, System.EventArgs e)
    {
      if (mImageFileForm != null)
        return;

      mUseImageFile = true;

      mImageFileForm = new LoadImageForm();
      mImageFileForm.mImageFileEdit.Subject = mImageFileTool;
      mImageFileForm.FormClosing += OnImageFileFormClosing;
      mImageFileForm.Show();

    } // ImageFileBtn_Click(...)


    private void RunConfiguredToolBtn_Click(object sender, System.EventArgs e)
    {
      // Clean up the status
      statusStrip1.Items[1].Text = "";
      bool bKeepExisting = false;

      String loadVppFileName = PromptForNameOfVppToLoad();

      // If a vpp file is not selected
      if (loadVppFileName == "")
      {
        // Classify tool was loaded previously
        if (mLoadedClassifyTool != null)
        {
          const String sPrompt = "Configuration file is not selected. " +
            "Do you want to keep existing one?";
          const String sCaption = "Load Configuration File";
          if (DialogResult.No == MessageBox.Show(sPrompt, sCaption,
          MessageBoxButtons.YesNo))
          {
            mLoadedClassifyTool = null;
            mRecordsDisplay.Subject = null;
          }
          else
          {
            bKeepExisting = true;
          }
        }

        if (bKeepExisting)
        {
          statusStrip1.Items[1].Text = "Using previously loaded " +
            "configuration file.";
        }
        else
        {
          statusStrip1.Items[1].Text = "Configuration file is not loaded.";
        }
        return;
      }

      // If a Vpp file is selected
      mLoadedClassifyTool =
        CogSerializer.LoadObjectFromFile(loadVppFileName) as CogClassifyTool;
      if (mUseImageFile)
      {
        mLoadedClassifyTool.InputImage =
          mImageFileTool.OutputImage as ICogVisionData;
      }
      else // do not use image file
      {
        mLoadedClassifyTool.InputImage =
          mAcqFifoTool.OutputImage as ICogVisionData;
      }

      RunClassifyTool("Configuration file loaded");

    } // RunConfiguredToolBtn_Click(...)


    private void RunOnceBtn_Click(object sender, System.EventArgs e)
    {
      // Tool already configured
      if (mLoadedClassifyTool != null)
      {
        if (mUseImageFile)
        {
          mImageFileTool.Run();

          // Update image display with new image
          mDisplay.Image = mImageFileTool.OutputImage;
          mLoadedClassifyTool.InputImage =
            mImageFileTool.OutputImage as ICogVisionData;
        }
        else // mUseImageFile == false
        {
          mAcqFifoTool.Run();

          // Update image display with new image
          mDisplay.Image = mAcqFifoTool.OutputImage;
          mLoadedClassifyTool.InputImage =
            mAcqFifoTool.OutputImage as ICogVisionData;
        }
        mDisplay.AutoFitWithGraphics = true;

        RunClassifyTool("");
      }
      else
      {
        statusStrip1.Items[1].Text = "Tool is not configured.";
      }

    } // RunOnceBtn_Click(...)


    private void AcqFifoTool_Ran(object sender, System.EventArgs e)
    {
      if (mUseImageFile) return;

      mDisplay.Image = mAcqFifoTool.OutputImage;
      mDisplay.AutoFitWithGraphics = true;

      if (mClassifyTool != null)
      {
        mClassifyTool.InputImage = mAcqFifoTool.OutputVisionData;
      }

    } // AcqFifoTool_Ran(...)


    private void ImageFileTool_Ran(object sender, System.EventArgs e)
    {
      if (!mUseImageFile) return;

      mDisplay.Image = mImageFileTool.OutputImage;
      mDisplay.AutoFitWithGraphics = true;

      if (mClassifyTool != null)
      {
        mClassifyTool.InputImage =
          mImageFileTool.OutputImage as ICogVisionData;
      }

    } // ImageFileTool_Ran(...)


    private void RunClassifyTool(String message)
    {
      try
      {
        mLoadedClassifyTool.Run(); // does not throw

        // NOTE: the GeneratePredictedResults() method is only available
        // via the VisionInteropTool property. And note that it may throw.
        mLoadedClassifyTool.VisionInteropTool.GeneratePredictedResults();

        // NOTE: CreateLastRunRecord produces a CogRecord that is
        // equivalent to the underlying VI tool's last run record.
        mRecordsDisplay.Subject = mLoadedClassifyTool.CreateLastRunRecord();

        if (message != "")
        {
          statusStrip1.Items[1].Text = message;
        }
        else
        {
          statusStrip1.Items[1].Text = mLoadedClassifyTool.RunStatus.Message;
        }

      }
      catch(Exception ex)
      {
        statusStrip1.Items[1].Text = "Run Classify Tool: " + ex.Message;
      }

    } // RunClassifyTool(...)


    private String PromptForNameOfVppToLoad()
    {
      var aFileDialog = new OpenFileDialog();
      aFileDialog.Filter = "Files (.vpp)|*.vpp";
      aFileDialog.CheckFileExists = true;
      aFileDialog.Title = "Load CogClassifyTool";
      String sLoadVppFile = "";
      if (aFileDialog.ShowDialog() == DialogResult.OK)
        sLoadVppFile = aFileDialog.FileName;
      return sLoadVppFile;

    } // PromptForNameOfVppToLoad(...)


    private String PromptForNameOfVppToSave()
    {
      var aFileDialog = new SaveFileDialog();
      aFileDialog.Filter = "Files (.vpp)|*.vpp";
      aFileDialog.Title = "Save CogClassifyTool";
      String sSaveVppFile = "";
      if (aFileDialog.ShowDialog() == DialogResult.OK)
        sSaveVppFile = aFileDialog.FileName;
      return sSaveVppFile;
    }


    private void OnAcqFifoFormClosing(object sender, FormClosingEventArgs e)
    {
      if (mAcqFifoForm != null)
      {
        mAcqFifoForm.mAcqFifoEdit.Subject = null;
      }

      mAcqFifoForm = null;

    } // OnAcqFifoFormClosing(...)


    private void OnImageFileFormClosing(object sender, FormClosingEventArgs e)
    {
      if (mImageFileForm != null)
      {
        mImageFileForm.mImageFileEdit.Subject = null;
      }

      mImageFileForm = null;

    } // OnImageFileFormClosing(...)


    private void OnClassifyEditFormClosing(object sender,
      FormClosingEventArgs e)
    {
      if (mClassifyEditForm != null)
      {
        mClassifyEditForm.mElementHost.Child = null;
      }

      mClassifyEditForm = null;

    } // OnClassifyEditFormClosing(...)


    private void OnMainFormLoad(object sender, System.EventArgs e)
    {
      // These display settings must be set at run time:
      mRecordsDisplay.Display.HorizontalScrollBar = false;
      mRecordsDisplay.Display.VerticalScrollBar = false;

    } // OnMainFormLoad(...)


    private void OnMainFormClosing(object sender, FormClosingEventArgs e)
    {
      // Unsubscribe from Ran events on acq fifo tool and image file tool ...
      mAcqFifoTool.Ran -= AcqFifoTool_Ran;
      mImageFileTool.Ran -= ImageFileTool_Ran;

      mDisplay.Image = null;
      mRecordsDisplay.Display.Image = null;

    } //OnMainFormClosing(...)


    // NOTE: Here are delegates that override the behavior
    // of the ClassifyEdit control. These cause the
    // control to interact with the CogClassifyTool
    // rather than directly act upon the contained
    // VI ClassifyTool. That means, for example, that the
    // control's various Open and Save buttons will
    // correctly open or save VisionPro objects rather
    // than VI objects.

    private void ClassifyOpen()
    {
      String sFileName = PromptForNameOfVppToLoad();
      if (sFileName == "") return;

      try
      {
        var aT =
          CogSerializer.LoadObjectFromFile(sFileName) as CogClassifyTool;

        mClassifyTool = aT;

        if (mClassifyEditForm != null)
        {
          ClassifyEdit aClassifyEdit =
            mClassifyEditForm.mElementHost.Child as ClassifyEdit;

          if (aClassifyEdit != null)
          {
            aClassifyEdit.Subject = mClassifyTool.VisionInteropTool;
          }
        }
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message);
      }

    } // ClassifyOpen()


    private void ClassifySave()
    {
      if (mClassifyTool == null) return;

      const String sFileName = "CogClassifyTool.vpp";
      try
      {
        CogSerializer.SaveObjectToFile(mClassifyTool, sFileName);
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message);
      }      

    } // ClassifySave()


    private void ClassifySaveAs()
    {
      if (mClassifyTool == null) return;

      String sFileName = PromptForNameOfVppToSave();
      if (sFileName == "") return;
      
      try
      {
        CogSerializer.SaveObjectToFile(mClassifyTool, sFileName);
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message);
      }      

    } // ClassifySaveAs()


    private void ClassifySaveMinimum()
    {
      if (mClassifyTool == null) return;

      const String sFileName = "CogClassifyTool.vpp";
      try
      {
        CogSerializer.SaveObjectToFile(mClassifyTool, sFileName,
          typeof(BinaryFormatter), CogSerializationOptionsConstants.Minimum);
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message);
      }      

    } // ClassifySaveMinimum()


    private void ClassifySaveMinimumAs()
    {
      if (mClassifyTool == null) return;

      String sFileName = PromptForNameOfVppToSave();
      if (sFileName == "") return;
      
      try
      {
        CogSerializer.SaveObjectToFile(mClassifyTool, sFileName,
          typeof(BinaryFormatter), CogSerializationOptionsConstants.Minimum);
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message);
      }      

    } // ClassifySaveMinimumAs()


    private void ClassifyReset()
    {
      mClassifyTool = new CogClassifyTool();

      if (mClassifyEditForm == null) return;

      ClassifyEdit aClassifyEdit =
        mClassifyEditForm.mElementHost.Child as ClassifyEdit;

      if (aClassifyEdit == null) return;

      aClassifyEdit.Subject = mClassifyTool.VisionInteropTool;

    } // ClassifyReset()


    private void ClassifyRun()
    {
      if (mClassifyTool != null)
      {
        mClassifyTool.Run(); // Never throws
      }
    } // ClassifyRun()


  } // public partial class ClassifyApp : Form


} // namespace ClassifyApp
