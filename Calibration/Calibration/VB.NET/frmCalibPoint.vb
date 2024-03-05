'*******************************************************************************
'Copyright (C) 2004 Cognex Corporation
'
'Subject to Cognex Corporation's terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.
'*******************************************************************************
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This sample demonstrates the effect of applying a configurable coordinate
' space to an image. It should be used to understand how changing calibration
' points will effect the resulting coordinate space. The sample source
' code makes extensive use of the VisionPro calibration tool and the coordinate
' transforms necessary for mapping coordinates and other 2D objects between
' spaces.
'
' Calibration has two sets of points: uncalibrated and calibrated.
' Uncalibrated points in this sample are specified in image coordinates.
' Calibrated points indicate the corresponding coordinates in the coordinate
' space you are defining.
'
' Calibration points in this sample can only be modified while in setup mode.
' To enter setup mode, click the <Setup> button.  To set the calibration
' input points, enter their coordinates into the appropriate textboxes.  The
' last step in defining your coordinate space is to name the calibrated units.
' This is done by picking a unit from the calibrated units list, or typing a
' custom unit into the calibrated units textbox.  Naming the units is not
' required to use calibration.  In fact, VisionPro has no mechanism for
' naming the units of a defined space.  It does, however, provide a mechanism
' for naming your defined space.  This sample allows you to name the units of
' your defined space to make clear the purpose for using calibration.
' Changing the calibration unit name will in no way change the calibrated
' space.  Once finished setting up your space, click the <Apply> button.
'
' The rectangle and coordinate axes graphics provide a visual description
' of your coordinate space.  The coordinate axes indicates the origin,
' calibrated coordinate (0,0), of your space.  The rectangle indicates how
' your coordinate space maps to the image coordinate space. For example, if
' the rectangle is skewed, your coordinate space has skew.  If the
' rectangle's height appears to be equal in length to its width, but its
' labels indicate the difference to be large, your coordinate space has non-
' uniform scale.
'
' Now that you have a new coordinate space, move your mouse over the
' calibration display and note how its location is reported in both
' uncalibrated and calibrated coordinates.  If VisionPro calibration was
' able to produce your space with a decent fitting score (a transform was
' found that could properly map each uncalibrated coordinate to the
' corresponding calibrated coordinate), positioning your mouse over each
' point graphic should report mouse location coordinates that are
' roughly equivalent to those of the corresponding calibration point.
'
' As an exercise in using the calibration sample, reconfigure the calibration
' points such that the resulting coordinate space is in centimeter units.
' Hint: this should be possible without changing the uncalibrated coordinate
' points.
Option Explicit On 
Imports Cognex.VisionPro.CalibFix
Imports Cognex.VisionPro
Imports Cognex.VisionPro.Exceptions
Namespace SampleCalibrationPoint

  Public Class frmCalibPoint
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
    Friend WithEvents CalibrationPointsFrame As System.Windows.Forms.GroupBox
    Friend WithEvents SampleDescription As System.Windows.Forms.TextBox
    Friend WithEvents MouseLocationFrame As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnSetup As System.Windows.Forms.Button
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents btnDefault As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents CalibratedUnits As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextBox0 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextBox14 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox13 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox12 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox11 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox15 As System.Windows.Forms.TextBox
    Friend WithEvents MouseUncalibratedX As System.Windows.Forms.Label
    Friend WithEvents MouseCalibratedX As System.Windows.Forms.Label
    Friend WithEvents MouseUncalibratedY As System.Windows.Forms.Label
    Friend WithEvents MouseCalibratedY As System.Windows.Forms.Label
    Friend WithEvents CalibrationDisplay As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextBox16 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmCalibPoint))
            Me.CalibrationDisplay = New Cognex.VisionPro.Display.CogDisplay
            Me.CalibrationPointsFrame = New System.Windows.Forms.GroupBox
            Me.TextBox15 = New System.Windows.Forms.TextBox
            Me.TextBox14 = New System.Windows.Forms.TextBox
            Me.TextBox13 = New System.Windows.Forms.TextBox
            Me.TextBox12 = New System.Windows.Forms.TextBox
            Me.TextBox11 = New System.Windows.Forms.TextBox
            Me.TextBox10 = New System.Windows.Forms.TextBox
            Me.TextBox9 = New System.Windows.Forms.TextBox
            Me.TextBox8 = New System.Windows.Forms.TextBox
            Me.TextBox7 = New System.Windows.Forms.TextBox
            Me.TextBox6 = New System.Windows.Forms.TextBox
            Me.TextBox5 = New System.Windows.Forms.TextBox
            Me.TextBox4 = New System.Windows.Forms.TextBox
            Me.btnDefault = New System.Windows.Forms.Button
            Me.btnApply = New System.Windows.Forms.Button
            Me.btnSetup = New System.Windows.Forms.Button
            Me.Label6 = New System.Windows.Forms.Label
            Me.Label5 = New System.Windows.Forms.Label
            Me.TextBox3 = New System.Windows.Forms.TextBox
            Me.TextBox2 = New System.Windows.Forms.TextBox
            Me.TextBox1 = New System.Windows.Forms.TextBox
            Me.TextBox0 = New System.Windows.Forms.TextBox
            Me.Label4 = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.Label10 = New System.Windows.Forms.Label
            Me.Label11 = New System.Windows.Forms.Label
            Me.Label12 = New System.Windows.Forms.Label
            Me.TextBox16 = New System.Windows.Forms.TextBox
            Me.SampleDescription = New System.Windows.Forms.TextBox
            Me.MouseLocationFrame = New System.Windows.Forms.GroupBox
            Me.MouseCalibratedY = New System.Windows.Forms.Label
            Me.MouseUncalibratedY = New System.Windows.Forms.Label
            Me.MouseCalibratedX = New System.Windows.Forms.Label
            Me.MouseUncalibratedX = New System.Windows.Forms.Label
            Me.Label7 = New System.Windows.Forms.Label
            Me.Label8 = New System.Windows.Forms.Label
            Me.CalibratedUnits = New System.Windows.Forms.ComboBox
            Me.Label9 = New System.Windows.Forms.Label
            CType(Me.CalibrationDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.CalibrationPointsFrame.SuspendLayout()
            Me.MouseLocationFrame.SuspendLayout()
            Me.SuspendLayout()
            '
            'CalibrationDisplay
            '
            Me.CalibrationDisplay.Location = New System.Drawing.Point(24, 184)
            Me.CalibrationDisplay.Name = "CalibrationDisplay"
            Me.CalibrationDisplay.OcxState = CType(resources.GetObject("CalibrationDisplay.OcxState"), System.Windows.Forms.AxHost.State)
            Me.CalibrationDisplay.Size = New System.Drawing.Size(688, 208)
            Me.CalibrationDisplay.TabIndex = 0
            '
            'CalibrationPointsFrame
            '
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox15)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox14)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox13)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox12)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox11)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox10)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox9)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox8)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox7)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox6)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox5)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox4)
            Me.CalibrationPointsFrame.Controls.Add(Me.btnDefault)
            Me.CalibrationPointsFrame.Controls.Add(Me.btnApply)
            Me.CalibrationPointsFrame.Controls.Add(Me.btnSetup)
            Me.CalibrationPointsFrame.Controls.Add(Me.Label6)
            Me.CalibrationPointsFrame.Controls.Add(Me.Label5)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox3)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox2)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox1)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox0)
            Me.CalibrationPointsFrame.Controls.Add(Me.Label4)
            Me.CalibrationPointsFrame.Controls.Add(Me.Label3)
            Me.CalibrationPointsFrame.Controls.Add(Me.Label2)
            Me.CalibrationPointsFrame.Controls.Add(Me.Label1)
            Me.CalibrationPointsFrame.Controls.Add(Me.Label10)
            Me.CalibrationPointsFrame.Controls.Add(Me.Label11)
            Me.CalibrationPointsFrame.Controls.Add(Me.Label12)
            Me.CalibrationPointsFrame.Controls.Add(Me.TextBox16)
            Me.CalibrationPointsFrame.Location = New System.Drawing.Point(16, 8)
            Me.CalibrationPointsFrame.Name = "CalibrationPointsFrame"
            Me.CalibrationPointsFrame.Size = New System.Drawing.Size(408, 160)
            Me.CalibrationPointsFrame.TabIndex = 1
            Me.CalibrationPointsFrame.TabStop = False
            Me.CalibrationPointsFrame.Text = "Calibration Points"
            '
            'TextBox15
            '
            Me.TextBox15.Location = New System.Drawing.Point(256, 128)
            Me.TextBox15.Name = "TextBox15"
            Me.TextBox15.Size = New System.Drawing.Size(40, 20)
            Me.TextBox15.TabIndex = 41
            Me.TextBox15.Text = ""
            '
            'TextBox14
            '
            Me.TextBox14.Location = New System.Drawing.Point(256, 104)
            Me.TextBox14.Name = "TextBox14"
            Me.TextBox14.Size = New System.Drawing.Size(40, 20)
            Me.TextBox14.TabIndex = 40
            Me.TextBox14.Text = ""
            '
            'TextBox13
            '
            Me.TextBox13.Location = New System.Drawing.Point(256, 80)
            Me.TextBox13.Name = "TextBox13"
            Me.TextBox13.Size = New System.Drawing.Size(40, 20)
            Me.TextBox13.TabIndex = 39
            Me.TextBox13.Text = ""
            '
            'TextBox12
            '
            Me.TextBox12.Location = New System.Drawing.Point(256, 48)
            Me.TextBox12.Name = "TextBox12"
            Me.TextBox12.Size = New System.Drawing.Size(40, 20)
            Me.TextBox12.TabIndex = 38
            Me.TextBox12.Text = ""
            '
            'TextBox11
            '
            Me.TextBox11.Location = New System.Drawing.Point(208, 128)
            Me.TextBox11.Name = "TextBox11"
            Me.TextBox11.Size = New System.Drawing.Size(40, 20)
            Me.TextBox11.TabIndex = 37
            Me.TextBox11.Text = ""
            '
            'TextBox10
            '
            Me.TextBox10.Location = New System.Drawing.Point(208, 104)
            Me.TextBox10.Name = "TextBox10"
            Me.TextBox10.Size = New System.Drawing.Size(40, 20)
            Me.TextBox10.TabIndex = 36
            Me.TextBox10.Text = ""
            '
            'TextBox9
            '
            Me.TextBox9.Location = New System.Drawing.Point(208, 80)
            Me.TextBox9.Name = "TextBox9"
            Me.TextBox9.Size = New System.Drawing.Size(40, 20)
            Me.TextBox9.TabIndex = 35
            Me.TextBox9.Text = ""
            '
            'TextBox8
            '
            Me.TextBox8.Location = New System.Drawing.Point(208, 48)
            Me.TextBox8.Name = "TextBox8"
            Me.TextBox8.Size = New System.Drawing.Size(40, 20)
            Me.TextBox8.TabIndex = 34
            Me.TextBox8.Text = ""
            '
            'TextBox7
            '
            Me.TextBox7.Location = New System.Drawing.Point(128, 128)
            Me.TextBox7.Name = "TextBox7"
            Me.TextBox7.Size = New System.Drawing.Size(40, 20)
            Me.TextBox7.TabIndex = 33
            Me.TextBox7.Text = ""
            '
            'TextBox6
            '
            Me.TextBox6.Location = New System.Drawing.Point(128, 104)
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Size = New System.Drawing.Size(40, 20)
            Me.TextBox6.TabIndex = 32
            Me.TextBox6.Text = ""
            '
            'TextBox5
            '
            Me.TextBox5.Location = New System.Drawing.Point(128, 80)
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Size = New System.Drawing.Size(40, 20)
            Me.TextBox5.TabIndex = 31
            Me.TextBox5.Text = ""
            '
            'TextBox4
            '
            Me.TextBox4.Location = New System.Drawing.Point(128, 48)
            Me.TextBox4.Name = "TextBox4"
            Me.TextBox4.Size = New System.Drawing.Size(40, 20)
            Me.TextBox4.TabIndex = 30
            Me.TextBox4.Text = ""
            '
            'btnDefault
            '
            Me.btnDefault.Location = New System.Drawing.Point(320, 128)
            Me.btnDefault.Name = "btnDefault"
            Me.btnDefault.TabIndex = 29
            Me.btnDefault.Text = "Default"
            '
            'btnApply
            '
            Me.btnApply.Location = New System.Drawing.Point(320, 88)
            Me.btnApply.Name = "btnApply"
            Me.btnApply.TabIndex = 28
            Me.btnApply.Text = "Apply"
            '
            'btnSetup
            '
            Me.btnSetup.Location = New System.Drawing.Point(320, 48)
            Me.btnSetup.Name = "btnSetup"
            Me.btnSetup.TabIndex = 27
            Me.btnSetup.Text = "Setup"
            '
            'Label6
            '
            Me.Label6.Location = New System.Drawing.Point(224, 16)
            Me.Label6.Name = "Label6"
            Me.Label6.TabIndex = 18
            Me.Label6.Text = "Calibrated"
            '
            'Label5
            '
            Me.Label5.Location = New System.Drawing.Point(80, 16)
            Me.Label5.Name = "Label5"
            Me.Label5.TabIndex = 17
            Me.Label5.Text = "Uncalibrated"
            '
            'TextBox3
            '
            Me.TextBox3.Location = New System.Drawing.Point(80, 128)
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Size = New System.Drawing.Size(40, 20)
            Me.TextBox3.TabIndex = 7
            Me.TextBox3.Text = ""
            '
            'TextBox2
            '
            Me.TextBox2.Location = New System.Drawing.Point(80, 104)
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Size = New System.Drawing.Size(40, 20)
            Me.TextBox2.TabIndex = 6
            Me.TextBox2.Text = ""
            '
            'TextBox1
            '
            Me.TextBox1.Location = New System.Drawing.Point(80, 80)
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Size = New System.Drawing.Size(40, 20)
            Me.TextBox1.TabIndex = 5
            Me.TextBox1.Text = ""
            '
            'TextBox0
            '
            Me.TextBox0.Location = New System.Drawing.Point(80, 48)
            Me.TextBox0.Name = "TextBox0"
            Me.TextBox0.Size = New System.Drawing.Size(40, 20)
            Me.TextBox0.TabIndex = 4
            Me.TextBox0.Text = ""
            '
            'Label4
            '
            Me.Label4.Location = New System.Drawing.Point(16, 128)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(48, 23)
            Me.Label4.TabIndex = 3
            Me.Label4.Text = "Point 3:"
            '
            'Label3
            '
            Me.Label3.Location = New System.Drawing.Point(16, 96)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(48, 23)
            Me.Label3.TabIndex = 2
            Me.Label3.Text = "Point 2:"
            '
            'Label2
            '
            Me.Label2.Location = New System.Drawing.Point(16, 72)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(48, 23)
            Me.Label2.TabIndex = 1
            Me.Label2.Text = "Point 1:"
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(16, 48)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(48, 23)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Point 0:"
            '
            'Label10
            '
            Me.Label10.Location = New System.Drawing.Point(96, 16)
            Me.Label10.Name = "Label10"
            Me.Label10.TabIndex = 17
            Me.Label10.Text = "Uncalibrated"
            '
            'Label11
            '
            Me.Label11.Location = New System.Drawing.Point(80, 16)
            Me.Label11.Name = "Label11"
            Me.Label11.TabIndex = 17
            Me.Label11.Text = "Uncalibrated"
            '
            'Label12
            '
            Me.Label12.Location = New System.Drawing.Point(80, 16)
            Me.Label12.Name = "Label12"
            Me.Label12.TabIndex = 17
            Me.Label12.Text = "Uncalibrated"
            '
            'TextBox16
            '
            Me.TextBox16.Location = New System.Drawing.Point(80, 48)
            Me.TextBox16.Name = "TextBox16"
            Me.TextBox16.Size = New System.Drawing.Size(40, 20)
            Me.TextBox16.TabIndex = 4
            Me.TextBox16.Text = ""
            '
            'SampleDescription
            '
            Me.SampleDescription.Location = New System.Drawing.Point(24, 408)
            Me.SampleDescription.Multiline = True
            Me.SampleDescription.Name = "SampleDescription"
            Me.SampleDescription.Size = New System.Drawing.Size(680, 24)
            Me.SampleDescription.TabIndex = 2
            Me.SampleDescription.Text = "Sample description:  shows how to create a calibrated coordinate space.  Also sho" & _
            "ws how to map 2D objects between spaces."
            '
            'MouseLocationFrame
            '
            Me.MouseLocationFrame.Controls.Add(Me.MouseCalibratedY)
            Me.MouseLocationFrame.Controls.Add(Me.MouseUncalibratedY)
            Me.MouseLocationFrame.Controls.Add(Me.MouseCalibratedX)
            Me.MouseLocationFrame.Controls.Add(Me.MouseUncalibratedX)
            Me.MouseLocationFrame.Controls.Add(Me.Label7)
            Me.MouseLocationFrame.Controls.Add(Me.Label8)
            Me.MouseLocationFrame.Location = New System.Drawing.Point(440, 16)
            Me.MouseLocationFrame.Name = "MouseLocationFrame"
            Me.MouseLocationFrame.Size = New System.Drawing.Size(344, 104)
            Me.MouseLocationFrame.TabIndex = 3
            Me.MouseLocationFrame.TabStop = False
            Me.MouseLocationFrame.Text = "Mouse Location"
            '
            'MouseCalibratedY
            '
            Me.MouseCalibratedY.Location = New System.Drawing.Point(224, 56)
            Me.MouseCalibratedY.Name = "MouseCalibratedY"
            Me.MouseCalibratedY.Size = New System.Drawing.Size(48, 23)
            Me.MouseCalibratedY.TabIndex = 24
            Me.MouseCalibratedY.Text = "0.0"
            '
            'MouseUncalibratedY
            '
            Me.MouseUncalibratedY.Location = New System.Drawing.Point(224, 32)
            Me.MouseUncalibratedY.Name = "MouseUncalibratedY"
            Me.MouseUncalibratedY.Size = New System.Drawing.Size(48, 23)
            Me.MouseUncalibratedY.TabIndex = 23
            Me.MouseUncalibratedY.Text = "0.0"
            '
            'MouseCalibratedX
            '
            Me.MouseCalibratedX.Location = New System.Drawing.Point(136, 64)
            Me.MouseCalibratedX.Name = "MouseCalibratedX"
            Me.MouseCalibratedX.Size = New System.Drawing.Size(56, 23)
            Me.MouseCalibratedX.TabIndex = 22
            Me.MouseCalibratedX.Text = "0.0"
            '
            'MouseUncalibratedX
            '
            Me.MouseUncalibratedX.Location = New System.Drawing.Point(136, 32)
            Me.MouseUncalibratedX.Name = "MouseUncalibratedX"
            Me.MouseUncalibratedX.Size = New System.Drawing.Size(48, 23)
            Me.MouseUncalibratedX.TabIndex = 21
            Me.MouseUncalibratedX.Text = "0.0"
            '
            'Label7
            '
            Me.Label7.Location = New System.Drawing.Point(24, 64)
            Me.Label7.Name = "Label7"
            Me.Label7.TabIndex = 20
            Me.Label7.Text = "Calibrated"
            '
            'Label8
            '
            Me.Label8.Location = New System.Drawing.Point(24, 32)
            Me.Label8.Name = "Label8"
            Me.Label8.TabIndex = 19
            Me.Label8.Text = "Uncalibrated"
            '
            'CalibratedUnits
            '
            Me.CalibratedUnits.AllowDrop = True
            Me.CalibratedUnits.Items.AddRange(New Object() {"feet", "meters", "inches", "centimeters", "millimeters", "microns"})
            Me.CalibratedUnits.Location = New System.Drawing.Point(560, 144)
            Me.CalibratedUnits.Name = "CalibratedUnits"
            Me.CalibratedUnits.Size = New System.Drawing.Size(121, 21)
            Me.CalibratedUnits.TabIndex = 4
            Me.CalibratedUnits.Text = "ComboBox1"
            '
            'Label9
            '
            Me.Label9.Location = New System.Drawing.Point(448, 144)
            Me.Label9.Name = "Label9"
            Me.Label9.TabIndex = 5
            Me.Label9.Text = "Calibrated Units:"
            '
            'frmCalibPoint
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(800, 446)
            Me.Controls.Add(Me.Label9)
            Me.Controls.Add(Me.CalibratedUnits)
            Me.Controls.Add(Me.MouseLocationFrame)
            Me.Controls.Add(Me.SampleDescription)
            Me.Controls.Add(Me.CalibrationPointsFrame)
            Me.Controls.Add(Me.CalibrationDisplay)
            Me.Name = "frmCalibPoint"
            Me.Text = "Calibration Sample Application"
            CType(Me.CalibrationDisplay, System.ComponentModel.ISupportInitialize).EndInit()
            Me.CalibrationPointsFrame.ResumeLayout(False)
            Me.MouseLocationFrame.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

#End Region
#Region "Module Level vars"
    Dim CalibrationTool As CogCalibNPointToNPointTool
    Dim CalibrationOperator As CogCalibNPointToNPoint

    Dim UncalibratedPoints(0, 3) As CogPointMarker
    'For Coding Convenience We'll Process TextBoxes as a Batch By Creating Control Arrays
    Dim PointUncalibratedX(3) As TextBox
    Dim PointUncalibratedY(3) As TextBox
    Dim PointCalibratedX(3) As TextBox
    Dim PointCalibratedY(3) As TextBox
#End Region
#Region "Forms and Controls Events"
    Private Sub frmCalibrationPoint_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      PointUncalibratedX(0) = TextBox0
      PointUncalibratedX(1) = TextBox1
      PointUncalibratedX(2) = TextBox2
      PointUncalibratedX(3) = TextBox3

      PointUncalibratedY(0) = TextBox4
      PointUncalibratedY(1) = TextBox5
      PointUncalibratedY(2) = TextBox6
      PointUncalibratedY(3) = TextBox7

      PointCalibratedX(0) = TextBox8
      PointCalibratedX(1) = TextBox9
      PointCalibratedX(2) = TextBox10
      PointCalibratedX(3) = TextBox11

      PointCalibratedY(0) = TextBox12
      PointCalibratedY(1) = TextBox13
      PointCalibratedY(2) = TextBox14
      PointCalibratedY(3) = TextBox15
      ' Initialize to an aptly configured calibration tool.
      SetDefaultCalibration()

      ' Create and install the uncalibrated (# = image coordinate based) points.
      Dim i As Integer

      For i = UncalibratedPoints.GetLowerBound(0) To UncalibratedPoints.GetUpperBound(0)
        UncalibratedPoints(0, i).SelectedSpaceName = "#"
        UncalibratedPoints(0, i).TipText = "Uncalibrated Point " & i.ToString
        UncalibratedPoints(0, i).Interactive = True
        CalibrationDisplay.InteractiveGraphics.Add(UncalibratedPoints(i, 0), "test", False)
      Next i

      ' Default to "not in SetupMode".
      SetupMode = False

      ' Default to an appropriate zoom factor.
      CalibrationDisplay.Fit(True)

    End Sub
    Private Sub btnSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetup.Click
      SetupMode = True
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
            SetupMode = False
    End Sub

    Private Sub btnDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefault.Click
      SetDefaultCalibration()
    End Sub

    Private Sub frmCalibPoint_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
      RemoveHandler CalibrationOperator.Changed, AddressOf CalibrationOperator_Changed
      RemoveHandler CalibrationTool.Changed, AddressOf CalibrationTool_Changed
      If Not CalibrationTool Is Nothing Then CalibrationTool.Dispose()
    End Sub
#End Region
#Region "Module Level Routines"
    Private Sub RefreshCalibrationPointControls()
      ' Sync the text controls to reflect the calibration object's current
      ' point coordinates.
      Dim i As Integer
      For i = 0 To CalibrationTool.Calibration.NumPoints - 1
        PointUncalibratedX(i).Text = _
          CalibrationTool.Calibration.GetUncalibratedPointX(i).ToString
        PointUncalibratedY(i).Text = _
          CalibrationTool.Calibration.GetUncalibratedPointY(i).ToString
        PointCalibratedX(i).Text = _
          CalibrationTool.Calibration.GetRawCalibratedPointX(i).ToString
        PointCalibratedY(i).Text = _
          CalibrationTool.Calibration.GetRawCalibratedPointY(i).ToString
      Next i
    End Sub

    Private Sub SetDefaultCalibration()
      ' Locate the tool configuration file using the environment variable that
      ' indicates where VisionPro is installed.  If the environment variable is
      ' not set, display the error and terminate the application.
      Dim CalibrationVPPFile As String
      CalibrationVPPFile = Environment.GetEnvironmentVariable("VPRO_ROOT")
            If CalibrationVPPFile = "" Then
                MessageBox.Show("Required environment variable VPRO_ROOT not set.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End
            End If
      CalibrationVPPFile = CalibrationVPPFile & _
                           "\Samples\Programming\Calibration\Calibration\BracketMillimeters.vpp"

      ' The default calibration was configured using precise measurements
      ' obtained through the use of VisionPro vision tools.
      Try
        CalibrationTool = CogSerializer.LoadObjectFromFile(CalibrationVPPFile)
        AddHandler CalibrationTool.Changed, AddressOf CalibrationTool_Changed

        CalibratedUnits.Text = "millimeters"

        ' We need to use the Calibration operator in order to capture the operator's
        ' Change event.
        CalibrationOperator = CalibrationTool.Calibration
        AddHandler CalibrationOperator.Changed, AddressOf CalibrationOperator_Changed
        ' Update controls and graphics to reflect the new calibration's point
        ' coordinates.
        RefreshCalibrationPointControls()
        RefreshCalibrationPointGraphics()

        ' Calibrate and run.  In addition to applying the calibration we just
        ' loaded, these operations will result in events the application will
        ' use to handle the presence of a new calibration space and the image
        ' to which it is attached.
        CalibrationTool.Calibration.Calibrate()
        CalibrationTool.Run()

      Catch ex As Exception

                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Application.Exit()
      End Try
    End Sub

    Private Sub RefreshCalibrationPointGraphics()
      ' Sync the point graphics to reflect the calibration object's current
      ' points.
      Dim i As Integer
      For i = 0 To CalibrationTool.Calibration.NumPoints - 1
        UncalibratedPoints(0, i) = New CogPointMarker
        UncalibratedPoints(0, i).X = _
          CalibrationTool.Calibration.GetUncalibratedPointX(i)
        UncalibratedPoints(0, i).Y = _
          CalibrationTool.Calibration.GetUncalibratedPointY(i)
      Next i
    End Sub


    Private Function CalcDistance(ByVal x1 As Double, ByVal x2 As Double) As Double
      ' Calculating the distance between two points on a number line using
      ' subtraction works for all combinations of (neg, neg), (neg, pos) and
      ' (pos, pos) as long as you adjust for the potentially negative result.
      CalcDistance = Math.Abs(x2 - x1)
    End Function
    Property SetupMode() As Boolean
      Get
        SetupMode = btnApply.Enabled
      End Get
      Set(ByVal enterSetupMode As Boolean)
        Try
          btnSetup.Enabled = Not enterSetupMode
          btnApply.Enabled = enterSetupMode
          CalibratedUnits.Enabled = enterSetupMode

          ' If we continue to sync our controls and graphics to the calibration
          ' points, the first coordinate updated will indirectly cause all other
          ' controls and graphics to get "reset" to the corresponding calibration
          ' point thereby losing all changes except the first.  Temporarily
          ' disable calibration events.
          CalibrationOperator = Nothing

          Dim i As Integer
          For i = 0 To 3
            ' Calibration coordinates are only editable if in setup mode.
            PointUncalibratedX(i).Enabled = enterSetupMode
            PointUncalibratedY(i).Enabled = enterSetupMode
            PointCalibratedX(i).Enabled = enterSetupMode
            PointCalibratedY(i).Enabled = enterSetupMode



            ' Sync uncalibrated coordinates with textbox values.
            CalibrationTool.Calibration.SetUncalibratedPointX(i, CDbl(PointUncalibratedX(i).Text))
            CalibrationTool.Calibration.SetUncalibratedPointY(i, CDbl(PointUncalibratedY(i).Text))

            ' Sync calibrated coordinates with textbox values.
            CalibrationTool.Calibration.SetRawCalibratedPointX(i, CDbl(PointCalibratedX(i).Text))
            CalibrationTool.Calibration.SetRawCalibratedPointY(i, CDbl(PointCalibratedY(i).Text))


          Next i

          ' Re-enable calibration events.  See comment above for more info.
          ' We need to use the Calibration operator in order to capture the operator's
          ' Change event.
          CalibrationOperator = CalibrationTool.Calibration

          If Not enterSetupMode Then
            ' Leaving setup mode... calibrate based on new points/coordinates.  If
            ' calibration fails, displya the error and return to setup mode.

            CalibrationTool.Calibration.Calibrate()
            CalibrationTool.Run()


            ' Since we disabled events while updating calibration points, we must
            ' sync our controls and graphics manually.
            RefreshCalibrationPointControls()
            RefreshCalibrationPointGraphics()
          End If

          Exit Property

        Catch ex As Exception
                    MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End Try
      End Set
    End Property
#End Region
#Region "Cognex Objects Events"
    Private Sub CalibrationOperator_Changed(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogChangedEventArgs) 'Handles CalibrationOperator.Changed
      ' If there is a new calibration space, the existing graphics are obsolete.

            If e.StateFlags = CogCalibNPointToNPoint.SfCalibrated Or _
              CogCalibNPointToNPoint.SfGetComputedUncalibratedFromCalibratedTransform Or _
              CogCalibNPointToNPoint.SfGetComputedUncalibratedFromRawCalibratedTransform Or _
              CogCalibNPointToNPoint.SfComputedRMSError Or CogCalibNPointToNPoint.SfGetInfoStrings Then

                CalibrationDisplay.StaticGraphics.Clear()
            End If

      ' If the calibration points have changed, update controls and graphics
      ' to reflect new point coordinates.

            If e.StateFlags = CogCalibNPointToNPoint.SfNumPoints Or _
        CogCalibNPointToNPoint.SfGetUncalibratedPointX Or _
        CogCalibNPointToNPoint.SfGetUncalibratedPointY Or _
        CogCalibNPointToNPoint.SfGetRawCalibratedPointX Or _
        CogCalibNPointToNPoint.SfGetRawCalibratedPointY Then
                RefreshCalibrationPointControls()
                RefreshCalibrationPointGraphics()
            End If
    End Sub

    Private Sub CalibrationTool_Changed(ByVal sender As Object, ByVal e As Cognex.VisionPro.CogChangedEventArgs) 'Handles CalibrationTool.Changed
      ' If the calibration tool is run, use the new calibration output image.

      If e.StateFlags = CogCalibNPointToNPointTool.SfOutputImage Or CogCalibNPointToNPointTool.SfRunStatus Or _
      CogCalibNPointToNPointTool.SfCreateLastRunRecord Then
        ' Display calibration image.  If no image is available, exit handler.
        CalibrationDisplay.Image = CalibrationTool.OutputImage
        If CalibrationTool.OutputImage Is Nothing Then
          Exit Sub
        End If

        ' Draw a rectangle to represent how the calibrated coordinate space maps
        ' to the image coordinate space (#).
        Dim Transform As CogTransform2DLinear
        Transform = _
          CalibrationDisplay.GetTransform( _
          CalibrationTool.RunParams.CalibratedSpaceName, "#")

        Dim ImageWidth As Double, ImageHeight As Double
        ImageWidth = CalibrationDisplay.Image.Width
        ImageHeight = CalibrationDisplay.Image.Height

        ' Compute the position of the affine rectangle which is used to
        ' show the coordinate space. The affine rectangle will be 15%
        ' smaller than the image so that the user can see it easily.
        Dim PoX As Double, PoY As Double
        PoX = ImageWidth * 0.15
        PoY = ImageHeight * 0.15
        Transform.MapPoint(PoX, PoY, PoX, PoY)

        Dim PxX As Double, PxY As Double
        PxX = ImageWidth * 0.85
        PxY = ImageHeight * 0.15
        Transform.MapPoint(PxX, PxY, PxX, PxY)

        Dim PyX As Double, PyY As Double
        PyX = ImageWidth * 0.15
        PyY = ImageHeight * 0.85
        Transform.MapPoint(PyX, PyY, PyX, PyY)

        Dim CenterX As Double, CenterY As Double
        CenterX = ImageWidth / 2.0#   ' VB replaces 2.0 with 2#
        CenterY = ImageHeight / 2.0#  ' VB replaces 2.0 with 2#
        Transform.MapPoint(CenterX, CenterY, CenterX, CenterY)

        Dim CoordSpaceRect As New CogRectangleAffine
        CoordSpaceRect.SelectedSpaceName = _
          CalibrationTool.RunParams.CalibratedSpaceName
        CoordSpaceRect.SetCenterLengthsRotationSkew(CenterX, CenterY, _
                                                    CalcDistance(PxX, PoX), _
                                                    CalcDistance(PyY, PoY), 0, 0)
        CalibrationDisplay.StaticGraphics.Add(CoordSpaceRect, "test")

        ' Draw labels conveying the lengths of rectangle's sides.
        Dim RectWidthAdornment As New CogGraphicLabel
        Dim RectHeightAdornment As New CogGraphicLabel
        RectWidthAdornment.SelectedSpaceName = _
          CalibrationTool.RunParams.CalibratedSpaceName
        RectHeightAdornment.SelectedSpaceName = _
          CalibrationTool.RunParams.CalibratedSpaceName
        RectWidthAdornment.Rotation = CogMisc.DegToRad(0)
        RectHeightAdornment.Rotation = CogMisc.DegToRad(90)
        Dim WidthAdornmentYOffset As Double, ignore As Double
        Transform.MapVector(0, ImageHeight / 2.0# - ImageHeight * 0.12, _
                            ignore, WidthAdornmentYOffset) ' 2# = 2.0
        RectWidthAdornment.SetXYText(CenterX, CenterY - WidthAdornmentYOffset, _
          Math.Round(CalcDistance(PxX, PoX), 2).ToString & " " & CalibratedUnits.Text)
        Dim HeightAdornmentXOffset As Double
        Transform.MapVector(ImageWidth / 2.0# - ImageWidth * 0.12, 0, _
                            HeightAdornmentXOffset, ignore) ' 2# = 2.0
        RectHeightAdornment.SetXYText(CenterX + HeightAdornmentXOffset, CenterY, _
          Math.Round(CalcDistance(PyY, PoY), 2).ToString & " " & CalibratedUnits.Text)
        CalibrationDisplay.StaticGraphics.Add(RectWidthAdornment, "test")
        CalibrationDisplay.StaticGraphics.Add(RectHeightAdornment, "test")

        ' Draw a coordinate axes to represent the calibrated coordinate space's
        ' origin.  VB replaces 0.0 with 0#.
        Dim CoordSpaceAxes As New CogCoordinateAxes
        CoordSpaceAxes.SelectedSpaceName = _
          CalibrationTool.RunParams.CalibratedSpaceName
        CoordSpaceAxes.SetOriginLengthAspectRotationSkew(0.0#, 0.0#, _
          CoordSpaceRect.SideXLength, _
          CoordSpaceRect.SideYLength / CoordSpaceRect.SideXLength, 0, 0)

        CalibrationDisplay.StaticGraphics.Add(CoordSpaceAxes, "test")
      End If
    End Sub

    Private Sub CalibrationDisplay_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CalibrationDisplay.MouseMove
      ' Map from screen coordinates (*) to image coordinates (#).

      Dim UncalibratedX As Double, UncalibratedY As Double
      CalibrationDisplay.GetTransform("#", "*").MapPoint(e.X, e.Y, UncalibratedX, UncalibratedY)
      MouseUncalibratedX.Text = Math.Round(UncalibratedX, 2).ToString
      MouseUncalibratedY.Text = Math.Round(UncalibratedY, 2).ToString

      ' Map from image coordinates (#) to calibrated coordinates.
      Dim CalibratedX As Double, CalibratedY As Double
      CalibrationDisplay.GetTransform( _
          CalibrationTool.RunParams.CalibratedSpaceName, "#").MapPoint( _
              UncalibratedX, UncalibratedY, CalibratedX, CalibratedY)
      MouseCalibratedX.Text = Math.Round(CalibratedX, 2).ToString
      MouseCalibratedY.Text = Math.Round(CalibratedY, 2).ToString
    End Sub

#End Region



  End Class
End Namespace