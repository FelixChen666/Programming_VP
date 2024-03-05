//*******************************************************************************
// Copyright (C) 2011 Cognex Corporation
//
// Subject to Cognex Corporation's terms and conditions and license agreement,
// you are authorized to use and modify this source code in any way you find
// useful, provided the Software and/or the modified Software is used solely in
// conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
// and agree that Cognex has no warranty, obligations or liability for your use
// of the Software.
// *******************************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Cognex.VisionPro;

namespace ToolBlockLoad
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {

      Cognex.Vision.Startup.Initialize(Cognex.Vision.Startup.ProductKey.VProX);
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1());

      //Releasing framegrabbers
      CogFrameGrabbers frameGrabbers = new CogFrameGrabbers();
      for (int i = 0; i < frameGrabbers.Count; i++)
      {
          frameGrabbers[i].Disconnect(false);
      }

      Cognex.Vision.Startup.Shutdown();
    }
  }
}
