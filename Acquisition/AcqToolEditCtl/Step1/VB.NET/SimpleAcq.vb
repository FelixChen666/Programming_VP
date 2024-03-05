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


' This sample demonstrates how to add a CogAcqFifoEdit control to a VB form.
' Step 1) Create an empty VB form
' Step 2) Choose the Project -> Components menu option.
' Step 3) Select the Cognex AcqFifo Edit Control once a list box of Visual Basic
'         components appears by clicking its checkbox. The control is now added
'         to the toolbox.
' Step 4) Drag the CogAcqFifoEdit control from the toolbox onto the form.
'
' All of Cognex tool edit controls provide the AutoCreateTool property. When this
' property is set to True (its default value is True), the tool edit control creates
' the underlying tool automatically. This makes the control interactive
' immediately after it is created. However, this may not be what most of you
' want. SimpleAcq2 will show how to override the AutoCreateTool property.
'
' NOTE: The AutoCreateTool property value can only be changed during design time.
'
' Increase either exposure or brightness if you see a dark image from the CogAcqFifoEdit control.


Option Explicit On 
' Needed for VisionPro
Imports Cognex.VisionPro
' Needed for VisionPro Exceptions
Imports Cognex.VisionPro.Exceptions

Namespace SimpleAcq
  Public Class SimpleAcq
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
            Me.txtDescription.Text = "Sample description: demonstrates how to add a CogAcqFifoEdit control to a VB form" & _
        "." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " A Cognex frame grabber board must be present in order to run this " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "sample p" & _
        "rogram."
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
            'SimpleAcq
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(752, 494)
            Me.Controls.Add(Me.CogAcqFifoEditV21)
            Me.Controls.Add(Me.txtDescription)
            Me.Name = "SimpleAcq"
            Me.Text = "Shows how to add and use an CogAcqFifoCtl"
            CType(Me.CogAcqFifoEditV21, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region
  End Class
End Namespace
