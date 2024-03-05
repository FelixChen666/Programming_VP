' *******************************************************************************
' Copyright (C) 2004 Cognex Corporation
' 
' Subject to Cognex Corporation's terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
' *******************************************************************************
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' *******************************************************************************
' Description:
' 
' This sample demonstrates three ways to use OCV for applications in which the
' pattern to verify changes frequently.  Examples of this include:
'   - Serial number verification, where the serial number increases with each
'     presented image.
'   - Credit card verification, where the credit card number and the name change
'     with each presented image.
' 
' Three different algorithms are provided for solving these applications.  All three
' algorithms assume that that the font has already been constructed (e.g. using
' the font editor, provided with the VPro OCV component).
' 
' The algorithms are:
'   - Standard:  With each new image and associated pattern to verify, a new pattern
'                is trained.  The total time to process an image is the time to
'                train the new pattern plus the time to run verification of that pattern
'                on the provided image.
' 
'   - Multiple Models: This algorithm uses limited OCR to solve the OCV problem.
'                During setup time, each position in the pattern is trained with
'                all the possible font models that might appear at that position.  For
'                example, if a particular pattern position is known to be numeric, the
'                position is trained with patterns 0-9.  At run time, when a new image
'                is presented for verification, OCV tests all the font models that have
'                been trained at each position and returns the font model with the
'                highest score at that position. The set of highest scoring font models
'                is compared to the verification string to determine if they match.  The
'                total time to process an image is the time to run verfication of the
'                provided pattern on the image.
'                Notes:
'                  1. The time to run verification is directly dependent on the number
'                     of models at each position.  Since OCV must test all models
'                     provided at each position, it will be slower for cases in which
'                     a large number of models are trained at each position.
'                  2. OCV does not check for confusion between models that are trained
'                     at a given position.  For example, if the user trains both an
'                     O (the letter) and a 0 (the number) at a particular position,
'                     then confusion will not be checked between the two.  OCV will
'                     simply return the highest scoring model.
' 
'   - Smart Placement: This algorithm tries to efficiently verify only the expected
'                 model at each position.  At setup time, a separate pattern is
'                 trained for each model in the font.  The pattern consists of only
'                 a single model.  For example, if the font consists of the
'                 numbers 0-9, then 10 patterns are constructed: Pattern0 has
'                 only the 0 model, Pattern1 has only the 1 model, etc.  Each pattern
'                 pose is set to some default value. At run time, the verification
'                 string is used to select which patterns are tested on the image, as
'                 well as the location of each pattern.  For example, to verify the
'                 pattern "Thomas", the run-time pose for pattern containing the "T"
'                 model is placed at the expected pose for the overall string.
'                 That pattern is then verified.  Next, the run-time pose for the
'                 pattern containing the "h" is assigned by offsetting it's position
'                 from the "T".  The pattern "h" is then verified. The process of
'                 pose determination and verification continues until the entire string
'                 is verified.
'                 Notes:
'                 1. This algorithm is by far the faster of the three since it is only
'                    verifying one model per position.
'                 2. The run-time pose for each pattern must be carefully calculated
'                    by the application.  This works well for fixed width fonts, but
'                    poorly for proportional width fonts.
' 
' This sample code uses two image sets:
'  - alphas.idb: a set of images consisting of alpha (non-numeric) strings in
'                courier 14pt bold.
'  - numbers.idb: a set of images consiting of numeric (non-alpha) strings in
'                courier 14pt bold.
'  There are also a set of text files containing ground truth information for
'  the images:
'   - alphas.txt and numbers.txt
' 
' This example also includes two pre-constructed font files:
'   - courier14ptalpha.vpp: the font for alpha (non-numeric) courier 14pt bold chars.
'   - courier14ptnumeric.vpp: the font for numeric (non-alpha) courier 14pt bold chars.
' 
' *** It is a requirement that the the .txt files and the .vpp files                ***
' *** described are located in the same directory as this sample code.  The sample ***
' *** code only looks in its current directory for these files.                    ***

' *** The .idb files must be located in the <VPRO_ROOT>\images directory.          ***

' For reference, the image alphanumbers.bmp is included in the <VPRO_ROOT>\images
' directory.  This image contains the courier 14pt bold chars plus other
' sample font chars.
' *******************************************************************************
Option Explicit On 
Imports System.IO

Imports Cognex.VisionPro                  'used by VisionPro basic functionality
Imports Cognex.VisionPro.Exceptions       'used by VisionPro exceptions
Imports Cognex.VisionPro.Display          'used by CogDisplay
Imports Cognex.VisionPro.ImageFile        'used by CogImageFileTool
Imports Cognex.VisionPro.OC               'used by CogOCVTool and cogOCFont

Namespace OCVPatternChangeSample
  Public Class OCVPatternChangeForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call
      mOCFont = New CogOCFont
      mImageFile = New CogImageFile
      mOCVResults = New ArrayList

      optNumeric.Checked = True
      optAlpha.Checked = False
      btnProcess.Enabled = False
      txtInfo.Text = ""

      mDisplay.AutoFit = True  ' turn on auto fit image of display
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
      If disposing Then
        ' release any resources we employed in the application
        If Not mOCVTool Is Nothing Then mOCVTool.Dispose()
        If Not mImageFile Is Nothing Then mImageFile.Close()
        If Not mSmartPatterns Is Nothing Then mSmartPatterns.Clear() ' release the reference to pattern objects
        If Not mOCVResults Is Nothing Then mOCVResults.Clear()

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
    Friend WithEvents btnProcess As System.Windows.Forms.Button
    Friend WithEvents txtTimePerChar As System.Windows.Forms.TextBox
    Friend WithEvents txtTimeTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtResult As System.Windows.Forms.TextBox
    Friend WithEvents txtInfo As System.Windows.Forms.TextBox
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents lblResult As System.Windows.Forms.Label
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents optAlpha As System.Windows.Forms.RadioButton
    Friend WithEvents optNumeric As System.Windows.Forms.RadioButton
    Friend WithEvents mDisplay As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents mnuMain As System.Windows.Forms.MainMenu
    Friend WithEvents mnuSamples As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMultipleModels As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSmartPlacement As System.Windows.Forms.MenuItem
    Friend WithEvents mnuStandard As System.Windows.Forms.MenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(OCVPatternChangeForm))
      Me.btnProcess = New System.Windows.Forms.Button
      Me.txtTimePerChar = New System.Windows.Forms.TextBox
      Me.txtTimeTotal = New System.Windows.Forms.TextBox
      Me.txtResult = New System.Windows.Forms.TextBox
      Me.txtInfo = New System.Windows.Forms.TextBox
      Me.txtDescription = New System.Windows.Forms.TextBox
      Me.label2 = New System.Windows.Forms.Label
      Me.label1 = New System.Windows.Forms.Label
      Me.lblResult = New System.Windows.Forms.Label
      Me.lblDescription = New System.Windows.Forms.Label
      Me.optAlpha = New System.Windows.Forms.RadioButton
      Me.optNumeric = New System.Windows.Forms.RadioButton
      Me.mDisplay = New Cognex.VisionPro.Display.CogDisplay
      Me.mnuMain = New System.Windows.Forms.MainMenu
      Me.mnuSamples = New System.Windows.Forms.MenuItem
      Me.mnuMultipleModels = New System.Windows.Forms.MenuItem
      Me.mnuSmartPlacement = New System.Windows.Forms.MenuItem
      Me.mnuStandard = New System.Windows.Forms.MenuItem
      Me.mnuExit = New System.Windows.Forms.MenuItem
      CType(Me.mDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'btnProcess
      '
      Me.btnProcess.Location = New System.Drawing.Point(32, 8)
      Me.btnProcess.Name = "btnProcess"
      Me.btnProcess.Size = New System.Drawing.Size(136, 48)
      Me.btnProcess.TabIndex = 1
      Me.btnProcess.Text = "Process Next Image"
      '
      'txtTimePerChar
      '
      Me.txtTimePerChar.Location = New System.Drawing.Point(592, 160)
      Me.txtTimePerChar.Name = "txtTimePerChar"
      Me.txtTimePerChar.ReadOnly = True
      Me.txtTimePerChar.Size = New System.Drawing.Size(72, 20)
      Me.txtTimePerChar.TabIndex = 23
      Me.txtTimePerChar.Text = ""
      '
      'txtTimeTotal
      '
      Me.txtTimeTotal.Location = New System.Drawing.Point(416, 160)
      Me.txtTimeTotal.Name = "txtTimeTotal"
      Me.txtTimeTotal.ReadOnly = True
      Me.txtTimeTotal.Size = New System.Drawing.Size(72, 20)
      Me.txtTimeTotal.TabIndex = 22
      Me.txtTimeTotal.Text = ""
      '
      'txtResult
      '
      Me.txtResult.Location = New System.Drawing.Point(112, 160)
      Me.txtResult.Name = "txtResult"
      Me.txtResult.ReadOnly = True
      Me.txtResult.Size = New System.Drawing.Size(236, 20)
      Me.txtResult.TabIndex = 21
      Me.txtResult.Text = ""
      '
      'txtInfo
      '
      Me.txtInfo.Location = New System.Drawing.Point(248, 56)
      Me.txtInfo.Multiline = True
      Me.txtInfo.Name = "txtInfo"
      Me.txtInfo.ReadOnly = True
      Me.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both
      Me.txtInfo.Size = New System.Drawing.Size(440, 68)
      Me.txtInfo.TabIndex = 17
      Me.txtInfo.Text = ""
      '
      'txtDescription
      '
      Me.txtDescription.Location = New System.Drawing.Point(368, 16)
      Me.txtDescription.Name = "txtDescription"
      Me.txtDescription.ReadOnly = True
      Me.txtDescription.Size = New System.Drawing.Size(320, 20)
      Me.txtDescription.TabIndex = 16
      Me.txtDescription.Text = ""
      '
      'label2
      '
      Me.label2.Location = New System.Drawing.Point(520, 160)
      Me.label2.Name = "label2"
      Me.label2.Size = New System.Drawing.Size(68, 32)
      Me.label2.TabIndex = 20
      Me.label2.Text = "Per Char Time (ms)"
      '
      'label1
      '
      Me.label1.Location = New System.Drawing.Point(360, 160)
      Me.label1.Name = "label1"
      Me.label1.Size = New System.Drawing.Size(56, 28)
      Me.label1.TabIndex = 19
      Me.label1.Text = "Total Time (ms)"
      '
      'lblResult
      '
      Me.lblResult.Location = New System.Drawing.Point(40, 160)
      Me.lblResult.Name = "lblResult"
      Me.lblResult.Size = New System.Drawing.Size(68, 20)
      Me.lblResult.TabIndex = 18
      Me.lblResult.Text = "Results"
      '
      'lblDescription
      '
      Me.lblDescription.Location = New System.Drawing.Point(248, 24)
      Me.lblDescription.Name = "lblDescription"
      Me.lblDescription.Size = New System.Drawing.Size(104, 16)
      Me.lblDescription.TabIndex = 15
      Me.lblDescription.Text = "Sample Description"
      '
      'optAlpha
      '
      Me.optAlpha.Location = New System.Drawing.Point(48, 104)
      Me.optAlpha.Name = "optAlpha"
      Me.optAlpha.Size = New System.Drawing.Size(124, 20)
      Me.optAlpha.TabIndex = 14
      Me.optAlpha.Text = "Alpha Images"
      '
      'optNumeric
      '
      Me.optNumeric.Location = New System.Drawing.Point(48, 72)
      Me.optNumeric.Name = "optNumeric"
      Me.optNumeric.Size = New System.Drawing.Size(124, 20)
      Me.optNumeric.TabIndex = 13
      Me.optNumeric.Text = "Numeric Images"
      '
      'mDisplay
      '
      Me.mDisplay.Location = New System.Drawing.Point(40, 196)
      Me.mDisplay.Name = "mDisplay"
      Me.mDisplay.OcxState = CType(resources.GetObject("mDisplay.OcxState"), System.Windows.Forms.AxHost.State)
      Me.mDisplay.Size = New System.Drawing.Size(668, 308)
      Me.mDisplay.TabIndex = 24
      '
      'mnuMain
      '
      Me.mnuMain.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuSamples})
      '
      'mnuSamples
      '
      Me.mnuSamples.Index = 0
      Me.mnuSamples.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuMultipleModels, Me.mnuSmartPlacement, Me.mnuStandard, Me.mnuExit})
      Me.mnuSamples.Text = "Samples"
      '
      'mnuMultipleModels
      '
      Me.mnuMultipleModels.Index = 0
      Me.mnuMultipleModels.Text = "Multiple Models"
      '
      'mnuSmartPlacement
      '
      Me.mnuSmartPlacement.Index = 1
      Me.mnuSmartPlacement.Text = "Smart Placement"
      '
      'mnuStandard
      '
      Me.mnuStandard.Index = 2
      Me.mnuStandard.Text = "Standard"
      '
      'mnuExit
      '
      Me.mnuExit.Index = 3
      Me.mnuExit.Text = "Exit"
      '
      'OCVPatternChangeForm
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(756, 529)
      Me.Controls.Add(Me.mDisplay)
      Me.Controls.Add(Me.txtTimePerChar)
      Me.Controls.Add(Me.txtTimeTotal)
      Me.Controls.Add(Me.txtResult)
      Me.Controls.Add(Me.txtInfo)
      Me.Controls.Add(Me.txtDescription)
      Me.Controls.Add(Me.label2)
      Me.Controls.Add(Me.label1)
      Me.Controls.Add(Me.lblResult)
      Me.Controls.Add(Me.lblDescription)
      Me.Controls.Add(Me.optAlpha)
      Me.Controls.Add(Me.optNumeric)
      Me.Controls.Add(Me.btnProcess)
      Me.Menu = Me.mnuMain
      Me.Name = "OCVPatternChangeForm"
      Me.Text = "OCV Pattern Change Sample Application"
      CType(Me.mDisplay, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Private Fields"

    Private mOCVTool As CogOCVTool = Nothing
    Private mOCFont As CogOCFont = Nothing
    Private mImageFile As CogImageFile = Nothing

    Private mCurrentImage As Integer = 0
    Private mMaxStringLength As Integer = 0
    Private mPatternOriginX As Integer = 0
    Private mPatternOriginY As Integer = 0
    Private mFontWidth As Integer = 0
    Private mFontHeight As Integer = 0

    Private mRunOption As MenuOptionConstants = MenuOptionConstants.None
    Private mOCVStringsToVerify As ArrayList = Nothing
    Private mSmartPatterns As ArrayList = Nothing
    Private mOCVResults As ArrayList = Nothing

#End Region

#Region "Private Enums"
    Private Enum MenuOptionConstants
      None = -1
      MultipleModels
      SmartPlacement
      Standard
    End Enum
#End Region

#Region "Private Control Event Handler"
    Private Sub btnProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcess.Click
      Select Case (mRunOption)
        Case MenuOptionConstants.MultipleModels
          ProcessMultipleModels()
        Case MenuOptionConstants.SmartPlacement
          ProcessSmartPlacement()
        Case MenuOptionConstants.Standard
          ProcessStandard()
      End Select
    End Sub

    Private Sub mnuMultipleModels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMultipleModels.Click
      SelectRunMode(MenuOptionConstants.MultipleModels)
    End Sub

    Private Sub mnuSmartPlacement_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSmartPlacement.Click
      SelectRunMode(MenuOptionConstants.SmartPlacement)
    End Sub

    Private Sub mnuStandard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuStandard.Click
      SelectRunMode(MenuOptionConstants.Standard)
    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
      Me.Close()
    End Sub

    Private Sub optNumeric_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optNumeric.CheckedChanged
      If optNumeric.Checked Then
        OpenDB()  ' openDB since selection has changed
        SelectRunMode(mRunOption)
      End If
    End Sub

    Private Sub optAlpha_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optAlpha.CheckedChanged
      If optAlpha.Checked Then
        OpenDB()  ' openDB since selection has changed
        SelectRunMode(mRunOption)
      End If
    End Sub
#End Region

#Region "Private Methods"
    ''' <summary>
    ''' This subroutine opens the image database files (*.idb), the font files (*.vpp)
    ''' And the text file that describes the strings in each image.
    ''' </summary>
    Private Sub OpenDB()
      Try
        Me.Cursor = Cursors.WaitCursor
        Dim sBaseDir As String = Environment.GetEnvironmentVariable("VPRO_ROOT")
        If sBaseDir = "" Then
          MessageBox.Show("Required environment variable VPRO_ROOT is not set", "OCVPatternChange", MessageBoxButtons.OK, MessageBoxIcon.Stop)
          Exit Sub
        End If
        mImageFile.Close()
                Dim sDBFile As String = sBaseDir + "/Samples/Programming/VisionTools/OCVPatternChange/"
                Dim sFontFile As String = sBaseDir + "/Samples/Programming/VisionTools/OCVPatternChange/"
        If optNumeric.Checked Then
          mImageFile.Open(sBaseDir & "\Images\numbers.idb", CogImageFileModeConstants.Read)
          sDBFile = sDBFile & "numbers.txt"  ' numeric file 0-9
          sFontFile = sFontFile & "courier14ptnumeric.vpp"
        Else
          mImageFile.Open(sBaseDir & "\Images\alphas.idb", CogImageFileModeConstants.Read)
          sDBFile += "alphas.txt"   ' Alpha font (letters A-Z)
          sFontFile += "courier14ptalpha.vpp"
        End If
        mOCFont = CogSerializer.LoadObjectFromFile(sFontFile)
        ' Note that the text file have header information consisting of:
        '   maxStringLength: The maximum string length for any string in the file.
        '   patternOriginX, patternOriginY: The rough expected position of the first character in each string.
        '   fontWidth: The average width of each character in the font.
        '   Note that this is for fixed-width fonts only.
        '   fontHeight: The average height of each character in the font.
        Dim reader As StreamReader = File.OpenText(sDBFile)
        mMaxStringLength = System.Convert.ToInt32(reader.ReadLine())
        Dim origin As String = reader.ReadLine()
        mPatternOriginX = Convert.ToInt32(origin.Substring(0, origin.IndexOf(",")))
        mPatternOriginY = Convert.ToInt32(origin.Substring(origin.Length - origin.IndexOf(",")).Trim())
        Dim font As String = reader.ReadLine()
        mFontWidth = Convert.ToInt32(font.Substring(0, font.IndexOf(",")))
        mFontHeight = Convert.ToInt32(font.Substring(font.Length - font.IndexOf(",")).Trim())
        mOCVStringsToVerify = New ArrayList
        Dim sNext As String = reader.ReadLine()
        While Not sNext Is Nothing
          mOCVStringsToVerify.Add(sNext)
          sNext = reader.ReadLine()
        End While
        reader.Close()

        If mSmartPatterns Is Nothing Then mSmartPatterns = New ArrayList
        ' if the count of patterns in the list is less than the count of font models, add more
        For iPattern As Integer = mSmartPatterns.Count To mOCFont.FontModels.Count - 1
          mSmartPatterns.Add(New CogOCVPattern)
        Next iPattern
        ' if the count of patterns in the list is more than the count of font models, remove them
        While mSmartPatterns.Count > mOCFont.FontModels.Count
          mSmartPatterns.RemoveAt(0)
        End While
      Catch ex As CogException
        MessageBox.Show(ex.Message, "OCVPatternChange")
        btnProcess.Enabled = False
      Catch ex As Exception
        MessageBox.Show(ex.Message, "OCVPatternChange")
        btnProcess.Enabled = False
      Finally
        mCurrentImage = 0
        Me.ResetCursor()
      End Try
    End Sub
    ''' <summary>
    ''' This subroutine is the run-time processing for the multiple models method.
    ''' </summary>
    Private Sub ProcessMultipleModels()
      Try
        Me.Cursor = Cursors.WaitCursor
        btnProcess.Enabled = False
        ' clean-up the old result list
        mOCVResults.Clear()
        ' remove old graphics if they exist
        mDisplay.StaticGraphics.Clear()

        ' Get the image from the CogImageFile operator
        Dim mImage As ICogImage = mImageFile(mCurrentImage)
        ' Display it on my control's CogDisplay
        mDisplay.Image = mImage
        ' Set run-time parameters for the OCV tool.
        mOCVTool.InputImage = mImage
        mOCVTool.PatternPosition.ExpectedPose.TranslationX = 0
        mOCVTool.PatternPosition.ExpectedPose.TranslationY = 0
        mOCVTool.PatternPosition.ExpectedPose.Rotation = 0
        mOCVTool.PatternPosition.ExpectedPose.Scaling = 1
        ' make sure to set the selected space name or the positional information will
        ' be incorrect.
        mOCVTool.PatternPosition.SelectedSpaceName = mImage.SelectedSpaceName
        mOCVTool.PatternPosition.TranslationUncertainty = 4 '+/- 4 pixels in TransX, TransY
        mOCVTool.PatternPosition.RotationUncertainty = 0
        mOCVTool.PatternPosition.ScalingUncertainty = 0
        ' All set, so run the tool
        mOCVTool.Run()
        ' Get the next record in the database
        Dim sPattern As String = mOCVStringsToVerify(mCurrentImage)
        ' Get the results out, one by one.
        If (mOCVTool.RunStatus.Exception Is Nothing) Then 'no exceptions
          Dim resultString As String = ""
          For i As Integer = 0 To sPattern.Length - 1
            mOCVResults.Add(mOCVTool.Results(i))
            If (mOCVTool.Results(i).Status = CogOCVCharacterStatusConstants.Verified) Then
              resultString = resultString + mOCVTool.Results(i).FontModelName
            Else
              resultString = resultString & "?"
            End If
          Next i
          ' Post the results to the control.
          PostResults(mOCVTool.RunStatus.ProcessingTime, sPattern, resultString, sPattern.Length)
        End If
        btnProcess.Enabled = True
      Catch ex As CogException
        MessageBox.Show("ProcessSmartPlacement caught exception: " & ex.Message)
      Catch ex As Exception
        MessageBox.Show("ProcessSmartPlacement caught exception: " & ex.Message)
      Finally
        mCurrentImage = IIf(mCurrentImage + 1 >= mImageFile.Count, 0, mCurrentImage + 1)
        Me.ResetCursor()
      End Try
    End Sub
    ''' <summary>
    ''' This subroutine is the run-time processing for the smart placement method. 
    ''' </summary>
    Private Sub ProcessSmartPlacement()
      Try
        Me.Cursor = Cursors.WaitCursor
        btnProcess.Enabled = False

        ' clear old result list
        mOCVResults.Clear()

        ' remove old graphics if they exist
        mDisplay.StaticGraphics.Clear()

        ' Get the image from the CogImageFile operator
        Dim mImage As ICogImage = mImageFile(mCurrentImage)
        ' Display it on my control's CogDisplay
        mDisplay.Image = mImage

        ' Get the next record in the database
        Dim sPattern As String = mOCVStringsToVerify(mCurrentImage)

        ' Now we're going to use the characters in the verification string and the
        ' model width & pattern origin info to lay out our string to verify.  We do this
        ' dynamically.  After verifying the first character, we use it's position to
        ' place the next character for verification.  We then verify it. We repeat
        ' until the string is complete.
        ' Note that in this sample code, we are making the assumption that the characters
        ' are arranged horizontally.  If this is not true in your application, then
        ' the position of the next character needs to be computed in a different manner.
        Dim numBlanks As Integer = 0
        Dim totalTime As Double = 0.0
        Dim resultsString As String = ""
        For iPos As Integer = 0 To sPattern.Length - 1
          If Not mOCVTool Is Nothing Then mOCVTool.Dispose()
          mOCVTool = New CogOCVTool
          ' Get the first character to verify
          Dim thisChar As String = sPattern.Substring(iPos, 1)
          If thisChar <> " " Then
            ' find the pattern corresponding to this character in the mySmartPatterns array
            Dim smartPattern As CogOCVPattern = Nothing
            For Each pattern As CogOCVPattern In mSmartPatterns
              Dim iModelInstance As Integer
                            Dim sModelName As String
                            sModelName = ""
              pattern.CharacterPositions(0).GetFontModel(0, sModelName, iModelInstance)
              If sModelName = thisChar Then
                smartPattern = pattern
                Exit For
              End If
            Next pattern
            If (smartPattern Is Nothing) Then
              Throw New Exception("Illegal character: " & thisChar & " found")
            End If
            ' Set the tools pattern to the pattern associated with this character
            mOCVTool.Pattern = smartPattern
            mOCVTool.InputImage = mImage
            ' Use PatternPosition.ExpectedPose to place the expected location of
            ' this pattern (consisting of a single character) in the image.
            ' Shift the origin of the next character by the width of a character in the font.
            ' Note that this assumes that the string is positioned horizontally.  Your
            ' application my differ.
            mOCVTool.PatternPosition.ExpectedPose.TranslationX = mPatternOriginX + iPos * mFontWidth
            mOCVTool.PatternPosition.ExpectedPose.TranslationY = mPatternOriginY
            mOCVTool.PatternPosition.ExpectedPose.Rotation = 0
            mOCVTool.PatternPosition.ExpectedPose.Scaling = 1
            mOCVTool.PatternPosition.SelectedSpaceName = mImage.SelectedSpaceName
            mOCVTool.PatternPosition.TranslationUncertainty = 2
            mOCVTool.PatternPosition.RotationUncertainty = 0
            mOCVTool.PatternPosition.ScalingUncertainty = 0
            ' All set, so run the tool
            mOCVTool.Run()
            totalTime = totalTime + mOCVTool.RunStatus.ProcessingTime
            If (mOCVTool.RunStatus.Exception Is Nothing) Then
              mOCVResults.Add(mOCVTool.Results(0))
              If mOCVTool.Results(0).Status = CogOCVCharacterStatusConstants.Verified Then
                resultsString = resultsString + mOCVTool.Results(0).FontModelName
              Else
                resultsString = resultsString & "?"
              End If
            End If
          Else
            mOCVResults.Add(Nothing)   ' space filler
            resultsString = resultsString & " "
            numBlanks = numBlanks + 1
          End If
        Next iPos
        ' Post the results to the control.
        PostResults(totalTime, sPattern, resultsString, sPattern.Length - numBlanks)
        btnProcess.Enabled = True
      Catch ex As CogException
        MessageBox.Show("ProcessSmartPlacement caught exception: " & ex.Message)
      Catch ex As Exception
        MessageBox.Show("ProcessSmartPlacement caught exception: " & ex.Message)
      Finally
        mCurrentImage = IIf(mCurrentImage + 1 >= mImageFile.Count, 0, mCurrentImage + 1)
        Me.ResetCursor()
      End Try
    End Sub
    ''' <summary> 
    ''' This subroutine is the run-time processing for the standard method.
    ''' </summary>
    Private Sub ProcessStandard()
      Try
        Me.Cursor = Cursors.WaitCursor
        btnProcess.Enabled = False

        ' clear old result list
        mOCVResults.Clear()

        ' remove old graphics if they exist
        mDisplay.StaticGraphics.Clear()

        ' Get the image from the CogImageFile operator
        Dim mImage As ICogImage = mImageFile(mCurrentImage)
        ' Display it on my control's CogDisplay
        mDisplay.Image = mImage

        ' Get the next record in the database
        Dim sPattern As String = mOCVStringsToVerify(mCurrentImage)

        If Not mOCVTool Is Nothing Then mOCVTool.Dispose()
        mOCVTool = New CogOCVTool        'first, make an OCV tool
        mOCVTool.Pattern.Font = mOCFont  ' assign the font
        mOCVTool.InputImage = mImage     ' assign the image
        ' For each position in the string get the character at that position
        ' and add it to the pattern.
        For iPos As Integer = 0 To sPattern.Length - 1
          Dim mCharPosition As CogOCVCharacterPosition = New CogOCVCharacterPosition
          Dim thisChar As String = sPattern.Substring(iPos, 1)
          mCharPosition.AddFontModel(thisChar, 0)
          ' Set position, rotation, scaling and associated uncertainty parameters
          ' Note that in this example the uncertainties are the same for each position,
          ' so I'm setting UsePatternXXXX = true.
          mCharPosition.UsePatternCharacterUncertainties = True
          mCharPosition.UsePatternRunTimeCharacterThresholds = True
          mCharPosition.Rotation = 0
          mCharPosition.Scaling = 1
          mCharPosition.TranslationY = mPatternOriginY
          ' Note that this example assumes that the string to verify is positioned horizontally so
          ' when laying out the pattern, the position of the next character is
          ' fontWidth pixels away (in the X direction) from the current character. So
          ' the first character is at patternOriginX, the second at
          ' patternOriginX + 1 * fontWidth, the third at patternOriginX + 2 * fontWidth, etc.
          mCharPosition.TranslationX = mPatternOriginX + iPos * mFontWidth
          ' Add this position to the pattern.
          mOCVTool.Pattern.CharacterPositions.Add(mCharPosition)
        Next iPos
        ' Set uncertainties for translation, rotation, and scale.
        mOCVTool.Pattern.CharacterTranslationUncertainty = 4 'pixels
        mOCVTool.Pattern.CharacterRotationUncertainty = 0.0  'radians
        mOCVTool.Pattern.CharacterScalingUncertainty = 0.0   'percentage
        ' Set accept and confusion thresholds
        mOCVTool.Pattern.RunTimeCharacterAcceptThreshold = 0.5
        mOCVTool.Pattern.RunTimeCharacterConfidenceThreshold = 0.0

        ' Get the time it takes to train the pattern. Note that we need to count this time
        ' towards overall pattern verification time since we are changing the pattern with each
        ' new pattern/image we verify.
        Dim mTimer As CogStopwatch = New CogStopwatch
        mTimer.Reset()
        mTimer.Start()
        mOCVTool.Pattern.Train()
        mTimer.Stop()

        ' Now let's set up the runtime info
        mOCVTool.PatternPosition.ExpectedPose.TranslationX = 0
        mOCVTool.PatternPosition.ExpectedPose.TranslationY = 0
        mOCVTool.PatternPosition.ExpectedPose.Rotation = 0
        mOCVTool.PatternPosition.ExpectedPose.Scaling = 1
        mOCVTool.PatternPosition.SelectedSpaceName = mImage.SelectedSpaceName
        mOCVTool.PatternPosition.TranslationUncertainty = 4
        mOCVTool.PatternPosition.RotationUncertainty = 0
        mOCVTool.PatternPosition.ScalingUncertainty = 0
        ' All set, so run the tool
        mOCVTool.Run()
        Dim resultsString As String = ""
        If (mOCVTool.RunStatus.Exception Is Nothing) Then
          For iPos As Integer = 0 To sPattern.Length - 1
            If mOCVTool.Results(iPos).Status = CogOCVCharacterStatusConstants.Verified Then
              resultsString = resultsString + mOCVTool.Results(iPos).FontModelName
            Else
              resultsString = resultsString & "?"
            End If
            mOCVResults.Add(mOCVTool.Results(iPos))
          Next iPos
        End If
        ' Post the results to the control.
        PostResults(mOCVTool.RunStatus.ProcessingTime + mTimer.Milliseconds, sPattern, resultsString, sPattern.Length)
        btnProcess.Enabled = True
      Catch ex As CogException
        MessageBox.Show("ProcessStandard caught exception: " & ex.Message)
      Catch ex As Exception
        MessageBox.Show("ProcessStandard caught exception: " & ex.Message)
      Finally
        mCurrentImage = IIf(mCurrentImage + 1 >= mImageFile.Count, 0, mCurrentImage + 1)
        Me.ResetCursor()
      End Try
    End Sub
    ''' <summary>
    ''' This method called from menu item event handlers
    ''' </summary>
        ''' <param name="selection"></param>
    Private Sub SelectRunMode(ByVal selection As MenuOptionConstants)
      mRunOption = selection
      mCurrentImage = 0    ' reset the image index to start from beginning
      Select Case (selection)
        Case MenuOptionConstants.MultipleModels : SelectMultipleModels()
        Case MenuOptionConstants.SmartPlacement : SelectSmartPlacement()
        Case MenuOptionConstants.Standard : SelectStandard()
        Case Else : mRunOption = MenuOptionConstants.None
      End Select
    End Sub
    ''' <summary>
    ''' This method prepares MultipleModels tests
    ''' </summary>
    Private Sub SelectMultipleModels()
      Try
        ' set cursor to be busy state
        Me.Cursor = Cursors.WaitCursor
        btnProcess.Enabled = False
        ' dispose the current OCV tool if there is one. We will create a new one for
        ' each running mode
        If Not mOCVTool Is Nothing Then mOCVTool.Dispose()
        ' create a new OCV Tool
        mOCVTool = New CogOCVTool
        txtInfo.Text = "This example uses multiple models at each position. " & _
                      "It is useful for applications in which the number " & _
                      "of characters and their positions are known, but each " & _
                      "position may take on a number of character values.  " & _
                      "Examples include serial number or credit card validation " & _
                      "where fixed-sized font models are employed.  It is slower " & _
                      "than Smart Placement, because it has to evaluate many models " & _
                      "at each position. However, it is usually faster than the Standard technique." & _
                      "Note that numeric processing is much faster " & _
                      "than alpha since there are fewer models in the numeric font than the alpha font."
        txtDescription.Text = "Multiple Models"

        mOCVTool.Pattern.Font = mOCFont
        ' For each position in the string...
        For iPos As Integer = 0 To mMaxStringLength - 1
          Dim mCharPosition As CogOCVCharacterPosition = New CogOCVCharacterPosition
          ' Add every model to each position except don't add a blank model to the first
          ' position due to a bug.
          For iModel As Integer = 0 To mOCVTool.Pattern.Font.FontModels.Count - 1
            If iPos <> 0 Or (iPos = 0 And mOCFont.FontModels(iModel).Type <> CogOCFontModelTypeConstants.Blank) Then
              mCharPosition.AddFontModel(mOCFont.FontModels(iModel).Name, mOCFont.FontModels(iModel).Instance)
            End If
          Next iModel
          ' Set position, rotation, scaling and associated uncertainty parameters
          ' Note that in this example the uncertainties are the same for each position,
          ' so I'm setting UsePatternXXXX = true.
          mCharPosition.UsePatternCharacterUncertainties = True
          mCharPosition.UsePatternRunTimeCharacterThresholds = True
          mCharPosition.Rotation = 0
          mCharPosition.Scaling = 1
          mCharPosition.TranslationY = mPatternOriginY
          ' Note that this example assumes that the string to verify is positioned horizontally.
          ' Position the next character to be fontWidth away from my current character.
          mCharPosition.TranslationX = mPatternOriginX + iPos * mFontWidth
          ' Add this position to my pattern.
          mOCVTool.Pattern.CharacterPositions.Add(mCharPosition)
        Next iPos
        ' Set overall pattern uncertainties for translation, rotation, and scale.
        mOCVTool.Pattern.CharacterTranslationUncertainty = 4 'pixels
        mOCVTool.Pattern.CharacterRotationUncertainty = 0.0  ' radians
        mOCVTool.Pattern.CharacterScalingUncertainty = 0.0   'percentage
        ' Set accept and confusion thresholds
        mOCVTool.Pattern.RunTimeCharacterAcceptThreshold = 0.5
        mOCVTool.Pattern.RunTimeCharacterConfidenceThreshold = 0.0
        mOCVTool.Pattern.Train()

        btnProcess.Enabled = True
      Catch ex As CogException
        MessageBox.Show(ex.Message, "OCVPatternChange")
      Catch ex As Exception
        MessageBox.Show(ex.Message, "OCVPatternChange")
      Finally
        ' reset cursor
        Me.ResetCursor()
      End Try
    End Sub
    ''' <summary>
    ''' This method prepares SmartPlacement tests
    ''' </summary>
    Private Sub SelectSmartPlacement()
      Try
        ' set cursor to be busy state
        Me.Cursor = Cursors.WaitCursor
        btnProcess.Enabled = False

        txtInfo.Text = "This example uses a single model at each position. It employs a different pattern " & _
                "at each position, which is dynamically placed at run-time based on the previous found model.  " & _
                "It is useful for applications in which the number " & _
                "of characters and their positions are known, but each " & _
                "position may take on a number of character values.  " & _
                "Examples include serial number or credit card validation " & _
                "where fixed-sized font models are employed.  It is faster than " & _
                "the Multiple Models or Standard techniques, because each pattern is pre-trained and " & _
                "it has to evaluate only one model " & _
                "at each position.  However, it is more prone to positioning errors."
        txtDescription.Text = "Smart Placement"
        Dim iPattern As Integer = 0
        For iModel As Integer = 0 To mOCFont.FontModels.Count - 1
          Dim mCharPosition As CogOCVCharacterPosition = New CogOCVCharacterPosition
          Dim model As CogOCFontModel = mOCFont.FontModels(iModel)
          ' Note that we don't create a blank pattern - it's not allowed in OCV. Instead,
          ' we will skip blank positions at run-time.
          If model.Type <> CogOCFontModelTypeConstants.Blank Then
            ' mSmartPatterns holds all the trained patterns. There is one pattern for
            ' each character model.  The only thing in a given pattern is a single
            ' character model.
            Dim pattern As CogOCVPattern = mSmartPatterns(iPattern)
            pattern.Font = mOCFont
            mCharPosition.AddFontModel(model.Name, model.Instance)
            ' Set position, rotation, scaling and associated uncertainty parameters
            ' Note that in this example the uncertainties are the same for each position,
            ' so I'm setting UsePatternXXXX = true.
            mCharPosition.UsePatternCharacterUncertainties = True
            mCharPosition.UsePatternRunTimeCharacterThresholds = True
            mCharPosition.Rotation = 0
            mCharPosition.Scaling = 1
            mCharPosition.TranslationX = 0
            mCharPosition.TranslationY = 0
            ' first, clean out the pattern
            pattern.CharacterPositions.Clear()
            ' Add the single character model.
            pattern.CharacterPositions.Add(mCharPosition)
            ' Set uncertainties for translation, rotation, and scale.
            pattern.CharacterTranslationUncertainty = 2 'pixels
            pattern.CharacterRotationUncertainty = 0.0  'radians
            pattern.CharacterScalingUncertainty = 0.0   'percentage
            ' Set accept and confusion thresholds
            pattern.RunTimeCharacterAcceptThreshold = 0.5
            pattern.RunTimeCharacterConfidenceThreshold = 0.0
            ' Train this pattern.
            pattern.Train()
            iPattern = iPattern + 1
          End If
        Next iModel
        btnProcess.Enabled = True
      Catch ex As CogException
        MessageBox.Show(ex.Message, "OCVPatternChange")
      Catch ex As Exception
        MessageBox.Show(ex.Message, "OCVPatternChange")
      Finally
        ' reset cursor
        Me.ResetCursor()
      End Try
    End Sub
    ''' <summary>
    ''' This method prepares Standard tests
    ''' </summary>
    Private Sub SelectStandard()
      Try
        ' set cursor to be busy state
        Me.Cursor = Cursors.WaitCursor
        btnProcess.Enabled = False
        ' dispose the current OCV tool if there is one. We will create a new one for
        ' each running mode
        If Not mOCVTool Is Nothing Then mOCVTool.Dispose()
        ' create a new OCV Tool
        mOCVTool = New CogOCVTool

        txtInfo.Text = "This example uses standard OCV for each pattern. " & _
              "For each new pattern, an OCV tool is trained and run. " & _
              "The total pattern time is the sum of the train time and " & _
              "run time.  "
        txtDescription.Text = "Standard"
        btnProcess.Enabled = True
      Catch ex As CogException
        MessageBox.Show(ex.Message, "OCVPatternChange")
      Catch ex As Exception
        MessageBox.Show(ex.Message, "OCVPatternChange")
      Finally
        ' reset cursor
        Me.ResetCursor()
      End Try
    End Sub
    ''' <summary>
    ''' This subroutine displays the graphics on the control
    ''' </summary>
    ''' <param name="resultString">result string</param>
    Private Sub DisplayGraphics(ByVal resultString As String)
      mDisplay.DrawingEnabled = False
      For iPos As Integer = 0 To resultString.Length - 1
        ' Get the center, width, height of the found model.
        ' Also compare the font model to the model to verify.
        ' If correct, display in a green rectangle, else display in red.
        If resultString.Substring(iPos, 1) <> " " Or mRunOption <> MenuOptionConstants.SmartPlacement Then
          Dim result As CogOCVResult = mOCVResults(iPos)
          Dim model As CogOCFontModel = mOCFont.FontModels.GetFontModelByNameInstance(result.FontModelName, result.FontModelInstance)
          Dim centerX As Double = result.GetPose().TranslationX
          Dim centerY As Double = result.GetPose().TranslationY
          Dim width As Integer, height As Integer
          If model.Type = CogOCFontModelTypeConstants.Normal Then
            ' normal font
            width = model.Image.Width
            height = model.Image.Height
          Else
            ' blank font
            width = model.BlankWidth
            height = model.BlankHeight
          End If
          Dim rect As CogRectangle = New CogRectangle
          rect.SetCenterWidthHeight(centerX, centerY, width, height)
          rect.SelectedSpaceName = mOCVTool.InputImage.SelectedSpaceName
          rect.GraphicDOFEnable = CogRectangleDOFConstants.None
          If result.Status = CogOCVCharacterStatusConstants.Verified Then
            rect.Color = CogColorConstants.Green
          Else
            rect.Color = CogColorConstants.Red
          End If
          mDisplay.StaticGraphics.Add(rect, "Results")
        End If
      Next iPos
      mDisplay.DrawingEnabled = True
    End Sub
    ''' <summary>
    ''' This method posts the results to the control and insures that the graphics are
    ''' presented
    ''' </summary>
    ''' <param name="totalTime"></param>
    ''' <param name="stringToVerify"></param>
    ''' <param name="resultString"></param>
    ''' <param name="stringLength"></param>
    Private Sub PostResults(ByVal totalTime As Double, ByVal stringToVerify As String, ByVal resultString As String, ByVal stringLength As Integer)
      Dim perCharTime As Double = totalTime / stringLength
      ' Post the processing time
      txtTimeTotal.Text = totalTime.ToString("F2")
      txtTimePerChar.Text = perCharTime.ToString("F2")

      ' Post the results string
      txtResult.Text = resultString
      ' Draw graphics on screen
      DisplayGraphics(resultString)
    End Sub
#End Region

  End Class
End Namespace
