//*****************************************************************************
// Copyright (C) 2014 Cognex Corporation
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
// Cross Section tool.
//
// The following sample illustrates calling the Cross Section tool multiple 
// times and stepping through a defined scan region one slice at a time.
//
// This sample is designed to work with the scan region size and location 
// used and the specific operators provided. 
//
// The sample:
//   1) Aligns the part within the image,
//   2) Divides the scan region into thin slices using a user defined step 
//      (default= 1 pixel)
//   3) Runs the cross section tool to find a divot in the brake pad surface 
//      that is outside a specified tolerance of .4 mm on each slice.
//
// The sample code uses:
//   CogPixelMapTool to convert the CogImage16Range to CogImage8Grey
//   CogPMAlignTool  to locate the left edge of the brake pad surface
//   CogFixtureTool  to fixture the range image using the pose returned from 
//                   PMAlign tool
//   Cog3DRangeImageCrossSectionTool to create the profile of each slice, 
//                                   find the divot and check that it is within
//                                   allowed tolerance.
//   
// The scan region is specified with respect the pose found by the PMAlign tool.
// The scan region is divided into a number of thin slices. 
// The height of the thin slice is specified by stepSizeInPixels available 
// as a numeric field through the GUI.
//
// The GUI contains the following objects:
//   1) 3 CogRecordDisplay(s):
//       - “Defects In scan region” record display shows:
//            The image
//            The scan region
//            Point markers where the divot in the surface was greater than 0.4 mm
//       - “Processing region” record display shows:
//            The image
//            The section of the region that is processed
//       - “Profile graphic” record display shows:
//            The combined graphics record extracted from the last run record.
//
//   2) “Freeze on defect” checkbox: (default is checked)
//      If checked, the processing will stop when the divot is outside the 
//      allowable tolerance. Otherwise, the processing will continue until 
//      the scan region has been fully processed. 
//
//   3) “Step size in pixels” numeric box:
//      The value is used to slice to the region into thin sections. Each 
//      section is a small rectangle affine where the width is the same as the 
//      scan region width and the height is equal to the step size specified. 
//
//   4) “Next Image” Button:
//      Reads the next image from the image file (BrakePad.cdb) and runs the 
//      tools on the new image.
//
//   5) “Show Control” Button:
//      Launches a different form containing the Cog3DRangeImageCrossSectionEditV2 
//      and attaches the Cog3DRangeImageCrossSectionTool to the control subject 
//      for viewing
//
//  6) “Start Processing” Button:
//      Slices the region
//      Runs the Cross Section Tool on each slice
//      Breaks if “freeze on defect” is checked and a divot is outside tolerance
//      Adds defects to the record display
// 
///////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro3D;
using System.Threading;
using Cognex.VisionPro.Implementation;
using Cognex.VisionPro.Internal;
using Cognex.VisionPro.PixelMap;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.CalibFix;

namespace CrossSection
{
  public partial class Form1 : Form
  {
    // VisionPro tools
    private CogImageFileTool imageFileTool = null;
    private Cog3DRangeImageCrossSectionTool csTool = null;
    private CogPixelMapTool pixelMapTool = null;
    private CogPMAlignTool pmTool = null;
    private CogFixtureTool fixtureTool = null;

    // Range image (brake pad)
    private ICogImage rangeImage = null;

    // Range image region to scan: the code will go create a small region 
    // that uses the stepSizeInPixels as the height and run the Cross section tool
    private CogRectangleAffine scanRegionInPixels = null;
    private CogRectangleAffine scanRegionInSSN = null;

    // The projection height of the small region in pixels
    private Int32 stepSizeInPixels = 0;
    private double stepSizeInSSN = 0;

    private Int32 stepIndex = 0;
    private Int32 numberOfSteps = 0;
    
    // The collection of point markers representing the defects. 
    // The defect is when the minimum height value is greater than 0.4 mm
    private CogGraphicCollection defectGraphicCollection = null;

    // The records associated with the displays
    private ICogRecord recordDefects = null;
    private ICogRecord recordProcessingRegion = null;

    // The form to show the Cross Section Edit
    ControlForm crossSectionEditForm = null;
    
    public Form1()
    {
      InitializeComponent();

      // Initialize the defects graphic collection
      defectGraphicCollection = new CogGraphicCollection();

      /////////////////////////////////////
      // Image File Tool
      /////////////////////////////////////
        // Create the ImageFileTool and open the BrakePad.cdb file
        String imagePath = Environment.GetEnvironmentVariable("VPRO_ROOT");
        imagePath = imagePath + @"\Images\BrakePad.cdb";
        imageFileTool = new CogImageFileTool();
        imageFileTool.Operator.Open(imagePath, 
                                    CogImageFileModeConstants.Read);

        // Run Image File tool 
        imageFileTool.Run();
        rangeImage = imageFileTool.OutputImage;
        if (null == rangeImage)
        {
          MessageBox.Show("Unable to open the image file. Stopping.");
          return;
        }

      ////////////////////////////////////
      // Pixel Map Tool
      ////////////////////////////////////
        // Run Pixel Map tool to convert the range image to CogImage8Grey
        pixelMapTool = new CogPixelMapTool();
        pixelMapTool.InputImage = rangeImage;
        pixelMapTool.Run();


      /////////////////////////////////////
      // PMAlign Tool
      /////////////////////////////////////
        // This sample uses PMAlign tool to train a pattern to locate the edge of
        // the brake pad. The edge is later on used to specify the scan region
        //
        // Create the PMAlign tool
        pmTool = new CogPMAlignTool();
        pmTool.InputImage = pixelMapTool.OutputImage as CogImage8Grey;

        // Attach the first image to train the pattern
        pmTool.Pattern.TrainImage = pixelMapTool.OutputImage as CogImage8Grey;

        // Specify train region and pattern origin
        CogRectangleAffine pmTrainRegion = new CogRectangleAffine();
        pmTrainRegion.SetOriginLengthsRotationSkew(-43, -85, 5, 25, 0, 0);
        pmTool.Pattern.TrainRegion = pmTrainRegion;
        pmTool.Pattern.Origin.Rotation = 0;
        pmTool.Pattern.Origin.Skew = 0;
        pmTool.Pattern.Origin.TranslationX = -41;
        pmTool.Pattern.Origin.TranslationY = -63;

        // Adjust PMAlign training params to find the edge of the brake pad without
        // including shadow areas of missing pixels.
        pmTool.Pattern.GrainLimitAutoSelect = false;
        pmTool.Pattern.GrainLimitCoarse = 5;
        pmTool.Pattern.GrainLimitFine = 5;
        pmTool.Pattern.AutoEdgeThresholdEnabled = false;
        pmTool.Pattern.EdgeThreshold = 5;

        // Train PMAlign
        pmTool.Pattern.Train();

        // Enable angle DOF to find the pattern when the brake pad is
        // rotated and run the PMAlign tool
        pmTool.InputImage = pixelMapTool.OutputImage as CogImage8Grey;
        pmTool.RunParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;
        pmTool.RunParams.ZoneAngle.Low = -Math.PI / 4;
        pmTool.RunParams.ZoneAngle.High = Math.PI / 4;
        pmTool.Run();

      ///////////////////////////
      // Fixture Tool
      ///////////////////////////
        // Run Fixture tool using the pose found by PMAlign tool as fixture space
        fixtureTool = new CogFixtureTool();
        fixtureTool.InputImage = rangeImage;
        fixtureTool.RunParams.FixturedSpaceName = "FixtureBrakePad";
        fixtureTool.RunParams.UnfixturedFromFixturedTransform = pmTool.Results[0].GetPose();
        fixtureTool.Run();

      ////////////////////////////
      // Cross Section Tool
      ///////////////////////////
        // Create a cross section tool
        csTool = new Cog3DRangeImageCrossSectionTool();
        csTool.InputImage = fixtureTool.OutputImage;

        // Configure the region to scan
        scanRegionInSSN = new CogRectangleAffine();
        scanRegionInSSN.SelectedSpaceName = "FixtureBrakePad";
        scanRegionInSSN.SetOriginLengthsRotationSkew(1, -21, 76, 21, 0, 0);
        scanRegionInSSN.GraphicDOFEnable = CogRectangleAffineDOFConstants.Size | 
                                           CogRectangleAffineDOFConstants.Position | 
                                           CogRectangleAffineDOFConstants.Rotation;
        scanRegionInSSN.Interactive = true;

        // Copy it to the Cross Section tool.
        csTool.RunParams.ProfileParams.Region = new CogRectangleAffine(scanRegionInSSN);

      //////////////////////////////////////////
      // Convert Scan Region To Pixels
      //////////////////////////////////////////
        // Convert the scan region to pixel space
        scanRegionInPixels = new CogRectangleAffine();
        ICogTransform2D pixelFromSSNTransform = csTool.InputImage.GetTransform("@", "FixtureBrakePad");
        scanRegionInPixels = scanRegionInSSN.Map(pixelFromSSNTransform, 
                                                 CogCopyShapeConstants.GeometryOnly) as CogRectangleAffine;
        scanRegionInPixels.SelectedSpaceName = "@";

      ////////////////////////////////////////
      // Defect Record
      ///////////////////////////////////////
        // Add the range image and the scanRegionInSSN to the defects display
        recordDefects = new CogRecord("Image",
                                      typeof(CogImage8Grey),
                                      CogRecordUsageConstants.Input, 
                                      false, 
                                      rangeImage, 
                                      "Image");

        recordDefects.SubRecords.Add(new CogRecord("Region",
                                                   typeof(CogRectangleAffine),
                                                   CogRecordUsageConstants.Configuration,
                                                   true,
                                                   scanRegionInSSN,
                                                   "Region")
                                    );

        DefectDisplay.Record = recordDefects;
        DefectDisplay.Fit(true);

      ///////////////////////////////////////////////////////////////////////
      // Operators
      ////////////////////////////////////////////////////////////////////////
        // Add 3 operators to the Cross Section tool
        //  1) ExtractLineSegment: uses 2 regions to extract the brake pad surface line
        //  2) ExtractPoint: used the same 2 region to find the lowest height point on the brake pad
        //  3) DistancePointLine: computes the distance between the point and the line
        //                        enables tolerance checking on the distance (0 - 0.4mm)

        // Create ExtractLineSegment operator
        Cog3DRangeImageCrossSectionExtractLineSegment opExtractLS = new Cog3DRangeImageCrossSectionExtractLineSegment();
        // Add the ExtractLineSegment to the operators' collection
        csTool.RunParams.OperatorsParams.Add(opExtractLS);

        // Create the ExtractPoint operator
        Cog3DRangeImageCrossSectionExtractPoint opExtractMin = new Cog3DRangeImageCrossSectionExtractPoint();
        // Add the ExtractPoint to the operators' collection
        csTool.RunParams.OperatorsParams.Add(opExtractMin);

        // Create the DistancePointLine operator
        Cog3DRangeImageCrossSectionDistancePointLine opDistance = new Cog3DRangeImageCrossSectionDistancePointLine();
        // Add the DistancePointLine to the operators' collection
        csTool.RunParams.OperatorsParams.Add(opDistance);

      ////////////////////////////////////////////////////////////////
      // Configure the ExtractLineSegment
      ////////////////////////////////////////////////////////////////
        // Change the name of the ExtractLineSegment Operator
        opExtractLS.Name = "Brake Pad Surface Line";

        // Disable the line segment graphic from showing up in the combined graphics record
        opExtractLS.CombineGraphicsEnabled = false;

      ///////////////////////////////////////////////////////////////
      // Configure the ExtractPoint operator
      ////////////////////////////////////////////////////////////////
        // Extract lowest point
        opExtractMin.PointType = Cog3DRangeImageCrossSectionPointTypeConstants.Bottom;

        // Change the name of the ExtractPoint operator
        opExtractMin.Name = "Min Point";

        // Disable the point graphic from showing up in the combined graphics record
        opExtractMin.CombineGraphicsEnabled = false;

      ///////////////////////////////////////////////////////////////
      // Configure the DistancePointLine operator
      //////////////////////////////////////////////////////////////
        // Set the point and the line segment to compute the distance between
        opDistance.SetLineSegmentPointUsingOperators(csTool.RunParams.OperatorsParams, 
                                                     opExtractLS, 
                                                     opExtractMin);

        // Enable Distance tolerance
        opDistance.DistanceTolerance.Enabled = true;
        opDistance.DistanceTolerance.Max = 0.4; //mm
        opDistance.DistanceTolerance.Min = 0;

        // Enable the distance graphic in the combined graphics record.
        opDistance.CombineGraphicsEnabled = true;

      //////////////////////////////////////////////////////////////
      // Configure Regions
      //////////////////////////////////////////////////////////////
        // Define 2 regions to use in ExtractLineSegment and ExtractPoint
        CogRectangleAffine opRectLeft = new CogRectangleAffine();
        opRectLeft.SetOriginLengthsRotationSkew(12, 110, 20, 8, 0, 0);
        opRectLeft.GraphicDOFEnable = CogRectangleAffineDOFConstants.Size | 
                                      CogRectangleAffineDOFConstants.Position | 
                                      CogRectangleAffineDOFConstants.Rotation;
        opRectLeft.Interactive = true;

        CogRectangleAffine opRectRight = new CogRectangleAffine();
        opRectRight.SetOriginLengthsRotationSkew(43, 110, 20, 8, 0, 0);
        opRectRight.GraphicDOFEnable = CogRectangleAffineDOFConstants.Size | 
                                       CogRectangleAffineDOFConstants.Position | 
                                       CogRectangleAffineDOFConstants.Rotation;
        opRectRight.Interactive = true;

        // Modify the initial region
        opExtractLS.Regions[0] = opRectLeft;
        // Add the second region
        opExtractLS.Regions.Add(opRectRight);

        // Modify the initial region
        opExtractMin.Regions[0] = opRectLeft;
        // Add the second region
        opExtractMin.Regions.Add(opRectRight);

    }

    // Method used to run all the tools when a new image is passed in
    // It runs the following:
    //   Pixel Map tool: Convert the range image to 8 bit.
    //   PatMax tool: Locate the edge of the brake pad. 
    //   Fixture tool: Add a fixture space "FixtureBrakePad" using the pose from PMAlign.
    // And finally attaches the fixtured range image to the cross section tool
    private void RunAllTools()
    {
      // Run Pixel Map tool
      pixelMapTool.InputImage = rangeImage;
      pixelMapTool.Run();

      // Run PMAlign tool
      pmTool.InputImage = pixelMapTool.OutputImage as CogImage8Grey;
      pmTool.Run();

      // Run Fixture tool
      fixtureTool.InputImage = rangeImage;
      fixtureTool.RunParams.UnfixturedFromFixturedTransform = pmTool.Results[0].GetPose();
      fixtureTool.Run();

      // Attach the fixtured range image to the Cross Section tool
      csTool.InputImage = fixtureTool.OutputImage;

      // Update the defects record with the new image and the scan region
      recordDefects = new CogRecord("Image",
                                    typeof(CogImage8Grey),
                                    CogRecordUsageConstants.Input, 
                                    false, 
                                    rangeImage, 
                                    "Image");

      recordDefects.SubRecords.Add(new CogRecord("Region",
                                                 typeof(CogRectangleAffine),
                                                 CogRecordUsageConstants.Configuration,
                                                 true,
                                                 scanRegionInSSN,
                                                 "Region")
                                   );

      DefectDisplay.Record = recordDefects;
    }

    // Converts the scan region to pixel space
    private void updateScanRegionInPixels()
    {
      // Convert the cross section scan region to pixel space
      ICogTransform2D xFormPixelFromSSN = csTool.InputImage.GetTransform("@", "FixtureBrakePad");

      CogRectangleAffine rect = csTool.RunParams.ProfileParams.Region as CogRectangleAffine;
      scanRegionInPixels = new CogRectangleAffine();
      scanRegionInPixels = rect.Map(xFormPixelFromSSN, 
                                    CogCopyShapeConstants.GeometryOnly) as CogRectangleAffine;
      scanRegionInPixels.SelectedSpaceName = "@"; 
    }

    // Check if all slices within a scan region have been processed
    private bool isDoneProcessing()
    {
      if (stepIndex >= numberOfSteps)
          return true;
      return false;
    }

    // Update the Processing Region display with each slice
    private void updateProcessingDisplay()
    {
      recordProcessingRegion = null;
      recordProcessingRegion = new CogRecord("Image",
                                             typeof(CogImage8Grey),
                                             CogRecordUsageConstants.Input, 
                                             false, 
                                             rangeImage, 
                                             "Image");

      recordProcessingRegion.SubRecords.Add(new CogRecord("Region",
                                                          typeof(CogRectangleAffine),
                                                          CogRecordUsageConstants.Configuration,
                                                          true,
                                                          csTool.RunParams.ProfileParams.Region,
                                                          "Region")
                                           );

      RegionDisplay.Record = recordProcessingRegion;
      RegionDisplay.Fit(true);
    }
    // Add the defect graphic to the Defects display as a CogPointMarker
    private void addDefectGraphic()
    {
      Cog3DRangeImageCrossSectionExtractPoint opExtractPt = csTool.RunParams.OperatorsParams[1] as Cog3DRangeImageCrossSectionExtractPoint;
      CogPointMarker pt = opExtractPt.PointInImage;
      Cog3DRangeImageCrossSectionDistancePointLine opDistance = csTool.RunParams.OperatorsParams[2] as Cog3DRangeImageCrossSectionDistancePointLine;
      if (pt != null && opDistance.Status == Cog3DRangeImageCrossSectionOperatorResultConstants.FailedTolerance)
      {
        pt.Interactive = true;
        pt.Visible = true;
        pt.Color = CogColorConstants.Magenta;
        pt.SelectedSpaceName = "FixtureBrakePad";
        pt.GraphicDOFEnable = CogPointMarkerDOFConstants.None;
        defectGraphicCollection.Add(pt);
        recordDefects.SubRecords.Add(new CogRecord("Defects",
                                                   typeof(CogGraphicCollection),
                                                   CogRecordUsageConstants.Configuration,
                                                   true,
                                                   defectGraphicCollection,
                                                   "Defects")
                                    );
        DefectDisplay.Record = recordDefects;
      }
    }
    // At the beginning of each processing, initialize all needed variables.
    private void InitialializeProcessing()
    {
      // Initialize stepSizeInPixels
      stepSizeInPixels = (Int32)numericUpDown1.Value;

      // Grab the transform from the range image
      ICogTransform2D xFormSSNFromRoot = csTool.InputImage.GetTransform("FixtureBrakePad", "@");

      // Extract the scaling in Y between FixtureBarkePad and root
      double scaleY = 1;
      if (xFormSSNFromRoot.Linear)
        scaleY = xFormSSNFromRoot.LinearTransform(0, 0).ScalingY;
      else
      {
        MessageBox.Show("Range Image FixtureBrakePad space is not linear.");
        stepIndex = numberOfSteps;
        return;
      }

      // Compute the step size in SSN
      stepSizeInSSN = stepSizeInPixels * scaleY;

      // Convert the cross section image region to pixel space
      updateScanRegionInPixels();

      // Clear the defects collection
      defectGraphicCollection.Clear();

      // Disable the stepSizeInPixels and the NextImage button while processing
      btnNextImage.Enabled = false;
      numericUpDown1.Enabled = false;

      // Compute the number of steps to be performed
      numberOfSteps = (Int32)Math.Round(scanRegionInPixels.SideYLength / stepSizeInPixels);

    }
    // The processing section of this sample. 
    //      Slices the region
    //      Runs the Cross Section Tool on each slice
    //      Breaks if “freeze on defect” is checked and a divot is outside tolerance
    //      Adds defects to the record display
    private void btStartProcessing_Click(object sender, EventArgs e)
    {
      btStartProcessing.Text = "Processing";

      // First time
      if (stepIndex == 0)
        InitialializeProcessing();

      for (int i = stepIndex; i < numberOfSteps; i++)
      {        
        // Compute the CornerOrigin of the thin slice as we move through
        // the scan region
        // x = CornerOriginX + ((CornerYX - CornerOriginX) * (stepIndex/numberOfSteps))
        // y = CornerOriginY + ((CornerYY - CornerOriginY) * (stepIndex/numberOfSteps))

        double a = (double) stepIndex / (double) numberOfSteps;
        double x = scanRegionInSSN.CornerOriginX +
                   ((scanRegionInSSN.CornerYX - scanRegionInSSN.CornerOriginX) * a);
        double y = scanRegionInSSN.CornerOriginY +
                   ((scanRegionInSSN.CornerYY - scanRegionInSSN.CornerOriginY) * a);

        csTool.RunParams.ProfileParams.Region.SetOriginLengthsRotationSkew(x,
          y,
          scanRegionInSSN.SideXLength,
          stepSizeInSSN, // length Y is step size
          scanRegionInSSN.Rotation,
          scanRegionInSSN.Skew);
        
        // Run the Cross Section tool
        csTool.Run();

        // Update the processing region display
        updateProcessingDisplay();

        // Update profile display
        ProfileDisplay.Record = csTool.CreateLastRunRecord().SubRecords[1];
        ProfileDisplay.Fit(true);

        // Increment the stepIndex so
        // the next slice of the image will be processed after a freeze
        stepIndex = i + 1;

        // If distance is outside tolerance, add defect graphic
        if (!csTool.RunParams.OperatorsParams.Status)
        {
          addDefectGraphic();
          if (chkFreezeOnDefect.Checked)
          {
            btStartProcessing.Text = "Continue Processing";
            break;
          }
        }

      }
      if (isDoneProcessing())
      {
        // Update the Button text
        btStartProcessing.Text = "Start Processing";

        // Re-Enable the stepSize and the NextImage in the GUI
        btnNextImage.Enabled = true;
        numericUpDown1.Enabled = true;

        // Reset the stepIndex
        stepIndex = 0;

        // Reset the Cross Section initial scan region
        csTool.RunParams.ProfileParams.Region = new CogRectangleAffine(scanRegionInSSN);
      }

    }

    private void btnNextImage_Click(object sender, EventArgs e)
    {
      // Grab the next image from the cdb file
      imageFileTool.Run();

      // Update the range image
      rangeImage = imageFileTool.OutputImage;
      
      // Run all the tools 
      RunAllTools();
      
      // reset stepIndex
      stepIndex = 0;
    }

    private void btnShowControl_Click(object sender, EventArgs e)
    {
      // Show the Cross Section Tool in the Edit
      crossSectionEditForm = new ControlForm(csTool);
      crossSectionEditForm.Show();
    }

  }
}
