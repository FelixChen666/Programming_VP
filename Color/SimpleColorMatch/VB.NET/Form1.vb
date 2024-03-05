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
' The following steps show how to match colors from images.
' Step 1) Load an image file.
' Step 2) Create a CogColorColorMatchTool.
' Step 3) Add colors to the Tool.
' Step 4) Run the tool for an input image and a region
' Step 5) Get and display the match scores.
'         
Option Explicit On
Imports Cognex.VisionPro            ' need to access basic VisionPro functionality
Imports Cognex.VisionPro.ColorMatch 'need to access CogColorMatchTool
Imports Cognex.VisionPro.ImageFile  'need to access ImageFileTool
Imports Cognex.VisionPro.Exceptions
Public Class FormColorMatch
    Private mTool As CogColorMatchTool
    Private mImageFileTool As CogImageFileTool
    Private mRegions As New Microsoft.VisualBasic.Collection()
    Private mIndex As Integer

    Private Sub FormColorMatch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            ' Create the CogColorMatchTool and CogImageFileTool
            mTool = New CogColorMatchTool
            mImageFileTool = New CogImageFileTool

            ' Get VPRO_ROOT from environment which is needed to locate the image file.
            Const ImageFileName As String = "/Images/JellOBoxes.tif"
            Dim strBaseDir As String
            strBaseDir = Environment.GetEnvironmentVariable("VPRO_ROOT")
            If strBaseDir = "" Then
                DisplayErrorAndExit("Required environment variable VPRO_ROOT not set.")
            End If

            ' Step 1 - Load images and create colors and regions.
            ' Create the image file tool to open a file of color image.
            mImageFileTool.[Operator].Open(strBaseDir & ImageFileName, CogImageFileModeConstants.Read)
            mImageFileTool.Run()
            ' Display the first image.
            Dim i As Integer
            CogDisplayInputImage.Image = mImageFileTool.OutputImage
            CogDisplayInputImage.Fit()

            'Set the color collection.
            Dim ColorLemon As CogSimpleColorItem
            ColorLemon = New CogSimpleColorItem(CogImageColorSpaceConstants.RGB, 134, 98, 20)

            Dim ColorOrange As CogSimpleColorItem
            ColorOrange = New CogSimpleColorItem(CogImageColorSpaceConstants.RGB, 128, 43, 10)

            Dim ColorLime As CogSimpleColorItem
            ColorLime = New CogSimpleColorItem(CogImageColorSpaceConstants.RGB, 35, 52, 14)

            Dim ColorGrape As CogSimpleColorItem
            ColorGrape = New CogSimpleColorItem(CogImageColorSpaceConstants.RGB, 29, 14, 17)

            Dim ColorBlackCherry As CogSimpleColorItem
            ColorBlackCherry = New CogSimpleColorItem(CogImageColorSpaceConstants.RGB, 45, 13, 9)

            mTool.RunParams.ColorCollection.Add(ColorLemon)
            mTool.RunParams.ColorCollection.Add(ColorOrange)
            mTool.RunParams.ColorCollection.Add(ColorLime)
            mTool.RunParams.ColorCollection.Add(ColorGrape)
            mTool.RunParams.ColorCollection.Add(ColorBlackCherry)


            'Create the regions of interest in the images ( one ROI in each image).
            Dim Region As CogRectangle            
            Dim RegionX() As Double = {243.6, 380.9, 285.7, 336.6, 398.6, 383.1, 383.1, 367.6, 540.4, 378.7, 270.2, 256.9, 341.1, 385.4, 352.2, 584.7, 330.0}
            Dim RegionY() As Double = {405.3, 387.5, 369.8, 369.8, 358.7, 389.8, 347.7, 323.3, 112.9, 347.7, 287.9, 361.0, 389.8, 299.0, 46.5, 148.4, 338.9}
            Dim RegionW() As Double = {57.6, 42.1, 39.8, 39.8, 39.8, 39.8, 39.8, 39.8, 39.8, 39.8, 39.8, 39.8, 39.8, 39.8, 39.8, 15.0, 53.1}
            Dim RegionH() As Double = {24.4, 15.5, 19.9, 19.9, 19.9, 19.9, 19.9, 19.9, 19.9, 19.9, 19.9, 19.9, 19.9, 19.9, 19.9, 57.5, 24.3}

            For i = 0 To RegionX.Length - 1
                Region = New CogRectangle()
                Region.Color = CogColorConstants.Red

                Region.X = RegionX(i)
                Region.Y = RegionY(i)
                Region.Width = RegionW(i)
                Region.Height = RegionH(i)
                mRegions.Add(Region)
            Next i

            'Dispplay the colors in the color collection.
            TextBoxColor1.BackColor = mTool.RunParams.ColorCollection.Item(0).SystemColorValue
            TextBoxColor2.BackColor = mTool.RunParams.ColorCollection.Item(1).SystemColorValue
            TextBoxColor3.BackColor = mTool.RunParams.ColorCollection.Item(2).SystemColorValue
            TextBoxColor4.BackColor = mTool.RunParams.ColorCollection.Item(3).SystemColorValue
            TextBoxColor5.BackColor = mTool.RunParams.ColorCollection.Item(4).SystemColorValue

            mTool.RunParams.SortResultSetByScores = False

            'Initialize the region index.
            mIndex = 1

        Catch cogexFile As CogFileOpenException
            DisplayErrorAndExit("Encountered the following Cognex File Open specific error: " & cogexFile.Message)
        Catch cogex As CogException

            DisplayErrorAndExit("Encountered the following Cognex specific error: " & cogex.Message)
        Catch ex As Exception

            DisplayErrorAndExit("Encountered the following error: " & ex.Message)
        End Try

    End Sub

    ' Run the color match tool and update the scores.
    Private Sub RunColorMatch()
        mTool.Run()
        'Display the match scores.
        If (mTool.Result IsNot Nothing) Then

            TextBoxScore1.Text = mTool.Result.Item(0).MatchScore().ToString
            TextBoxScore2.Text = mTool.Result.Item(1).MatchScore().ToString
            TextBoxScore3.Text = mTool.Result.Item(2).MatchScore().ToString
            TextBoxScore4.Text = mTool.Result.Item(3).MatchScore().ToString
            TextBoxScore5.Text = mTool.Result.Item(4).MatchScore().ToString
        End If

    End Sub


    Private Sub ButtonRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRun.Click
        'Set the input image and region for the color match tool        
        CogDisplayInputImage.Image = mImageFileTool.OutputImage
        CogDisplayInputImage.Fit()

        mTool.InputImage = CogDisplayInputImage.Image
        mTool.Region = mRegions.Item(mIndex)

        'Display the region graphic on the input image
        CogDisplayInputImage.StaticGraphics.Clear()
        CogDisplayInputImage.StaticGraphics.Add(mTool.Region, "region")

        RunColorMatch()

        'Choose next region index.
        mIndex = mIndex + 1
        If (mIndex > mRegions.Count) Then
            mIndex = 1
        End If

        'Prepare the next image
        mImageFileTool.Run()

    End Sub

    Private Sub CheckBoxHueOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxHueOnly.CheckedChanged
        'Use only the hue component for color match in HSI color space
        If (CheckBoxHueOnly.Checked) Then
            mTool.RunParams.Weight0 = 1.0
            mTool.RunParams.Weight1 = 0.0
            mTool.RunParams.Weight2 = 0.0
            mTool.RunParams.MatchScoreMetricType = CogColorMatchScoreMetricTypeConstants.WeightedEuclideanDistanceInHSIColorSpace

        Else
            'Use all R, G and B components for color match in RGB color space
            mTool.RunParams.Weight0 = 1.0
            mTool.RunParams.Weight1 = 1.0
            mTool.RunParams.Weight2 = 1.0
            mTool.RunParams.MatchScoreMetricType = CogColorMatchScoreMetricTypeConstants.WeightedEuclideanDistanceInRGBColorSpace

        End If

        'Rerun the tool for the current image/region and updates the scores.
        RunColorMatch()
        
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
