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

' This sample demonstrates how rotate both image and graphics by 90 degrees.
' The sample uses the objects and interfaces defined in the Cognex Core and
' Cognex Image type libraries.
'
' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
'
' See samples\Programming\Graphics\DisplayGraphics sample to learn more about
' graphics programming.
'
' The following steps show how to rotate both image and shapes.
' Step 1) Load an image and create shapes.
' Step 2) Create the CogIPOneImageFlipRotate and add it to the CogIPOneImageTool.
' Step 3) Set the OperationInPixelSpace property to cogIPOneImageFlipRotateOperationRotate90Deg
'         and run the tool
'
Option Explicit On 
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.ImageProcessing
Imports Cognex.VisionPro
Imports Cognex.VisionPro.Exceptions
Namespace SampleRotate90
  Public Class Form1
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
    Friend WithEvents btnRotate As System.Windows.Forms.Button
    Friend WithEvents SampleDescription As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form1))
      Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
      Me.btnRotate = New System.Windows.Forms.Button
      Me.SampleDescription = New System.Windows.Forms.TextBox
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'CogDisplay1
      '
      Me.CogDisplay1.Location = New System.Drawing.Point(128, 16)
      Me.CogDisplay1.Name = "CogDisplay1"
      Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
      Me.CogDisplay1.Size = New System.Drawing.Size(504, 368)
      Me.CogDisplay1.TabIndex = 0
      '
      'btnRotate
      '
      Me.btnRotate.Location = New System.Drawing.Point(8, 24)
      Me.btnRotate.Name = "btnRotate"
      Me.btnRotate.Size = New System.Drawing.Size(104, 48)
      Me.btnRotate.TabIndex = 1
      Me.btnRotate.Text = "Rotate"
      '
      'SampleDescription
      '
      Me.SampleDescription.Location = New System.Drawing.Point(16, 432)
      Me.SampleDescription.Multiline = True
      Me.SampleDescription.Name = "SampleDescription"
      Me.SampleDescription.Size = New System.Drawing.Size(616, 20)
      Me.SampleDescription.TabIndex = 2
      Me.SampleDescription.Text = "Sample description: shows how to rotate an image and graphics by 90 degrees."
      '
      'Form1
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(680, 502)
      Me.Controls.Add(Me.SampleDescription)
      Me.Controls.Add(Me.btnRotate)
      Me.Controls.Add(Me.CogDisplay1)
      Me.Name = "Form1"
      Me.Text = "Form1"
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region "Module Level vars"
    Private mTool As CogIPOneImageTool
#End Region
#Region "Form and Controls Events"
    Private Sub btnRotate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRotate.Click
      Try

        ' Step 2 - Create the CogIPOneImageFlipRotate.
        ' This object allows an image to be rotated and/or flipped to produce a new image.
        ' The CogIPOneImageTool will not create the CogIPOneImageFlipRotate operator by default.
        ' We must create one and add it to the CogIPOneImageTool.
        Dim Flip As CogIPOneImageFlipRotate
        Flip = New CogIPOneImageFlipRotate
        mTool.Operators.Add(Flip)

		' Step 3 - Set the OperationInPixelSpace property to cogIPOneImageFlipRotateOperationRotate90Deg
		'          and run the tool
		' The operation used to reorient the image.
		Flip.OperationInPixelSpace = CogIPOneImageFlipRotateOperationConstants.Rotate90Deg

		' Perform the flip operation and produce an output image
		mTool.Run()

		' Show the result.
		CogDisplay1.Image = mTool.OutputImage
      Catch cogex As CogException

        DisplayErrorAndExit("Encountered the following Cognex specific error: " & cogex.Message)
      Catch ex As Exception

        DisplayErrorAndExit("Encountered the following error: " & ex.Message)
      End Try
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Try

        ' Create the CogIPOneImageTool
        mTool = New CogIPOneImageTool

        ' Get VPRO_ROOT from environment which is needed to locate polygonRegion.cdb.
        Const ImageFileName As String = "/Images/bracket_std.idb"
        Dim strBaseDir As String
        strBaseDir = Environment.GetEnvironmentVariable("VPRO_ROOT")
        If strBaseDir = "" Then _
          DisplayErrorAndExit("Required environment variable VPRO_ROOT not set.")

        ' Step 1 - Load an image and create shapes.
        ' Temporarily create the image file tool to open bracket_std.idb.
        Dim ImageFile As New CogImageFileTool
        ImageFile.[Operator].Open(strBaseDir & ImageFileName, CogImageFileModeConstants.Read)
        ' We only need the first image
        mTool.InputImage = ImageFile.[Operator].Item(0)
        CogDisplay1.Image = ImageFile.[Operator].Item(0)
        ' Close the image file since we are going to use the same image.
        ImageFile.[Operator].Close()

        ' Let's add the graphics to the CogDisplay
        AddGraphics()
      Catch cogex As CogException

        DisplayErrorAndExit("Encountered the following Cognex specific error: " & cogex.Message)
      Catch ex As Exception

        DisplayErrorAndExit("Encountered the following error: " & ex.Message)
      End Try
    End Sub
    Private Sub Form1_closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
      If Not mTool Is Nothing Then mTool.Dispose()
    End Sub
#End Region
#Region "Module Level Routines"
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Helper function.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Displays an error message and then exits the program.
    ' Call this when an unrecoverable error has occurred.
    Private Sub DisplayErrorAndExit(ByVal ErrorMsg As String)
      MessageBox.Show(ErrorMsg & Environment.NewLine & "Press OK to exit.")
      Me.Close()
      End      ' quit if it is called from Form_Load
    End Sub


    ' Add only static graphics
    Private Sub AddGraphics()
      ' Create and initialize the new graphic.
      Dim StaticGraphic As New CogRectangleAffine
      StaticGraphic.CenterX = 200
      StaticGraphic.CenterY = 200
      StaticGraphic.Rotation = 20.0#
      StaticGraphic.Color = Cognex.VisionPro.CogColorConstants.Orange
      StaticGraphic.LineWidthInScreenPixels = StaticGraphic.LineWidthInScreenPixels * 3
      ' Draw the affine rectangle in a pixel space.
      StaticGraphic.SelectedSpaceName = "."
      ' Add the new graphic.
      CogDisplay1.StaticGraphics.Add(StaticGraphic, "test")

      ' Draw a line on the display
      Dim Line3 As New CogLineSegment
      Line3.Color = Cognex.VisionPro.CogColorConstants.Magenta
      Line3.StartX = 300
      Line3.StartY = 170
      Line3.EndX = 500
      Line3.EndY = 170
      ' Draw the line in a pixel space.
      Line3.SelectedSpaceName = "."
      ' Add the new graphic.
      CogDisplay1.StaticGraphics.Add(Line3, "test")
    End Sub
#End Region


  End Class
End Namespace