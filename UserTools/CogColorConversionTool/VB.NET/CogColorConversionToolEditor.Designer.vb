Namespace CogColorConversionTool
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class CogColorConversionToolEditor
        Inherits Cognex.VisionPro.CogToolEditControlBaseV2

        'UserControl overrides dispose to clean up the component list.
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CogColorConversionToolEditor))
            Me.chkConvertToHSI = New System.Windows.Forms.CheckBox
            Me.tpgRegion = New System.Windows.Forms.TabPage
            Me.cboRegionShape = New System.Windows.Forms.ComboBox
            Me.lblRegionShape = New System.Windows.Forms.Label
            CType(Me.sbpIcon, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.sbpStatusCode, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.sbpStatusMessage, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.sbpProcessingTime, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.sbpTotalTime, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabControl.SuspendLayout()
            Me.tpgSettings.SuspendLayout()
            Me.tpgRegion.SuspendLayout()
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
            Me.tabControl.Controls.Add(Me.tpgRegion)
            Me.tabControl.Size = New System.Drawing.Size(489, 484)
            Me.tabControl.Controls.SetChildIndex(Me.tpgRegion, 0)
            Me.tabControl.Controls.SetChildIndex(Me.tpgSettings, 0)
            '
            'tpgSettings
            '
            Me.tpgSettings.Controls.Add(Me.chkConvertToHSI)
            Me.tpgSettings.Size = New System.Drawing.Size(481, 458)
            Me.tpgSettings.UseVisualStyleBackColor = True
            '
            'lblControlState
            '
            Me.lblControlState.Location = New System.Drawing.Point(8, 487)
            '
            'chkConvertToHSI
            '
            Me.chkConvertToHSI.AutoSize = True
            Me.chkConvertToHSI.Location = New System.Drawing.Point(27, 25)
            Me.chkConvertToHSI.Name = "chkConvertToHSI"
            Me.chkConvertToHSI.Size = New System.Drawing.Size(96, 17)
            Me.chkConvertToHSI.TabIndex = 0
            Me.chkConvertToHSI.Text = "Convert to HSI"
            Me.chkConvertToHSI.UseVisualStyleBackColor = True
            '
            'tpgRegion
            '
            Me.tpgRegion.Controls.Add(Me.cboRegionShape)
            Me.tpgRegion.Controls.Add(Me.lblRegionShape)
            Me.tpgRegion.Location = New System.Drawing.Point(4, 22)
            Me.tpgRegion.Name = "tpgRegion"
            Me.tpgRegion.Size = New System.Drawing.Size(481, 458)
            Me.tpgRegion.TabIndex = 1
            Me.tpgRegion.Text = "Region"
            Me.tpgRegion.UseVisualStyleBackColor = True
            '
            'cboRegionShape
            '
            Me.cboRegionShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboRegionShape.FormattingEnabled = True
            Me.cboRegionShape.Items.AddRange(New Object() {"CogCircle", "CogRectangle", "<None = Use Entire Image>"})
            Me.cboRegionShape.Location = New System.Drawing.Point(30, 59)
            Me.cboRegionShape.Name = "cboRegionShape"
            Me.cboRegionShape.Size = New System.Drawing.Size(197, 21)
            Me.cboRegionShape.TabIndex = 1
            '
            'lblRegionShape
            '
            Me.lblRegionShape.AutoSize = True
            Me.lblRegionShape.Location = New System.Drawing.Point(27, 25)
            Me.lblRegionShape.Name = "lblRegionShape"
            Me.lblRegionShape.Size = New System.Drawing.Size(75, 13)
            Me.lblRegionShape.TabIndex = 0
            Me.lblRegionShape.Text = "Region Shape"
            '
            'CogColorConversionToolEditor
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Name = "CogColorConversionToolEditor"
            CType(Me.sbpIcon, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.sbpStatusCode, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.sbpStatusMessage, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.sbpProcessingTime, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.sbpTotalTime, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabControl.ResumeLayout(False)
            Me.tpgSettings.ResumeLayout(False)
            Me.tpgSettings.PerformLayout()
            Me.tpgRegion.ResumeLayout(False)
            Me.tpgRegion.PerformLayout()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents chkConvertToHSI As System.Windows.Forms.CheckBox
        Friend WithEvents tpgRegion As System.Windows.Forms.TabPage
        Friend WithEvents cboRegionShape As System.Windows.Forms.ComboBox
        Friend WithEvents lblRegionShape As System.Windows.Forms.Label

    End Class
End Namespace
