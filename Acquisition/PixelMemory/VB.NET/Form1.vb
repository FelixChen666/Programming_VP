'*******************************************************************************
' Copyright (C) 2004 Cognex Corporation
'
' Subject to Cognex Corporation's terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore, you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
'*******************************************************************************
'
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This application illustrates how to pass user-supplied memory
' into a CogImage8Grey without copying the pixel data. The CImageRoot
' class allocates memory from the Windows heap for illustration purposes,
' but it can be easily modified to use image memory allocated by a third-party
' image acquisition system for example. Once the pixel memory is passed
' into the CogImage8Grey, the resulting image can be used by VisionPro tools.
'
' This program assumes that you have some knowledge of VB and VisionPro programming.
'
' This sample uses the CogCopyRegion tool to fill the image with a grey color, and
' to draw a white ellipse at a random location.
'
' The following steps show how to pass user-supplied memory into a CogImage8Grey
' without copying the pixel data.
' Step 1) Create a CogImage8Grey object and set its root to the newly allocated
'         memory
' Step 2) Install the image to the CogDisplay.
'
Option Explicit On 
' Needed for VisionPro
Imports Cognex.VisionPro
' Needed for VisionPro Exceptions
Imports Cognex.VisionPro.Exceptions
' Needed for VisionPro Image Processing
Imports Cognex.VisionPro.ImageProcessing
Namespace PixelMemory
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
    Friend WithEvents textBox1 As System.Windows.Forms.TextBox
    Friend WithEvents cmdGrabNewImage As System.Windows.Forms.Button
    Friend WithEvents CogDisplay1 As Cognex.VisionPro.Display.CogDisplay
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form1))
      Me.textBox1 = New System.Windows.Forms.TextBox
      Me.cmdGrabNewImage = New System.Windows.Forms.Button
      Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'textBox1
      '
      Me.textBox1.Location = New System.Drawing.Point(0, 424)
      Me.textBox1.Multiline = True
      Me.textBox1.Name = "textBox1"
      Me.textBox1.ReadOnly = True
      Me.textBox1.Size = New System.Drawing.Size(504, 56)
      Me.textBox1.TabIndex = 9
      Me.textBox1.Text = "Sample description: illustrates how to pass user-supplied memory into a CogImage8" & _
      "Grey " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "without copying the pixel data. Once the pixel memory is passed into the " & _
      "" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "CogImage8Grey, the resulting image can be used by VisionPro tools." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Note: This" & _
      " sample does not acquire an actual image."
      Me.textBox1.WordWrap = False
      '
      'cmdGrabNewImage
      '
      Me.cmdGrabNewImage.Location = New System.Drawing.Point(16, 376)
      Me.cmdGrabNewImage.Name = "cmdGrabNewImage"
      Me.cmdGrabNewImage.Size = New System.Drawing.Size(128, 32)
      Me.cmdGrabNewImage.TabIndex = 10
      Me.cmdGrabNewImage.Text = " Grab New Image"
      '
      'CogDisplay1
      '
      Me.CogDisplay1.Location = New System.Drawing.Point(8, 16)
      Me.CogDisplay1.Name = "CogDisplay1"
      Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
      Me.CogDisplay1.Size = New System.Drawing.Size(488, 352)
      Me.CogDisplay1.TabIndex = 11
      '
      'Form1
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(512, 486)
      Me.Controls.Add(Me.CogDisplay1)
      Me.Controls.Add(Me.cmdGrabNewImage)
      Me.Controls.Add(Me.textBox1)
      Me.Name = "Form1"
      Me.Text = "Pixel Memory Example"
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdGrabNewImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGrabNewImage.Click
      Try
        ' Step 1) Create a new CogImage8Grey and set its root to the user-allocated
        '         pixel memory
        Dim Image As New CogImage8Grey
        Dim NewRoot As New CImageRoot

        Image.SetRoot(NewRoot.CogImage8RootFromMemory(640, 480))

        ' Fill the image with grey
        Dim CopyRegionTool As New CogCopyRegionTool
        ' First, do not fill the bounding box with FillBoundingBoxValue
        CopyRegionTool.RunParams.FillBoundingBox = False
        ' Now, fill the destination region with FillRegionValue instead of copying pixels
        ' from the source image.
        CopyRegionTool.RunParams.FillRegion = True
        CopyRegionTool.RunParams.FillRegionValue = 128  ' grey color
        CopyRegionTool.Region = Nothing                 ' Choose the entire region
        CopyRegionTool.InputImage = Image               ' Source and destination images are the same
        CopyRegionTool.DestinationImage = Image
        CopyRegionTool.Run()

        ' Draw a white ellipse at a random location
        Dim Ellipse As New CogEllipse
        Ellipse.SetCenterXYRadiusXYRotation(200, 100, 200, 100, 0)
        CopyRegionTool.RunParams.ImageAlignmentEnabled = True
        ' Assign random x and y coordinates
        CopyRegionTool.RunParams.DestinationImageAlignmentX = Rnd() * 240
        CopyRegionTool.RunParams.DestinationImageAlignmentY = Rnd() * 280
        CopyRegionTool.RunParams.FillBoundingBoxValue = 0
        CopyRegionTool.RunParams.FillRegionValue = 255    ' white color
        CopyRegionTool.Region = Ellipse                 ' Choose the entire region
        CopyRegionTool.InputImage = Image               ' Source and destination images are the same
        CopyRegionTool.DestinationImage = Image
        CopyRegionTool.Run()

        ' Step 2) Display the image
        CogDisplay1.Image = Image
      Catch ex As CogException
        MessageBox.Show(ex.Message)
        Application.Exit()
      Catch gex As Exception
        MessageBox.Show(gex.Message)
        Application.Exit()
      End Try
    End Sub
  End Class
End Namespace