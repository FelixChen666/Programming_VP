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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Cognex.VisionPro;
using Cognex.VisionPro.Database;
using Cognex.VisionPro.Inspection;

namespace UserGrading
{
    public partial class UserGradingForm : Form
    {
        public UserGradingForm()
        {
            InitializeComponent();
        }

        // Create variable for input database and the current record
        private CogVerificationDatabase mInputDatabase = null;
        private CogVerificationData mCurrentRecord = null;

        private void importImages()
        {
            // Import images from SampleImages directory
            string rawPath = @"%VPRO_ROOT%\samples\Programming\Inspection\AdvancedGrading\SampleImages";
            string expandedPath = System.Environment.ExpandEnvironmentVariables(rawPath);

            DirectoryInfo di = new DirectoryInfo(expandedPath);
            
            // Get only .bmp files
            FileInfo[] rgFiles = di.GetFiles("*.bmp");

            if (mInputDatabase != null && mInputDatabase.Database.Connected)
            {
                foreach (FileInfo fi in rgFiles)
                {
                    //Add image to the database
                    mInputDatabase.AddImage(fi.FullName,true,CogVerificationSimpleResultConstants.Accept,"Excellent");
                }
            }

            // Update grades
            string[] oldgrades = mInputDatabase.GetGrades();
            foreach (string oldgrade in oldgrades)
                mInputDatabase.RemoveGrade(oldgrade);
            
            mInputDatabase.AddGrade("Poor");
            mInputDatabase.AddGrade("Good");
            mInputDatabase.AddGrade("Excellent");

            // Fill up combobox with the grades
            cboGrades.Items.Clear();
            foreach (string grade in mInputDatabase.GetGrades())
            {
                cboGrades.Items.Add(grade);
            }

            // Set "Excellent" as the default grade
            cboGrades.SelectedItem = "Excellent";
        }

        #region "Event handlers"

        private void UserGradingForm_Load(object sender, EventArgs e)
        {
            // Load database and connect
            string rawPath = @"%VPRO_ROOT%\samples\Programming\Inspection\AdvancedGrading\SampleDatabase";
            string expandedPath = System.Environment.ExpandEnvironmentVariables(rawPath);

            bool exists = System.IO.Directory.Exists(expandedPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(expandedPath);

            mInputDatabase = new CogVerificationDatabase(new CogDatabaseDirectory(expandedPath));
            mInputDatabase.Connect();

            // If the database is empty, import the images
            if (mInputDatabase.Database.GetCount() < 1)
            {
                importImages();
            }

            // Fill grades
            cboGrades.Items.Clear(); 
            foreach (string grade in mInputDatabase.GetGrades())
            {
                cboGrades.Items.Add(grade);
            }

            // Set the scrollbar
            scrlImages.Maximum = mInputDatabase.Database.GetCount();
            scrlImages.Enabled = true;
            scrlImages_ValueChanged(sender, e);

            // Set other controls
            mInputDatabaseLabel.Text = "Connected!";
            btnClear.Enabled = true;
            btnMeasure.Enabled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Restore all records in database?", "Confrimation", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                // Clear current record
                mCurrentRecord = null;
                
                // Clear database
                try
                {
                    mInputDatabase.Clear();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                
                // Import images
                importImages();

                // Scroll to the first image
                scrlImages.Value = 1;
                scrlImages_ValueChanged(sender, e);
            }
        }

        private void btnMeasure_Click(object sender, EventArgs e)
        {
            // Measure the values with blob tool

            this.Cursor = Cursors.WaitCursor;

            // Create a default constructed blob tool
            Cognex.VisionPro.Blob.CogBlobTool blobTool = new Cognex.VisionPro.Blob.CogBlobTool();

            // Set blob tool's input image
            blobTool.InputImage = mCurrentRecord.Params.Inputs.InputImage.Value as Cognex.VisionPro.CogImage8Grey;

            // Run the blob tool
            blobTool.Run();

            // If blob tool has no results
            if (blobTool.Results == null)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("Error running blob tool!");
                return;
            }

            // Get the measured values and set ranges
            nudArea.Value = Convert.ToDecimal(blobTool.Results.GetBlobs()[0].Area);
            nudRArea.Value = 0.05m;
            nudCOMX.Value = Convert.ToDecimal(blobTool.Results.GetBlobs()[0].CenterOfMassX);
            nudRCOMX.Value = 0.05m;
            nudCOMY.Value = Convert.ToDecimal(blobTool.Results.GetBlobs()[0].CenterOfMassY);
            nudRCOMY.Value = 0.05m;

            // Calculate the grade
            if (blobTool.Results.GetBlobs()[0].Area < 2000)
                cboGrades.SelectedItem = "Poor";
            else if ((blobTool.Results.GetBlobs()[0].Area >= 2000) && (blobTool.Results.GetBlobs()[0].Area < 13000))
                cboGrades.SelectedItem = "Good";
            else
                cboGrades.SelectedItem = "Excellent";

            this.Cursor = Cursors.Default;
        }

        private void scrlImages_ValueChanged(object sender, EventArgs e)
        {
            // Set the current record
            mCurrentRecord = mInputDatabase.Fetch(scrlImages.Value - 1);

            // Create additional fields if needed
            if (!mCurrentRecord.Params.ExpectedOutputs.Contains("Area"))
                mCurrentRecord.Params.ExpectedOutputs.Add("Area", (double)0, (double)0);

            if (!mCurrentRecord.Params.ExpectedOutputs.Contains("CenterOfMassX"))
                mCurrentRecord.Params.ExpectedOutputs.Add("CenterOfMassX", (double)0, (double)0);

            if (!mCurrentRecord.Params.ExpectedOutputs.Contains("CenterOfMassY"))
                mCurrentRecord.Params.ExpectedOutputs.Add("CenterOfMassY", (double)0, (double)0);

            // Update the record in the database
            try
            {
                mInputDatabase.Replace(mCurrentRecord);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            // Update values
            nudArea.Value = Convert.ToDecimal(mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords["Area"].Content);
            nudRArea.Value = Convert.ToDecimal(mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords["Area"].SubRecords[CogFieldNameConstants.Range].Content);
            nudCOMX.Value = Convert.ToDecimal(mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords["CenterOfMassX"].Content);
            nudRCOMX.Value = Convert.ToDecimal(mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords["CenterOfMassX"].SubRecords[CogFieldNameConstants.Range].Content);
            nudCOMY.Value = Convert.ToDecimal(mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords["CenterOfMassY"].Content);
            nudRCOMY.Value = Convert.ToDecimal(mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords["CenterOfMassY"].SubRecords[CogFieldNameConstants.Range].Content);
            cboGrades.SelectedItem = mCurrentRecord.Params.ExpectedOutputs.Grade.Value;

            // Display the contained image
            cogDisplay1.Image = mCurrentRecord.Params.Inputs.InputImage.Value;
            cogDisplay1.Fit(true);

            // Display record key
            lblRecordName.Text = mCurrentRecord.Record.RecordKey;
        }

        private void UserGradingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Be sure to disconnect.

            if (mInputDatabase != null && mInputDatabase.Database.Connected)
            {
                mInputDatabase.Disconnect();
            }
        }

        #endregion

        #region "Value changes"

        // For each value changes the record will be updated

        private void nudArea_ValueChanged(object sender, EventArgs e)
        {
            mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords["Area"].Content = (double)nudArea.Value;

            // Update the record in the database
            mInputDatabase.Replace(mCurrentRecord);
        }

        private void nudRArea_ValueChanged(object sender, EventArgs e)
        {
            mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords["Area"].SubRecords[CogFieldNameConstants.Range].Content = (double)nudRArea.Value;

            // Update the record in the database
            mInputDatabase.Replace(mCurrentRecord);
        }

        private void nudCOMX_ValueChanged(object sender, EventArgs e)
        {
            mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords["CenterOfMassX"].Content = (double)nudCOMX.Value;

            // Update the record in the database
            mInputDatabase.Replace(mCurrentRecord);
        }

        private void nudRCOMX_ValueChanged(object sender, EventArgs e)
        {
            mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords["CenterOfMassX"].SubRecords[CogFieldNameConstants.Range].Content = (double)nudRCOMX.Value;

            // Update the record in the database
            mInputDatabase.Replace(mCurrentRecord);
        }

        private void nudCOMY_ValueChanged(object sender, EventArgs e)
        {
            mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords["CenterOfMassY"].Content = (double)nudCOMY.Value;

            // Update the record in the database
            mInputDatabase.Replace(mCurrentRecord);
        }

        private void nudRCOMY_ValueChanged(object sender, EventArgs e)
        {
            mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords["CenterOfMassY"].SubRecords[CogFieldNameConstants.Range].Content = (double)nudRCOMY.Value;

            // Update the record in the database
            mInputDatabase.Replace(mCurrentRecord);
        }

        private void cboGrades_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mCurrentRecord != null)
            {
                mCurrentRecord.Params.ExpectedOutputs.Grade.Value = cboGrades.SelectedItem.ToString();

                // Update the record in the database
                mInputDatabase.Replace(mCurrentRecord);
            }
        }

        #endregion
    }
}
