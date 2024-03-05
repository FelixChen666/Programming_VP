// ***************************************************************************
// Copyright (C) 2007 Cognex Corporation
//
// Subject to Cognex Corporation's terms and conditions and license agreement,
// you are authorized to use and modify this source code in any way you find
// useful, provided the Software and/or the modified Software is used solely
// in conjunction with a Cognex Machine Vision System.  Furthermore you
// acknowledge and agree that Cognex has no warranty, obligations or liability
// for your use of the Software.
// ***************************************************************************
// This sample program is designed to illustrate certain VisionPro features or 
// techniques in the simplest way possible. It is not intended as the framework
// for a complete application. In particular, the sample program may not provide
// proper error handling, event handling, cleanup, repeatability, and other 
// mechanisms that a commercial quality application requires.
//
// This application makes use of the .NET method "Invoke" to move data between
// the Job Thread (worker thread) and the GUI thread.  Invoke() is a .NET 
// construct that allows thread safe access to objects on the Forms main GUI 
// thread (i.e. controls) from other threads.  It is forbidden to access GUI 
// objects directly from a thread that is not the Forms main GUI thread.  
//
// Invoke() is an important technique to understand when working with the
// CogJobManager.  Under the hood the CogJobManager uses many worker threads 
// to acquire images and run the jobs.  Often when the CogJobManager fires an 
// event the listening event handler function will be called on the worker 
// thread from which the event was fired or raised, and not the main GUI 
// thread.  For instance, a popular thing to do is to display a job's user 
// results on the GUI when the UserResultAvailable event is raised from the
// CogJobManager.  The UserResultAvailable event does not call its handler 
// function on the on the main GUI thread.  As updating the GUI to display 
// results obviously requires accessing controls(GUI thread objects), this is
// an example of a situation that requires Invoke().
//
// Using the Invoke mechanism has three main parts.
//
//    1.  The Delegate - The delegate is a .NET name for a function pointer.  
//        We need to declare a delegate and then instantiate an instance of 
//        this delegate.  The delegate is then passed by the Invoke() method
//        to tell Invoke() which method to call on the GUI thread.
//
//    2.  The InvokeRequired() call - This call simply returns true if the code
//        that is currently executing is not on the main GUI thread.  If 
//	      InvokeRequired is true, the current code is not running on the GUI
//	      thread, so we must call Invoke to access GUI objects.
//
//    3.  The Invoke() call - This call will block the current thread and wait
//        for the GUI thread to execute the method whose delegate we have passed
//        to the Invoke() call.
//
// Below is an example of how we use Invoke in this program, notice that we use
// InvokeRequired to see if we are on the correct thread, if not we create a 
// delegate to the same function and Invoke it on the GUI thread, when
// myJobManager_Stopped then executes on the GUI thread InvokeRequired() will 
// is false and we continue on to update the GUI elements.
//
//  // Delegate whose signature matches CJM events.
//  delegate void myJobManagerDelegate(Object Sender, 
//    CogJobManagerActionEventArgs e);
//
//  private void myJobManager_UserResultAvailable(object sender,
//    CogJobManagerActionEventArgs e)
//  {
//      if (InvokeRequired)
//      {
//          Invoke(new myJobManagerDelegate(myJobManager_UserResultAvailable),
//            new object[] { sender, e });
//          return;
//      }
//
//           ' CODE THAT UPDATES THE GUI GOES HERE
//  }
//
//  Note:  This sample is the same application that is generated as part of the
//         VisionPro training course; more insight into its parts can be gained 
//	       from reviewing the course material for the Application Development
//         section of this course.  If you do not have access to this material
//         please contact Cognex Technical Support (508)650-3100 for more
//         information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Cognex.VisionPro;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.PMAlign;

namespace AdvancedAppOne
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

    #region Varaible declarations

        private CogJobManager myJobManager; 
        private CogJob myJob; 
        private CogJobIndependent myIndependentJob; 

        // Delegate whose signature matches CJM events.
        delegate void myJobManagerDelegate(Object Sender, 
          CogJobManagerActionEventArgs e);

    #endregion

    #region Code that executes when the form first loads
        /// <summary>
        /// This function is responsible for the initial setup of the app. 
        /// It loads and prepares the saved QuickBuild app into a CogJobManager
        /// object, attaches event handlers to to interesting CogJobManager
        /// events, and sets up the CogDisplayStatusBar to reflect the status 
        /// of the CogDisplay we are using.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

            // Depersist the CogJobManager saved via QuickBuild
            myJobManager = CogSerializer.LoadObjectFromFile(
              Environment.GetEnvironmentVariable("VPRO_ROOT") +
              "\\Samples\\Programming\\QuickBuild\\advancedAppOne.vpp") as CogJobManager;

            // Initialize Variables
            myJob = myJobManager.Job(0);
            myIndependentJob = myJob.OwnedIndependent;

            // Flush queues
            myJobManager.UserQueueFlush();
            myJobManager.FailureQueueFlush();
            myJob.ImageQueueFlush();
            myIndependentJob.RealTimeQueueFlush();

            // Register handler for Stopped event
            myJobManager.Stopped +=
              new CogJobManager.CogJobManagerStoppedEventHandler(
              myJobManager_Stopped);

            // Register handler for UserResultAvailable event
            myJobManager.UserResultAvailable +=
              new CogJobManager.CogUserResultAvailableEventHandler(
              myJobManager_UserResultAvailable);

            // Connect the status bar
            this.cogDisplayStatusBar1.Display = this.cogRecordDisplay1;

        }
    #endregion

    #region Logic to handle button clicks

        /// <summary>
        /// This function handles the click event for the RunOnce button
        /// notice that it disables some other functionality that prevents
        /// the user from trying to do other things while the job is 
        /// allready running.  The functionality is re-enabled when the 
        /// CogJobManager raises or fires its "stopped" event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunOnceButton_Click(object sender, EventArgs e)
        {
            try
            {
                RunOnceButton.Enabled = false;
                RunContCheckBox.Enabled = false; // Disable if running
                ShowTrainCheckBox.Enabled = false;

                myJobManager.Run();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// This method handles a RunContinuous click similarly to the 
        /// RunOnce handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunContCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RunContCheckBox.Checked)
            {
                try
                {
                    RunOnceButton.Enabled = false;
                    ShowTrainCheckBox.Enabled = false;
                    myJobManager.RunContinuous();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else
            {
                try
                {
                    RunContCheckBox.Enabled = false;
                    ShowTrainCheckBox.Enabled = true;
                    myJobManager.Stop();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// This handles a click to the Show Train Image button.
        /// It allows the user to view train image record the pattern was 
        /// trained off of and also enable the Retrain button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowTrainCheckBox_CheckedChanged(
          object sender, EventArgs e)
        {
            if (this.ShowTrainCheckBox.Checked)
            {
                RunOnceButton.Enabled = false;
                RunContCheckBox.Enabled = false;
                RetrainButton.Enabled = true;

                CogToolGroup myTG = myJob.VisionTool as CogToolGroup;
                CogPMAlignTool myPMTool = myTG.Tools["CogPMAlignTool1"] as CogPMAlignTool;
                Cognex.VisionPro.ICogRecord tmpRecord;
                tmpRecord = myPMTool.CreateCurrentRecord();
                tmpRecord = tmpRecord.SubRecords["TrainImage"];
                cogRecordDisplay1.Record = tmpRecord;
                cogRecordDisplay1.Fit(true);
                RunStatusTextBox.Text = "";
            }
            else
            {
                RunOnceButton.Enabled = true;
                RunContCheckBox.Enabled = true;
                RetrainButton.Enabled = false;
                cogRecordDisplay1.Record = null;
            }
        }

        /// <summary>
        /// Handles a click to the Retrain Button, when clicked the PMAlignTool
        /// will retrain its pattern.  The new trained pattern will reflect any
        /// changes made to the train image record in the CogRecordDisplay.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RetrainButton_Click(object sender, EventArgs e)
        {
            CogToolGroup myTG = myJob.VisionTool as CogToolGroup;
            CogPMAlignTool myPMTool = myTG.Tools["CogPMAlignTool1"] as CogPMAlignTool;
            myPMTool.Pattern.Train();
        }

        #endregion

    #region CogJobManager event handlers
        /// <summary>
        ///  This function handles the stopped event from the CogJobManager
        ///  When the Job is stopped it re-enables the Run Buttons. Note that
        ///  this function uses Invoke() as described in the beginning of 
        ///  this document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myJobManager_Stopped(object sender, 
          CogJobManagerActionEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new myJobManagerDelegate(myJobManager_Stopped),
                  new object[] { sender, e });

                return;
            }

            RunOnceButton.Enabled = true;
            RunContCheckBox.Enabled = true;  // Enable when stopped
            
        }

        /// <summary>
        /// This method is the event handler for the user result available 
        /// event of the CogjobManger.  When this method is called the 
        /// CogJobManager is telling us that one of the jobs has run and has 
        /// genreated a new UserResult record packet.  This packet contians 
        /// information about which job, if it passed or failed, or any
        /// other information, objects, or images that were added to the
        /// PostedItems section of QuickBuild.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Note that this app updates the GUI every time a new result is
        /// available.  Processor time is used every time the GUI is updated.
        /// For applications that require very high throughput it might not
        /// make sense to update the GUI for each run of the job as it will not
        /// be noticeable to a user, and can slow down your overall throughput.
        /// 
        /// For high-throughput applications it will often make sense to 
        /// consider options like only updating the GUI for every other result
        /// for example.
        /// </remarks>
        private void myJobManager_UserResultAvailable(object sender,
          CogJobManagerActionEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new myJobManagerDelegate(
                  myJobManager_UserResultAvailable),new object[] 
                  { sender, e });

                return;

            }

             Cognex.VisionPro.ICogRecord topRecord =
               myJobManager.UserResult();
            RunStatusTextBox.Text = 
              topRecord.SubRecords["UserResultTag"].Content + ": " 
              + topRecord.SubRecords["JobName"].Content + " --> " 
              + topRecord.SubRecords["RunStatus"].Content.ToString();


            Cognex.VisionPro.ICogRecord tmpRecord;
            // Assume the required record is present and get it.
            tmpRecord = topRecord.SubRecords["ShowLastRunRecordForUserQueue"];
            tmpRecord = tmpRecord.SubRecords["LastRun"];
            tmpRecord = tmpRecord.SubRecords["CogFixtureTool1.OutputImage"];
            cogRecordDisplay1.Record = tmpRecord;
            cogRecordDisplay1.Fit(true);
        }

        #endregion

    #region Logic to gracefully close the app
        /// <summary>
        /// This function is called as the application is closing.  It 
        /// contains some important details about properly cleaning up
        /// an applicaiton that utilizes the CogJobManager.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // UNregister handler for Stopped event
            myJobManager.Stopped -= 
              new CogJobManager.CogJobManagerStoppedEventHandler(
              myJobManager_Stopped);

            // UNregister handler for UserResultAvailable event
            myJobManager.UserResultAvailable-=
              new CogJobManager.CogUserResultAvailableEventHandler(
              myJobManager_UserResultAvailable);
        
            // This line prevents a deadlock that can occur
            // if a UserResult becomes available after
            // Form1_FormClosing() is called but before we unregister
            // the UserResultAvailable event.
            Application.DoEvents();

            // Be sure to shutdown the CogJobManager!!
            myJobManager.Shutdown();

            // Explictly Dispose of all VisionPro controls
            cogDisplayStatusBar1.Dispose();
            cogRecordDisplay1.Dispose();
        }

        #endregion

    }
}