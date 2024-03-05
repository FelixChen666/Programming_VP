/*******************************************************************************
 Copyright (C) 2014 Cognex Corporation

 Subject to Cognex Corporations terms and conditions and license agreement,
 you are authorized to use and modify this source code in any way you find
 useful, provided the Software and/or the modified Software is used solely in
 conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
 and agree that Cognex has no warranty, obligations or liability for your use
 of the Software.
*******************************************************************************
This sample program is designed to illustrate certain VisionPro features or 
techniques in the simplest way possible. It is not intended as the framework 
for a complete application. In particular, the sample program may not provide
proper error handling, event handling, cleanup, repeatability, and other 
mechanisms that a commercial quality application requires.
 
This sample demonstrates the use of the Cognex Communication Card's Network
Data Model (NDM) API. The sample shows how to:
  1. Use the CogEthernetPort class call to configure the Cognex Communication
     Card's IP address.
  2. Use the CogNdm class to connect to a third party PLC running an Ethernet
      based Factory Floor Protocol such as EtherNet/IP or PROFINET.
  3. Use the CogNdm class to receive NDM signal events from a PLC.
  4. Use the CogNdm class to send NDM notification signals to the PLC.
 
The application prints each signal event to a log as they arrive from the PLC.
The application provides buttons that that send data to the PLC when clicked.

In a real vision application you would insert your code into the NDM 
event handlers to process PLC signals, you would also call the appropriate 
NDM notification methods to signal the PLC with the current state of the vision
system.
  
This sample expects that you have a compatible PLC connected to the same LAN
as the Communication Card.
  
For more information about using the Communication Card with a PLC refer to the
VisionPro documentation.
*/

using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net; // Needed for IPAddress

using Cognex.VisionPro;
using Cognex.VisionPro.Comm;

namespace BasicFactoryFloorProtocol
{
  public partial class Form1 : Form
  {

    #region Member Variables

    // Holds a reference to the Comm Card Collection.
    private CogCommCards mCards;

    // Holds a reference to the Comm Card.
    private CogCommCard mCard;

    // Hold a reference to the Comm Card's NDM interface.
    private CogNdm mNdm;

    // Hold a reference to the Comm
    private CogEthernetPort mEthernetPort;

    // Hold are refernce to the Ethernet port settings.
    private CogEthernetPortSettings mEthernetPortSettings;

    // Used to time out when first trying to connect to the PLC.
    private System.Timers.Timer waitForProtocol;

    #endregion

    #region Form Constructor

    public Form1()
    {
      InitializeComponent();
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Initializes the comm card for this sample
    /// </summary>
    private bool InitCommCard()
    {
      // Enumerate all the Comm Cards in the system.
      mCards = new CogCommCards();

      if (mCards.Count == 0)
      {
        System.Windows.Forms.MessageBox.Show(
          "Sample requires a Comm Card 24A or Comm Card 24C.");
        return false;
      }
      // Use the 0th Comm Card in this sample.
      mCard = mCards[0];

      // Does the Comm Card support Factory Floor Protocols?
      if (mCard.FfpAccess == null)
      {
        System.Windows.Forms.MessageBox.Show(
          "Sample requires a Comm Card with FFP support.");
        return false;
      }

      // Create a software object to interact with Ethernet port.
      mEthernetPort = mCard.EthernetPortAccess.CreateEthernetPort(0);

      // Read the settings stored in Comm Card flash.
      mEthernetPortSettings = mEthernetPort.ReadSettings();

      // Update the GUI with tthe flash settings
      this.txt_IPAddress.Text = mEthernetPortSettings.IPAddress.ToString();
      this.txt_Subnet.Text = mEthernetPortSettings.SubnetMask.ToString();
      this.txt_HostName.Text = mEthernetPortSettings.HostName;
      this.ckb_DHCP.Checked = mEthernetPortSettings.DHCPEnable;

      // Read the active settings on the card.
      if (mEthernetPort.IsInterfaceUp)
      {
        CogEthernetPortSettings eActiveSettings = 
          mEthernetPort.ReadActiveSettings();

        this.txt_ActiveIP.Text = eActiveSettings.IPAddress.ToString();
        this.txt_ActiveSubnet.Text = eActiveSettings.SubnetMask.ToString();
        this.txt_HostName.Text = eActiveSettings.HostName.ToString();
      }

      return true;
    }

    /// <summary>
    /// Sign up for NDM the signal events.
    /// These events are raised when the PLC sends signals to the Vision System.
    /// </summary>
    private void AddEventHandlers()
    {
      mNdm.ClearError += new CogNdmClearErrorEventHandler(mNdm_ClearError);
      mNdm.JobChangeRequested += new CogNdmJobChangeRequestedEventHandler(mNdm_JobChangeRequested);
      mNdm.NewUserData += new CogNdmNewUserDataEventHandler(mNdm_NewUserData);
      mNdm.OfflineRequested += new CogNdmOfflineRequestedEventHandler(mNdm_OfflineRequested);
      mNdm.ProtocolStatusChanged += new CogNdmProtocolStatusChangedEventHandler(mNdm_ProtocolStatusChanged);
      mNdm.TriggerAcquisition += new CogNdmTriggerAcquisitionEventHandler(mNdm_TriggerAcquisition);
      mNdm.TriggerAcquisitionDisabledError += new CogNdmTriggerAcquisitionDisabledErrorEventHandler(mNdm_TriggerAcquisitionDisabledError);
      mNdm.TriggerAcquisitionNotReadyError += new CogNdmTriggerAcquisitionNotReadyErrorEventHandler(mNdm_TriggerAcquisitionNotReadyError);
      mNdm.TriggerAcquisitionStop += new CogNdmTriggerAcquisitionStopEventHandler(mNdm_TriggerAcquisitionStop);
      mNdm.TriggerSoftEvent += new CogNdmTriggerSoftEventEventHandler(mNdm_TriggerSoftEvent);
      mNdm.TriggerSoftEventOff += new CogNdmTriggerSoftEventOffEventHandler(mNdm_TriggerSoftEventOff);
    }

    /// <summary>
    /// Unsubscribe from the NDM signal eventss.
    /// </summary>
    private void RemoveEventHandlers()
    {
      mNdm.ClearError -= new CogNdmClearErrorEventHandler(mNdm_ClearError);
      mNdm.JobChangeRequested -= new CogNdmJobChangeRequestedEventHandler(mNdm_JobChangeRequested);
      mNdm.NewUserData -= new CogNdmNewUserDataEventHandler(mNdm_NewUserData);
      mNdm.OfflineRequested -= new CogNdmOfflineRequestedEventHandler(mNdm_OfflineRequested);
      mNdm.ProtocolStatusChanged -= new CogNdmProtocolStatusChangedEventHandler(mNdm_ProtocolStatusChanged);
      mNdm.TriggerAcquisition -= new CogNdmTriggerAcquisitionEventHandler(mNdm_TriggerAcquisition);
      mNdm.TriggerAcquisitionDisabledError -= new CogNdmTriggerAcquisitionDisabledErrorEventHandler(mNdm_TriggerAcquisitionDisabledError);
      mNdm.TriggerAcquisitionNotReadyError -= new CogNdmTriggerAcquisitionNotReadyErrorEventHandler(mNdm_TriggerAcquisitionNotReadyError);
      mNdm.TriggerAcquisitionStop -= new CogNdmTriggerAcquisitionStopEventHandler(mNdm_TriggerAcquisitionStop);
      mNdm.TriggerSoftEvent -= new CogNdmTriggerSoftEventEventHandler(mNdm_TriggerSoftEvent);
      mNdm.TriggerSoftEventOff -= new CogNdmTriggerSoftEventOffEventHandler(mNdm_TriggerSoftEventOff);
    }

    /// <summary>
    /// Writes a string to the text box log in the GUI
    /// </summary>
    private void Log(string logString, params object[] logStringFormatArgs)
    {
      textBox1.AppendText(String.Format(logString, logStringFormatArgs));
      textBox1.AppendText(Environment.NewLine + Environment.NewLine);
    }
      
    #endregion Helper Methods

    #region NDM singal event handlers

    /// <summary>
    /// The NDM raises the TriggerAcquisition event to inform the vision 
    /// system that the remote device has requested an image Acquisition.
    /// </summary>
    void mNdm_TriggerAcquisition(object sender, CogNdmTriggerAcquisitionEventArgs e)
    {
      // The event is raised from a non-GUI thread.
      // Invoke this function on the GUI thread.
      if (InvokeRequired)
      {
        Invoke(new CogNdmTriggerAcquisitionEventHandler(mNdm_TriggerAcquisition), new Object[] { sender, e });
        return;
      }

      // In a real application your code would examine e.CameraIndex acquire or start acquiring
      // an image from that camera.

      // If the acquisition request resulted in an error,
      // set 
      // e.ResponseCode = CogNdmTriggerAcquisitionResponseCodeConstants.AcquisitionError;

      // Otherwise set 
      // e.ResponseCode = CogNdmTriggerAcquisitionResponseCodeConstants.Success;
      // and be sure to call 
      // mNdm.NotifyAcquisitionComplete(e.CameraIndex, acquisitionID);
      // when the acquisition eventually completes.

      // log the event
      Log("EVENT TriggerAcquisition, MessageID: {0} CameraIndex: {1} ResponseCode: {2}",
          e.MessageID,
          e.CameraIndex,
          e.ResponseCode);
    }

    /// <summary>
    /// The NDM raises the TriggerAcquisitionStop event to tell the 
    /// vision system that the Acquisition trigger has been reset. 
    /// </summary>
    void mNdm_TriggerAcquisitionStop(object sender, CogNdmTriggerAcquisitionStopEventArgs e)
    {
      // The event is raised from a non-GUI thread.
      // Invoke this function on the GUI thread.
      if (InvokeRequired)
      {
        Invoke(new CogNdmTriggerAcquisitionStopEventHandler(mNdm_TriggerAcquisitionStop), new Object[] { sender, e });
        return;
      }

      // Log the event
      Log("EVENT TriggerAcquisitionStop, MessageID: {0} CameraIndex: {1}",
          e.MessageID,
          e.CameraIndex);
    }

    /// <summary>
    /// The NDM raises the TriggerSoftEvent event to inform the vision system 
    /// that the remote device has requested that a soft event execute.
    /// </summary>
    void mNdm_TriggerSoftEvent(object sender, CogNdmTriggerSoftEventEventArgs e)
    {
      // The event is raised from a non-GUI thread.
      // Invoke this function on the GUI thread.
      if (InvokeRequired)
      {
        Invoke(new CogNdmTriggerSoftEventEventHandler(mNdm_TriggerSoftEvent), new Object[] { sender, e });
        return;
      }

      // In a real application your code would examine e.SoftEventID and do any processing
      // required by the given soft event.

      // If the processing for the soft event is complete when this method returns,
      // set 
      // e.ResponseCode = CogNdmSoftEventResponseCodeConstants.Finished;

      // Otherwise set 
      // e.ResponseCode = CogNdmSoftEventResponseCodeConstants.NotFinished;
      // and be sure to call 
      // mNdm.NotifyAsyncSoftEventComplete(e.MessageID, e.SoftEventID)
      // later when the processing completes.

      // Log the event
      Log("EVENT TriggerSoftEvent, MessageID: {0} SoftEventID: {1} ResponseCode: {2}",
          e.MessageID,
          e.SoftEventID,
          e.ResponseCode);
    }

    /// <summary>
    /// The NDM raises the TriggerSoftEventOff event to tell the vision system
    /// that the soft event trigger bit has been reset.
    /// </summary>
    void mNdm_TriggerSoftEventOff(object sender, CogNdmTriggerSoftEventOffEventArgs e)
    {
      // The event is raised from a non-GUI thread.
      // Invoke this function on the GUI thread.
      if (InvokeRequired)
      {
        Invoke(new CogNdmTriggerSoftEventOffEventHandler(mNdm_TriggerSoftEventOff), new Object[] { sender, e });
        return;
      }

      // Log the event
      Log("EVENT TriggerSoftEventOff, MessageID: {0} SoftEventID: {1}",
          e.MessageID,
          e.SoftEventID);
    }

    /// <summary>
    /// The NDM raises the TriggerAcquisitionDisabledError event to tell the
    /// vision system that an acquisition trigger was set but the acquisition 
    /// trigger was not enabled. 
    /// </summary>
    void mNdm_TriggerAcquisitionDisabledError(object sender, CogNdmTriggerAcquisitionDisabledErrorEventArgs e)
    {
      // The event is raised from a non-GUI thread.
      // Invoke this function on the GUI thread.
      if (InvokeRequired)
      {
        Invoke(new CogNdmTriggerAcquisitionDisabledErrorEventHandler(mNdm_TriggerAcquisitionDisabledError), new Object[] { sender, e });
        return;
      }

      // Log the event
      Log("EVENT TriggerAcquisitionDisabledError, MessageID: {0}", e.MessageID);
    }

    /// <summary>
    /// The NDM raises the TriggerAcquisitionNotReadyError event to tell the
    /// vision system that an acquisition trigger was set on the PLC but the
    /// vision system was not ready to receive it. 
    /// </summary>
    void mNdm_TriggerAcquisitionNotReadyError(object sender, CogNdmTriggerAcquisitionNotReadyErrorEventArgs e)
    {
      // The event is raised from a non-GUI thread.
      // Invoke this function on the GUI thread.
      if (InvokeRequired)
      {
        Invoke(new CogNdmTriggerAcquisitionNotReadyErrorEventHandler(mNdm_TriggerAcquisitionNotReadyError), new Object[] { sender, e });
        return;
      }

      // Log the event
      Log("EVENT TriggerAcquisitionNotReadyError, MessageID: {0}", e.MessageID);
    }

    /// <summary>
    /// The NDM raises the ProtocolStatusChanged event to inform the vision
    /// system that the status of the PLC protocol connection has changed. 
    /// </summary>
    void mNdm_ProtocolStatusChanged(object sender, CogNdmProtocolStatusChangedEventArgs e)
    {
      // The event is raised from a non-GUI thread.
      // Invoke this function on the GUI thread.
      if (InvokeRequired)
      {
        Invoke(new CogNdmProtocolStatusChangedEventHandler(mNdm_ProtocolStatusChanged), new Object[] { sender, e });
        return;
      }

      // log the event
      Log("EVENT ProtocolStatusChanged, MessageID: {0} ProtocolStatus: {1} RemoteAddress: {2}",
          e.MessageID,
          e.ProtocolStatus,
          e.RemoteAddress != null ? e.RemoteAddress.ToString() : "N/A"); // RemoteAddress can be null!

      // Enable the Notify group box and the "Read User Data" button once the protocol is connected
      if (e.ProtocolStatus == CogNdmConnectionStatusConstants.Connected)
      {
        textBox1.AppendText("FFP Initialized.");
        if (waitForProtocol != null)
          waitForProtocol.Enabled = false;
        grpNotify.Enabled = true;
        btnReadUserData.Enabled = true;
      }
    }

    /// <summary>
    /// The NDM raises the OfflineRequested event to tell the vision system 
    /// that it should go offline. 
    /// </summary>
    void mNdm_OfflineRequested(object sender, CogNdmOfflineRequestedEventArgs e)
    {
      // The event is raised from a non-GUI thread.
      // Invoke this function on the GUI thread.
      if (InvokeRequired)
      {
        Invoke(new CogNdmOfflineRequestedEventHandler(mNdm_OfflineRequested), new Object[] { sender, e });
        return;
      }

      // log the event
      Log("EVENT OfflineRequested, MessageID: {0} ReturnToPreviousState: {1}",          
          e.MessageID,
          e.ReturnToPreviousState);
    }

    /// <summary>
    /// The NDM raises the NewUserData event to tell the vision system that
    /// new user data has arrived from the remote device.
    /// </summary>
    void mNdm_NewUserData(object sender, CogNdmNewUserDataEventArgs e)
    {
      // The event is raised from a non-GUI thread.
      // Invoke this function on the GUI thread.
      if (InvokeRequired)
      {
        Invoke(new CogNdmNewUserDataEventHandler(mNdm_NewUserData), new Object[] { sender, e });
        return;
      }

      // log the event
      Log("EVENT NewUserData: MessageID: {0}", e.MessageID);

      // Read the first 8 bytes of user data and add it to the log
      byte[] data = mNdm.ReadUserData(0, 8);
      foreach (byte b in data)
        textBox1.AppendText(Convert.ToString(b, 2) + Environment.NewLine);
    }

    /// <summary>
    /// The NDM raises the JobChangeRequested event to inform the vision system
    /// that the remote device has requested a job change.
    /// </summary>
    void mNdm_JobChangeRequested(object sender, CogNdmJobChangeRequestedEventArgs e)
    {
      // The event is raised from a non-GUI thread.
      // Invoke this function on the GUI thread.
      if (InvokeRequired)
      {
        Invoke(new CogNdmJobChangeRequestedEventHandler(mNdm_JobChangeRequested), new Object[] { sender, e });
        return;
      }

      // log the event
      Log("EVENT JobChangeRequested, MessageID: {0} JobID: {1} JobSlot: {2}",
          e.MessageID,
          e.JobID,
          e.JobSlot);
    }

    /// <summary>
    /// The NDM raises the ClearError event to inform the vision system that 
    /// the remote device has been notified of an error reported by the vision
    /// system and the error has been be cleared. 
    /// </summary>
    void mNdm_ClearError(object sender, CogNdmClearErrorEventArgs e)
    {
      // The event is raised from a non-GUI thread.
      // Invoke this function on the GUI thread.
      if (InvokeRequired)
      {
        Invoke(new CogNdmClearErrorEventHandler(mNdm_ClearError), new Object[] { sender, e });
        return;
      }

      // log the event
      Log("EVENT ClearError, MessageID: {0}", e.MessageID);
    }
    
    #endregion 

    #region NDM Notify button click methods

    /// <summary>
    /// Notify the PLC that the Vision System is running or online. 
    /// </summary>
    private void btnNotifyRunning_Click(object sender, EventArgs e)
    {
      try
      {
        Log("NotifyRunning");
        mNdm.NotifyRunning();
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }
    }

    /// <summary>
    /// Notify the PLC that the Vision System stopped running. 
    /// </summary>
    private void btnNotifyStopped_Click(object sender, EventArgs e)
    {
      try
      {
        Log("NotifyStopped");
        mNdm.NotifyStopped(CogNdmStoppedCodeConstants.None);
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }
    }

    /// <summary>
    /// Notify the remote device that an acquisition has started. 
    /// </summary>
    private void btnNotifyAcquisitionStarted_Click(object sender, EventArgs e)
    {
      try
      {
        Log("NotifyAcquisitionStarted");
        mNdm.NotifyAcquisitionStarted((int)numCamIndex.Value, (int)numAcqID.Value);
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }
    }

    /// <summary>
    /// Notify the PLC that an acquisition has completed. 
    /// </summary>
    private void btnAcqComplete_Click(object sender, EventArgs e)
    {
      try
      {
        Log("NotifyAcquisitionComplete");
        mNdm.NotifyAcquisitionComplete((int)numCamIndex.Value, (int)numAcqID.Value);
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }
    }

    /// <summary>
    /// Notify the PLC that the Vision System is ready to receive acquisition triggers. 
    /// </summary>
    private void btnAcqReady_Click(object sender, EventArgs e)
    {
      try
      {
        Log("NotifyAcquisitionReady");
        mNdm.NotifyAcquisitionReady((int)numCamIndex.Value);
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }
    }

    /// <summary>
    /// Notify the PLC that the Vision System is unable to receive acquisition triggers.
    /// </summary>
    private void btnNotifyAcqDisabled_Click(object sender, EventArgs e)
    {
      try
      {
        Log("NotifyAcquisitionDisabled");
        mNdm.NotifyAcquisitionDisabled((int)numCamIndex.Value);
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }
    }

    /// <summary>
    /// Notify the remote device that an acquisition error has occurred. 
    /// </summary>
    private void btnNotifyAcqError_Click(object sender, EventArgs e)
    {
      try
      {
        Log("NotifyAcquisitionError");
        mNdm.NotifyAcquisitionError((int)numCamIndex.Value);
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }
    }

    /// <summary>
    /// Notify the remote device that the exposure is complete or the strobe has fired and it is now safe to move the part from the field of view. 
    /// </summary>
    private void btnNotifyAcqMovePart_Click(object sender, EventArgs e)
    {
      try
      {
        Log("NotifyAcquisitionMovePart");
        mNdm.NotifyAcquisitionMovePart((int)numCamIndex.Value, (int)numAcqID.Value);
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }

    }


    /// <summary>
    /// Notify the remote device (PLC) that the Vision System has encountered an error. 
    /// </summary>
    private void btnNotifyError_Click(object sender, EventArgs e)
    {
      try
      {
        Log("NotifyError");
        mNdm.NotifyError((int)numErrorID.Value);
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }

    }

    /// <summary>
    /// Notify the PLC that an inspection has finished. 
    /// </summary>
    private void btnNotifyInspectionComplete_Click(object sender, EventArgs e)
    {
      try
      {
        Log("NotifyInspectionComplete");

        // Create an inspection result object to simulate the 
        // result of an actual inspection.
        CogNdmInspectionResult res = new CogNdmInspectionResult();

        // Set the inspection index/channel that created the result.
        res.InspectionIndex = (int) numCamIndex.Value;

        // Did the inspection pass?
        res.InspectionPassed = true;

        // Add some result data to the inspection result.
        int resultData = 1020;
        byte[] bytes = BitConverter.GetBytes(resultData);
        res.ResultData = bytes;
        res.ResultDataOffset = 0;

        // Set the inspection result code
        res.ResultCode = 5;

        // Construct an object which identifies which image(s)
        // were processed to create the inspection result.
        CogNdmUsedAcquisitionIDCollection ids = 
          new CogNdmUsedAcquisitionIDCollection() {
            new CogNdmUsedAcquisitionID((int)numAcqID.Value, (int)numCamIndex.Value)
          };

        // Add the processed image ids to the result object.
        res.UsedAcquisitionIDs = ids;

        // finally, notify the PLC of the completed inspection result.
        mNdm.NotifyInspectionComplete(res);
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }
    }

    /// <summary>
    /// Notify the PLC that the Job ID has changed
    /// </summary>
    private void btnNotifyJobState_Click(object sender, EventArgs e)
    {
      try
      {
        Log("NotifyJobState");
        mNdm.NotifyJobState((int)numJobID.Value);
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }
    }

    /// <summary>
    /// Notifies the PLC of the Vision System status.
    /// </summary>
    private void btnSystemStatus_Click(object sender, EventArgs e)
    {
      try
      {
        Log("NotifySystemStatus");
        mNdm.NotifySystemStatus(chkReady.Checked, chkBusy.Checked);
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }
    }

    /// <summary>
    /// Reads the user data sent from the remote device to the vision system. 
    /// </summary>
    private void btnReadUserData_Click(object sender, EventArgs e)
    {
      try
      {
        Log("ReadUserData");

        // read and print the first 8 bytes of user data and
        byte[] data = mNdm.ReadUserData(0, 8);
        foreach (byte b in data)
          textBox1.AppendText(Convert.ToString(b, 2) + Environment.NewLine);
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }
    }

    #endregion

    #region Miscellaneous GUI event handlers and Form Overrides

    /// <summary>
    /// Updates the Ethernet setting on the Comm card.
    /// </summary>
    private void btnUpdateSettings_Click(object sender, EventArgs e)
    {
      // Read the IP address from the form
      IPAddress ip;
      if (!IPAddress.TryParse(txt_IPAddress.Text, out ip))
      {
        MessageBox.Show("Can't parse IP address!");
        return;
      }
      // Read the subject from the form
      IPAddress subnet;
      if (!IPAddress.TryParse(txt_Subnet.Text, out subnet))
      {
        MessageBox.Show("Can't parse Subnet Mask!");
        return;
      }

      // Update the local variable mEthernetPortSettings with the form values
      try
      {
        mEthernetPortSettings.IPAddress = ip;
        mEthernetPortSettings.SubnetMask = subnet;
        mEthernetPortSettings.HostName = txt_HostName.Text;
        mEthernetPortSettings.DHCPEnable = ckb_DHCP.Checked;

        // If the interface is up, bring it down
        if (mEthernetPort.IsInterfaceUp)
        {
          textBox1.AppendText(Environment.NewLine + "Bringing Interface DOWN...");

          // Bringing the interface down.
          bool timedOut = !mEthernetPort.BringInterfaceDownAsync().Wait(5000);
          if (timedOut)
          {
            textBox1.AppendText(Environment.NewLine + "Bringing Interface Down TIMED OUT!!");
            return;
          }
          textBox1.AppendText(Environment.NewLine + "The Interface is DOWN.");
        }
        // Write the setting to the port.
        mEthernetPort.WriteSettings(mEthernetPortSettings);

        // If the interface is down, bring it up
        if (!mEthernetPort.IsInterfaceUp)
        {
          // Bringing the interface up.
          bool timeout = !mEthernetPort.BringInterfaceUpAsync().Wait(15000);
          if (timeout)
          {
            throw new Exception("Bringing the interface up timed out.");
          }
          textBox1.AppendText(Environment.NewLine + "The Interface is UP.");
        }
        if (mEthernetPort.IsInterfaceUp)
        {
          CogEthernetPortSettings eActiveSettings = mEthernetPort.ReadActiveSettings();
          txt_ActiveIP.Text = eActiveSettings.IPAddress.ToString();
          txt_HostName.Text = eActiveSettings.HostName.ToString();
          txt_Subnet.Text = eActiveSettings.SubnetMask.ToString();
        }
      }
      catch (Exception ex1)
      {
        MessageBox.Show(ex1.Message);
      }

    }

    /// <summary>
    /// Initialize FFP for a specific protocol.
    /// </summary>
    private void btnInitFFP_Click(object sender, EventArgs e)
    {
      // Create the FFP interface.
      try
      {
        textBox1.AppendText(Environment.NewLine + "Initializing FFP, please wait..." + Environment.NewLine);
        if (radProfinet.Checked)
        {
          mNdm = mCard.FfpAccess.CreateNetworkDataModel(CogFfpProtocolConstants.Profinet);
        }
        else if (radEthernetIP.Checked)
        {
          mNdm = mCard.FfpAccess.CreateNetworkDataModel(CogFfpProtocolConstants.EthernetIp);
        }
        // Disable the Init FFP button. The user will not be able to switch protocols.
        // The application must be restrated.
        btnInitFFP.Enabled = false;
      }
      catch (Exception ex)
      {
        System.Windows.Forms.MessageBox.Show(
          "Could not initialize Comm Card Network Data Model interface." + ex.Message);
        return;
      }

      // Sign up for input event notifications.
      AddEventHandlers();

      // Start the NDM
      mNdm.Start();

      // Start a timer to wait for the sample to connect to the PLC
      // Wait for 30 seconds.
      waitForProtocol = new System.Timers.Timer(30000);
      waitForProtocol.Elapsed += new System.Timers.ElapsedEventHandler(waitForProtocol_Elapsed);
      waitForProtocol.Enabled = true;
    }

    void waitForProtocol_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      waitForProtocol.Enabled = false;
      DialogResult res = MessageBox.Show("The PLC may not be connected. Do you want to exit the application?", "Initialize FFP", MessageBoxButtons.YesNo);
      if (res == DialogResult.Yes)
        Application.Exit();
    }

    /// <summary>
    /// Clears the log when the "Clear Log" button is clicked
    /// </summary>
    private void btnClearLog_Click(object sender, EventArgs e)
    {
      textBox1.Clear();
    }

    /// <summary>
    /// Called once the form has been loaded.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      //Initialize Comm card
      if (!InitCommCard())
        Application.Exit();

      // The Ethernet settings group box is enabled
      grpEthernetSettings.Enabled = true;

      // The Protocol selection group box is enabled
      grpProtocol.Enabled = true;

      // The Notify group box is disabled
      grpNotify.Enabled = false;

      // The "Read User Data" button is disabled
      btnReadUserData.Enabled = false;
    }

    /// <summary>
    /// Called when the user tries to close the application.
    /// </summary>
    protected override void OnClosing(CancelEventArgs e)
    {
      if (mNdm != null)
      {
        // Cancel notification of input events change.
        RemoveEventHandlers();
        // bring the interface down
        mEthernetPort.BringInterfaceDownAsync();
        // Stop the NDM
        mNdm.Stop();
        // Some events may already be in the message queue. 
        // This call will flush the message queue so the application
        // can exit cleanly.
        Application.DoEvents();
      }
      base.OnClosing(e);
    }
  }

  #endregion

}
