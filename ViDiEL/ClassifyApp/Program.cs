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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cognex.Vision;

namespace ClassifyApp
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      // NOTE: use of ViDiEL tool(s) and/or CogToolBlock require
      // this call to Startup.Initialize(...) as early in the
      // process as possible.
      Startup.Initialize(Startup.ProductKey.VProX);

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new ClassifyApp());

      // NOTE: a call to Startup.Initialize(...) should always
      // be paired with a call to Startup.Shutdown() as late
      // in the process as possible.
      Startup.Shutdown();
    }
  }
}
