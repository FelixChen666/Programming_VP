'*******************************************************************************
'Copyright (C) 2008 Cognex Corporation

'Subject to Cognex Corporations terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.
'*******************************************************************************/
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

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
        Me.CogImageFileEditV21 = New Cognex.VisionPro.ImageFile.CogImageFileEditV2
        Me.SimpleToolEditV21 = New SimpleUserToolVB.SimpleToolEditV2
        CType(Me.CogImageFileEditV21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SimpleToolEditV21, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CogImageFileEditV21
        '
        Me.CogImageFileEditV21.AllowDrop = True
        Me.CogImageFileEditV21.Location = New System.Drawing.Point(12, 12)
        Me.CogImageFileEditV21.MinimumSize = New System.Drawing.Size(489, 0)
        Me.CogImageFileEditV21.Name = "CogImageFileEditV21"
        Me.CogImageFileEditV21.Size = New System.Drawing.Size(748, 286)
        Me.CogImageFileEditV21.SuspendElectricRuns = False
        Me.CogImageFileEditV21.TabIndex = 0
        '
        'SimpleToolEditV21
        '
        Me.SimpleToolEditV21.Location = New System.Drawing.Point(12, 304)
        Me.SimpleToolEditV21.MinimumSize = New System.Drawing.Size(489, 0)
        Me.SimpleToolEditV21.Name = "SimpleToolEditV21"
        Me.SimpleToolEditV21.Size = New System.Drawing.Size(748, 269)
        Me.SimpleToolEditV21.SuspendElectricRuns = False
        Me.SimpleToolEditV21.TabIndex = 1
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(777, 585)
        Me.Controls.Add(Me.SimpleToolEditV21)
        Me.Controls.Add(Me.CogImageFileEditV21)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.CogImageFileEditV21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SimpleToolEditV21, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CogImageFileEditV21 As Cognex.VisionPro.ImageFile.CogImageFileEditV2
    Friend WithEvents SimpleToolEditV21 As SimpleUserToolVB.SimpleToolEditV2

End Class
