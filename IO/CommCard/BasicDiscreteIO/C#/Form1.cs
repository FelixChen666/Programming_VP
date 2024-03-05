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
 
 * This sample demonstrates basic discrete I/O using the Cognex Communications
 * Card. The sample shows how to:
 *   1. Receive notifications when the input lines change.
 *   2. Set the value of the output lines high or low.
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Cognex.VisionPro;
using Cognex.VisionPro.Comm;

namespace BasicDiscreteIO
{
  public partial class Form1 : Form
  {
    // Holds a reference to the Comm Card Collection.
    CogCommCards mCards;

    // Holds a reference to the Comm Card used
    // in this sample.
    CogCommCard mCard;

    // Holds a reference to the Precision I/O 
    // interface used in this sample.
    CogPrio mPrio;

    // T0 timestamp read from the Comm Card
    // when the application is initialized.
    CogPrioState mT0;

    // Tracks whether a checkbox value was
    // changed due to a user click or not
    Boolean mUserCausedCheckChangedFlag = true;
      
    public Form1()
    {
      InitializeComponent();

      try
      {

        // Initialize the comm card for this sample
        InitCommCard();

        // Now initialize the Form
        InitForm();

        // Enable events
        mPrio.EnableEvents();

      }
      catch (Exception e)
      {
        Log("ERROR: " + e.Message);
      }
    }

    /// <summary>
    /// Initializes the comm card for this sample
    /// </summary>
    private void InitCommCard()
    {
      // Enumerate all the Comm Cards in the system.
      mCards = new CogCommCards();

      if (mCards.Count == 0)
      {
        throw new Exception("Sample requires a Comm Card 24A or Comm Card 24C.");        
      }

      // Use the 0th Comm Card in this sample.
      mCard = mCards[0];

      // Check for Discrete I/O support
      if (mCard.DiscreteIOAccess == null)
      {
        throw new Exception("Sample requires a Comm Card with Discrete I/O support.");       
      }
      
      // Create the Precision I/O interface that this sample uses to
      // interact with the Comm Card's I/O lines.
      mPrio = mCard.DiscreteIOAccess.CreatePrecisionIO();

      // disable events until the UI has been initialized
      mPrio.DisableEvents();

      // Configure some example PRIO events.
      ConfigureDefaultEvents(mPrio);
    }

    /// <summary>
    /// Initializes the state of the Form to match the 
    /// initial state of the Comm Card.
    /// </summary>
    private void InitForm()
    {
      if (mCard != null && mPrio != null)
      {
        // make the controls visible if the
        // comm card and prio objects are not null.
        grpInputBank0.Visible = true;
        grpOutputBank0.Visible = true;
        btnReadState.Visible = true;
        btnClearLog.Visible = true;

        if (mCard.Name == "Cognex Communication Card 24A")
        {
          // The Cognex Communication Card 24A supports an 
          // additional output bank... enable this here
          grpDSOutputBank0.Visible = true;
        }

        // Read the initial state... as a convienince to 
        // the user, all timestamps are reported relative 
        // to to T0, this makes it a little easier to read
        // the log.
        mT0 = mPrio.ReadState();

        // Update the checkboxes which represent the high/low
        // state of the individual lines to match the initial 
        // state
        RefreshCheckBoxes(mT0);
      }
    }

    /// <summary>
    /// Configure a set of default precision I/O events for the hardware.
    /// <para>
    /// Adds an "InputChanged_" event for each input line.
    /// The event for input line 0 is named "InputChanged_0" and so on...
    /// The input events are caused by any signal transition on the dedicated input line.
    /// The input events are not configured with a response.
    /// </para>
    /// <para>
    /// Adds an "PulseOutput_" event for each output line.
    /// The event for output line 0 is named "PulseOutput_0" and so on...
    /// The output events respond by pulsing the the dedicated output line high for 10.0 mS.
    /// The output events are not configured with a cause and only occcur as a result 
    /// of direct user scheduling.
    /// </para>
    /// </summary>
    private void ConfigureDefaultEvents(CogPrio prio)
    {
      // A new collection of prioEvents to be configured.
      CogPrioEventCollection prioEvents = new CogPrioEventCollection();

      // Configure a prio event fore each INPUT line.
      for (int i = 0; i < prio.GetNumLines(CogPrioBankConstants.InputBank0); i++)
      {
        CogPrioEvent prioEvent = new CogPrioEvent()
        {
          // The events are named InputChange_0, InputChanged_1, and so on ...
          Name = String.Format("InputChanged_{0}", i),

          // The event is caused by transitions on the given line number.
          CausesLine = new CogPrioEventCauseLineCollection() 
          {
            new CogPrioEventCauseLine() 
            {
              LineBank = CogPrioBankConstants.InputBank0,
              LineNumber = i,
              LineTransition = CogPrioLineTransitionConstants.Any
            }
          }
        };

        // sign up for Host Notification on each input event.
        prioEvent.HostNotification += new CogPrioEventHandler(InputChanged_HostNotification);

        // add the event to events the collection
        prioEvents.Add(prioEvent);
      }

      // Configure events for each OUTPUT line. When scheduled, these events 
      // respond by pulsing an output line high for 10ms.
      for (int i = 0; i < mPrio.GetNumLines(CogPrioBankConstants.OutputBank0); i++)
      {
        CogPrioEvent prioEvent = new CogPrioEvent()
        {
          // The events are named PulseOutput_0, PulseOutput_1, and so on ...
          Name = String.Format("PulseOutput_{0}", i),

          // respond to the event by pulsing the given line number high for 10 mS.
          ResponsesLine = new CogPrioEventResponseLineCollection() 
          {
            new CogPrioEventResponseLine() 
            {
              OutputLineBank = CogPrioBankConstants.OutputBank0,
              OutputLineNumber = i,
              OutputLineValue = CogPrioOutputLineValueConstants.SetHigh,
              PulseDuration = 10.0,
              DelayType = CogPrioDelayTypeConstants.None,
              DelayValue = 0.0
            }
          }
        };

        // add the event to events the collection
        prioEvents.Add(prioEvent);
      }

      // Clear any pre-existing event configs from the hardware.
      mPrio.Events.Clear(); 

      // Finally, set the configured events collection on the hardware.
      mPrio.Events = prioEvents;

      // Ensure the the configured events are valid!
      // Always check the valid CogPrio.Valid after modiying the
      // the events collection... if Valid is false the configured events
      // will not be written to the card.
      if (!mPrio.Valid)
      {
        throw new Exception(mPrio.ValidationErrorMsg[0]);
      }
    }

    /// <summary>
    /// Writes a string to the text box log in the GUI.
    /// </summary>
    private void Log(string logString, params object[] logStringFormatArgs)
    {
      textBox1.AppendText(String.Format(logString, logStringFormatArgs));
      textBox1.AppendText(Environment.NewLine + Environment.NewLine);
    }

    /// <summary>
    /// Writes the state of all the I/O lines to the text box log.
    /// </summary>
    private void Log(CogPrioState state)
    {
      textBox1.AppendText(state.ToString());
      textBox1.AppendText(Environment.NewLine);
    }
    
    /// <summary>
    /// Event handler which is called whenever any of the Comm Card input lines changes.
    /// </summary>
    private void InputChanged_HostNotification(object sender, CogPrioEventArgs e)
    {
      // The Host Notification event handler is called from different threads.
      // We must invoke back to the GUI thread in order to update the GUI.
      if (InvokeRequired)
      {
        Invoke(new CogPrioEventHandler(InputChanged_HostNotification), new Object[] { sender, e });
        return;
      }

      // Log the event, and the time at which the evnt occurred
      Log("EVENT {0}, at: {1:g6}ms", e.EventName, mT0.TimeDifference(e.State));

      // Log the state of the I/O lines when the event occurred.
      Log(e.State);
      
      // Refresh the check boxes in the GUI to match the current state
      RefreshCheckBoxes(e.State);
    }

    /// <summary>
    /// Called whenever an input line has changed... refreshes the check boxes
    /// from the current state
    /// </summary>
    private void RefreshCheckBoxes(CogPrioState state)
    {
      // The check boxes are being updated due to an input change,
      // not a user click.
      mUserCausedCheckChangedFlag = false;

      // set the checked state to match the state of the corresponding 
      // I/O line.

      chkInput0.Checked = state[CogPrioBankConstants.InputBank0, 0];
      chkInput1.Checked = state[CogPrioBankConstants.InputBank0, 1];
      chkInput2.Checked = state[CogPrioBankConstants.InputBank0, 2];
      chkInput3.Checked = state[CogPrioBankConstants.InputBank0, 3];
      chkInput4.Checked = state[CogPrioBankConstants.InputBank0, 4];
      chkInput5.Checked = state[CogPrioBankConstants.InputBank0, 5];
      chkInput6.Checked = state[CogPrioBankConstants.InputBank0, 6];
      chkInput7.Checked = state[CogPrioBankConstants.InputBank0, 7];

      chkOutput0.Checked = state[CogPrioBankConstants.OutputBank0, 0];
      chkOutput1.Checked = state[CogPrioBankConstants.OutputBank0, 1];
      chkOutput2.Checked = state[CogPrioBankConstants.OutputBank0, 2];
      chkOutput3.Checked = state[CogPrioBankConstants.OutputBank0, 3];
      chkOutput4.Checked = state[CogPrioBankConstants.OutputBank0, 4];
      chkOutput5.Checked = state[CogPrioBankConstants.OutputBank0, 5];
      chkOutput6.Checked = state[CogPrioBankConstants.OutputBank0, 6];
      chkOutput7.Checked = state[CogPrioBankConstants.OutputBank0, 7];
      chkOutput8.Checked = state[CogPrioBankConstants.OutputBank0, 8];
      chkOutput9.Checked = state[CogPrioBankConstants.OutputBank0, 9];
      chkOutput10.Checked = state[CogPrioBankConstants.OutputBank0, 10];
      chkOutput11.Checked = state[CogPrioBankConstants.OutputBank0, 11];
      chkOutput12.Checked = state[CogPrioBankConstants.OutputBank0, 12];
      chkOutput13.Checked = state[CogPrioBankConstants.OutputBank0, 13];
      chkOutput14.Checked = state[CogPrioBankConstants.OutputBank0, 14];
      chkOutput15.Checked = state[CogPrioBankConstants.OutputBank0, 15];

      if (mCard.Name == "Cognex Communication Card 24A")
      {
        chkDsOutput0.Checked = state[CogPrioBankConstants.DS1000OutputBank0, 0];
        chkDsOutput1.Checked = state[CogPrioBankConstants.DS1000OutputBank0, 1];
        chkDsOutput2.Checked = state[CogPrioBankConstants.DS1000OutputBank0, 2];
        chkDsOutput3.Checked = state[CogPrioBankConstants.DS1000OutputBank0, 3];
        chkDsOutput4.Checked = state[CogPrioBankConstants.DS1000OutputBank0, 4];
        chkDsOutput5.Checked = state[CogPrioBankConstants.DS1000OutputBank0, 5];
        chkDsOutput6.Checked = state[CogPrioBankConstants.DS1000OutputBank0, 6];
        chkDsOutput7.Checked = state[CogPrioBankConstants.DS1000OutputBank0, 7];
      }

      mUserCausedCheckChangedFlag = true;
    }

    /// <summary>
    /// Event handler which is called whenever any of the output check boxes is checked or unchecked
    /// </summary>
    private void chkOutput_CheckedChanged(object sender, EventArgs e)
    {

      if (!mUserCausedCheckChangedFlag) // ignore events which were not caused by the user
        return;                         // clicking on a check box.

      // The user checked or unchecked an output check box...
      // Set the output line to the value of the check box.

      CheckBox chkSender = sender as CheckBox;
      if (chkSender != null)
      {
        // Set the output line high or low based on the whether the check box
        // was checked or unchecked.
        CogPrioOutputLineValueConstants valueToSet =
          chkSender.Checked ? CogPrioOutputLineValueConstants.SetHigh :
                              CogPrioOutputLineValueConstants.SetLow;    

        // Figure out which bank of outputs to set from the name of the checkbox.
        CogPrioBankConstants bankToSet = 
          chkSender.Name.Contains("DsOutput") ? CogPrioBankConstants.DS1000OutputBank0 :
                                                CogPrioBankConstants.OutputBank0;
        
        // retrieve the line number from the tag property of the check box that was clicked.    
        int lineNumToSet = int.Parse(chkSender.Tag.ToString());

        Log("CALL SetOutput, Bank: {0} Line: {1} Value {2}",
          bankToSet, lineNumToSet, valueToSet);

        // and finally, set the output.
        mPrio.SetOutput(bankToSet, lineNumToSet, valueToSet);

        // Print the state of the I/O lines after setting the output
        // Unfortunatley you can't get host notifications for output events.
        Log(mPrio.ReadState()); 
      }
    }

    /// <summary>
    /// This is the event handler when the user clicks on the "Read State" button.
    /// </summary>
    private void btnReadState_Click(object sender, EventArgs e)
    {
      if (mPrio != null)
      {
        // Read the state of the IO lines
        CogPrioState state = mPrio.ReadState();

        // Log the state to the GUI
        Log(state);

        // Update the checkboxes using the state
        RefreshCheckBoxes(state);
      }
    }

    /// <summary>
    /// Called when the user clicks on the "Clear Log" button.
    /// </summary>
    private void btnClearLog_Click(object sender, EventArgs e)
    {
      // Clear the textbox
      textBox1.Clear();
    }

    /// <summary>
    /// Called when the user click the "Pulse Output" button.
    /// </summary>
    private void btnPulseOutput_Click(object sender, EventArgs e)
    {
      // schedule an output pulse on the given line to occur immediately
      Log("CALL Schedule PulseOutput");
      mPrio.Events["PulseOutput_" + numLineToPulse.Value.ToString()].Schedule();
    }

    /// <summary>
    /// Called when the user closes the application.
    /// </summary>
    protected override void OnClosing(CancelEventArgs e)
    {
      if (mPrio != null)
      {
        // Disable precision IO events
        mPrio.DisableEvents();

        // disconnect the event handler
        for (int i=0; i < mPrio.Events.Count; i++)
          mPrio.Events[i].HostNotification -= new CogPrioEventHandler(InputChanged_HostNotification);

        // Some events may already be in the message queue. 
        // This call will flush the message queue so the application
        // can exit cleanly.
        Application.DoEvents();

      }
      base.OnClosing(e);
    }
  }
}
