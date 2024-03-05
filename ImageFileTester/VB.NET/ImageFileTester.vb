'*******************************************************************************
' Copyright (C) 2004 Cognex Corporation
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

' This sample demonstrates how to run through an image database using a
' CogImageFileTool inside a CogToolGroup, running the inspection once for
' each image in the database.
'
' When the user presses the RunTest button, this program does the following:
'
'1. If there are no CogImageFileTools in the toolgroup, it displays a message
'   telling the user that the application requires at least one CIFT, and
'   goes no further.
'
'2. It goes through the toolgroup and resets all CogImageFileTools to the first
'   image in their image database (if they have one open).
'
'3. It goes through the toolgroup and resets the statistics on any
'   CogDataAnalysisTools it finds.
'
'4. It runs the toolgroup repeatedly until the first CogImageFileTool in
'   the tool group has reached the last image in its database.
Option Explicit On 
'Needed for VisionPro
Imports Cognex.VisionPro
' needed for image processing
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.ImageProcessing
Imports Cognex.VisionPro.ToolGroup
Imports Cognex.VisionPro.Implementation
Imports Cognex.VisionPro.Implementation.Internal
'Needed for VisionPro exceptions
Imports Cognex.VisionPro.Exceptions
Namespace ImageFileTester
 Public Class ImageFileTester
  Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()
    Cognex.Vision.Startup.Initialize(Cognex.Vision.Startup.ProductKey.VProX)
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
    Cognex.Vision.Startup.Shutdown()
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
        Friend WithEvents txtDescription As System.Windows.Forms.TextBox
        Friend WithEvents ListView1 As System.Windows.Forms.ListView
        Friend WithEvents cmdRunTest As System.Windows.Forms.Button
        Friend WithEvents Image As System.Windows.Forms.ColumnHeader
        Friend WithEvents Time As System.Windows.Forms.ColumnHeader
        Friend WithEvents Status As System.Windows.Forms.ColumnHeader
        Friend WithEvents lblAccept As System.Windows.Forms.Label
        Friend WithEvents lblWarn As System.Windows.Forms.Label
        Friend WithEvents lblReject As System.Windows.Forms.Label
        Friend WithEvents lblError As System.Windows.Forms.Label
        Friend WithEvents lblTotal As System.Windows.Forms.Label
        Friend WithEvents txtAccept As System.Windows.Forms.TextBox
        Friend WithEvents txtWarn As System.Windows.Forms.TextBox
        Friend WithEvents txtReject As System.Windows.Forms.TextBox
        Friend WithEvents txtError As System.Windows.Forms.TextBox
        Friend WithEvents CogToolGroupEdit1 As Cognex.VisionPro.ToolGroup.CogToolGroupEditV2
        Friend WithEvents txtTotal As System.Windows.Forms.TextBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ImageFileTester))
            Me.txtDescription = New System.Windows.Forms.TextBox
            Me.ListView1 = New System.Windows.Forms.ListView
            Me.Image = New System.Windows.Forms.ColumnHeader
            Me.Time = New System.Windows.Forms.ColumnHeader
            Me.Status = New System.Windows.Forms.ColumnHeader
            Me.cmdRunTest = New System.Windows.Forms.Button
            Me.lblAccept = New System.Windows.Forms.Label
            Me.lblWarn = New System.Windows.Forms.Label
            Me.lblReject = New System.Windows.Forms.Label
            Me.lblError = New System.Windows.Forms.Label
            Me.lblTotal = New System.Windows.Forms.Label
            Me.txtAccept = New System.Windows.Forms.TextBox
            Me.txtWarn = New System.Windows.Forms.TextBox
            Me.txtReject = New System.Windows.Forms.TextBox
            Me.txtError = New System.Windows.Forms.TextBox
            Me.txtTotal = New System.Windows.Forms.TextBox
            Me.CogToolGroupEdit1 = New Cognex.VisionPro.ToolGroup.CogToolGroupEditV2
            CType(Me.CogToolGroupEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'txtDescription
            '
            Me.txtDescription.Location = New System.Drawing.Point(8, 520)
            Me.txtDescription.Multiline = True
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Size = New System.Drawing.Size(600, 56)
            Me.txtDescription.TabIndex = 1
            Me.txtDescription.Text = resources.GetString("txtDescription.Text")
            '
            'ListView1
            '
            Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Image, Me.Time, Me.Status})
            Me.ListView1.GridLines = True
            Me.ListView1.Location = New System.Drawing.Point(208, 8)
            Me.ListView1.Name = "ListView1"
            Me.ListView1.Size = New System.Drawing.Size(416, 224)
            Me.ListView1.TabIndex = 2
            Me.ListView1.UseCompatibleStateImageBehavior = False
            Me.ListView1.View = System.Windows.Forms.View.Details
            '
            'Image
            '
            Me.Image.Text = "Image"
            Me.Image.Width = 115
            '
            'Time
            '
            Me.Time.Text = "Time"
            Me.Time.Width = 84
            '
            'Status
            '
            Me.Status.Text = "Status"
            Me.Status.Width = 215
            '
            'cmdRunTest
            '
            Me.cmdRunTest.Location = New System.Drawing.Point(32, 16)
            Me.cmdRunTest.Name = "cmdRunTest"
            Me.cmdRunTest.Size = New System.Drawing.Size(136, 40)
            Me.cmdRunTest.TabIndex = 3
            Me.cmdRunTest.Text = "Run Test"
            '
            'lblAccept
            '
            Me.lblAccept.Location = New System.Drawing.Point(32, 72)
            Me.lblAccept.Name = "lblAccept"
            Me.lblAccept.Size = New System.Drawing.Size(64, 16)
            Me.lblAccept.TabIndex = 5
            Me.lblAccept.Text = "Accepts:"
            '
            'lblWarn
            '
            Me.lblWarn.Location = New System.Drawing.Point(32, 104)
            Me.lblWarn.Name = "lblWarn"
            Me.lblWarn.Size = New System.Drawing.Size(64, 16)
            Me.lblWarn.TabIndex = 6
            Me.lblWarn.Text = "Warnings:"
            '
            'lblReject
            '
            Me.lblReject.Location = New System.Drawing.Point(32, 136)
            Me.lblReject.Name = "lblReject"
            Me.lblReject.Size = New System.Drawing.Size(64, 16)
            Me.lblReject.TabIndex = 7
            Me.lblReject.Text = "Rejects:"
            '
            'lblError
            '
            Me.lblError.Location = New System.Drawing.Point(32, 168)
            Me.lblError.Name = "lblError"
            Me.lblError.Size = New System.Drawing.Size(64, 13)
            Me.lblError.TabIndex = 8
            Me.lblError.Text = "Errors:"
            '
            'lblTotal
            '
            Me.lblTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTotal.Location = New System.Drawing.Point(32, 200)
            Me.lblTotal.Name = "lblTotal"
            Me.lblTotal.Size = New System.Drawing.Size(64, 16)
            Me.lblTotal.TabIndex = 9
            Me.lblTotal.Text = "Totals:"
            '
            'txtAccept
            '
            Me.txtAccept.BackColor = System.Drawing.SystemColors.Control
            Me.txtAccept.Location = New System.Drawing.Point(104, 72)
            Me.txtAccept.Name = "txtAccept"
            Me.txtAccept.Size = New System.Drawing.Size(64, 20)
            Me.txtAccept.TabIndex = 10
            Me.txtAccept.Text = "0"
            Me.txtAccept.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'txtWarn
            '
            Me.txtWarn.BackColor = System.Drawing.SystemColors.Control
            Me.txtWarn.Location = New System.Drawing.Point(104, 104)
            Me.txtWarn.Name = "txtWarn"
            Me.txtWarn.Size = New System.Drawing.Size(64, 20)
            Me.txtWarn.TabIndex = 11
            Me.txtWarn.Text = "0"
            Me.txtWarn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'txtReject
            '
            Me.txtReject.BackColor = System.Drawing.SystemColors.Control
            Me.txtReject.Location = New System.Drawing.Point(104, 136)
            Me.txtReject.Name = "txtReject"
            Me.txtReject.Size = New System.Drawing.Size(64, 20)
            Me.txtReject.TabIndex = 12
            Me.txtReject.Text = "0"
            Me.txtReject.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'txtError
            '
            Me.txtError.BackColor = System.Drawing.SystemColors.Control
            Me.txtError.Location = New System.Drawing.Point(104, 168)
            Me.txtError.Name = "txtError"
            Me.txtError.Size = New System.Drawing.Size(64, 20)
            Me.txtError.TabIndex = 13
            Me.txtError.Text = "0"
            Me.txtError.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'txtTotal
            '
            Me.txtTotal.BackColor = System.Drawing.SystemColors.Control
            Me.txtTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.txtTotal.Location = New System.Drawing.Point(104, 200)
            Me.txtTotal.Name = "txtTotal"
            Me.txtTotal.Size = New System.Drawing.Size(64, 20)
            Me.txtTotal.TabIndex = 14
            Me.txtTotal.Text = "0"
            Me.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'CogToolGroupEdit1
            '
            Me.CogToolGroupEdit1.AllowDrop = True
            Me.CogToolGroupEdit1.ContextMenuCustomizer = Nothing
            Me.CogToolGroupEdit1.Location = New System.Drawing.Point(12, 238)
            Me.CogToolGroupEdit1.MinimumSize = New System.Drawing.Size(489, 0)
            Me.CogToolGroupEdit1.Name = "CogToolGroupEdit1"
            Me.CogToolGroupEdit1.Size = New System.Drawing.Size(612, 276)
            Me.CogToolGroupEdit1.SuspendElectricRuns = False
            Me.CogToolGroupEdit1.TabIndex = 15
            '
            'ImageFileTester
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(632, 582)
            Me.Controls.Add(Me.CogToolGroupEdit1)
            Me.Controls.Add(Me.txtTotal)
            Me.Controls.Add(Me.txtError)
            Me.Controls.Add(Me.txtReject)
            Me.Controls.Add(Me.txtWarn)
            Me.Controls.Add(Me.txtAccept)
            Me.Controls.Add(Me.lblTotal)
            Me.Controls.Add(Me.lblError)
            Me.Controls.Add(Me.lblReject)
            Me.Controls.Add(Me.lblWarn)
            Me.Controls.Add(Me.lblAccept)
            Me.Controls.Add(Me.cmdRunTest)
            Me.Controls.Add(Me.ListView1)
            Me.Controls.Add(Me.txtDescription)
            Me.Name = "ImageFileTester"
            Me.Text = "Cognex VisionPro Image File Test Application"
            CType(Me.CogToolGroupEdit1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region
#Region " Private vars "
        Private Const Title As String = "Cognex VisionPro Image File Test Application"
        Private mTestInProgress As Boolean  ' True while a test is running
        Private mAccept As Long ' Number of accepts
        Private mWarn As Long ' Number of warns
        Private mReject As Long ' Number of rejects
        Private mError As Long ' Number of errors

#End Region
#Region " Private subs and functions"
        ' Update the text boxes displaying the running totals
        Private Sub UpdateTotals()
            txtAccept.Text = mAccept
            txtWarn.Text = mWarn
            txtReject.Text = mReject
            txtError.Text = mError
            txtTotal.Text = mAccept + mWarn + mReject + mError
        End Sub

#End Region
#Region " 'Run' button click handler"
        Private Sub cmdRunTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRunTest.Click

            Try
                ' While running a test, this becomes the Stop Test button
                If mTestInProgress Then
                    mTestInProgress = False ' Stops the test prematurely
                    Exit Sub
                End If

                Dim ToolGroup As CogToolGroup
                ToolGroup = CogToolGroupEdit1.Subject

                mAccept = 0
                mWarn = 0
                mReject = 0
                mError = 0
                UpdateTotals()
                ListView1.Items.Clear()
                ListView1.Activation = ItemActivation.OneClick

                ' Step over all the tools in the toolgroup (do this in reverse order so
                ' that we keep a reference to the FIRST image file tool).
                Dim Index As Integer, ImageFileTool As CogImageFileTool
                ImageFileTool = Nothing
                For Index = ToolGroup.Tools.Count - 1 To 0 Step -1
                    If TypeOf ToolGroup.Tools.Item(Index) Is CogImageFileTool Then
                        ImageFileTool = CType(ToolGroup.Tools.Item(Index), CogImageFileTool)
                        ImageFileTool.NextImageIndex = 0 ' Reset it to the first image
                    End If
                Next Index


                ' If no image file tool, exit
                If ImageFileTool Is Nothing Then
                    MsgBox("Your tool group must have at least one CogImageFileTool in order to run the test.")
                    Exit Sub
                End If

                ' If no image file open for reading, exit
                If ImageFileTool.[Operator].OpenMode <> CogImageFileModeConstants.Read Then
                    MsgBox("The first CogImageFileTool must have an image file open for reading in order to run the test.")
                    Exit Sub
                End If

                ' If no images in the file, exit
                If ImageFileTool.[Operator].Count < 1 Then
                    MsgBox("The first CogImageFileTool must have an image file containing images in order to run the test.")
                    Exit Sub
                End If

                ' If ImageIndexIncrement=0, the test would never finish so exit
                If ImageFileTool.ImageIndexIncrement = 0 Then
                    MsgBox("The first CogImageFileTool must have a nonzero ImageIndexIncrement in order to run the test.")
                    Exit Sub
                End If

                ' Reset the statistics of all CogDataAnalysisTools
                Dim Tool As Cognex.VisionPro.ICogTool, DataAnalysisTool As CogDataAnalysisTool
                For Each Tool In ToolGroup.Tools
                    If TypeOf Tool Is CogDataAnalysisTool Then
                        DataAnalysisTool = CType(Tool, CogDataAnalysisTool)
                        DataAnalysisTool.RunParams.ResetBufferedValues()
                        DataAnalysisTool.RunParams.ResetRunningStatistics()
                    End If
                Next Tool

                cmdRunTest.Text = "Stop Test"
                mTestInProgress = True

                ' Run the test over all images in the file
                Dim NumProcessed As Long
                Do
                    ' Run the toolgroup
                    Me.Text = Title & " (" & NumProcessed + 1 & " of " & _
                      ImageFileTool.[Operator].Count & ")"
                    ToolGroup.Run()
                    NumProcessed = NumProcessed + 1

                    ' Add an item to the list
                    Dim NewItem As New ListViewItem((NumProcessed - 1).ToString)
                    NewItem.UseItemStyleForSubItems = False
                    NewItem.SubItems.Add(Format(ToolGroup.RunStatus.ProcessingTime, "F6"))

                    ' Check the overall status of the toolgroup

                    If ToolGroup.RunStatus.Result = CogToolResultConstants.Error Then
                        mError = mError + 1
                        NewItem.SubItems.Add("Error")
                        NewItem.SubItems(2).ForeColor = System.Drawing.Color.Red
                        'NewItem.SubItems(2).Font.Bold = True
                    ElseIf ToolGroup.RunStatus.Result = CogToolResultConstants.Warning Then
                        mWarn = mWarn + 1
                        NewItem.SubItems.Add("Warning")
                        NewItem.SubItems(2).ForeColor = System.Drawing.Color.Blue
                    ElseIf ToolGroup.RunStatus.Result = CogToolResultConstants.Reject Then
                        mReject = mReject + 1
                        NewItem.SubItems.Add("Reject")
                        NewItem.SubItems(2).ForeColor = System.Drawing.Color.Red
                    Else
                        mAccept = mAccept + 1
                        NewItem.SubItems.Add("Accept")
                        NewItem.SubItems(2).ForeColor = System.Drawing.Color.Green
                    End If
                    ListView1.Items.Add(NewItem)
                    UpdateTotals()

                    Application.DoEvents() ' Lets controls paint themselves
                Loop While mTestInProgress And ImageFileTool.CurrentImageIndex < ImageFileTool.[Operator].Count - 1

                cmdRunTest.Text = "Run Test"
                mTestInProgress = False

                Me.Text = Title
            Catch ex As CogException
                MessageBox.Show(ex.Message)
            Catch gex As Exception
                MessageBox.Show(gex.Message)
            End Try

        End Sub

#End Region
#Region " Form's closing and close handlers"
        ' Before unloading the form, clear out the Subject of the tool group edit
        ' control so it closes any open tool-edit dialogs.
        Private Sub ImageFileTester_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
            If Not CogToolGroupEdit1 Is Nothing Then
                CogToolGroupEdit1.Subject = Nothing
            End If
        End Sub
        ' Runs when the form is about to be unloaded
        Private Sub ImageFileTester_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
            Dim response As DialogResult = MessageBox.Show("Save the tool group before exiting?", "Image File tester", MessageBoxButtons.YesNo)
            If response = Windows.Forms.DialogResult.Yes Then
                ' go back and save the tool group 

                Dim saveFileDialog As New SaveFileDialog
                saveFileDialog.Title = "Save Tool Group"
                saveFileDialog.Filter = "QuickBuild Tool File|*.vpp"
                saveFileDialog.ShowDialog()

                CogSerializer.SaveObjectToFile(CogToolGroupEdit1.Subject, saveFileDialog.FileName)

            End If

        End Sub

#End Region
#Region " ListView handlers"
        ' Prevent editing of the list items
        Private Sub ListView1_BeforeLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.LabelEditEventArgs) Handles ListView1.BeforeLabelEdit
            e.CancelEdit = True
        End Sub
        ' Sort the list when the user clicks on a column
        Private Sub ListView1_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick
            If ListView1.Sorting = SortOrder.Ascending Or ListView1.Sorting = SortOrder.None Then
                ListView1.Sorting = SortOrder.Descending
            Else
                ListView1.Sorting = SortOrder.Ascending
            End If
        End Sub
        ' This event fires when the user clicks an item in the list
        Private Sub ListView1_ItemActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.ItemActivate
            If mTestInProgress Then
                Exit Sub
            End If

            ' Step over all the tools in the toolgroup
            Dim ToolGroup As CogToolGroup
            ToolGroup = CogToolGroupEdit1.Subject
            Dim ImageFileTool As CogImageFileTool
            ImageFileTool = Nothing
            Dim Index As Integer
            For Index = 0 To ToolGroup.Tools.Count - 1
                ' If this tool is a CogImageFileTool
                If TypeOf ToolGroup.Tools.Item(Index) Is CogImageFileTool Then
                    ImageFileTool = ToolGroup.Tools.Item(Index)
                    ' Set the tool to run the image specified by the item's text
                    ImageFileTool.NextImageIndex = CInt(ListView1.SelectedItems(0).SubItems(0).Text)
                End If
            Next Index

            ' Now run the toolgroup to show the results of the selected image
            ToolGroup.Run()

        End Sub

#End Region

  End Class
end namespace