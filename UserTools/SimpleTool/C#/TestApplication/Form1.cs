/*******************************************************************************
Copyright (C) 2008 Cognex Corporation

Subject to Cognex Corporations terms and conditions and license agreement,
you are authorized to use and modify this source code in any way you find
useful, provided the Software and/or the modified Software is used solely in
conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
and agree that Cognex has no warranty, obligations or liability for your use
of the Software.
*******************************************************************************/
// This sample program is designed to illustrate certain VisionPro
// features or techniques in the simplest way possible. It is not
// intended as the framework for a complete application. In
// particular, the sample program may not provide proper error
// handling, event handling, cleanup, repeatability, and other
// mechanisms that a commercial quality application requires.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cogImageFileEditV21.Subject.Ran += new EventHandler(Subject_Ran);
        }

        void Subject_Ran(object sender, EventArgs e)
        {
            Cognex.VisionPro.CogImage8Grey grey = cogImageFileEditV21.Subject.OutputImage as Cognex.VisionPro.CogImage8Grey;
            if (grey == null)
               MessageBox.Show("Image type not supported.");
            simpleToolEditV21.Subject.InputImage = grey;
        }
    }
}