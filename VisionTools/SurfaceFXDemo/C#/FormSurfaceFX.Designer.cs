namespace AcqFifoDemo
{
    partial class FormSurfaceFX
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSurfaceFX));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stlStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.cogDisplay_LitFromRight = new Cognex.VisionPro.Display.CogDisplay();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnChoose_LitFromTop = new System.Windows.Forms.Button();
            this.checkBox_LitFromTop = new System.Windows.Forms.CheckBox();
            this.btnChoose_LitFromLeft = new System.Windows.Forms.Button();
            this.btnChoose_LitFromBottom = new System.Windows.Forms.Button();
            this.checkBox_LitFromRight = new System.Windows.Forms.CheckBox();
            this.cogDisplay_LitFromTop = new Cognex.VisionPro.Display.CogDisplay();
            this.cogDisplay_LitFromBottom = new Cognex.VisionPro.Display.CogDisplay();
            this.checkBox_LitFromLeft = new System.Windows.Forms.CheckBox();
            this.btnChoose_LitFromRight = new System.Windows.Forms.Button();
            this.cogDisplay_LitFromLeft = new Cognex.VisionPro.Display.CogDisplay();
            this.checkBox_LitFromBottom = new System.Windows.Forms.CheckBox();
            this.cogRecordsDisplay1 = new Cognex.VisionPro.CogRecordsDisplay();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.ImageAcquisition = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_ImageFiles = new System.Windows.Forms.RadioButton();
            this.radioButton_Acquire = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SurfaceFX = new System.Windows.Forms.TabPage();
            this.cogSurfaceFXEditV21 = new Cognex.VisionPro.SurfaceFX.CogSurfaceFXEditV2();
            this.ImageAcquisitionCommonDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnRunSFX = new System.Windows.Forms.Button();
            this.btnAcquire_LitFromRight = new System.Windows.Forms.Button();
            this.btnAcquire_LitFromBottom = new System.Windows.Forms.Button();
            this.btnAcquire_LitFromLeft = new System.Windows.Forms.Button();
            this.btnAcquire_LitFromTop = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_LitFromRight)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_LitFromTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_LitFromBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_LitFromLeft)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.ImageAcquisition.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SurfaceFX.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogSurfaceFXEditV21)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stlStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 699);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(771, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stlStatus
            // 
            this.stlStatus.Name = "stlStatus";
            this.stlStatus.Size = new System.Drawing.Size(756, 17);
            this.stlStatus.Spring = true;
            this.stlStatus.Text = "Ready";
            this.stlStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cogDisplay1
            // 
            this.cogDisplay_LitFromRight.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_LitFromRight.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_LitFromRight.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_LitFromRight.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_LitFromRight.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_LitFromRight.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_LitFromRight.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_LitFromRight.Location = new System.Drawing.Point(63, 49);
            this.cogDisplay_LitFromRight.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_LitFromRight.MouseWheelSensitivity = 1D;
            this.cogDisplay_LitFromRight.Name = "cogDisplay1";
            this.cogDisplay_LitFromRight.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay1.OcxState")));
            this.cogDisplay_LitFromRight.Size = new System.Drawing.Size(152, 143);
            this.cogDisplay_LitFromRight.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnAcquire_LitFromTop);
            this.groupBox1.Controls.Add(this.btnAcquire_LitFromLeft);
            this.groupBox1.Controls.Add(this.btnAcquire_LitFromBottom);
            this.groupBox1.Controls.Add(this.btnAcquire_LitFromRight);
            this.groupBox1.Controls.Add(this.btnChoose_LitFromTop);
            this.groupBox1.Controls.Add(this.checkBox_LitFromTop);
            this.groupBox1.Controls.Add(this.btnChoose_LitFromLeft);
            this.groupBox1.Controls.Add(this.btnChoose_LitFromBottom);
            this.groupBox1.Controls.Add(this.checkBox_LitFromRight);
            this.groupBox1.Controls.Add(this.cogDisplay_LitFromTop);
            this.groupBox1.Controls.Add(this.cogDisplay_LitFromRight);
            this.groupBox1.Controls.Add(this.cogDisplay_LitFromBottom);
            this.groupBox1.Controls.Add(this.checkBox_LitFromLeft);
            this.groupBox1.Controls.Add(this.btnChoose_LitFromRight);
            this.groupBox1.Controls.Add(this.cogDisplay_LitFromLeft);
            this.groupBox1.Controls.Add(this.checkBox_LitFromBottom);
            this.groupBox1.Location = new System.Drawing.Point(6, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(737, 249);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Images";
            // 
            // btnChoose4
            // 
            this.btnChoose_LitFromTop.Enabled = false;
            this.btnChoose_LitFromTop.Location = new System.Drawing.Point(540, 198);
            this.btnChoose_LitFromTop.Name = "btnChoose4";
            this.btnChoose_LitFromTop.Size = new System.Drawing.Size(75, 23);
            this.btnChoose_LitFromTop.TabIndex = 13;
            this.btnChoose_LitFromTop.Text = "Choose File";
            this.btnChoose_LitFromTop.UseVisualStyleBackColor = true;
            this.btnChoose_LitFromTop.Click += new System.EventHandler(this.btnChoose_LitFromTop_Click);
            // 
            // checkBox4
            // 
            this.checkBox_LitFromTop.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_LitFromTop.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBox_LitFromTop.Enabled = false;
            this.checkBox_LitFromTop.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.checkBox_LitFromTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox_LitFromTop.Location = new System.Drawing.Point(537, 20);
            this.checkBox_LitFromTop.Name = "checkBox4";
            this.checkBox_LitFromTop.Size = new System.Drawing.Size(155, 23);
            this.checkBox_LitFromTop.TabIndex = 3;
            this.checkBox_LitFromTop.Text = "Lit From Top";
            this.checkBox_LitFromTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_LitFromTop.UseVisualStyleBackColor = true;
            // 
            // btnChoose3
            // 
            this.btnChoose_LitFromLeft.Enabled = false;
            this.btnChoose_LitFromLeft.Location = new System.Drawing.Point(379, 198);
            this.btnChoose_LitFromLeft.Name = "btnChoose3";
            this.btnChoose_LitFromLeft.Size = new System.Drawing.Size(75, 23);
            this.btnChoose_LitFromLeft.TabIndex = 12;
            this.btnChoose_LitFromLeft.Text = "Choose File";
            this.btnChoose_LitFromLeft.UseVisualStyleBackColor = true;
            this.btnChoose_LitFromLeft.Click += new System.EventHandler(this.btnChoose_LitFromLeft_Click);
            // 
            // btnChoose2
            // 
            this.btnChoose_LitFromBottom.Enabled = false;
            this.btnChoose_LitFromBottom.Location = new System.Drawing.Point(221, 198);
            this.btnChoose_LitFromBottom.Name = "btnChoose2";
            this.btnChoose_LitFromBottom.Size = new System.Drawing.Size(75, 23);
            this.btnChoose_LitFromBottom.TabIndex = 11;
            this.btnChoose_LitFromBottom.Text = "Choose File";
            this.btnChoose_LitFromBottom.UseVisualStyleBackColor = true;
            this.btnChoose_LitFromBottom.Click += new System.EventHandler(this.btnChoose_LitFromBottom_Click);
            // 
            // checkBox1
            // 
            this.checkBox_LitFromRight.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_LitFromRight.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBox_LitFromRight.Enabled = false;
            this.checkBox_LitFromRight.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.checkBox_LitFromRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox_LitFromRight.Location = new System.Drawing.Point(63, 20);
            this.checkBox_LitFromRight.Name = "checkBox1";
            this.checkBox_LitFromRight.Size = new System.Drawing.Size(152, 23);
            this.checkBox_LitFromRight.TabIndex = 0;
            this.checkBox_LitFromRight.Text = "Lit From Right";
            this.checkBox_LitFromRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_LitFromRight.UseVisualStyleBackColor = true;
            // 
            // cogDisplay4
            // 
            this.cogDisplay_LitFromTop.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_LitFromTop.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_LitFromTop.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_LitFromTop.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_LitFromTop.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_LitFromTop.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_LitFromTop.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_LitFromTop.Location = new System.Drawing.Point(537, 49);
            this.cogDisplay_LitFromTop.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_LitFromTop.MouseWheelSensitivity = 1D;
            this.cogDisplay_LitFromTop.Name = "cogDisplay4";
            this.cogDisplay_LitFromTop.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay4.OcxState")));
            this.cogDisplay_LitFromTop.Size = new System.Drawing.Size(155, 143);
            this.cogDisplay_LitFromTop.TabIndex = 7;
            // 
            // cogDisplay2
            // 
            this.cogDisplay_LitFromBottom.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_LitFromBottom.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_LitFromBottom.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_LitFromBottom.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_LitFromBottom.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_LitFromBottom.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_LitFromBottom.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_LitFromBottom.Location = new System.Drawing.Point(221, 49);
            this.cogDisplay_LitFromBottom.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_LitFromBottom.MouseWheelSensitivity = 1D;
            this.cogDisplay_LitFromBottom.Name = "cogDisplay2";
            this.cogDisplay_LitFromBottom.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay2.OcxState")));
            this.cogDisplay_LitFromBottom.Size = new System.Drawing.Size(152, 143);
            this.cogDisplay_LitFromBottom.TabIndex = 5;
            // 
            // checkBox3
            // 
            this.checkBox_LitFromLeft.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_LitFromLeft.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBox_LitFromLeft.Enabled = false;
            this.checkBox_LitFromLeft.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.checkBox_LitFromLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox_LitFromLeft.Location = new System.Drawing.Point(379, 20);
            this.checkBox_LitFromLeft.Name = "checkBox3";
            this.checkBox_LitFromLeft.Size = new System.Drawing.Size(152, 23);
            this.checkBox_LitFromLeft.TabIndex = 2;
            this.checkBox_LitFromLeft.Text = "Lit From Left";
            this.checkBox_LitFromLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_LitFromLeft.UseVisualStyleBackColor = true;
            // 
            // btnChoose
            // 
            this.btnChoose_LitFromRight.Enabled = false;
            this.btnChoose_LitFromRight.Location = new System.Drawing.Point(63, 198);
            this.btnChoose_LitFromRight.Name = "btnChoose";
            this.btnChoose_LitFromRight.Size = new System.Drawing.Size(75, 23);
            this.btnChoose_LitFromRight.TabIndex = 10;
            this.btnChoose_LitFromRight.Text = "Choose File";
            this.btnChoose_LitFromRight.UseVisualStyleBackColor = true;
            this.btnChoose_LitFromRight.Click += new System.EventHandler(this.btnChoose_LitFromRight_Click);
            // 
            // cogDisplay3
            // 
            this.cogDisplay_LitFromLeft.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_LitFromLeft.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_LitFromLeft.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_LitFromLeft.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_LitFromLeft.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_LitFromLeft.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_LitFromLeft.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_LitFromLeft.Location = new System.Drawing.Point(379, 49);
            this.cogDisplay_LitFromLeft.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_LitFromLeft.MouseWheelSensitivity = 1D;
            this.cogDisplay_LitFromLeft.Name = "cogDisplay3";
            this.cogDisplay_LitFromLeft.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay3.OcxState")));
            this.cogDisplay_LitFromLeft.Size = new System.Drawing.Size(152, 143);
            this.cogDisplay_LitFromLeft.TabIndex = 6;
            // 
            // checkBox2
            // 
            this.checkBox_LitFromBottom.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_LitFromBottom.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBox_LitFromBottom.Enabled = false;
            this.checkBox_LitFromBottom.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.checkBox_LitFromBottom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox_LitFromBottom.Location = new System.Drawing.Point(221, 20);
            this.checkBox_LitFromBottom.Name = "checkBox2";
            this.checkBox_LitFromBottom.Size = new System.Drawing.Size(152, 23);
            this.checkBox_LitFromBottom.TabIndex = 1;
            this.checkBox_LitFromBottom.Text = "Lit From Bottom";
            this.checkBox_LitFromBottom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_LitFromBottom.UseVisualStyleBackColor = true;
            // 
            // cogRecordsDisplay1
            // 
            this.cogRecordsDisplay1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cogRecordsDisplay1.Location = new System.Drawing.Point(46, 341);
            this.cogRecordsDisplay1.Name = "cogRecordsDisplay1";
            this.cogRecordsDisplay1.SelectedRecordKey = null;
            this.cogRecordsDisplay1.ShowRecordsDropDown = true;
            this.cogRecordsDisplay1.Size = new System.Drawing.Size(652, 287);
            this.cogRecordsDisplay1.Subject = null;
            this.cogRecordsDisplay1.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.ImageAcquisition);
            this.tabControl1.Controls.Add(this.SurfaceFX);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(759, 684);
            this.tabControl1.TabIndex = 10;
            // 
            // ImageAcquisition
            // 
            this.ImageAcquisition.Controls.Add(this.btnRunSFX);
            this.ImageAcquisition.Controls.Add(this.groupBox2);
            this.ImageAcquisition.Controls.Add(this.label1);
            this.ImageAcquisition.Controls.Add(this.cogRecordsDisplay1);
            this.ImageAcquisition.Controls.Add(this.groupBox1);
            this.ImageAcquisition.Location = new System.Drawing.Point(4, 22);
            this.ImageAcquisition.Name = "ImageAcquisition";
            this.ImageAcquisition.Padding = new System.Windows.Forms.Padding(3);
            this.ImageAcquisition.Size = new System.Drawing.Size(751, 658);
            this.ImageAcquisition.TabIndex = 0;
            this.ImageAcquisition.Text = "Image Acquisition";
            this.ImageAcquisition.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_ImageFiles);
            this.groupBox2.Controls.Add(this.radioButton_Acquire);
            this.groupBox2.Location = new System.Drawing.Point(247, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(279, 43);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sources";
            // 
            // radioButton1
            // 
            this.radioButton_ImageFiles.AutoSize = true;
            this.radioButton_ImageFiles.Checked = true;
            this.radioButton_ImageFiles.Location = new System.Drawing.Point(21, 19);
            this.radioButton_ImageFiles.Name = "radioButton1";
            this.radioButton_ImageFiles.Size = new System.Drawing.Size(104, 17);
            this.radioButton_ImageFiles.TabIndex = 11;
            this.radioButton_ImageFiles.TabStop = true;
            this.radioButton_ImageFiles.Text = "Use Live Display";
            this.radioButton_ImageFiles.UseVisualStyleBackColor = true;
            this.radioButton_ImageFiles.CheckedChanged += new System.EventHandler(this.radioButton_ImageFiles_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton_Acquire.AutoSize = true;
            this.radioButton_Acquire.Location = new System.Drawing.Point(136, 19);
            this.radioButton_Acquire.Name = "radioButton2";
            this.radioButton_Acquire.Size = new System.Drawing.Size(132, 17);
            this.radioButton_Acquire.TabIndex = 12;
            this.radioButton_Acquire.Text = "Select Image From File";
            this.radioButton_Acquire.UseVisualStyleBackColor = true;
            this.radioButton_Acquire.CheckedChanged += new System.EventHandler(this.radioButton_Acquire_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 318);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(737, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Results";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SurfaceFX
            // 
            this.SurfaceFX.Controls.Add(this.cogSurfaceFXEditV21);
            this.SurfaceFX.Location = new System.Drawing.Point(4, 22);
            this.SurfaceFX.Name = "SurfaceFX";
            this.SurfaceFX.Size = new System.Drawing.Size(751, 658);
            this.SurfaceFX.TabIndex = 2;
            this.SurfaceFX.Text = "SurfaceFX";
            this.SurfaceFX.UseVisualStyleBackColor = true;
            // 
            // cogSurfaceFXEditV21
            // 
            this.cogSurfaceFXEditV21.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cogSurfaceFXEditV21.Location = new System.Drawing.Point(3, 3);
            this.cogSurfaceFXEditV21.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogSurfaceFXEditV21.Name = "cogSurfaceFXEditV21";
            this.cogSurfaceFXEditV21.Size = new System.Drawing.Size(745, 652);
            this.cogSurfaceFXEditV21.SuspendElectricRuns = false;
            this.cogSurfaceFXEditV21.TabIndex = 0;
            // 
            // btnRunSFX
            // 
            this.btnRunSFX.Location = new System.Drawing.Point(340, 629);
            this.btnRunSFX.Name = "btnRunSFX";
            this.btnRunSFX.Size = new System.Drawing.Size(75, 23);
            this.btnRunSFX.TabIndex = 14;
            this.btnRunSFX.Text = "RUN";
            this.btnRunSFX.UseVisualStyleBackColor = true;
            this.btnRunSFX.Click += new System.EventHandler(this.btnRunSurfaceFX_Click);
            // 
            // btnAcquire1
            // 
            this.btnAcquire_LitFromRight.Location = new System.Drawing.Point(140, 198);
            this.btnAcquire_LitFromRight.Name = "btnAcquire1";
            this.btnAcquire_LitFromRight.Size = new System.Drawing.Size(75, 23);
            this.btnAcquire_LitFromRight.TabIndex = 14;
            this.btnAcquire_LitFromRight.Text = "Acquire";
            this.btnAcquire_LitFromRight.UseVisualStyleBackColor = true;
            this.btnAcquire_LitFromRight.Click += new System.EventHandler(this.btnAcquire_LitFromRight_Click);
            // 
            // btnAcquire2
            // 
            this.btnAcquire_LitFromBottom.Location = new System.Drawing.Point(298, 198);
            this.btnAcquire_LitFromBottom.Name = "btnAcquire2";
            this.btnAcquire_LitFromBottom.Size = new System.Drawing.Size(75, 23);
            this.btnAcquire_LitFromBottom.TabIndex = 15;
            this.btnAcquire_LitFromBottom.Text = "Acquire";
            this.btnAcquire_LitFromBottom.UseVisualStyleBackColor = true;
            this.btnAcquire_LitFromBottom.Click += new System.EventHandler(this.btnAcquire_LitFromBottom_Click);
            // 
            // btnAcquire3
            // 
            this.btnAcquire_LitFromLeft.Location = new System.Drawing.Point(456, 198);
            this.btnAcquire_LitFromLeft.Name = "btnAcquire3";
            this.btnAcquire_LitFromLeft.Size = new System.Drawing.Size(75, 23);
            this.btnAcquire_LitFromLeft.TabIndex = 16;
            this.btnAcquire_LitFromLeft.Text = "Acquire";
            this.btnAcquire_LitFromLeft.UseVisualStyleBackColor = true;
            this.btnAcquire_LitFromLeft.Click += new System.EventHandler(this.btnAcquire_LitFromLeft_Click);
            // 
            // btnAcquire4
            // 
            this.btnAcquire_LitFromTop.Location = new System.Drawing.Point(617, 198);
            this.btnAcquire_LitFromTop.Name = "btnAcquire4";
            this.btnAcquire_LitFromTop.Size = new System.Drawing.Size(75, 23);
            this.btnAcquire_LitFromTop.TabIndex = 17;
            this.btnAcquire_LitFromTop.Text = "Acquire";
            this.btnAcquire_LitFromTop.UseVisualStyleBackColor = true;
            this.btnAcquire_LitFromTop.Click += new System.EventHandler(this.btnAcquire_LitFromTop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 721);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "Surface FX Demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_LitFromRight)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_LitFromTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_LitFromBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_LitFromLeft)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ImageAcquisition.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.SurfaceFX.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogSurfaceFXEditV21)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stlStatus;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay_LitFromRight;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_LitFromTop;
        private System.Windows.Forms.CheckBox checkBox_LitFromLeft;
        private System.Windows.Forms.CheckBox checkBox_LitFromBottom;
        private System.Windows.Forms.CheckBox checkBox_LitFromRight;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay_LitFromBottom;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay_LitFromLeft;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay_LitFromTop;
        private Cognex.VisionPro.CogRecordsDisplay cogRecordsDisplay1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage ImageAcquisition;
        private System.Windows.Forms.TabPage SurfaceFX;
        private Cognex.VisionPro.SurfaceFX.CogSurfaceFXEditV2 cogSurfaceFXEditV21;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton_Acquire;
        private System.Windows.Forms.RadioButton radioButton_ImageFiles;
        private System.Windows.Forms.Button btnChoose_LitFromRight;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.OpenFileDialog ImageAcquisitionCommonDialog;
        private System.Windows.Forms.Button btnChoose_LitFromTop;
        private System.Windows.Forms.Button btnChoose_LitFromLeft;
        private System.Windows.Forms.Button btnChoose_LitFromBottom;
        private System.Windows.Forms.Button btnRunSFX;
        private System.Windows.Forms.Button btnAcquire_LitFromTop;
        private System.Windows.Forms.Button btnAcquire_LitFromLeft;
        private System.Windows.Forms.Button btnAcquire_LitFromBottom;
        private System.Windows.Forms.Button btnAcquire_LitFromRight;
    }
}

