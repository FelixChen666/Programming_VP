<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormCompositeColorMatch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormCompositeColorMatch))
        Me.CogDisplayInputImage = New Cognex.VisionPro.Display.CogDisplay
        Me.LabelColor = New System.Windows.Forms.Label
        Me.LabelScore = New System.Windows.Forms.Label
        Me.TextBoxScore1 = New System.Windows.Forms.TextBox
        Me.TextBoxScore2 = New System.Windows.Forms.TextBox
        Me.TextBoxScore3 = New System.Windows.Forms.TextBox
        Me.TextBoxScore4 = New System.Windows.Forms.TextBox
        Me.TextBoxDescription = New System.Windows.Forms.TextBox
        Me.ButtonRun = New System.Windows.Forms.Button
        Me.CheckBoxNormalizeIntensity = New System.Windows.Forms.CheckBox
        Me.CogDisplayColor4 = New Cognex.VisionPro.Display.CogDisplay
        Me.CogDisplayColor3 = New Cognex.VisionPro.Display.CogDisplay
        Me.CogDisplayColor2 = New Cognex.VisionPro.Display.CogDisplay
        Me.CogDisplayColor1 = New Cognex.VisionPro.Display.CogDisplay
        Me.CogDisplayColor5 = New Cognex.VisionPro.Display.CogDisplay
        Me.TextBoxScore5 = New System.Windows.Forms.TextBox
        Me.CogDisplayColor6 = New Cognex.VisionPro.Display.CogDisplay
        Me.TextBoxScore6 = New System.Windows.Forms.TextBox
        Me.CogDisplayColor7 = New Cognex.VisionPro.Display.CogDisplay
        Me.TextBoxScore7 = New System.Windows.Forms.TextBox
        CType(Me.CogDisplayInputImage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CogDisplayColor4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CogDisplayColor3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CogDisplayColor2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CogDisplayColor1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CogDisplayColor5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CogDisplayColor6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CogDisplayColor7, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CogDisplayInputImage
        '
        Me.CogDisplayInputImage.Location = New System.Drawing.Point(34, 21)
        Me.CogDisplayInputImage.Name = "CogDisplayInputImage"
        Me.CogDisplayInputImage.OcxState = CType(resources.GetObject("CogDisplayInputImage.OcxState"), System.Windows.Forms.AxHost.State)
        Me.CogDisplayInputImage.Size = New System.Drawing.Size(324, 285)
        Me.CogDisplayInputImage.TabIndex = 0
        '
        'LabelColor
        '
        Me.LabelColor.AutoSize = True
        Me.LabelColor.Location = New System.Drawing.Point(449, 6)
        Me.LabelColor.Name = "LabelColor"
        Me.LabelColor.Size = New System.Drawing.Size(31, 13)
        Me.LabelColor.TabIndex = 1
        Me.LabelColor.Text = "Color"
        '
        'LabelScore
        '
        Me.LabelScore.AutoSize = True
        Me.LabelScore.Location = New System.Drawing.Point(579, 6)
        Me.LabelScore.Name = "LabelScore"
        Me.LabelScore.Size = New System.Drawing.Size(35, 13)
        Me.LabelScore.TabIndex = 2
        Me.LabelScore.Text = "Score"
        '
        'TextBoxScore1
        '
        Me.TextBoxScore1.Location = New System.Drawing.Point(559, 41)
        Me.TextBoxScore1.Name = "TextBoxScore1"
        Me.TextBoxScore1.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxScore1.TabIndex = 3
        '
        'TextBoxScore2
        '
        Me.TextBoxScore2.Location = New System.Drawing.Point(559, 82)
        Me.TextBoxScore2.Name = "TextBoxScore2"
        Me.TextBoxScore2.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxScore2.TabIndex = 4
        '
        'TextBoxScore3
        '
        Me.TextBoxScore3.Location = New System.Drawing.Point(559, 123)
        Me.TextBoxScore3.Name = "TextBoxScore3"
        Me.TextBoxScore3.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxScore3.TabIndex = 5
        '
        'TextBoxScore4
        '
        Me.TextBoxScore4.Location = New System.Drawing.Point(559, 164)
        Me.TextBoxScore4.Name = "TextBoxScore4"
        Me.TextBoxScore4.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxScore4.TabIndex = 6
        '
        'TextBoxDescription
        '
        Me.TextBoxDescription.Location = New System.Drawing.Point(34, 384)
        Me.TextBoxDescription.Name = "TextBoxDescription"
        Me.TextBoxDescription.Size = New System.Drawing.Size(604, 20)
        Me.TextBoxDescription.TabIndex = 7
        Me.TextBoxDescription.Text = "Sample Description: runs a CogCompositeColorMatchTool and gets the match scores."
        '
        'ButtonRun
        '
        Me.ButtonRun.Location = New System.Drawing.Point(559, 335)
        Me.ButtonRun.Name = "ButtonRun"
        Me.ButtonRun.Size = New System.Drawing.Size(81, 30)
        Me.ButtonRun.TabIndex = 12
        Me.ButtonRun.Text = "Run"
        Me.ButtonRun.UseVisualStyleBackColor = True
        '
        'CheckBoxNormalizeIntensity
        '
        Me.CheckBoxNormalizeIntensity.AutoSize = True
        Me.CheckBoxNormalizeIntensity.Location = New System.Drawing.Point(437, 343)
        Me.CheckBoxNormalizeIntensity.Name = "CheckBoxNormalizeIntensity"
        Me.CheckBoxNormalizeIntensity.Size = New System.Drawing.Size(114, 17)
        Me.CheckBoxNormalizeIntensity.TabIndex = 13
        Me.CheckBoxNormalizeIntensity.Text = "Normalize Intensity"
        Me.CheckBoxNormalizeIntensity.UseVisualStyleBackColor = True
        '
        'CogDisplayColor4
        '
        Me.CogDisplayColor4.Location = New System.Drawing.Point(437, 157)
        Me.CogDisplayColor4.Name = "CogDisplayColor4"
        Me.CogDisplayColor4.OcxState = CType(resources.GetObject("CogDisplayColor4.OcxState"), System.Windows.Forms.AxHost.State)
        Me.CogDisplayColor4.Size = New System.Drawing.Size(102, 34)
        Me.CogDisplayColor4.TabIndex = 21
        '
        'CogDisplayColor3
        '
        Me.CogDisplayColor3.Location = New System.Drawing.Point(437, 115)
        Me.CogDisplayColor3.Name = "CogDisplayColor3"
        Me.CogDisplayColor3.OcxState = CType(resources.GetObject("CogDisplayColor3.OcxState"), System.Windows.Forms.AxHost.State)
        Me.CogDisplayColor3.Size = New System.Drawing.Size(102, 34)
        Me.CogDisplayColor3.TabIndex = 20
        '
        'CogDisplayColor2
        '
        Me.CogDisplayColor2.Location = New System.Drawing.Point(437, 77)
        Me.CogDisplayColor2.Name = "CogDisplayColor2"
        Me.CogDisplayColor2.OcxState = CType(resources.GetObject("CogDisplayColor2.OcxState"), System.Windows.Forms.AxHost.State)
        Me.CogDisplayColor2.Size = New System.Drawing.Size(102, 30)
        Me.CogDisplayColor2.TabIndex = 19
        '
        'CogDisplayColor1
        '
        Me.CogDisplayColor1.Location = New System.Drawing.Point(437, 35)
        Me.CogDisplayColor1.Name = "CogDisplayColor1"
        Me.CogDisplayColor1.OcxState = CType(resources.GetObject("CogDisplayColor1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.CogDisplayColor1.Size = New System.Drawing.Size(102, 34)
        Me.CogDisplayColor1.TabIndex = 18
        '
        'CogDisplayColor5
        '
        Me.CogDisplayColor5.Location = New System.Drawing.Point(437, 199)
        Me.CogDisplayColor5.Name = "CogDisplayColor5"
        Me.CogDisplayColor5.OcxState = CType(resources.GetObject("CogDisplayColor5.OcxState"), System.Windows.Forms.AxHost.State)
        Me.CogDisplayColor5.Size = New System.Drawing.Size(102, 34)
        Me.CogDisplayColor5.TabIndex = 23
        '
        'TextBoxScore5
        '
        Me.TextBoxScore5.Location = New System.Drawing.Point(559, 205)
        Me.TextBoxScore5.Name = "TextBoxScore5"
        Me.TextBoxScore5.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxScore5.TabIndex = 22
        '
        'CogDisplayColor6
        '
        Me.CogDisplayColor6.Location = New System.Drawing.Point(437, 241)
        Me.CogDisplayColor6.Name = "CogDisplayColor6"
        Me.CogDisplayColor6.OcxState = CType(resources.GetObject("CogDisplayColor6.OcxState"), System.Windows.Forms.AxHost.State)
        Me.CogDisplayColor6.Size = New System.Drawing.Size(102, 34)
        Me.CogDisplayColor6.TabIndex = 25
        '
        'TextBoxScore6
        '
        Me.TextBoxScore6.Location = New System.Drawing.Point(559, 246)
        Me.TextBoxScore6.Name = "TextBoxScore6"
        Me.TextBoxScore6.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxScore6.TabIndex = 24
        '
        'CogDisplayColor7
        '
        Me.CogDisplayColor7.Location = New System.Drawing.Point(437, 283)
        Me.CogDisplayColor7.Name = "CogDisplayColor7"
        Me.CogDisplayColor7.OcxState = CType(resources.GetObject("CogDisplayColor7.OcxState"), System.Windows.Forms.AxHost.State)
        Me.CogDisplayColor7.Size = New System.Drawing.Size(102, 34)
        Me.CogDisplayColor7.TabIndex = 27
        '
        'TextBoxScore7
        '
        Me.TextBoxScore7.Location = New System.Drawing.Point(559, 287)
        Me.TextBoxScore7.Name = "TextBoxScore7"
        Me.TextBoxScore7.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxScore7.TabIndex = 26
        '
        'FormCompositeColorMatch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(651, 416)
        Me.Controls.Add(Me.CogDisplayColor7)
        Me.Controls.Add(Me.TextBoxScore7)
        Me.Controls.Add(Me.CogDisplayColor6)
        Me.Controls.Add(Me.TextBoxScore6)
        Me.Controls.Add(Me.CogDisplayColor5)
        Me.Controls.Add(Me.TextBoxScore5)
        Me.Controls.Add(Me.CogDisplayColor4)
        Me.Controls.Add(Me.CogDisplayColor3)
        Me.Controls.Add(Me.CogDisplayColor2)
        Me.Controls.Add(Me.CogDisplayColor1)
        Me.Controls.Add(Me.CheckBoxNormalizeIntensity)
        Me.Controls.Add(Me.ButtonRun)
        Me.Controls.Add(Me.TextBoxDescription)
        Me.Controls.Add(Me.TextBoxScore4)
        Me.Controls.Add(Me.TextBoxScore3)
        Me.Controls.Add(Me.TextBoxScore2)
        Me.Controls.Add(Me.TextBoxScore1)
        Me.Controls.Add(Me.LabelScore)
        Me.Controls.Add(Me.LabelColor)
        Me.Controls.Add(Me.CogDisplayInputImage)
        Me.Name = "FormCompositeColorMatch"
        Me.Text = "Composite Color Match"
        CType(Me.CogDisplayInputImage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CogDisplayColor4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CogDisplayColor3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CogDisplayColor2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CogDisplayColor1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CogDisplayColor5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CogDisplayColor6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CogDisplayColor7, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CogDisplayInputImage As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents LabelColor As System.Windows.Forms.Label
    Friend WithEvents LabelScore As System.Windows.Forms.Label
    Friend WithEvents TextBoxScore1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxScore2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxScore3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxScore4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxDescription As System.Windows.Forms.TextBox
    Friend WithEvents ButtonRun As System.Windows.Forms.Button
    Friend WithEvents CheckBoxNormalizeIntensity As System.Windows.Forms.CheckBox
    Friend WithEvents CogDisplayColor4 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents CogDisplayColor3 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents CogDisplayColor2 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents CogDisplayColor1 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents CogDisplayColor5 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents TextBoxScore5 As System.Windows.Forms.TextBox
    Friend WithEvents CogDisplayColor6 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents TextBoxScore6 As System.Windows.Forms.TextBox
    Friend WithEvents CogDisplayColor7 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents TextBoxScore7 As System.Windows.Forms.TextBox

End Class
