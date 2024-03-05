'*******************************************************************************
'Copyright (C) 2004 Cognex Corporation

'Subject to Cognex Corporations terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.
'*******************************************************************************
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

'This sample tests the CogColorConversionToolEditor and associated CogColorConversionTool.

'To use: Load an RGB image in the image file tool edit control and run the image file tool.
'Smiley.bmp in the installed images directory is one such file.  Then use the CogColorConversionToolEditor
'to run the CogColorConversionTool on the loaded image.
'
' needed for VisionPro

Option Explicit On 

Imports Cognex.VisionPro
' needed for image processing
Imports Cognex.VisionPro.ImageProcessing
Imports Cognex.VisionPro.ImageFile
' needed for VisionPro exceptions
Imports Cognex.VisionPro.Exceptions
Namespace CogConversionEditControlTest
  Public Class CogConversionEditControlTest
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
    Friend WithEvents cogImageFileEdit1 As Cognex.VisionPro.ImageFile.CogImageFileEdit
    Friend WithEvents CogColorConversionToolEditor1 As Cognex.VisionPro.ImageProcessing.Controls.CogColorConversionToolEditor
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CogConversionEditControlTest))
      Me.cogImageFileEdit1 = New Cognex.VisionPro.ImageFile.CogImageFileEdit
      Me.CogColorConversionToolEditor1 = New Cognex.VisionPro.ImageProcessing.Controls.CogColorConversionToolEditor
      CType(Me.cogImageFileEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'cogImageFileEdit1
      '
      Me.cogImageFileEdit1.Enabled = True
      Me.cogImageFileEdit1.Location = New System.Drawing.Point(32, 24)
      Me.cogImageFileEdit1.Name = "cogImageFileEdit1"
      Me.cogImageFileEdit1.OcxState = CType(resources.GetObject("cogImageFileEdit1.OcxState"), System.Windows.Forms.AxHost.State)
      Me.cogImageFileEdit1.Size = New System.Drawing.Size(760, 285)
      Me.cogImageFileEdit1.TabIndex = 2
      '
      'CogColorConversionToolEditor1
      '
      Me.CogColorConversionToolEditor1.Location = New System.Drawing.Point(32, 328)
      Me.CogColorConversionToolEditor1.Name = "CogColorConversionToolEditor1"
      Me.CogColorConversionToolEditor1.Size = New System.Drawing.Size(760, 264)
      Me.CogColorConversionToolEditor1.Subject = Nothing
      Me.CogColorConversionToolEditor1.TabIndex = 3
      '
      'CogConversionEditControlTest
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(832, 622)
      Me.Controls.Add(Me.CogColorConversionToolEditor1)
      Me.Controls.Add(Me.cogImageFileEdit1)
      Me.Name = "CogConversionEditControlTest"
      Me.Text = "Conversion Edit Control Test"
      CType(Me.cogImageFileEdit1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region " Private vars"
    Private myImageFileTool As new CogImageFileTool
#End Region
#Region " Image file tool 'Ran' event handler"

    ' After the image file tool has run, provide its output image to the input
    ' image of the CogColorConversionTool.
    Private Sub myImageFileTool_Ran(ByVal sender As Object, ByVal e As EventArgs)
      Dim newImage As CogImage24PlanarColor
      newImage = CType(myImageFileTool.OutputImage, CogImage24PlanarColor)
      If Not newImage Is Nothing Then
        CogColorConversionToolEditor1.Subject.InputImage = newImage
       Else
        MessageBox.Show("This sample requires a 24-bit color image as input. Please load a new image file.")
      End If
    End Sub

#End Region
#Region " Custom exception"
    ' Create a custom exception that will be thrown when the VPRO_ROOT isn't set.
    Public Class VPRORootNotSetException
      Inherits CogException

      Public Sub New()
        MyBase.new("VPRO_ROOT not set")
      End Sub
      Public Sub New(ByVal msg As String)
        MyBase.new(msg)
      End Sub
    End Class
#End Region

    Private Sub CogConversionEditControlTest_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Try
        CogColorConversionToolEditor1.Subject = New CogColorConversionTool
        cogImageFileEdit1.Subject = myImageFileTool
        AddHandler myImageFileTool.Ran, New EventHandler(AddressOf myImageFileTool_Ran)
        Dim VProLocation As String
        ' Locate the image file using the environment variable that indicates
        ' where VisionPro is installed.  If the environment variable is not set,
        ' throw an exception that will be caught by the caller.
        VProLocation = Environment.GetEnvironmentVariable("VPRO_ROOT")
        If VProLocation = "" Then
          Throw New VPRORootNotSetException
        End If

        VProLocation = VProLocation & "/Images/smiley.bmp"
        myImageFileTool.[Operator].Open(VProLocation, CogImageFileModeConstants.Read)
        myImageFileTool.Run()
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