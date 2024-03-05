'*******************************************************************************
' Copyright (C) 2010 Cognex Corporation
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
'
' This program assumes that you have good knowledge of VB.NET and VisionPro
' programming.
'
' This program demonstrates how to interact with some of the CogToolBlock APIs

' 1) The sample loads a ToolBlock from a vpp file 
' 2) The user can modify the value of the Toolblock input terminals through the 
'    numeric up down controls on the application form.
' 3) The user can also select an image from coins.idb or from an acquisition fifo.
' 4) The run once button does the following
'    - Acquire the next image or read the next image
'    - Pass the image to the ToolBlock input image 
'    - Run the toolblock once
' 5) The sample also demonstrate how to read the output terminal value to updated 
'    the application labels with the inspection result
' 6) The user can change the code to create an acquisition fifo that would work 
'    specifically with the camera available
' 7) The top level script is a C# simple script. It runs the tools. 
' 8) The TBInspectionTest ToolBlock is used as result analysis tool to decide 
'    if the inspection passed or failed and sets the value of the output terminal
' 9) The sample will allow the user to run the toolblock from the menu buttons but 
'    the toolblock will run against the same image.
' 10)The sample also take advantage of the ran event so update the display with 
'    the results from blob tool.


Imports Cognex.VisionPro
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.ToolBlock
Imports Cognex.VisionPro.Blob

Public Class Form1
  Private mIFTool As CogImageFileTool
  Private mAcqTool As CogAcqFifoTool
  Private numPass As Long
  Private numFail As Long

    ' Open the image file
    ' Create the acq fifo tool
    ' Set the exposure
    ' The load is used to load the TB.vpp file
    ' Hook the event for ran() and SubjectChanged
    ' Since the ToolBlockEditV2 allows you to reset or load a different vpp file
    ' We need to block the Run Once button
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Cognex.Vision.Startup.Initialize(Cognex.Vision.Startup.ProductKey.VProX)
        CogToolBlockEditV21.LocalDisplayVisible = False
        mIFTool = New CogImageFileTool()
        mIFTool.Operator.Open(Environment.GetEnvironmentVariable("VPRO_ROOT") + "\images\coins.idb", CogImageFileModeConstants.Read)
        mAcqTool = New CogAcqFifoTool()
        ' If no camera is attached, disable the radio button
        If (mAcqTool.Operator Is Nothing) Then
            radCamera.Enabled = False
        Else
            mAcqTool.Operator.OwnedExposureParams.Exposure = 10
        End If

        CogToolBlockEditV21.Subject = CogSerializer.LoadObjectFromFile(Environment.GetEnvironmentVariable("VPRO_ROOT") + "\samples\programming\toolblock\toolblockload\tb.vpp")
        AddHandler CogToolBlockEditV21.Subject.Ran, AddressOf Subject_Ran
        CogToolBlockEditV21.Subject.Inputs.Item("FilterLowValue").Value = nAreaLow.Value
        CogToolBlockEditV21.Subject.Inputs.Item("FilterHighValue").Value = nAreaHigh.Value
        AddHandler CogToolBlockEditV21.SubjectChanged, AddressOf CogToolBlockEditV21_SubjectChanged

    End Sub
    'Form overrides dispose to clean up the component list.
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    'Releasing framegrabbers
    If (disposing) Then
      Dim frameGrabbers As New CogFrameGrabbers()
      For i As Integer = 0 To frameGrabbers.Count - 1
        frameGrabbers(i).Disconnect(False)
      Next
    End If

    ' Disconnect the event handlers before closing the form
    RemoveHandler CogToolBlockEditV21.SubjectChanged, AddressOf CogToolBlockEditV21_SubjectChanged
    RemoveHandler CogToolBlockEditV21.Subject.Ran, AddressOf Subject_Ran
    Cognex.Vision.Startup.Shutdown()
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try

  End Sub


  Private Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
    ' Get the next image
    If (radImageFile.Checked = True) Then
      mIFTool.Run()
      CogToolBlockEditV21.Subject.Inputs.Item("Image").Value = mIFTool.OutputImage
    Else
      mAcqTool.Run()
      CogToolBlockEditV21.Subject.Inputs.Item("Image").Value = mAcqTool.OutputImage
    End If
    ' Run the toolblock
    CogToolBlockEditV21.Subject.Run()

  End Sub

  Private Sub nAreaHigh_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles nAreaHigh.ValueChanged
    ' It seems VB.NET code initialization gets in this code, so we need to condition the code
    ' To make sure the application form is up
    If (Me.Visible = True) Then
      ' Update the input terminal value whenever the user changes the value through the GUI
      CogToolBlockEditV21.Subject.Inputs.Item("FilterHighValue").Value = nAreaHigh.Value
    End If

  End Sub

  Private Sub nAreaLow_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles nAreaLow.ValueChanged
    ' It seems VB.NET code initialization gets in this code, so we need to condition the code
    ' To make sure the application form is up
    If (Me.Visible = True) Then
      ' Update the input terminal value whenever the user changes the value through the GUI
      CogToolBlockEditV21.Subject.Inputs.Item("FilterLowValue").Value = nAreaLow.Value
    End If
  End Sub

  Private Sub CogToolBlockEditV21_SubjectChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CogToolBlockEditV21.SubjectChanged
    If (Me.Visible = True) Then
      ' The application is meant to be used with the TB.vpp so whenever the user changes the TB
      ' we disable the run once button
      btnRun.Enabled = False
    End If
  End Sub
  Private Sub Subject_Ran(ByVal sender As Object, ByVal e As System.EventArgs)
    ' This method executes each time the TB runs
    If (CogToolBlockEditV21.Subject.Outputs.Item("InspectionPassed").Value = True) Then
      numPass += 1
    Else
      numFail += 1
    End If
    ' Update the label with pass and fail
    nPass.Text = numPass.ToString()
    nFail.Text = numFail.ToString()

    ' Update the CogDisplayRecord with the image and the lastRunRecord
    CogRecordDisplay1.Image = CogToolBlockEditV21.Subject.Inputs.Item("Image").Value
    Dim mBlobTool As CogBlobTool = CogToolBlockEditV21.Subject.Tools.Item("CogBlobTool1")
    CogRecordDisplay1.Record = mBlobTool.CreateLastRunRecord()
    CogRecordDisplay1.Fit(True)

  End Sub
End Class
