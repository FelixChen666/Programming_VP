'*******************************************************************************
' Copyright (C) 2004 Cognex Corporation
'
' Subject to Cognex Corporation's terms and conditions and license agreement,
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

' This sample shows how to display a Blob Tool's LastRunRecord graphics
' on a form using a CogRecordDisplay.  It also demonstrates how to
' selectively enable or disable different graphic features using
' the LastRunRecordEnable property of the BlobTool.
' This same pattern can be applied to all VisionPro tools.

Option Explicit On 
Imports Cognex.VisionPro                 'needed for VisionPro basic functionality
Imports Cognex.VisionPro.Exceptions      'needed for VisionPro exceptions
Imports Cognex.VisionPro.Implementation  'needed for CogRecord
Imports Cognex.VisionPro.ImageFile       'needed for CogImageFile
Imports Cognex.VisionPro.Blob            'needed for CogBlobTool

Namespace GraphicsRecordSample
  Public Class GraphicsRecordForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call
      Dim sImageFile As String = Environment.GetEnvironmentVariable("VPRO_ROOT")
      Try
        ' Open the image file.  If an error occurs when opening the image file
        ' (not found, for example), report it and return failure, using try...catch...  
        If sImageFile = "" Then
          Throw New Exception("Required environment variable VPRO_ROOT is not set")
        End If
        sImageFile = sImageFile + "\images\bracket_std.idb"

        ' sink the events we are interested in
        mImageFileTool = mImageFileEdit.Subject
        mImageFileTool.[Operator].Open(sImageFile, CogImageFileModeConstants.Read)

        mBlobTool = mBlobToolEdit.Subject
        AddHandler mBlobTool.Changed, AddressOf mBlobTool_Changed
        AddHandler mBlobTool.Ran, AddressOf mBlobTool_Ran

        ' use DataBinding property to link the output of ImageFileTool to input of BlobTool
        mBlobTool.DataBindings.Add("InputImage", mImageFileTool, "OutputImage")

        ' run the image file tool and blob tool once
        mImageFileTool.Run()
        mBlobTool.Run()

        ' synchronize the controls with tool settings
        SyncGraphicsControls()

        ' setup txtDescription
        txtDescription.Text = ""
        txtDescription.AppendText("Sample description: shows how to display CogRecord graphics in a CogRecordDisplay.")
        txtDescription.AppendText(Environment.NewLine)
        txtDescription.AppendText("Sample usage: configure the graphics shown using the Vision Tool Graphics checkboxes.  ")
        txtDescription.AppendText("The graphics shown can also be configured using the Graphics tab of the tool edit control.  ")
        txtDescription.AppendText("The tool edit control can be found on the Vision Tool tab of this Form.")
      Catch ex As CogFileOpenException
        ShowErrorAndExit(ex.GetType().ToString(), ex.Message + Environment.NewLine + sImageFile)
      Catch ex As CogException
        ShowErrorAndExit(ex.GetType().ToString(), "Caught Cognex exception: " + ex.Message)
      Catch ex As Exception
        ShowErrorAndExit(ex.GetType().ToString(), ex.Message)
      End Try
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
      If Not mBlobTool Is Nothing Then
        RemoveHandler mBlobTool.Changed, AddressOf mBlobTool_Changed
        RemoveHandler mBlobTool.Ran, AddressOf mBlobTool_Ran
      End If

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
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents tabSample As System.Windows.Forms.TabPage
    Friend WithEvents grpToolGraphics As System.Windows.Forms.GroupBox
    Friend WithEvents chkShowCOM As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowBoundary As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowBoundingBox As System.Windows.Forms.CheckBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents tabGraphicsRecord As System.Windows.Forms.TabControl
    Friend WithEvents mDisplay As Cognex.VisionPro.CogRecordDisplay
    Friend WithEvents tabImageFile As System.Windows.Forms.TabPage
    Friend WithEvents tabTool As System.Windows.Forms.TabPage
        Friend WithEvents mImageFileEdit As Cognex.VisionPro.ImageFile.CogImageFileEditV2
        Friend WithEvents mBlobToolEdit As Cognex.VisionPro.Blob.CogBlobEditV2
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GraphicsRecordForm))
      Me.tabGraphicsRecord = New System.Windows.Forms.TabControl
      Me.tabSample = New System.Windows.Forms.TabPage
      Me.mDisplay = New Cognex.VisionPro.CogRecordDisplay
      Me.grpToolGraphics = New System.Windows.Forms.GroupBox
      Me.chkShowCOM = New System.Windows.Forms.CheckBox
      Me.chkShowBoundary = New System.Windows.Forms.CheckBox
      Me.chkShowBoundingBox = New System.Windows.Forms.CheckBox
      Me.btnClose = New System.Windows.Forms.Button
      Me.btnRun = New System.Windows.Forms.Button
      Me.tabImageFile = New System.Windows.Forms.TabPage
      Me.mImageFileEdit = New Cognex.VisionPro.ImageFile.CogImageFileEditV2
      Me.tabTool = New System.Windows.Forms.TabPage
      Me.mBlobToolEdit = New Cognex.VisionPro.Blob.CogBlobEditV2
      Me.txtDescription = New System.Windows.Forms.TextBox
      Me.tabGraphicsRecord.SuspendLayout()
      Me.tabSample.SuspendLayout()
      CType(Me.mDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.grpToolGraphics.SuspendLayout()
      Me.tabImageFile.SuspendLayout()
      CType(Me.mImageFileEdit, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.tabTool.SuspendLayout()
      CType(Me.mBlobToolEdit, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'tabGraphicsRecord
      '
      Me.tabGraphicsRecord.Controls.Add(Me.tabSample)
      Me.tabGraphicsRecord.Controls.Add(Me.tabImageFile)
      Me.tabGraphicsRecord.Controls.Add(Me.tabTool)
      Me.tabGraphicsRecord.Dock = System.Windows.Forms.DockStyle.Top
      Me.tabGraphicsRecord.Location = New System.Drawing.Point(0, 0)
      Me.tabGraphicsRecord.Name = "tabGraphicsRecord"
      Me.tabGraphicsRecord.SelectedIndex = 0
      Me.tabGraphicsRecord.Size = New System.Drawing.Size(800, 367)
      Me.tabGraphicsRecord.TabIndex = 0
      '
      'tabSample
      '
      Me.tabSample.Controls.Add(Me.mDisplay)
      Me.tabSample.Controls.Add(Me.grpToolGraphics)
      Me.tabSample.Controls.Add(Me.btnClose)
      Me.tabSample.Controls.Add(Me.btnRun)
      Me.tabSample.Location = New System.Drawing.Point(4, 22)
      Me.tabSample.Name = "tabSample"
      Me.tabSample.Size = New System.Drawing.Size(792, 341)
      Me.tabSample.TabIndex = 0
      Me.tabSample.Text = "Graphics Record Sample"
      '
      'mDisplay
      '
      Me.mDisplay.Location = New System.Drawing.Point(4, 4)
      Me.mDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
      Me.mDisplay.MouseWheelSensitivity = 1
      Me.mDisplay.Name = "mDisplay"
      Me.mDisplay.OcxState = CType(resources.GetObject("mDisplay.OcxState"), System.Windows.Forms.AxHost.State)
      Me.mDisplay.Size = New System.Drawing.Size(562, 334)
      Me.mDisplay.TabIndex = 5
      '
      'grpToolGraphics
      '
      Me.grpToolGraphics.Controls.Add(Me.chkShowCOM)
      Me.grpToolGraphics.Controls.Add(Me.chkShowBoundary)
      Me.grpToolGraphics.Controls.Add(Me.chkShowBoundingBox)
      Me.grpToolGraphics.Location = New System.Drawing.Point(572, 96)
      Me.grpToolGraphics.Name = "grpToolGraphics"
      Me.grpToolGraphics.Size = New System.Drawing.Size(216, 108)
      Me.grpToolGraphics.TabIndex = 4
      Me.grpToolGraphics.TabStop = False
      Me.grpToolGraphics.Text = "Vision Tool Graphics"
      '
      'chkShowCOM
      '
      Me.chkShowCOM.Location = New System.Drawing.Point(12, 76)
      Me.chkShowCOM.Name = "chkShowCOM"
      Me.chkShowCOM.Size = New System.Drawing.Size(196, 24)
      Me.chkShowCOM.TabIndex = 2
      Me.chkShowCOM.Text = "Show Center Of Mass"
      '
      'chkShowBoundary
      '
      Me.chkShowBoundary.Location = New System.Drawing.Point(12, 46)
      Me.chkShowBoundary.Name = "chkShowBoundary"
      Me.chkShowBoundary.Size = New System.Drawing.Size(196, 24)
      Me.chkShowBoundary.TabIndex = 1
      Me.chkShowBoundary.Text = "Show Boundary"
      '
      'chkShowBoundingBox
      '
      Me.chkShowBoundingBox.Location = New System.Drawing.Point(12, 20)
      Me.chkShowBoundingBox.Name = "chkShowBoundingBox"
      Me.chkShowBoundingBox.Size = New System.Drawing.Size(196, 24)
      Me.chkShowBoundingBox.TabIndex = 0
      Me.chkShowBoundingBox.Text = "Show Bounding Box"
      '
      'btnClose
      '
      Me.btnClose.Location = New System.Drawing.Point(696, 36)
      Me.btnClose.Name = "btnClose"
      Me.btnClose.Size = New System.Drawing.Size(88, 36)
      Me.btnClose.TabIndex = 4
      Me.btnClose.Text = "Close"
      '
      'btnRun
      '
      Me.btnRun.Location = New System.Drawing.Point(572, 36)
      Me.btnRun.Name = "btnRun"
      Me.btnRun.Size = New System.Drawing.Size(88, 36)
      Me.btnRun.TabIndex = 3
      Me.btnRun.Text = "Run"
      '
      'tabImageFile
      '
      Me.tabImageFile.Controls.Add(Me.mImageFileEdit)
      Me.tabImageFile.Location = New System.Drawing.Point(4, 22)
      Me.tabImageFile.Name = "tabImageFile"
      Me.tabImageFile.Size = New System.Drawing.Size(792, 341)
      Me.tabImageFile.TabIndex = 1
      Me.tabImageFile.Text = "Image File"
      '
      'mImageFileEdit
      '
      Me.mImageFileEdit.AllowDrop = True
      Me.mImageFileEdit.Dock = System.Windows.Forms.DockStyle.Fill
      Me.mImageFileEdit.Location = New System.Drawing.Point(0, 0)
      Me.mImageFileEdit.MinimumSize = New System.Drawing.Size(489, 0)
      Me.mImageFileEdit.Name = "mImageFileEdit"
      Me.mImageFileEdit.OutputHighLight = System.Drawing.Color.Lime
      Me.mImageFileEdit.Size = New System.Drawing.Size(792, 341)
      Me.mImageFileEdit.SuspendElectricRuns = False
      Me.mImageFileEdit.TabIndex = 0
      '
      'tabTool
      '
      Me.tabTool.Controls.Add(Me.mBlobToolEdit)
      Me.tabTool.Location = New System.Drawing.Point(4, 22)
      Me.tabTool.Name = "tabTool"
      Me.tabTool.Size = New System.Drawing.Size(792, 341)
      Me.tabTool.TabIndex = 2
      Me.tabTool.Text = "Vision Tool"
      '
      'mBlobToolEdit
      '
      Me.mBlobToolEdit.Dock = System.Windows.Forms.DockStyle.Fill
      Me.mBlobToolEdit.Location = New System.Drawing.Point(0, 0)
      Me.mBlobToolEdit.MinimumSize = New System.Drawing.Size(489, 0)
      Me.mBlobToolEdit.Name = "mBlobToolEdit"
      Me.mBlobToolEdit.Size = New System.Drawing.Size(792, 341)
      Me.mBlobToolEdit.SuspendElectricRuns = False
      Me.mBlobToolEdit.TabIndex = 0
      '
      'txtDescription
      '
      Me.txtDescription.Dock = System.Windows.Forms.DockStyle.Bottom
      Me.txtDescription.Location = New System.Drawing.Point(0, 373)
      Me.txtDescription.Multiline = True
      Me.txtDescription.Name = "txtDescription"
      Me.txtDescription.ReadOnly = True
      Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both
      Me.txtDescription.Size = New System.Drawing.Size(800, 108)
      Me.txtDescription.TabIndex = 1
      '
      'GraphicsRecordForm
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(800, 481)
      Me.Controls.Add(Me.txtDescription)
      Me.Controls.Add(Me.tabGraphicsRecord)
      Me.Name = "GraphicsRecordForm"
      Me.Text = "Graphics Record Sample Application"
      Me.tabGraphicsRecord.ResumeLayout(False)
      Me.tabSample.ResumeLayout(False)
      CType(Me.mDisplay, System.ComponentModel.ISupportInitialize).EndInit()
      Me.grpToolGraphics.ResumeLayout(False)
      Me.tabImageFile.ResumeLayout(False)
      CType(Me.mImageFileEdit, System.ComponentModel.ISupportInitialize).EndInit()
      Me.tabTool.ResumeLayout(False)
      CType(Me.mBlobToolEdit, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)
      Me.PerformLayout()

    End Sub

#End Region

#Region " Private Fields"
    Private mImageFileTool As CogImageFileTool = Nothing
    Private mBlobTool As CogBlobTool = Nothing
#End Region

#Region " Private Methods"
    Private Sub SyncGraphicsControls()
      chkShowBoundingBox.Checked = (mBlobTool.LastRunRecordEnable And CogBlobLastRunRecordConstants.ResultsBoundingBoxExtremaAngle) <> 0
      chkShowBoundary.Checked = (mBlobTool.LastRunRecordEnable And CogBlobLastRunRecordConstants.ResultsBoundary) <> 0
      chkShowCOM.Checked = (mBlobTool.LastRunRecordEnable And CogBlobLastRunRecordConstants.ResultsCenterOfMass) <> 0
    End Sub

    Private Sub SetLastRunRecordEnableFlag(ByVal enabled As Boolean, ByVal flag As CogBlobLastRunRecordConstants)
      If (enabled) Then
        mBlobTool.LastRunRecordEnable = mBlobTool.LastRunRecordEnable Or flag
      Else
        mBlobTool.LastRunRecordEnable = mBlobTool.LastRunRecordEnable And Not flag
      End If
    End Sub

    Private Sub ShowErrorAndExit(ByVal errorType As String, ByVal message As String)
      MessageBox.Show(message, "GraphicsRecord", MessageBoxButtons.OK)
      Me.Close()
    End Sub
#End Region

#Region " Private Event Handlers"
    ''' <summary>
    ''' BlobTool Changed event handler, checking if tool result has changed or the tool result
    ''' graphics flags have been changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mBlobTool_Changed(ByVal sender As Object, ByVal e As CogChangedEventArgs)
      Dim GraphicsRecordUpdated As Boolean = False

      ' If the graphics record's enabled flags have changed, sync the graphics
      ' control values with the graphics record's flags and set updated flag.
      If (e.StateFlags And CogBlobTool.SfLastRunRecordEnable) <> 0 Then
        SyncGraphicsControls()
        GraphicsRecordUpdated = True
      End If
      ' If the last run record has new data, set updated flag.
      If (e.StateFlags And CogBlobTool.SfCreateLastRunRecord) <> 0 Then
        GraphicsRecordUpdated = True
      End If

      ' create and redraw graphics records if they have changed      
      If GraphicsRecordUpdated Then
        ' Get the last run recrod from the blob tool.
        Dim lastRunRecord As ICogRecord = mBlobTool.CreateLastRunRecord()

        If lastRunRecord IsNot Nothing AndAlso lastRunRecord.SubRecords.ContainsKey("InputImage") Then
          ' Display the InputImage sub-record from the blob tool's last run 
          ' records.
          mDisplay.Record = lastRunRecord.SubRecords("InputImage")
          mDisplay.Fit(True)
        Else
          ' clear the display.
          mDisplay.Record = Nothing
        End If
      End If
    End Sub

    ''' <summary>
    ''' BlobTool Ran event handler, check the tool result. If tool fails, display the error message
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mBlobTool_Ran(ByVal sender As Object, ByVal e As System.EventArgs)
      If mBlobTool.RunStatus.Result <> CogToolResultConstants.Accept Then
        MessageBox.Show(mBlobTool.RunStatus.Message, "GraphicsRecord")
      End If
    End Sub

#End Region

#Region " Private Control Event Handlers"
    ''' <summary>
    ''' Run button event handler - run the sample application
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
      If Not mImageFileTool Is Nothing Then mImageFileTool.Run()
      If Not mBlobTool Is Nothing Then mBlobTool.Run()
    End Sub

    ''' <summary>
    ''' Close button event handler - Close the application
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
      Me.Close()
    End Sub
    ''' <summary>
    ''' ShowBoundingBox - enable/disable result graphics
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chkShowBoundingBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkShowBoundingBox.CheckedChanged
      SetLastRunRecordEnableFlag(chkShowBoundingBox.Checked, CogBlobLastRunRecordConstants.ResultsBoundingBoxExtremaAngle)
    End Sub

    ''' <summary>
    ''' ShowBoundary - enable/disable result graphics
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chkShowBoundary_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkShowBoundary.CheckedChanged
      SetLastRunRecordEnableFlag(chkShowBoundary.Checked, CogBlobLastRunRecordConstants.ResultsBoundary)
    End Sub

    ''' <summary>
    ''' ShowCOM - enable/disable result graphics
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chkShowCOM_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkShowCOM.CheckedChanged
      SetLastRunRecordEnableFlag(chkShowCOM.Checked, CogBlobLastRunRecordConstants.ResultsCenterOfMass)
    End Sub

#End Region

  End Class
End Namespace
