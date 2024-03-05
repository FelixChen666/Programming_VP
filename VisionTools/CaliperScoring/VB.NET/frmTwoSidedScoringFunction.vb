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

' This sample demonstrates the use of a caliper tool and a two sided scoring function.
' It also shows how to use / update the scoring function edit control in the GUI.
' The sample uses the objects and interfaces defined in the Cognex Core and
' Cognex Image type libraries.
'
' This program assumes that you have some knowledge of Visual Basic and VisionPro
' programming.
'
' The following steps show how to edges using caliper and a scoring function.
' Step 1) Create a CogCaliper
' Step 2) Load an image from an IDB file.
' Step 3) Setup a two sided scoring function based on the size (width) of an edge pair.
' Step 4) Display the width score, allow the user to modify the scoring function and
'         re-run the caliper tool as often as they like.

Option Explicit On 
Imports Cognex.VisionPro                'need to access basic VisionPro functionality
Imports Cognex.VisionPro.ImageFile      'need to access ImageFileTool 
Imports Cognex.VisionPro.Caliper        'need to access CogCaliperTool
Namespace SampleCaliperTwoSidedScoringFunction
  Public Class frmTwoSidedScoring
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
    Friend WithEvents CogDisplay1 As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents txtResults As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtX1 As System.Windows.Forms.TextBox
    Friend WithEvents txtXC As System.Windows.Forms.TextBox
    Friend WithEvents txtY0 As System.Windows.Forms.TextBox
    Friend WithEvents txtY1 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtY1H As System.Windows.Forms.TextBox
    Friend WithEvents txtY0H As System.Windows.Forms.TextBox
    Friend WithEvents txtXCH As System.Windows.Forms.TextBox
    Friend WithEvents txtX1H As System.Windows.Forms.TextBox
    Friend WithEvents txtX0H As System.Windows.Forms.TextBox
    Friend WithEvents txtX0 As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label

        Friend WithEvents CogScoringEdit1 As Cognex.VisionPro.Caliper.CogScoringEditV2
    Friend WithEvents ExpectedWidth As Cognex.VisionPro.CogNumberBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmTwoSidedScoring))
      Me.CogDisplay1 = New Cognex.VisionPro.Display.CogDisplay
      Me.btnRun = New System.Windows.Forms.Button
      Me.txtResults = New System.Windows.Forms.TextBox
      Me.GroupBox1 = New System.Windows.Forms.GroupBox
      Me.Label5 = New System.Windows.Forms.Label
      Me.Label4 = New System.Windows.Forms.Label
      Me.Label3 = New System.Windows.Forms.Label
      Me.Label2 = New System.Windows.Forms.Label
      Me.Label1 = New System.Windows.Forms.Label
      Me.txtY1 = New System.Windows.Forms.TextBox
      Me.txtY0 = New System.Windows.Forms.TextBox
      Me.txtXC = New System.Windows.Forms.TextBox
      Me.txtX1 = New System.Windows.Forms.TextBox
      Me.txtX0 = New System.Windows.Forms.TextBox
      Me.GroupBox3 = New System.Windows.Forms.GroupBox
      Me.Label6 = New System.Windows.Forms.Label
      Me.Label7 = New System.Windows.Forms.Label
      Me.Label8 = New System.Windows.Forms.Label
      Me.Label9 = New System.Windows.Forms.Label
      Me.Label10 = New System.Windows.Forms.Label
      Me.txtY1H = New System.Windows.Forms.TextBox
      Me.txtY0H = New System.Windows.Forms.TextBox
      Me.txtXCH = New System.Windows.Forms.TextBox
      Me.txtX1H = New System.Windows.Forms.TextBox
      Me.txtX0H = New System.Windows.Forms.TextBox
      Me.Label11 = New System.Windows.Forms.Label
            Me.CogScoringEdit1 = New Cognex.VisionPro.Caliper.CogScoringEditV2
      Me.ExpectedWidth = New Cognex.VisionPro.CogNumberBox
      Me.TextBox1 = New System.Windows.Forms.TextBox
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.GroupBox1.SuspendLayout()
      Me.GroupBox3.SuspendLayout()
      CType(Me.ExpectedWidth, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'CogDisplay1
      '
      Me.CogDisplay1.Location = New System.Drawing.Point(8, 16)
      Me.CogDisplay1.Name = "CogDisplay1"
      Me.CogDisplay1.OcxState = CType(resources.GetObject("CogDisplay1.OcxState"), System.Windows.Forms.AxHost.State)
      Me.CogDisplay1.Size = New System.Drawing.Size(624, 264)
      Me.CogDisplay1.TabIndex = 0
      '
      'btnRun
      '
      Me.btnRun.Location = New System.Drawing.Point(664, 16)
      Me.btnRun.Name = "btnRun"
      Me.btnRun.Size = New System.Drawing.Size(104, 23)
      Me.btnRun.TabIndex = 1
      Me.btnRun.Text = "Run"
      '
      'txtResults
      '
      Me.txtResults.Location = New System.Drawing.Point(664, 56)
      Me.txtResults.Multiline = True
      Me.txtResults.Name = "txtResults"
      Me.txtResults.Size = New System.Drawing.Size(104, 88)
      Me.txtResults.TabIndex = 2
      Me.txtResults.Text = ""
      '
      'GroupBox1
      '
      Me.GroupBox1.Controls.Add(Me.Label5)
      Me.GroupBox1.Controls.Add(Me.Label4)
      Me.GroupBox1.Controls.Add(Me.Label3)
      Me.GroupBox1.Controls.Add(Me.Label2)
      Me.GroupBox1.Controls.Add(Me.Label1)
      Me.GroupBox1.Controls.Add(Me.txtY1)
      Me.GroupBox1.Controls.Add(Me.txtY0)
      Me.GroupBox1.Controls.Add(Me.txtXC)
      Me.GroupBox1.Controls.Add(Me.txtX1)
      Me.GroupBox1.Controls.Add(Me.txtX0)
      Me.GroupBox1.Location = New System.Drawing.Point(8, 352)
      Me.GroupBox1.Name = "GroupBox1"
      Me.GroupBox1.Size = New System.Drawing.Size(376, 136)
      Me.GroupBox1.TabIndex = 3
      Me.GroupBox1.TabStop = False
      Me.GroupBox1.Text = "Left Side Scoring Function"
      '
      'Label5
      '
      Me.Label5.Location = New System.Drawing.Point(240, 80)
      Me.Label5.Name = "Label5"
      Me.Label5.Size = New System.Drawing.Size(24, 16)
      Me.Label5.TabIndex = 9
      Me.Label5.Text = "Y1"
      '
      'Label4
      '
      Me.Label4.Location = New System.Drawing.Point(168, 80)
      Me.Label4.Name = "Label4"
      Me.Label4.Size = New System.Drawing.Size(24, 16)
      Me.Label4.TabIndex = 8
      Me.Label4.Text = "Y0"
      '
      'Label3
      '
      Me.Label3.Location = New System.Drawing.Point(104, 80)
      Me.Label3.Name = "Label3"
      Me.Label3.Size = New System.Drawing.Size(24, 16)
      Me.Label3.TabIndex = 7
      Me.Label3.Text = "XC"
      '
      'Label2
      '
      Me.Label2.Location = New System.Drawing.Point(56, 80)
      Me.Label2.Name = "Label2"
      Me.Label2.Size = New System.Drawing.Size(24, 16)
      Me.Label2.TabIndex = 6
      Me.Label2.Text = "X1"
      '
      'Label1
      '
      Me.Label1.Location = New System.Drawing.Point(8, 80)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(24, 16)
      Me.Label1.TabIndex = 5
      Me.Label1.Text = "X0"
      '
      'txtY1
      '
      Me.txtY1.Location = New System.Drawing.Point(240, 40)
      Me.txtY1.Name = "txtY1"
      Me.txtY1.Size = New System.Drawing.Size(48, 20)
      Me.txtY1.TabIndex = 4
      Me.txtY1.Text = "0.5"
      '
      'txtY0
      '
      Me.txtY0.Location = New System.Drawing.Point(168, 40)
      Me.txtY0.Name = "txtY0"
      Me.txtY0.Size = New System.Drawing.Size(48, 20)
      Me.txtY0.TabIndex = 3
      Me.txtY0.Text = "1.0"
      '
      'txtXC
      '
      Me.txtXC.Location = New System.Drawing.Point(104, 40)
      Me.txtXC.Name = "txtXC"
      Me.txtXC.Size = New System.Drawing.Size(40, 20)
      Me.txtXC.TabIndex = 2
      Me.txtXC.Text = "-90"
      '
      'txtX1
      '
      Me.txtX1.Location = New System.Drawing.Point(56, 40)
      Me.txtX1.Name = "txtX1"
      Me.txtX1.Size = New System.Drawing.Size(40, 20)
      Me.txtX1.TabIndex = 1
      Me.txtX1.Text = "-50"
      '
      'txtX0
      '
      Me.txtX0.Location = New System.Drawing.Point(8, 40)
      Me.txtX0.Name = "txtX0"
      Me.txtX0.Size = New System.Drawing.Size(40, 20)
      Me.txtX0.TabIndex = 0
      Me.txtX0.Text = "0.0"
      '
      'GroupBox3
      '
      Me.GroupBox3.Controls.Add(Me.Label6)
      Me.GroupBox3.Controls.Add(Me.Label7)
      Me.GroupBox3.Controls.Add(Me.Label8)
      Me.GroupBox3.Controls.Add(Me.Label9)
      Me.GroupBox3.Controls.Add(Me.Label10)
      Me.GroupBox3.Controls.Add(Me.txtY1H)
      Me.GroupBox3.Controls.Add(Me.txtY0H)
      Me.GroupBox3.Controls.Add(Me.txtXCH)
      Me.GroupBox3.Controls.Add(Me.txtX1H)
      Me.GroupBox3.Controls.Add(Me.txtX0H)
      Me.GroupBox3.Location = New System.Drawing.Point(400, 352)
      Me.GroupBox3.Name = "GroupBox3"
      Me.GroupBox3.Size = New System.Drawing.Size(376, 136)
      Me.GroupBox3.TabIndex = 5
      Me.GroupBox3.TabStop = False
      Me.GroupBox3.Text = "Right Side Scoring Function"
      '
      'Label6
      '
      Me.Label6.Location = New System.Drawing.Point(240, 80)
      Me.Label6.Name = "Label6"
      Me.Label6.Size = New System.Drawing.Size(32, 16)
      Me.Label6.TabIndex = 9
      Me.Label6.Text = "Y1H"
      '
      'Label7
      '
      Me.Label7.Location = New System.Drawing.Point(168, 80)
      Me.Label7.Name = "Label7"
      Me.Label7.Size = New System.Drawing.Size(32, 16)
      Me.Label7.TabIndex = 8
      Me.Label7.Text = "Y0H"
      '
      'Label8
      '
      Me.Label8.Location = New System.Drawing.Point(104, 80)
      Me.Label8.Name = "Label8"
      Me.Label8.Size = New System.Drawing.Size(32, 16)
      Me.Label8.TabIndex = 7
      Me.Label8.Text = "XCH"
      '
      'Label9
      '
      Me.Label9.Location = New System.Drawing.Point(56, 80)
      Me.Label9.Name = "Label9"
      Me.Label9.Size = New System.Drawing.Size(32, 16)
      Me.Label9.TabIndex = 6
      Me.Label9.Text = "X1H"
      '
      'Label10
      '
      Me.Label10.Location = New System.Drawing.Point(8, 80)
      Me.Label10.Name = "Label10"
      Me.Label10.Size = New System.Drawing.Size(32, 16)
      Me.Label10.TabIndex = 5
      Me.Label10.Text = "X0H"
      '
      'txtY1H
      '
      Me.txtY1H.Location = New System.Drawing.Point(240, 40)
      Me.txtY1H.Name = "txtY1H"
      Me.txtY1H.Size = New System.Drawing.Size(48, 20)
      Me.txtY1H.TabIndex = 4
      Me.txtY1H.Text = "0.5"
      '
      'txtY0H
      '
      Me.txtY0H.Location = New System.Drawing.Point(168, 40)
      Me.txtY0H.Name = "txtY0H"
      Me.txtY0H.Size = New System.Drawing.Size(48, 20)
      Me.txtY0H.TabIndex = 3
      Me.txtY0H.Text = "1.0"
      '
      'txtXCH
      '
      Me.txtXCH.Location = New System.Drawing.Point(104, 40)
      Me.txtXCH.Name = "txtXCH"
      Me.txtXCH.Size = New System.Drawing.Size(40, 20)
      Me.txtXCH.TabIndex = 2
      Me.txtXCH.Text = "90.0"
      '
      'txtX1H
      '
      Me.txtX1H.Location = New System.Drawing.Point(56, 40)
      Me.txtX1H.Name = "txtX1H"
      Me.txtX1H.Size = New System.Drawing.Size(40, 20)
      Me.txtX1H.TabIndex = 1
      Me.txtX1H.Text = "50.0"
      '
      'txtX0H
      '
      Me.txtX0H.Location = New System.Drawing.Point(8, 40)
      Me.txtX0H.Name = "txtX0H"
      Me.txtX0H.Size = New System.Drawing.Size(40, 20)
      Me.txtX0H.TabIndex = 0
      Me.txtX0H.Text = "0.0"
      '
      'Label11
      '
      Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.Label11.Location = New System.Drawing.Point(632, 168)
      Me.Label11.Name = "Label11"
      Me.Label11.Size = New System.Drawing.Size(168, 32)
      Me.Label11.TabIndex = 7
      Me.Label11.Text = "Expected Part Width "
      '
      'CogScoringEdit1
      '
      Me.CogScoringEdit1.Enabled = True
      Me.CogScoringEdit1.Location = New System.Drawing.Point(16, 288)
      Me.CogScoringEdit1.Name = "CogScoringEdit1"
      Me.CogScoringEdit1.Size = New System.Drawing.Size(464, 55)
      Me.CogScoringEdit1.TabIndex = 10
      '
      'ExpectedWidth
      '
      Me.ExpectedWidth.AllowDrop = True
      Me.ExpectedWidth.Electric = False
      Me.ExpectedWidth.Location = New System.Drawing.Point(664, 232)
      Me.ExpectedWidth.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
      Me.ExpectedWidth.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
      Me.ExpectedWidth.Name = "ExpectedWidth"
      Me.ExpectedWidth.Path = Nothing
      Me.ExpectedWidth.ShowToolTips = False
      Me.ExpectedWidth.Subject = Nothing
      Me.ExpectedWidth.TabIndex = 11
      Me.ExpectedWidth.ToolTipText = Nothing
      Me.ExpectedWidth.UseAngleUnit = True
      Me.ExpectedWidth.Value = New Decimal(New Integer() {294, 0, 0, 0})
      '
      'TextBox1
      '
      Me.TextBox1.AcceptsReturn = True
      Me.TextBox1.AcceptsTab = True
      Me.TextBox1.Location = New System.Drawing.Point(16, 504)
      Me.TextBox1.Multiline = True
      Me.TextBox1.Name = "TextBox1"
      Me.TextBox1.Size = New System.Drawing.Size(768, 20)
      Me.TextBox1.TabIndex = 12
      Me.TextBox1.Text = "Sample Description: show the user how to setup a two sided scoring function to aff" & _
      "ect the tool behavior (score)."
      '
      'frmTwoSidedScoring
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(816, 574)
      Me.Controls.Add(Me.TextBox1)
      Me.Controls.Add(Me.ExpectedWidth)
      Me.Controls.Add(Me.CogScoringEdit1)
      Me.Controls.Add(Me.Label11)
      Me.Controls.Add(Me.GroupBox3)
      Me.Controls.Add(Me.GroupBox1)
      Me.Controls.Add(Me.txtResults)
      Me.Controls.Add(Me.btnRun)
      Me.Controls.Add(Me.CogDisplay1)
      Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.Name = "frmTwoSidedScoring"
      Me.Text = "Shows how to setup a caliper tool with a two sided scoring function."
      CType(Me.CogDisplay1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.GroupBox1.ResumeLayout(False)
      Me.GroupBox3.ResumeLayout(False)
      CType(Me.ExpectedWidth, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region " Private vars"

    Private mTool As CogCaliperTool                            ' caliper tool
    Private ImageFile As CogImageFileTool                      ' image file tool
    Private InteractiveRectangleGraphic As Cognex.VisionPro.CogRectangleAffine  ' region of interest (ROI)
#End Region
#Region "Form and Controls Events"
    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click

      ' clear any old graphics
      CogDisplay1.StaticGraphics.Clear()

      update_scoring_function()
    End Sub

    Private Sub frmTwoSidedScoringFunction_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      ExpectedWidth.Enabled = True
      ' Step 1 - Create a CogCaliperTool.
      mTool = New CogCaliperTool

      ImageFile = New CogImageFileTool                           ' image file tool
      InteractiveRectangleGraphic = New CogRectangleAffine      ' region of interest (ROI)

      ' Get VPRO_ROOT from environment which is needed to locate bracket_std.idb.
      Const ImageFileName As String = "/Images/bracket_std.idb"
      Dim strBaseDir As String
      strBaseDir = Environ("VPRO_ROOT")
      If strBaseDir = "" Then
        DisplayErrorAndExit("Required environment variable VPRO_ROOT not set.")
      End If

      ' Step 2 - Load an image and create shapes.
      ' Temporarily create the image file tool to open bracket_std.idb.
      ImageFile.[Operator].Open(strBaseDir & ImageFileName, CogImageFileModeConstants.Read)

      ' We only need the first image
      mTool.InputImage = ImageFile.[Operator].Item(0)
      CogDisplay1.Image = ImageFile.[Operator].Item(0)

      ' Close the image file since we are going to use the same image.
      ImageFile.[Operator].Close()

      setup_interactive_region_of_interest()
      update_scoring_function()
    End Sub
    Private Sub frmTwoSidedScoring_closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
      If Not ImageFile Is Nothing Then ImageFile.Dispose()
    End Sub
#End Region
#Region "Form Helper Routines"
    Private Sub update_scoring_function()
      Try
        ' Step 3) Setup a two sided scoring function and the associated EditControl to view it.
        '         Notice that CogNumberExpectedWidth plays a large role in how the function
        '         is shaped and whether or not you find the set of edges you are looking for.

        ' setup an initial caliper Region Of Interest (ROI).
        mTool.Region.Color = CogColorConstants.Green
        mTool.Region.CenterX = InteractiveRectangleGraphic.CenterX
        mTool.Region.CenterY = InteractiveRectangleGraphic.CenterY
        mTool.Region.Rotation = InteractiveRectangleGraphic.Rotation
        mTool.Region.SideXLength = InteractiveRectangleGraphic.SideXLength
        mTool.Region.SideYLength = InteractiveRectangleGraphic.SideYLength

        ' setup expected polarities of the edge pair and expected width of the edge pair
        mTool.RunParams.Edge0Polarity = CogCaliperPolarityConstants.DarkToLight
        mTool.RunParams.Edge1Polarity = CogCaliperPolarityConstants.LightToDark
        mTool.RunParams.Edge0Position = -ExpectedWidth.Value / 2
        mTool.RunParams.Edge1Position = ExpectedWidth.Value / 2
        mTool.RunParams.EdgeMode = CogCaliperEdgeModeConstants.Pair

        ' Left Side scoring function
        Dim x0 As Double, x1 As Double, xc As Double, y0 As Double, y1 As Double
        x0 = Val(txtX0.Text)
        x1 = Val(txtX1.Text)
        xc = Val(txtXC.Text)
        y0 = Val(txtY0.Text)
        y1 = Val(txtY1.Text)

        ' Right Side scoring function
        Dim x0h As Double, x1h As Double, xch As Double, y0h As Double, y1h As Double
        x0h = Val(txtX0H.Text)
        x1h = Val(txtX1H.Text)
        xch = Val(txtXCH.Text)
        y0h = Val(txtY0H.Text)
        y1h = Val(txtY1H.Text)

        Dim TwoSidedScoreFunction As New CogCaliperScorerSizeDiffNormAsym
        TwoSidedScoreFunction.Enabled = True
        TwoSidedScoreFunction.SetXYParameters(x0, x1, xc, y0, y1, x0h, x1h, xch, y0h, y1h)
        mTool.RunParams.TwoEdgeScorers.Add(TwoSidedScoreFunction)

        ' This is the only line required to update the Edit Control in the GUI.
                CogScoringEdit1.Subject = TwoSidedScoreFunction

        ' Run the caliper tool
        mTool.RunParams.ContrastThreshold = 30
        mTool.RunParams.MaxResults = 5
        mTool.Run()

        ' Step 4) display the status, width and score here.
        txtResults.Text = ""
        If mTool.Results.Count > 0 Then
                    CogDisplay1.StaticGraphics.Add(mTool.Results.Item(0).CreateResultGraphics(CogCaliperResultGraphicConstants.All), "test")
          txtResults.Text = txtResults.Text & "PASSED" & Environment.NewLine
          txtResults.Text = txtResults.Text & "width = " & FormatNumber(mTool.Results.Item(0).Width, 2) & Environment.NewLine
          txtResults.Text = txtResults.Text & "score = " & FormatNumber(mTool.Results.Item(0).Score, 3) & Environment.NewLine
        Else
          txtResults.Text = "FAILED" & Environment.NewLine
          txtResults.Text = txtResults.Text & "no width" & Environment.NewLine
          txtResults.Text = txtResults.Text & "no score" & Environment.NewLine
        End If
      Catch ex As Exception
        MessageBox.Show(ex.Message)
      End Try
    End Sub
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Helper function.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Displays an error message and then exits the program.
    ' Call this when an unrecoverable error has occurred.
    Private Sub DisplayErrorAndExit(ByVal ErrorMsg As String)
      MessageBox.Show(ErrorMsg & Environment.NewLine & "Press OK to exit.")
      Me.Close()
      End        ' Quit if it is called from Form_Load
    End Sub
    Private Sub setup_interactive_region_of_interest()

      ' Give the user a rectangle to move around.
      InteractiveRectangleGraphic.CenterX = 320
      InteractiveRectangleGraphic.CenterY = 270
      InteractiveRectangleGraphic.SideXLength = 400
      InteractiveRectangleGraphic.SideYLength = 50
      InteractiveRectangleGraphic.Color = CogColorConstants.Green

      ' Set the graphic's degree of interactivity.  There is no mechanism for
      ' making an interactive graphic non-selectable without making it non-
      ' interactive.  If we make the selected color be the same as the
      ' unselected color, we can make the graphic appear to be unselectable.
      InteractiveRectangleGraphic.Interactive = True
      InteractiveRectangleGraphic.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position Or _
      CogRectangleAffineDOFConstants.Rotation Or CogRectangleAffineDOFConstants.Size
      InteractiveRectangleGraphic.LineStyle = CogGraphicLineStyleConstants.Solid

      Dim GenericInteractive As Cognex.VisionPro.ICogGraphicInteractive
      GenericInteractive = InteractiveRectangleGraphic
      GenericInteractive.MouseCursor = CogStandardCursorConstants.ManipulableGraphic
      GenericInteractive.SelectedLineStyle = CogGraphicLineStyleConstants.Solid
      GenericInteractive.DragLineStyle = CogGraphicLineStyleConstants.Solid
      ' Install the new graphic on the display.
      CogDisplay1.InteractiveGraphics.Add(InteractiveRectangleGraphic, "test", False)

    End Sub
#End Region
    

  End Class
End Namespace