/*******************************************************************************
 Copyright (C) 2007- 2020 Cognex Corporation

 Subject to Cognex Corporations terms and conditions and license agreement,
 you are authorized to use and modify this source code in any way you find
 useful, provided the Software and/or the modified Software is used solely in
 conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
 and agree that Cognex has no warranty, obligations or liability for your use
 of the Software.
*******************************************************************************/

// This sample demonstrates how to access camera specific features of 
// Basler Scout and Basler Ace GigE Vision cameras.  This sample can also be 
// used as a utility to save desired settings as the camera's default, so
// that you will not need to configure the features in your main application.

// Many of the functions normally handled by VisionPro for a Cognex frame grabber,
// such as strobing, must be performed in a camera specific manner with a GigE
// Vision camera.

// This sample is different than most other samples in that it contains
// snippets of code which demonstrate how to perform various functions
// specific to Basler cameras.  Note that you can un-comment the final section
// of code to save the settings in the camera's non-volatile memory.

// Note that the camera specific nature of these features may vary not only
// between different models of camera, but also between firmware versions of
// otherwise identical cameras.  This sample was written to Scout firmware 
// version 3.0 using version 8 of the Scout User's Manual.

// A full list of camera features can be obtained using the GigE Vision 
// Configurator. The Basler Scout or Ace User's Manuals describe feature 
// functionality in detail and are required reading in order to properly use
// the camera features.

// The camera features shown below are:
//
//   - Hardware Trigger Configuration
//   - Strobe Configuration
//   - Data Rate Control
//   - User Set Management


using System;
using System.Collections.Generic;
using System.Text;

using Cognex.VisionPro;
using Cognex.VisionPro.FGGigE;

namespace BaslerAreaScan
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get a reference to a Basler Frame Grabber attached to this PC.
            ICogFrameGrabber frameGrabber = BaslerASConfig.FindFrameGrabber();

            // If no Basler area scan cameras are found, exit the application.
            if (frameGrabber == null)
                return;

            // Some features of a GigE Vision camera are controlled
            // directly by VisionPro interfaces.  These features 
            // (like Exposure) can be initialized and edited by creating
            // a CogAcqFifo object, setting the VisionPro properties to their
            // desired value, and then calling the Fifo's Prepare() function.
            // Exposure is an example of one such feature.

            // Create a CogAcqFifo object for this camera.
            ICogAcqFifo acqFifo = frameGrabber.CreateAcqFifo(
                "Generic GigEVision (Mono)",
                CogAcqFifoPixelFormatConstants.Format8Grey,
                0, false);

            // Set the ExposureTimeAbs to 1000
            BaslerASConfig.ConfigureExposure(acqFifo, 0.00123);

            // Other camera features are not directly controlled by VisionPro
            // interfaces and must be controlled using the OwnedGigEAccess 
            // properties.  The rest of this sample demonstrates examples of
            // these types of features.

            // Get a reference to the GigEAccess interface of the Frame-Grabber.
            ICogGigEAccess gigEAccess = frameGrabber.OwnedGigEAccess;
            if (gigEAccess == null)  // Check for GigE Access support.
                return;  // Exit if no GigE support.

            // Hardware Trigger Configuration
            BaslerASConfig.ConfigureTrigger(gigEAccess, 100);

            // Strobe Configuration
            BaslerASConfig.ConfigureStrobe(gigEAccess, 100, 200);

            // User Set Management... Uncomment next line to enable.
            //BaslerASConfig.SaveUserSet(gigEAccess);

            // Data Rate Control... Uncomment next line to enable.
            //BaslerASConfig.SetBandwidth(gigEAccess, 0.5);

            System.Windows.Forms.MessageBox.Show(
                "Sample Code Execution Complete");

            CogFrameGrabbers frameGrabbers = new CogFrameGrabbers();
            foreach (ICogFrameGrabber fg in frameGrabbers)
              fg.Disconnect(false);

            return;
        }
    }

    /// <summary>
    /// Class which provides GigE Vision camera configuration for 
    /// features which are not directly exposed through the VisionPro
    /// API.
    /// </summary>
    static class BaslerASConfig
    {
        /// <summary>
        /// Configures a GigE camera's exposure using the standard
        /// VisionPro interface.
        /// </summary>
        /// <param name="AcqFifo"></param>
        /// <param name="Exposure"></param>
        public static void ConfigureExposure(ICogAcqFifo acqFifo,
            double exposure)
        {
            // Get a reference to the ExposureParams interface of the AcqFifo.
            ICogAcqExposure exposureParams = acqFifo.OwnedExposureParams;
            // Always check to see an "Owned" property is supported
            // before using it.
            if (exposureParams != null)  // Check for exposure support.
            {
                exposureParams.Exposure = exposure;  // sets ExposureTimeAbs
                acqFifo.Prepare();  // writes the properties to the camera.
            }
        }

        /// <summary>
        /// When using a VisionPro hardware (Auto) trigger model with a 
        /// GigE Vision Camera, some additional camera specific setup may 
        /// be required. The following sets the trigger input line and the 
        /// trigger polarity.  An input line filter is also set.
        /// </summary>
        /// <param name="GigEAccess">The ICogGigEAccess on which to configure
        /// the trigger.</param>
        /// <param name="LineDebouncerTime"></param>
        public static void ConfigureTrigger(ICogGigEAccess gigEAccess,
            double lineDebouncerTime)
        {
            // Setup the trigger features.
            gigEAccess.SetFeature("TriggerSelector", "AcquisitionStart");
            gigEAccess.SetFeature("TriggerSource", "Line1");

            // Setup the trigger edge.
            gigEAccess.SetFeature("TriggerActivation", "RisingEdge");
            // Alternatively you can the trigger activaton to FallingEdge.
            // gigEAccess.SetFeature("TriggerActivation", "FallingEdge");

            // Set the settling time to 100 microseconds.
            gigEAccess.SetFeature("LineSelector", "Line1");
            gigEAccess.SetDoubleFeature("LineDebouncerTimeAbs", lineDebouncerTime);
        }

        /// <summary>
        /// Enable a strobe output on Basler Scout or Basler Ace.  This is a two step
        /// process: (1) A timer is configured to activate when exposure
        /// starts, then (2) the timer is used to generate a pulse on an 
        /// output line.  
        /// Note that the strobe output signal can be turned off by 
        /// configuring the output line as a general purpose output signal.
        /// </summary>
        /// <param name="GigEAccess">The ICogGigEAccess on which to configure
        /// the strobe</param>
        /// <param name="StrobeDelay">Timer delay shifts the beginning of the 
        /// pulse relative to exposure start.
        /// </param>
        ///<param name="StrobeDuration">Timer duration controls the width of
        /// the pulse. 
        /// </param>
        public static void ConfigureStrobe(ICogGigEAccess gigEAccess,
            double strobeDelay, double strobeDuration)
        {
            gigEAccess.SetFeature("TimerSelector", "Timer1");
            gigEAccess.SetFeature("TimerTriggerSource", "ExposureStart");

            // Timer delay shifts the beginning of the pulse relative to 
            // exposure start.
            gigEAccess.SetDoubleFeature("TimerDelayAbs", strobeDelay); //uS

            // Timer duration controls the width of the pulse.
            gigEAccess.SetDoubleFeature("TimerDurationAbs", strobeDuration); //uS

            gigEAccess.SetFeature("LineSelector", "Out1");
            gigEAccess.SetFeature("LineSource", "TimerActive");
        }

        /// <summary>
        /// Configures the current settings as the power-on default for the 
        /// camera, which will be used by VisionPro on start-up.  For 
        /// example, you can save the desired trigger polarity in the camera
        /// using this sample code, instead of setting it explicitly in your
        /// application.

        /// Note that saving the current settings can also affect the 
        /// default values of some Acq FIFO properties such as exposure.
        /// </summary>
        /// <param name="GigEAccess">The ICogGigEAccess on which to save the
        /// current settings</param>
        public static void SaveUserSet(ICogGigEAccess gigEAccess)
        {
            // Save the current settings to the camera's first user set
            gigEAccess.SetFeature("UserSetSelector", "UserSet1");
            gigEAccess.ExecuteCommand("UserSetSave");

            // Choose the user set to use as the power-on default
            gigEAccess.SetFeature("UserSetDefaultSelector", "UserSet1");
        }

        /// <summary>
        /// When using multiple cameras, it is possible that the
        /// total data rate of the cameras will exceed the bandwidth of the 
        /// GigE network.  Acquisition can still be successfully performed
        /// if the data rate of the cameras is reduced to fit within the 
        /// available bandwidth.
        ///
        /// The general idea is that if you have an n camera
        /// application, you set each camera's bandwidth to 1/n and the data
        /// rate will be reduced as needed to allow all cameras to work
        /// simultaneously.  See additional comments on the SetBandwidth
        /// function.
        //
        /// Note that this code will only work if the camera supports
        /// the required GigE Vision registers.  You can discover if these
        /// are supported by looking over the XML description file for
        /// the camera. 
        /// </summary>
        /// <param name="GigEAccess"> The ICogGigEAccess on which to set the
        /// Bandwidth</param>
        /// <param name="PercentageOfBandwidth"> Percentage of total bandwidth
        /// which is to be dedicated to this camera.</param>
        public static void SetBandwidth(ICogGigEAccess gigEAccess,
            double percentageOfBandwidth)
        {
            // 100 MBytes / sec overall throughput
            Double maxRate = 100 * 1024 * 1024;
            uint packetSize = gigEAccess.GetIntegerFeature("GevSCPSPacketSize");
            Double packetTime = packetSize / maxRate;

            // Use the bandwidth setting to calculate the time it should require to
            // transmit each packet to achieve the desired bandwidth.  For example, a
            // bandwidth setting of 0.25 means we want each packet to take 4 times
            // longer than it would at full speed.
            Double desiredTime = packetTime / percentageOfBandwidth;

            // The difference between the desired and actual times is the delay we want
            // between each packet.  Note that until the delay becomes larger than the
            // intrinsic delay between each packet sent by the camera, changes in
            // bandwidth won't have any effect on the data rate.
            Double delaySec = desiredTime - packetTime;

            ulong timeStampFreq = gigEAccess.TimeStampFrequency;
            uint delay = (uint)(delaySec * timeStampFreq);
            gigEAccess.SetIntegerFeature("GevSCPD", delay);
        }

        /// <summary>
        /// Returns a reference to a GigE Vision Basler Scout or Ace
        /// frame grabber object if one is connected to this PC.
        /// </summary>
        /// <returns></returns>
        public static ICogFrameGrabber FindFrameGrabber()
        {
            // Get a reference to a collection of all the GigE Vision cameras
            // found by this system.
            CogFrameGrabberGigEs frameGrabbers = new CogFrameGrabberGigEs();

            // Iterate over all the found cameras looking for a Basler Ace
            // or Basler Scout area scan camera.
            foreach (ICogFrameGrabber fg in frameGrabbers)
            {
                if (fg.Name.Contains("Basler") &&
                    (fg.Name.Contains("ac") ||
                     fg.Name.Contains("sc")))
                {
                    return (fg);
                }
            }
            return null;
        }
    }
}
