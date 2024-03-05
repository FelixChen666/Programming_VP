'*******************************************************************************
'Copyright (C) 2008 Cognex Corporation

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

Public Class Form1
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        AddHandler CogImageFileEditV21.Subject.Ran, New EventHandler(AddressOf mImageFileTool_Ran)
        ' Add any initialization after the InitializeComponent() call.

    End Sub

#Region " Image file tool 'Ran' event handler"

    ' After the image file tool has run, provide its output image to the input
    ' image of the CogColorConversionTool.
    Private Sub mImageFileTool_Ran(ByVal sender As Object, ByVal e As EventArgs)
        Dim newImage As CogImage24PlanarColor
        newImage = TryCast(CogImageFileEditV21.Subject.OutputImage, CogImage24PlanarColor)
        If Not newImage Is Nothing Then
            CogColorConversionToolEditor1.Subject.InputImage = newImage
        Else
            MessageBox.Show("This sample requires a 24-bit color image as input. Please load a new image file.")
        End If
    End Sub

#End Region
End Class
