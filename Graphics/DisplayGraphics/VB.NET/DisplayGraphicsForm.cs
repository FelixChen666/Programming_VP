
// *******************************************************************************
// Copyright (C) 2004 Cognex Corporation
// 
// Subject to Cognex Corporation's terms and conditions and license agreement,
// you are authorized to use and modify this source code in any way you find
// useful, provided the Software and/or the modified Software is used solely in
// conjunction with a Cognex Machine Vision System.  Furthermore, you acknowledge
// and agree that Cognex has no warranty, obligations or liability for your use
// of the Software.
// *******************************************************************************
// This sample program is designed to illustrate certain VisionPro features or 
// techniques in the simplest way possible. It is not intended as the framework 
// for a complete application. In particular, the sample program may not provide
// proper error handling, event handling, cleanup, repeatability, and other 
// mechanisms that a commercial quality application requires.

// This sample demonstrates how various graphics can be added to and removed from
// a display control. The sample uses the objects and interfaces defined in
// the Cognex Core type library and in the Cognex Display control (CogDisplay).
// No VisionPro tools are required to create, add or remove graphics.
// This program assumes that you have some knowledge of Visual Basic programming.
// 
// CogDispay supports two types of graphics: static and interactive graphics.
// 1) Static graphics cannot be moved or changed once they've been added to
// the display. No TipText can be added to a static graphic.
// 2) Interactive graphics can be moved or changed by the program, or by the user
// when the graphic's Interactive property is enabled. TipText can be added
// to an interactive graphic. This sample shows how to add TipText.
// 
// The following steps show how to add either static or interactive graphics to the CogDisplay.
// 
// How to add interactive graphics (see AddInteractive) :
// Step 1) Create a shape. CogEllipse is used in this example.
// Step 2) Retrieve the CogGraphicInteractive interface from the interactive shape.
// Step 3) Set the graphic's degree of freedom property.
// Step 4) Add the shape to the interactive graphics collection to display it.
// 
// How to add static graphics (see AddStatic_Click):
// Step 1) Create a shape. CogRectangleAffine is used in this example.
// Step 2) Add the shape to the static graphics collection to display it.
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Cognex.VisionPro;

namespace DisplayGraphics.SampleDisplayGraphics
{
    public class DisplayGraphicsForm : Form
    {

        #region  Windows Form Designer generated code 

        public DisplayGraphicsForm() : base()
        {

            // This call is required by the Windows Form Designer.
            InitializeComponent();
            Load += DisplayGraphicsForm_Load;

            // Add any initialization after the InitializeComponent() call

        }

        // Form overrides dispose to clean up the component list.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components is not null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        private Cognex.VisionPro.Display.CogDisplay _GraphicsDisplay;

        internal virtual Cognex.VisionPro.Display.CogDisplay GraphicsDisplay
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _GraphicsDisplay;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _GraphicsDisplay = value;
            }
        }
        private GroupBox _GroupBox1;

        internal virtual GroupBox GroupBox1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _GroupBox1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _GroupBox1 = value;
            }
        }
        private Button _AddNonInteractive;

        internal virtual Button AddNonInteractive
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _AddNonInteractive;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_AddNonInteractive != null)
                {
                    _AddNonInteractive.Click -= AddNonInteractive_Click;
                }

                _AddNonInteractive = value;
                if (_AddNonInteractive != null)
                {
                    _AddNonInteractive.Click += AddNonInteractive_Click;
                }
            }
        }
        private Button _AddNonSelectable;

        internal virtual Button AddNonSelectable
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _AddNonSelectable;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_AddNonSelectable != null)
                {
                    _AddNonSelectable.Click -= AddNonSelectable_Click;
                }

                _AddNonSelectable = value;
                if (_AddNonSelectable != null)
                {
                    _AddNonSelectable.Click += AddNonSelectable_Click;
                }
            }
        }
        private Button _AddFullyInteractive;

        internal virtual Button AddFullyInteractive
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _AddFullyInteractive;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_AddFullyInteractive != null)
                {
                    _AddFullyInteractive.Click -= AddFullyInteractive_Click;
                }

                _AddFullyInteractive = value;
                if (_AddFullyInteractive != null)
                {
                    _AddFullyInteractive.Click += AddFullyInteractive_Click;
                }
            }
        }
        private Button _AddStatic;

        internal virtual Button AddStatic
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _AddStatic;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_AddStatic != null)
                {
                    _AddStatic.Click -= AddStatic_Click;
                }

                _AddStatic = value;
                if (_AddStatic != null)
                {
                    _AddStatic.Click += AddStatic_Click;
                }
            }
        }
        private Button _RemoveGraphics;

        internal virtual Button RemoveGraphics
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _RemoveGraphics;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_RemoveGraphics != null)
                {
                    _RemoveGraphics.Click -= RemoveGraphics_Click;
                }

                _RemoveGraphics = value;
                if (_RemoveGraphics != null)
                {
                    _RemoveGraphics.Click += RemoveGraphics_Click;
                }
            }
        }
        private TextBox _TextBox1;

        internal virtual TextBox TextBox1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _TextBox1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _TextBox1 = value;
            }
        }
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.Resources.ResourceManager(typeof(DisplayGraphicsForm));
            _GraphicsDisplay = new Cognex.VisionPro.Display.CogDisplay();
            _GroupBox1 = new GroupBox();
            _AddFullyInteractive = new Button();
            _AddFullyInteractive.Click += new EventHandler(AddFullyInteractive_Click);
            _AddNonSelectable = new Button();
            _AddNonSelectable.Click += new EventHandler(AddNonSelectable_Click);
            _AddNonInteractive = new Button();
            _AddNonInteractive.Click += new EventHandler(AddNonInteractive_Click);
            _AddStatic = new Button();
            _AddStatic.Click += new EventHandler(AddStatic_Click);
            _RemoveGraphics = new Button();
            _RemoveGraphics.Click += new EventHandler(RemoveGraphics_Click);
            _TextBox1 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)_GraphicsDisplay).BeginInit();
            _GroupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // GraphicsDisplay
            // 
            _GraphicsDisplay.Location = new Point(8, 8);
            _GraphicsDisplay.Name = "_GraphicsDisplay";
            _GraphicsDisplay.OcxState = (AxHost.State)resources.GetObject("GraphicsDisplay.OcxState");
            _GraphicsDisplay.Size = new Size(640, 352);
            _GraphicsDisplay.TabIndex = 0;
            // 
            // GroupBox1
            // 
            _GroupBox1.Controls.Add(_AddFullyInteractive);
            _GroupBox1.Controls.Add(_AddNonSelectable);
            _GroupBox1.Controls.Add(_AddNonInteractive);
            _GroupBox1.Location = new Point(656, 8);
            _GroupBox1.Name = "_GroupBox1";
            _GroupBox1.Size = new Size(200, 224);
            _GroupBox1.TabIndex = 1;
            _GroupBox1.TabStop = false;
            _GroupBox1.Text = "Add Interactive Graphics";
            // 
            // AddFullyInteractive
            // 
            _AddFullyInteractive.Location = new Point(32, 168);
            _AddFullyInteractive.Name = "_AddFullyInteractive";
            _AddFullyInteractive.Size = new Size(120, 48);
            _AddFullyInteractive.TabIndex = 2;
            _AddFullyInteractive.Text = "Fully Interactive";
            // 
            // AddNonSelectable
            // 
            _AddNonSelectable.Location = new Point(32, 104);
            _AddNonSelectable.Name = "_AddNonSelectable";
            _AddNonSelectable.Size = new Size(120, 48);
            _AddNonSelectable.TabIndex = 1;
            _AddNonSelectable.Text = "NonSelectable";
            // 
            // AddNonInteractive
            // 
            _AddNonInteractive.Location = new Point(32, 40);
            _AddNonInteractive.Name = "_AddNonInteractive";
            _AddNonInteractive.Size = new Size(112, 48);
            _AddNonInteractive.TabIndex = 0;
            _AddNonInteractive.Text = "NonInteractive";
            // 
            // AddStatic
            // 
            _AddStatic.Location = new Point(696, 240);
            _AddStatic.Name = "_AddStatic";
            _AddStatic.Size = new Size(112, 48);
            _AddStatic.TabIndex = 2;
            _AddStatic.Text = "Add Static Graphic";
            // 
            // RemoveGraphics
            // 
            _RemoveGraphics.Location = new Point(696, 296);
            _RemoveGraphics.Name = "_RemoveGraphics";
            _RemoveGraphics.Size = new Size(112, 48);
            _RemoveGraphics.TabIndex = 3;
            _RemoveGraphics.Text = "Remove All Graphics";
            // 
            // TextBox1
            // 
            _TextBox1.Location = new Point(8, 392);
            _TextBox1.Multiline = true;
            _TextBox1.Name = "_TextBox1";
            _TextBox1.Size = new Size(800, 40);
            _TextBox1.TabIndex = 4;
            _TextBox1.Text = "Sample description: shows how to add variably configured graphics to a display." + '\r' + '\n' + "Sample usage: click the buttons to add or remove graphics.";
            // 
            // DisplayGraphicsForm
            // 
            AutoScaleBaseSize = new Size(5, 13);
            ClientSize = new Size(872, 454);
            Controls.Add(_TextBox1);
            Controls.Add(_RemoveGraphics);
            Controls.Add(_AddStatic);
            Controls.Add(_GroupBox1);
            Controls.Add(_GraphicsDisplay);
            Name = "DisplayGraphicsForm";
            Text = "Display Graphics Sample Application";
            ((System.ComponentModel.ISupportInitialize)_GraphicsDisplay).EndInit();
            _GroupBox1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        #region Module Level vars
        public enum GraphicInteractivityLevel
        {
            eNonInteractive = 0,
            eNonSelectable = 1,
            eFullyInteractive = 2
        }
        #endregion
        #region Form and Controls Events
        private void DisplayGraphicsForm_Load(object sender, EventArgs e)
        {
            // Install a display image.
            // NOTE: An image must be installed before adding any graphics. Otherwise,
            // the graphics will not appear.
            GraphicsDisplay.Image = SyntheticImage();
        }
        private void AddNonInteractive_Click(object sender, EventArgs e)
        {
            AddInteractive(CogColorConstants.Red, GraphicInteractivityLevel.eNonInteractive);
        }

        private void AddNonSelectable_Click(object sender, EventArgs e)
        {
            AddInteractive(CogColorConstants.Yellow, GraphicInteractivityLevel.eNonSelectable);
        }

        private void AddFullyInteractive_Click(object sender, EventArgs e)
        {
            AddInteractive(CogColorConstants.Green, GraphicInteractivityLevel.eFullyInteractive);
        }

        private double _AddStatic_Click_StaticPositionX = default;
        private double _AddStatic_Click_StaticPositionY = default;

        // The first static graphic is installed at coordinate (0,50). This coordinate
        // was chosen so that static graphics do not overlap interactive graphics.
        // NOTE: Unlike an interactive graphic, once a static graphic is added to the
        // Cognex Display control, it cannot be accessed. Thus, it is necessary
        // to create two static variables to compute CenterX and CenterY.
        private void AddStatic_Click(object sender, EventArgs e)
        {
            if (GraphicsDisplay.StaticGraphics.ZOrderGroups.Count == 0)
            {
                _AddStatic_Click_StaticPositionX = 0d;
                _AddStatic_Click_StaticPositionY = 50d;
            }

            // Step 1 - Create and initialize the new graphic.
            CogRectangleAffine StaticGraphic;
            StaticGraphic = new CogRectangleAffine();
            // Set the graphic's display location
            StaticGraphic.CenterX = _AddStatic_Click_StaticPositionX;
            StaticGraphic.CenterY = _AddStatic_Click_StaticPositionY;
            // Static graphics will not display TipText. The assigned TipText
            // will not appear even though the mouse is placed over the graphic.
            StaticGraphic.TipText = "Static graphics don't support tiptext!!!";
            // The gaphic's color can be changed
            StaticGraphic.Color = CogColorConstants.Orange;
            // The line width can also be defined by the user.
            StaticGraphic.LineWidthInScreenPixels = StaticGraphic.LineWidthInScreenPixels * 3;

            // Step 2) Add the shape to the static graphics collection to display it.
            GraphicsDisplay.StaticGraphics.Add(StaticGraphic, "test");

            // Subsequent graphics are installed at an offset of (50,50) from their
            // predecessor.
            _AddStatic_Click_StaticPositionX = _AddStatic_Click_StaticPositionX + 50d;
            _AddStatic_Click_StaticPositionY = _AddStatic_Click_StaticPositionY + 50d;
        }

        private void RemoveGraphics_Click(object sender, EventArgs e)
        {
            // Call the Clear method for clearing static graphics
            GraphicsDisplay.StaticGraphics.Clear();

            // Call the Remove method for removing each interactive graphic.
            while (GraphicsDisplay.InteractiveGraphics.Count > 0)
                GraphicsDisplay.InteractiveGraphics.Remove(0);
        }
        #endregion
        #region Module Level Auxilliary Routines
        // Create a blank image.
        private CogImage8Grey SyntheticImage()
        {
            CogImage8Grey SyntheticImageRet = default;
            var BlankImage = new CogImage8Grey();
            BlankImage.Allocate(200, 200);

            // Slow, but simple.
            int col;
            int row;
            var loopTo = BlankImage.Width - 1;
            for (col = 0; col <= loopTo; col++)
            {
                var loopTo1 = BlankImage.Height - 1;
                for (row = 0; row <= loopTo1; row++)
                    BlankImage.SetPixel(col, row, 128);
            }

            SyntheticImageRet = BlankImage;
            return SyntheticImageRet;
        }
        // Add an interactive graphic to the graphics display.
        private void AddInteractive(CogColorConstants color, GraphicInteractivityLevel interactivityLevel)
        {
            // Step 1) Create a shape. CogEllipse is used in this example.
            CogEllipse InteractiveGraphic;
            InteractiveGraphic = new CogEllipse();

            if (GraphicsDisplay.InteractiveGraphics.Count == 0)
            {
                // The first interactive graphic is installed at coordinate (0,0).
                InteractiveGraphic.CenterX = 0d;
                InteractiveGraphic.CenterY = 0d;
            }
            else
            {
                // Subsequent graphics are installed at an offset of (50,50) from their predecessor.
                CogEllipse PreviousGraphic;
                PreviousGraphic = (CogEllipse)GraphicsDisplay.InteractiveGraphics[GraphicsDisplay.InteractiveGraphics.Count - 1];
                InteractiveGraphic.CenterX = PreviousGraphic.CenterX + 50d;
                InteractiveGraphic.CenterY = PreviousGraphic.CenterY + 50d;
            }

            // Add a TipText. This TipText will appear when the mouse is placed over the graphic.
            InteractiveGraphic.TipText = "Interactive Graphic Number " + (InteractiveGraphic.CenterX / 50d).ToString();
            // Set the graphic's color
            InteractiveGraphic.Color = color;

            // Step 2) Retrieve the CogGraphicInteractive interface from the interactive shape.
            // Some graphic properties are only accessible through the generic interface.
            ICogGraphicInteractive GenericInteractive;
            GenericInteractive = InteractiveGraphic;

            // Step 3) Set the graphic's degree of freedom (DOF) property.
            // There is no mechanism for making an interactive graphic non-selectable
            // without making it non-interactive.  If the selected color is also the same
            // as the unselected color, they both appear to be unselectable.
            InteractiveGraphic.Interactive = interactivityLevel > GraphicInteractivityLevel.eNonInteractive;
            if (interactivityLevel > GraphicInteractivityLevel.eNonSelectable)
            {
                InteractiveGraphic.GraphicDOFEnable = CogEllipseDOFConstants.All;
            }
            else
            {
                GenericInteractive.SelectedColor = InteractiveGraphic.Color;
                GenericInteractive.GraphicDOFEnableBase = CogGraphicDOFConstants.None;
            }

            // Draw the graphic using a line style and mouse cursor that correspond to
            // its degree of interactivity.  See CogStandardCursorConstants for all
            // supported cursor types.  See CogGraphicLineStyleConstants for all
            // supported line styles.
            switch (interactivityLevel)
            {
                case GraphicInteractivityLevel.eNonInteractive:
                    {
                        // No special cursor.
                        InteractiveGraphic.LineStyle = CogGraphicLineStyleConstants.Dot;
                        break;
                    }
                case GraphicInteractivityLevel.eNonSelectable:
                    {
                        GenericInteractive.MouseCursor = CogStandardCursorConstants.TipTextGraphic;
                        InteractiveGraphic.LineStyle = CogGraphicLineStyleConstants.Dash;
                        GenericInteractive.SelectedLineStyle = CogGraphicLineStyleConstants.Dash;
                        break;
                    }
                case GraphicInteractivityLevel.eFullyInteractive:
                    {
                        GenericInteractive.MouseCursor = CogStandardCursorConstants.ManipulableGraphic;
                        InteractiveGraphic.LineStyle = CogGraphicLineStyleConstants.Solid;
                        GenericInteractive.SelectedLineStyle = CogGraphicLineStyleConstants.Solid;
                        GenericInteractive.DragLineStyle = CogGraphicLineStyleConstants.Solid;
                        break;
                    }

                default:
                    {
                        MessageBox.Show("Unrecognized interactivity level.");
                        break;
                    }
            }

            // Step 4) Add the shape to the interactive graphics collection to display it.
            GraphicsDisplay.InteractiveGraphics.Add(InteractiveGraphic, "test", false);

        }

        #endregion
    }
}