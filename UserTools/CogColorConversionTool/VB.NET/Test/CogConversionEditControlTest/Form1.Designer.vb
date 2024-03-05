' *******************************************************************************
'Copyright (C) 2008 Cognex Corporation

'Subject to Cognex Corporations terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.
'*******************************************************************************
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

'This sample demonstrates how a user can create their own Vision Tools using 
'base classes the Cognex provides.  The tool created in this class converts 3-plane
'RGB images to individual R, G, and B planes, or converts a 3-plane RGB image to 
'individual H, S, and I planes.

'The tool created is called the CogColorConversionTool.  It makes use of an operator, which
'does all the real work, called CogColorConversion.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.CogColorConversionToolEditor1 = New CogColorConversionTool.CogColorConversionToolEditor
        Me.CogImageFileEditV21 = New Cognex.VisionPro.ImageFile.CogImageFileEditV2
        CType(Me.CogColorConversionToolEditor1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CogImageFileEditV21, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CogColorConversionToolEditor1
        '
        Me.CogColorConversionToolEditor1.Location = New System.Drawing.Point(12, 304)
        Me.CogColorConversionToolEditor1.MinimumSize = New System.Drawing.Size(489, 0)
        Me.CogColorConversionToolEditor1.Name = "CogColorConversionToolEditor1"
        Me.CogColorConversionToolEditor1.Size = New System.Drawing.Size(748, 280)
        Me.CogColorConversionToolEditor1.Subject = Nothing
        Me.CogColorConversionToolEditor1.SuspendElectricRuns = False
        Me.CogColorConversionToolEditor1.TabIndex = 0
        '
        'CogImageFileEditV21
        '
        Me.CogImageFileEditV21.AllowDrop = True
        Me.CogImageFileEditV21.Location = New System.Drawing.Point(12, 12)
        Me.CogImageFileEditV21.MinimumSize = New System.Drawing.Size(489, 0)
        Me.CogImageFileEditV21.Name = "CogImageFileEditV21"
        Me.CogImageFileEditV21.Size = New System.Drawing.Size(748, 286)
        Me.CogImageFileEditV21.SuspendElectricRuns = False
        Me.CogImageFileEditV21.TabIndex = 1
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(772, 598)
        Me.Controls.Add(Me.CogImageFileEditV21)
        Me.Controls.Add(Me.CogColorConversionToolEditor1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.CogColorConversionToolEditor1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CogImageFileEditV21, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CogColorConversionToolEditor1 As CogColorConversionTool.CogColorConversionToolEditor
    Friend WithEvents CogImageFileEditV21 As Cognex.VisionPro.ImageFile.CogImageFileEditV2

End Class
