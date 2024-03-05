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
' The following steps show how to match composite colors from images.
' Step 1) Load a color image file.
' Step 2) Create a CogCompositeColorColorMatchTool.
' Step 3) Add composite colors to the Tool.
' Step 4) Run the tool for an input image and a region.
' Step 5) Get and display the match scores.
'         
Option Explicit On
Imports Cognex.VisionPro            ' need to access basic VisionPro functionality
Imports Cognex.VisionPro.CompositeColorMatch 'need to access CogCompositeColorMatchTool
Imports Cognex.VisionPro.ImageFile  'need to access ImageFileTool
Imports Cognex.VisionPro.Exceptions
Public Class FormCompositeColorMatch
    Private mTool As CogCompositeColorMatchTool
    Private mImageFileTool As CogImageFileTool
    Private mRegions As New Microsoft.VisualBasic.Collection()
    Private mIndex As Integer



    Private Sub FormColorMatch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            ' Create the CogCompositeColorMatchTool and CogImageFileTool.
            mTool = New CogCompositeColorMatchTool
            mImageFileTool = New CogImageFileTool

            ' Get VPRO_ROOT from environment which is needed to locate the color image file.
            Const ImageFileName As String = "/Images/Lt_Urban.tif"
            Dim strBaseDir As String
            strBaseDir = Environment.GetEnvironmentVariable("VPRO_ROOT")
            If strBaseDir = "" Then
                DisplayErrorAndExit("Required environment variable VPRO_ROOT not set.")
            End If

            ' Step 1 - Load  images and create colors from image regions.
            ' Create the image file tool to open a file of color images.
            mImageFileTool.[Operator].Open(strBaseDir & ImageFileName, CogImageFileModeConstants.Read)

            'Set the color collection and region collection.
            Dim Colors(7) As CogCompositeColorItem

            Dim RegionColor As CogColorConstants
            Dim Region1 As CogRectangle

            RegionColor = CogColorConstants.Red


            Region1 = New CogRectangle()
            Region1.Color = RegionColor
            Region1.X = 95
            Region1.Y = 100
            Region1.Width = 438
            Region1.Height = 270

            Dim i As Integer
            For i = 0 To Colors.Length - 1
                mImageFileTool.Run()
                Colors(i) = New CogCompositeColorItem(mImageFileTool.OutputImage, Region1)
                mTool.Pattern.CompositeColorCollection.Add(Colors(i))
                mRegions.Add(Region1)

            Next i

            mImageFileTool.Run()
            CogDisplayInputImage.Image = mImageFileTool.OutputImage
            CogDisplayInputImage.Fit()


            'Display the composite colors.
            CogDisplayColor1.Image = Colors(0).Image
            CogDisplayColor1.Fit()
            CogDisplayColor2.Image = Colors(1).Image
            CogDisplayColor2.Fit()
            CogDisplayColor3.Image = Colors(2).Image
            CogDisplayColor3.Fit()
            CogDisplayColor4.Image = Colors(3).Image
            CogDisplayColor4.Fit()
            CogDisplayColor5.Image = Colors(4).Image
            CogDisplayColor5.Fit()
            CogDisplayColor6.Image = Colors(5).Image
            CogDisplayColor6.Fit()
            CogDisplayColor7.Image = Colors(6).Image
            CogDisplayColor7.Fit()

            mTool.RunParams.SortResultSetByScores = False

            'Train the tool
            mTool.Pattern.Train()

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

    Private Sub RunCompositeColorMatch()
        mTool.Run()
        If (mTool.Result IsNot Nothing) Then
            'Display the match scores.
            TextBoxScore1.Text = CDec(mTool.Result.Item(0).MatchScore()).ToString
            TextBoxScore2.Text = CDec(mTool.Result.Item(1).MatchScore()).ToString
            TextBoxScore3.Text = CDec(mTool.Result.Item(2).MatchScore()).ToString
            TextBoxScore4.Text = CDec(mTool.Result.Item(3).MatchScore()).ToString
            TextBoxScore5.Text = CDec(mTool.Result.Item(4).MatchScore()).ToString
            TextBoxScore6.Text = CDec(mTool.Result.Item(5).MatchScore()).ToString
            TextBoxScore7.Text = CDec(mTool.Result.Item(6).MatchScore()).ToString
        End If

    End Sub

    Private Sub ButtonRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRun.Click
        'Get the next image        
        CogDisplayInputImage.Image = mImageFileTool.OutputImage
        CogDisplayInputImage.Fit()

        'Set the input image and region for the color match tool
        mTool.InputImage = CogDisplayInputImage.Image
        mTool.Region = mRegions.Item(mIndex)

        'Choose next region index for the next run
        mIndex = mIndex + 1
        If (mIndex > mRegions.Count) Then
            mIndex = 1
        End If

        'Display the region graphic on the input image
        CogDisplayInputImage.StaticGraphics.Clear()
        CogDisplayInputImage.StaticGraphics.Add(mTool.Region, "region")

        RunCompositeColorMatch()

        ' Prepare the next run image.
        mImageFileTool.Run()

    End Sub

    Private Sub CheckBoxNormalizeIntensity_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxNormalizeIntensity.CheckedChanged
        'Normalize Intensity during color match
        If (CheckBoxNormalizeIntensity.Checked) Then
            mTool.RunParams.NormalizeIntensityEnabled = True
        Else
            'No intensity normalization during color match
            mTool.RunParams.NormalizeIntensityEnabled = False
        End If

        'Rerun the tool.
        RunCompositeColorMatch()


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
