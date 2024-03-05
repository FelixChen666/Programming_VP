'/*******************************************************************************
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

'This sample tests the CogColorConversionTool.

'*/

Option Explicit On

' needed for VisionPro
Imports Cognex.VisionPro
' needed for image processing
Imports Cognex.VisionPro.ImageProcessing
Imports Cognex.VisionPro.ImageFile
' needed for VisionPro exceptions
Imports Cognex.VisionPro.Exceptions

Public Class Form1
    Public Sub New()
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
#Region " Private vars"
    Private hasConverted As Boolean
    Private origRegion As CogRectangle = New CogRectangle
    Private mColorConversionTool As CogColorConversionTool.CogColorConversionTool = _
        New CogColorConversionTool.CogColorConversionTool
    Private inputImage As CogImage24PlanarColor
#End Region

    Private Sub btnConvert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConvert.Click
        Try
            mColorConversionTool.HSI = chkConvertToHSI.Checked
            mColorConversionTool.Region = origRegion
            mColorConversionTool.InputImage = inputImage
            mColorConversionTool.Run()
            hasConverted = True
            If (radioPlane0.Checked) Then
                convertedDisplay.Image = mColorConversionTool.OutputImage0
            ElseIf (radioPlane1.Checked) Then
                convertedDisplay.Image = mColorConversionTool.OutputImage1
            ElseIf (radioPlane2.Checked) Then
                convertedDisplay.Image = mColorConversionTool.OutputImage2
            Else

                radioPlane0.Checked = True
                convertedDisplay.Image = mColorConversionTool.OutputImage0
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub radioPlane0_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioPlane0.CheckedChanged
        If (hasConverted) Then
            convertedDisplay.Image = mColorConversionTool.OutputImage0
        ElseIf (radioPlane0.Checked) Then
            MessageBox.Show("Image hasn't yet been converted")
        End If
    End Sub

    Private Sub radioPlane1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioPlane1.CheckedChanged
        If (hasConverted) Then
            convertedDisplay.Image = mColorConversionTool.OutputImage1
        ElseIf (radioPlane1.Checked) Then
            MessageBox.Show("Image hasn't yet been converted")
        End If
    End Sub

    Private Sub radioPlane2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioPlane2.CheckedChanged
        If (hasConverted) Then
            convertedDisplay.Image = mColorConversionTool.OutputImage2
        ElseIf (radioPlane2.Checked) Then
            MessageBox.Show("Image hasn't yet been converted")
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim mImageFileTool As CogImageFileTool = New CogImageFileTool
        Dim VProLocation As String
        ' Locate the image file using the environment variable that indicates
        ' where VisionPro is installed.  If the environment variable is not set,
        ' throw an exception that will be caught by the caller.
        Try
            VProLocation = Environment.GetEnvironmentVariable("VPRO_ROOT")
            If (VProLocation = Nothing) Or (VProLocation = "") Then
                Throw New VPRORootNotSetException
            End If
            VProLocation = VProLocation + "/Images/smiley.bmp"
            mImageFileTool.[Operator].Open(VProLocation, CogImageFileModeConstants.Read)
            hasConverted = False
            mImageFileTool.Run()
            inputImage = CType(mImageFileTool.OutputImage, CogImage24PlanarColor)
            origDisplay.Image = inputImage
            origRegion.GraphicDOFEnable = CogRectangleDOFConstants.All
            origRegion.Interactive = True
            origRegion.SetXYWidthHeight(inputImage.Width / 4, inputImage.Height / 4, inputImage.Width / 2, inputImage.Height / 2)
            origDisplay.InteractiveGraphics.Add(origRegion, "Input Region", False)
        Catch ex As CogException
            MessageBox.Show(ex.Message)
            Application.Exit()
        Catch gex As Exception
            MessageBox.Show(gex.Message)
            Application.Exit()
        End Try

    End Sub


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

End Class