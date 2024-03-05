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
// This sample program is designed to illustrate certain VisionPro features or 
// techniques in the simplest way possible. It is not intended as the framework 
// for a complete application. In particular, the sample program may not provide
// proper error handling, event handling, cleanup, repeatability, and other 
// mechanisms that a commercial quality application requires.
//
// This sample demonstrates how to use the enhanced verification API to add any arbitrary
// property to your records in a database and have it verified with the built-in Toolblock
// verification.
//
// Rational:
// In some vision applications, it is required to expose certain configuration parameters 
// to the end users.  After an end user changes one of these exposed parameters, the end 
// user does not know if they have negatively impacted the vision application.  Verification 
// allows the end user to run their CogToolBlock against an input database in order to 
// confirm that the configuration changes did not break the vision application.
//
// *******************************************************************************
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UserGrading
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UserGradingForm());
        }
    }
}