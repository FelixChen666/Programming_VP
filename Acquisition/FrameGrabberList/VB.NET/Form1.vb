'*******************************************************************************
' Copyright (C) 2004-2010 Cognex Corporation
'
' Subject to Cognex Corporation's terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
'*******************************************************************************
'
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This sample demonstrates how to list all the frame grabbers installed in a system.
' A Cognex frame grabber board(s) must be present in order to make the sample work.
' If no Cognex board is present, the program displays "No Frame Grabbers"
'
' This program assumes that you have some knowledge of Visual Basic.
'
' The following steps show how to list frame grabber names.
' Step 1) Create the CogFrameGrabbers
' Step 2) Check the frame grabber counts. If it is greater than 1
'         add each frame grabber name to the combobox
Option Explicit On 

' Needed for VisionPro
Imports Cognex.VisionPro
'Needed for VisionPro Exceptions
Imports Cognex.VisionPro.Exceptions
Namespace FrameGrabberList
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

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.Label1 = New System.Windows.Forms.Label
      Me.ComboBox1 = New System.Windows.Forms.ComboBox
      Me.SuspendLayout()
      '
      'Label1
      '
      Me.Label1.Location = New System.Drawing.Point(40, 24)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(136, 24)
      Me.Label1.TabIndex = 0
      Me.Label1.Text = "Frame Grabber Names:"
      '
      'ComboBox1
      '
      Me.ComboBox1.Location = New System.Drawing.Point(32, 80)
      Me.ComboBox1.Name = "ComboBox1"
      Me.ComboBox1.Size = New System.Drawing.Size(216, 21)
      Me.ComboBox1.TabIndex = 1
      '
      'Form1
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(312, 158)
      Me.Controls.Add(Me.ComboBox1)
      Me.Controls.Add(Me.Label1)
      Me.Name = "Form1"
      Me.Text = "Lists names of all the frame grabbers installed in a system"
      Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub InitializeAcquisition()
      ' Step 1 - Create the CogFrameGrabbers
      Dim fgList As CogFrameGrabbers
      fgList = New CogFrameGrabbers
      If fgList Is Nothing Then
        Throw New Exception("Failed to create the CogFrameGrabbers object.")
      End If
      ' Step 2 - Check the frame grabber counts. If it is greater than 0
      '          add each frame grabber name to the combobox
      If fgList.Count > 0 Then
        Dim fg As Cognex.VisionPro.ICogFrameGrabber
        ' Add each frame grabber name to the combobox.
        For Each fg In fgList
          ComboBox1.Items.Add(fg.Name)
        Next fg
      Else
        ComboBox1.Items.Add("No Frame Grabbers")
      End If
      ComboBox1.SelectedIndex = 0


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