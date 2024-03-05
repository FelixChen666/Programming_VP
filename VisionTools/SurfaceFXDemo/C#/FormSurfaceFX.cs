//*****************************************************************************
// Copyright (C) 2017 Cognex Corporation
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
//
// This program assumes that you have some knowledge of C# and VisionPro 
// programming.
//
// This sample program demonstrates the programmatic use of the VisionPro 
// SurfaceFX Tool.
//
// The following sample illustrates how to set up a SurfaceFX tool to capture
// images from a stationary part in order to detect surface features. It uses
// a CC24 IO Card, a GigE Card, a GigE Camera and a Strobe Controller
//
// This sample is designed to work with either canned images OR images captured
// live by the user using the software. 
//
// If the user chooses to use canned images, they should use the images provided
// in vision pro installation in the images/SurfaceFX folder.
// 
// If the user chooses to take their own images they will need all of the proper
// equipment including the CC24 IO Card, GigE camera, GigE Card and Strobe Controller
// connected to the IO card based on IO settings in this sample.
//
// The photographed object should remain stationary when acquired. Motion and rotation 
// of the part are not supported in this sample. 
//
// The sample:
//   1) Allows the user to select whether to use image files or acquire from a GigE camera
//   2) Offers the user Vision Tools which are used to either open or capture the images 
//      required for SurfaceFX
//   3) Allows them to tweak the SurfaceFX settings
//   4) Runs surfaceFX and displays the result at the push of a button.
//
// The sample code uses:
//   CogAcqFifoTool   to capture a live image
//   CogImageFileTool to load a saved image
//   CogSurfaceFXTool to detect surface features and provide a result image
//   
// Images are either loaded with the ImageFileTool or captured with the AcqFifoTool.
// Each image must be opened or acquired separately. These imagesare automatically 
// fed to the SurfaceFXTool and the output image is displayed.
//
// The GUI contains the following objects:
//   1) 5 CogRecordDisplay(s):
//     - "Lit From Right" displays the selected image illuminated from the right
//		 - "Lit From Bottom" displays the selected image illuminated from the bottom
//		 - "Lit From Left" displays the selected image illuminated from the left
//		 - "Lit From Top" displays the selected image illuminated from the top
//		 - "Results" displays the output image after processing through SurfaceFX
//
//   2) "Sources" radio buttons (Default is Use Live Display)
//    If Use Live Display is selected, a live image will be captured using CogAcqFifoTool. 
//		If Select Image From File is selected, a canned image will be used instead.
//		Other options ("Choose File" and "Acquire") are enabled or disabled based on
// 		which selection is made by the user.
//
//   3) “Surface FX Tab" Contains all the default controls for a SurfaceFX Tool:
//     In this tab you may configure the brightness, contrast, smoothness and
//		 region shape of the image, as you would in Quick Build.
//
//   4) “Acquire” Buttons:
//    Clicking one of these buttons will open a control panel for the CogAcqFifoTool.
//		Running this tool will trigger the image acquisition. The result will be assigned to 
//		the SurfaceFX tool as one of its inputs and shown on the display above.
//
//   5) “Choose” Buttons:
//    Clicking one of these buttons will open a control panel for the CogImageFileTool.
// 		Upon selecting an image and running this tool, the image will be assigned to one of
//    the SurfaceFX Inputs and shown on the display above.
//
//  6) “Run” Button:
//    If all of the inputs have been set, then this button will run SurfaceFX with whatever
//    configuration you have set and provide an output image of the object's surface features.
// 
///////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using Cognex.VisionPro;
using Cognex.VisionPro.FGGigE;
using Cognex.VisionPro.Comm;
using Cognex.VisionPro.SurfaceFX;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.Exceptions;

namespace AcqFifoDemo
{
    public partial class FormSurfaceFX : Form
    {

        //Light Options
        private int pulseMilliseconds = 1000;


        // FIFO
        private CogAcqFifoTool acqFifoTool;
        private AcqFifoForm acqFifoForm;

        // Comm Card
        CogCommCards commCardCollection;
        CogCommCard commCard;
        CogPrio mPrio;

        // Checkboxes
        private CheckBox[] checkboxes = new CheckBox[4];

        // SurfaceFX
        CogSurfaceFXTool surfaceFXTool;

        // Image File
        CogImageFileTool ImageFileTool;
        ImageFileDisplayForm imageForm;


        // Delegate Types
        delegate void VoidDelegate();
        delegate void IntDelegate(int value);




        public FormSurfaceFX()
        {
            InitializeComponent();
            checkboxes[0] = checkBox_LitFromRight;
            checkboxes[1] = checkBox_LitFromBottom;
            checkboxes[2] = checkBox_LitFromLeft;
            checkboxes[3] = checkBox_LitFromTop;
            InitializeFifo();
            InitializeComm();
            InitializeSurfaceFX();
            InitializeImageFile();
        }

        #region "Initialization"

        /// <summary>
        /// Initialize Comm Card
        /// </summary>

        private void InitializeComm()
        {
            commCardCollection = new CogCommCards();

            // Make sure we have at least one accessable comm card and set up
            // that comm card for precision IO.
            // Also add pulse events to control the timing of the light pulses.
            if (commCardCollection.Count > 0)
            {
                commCard = commCardCollection[0];
                mPrio = commCard.DiscreteIOAccess.CreatePrecisionIO();

                setupLightPulseEvents();
            }

          
            else
            {
                // Display an error status
                stlStatus.Text = "Error: No IO Card Detected. Please install and configure your Comm card.";
            }
        }

        /// <summary>
        /// Setup Pulse Light Events
        /// There should be one event for each output used
        /// </summary>

        private void setupLightPulseEvents()
        {
            CogPrioEventCollection prioEvents = new CogPrioEventCollection();
            if (mPrio != null)
            {
                mPrio.DisableEvents();

                // For each of our outputs, we are going to need an event that pulses the 
                // light. 
                //
                // Pulses are activated when the acquisition tool starts running, without a delay.
                // The strobe controller must be configured to respond to the positive edge of the pulse or your
                // strobe will be delivered out of sync with the camera acquisition and result in a poor quality image.

                for (int i = 0; i <= 4; i++)
                {
                    prioEvents.Add(createImmediatePulse(i));
                }

                // This call clears out the event queue before we replace it with a new one
                mPrio.Events.Clear();
                mPrio.Events = prioEvents;

                if (!mPrio.Valid)
                {
                    MessageBox.Show("Event Collection Not Valid.");
                }
                else
                {
                    mPrio.EnableEvents();
                }
            }
            else
            {
                // Display an error status
                stlStatus.Text = "Error: Could not create IO Events.";
            }


        }

        /// <summary>
        /// Initialize FIFO
        /// </summary>
        private void InitializeFifo()
        {
            // Setup AcqFifo queue
            acqFifoTool = new CogAcqFifoTool();
        }

        /// <summary>
        /// Event that schedules pulses to turn on the light
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void acqFifoTool_LitFromRight_Running(object sender, EventArgs e)
        {
            if (commCardCollection.Count > 0 && commCard != null && mPrio != null)
            {
              mPrio.Events[3].Schedule();
            }
            else
            {
              MessageBox.Show("No Comm Card is detected. This sample requires a Cognex CC24 Communications Card.");
            }
            
        }

        /// <summary>
        /// Event handler that schedules pulses to turn on the light.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void acqFifoTool_LitFromBottom_Running(object sender, EventArgs e)
        {
            if (commCardCollection.Count > 0 && commCard != null && mPrio != null)
            {
              mPrio.Events[2].Schedule();
            }
            else
            {
              MessageBox.Show("No Comm Card is detected. This sample requires a Cognex CC24 Communications Card.");
            }
        }

        /// <summary>
        /// Event handler that triggers pulses to turn on the light.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void acqFifoTool_LitFromLeft_Running(object sender, EventArgs e)
        {
            if (commCardCollection.Count > 0 && commCard != null && mPrio != null)
            {
              mPrio.Events[1].Schedule();
            }
            else
            {
              MessageBox.Show("No Comm Card is detected. This sample requires a Cognex CC24 Communications Card.");
            }
        }       

        /// <summary>
        /// Event handler that triggers pulses to turn on the light
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void acqFifoTool_LitFromTop_Running(object sender, EventArgs e)
        {
          if (commCardCollection.Count > 0 && commCard != null && mPrio != null)
          {
            mPrio.Events[0].Schedule();
          }
          else
          {
            MessageBox.Show("No Comm Card is detected. This sample requires a Cognex CC24 Communications Card.");
          }
        }

        /// <summary>
        /// Event handler that displays an image acquired by the user via a CogAcqFifoTool 
        /// and sets it to the "LitFromRightInputImage" of the surface FX tool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void acqFifoTool_LitFromRight_Ran(object sender, EventArgs e)
        {
          if (acqFifoTool.OutputImage != null)
          {
            cogDisplay_LitFromRight.Image = acqFifoTool.OutputImage;
            surfaceFXTool.LitFromRightInputImage = acqFifoTool.OutputImage;
            // Light this checkbox to indicate that the image has been acquired
            checkCheckbox(0);
          }
        }

        /// <summary>
        /// Event handler that displays an image acquired by the user via a CogAcqFifoTool 
        /// and sets it to the "LitFromBottomInputImage" of the surface FX tool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void acqFifoTool_LitFromBottom_Ran(object sender, EventArgs e)
        {
          if (acqFifoTool.OutputImage != null)
          {
            cogDisplay_LitFromBottom.Image = acqFifoTool.OutputImage;
            surfaceFXTool.LitFromBottomInputImage = acqFifoTool.OutputImage;
            // Light this checkbox to indicate that the image has been acquired
            checkCheckbox(1);
          }
        }

        /// <summary>
        /// Event handler that displays an image acquired by the user via a CogAcqFifoTool 
        /// and sets it to the "LitFromLeftInputImage" of the surface FX tool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void acqFifoTool_LitFromLeft_Ran(object sender, EventArgs e)
        {
          if (acqFifoTool.OutputImage != null)
          {
            cogDisplay_LitFromLeft.Image = acqFifoTool.OutputImage;
            surfaceFXTool.LitFromLeftInputImage = acqFifoTool.OutputImage;
            // Light this checkbox to indicate that the image has been acquired
            checkCheckbox(2);
          }
        }

        /// <summary>
        /// Event handler that displays an image acquired by the user via a CogAcqFifoTool 
        /// and sets it to the "LitFromTopInputImage" of the surface FX tool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void acqFifoTool_LitFromTop_Ran(object sender, EventArgs e)
        {
          if (acqFifoTool.OutputImage != null)
          {
            cogDisplay_LitFromTop.Image = acqFifoTool.OutputImage;
            surfaceFXTool.LitFromTopInputImage = acqFifoTool.OutputImage;
            // Light this checkbox to indicate that the image has been acquired
            checkCheckbox(3);
          }
        }


        /// <summary>
        /// Initialize the SurfaceFX Tool
        /// </summary>

        private void InitializeSurfaceFX()
        {
            surfaceFXTool = new CogSurfaceFXTool();
            cogSurfaceFXEditV21.Subject = surfaceFXTool;
        }

        /// <summary>
        /// Creates a single image file tool to handle opening image files for canned images
        /// </summary>
        private void InitializeImageFile()
        {
            // Create the image file tool
            ImageFileTool = new CogImageFileTool();

        }

        /// <summary>
        /// Event handler that displays the user selected image and then sets it as the 
        /// LitFromRightInputImage for the SurfaceFX tool 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImageFileTool_LitFromRight_Ran(object sender, EventArgs e)
        {
            cogDisplay_LitFromRight.Image = ImageFileTool.OutputImage;
            surfaceFXTool.LitFromRightInputImage = ImageFileTool.OutputImage;
            // Light this checkbox to indicate that the image file has been opened and assigned
            checkCheckbox(0);
        }

        /// <summary>
        /// Event handler that displays the user selected image and then sets it as the 
        /// LitFromBottomInputImage for the SurfaceFX tool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImageFileTool_LitFromBottom_Ran(object sender, EventArgs e)
        {
            cogDisplay_LitFromBottom.Image = ImageFileTool.OutputImage;
            surfaceFXTool.LitFromBottomInputImage = ImageFileTool.OutputImage;
            // Light this checkbox to indicate that the image file has been opened and assigned
            checkCheckbox(1);
        }


        /// <summary>
        /// Event handler that displays the user selected image and then sets it as the 
        /// LitFromLeftInputImage for the SurfaceFX tool 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImageFileTool_LitFromLeft_Ran(object sender, EventArgs e)
        {
            cogDisplay_LitFromLeft.Image = ImageFileTool.OutputImage;
            surfaceFXTool.LitFromLeftInputImage = ImageFileTool.OutputImage;
            // Light this checkbox to indicate that the image file has been opened and assigned
            checkCheckbox(2);
        }

        /// <summary>
        /// Event handler that displays the user selected image and then sets it as the 
        /// LitFromTopInputImage for the SurfaceFX tool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImageFileTool_LitFromTop_Ran(object sender, EventArgs e)
        {
            cogDisplay_LitFromTop.Image = ImageFileTool.OutputImage;
            surfaceFXTool.LitFromTopInputImage = ImageFileTool.OutputImage;
            // Light this checkbox to indicate that the image file has been opened and assigned
            checkCheckbox(3);
        }

        #endregion

        #region "IO"

        /// <summary>
        /// Creates a CogPrioEvent that schedules a pulse on a given output without a delay.
        /// The length of the pulse is based on the variable "pulseMilliseconds" which you can
        /// set to whatever you like.
        /// </summary>
        /// <param name="output">The CogPrioEvent created.</param>
        /// <returns></returns>
        private CogPrioEvent createImmediatePulse(int output)
        {
            double delay = 0;
            CogPrioEvent pulseOutput = new CogPrioEvent();
            pulseOutput.Name = "Enable_Output_Immedate_" + output.ToString();
            CogPrioEventResponseLine response = new CogPrioEventResponseLine(CogPrioBankConstants.OutputBank0, output, CogPrioOutputLineValueConstants.SetHigh, pulseMilliseconds, CogPrioDelayTypeConstants.Time, delay);
            pulseOutput.ResponsesLine.Add(response);
            return pulseOutput;
        }

        #endregion

       
        #region "User Interface Methods"


        /// <summary>
        /// Utility function to check or uncheck a given checkbox.
        /// Checkbox must be in the "checkboxes" list.
        /// </summary>
        /// <param name="index"></param>

        private void checkCheckbox(int index)
        {
            // The actual checking of the checkbox must be done on the UI thread.
            if (checkboxes[index].InvokeRequired)
            {
                IntDelegate d = new IntDelegate(checkCheckbox);
                Invoke(d, new object[] { index });
            }
            else
            {
                if (index < checkboxes.Length)
                {
                    checkboxes[index].Checked = true;
                }
            }
        }


        /// <summary>
        ///  Resets each checkbox in the "checkboxes" list to false.
        /// </summary>
        private void resetcheckboxes()
        {
            // The actual checking of the checkboxes must be done in the UI thread.
            if (checkboxes[0].InvokeRequired)
            {
                VoidDelegate d = new VoidDelegate(resetcheckboxes);
                Invoke(d);
            }
            else
            {
                foreach (CheckBox box in checkboxes)
                {
                    box.Checked = false;
                }
            }


        }


        /// <summary>
        /// When radioButton_Acquire is checked, it will disable all of the buttons that open
        /// a CogImageFileTool and enable the buttons that open a CogAcqFifoTool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Acquire_CheckedChanged(object sender, EventArgs e)
        {
            // Only do it if the button is now checked
            if (radioButton_Acquire.Checked)
            {
                btnAcquire_LitFromRight.Enabled = false;
                btnAcquire_LitFromBottom.Enabled = false;
                btnAcquire_LitFromLeft.Enabled = false;
                btnAcquire_LitFromTop.Enabled = false;
                // Don't enable a given button if the form has not been disposed
                // unless it is null (because they start null, a new form is created
                // each time the Choose button is clicked)
                if (null == imageForm || imageForm.IsDisposed)
                {
                    btnChoose_LitFromRight.Enabled = true;
                    btnChoose_LitFromBottom.Enabled = true;
                    btnChoose_LitFromLeft.Enabled = true;
                    btnChoose_LitFromTop.Enabled = true;
                }
                
            }
        }

        /// <summary>
        /// When radioButton1 is checked, it will disable all of the buttons that open
        /// a CogAcqFifoTool and enable the buttons that open a CogImageFileTool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_ImageFiles_CheckedChanged(object sender, EventArgs e)
        {
            // Only do it if the radio button is now checked
            if (radioButton_ImageFiles.Checked)
            {
                btnChoose_LitFromRight.Enabled = false;
                btnChoose_LitFromBottom.Enabled = false;
                btnChoose_LitFromLeft.Enabled = false;
                btnChoose_LitFromTop.Enabled = false;
                // Don't enable a given button if the form has not been disposed
                // unless it is null (because they start null, a new form is created
                // each time the Acquire button is clicked)
                if (null == acqFifoForm || acqFifoForm.IsDisposed)
                {
                    btnAcquire_LitFromRight.Enabled = true;
                    btnAcquire_LitFromBottom.Enabled = true;
                    btnAcquire_LitFromLeft.Enabled = true;
                    btnAcquire_LitFromTop.Enabled = true;
                }
            }
        }

        //
        // Image Chooser Buttons
        //


        /// <summary>
        /// Enables all "Choose" buttons for image file selection and enables radio buttons to allow
        /// the user to switch between AcqFifo or Image File methods of selecting images. Designed to
        /// be used as a callback when a form closes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enableBtnChoose_All(object sender, FormClosingEventArgs e)
        {
            btnChoose_LitFromRight.Enabled = true;
            btnChoose_LitFromLeft.Enabled = true;
            btnChoose_LitFromTop.Enabled = true;
            btnChoose_LitFromBottom.Enabled = true;
            radioButton_Acquire.Enabled = true;
            radioButton_ImageFiles.Enabled = true;

        }

        /// <summary>
        /// Disables all "Choose" buttons and disables the radio buttons to allow the user to switch 
        /// between AcqFifo and Image File methods of selecting images. Designed to be called when a
        /// form is opened to prevent opening multiple file selection forms or changing modes while one
        /// of these panels is open.
        /// </summary>
        private void disableBtnChoose_All()
        {
            btnChoose_LitFromRight.Enabled = false;
            btnChoose_LitFromLeft.Enabled = false;
            btnChoose_LitFromTop.Enabled = false;
            btnChoose_LitFromBottom.Enabled = false;
            radioButton_Acquire.Enabled = false;
            radioButton_ImageFiles.Enabled = false;
        }


        /// <summary>
        /// When clicked, this button creates a new ImageFileDisplayForm. The code for this form is included
        /// in the sample. It contains an ImageFileEditV2 Control which is exposed so we can set its subject.
        /// Disables all "Choose" buttons as well as the radio buttons that switch between file selection and 
        /// AcqFifo. These are re enabled upon closing the form by adding the event handler "enableBtnChoose_All"
        /// to the FormClosing event for the ImageForm. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChoose_LitFromRight_Click(object sender, EventArgs e)
        {
            // Disable "Choose" buttons as well as the radio buttons that switch between file selection
            // and AcqFifo
            disableBtnChoose_All();
            imageForm = new ImageFileDisplayForm();
            imageForm.cogImageFileEditV21.Subject = ImageFileTool;
            // Called when the tool completes to set the resulting image in the right variable for SurfaceFX
            // and display the image in the correct display.
            ImageFileTool.Ran += ImageFileTool_LitFromRight_Ran;
            // Enable all the "Choose" buttons and radio buttons when the form closes
            imageForm.FormClosing += enableBtnChoose_All;
            // When the form closes, we need to remove the event handlers from  the ImageFileTool
            imageForm.FormClosing += removeRightImageFileEventHandlersFromTool;
            imageForm.Show();

        }

        /// <summary>
        /// Removes an event handler from the ImageFileTool's ran event because we need to re-use the ImageFileTool and
        /// we leaving the event handler subscribed to this event will cause our image files to be stored incorrectly for 
        /// SurfaceFX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeRightImageFileEventHandlersFromTool(object sender, FormClosingEventArgs e)
        {
            ImageFileTool.Ran -= ImageFileTool_LitFromRight_Ran;
        }

        /// <summary>
        /// When clicked, this button creates a new ImageFileDisplayForm. The code for this form is included
        /// in the sample. It contains an ImageFileEditV2 Control which is exposed so we can set its subject.
        /// Disables all "Choose" buttons as well as the radio buttons that switch between file selection and 
        /// AcqFifo. These are re enabled upon closing the form by adding the event handler "enableBtnChoose_All"
        /// to the FormClosing event for the ImageForm. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChoose_LitFromBottom_Click(object sender, EventArgs e)
        {
            // Disable "Choose" buttons as well as the radio buttons that switch between file selection
            // and AcqFifo
            disableBtnChoose_All();
            imageForm = new ImageFileDisplayForm();
            imageForm.cogImageFileEditV21.Subject = ImageFileTool;
            // Called when the tool completes to set the resulting image in the right variable for SurfaceFX
            // and display the image in the correct display.
            ImageFileTool.Ran += ImageFileTool_LitFromBottom_Ran;
            // Enable all the "Choose" buttons and radio buttons when the form closes
            imageForm.FormClosing += enableBtnChoose_All;
            imageForm.FormClosing += removeBottomImageFileEventHandlersFromTool;
            imageForm.Show();
        }

        /// <summary>
        /// Removes an event handler from the ImageFileTool's ran event because we need to re-use the ImageFileTool and
        /// we leaving the event handler subscribed to this event will cause our image files to be stored incorrectly for 
        /// SurfaceFX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeBottomImageFileEventHandlersFromTool(object sender, FormClosingEventArgs e)
        {
            ImageFileTool.Ran -= ImageFileTool_LitFromBottom_Ran;
        }

        /// <summary>
        /// When clicked, this button creates a new ImageFileDisplayForm. The code for this form is included
        /// in the sample. It contains an ImageFileEditV2 Control which is exposed so we can set its subject.
        /// Disables all "Choose" buttons as well as the radio buttons that switch between file selection and 
        /// AcqFifo. These are re enabled upon closing the form by adding the event handler "enableBtnChoose_All"
        /// to the FormClosing event for the ImageForm. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChoose_LitFromLeft_Click(object sender, EventArgs e)
        {
            // Disable "Choose" buttons as well as the radio buttons that switch between file selection
            // and AcqFifo
            disableBtnChoose_All();
            imageForm = new ImageFileDisplayForm();
            imageForm.cogImageFileEditV21.Subject = ImageFileTool;
            // Called when the tool completes to set the resulting image in the right variable for SurfaceFX
            // and display the image in the correct display.
            ImageFileTool.Ran += ImageFileTool_LitFromLeft_Ran;
            // Enable all the "Choose" buttons and radio buttons when the form closes
            imageForm.FormClosing += enableBtnChoose_All;
            imageForm.FormClosing += removeLeftIamgeFileEventHandlersFromTool;
            imageForm.Show();
        }

        /// <summary>
        /// Removes an event handler from the ImageFileTool's ran event because we need to re-use the ImageFileTool and
        /// we leaving the event handler subscribed to this event will cause our image files to be stored incorrectly for 
        /// SurfaceFX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeLeftIamgeFileEventHandlersFromTool(object sender, FormClosingEventArgs e)
        {
            ImageFileTool.Ran -= ImageFileTool_LitFromLeft_Ran;
        }

        /// <summary>
        /// When clicked, this button creates a new ImageFileDisplayForm. The code for this form is included
        /// in the sample. It contains an ImageFileEditV2 Control which is exposed so we can set its subject.
        /// Disables all "Choose" buttons as well as the radio buttons that switch between file selection and 
        /// AcqFifo. These are re enabled upon closing the form by adding the event handler "enableBtnChoose_All"
        /// to the FormClosing event for the ImageForm. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChoose_LitFromTop_Click(object sender, EventArgs e)
        {
            // Disable "Choose" buttons as well as the radio buttons that switch between file selection
            // and AcqFifo
            disableBtnChoose_All();
            imageForm = new ImageFileDisplayForm();
            imageForm.cogImageFileEditV21.Subject = ImageFileTool;
            // Called when the tool completes to set the resulting image in the right variable for SurfaceFX
            // and display the image in the correct display.
            ImageFileTool.Ran += ImageFileTool_LitFromTop_Ran;
            // Enable all the "Choose" buttons and radio buttons when the form closes
            imageForm.FormClosing += enableBtnChoose_All;
            imageForm.FormClosing += removeTopImageFileEventHandlersFromTool;
            imageForm.Show();
        }

        /// <summary>
        /// Removes an event handler from the ImageFileTool's ran event because we need to re-use the ImageFileTool and
        /// we leaving the event handler subscribed to this event will cause our image files to be stored incorrectly for 
        /// SurfaceFX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeTopImageFileEventHandlersFromTool(object sender, FormClosingEventArgs e)
        {
            ImageFileTool.Ran -= ImageFileTool_LitFromTop_Ran;
        }

        /// <summary>
        /// Runs the surface FX tool with the currently selected settings and 
        /// displays the resulting image in a cog records display.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRunSurfaceFX_Click(object sender, EventArgs e)
        {
            surfaceFXTool.Run();
            stlStatus.Text = surfaceFXTool.RunStatus.ToString();
            cogRecordsDisplay1.Display.StopLiveDisplay();
            cogRecordsDisplay1.Display.Image = surfaceFXTool.OutputImage;
            cogRecordsDisplay1.Display.AutoFit = true;
        }

        /// <summary>
        /// Event handler used to re-enable the acquire buttons after the acquisition form has closed. Also
        /// enables the radio buttons that allow switching between AcqFifo and ImageFile mode. 
        /// Designed to be called by the Aquisition form's FormClosing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enableBtnAcquire_All(object sender, FormClosingEventArgs e)
        {
            btnAcquire_LitFromRight.Enabled = true;
            btnAcquire_LitFromLeft.Enabled = true;
            btnAcquire_LitFromTop.Enabled = true;
            btnAcquire_LitFromBottom.Enabled = true;
            radioButton_Acquire.Enabled = true;
            radioButton_ImageFiles.Enabled = true;
        }

        /// <summary>
        /// Disables all "Acquire" buttons, as well as the radio buttons that switch between
        /// AcqFifo mode and image file selection mode. Designed to be called whenever a 
        /// AcqFifo panel is opened so that no others can be opened simultaneously and the mode
        /// can not be switched while a panel is open.
        /// </summary>
        private void disableBtnAcquire_All()
        {
            disableButton(btnAcquire_LitFromRight);
            disableButton(btnAcquire_LitFromLeft);
            disableButton(btnAcquire_LitFromTop);
            disableButton(btnAcquire_LitFromBottom);
            radioButton_Acquire.Enabled = false;
            radioButton_ImageFiles.Enabled = false;
        }

        /// <summary>
        /// Opens up a CogAcqFifoTool in a form so that the user can get an image from the frame grabber.
        /// Disagbles all "Acquire" buttons as well as the radio buttons that change modes so that none of these
        /// can be clicked on while this form is open.
        /// Additionally, sets up the Ran and Running event for the acqFifoTool so that the correct IO events
        /// are triggered for capturing the image.
        /// When the form closes we want to remove these event handlers, so we also add an event handler to 
        /// the AcqFifoForm's FormClosing event so that they aren't called when the user clicks a different button.
        /// The disabled buttons will also be enabled at this time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAcquire_LitFromRight_Click(object sender, EventArgs e)
        {
            // Disable all "Acquire" buttons as well as the radio buttons that switch modes
            disableBtnAcquire_All();
            acqFifoForm = new AcqFifoForm();
            acqFifoForm.setSubject(acqFifoTool); 
            // Event Handlers to tell the acqFifo tool which outputs to pulse when the tool runs and what to do with
            // the captured image when it completes
            acqFifoTool.Ran += acqFifoTool_LitFromRight_Ran;
            acqFifoTool.Running += acqFifoTool_LitFromRight_Running;
            // Event Handler to enable buttons again when the form closes
            acqFifoForm.FormClosing += enableBtnAcquire_All;
            // Event Handler to remove the Ran and Running event handlers from the acqFifo tool when the form closes so it can be re-used.
            acqFifoForm.FormClosing += removeRightAcqFifoEventHandlersFromTool;
            
            acqFifoForm.Show();
        }

        /// <summary>
        /// Remove event handlers from the AcqFifoTool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeRightAcqFifoEventHandlersFromTool(object sender, FormClosingEventArgs e)
        {
            acqFifoTool.Ran -= acqFifoTool_LitFromRight_Ran;
            acqFifoTool.Running -= acqFifoTool_LitFromRight_Running;
        }

        /// <summary>
        /// Opens up a CogAcqFifoTool in a form so that the user can get an image from the frame grabber.
        /// Disagbles all "Acquire" buttons as well as the radio buttons that change modes so that none of these
        /// can be clicked on while this form is open.
        /// Additionally, sets up the Ran and Running event for the acqFifoTool so that the correct IO events
        /// are triggered for capturing the image.
        /// When the form closes we want to remove these event handlers, so we also add an event handler to 
        /// the AcqFifoForm's FormClosing event so that they aren't called when the user clicks a different button.
        /// The disabled buttons will also be enabled at this time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAcquire_LitFromBottom_Click(object sender, EventArgs e)
        {
            // Disable all "Acquire" buttons as well as the radio buttons that switch modes
            disableBtnAcquire_All();
            acqFifoForm = new AcqFifoForm();
            acqFifoForm.setSubject(acqFifoTool);
            // Event handlers to tell the acqFifo tool which outputs to pulse when the tool runs and what to do with
            // the captured image when it completes
            acqFifoTool.Ran += acqFifoTool_LitFromBottom_Ran;
            acqFifoTool.Running += acqFifoTool_LitFromBottom_Running;
            // Event handler to enable buttons again when the form closes
            acqFifoForm.FormClosing += enableBtnAcquire_All;
            // Event handler to remove the Ran and Running event handlers from the acqFifo tool when the form closes so it can be re-used.
            acqFifoForm.FormClosing += removeBottomAcqFifoEventHandlersFromTool;
            
            acqFifoForm.Show();
        }

        /// <summary>
        /// Remove event handlers from the AcqFifoTool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void removeBottomAcqFifoEventHandlersFromTool(object sender, FormClosingEventArgs e)
        {
            acqFifoTool.Ran -= acqFifoTool_LitFromBottom_Ran;
            acqFifoTool.Running -= acqFifoTool_LitFromBottom_Running;
        }

        /// <summary>
        /// Opens up a CogAcqFifoTool in a form so that the user can get an image from the frame grabber.
        /// Disagbles all "Acquire" buttons as well as the radio buttons that change modes so that none of these
        /// can be clicked on while this form is open.
        /// Additionally, sets up the Ran and Running event for the acqFifoTool so that the correct IO events
        /// are triggered for capturing the image.
        /// When the form closes we want to remove these event handlers, so we also add an event handler to 
        /// the AcqFifoForm's FormClosing event so that they aren't called when the user clicks a different button.
        /// The disabled buttons will also be enabled at this time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAcquire_LitFromLeft_Click(object sender, EventArgs e)
        {
            // Disable all "Acquire" buttons as well as the radio buttons that switch modes
            disableBtnAcquire_All();
            acqFifoForm = new AcqFifoForm();
            acqFifoForm.setSubject(acqFifoTool);
            // Event handlers to tell the acqFifo tool which outputs to pulse when the tool runs and what to do with
            // the captured image when it completes
            acqFifoTool.Ran += acqFifoTool_LitFromLeft_Ran;
            acqFifoTool.Running += acqFifoTool_LitFromLeft_Running;
            // Event handler to enable buttons again when the form closes
            acqFifoForm.FormClosing += enableBtnAcquire_All;
            // Event handler to remove the Ran and Running event handlers from the acqFifo tool when the form closes so it can be re-used.
            acqFifoForm.FormClosing += removeLeftAcqFifoEventHandlersFromTool;
            
            acqFifoForm.Show();
        }

        /// <summary>
        /// Remove event handlers from the AcqFifoTool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeLeftAcqFifoEventHandlersFromTool(object sender, FormClosingEventArgs e)
        {
            acqFifoTool.Ran -= acqFifoTool_LitFromLeft_Ran;
            acqFifoTool.Running -= acqFifoTool_LitFromLeft_Running;
        }

        /// <summary>
        /// Opens up a CogAcqFifoTool in a form so that the user can get an image from the frame grabber.
        /// Disagbles all "Acquire" buttons as well as the radio buttons that change modes so that none of these
        /// can be clicked on while this form is open.
        /// Additionally, sets up the Ran and Running event for the acqFifoTool so that the correct IO events
        /// are triggered for capturing the image.
        /// When the form closes we want to remove these event handlers, so we also add an event handler to 
        /// the AcqFifoForm's FormClosing event so that they aren't called when the user clicks a different button.
        /// The disabled buttons will also be enabled at this time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAcquire_LitFromTop_Click(object sender, EventArgs e)
        {
            // Disable all "Acquire" buttons as well as the radio buttons that switch modes
            disableBtnAcquire_All();
            acqFifoForm = new AcqFifoForm();
            acqFifoForm.setSubject(acqFifoTool);
            // Event handlers to tell the acqFifo tool which outputs to pulse when the tool runs and what to do with
            // the captured image when it completes
            acqFifoTool.Ran += acqFifoTool_LitFromTop_Ran;
            acqFifoTool.Running += acqFifoTool_LitFromTop_Running;
            // Event handler to enable buttons again when the form closes
            acqFifoForm.FormClosing += enableBtnAcquire_All;
            // Event handler to remove the Ran and Running event handlers from the acqFifo tool when the form closes so it can be re-used.
            acqFifoForm.FormClosing += removeTopAcqFifoEventHandlersFromTool;
            
            acqFifoForm.Show();
        }

        /// <summary>
        /// Remove event handlers from the AcqFifoTool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeTopAcqFifoEventHandlersFromTool(object sender, FormClosingEventArgs e)
        {
            acqFifoTool.Ran -= acqFifoTool_LitFromTop_Ran;
            acqFifoTool.Running -= acqFifoTool_LitFromTop_Running;
        }

        

        /// <summary>
        /// Disables whatever button is passed to it.
        /// </summary>
        /// <param name="b"></param>
        protected void disableButton(Button b)
        {
            b.Enabled = false;
        }

        #endregion

        #region "Shutdown"

        /// <summary>
        /// Called when the program is closed to gracefully shut down the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           

            CogFrameGrabbers grabbers = new CogFrameGrabbers();
            foreach(ICogFrameGrabber aGrabber in grabbers)
            {
                aGrabber.Disconnect(false);
            }

          if (null != imageForm && !imageForm.IsDisposed)
          {
            imageForm.Hide();
          }
          if (null != acqFifoForm && !acqFifoForm.IsDisposed)
          {
            acqFifoForm.Hide();
          }



        }

        #endregion









    }
}
