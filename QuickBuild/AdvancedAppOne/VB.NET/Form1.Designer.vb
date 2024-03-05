<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.RunOnceButton = New System.Windows.Forms.Button
        Me.RunContCheckBox = New System.Windows.Forms.CheckBox
        Me.RunStatusTextBox = New System.Windows.Forms.TextBox
        Me.CogRecordDisplay1 = New Cognex.VisionPro.CogRecordDisplay
        Me.CogDisplayStatusBar1 = New Cognex.VisionPro.CogDisplayStatusBarV2
        Me.ShowTrainCheckBox = New System.Windows.Forms.CheckBox
        Me.RetrainButton = New System.Windows.Forms.Button
        CType(Me.CogRecordDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RunOnceButton
        '
        Me.RunOnceButton.Location = New System.Drawing.Point(23, 22)
        Me.RunOnceButton.Name = "RunOnceButton"
        Me.RunOnceButton.Size = New System.Drawing.Size(93, 23)
        Me.RunOnceButton.TabIndex = 0
        Me.RunOnceButton.Text = "Run Once"
        Me.RunOnceButton.UseVisualStyleBackColor = True
        '
        'RunContCheckBox
        '
        Me.RunContCheckBox.Appearance = System.Windows.Forms.Appearance.Button
        Me.RunContCheckBox.AutoSize = True
        Me.RunContCheckBox.Location = New System.Drawing.Point(23, 51)
        Me.RunContCheckBox.Name = "RunContCheckBox"
        Me.RunContCheckBox.Size = New System.Drawing.Size(93, 23)
        Me.RunContCheckBox.TabIndex = 1
        Me.RunContCheckBox.Text = "Run Continuous"
        Me.RunContCheckBox.UseVisualStyleBackColor = True
        '
        'RunStatusTextBox
        '
        Me.RunStatusTextBox.Location = New System.Drawing.Point(5, 347)
        Me.RunStatusTextBox.Name = "RunStatusTextBox"
        Me.RunStatusTextBox.Size = New System.Drawing.Size(522, 20)
        Me.RunStatusTextBox.TabIndex = 2
        '
        'CogRecordDisplay1
        '
        Me.CogRecordDisplay1.Location = New System.Drawing.Point(159, 22)
        Me.CogRecordDisplay1.Name = "CogRecordDisplay1"
        Me.CogRecordDisplay1.OcxState = CType(resources.GetObject("CogRecordDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.CogRecordDisplay1.Size = New System.Drawing.Size(362, 274)
        Me.CogRecordDisplay1.TabIndex = 3
        '
        'CogDisplayStatusBar1
        '
        Me.CogDisplayStatusBar1.Enabled = True
        Me.CogDisplayStatusBar1.Location = New System.Drawing.Point(159, 302)
        Me.CogDisplayStatusBar1.Name = "CogDisplayStatusBar1"
        Me.CogDisplayStatusBar1.Size = New System.Drawing.Size(362, 18)
        Me.CogDisplayStatusBar1.TabIndex = 4
        '
        'ShowTrainCheckBox
        '
        Me.ShowTrainCheckBox.Appearance = System.Windows.Forms.Appearance.Button
        Me.ShowTrainCheckBox.AutoSize = True
        Me.ShowTrainCheckBox.Location = New System.Drawing.Point(24, 244)
        Me.ShowTrainCheckBox.Name = "ShowTrainCheckBox"
        Me.ShowTrainCheckBox.Size = New System.Drawing.Size(117, 23)
        Me.ShowTrainCheckBox.TabIndex = 5
        Me.ShowTrainCheckBox.Text = "Show Training Image"
        Me.ShowTrainCheckBox.UseVisualStyleBackColor = True
        '
        'RetrainButton
        '
        Me.RetrainButton.Enabled = False
        Me.RetrainButton.Location = New System.Drawing.Point(23, 273)
        Me.RetrainButton.Name = "RetrainButton"
        Me.RetrainButton.Size = New System.Drawing.Size(117, 23)
        Me.RetrainButton.TabIndex = 6
        Me.RetrainButton.Text = "Retrain"
        Me.RetrainButton.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(539, 379)
        Me.Controls.Add(Me.RetrainButton)
        Me.Controls.Add(Me.ShowTrainCheckBox)
        Me.Controls.Add(Me.CogDisplayStatusBar1)
        Me.Controls.Add(Me.CogRecordDisplay1)
        Me.Controls.Add(Me.RunStatusTextBox)
        Me.Controls.Add(Me.RunContCheckBox)
        Me.Controls.Add(Me.RunOnceButton)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.CogRecordDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RunOnceButton As System.Windows.Forms.Button
    Friend WithEvents RunContCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents RunStatusTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CogRecordDisplay1 As Cognex.VisionPro.CogRecordDisplay
    Friend WithEvents CogDisplayStatusBar1 As Cognex.VisionPro.CogDisplayStatusBarV2
    Friend WithEvents ShowTrainCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents RetrainButton As System.Windows.Forms.Button

End Class
