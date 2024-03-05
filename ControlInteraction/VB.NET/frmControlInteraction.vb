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

' This sample demonstrates how VisionPro controls can interact with other
' VisionPro controls. Examples of VisionPro controls interacting with standard
' Windows controls are also provided.
'
' The tool edit controls in this sample have their AutoCreateTool property
' set to TRUE.  This causes each tool edit control to automatically create
' and attach its tool.  See Run_Click() for examples of how to access a tool
' edit control tool's properties and methods.  In particular, the code
' demonstrates how to pass an image from one tool to another.
'
' The sample also demonstrates how a tab control can be used to include
' multiple tool edit controls on a single form.  Because tool edit controls
' can be quite large, tab controls provide a nice mechanism for limiting
' their impact on screen real estate.
'
Option Explicit On 
Namespace SampleControlInteraction
  Public Class frmControlInteraction
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
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
        Friend WithEvents ImageFileEdit As Cognex.VisionPro.ImageFile.CogImageFileEditV2
        Friend WithEvents CaliperEdit As Cognex.VisionPro.Caliper.CogCaliperEditV2
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmControlInteraction))
      Me.TabControl1 = New System.Windows.Forms.TabControl
      Me.TabPage1 = New System.Windows.Forms.TabPage
            Me.ImageFileEdit = New Cognex.VisionPro.ImageFile.CogImageFileEditV2
      Me.TabPage2 = New System.Windows.Forms.TabPage
            Me.CaliperEdit = New Cognex.VisionPro.Caliper.CogCaliperEditV2
      Me.btnRun = New System.Windows.Forms.Button
      Me.TextBox1 = New System.Windows.Forms.TextBox
      Me.TabControl1.SuspendLayout()
      Me.TabPage1.SuspendLayout()
      CType(Me.ImageFileEdit, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.TabPage2.SuspendLayout()
      CType(Me.CaliperEdit, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'TabControl1
      '
      Me.TabControl1.Controls.Add(Me.TabPage1)
      Me.TabControl1.Controls.Add(Me.TabPage2)
      Me.TabControl1.Location = New System.Drawing.Point(-8, 8)
      Me.TabControl1.Name = "TabControl1"
      Me.TabControl1.SelectedIndex = 0
      Me.TabControl1.Size = New System.Drawing.Size(736, 384)
      Me.TabControl1.TabIndex = 0
      '
      'TabPage1
      '
      Me.TabPage1.Controls.Add(Me.ImageFileEdit)
      Me.TabPage1.Location = New System.Drawing.Point(4, 22)
      Me.TabPage1.Name = "TabPage1"
      Me.TabPage1.Size = New System.Drawing.Size(728, 358)
      Me.TabPage1.TabIndex = 0
      Me.TabPage1.Text = "ImageFileTool Edit Control"
      '
      'ImageFileEdit
      '
            Me.ImageFileEdit.Location = New System.Drawing.Point(8, 0)
            Me.ImageFileEdit.MinimumSize = New System.Drawing.Size(489, 0)
      Me.ImageFileEdit.Name = "ImageFileEdit"
            Me.ImageFileEdit.Size = New System.Drawing.Size(712, 328)
            Me.ImageFileEdit.SuspendElectricRuns = False
      Me.ImageFileEdit.TabIndex = 0
      '
      'TabPage2
      '
      Me.TabPage2.Controls.Add(Me.CaliperEdit)
      Me.TabPage2.Location = New System.Drawing.Point(4, 22)
      Me.TabPage2.Name = "TabPage2"
      Me.TabPage2.Size = New System.Drawing.Size(728, 358)
      Me.TabPage2.TabIndex = 1
      Me.TabPage2.Text = "CaliperTool Edit Control"
      '
      'CaliperEdit
      '
            Me.CaliperEdit.Location = New System.Drawing.Point(8, 8)
            Me.CaliperEdit.MinimumSize = New System.Drawing.Size(489, 0)
      Me.CaliperEdit.Name = "CaliperEdit"
            Me.CaliperEdit.Size = New System.Drawing.Size(784, 480)
            Me.CaliperEdit.SuspendElectricRuns = False
      Me.CaliperEdit.TabIndex = 0
      '
      'btnRun
      '
      Me.btnRun.Location = New System.Drawing.Point(752, 32)
      Me.btnRun.Name = "btnRun"
      Me.btnRun.Size = New System.Drawing.Size(96, 64)
      Me.btnRun.TabIndex = 1
      Me.btnRun.Text = "Run"
      '
      'TextBox1
      '
      Me.TextBox1.Location = New System.Drawing.Point(16, 416)
      Me.TextBox1.Multiline = True
      Me.TextBox1.Name = "TextBox1"
      Me.TextBox1.Size = New System.Drawing.Size(712, 20)
      Me.TextBox1.TabIndex = 2
      Me.TextBox1.Text = "Sample description: shows how to pass parameters between tool edit control subjec" & _
      "ts."
      '
      'frmControlInteraction
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(888, 478)
      Me.Controls.Add(Me.TextBox1)
      Me.Controls.Add(Me.btnRun)
      Me.Controls.Add(Me.TabControl1)
      Me.Name = "frmControlInteraction"
      Me.Text = "Control Interaction Sample Application"
      Me.TabControl1.ResumeLayout(False)
      Me.TabPage1.ResumeLayout(False)
      CType(Me.ImageFileEdit, System.ComponentModel.ISupportInitialize).EndInit()
      Me.TabPage2.ResumeLayout(False)
      CType(Me.CaliperEdit, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region "Form and Controls Events"
    Private Sub frmControlInteraction_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


      ' Locate the default image file using the environment variable that
      ' indicates where VisionPro is installed.  If the environment variable is
      ' not set, display the error and return failure.

      Dim ImageFilePath As String
      Try
        ImageFilePath = Environment.GetEnvironmentVariable("VPRO_ROOT")
        If ImageFilePath = "" Then _
          DisplayErrorAndExit("Required environment variable VPRO_ROOT not set.")

        ImageFilePath = ImageFilePath & "\Images\Chips.idb"
        ImageFileEdit.Subject.[Operator].Open(ImageFilePath, Cognex.VisionPro.ImageFile.CogImageFileModeConstants.Read)

        ' Show the second tab so that the user can see the caliper graphics.
        TabControl1.SelectedIndex = 1
      Catch cogex As Cognex.VisionPro.exceptions.CogException
        MessageBox.Show("Following Specific Cognex Error Occured:" & cogex.Message)
      Catch ex As Exception
        DisplayErrorAndExit("Encountered the following error: " & ex.Message)
      End Try
    End Sub

    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
      ' Get new image from image file tool edit control's subject, pass new
      ' image to caliper tool edit control's subject, then run the caliper
      ' tool edit control's subject.  Since each of the subjects is a type
      ' of VisionPro tool, the previous statement is equivalent to: Get new
      ' image from image file tool, pass new image to caliper tool, then run
      ' the caliper tool.
      ImageFileEdit.Subject.Run()
      CaliperEdit.Subject.InputImage = ImageFileEdit.Subject.OutputImage
      CaliperEdit.Subject.Run()
    End Sub
    ' Displays an error message and then exits the program.
    ' Call this when an unrecoverable error has occurred.
    Private Sub DisplayErrorAndExit(ByVal ErrorMsg As String)
      MessageBox.Show(ErrorMsg & Environment.NewLine & "Press OK to exit.")
      Me.Close()
      End      ' quit if it is called from Form_Load
    End Sub
#End Region
  End Class
End Namespace