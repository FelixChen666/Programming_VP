/*******************************************************************************
 Copyright (C) 2010 Cognex Corporation

 Subject to Cognex Corporations terms and conditions and license agreement,
 you are authorized to use and modify this source code in any way you find
 useful, provided the Software and/or the modified Software is used solely in
 conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
 and agree that Cognex has no warranty, obligations or liability for your use
 of the Software.
*******************************************************************************
 This sample program is designed to illustrate certain VisionPro features or 
 techniques in the simplest way possible. It is not intended as the framework 
 for a complete application. In particular, the sample program may not provide
 proper error handling, event handling, cleanup, repeatability, and other 
 mechanisms that a commercial quality application requires.

 This program assumes that you have good knowledge of C# and VisionPro
 programming.

 This program demonstrates how to interact with some of the CogToolBlock APIs

 1) The sample loads a ToolBlock from a vpp file 
 2) The user can modify the value of the Toolblock input terminals through the 
    numeric up down controls on the application form.
 3) The user can also select an image from coins.idb or from an acquisition fifo.
 4) The run once button does the following
    - Acquire the next image or read the next image
    - Pass the image to the ToolBlock input image 
    - Run the toolblock once
 5) The sample also demonstrate how to read the output terminal value to updated 
    the application labels with the inspection result
 6) The user can change the code to create an acquisition fifo that would work 
    specifically with the camera available
 7) The top level script is a C# simple script. It runs the tools. 
 8) The TBInspectionTest ToolBlock is used as result analysis tool to decide 
    if the inspection passed or failed and sets the value of the output terminal
 9) The sample will allow the user to run the toolblock from the menu buttons but 
    the toolblock will run against the same image.
 10)The sample also take advantage of the ran event so update the display with 
    the results from blob tool.

 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.Blob;

namespace ToolBlockLoad
{
  public partial class Form1 : Form
  {
    CogImageFileTool mIFTool;
    CogAcqFifoTool mAcqTool;
    long numPass = 0;
    long numFail = 0;
    // Open the image file
    // Create the acq fifo tool
    // Set the exposure
    // Load the TB.vpp file
    // Hook the event for ran() and SubjectChanged
    // Since the ToolBlockEditV2 allows you to reset or load a different vpp file
    // We need to block the Run Once button
    public Form1()
    {
      InitializeComponent();
      cogToolBlockEditV21.LocalDisplayVisible = false;
      mIFTool = new CogImageFileTool();
      mIFTool.Operator.Open(Environment.GetEnvironmentVariable("VPRO_ROOT") + @"\images\coins.idb", CogImageFileModeConstants.Read);
      mAcqTool = new CogAcqFifoTool();
      // If no camera is attached, disable the radio button
      if (mAcqTool.Operator == null)
      {
        radCamera.Enabled = false;
      }
      else
      {
        mAcqTool.Operator.OwnedExposureParams.Exposure = 10;
      }
      cogToolBlockEditV21.Subject = CogSerializer.LoadObjectFromFile(Environment.GetEnvironmentVariable("VPRO_ROOT") + @"\samples\programming\toolblock\toolblockload\tb.vpp") as CogToolBlock;
      cogToolBlockEditV21.Subject.Ran += new EventHandler(Subject_Ran);
      cogToolBlockEditV21.Subject.Inputs["FilterLowValue"].Value = nAreaLow.Value;
      cogToolBlockEditV21.Subject.Inputs["FilterHighValue"].Value = nAreaHigh.Value;
      cogToolBlockEditV21.SubjectChanged += new EventHandler(cogToolBlockEditV21_SubjectChanged);
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      // Disconnect the event handlers before closing the form
      cogToolBlockEditV21.Subject.Ran -= new EventHandler(Subject_Ran);
      cogToolBlockEditV21.SubjectChanged -= new EventHandler(cogToolBlockEditV21_SubjectChanged);
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    void cogToolBlockEditV21_SubjectChanged(object sender, EventArgs e)
    {
      // The application is meant to be used with the TB.vpp so whenever the user changes the TB
      // We disable the run once button
      btnRun.Enabled = false;
    }

    void Subject_Ran(object sender, EventArgs e)
    {
      // This method executes each time the TB runs
      if ((bool)(cogToolBlockEditV21.Subject.Outputs["InspectionPassed"].Value) == true)
        numPass++;
      else
        numFail++;
      // Update the label with pass and fail
      nPass.Text = numPass.ToString();
      nFail.Text = numFail.ToString();
      // Update the CogDisplayRecord with the lastRunRecord
      
      CogBlobTool mBlobTool = cogToolBlockEditV21.Subject.Tools["CogBlobTool1"] as CogBlobTool;
      cogRecordDisplay1.Record = mBlobTool.CreateLastRunRecord();
      
      // Update the CogDisplayRecord with the image 
      cogRecordDisplay1.Image = cogToolBlockEditV21.Subject.Inputs["Image"].Value as CogImage8Grey;
      cogRecordDisplay1.Fit(true);

    }

    private void btnRun_Click(object sender, EventArgs e)
    {
      // Get the next image
      if (radImageFile.Checked == true)
      {
        mIFTool.Run();
        cogToolBlockEditV21.Subject.Inputs["Image"].Value = mIFTool.OutputImage as CogImage8Grey;
      }
      else
      {
        mAcqTool.Run();
        cogToolBlockEditV21.Subject.Inputs["Image"].Value = mAcqTool.OutputImage as CogImage8Grey;
      }
      // Run the toolblock
      cogToolBlockEditV21.Subject.Run();
    }

    private void nAreaLow_ValueChanged(object sender, EventArgs e)
    {
      // Update the input terminal value whenever the user changes the value through the GUI
      cogToolBlockEditV21.Subject.Inputs["FilterLowValue"].Value = nAreaLow.Value;
    }

    private void nAreaHigh_ValueChanged(object sender, EventArgs e)
    {
      // Update the input terminal value whenever the user changes the value through the GUI
      cogToolBlockEditV21.Subject.Inputs["FilterHighValue"].Value = nAreaHigh.Value;
    }
  }
}
