<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form


    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
    Me.CogRecordDisplay1 = New Cognex.VisionPro.CogRecordDisplay
    Me.CogToolBlockEditV21 = New Cognex.VisionPro.ToolBlock.CogToolBlockEditV2
    Me.radImageFile = New System.Windows.Forms.RadioButton
    Me.radCamera = New System.Windows.Forms.RadioButton
    Me.Label1 = New System.Windows.Forms.Label
    Me.Label2 = New System.Windows.Forms.Label
    Me.Label3 = New System.Windows.Forms.Label
    Me.nAreaLow = New System.Windows.Forms.NumericUpDown
    Me.nAreaHigh = New System.Windows.Forms.NumericUpDown
    Me.btnRun = New System.Windows.Forms.Button
    Me.Label4 = New System.Windows.Forms.Label
    Me.Label5 = New System.Windows.Forms.Label
    Me.Label6 = New System.Windows.Forms.Label
    Me.nPass = New System.Windows.Forms.Label
    Me.nFail = New System.Windows.Forms.Label
    CType(Me.CogRecordDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.CogToolBlockEditV21, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.nAreaLow, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.nAreaHigh, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'CogRecordDisplay1
    '
    Me.CogRecordDisplay1.Location = New System.Drawing.Point(0, 0)
    Me.CogRecordDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
    Me.CogRecordDisplay1.MouseWheelSensitivity = 1
    Me.CogRecordDisplay1.Name = "CogRecordDisplay1"
    Me.CogRecordDisplay1.OcxState = CType(resources.GetObject("CogRecordDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
    Me.CogRecordDisplay1.Size = New System.Drawing.Size(461, 383)
    Me.CogRecordDisplay1.TabIndex = 0
    '
    'CogToolBlockEditV21
    '
    Me.CogToolBlockEditV21.AllowDrop = True
    Me.CogToolBlockEditV21.ContextMenuCustomizer = Nothing
    Me.CogToolBlockEditV21.Location = New System.Drawing.Point(467, 0)
    Me.CogToolBlockEditV21.MinimumSize = New System.Drawing.Size(489, 0)
    Me.CogToolBlockEditV21.Name = "CogToolBlockEditV21"
    Me.CogToolBlockEditV21.ShowNodeToolTips = True
    Me.CogToolBlockEditV21.Size = New System.Drawing.Size(489, 377)
    Me.CogToolBlockEditV21.SuspendElectricRuns = False
    Me.CogToolBlockEditV21.TabIndex = 1
    '
    'radImageFile
    '
    Me.radImageFile.AutoSize = True
    Me.radImageFile.Checked = True
    Me.radImageFile.Location = New System.Drawing.Point(12, 454)
    Me.radImageFile.Name = "radImageFile"
    Me.radImageFile.Size = New System.Drawing.Size(73, 17)
    Me.radImageFile.TabIndex = 2
    Me.radImageFile.TabStop = True
    Me.radImageFile.Text = "Image File"
    Me.radImageFile.UseVisualStyleBackColor = True
    '
    'radCamera
    '
    Me.radCamera.AutoSize = True
    Me.radCamera.Location = New System.Drawing.Point(12, 477)
    Me.radCamera.Name = "radCamera"
    Me.radCamera.Size = New System.Drawing.Size(61, 17)
    Me.radCamera.TabIndex = 3
    Me.radCamera.Text = "Camera"
    Me.radCamera.UseVisualStyleBackColor = True
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label1.Location = New System.Drawing.Point(12, 423)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(42, 13)
    Me.Label1.TabIndex = 4
    Me.Label1.Text = "Inputs"
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Location = New System.Drawing.Point(165, 458)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(110, 13)
    Me.Label2.TabIndex = 5
    Me.Label2.Text = "Area Low Filter Value:"
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Location = New System.Drawing.Point(165, 481)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(112, 13)
    Me.Label3.TabIndex = 6
    Me.Label3.Text = "Area High Filter Value:"
    '
    'nAreaLow
    '
    Me.nAreaLow.Location = New System.Drawing.Point(299, 451)
    Me.nAreaLow.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
    Me.nAreaLow.Name = "nAreaLow"
    Me.nAreaLow.Size = New System.Drawing.Size(134, 20)
    Me.nAreaLow.TabIndex = 7
    Me.nAreaLow.Value = New Decimal(New Integer() {5050, 0, 0, 0})
    '
    'nAreaHigh
    '
    Me.nAreaHigh.Location = New System.Drawing.Point(299, 474)
    Me.nAreaHigh.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
    Me.nAreaHigh.Name = "nAreaHigh"
    Me.nAreaHigh.Size = New System.Drawing.Size(133, 20)
    Me.nAreaHigh.TabIndex = 8
    Me.nAreaHigh.Value = New Decimal(New Integer() {8050, 0, 0, 0})
    '
    'btnRun
    '
    Me.btnRun.Location = New System.Drawing.Point(478, 458)
    Me.btnRun.Name = "btnRun"
    Me.btnRun.Size = New System.Drawing.Size(98, 30)
    Me.btnRun.TabIndex = 9
    Me.btnRun.Text = "Run Once"
    Me.btnRun.UseVisualStyleBackColor = True
    '
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label4.Location = New System.Drawing.Point(609, 423)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(51, 13)
    Me.Label4.TabIndex = 10
    Me.Label4.Text = "Outputs"
    '
    'Label5
    '
    Me.Label5.AutoSize = True
    Me.Label5.Location = New System.Drawing.Point(613, 451)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(33, 13)
    Me.Label5.TabIndex = 11
    Me.Label5.Text = "Pass:"
    '
    'Label6
    '
    Me.Label6.AutoSize = True
    Me.Label6.Location = New System.Drawing.Point(613, 474)
    Me.Label6.Name = "Label6"
    Me.Label6.Size = New System.Drawing.Size(26, 13)
    Me.Label6.TabIndex = 12
    Me.Label6.Text = "Fail:"
    '
    'nPass
    '
    Me.nPass.AutoSize = True
    Me.nPass.Location = New System.Drawing.Point(683, 451)
    Me.nPass.Name = "nPass"
    Me.nPass.Size = New System.Drawing.Size(13, 13)
    Me.nPass.TabIndex = 13
    Me.nPass.Text = "0"
    '
    'nFail
    '
    Me.nFail.AutoSize = True
    Me.nFail.Location = New System.Drawing.Point(683, 474)
    Me.nFail.Name = "nFail"
    Me.nFail.Size = New System.Drawing.Size(13, 13)
    Me.nFail.TabIndex = 14
    Me.nFail.Text = "0"
    '
    'Form1
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(850, 506)
    Me.Controls.Add(Me.nFail)
    Me.Controls.Add(Me.nPass)
    Me.Controls.Add(Me.Label6)
    Me.Controls.Add(Me.Label5)
    Me.Controls.Add(Me.Label4)
    Me.Controls.Add(Me.btnRun)
    Me.Controls.Add(Me.nAreaHigh)
    Me.Controls.Add(Me.nAreaLow)
    Me.Controls.Add(Me.Label3)
    Me.Controls.Add(Me.Label2)
    Me.Controls.Add(Me.Label1)
    Me.Controls.Add(Me.radCamera)
    Me.Controls.Add(Me.radImageFile)
    Me.Controls.Add(Me.CogToolBlockEditV21)
    Me.Controls.Add(Me.CogRecordDisplay1)
    Me.Name = "Form1"
    Me.Text = "Form1"
    CType(Me.CogRecordDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.CogToolBlockEditV21, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.nAreaLow, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.nAreaHigh, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents CogRecordDisplay1 As Cognex.VisionPro.CogRecordDisplay
  Friend WithEvents CogToolBlockEditV21 As Cognex.VisionPro.ToolBlock.CogToolBlockEditV2
  Friend WithEvents radImageFile As System.Windows.Forms.RadioButton
  Friend WithEvents radCamera As System.Windows.Forms.RadioButton
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents nAreaLow As System.Windows.Forms.NumericUpDown
  Friend WithEvents nAreaHigh As System.Windows.Forms.NumericUpDown
  Friend WithEvents btnRun As System.Windows.Forms.Button
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents Label6 As System.Windows.Forms.Label
  Friend WithEvents nPass As System.Windows.Forms.Label
  Friend WithEvents nFail As System.Windows.Forms.Label

End Class
