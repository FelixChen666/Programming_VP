'*******************************************************************************
' Copyright (C) 2002 Cognex Corporation
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

' This sample demonstrates how to create a tool, assign it to a tool edit control
' and capture the tool Change, PostRun and PreRun events. The CogBlobTool and
' the CogBlobEdit control are used for this purpose. The CogImageFileTool will
' automatically load braket_std.idb that is located in the images directory
' at the start up. The program retrieves an image from the CogImageFileCDB
' and assigns it as an input image of the CogBlobTool. This is done inside
' of the CogBlobTool’s PreRun event handler.
'
' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
'
Option Explicit On 
'needed for VisionPro
Imports Cognex.VisionPro
'needed for image processing
Imports Cognex.VisionPro.ImageProcessing
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.Blob
'needed for VisionPro
Imports Cognex.VisionPro.Exceptions
Namespace ToolEvents
  Public Class ToolEventsForm
    Inherits System.Windows.Forms.Form
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
        Friend WithEvents VisionProToolEdit As Cognex.VisionPro.Blob.CogBlobEditV2
    Friend WithEvents SampleDescription As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ToolEventsForm))
            Me.VisionProToolEdit = New Cognex.VisionPro.Blob.CogBlobEditV2
      Me.SampleDescription = New System.Windows.Forms.TextBox
      CType(Me.VisionProToolEdit, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'VisionProToolEdit
      '
            Me.VisionProToolEdit.Location = New System.Drawing.Point(8, 8)
            Me.VisionProToolEdit.MinimumSize = New System.Drawing.Size(489, 0)
      Me.VisionProToolEdit.Name = "VisionProToolEdit"
            Me.VisionProToolEdit.Size = New System.Drawing.Size(808, 424)
            Me.VisionProToolEdit.SuspendElectricRuns = False
      Me.VisionProToolEdit.TabIndex = 0
      '
      'SampleDescription
      '
      Me.SampleDescription.Location = New System.Drawing.Point(0, 456)
      Me.SampleDescription.Multiline = True
      Me.SampleDescription.Name = "SampleDescription"
      Me.SampleDescription.Size = New System.Drawing.Size(800, 48)
      Me.SampleDescription.TabIndex = 1
      Me.SampleDescription.Text = "Sample description: shows how tool event handlers can be implemented to do things" & _
      " at various stages of a tool's run method." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Sample usage: switch to the last run" & _
      " input image display, then press the run button on the tool edit control.  Note " & _
      "that a new image is inspected each " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "time the tool is run."
      Me.SampleDescription.WordWrap = False
      '
      'ToolEventsForm
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(816, 510)
      Me.Controls.Add(Me.SampleDescription)
      Me.Controls.Add(Me.VisionProToolEdit)
      Me.Name = "ToolEventsForm"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
      Me.Text = "Tool Events Sample Application"
      CType(Me.VisionProToolEdit, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region " Private vars"
    ' Handle Change and PreRun events.
    Private WithEvents VisionProTool As CogBlobTool
    ' Used by PreRun event to obtain input image from image file.
    Private ImageSource As CogImageFileTool
#End Region
#Region " Initialization"
    Private Sub Initialize()
      Dim ImageSourceFile As String
         ImageSource = New CogImageFileTool
        ' Locate the image file using the environment variable that indicates
        ' where VisionPro is installed.  If the environment variable is not set,
        ' display the error and terminate the application.
        ImageSourceFile = Environ("VPRO_ROOT")
        If ImageSourceFile = "" Then
          Throw New Exception("Required environment variable VPRO_ROOT not set.")
        End If
        ImageSourceFile = ImageSourceFile & "/Images/bracket_std.idb"

        ' Open the image file.  If an error occurs when opening the image file
        ' (not found, for example), report it and return failure.  Once the
        ' file is opened, exception handling is disabled via On Error GoTo 0.

        ImageSource.[Operator].Open(ImageSourceFile, CogImageFileModeConstants.Read)

        ' Create tool and attach it to the tool edit control.
        VisionProTool = New CogBlobTool
        VisionProToolEdit.Subject = VisionProTool

        ' Run once at startup.
        VisionProTool.Run()


    End Sub
#End Region
#Region " Helper functions"
    ' Displays an error message and then exits the program.
    ' Call this when an unrecoverable error has occurred.
    Private Sub DisplayErrorAndExit(ByVal ErrorMsg As String)
      MsgBox(ErrorMsg & vbCr & "Press OK to exit.")
      Application.Exit()
    End Sub
#End Region
#Region " Tool event handlers"
    Private Sub VisionProTool_Changed(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogChangedEventArgs) Handles VisionProTool.Changed
      ' Report error conditions, if any.
      If VisionProTool.RunStatus.Result <> CogToolResultConstants.Accept Then
        MsgBox(VisionProTool.RunStatus.Message, vbExclamation)
      End If

    End Sub
    Private Sub VisionProTool_Running(ByVal sender As Object, ByVal e As System.EventArgs) Handles VisionProTool.Running
      ' Get input image and 
      ' Report error conditions, if any.
      Try
        ImageSource.Run()
        VisionProTool.InputImage = CType(ImageSource.OutputImage, CogImage8Grey)
      Catch ex As CogException
        MessageBox.Show(ex.Message)
      Catch gex As Exception
        MessageBox.Show(gex.Message)
      End Try
    End Sub


    Private Sub VisionProTool_Ran(ByVal sender As Object, ByVal e As System.EventArgs) Handles VisionProTool.Ran
      MessageBox.Show("Tool has run")
    End Sub

#End Region

    Private Sub ToolEventsForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Try
        Initialize()
      Catch ex As Cognex.VisionPro.Exceptions.CogException
        DisplayErrorAndExit(ex.Message)
      End Try

    End Sub
  End Class

End Namespace
