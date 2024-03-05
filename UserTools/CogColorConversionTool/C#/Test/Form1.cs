/*******************************************************************************
Copyright (C) 2008 Cognex Corporation

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

This sample demonstrates how a user can create their own Vision Tools using 
base classes the Cognex provides.  The tool created in this class converts 3-plane
RGB images to individual R, G, and B planes, or converts a 3-plane RGB image to 
individual H, S, and I planes.

The tool created is called the CogColorConversionTool.  It makes use of an operator, which
does all the real work, called CogColorConversion.

*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.Exceptions;

namespace ColorConversionTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            CogImageFileTool mImageFileTool = new CogImageFileTool();
            InitializeComponent();

            String VProLocation;
            // Locate the image file using the environment variable that indicates
            // where VisionPro is installed.  If the environment variable is not set,
            // throw an exception that will be caught by the caller.
            VProLocation = Environment.GetEnvironmentVariable("VPRO_ROOT");
            if ((VProLocation == null) || (VProLocation == ""))
                throw new VPRORootNotSetException();

            VProLocation = VProLocation + "/Images/smiley.bmp";
            mImageFileTool.Operator.Open(VProLocation, CogImageFileModeConstants.Read);
            hasConverted = false;
            mImageFileTool.Run();
            inputImage = (CogImage24PlanarColor)mImageFileTool.OutputImage;
            origDisplay.Image = inputImage;
            origRegion.GraphicDOFEnable = CogRectangleDOFConstants.All;
            origRegion.Interactive = true;
            origRegion.SetXYWidthHeight(inputImage.Width / 4, inputImage.Height / 4,
                inputImage.Width / 2, inputImage.Height / 2);
            origDisplay.InteractiveGraphics.Add(origRegion, "Input Region", false);
        }

        private bool hasConverted;
        private CogRectangle origRegion = new CogRectangle();
        private CogImage24PlanarColor inputImage;
        CogColorConversionTool.CogColorConversionTool mColorConversionTool = 
            new CogColorConversionTool.CogColorConversionTool();

        private void radioPlane0_CheckedChanged(object sender, EventArgs e)
        {
            if (hasConverted)
                convertedDisplay.Image = mColorConversionTool.OutputImage0;
            else if (radioPlane0.Checked)
                MessageBox.Show("Image hasn't yet been converted");
        }

        private void radioPlane1_CheckedChanged(object sender, EventArgs e)
        {
            if (hasConverted)
                convertedDisplay.Image = mColorConversionTool.OutputImage1;
            else if (radioPlane1.Checked)
                MessageBox.Show("Image hasn't yet been converted");
        }

        private void radioPlane2_CheckedChanged(object sender, EventArgs e)
        {
            if (hasConverted)
                convertedDisplay.Image = mColorConversionTool.OutputImage2;
            else if (radioPlane2.Checked)
                MessageBox.Show("Image hasn't yet been converted");
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            mColorConversionTool.HSI = chkConvertToHSI.Checked;
            mColorConversionTool.Region = origRegion;
            mColorConversionTool.InputImage = inputImage;
            mColorConversionTool.Run();

            hasConverted = true;
            if (radioPlane0.Checked)
                convertedDisplay.Image = mColorConversionTool.OutputImage0;
            else if (radioPlane1.Checked)
                convertedDisplay.Image = mColorConversionTool.OutputImage1;
            else if (radioPlane2.Checked)
                convertedDisplay.Image = mColorConversionTool.OutputImage2;
            else
            {
                radioPlane0.Checked = true;
                convertedDisplay.Image = mColorConversionTool.OutputImage0;
            }
        }

        // Create a custom exception that will be thrown when the VPRO_ROOT isn't set.
        class VPRORootNotSetException : CogException
        {
            public VPRORootNotSetException()
                : base("VPRO_ROOT not set")
            {
            }
            public VPRORootNotSetException(String msg)
                : base(msg)
            {
            }
        }
    }
}