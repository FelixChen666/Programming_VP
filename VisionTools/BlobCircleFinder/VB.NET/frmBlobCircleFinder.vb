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

' This sample demonstrates how rotate an image using an affine transform.
' The sample uses the objects and interfaces defined in the Cognex Core and
' Cognex Image type libraries.
'
' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
'
' The following steps show how to find circular blobs in images.
' Step 1) Load an image.
' Step 2) Create a CogBlobTool.
' Step 3) Add the Acircularity Property to the Results.
' Step 4) Default mode shows results for circular (acircularity near 1.0) blobs
'         changing a toggle in the GUI alternatively draw the non-circular objects.
'
Option Explicit On 
Imports Cognex.VisionPro            ' need to access basic VisionPro functionality
Imports Cognex.VisionPro.Blob       'need to access CogBlobTool
Imports Cognex.VisionPro.ImageFile  'need to access ImageFileTool
Imports Cognex.VisionPro.Exceptions
Namespace SampleBlobCircleFinder
  Public Class frmBlobCircleFinder
    Inherits System.Windows.Forms.Form
    Private mTool As CogBlobTool
    Private mImageFileTool As CogImageFileTool
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
    Friend WithEvents CogDisplay1 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents cmdBlobAndFilter As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents optCircular As System.Windows.Forms.RadioButton
    Friend WithEvents optOthers As System.Windows.Forms.RadioButton
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmBlobCircleFinder))
      Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
      Me.cmdBlobAndFilter = New System.Windows.Forms.Button
      Me.GroupBox1 = New System.Windows.Forms.GroupBox
      Me.optOthers = New System.Windows.Forms.RadioButton
      Me.optCircular = New System.Windows.Forms.RadioButton
      Me.TextBox1 = New System.Windows.Forms.TextBox
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.GroupBox1.SuspendLayout()
      Me.SuspendLayout()
      '
      'CogDisplay1
      '
      Me.CogDisplay1.Location = New System.Drawing.Point(16, 8)
      Me.CogDisplay1.Name = "CogDisplay1"
      Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
      Me.CogDisplay1.Size = New System.Drawing.Size(432, 240)
      Me.CogDisplay1.TabIndex = 0
      '
      'cmdBlobAndFilter
      '
      Me.cmdBlobAndFilter.Location = New System.Drawing.Point(504, 16)
      Me.cmdBlobAndFilter.Name = "cmdBlobAndFilter"
      Me.cmdBlobAndFilter.Size = New System.Drawing.Size(96, 32)
      Me.cmdBlobAndFilter.TabIndex = 1
      Me.cmdBlobAndFilter.Text = "Run"
      '
      'GroupBox1
      '
      Me.GroupBox1.Controls.Add(Me.optOthers)
      Me.GroupBox1.Controls.Add(Me.optCircular)
      Me.GroupBox1.Location = New System.Drawing.Point(456, 88)
      Me.GroupBox1.Name = "GroupBox1"
      Me.GroupBox1.TabIndex = 2
      Me.GroupBox1.TabStop = False
      Me.GroupBox1.Text = "Types of Blobs"
      '
      'optOthers
      '
      Me.optOthers.Location = New System.Drawing.Point(48, 56)
      Me.optOthers.Name = "optOthers"
      Me.optOthers.Size = New System.Drawing.Size(120, 24)
      Me.optOthers.TabIndex = 1
      Me.optOthers.Text = "Find Non_Circles"
      '
      'optCircular
      '
      Me.optCircular.Checked = True
      Me.optCircular.Location = New System.Drawing.Point(48, 24)
      Me.optCircular.Name = "optCircular"
      Me.optCircular.TabIndex = 0
      Me.optCircular.TabStop = True
      Me.optCircular.Text = "Find Circles"
      '
      'TextBox1
      '
      Me.TextBox1.AcceptsReturn = True
      Me.TextBox1.AcceptsTab = True
      Me.TextBox1.Location = New System.Drawing.Point(24, 272)
      Me.TextBox1.Multiline = True
      Me.TextBox1.Name = "TextBox1"
      Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
      Me.TextBox1.Size = New System.Drawing.Size(584, 40)
      Me.TextBox1.TabIndex = 3
      Me.TextBox1.Text = "Sample Description: runs a cogBlobTool and filters them based on a blobs acircula" & _
      "rity. You can make up your own filter by changing the code to look at one of the" & _
      " many blob properties."
      '
      'frmBlobCircleFinder
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(672, 366)
      Me.Controls.Add(Me.TextBox1)
      Me.Controls.Add(Me.GroupBox1)
      Me.Controls.Add(Me.cmdBlobAndFilter)
      Me.Controls.Add(Me.CogDisplay1)
      Me.Name = "frmBlobCircleFinder"
      Me.Text = "Shows how to run blob and find circles by filtering the output."
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.GroupBox1.ResumeLayout(False)
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region "Form and Controls events"
    Private Sub frmBlobCircleFinder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Try

        ' Create the CogBlobTool
        mTool = New CogBlobTool
        mImageFileTool = New CogImageFileTool

        ' Get VPRO_ROOT from environment which is needed to locate bracket_std.idb.
        Const ImageFileName As String = "/Images/bracket_std.idb"
        Dim strBaseDir As String
        strBaseDir = Environment.GetEnvironmentVariable("VPRO_ROOT")
        If strBaseDir = "" Then
          DisplayErrorAndExit("Required environment variable VPRO_ROOT not set.")
        End If

        ' Step 1 - Load an image and create shapes.
        ' Temporarily create the image file tool to open bracket_std.idb.
        mImageFileTool.[Operator].Open(strBaseDir & ImageFileName, CogImageFileModeConstants.Read)
        mImageFileTool.Run()
        ' We only need the first image
        CogDisplay1.Image = mImageFileTool.OutputImage
      Catch cogexFile As CogFileOpenException
        DisplayErrorAndExit("Encountered the following Cognex File Open specific error: " & cogexFile.Message)
      Catch cogex As CogException

        DisplayErrorAndExit("Encountered the following Cognex specific error: " & cogex.Message)
      Catch ex As Exception

        DisplayErrorAndExit("Encountered the following error: " & ex.Message)
      End Try
    End Sub

    Private Sub cmdBlobAndFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBlobAndFilter.Click
      ' clear any old graphics
      CogDisplay1.StaticGraphics.Clear()


      ' update the runtime image
      mTool.InputImage = mImageFileTool.OutputImage

      ' Run the tool
      mTool.Run()

      ' Acircularity is a measure of a blobs roundness.
      ' Acircularity values between 0.9 and 1.1 are circular objects.
      ' Those blobs with values outside that range are not circular objects.
      ' You can make up your own rules for filtering blobs.
      Dim r As Integer
      For r = 0 To mTool.Results.GetBlobs.Count - 1

        If optCircular.Checked Then
          ' display circular objects here
          If mTool.Results.GetBlobs.Item(r).Acircularity > 0.9 And mTool.Results.GetBlobs.Item(r).Acircularity < 1.1 Then
            CogDisplay1.StaticGraphics.Add(mTool.Results.GetBlobs.Item(r).CreateResultGraphics(CogBlobResultGraphicConstants.Boundary), "test")
          End If
        Else
          ' display non-circular objects here
          If mTool.Results.GetBlobs.Item(r).Acircularity <= 0.9 Or mTool.Results.GetBlobs.Item(r).Acircularity >= 1.1 Then
            CogDisplay1.StaticGraphics.Add(mTool.Results.GetBlobs.Item(r).CreateResultGraphics(CogBlobResultGraphicConstants.Boundary), "test")
          End If
        End If

      Next r

      ' Show the image
      CogDisplay1.Image = mImageFileTool.OutputImage

      ' Set the next image in the image file
      mImageFileTool.Run()
    End Sub
    Private Sub frmBlobCircleFinder_closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
      If Not mTool Is Nothing Then mTool.Dispose()
      If Not mImageFileTool Is Nothing Then mImageFileTool.Dispose()
    End Sub
#End Region
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
End Namespace