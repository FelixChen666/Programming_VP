'*******************************************************************************
' Copyright (C) 2010 Cognex Corporation
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
' This sample demonstrates how to use the CogVerifierBasic to verify a CogToolBlock
' against a database.  This sample shows how to expose this functionality in a
' WinForms application.  
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

Imports Cognex.VisionPro
Imports Cognex.VisionPro.Database
Imports Cognex.VisionPro.Inspection
Imports Cognex.VisionPro.ToolBlock


Partial Public Class UserVerificationForm
    Inherits Form

    Private mInputDatabase As CogVerificationDatabase = Nothing
    Private mOutputDatabase As CogVerificationDatabase = Nothing
    Private mToolBlock As CogToolBlock = Nothing
    Private mVerifier As CogVerifierBasic = Nothing

    Private Sub UserVerificationForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Connect to the sample input database
        Dim rawPath As String = "%VPRO_ROOT%\samples\Programming\Inspection\Verification\SampleDatabase"
        Dim expandedPath As String = System.Environment.ExpandEnvironmentVariables(rawPath)
        mInputDatabase = New CogVerificationDatabase(New CogDatabaseDirectory(expandedPath))
        mInputDatabase.Connect()
        mInputDatabaseLabel.Text = mInputDatabase.Database.Name

        ' Create a output database and connect to it.  The output database is where
        ' the results of verification will be stored.
        Dim randomFilename As String = System.IO.Path.GetRandomFileName()
        Dim outputDatabasePath As String = System.IO.Path.Combine(System.IO.Path.GetTempPath(), randomFilename)
        System.IO.Directory.CreateDirectory(outputDatabasePath)
        mOutputDatabase = New CogVerificationDatabase(New CogDatabaseDirectory(outputDatabasePath))
        mOutputDatabase.Connect()
        mOutputDatabaseLabel.Text = mOutputDatabase.Database.Name

        ' Load up the sample CogToolBlock that needs to be verified.  Note, this 
        ' CogToolBlock has an InputImage and BlobThreshold input terminals.  The 
        ' InputImage terminal is required for verification.
        rawPath = "%VPRO_ROOT%\samples\Programming\Inspection\Verification\UserVerification\CogToolBlock.vpp"
        expandedPath = System.Environment.ExpandEnvironmentVariables(rawPath)
        Try
            mToolBlock = TryCast(CogSerializer.LoadObjectFromFile(expandedPath), CogToolBlock)
        Catch ex As Exception
            MessageBox.Show("Cannot load CogToolBlock: " + expandedPath, "Error")
            Application.Exit()
        End Try

        mToolBlockLabel.Text = expandedPath

        ' Create the verfier and connect to various interesting event handlers.
        mVerifier = New CogVerifierBasic(mToolBlock, mInputDatabase, mOutputDatabase)
        mVerifier.UnknownResultBehavior = CogVerificationUnknownResultBehaviorConstants.AlwaysMatch
        AddHandler mVerifier.RunStarted, AddressOf mVerifier_RunStarted
        AddHandler mVerifier.RunCompleted, AddressOf mVerifier_RunCompleted
        AddHandler mVerifier.RunProgressChanged, AddressOf mVerifier_RunProgressChanged

        mBlobThresholdNumericUpDown.Value = 170
    End Sub

    Private Delegate Sub verfier_RunStartedDelegate(ByVal sender As Object, ByVal e As EventArgs)
    Private Sub mVerifier_RunStarted(ByVal sender As Object, ByVal e As EventArgs)

        ' The RunStarted event fires when the verfier first starts running.

        ' Make sure to invoke to the GUI thread since all verfier events are fired from a
        ' non-GUI thread.
        If mVerificationResultLabel.InvokeRequired Then
            mVerificationResultLabel.Invoke(New verfier_RunStartedDelegate(AddressOf mVerifier_RunStarted), New Object() {sender, e})
            Return
        End If

        mTotalLabel.Text = "Total: 0"
        mMatchedLabel.Text = "Match: 0"
        mMismatchedLabel.Text = "Mismatch: 0"

        mVerifyButton.Enabled = False
    End Sub

    Private Delegate Sub verfier_RunProgressChangedDelegate(ByVal sender As Object, ByVal e As CogVerifierRunProgressChangedEventArgs)
    Private Sub mVerifier_RunProgressChanged(ByVal sender As Object, ByVal e As CogVerifierRunProgressChangedEventArgs)

        ' The RunProgressChanged event fires when the verfier has finished verifying one item from
        ' the input database.

        ' Make sure to invoke to the GUI thread since all verfier events are fired from a
        ' non-GUI thread.
        If mVerificationResultLabel.InvokeRequired Then
            mVerificationResultLabel.Invoke(New verfier_RunProgressChangedDelegate(AddressOf mVerifier_RunProgressChanged), New Object() {sender, e})
            Return
        End If

        mVerificationResultLabel.ForeColor = System.Drawing.Color.Black
        mVerificationResultLabel.BackColor = System.Drawing.SystemColors.Control
        mVerificationResultLabel.Text = "Verifying... " & Convert.ToString(e.ProgressPercentage) & "% complete"
    End Sub

    Private Delegate Sub verfier_RunCompletedDelegate(ByVal sender As Object, ByVal e As CogVerifierRunCompletedEventArgs)
    Private Sub mVerifier_RunCompleted(ByVal sender As Object, ByVal e As CogVerifierRunCompletedEventArgs)

        ' The RunCompleted event fires when the verfier has finished verifying all items in the
        ' input database.

        ' Make sure to invoke to the GUI thread since all verfier events are fired from a
        ' non-GUI thread.
        If mVerificationResultLabel.InvokeRequired Then
            mVerificationResultLabel.Invoke(New verfier_RunCompletedDelegate(AddressOf mVerifier_RunCompleted), New Object() {sender, e})
            Return
        End If

        If e.[Error] IsNot Nothing Then
            MessageBox.Show(e.[Error].Message, "Error from Verifier")
        End If

        ' Go through the output database and figure out the total verification result.
        Dim failureCount As Integer = 0
        For Each vdata As CogVerificationData In mOutputDatabase
            If vdata.Results.OverallVerificationResult.Value = False Then
                failureCount += 1
            End If
        Next

        If failureCount > 0 Then
            mVerificationResultLabel.Text = "Verification FAILED"
            mVerificationResultLabel.ForeColor = System.Drawing.Color.White
            mVerificationResultLabel.BackColor = System.Drawing.Color.Red
        Else
            mVerificationResultLabel.Text = "Verification Passed"
            mVerificationResultLabel.ForeColor = System.Drawing.Color.White
            mVerificationResultLabel.BackColor = System.Drawing.Color.Green
        End If


        mTotalLabel.Text = "Total: " & mVerifier.Statistics.TotalCount.ToString()
        mMatchedLabel.Text = "Match: " & mVerifier.Statistics.MatchesCount.ToString()
        mMismatchedLabel.Text = "Mismatch: " & mVerifier.Statistics.MismatchesCount.ToString()

        mVerifyButton.Enabled = True
    End Sub

    Private Sub UserVerificationForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)

        ' Be sure to cleanup the input and output databases before exiting.

        If mInputDatabase IsNot Nothing AndAlso mInputDatabase.Database.Connected Then
            mInputDatabase.Disconnect()
        End If

        If mOutputDatabase IsNot Nothing AndAlso mOutputDatabase.Database.Connected Then
            mOutputDatabase.Disconnect()
            System.IO.Directory.Delete(mOutputDatabase.Database.Name, True)
        End If
    End Sub

    Private Sub mVerifyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mVerifyButton.Click

        ' Run the verfier asynchronously.  This call will return immediately and the verification will
        ' occur in a worker thread.  Do not call the synchronous Run() method in a GUI callback since the
        ' verification may take a considerable amount of time and during this time all GUI events will be 
        ' blocked.

        mVerifier.RunAsync()

    End Sub

    Private Sub mNumBlobsNumericUpDown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mBlobThresholdNumericUpDown.ValueChanged
        mToolBlock.Inputs("BlobThreshold").Value = mBlobThresholdNumericUpDown.Value
    End Sub

End Class


