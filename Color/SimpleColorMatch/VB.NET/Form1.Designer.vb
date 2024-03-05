<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormColorMatch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormColorMatch))
        Me.CogDisplayInputImage = New Cognex.VisionPro.Display.CogDisplay
        Me.LabelColor = New System.Windows.Forms.Label
        Me.LabelScore = New System.Windows.Forms.Label
        Me.TextBoxScore1 = New System.Windows.Forms.TextBox
        Me.TextBoxScore2 = New System.Windows.Forms.TextBox
        Me.TextBoxScore3 = New System.Windows.Forms.TextBox
        Me.TextBoxScore4 = New System.Windows.Forms.TextBox
        Me.TextBoxDescription = New System.Windows.Forms.TextBox
        Me.ButtonRun = New System.Windows.Forms.Button
        Me.CheckBoxHueOnly = New System.Windows.Forms.CheckBox
        Me.TextBoxColor4 = New System.Windows.Forms.TextBox
        Me.TextBoxColor3 = New System.Windows.Forms.TextBox
        Me.TextBoxColor2 = New System.Windows.Forms.TextBox
        Me.TextBoxColor1 = New System.Windows.Forms.TextBox
        Me.TextBoxColor5 = New System.Windows.Forms.TextBox
        Me.TextBoxScore5 = New System.Windows.Forms.TextBox
        CType(Me.CogDisplayInputImage, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.LabelColor.Location = New System.Drawing.Point(449, 31)
        Me.LabelColor.Name = "LabelColor"
        Me.LabelColor.Size = New System.Drawing.Size(31, 13)
        Me.LabelColor.TabIndex = 1
        Me.LabelColor.Text = "Color"
        '
        'LabelScore
        '
        Me.LabelScore.AutoSize = True
        Me.LabelScore.Location = New System.Drawing.Point(579, 31)
        Me.LabelScore.Name = "LabelScore"
        Me.LabelScore.Size = New System.Drawing.Size(35, 13)
        Me.LabelScore.TabIndex = 2
        Me.LabelScore.Text = "Score"
        '
        'TextBoxScore1
        '
        Me.TextBoxScore1.Location = New System.Drawing.Point(559, 69)
        Me.TextBoxScore1.Name = "TextBoxScore1"
        Me.TextBoxScore1.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxScore1.TabIndex = 3
        '
        'TextBoxScore2
        '
        Me.TextBoxScore2.Location = New System.Drawing.Point(559, 113)
        Me.TextBoxScore2.Name = "TextBoxScore2"
        Me.TextBoxScore2.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxScore2.TabIndex = 4
        '
        'TextBoxScore3
        '
        Me.TextBoxScore3.Location = New System.Drawing.Point(559, 156)
        Me.TextBoxScore3.Name = "TextBoxScore3"
        Me.TextBoxScore3.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxScore3.TabIndex = 5
        '
        'TextBoxScore4
        '
        Me.TextBoxScore4.Location = New System.Drawing.Point(559, 202)
        Me.TextBoxScore4.Name = "TextBoxScore4"
        Me.TextBoxScore4.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxScore4.TabIndex = 6
        '
        'TextBoxDescription
        '
        Me.TextBoxDescription.Location = New System.Drawing.Point(34, 338)
        Me.TextBoxDescription.Name = "TextBoxDescription"
        Me.TextBoxDescription.Size = New System.Drawing.Size(604, 20)
        Me.TextBoxDescription.TabIndex = 7
        Me.TextBoxDescription.Text = "Sample Description: runs a CogColorMatchTool and gets the match scores."
        '
        'ButtonRun
        '
        Me.ButtonRun.Location = New System.Drawing.Point(557, 285)
        Me.ButtonRun.Name = "ButtonRun"
        Me.ButtonRun.Size = New System.Drawing.Size(81, 30)
        Me.ButtonRun.TabIndex = 12
        Me.ButtonRun.Text = "Run"
        Me.ButtonRun.UseVisualStyleBackColor = True
        '
        'CheckBoxHueOnly
        '
        Me.CheckBoxHueOnly.AutoSize = True
        Me.CheckBoxHueOnly.Location = New System.Drawing.Point(435, 293)
        Me.CheckBoxHueOnly.Name = "CheckBoxHueOnly"
        Me.CheckBoxHueOnly.Size = New System.Drawing.Size(70, 17)
        Me.CheckBoxHueOnly.TabIndex = 13
        Me.CheckBoxHueOnly.Text = "Hue Only"
        Me.CheckBoxHueOnly.UseVisualStyleBackColor = True
        '
        'TextBoxColor4
        '
        Me.TextBoxColor4.Location = New System.Drawing.Point(435, 202)
        Me.TextBoxColor4.Name = "TextBoxColor4"
        Me.TextBoxColor4.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxColor4.TabIndex = 17
        '
        'TextBoxColor3
        '
        Me.TextBoxColor3.Location = New System.Drawing.Point(435, 156)
        Me.TextBoxColor3.Name = "TextBoxColor3"
        Me.TextBoxColor3.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxColor3.TabIndex = 16
        '
        'TextBoxColor2
        '
        Me.TextBoxColor2.Location = New System.Drawing.Point(435, 113)
        Me.TextBoxColor2.Name = "TextBoxColor2"
        Me.TextBoxColor2.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxColor2.TabIndex = 15
        '
        'TextBoxColor1
        '
        Me.TextBoxColor1.Location = New System.Drawing.Point(435, 69)
        Me.TextBoxColor1.Name = "TextBoxColor1"
        Me.TextBoxColor1.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxColor1.TabIndex = 14
        '
        'TextBoxColor5
        '
        Me.TextBoxColor5.Location = New System.Drawing.Point(435, 243)
        Me.TextBoxColor5.Name = "TextBoxColor5"
        Me.TextBoxColor5.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxColor5.TabIndex = 19
        '
        'TextBoxScore5
        '
        Me.TextBoxScore5.Location = New System.Drawing.Point(559, 243)
        Me.TextBoxScore5.Name = "TextBoxScore5"
        Me.TextBoxScore5.Size = New System.Drawing.Size(79, 20)
        Me.TextBoxScore5.TabIndex = 18
        '
        'FormColorMatch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(667, 393)
        Me.Controls.Add(Me.TextBoxColor5)
        Me.Controls.Add(Me.TextBoxScore5)
        Me.Controls.Add(Me.TextBoxColor4)
        Me.Controls.Add(Me.TextBoxColor3)
        Me.Controls.Add(Me.TextBoxColor2)
        Me.Controls.Add(Me.TextBoxColor1)
        Me.Controls.Add(Me.CheckBoxHueOnly)
        Me.Controls.Add(Me.ButtonRun)
        Me.Controls.Add(Me.TextBoxDescription)
        Me.Controls.Add(Me.TextBoxScore4)
        Me.Controls.Add(Me.TextBoxScore3)
        Me.Controls.Add(Me.TextBoxScore2)
        Me.Controls.Add(Me.TextBoxScore1)
        Me.Controls.Add(Me.LabelScore)
        Me.Controls.Add(Me.LabelColor)
        Me.Controls.Add(Me.CogDisplayInputImage)
        Me.Name = "FormColorMatch"
        Me.Text = "Color Match"
        CType(Me.CogDisplayInputImage, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents CheckBoxHueOnly As System.Windows.Forms.CheckBox
    Friend WithEvents TextBoxColor4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxColor3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxColor2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxColor1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxColor5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxScore5 As System.Windows.Forms.TextBox

End Class
