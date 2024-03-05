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

' This sample demonstrates how to train PatMax with a training region and a
' training mask that blocks some of the features from being trained.
'
' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
'
' The following steps show how to find circular blobs in images.
' Step 1) Load a training image
' Step 2) Load a training mask image.
' Step 3) Train PatMax using the training image and mask image.
'         Display trained features so user can see what was masked and what wasn't.
' Step 4) Run PatMax, display only the found features.  Notice that only
'         the features that were trained will be found at runtime!

Option Explicit On 
Imports Cognex.VisionPro
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.PMAlign
Imports Cognex.VisionPro.Exceptions
Namespace SamplePMAlignTrainWithMask
  Public Class frmPMAlignTrainWithMask
    Inherits System.Windows.Forms.Form

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
      End If
      MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents CogDisplay1 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents CogDisplay2 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents cmdTrain As System.Windows.Forms.Button
    Friend WithEvents cmdRun As System.Windows.Forms.Button
    Friend WithEvents lblImgTitle As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmPMAlignTrainWithMask))
      Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
      Me.CogDisplay2 = New Cognex.VisionPro.Display.CogDisplay
      Me.cmdTrain = New System.Windows.Forms.Button
      Me.cmdRun = New System.Windows.Forms.Button
      Me.lblImgTitle = New System.Windows.Forms.Label
      Me.Label1 = New System.Windows.Forms.Label
      Me.TextBox1 = New System.Windows.Forms.TextBox
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.CogDisplay2, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'CogDisplay1
      '
      Me.CogDisplay1.Location = New System.Drawing.Point(8, 40)
      Me.CogDisplay1.Name = "CogDisplay1"
      Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
      Me.CogDisplay1.Size = New System.Drawing.Size(328, 272)
      Me.CogDisplay1.TabIndex = 0
      '
      'CogDisplay2
      '
      Me.CogDisplay2.Location = New System.Drawing.Point(400, 40)
      Me.CogDisplay2.Name = "CogDisplay2"
      Me.CogDisplay2.OcxState = CType(resources.GetObject("CogDisplay2.OcxState"), System.Windows.Forms.AxHost.State)
      Me.CogDisplay2.Size = New System.Drawing.Size(336, 272)
      Me.CogDisplay2.TabIndex = 1
      '
      'cmdTrain
      '
      Me.cmdTrain.Location = New System.Drawing.Point(248, 328)
      Me.cmdTrain.Name = "cmdTrain"
      Me.cmdTrain.Size = New System.Drawing.Size(88, 48)
      Me.cmdTrain.TabIndex = 2
      Me.cmdTrain.Text = "Train"
      '
      'cmdRun
      '
      Me.cmdRun.Location = New System.Drawing.Point(400, 328)
      Me.cmdRun.Name = "cmdRun"
      Me.cmdRun.Size = New System.Drawing.Size(88, 48)
      Me.cmdRun.TabIndex = 3
      Me.cmdRun.Text = "Run"
      '
      'lblImgTitle
      '
      Me.lblImgTitle.Location = New System.Drawing.Point(480, 8)
      Me.lblImgTitle.Name = "lblImgTitle"
      Me.lblImgTitle.Size = New System.Drawing.Size(152, 23)
      Me.lblImgTitle.TabIndex = 4
      Me.lblImgTitle.Text = "Training Mask Image"
      '
      'Label1
      '
      Me.Label1.Location = New System.Drawing.Point(112, 8)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(152, 23)
      Me.Label1.TabIndex = 5
      Me.Label1.Text = "Training Image"
      '
      'TextBox1
      '
      Me.TextBox1.Location = New System.Drawing.Point(24, 392)
      Me.TextBox1.Multiline = True
      Me.TextBox1.Name = "TextBox1"
      Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
      Me.TextBox1.Size = New System.Drawing.Size(712, 56)
      Me.TextBox1.TabIndex = 6
      Me.TextBox1.Text = "Sample Description: Demonstrates the use of a mask while training a PatMax patter" & _
      "n.  Black pixels (value 0) are considered ""don't care"" pixels, white pixels (val" & _
      "ue 255) are considered ""care"" pixels."
      '
      'frmPMAlignTrainWithMask
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(784, 462)
      Me.Controls.Add(Me.TextBox1)
      Me.Controls.Add(Me.Label1)
      Me.Controls.Add(Me.lblImgTitle)
      Me.Controls.Add(Me.cmdRun)
      Me.Controls.Add(Me.cmdTrain)
      Me.Controls.Add(Me.CogDisplay2)
      Me.Controls.Add(Me.CogDisplay1)
      Me.Name = "frmPMAlignTrainWithMask"
      Me.Text = "Shows how to train a PMAlign pattern with masking"
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.CogDisplay2, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Module Level vars"
    Private mTool As CogPMAlignTool
    Private ImageFile As New CogImageFileTool
    Private MaskFile As New CogImageFileTool
#End Region
#Region "Form and Controls Events"
    Private Sub cmdTrain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTrain.Click
      Try



        ' Point to the first image so that when then the image tool runs next
        ' it will output this first image.
        ImageFile.NextImageIndex = 0

        ' Step 3, train a patmax pattern using a mask image to
        ' filter out unreliable or undesireable features.
        CogDisplay1.Image = ImageFile.[Operator].Item(0)
        CogDisplay2.Image = MaskFile.[Operator].Item(0)
        lblImgTitle.Text = "Training Mask Image"

        ' Clear any old graphics.
        CogDisplay1.StaticGraphics.Clear()
        CogDisplay2.StaticGraphics.Clear()

        Dim PatMaxTrainRegion As CogRectangleAffine
        PatMaxTrainRegion = mTool.Pattern.TrainRegion
        If Not PatMaxTrainRegion Is Nothing Then
          PatMaxTrainRegion.SetCenterLengthsRotationSkew(320, 240, 500, 400, 0, 0)
          PatMaxTrainRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position Or CogRectangleAffineDOFConstants.Position Or CogRectangleAffineDOFConstants.Size

        End If

        ' Setup the training image and the training mask.
        mTool.Pattern.TrainImage = ImageFile.[Operator].Item(0)
        mTool.Pattern.TrainImageMask = MaskFile.[Operator].Item(0)

        ' Train the tool in PatMax mode.
        ' Note: PatQuick mode does not have the same runtime output (graphics).
        mTool.Pattern.TrainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatMaxAndPatQuick
        mTool.Pattern.Train()

        ' Show training time coarse and fine feature graphics.
        CogDisplay1.StaticGraphics.AddList(mTool.Pattern.CreateGraphicsCoarse(Cognex.VisionPro.CogColorConstants.Cyan), "test")
        CogDisplay1.StaticGraphics.AddList(mTool.Pattern.CreateGraphicsFine(Cognex.VisionPro.CogColorConstants.Green), "test")

        'enable the Run Button
        cmdRun.Enabled = True

      Catch cogex As CogException
        MessageBox.Show("Following Specific Cognex Error Occured:" & cogex.Message)
      Catch ex As Exception
        DisplayErrorAndExit("Encountered the following error: " & Err.Description)
      End Try
    End Sub


    Private Sub cmdRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRun.Click
      Try

        ' Step 4 - Run PatMax, overlay all the runtime features on the runtime image.
        '          Notice the holes were not trained and therefore do not show up
        '          as features detected at runtime!

        ' Setup for running.
        ImageFile.Run()
        mTool.InputImage = ImageFile.OutputImage
        mTool.RunParams.SaveMatchInfo = True
        mTool.RunParams.ScoreUsingClutter = False

        ' Allow +/- 45 degrees of rotation.
        mTool.RunParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh
        mTool.RunParams.ZoneAngle.High = 3.14159 / 4   ' +45 degrees
        mTool.RunParams.ZoneAngle.Low = -3.14159 / 4   ' -45 degrees

        ' Allow +/- 5 percent of scale.
        mTool.RunParams.ZoneScale.Configuration = CogPMAlignZoneConstants.LowHigh
        mTool.RunParams.ZoneScale.High = 1.05  ' +5 percent
        mTool.RunParams.ZoneScale.Low = 0.95   ' -5 percent

        ' Run PatMax
        mTool.Run()
        ' Display the runtime pattern shapes on top of the runtime image.
        If mTool.Results Is Nothing Then

        Else ' No results. You can notify the user...
          Dim resultGraphics As CogCompositeShape
          CogDisplay2.Image = ImageFile.OutputImage
          CogDisplay2.StaticGraphics.Clear()
          ' We only want to show the first result in this sample even though there may be more than one result.
          resultGraphics = mTool.Results(0).CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchRegion Or _
           CogPMAlignResultGraphicConstants.MatchFeatures)
          CogDisplay2.StaticGraphics.Add(resultGraphics, "test")
          lblImgTitle.Text = "LastRun OutputImage"
        End If

      Catch cogex As CogException
        MessageBox.Show("Following Specific Cognex Error Occured:" & cogex.Message)
      Catch ex As Exception
        DisplayErrorAndExit("Encountered the following error: " & Err.Description)
      End Try
    End Sub
    Private Sub frmPMAlignTrainWithMask_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Try

        'disable the Run Button
        cmdRun.Enabled = False

        ' Create the CogPMAlignTool
        mTool = New CogPMAlignTool

        ' Get VPRO_ROOT from environment which is needed to locate bracket_std.idb.
        Const ImageFileName As String = "/Images/bracket_std.idb"
        Const MaskFileName As String = "/Images/bracket_mask.bmp"
        Dim strBaseDir As String
        strBaseDir = Environment.GetEnvironmentVariable("VPRO_ROOT")
        If strBaseDir = "" Then
          DisplayErrorAndExit("Required environment variable VPRO_ROOT not set.")
        End If

        ' Step 1 - Load an image and create shapes.
        ImageFile.[Operator].Open(strBaseDir & ImageFileName, CogImageFileModeConstants.Read)

        ' Step 2 - Load a training image to use as a masek
        MaskFile.[Operator].Open(strBaseDir & MaskFileName, CogImageFileModeConstants.Read)

        ' We only need the first image
        CogDisplay1.Image = ImageFile.[Operator].Item(0)
        CogDisplay2.Image = MaskFile.[Operator].Item(0)
        CogDisplay1.Fit()
        CogDisplay2.Fit()
      Catch cogex As CogException
        MessageBox.Show("Following Specific Cognex Error Occured:" & cogex.Message)
      Catch ex As Exception
        DisplayErrorAndExit("Encountered the following error: " & ex.Message)
      End Try
    End Sub
    Private Sub frmPMAlignTrainWithMask_closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
      If Not ImageFile Is Nothing Then ImageFile.Dispose()
      If Not mTool Is Nothing Then mTool.Dispose()
    End Sub
#End Region
#Region "Module Level Helper Routine"
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Helper function.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Displays an error message and then exits the program.
    ' Call this when an unrecoverable error has occurred.
    Private Sub DisplayErrorAndExit(ByVal ErrorMsg As String)
      MessageBox.Show(ErrorMsg & Environment.NewLine & "Press OK to exit.")
      End
    End Sub

#End Region


  End Class
End Namespace
