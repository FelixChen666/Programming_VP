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

' This sample demonstrates how to detect a particular security bit. In this example,
' we will test for the PMAlign security bit.
'
Option Explicit On 
'needed for VisionPro
Imports Cognex.VisionPro
'needed for VisionPro exceptions
Imports Cognex.VisionPro.Exceptions
Namespace LicenseCheck
  Public Class LicenseCheck
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
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents List1 As System.Windows.Forms.ListBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.List1 = New System.Windows.Forms.ListBox
      Me.lblMessage = New System.Windows.Forms.Label
      Me.Label1 = New System.Windows.Forms.Label
      Me.txtDescription = New System.Windows.Forms.TextBox
      Me.SuspendLayout()
      '
      'List1
      '
      Me.List1.Location = New System.Drawing.Point(0, 104)
      Me.List1.Name = "List1"
      Me.List1.Size = New System.Drawing.Size(456, 277)
      Me.List1.TabIndex = 0
      '
      'lblMessage
      '
      Me.lblMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblMessage.Location = New System.Drawing.Point(0, 16)
      Me.lblMessage.Name = "lblMessage"
      Me.lblMessage.Size = New System.Drawing.Size(456, 48)
      Me.lblMessage.TabIndex = 1
      '
      'Label1
      '
      Me.Label1.Location = New System.Drawing.Point(0, 80)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(248, 16)
      Me.Label1.TabIndex = 2
      Me.Label1.Text = "Available Features:"
      '
      'txtDescription
      '
      Me.txtDescription.Location = New System.Drawing.Point(8, 392)
      Me.txtDescription.Multiline = True
      Me.txtDescription.Name = "txtDescription"
      Me.txtDescription.Size = New System.Drawing.Size(432, 56)
      Me.txtDescription.TabIndex = 3
      Me.txtDescription.Text = "Sample Description: demonstrates how to determine if a particular feature is lice" & _
      "nsed. " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "The sample detects the PMAlign security bit. Listbox displays all the fe" & _
      "atures that are " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "available."
      Me.txtDescription.WordWrap = False
      '
      'LicenseCheck
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(456, 462)
      Me.Controls.Add(Me.txtDescription)
      Me.Controls.Add(Me.Label1)
      Me.Controls.Add(Me.lblMessage)
      Me.Controls.Add(Me.List1)
      Me.Name = "LicenseCheck"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
      Me.Text = "License Check"
      Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub LicenseCheck_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
      ' Put a default string
      lblMessage.Text = "PMAlign license bit is disabled"

      Dim LicensedFeatures As CogStringCollection
      ' GetLicensedFeatures returns a string collection containing the names of all features
      ' for which licenses are available.
      Try
        LicensedFeatures = CogLicense.GetLicensedFeatures(False, False)

        Dim Feature As Object
        For Each Feature In LicensedFeatures
          ' Add a feature to the list.
          List1.Items.Add(CType(Feature, String))
          If StrComp(CType(Feature, String), "VisionPro.PatMax") = 0 Then
            lblMessage.Text = "PMAlign license bit is enabled"
          End If
        Next Feature
      Catch ex As CogException
        MsgBox(ex.Message)
      Catch gex As Exception
        MsgBox(gex.Message)
      End Try
    End Sub
  End Class
End Namespace