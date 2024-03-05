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

' This sample demonstrates how to initialize a tool edit control from
' a persisted VisionPro object file (.vpp). The sample loads the CogPMAlignTool
' from the PMAlign.vpp file located in the VPP directory.
'
' The file loaded contains the entire vision tool configuration including a
' train image, a last run input image and a current input image.  To see the
' result of running the tool, switch to the last run input image, then press
' the run button on the tool edit control.  Note that only the initial run
' will have any noticable effect as the tool's configuration (including its
' input image) never changes.
'
Option Explicit On 
'needed for VisionPro
Imports Cognex.VisionPro
' needed for VissionPro tool
Imports Cognex.VisionPro.PMAlign
'needed for VisionPro Exceptions
Imports Cognex.VisionPro.Exceptions

Namespace LoadPersistedTool
  Public Class LoadPersistedToolForm
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
    Friend WithEvents SampleDescription As System.Windows.Forms.TextBox
        Friend WithEvents VisionToolEdit As Cognex.VisionPro.PMAlign.CogPMAlignEditV2
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(LoadPersistedToolForm))
            Me.VisionToolEdit = New Cognex.VisionPro.PMAlign.CogPMAlignEditV2
      Me.SampleDescription = New System.Windows.Forms.TextBox
      CType(Me.VisionToolEdit, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'VisionToolEdit
      '
            Me.VisionToolEdit.Location = New System.Drawing.Point(8, 16)
            Me.VisionToolEdit.MinimumSize = New System.Drawing.Size(489, 0)
      Me.VisionToolEdit.Name = "VisionToolEdit"
            Me.VisionToolEdit.Size = New System.Drawing.Size(784, 448)
            Me.VisionToolEdit.SuspendElectricRuns = False
      Me.VisionToolEdit.TabIndex = 0
      '
      'SampleDescription
      '
      Me.SampleDescription.Location = New System.Drawing.Point(8, 472)
      Me.SampleDescription.Multiline = True
      Me.SampleDescription.Name = "SampleDescription"
      Me.SampleDescription.Size = New System.Drawing.Size(768, 48)
      Me.SampleDescription.TabIndex = 1
      Me.SampleDescription.Text = "Sample description: shows how to restore a tool's configuration from a .vpp file." & _
      "" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Sample usage: switch to the last run input image display, then press the run b" & _
      "utton on the tool edit control.  Note that only the initial run will produce " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "a" & _
      "ny noticable effects.  See the comment at the top of this form's source code for" & _
      " more information."
      '
      'LoadPersistedToolForm
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(792, 534)
      Me.Controls.Add(Me.SampleDescription)
      Me.Controls.Add(Me.VisionToolEdit)
      Me.Name = "LoadPersistedToolForm"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
      Me.Text = "Load Persisted Tool Sample Application"
      CType(Me.VisionToolEdit, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub LoadPersistedToolForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
      ' Locate the tool configuration file using the environment variable that
      ' indicates where VisionPro is installed.  If the environment variable is
      ' not set, display the error and terminate the application.
      Dim PMAlignVPPFile As String
      Try
        PMAlignVPPFile = Environ("VPRO_ROOT")
        If PMAlignVPPFile = "" Then
          Throw New Exception("Required environment variable VPRO_ROOT not set.")
        End If
        PMAlignVPPFile = PMAlignVPPFile & "\Samples\Programming\LoadPersistedTool\PMAlign.vpp"

        ' The edit control's AutoCreate property is false.  Its subject is
        ' loaded from a persistence file.

        VisionToolEdit.Subject = CType(CogSerializer.LoadObjectFromFile(PMAlignVPPFile), CogPMAlignTool)
        ' If loading the persistence file failed, display the error and terminate
        ' the application.
      Catch ex As CogException
        MsgBox(ex.Message, MsgBoxStyle.Critical)
        Application.Exit()
      Catch gex As Exception
        MsgBox(gex.Message, MsgBoxStyle.Critical)
        Application.Exit()
      End Try

    End Sub
  End Class

End Namespace
