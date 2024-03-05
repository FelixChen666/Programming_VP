/*******************************************************************************
 Copyright (C) 2007- 2010 Cognex Corporation

 Subject to Cognex Corporations terms and conditions and license agreement,
 you are authorized to use and modify this source code in any way you find
 useful, provided the Software and/or the modified Software is used solely in
 conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
 and agree that Cognex has no warranty, obligations or liability for your use
 of the Software.
*******************************************************************************/
// Many of the functions normally handled by VisionPro for a Cognex frame 
// grabber, such as encoder control, must be performed in a camera specific 
// manner with a GigE Vision camera.  This sample demonstrates how to use some 
// of the camera specific features of Basler Runner GigE Vision line scan 
// cameras.  Please  refer to the Runner camera manual or user's guide for 
// details on supported camera features.  The GigE Vision Configurator utility
// can be used to list camera feature names and valid feature values.

// This sample can also be used as a utility to save the current settings as
// the camera's default, so that you will not need to configure these features
// in your main application. You can uncomment the final section of code to 
// save the settings in camera memory.

// Note that feature interfaces may vary between different models of cameras
// in the same camera series and also between different firmware versions of
// otherwise identical cameras.  This sample was developed using the Runner 
// User's Manual V4, Runner firmware version 1.0, and Runner XML version 1.3.

// The camera features demonstrated in this sample are:
//
//   - Hardware Trigger Configuration
//   - Encoder Configuration
//   - Data rate Control
//   - User Set Management

using System;
using System.Collections.Generic;
using System.Text;

using Cognex.VisionPro;
using Cognex.VisionPro.FGGigE;

namespace BaslerLineScan
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get a reference to a Basler Frame Grabber attached to this PC.
            ICogFrameGrabber frameGrabber = BaslerLSConfig.FindFrameGrabber();

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
                0,false);

            // Set the Exposure.
            BaslerLSConfig.ConfigureExposure(acqFifo, 0.00002);
       
            // Other camera features are not directly controlled by VisionPro
            // interfaces and must be controlled using the OwnedGigEAccess 
            // properties.  The rest of this sample demonstrates examples of
            // these types of features.

            // Get a reference to the GigEAccess interface of the Frame-Grabber.
            ICogGigEAccess gigEAccess = frameGrabber.OwnedGigEAccess;
            if (gigEAccess == null)  // Check for GigE Access support.
                return;  // Exit if no GigE support.

            // Hardware Trigger Configuration
            BaslerLSConfig.ConfigureTrigger(gigEAccess, 100);

            // Encoder Configuration
            BaslerLSConfig.ConfigureEncoder(gigEAccess);
            
            // User Set Management... Uncomment next line to enable.
            //BaslerLSConfig.SaveUserSet(gigEAccess);

            // Data Rate Control... Uncomment next line to enable.
            //BaslerLSConfig.SetBandwidth(gigEAccess, 0.5);

            System.Windows.Forms.MessageBox.Show(
                "Sample Code Execution Complete");

            CogFrameGrabbers frameGrabbers = new CogFrameGrabbers();
            foreach (ICogFrameGrabber fg in frameGrabbers)
              fg.Disconnect(false);

            return;
        }
    }

    /// <summary>
    /// Class which provides GigEVision camera configuration for 
    /// features which are not directly exposed through the VisionPro
    /// API.
    /// </summary>
    static class BaslerLSConfig
    {
        /// <summary>
        /// Configures a GigE camera's exposure setting using the standard
        /// VisionPro interface.
        /// </summary>
        /// <param name="acqFifo"></param>
        /// <param name="exposure"></param>
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
        /// be required. This function configures the trigger input line and the 
        /// trigger polarity features of a Basler "Runner" camera for use with
        /// the VisionPro Hardware Auto trigger mode.  An input line filter is
        /// also set.
        /// </summary>
        /// <param name="gigEAccess">The ICogGigEAccess on which to configure
        /// the trigger.</param>
        /// <param name="lineDebouncerTime"></param>
        public static void ConfigureTrigger(ICogGigEAccess gigEAccess, 
            double lineDebouncerTime)
        {
            //The following sets the trigger input line and the trigger 
            //polarity.
            gigEAccess.SetFeature("TriggerSelector", "FrameStart");
            gigEAccess.SetFeature("TriggerSource", "Line1");

            // Setup the trigger edge.
            gigEAccess.SetFeature("TriggerActivation", "RisingEdge");
            // Alternatively you can the trigger activaton to FallingEdge.
            //GigEAccess.SetFeature("TriggerActivation", "FallingEdge");

            gigEAccess.SetFeature("LineSelector", "Line1");
            // Since the trigger signal used in this example is TTL, the line
            // termination must be disabled.
            gigEAccess.SetFeature("LineTermination", "false");
            gigEAccess.SetDoubleFeature("LineDebouncerTimeAbs",
                lineDebouncerTime);
        }

        /// <summary>
        /// Configure the encoder to of a Basler "Runner".
        /// </summary>
        /// <param name="gigEAccess">The ICogGigEAccess interface to the 
        /// camera on which to configure the encoder.</param>
        public static void ConfigureEncoder(ICogGigEAccess gigEAccess)
        {
            // Select physical input line 2 as the source signal for the
            // module’s Phase A input and physical input line 3 as the 
            // source signal for the Phase B input
            gigEAccess.SetFeature("ShaftEncoderModuleLineSelector", "PhaseA");
            gigEAccess.SetFeature("ShaftEncoderModuleLineSource", "Line2");
            gigEAccess.SetFeature("ShaftEncoderModuleLineSelector", "PhaseB");
            gigEAccess.SetFeature("ShaftEncoderModuleLineSource", "Line3");

            // Enable line termination for the RS-422 encoder signals
            gigEAccess.SetFeature("LineSelector", "Line2");
            gigEAccess.SetFeature("LineTermination", "true");
            gigEAccess.SetFeature("LineSelector", "Line3");
            gigEAccess.SetFeature("LineTermination", "true");

            // Set the shaft encoder module counter mode
            gigEAccess.SetFeature("ShaftEncoderModuleCounterMode",
                "IgnoreDirection");

            // Enable the camera’s Line Start Trigger function and select 
            // the encoder module’s output as the source signal for the 
            // Line Start Trigger.
            gigEAccess.SetFeature("TriggerSelector", "LineStart");
            gigEAccess.SetFeature("TriggerMode", "On");
            gigEAccess.SetFeature("TriggerSource", "ShaftEncoderModuleOut");
            gigEAccess.SetFeature("TriggerActivation", "RisingEdge");
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
        /// <param name="gigEAccess">The ICogGigEAccess on which to save the
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
        /// <param name="gigEAccess"> The ICogGigEAccess on which to set the
        /// Bandwidth</param>
        /// <param name="percentageOfBandwidth"> Percentage of total bandwidth
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
        /// Returns a reference to a GigE Vision Basler "Runner"
        /// frame grabber object if one is connected to this PC.
        /// </summary>
        /// <returns></returns>
        public static ICogFrameGrabber FindFrameGrabber()
        {
            // Get a reference to a collection of all the GigE Vision cameras
            // found by this system.
            CogFrameGrabberGigEs frameGrabbers = new CogFrameGrabberGigEs();

            // Iterate over all the found cameras looking for a Basler "Runner"
            // line scan camera.
            foreach (ICogFrameGrabber fg in frameGrabbers)
            {
                if (fg.Name.Contains("Basler") &&
                    fg.Name.Contains("ru"))
                {
                    return(fg);
                }
            }
            return null;
        }
    }
}