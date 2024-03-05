'*******************************************************************************
' Copyright (C) 2006 Cognex Corporation
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

' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
'
' The following steps show how to segment colors in images.
' Step 1) Load an image.
' Step 2) Create a CogColorSegmenterTool.
' Step 3) Add color ranges to the Tool.
' Step 4) Get and display the segment image based on selected color ranges.
'         
'
Option Explicit On
Imports Cognex.VisionPro            ' need to access basic VisionPro functionality
Imports Cognex.VisionPro.ColorSegmenter       'need to access CogColorSegmenterTool
Imports Cognex.VisionPro.ImageFile  'need to access ImageFileTool
Imports Cognex.VisionPro.Exceptions

Public Class ColorImageSegmenter
    Private mTool As CogColorSegmenterTool
    Private mImageFileTool As CogImageFileTool
    Private mColorRangesInitialized As Boolean

    Private Sub ColorImageSegmenter_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        mColorRangesInitialized = False

        Try

            ' Create the CogColorSegmenterTool and CogImageFileTool.
            mTool = New CogColorSegmenterTool
            mImageFileTool = New CogImageFileTool

            ' Get VPRO_ROOT from environment which is needed to locate the color image.
            Const ImageFileName As String = "/Images/blister.tif"
            Dim strBaseDir As String
            strBaseDir = Environment.GetEnvironmentVariable("VPRO_ROOT")
            If strBaseDir = "" Then
                DisplayErrorAndExit("Required environment variable VPRO_ROOT not set.")
            End If

            ' Step 1 - Load an image and create color ranges.
            ' Temporarily create the image file tool to open a file of color image.
            mImageFileTool.[Operator].Open(strBaseDir & ImageFileName, CogImageFileModeConstants.Read)
            mImageFileTool.Run()
            ' We only need the first image
            CogDisplayInputImage.Image = mImageFileTool.OutputImage

            'Initialize the color ranges, and add them to the color range 
            'collection of the tool.
            'Since it is difficult to segment all the regions of interest 
            'using only one color range, multiple color ranges will be defined.
            Dim defaultColor As CogSimpleColor
            defaultColor = New CogSimpleColor(CogImageColorSpaceConstants.RGB)

            'Define 2 yellow color ranges for segmenting yellow color regions.
            Dim colorRangeYellow1 As CogColorRangeItem
            colorRangeYellow1 = New CogColorRangeItem(defaultColor)
            'Update the range's Nominal, LowTolerance, HighTolerance, and 
            'Softness values.
            colorRangeYellow1.PlaneRange0.Update(229, -81, 26, 0)
            colorRangeYellow1.PlaneRange1.Update(227, -100.2, 28, 0)
            colorRangeYellow1.PlaneRange2.Update(49, -33.3, 36.7, 0)
            mTool.RunParams.ColorRangeCollection.Add(colorRangeYellow1)

            Dim colorRangeYellow2 As CogColorRangeItem
            colorRangeYellow2 = New CogColorRangeItem(defaultColor)
            colorRangeYellow2.PlaneRange0.Update(130, -29.8, 23.7, 0)
            colorRangeYellow2.PlaneRange1.Update(122, -28.1, 32.7, 0)
            colorRangeYellow2.PlaneRange2.Update(22, -11.8, 11.8, 0)
            mTool.RunParams.ColorRangeCollection.Add(colorRangeYellow2)

            'Define 3 red color ranges for segmenting red color regions.
            Dim colorRangeRed1 As CogColorRangeItem
            colorRangeRed1 = New CogColorRangeItem(defaultColor)
            colorRangeRed1.PlaneRange0.Update(109, -28.4, 33.8, 0)
            colorRangeRed1.PlaneRange1.Update(34, -20.8, 17.8, 0)
            colorRangeRed1.PlaneRange2.Update(11, -8.7, 10.2, 0)
            colorRangeRed1.Selected = False
            mTool.RunParams.ColorRangeCollection.Add(colorRangeRed1)

            Dim colorRangeRed2 As CogColorRangeItem
            colorRangeRed2 = New CogColorRangeItem(defaultColor)
            colorRangeRed2.PlaneRange0.Update(197, -77.4, 46.6, 0)
            colorRangeRed2.PlaneRange1.Update(65, -26.7, 25.9, 0)
            colorRangeRed2.PlaneRange2.Update(23, -20.2, 16.3, 0)
            colorRangeRed2.Selected = False
            mTool.RunParams.ColorRangeCollection.Add(colorRangeRed2)

            Dim colorRangeRed3 As CogColorRangeItem
            colorRangeRed3 = New CogColorRangeItem(defaultColor)
            colorRangeRed3.PlaneRange0.Update(221, -33.2, 34, 0)
            colorRangeRed3.PlaneRange1.Update(74, -3.8, 23.8, 0)
            colorRangeRed3.PlaneRange2.Update(26, -4.8, 28.7, 0)
            colorRangeRed3.Selected = False
            mTool.RunParams.ColorRangeCollection.Add(colorRangeRed3)


            mColorRangesInitialized = True

        Catch cogexFile As CogFileOpenException
            DisplayErrorAndExit("Encountered the following Cognex File Open specific error: " & cogexFile.Message)
        Catch cogex As CogException

            DisplayErrorAndExit("Encountered the following Cognex specific error: " & cogex.Message)
        Catch ex As Exception

            DisplayErrorAndExit("Encountered the following error: " & ex.Message)
        End Try

    End Sub

    Private Sub ButtonRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRun.Click

        mTool.InputImage = CogDisplayInputImage.Image        
        mTool.Run()
        If (mTool.RunStatus.Result.Equals(CogToolResultConstants.Accept)) Then
            CogDisplaySegmentImage.Image = mTool.Result
        Else
            CogDisplaySegmentImage.Image = Nothing
        End If
 
    End Sub

    
    Private Sub CheckBoxYellow_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxYellow.CheckedChanged
        If (mColorRangesInitialized) Then
            mTool.RunParams.ColorRangeCollection.Item(0).Selected = CheckBoxYellow.Checked
            mTool.RunParams.ColorRangeCollection.Item(1).Selected = CheckBoxYellow.Checked
        End If

    End Sub

    Private Sub CheckBoxRed_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxRed.CheckedChanged
        If (mColorRangesInitialized) Then
            mTool.RunParams.ColorRangeCollection.Item(2).Selected = CheckBoxRed.Checked
            mTool.RunParams.ColorRangeCollection.Item(3).Selected = CheckBoxRed.Checked
            mTool.RunParams.ColorRangeCollection.Item(4).Selected = CheckBoxRed.Checked

        End If

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        mColorRangesInitialized = False
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

#Region "Module Level Helper Routine"

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Helper function.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Displays an error message and then exits the program.
    ' Call this when an unrecoverable error has occurred.
    Private Sub DisplayErrorAndExit(ByVal ErrorMsg As String)
        MessageBox.Show(ErrorMsg & Environment.NewLine & "Press OK to exit.")
        Me.Close()
        End      ' quit if it called from Form_Load
    End Sub
#End Region

  
End Class
