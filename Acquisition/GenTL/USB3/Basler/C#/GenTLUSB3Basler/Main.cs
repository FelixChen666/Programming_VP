/*******************************************************************************
 Copyright (C) 2021 Cognex Corporation

 Subject to Cognex Corporations terms and conditions and license agreement,
 you are authorized to use and modify this source code in any way you find
 useful, provided the Software and/or the modified Software is used solely in
 conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
 and agree that Cognex has no warranty, obligations or liability for your use
 of the Software.
*******************************************************************************/

// This sample demonstrates a simple example of how to utilize the OwnedGenTLAccess
// interface to interact with a Basler USB3 camera's features, as well as the features
// exposed by the Basler USB3 producer.

// This sample was developed against and tested with a Basler acA2040-90um and while
// the features presented here may be present on other cameras, this cannot be assured,
// as some of them are specified as optional features within the Genicam specification.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cognex.VisionPro;
using Cognex.VisionPro.GenTL;
using Cognex.VisionPro.Display;
using System.Runtime.InteropServices;

namespace GenTLUSB3Basler
{
  public partial class Main : Form
  {
    private CogFrameGrabberGenTLs enumeratedCameras;
    private ICogAcqFifo activeFifo;
    private int currentAcquisitionTicket = -1;

    public Main()
    {
      InitializeComponent();
    }

    private void DisposeActiveFifo()
    {
      //Dispose of the previous Fifo. These hold references in the module, so it is 
      //a good practice to dispose of them once you are finished using the Fifo.
      if (activeFifo != null)
      {
        activeFifo.Flush();
        activeFifo.Complete -= ActiveFifo_Complete;
        //The underlying object implements IDisposable
        ((IDisposable)activeFifo).Dispose();
        activeFifo = null;
      }
    }

    private void Main_Load(object sender, EventArgs e)
    {
      //Get the enumerated GenTL Cameras by constructing the appropriate collection class
      //They will also be present in the generic CogFrameGrabbers collection, alongside 
      //any non-GenTL cameras that were enumerated by other modules.
      enumeratedCameras = new CogFrameGrabberGenTLs();

      if (enumeratedCameras.Count == 0)
      {
        MessageBox.Show("No GenTL cameras were enumerated. " + 
          "Please ensure that at least one USB3 GenTL is connected to the system and " +
          "is receiving power. Refer to the 'GenTL Architecture' page in the help documentation " +
          "for more information on requirements such as cable usage for GenTL camera enumeration if failure to " +
          "enumerate persists");
        return;
      }

      //Populate the camera selection combo box
      cameraSelectionComboBox.DisplayMember = "Name";
      foreach (ICogFrameGrabber enumeratedCamera in enumeratedCameras)
      {
        //We are only looking at USB3 cameras
        //U3V is a string constant defined by the GenTL standard and stands for 
        //"USB3 Vision Standard"
        if (enumeratedCamera.OwnedGenTLAccess.TLType.Equals("U3V"))
        {
          cameraSelectionComboBox.Items.Add(enumeratedCamera);
        }
      }
    }

    //Disconnect all framegrabbers when the application is closing
    private void Main_FormClosing(object sender, FormClosingEventArgs e)
    {
      DisposeActiveFifo();

      CogFrameGrabbers allFramegrabbers = new CogFrameGrabbers();

      foreach (ICogFrameGrabber framegrabber in allFramegrabbers)
      {
        framegrabber.Disconnect(false);
      }
    }

    private void cameraSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      DisposeActiveFifo();

      getSnapshotButton.Enabled = false;
      getFeatureNodeTreeButton.Enabled = false;
      createFifoButton.Enabled = false;

      getFeatureButton.Enabled = false;
      setFeatureButton.Enabled = false;
      executeCommandButton.Enabled = false;

      videoFormatSelectionComboBox.Enabled = false;
      videoFormatSelectionComboBox.Items.Clear();

      acquireSinglebutton.Enabled = false;
      acquireContinuousToggleButton.Enabled = false;
      createCustomPropertyButton.Enabled = false;
      clearCustomPropertiesButton.Enabled = false;

      if (cameraSelectionComboBox.SelectedIndex != -1)
      {
        CogStringCollection availableFormats = null;
        String cameraName = "[Error ecountered during cast or camera name retrieval]";

        try
        {
          ICogFrameGrabber castedCamera = ((ICogFrameGrabber) cameraSelectionComboBox.SelectedItem);
          cameraName = castedCamera.Name;
          availableFormats = castedCamera.AvailableVideoFormats;
        }
        catch (Exception err)
        {
          MessageBox.Show("Exception of type " + err.GetType() + " was encountered like attempting to obtain the available video formats for the camera " +
            cameraName + " with the message: " + err.Message);

          //Failed to obtain video formats, don't enable any buttons. Also clear out the camera selection
          cameraSelectionComboBox.SelectedIndex = -1;
          return; 
        }

        foreach (String videoFormat in availableFormats)
        {
          videoFormatSelectionComboBox.Items.Add(videoFormat);
        }

        videoFormatSelectionComboBox.Enabled = true;

        getSnapshotButton.Enabled = true;
        getFeatureNodeTreeButton.Enabled = true;

        getFeatureButton.Enabled = true;
        setFeatureButton.Enabled = true;
        executeCommandButton.Enabled = true;
      }
    }

    private void videoFormatSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      DisposeActiveFifo();

      createFifoButton.Enabled = false;
      acquireSinglebutton.Enabled = false;
      acquireContinuousToggleButton.Enabled = false;
      createCustomPropertyButton.Enabled = false;
      clearCustomPropertiesButton.Enabled = false;

      if (videoFormatSelectionComboBox.SelectedIndex != -1)
      {
        createFifoButton.Enabled = true;
      }
    }

    //The feature snapshot shows the available features across all sections of GenTL. Only RemoteDevice is the actual camera features as per 
    //Genicam, which is what you see in a snapshot created for a GigE camera. The other interfaces are for various layers and aspects of the 
    //transportation layer, as allowed and specified by the GenTL standard. Note that addressing these features requires pre-pending a specifier 
    //to indicate which interface that feature "belongs" to. Please see the returned tree from the Feature Node Tree button for details on how 
    //the feature nodes are organized, including the pre-pended specifiers which are omitted by the snapshot.
    private void getSnapshotButton_Click(object sender, EventArgs e)
    {
      String snapshot = "";

      try
      {
        ICogFrameGrabber castedCamera = ((ICogFrameGrabber)cameraSelectionComboBox.SelectedItem);
        snapshot = castedCamera.OwnedGenTLAccess.GetFSFromCamera();
      }
      catch (Exception err)
      {
        MessageBox.Show("Exception of type " + err.GetType() + 
          " was encountered while attempting to retrieve a feature snapshot with the message: " + err.Message);
        return;
      }

      SnapshotViewer snapshotViewer = new SnapshotViewer();
      snapshotViewer.SnapshotText = snapshot;
      snapshotViewer.Show();
    }

    private void getFeatureNodeTreeButton_Click(object sender, EventArgs e)
    {
      //"Root" is a special case "feature" defined by Cognex to provide the top layer of available GenTL interface nodes.
      //This node is a "category" type node, as are all of its immediate child nodes. This type of node does not have a meaningful value 
      //associated with them.
      ICogFrameGrabber castedCamera = ((ICogFrameGrabber) cameraSelectionComboBox.SelectedItem);
      TreeNode rootNode = recursiveNodeTreeBuilder(castedCamera.OwnedGenTLAccess, "Root");

      FeatureNodeTreeViewer nodeViewer = new FeatureNodeTreeViewer();
      nodeViewer.Root = rootNode;
      nodeViewer.Show();
    }

    //The genTLAccess parameter is just for convenience so we don't have to retrieve it on each recursive call
    private TreeNode recursiveNodeTreeBuilder(ICogGenTLAccess genTLAccess, String nodeName)
    {
      TreeNode node = new TreeNode(nodeName);

      CogStringCollection childFeatures = new CogStringCollection();

      try
      {
        //Note that it is not an error to call this on a non-category feature. It simply returns an empty collection
        childFeatures = genTLAccess.GetAvailableFeatures(nodeName);
      }
      catch (COMException err)
      {
        node.Nodes.Add(new TreeNode("Exception encountered retrieving the child features of this node with the message: " + err.Message));
      }

      //This is an empty collection if we failed to retrieve the available features
      foreach (String childFeature in childFeatures)
      {
        //Add nodes for each child feature, which will have all their children enumerated etc. until reaching leaf nodes
        node.Nodes.Add(recursiveNodeTreeBuilder(genTLAccess, childFeature));
      }

      return node;
    }

    private void createFifoButton_Click(object sender, EventArgs e)
    {
      String videoFormat = "[Error encountered when attempting to retrieve video format name from the combo box]";
      String cameraName = "[Error ecountered during cast or camera name retrieval]";

      try
      {
        videoFormat = (String) videoFormatSelectionComboBox.SelectedItem;
        ICogFrameGrabber castedCamera = ((ICogFrameGrabber)cameraSelectionComboBox.SelectedItem);
        cameraName = castedCamera.Name;

        activeFifo = 
          castedCamera.CreateAcqFifo(videoFormat, /* Video Format is what determines what will be set for what the Genicam spec calls the Pixel Format */
          CogAcqFifoPixelFormatConstants.Format8Grey /* Ignored param in all cases, included for backwards compatibility reasons */, 
          0 /* For a USB3 camera, there are no ports, this is relevant for actual framegrabbers, which could be supported by GenTL but not this sample */, 
          true /* This one is not required to be true, we're just using that for this sample. Refer to documentation for details */);
      }
      catch (Exception err)
      {
        MessageBox.Show("Exception of type " + err.GetType() + " was encountered while attempting to create the AcqFifo (video format: " + videoFormat +
          ", camera name: " + cameraName + ") with the message: " + err.Message);

        return; //Failed to create fifo, don't enable any buttons
      }

      try
      {
        activeFifo.Complete += ActiveFifo_Complete;
      }
      catch (Exception err)
      {
        MessageBox.Show("Exception of type " + err.GetType() + " was encountered while attempting to assign event handlers for the AcqFifo (video format: " +
          videoFormat + ", camera name: " + cameraName + ") with the message: " + err.Message);

        //Failed to properly configure the fifo, don't enable any buttons and clear out the fifo
        DisposeActiveFifo();
        return; 
      }

      acquireSinglebutton.Enabled = true;
      acquireContinuousToggleButton.Enabled = true;
      createCustomPropertyButton.Enabled = true;
      clearCustomPropertiesButton.Enabled = true;

      createFifoButton.Enabled = false; //Prevent trying to create the Fifo repeatedly if it was successful

      //Trigger model for a single acquisition triggered via software
      activeFifo.OwnedTriggerParams.TriggerModel = CogAcqTriggerModelConstants.Manual;
      //We will wait until we're actually about to perform an acquisition before enabling triggers
      activeFifo.OwnedTriggerParams.TriggerEnabled = false;
      //setting a large number of buffers, since we might do continuous acquisition
      activeFifo.OwnedGenTLBuffersParams.NumGenTLBuffers = 100;
      //This is mostly just to show that Exposure is a reserved feature with its own interface for setting it as an example of reserved features
      //Unit is ms
      activeFifo.OwnedExposureParams.Exposure = 50;
    }

    private int completeCallCount = 0;
    private int garbageCollectionCounter = 0;
    private int pending = 0;
    private int ready = 0;
    private bool busy = false;

    private void ActiveFifo_Complete(object sender, CogCompleteEventArgs e)
    {
      if (completeCallCount < Int32.MaxValue)
      {//Just in case, unlikely you'd ever hit the max value of an int in this circumstance, but it is good to keep such scenarios in mind
        completeCallCount++;
      }

      ICogImage acquiredImage = null;
      //We are not using these, but they are required by CompleteAcquire
      //See the documentation on CompleteAcquire for further details
      int completedTicket = -1;
      int triggerNumber = -1;

      try
      {
        activeFifo.GetFifoState(out pending, out ready, out busy); //The important value is ready, informing us if any images may be retrieved via CompleteAcquire.
        if (ready > 0) //In theory, this should always be true for our particular application. In practice, it does not hurt to be certain.
        {
          //currentAcquisitionTicket is -1 in the case of continuous, meaning take the oldest buffered acquistion result
          acquiredImage = activeFifo.CompleteAcquire(currentAcquisitionTicket, out completedTicket, out triggerNumber);
        }
      }
      catch (Exception err)
      {
        //We'll get things back in the default state before triggers were initiated
        activeFifo.OwnedTriggerParams.TriggerModel = CogAcqTriggerModelConstants.Manual;
        activeFifo.Flush();

        if (InvokeRequired)
        {//This is needed because this event handler does not execute on the main GUI thread, but we are performing changes to GUI components
          Invoke(new handleCompleteAcquireExceptionDelegate(handleCompleteAcquireException), new object[] { err });
        }

        return;//Don't bother trying to set the display
      }

      if (InvokeRequired)
      {//This is needed because this event handler does not execute on the main GUI thread, but we are performing changes to GUI components
        Invoke(new assignImageToDisplayDelegate(assignImageToDisplay), new object[] { acquiredImage });
      }

      garbageCollectionCounter++;

      //This is important, without the periodic garbage collection calls, the garbage collector will not think it needs to free the relatively small amount of
      //managed memory in use. However, that managed memory is holding alive a much larger amount of unmanaged memory by maintaining a reference to it. 
      //Thus we must manually remind the garbage collector to do a collection periodically in order to work around this limitation of mixed managed/unmanaged code.
      //You do not need to use 4 specifically, but the value that you choose should be relatively small and 4 is generally a reasonable value to keep unmanaged 
      //memory under control.
      if (garbageCollectionCounter > 4)
      {
        garbageCollectionCounter = 0;
        GC.Collect();
      }
    }

    private delegate void handleCompleteAcquireExceptionDelegate(Exception err);

    private void handleCompleteAcquireException(Exception err)
    {
      activeFifo.OwnedTriggerParams.TriggerEnabled = false;
      activeFifo.Flush();
      activeFifo.OwnedTriggerParams.TriggerModel = CogAcqTriggerModelConstants.Manual;

      acquireSinglebutton.Enabled = true;

      if (acquireContinuousToggleButton.Checked)
      {
        acquireContinuousToggleButton.Checked = false;
        acquireContinuousToggleButton.Text = "Begin Continuous Acquisition";
      }

      MessageBox.Show("Exception of type " + err.GetType() + " was encountered while attempting to complete an acquire (Fifo Complete call count: " + 
        completeCallCount +", pending: " + pending + ", ready: " + ready + ", busy: " + busy + ") with the message: " + err.Message);
    }

    private delegate void assignImageToDisplayDelegate(ICogImage image);

    private void assignImageToDisplay(ICogImage image)
    {
      imageDisplay.Image = image;
      imageDisplay.Fit();

      currentAcquisitionTicket = -1;
      if (!acquireContinuousToggleButton.Checked)
      {
        //We enable the trigger each time prior to a single acquisition, so we may as well disable it again after
        activeFifo.OwnedTriggerParams.TriggerEnabled = false;
        acquireSinglebutton.Enabled = true;
      }
    }

    private void getFeatureButton_Click(object sender, EventArgs e)
    {
      try
      {
        ICogFrameGrabber castedCamera = ((ICogFrameGrabber)cameraSelectionComboBox.SelectedItem);
        featureValueTextBox.Text = castedCamera.OwnedGenTLAccess.GetFeature(featureNameTextBox.Text);
      }
      catch (COMException err)
      {
        MessageBox.Show("Error encountered attempting to retrieve the value of the feature with the name '" + featureNameTextBox.Text + 
          "'. Exception message: " + err.Message);
      }
    }

    private void setFeatureButton_Click(object sender, EventArgs e)
    {
      try
      {
        ICogFrameGrabber castedCamera = ((ICogFrameGrabber)cameraSelectionComboBox.SelectedItem);
        castedCamera.OwnedGenTLAccess.SetFeature(featureNameTextBox.Text, featureValueTextBox.Text);
      }
      catch (COMException err)
      {
        MessageBox.Show("Error encountered attempting to write the value '" + featureValueTextBox.Text + 
          "' to the feature with the name '" + featureNameTextBox.Text + "'. Exception message: " + err.Message);
      }
    }

    private void createCustomPropertyButton_Click(object sender, EventArgs e)
    {
      CogCustomPropertyTypeConstants featureType = CogCustomPropertyTypeConstants.TypeUnknown;

      try
      {
        featureType = activeFifo.FrameGrabber.OwnedGenTLAccess.GetFeatureType(featureNameTextBox.Text);
      }
      catch (COMException err)
      {
        MessageBox.Show("Error encountered attempting to retrieve the type of the feature with the name '" + featureNameTextBox.Text + 
          "'. Exception message: " + err.Message);
        return;
      }

      //Custom properties are esentially feature writes that act like reserved features and will be passed to the camera in the same manner
      //when an acquisition occurs. Note that if a custom property is added for a reserved feature, the reserved nature of that feature still 
      //takes priority. Eg. if you create a custom property for ExposureTime, the OwnedExposureParams will still take priority.
      CogAcqCustomProperty customProperty = new CogAcqCustomProperty(featureNameTextBox.Text, featureValueTextBox.Text, featureType);
      //The entire Custom Properties list must be replaced. The existing list cannot be modified "in place"
      List<CogAcqCustomProperty> newCustomProperties = new List<CogAcqCustomProperty>();
      newCustomProperties.Add(customProperty);
      activeFifo.OwnedCustomPropertiesParams.CustomProps = newCustomProperties;
    }

    private void clearCustomPropertiesButton_Click(object sender, EventArgs e)
    {
      activeFifo.OwnedCustomPropertiesParams.CustomProps = new List<CogAcqCustomProperty>();
    }

    private void acquireSinglebutton_Click(object sender, EventArgs e)
    {
      //Re-enable once acquisition completes in the event handler
      //While it would be possible to set up a queue and allow for repeated button clicks, that is beyond the scope of this sample
      acquireSinglebutton.Enabled = false;

      try
      {
        activeFifo.Flush();//Just in case. Should not be any outstanding acquisitions
        activeFifo.OwnedTriggerParams.TriggerEnabled = true;
        currentAcquisitionTicket = activeFifo.StartAcquire();
      }
      catch (Exception err)
      {
        MessageBox.Show("Exception of type " + err.GetType() + " was encountered like attempting to start a single acquire with the message: " + 
          err.Message);
      }
    }

    private void acquireContinuousToggleButton_Click(object sender, EventArgs e)
    {
      if (acquireContinuousToggleButton.Checked)
      {
        acquireSinglebutton.Enabled = false;
        acquireContinuousToggleButton.Text = "Stop Continuous Acquisition";

        activeFifo.Flush();//Just in case. Should not be any outstanding acquisitions
        activeFifo.OwnedTriggerParams.TriggerModel = CogAcqTriggerModelConstants.FreeRun;
        activeFifo.OwnedTriggerParams.TriggerEnabled = true;
      }
      else
      {
        acquireSinglebutton.Enabled = true;
        acquireContinuousToggleButton.Text = "Begin Continuous Acquisition";

        activeFifo.OwnedTriggerParams.TriggerEnabled = false;
        activeFifo.Flush(); //clear out any remaining buffered acquisitions as we leave continuous acq
        activeFifo.OwnedTriggerParams.TriggerModel = CogAcqTriggerModelConstants.Manual;
      }
    }

    //Note that if you try to execute an acquisition initiating command, the image will not be placed into the display, even if you have created a 
    //Fifo. The way in which the Fifo listens for new image data is bypassed by using a command. Value is ignored for commands, they do not have parameters.
    private void executeCommandButton_Click(object sender, EventArgs e)
    {
      CogCustomPropertyTypeConstants featureType = CogCustomPropertyTypeConstants.TypeUnknown;
      ICogFrameGrabber castedCamera = ((ICogFrameGrabber)cameraSelectionComboBox.SelectedItem);


      try
      {
        featureType = castedCamera.OwnedGenTLAccess.GetFeatureType(featureNameTextBox.Text);
      }
      catch (COMException err)
      {
        MessageBox.Show("Error encountered attempting to retrieve the type of the feature with the name '" + featureNameTextBox.Text +
          "'. Exception message: " + err.Message);
        return;
      }

      if (featureType != CogCustomPropertyTypeConstants.TypeCommand)
      {
        MessageBox.Show("The provided feature with name '" + featureNameTextBox.Text + "' is not a Command type feature.");
        return;
      }

      try
      {
        castedCamera.OwnedGenTLAccess.ExecuteCommand(featureNameTextBox.Text);
      }
      catch (COMException err)
      {
        MessageBox.Show("Error encountered attempting to execute the command feature with the name '" + featureNameTextBox.Text + 
          "'. Exception message: " + err.Message);
      }
    }
  }
}
