'/*******************************************************************************
' Copyright (C) 2004-2010 Cognex Corporation

' Subject to Cognex Corporations terms and conditions and license agreement,
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

' This sample demonstrates how to create a new ICogAcqFifo operator for a given
' video format. The CogAcqFifoTool does not acquire images by itself. Instead it
' uses the ICogAcqFifo to acquire images.

' When the user wants to change the video format, a new ICogAcqFifo must be created.
' The sample displays an error and exits if it cannot locate a frame grabber. This
' is because the frame grabber creates the ICogAcqFifo.

' The sample will acquire an image when the Acquire button is pressed and display
' on the CogDisplay.

' This program assumes that you have some knowledge of C# and VisionPro
' programming.

' The following steps show how to create a new CogAcqFifo operator.
' Step 1) Create the CogFrameGrabbers. Make sure there is at least one Cognex
'         frame grabber on the system.
' Step 2) Select the first frame grabber.
' Step 3) Create an ICogAcqFifo operator with the selected video format.
' Step 4) Acquire an image and display it when the Acquire button is pressed.

' Note that the .NET garbage collector is called every 5th image to free up images that
' are being held on the heap.

'*/
Option Explicit On 

imports System 
imports System.Drawing 
imports System.Collections 
imports System.Windows.Forms 
Imports System.Data
' Needed for VisionPro
imports Cognex.VisionPro 
' Needed for VisionPro Exceptions
Imports Cognex.VisionPro.Exceptions

Namespace CreateAcqFifo
    Public Class Form1
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
        Dim frameGrabbers As New CogFrameGrabbers
        For Each fg As Cognex.VisionPro.ICogFrameGrabber In frameGrabbers
          fg.Disconnect(False)
        Next
      End If
      MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents cogDisplay1 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents textBox1 As System.Windows.Forms.TextBox
    Friend WithEvents AcquireButton As System.Windows.Forms.Button
    Friend WithEvents VidFormatComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents BoardTypeLabel As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form1))
      Me.cogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
      Me.textBox1 = New System.Windows.Forms.TextBox
      Me.AcquireButton = New System.Windows.Forms.Button
      Me.VidFormatComboBox = New System.Windows.Forms.ComboBox
      Me.label2 = New System.Windows.Forms.Label
      Me.BoardTypeLabel = New System.Windows.Forms.Label
      Me.label1 = New System.Windows.Forms.Label
      CType(Me.cogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'cogDisplay1
      '
      Me.cogDisplay1.Location = New System.Drawing.Point(312, 16)
      Me.cogDisplay1.Name = "cogDisplay1"
      Me.cogDisplay1.OcxState = CType(resources.GetObject("cogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
      Me.cogDisplay1.Size = New System.Drawing.Size(336, 296)
      Me.cogDisplay1.TabIndex = 1
      '
      'textBox1
      '
      Me.textBox1.Location = New System.Drawing.Point(4, 320)
      Me.textBox1.Multiline = True
      Me.textBox1.Name = "textBox1"
      Me.textBox1.ReadOnly = True
      Me.textBox1.Size = New System.Drawing.Size(648, 40)
      Me.textBox1.TabIndex = 7
      Me.textBox1.Text = "This sample demonstrates how to create an acquisition fifo for a selected video f" & _
      "ormat.  Select a video format and click Acquire to grab and display an image."
      '
      'AcquireButton
      '
      Me.AcquireButton.Location = New System.Drawing.Point(96, 184)
      Me.AcquireButton.Name = "AcquireButton"
      Me.AcquireButton.Size = New System.Drawing.Size(88, 40)
      Me.AcquireButton.TabIndex = 12
      Me.AcquireButton.Text = "Acquire"
      '
      'VidFormatComboBox
      '
      Me.VidFormatComboBox.Location = New System.Drawing.Point(32, 120)
      Me.VidFormatComboBox.Name = "VidFormatComboBox"
      Me.VidFormatComboBox.Size = New System.Drawing.Size(224, 21)
      Me.VidFormatComboBox.TabIndex = 11
      Me.VidFormatComboBox.Text = "Video Format"
      '
      'label2
      '
      Me.label2.Location = New System.Drawing.Point(32, 88)
      Me.label2.Name = "label2"
      Me.label2.Size = New System.Drawing.Size(136, 16)
      Me.label2.TabIndex = 10
      Me.label2.Text = "Select a Video Format:"
      '
      'BoardTypeLabel
      '
      Me.BoardTypeLabel.Location = New System.Drawing.Point(112, 40)
      Me.BoardTypeLabel.Name = "BoardTypeLabel"
      Me.BoardTypeLabel.Size = New System.Drawing.Size(160, 24)
      Me.BoardTypeLabel.TabIndex = 9
      Me.BoardTypeLabel.Text = "Unknown"
      '
      'label1
      '
      Me.label1.Location = New System.Drawing.Point(32, 40)
      Me.label1.Name = "label1"
      Me.label1.Size = New System.Drawing.Size(72, 24)
      Me.label1.TabIndex = 8
      Me.label1.Text = "Board Type:"
      '
      'Form1
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(656, 366)
      Me.Controls.Add(Me.AcquireButton)
      Me.Controls.Add(Me.VidFormatComboBox)
      Me.Controls.Add(Me.label2)
      Me.Controls.Add(Me.BoardTypeLabel)
      Me.Controls.Add(Me.label1)
      Me.Controls.Add(Me.textBox1)
      Me.Controls.Add(Me.cogDisplay1)
      Me.Name = "Form1"
      Me.Text = "Create Acqusition Fifo Sample"
      CType(Me.cogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region
        Private mAcqFifo As Cognex.VisionPro.ICogAcqFifo = Nothing
        Private mFrameGrabber As Cognex.VisionPro.ICogFrameGrabber = Nothing
        Private numAcqs As Integer = 0

#Region " Initialization"
    Private Sub InitializeAcquisition()
      ' Step 1 - Create the CogFrameGrabbers
      Dim mFrameGrabbers As New CogFrameGrabbers
      If mFrameGrabbers.Count < 1 Then
        Throw New CogAcqNoFrameGrabberException("No frame grabbers found")
      End If
      ' Step 2 - Select the first frame grabber even if there is more than one.
      mFrameGrabber = mFrameGrabbers(0)
      ' Display the board type
      BoardTypeLabel.Text = mFrameGrabber.Name

      ' Fill in video formats so that the user can choose one later.
      VidFormatComboBox.Items.Clear()
      Dim i As Integer
      For i = 0 To mFrameGrabber.AvailableVideoFormats.Count - 1
        VidFormatComboBox.Items.Add(mFrameGrabber.AvailableVideoFormats(i))
      Next

      AcquireButton.Enabled = False
    End Sub

#End Region
#Region " Form controls handlers"
    Private Sub VidFormatComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VidFormatComboBox.SelectedIndexChanged
      ' Step 3: Create the acq fifo with the selected video format.
      Dim videoFormat As String = VidFormatComboBox.SelectedItem.ToString()
      ' note that CreateAcqFifo will throw an exception if it cannot create the
      ' acq fifo with the specified video format.
      mAcqFifo = mFrameGrabber.CreateAcqFifo(videoFormat, _
                  CogAcqFifoPixelFormatConstants.Format8Grey, 0, True)
      AcquireButton.Enabled = True

    End Sub
    Private Sub AcquireButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AcquireButton.Click
      Dim trignum As Integer
      ' Step 4: Acquire an image
      cogDisplay1.Image = mAcqFifo.Acquire(trignum)
      numAcqs += 1
      If numAcqs > 4 Then
        GC.Collect()
        numAcqs = 0
      End If

    End Sub

#End Region

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Try
        InitializeAcquisition()
      Catch ex As CogException
        MessageBox.Show(ex.Message)
        Application.Exit()
      Catch gex As Exception
        MessageBox.Show(gex.Message)
        Application.Exit()
      End Try

    End Sub
  End Class
End Namespace
