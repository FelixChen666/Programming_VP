'/*******************************************************************************
'Copyright (C) 2005-2010 Cognex Corporation

'Subject to Cognex Corporations terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.
'*******************************************************************************
'This sample program is designed to illustrate certain VisionPro features or 
'techniques in the simplest way possible. It is not intended as the framework 
'for a complete application. In particular, the sample program may not provide 
'proper error handling, event handling, cleanup, repeatability, and other 
'mechanisms that a commercial quality application requires.
'
'This sample demonstrates acquisition from a color 1394DCAM camera, in particular:
'  * Methods for converting raw bayer data to a more usable format
'  * Manually adjusting the white balance setting of the camera (if supported by the camera)
'  * Using the "One-Push" white balance feature of the camera (if supported by the camera)
'
'Note that this sample performs acquisition in the GUI thread in response to a
'timer event.  This avoids issues associated with multiple threads, which simplifies
'the sample but frequently causes poor GUI response.  In a real vision application
'you would use a worker thread to perform acquisition.

Imports Cognex.VisionPro
Imports Cognex.VisionPro.FG1394DCAM

Public Class Form1
  Inherits System.Windows.Forms.Form

  Private WithEvents _acqFifo As Cognex.VisionPro.ICogAcqFifo = Nothing
  Private _changed As Boolean
  Private _UGain As Integer
  Private _VGain As Integer
  Private _exposure As Integer = -1
  Friend WithEvents gbFifo As System.Windows.Forms.GroupBox
  Friend WithEvents _fgList As System.Windows.Forms.ComboBox
  Friend WithEvents _vfList As System.Windows.Forms.ComboBox
  Friend WithEvents gbProperties As System.Windows.Forms.GroupBox
  Friend WithEvents _exposureTB As System.Windows.Forms.TrackBar
  Friend WithEvents btnAuto As System.Windows.Forms.Button
  Friend WithEvents _blueTB As System.Windows.Forms.TrackBar
  Friend WithEvents _redTB As System.Windows.Forms.TrackBar
  Friend WithEvents Timer1 As System.Windows.Forms.Timer
  Friend WithEvents gbPixelFormat As System.Windows.Forms.GroupBox
  Friend WithEvents rbPFColor As System.Windows.Forms.RadioButton
  Friend WithEvents rbPFRaw As System.Windows.Forms.RadioButton
  Friend WithEvents rbPFGrey As System.Windows.Forms.RadioButton
  Friend WithEvents gbConversion As System.Windows.Forms.GroupBox
  Friend WithEvents rbConvertNone As System.Windows.Forms.RadioButton
  Friend WithEvents rbConvertHSI As System.Windows.Forms.RadioButton
  Friend WithEvents rbConvertGrey As System.Windows.Forms.RadioButton
  Friend WithEvents rbConvertColor As System.Windows.Forms.RadioButton
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call

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
  Friend WithEvents _display As Cognex.VisionPro.Display.CogDisplay
  Friend WithEvents _displayStatusBar As Cognex.VisionPro.CogDisplayStatusBarV2
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
    Me._display = New Cognex.VisionPro.Display.CogDisplay
    Me._displayStatusBar = New Cognex.VisionPro.CogDisplayStatusBarV2
    Me.gbFifo = New System.Windows.Forms.GroupBox
    Me._fgList = New System.Windows.Forms.ComboBox
    Me._vfList = New System.Windows.Forms.ComboBox
    Me.gbProperties = New System.Windows.Forms.GroupBox
    Me.Label3 = New System.Windows.Forms.Label
    Me.Label2 = New System.Windows.Forms.Label
    Me.Label1 = New System.Windows.Forms.Label
    Me.btnAuto = New System.Windows.Forms.Button
    Me._blueTB = New System.Windows.Forms.TrackBar
    Me._redTB = New System.Windows.Forms.TrackBar
    Me._exposureTB = New System.Windows.Forms.TrackBar
    Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
    Me.gbPixelFormat = New System.Windows.Forms.GroupBox
    Me.rbPFRaw = New System.Windows.Forms.RadioButton
    Me.rbPFGrey = New System.Windows.Forms.RadioButton
    Me.rbPFColor = New System.Windows.Forms.RadioButton
    Me.gbConversion = New System.Windows.Forms.GroupBox
    Me.rbConvertNone = New System.Windows.Forms.RadioButton
    Me.rbConvertHSI = New System.Windows.Forms.RadioButton
    Me.rbConvertGrey = New System.Windows.Forms.RadioButton
    Me.rbConvertColor = New System.Windows.Forms.RadioButton
    Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
    CType(Me._display, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.gbFifo.SuspendLayout()
    Me.gbProperties.SuspendLayout()
    CType(Me._blueTB, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me._redTB, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me._exposureTB, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.gbPixelFormat.SuspendLayout()
    Me.gbConversion.SuspendLayout()
    Me.SuspendLayout()
    '
    '_display
    '
    Me._display.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me._display.Location = New System.Drawing.Point(288, 8)
    Me._display.Name = "_display"
    Me._display.OcxState = CType(resources.GetObject("_display.OcxState"), System.Windows.Forms.AxHost.State)
    Me._display.Size = New System.Drawing.Size(593, 467)
    Me._display.TabIndex = 8
    '
    '_displayStatusBar
    '
    Me._displayStatusBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me._displayStatusBar.Enabled = True
    Me._displayStatusBar.Location = New System.Drawing.Point(288, 475)
    Me._displayStatusBar.Name = "_displayStatusBar"
        Me._displayStatusBar.Size = New System.Drawing.Size(593, 24)
    Me._displayStatusBar.TabIndex = 9
    '
    'gbFifo
    '
    Me.gbFifo.Controls.Add(Me._fgList)
    Me.gbFifo.Controls.Add(Me._vfList)
    Me.gbFifo.Location = New System.Drawing.Point(8, 12)
    Me.gbFifo.Name = "gbFifo"
    Me.gbFifo.Size = New System.Drawing.Size(274, 73)
    Me.gbFifo.TabIndex = 12
    Me.gbFifo.TabStop = False
    Me.gbFifo.Text = "Select Camera and Video Format"
    '
    '_fgList
    '
    Me._fgList.DropDownWidth = 500
    Me._fgList.Location = New System.Drawing.Point(6, 19)
    Me._fgList.Name = "_fgList"
    Me._fgList.Size = New System.Drawing.Size(262, 21)
    Me._fgList.TabIndex = 3
    Me.ToolTip1.SetToolTip(Me._fgList, "Select the camera to acquire from")
    '
    '_vfList
    '
    Me._vfList.DropDownWidth = 500
    Me._vfList.Location = New System.Drawing.Point(6, 46)
    Me._vfList.Name = "_vfList"
    Me._vfList.Size = New System.Drawing.Size(262, 21)
    Me._vfList.TabIndex = 2
    Me.ToolTip1.SetToolTip(Me._vfList, "Select the video format to use.  Pseudo-live display will start automatically.")
    '
    'gbProperties
    '
    Me.gbProperties.Controls.Add(Me.Label3)
    Me.gbProperties.Controls.Add(Me.Label2)
    Me.gbProperties.Controls.Add(Me.Label1)
    Me.gbProperties.Controls.Add(Me.btnAuto)
    Me.gbProperties.Controls.Add(Me._blueTB)
    Me.gbProperties.Controls.Add(Me._redTB)
    Me.gbProperties.Controls.Add(Me._exposureTB)
    Me.gbProperties.Location = New System.Drawing.Point(8, 92)
    Me.gbProperties.Name = "gbProperties"
    Me.gbProperties.Size = New System.Drawing.Size(274, 146)
    Me.gbProperties.TabIndex = 13
    Me.gbProperties.TabStop = False
    Me.gbProperties.Text = "Set Image Parameters"
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Location = New System.Drawing.Point(6, 87)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(56, 13)
    Me.Label3.TabIndex = 3
    Me.Label3.Text = "Blue Gain:"
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Location = New System.Drawing.Point(6, 56)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(55, 13)
    Me.Label2.TabIndex = 3
    Me.Label2.Text = "Red Gain:"
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(6, 25)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(54, 13)
    Me.Label1.TabIndex = 2
    Me.Label1.Text = "Exposure:"
    '
    'btnAuto
    '
    Me.btnAuto.Location = New System.Drawing.Point(9, 112)
    Me.btnAuto.Name = "btnAuto"
    Me.btnAuto.Size = New System.Drawing.Size(122, 23)
    Me.btnAuto.TabIndex = 1
    Me.btnAuto.Text = "Auto White Balance"
    Me.ToolTip1.SetToolTip(Me.btnAuto, "Uses the camera's ""one push"" feature to set red and blue gain.  Use with a white " & _
            "scene for best results.")
    Me.btnAuto.UseVisualStyleBackColor = True
    '
    '_blueTB
    '
    Me._blueTB.AutoSize = False
    Me._blueTB.Location = New System.Drawing.Point(70, 81)
    Me._blueTB.Maximum = 100
    Me._blueTB.Name = "_blueTB"
    Me._blueTB.Size = New System.Drawing.Size(198, 25)
    Me._blueTB.TabIndex = 0
    Me._blueTB.TickFrequency = 0
    Me._blueTB.TickStyle = System.Windows.Forms.TickStyle.None
    Me.ToolTip1.SetToolTip(Me._blueTB, "Adjust the amount of blue data in the image")
    '
    '_redTB
    '
    Me._redTB.AutoSize = False
    Me._redTB.Location = New System.Drawing.Point(70, 50)
    Me._redTB.Maximum = 100
    Me._redTB.Name = "_redTB"
    Me._redTB.Size = New System.Drawing.Size(198, 25)
    Me._redTB.TabIndex = 0
    Me._redTB.TickFrequency = 0
    Me._redTB.TickStyle = System.Windows.Forms.TickStyle.None
    Me.ToolTip1.SetToolTip(Me._redTB, "Adjust the amount of red data in the image")
    '
    '_exposureTB
    '
    Me._exposureTB.AutoSize = False
    Me._exposureTB.Location = New System.Drawing.Point(70, 19)
    Me._exposureTB.Maximum = 75
    Me._exposureTB.Name = "_exposureTB"
    Me._exposureTB.Size = New System.Drawing.Size(198, 25)
    Me._exposureTB.TabIndex = 0
    Me._exposureTB.TickFrequency = 0
    Me._exposureTB.TickStyle = System.Windows.Forms.TickStyle.None
    Me.ToolTip1.SetToolTip(Me._exposureTB, "Adjust the exposure to get a good looking image.")
    '
    'Timer1
    '
    Me.Timer1.Enabled = True
    Me.Timer1.Interval = 50
    '
    'gbPixelFormat
    '
    Me.gbPixelFormat.Controls.Add(Me.rbPFRaw)
    Me.gbPixelFormat.Controls.Add(Me.rbPFGrey)
    Me.gbPixelFormat.Controls.Add(Me.rbPFColor)
    Me.gbPixelFormat.Location = New System.Drawing.Point(8, 244)
    Me.gbPixelFormat.Name = "gbPixelFormat"
    Me.gbPixelFormat.Size = New System.Drawing.Size(274, 88)
    Me.gbPixelFormat.TabIndex = 14
    Me.gbPixelFormat.TabStop = False
    Me.gbPixelFormat.Text = "Pixel Format"
    '
    'rbPFRaw
    '
    Me.rbPFRaw.AutoSize = True
    Me.rbPFRaw.Location = New System.Drawing.Point(6, 65)
    Me.rbPFRaw.Name = "rbPFRaw"
    Me.rbPFRaw.Size = New System.Drawing.Size(112, 17)
    Me.rbPFRaw.TabIndex = 0
    Me.rbPFRaw.TabStop = True
    Me.rbPFRaw.Text = "Acquire Raw Data"
    Me.ToolTip1.SetToolTip(Me.rbPFRaw, "Acquire raw bayer data, then convert using CogImageConvert")
    Me.rbPFRaw.UseVisualStyleBackColor = True
    '
    'rbPFGrey
    '
    Me.rbPFGrey.AutoSize = True
    Me.rbPFGrey.Location = New System.Drawing.Point(6, 42)
    Me.rbPFGrey.Name = "rbPFGrey"
    Me.rbPFGrey.Size = New System.Drawing.Size(122, 17)
    Me.rbPFGrey.TabIndex = 0
    Me.rbPFGrey.TabStop = True
    Me.rbPFGrey.Text = "Acquire in Greyscale"
    Me.ToolTip1.SetToolTip(Me.rbPFGrey, "Use OutputPixelFormat to create a greyscale image.")
    Me.rbPFGrey.UseVisualStyleBackColor = True
    '
    'rbPFColor
    '
    Me.rbPFColor.AutoSize = True
    Me.rbPFColor.Location = New System.Drawing.Point(6, 19)
    Me.rbPFColor.Name = "rbPFColor"
    Me.rbPFColor.Size = New System.Drawing.Size(100, 17)
    Me.rbPFColor.TabIndex = 0
    Me.rbPFColor.TabStop = True
    Me.rbPFColor.Text = "Acquire In Color"
    Me.ToolTip1.SetToolTip(Me.rbPFColor, "Use OutputPixelFormat to create a planar image.")
    Me.rbPFColor.UseVisualStyleBackColor = True
    '
    'gbConversion
    '
    Me.gbConversion.Controls.Add(Me.rbConvertNone)
    Me.gbConversion.Controls.Add(Me.rbConvertHSI)
    Me.gbConversion.Controls.Add(Me.rbConvertGrey)
    Me.gbConversion.Controls.Add(Me.rbConvertColor)
    Me.gbConversion.Location = New System.Drawing.Point(8, 338)
    Me.gbConversion.Name = "gbConversion"
    Me.gbConversion.Size = New System.Drawing.Size(274, 119)
    Me.gbConversion.TabIndex = 15
    Me.gbConversion.TabStop = False
    Me.gbConversion.Text = "Raw Data Conversion"
    '
    'rbConvertNone
    '
    Me.rbConvertNone.AutoSize = True
    Me.rbConvertNone.Checked = True
    Me.rbConvertNone.Location = New System.Drawing.Point(6, 88)
    Me.rbConvertNone.Name = "rbConvertNone"
    Me.rbConvertNone.Size = New System.Drawing.Size(99, 17)
    Me.rbConvertNone.TabIndex = 2
    Me.rbConvertNone.TabStop = True
    Me.rbConvertNone.Text = "Do Not Convert"
    Me.ToolTip1.SetToolTip(Me.rbConvertNone, "Display raw data without conversion.")
    Me.rbConvertNone.UseVisualStyleBackColor = True
    '
    'rbConvertHSI
    '
    Me.rbConvertHSI.AutoSize = True
    Me.rbConvertHSI.Location = New System.Drawing.Point(6, 65)
    Me.rbConvertHSI.Name = "rbConvertHSI"
    Me.rbConvertHSI.Size = New System.Drawing.Size(95, 17)
    Me.rbConvertHSI.TabIndex = 1
    Me.rbConvertHSI.Text = "Convert to HSI"
    Me.ToolTip1.SetToolTip(Me.rbConvertHSI, "Use CogImageConvert to creata an HSI image.")
    Me.rbConvertHSI.UseVisualStyleBackColor = True
    '
    'rbConvertGrey
    '
    Me.rbConvertGrey.AutoSize = True
    Me.rbConvertGrey.Location = New System.Drawing.Point(6, 42)
    Me.rbConvertGrey.Name = "rbConvertGrey"
    Me.rbConvertGrey.Size = New System.Drawing.Size(124, 17)
    Me.rbConvertGrey.TabIndex = 1
    Me.rbConvertGrey.Text = "Convert to Greyscale"
    Me.ToolTip1.SetToolTip(Me.rbConvertGrey, "Use CogImageConvert to creata a greyscale image.")
    Me.rbConvertGrey.UseVisualStyleBackColor = True
    '
    'rbConvertColor
    '
    Me.rbConvertColor.AutoSize = True
    Me.rbConvertColor.Location = New System.Drawing.Point(6, 19)
    Me.rbConvertColor.Name = "rbConvertColor"
    Me.rbConvertColor.Size = New System.Drawing.Size(101, 17)
    Me.rbConvertColor.TabIndex = 0
    Me.rbConvertColor.Text = "Convert to Color"
    Me.ToolTip1.SetToolTip(Me.rbConvertColor, "Use CogImageConvert to creata a planar color image.")
    Me.rbConvertColor.UseVisualStyleBackColor = True
    '
    'Form1
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(889, 505)
    Me.Controls.Add(Me.gbConversion)
    Me.Controls.Add(Me.gbPixelFormat)
    Me.Controls.Add(Me.gbProperties)
    Me.Controls.Add(Me.gbFifo)
    Me.Controls.Add(Me._displayStatusBar)
    Me.Controls.Add(Me._display)
    Me.MinimumSize = New System.Drawing.Size(544, 376)
    Me.Name = "Form1"
    Me.Text = "1394 DCAM Capture and Display"
    CType(Me._display, System.ComponentModel.ISupportInitialize).EndInit()
    Me.gbFifo.ResumeLayout(False)
    Me.gbProperties.ResumeLayout(False)
    Me.gbProperties.PerformLayout()
    CType(Me._blueTB, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me._redTB, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me._exposureTB, System.ComponentModel.ISupportInitialize).EndInit()
    Me.gbPixelFormat.ResumeLayout(False)
    Me.gbPixelFormat.PerformLayout()
    Me.gbConversion.ResumeLayout(False)
    Me.gbConversion.PerformLayout()
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    _displayStatusBar.Display = _display

    Dim fgs As CogFrameGrabber1394DCAMs = New CogFrameGrabber1394DCAMs
    For Each fg As Cognex.VisionPro.ICogFrameGrabber In fgs
      _fgList.Items.Add(fg.Name)
    Next
  End Sub

  ' Acquisition is performed in response to a timer event.  This is not the way to write a real application,
  ' but is a simple way to avoid threading issues in this sample.
  Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
    If _acqFifo Is Nothing Then
      Return
    End If

    'After an automatic white balance occurs, read the new settings and update the
    'sliders to match.
    If _changed Then
      UpdateSliders()
      _changed = False
    End If

    'Set the white balance property (if supported) based on the sliders.
    'Important: The fifo's values are set only if the desired setting has changed.  This avoids
    'overwriting the settings determined via automatic white balance before they can be read by
    'the code above.
    If Not _acqFifo.OwnedWhiteBalanceParams Is Nothing Then
      If Not _UGain = _redTB.Value Then
        _UGain = _redTB.Value
        _acqFifo.OwnedWhiteBalanceParams.UGain = _UGain / 100
      End If
      If Not _VGain = _blueTB.Value Then
        _VGain = _blueTB.Value
        _acqFifo.OwnedWhiteBalanceParams.VGain = _VGain / 100
      End If
    End If

    If Not _exposure = _exposureTB.Value Then
      _exposure = _exposureTB.Value
      _acqFifo.OwnedExposureParams.Exposure = System.Math.Pow(1.4, _exposure)
    End If

    'A few words about pixel formats:
    '
    'The "pixel format" defines how each pixel's data is stored in memory.  There is
    'a wide variety of pixel formats that can be acquired from a camera, but VisionPro
    'can only process a few of them.  The fifo's OutputPixelFormat property allows you
    'to specify the desired pixel format for processing, and the fifo will perform
    'a conversion if needed to produce an image in the desired pixel format.
    '
    'PlanarRGB8 is the default setting for color cameras, and is compatible with color
    'vision tools.
    '
    'Grey8 is the default setting for greyscale cameras, and is compatible with greyscale
    'vision tools.
    '
    'All fifos support PlanarRGB8 and Grey8 settings for OutputPixelFormat.
    '
    'Bayer8 is only available for bayer color cameras, and is intended for backwards
    'compatibility or if you need more control over the image conversion process.
    If rbPFColor.Checked Then
      _acqFifo.OutputPixelFormat = CogImagePixelFormatConstants.PlanarRGB8
    ElseIf rbPFGrey.Checked Then
      _acqFifo.OutputPixelFormat = CogImagePixelFormatConstants.Grey8
    ElseIf rbPFRaw.Checked Then
      _acqFifo.OutputPixelFormat = CogImagePixelFormatConstants.Bayer8
    End If

    'Acquire an image
    Dim trigNum As Integer
    Dim image As Cognex.VisionPro.ICogImage
    image = _acqFifo.Acquire(trigNum)

    'If the image was acquired in raw bayer format, use CogImageConvert to convert it to
    'a different pixel format.
    If rbPFRaw.Checked Then
      If rbConvertGrey.Checked Then
        image = Cognex.VisionPro.CogImageConvert. _
          GetIntensityImageFromBayer(image, 0, 0, 0, 0, _
          0.299, 0.587, 0.114)
      End If

      If rbConvertColor.Checked Then
        image = Cognex.VisionPro.CogImageConvert. _
          GetRGBImageFromBayer(image, 0, 0, 0, 0, 1.0, 1.0, 1.0)
      End If

      If rbConvertHSI.Checked Then
        image = Cognex.VisionPro.CogImageConvert. _
          GetHSIImageFromBayer(image, 0, 0, 0, 0, 1.0, 1.0, 1.0)
      End If

      'There is no code associated with the "no conversion" option.
    End If

    _display.Image = image
    GC.Collect()
  End Sub

  'When a camera is selected
  Private Sub _fgList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _fgList.SelectedIndexChanged
    _vfList.Items.Clear()
    Dim fgs As CogFrameGrabber1394DCAMs = New CogFrameGrabber1394DCAMs
    Dim cb As ComboBox = sender

    'Populate the list of available video formats, then set selectedIndex to 0, which as a side effect will
    'cause the first format in the list to automatically take effect.
    For Each vf As String In fgs(cb.SelectedIndex).AvailableVideoFormats
      _vfList.Items.Add(vf)
    Next
    _vfList.SelectedIndex = 0
  End Sub

  'When a video format is selected
  Private Sub _vfList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _vfList.SelectedIndexChanged

    'Remove any handler associated with a previous fifo.
    If Not _acqFifo Is Nothing Then
      RemoveHandler _acqFifo.Changed, New CogChangedEventHandler(AddressOf Operator_Changed)
    End If

    'Construct a new fifo using the selected format and camera.
    Dim fgs As CogFrameGrabber1394DCAMs = New CogFrameGrabber1394DCAMs
    _acqFifo = fgs(_fgList.SelectedIndex).CreateAcqFifo( _
      fgs(_fgList.SelectedIndex).AvailableVideoFormats(_vfList.SelectedIndex()), _
      CogAcqFifoPixelFormatConstants.Format8Grey, 0, False)

    'Add a handler to detect changes to the white balance settings.
    AddHandler _acqFifo.Changed, New CogChangedEventHandler(AddressOf Operator_Changed)

    'Bayer8 output is only available with bayer cameras.  This is enforced in the sample code by
    'disabling the "Raw" if the acquired format is not Bayer8.
    If _acqFifo.AcquiredPixelFormat = CogImagePixelFormatConstants.Bayer8 Then
      rbPFRaw.Enabled = True
    Else
      rbPFRaw.Enabled = False
    End If

    'Update radio button state to match default OutputPixelFormat for the fifo.
    If _acqFifo.OutputPixelFormat = CogImagePixelFormatConstants.PlanarRGB8 Then
      rbPFColor.Checked = True
    Else
      rbPFGrey.Checked = True
    End If

    gbConversion.Enabled = False

    'Reset slider controls
    _exposureTB.Value = 10
    _changed = True
  End Sub

  'Fifo changed event handler.  Set _changed to true when white balance has changed, so that the updated
  'values can be read during the next acquisition.
  Private Sub Operator_Changed(ByVal sender As Object, ByVal e As CogChangedEventArgs)
    If e.StateFlags = CogAcqFifoStateFlags.SfUGain Or _
       e.StateFlags = CogAcqFifoStateFlags.SfVGain Then
      _changed = True
    End If
  End Sub

  'Read the white balance values and set the sliders accordingly.
  Private Sub UpdateSliders()
    Dim wb As Cognex.VisionPro.ICogAcqWhiteBalance
    wb = _acqFifo.OwnedWhiteBalanceParams
    If Not wb Is Nothing Then
      _UGain = wb.UGain * 100.0
      _VGain = wb.VGain * 100.0
      _redTB.Value = _UGain
      _blueTB.Value = _VGain

    End If
  End Sub

  'Perform an automatic white balance using the camera's "one push" function.
  Private Sub btnAuto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuto.Click
    If Not _acqFifo Is Nothing Then
      Dim wb As Cognex.VisionPro.ICogAcqWhiteBalance
      wb = _acqFifo.OwnedWhiteBalanceParams

      If Not wb Is Nothing Then
        'Calling AutoWhiteBalance does not immediately perform a white balance.  Instead, it requests
        'that an automatic white balance be performed prior to the next acquisition.  After the automatic
        'white balance occurs, the changed event will be fired and the new white balance values can be
        'read from the fifo's properties.
        wb.AutoWhiteBalance()

        'If the fifo is idle, prepare can be used to cause the requested automatic white balance
        'to occur.
        _acqFifo.Prepare()
      End If
    End If
  End Sub

  'Conditionally enable the ImageConvert options based on whether raw bayer data was acquired or not.
  Private Sub rbPFRaw_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbPFRaw.CheckedChanged
    gbConversion.Enabled = rbPFRaw.Checked
  End Sub
End Class
