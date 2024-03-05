//*******************************************************************************
// Copyright (C) 2010 Cognex Corporation
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
// This sample demonstrates how to use the CogVerifierBasic to verify a CogToolBlock
// against a database.  This sample shows how to expose this functionality in a
// WinForms application.  
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Cognex.VisionPro;
using Cognex.VisionPro.Database;
using Cognex.VisionPro.Inspection;
using Cognex.VisionPro.ToolBlock;

namespace UserVerification
{
    public partial class UserVerificationForm : Form
    {
        public UserVerificationForm()
        {
            InitializeComponent();
        }

        private CogVerificationDatabase mInputDatabase = null;
        private CogVerificationDatabase mOutputDatabase = null;
        private CogToolBlock mToolBlock = null;
        private CogVerifierBasic mVerifier = null;

        private void UserVerificationForm_Load(object sender, EventArgs e)
        {
            // Connect to the sample input database
            string rawPath = @"%VPRO_ROOT%\samples\Programming\Inspection\Verification\SampleDatabase";
            string expandedPath = System.Environment.ExpandEnvironmentVariables(rawPath);
            mInputDatabase = new CogVerificationDatabase(new CogDatabaseDirectory(expandedPath));
            mInputDatabase.Connect();
            mInputDatabaseLabel.Text = mInputDatabase.Database.Name;

            // Create a output database and connect to it.  The output database is where
            // the results of Verification will be stored.
            string randomFilename = System.IO.Path.GetRandomFileName();
            string outputDatabasePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), randomFilename);
            System.IO.Directory.CreateDirectory(outputDatabasePath);
            mOutputDatabase = new CogVerificationDatabase(new CogDatabaseDirectory(outputDatabasePath));
            mOutputDatabase.Connect();
            mOutputDatabaseLabel.Text = mOutputDatabase.Database.Name;

            // Load up the sample CogToolBlock that needs to be verified.  Note, this 
            // CogToolBlock has an InputImage and BlobThreshold input terminals.  The 
            // InputImage terminal is required for Verification.
            rawPath = @"%VPRO_ROOT%\samples\Programming\Inspection\Verification\UserVerification\CogToolBlock.vpp";
            expandedPath = System.Environment.ExpandEnvironmentVariables(rawPath);
            try
            {
                mToolBlock = CogSerializer.LoadObjectFromFile(expandedPath) as CogToolBlock;
            }
            catch(Exception /* ex */)
            {
                MessageBox.Show("Cannot load CogToolBlock: " + expandedPath, "Error");
                Application.Exit();
            }
           
            mToolBlockLabel.Text = expandedPath;

            // Create the verifier and connect to various interesting event handlers.
            mVerifier = new CogVerifierBasic(mToolBlock, mInputDatabase, mOutputDatabase);
            mVerifier.UnknownResultBehavior = CogVerificationUnknownResultBehaviorConstants.AlwaysMatch;
            mVerifier.RunStarted += new EventHandler(mVerifier_RunStarted);
            mVerifier.RunCompleted += new CogVerifierRunCompletedEventHandler(mVerifier_RunCompleted);
            mVerifier.RunProgressChanged += new CogVerifierRunProgressChangedEventHandler(mVerifier_RunProgressChanged);

            mBlobThresholdNumericUpDown.Value = 170;
        }

        private delegate void Verifier_RunStartedDelegate(object sender, EventArgs e);
        void mVerifier_RunStarted(object sender, EventArgs e)
        {
            // The RunStarted event fires when the verifier first starts running.

            // Make sure to invoke to the GUI thread since all verifier events are fired from a
            // non-GUI thread.
            if (mVerificationResultLabel.InvokeRequired)
            {
                mVerificationResultLabel.Invoke(new Verifier_RunStartedDelegate(mVerifier_RunStarted), new object[] { sender, e });
                return;
            }

            mVerifyButton.Enabled = false;

            mTotalLabel.Text = "Total: 0";
            mMatchedLabel.Text = "Match: 0";
            mMismatchedLabel.Text = "Mismatch: 0"; 

        }

        private delegate void Verifier_RunProgressChangedDelegate(object sender, CogVerifierRunProgressChangedEventArgs e);
        void mVerifier_RunProgressChanged(object sender, CogVerifierRunProgressChangedEventArgs e)
        {
            // The RunProgressChanged event fires when the verifier has finished verifying one item from
            // the input database.

            // Make sure to invoke to the GUI thread since all verifier events are fired from a
            // non-GUI thread.
            if (mVerificationResultLabel.InvokeRequired)
            {
                mVerificationResultLabel.Invoke(new Verifier_RunProgressChangedDelegate(mVerifier_RunProgressChanged), new object[] { sender, e });
                return;
            }

            mVerificationResultLabel.ForeColor = System.Drawing.Color.Black;
            mVerificationResultLabel.BackColor = System.Drawing.SystemColors.Control;
            mVerificationResultLabel.Text = "Verifying... " + e.ProgressPercentage + "% complete";
        }

        private delegate void Verifier_RunCompletedDelegate(object sender, CogVerifierRunCompletedEventArgs e);
        void mVerifier_RunCompleted(object sender, CogVerifierRunCompletedEventArgs e)
        {
            // The RunCompleted event fires when the verifier has finished verifying all items in the
            // input database.

            // Make sure to invoke to the GUI thread since all verifier events are fired from a
            // non-GUI thread.
            if (mVerificationResultLabel.InvokeRequired)
            {
                mVerificationResultLabel.Invoke(new Verifier_RunCompletedDelegate(mVerifier_RunCompleted), new object[] { sender, e });
                return;
            }

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error from verifier");
            }

            // Go through the output database and figure out the total Verification result.
            int failureCount = 0;
            foreach (CogVerificationData vdata in mOutputDatabase)
            {
                if (vdata.Results.OverallVerificationResult.Value == false)
                {
                    failureCount++;
                }
            }

            if (failureCount > 0)
            {
                mVerificationResultLabel.Text = "Verification FAILED";
                mVerificationResultLabel.ForeColor = System.Drawing.Color.White;
                mVerificationResultLabel.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                mVerificationResultLabel.Text = "Verification Passed";
                mVerificationResultLabel.ForeColor = System.Drawing.Color.White;
                mVerificationResultLabel.BackColor = System.Drawing.Color.Green;
            }

            mTotalLabel.Text = "Total: " + mVerifier.Statistics.TotalCount.ToString();
            mMatchedLabel.Text = "Match: " + mVerifier.Statistics.MatchesCount.ToString();
            mMismatchedLabel.Text = "Mismatch: " + mVerifier.Statistics.MismatchesCount.ToString();

            mVerifyButton.Enabled = true;
        }

        private void UserVerificationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Be sure to cleanup the input and output databases before exiting.

            if (mInputDatabase != null && mInputDatabase.Database.Connected)
            {
                mInputDatabase.Disconnect();
            }

            if (mOutputDatabase != null && mOutputDatabase.Database.Connected)
            {
                mOutputDatabase.Disconnect();
                System.IO.Directory.Delete(mOutputDatabase.Database.Name, true);
            }
        }

        private void mVerifyButton_Click(object sender, EventArgs e)
        {
            // Run the verifier asynchronously.  This call will return immediately and the Verification will
            // occur in a worker thread.  Do not call the synchronous Run() method in a GUI callback since the
            // Verification may take a considerable amount of time and during this time all GUI events will be 
            // blocked.

            mVerifier.RunAsync();
        }

        private void mNumBlobsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            mToolBlock.Inputs["BlobThreshold"].Value = mBlobThresholdNumericUpDown.Value;
        }
    }
}