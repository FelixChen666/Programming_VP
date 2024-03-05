'*******************************************************************************
' Copyright (C) 2004 Cognex Corporation
'
' Subject to Cognex Corporation's terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
'*******************************************************************************
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This sample demonstrates how to trap an error when an error is thrown.
'
' It is a good programming practice to trap (catch) errors before the user sees
' them, and write one's own error messages. This is because Visual Basic error
' messages are often obscure.
'
' This sample program shows two different ways of trapping errors.
'
' Case 1) Try-Catch (VB.NET style)
'
'
' Old (VB6 Style)
' Case 2) On Error GoTo Label.
'         This is one way to setup error-trapping code in a Visual Basic program.
'         One must add a labeled section of code with an automatic
'         On Error GoTo Label (e.g. ErrorHandler) branch set up at the begining
'         of the procedure (see cmdErrorHandle_Click).
'
' Case 3) On Error Resume Next
'         This technique is often called inline error trapping. Must check for
'         possible errors immediately after the lines of code in which errors
'         might occur (see cmdResume_Click).
Namespace SampleErrorHandle
  Public Class frmErrorHandle
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
      If disposing Then
        If Not (components Is Nothing) Then
          components.Dispose()
        End If
      End If
      MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents lblErrorMessage As System.Windows.Forms.Label
    Friend WithEvents btnVBNETHandling As System.Windows.Forms.Button
    Friend WithEvents btnVB6OnError As System.Windows.Forms.Button
    Friend WithEvents btnResume As System.Windows.Forms.Button
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.lblErrorMessage = New System.Windows.Forms.Label
      Me.btnVBNETHandling = New System.Windows.Forms.Button
      Me.btnVB6OnError = New System.Windows.Forms.Button
      Me.btnResume = New System.Windows.Forms.Button
      Me.txtDescription = New System.Windows.Forms.TextBox
      Me.SuspendLayout()
      '
      'lblErrorMessage
      '
      Me.lblErrorMessage.Location = New System.Drawing.Point(56, 32)
      Me.lblErrorMessage.Name = "lblErrorMessage"
      Me.lblErrorMessage.Size = New System.Drawing.Size(264, 160)
      Me.lblErrorMessage.TabIndex = 0
      '
      'btnVBNETHandling
      '
      Me.btnVBNETHandling.Location = New System.Drawing.Point(336, 24)
      Me.btnVBNETHandling.Name = "btnVBNETHandling"
      Me.btnVBNETHandling.Size = New System.Drawing.Size(200, 40)
      Me.btnVBNETHandling.TabIndex = 1
      Me.btnVBNETHandling.Text = "Handle Exception"
      '
      'btnVB6OnError
      '
      Me.btnVB6OnError.Location = New System.Drawing.Point(336, 88)
      Me.btnVB6OnError.Name = "btnVB6OnError"
      Me.btnVB6OnError.Size = New System.Drawing.Size(200, 40)
      Me.btnVB6OnError.TabIndex = 2
      Me.btnVB6OnError.Text = "On Error GoTo"
      '
      'btnResume
      '
      Me.btnResume.Location = New System.Drawing.Point(336, 144)
      Me.btnResume.Name = "btnResume"
      Me.btnResume.Size = New System.Drawing.Size(200, 40)
      Me.btnResume.TabIndex = 3
      Me.btnResume.Text = "On Error Resume Next"
      '
      'txtDescription
      '
      Me.txtDescription.Location = New System.Drawing.Point(48, 232)
      Me.txtDescription.Name = "txtDescription"
      Me.txtDescription.Size = New System.Drawing.Size(488, 20)
      Me.txtDescription.TabIndex = 4
      Me.txtDescription.Text = "Sample description: demonstrates how to trap an error when an error is thrown."
      '
      'frmErrorHandle
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(552, 398)
      Me.Controls.Add(Me.txtDescription)
      Me.Controls.Add(Me.btnResume)
      Me.Controls.Add(Me.btnVB6OnError)
      Me.Controls.Add(Me.btnVBNETHandling)
      Me.Controls.Add(Me.lblErrorMessage)
      Me.Name = "frmErrorHandle"
      Me.Text = "Show how to handle errors"
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region "Form and Controls Events "
    ' Shows how to trap an error and display one's own error message.
    Private Sub btnVBNETHandling_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVBNETHandling.Click
      Dim y As Integer
      Dim x As Integer
      Dim w As Integer
      x = 4
      w = 0
      Try
        y = x / w
      Catch ex As Exception
        lblErrorMessage.Text = ex.Message.ToString
      End Try
    End Sub

    'Next is VB6 style
    Private Sub btnVB6OnError_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVB6OnError.Click
      Dim y As Integer
      On Error GoTo ErrorHandler

      y = Math.Sqrt(-2)

      ' The following statement will turn off error trapping
      ' On Error GoTo 0

      ' Do not forget to add "Exit Sub" before ErrorHandler
      Exit Sub

ErrorHandler:
      lblErrorMessage.Text = "Visual Basic produced the following obscure error message while executing 'Sqr(-2)'--" & Err.Description & _
      Environment.NewLine & " However, one should replace this message with a friendly message like 'Cannot compute square root of a negative number.'"
    End Sub

    'Next is VB6 style
    Private Sub btnResume_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResume.Click
      ' On Error Resume Next will run the next line(s) of code in the program whether
      ' the program encountered an error or not.
      Dim y As Integer
      On Error Resume Next
      y = Math.Sqrt(-2)

      lblErrorMessage.Text = "Line of Code After Run Time Error Still Executes"

      ' The following statement will turn off error trapping
      ' On Error GoTo 0
    End Sub
#End Region
  End Class

End Namespace