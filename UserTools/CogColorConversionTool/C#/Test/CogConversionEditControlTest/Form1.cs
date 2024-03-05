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

namespace CogConversionEditControlTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cogImageFileEditV21.Subject.Ran += new EventHandler(Subject_Ran);
        }

        /// <summary>
        /// After the image file tool has run, provide its output image to the input
        /// image of the CogColorConversionTool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Subject_Ran(object sender, EventArgs e)
        {
            CogImage24PlanarColor newImage;
            newImage = cogImageFileEditV21.Subject.OutputImage as CogImage24PlanarColor;
            if (newImage != null)
                cogColorConversionToolEditor1.Subject.InputImage = newImage;
            else
                MessageBox.Show("This sample requires a 24-bit color image as input. Please load a new image file.");
        }
    }
}