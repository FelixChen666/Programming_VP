//*******************************************************************************
// Copyright (C) 2015 Cognex Corporation
//
// Subject to Cognex Corporation's terms and conditions and license agreement,
// you are authorized to use and modify this source code in any way you find
// useful, provided the Software and/or the modified Software is used solely in
// conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
// and agree that Cognex has no warranty, obligations or liability for your use
// of the Software.
//*******************************************************************************
//
// This sample code was created to demonstrate the use of the Cog3DDisplayV2 in a 
// WPF environment. This sample gives an overview about the most important
// functionality of the Cog3DDisplayV2 along with the methods how these functionalities 
// can be used.
//
// The sample loads the %VPRO_ROOT%\Images\RangeWithGrey.idb file whic contains range
// images with their grey overlays. It demonstrates how to create the graphic wrapper
// for a range image with its grey overlay on top.
//
// It also has a preconfigured toolblock (ToolBlock.vpp) that looks for a Matrix code
// on the rgey image and displays the result with a see-through 3D bounding-box. It also
// shows the necessary steps to process the results of the grey image (which are in Sensor2D space)
// and how to figure out the 3D coordinates of some X,Y from Sensor2D in Sensor3D.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro3D;
using Cognex.VisionPro.ID;

namespace Cog3DDisplayV2WithGraphics
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    //local variable for the ToolBlock instance that will be used to process the range image
    private CogToolBlock tbProcessImage = null;

    //local collection of range images found in idb file
    List<CogImage16Range> lsRangeImages = new List<CogImage16Range>();

    public MainWindow()
    {
      InitializeComponent();

      //make sure to release Cog3DDisplayV2 WPF control when the application exits
      this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);

      //init application, load resources
      initApp();
    }

    void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      //make sure to release Cog3DDisplayV2 WPF control when the application exits
      display3D.Dispose();
    }

    private void initApp()
    {
      //load the ToolBlock.vpp that has the image processing logic
      tbProcessImage = CogSerializer.LoadObjectFromFile(@"ToolBlock.vpp") as CogToolBlock;

      //load the range image file
      loadRangeWithGrey();
    }

    // Loads the given range-with-grey range image
    private void loadRangeWithGrey()
    {
      //load the image file: RangeWithGrey.idb
      CogImageFile imageFile = new CogImageFile();
      CogImage16Range img = null;

      try
      {
        imageFile.Open(@"%VPRO_ROOT%\Images\RangeWithGrey.idb", CogImageFileModeConstants.Read);
        for (int i = 0; i < imageFile.Count; i++)
        {
          // Open image
          img = imageFile[i] as CogImage16Range;
          if (img != null)
            lsRangeImages.Add(img);
        }

      }
      catch (Exception ex)
      {
      }
      finally
      {
        if (imageFile != null)
          imageFile.Close();
      }

      //initialize scroll bar
      imageScroll.Value = 0;
      imageScroll.Maximum = lsRangeImages.Count - 1;

      //load first image
      loadAndProcessRangeImage(0);

      //fit into view
      display3D.FitView();
    }

    private void imageScroll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      loadAndProcessRangeImage((int)imageScroll.Value);
    }

    private void loadAndProcessRangeImage(int idx)
    {
      //run toolblock on the grey image and show the results
      tbProcessImage.Inputs["InputImage"].Value = lsRangeImages[idx];
      tbProcessImage.Run();

      //clear the contents of the display
      display3D.Clear();

      if (tbProcessImage.RunStatus.Result == CogToolResultConstants.Accept)
      {
        CogIDResults idResults = tbProcessImage.Outputs["Results"].Value as CogIDResults;
        CogImage16Range rangeImage = tbProcessImage.Outputs["RangeImage"].Value as CogImage16Range;
        CogImage16Grey greyImage = tbProcessImage.Outputs["GreyImage"].Value as CogImage16Grey;

        //create graphic wrapper for the range image with its grey overlay
        Cog3DRangeImageGraphic rImgG = new Cog3DRangeImageGraphic(rangeImage, greyImage);

        //add it to the display
        display3D.Add(rImgG);
        
        showResults(rImgG, idResults);
      }
      else
      {
        MessageBox.Show(tbProcessImage.RunStatus.Message, "ToolBlock error");
      }
    }

    // Shows the found Matrix codes with a 3D bounding box on the range image
    private void showResults(Cog3DRangeImageGraphic rImgG, CogIDResults idResults)
    {
      //if there is any result at all
      if (idResults != null && idResults.Count > 0)
      {
        double boxSize = 5.1; //the matrix code we are looking for is apprx 5.1x5.1 mm
        double boxSizeZ = 2; //some height for the bounding box just to make it 3D
        
        CogImage16Range image = rImgG.RangeImage;
        ICogTransform2D xform2D = rImgG.GreyImage.GetTransform("#", "Sensor2D");
        ICog3DTransform xform3D = image.GetTransform3D("Sensor3D", "@");

        bool visible;
        ushort heightVal;
        double mappedX, mappedY;
        for (int i = 0; i < idResults.Count; i++)
        {
          CogIDResult r = idResults[i];

          //figure out the result's coordinates in Sensor3D
          //#1: figure out the XY pixel coordinates from the XY mm values
          xform2D.MapPoint(r.CenterX, r.CenterY, out mappedX, out mappedY);
          int x = (int)mappedX;
          int y = (int)mappedY;

          //#2: get the height value at that XY from the RangeImage
          image.GetPixel(x, y, out visible, out heightVal);

          //#3: map that XYZ from pixel space to mm space
          Cog3DVect3 centerOfResultInMM = xform3D.MapPoint(new Cog3DVect3(x, y, heightVal));

          //create a marker box with its center positioned at the origin
          Cog3DBox b = new Cog3DBox();
          b.SetOriginVertexXVectorYVectorZ(new Cog3DVect3(- boxSize / 2, - boxSize / 2, - boxSizeZ / 2), new Cog3DVect3(boxSize, 0, 0), new Cog3DVect3(0, boxSize, 0), boxSizeZ);

          //rotate the box around the Z-axis by the result's Angle
          //and translate it by the result's coordinates in Sensor3D
          Cog3DTransformRotation rotation = new Cog3DTransformRotation(new Cog3DEulerXYZ(0, 0, r.Angle));
          b = b.MapShape(new Cog3DTransformRigid(rotation, centerOfResultInMM)) as Cog3DBox;

          if (b != null)
          {
            Cog3DBoxGraphic bg = new Cog3DBoxGraphic(b);
            bg.Opacity = 0.5;
            bg.Color = CogColorConstants.Cyan;
            bg.DisplayState = Cog3DGraphicDisplayStateConstants.SurfaceWithWireFrame;

            display3D.Add(bg, rImgG);
          }
        }
      }
    }
  }
}
