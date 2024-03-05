'******************************************************************************
' Copyright (C) 2011 Cognex Corporation
'
' Subject to Cognex Corporation's terms and conditions and license
' agreement, you are authorized to use and modify this source code in
' any way you find useful, provided the Software and/or the modified
' Software is used solely in conjunction with a Cognex Machine Vision
' System.  Furthermore you acknowledge and agree that Cognex has no
' warranty, obligations or liability for your use of the Software.
'******************************************************************************
' This sample program is designed to illustrate certain VisionPro
' features or techniques in the simplest way possible. It is not
' intended as the framework for a complete application. In particular,
' the sample program may not provide proper error handling, event
' handling, cleanup, repeatability, and other mechanisms that a
' commercial quality application requires.
'
' This program assumes that you have some knowledge of Visual Basic and
' VisionPro programming.
'
' This sample program demonstrates the programmatic use of the VisionPro
' OCRMax Tool.
'
Option Explicit On

Imports Cognex.VisionPro
Imports Cognex.VisionPro.Display
Imports Cognex.VisionPro.ImageFile
Imports Cognex.VisionPro.OCRMax


Namespace OCRMaxSample


  Public Class Form1
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Private WithEvents ctrlToolDisplay As Cognex.VisionPro.CogToolDisplay
    Private WithEvents txtDescription As System.Windows.Forms.TextBox
    Private WithEvents grpFielding As System.Windows.Forms.GroupBox
    Private WithEvents txtFieldString As System.Windows.Forms.TextBox
    Private WithEvents grpResult As System.Windows.Forms.GroupBox
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents chkFieldingAliasAlpha As System.Windows.Forms.CheckBox
    Private WithEvents txtResult As System.Windows.Forms.TextBox
    Private WithEvents chkFieldingAliasAny As System.Windows.Forms.CheckBox
    Private WithEvents chkFieldingAliasNumeric As System.Windows.Forms.CheckBox


    Private mTool As CogOCRMaxTool


    Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call

    End Sub


    Private Sub DisplayErrorAndExit( _
    ByVal sMsg As String)

      MessageBox.Show(sMsg, "OCRMax Error", _
       MessageBoxButtons.OK, MessageBoxIcon.Error)
      Application.Exit()

    End Sub ' DisplayErrorAndExit(...)


    Private Sub Form1_Load( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles MyBase.Load

      ' Some minor initialization of the application form ...
      txtDescription.Text = ""
      txtDescription.AppendText("Sample Description: ")
      txtDescription.AppendText("This application demonstrates the ")
      txtDescription.AppendText("programmatic use of the Cognex VisionPro ")
      txtDescription.AppendText("OCRMax Tool.")
      txtDescription.AppendText(Environment.NewLine)
      txtDescription.AppendText("Sample Usage: click on Run to ")
      txtDescription.AppendText("perform OCRMax with the specified fielding.")

      ' Find our test image and load it ...
      Dim sPath As String = Environment.GetEnvironmentVariable("VPRO_ROOT")
      If (sPath Is Nothing) Then
        DisplayErrorAndExit("Could not read VPRO_ROOT environment variable.")
        Return
      End If
      sPath += "\\images\\alphanumbers.bmp"

      Dim aImageFile As New CogImageFile()
      Try
        aImageFile.Open(sPath, CogImageFileModeConstants.Read)
      Catch ex As Exception
        DisplayErrorAndExit("Could not load image file " + sPath)
        Return
      End Try

      Dim aImage As CogImage8Grey = aImageFile.Item(0)

      aImageFile.Close()

      ' Define an appropriate region of interest ...
      Dim aROI As New CogRectangleAffine()
      aROI.SetOriginLengthsRotationSkew(340, 748, 1010, 93, 0, 0)

      ' Create a CogOCRMaxTool ...
      mTool = New CogOCRMaxTool()

      ' Start to configure the tool ...
      mTool.InputImage = aImage
      mTool.Region = aROI

      ' Initially, run just the tool's segmenter. We'll use
      ' segmentation results to populate a font object for
      ' use by the classifier ...
      Dim aSegmenterParagraphResult As CogOCRMaxSegmenterParagraphResult = Nothing
      Try
        aSegmenterParagraphResult = mTool.Segmenter.Execute(aImage, aROI)
      Catch aX As Exception
        DisplayErrorAndExit(aX.Message)
        Return
      End Try

      Dim aSegmenterLineResult As CogOCRMaxSegmenterLineResult = _
        aSegmenterParagraphResult.Item(0)

      ' We know apriori that this image with this ROI will yield
      ' segmented images of the alphabet A through Z ...
      Dim iCode As Integer = Microsoft.VisualBasic.AscW("A") 'first codepoint
      For iC As Integer = 0 To 25
        Dim aSegmenterPositionResult As CogOCRMaxSegmenterPositionResult = _
          aSegmenterLineResult.Item(iC)
        Dim aC As CogOCRMaxChar = aSegmenterPositionResult.Character
        aC.CharacterCode = iCode
        mTool.Classifier.Font.Add(aC)
        iCode += 1
      Next iC

      ' Now we can train the classifier ...
      Try
        mTool.Classifier.Train()
      Catch aX As Exception
        DisplayErrorAndExit(aX.Message)
        Return
      End Try

      If (Not mTool.Classifier.Trained) Then
        DisplayErrorAndExit("Could not train classifier.")
        Return
      End If

      ' Now some final form setup from the tool ...
      txtFieldString.Text = mTool.Fielding.FieldString
      chkFieldingAliasAlpha.Checked = _
        mTool.Fielding.FieldingDefinitions.Item("A"c).Enabled
      chkFieldingAliasNumeric.Checked = _
        mTool.Fielding.FieldingDefinitions.Item("N"c).Enabled
      chkFieldingAliasAny.Checked = _
        mTool.Fielding.FieldingDefinitions.Item("*"c).Enabled
      ctrlToolDisplay.Tool = mTool

    End Sub ' Form1_Load(...)


    Private Sub txtFieldString_TextChanged( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles txtFieldString.TextChanged

      mTool.Fielding.FieldString = txtFieldString.Text

    End Sub ' txtFieldString_TextChanged(...)


    Private Sub chkFieldingAliasAlpha_CheckedChanged( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles chkFieldingAliasAlpha.CheckedChanged

      Dim aFD As CogOCRMaxFieldingDefinition = _
          mTool.Fielding.FieldingDefinitions.Item("A"c)
      If (aFD Is Nothing) Then
        Return
      End If

      aFD.Enabled = chkFieldingAliasAlpha.Checked

    End Sub ' chkFieldingAliasAlpha_CheckedChanged(...)


    Private Sub chkFieldingAliasNumeric_CheckedChanged( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles chkFieldingAliasNumeric.CheckedChanged

      Dim aFD As CogOCRMaxFieldingDefinition = _
          mTool.Fielding.FieldingDefinitions.Item("N"c)
      If (aFD Is Nothing) Then
        Return
      End If

      aFD.Enabled = chkFieldingAliasNumeric.Checked

    End Sub ' chkFieldingAliasNumeric_CheckedChanged(...)


    Private Sub chkFieldingAliasAny_CheckedChanged( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles chkFieldingAliasAny.CheckedChanged

      Dim aFD As CogOCRMaxFieldingDefinition = _
          mTool.Fielding.FieldingDefinitions.Item("*"c)
      If (aFD Is Nothing) Then
        Return
      End If

      aFD.Enabled = chkFieldingAliasAny.Checked

    End Sub ' chkFieldingAliasAny_CheckedChanged(...)


    Private Sub btnRun_Click( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles btnRun.Click

      txtResult.Text = ""

      txtFieldString.Enabled = False
      chkFieldingAliasAlpha.Enabled = False
      chkFieldingAliasNumeric.Enabled = False
      chkFieldingAliasAny.Enabled = False

      mTool.Run() ' won't ever throw

      Dim aRunStatus As Cognex.VisionPro.ICogRunStatus = mTool.RunStatus

      If (aRunStatus.Result = CogToolResultConstants.Error) Then
        Dim sMsg As String = "Error running CogOCRMaxTool."
        If (Not aRunStatus.Message Is Nothing) Then
          sMsg += Environment.NewLine + aRunStatus.Message
        End If
        MessageBox.Show(sMsg, "OCRMax Error", _
          MessageBoxButtons.OK, MessageBoxIcon.Error)
      Else
        txtResult.Text = mTool.LineResult.ResultString
      End If

      txtFieldString.Enabled = True
      chkFieldingAliasAlpha.Enabled = True
      chkFieldingAliasNumeric.Enabled = True
      chkFieldingAliasAny.Enabled = True

    End Sub ' btnRun_Click(...)


    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.ctrlToolDisplay = New Cognex.VisionPro.CogToolDisplay
      Me.txtDescription = New System.Windows.Forms.TextBox
      Me.grpFielding = New System.Windows.Forms.GroupBox
      Me.grpResult = New System.Windows.Forms.GroupBox
      Me.btnRun = New System.Windows.Forms.Button
      Me.txtFieldString = New System.Windows.Forms.TextBox
      Me.txtResult = New System.Windows.Forms.TextBox
      Me.chkFieldingAliasAlpha = New System.Windows.Forms.CheckBox
      Me.chkFieldingAliasNumeric = New System.Windows.Forms.CheckBox
      Me.chkFieldingAliasAny = New System.Windows.Forms.CheckBox
      Me.grpFielding.SuspendLayout()
      Me.grpResult.SuspendLayout()
      Me.SuspendLayout()
      '
      'ctrlToolDisplay
      '
      Me.ctrlToolDisplay.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
        Or System.Windows.Forms.AnchorStyles.Left) _
        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.ctrlToolDisplay.Location = New System.Drawing.Point(345, 12)
      Me.ctrlToolDisplay.Name = "ctrlToolDisplay"
      Me.ctrlToolDisplay.SelectedRecordKey = Nothing
      Me.ctrlToolDisplay.ShowRecordsDropDown = True
      Me.ctrlToolDisplay.Size = New System.Drawing.Size(312, 319)
      Me.ctrlToolDisplay.TabIndex = 0
      Me.ctrlToolDisplay.Tool = Nothing
      Me.ctrlToolDisplay.ToolSyncObject = Nothing
      Me.ctrlToolDisplay.UserRecord = Nothing
      '
      'txtDescription
      '
      Me.txtDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.txtDescription.Location = New System.Drawing.Point(12, 356)
      Me.txtDescription.Multiline = True
      Me.txtDescription.Name = "txtDescription"
      Me.txtDescription.ReadOnly = True
      Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both
      Me.txtDescription.Size = New System.Drawing.Size(645, 82)
      Me.txtDescription.TabIndex = 1
      '
      'grpFielding
      '
      Me.grpFielding.Controls.Add(Me.chkFieldingAliasAny)
      Me.grpFielding.Controls.Add(Me.chkFieldingAliasNumeric)
      Me.grpFielding.Controls.Add(Me.chkFieldingAliasAlpha)
      Me.grpFielding.Controls.Add(Me.txtFieldString)
      Me.grpFielding.Location = New System.Drawing.Point(12, 12)
      Me.grpFielding.Name = "grpFielding"
      Me.grpFielding.Size = New System.Drawing.Size(317, 154)
      Me.grpFielding.TabIndex = 2
      Me.grpFielding.TabStop = False
      Me.grpFielding.Text = "Fielding"
      '
      'grpResult
      '
      Me.grpResult.Controls.Add(Me.txtResult)
      Me.grpResult.Location = New System.Drawing.Point(12, 188)
      Me.grpResult.Name = "grpResult"
      Me.grpResult.Size = New System.Drawing.Size(317, 73)
      Me.grpResult.TabIndex = 3
      Me.grpResult.TabStop = False
      Me.grpResult.Text = "Result"
      '
      'btnRun
      '
      Me.btnRun.Location = New System.Drawing.Point(12, 285)
      Me.btnRun.Name = "btnRun"
      Me.btnRun.Size = New System.Drawing.Size(124, 46)
      Me.btnRun.TabIndex = 4
      Me.btnRun.Text = "Run"
      Me.btnRun.UseVisualStyleBackColor = True
      '
      'txtFieldString
      '
      Me.txtFieldString.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.txtFieldString.Location = New System.Drawing.Point(6, 28)
      Me.txtFieldString.Name = "txtFieldString"
      Me.txtFieldString.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
      Me.txtFieldString.Size = New System.Drawing.Size(305, 22)
      Me.txtFieldString.TabIndex = 0
      '
      'txtResult
      '
      Me.txtResult.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.txtResult.Location = New System.Drawing.Point(6, 29)
      Me.txtResult.Name = "txtResult"
      Me.txtResult.ReadOnly = True
      Me.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
      Me.txtResult.Size = New System.Drawing.Size(305, 22)
      Me.txtResult.TabIndex = 0
      '
      'chkFieldingAliasAlpha
      '
      Me.chkFieldingAliasAlpha.AutoSize = True
      Me.chkFieldingAliasAlpha.Location = New System.Drawing.Point(6, 70)
      Me.chkFieldingAliasAlpha.Name = "chkFieldingAliasAlpha"
      Me.chkFieldingAliasAlpha.Size = New System.Drawing.Size(233, 17)
      Me.chkFieldingAliasAlpha.TabIndex = 1
      Me.chkFieldingAliasAlpha.Text = "A = ABCDEFGHIJKLMNOPQRSTUVWXYZ"
      Me.chkFieldingAliasAlpha.UseVisualStyleBackColor = True
      '
      'chkFieldingAliasNumeric
      '
      Me.chkFieldingAliasNumeric.AutoSize = True
      Me.chkFieldingAliasNumeric.Location = New System.Drawing.Point(6, 93)
      Me.chkFieldingAliasNumeric.Name = "chkFieldingAliasNumeric"
      Me.chkFieldingAliasNumeric.Size = New System.Drawing.Size(106, 17)
      Me.chkFieldingAliasNumeric.TabIndex = 2
      Me.chkFieldingAliasNumeric.Text = "N = 0123456789"
      Me.chkFieldingAliasNumeric.UseVisualStyleBackColor = True
      '
      'chkFieldingAliasAny
      '
      Me.chkFieldingAliasAny.AutoSize = True
      Me.chkFieldingAliasAny.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.chkFieldingAliasAny.Location = New System.Drawing.Point(6, 116)
      Me.chkFieldingAliasAny.Name = "chkFieldingAliasAny"
      Me.chkFieldingAliasAny.Size = New System.Drawing.Size(136, 20)
      Me.chkFieldingAliasAny.TabIndex = 3
      Me.chkFieldingAliasAny.Text = "* = {any character}"
      Me.chkFieldingAliasAny.UseVisualStyleBackColor = True
      '
      'Form1
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.ClientSize = New System.Drawing.Size(669, 455)
      Me.Controls.Add(Me.btnRun)
      Me.Controls.Add(Me.grpResult)
      Me.Controls.Add(Me.grpFielding)
      Me.Controls.Add(Me.txtDescription)
      Me.Controls.Add(Me.ctrlToolDisplay)
      Me.MinimumSize = New System.Drawing.Size(677, 482)
      Me.Name = "Form1"
      Me.Text = "OCRMax Sample"
      Me.grpFielding.ResumeLayout(False)
      Me.grpFielding.PerformLayout()
      Me.grpResult.ResumeLayout(False)
      Me.grpResult.PerformLayout()
      Me.ResumeLayout(False)
      Me.PerformLayout()

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


    Private Sub Form1_Disposed( _
      ByVal sender As Object, _
      ByVal e As System.EventArgs) Handles MyBase.Disposed
      If Not mTool Is Nothing Then mTool.Dispose()
    End Sub


  End Class


End Namespace
