' ***************************************************************************
' Copyright (C) 2005 Cognex Corporation
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
' result queue. 
'
' The provided .vpp file is configured to use "VPRO_ROOT\Images\pmSample.idb" as the
' source of images and a blob tool.
'
' To use: If necessary, reconfigure acquisition using QuickBuild.  Then run
' this sample code.  The number of blobs will be displayed in the count text
' box and the Blob tool input image will be displayed in the image display
' control.
'
' This application makes use of the .NET method "Invoke" to move data between the Job
' Thread (worker thread) and the GUI thread.  As described in the .NET documenation, 
' Invoke allows a worker thread to tell the GUI thread to run a specified method with
' specified parameters.  This is the .NET recommended manner for getting worker threads
' to update the GUI.  Because we use Invoke in this sample, is efficient, but may be a 
' bit complicated for those unfamiliar with threading.


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
  Friend WithEvents RunContCheckBox As System.Windows.Forms.CheckBox
  Friend WithEvents CogRecordDisplay1 As Cognex.VisionPro.CogRecordDisplay
  Friend WithEvents RunOnceButton As System.Windows.Forms.Button
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
    Me.myCountText = New System.Windows.Forms.TextBox
    Me.Label1 = New System.Windows.Forms.Label
    Me.SampleTextBox = New System.Windows.Forms.TextBox
    Me.RunContCheckBox = New System.Windows.Forms.CheckBox
    Me.RunOnceButton = New System.Windows.Forms.Button
    Me.CogRecordDisplay1 = New Cognex.VisionPro.CogRecordDisplay
    CType(Me.CogRecordDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'myCountText
    '
    Me.myCountText.Location = New System.Drawing.Point(48, 160)
    Me.myCountText.Name = "myCountText"
    Me.myCountText.ReadOnly = True
    Me.myCountText.Size = New System.Drawing.Size(64, 20)
    Me.myCountText.TabIndex = 3
    '
    'Label1
    '
    Me.Label1.Location = New System.Drawing.Point(24, 136)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(40, 16)
    Me.Label1.TabIndex = 2
    Me.Label1.Text = "Count:"
    '
    'SampleTextBox
    '
    Me.SampleTextBox.Location = New System.Drawing.Point(432, 16)
    Me.SampleTextBox.Multiline = True
    Me.SampleTextBox.Name = "SampleTextBox"
    Me.SampleTextBox.ReadOnly = True
    Me.SampleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.SampleTextBox.Size = New System.Drawing.Size(256, 184)
    Me.SampleTextBox.TabIndex = 5
    '
    'RunContCheckBox
    '
    Me.RunContCheckBox.Appearance = System.Windows.Forms.Appearance.Button
    Me.RunContCheckBox.Location = New System.Drawing.Point(16, 56)
    Me.RunContCheckBox.Name = "RunContCheckBox"
    Me.RunContCheckBox.Size = New System.Drawing.Size(96, 32)
    Me.RunContCheckBox.TabIndex = 2
    Me.RunContCheckBox.Text = "Run Continuous"
    Me.RunContCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'RunOnceButton
    '
    Me.RunOnceButton.Location = New System.Drawing.Point(16, 16)
    Me.RunOnceButton.Name = "RunOnceButton"
    Me.RunOnceButton.Size = New System.Drawing.Size(96, 32)
    Me.RunOnceButton.TabIndex = 1
    Me.RunOnceButton.Text = "Run Once"
    '
    'CogRecordDisplay1
    '
    Me.CogRecordDisplay1.Location = New System.Drawing.Point(137, 16)
    Me.CogRecordDisplay1.Name = "CogRecordDisplay1"
    Me.CogRecordDisplay1.OcxState = CType(resources.GetObject("CogRecordDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
    Me.CogRecordDisplay1.Size = New System.Drawing.Size(289, 198)
    Me.CogRecordDisplay1.TabIndex = 6
    '
    'Form1
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(712, 246)
    Me.Controls.Add(Me.CogRecordDisplay1)
    Me.Controls.Add(Me.RunContCheckBox)
    Me.Controls.Add(Me.RunOnceButton)
    Me.Controls.Add(Me.SampleTextBox)
    Me.Controls.Add(Me.Label1)
    Me.Controls.Add(Me.myCountText)
    Me.Name = "Form1"
    Me.Text = "QuickBuild Sample Application"
    CType(Me.CogRecordDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub

#End Region

  Dim myJobManager As CogJobManager
  Dim myJob As CogJob
  Dim myIndependentJob As CogJobIndependent

  'VB.NET is a multi-threaded language, unlike VB6. Because of this, one must be careful
  'when worker threads interact with the GUI (which is on it's own thread). One preferred
  'way to do this in .NET is to use the InvokeRequired()/BeginInvoke() mechanisms.  These 
  'mechanisms allow worker threads to tell the GUI thread that a given method needs to be
  'run on the GUI thread.  This insures thread safety on the GUI thread.  Otherwise, bad
  'things (crashes, etc.) might occur if non-GUI threads tried to update the GUI.

  'When InvokeRequired() is called from a Form's method, it determines if the calling thread
  'is different from the Form's thread. If so, it returns true which indicates that 
  'a worker thread wants to post something to the GUI.  Else, it returns false.

  'If InvokeRequired() is true, then the caller is trying to tell the GUI thread to run a  
  'particular method to post something on the GUI.  To do this, one employs BeginInvoke with
  'a delegate that contains the address of the particular method to run along with parameters needed
  'by that method.

  ' Delegates which dictate the signature of the methods that will post to the GUI.
  Delegate Sub UserResultEventHandlerDelegate(ByVal sender As Object, ByVal e As CogJobManagerActionEventArgs)


  'This method is called from within Form.New() after InitializeComponent. Alternatively, this
  'code could appear in Form.Load()
  Private Sub InitializeJobManager()
    SampleTextBox.Text = _
        "This sample demonstrates how to load a persisted QuickBuild application and access " + _
        "the results provided in the real-time result queue and the user result queue." + _
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
        myJobManager = CType(CogSerializer.LoadObjectFromFile( _
            Environment.GetEnvironmentVariable("VPRO_ROOT") _
            + "\\Samples\\Programming\\QuickBuild\\mySavedQB.vpp"), CogJobManager)
    myJob = myJobManager.Job(0)
    myIndependentJob = myJob.OwnedIndependent

    ' flush queues
    myJobManager.UserQueueFlush()
    myJobManager.FailureQueueFlush()
    myJob.ImageQueueFlush()
    myIndependentJob.RealTimeQueueFlush()

    ' setup event handlers.  These are called when a result packet is available on
    ' the User Result Queue or the Real-Time Queue, respectively.
    AddHandler myJobManager.UserResultAvailable, AddressOf UserResultAvailableHandler

  End Sub
  'If it is called by a worker thread,
  'InvokeRequired is true, as described above.  When this occurs, a delegate is constructed
  'which is really a pointer to the method that the GUI thread should call.
  'BeginInvoke is then called, with this delegate and the Count parameter.
  'Notice that this subroutine tells the GUI thread to call the same subroutine!  
  'When the GUI calls this method on its own thread, InvokeRequired() will be false and the 
  'text box is updated with the passed in value (Count).

  ' This method handles the UserResultAvailable Event. The user packet
  ' has been configured to contain the blob tool input image, which we retrieve and display.
  Private Sub UserResultAvailableHandler(ByVal sender As Object, ByVal e As CogJobManagerActionEventArgs)
    ' get results - they are stored in ICogRecords
    ' Display the image on the GUI.
    If InvokeRequired Then
      Dim eventArgs() As Object = {sender, e}
      Invoke(New UserResultEventHandlerDelegate(AddressOf UserResultAvailableHandler), eventArgs)
      Return
    End If
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
    RemoveHandler myJobManager.UserResultAvailable, AddressOf UserResultAvailableHandler
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
