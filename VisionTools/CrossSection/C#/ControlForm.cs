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
  public partial class ControlForm : Form
  {
    public ControlForm(Cog3DRangeImageCrossSectionTool subject)
    {
      InitializeComponent();
      cog3DRangeImageCrossSectionEditV21.Subject = subject;

    }

  }
}
