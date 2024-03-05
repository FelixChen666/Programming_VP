
'*******************************************************************************
' Copyright (C) 2004 Cognex Corporation
'
' Subject to Cognex Corporation's terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore, you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
'*******************************************************************************
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' This sample demonstrates how various graphics can be added to and removed from
' a display control. The sample uses the objects and interfaces defined in
' the Cognex Core type library and in the Cognex Display control (CogDisplay).
' No VisionPro tools are required to create, add or remove graphics.
' This program assumes that you have some knowledge of Visual Basic programming.
'
' CogDispay supports two types of graphics: static and interactive graphics.
'  1) Static graphics cannot be moved or changed once they've been added to
'     the display. No TipText can be added to a static graphic.
'  2) Interactive graphics can be moved or changed by the program, or by the user
'     when the graphic's Interactive property is enabled. TipText can be added
'     to an interactive graphic. This sample shows how to add TipText.
'
' The following steps show how to add either static or interactive graphics to the CogDisplay.
'
' How to add interactive graphics (see AddInteractive) :
' Step 1) Create a shape. CogEllipse is used in this example.
' Step 2) Retrieve the CogGraphicInteractive interface from the interactive shape.
' Step 3) Set the graphic's degree of freedom property.
' Step 4) Add the shape to the interactive graphics collection to display it.
'
' How to add static graphics (see AddStatic_Click):
' Step 1) Create a shape. CogRectangleAffine is used in this example.
' Step 2) Add the shape to the static graphics collection to display it.
Option Explicit On 
Imports Cognex.VisionPro
Namespace SampleDisplayGraphics
  Public Class DisplayGraphicsForm
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
    Friend WithEvents GraphicsDisplay As Cognex.VisionPro.Display.CogDisplay
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents AddNonInteractive As System.Windows.Forms.Button
    Friend WithEvents AddNonSelectable As System.Windows.Forms.Button
    Friend WithEvents AddFullyInteractive As System.Windows.Forms.Button
    Friend WithEvents AddStatic As System.Windows.Forms.Button
    Friend WithEvents RemoveGraphics As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(DisplayGraphicsForm))
      Me.GraphicsDisplay = New Cognex.VisionPro.Display.CogDisplay
      Me.GroupBox1 = New System.Windows.Forms.GroupBox
      Me.AddFullyInteractive = New System.Windows.Forms.Button
      Me.AddNonSelectable = New System.Windows.Forms.Button
      Me.AddNonInteractive = New System.Windows.Forms.Button
      Me.AddStatic = New System.Windows.Forms.Button
      Me.RemoveGraphics = New System.Windows.Forms.Button
      Me.TextBox1 = New System.Windows.Forms.TextBox
      CType(Me.GraphicsDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.GroupBox1.SuspendLayout()
      Me.SuspendLayout()
      '
      'GraphicsDisplay
      '
      Me.GraphicsDisplay.Location = New System.Drawing.Point(8, 8)
      Me.GraphicsDisplay.Name = "GraphicsDisplay"
      Me.GraphicsDisplay.OcxState = CType(resources.GetObject("GraphicsDisplay.OcxState"), System.Windows.Forms.AxHost.State)
      Me.GraphicsDisplay.Size = New System.Drawing.Size(640, 352)
      Me.GraphicsDisplay.TabIndex = 0
      '
      'GroupBox1
      '
      Me.GroupBox1.Controls.Add(Me.AddFullyInteractive)
      Me.GroupBox1.Controls.Add(Me.AddNonSelectable)
      Me.GroupBox1.Controls.Add(Me.AddNonInteractive)
      Me.GroupBox1.Location = New System.Drawing.Point(656, 8)
      Me.GroupBox1.Name = "GroupBox1"
      Me.GroupBox1.Size = New System.Drawing.Size(200, 224)
      Me.GroupBox1.TabIndex = 1
      Me.GroupBox1.TabStop = False
      Me.GroupBox1.Text = "Add Interactive Graphics"
      '
      'AddFullyInteractive
      '
      Me.AddFullyInteractive.Location = New System.Drawing.Point(32, 168)
      Me.AddFullyInteractive.Name = "AddFullyInteractive"
      Me.AddFullyInteractive.Size = New System.Drawing.Size(120, 48)
      Me.AddFullyInteractive.TabIndex = 2
      Me.AddFullyInteractive.Text = "Fully Interactive"
      '
      'AddNonSelectable
      '
      Me.AddNonSelectable.Location = New System.Drawing.Point(32, 104)
      Me.AddNonSelectable.Name = "AddNonSelectable"
      Me.AddNonSelectable.Size = New System.Drawing.Size(120, 48)
      Me.AddNonSelectable.TabIndex = 1
      Me.AddNonSelectable.Text = "NonSelectable"
      '
      'AddNonInteractive
      '
      Me.AddNonInteractive.Location = New System.Drawing.Point(32, 40)
      Me.AddNonInteractive.Name = "AddNonInteractive"
      Me.AddNonInteractive.Size = New System.Drawing.Size(112, 48)
      Me.AddNonInteractive.TabIndex = 0
      Me.AddNonInteractive.Text = "NonInteractive"
      '
      'AddStatic
      '
      Me.AddStatic.Location = New System.Drawing.Point(696, 240)
      Me.AddStatic.Name = "AddStatic"
      Me.AddStatic.Size = New System.Drawing.Size(112, 48)
      Me.AddStatic.TabIndex = 2
      Me.AddStatic.Text = "Add Static Graphic"
      '
      'RemoveGraphics
      '
      Me.RemoveGraphics.Location = New System.Drawing.Point(696, 296)
      Me.RemoveGraphics.Name = "RemoveGraphics"
      Me.RemoveGraphics.Size = New System.Drawing.Size(112, 48)
      Me.RemoveGraphics.TabIndex = 3
      Me.RemoveGraphics.Text = "Remove All Graphics"
      '
      'TextBox1
      '
      Me.TextBox1.Location = New System.Drawing.Point(8, 392)
      Me.TextBox1.Multiline = True
      Me.TextBox1.Name = "TextBox1"
      Me.TextBox1.Size = New System.Drawing.Size(800, 40)
      Me.TextBox1.TabIndex = 4
      Me.TextBox1.Text = "Sample description: shows how to add variably configured graphics to a display." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & _
      "Sample usage: click the buttons to add or remove graphics."
      '
      'DisplayGraphicsForm
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(872, 454)
      Me.Controls.Add(Me.TextBox1)
      Me.Controls.Add(Me.RemoveGraphics)
      Me.Controls.Add(Me.AddStatic)
      Me.Controls.Add(Me.GroupBox1)
      Me.Controls.Add(Me.GraphicsDisplay)
      Me.Name = "DisplayGraphicsForm"
      Me.Text = "Display Graphics Sample Application"
      CType(Me.GraphicsDisplay, System.ComponentModel.ISupportInitialize).EndInit()
      Me.GroupBox1.ResumeLayout(False)
      Me.ResumeLayout(False)

    End Sub

#End Region
#Region "Module Level vars"
    Enum GraphicInteractivityLevel
      eNonInteractive = 0
      eNonSelectable = 1
      eFullyInteractive = 2
    End Enum
#End Region
#Region "Form and Controls Events"
    Private Sub DisplayGraphicsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      ' Install a display image.
      ' NOTE: An image must be installed before adding any graphics. Otherwise,
      ' the graphics will not appear.
      GraphicsDisplay.Image = SyntheticImage()
    End Sub
    Private Sub AddNonInteractive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNonInteractive.Click
      AddInteractive(CogColorConstants.Red, GraphicInteractivityLevel.eNonInteractive)
    End Sub

    Private Sub AddNonSelectable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNonSelectable.Click
      AddInteractive(CogColorConstants.Yellow, GraphicInteractivityLevel.eNonSelectable)
    End Sub

    Private Sub AddFullyInteractive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddFullyInteractive.Click
      AddInteractive(CogColorConstants.Green, GraphicInteractivityLevel.eFullyInteractive)
    End Sub

    Private Sub AddStatic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddStatic.Click
      ' The first static graphic is installed at coordinate (0,50). This coordinate
      ' was chosen so that static graphics do not overlap interactive graphics.
      ' NOTE: Unlike an interactive graphic, once a static graphic is added to the
      '       Cognex Display control, it cannot be accessed. Thus, it is necessary
      '       to create two static variables to compute CenterX and CenterY.
      Static StaticPositionX As Double, StaticPositionY As Double
      If GraphicsDisplay.StaticGraphics.ZOrderGroups.Count = 0 Then
        StaticPositionX = 0
        StaticPositionY = 50
      End If

      ' Step 1 - Create and initialize the new graphic.
      Dim StaticGraphic As CogRectangleAffine
      StaticGraphic = New CogRectangleAffine
      ' Set the graphic's display location
      StaticGraphic.CenterX = StaticPositionX
      StaticGraphic.CenterY = StaticPositionY
      ' Static graphics will not display TipText. The assigned TipText
      ' will not appear even though the mouse is placed over the graphic.
      StaticGraphic.TipText = "Static graphics don't support tiptext!!!"
      ' The gaphic's color can be changed
      StaticGraphic.Color = CogColorConstants.Orange
      ' The line width can also be defined by the user.
      StaticGraphic.LineWidthInScreenPixels = StaticGraphic.LineWidthInScreenPixels * 3

      ' Step 2) Add the shape to the static graphics collection to display it.
      GraphicsDisplay.StaticGraphics.Add(StaticGraphic, "test")

      ' Subsequent graphics are installed at an offset of (50,50) from their
      ' predecessor.
      StaticPositionX = StaticPositionX + 50
      StaticPositionY = StaticPositionY + 50
    End Sub

    Private Sub RemoveGraphics_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveGraphics.Click
      ' Call the Clear method for clearing static graphics
      GraphicsDisplay.StaticGraphics.Clear()

      ' Call the Remove method for removing each interactive graphic.
      Do While GraphicsDisplay.InteractiveGraphics.Count > 0
        GraphicsDisplay.InteractiveGraphics.Remove(0)
      Loop
    End Sub
#End Region
#Region "Module Level Auxilliary Routines"
    ' Create a blank image.
    Private Function SyntheticImage() As CogImage8Grey
      Dim BlankImage As New CogImage8Grey
      BlankImage.Allocate(200, 200)

      ' Slow, but simple.
      Dim col As Integer, row As Integer
      For col = 0 To BlankImage.Width - 1
        For row = 0 To BlankImage.Height - 1
          BlankImage.SetPixel(col, row, 128)
        Next row
      Next col

      SyntheticImage = BlankImage
    End Function
    ' Add an interactive graphic to the graphics display.
    Private Sub AddInteractive(ByVal color As CogColorConstants, _
                               ByVal interactivityLevel As GraphicInteractivityLevel)
      ' Step 1) Create a shape. CogEllipse is used in this example.
      Dim InteractiveGraphic As CogEllipse
      InteractiveGraphic = New CogEllipse

      If GraphicsDisplay.InteractiveGraphics.Count = 0 Then
        ' The first interactive graphic is installed at coordinate (0,0).
        InteractiveGraphic.CenterX = 0
        InteractiveGraphic.CenterY = 0
      Else
        ' Subsequent graphics are installed at an offset of (50,50) from their predecessor.
        Dim PreviousGraphic As CogEllipse
        PreviousGraphic = GraphicsDisplay.InteractiveGraphics.Item(GraphicsDisplay.InteractiveGraphics.Count - 1)
        InteractiveGraphic.CenterX = PreviousGraphic.CenterX + 50
        InteractiveGraphic.CenterY = PreviousGraphic.CenterY + 50
      End If

      ' Add a TipText. This TipText will appear when the mouse is placed over the graphic.
      InteractiveGraphic.TipText = "Interactive Graphic Number " & _
                                   CStr(InteractiveGraphic.CenterX / 50)
      ' Set the graphic's color
      InteractiveGraphic.Color = color

      ' Step 2) Retrieve the CogGraphicInteractive interface from the interactive shape.
      '         Some graphic properties are only accessible through the generic interface.
      Dim GenericInteractive As Cognex.VisionPro.ICogGraphicInteractive
      GenericInteractive = InteractiveGraphic

      ' Step 3) Set the graphic's degree of freedom (DOF) property.
      '         There is no mechanism for making an interactive graphic non-selectable
      '         without making it non-interactive.  If the selected color is also the same
      '         as the unselected color, they both appear to be unselectable.
      InteractiveGraphic.Interactive = interactivityLevel > GraphicInteractivityLevel.eNonInteractive
      If interactivityLevel > GraphicInteractivityLevel.eNonSelectable Then
        InteractiveGraphic.GraphicDOFEnable = CogEllipseDOFConstants.All
      Else
        GenericInteractive.SelectedColor = InteractiveGraphic.Color
        GenericInteractive.GraphicDOFEnableBase = CogGraphicDOFConstants.None
      End If

      ' Draw the graphic using a line style and mouse cursor that correspond to
      ' its degree of interactivity.  See CogStandardCursorConstants for all
      ' supported cursor types.  See CogGraphicLineStyleConstants for all
      ' supported line styles.
      Select Case interactivityLevel
        Case GraphicInteractivityLevel.eNonInteractive
          ' No special cursor.
          InteractiveGraphic.LineStyle = CogGraphicLineStyleConstants.Dot
        Case GraphicInteractivityLevel.eNonSelectable
          GenericInteractive.MouseCursor = _
            CogStandardCursorConstants.TipTextGraphic
          InteractiveGraphic.LineStyle = CogGraphicLineStyleConstants.Dash
          GenericInteractive.SelectedLineStyle = CogGraphicLineStyleConstants.Dash
        Case GraphicInteractivityLevel.eFullyInteractive
          GenericInteractive.MouseCursor = _
            CogStandardCursorConstants.ManipulableGraphic
          InteractiveGraphic.LineStyle = CogGraphicLineStyleConstants.Solid
          GenericInteractive.SelectedLineStyle = CogGraphicLineStyleConstants.Solid
          GenericInteractive.DragLineStyle = CogGraphicLineStyleConstants.Solid
        Case Else
          MessageBox.Show("Unrecognized interactivity level.")
      End Select

      ' Step 4) Add the shape to the interactive graphics collection to display it.
      GraphicsDisplay.InteractiveGraphics.Add(InteractiveGraphic, "test", False)

    End Sub

#End Region
  End Class
End Namespace