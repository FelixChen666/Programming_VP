'******************************************************************************
'Copyright (C) 2008 Cognex Corporation

'Subject to Cognex Corporations terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.
'*****************************************************************************/
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated(), _
System.ComponentModel.ToolboxItem(True)> _
Partial Class SimpleToolEditV2
    Inherits Cognex.VisionPro.CogToolEditControlBaseV2

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SimpleToolEditV2))
        Me.chkCopyTwice = New System.Windows.Forms.CheckBox
        CType(Me.sbpIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sbpStatusCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sbpStatusMessage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sbpProcessingTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sbpTotalTime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabControl.SuspendLayout()
        Me.tpgSettings.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbrButtons
        '
        Me.tbrButtons.Size = New System.Drawing.Size(748, 28)
        '
        'sbpStatusMessage
        '
        Me.sbpStatusMessage.Width = 502
        '
        'imlButtons
        '
        Me.imlButtons.ImageStream = CType(resources.GetObject("imlButtons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlButtons.Images.SetKeyName(0, "")
        Me.imlButtons.Images.SetKeyName(1, "")
        Me.imlButtons.Images.SetKeyName(2, "")
        Me.imlButtons.Images.SetKeyName(3, "")
        Me.imlButtons.Images.SetKeyName(4, "")
        Me.imlButtons.Images.SetKeyName(5, "")
        Me.imlButtons.Images.SetKeyName(6, "")
        Me.imlButtons.Images.SetKeyName(7, "")
        Me.imlButtons.Images.SetKeyName(8, "")
        Me.imlButtons.Images.SetKeyName(9, "")
        Me.imlButtons.Images.SetKeyName(10, "")
        '
        'tabControl
        '
        Me.tabControl.Size = New System.Drawing.Size(489, 484)
        '
        'tpgSettings
        '
        Me.tpgSettings.Controls.Add(Me.chkCopyTwice)
        Me.tpgSettings.Size = New System.Drawing.Size(481, 458)
        '
        'lblControlState
        '
        Me.lblControlState.Location = New System.Drawing.Point(8, 487)
        '
        'chkCopyTwice
        '
        Me.chkCopyTwice.AutoSize = True
        Me.chkCopyTwice.Location = New System.Drawing.Point(27, 22)
        Me.chkCopyTwice.Name = "chkCopyTwice"
        Me.chkCopyTwice.Size = New System.Drawing.Size(88, 17)
        Me.chkCopyTwice.TabIndex = 0
        Me.chkCopyTwice.Text = "Copy Twice?"
        Me.chkCopyTwice.UseVisualStyleBackColor = True
        '
        'SimpleToolEditV2
        '
        Me.Name = "SimpleToolEditV2"
        CType(Me.sbpIcon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sbpStatusCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sbpStatusMessage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sbpProcessingTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sbpTotalTime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabControl.ResumeLayout(False)
        Me.tpgSettings.ResumeLayout(False)
        Me.tpgSettings.PerformLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkCopyTwice As System.Windows.Forms.CheckBox

End Class
