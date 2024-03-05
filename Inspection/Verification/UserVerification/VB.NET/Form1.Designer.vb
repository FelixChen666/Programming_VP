<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UserVerificationForm
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
        Me.groupBox3 = New System.Windows.Forms.GroupBox
        Me.label2 = New System.Windows.Forms.Label
        Me.mBlobThresholdNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.mToolBlockLabel = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.mOutputDatabaseLabel = New System.Windows.Forms.Label
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.mInputDatabaseLabel = New System.Windows.Forms.Label
        Me.mVerificationResultLabel = New System.Windows.Forms.Label
        Me.mVerifyButton = New System.Windows.Forms.Button
        Me.mMismatchedLabel = New System.Windows.Forms.Label
        Me.mMatchedLabel = New System.Windows.Forms.Label
        Me.mTotalLabel = New System.Windows.Forms.Label
        Me.groupBox3.SuspendLayout()
        CType(Me.mBlobThresholdNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox2.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'groupBox3
        '
        Me.groupBox3.Controls.Add(Me.label2)
        Me.groupBox3.Controls.Add(Me.mBlobThresholdNumericUpDown)
        Me.groupBox3.Controls.Add(Me.mToolBlockLabel)
        Me.groupBox3.Controls.Add(Me.label1)
        Me.groupBox3.Location = New System.Drawing.Point(12, 102)
        Me.groupBox3.Name = "groupBox3"
        Me.groupBox3.Size = New System.Drawing.Size(614, 68)
        Me.groupBox3.TabIndex = 12
        Me.groupBox3.TabStop = False
        Me.groupBox3.Text = "CogToolBlock under test"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(186, 44)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(400, 13)
        Me.label2.TabIndex = 11
        Me.label2.Text = "Change this threshold to 10, 100, or 255 and press Verify to see a verification f" & _
            "ailure"
        '
        'mBlobThresholdNumericUpDown
        '
        Me.mBlobThresholdNumericUpDown.Location = New System.Drawing.Point(103, 42)
        Me.mBlobThresholdNumericUpDown.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.mBlobThresholdNumericUpDown.Name = "mBlobThresholdNumericUpDown"
        Me.mBlobThresholdNumericUpDown.Size = New System.Drawing.Size(77, 20)
        Me.mBlobThresholdNumericUpDown.TabIndex = 9
        '
        'mToolBlockLabel
        '
        Me.mToolBlockLabel.AutoSize = True
        Me.mToolBlockLabel.Location = New System.Drawing.Point(6, 16)
        Me.mToolBlockLabel.Name = "mToolBlockLabel"
        Me.mToolBlockLabel.Size = New System.Drawing.Size(124, 13)
        Me.mToolBlockLabel.TabIndex = 3
        Me.mToolBlockLabel.Text = "CogToolBlock under test"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(6, 44)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(81, 13)
        Me.label1.TabIndex = 8
        Me.label1.Text = "Blob Threshold:"
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.mOutputDatabaseLabel)
        Me.groupBox2.Location = New System.Drawing.Point(12, 57)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(614, 39)
        Me.groupBox2.TabIndex = 11
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Output Database"
        '
        'mOutputDatabaseLabel
        '
        Me.mOutputDatabaseLabel.AutoSize = True
        Me.mOutputDatabaseLabel.Location = New System.Drawing.Point(6, 16)
        Me.mOutputDatabaseLabel.Name = "mOutputDatabaseLabel"
        Me.mOutputDatabaseLabel.Size = New System.Drawing.Size(88, 13)
        Me.mOutputDatabaseLabel.TabIndex = 2
        Me.mOutputDatabaseLabel.Text = "Output Database"
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.mInputDatabaseLabel)
        Me.groupBox1.Location = New System.Drawing.Point(12, 12)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(614, 39)
        Me.groupBox1.TabIndex = 10
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Input Database"
        '
        'mInputDatabaseLabel
        '
        Me.mInputDatabaseLabel.AutoSize = True
        Me.mInputDatabaseLabel.Location = New System.Drawing.Point(6, 17)
        Me.mInputDatabaseLabel.Name = "mInputDatabaseLabel"
        Me.mInputDatabaseLabel.Size = New System.Drawing.Size(80, 13)
        Me.mInputDatabaseLabel.TabIndex = 1
        Me.mInputDatabaseLabel.Text = "Input Database"
        '
        'mVerificationResultLabel
        '
        Me.mVerificationResultLabel.AutoSize = True
        Me.mVerificationResultLabel.BackColor = System.Drawing.SystemColors.Control
        Me.mVerificationResultLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mVerificationResultLabel.Location = New System.Drawing.Point(93, 212)
        Me.mVerificationResultLabel.Name = "mVerificationResultLabel"
        Me.mVerificationResultLabel.Size = New System.Drawing.Size(0, 42)
        Me.mVerificationResultLabel.TabIndex = 9
        '
        'mVerifyButton
        '
        Me.mVerifyButton.Location = New System.Drawing.Point(12, 212)
        Me.mVerifyButton.Name = "mVerifyButton"
        Me.mVerifyButton.Size = New System.Drawing.Size(75, 42)
        Me.mVerifyButton.TabIndex = 8
        Me.mVerifyButton.Text = "Verify"
        Me.mVerifyButton.UseVisualStyleBackColor = True
        '
        'mMismatchedLabel
        '
        Me.mMismatchedLabel.AutoSize = True
        Me.mMismatchedLabel.Location = New System.Drawing.Point(562, 182)
        Me.mMismatchedLabel.Name = "mMismatchedLabel"
        Me.mMismatchedLabel.Size = New System.Drawing.Size(64, 13)
        Me.mMismatchedLabel.TabIndex = 15
        Me.mMismatchedLabel.Text = "Mismatch: 0"
        '
        'mMatchedLabel
        '
        Me.mMatchedLabel.AutoSize = True
        Me.mMatchedLabel.Location = New System.Drawing.Point(299, 182)
        Me.mMatchedLabel.Name = "mMatchedLabel"
        Me.mMatchedLabel.Size = New System.Drawing.Size(49, 13)
        Me.mMatchedLabel.TabIndex = 14
        Me.mMatchedLabel.Text = "Match: 0"
        '
        'mTotalLabel
        '
        Me.mTotalLabel.AutoSize = True
        Me.mTotalLabel.Location = New System.Drawing.Point(13, 182)
        Me.mTotalLabel.Name = "mTotalLabel"
        Me.mTotalLabel.Size = New System.Drawing.Size(43, 13)
        Me.mTotalLabel.TabIndex = 13
        Me.mTotalLabel.Text = "Total: 0"
        '
        'UserVerificationForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(638, 266)
        Me.Controls.Add(Me.mMismatchedLabel)
        Me.Controls.Add(Me.mMatchedLabel)
        Me.Controls.Add(Me.mTotalLabel)
        Me.Controls.Add(Me.groupBox3)
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.mVerificationResultLabel)
        Me.Controls.Add(Me.mVerifyButton)
        Me.Name = "UserVerificationForm"
        Me.Text = "User Verification Sample Code"
        Me.groupBox3.ResumeLayout(False)
        Me.groupBox3.PerformLayout()
        CType(Me.mBlobThresholdNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents groupBox3 As System.Windows.Forms.GroupBox
    Private WithEvents mBlobThresholdNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents mToolBlockLabel As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Private WithEvents mOutputDatabaseLabel As System.Windows.Forms.Label
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents mInputDatabaseLabel As System.Windows.Forms.Label
    Private WithEvents mVerificationResultLabel As System.Windows.Forms.Label
    Private WithEvents mVerifyButton As System.Windows.Forms.Button
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents mMismatchedLabel As System.Windows.Forms.Label
    Private WithEvents mMatchedLabel As System.Windows.Forms.Label
    Private WithEvents mTotalLabel As System.Windows.Forms.Label

End Class
