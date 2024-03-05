'/*******************************************************************************
' Copyright (C) 2007- 2010 Cognex Corporation
'
' Subject to Cognex Corporations terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
'*******************************************************************************
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.
'
' This sample demonstrates acquisition with GigE Vision cameras.
'
' Using the general VisionPro acquisition interface, you can control:
'
'   * exposure;
'   * contrast;
'   * brightness;
'
'
' The VisionPro generic GigE GeniCam feature interface is demonstrated to
' control the following properties in this sample:
'
'   * camera heartbeat;
'   * camera bandwidth;
'   * strobing;
'   * power on behavior;
'
'
' This sample has been qualified with GigE Basler scout cameras.
'
' It may run well with other GigE camera models. However, feature set differences 
' among cameras may produce unforseen behavior with the code used here.
'

Imports Cognex.VisionPro
Imports Cognex.VisionPro.Display
Imports Cognex.VisionPro.FGGigE
Imports Cognex.VisionPro.Exceptions

Namespace GigEVisionPropertySample

  Public Class GigEVisionPropertyForm
    Inherits System.Windows.Forms.Form

#Region " Application Variables and GigE Supporting Functions"
    Private mFifo As ICogAcqFifo = Nothing
    Private mGigEVisionCameras As CogFrameGrabberGigEs = Nothing
    Private mGigEVisionCamera As ICogFrameGrabber = Nothing
    Private mGigECameraAccess As ICogGigEAccess = Nothing
    Private mCurrentCameraIndex As Integer = -1
    Private mCurrentVideoIndex As Integer = -1

    Public Sub GigESetBandwidth(ByRef gigECameraAcess As ICogGigEAccess, ByVal percentageOfBandwidth As Double)
      '100 MBytes / sec overall throughput
      Const maxRate As Double = 100 * 1024 * 1024
      Dim packetSize As Long = gigECameraAcess.GetIntegerFeature("GevSCPSPacketSize")
      Dim packetTime As Double = packetSize / maxRate

      'Use the bandwidth setting to calculate the time it should require to
      'transmit each packet to achieve the desired bandwidth.  For example, a
      'bandwidth setting of 0.25 means we want each packet to take 4 times
      'longer than it would at full speed.
      Dim desiredTime As Double = packetTime / percentageOfBandwidth

      'The different between the desired and actual times is the delay we want
      'between each packet.  Note that until the delay becomes larger than the
      'intrinsic delay between each packet sent by the camera, changes in
      'bandwidth won't have any effect on the data rate.
      Dim delaySec As Double = desiredTime - packetTime

      Dim timeStampFreq As ULong = gigECameraAcess.TimeStampFrequency
      Dim delay As UInteger = CUInt(delaySec * timeStampFreq)
      gigECameraAcess.SetIntegerFeature("GevSCPD", delay)

    End Sub

    Public Sub GigESetStrobe(ByRef gigECameraAcess As ICogGigEAccess, ByVal strobeOn As Boolean)
      If strobeOn Then
        gigECameraAcess.SetFeature("TimerSelector", "Timer1")
        gigECameraAcess.SetFeature("TimerTriggerSource", "ExposureStart")

        'Timer delay shifts the beginning of the pulse relative to exposure start.
        gigECameraAcess.SetDoubleFeature("TimerDelayAbs", 100)

        ' Timer duration controls the width of the pulse
        gigECameraAcess.SetDoubleFeature("TimerDurationAbs", 200)
        gigECameraAcess.SetFeature("LineSelector", "Out1")
        gigECameraAcess.SetFeature("LineSource", "TimerActive")
      Else
        gigECameraAcess.SetFeature("LineSelector", "Out1")
        gigECameraAcess.SetFeature("LineSource", "UserOutput")
        gigECameraAcess.SetFeature("UserOutputSelector", "UserOutput1")
        gigECameraAcess.SetFeature("UserOutputValue", "0")
      End If
    End Sub

    Public Sub GigESetHeartBeat(ByRef gigECameraAcess As ICogGigEAccess, ByVal RateInMs As Double)
      gigECameraAcess.SetIntegerFeature("GevHeartbeatTimeout", CUInt(RateInMs))
    End Sub

    Public Sub GigESetTriggerPolarity(ByRef gigECameraAcess As ICogGigEAccess, ByVal polarityLowToHigh As Boolean)
      'For older versions of Basler cameras, use "High" or "Low"
      'For newer versions of Basler cameras, use "RisingEdge" or "FallingEdge"
      If polarityLowToHigh Then
        'mGigECameraAccess.SetFeature("TriggerActivation", "High")        
        mGigECameraAccess.SetFeature("TriggerActivation", "RisingEdge")
      Else
        'mGigECameraAccess.SetFeature("TriggerActivation", "Low")        
        mGigECameraAccess.SetFeature("TriggerActivation", "FallingEdge")
      End If
    End Sub

    Public Function GigEGetStrobe(ByRef gigECameraAcess As ICogGigEAccess) As Boolean
      Dim strobeOn As Boolean = False
      gigECameraAcess.SetFeature("TimerSelector", "Timer1")
      If (gigECameraAcess.GetFeature("TimerTriggerSource") = "ExposureStart") Then
        gigECameraAcess.SetFeature("LineSelector", "Out1")
        If (gigECameraAcess.GetFeature("LineSource") = "TimerActive") Then
          strobeOn = True
        End If
      End If
      Return strobeOn
    End Function

    Public Function GigEGetHeartBeat(ByRef gigECameraAcess As ICogGigEAccess) As Long
      Return gigECameraAcess.GetIntegerFeature("GevHeartbeatTimeout")
    End Function

    Public Function GigEGetTriggerPolarity(ByRef gigECameraAcess As ICogGigEAccess) As Boolean
      'For older versions of Basler cameras, use "High" or "Low"
      'For newer versions of Basler cameras, use "RisingEdge" or "FallingEdge"
      Dim polarityLowToHigh As Boolean = True
      Dim triggerActivation As String = gigECameraAcess.GetFeature("TriggerActivation")

      If triggerActivation = "RisingEdge" Or triggerActivation = "High" Then
        Return polarityLowToHigh
      ElseIf triggerActivation = "FallingEdge" Or triggerActivation = "Low" Then
        polarityLowToHigh = False
      End If
      Return polarityLowToHigh
    End Function

    Public Shared Sub Main()
      Dim GigEVisionCameras As CogFrameGrabberGigEs = New CogFrameGrabberGigEs
      If GigEVisionCameras.Count = 0 Then
        MessageBox.Show("This sample requires a GigE Vision camera. Please install one and relaunch this sample.", _
                        "No GigE Vision camera found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
      Else
        Application.Run(New GigEVisionPropertyForm)
      End If
    End Sub

#End Region

#Region " GUI and Acquisition Management"

    Public Sub ManageGuiControls(ByVal EnableControls As Boolean)
      'Set acquisition and GigE controls accordingly

      gbUseVProAcqApi.Enabled = EnableControls
      gbSetGigEProperties.Enabled = EnableControls
      lblSerialNumberRunTime.Enabled = EnableControls
      lblFirmwareRunTime.Enabled = EnableControls
      lblCurrentIP.Enabled = EnableControls
      pbStartAcquire.Enabled = EnableControls

      bnApplyBandwidth.Enabled = EnableControls
      bnApplyHeartbeat.Enabled = EnableControls
      cbStrobeOnOff.Enabled = EnableControls
      cbTriggerPolarity.Enabled = EnableControls
      bnSaveUserSet.Enabled = EnableControls
      bnRestoreFactoryDefault.Enabled = EnableControls

    End Sub

    Private Sub InitializeAcquisition()
      ' Associate the display tool and status bars with the display control
      CogDisplayToolbar1.Display = CogDisplay1
      CogDisplayStatusBar1.Display = CogDisplay1

      ' Query for available GigE cameras, and fill in the drop down list.
      mGigEVisionCameras = New CogFrameGrabberGigEs

      cbCamera.Items.Clear()
      For Each CameraItem As ICogFrameGrabber In mGigEVisionCameras
        cbCamera.Items.Add(CameraItem.Name)
      Next
      cbCamera.SelectedIndex = 0
      mCurrentCameraIndex = cbCamera.SelectedIndex

      mGigEVisionCamera = mGigEVisionCameras(cbCamera.SelectedIndex)
      mGigECameraAccess = mGigEVisionCamera.OwnedGigEAccess

      ' Query for supported video formats and fill in the drop down list,
      ' using the pre-selected camera to develop the list
      cbVideoFormat.Items.Clear()
      Try
        For Each VideoFormatName As String In _
        mGigEVisionCameras(cbCamera.SelectedIndex).AvailableVideoFormats
          cbVideoFormat.Items.Add(VideoFormatName)
        Next
      Catch Ex As CogException
        If Ex.Message.Contains("in use") Then
          cbVideoFormat.Items.Add("Camera already in use")
          cbVideoFormat.Enabled = False
          btnConstruct.Enabled = False
        Else
          cbVideoFormat.Items.Add("Exception occurred obtaining video formats")
          cbVideoFormat.Enabled = False
          btnConstruct.Enabled = False
        End If
      End Try
      cbVideoFormat.SelectedIndex = 0

      ' Update the video format index variable
      mCurrentVideoIndex = cbVideoFormat.SelectedIndex

    End Sub

    Private Sub BtnConstruct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConstruct.Click
      ' Get the selected camera and format and try to construct a fifo
      Dim VideoFormat As String = cbVideoFormat.SelectedItem.ToString()
      Try
        mFifo = mGigEVisionCameras(cbCamera.SelectedIndex).CreateAcqFifo(VideoFormat, _
                                CogAcqFifoPixelFormatConstants.Format8Grey, _
                                0, _
                                True)
      Catch except As CogAcqException
        ' Construction failed, display a message box, disable acquisition, and exit.
        MessageBox.Show(except.Message, "Fifo Construction Error", _
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        'textConstructStatus.Text = Except.Message
        Return
      End Try

      ManageGuiControls(True)

      'Manage the fifo: 
      '
      '   1) Disable triggers; 
      '   2) Add Complete Event handler; 
      '   3) Pre queue starts;
      '   4) Inititalize camera
      '
      ' We have hardcoded use of the manual trigger model 
      ' for this sample. To make the sample generic to all
      ' trigger models, we should make all code which calls
      ' SartAcquire() conditional on the trigger model.
      Try
        mFifo.OwnedTriggerParams.TriggerEnabled = False
        mFifo.OwnedTriggerParams.TriggerModel = CogAcqTriggerModelConstants.Manual

        ' Add the complete handler for the fifo
        AddHandler mFifo.Complete, AddressOf CompleteEventHandler

        mFifo.StartAcquire()

        ' Inititalize the current application settings with the 
        ' current camera settings.

        ' If the fifo supports exposure, enable the exposure control.
        If Not mFifo.OwnedExposureParams Is Nothing Then
          nbExposure.Enabled = True
          bnApplyExposure.Enabled = True
          nbExposure.Value = CType(mFifo.OwnedExposureParams.Exposure, Decimal)
        Else
          nbExposure.Enabled = False
        End If

        ' If the fifo supports contrast, enable the contrast control.
        If Not mFifo.OwnedContrastParams Is Nothing Then
          nbContrast.Enabled = True
          bnApplyContrast.Enabled = True
          nbContrast.Value = CType(mFifo.OwnedContrastParams.Contrast, Decimal)
        Else
          nbContrast.Enabled = False
        End If

        ' If the fifo supports brightness, enable the brightness control.
        If Not mFifo.OwnedBrightnessParams Is Nothing Then
          nbBrightness.Enabled = True
          bnApplyBrightness.Enabled = True
          nbBrightness.Value = CType(mFifo.OwnedBrightnessParams.Brightness, Decimal)
        Else
          nbBrightness.Enabled = False
        End If

        ' Inititalize GigE controls with camera settings,
        ' except Bandwidth.
        Try
          nbHeartbeat.Value = GigEGetHeartBeat(mGigECameraAccess)
        Catch ex As CogException
          MessageBox.Show(ex.Message, "Error Obtaining Heartbeat", _
                              MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
          nbHeartbeat.Value = 0
        End Try

        Try
          If GigEGetStrobe(mGigECameraAccess) Then
            cbStrobeOnOff.CheckState = CheckState.Checked
          Else
            cbStrobeOnOff.CheckState = CheckState.Unchecked
          End If
        Catch ex As CogException
          MessageBox.Show(ex.Message, "Error Obtaining Strobe State", _
                              MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
          cbStrobeOnOff.CheckState = CheckState.Unchecked
        End Try

        Try
          If GigEGetTriggerPolarity(mGigECameraAccess) Then
            cbTriggerPolarity.CheckState = CheckState.Checked
          Else
            cbTriggerPolarity.CheckState = CheckState.Unchecked
          End If
        Catch ex As CogException
          MessageBox.Show(ex.Message, "Error Obtaining Trigger Polarity", _
                              MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
          cbTriggerPolarity.CheckState = CheckState.Checked
        End Try


        'Display the camera's serial number
        Try
          lblSerialNumberRunTime.Text = mGigEVisionCamera.SerialNumber
        Catch ex As CogException
          MessageBox.Show(ex.Message, "Error Obtaining Serial Number", _
                              MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
          lblSerialNumberRunTime.Text = "Can't Get Serial Number"
        End Try

        'Display the camera's Firmware identifier
        Try
          lblFirmwareRunTime.Text = mGigECameraAccess.GetFeature("DeviceFirmwareVersion")
        Catch ex As CogException
          MessageBox.Show(ex.Message, "Error Obtaining Firmware", _
                              MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
          lblFirmwareRunTime.Text = "Can't Get Firmware"
        End Try


        'Display the camera's Current IP address
        Try
          lblCurrentIP.Text = mGigECameraAccess.CurrentIPAddress
        Catch ex As CogException
          MessageBox.Show(ex.Message, "Error Obtaining Current IP", _
                              MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
          lblCurrentIP.Text = "Can't Get Current IP"
        End Try

        btnConstruct.Enabled = False

      Catch Ex As CogAcqException
        MessageBox.Show(Ex.Message, "Error Inititalizing Fifo", _
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Return
      Catch Ex As Exception
        MessageBox.Show(Ex.Message, "Error Inititalizing Fifo", _
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Return
      End Try

    End Sub

    Private Sub CompleteEventHandler(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogCompleteEventArgs)
      ' This is the complete event handler for acquisition. It receives an image, 
      ' displays it, then calls StartAcquire if it's okay to do so.

      Static numberOfAcquisitions As Integer = 0

      If InvokeRequired Then
        Dim eventArgs() As Object = {sender, e}
        Invoke(New CogCompleteEventHandler(AddressOf CompleteEventHandler), _
          eventArgs)
        Return
      End If

      Dim numberReady, numberPending As Integer
      Dim IsBusy As Boolean
	  Dim info As New CogAcqInfo
      Try
        mFifo.GetFifoState(numberPending, numberReady, IsBusy)
        If (numberReady > 0) Then
          CogDisplay1.Image = mFifo.CompleteAcquireEx(info)
          numberOfAcquisitions += 1
        End If
        ' We need to run the garbage collector on occasion to cleanup
        ' images that are no longer being used.
        If numberOfAcquisitions > 4 Then
          GC.Collect()
          numberOfAcquisitions = 0
        End If

        ' Make sure images waiting plus starts is less than maximum allowed, 32 
        If (numberReady + numberPending < 30) Then
          mFifo.StartAcquire()
        End If

        ' This call is suitable for single camera demonstration 
        ' purposes. Calling DoEvents() in the a CompleteEvent
        ' handler can be problematical for multi camera 
        ' applications.
        Application.DoEvents()

      Catch ex As CogException
        MessageBox.Show(ex.Message, "Error In CompleteEvent ", _
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
      Catch ex As Exception
        MessageBox.Show(ex.Message, "Error In CompleteEvent ", _
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
      End Try

    End Sub

    Private Sub cbCamera_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCamera.SelectedIndexChanged
      ' Do nothing if the user selects what is already in use
      If mCurrentCameraIndex = cbCamera.SelectedIndex Then
        Return
      End If

      ' If acquisition is occurring, stop it
      If pbStartAcquire.Enabled And pbStartAcquire.Checked Then
        pbStartAcquire.CheckState = CheckState.Unchecked
      End If

      ' Erase old data in text boxes
      lblSerialNumberRunTime.Text = ""
      lblFirmwareRunTime.Text = ""
      lblCurrentIP.Text = ""

      ' Turn off all acq controls until a new fifo is made
      ManageGuiControls(False)

      ' Enable the construction button
      btnConstruct.Enabled = True

      ' Update the current camera index variable
      mCurrentCameraIndex = cbCamera.SelectedIndex

      ' Update camera, too
      mGigEVisionCamera = mGigEVisionCameras(cbCamera.SelectedIndex)
      mGigECameraAccess = mGigEVisionCamera.OwnedGigEAccess

      ' Repopulate the video formats
      cbVideoFormat.Items.Clear()
      Try
        For Each VideoFormatName As String In _
        mGigEVisionCameras(cbCamera.SelectedIndex).AvailableVideoFormats
          cbVideoFormat.Items.Add(VideoFormatName)
        Next
      Catch Ex As CogException
        If Ex.Message.Contains("in use") Then
          cbVideoFormat.Items.Add("Camera already in use")
          cbVideoFormat.Enabled = False
          btnConstruct.Enabled = False
        Else
          cbVideoFormat.Items.Add("Exception occurred obtaining video formats")
          cbVideoFormat.Enabled = False
          btnConstruct.Enabled = False
        End If
      End Try
      cbVideoFormat.SelectedIndex = 0

      ' Update the video format index variable
      mCurrentVideoIndex = cbVideoFormat.SelectedIndex

    End Sub
    Private Sub cbVideoFormat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCamera.SelectedIndexChanged
      ' Do nothing if the user selects what is already in use
      If mCurrentVideoIndex = cbVideoFormat.SelectedIndex Then
        Return
      End If

      ' If acquisition is occurring, stop it
      If pbStartAcquire.Enabled And pbStartAcquire.Checked Then
        pbStartAcquire.CheckState = CheckState.Unchecked
      End If

      ' Erase old data in text boxes
      lblSerialNumberRunTime.Text = ""
      lblFirmwareRunTime.Text = ""
      lblCurrentIP.Text = ""

      ' Turn off all acq controls until a new fifo is made
      ManageGuiControls(False)

      ' Enable the construction button
      btnConstruct.Enabled = True

      ' Update the current video format index variable 
      mCurrentVideoIndex = cbVideoFormat.SelectedIndex

    End Sub

    Private Sub GigEVisionPropertyForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

      ' Stop acquiring (includes flush)
      pbStartAcquire.CheckState = CheckState.Unchecked

      ' Remove Image from CogDisplay
      CogDisplay1.Image = Nothing

      ' Remove the complete handler for the fifo
      If Not mFifo Is Nothing Then
        RemoveHandler mFifo.Complete, AddressOf CompleteEventHandler
      End If

    End Sub
#End Region

#Region " VisionPro GigE Vision Acquisition Code"
    Private Sub cbTriggerPolarity_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTriggerPolarity.CheckedChanged
      Try
        GigESetTriggerPolarity(mGigECameraAccess, cbTriggerPolarity.Checked)
      Catch ex As CogException
        MessageBox.Show(ex.Message, "Error In GigESetTriggerPolarity: ", _
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
      End Try
    End Sub

    Private Sub bnApplyBandwidth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnApplyBandwidth.Click
      Dim bandWidthPercentage As Double = nbBandwidth.Value / 100.0
      Try
        GigESetBandwidth(mGigECameraAccess, bandWidthPercentage)
      Catch ex As CogException
        MessageBox.Show(ex.Message, "Error In GigESetBandwidth: ", _
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
      End Try
    End Sub

    Private Sub cbStrobeOnOff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbStrobeOnOff.CheckedChanged
      ' This code uses the camera's first output line to deliver a strobe pulse when
      ' acquisition begins on the camera.

      Try
        GigESetStrobe(mGigECameraAccess, cbStrobeOnOff.CheckState)
      Catch ex As CogException
        MessageBox.Show(ex.Message, "Error In GigESetStrobe: ", _
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
      End Try

    End Sub

    Private Sub bnApplyHeartbeat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnApplyHeartbeat.Click

      Try
        GigESetHeartBeat(mGigECameraAccess, nbHeartbeat.Value)
      Catch ex As CogException
        MessageBox.Show(ex.Message, "Error In GigESetHeartbeat: ", _
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
      End Try
    End Sub

    Private Sub pbStartAcquire_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbStartAcquire.CheckedChanged
      If pbStartAcquire.Checked Then
        mFifo.OwnedTriggerParams.TriggerEnabled = True
        pbStartAcquire.Text = "Stop Acquire"
        mFifo.StartAcquire()
      Else
        mFifo.OwnedTriggerParams.TriggerEnabled = False
        mFifo.Flush()
        pbStartAcquire.Text = "Start Acquire"
      End If
    End Sub

    Private Sub bnSaveUserSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnSaveUserSet.Click
      ' Save the current settings to the camera's first user set
      Try

        mGigECameraAccess.SetFeature("UserSetSelector", "UserSet1")
        mGigECameraAccess.ExecuteCommand("UserSetSave")

        ' Choose UserSet1 to use as the power-on default
        mGigECameraAccess.SetFeature("UserSetDefaultSelector", "UserSet1")
      Catch ex As CogException
        MessageBox.Show(ex.Message, "Error In UserSetSave: ", _
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
      End Try

    End Sub

    Private Sub bnRestoreFactoryDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnRestoreFactoryDefault.Click
      ' Choose factory default settings to use as the power-on default
      Try
        mGigECameraAccess.SetFeature("UserSetDefaultSelector", "Default")
      Catch ex As Exception
        MessageBox.Show(ex.Message, "Error In RestoreFactoryDefault: ", _
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
      End Try
    End Sub
#End Region

#Region " VisionPro Acquisition Code"
    Private Sub bnApplyExposure_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnApplyExposure.Click
      mFifo.OwnedExposureParams.Exposure = nbExposure.Value
    End Sub

    Private Sub bnApplyContrast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnApplyContrast.Click
      mFifo.OwnedContrastParams.Contrast = nbContrast.Value
    End Sub

    Private Sub bnApplyBrightness_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnApplyBrightness.Click
      mFifo.OwnedBrightnessParams.Brightness = nbContrast.Value
    End Sub
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      InitializeAcquisition()

      Me.Show()

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
      If disposing Then
        If Not (components Is Nothing) Then
          components.Dispose()
        End If
        Dim frameGrabbers As New CogFrameGrabbers
        For Each fg As Cognex.VisionPro.ICogFrameGrabber In frameGrabbers
          fg.Disconnect(False)
        Next
      End If
      MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents gbMakeFifo As System.Windows.Forms.GroupBox
    Friend WithEvents cbVideoFormat As System.Windows.Forms.ComboBox
    Friend WithEvents btnConstruct As System.Windows.Forms.Button
    Friend WithEvents textConstructStatus As System.Windows.Forms.TextBox
    Friend WithEvents gbUseVProAcqApi As System.Windows.Forms.GroupBox
    Friend WithEvents nbBrightness As Cognex.VisionPro.CogNumberBox
    Friend WithEvents nbExposure As Cognex.VisionPro.CogNumberBox
    Friend WithEvents lblContrast As System.Windows.Forms.TextBox
    Friend WithEvents lblBrightness As System.Windows.Forms.TextBox
    Friend WithEvents nbContrast As Cognex.VisionPro.CogNumberBox
    Friend WithEvents lblExposure As System.Windows.Forms.TextBox
    Friend WithEvents txtGreeting As System.Windows.Forms.TextBox
    Friend WithEvents CogDisplay1 As Cognex.VisionPro.Display.CogDisplay
        Friend WithEvents CogDisplayStatusBar1 As Cognex.VisionPro.CogDisplayStatusBarV2
    Friend WithEvents cbCamera As System.Windows.Forms.ComboBox
    Friend WithEvents txtSelectVF As System.Windows.Forms.TextBox
    Friend WithEvents txtSelectFG As System.Windows.Forms.TextBox
    Friend WithEvents gbSetGigEProperties As System.Windows.Forms.GroupBox
    Friend WithEvents bnApplyHeartbeat As System.Windows.Forms.Button
    Friend WithEvents lblBandwidth As System.Windows.Forms.Label
    Friend WithEvents bnApplyBandwidth As System.Windows.Forms.Button
    Friend WithEvents cbStrobeOnOff As System.Windows.Forms.CheckBox
    Friend WithEvents lblHeartbeat As System.Windows.Forms.Label
    Friend WithEvents nbBandwidth As System.Windows.Forms.NumericUpDown
    Friend WithEvents nbHeartbeat As System.Windows.Forms.NumericUpDown
    Friend WithEvents cbTriggerPolarity As System.Windows.Forms.CheckBox
    Friend WithEvents bnApplyBrightness As System.Windows.Forms.Button
    Friend WithEvents bnApplyContrast As System.Windows.Forms.Button
    Friend WithEvents bnApplyExposure As System.Windows.Forms.Button
    Friend WithEvents lblSerialNumberTitle As System.Windows.Forms.Label
    Friend WithEvents bnSaveUserSet As System.Windows.Forms.Button
    Friend WithEvents bnRestoreFactoryDefault As System.Windows.Forms.Button
    Friend WithEvents lblFirmwareTitle As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblSerialNumberRunTime As System.Windows.Forms.Label
    Friend WithEvents lblFirmwareRunTime As System.Windows.Forms.Label
    Friend WithEvents lblCurrentIP As System.Windows.Forms.Label
    Friend WithEvents lblCurrentIPTitle As System.Windows.Forms.Label
    Friend WithEvents pbStartAcquire As System.Windows.Forms.CheckBox
        Friend WithEvents CogDisplayToolbar1 As Cognex.VisionPro.CogDisplayToolbarV2
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GigEVisionPropertyForm))
      Me.gbMakeFifo = New System.Windows.Forms.GroupBox
      Me.txtSelectVF = New System.Windows.Forms.TextBox
      Me.txtSelectFG = New System.Windows.Forms.TextBox
      Me.cbCamera = New System.Windows.Forms.ComboBox
      Me.cbVideoFormat = New System.Windows.Forms.ComboBox
      Me.btnConstruct = New System.Windows.Forms.Button
      Me.textConstructStatus = New System.Windows.Forms.TextBox
      Me.gbUseVProAcqApi = New System.Windows.Forms.GroupBox
      Me.bnApplyBrightness = New System.Windows.Forms.Button
      Me.bnApplyContrast = New System.Windows.Forms.Button
      Me.bnApplyExposure = New System.Windows.Forms.Button
      Me.nbBrightness = New Cognex.VisionPro.CogNumberBox
      Me.nbExposure = New Cognex.VisionPro.CogNumberBox
      Me.lblContrast = New System.Windows.Forms.TextBox
      Me.lblBrightness = New System.Windows.Forms.TextBox
      Me.nbContrast = New Cognex.VisionPro.CogNumberBox
      Me.lblExposure = New System.Windows.Forms.TextBox
      Me.txtGreeting = New System.Windows.Forms.TextBox
      Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
            Me.CogDisplayStatusBar1 = New Cognex.VisionPro.CogDisplayStatusBarV2
            Me.CogDisplayToolbar1 = New Cognex.VisionPro.CogDisplayToolbarV2
      Me.gbSetGigEProperties = New System.Windows.Forms.GroupBox
      Me.Label1 = New System.Windows.Forms.Label
      Me.bnRestoreFactoryDefault = New System.Windows.Forms.Button
      Me.bnSaveUserSet = New System.Windows.Forms.Button
      Me.bnApplyHeartbeat = New System.Windows.Forms.Button
      Me.lblBandwidth = New System.Windows.Forms.Label
      Me.bnApplyBandwidth = New System.Windows.Forms.Button
      Me.cbStrobeOnOff = New System.Windows.Forms.CheckBox
      Me.lblHeartbeat = New System.Windows.Forms.Label
      Me.nbBandwidth = New System.Windows.Forms.NumericUpDown
      Me.nbHeartbeat = New System.Windows.Forms.NumericUpDown
      Me.cbTriggerPolarity = New System.Windows.Forms.CheckBox
      Me.lblSerialNumberTitle = New System.Windows.Forms.Label
      Me.lblFirmwareTitle = New System.Windows.Forms.Label
      Me.lblSerialNumberRunTime = New System.Windows.Forms.Label
      Me.lblFirmwareRunTime = New System.Windows.Forms.Label
      Me.lblCurrentIP = New System.Windows.Forms.Label
      Me.lblCurrentIPTitle = New System.Windows.Forms.Label
      Me.pbStartAcquire = New System.Windows.Forms.CheckBox
      Me.gbMakeFifo.SuspendLayout()
      Me.gbUseVProAcqApi.SuspendLayout()
      CType(Me.nbBrightness, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.nbExposure, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.nbContrast, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.gbSetGigEProperties.SuspendLayout()
      CType(Me.nbBandwidth, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.nbHeartbeat, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'gbMakeFifo
      '
      Me.gbMakeFifo.Controls.Add(Me.txtSelectVF)
      Me.gbMakeFifo.Controls.Add(Me.txtSelectFG)
      Me.gbMakeFifo.Controls.Add(Me.cbCamera)
      Me.gbMakeFifo.Controls.Add(Me.cbVideoFormat)
      Me.gbMakeFifo.Controls.Add(Me.btnConstruct)
      Me.gbMakeFifo.Controls.Add(Me.textConstructStatus)
      Me.gbMakeFifo.Location = New System.Drawing.Point(8, 118)
      Me.gbMakeFifo.Name = "gbMakeFifo"
      Me.gbMakeFifo.Size = New System.Drawing.Size(288, 152)
      Me.gbMakeFifo.TabIndex = 7
      Me.gbMakeFifo.TabStop = False
      Me.gbMakeFifo.Text = "Construct an Acquisition Fifo"
      '
      'txtSelectVF
      '
      Me.txtSelectVF.BorderStyle = System.Windows.Forms.BorderStyle.None
      Me.txtSelectVF.Location = New System.Drawing.Point(11, 71)
      Me.txtSelectVF.Name = "txtSelectVF"
      Me.txtSelectVF.ReadOnly = True
      Me.txtSelectVF.Size = New System.Drawing.Size(120, 13)
      Me.txtSelectVF.TabIndex = 10
      Me.txtSelectVF.Text = "Video Format:"
      '
      'txtSelectFG
      '
      Me.txtSelectFG.BorderStyle = System.Windows.Forms.BorderStyle.None
      Me.txtSelectFG.Location = New System.Drawing.Point(11, 25)
      Me.txtSelectFG.Name = "txtSelectFG"
      Me.txtSelectFG.ReadOnly = True
      Me.txtSelectFG.Size = New System.Drawing.Size(100, 13)
      Me.txtSelectFG.TabIndex = 9
      Me.txtSelectFG.Text = "Camera:"
      '
      'cbCamera
      '
      Me.cbCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
      Me.cbCamera.DropDownWidth = 269
      Me.cbCamera.Location = New System.Drawing.Point(11, 44)
      Me.cbCamera.Name = "cbCamera"
      Me.cbCamera.Size = New System.Drawing.Size(269, 21)
      Me.cbCamera.TabIndex = 8
      '
      'cbVideoFormat
      '
      Me.cbVideoFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
      Me.cbVideoFormat.DropDownWidth = 269
      Me.cbVideoFormat.Location = New System.Drawing.Point(11, 87)
      Me.cbVideoFormat.Name = "cbVideoFormat"
      Me.cbVideoFormat.Size = New System.Drawing.Size(269, 21)
      Me.cbVideoFormat.TabIndex = 2
      '
      'btnConstruct
      '
      Me.btnConstruct.Location = New System.Drawing.Point(205, 119)
      Me.btnConstruct.Name = "btnConstruct"
      Me.btnConstruct.Size = New System.Drawing.Size(75, 23)
      Me.btnConstruct.TabIndex = 5
      Me.btnConstruct.Text = "Make Fifo"
      '
      'textConstructStatus
      '
      Me.textConstructStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
      Me.textConstructStatus.Location = New System.Drawing.Point(11, 119)
      Me.textConstructStatus.Multiline = True
      Me.textConstructStatus.Name = "textConstructStatus"
      Me.textConstructStatus.ReadOnly = True
      Me.textConstructStatus.Size = New System.Drawing.Size(165, 27)
      Me.textConstructStatus.TabIndex = 7
      Me.textConstructStatus.Text = "Select a Camera, Video Format " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and click Make Fifo."
      '
      'gbUseVProAcqApi
      '
      Me.gbUseVProAcqApi.Controls.Add(Me.bnApplyBrightness)
      Me.gbUseVProAcqApi.Controls.Add(Me.bnApplyContrast)
      Me.gbUseVProAcqApi.Controls.Add(Me.bnApplyExposure)
      Me.gbUseVProAcqApi.Controls.Add(Me.nbBrightness)
      Me.gbUseVProAcqApi.Controls.Add(Me.nbExposure)
      Me.gbUseVProAcqApi.Controls.Add(Me.lblContrast)
      Me.gbUseVProAcqApi.Controls.Add(Me.lblBrightness)
      Me.gbUseVProAcqApi.Controls.Add(Me.nbContrast)
      Me.gbUseVProAcqApi.Controls.Add(Me.lblExposure)
      Me.gbUseVProAcqApi.Enabled = False
      Me.gbUseVProAcqApi.Location = New System.Drawing.Point(8, 288)
      Me.gbUseVProAcqApi.Name = "gbUseVProAcqApi"
      Me.gbUseVProAcqApi.Size = New System.Drawing.Size(288, 161)
      Me.gbUseVProAcqApi.TabIndex = 10
      Me.gbUseVProAcqApi.TabStop = False
      Me.gbUseVProAcqApi.Text = "Set Acquisition Properties"
      '
      'bnApplyBrightness
      '
      Me.bnApplyBrightness.Enabled = False
      Me.bnApplyBrightness.Location = New System.Drawing.Point(79, 119)
      Me.bnApplyBrightness.Name = "bnApplyBrightness"
      Me.bnApplyBrightness.Size = New System.Drawing.Size(65, 24)
      Me.bnApplyBrightness.TabIndex = 25
      Me.bnApplyBrightness.Text = "Apply"
      Me.bnApplyBrightness.UseVisualStyleBackColor = True
      '
      'bnApplyContrast
      '
      Me.bnApplyContrast.Enabled = False
      Me.bnApplyContrast.Location = New System.Drawing.Point(79, 77)
      Me.bnApplyContrast.Name = "bnApplyContrast"
      Me.bnApplyContrast.Size = New System.Drawing.Size(65, 24)
      Me.bnApplyContrast.TabIndex = 24
      Me.bnApplyContrast.Text = "Apply"
      Me.bnApplyContrast.UseVisualStyleBackColor = True
      '
      'bnApplyExposure
      '
      Me.bnApplyExposure.Enabled = False
      Me.bnApplyExposure.Location = New System.Drawing.Point(79, 37)
      Me.bnApplyExposure.Name = "bnApplyExposure"
      Me.bnApplyExposure.Size = New System.Drawing.Size(65, 24)
      Me.bnApplyExposure.TabIndex = 23
      Me.bnApplyExposure.Text = "Apply"
      Me.bnApplyExposure.UseVisualStyleBackColor = True
      '
      'nbBrightness
      '
      Me.nbBrightness.DecimalPlaces = 2
      Me.nbBrightness.Electric = False
      Me.nbBrightness.Enabled = False
      Me.nbBrightness.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
      Me.nbBrightness.Location = New System.Drawing.Point(8, 119)
      Me.nbBrightness.Maximum = New Decimal(New Integer() {10, 0, 0, 65536})
      Me.nbBrightness.Name = "nbBrightness"
      Me.nbBrightness.Path = Nothing
      Me.nbBrightness.ShowToolTips = False
      Me.nbBrightness.Size = New System.Drawing.Size(59, 20)
      Me.nbBrightness.Subject = Nothing
      Me.nbBrightness.TabIndex = 8
      Me.nbBrightness.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
      Me.nbBrightness.ToolTipText = Nothing
      Me.nbBrightness.UseAngleUnit = False
      Me.nbBrightness.Value = New Decimal(New Integer() {5, 0, 0, 65536})
      '
      'nbExposure
      '
      Me.nbExposure.DecimalPlaces = 2
      Me.nbExposure.Electric = False
      Me.nbExposure.Enabled = False
      Me.nbExposure.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
      Me.nbExposure.Location = New System.Drawing.Point(8, 37)
      Me.nbExposure.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
      Me.nbExposure.Name = "nbExposure"
      Me.nbExposure.Path = Nothing
      Me.nbExposure.ShowToolTips = False
      Me.nbExposure.Size = New System.Drawing.Size(59, 20)
      Me.nbExposure.Subject = Nothing
      Me.nbExposure.TabIndex = 4
      Me.nbExposure.Tag = ""
      Me.nbExposure.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
      Me.nbExposure.ThousandsSeparator = True
      Me.nbExposure.ToolTipText = "Sets Exposure"
      Me.nbExposure.UseAngleUnit = False
      Me.nbExposure.Value = New Decimal(New Integer() {1, 0, 0, 0})
      '
      'lblContrast
      '
      Me.lblContrast.BorderStyle = System.Windows.Forms.BorderStyle.None
      Me.lblContrast.Location = New System.Drawing.Point(11, 63)
      Me.lblContrast.Name = "lblContrast"
      Me.lblContrast.ReadOnly = True
      Me.lblContrast.Size = New System.Drawing.Size(52, 13)
      Me.lblContrast.TabIndex = 7
      Me.lblContrast.Text = "Contrast"
      '
      'lblBrightness
      '
      Me.lblBrightness.BorderStyle = System.Windows.Forms.BorderStyle.None
      Me.lblBrightness.Location = New System.Drawing.Point(8, 103)
      Me.lblBrightness.Name = "lblBrightness"
      Me.lblBrightness.ReadOnly = True
      Me.lblBrightness.Size = New System.Drawing.Size(52, 13)
      Me.lblBrightness.TabIndex = 7
      Me.lblBrightness.Text = "Brightness"
      Me.lblBrightness.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
      '
      'nbContrast
      '
      Me.nbContrast.DecimalPlaces = 2
      Me.nbContrast.Electric = False
      Me.nbContrast.Enabled = False
      Me.nbContrast.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
      Me.nbContrast.Location = New System.Drawing.Point(8, 77)
      Me.nbContrast.Maximum = New Decimal(New Integer() {10, 0, 0, 65536})
      Me.nbContrast.Name = "nbContrast"
      Me.nbContrast.Path = Nothing
      Me.nbContrast.ShowToolTips = False
      Me.nbContrast.Size = New System.Drawing.Size(59, 20)
      Me.nbContrast.Subject = Nothing
      Me.nbContrast.TabIndex = 8
      Me.nbContrast.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
      Me.nbContrast.ToolTipText = Nothing
      Me.nbContrast.UseAngleUnit = False
      Me.nbContrast.Value = New Decimal(New Integer() {5, 0, 0, 65536})
      '
      'lblExposure
      '
      Me.lblExposure.BorderStyle = System.Windows.Forms.BorderStyle.None
      Me.lblExposure.Location = New System.Drawing.Point(11, 18)
      Me.lblExposure.Name = "lblExposure"
      Me.lblExposure.ReadOnly = True
      Me.lblExposure.Size = New System.Drawing.Size(74, 13)
      Me.lblExposure.TabIndex = 7
      Me.lblExposure.Text = "Exposure (ms)"
      '
      'txtGreeting
      '
      Me.txtGreeting.Location = New System.Drawing.Point(16, 16)
      Me.txtGreeting.Multiline = True
      Me.txtGreeting.Name = "txtGreeting"
      Me.txtGreeting.ReadOnly = True
      Me.txtGreeting.Size = New System.Drawing.Size(280, 91)
      Me.txtGreeting.TabIndex = 11
      Me.txtGreeting.Text = resources.GetString("txtGreeting.Text")
      '
      'CogDisplay1
      '
      Me.CogDisplay1.Location = New System.Drawing.Point(304, 8)
      Me.CogDisplay1.Name = "CogDisplay1"
      Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
      Me.CogDisplay1.Size = New System.Drawing.Size(532, 400)
      Me.CogDisplay1.TabIndex = 12
      '
      'CogDisplayStatusBar1
      '
      Me.CogDisplayStatusBar1.Enabled = True
      Me.CogDisplayStatusBar1.Location = New System.Drawing.Point(548, 423)
      Me.CogDisplayStatusBar1.Name = "CogDisplayStatusBar1"
            Me.CogDisplayStatusBar1.Size = New System.Drawing.Size(288, 21)
      Me.CogDisplayStatusBar1.TabIndex = 14
      '
      'CogDisplayToolbar1
      '
      Me.CogDisplayToolbar1.Enabled = True
      Me.CogDisplayToolbar1.Location = New System.Drawing.Point(304, 423)
      Me.CogDisplayToolbar1.Name = "CogDisplayToolbar1"
            Me.CogDisplayToolbar1.Size = New System.Drawing.Size(214, 26)
      Me.CogDisplayToolbar1.TabIndex = 13
      '
      'gbSetGigEProperties
      '
      Me.gbSetGigEProperties.Controls.Add(Me.Label1)
      Me.gbSetGigEProperties.Controls.Add(Me.bnRestoreFactoryDefault)
      Me.gbSetGigEProperties.Controls.Add(Me.bnSaveUserSet)
      Me.gbSetGigEProperties.Controls.Add(Me.bnApplyHeartbeat)
      Me.gbSetGigEProperties.Controls.Add(Me.lblBandwidth)
      Me.gbSetGigEProperties.Controls.Add(Me.bnApplyBandwidth)
      Me.gbSetGigEProperties.Controls.Add(Me.cbStrobeOnOff)
      Me.gbSetGigEProperties.Controls.Add(Me.lblHeartbeat)
      Me.gbSetGigEProperties.Controls.Add(Me.nbBandwidth)
      Me.gbSetGigEProperties.Controls.Add(Me.nbHeartbeat)
      Me.gbSetGigEProperties.Controls.Add(Me.cbTriggerPolarity)
      Me.gbSetGigEProperties.Enabled = False
      Me.gbSetGigEProperties.Location = New System.Drawing.Point(8, 455)
      Me.gbSetGigEProperties.Name = "gbSetGigEProperties"
      Me.gbSetGigEProperties.Size = New System.Drawing.Size(365, 176)
      Me.gbSetGigEProperties.TabIndex = 15
      Me.gbSetGigEProperties.TabStop = False
      Me.gbSetGigEProperties.Text = "Set GigE Vision Properties"
      '
      'Label1
      '
      Me.Label1.AutoSize = True
      Me.Label1.Location = New System.Drawing.Point(189, 21)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(122, 13)
      Me.Label1.TabIndex = 26
      Me.Label1.Text = "Power On Management:"
      '
      'bnRestoreFactoryDefault
      '
      Me.bnRestoreFactoryDefault.Enabled = False
      Me.bnRestoreFactoryDefault.Location = New System.Drawing.Point(192, 102)
      Me.bnRestoreFactoryDefault.Name = "bnRestoreFactoryDefault"
      Me.bnRestoreFactoryDefault.Size = New System.Drawing.Size(145, 47)
      Me.bnRestoreFactoryDefault.TabIndex = 25
      Me.bnRestoreFactoryDefault.Text = "Restore Factory Defaults"
      '
      'bnSaveUserSet
      '
      Me.bnSaveUserSet.Enabled = False
      Me.bnSaveUserSet.Location = New System.Drawing.Point(192, 39)
      Me.bnSaveUserSet.Name = "bnSaveUserSet"
      Me.bnSaveUserSet.Size = New System.Drawing.Size(145, 44)
      Me.bnSaveUserSet.TabIndex = 24
      Me.bnSaveUserSet.Text = "Save Current Settings"
      '
      'bnApplyHeartbeat
      '
      Me.bnApplyHeartbeat.Enabled = False
      Me.bnApplyHeartbeat.Location = New System.Drawing.Point(79, 86)
      Me.bnApplyHeartbeat.Name = "bnApplyHeartbeat"
      Me.bnApplyHeartbeat.Size = New System.Drawing.Size(65, 24)
      Me.bnApplyHeartbeat.TabIndex = 23
      Me.bnApplyHeartbeat.Text = "Apply"
      Me.bnApplyHeartbeat.UseVisualStyleBackColor = True
      '
      'lblBandwidth
      '
      Me.lblBandwidth.AutoSize = True
      Me.lblBandwidth.Location = New System.Drawing.Point(8, 21)
      Me.lblBandwidth.Name = "lblBandwidth"
      Me.lblBandwidth.Size = New System.Drawing.Size(149, 13)
      Me.lblBandwidth.TabIndex = 19
      Me.lblBandwidth.Text = "Camera % of Total Bandwidth:"
      '
      'bnApplyBandwidth
      '
      Me.bnApplyBandwidth.Enabled = False
      Me.bnApplyBandwidth.Location = New System.Drawing.Point(79, 39)
      Me.bnApplyBandwidth.Name = "bnApplyBandwidth"
      Me.bnApplyBandwidth.Size = New System.Drawing.Size(65, 24)
      Me.bnApplyBandwidth.TabIndex = 22
      Me.bnApplyBandwidth.Text = "Apply"
      Me.bnApplyBandwidth.UseVisualStyleBackColor = True
      '
      'cbStrobeOnOff
      '
      Me.cbStrobeOnOff.AutoSize = True
      Me.cbStrobeOnOff.Enabled = False
      Me.cbStrobeOnOff.Location = New System.Drawing.Point(13, 118)
      Me.cbStrobeOnOff.Name = "cbStrobeOnOff"
      Me.cbStrobeOnOff.Size = New System.Drawing.Size(74, 17)
      Me.cbStrobeOnOff.TabIndex = 16
      Me.cbStrobeOnOff.Text = "Strobe On"
      Me.cbStrobeOnOff.UseVisualStyleBackColor = True
      '
      'lblHeartbeat
      '
      Me.lblHeartbeat.AutoSize = True
      Me.lblHeartbeat.Location = New System.Drawing.Point(8, 70)
      Me.lblHeartbeat.Name = "lblHeartbeat"
      Me.lblHeartbeat.Size = New System.Drawing.Size(125, 13)
      Me.lblHeartbeat.TabIndex = 21
      Me.lblHeartbeat.Text = "Heartbeat Timeout in ms:"
      '
      'nbBandwidth
      '
      Me.nbBandwidth.Increment = New Decimal(New Integer() {5, 0, 0, 0})
      Me.nbBandwidth.InterceptArrowKeys = False
      Me.nbBandwidth.Location = New System.Drawing.Point(11, 39)
      Me.nbBandwidth.Name = "nbBandwidth"
      Me.nbBandwidth.Size = New System.Drawing.Size(56, 20)
      Me.nbBandwidth.TabIndex = 17
      Me.nbBandwidth.Tag = ""
      Me.nbBandwidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
      Me.nbBandwidth.Value = New Decimal(New Integer() {100, 0, 0, 0})
      '
      'nbHeartbeat
      '
      Me.nbHeartbeat.Increment = New Decimal(New Integer() {200, 0, 0, 0})
      Me.nbHeartbeat.InterceptArrowKeys = False
      Me.nbHeartbeat.Location = New System.Drawing.Point(11, 88)
      Me.nbHeartbeat.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
      Me.nbHeartbeat.Name = "nbHeartbeat"
      Me.nbHeartbeat.Size = New System.Drawing.Size(56, 20)
      Me.nbHeartbeat.TabIndex = 20
      Me.nbHeartbeat.Tag = ""
      Me.nbHeartbeat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
      '
      'cbTriggerPolarity
      '
      Me.cbTriggerPolarity.AutoSize = True
      Me.cbTriggerPolarity.Enabled = False
      Me.cbTriggerPolarity.Location = New System.Drawing.Point(13, 141)
      Me.cbTriggerPolarity.Name = "cbTriggerPolarity"
      Me.cbTriggerPolarity.Size = New System.Drawing.Size(123, 17)
      Me.cbTriggerPolarity.TabIndex = 18
      Me.cbTriggerPolarity.Text = "Trigger Low To High"
      Me.cbTriggerPolarity.UseVisualStyleBackColor = True
      '
      'lblSerialNumberTitle
      '
      Me.lblSerialNumberTitle.AutoSize = True
      Me.lblSerialNumberTitle.Location = New System.Drawing.Point(395, 476)
      Me.lblSerialNumberTitle.Name = "lblSerialNumberTitle"
      Me.lblSerialNumberTitle.Size = New System.Drawing.Size(115, 13)
      Me.lblSerialNumberTitle.TabIndex = 20
      Me.lblSerialNumberTitle.Text = "Camera Serial Number:"
      '
      'lblFirmwareTitle
      '
      Me.lblFirmwareTitle.AutoSize = True
      Me.lblFirmwareTitle.Location = New System.Drawing.Point(395, 525)
      Me.lblFirmwareTitle.Name = "lblFirmwareTitle"
      Me.lblFirmwareTitle.Size = New System.Drawing.Size(134, 13)
      Me.lblFirmwareTitle.TabIndex = 21
      Me.lblFirmwareTitle.Text = "Camera Firmware Identifier:"
      '
      'lblSerialNumberRunTime
      '
      Me.lblSerialNumberRunTime.BackColor = System.Drawing.SystemColors.ControlLight
      Me.lblSerialNumberRunTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
      Me.lblSerialNumberRunTime.Enabled = False
      Me.lblSerialNumberRunTime.Location = New System.Drawing.Point(398, 494)
      Me.lblSerialNumberRunTime.Name = "lblSerialNumberRunTime"
      Me.lblSerialNumberRunTime.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
      Me.lblSerialNumberRunTime.Size = New System.Drawing.Size(238, 23)
      Me.lblSerialNumberRunTime.TabIndex = 22
      '
      'lblFirmwareRunTime
      '
      Me.lblFirmwareRunTime.BackColor = System.Drawing.SystemColors.ControlLight
      Me.lblFirmwareRunTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
      Me.lblFirmwareRunTime.Enabled = False
      Me.lblFirmwareRunTime.Location = New System.Drawing.Point(398, 546)
      Me.lblFirmwareRunTime.Name = "lblFirmwareRunTime"
      Me.lblFirmwareRunTime.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
      Me.lblFirmwareRunTime.Size = New System.Drawing.Size(238, 23)
      Me.lblFirmwareRunTime.TabIndex = 23
      '
      'lblCurrentIP
      '
      Me.lblCurrentIP.BackColor = System.Drawing.SystemColors.ControlLight
      Me.lblCurrentIP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
      Me.lblCurrentIP.Enabled = False
      Me.lblCurrentIP.Location = New System.Drawing.Point(398, 596)
      Me.lblCurrentIP.Name = "lblCurrentIP"
      Me.lblCurrentIP.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
      Me.lblCurrentIP.Size = New System.Drawing.Size(238, 23)
      Me.lblCurrentIP.TabIndex = 25
      '
      'lblCurrentIPTitle
      '
      Me.lblCurrentIPTitle.AutoSize = True
      Me.lblCurrentIPTitle.Location = New System.Drawing.Point(395, 575)
      Me.lblCurrentIPTitle.Name = "lblCurrentIPTitle"
      Me.lblCurrentIPTitle.Size = New System.Drawing.Size(98, 13)
      Me.lblCurrentIPTitle.TabIndex = 24
      Me.lblCurrentIPTitle.Text = "Current IP Address:"
      '
      'pbStartAcquire
      '
      Me.pbStartAcquire.Appearance = System.Windows.Forms.Appearance.Button
      Me.pbStartAcquire.Enabled = False
      Me.pbStartAcquire.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.pbStartAcquire.Location = New System.Drawing.Point(657, 494)
      Me.pbStartAcquire.Name = "pbStartAcquire"
      Me.pbStartAcquire.Size = New System.Drawing.Size(169, 125)
      Me.pbStartAcquire.TabIndex = 26
      Me.pbStartAcquire.Text = "Start Acquire"
      Me.pbStartAcquire.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
      Me.pbStartAcquire.UseVisualStyleBackColor = True
      '
      'GigEVisionPropertyForm
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(848, 643)
      Me.Controls.Add(Me.pbStartAcquire)
      Me.Controls.Add(Me.lblCurrentIP)
      Me.Controls.Add(Me.lblCurrentIPTitle)
      Me.Controls.Add(Me.lblFirmwareRunTime)
      Me.Controls.Add(Me.lblSerialNumberRunTime)
      Me.Controls.Add(Me.lblFirmwareTitle)
      Me.Controls.Add(Me.lblSerialNumberTitle)
      Me.Controls.Add(Me.gbSetGigEProperties)
      Me.Controls.Add(Me.CogDisplayStatusBar1)
      Me.Controls.Add(Me.CogDisplayToolbar1)
      Me.Controls.Add(Me.CogDisplay1)
      Me.Controls.Add(Me.txtGreeting)
      Me.Controls.Add(Me.gbUseVProAcqApi)
      Me.Controls.Add(Me.gbMakeFifo)
      Me.Name = "GigEVisionPropertyForm"
      Me.Text = "GigE Vision Properties -- Basler scout Cameras"
      Me.gbMakeFifo.ResumeLayout(False)
      Me.gbMakeFifo.PerformLayout()
      Me.gbUseVProAcqApi.ResumeLayout(False)
      Me.gbUseVProAcqApi.PerformLayout()
      CType(Me.nbBrightness, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.nbExposure, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.nbContrast, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.gbSetGigEProperties.ResumeLayout(False)
      Me.gbSetGigEProperties.PerformLayout()
      CType(Me.nbBandwidth, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.nbHeartbeat, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)
      Me.PerformLayout()

    End Sub

#End Region

  End Class
End Namespace