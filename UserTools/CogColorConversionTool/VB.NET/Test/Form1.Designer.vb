'/*******************************************************************************
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

'This sample tests the CogColorConversionTool.

'*/

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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.origDisplay = New Cognex.VisionPro.Display.CogDisplay
        Me.convertedDisplay = New Cognex.VisionPro.Display.CogDisplay
        Me.lblOriginalImage = New System.Windows.Forms.Label
        Me.lblConvertedImagePlane = New System.Windows.Forms.Label
        Me.chkConvertToHSI = New System.Windows.Forms.CheckBox
        Me.btnConvert = New System.Windows.Forms.Button
        Me.grpDisplaySelector = New System.Windows.Forms.GroupBox
        Me.radioPlane2 = New System.Windows.Forms.RadioButton
        Me.radioPlane1 = New System.Windows.Forms.RadioButton
        Me.radioPlane0 = New System.Windows.Forms.RadioButton
        CType(Me.origDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.convertedDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDisplaySelector.SuspendLayout()
        Me.SuspendLayout()
        '
        'origDisplay
        '
        Me.origDisplay.Location = New System.Drawing.Point(12, 42)
        Me.origDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
        Me.origDisplay.MouseWheelSensitivity = 1
        Me.origDisplay.Name = "origDisplay"
        Me.origDisplay.OcxState = CType(resources.GetObject("origDisplay.OcxState"), System.Windows.Forms.AxHost.State)
        Me.origDisplay.Size = New System.Drawing.Size(300, 300)
        Me.origDisplay.TabIndex = 0
        '
        'convertedDisplay
        '
        Me.convertedDisplay.Location = New System.Drawing.Point(340, 42)
        Me.convertedDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
        Me.convertedDisplay.MouseWheelSensitivity = 1
        Me.convertedDisplay.Name = "convertedDisplay"
        Me.convertedDisplay.OcxState = CType(resources.GetObject("convertedDisplay.OcxState"), System.Windows.Forms.AxHost.State)
        Me.convertedDisplay.Size = New System.Drawing.Size(300, 300)
        Me.convertedDisplay.TabIndex = 1
        '
        'lblOriginalImage
        '
        Me.lblOriginalImage.AutoSize = True
        Me.lblOriginalImage.Location = New System.Drawing.Point(12, 16)
        Me.lblOriginalImage.Name = "lblOriginalImage"
        Me.lblOriginalImage.Size = New System.Drawing.Size(74, 13)
        Me.lblOriginalImage.TabIndex = 2
        Me.lblOriginalImage.Text = "Original Image"
        '
        'lblConvertedImagePlane
        '
        Me.lblConvertedImagePlane.AutoSize = True
        Me.lblConvertedImagePlane.Location = New System.Drawing.Point(337, 16)
        Me.lblConvertedImagePlane.Name = "lblConvertedImagePlane"
        Me.lblConvertedImagePlane.Size = New System.Drawing.Size(118, 13)
        Me.lblConvertedImagePlane.TabIndex = 3
        Me.lblConvertedImagePlane.Text = "Converted Image Plane"
        '
        'chkConvertToHSI
        '
        Me.chkConvertToHSI.AutoSize = True
        Me.chkConvertToHSI.Location = New System.Drawing.Point(44, 378)
        Me.chkConvertToHSI.Name = "chkConvertToHSI"
        Me.chkConvertToHSI.Size = New System.Drawing.Size(102, 17)
        Me.chkConvertToHSI.TabIndex = 4
        Me.chkConvertToHSI.Text = "Convert to HSI?"
        Me.chkConvertToHSI.UseVisualStyleBackColor = True
        '
        'btnConvert
        '
        Me.btnConvert.Location = New System.Drawing.Point(199, 378)
        Me.btnConvert.Name = "btnConvert"
        Me.btnConvert.Size = New System.Drawing.Size(91, 40)
        Me.btnConvert.TabIndex = 5
        Me.btnConvert.Text = "Convert!"
        Me.btnConvert.UseVisualStyleBackColor = True
        '
        'grpDisplaySelector
        '
        Me.grpDisplaySelector.Controls.Add(Me.radioPlane2)
        Me.grpDisplaySelector.Controls.Add(Me.radioPlane1)
        Me.grpDisplaySelector.Controls.Add(Me.radioPlane0)
        Me.grpDisplaySelector.Location = New System.Drawing.Point(340, 359)
        Me.grpDisplaySelector.Name = "grpDisplaySelector"
        Me.grpDisplaySelector.Size = New System.Drawing.Size(263, 72)
        Me.grpDisplaySelector.TabIndex = 6
        Me.grpDisplaySelector.TabStop = False
        Me.grpDisplaySelector.Text = "Display Selector"
        '
        'radioPlane2
        '
        Me.radioPlane2.AutoSize = True
        Me.radioPlane2.Location = New System.Drawing.Point(186, 31)
        Me.radioPlane2.Name = "radioPlane2"
        Me.radioPlane2.Size = New System.Drawing.Size(61, 17)
        Me.radioPlane2.TabIndex = 2
        Me.radioPlane2.TabStop = True
        Me.radioPlane2.Text = "Plane 2"
        Me.radioPlane2.UseVisualStyleBackColor = True
        '
        'radioPlane1
        '
        Me.radioPlane1.AutoSize = True
        Me.radioPlane1.Location = New System.Drawing.Point(104, 31)
        Me.radioPlane1.Name = "radioPlane1"
        Me.radioPlane1.Size = New System.Drawing.Size(61, 17)
        Me.radioPlane1.TabIndex = 1
        Me.radioPlane1.TabStop = True
        Me.radioPlane1.Text = "Plane 1"
        Me.radioPlane1.UseVisualStyleBackColor = True
        '
        'radioPlane0
        '
        Me.radioPlane0.AutoSize = True
        Me.radioPlane0.Location = New System.Drawing.Point(22, 31)
        Me.radioPlane0.Name = "radioPlane0"
        Me.radioPlane0.Size = New System.Drawing.Size(61, 17)
        Me.radioPlane0.TabIndex = 0
        Me.radioPlane0.TabStop = True
        Me.radioPlane0.Text = "Plane 0"
        Me.radioPlane0.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(653, 471)
        Me.Controls.Add(Me.grpDisplaySelector)
        Me.Controls.Add(Me.btnConvert)
        Me.Controls.Add(Me.chkConvertToHSI)
        Me.Controls.Add(Me.lblConvertedImagePlane)
        Me.Controls.Add(Me.lblOriginalImage)
        Me.Controls.Add(Me.convertedDisplay)
        Me.Controls.Add(Me.origDisplay)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.origDisplay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.convertedDisplay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDisplaySelector.ResumeLayout(False)
        Me.grpDisplaySelector.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents origDisplay As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents convertedDisplay As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents lblOriginalImage As System.Windows.Forms.Label
    Friend WithEvents lblConvertedImagePlane As System.Windows.Forms.Label
    Friend WithEvents chkConvertToHSI As System.Windows.Forms.CheckBox
    Friend WithEvents btnConvert As System.Windows.Forms.Button
    Friend WithEvents grpDisplaySelector As System.Windows.Forms.GroupBox
    Friend WithEvents radioPlane2 As System.Windows.Forms.RadioButton
    Friend WithEvents radioPlane1 As System.Windows.Forms.RadioButton
    Friend WithEvents radioPlane0 As System.Windows.Forms.RadioButton

End Class
