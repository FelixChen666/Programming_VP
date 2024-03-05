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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
    Me.textBox1 = New System.Windows.Forms.TextBox
    Me.btnOnline = New System.Windows.Forms.Button
    Me.btnRun = New System.Windows.Forms.Button
    Me.Text1 = New System.Windows.Forms.TextBox
    Me.SuspendLayout()
    '
    'textBox1
    '
    Me.textBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.textBox1.Location = New System.Drawing.Point(9, 2)
    Me.textBox1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
    Me.textBox1.Multiline = True
    Me.textBox1.Name = "textBox1"
    Me.textBox1.ReadOnly = True
    Me.textBox1.Size = New System.Drawing.Size(352, 125)
    Me.textBox1.TabIndex = 11
    '
    'btnOnline
    '
    Me.btnOnline.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnOnline.Location = New System.Drawing.Point(371, 2)
    Me.btnOnline.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
    Me.btnOnline.Name = "btnOnline"
    Me.btnOnline.Size = New System.Drawing.Size(105, 29)
    Me.btnOnline.TabIndex = 10
    Me.btnOnline.Text = "Online"
    Me.btnOnline.UseVisualStyleBackColor = True
    '
    'btnRun
    '
    Me.btnRun.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnRun.Location = New System.Drawing.Point(371, 44)
    Me.btnRun.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
    Me.btnRun.Name = "btnRun"
    Me.btnRun.Size = New System.Drawing.Size(105, 29)
    Me.btnRun.TabIndex = 9
    Me.btnRun.Text = "Run"
    Me.btnRun.UseVisualStyleBackColor = True
    '
    'Text1
    '
    Me.Text1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.Text1.Location = New System.Drawing.Point(2, 132)
    Me.Text1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
    Me.Text1.Multiline = True
    Me.Text1.Name = "Text1"
    Me.Text1.ReadOnly = True
    Me.Text1.Size = New System.Drawing.Size(487, 166)
    Me.Text1.TabIndex = 8
    Me.Text1.Text = resources.GetString("Text1.Text")
    '
    'Form1
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(493, 301)
    Me.Controls.Add(Me.textBox1)
    Me.Controls.Add(Me.btnOnline)
    Me.Controls.Add(Me.btnRun)
    Me.Controls.Add(Me.Text1)
    Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
    Me.Name = "Form1"
    Me.Text = "MCB Sample Program"
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Private WithEvents textBox1 As System.Windows.Forms.TextBox
  Private WithEvents btnOnline As System.Windows.Forms.Button
  Private WithEvents btnRun As System.Windows.Forms.Button
  Friend WithEvents Text1 As System.Windows.Forms.TextBox

End Class
