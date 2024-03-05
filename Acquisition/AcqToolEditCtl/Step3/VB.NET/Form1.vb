'*******************************************************************************
' Copyright (C) 2004-2010 Cognex Corporation
'
' Subject to Cognex Corporation's terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore, you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
'*******************************************************************************
'
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.


' This sample demonstrates how to modify the CogAcqFifoTool properties.
' The CogAcqFifoEdit control will update property values as they are changed.
' See SimpleAcq and SimpleAcq2 for instructions on adding the CogAcqFifoEdit
' and CogAcqFifoTool.
'
' This program assumes that you have some knowledge of Visual Basic programming.
'
' Step 1) Create the CogAcqFifoTool
' Step 2) Assign the CogAcqFifoTool to the cogAcqFifoEditV21
' Step 3) Change the Timeout value to 500 ms
' Step 4) Retrieve the CogAcqBrightness interface to change the brightness.
'


Option Explicit On 
' Needed for VisionPro
Imports Cognex.VisionPro
' Needed for VisionPro Exceptions
Imports Cognex.VisionPro.Exceptions
Namespace SimpleAcq3
  Public Class Form1
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
        Dim frameGrabbers As New CogFrameGrabbers
        For Each fg As Cognex.VisionPro.ICogFrameGrabber In frameGrabbers
          fg.Disconnect(False)
        Next
      End If
      MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Friend WithEvents CogAcqFifoEditV21 As Cognex.VisionPro.CogAcqFifoEditV2

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.txtDescription = New System.Windows.Forms.TextBox()
            Me.CogAcqFifoEditV21 = New Cognex.VisionPro.CogAcqFifoEditV2()
            CType(Me.CogAcqFifoEditV21, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'txtDescription
            '
            Me.txtDescription.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.txtDescription.Location = New System.Drawing.Point(0, 446)
            Me.txtDescription.Multiline = True
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Size = New System.Drawing.Size(752, 48)
            Me.txtDescription.TabIndex = 1
            Me.txtDescription.Text = "Sample description: demonstrates how to modify the CogAcqFifoTool properties. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "A" & _
        " Cognex frame grabber board must be present in order to run this sample " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "progra" & _
        "m."
            '
            'CogAcqFifoEditV21
            '
            Me.CogAcqFifoEditV21.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CogAcqFifoEditV21.Location = New System.Drawing.Point(0, 0)
            Me.CogAcqFifoEditV21.MinimumSize = New System.Drawing.Size(489, 0)
            Me.CogAcqFifoEditV21.Name = "CogAcqFifoEditV21"
            Me.CogAcqFifoEditV21.Size = New System.Drawing.Size(752, 446)
            Me.CogAcqFifoEditV21.SuspendElectricRuns = False
            Me.CogAcqFifoEditV21.TabIndex = 2
            '
            'Form1
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(752, 494)
            Me.Controls.Add(Me.CogAcqFifoEditV21)
            Me.Controls.Add(Me.txtDescription)
            Me.Name = "Form1"
            Me.Text = "Shows how to add and use an CogAcqFifoCtl"
            CType(Me.CogAcqFifoEditV21, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region
    ' This subroutine does all the initialization work...
    '
    Private Sub InitializeAcquisition()

      ' Step 1 - Create the CogAcqFifoTool
      Dim mTool As CogAcqFifoTool
      mTool = New CogAcqFifoTool
      '
      ' Step 2 - Assign the tool to the control
      CogAcqFifoEditV21.Subject = mTool
      '
      ' Step 3 - Change the Timeout to 500 ms.
      '          Make sure that the valid operator is present. If not, quit
      '          the application because nothing can be done.
      If mTool.[Operator] Is Nothing Then
        MsgBox("A board might be missing or not be functioning properly. Press OK to exit.")
        Return
      End If

      mTool.[Operator].Timeout = 500

      ' Step 4 - Query for the brightness interface. The brightness can be
      '          modified once its interface pointer is obtained.
      Dim Brightness As Cognex.VisionPro.ICogAcqBrightness
      ' Obtain the CogAcqBrightness interface. See ErrorHandle sample for handling errors.
      Brightness = mTool.[Operator].OwnedBrightnessParams
      If Not Brightness Is Nothing Then
        Brightness.Brightness = 0.9
      Else
        MsgBox("Failed to retrieve the CogAcqBrightness interface pointer. Press OK to exit.")
        Application.Exit()
      End If

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Try
        InitializeAcquisition()
      Catch ex As CogException
        MessageBox.Show(ex.Message)
        Application.Exit()
      Catch gex As Exception
        MessageBox.Show(gex.Message)
        Application.Exit()
      End Try

    End Sub
  End Class
End Namespace