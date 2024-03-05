<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UserGradingForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UserGradingForm))
        Me.mInputDatabaseLabel = New System.Windows.Forms.Label
        Me.mVerificationResultLabel = New System.Windows.Forms.Label
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.btnClear = New System.Windows.Forms.Button
        Me.cogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.cboGrades = New System.Windows.Forms.ComboBox
        Me.nudRCOMY = New System.Windows.Forms.NumericUpDown
        Me.nudCOMY = New System.Windows.Forms.NumericUpDown
        Me.nudRCOMX = New System.Windows.Forms.NumericUpDown
        Me.nudCOMX = New System.Windows.Forms.NumericUpDown
        Me.nudRArea = New System.Windows.Forms.NumericUpDown
        Me.nudArea = New System.Windows.Forms.NumericUpDown
        Me.scrlImages = New System.Windows.Forms.HScrollBar
        Me.btnMeasure = New System.Windows.Forms.Button
        Me.label10 = New System.Windows.Forms.Label
        Me.label7 = New System.Windows.Forms.Label
        Me.label6 = New System.Windows.Forms.Label
        Me.label5 = New System.Windows.Forms.Label
        Me.label4 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.lblRecordName = New System.Windows.Forms.Label
        Me.groupBox3 = New System.Windows.Forms.GroupBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.groupBox1.SuspendLayout()
        CType(Me.cogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox2.SuspendLayout()
        CType(Me.nudRCOMY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCOMY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRCOMX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCOMX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRArea, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudArea, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'mInputDatabaseLabel
        '
        Me.mInputDatabaseLabel.Location = New System.Drawing.Point(6, 21)
        Me.mInputDatabaseLabel.Name = "mInputDatabaseLabel"
        Me.mInputDatabaseLabel.Size = New System.Drawing.Size(119, 18)
        Me.mInputDatabaseLabel.TabIndex = 1
        Me.mInputDatabaseLabel.Text = "Input Database: "
        '
        'mVerificationResultLabel
        '
        Me.mVerificationResultLabel.AutoSize = True
        Me.mVerificationResultLabel.BackColor = System.Drawing.SystemColors.Control
        Me.mVerificationResultLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mVerificationResultLabel.Location = New System.Drawing.Point(93, 212)
        Me.mVerificationResultLabel.Name = "mVerificationResultLabel"
        Me.mVerificationResultLabel.Size = New System.Drawing.Size(0, 42)
        Me.mVerificationResultLabel.TabIndex = 4
        '
        'groupBox1
        '
        Me.groupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.groupBox1.Controls.Add(Me.btnClear)
        Me.groupBox1.Controls.Add(Me.mInputDatabaseLabel)
        Me.groupBox1.Location = New System.Drawing.Point(357, 12)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(289, 72)
        Me.groupBox1.TabIndex = 5
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Input Database"
        '
        'btnClear
        '
        Me.btnClear.Enabled = False
        Me.btnClear.Location = New System.Drawing.Point(9, 42)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(272, 23)
        Me.btnClear.TabIndex = 3
        Me.btnClear.Text = "Restore database"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'cogDisplay1
        '
        Me.cogDisplay1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cogDisplay1.Location = New System.Drawing.Point(12, 12)
        Me.cogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1
        Me.cogDisplay1.MouseWheelSensitivity = 1
        Me.cogDisplay1.Name = "cogDisplay1"
        Me.cogDisplay1.OcxState = CType(resources.GetObject("cogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.cogDisplay1.Size = New System.Drawing.Size(339, 315)
        Me.cogDisplay1.TabIndex = 13
        '
        'groupBox2
        '
        Me.groupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.groupBox2.Controls.Add(Me.cboGrades)
        Me.groupBox2.Controls.Add(Me.nudRCOMY)
        Me.groupBox2.Controls.Add(Me.nudCOMY)
        Me.groupBox2.Controls.Add(Me.nudRCOMX)
        Me.groupBox2.Controls.Add(Me.nudCOMX)
        Me.groupBox2.Controls.Add(Me.nudRArea)
        Me.groupBox2.Controls.Add(Me.nudArea)
        Me.groupBox2.Controls.Add(Me.scrlImages)
        Me.groupBox2.Controls.Add(Me.btnMeasure)
        Me.groupBox2.Controls.Add(Me.label10)
        Me.groupBox2.Controls.Add(Me.label7)
        Me.groupBox2.Controls.Add(Me.label6)
        Me.groupBox2.Controls.Add(Me.label5)
        Me.groupBox2.Controls.Add(Me.label4)
        Me.groupBox2.Controls.Add(Me.label3)
        Me.groupBox2.Controls.Add(Me.label2)
        Me.groupBox2.Controls.Add(Me.lblRecordName)
        Me.groupBox2.Location = New System.Drawing.Point(357, 89)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(289, 237)
        Me.groupBox2.TabIndex = 12
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Record controls"
        '
        'cboGrades
        '
        Me.cboGrades.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGrades.FormattingEnabled = True
        Me.cboGrades.Location = New System.Drawing.Point(120, 179)
        Me.cboGrades.Name = "cboGrades"
        Me.cboGrades.Size = New System.Drawing.Size(162, 21)
        Me.cboGrades.TabIndex = 42
        '
        'nudRCOMY
        '
        Me.nudRCOMY.DecimalPlaces = 2
        Me.nudRCOMY.Location = New System.Drawing.Point(233, 149)
        Me.nudRCOMY.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
        Me.nudRCOMY.Name = "nudRCOMY"
        Me.nudRCOMY.Size = New System.Drawing.Size(49, 20)
        Me.nudRCOMY.TabIndex = 41
        Me.nudRCOMY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudRCOMY.ThousandsSeparator = True
        '
        'nudCOMY
        '
        Me.nudCOMY.DecimalPlaces = 2
        Me.nudCOMY.Location = New System.Drawing.Point(120, 149)
        Me.nudCOMY.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
        Me.nudCOMY.Name = "nudCOMY"
        Me.nudCOMY.Size = New System.Drawing.Size(93, 20)
        Me.nudCOMY.TabIndex = 40
        Me.nudCOMY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudCOMY.ThousandsSeparator = True
        '
        'nudRCOMX
        '
        Me.nudRCOMX.DecimalPlaces = 2
        Me.nudRCOMX.Location = New System.Drawing.Point(233, 117)
        Me.nudRCOMX.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
        Me.nudRCOMX.Name = "nudRCOMX"
        Me.nudRCOMX.Size = New System.Drawing.Size(49, 20)
        Me.nudRCOMX.TabIndex = 39
        Me.nudRCOMX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudRCOMX.ThousandsSeparator = True
        '
        'nudCOMX
        '
        Me.nudCOMX.DecimalPlaces = 2
        Me.nudCOMX.Location = New System.Drawing.Point(120, 117)
        Me.nudCOMX.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
        Me.nudCOMX.Name = "nudCOMX"
        Me.nudCOMX.Size = New System.Drawing.Size(93, 20)
        Me.nudCOMX.TabIndex = 38
        Me.nudCOMX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudCOMX.ThousandsSeparator = True
        '
        'nudRArea
        '
        Me.nudRArea.DecimalPlaces = 2
        Me.nudRArea.Location = New System.Drawing.Point(232, 85)
        Me.nudRArea.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
        Me.nudRArea.Name = "nudRArea"
        Me.nudRArea.Size = New System.Drawing.Size(49, 20)
        Me.nudRArea.TabIndex = 37
        Me.nudRArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudRArea.ThousandsSeparator = True
        '
        'nudArea
        '
        Me.nudArea.DecimalPlaces = 2
        Me.nudArea.Location = New System.Drawing.Point(120, 85)
        Me.nudArea.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
        Me.nudArea.Name = "nudArea"
        Me.nudArea.Size = New System.Drawing.Size(93, 20)
        Me.nudArea.TabIndex = 36
        Me.nudArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudArea.ThousandsSeparator = True
        '
        'scrlImages
        '
        Me.scrlImages.Enabled = False
        Me.scrlImages.LargeChange = 1
        Me.scrlImages.Location = New System.Drawing.Point(9, 21)
        Me.scrlImages.Maximum = 10
        Me.scrlImages.Minimum = 1
        Me.scrlImages.Name = "scrlImages"
        Me.scrlImages.Size = New System.Drawing.Size(273, 29)
        Me.scrlImages.TabIndex = 35
        Me.scrlImages.Value = 1
        '
        'btnMeasure
        '
        Me.btnMeasure.Enabled = False
        Me.btnMeasure.Location = New System.Drawing.Point(9, 206)
        Me.btnMeasure.Name = "btnMeasure"
        Me.btnMeasure.Size = New System.Drawing.Size(272, 25)
        Me.btnMeasure.TabIndex = 34
        Me.btnMeasure.Text = "Measure image"
        Me.btnMeasure.UseVisualStyleBackColor = True
        '
        'label10
        '
        Me.label10.AutoSize = True
        Me.label10.Location = New System.Drawing.Point(6, 182)
        Me.label10.Name = "label10"
        Me.label10.Size = New System.Drawing.Size(39, 13)
        Me.label10.TabIndex = 32
        Me.label10.Text = "Grade:"
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(222, 151)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(13, 13)
        Me.label7.TabIndex = 30
        Me.label7.Text = "±"
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(222, 119)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(13, 13)
        Me.label6.TabIndex = 29
        Me.label6.Text = "±"
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(221, 87)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(13, 13)
        Me.label5.TabIndex = 28
        Me.label5.Text = "±"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(6, 151)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(113, 13)
        Me.label4.TabIndex = 18
        Me.label4.Text = "Blob center of mass Y:"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(6, 119)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(113, 13)
        Me.label3.TabIndex = 17
        Me.label3.Text = "Blob center of mass X:"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(6, 87)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(55, 13)
        Me.label2.TabIndex = 16
        Me.label2.Text = "Blob area:"
        '
        'lblRecordName
        '
        Me.lblRecordName.AutoSize = True
        Me.lblRecordName.Location = New System.Drawing.Point(6, 62)
        Me.lblRecordName.Name = "lblRecordName"
        Me.lblRecordName.Size = New System.Drawing.Size(10, 13)
        Me.lblRecordName.TabIndex = 14
        Me.lblRecordName.Text = "-"
        '
        'groupBox3
        '
        Me.groupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.groupBox3.Controls.Add(Me.TextBox1)
        Me.groupBox3.Location = New System.Drawing.Point(652, 12)
        Me.groupBox3.Name = "groupBox3"
        Me.groupBox3.Size = New System.Drawing.Size(172, 314)
        Me.groupBox3.TabIndex = 14
        Me.groupBox3.TabStop = False
        Me.groupBox3.Text = "Usage"
        '
        'TextBox1
        '
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox1.Location = New System.Drawing.Point(3, 16)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(166, 295)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = resources.GetString("TextBox1.Text")
        '
        'UserGradingForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(836, 339)
        Me.Controls.Add(Me.groupBox3)
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.cogDisplay1)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.mVerificationResultLabel)
        Me.Name = "UserGradingForm"
        Me.Text = "User Grading Sample Code"
        Me.groupBox1.ResumeLayout(False)
        CType(Me.cogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        CType(Me.nudRCOMY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCOMY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudRCOMX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCOMX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudRArea, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudArea, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox3.ResumeLayout(False)
        Me.groupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private mInputDatabaseLabel As System.Windows.Forms.Label
    Private mVerificationResultLabel As System.Windows.Forms.Label
    Private groupBox1 As System.Windows.Forms.GroupBox
    Private cogDisplay1 As Cognex.VisionPro.Display.CogDisplay
    Private groupBox2 As System.Windows.Forms.GroupBox
    Private lblRecordName As System.Windows.Forms.Label
    Private label4 As System.Windows.Forms.Label
    Private label3 As System.Windows.Forms.Label
    Private label2 As System.Windows.Forms.Label
    Private label7 As System.Windows.Forms.Label
    Private label6 As System.Windows.Forms.Label
    Private label5 As System.Windows.Forms.Label
    Private groupBox3 As System.Windows.Forms.GroupBox
    Private label10 As System.Windows.Forms.Label
    Private WithEvents btnClear As System.Windows.Forms.Button
    Private WithEvents btnMeasure As System.Windows.Forms.Button
    Private WithEvents scrlImages As System.Windows.Forms.HScrollBar
    Private WithEvents cboGrades As System.Windows.Forms.ComboBox
    Private WithEvents nudRCOMY As System.Windows.Forms.NumericUpDown
    Private WithEvents nudCOMY As System.Windows.Forms.NumericUpDown
    Private WithEvents nudRCOMX As System.Windows.Forms.NumericUpDown
    Private WithEvents nudCOMX As System.Windows.Forms.NumericUpDown
    Private WithEvents nudRArea As System.Windows.Forms.NumericUpDown
    Private WithEvents nudArea As System.Windows.Forms.NumericUpDown
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox

End Class
