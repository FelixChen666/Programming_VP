<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ColorImageSegmenter
    Inherits System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ColorImageSegmenter))
        Me.CogDisplayInputImage = New Cognex.VisionPro.Display.CogDisplay
        Me.CogDisplaySegmentImage = New Cognex.VisionPro.Display.CogDisplay
        Me.LabelInputImage = New System.Windows.Forms.Label
        Me.LabelSegmentImage = New System.Windows.Forms.Label
        Me.TextBoxSampleDescription = New System.Windows.Forms.TextBox
        Me.ButtonRun = New System.Windows.Forms.Button
        Me.CheckBoxYellow = New System.Windows.Forms.CheckBox
        Me.CheckBoxRed = New System.Windows.Forms.CheckBox
        CType(Me.CogDisplayInputImage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CogDisplaySegmentImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CogDisplayInputImage
        '
        Me.CogDisplayInputImage.Location = New System.Drawing.Point(34, 55)
        Me.CogDisplayInputImage.Name = "CogDisplayInputImage"
        Me.CogDisplayInputImage.OcxState = CType(resources.GetObject("CogDisplayInputImage.OcxState"), System.Windows.Forms.AxHost.State)
        Me.CogDisplayInputImage.Size = New System.Drawing.Size(286, 252)
        Me.CogDisplayInputImage.TabIndex = 0
        '
        'CogDisplaySegmentImage
        '
        Me.CogDisplaySegmentImage.Location = New System.Drawing.Point(337, 55)
        Me.CogDisplaySegmentImage.Name = "CogDisplaySegmentImage"
        Me.CogDisplaySegmentImage.OcxState = CType(resources.GetObject("CogDisplaySegmentImage.OcxState"), System.Windows.Forms.AxHost.State)
        Me.CogDisplaySegmentImage.Size = New System.Drawing.Size(272, 252)
        Me.CogDisplaySegmentImage.TabIndex = 1
        '
        'LabelInputImage
        '
        Me.LabelInputImage.AutoSize = True
        Me.LabelInputImage.Location = New System.Drawing.Point(100, 21)
        Me.LabelInputImage.Name = "LabelInputImage"
        Me.LabelInputImage.Size = New System.Drawing.Size(63, 13)
        Me.LabelInputImage.TabIndex = 2
        Me.LabelInputImage.Text = "Input Image"
        '
        'LabelSegmentImage
        '
        Me.LabelSegmentImage.AutoSize = True
        Me.LabelSegmentImage.Location = New System.Drawing.Point(379, 21)
        Me.LabelSegmentImage.Name = "LabelSegmentImage"
        Me.LabelSegmentImage.Size = New System.Drawing.Size(93, 13)
        Me.LabelSegmentImage.TabIndex = 3
        Me.LabelSegmentImage.Text = "Segmented Image"
        '
        'TextBoxSampleDescription
        '
        Me.TextBoxSampleDescription.Location = New System.Drawing.Point(46, 351)
        Me.TextBoxSampleDescription.Name = "TextBoxSampleDescription"
        Me.TextBoxSampleDescription.Size = New System.Drawing.Size(551, 20)
        Me.TextBoxSampleDescription.TabIndex = 4
        Me.TextBoxSampleDescription.Text = "Sample Description: runs a CogColorSegmenterTool and gets the segmentation result" & _
            "."
        '
        'ButtonRun
        '
        Me.ButtonRun.Location = New System.Drawing.Point(632, 251)
        Me.ButtonRun.Name = "ButtonRun"
        Me.ButtonRun.Size = New System.Drawing.Size(74, 24)
        Me.ButtonRun.TabIndex = 5
        Me.ButtonRun.Text = "Run"
        Me.ButtonRun.UseVisualStyleBackColor = True
        '
        'CheckBoxYellow
        '
        Me.CheckBoxYellow.AutoSize = True
        Me.CheckBoxYellow.Checked = True
        Me.CheckBoxYellow.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxYellow.Location = New System.Drawing.Point(632, 138)
        Me.CheckBoxYellow.Name = "CheckBoxYellow"
        Me.CheckBoxYellow.Size = New System.Drawing.Size(57, 17)
        Me.CheckBoxYellow.TabIndex = 6
        Me.CheckBoxYellow.Text = "Yellow"
        Me.CheckBoxYellow.UseVisualStyleBackColor = True
        '
        'CheckBoxRed
        '
        Me.CheckBoxRed.AutoSize = True
        Me.CheckBoxRed.Location = New System.Drawing.Point(632, 194)
        Me.CheckBoxRed.Name = "CheckBoxRed"
        Me.CheckBoxRed.Size = New System.Drawing.Size(46, 17)
        Me.CheckBoxRed.TabIndex = 7
        Me.CheckBoxRed.Text = "Red"
        Me.CheckBoxRed.UseVisualStyleBackColor = True
        '
        'ColorImageSegmenter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(718, 433)
        Me.Controls.Add(Me.CheckBoxRed)
        Me.Controls.Add(Me.CheckBoxYellow)
        Me.Controls.Add(Me.ButtonRun)
        Me.Controls.Add(Me.TextBoxSampleDescription)
        Me.Controls.Add(Me.LabelSegmentImage)
        Me.Controls.Add(Me.LabelInputImage)
        Me.Controls.Add(Me.CogDisplaySegmentImage)
        Me.Controls.Add(Me.CogDisplayInputImage)
        Me.Name = "ColorImageSegmenter"
        Me.Text = "Color Image Segmenter"
        CType(Me.CogDisplayInputImage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CogDisplaySegmentImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CogDisplayInputImage As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents CogDisplaySegmentImage As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents LabelInputImage As System.Windows.Forms.Label
    Friend WithEvents LabelSegmentImage As System.Windows.Forms.Label
    Friend WithEvents TextBoxSampleDescription As System.Windows.Forms.TextBox
    Friend WithEvents ButtonRun As System.Windows.Forms.Button
    Friend WithEvents CheckBoxYellow As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxRed As System.Windows.Forms.CheckBox

End Class
