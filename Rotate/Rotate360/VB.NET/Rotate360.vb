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

' This sample demonstrates how rotate an image using an affine transform.
' The sample uses the objects and interfaces defined in the Cognex Core and
' Cognex Image type libraries.
'
' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
'
'
' The following steps show how to rotate both image and shapes.
' Step 1) Load an image.
' Step 2) Create a CogAffineTransformTool.
' Step 3) Rotate the image from 0 to 360 by 1 degree increments.
'

Option Explicit On 
Imports Cognex.VisionPro
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.ImageProcessing
Imports Cognex.VisionPro.Exceptions
Namespace SampleRotate360
  Friend Class frmRotate360
    Inherits System.Windows.Forms.Form
#Region "Windows Form Designer generated code "
    Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()
    End Sub
    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
      If Disposing Then
        If Not components Is Nothing Then
          components.Dispose()
        End If
      End If
      MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents Text1 As System.Windows.Forms.TextBox
    Public WithEvents cmdRotate As System.Windows.Forms.Button

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents CogDisplay1 As Cognex.VisionPro.Display.CogDisplay
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmRotate360))
      Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
      Me.Text1 = New System.Windows.Forms.TextBox
      Me.cmdRotate = New System.Windows.Forms.Button
      Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'Text1
      '
      Me.Text1.AcceptsReturn = True
      Me.Text1.AutoSize = False
      Me.Text1.BackColor = System.Drawing.SystemColors.Window
      Me.Text1.Cursor = System.Windows.Forms.Cursors.IBeam
      Me.Text1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.Text1.ForeColor = System.Drawing.SystemColors.WindowText
      Me.Text1.Location = New System.Drawing.Point(0, 496)
      Me.Text1.MaxLength = 0
      Me.Text1.Multiline = True
      Me.Text1.Name = "Text1"
      Me.Text1.RightToLeft = System.Windows.Forms.RightToLeft.No
      Me.Text1.Size = New System.Drawing.Size(681, 24)
      Me.Text1.TabIndex = 2
      Me.Text1.Text = "Sample Description: demonstrates how rotate an image using an affine transform." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & _
      ""
      '
      'cmdRotate
      '
      Me.cmdRotate.BackColor = System.Drawing.SystemColors.Control
      Me.cmdRotate.Cursor = System.Windows.Forms.Cursors.Default
      Me.cmdRotate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.cmdRotate.ForeColor = System.Drawing.SystemColors.ControlText
      Me.cmdRotate.Location = New System.Drawing.Point(528, 16)
      Me.cmdRotate.Name = "cmdRotate"
      Me.cmdRotate.RightToLeft = System.Windows.Forms.RightToLeft.No
      Me.cmdRotate.Size = New System.Drawing.Size(116, 33)
      Me.cmdRotate.TabIndex = 1
      Me.cmdRotate.Text = "Rotate 360 degrees"
      '
      'CogDisplay1
      '
      Me.CogDisplay1.Location = New System.Drawing.Point(16, 16)
      Me.CogDisplay1.Name = "CogDisplay1"
      Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
      Me.CogDisplay1.Size = New System.Drawing.Size(504, 392)
      Me.CogDisplay1.TabIndex = 3
      '
      'frmRotate360
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.BackColor = System.Drawing.SystemColors.Control
      Me.ClientSize = New System.Drawing.Size(800, 536)
      Me.Controls.Add(Me.CogDisplay1)
      Me.Controls.Add(Me.Text1)
      Me.Controls.Add(Me.cmdRotate)
      Me.Cursor = System.Windows.Forms.Cursors.Default
      Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.Location = New System.Drawing.Point(4, 30)
      Me.Name = "frmRotate360"
      Me.RightToLeft = System.Windows.Forms.RightToLeft.No
      Me.Text = "Shows how to rotate 360 degrees using an Affine Transform."
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub
#End Region
#Region "Module Level vars"
    Private mTool As CogAffineTransformTool
    Private StaticGraphic As New CogRectangleAffine
    Private ImageFile As CogImageFile
#End Region
#Region "Form and Controls Events"
    Private Sub frmRotate360_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
      Try

        ' Create the CogAffineTransformTool
        mTool = New CogAffineTransformTool

        ' Get VPRO_ROOT from environment which is needed to locate bracket_std.idb.
        Const ImageFileName As String = "/Images/bracket_std.idb"

        Dim strBaseDir As String
        strBaseDir = Environment.GetEnvironmentVariable("VPRO_ROOT")
        If strBaseDir = "" Then
          DisplayErrorAndExit("Required environment variable VPRO_ROOT not set.")
        End If

        ' Step 1 - Load an image and create shapes.
        ' Temporarily create the image file tool to open bracket_std.idb.
        ImageFile = New CogImageFile
        ImageFile.Open(strBaseDir & ImageFileName, CogImageFileModeConstants.Read)
        ' We only need the first image

        mTool.InputImage = ImageFile.Item(0)
        CogDisplay1.Image = ImageFile.Item(0)
        ' Close the image file since we are going to use the same image.
        ImageFile.Close()

        ' Let's add the graphics to the CogDisplay
        AddGraphics()
      Catch cogex As CogException
        DisplayErrorAndExit("Following Specific Cognex Error Occured:" & cogex.Message)
      Catch ex As Exception
        DisplayErrorAndExit("Encountered the following error: " & ex.Message)
      End Try
    End Sub
    ' Executes the flipping operation
    Private Sub cmdRotate_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRotate.Click
      Try
        Dim r As Double

        For r = 0.0# To 360.0# Step 1.0#
          ' increase the angle
          StaticGraphic.Rotation = r / 180.0# * 3.14159
          mTool.Region = StaticGraphic

          ' Perform the flip operation and produce an output image
          mTool.Run()

          ' Show the result.
          CogDisplay1.Image = mTool.OutputImage
        Next r
      Catch cogex As CogException
        DisplayErrorAndExit("Following Specific Cognex Error Occured:" & cogex.Message)
      Catch ex As Exception
        DisplayErrorAndExit("Encountered the following error: " & ex.Message)
      End Try
    End Sub
    Private Sub frmRotate360_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
      If Not mTool Is Nothing Then mTool.Dispose()

    End Sub
#End Region
#Region "Module Level Routines"
    ' Add only static graphics
    Private Sub AddGraphics()
      Try
        ' Create and initialize the new graphic.

        StaticGraphic.CenterX = 320
        StaticGraphic.CenterY = 240
        StaticGraphic.SideXLength = 600
        StaticGraphic.SideYLength = 440
        StaticGraphic.Rotation = 0
        StaticGraphic.Color = CogColorConstants.Orange
        StaticGraphic.LineWidthInScreenPixels = StaticGraphic.LineWidthInScreenPixels
        StaticGraphic.Interactive = True

        ' Draw the affine rectangle in a pixel space.
        StaticGraphic.SelectedSpaceName = "."

        ' Add the new graphic.
        CogDisplay1.StaticGraphics.Add(StaticGraphic, "test")

        ' Use High Precision Affine, it doesn't take much longer than Bilinear and it's more accurate.
        mTool.RunParams.SamplingMode = CogAffineTransformSamplingModeConstants.HighPrecision
      Catch cogex As CogException
        DisplayErrorAndExit("Following Specific Cognex Error Occured:" & cogex.Message)
      Catch ex As Exception
        DisplayErrorAndExit("Encountered the following error: " & Err.Description)
      End Try
    End Sub


    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Helper function.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Displays an error message and then exits the program.
    ' Call this when an unrecoverable error has occurred.
    Private Sub DisplayErrorAndExit(ByVal ErrorMsg As String)
      DisplayErrorAndExit(ErrorMsg & Environment.NewLine & "Press OK to exit.")
      Me.Close()
      End ' quit if it is called from Form_Load
    End Sub

#End Region


 
  End Class
End Namespace