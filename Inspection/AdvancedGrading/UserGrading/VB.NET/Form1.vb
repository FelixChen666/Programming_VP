'*******************************************************************************
' Copyright (C) 2011 Cognex Corporation
'
' Subject to Cognex Corporation's terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
' *******************************************************************************
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.
'
' This sample demonstrates how to use the enhanced verification API to add any arbitrary
' property to your records in a database and have it verified with the built-in Toolblock
' verification.
'
' Rational:
' In some vision applications, it is required to expose certain configuration parameters 
' to the end users.  After an end user changes one of these exposed parameters, the end 
' user does not know if they have negatively impacted the vision application.  Verification 
' allows the end user to run their CogToolBlock against an input database in order to 
' confirm that the configuration changes did not break the vision application.
'
' *******************************************************************************

Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.IO

Imports Cognex.VisionPro
Imports Cognex.VisionPro.Database
Imports Cognex.VisionPro.Inspection


Partial Public Class UserGradingForm
    Inherits Form

    ' Create variable for input database and the current record
    Private mInputDatabase As CogVerificationDatabase = Nothing
    Private mCurrentRecord As CogVerificationData = Nothing

    Private Sub importImages()
        ' Import images from SampleImages directory
        Dim rawPath As String = "%VPRO_ROOT%\samples\Programming\Inspection\AdvancedGrading\SampleImages"
        Dim expandedPath As String = System.Environment.ExpandEnvironmentVariables(rawPath)

        Dim di As New DirectoryInfo(expandedPath)

        ' Get only .bmp files
        Dim rgFiles As FileInfo() = di.GetFiles("*.bmp")

        If mInputDatabase IsNot Nothing AndAlso mInputDatabase.Database.Connected Then
            For Each fi As FileInfo In rgFiles
                'Add image to the database
                mInputDatabase.AddImage(fi.FullName, True, CogVerificationSimpleResultConstants.Accept, "Excellent")
            Next
        End If

        ' Update grades
        Dim oldgrades As String() = mInputDatabase.GetGrades()
        For Each oldgrade As String In oldgrades
            mInputDatabase.RemoveGrade(oldgrade)
        Next

        mInputDatabase.AddGrade("Poor")
        mInputDatabase.AddGrade("Good")
        mInputDatabase.AddGrade("Excellent")

        ' Fill up combobox with the grades
        cboGrades.Items.Clear()
        For Each grade As String In mInputDatabase.GetGrades()
            cboGrades.Items.Add(grade)
        Next

        ' Set "Excellent" as the default grade
        cboGrades.SelectedItem = "Excellent"
    End Sub


    Private Sub UserGradingForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Load database and connect
        Dim rawPath As String = "%VPRO_ROOT%\samples\Programming\Inspection\AdvancedGrading\SampleDatabase"
        Dim expandedPath As String = System.Environment.ExpandEnvironmentVariables(rawPath)
        mInputDatabase = New CogVerificationDatabase(New CogDatabaseDirectory(expandedPath))
        mInputDatabase.Connect()

        ' If the database is empty, import the images
        If mInputDatabase.Database.GetCount() < 1 Then
            importImages()
        End If

        ' Fill grades
        cboGrades.Items.Clear()
        For Each grade As String In mInputDatabase.GetGrades()
            cboGrades.Items.Add(grade)
        Next

        ' Set the scrollbar
        scrlImages.Maximum = mInputDatabase.Database.GetCount()
        scrlImages.Enabled = True
        scrlImages_ValueChanged(sender, e)

        ' Set other controls
        mInputDatabaseLabel.Text = "Connected!"
        btnClear.Enabled = True
        btnMeasure.Enabled = True
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        If MessageBox.Show("Restore all records in database?", "Confrimation", MessageBoxButtons.YesNoCancel) = DialogResult.Yes Then
            
			' Clear database
			Try
				mInputDatabase.Clear()
			Catch ex As InvalidOperationException
				MessageBox.Show(ex.Message)
				Return
			End Try

            ' Clear database
            mInputDatabase.Clear()

            ' Import images
            importImages()

            ' Scroll to the first image
            scrlImages.Value = 1
            scrlImages_ValueChanged(sender, e)
        End If
    End Sub

    Private Sub btnMeasure_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMeasure.Click
        ' Measure the values with blob tool

        Me.Cursor = Cursors.WaitCursor

        ' Create a default constructed blob tool
        Dim blobTool As New Cognex.VisionPro.Blob.CogBlobTool()

        ' Set blob tool's input image
        blobTool.InputImage = TryCast(mCurrentRecord.Params.Inputs.InputImage.Value, Cognex.VisionPro.CogImage8Grey)

        ' Run the blob tool
        blobTool.Run()
		
		' If blob tool has no results
		If blobTool.Results Is Nothing Then
			Me.Cursor = Cursors.[Default]
			MessageBox.Show("Error ruining blob tool!")
			Return
		End If

        ' Get the measured values and set ranges
        nudArea.Value = Convert.ToDecimal(blobTool.Results.GetBlobs()(0).Area)
        nudRArea.Value = 0.05D
        nudCOMX.Value = Convert.ToDecimal(blobTool.Results.GetBlobs()(0).CenterOfMassX)
        nudRCOMX.Value = 0.05D
        nudCOMY.Value = Convert.ToDecimal(blobTool.Results.GetBlobs()(0).CenterOfMassY)
        nudRCOMY.Value = 0.05D

        ' Calculate the grade
        If blobTool.Results.GetBlobs()(0).Area < 2000 Then
            cboGrades.SelectedItem = "Poor"
        ElseIf (blobTool.Results.GetBlobs()(0).Area >= 2000) AndAlso (blobTool.Results.GetBlobs()(0).Area < 13000) Then
            cboGrades.SelectedItem = "Good"
        Else
            cboGrades.SelectedItem = "Excellent"
        End If

        Me.Cursor = Cursors.[Default]
    End Sub

    Private Sub scrlImages_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles scrlImages.ValueChanged
        ' Set the current record
        mCurrentRecord = mInputDatabase.Fetch(scrlImages.Value - 1)

        ' Create additional fields if needed
        If Not mCurrentRecord.Params.ExpectedOutputs.Contains("Area") Then
            mCurrentRecord.Params.ExpectedOutputs.Add("Area", CDbl(0), CDbl(0))
        End If

        If Not mCurrentRecord.Params.ExpectedOutputs.Contains("CenterOfMassX") Then
            mCurrentRecord.Params.ExpectedOutputs.Add("CenterOfMassX", CDbl(0), CDbl(0))
        End If

        If Not mCurrentRecord.Params.ExpectedOutputs.Contains("CenterOfMassY") Then
            mCurrentRecord.Params.ExpectedOutputs.Add("CenterOfMassY", CDbl(0), CDbl(0))
        End If
       
		' Update the record in the database
		Try
			mInputDatabase.Replace(mCurrentRecord)
		Catch ex As InvalidOperationException
			MessageBox.Show(ex.Message)
			Return
		End Try

        ' Update values
        nudArea.Value = Convert.ToDecimal(mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords("Area").Content)
        nudRArea.Value = Convert.ToDecimal(mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords("Area").SubRecords(CogFieldNameConstants.Range).Content)
        nudCOMX.Value = Convert.ToDecimal(mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords("CenterOfMassX").Content)
        nudRCOMX.Value = Convert.ToDecimal(mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords("CenterOfMassX").SubRecords(CogFieldNameConstants.Range).Content)
        nudCOMY.Value = Convert.ToDecimal(mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords("CenterOfMassY").Content)
        nudRCOMY.Value = Convert.ToDecimal(mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords("CenterOfMassY").SubRecords(CogFieldNameConstants.Range).Content)
        cboGrades.SelectedItem = mCurrentRecord.Params.ExpectedOutputs.Grade.Value

        ' Display the contained image
        cogDisplay1.Image = mCurrentRecord.Params.Inputs.InputImage.Value
        cogDisplay1.Fit(True)

        ' Display record key
        lblRecordName.Text = mCurrentRecord.Record.RecordKey
    End Sub


    Private Sub UserGradingForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        ' Be sure to disconnect.

        If mInputDatabase IsNot Nothing AndAlso mInputDatabase.Database.Connected Then
            mInputDatabase.Disconnect()
        End If
    End Sub

    Private Sub nudArea_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudArea.ValueChanged
        mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords("Area").Content = CDbl(nudArea.Value)

        ' Update the record in the database
        mInputDatabase.Replace(mCurrentRecord)
    End Sub

    Private Sub nudRArea_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudRArea.ValueChanged
        mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords("Area").SubRecords(CogFieldNameConstants.Range).Content = CDbl(nudRArea.Value)

        ' Update the record in the database
        mInputDatabase.Replace(mCurrentRecord)
    End Sub

    Private Sub nudCOMX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCOMX.ValueChanged
        mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords("CenterOfMassX").Content = CDbl(nudCOMX.Value)

        ' Update the record in the database
        mInputDatabase.Replace(mCurrentRecord)
    End Sub

    Private Sub nudRCOMX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudRCOMX.ValueChanged
        mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords("CenterOfMassX").SubRecords(CogFieldNameConstants.Range).Content = CDbl(nudRCOMX.Value)

        ' Update the record in the database
        mInputDatabase.Replace(mCurrentRecord)
    End Sub

    Private Sub nudCOMY_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCOMY.ValueChanged
        mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords("CenterOfMassY").Content = CDbl(nudCOMY.Value)

        ' Update the record in the database
        mInputDatabase.Replace(mCurrentRecord)
    End Sub

    Private Sub nudRCOMY_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudRCOMY.ValueChanged
        mCurrentRecord.Params.ExpectedOutputs.Record.SubRecords("CenterOfMassY").SubRecords(CogFieldNameConstants.Range).Content = CDbl(nudRCOMY.Value)

        ' Update the record in the database
        mInputDatabase.Replace(mCurrentRecord)
    End Sub

    Private Sub cboGrades_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboGrades.SelectedIndexChanged
        If mCurrentRecord IsNot Nothing Then
            mCurrentRecord.Params.ExpectedOutputs.Grade.Value = cboGrades.SelectedItem.ToString()

            ' Update the record in the database
            mInputDatabase.Replace(mCurrentRecord)
        End If
    End Sub
End Class


