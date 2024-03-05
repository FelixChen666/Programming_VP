' ***************************************************************************
' Copyright (C) 2006 Cognex Corporation
'
' Subject to Cognex Corporation's terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely
' in conjunction with a Cognex Machine Vision System.  Furthermore you
' acknowledge and agree that Cognex has no warranty, obligations or liability
' for your use of the Software.
' ***************************************************************************
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This sample demonstrates how to load a persisted QuickBuild application
' and access the results provided in the user
' result queue. The sample uses "mySavedQB.vpp", which consists of a single
' Job that executes a Blob tool with default parameters using images from
' a file.
'
' The provided .vpp file is configured to use "VPRO_ROOT\Images\pmSample.idb" as the
' source of images.  
'
' This application uses a timer to determine when results should be updated
' on the user interface.
'
' To use: If necessary, reconfigure acquisition using QuickBuild.  Then run
' this sample code.  The number of blobs will be displayed in the count text
' box and the Blob tool input image will be displayed in the image display
' control.

Imports Cognex.VisionPro
Imports Cognex.VisionPro.QuickBuild

Public Class Form1
  Inherits System.Windows.Forms.Form


#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()
    InitializeJobManager()

    'Add any initialization after the InitializeComponent() call

  End Sub

  'Form overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then
      If Not (components Is Nothing) Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents myCountText As System.Windows.Forms.TextBox
  Friend WithEvents SampleTextBox As System.Windows.Forms.TextBox
  Friend WithEvents Timer1 As System.Windows.Forms.Timer
  Friend WithEvents RunContCheckBox As System.Windows.Forms.CheckBox
  Friend WithEvents RunOnceButton As System.Windows.Forms.Button
  Friend WithEvents CogRecordDisplay1 As Cognex.VisionPro.CogRecordDisplay
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form1))
    Me.myCountText = New System.Windows.Forms.TextBox
    Me.Label1 = New System.Windows.Forms.Label
    Me.SampleTextBox = New System.Windows.Forms.TextBox
    Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
    Me.RunContCheckBox = New System.Windows.Forms.CheckBox
    Me.RunOnceButton = New System.Windows.Forms.Button
    Me.CogRecordDisplay1 = New Cognex.VisionPro.CogRecordDisplay
    CType(Me.CogRecordDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'myCountText
    '
    Me.myCountText.Location = New System.Drawing.Point(40, 136)
    Me.myCountText.Name = "myCountText"
    Me.myCountText.ReadOnly = True
    Me.myCountText.Size = New System.Drawing.Size(64, 20)
    Me.myCountText.TabIndex = 1
    Me.myCountText.Text = ""
    '
    'Label1
    '
    Me.Label1.Location = New System.Drawing.Point(24, 112)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(40, 16)
    Me.Label1.TabIndex = 2
    Me.Label1.Text = "Count:"
    '
    'SampleTextBox
    '
    Me.SampleTextBox.Location = New System.Drawing.Point(440, 8)
    Me.SampleTextBox.Multiline = True
    Me.SampleTextBox.Name = "SampleTextBox"
    Me.SampleTextBox.ReadOnly = True
    Me.SampleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.SampleTextBox.Size = New System.Drawing.Size(272, 192)
    Me.SampleTextBox.TabIndex = 4
    Me.SampleTextBox.Text = ""
    '
    'Timer1
    '
    Me.Timer1.Interval = 50
    '
    'RunContCheckBox
    '
    Me.RunContCheckBox.Appearance = System.Windows.Forms.Appearance.Button
    Me.RunContCheckBox.Location = New System.Drawing.Point(24, 56)
    Me.RunContCheckBox.Name = "RunContCheckBox"
    Me.RunContCheckBox.Size = New System.Drawing.Size(96, 32)
    Me.RunContCheckBox.TabIndex = 12
    Me.RunContCheckBox.Text = "Run Continuous"
    Me.RunContCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'RunOnceButton
    '
    Me.RunOnceButton.Location = New System.Drawing.Point(24, 16)
    Me.RunOnceButton.Name = "RunOnceButton"
    Me.RunOnceButton.Size = New System.Drawing.Size(96, 32)
    Me.RunOnceButton.TabIndex = 11
    Me.RunOnceButton.Text = "Run Once"
    '
    'CogRecordDisplay1
    '
    Me.CogRecordDisplay1.Location = New System.Drawing.Point(136, 8)
    Me.CogRecordDisplay1.Name = "CogRecordDisplay1"
    Me.CogRecordDisplay1.OcxState = CType(resources.GetObject("CogRecordDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
    Me.CogRecordDisplay1.Size = New System.Drawing.Size(288, 208)
    Me.CogRecordDisplay1.TabIndex = 13
    '
    'Form1
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(728, 238)
    Me.Controls.Add(Me.CogRecordDisplay1)
    Me.Controls.Add(Me.RunContCheckBox)
    Me.Controls.Add(Me.RunOnceButton)
    Me.Controls.Add(Me.SampleTextBox)
    Me.Controls.Add(Me.myCountText)
    Me.Controls.Add(Me.Label1)
    Me.Name = "Form1"
    Me.Text = "QuickBuild Sample Application"
    CType(Me.CogRecordDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

#End Region

  Dim myJobManager As CogJobManager
  Dim myJob As CogJob
  Dim myIndependentJob As CogJobIndependent


  'This method is called from within Form.New() after InitializeComponent. Alternatively, this
  'code could appear in Form.Load()
  Private Sub InitializeJobManager()
    SampleTextBox.Text = _
        "This sample demonstrates how to load a persisted QuickBuild application and access " + _
        "the results provided in the posted items queue (a.k.a. the user result queue)." + _
        vbCrLf + vbCrLf + _
        "The sample uses ""mySavedQB.vpp"", which consists of a single Job that " + _
        "executes a Blob tool with default parameters using images from a file.  " + _
        "The provided .vpp file is configured to use ""VPRO_ROOT\images\pmSample.idb"" as the " + _
        "source of images.  " + _
        vbCrLf + vbCrLf + _
        "To use: Click the Run button or the Run Continuous button.  " + _
        "The number of blobs will be displayed in the count text box " + _
        "and the Blob tool input image will be displayed in the image display control."

    'Depersist the QuickBuild session
    myJobManager = CType(CogSerializer.LoadObjectFromFile(Environment.GetEnvironmentVariable("VPRO_ROOT") + "\\Samples\\Programming\\QuickBuild\\mySavedQB.vpp"), CogJobManager)
    myJob = myJobManager.Job(0)
    myIndependentJob = myJob.OwnedIndependent

    ' flush queues
    myJobManager.UserQueueFlush()
    myJobManager.FailureQueueFlush()
    myJob.ImageQueueFlush()
    myIndependentJob.RealTimeQueueFlush()

    AddHandler Timer1.Tick, AddressOf timer1_Tick
    ' Start the timer.
    Timer1.Start()
  End Sub

  ' This method handles the tick event from the timer.  When the timer "ticks", 
  ' an image is taken from the Job Manager User Queue and is displayed on the GUI.  
  ' In this sample, the blob count, which is placed on the Job Real-Time Queue,
  ' is also displayed on the GUI.
  Private Sub timer1_Tick(ByVal sender As Object, ByVal e As EventArgs)
    UpdateGUI()
  End Sub

  ' This method grabs the blob count from the 
  ' Job Manager User Queue and displays it on the GUI.
  Private Sub UpdateGUI()
    Dim tmpRecord As Cognex.VisionPro.ICogRecord
    Dim topRecord As Cognex.VisionPro.ICogRecord = myJobManager.UserResult

    ' check to be sure results are available
    If topRecord Is Nothing Then Return

    ' Assume that the required "count" record is present, and go get it.
    tmpRecord = topRecord.SubRecords.Item("Tools.Item[""CogBlobTool1""].CogBlobTool.Results.GetBlobs().Count")
    Dim count As Integer = CType(tmpRecord.Content, Integer)
    myCountText.Text = count.ToString()

    ' Assume that the required "image" record is present, and go get it.
    tmpRecord = topRecord.SubRecords.Item("ShowLastRunRecordForUserQueue")
    tmpRecord = tmpRecord.SubRecords.Item("LastRun")
    tmpRecord = tmpRecord.SubRecords.Item("Image Source.OutputImage")
    CogRecordDisplay1.Record = tmpRecord
    CogRecordDisplay1.Fit(True)
  End Sub

  Private Sub Form1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
    Timer1.Stop()
    CogRecordDisplay1.Dispose()
    'Be sure to shudown the CogJobManager!!
    myJobManager.Shutdown()
  End Sub

  Private Sub RunOnceButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunOnceButton.Click
    Try
      myJobManager.Run()
    Catch ex As Exception
      MessageBox.Show(ex.Message)
    End Try

  End Sub

  Private Sub RunContCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunContCheckBox.CheckedChanged
    If (RunContCheckBox.Checked) Then
      Try
        myJobManager.RunContinuous()
      Catch ex As Exception
        MessageBox.Show(ex.Message)
      End Try
      RunOnceButton.Enabled = False
    Else
      Try
        myJobManager.Stop()
      Catch ex As Exception
        MessageBox.Show(ex.Message)
      End Try
      RunOnceButton.Enabled = True
    End If

  End Sub
End Class
