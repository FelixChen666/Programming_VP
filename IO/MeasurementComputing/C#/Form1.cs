// *******************************************************************************
// Copyright (C) 2010 Cognex Corporation
//
// Subject to Cognex Corporations terms and conditions and license agreement,
// you are authorized to use and modify this source code in any way you find
// useful, provided the Software and/or the modified Software is used solely in
// conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
// and agree that Cognex has no warranty, obligations or liability for your use
// of the Software.
// *******************************************************************************
//
//
// This sample program is designed to illustrate certain VisionPro features or 
// techniques in the simplest way possible. It is not intended as the framework 
// for a complete application. In particular, the sample program may not provide
// proper error handling, event handling, cleanup, repeatability, and other 
// mechanisms that a commercial quality application requires.
// 
// This application requires a Measurement Computing board to be installed.
//
// The intention of this sample program is to demonstrate how to program
// both input and output lines of an Measurement Computing board (MCB)
// using both script and a standalone application.
//
// This application loads QuickBuildMCB_Script.vpp, which contains 
// a single job called MCB_RaiseOutputs_OnAcqComplete_JobScript. Port CL 
// is programmed to be used as an output port. The main purpose of the script 
// is to show how to program output lines of the MCB.
//
// This application also configures port A as an input port and waits for a signal on line 0.
// When the line 0 is triggered by the external device, the GUI will display the message
// 'Input signal received count' on the output textbox.
// 
// You can also directly load QuickBuildMCB_Script.vpp from the QuickBuild 
// application and run it. The Online button on the QuickBuild toolbar works 
// the same way as the Online button in this sample program.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Cognex.VisionPro.QuickBuild;
using System.IO;
using Cognex.VisionPro;
using Cognex.VisionPro.QuickBuild.IO;
using System.Diagnostics;

namespace MeasurementComputing
{
  
  public partial class Form1 : Form
  {
    private CogJobManager _manager;
    // Measurement Computing Board (MCB) I/O Device
    CogIOMCB _mcb = null;
    const string offLine = "Offline";
    const string onLine = "Online";
    const string run = "Run";
    const string stop = "Stop";

    public Form1()
    {
      InitializeComponent();

      btnOnline.Text = onLine;
      btnOnline.Enabled = LoadQBApplication();
      btnRun.Enabled = btnOnline.Enabled;
      Text1.Select(0, 0); // un-highlight the description text
    }

    #region Initialization
    // This routine loads QuickBuildMCB_Script.vpp shipped with VisionPro
    // This file must reside in samples\Programming\IO\MeasurementComputing
    private bool LoadQBApplication()
    {
      const string expectedPath = "MeasurementComputing";
      const string sampleName = "QuickBuildMCB_Script.vpp";
      string path = Environment.GetEnvironmentVariable("VPRO_ROOT");
      if (path == null)
      {
        textBox1.Text = "Missing " + expectedPath +
          " directory. Fix the problem and try it again.";
        return false;
      }

      // Make sure QuickBuildMCB_Script.vpp can be found.
      path += @"\samples\Programming\IO\MeasurementComputing\" + sampleName;
      if (!File.Exists(path))
      {
        textBox1.Text = "Unable to locate " + expectedPath +
          ". Fix the problem and try it again.";
        return false;
      }
    
      try
      {  // Let's load the sample program.
        _manager = CogSerializer.LoadObjectFromFile(path) as CogJobManager;
        if (_manager == null)
        {
          textBox1.Text = path + " is corrupted. Unable to continue.";
          return false;
        }

        if (GetMCB())
        {
          // Wait for an input signal.
          InitializeInput();

          // Let's monitor the job manager stop event.
          // We need to do this so that we can change the button state.
          _manager.Stopped += Manager_Stopped;
        }
        else
        {
          textBox1.Text =
            "The system must have a Measurement Computing board to run this application.";
          btnOnline.Enabled = false;
          return false;
        }
      }
      catch (Exception ex)
      {
        textBox1.Text = "Unexpected error: " + ex.Message;
        return false;
      }
      return true;
    }

    private bool GetMCB()
    {
      // CogJobManager always creates an instance of MCB found in the system.
      // Hence, we must use the one that is created by CogJobManager.
      // This program finds the first MCB found by CogJobManager and uses it.
      foreach (CogIODiscreteDevice device in _manager.IODeviceList.DiscreteDevices)
      {
        // Make sure the device type if CogIOMCB
        if (device is CogIOMCB)
        {
          _mcb = device as CogIOMCB;
          break;
        }
      }
      if (_mcb == null)
      {
        // The system does not have an MCB board.
        return false;
      }

      // If you have more than one MCB board, you can use
      // DeviceIndex to identify a specific board.
      textBox1.Text = string.Format("Found {0} with an index {1}",
                                    _mcb.DeviceName, _mcb.DeviceIndex);
      return true;
    }

    #endregion

    #region Button handlers
    // Do not change the IOEnable state when CogJobManager is running.
    // This routine performs the same task as pressing the Online button
    // from QuickBuild.
    private void btnOnline_Click(object sender, EventArgs e)
    {
      if (btnOnline.Text == onLine)
      {
        // Let's go online which enables both input and output lines
        btnOnline.Text = offLine;
        if (!_manager.IOEnable)
          _manager.IOEnable = true;
      }
      else
      {
        btnOnline.Text = onLine;
        // Display both input and output lines.
        _manager.IOEnable = false;
      }
    }

    private void btnRun_Click(object sender, EventArgs e)
    {
      if (btnRun.Text == run)
      {
        // Let's run the QuickBuild application.
        btnRun.Text = stop;
        _manager.Run();

        // disable the other button to avoid any contention.
        btnOnline.Enabled = false;
      }
      else
      {
        // Let's stop the job manager
        _manager.Stop();

        // Now let's wait for the stopped event
        // See Manager_Stopped().
      }
    }
    #endregion

    #region Input Signal Handler
    //
    // A bank of the first eight lines of a Measurement Computing "USB-1024LS" or "PCI-DIO24/S"
    // board is configured as input lines; however, it only uses the first line in this sample.
    // The sample runs When it receives an input signal  
    // It overrides the Initialize, PostAcquisitionRefInfo to acheive this.
    //
    int _inputLine = 0;      // Wait for an input signal from line 0

    //Perform any initialization required by your script here.
    private void InitializeInput()
    {
      // Use CogIOMCBPortNumberConstant to define a port direction.
      // Set the the port direction to handle input
      _mcb.SetPortDirection(CogIOMCBPortNumberConstants.PortA,
        CogIOMCBPortDirectionConstants.Input);
      _mcb.SetLineEnabled(_inputLine, true);

      // Assign an input delegate
      _mcb.InputDelegate[_inputLine] += InputSignalReceived;

      // Let's wait for the input signal
    }

    private int _signalReceived = 0;
    private void InputSignalReceived(bool asserted)
    {
      if (textBox1.InvokeRequired)
      {
        // It's not safe to update the form from the input signal thread, 
        // let's switch to the GUI thread.
        textBox1.Invoke(new CogIODiscreteDevice.InputDelegateFn(InputSignalReceived), 
          new object[] {asserted});
        return;
      }
      // Increment the counter and update the GUI.
      _signalReceived++;
      this.textBox1.Text = 
        string.Format("Input signal received count = {0}", _signalReceived);
      
      // Perform other tasks...
    }
    #endregion

    #region Manager Stopped Handler
    // We get this event when CogJobManger stops
    void Manager_Stopped(object sender, CogJobManagerActionEventArgs e)
    {
      if (btnRun.InvokeRequired)
      {
        // It's not safe to update the form from the job manager stopped thread, 
        // let's switch to the GUI thread.
        btnRun.Invoke(new CogJobManager.CogJobManagerStoppedEventHandler(Manager_Stopped),
          new object[] {sender, e});
        return;
      }
      // Show "Run" on the button
      btnRun.Text = run;

      // Allow the user to change the IOEnable state.
      btnOnline.Enabled = true;

      // You might want to consider going Offline; otherwise,
      // the program will respond to the incoming input signal.
    }
    #endregion

    #region Form Closing
    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (_manager != null)
      {
        if (_mcb != null)
        {
          _mcb.MonitorInputs = false;
          _mcb.InputDelegate[_inputLine] -= InputSignalReceived;
        }

        // Must shutdown the job manager before existing
        _manager.Shutdown();
        _manager = null;
      }
    }
    #endregion
  }
}
