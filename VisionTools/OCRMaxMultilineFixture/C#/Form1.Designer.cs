//*****************************************************************************
// Copyright (C) 2011 Cognex Corporation
//
// Subject to Cognex Corporation's terms and conditions and license
// agreement, you are authorized to use and modify this source code in
// any way you find useful, provided the Software and/or the modified
// Software is used solely in conjunction with a Cognex Machine Vision
// System.  Furthermore you acknowledge and agree that Cognex has no
// warranty, obligations or liability for your use of the Software.
//*****************************************************************************
// This sample program is designed to illustrate certain VisionPro
// features or techniques in the simplest way possible. It is not
// intended as the framework for a complete application. In particular,
// the sample program may not provide proper error handling, event
// handling, cleanup, repeatability, and other mechanisms that a
// commercial quality application requires.
//
// This program assumes that you have some knowledge of C# and VisionPro
// programming.
//
// This sample program demonstrates the programmatic use of the VisionPro
// OCRMax Tool.
//

namespace OCRMaxMultiLineFixture
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txt_Description = new System.Windows.Forms.TextBox();
            this.gbx_no1 = new System.Windows.Forms.GroupBox();
            this.lbl_Status = new System.Windows.Forms.Label();
            this.txt_TrainString = new System.Windows.Forms.TextBox();
            this.txt_FileName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_TrainTool = new System.Windows.Forms.Button();
            this.btn_AddFonts = new System.Windows.Forms.Button();
            this.btn_ExtractChars = new System.Windows.Forms.Button();
            this.btn_OpenFile = new System.Windows.Forms.Button();
            this.cogRecordsDisplay = new Cognex.VisionPro.CogRecordsDisplay();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckb_DisplayAltChars = new System.Windows.Forms.CheckBox();
            this.txt_CharNum = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_RunTool = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_LoadImgFile = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cogDisplay3 = new Cognex.VisionPro.Display.CogDisplay();
            this.pnl_DisplayPanel = new System.Windows.Forms.Panel();
            this.btn_Up = new System.Windows.Forms.Button();
            this.btn_Down = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_Score0 = new System.Windows.Forms.TextBox();
            this.txt_Char0 = new System.Windows.Forms.TextBox();
            this.txt_Char3 = new System.Windows.Forms.TextBox();
            this.txt_Char2 = new System.Windows.Forms.TextBox();
            this.txt_Char1 = new System.Windows.Forms.TextBox();
            this.txt_Score3 = new System.Windows.Forms.TextBox();
            this.txt_Score2 = new System.Windows.Forms.TextBox();
            this.txt_Score1 = new System.Windows.Forms.TextBox();
            this.lbl_AltChar1 = new System.Windows.Forms.Label();
            this.lbl_AltChar2 = new System.Windows.Forms.Label();
            this.lbl_AltChar3 = new System.Windows.Forms.Label();
            this.cogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
            this.cogDisplay2 = new Cognex.VisionPro.Display.CogDisplay();
            this.label10 = new System.Windows.Forms.Label();
            this.gbx_no1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay3)).BeginInit();
            this.pnl_DisplayPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay2)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_Description
            // 
            this.txt_Description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Description.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txt_Description.Location = new System.Drawing.Point(12, 471);
            this.txt_Description.Multiline = true;
            this.txt_Description.Name = "txt_Description";
            this.txt_Description.ReadOnly = true;
            this.txt_Description.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_Description.Size = new System.Drawing.Size(645, 127);
            this.txt_Description.TabIndex = 2;
            // 
            // gbx_no1
            // 
            this.gbx_no1.Controls.Add(this.lbl_Status);
            this.gbx_no1.Controls.Add(this.txt_TrainString);
            this.gbx_no1.Controls.Add(this.txt_FileName);
            this.gbx_no1.Controls.Add(this.label5);
            this.gbx_no1.Controls.Add(this.label4);
            this.gbx_no1.Controls.Add(this.label3);
            this.gbx_no1.Controls.Add(this.label2);
            this.gbx_no1.Controls.Add(this.label1);
            this.gbx_no1.Controls.Add(this.btn_TrainTool);
            this.gbx_no1.Controls.Add(this.btn_AddFonts);
            this.gbx_no1.Controls.Add(this.btn_ExtractChars);
            this.gbx_no1.Controls.Add(this.btn_OpenFile);
            this.gbx_no1.Location = new System.Drawing.Point(12, 12);
            this.gbx_no1.Name = "gbx_no1";
            this.gbx_no1.Size = new System.Drawing.Size(645, 140);
            this.gbx_no1.TabIndex = 4;
            this.gbx_no1.TabStop = false;
            this.gbx_no1.Text = "Initialize Tools";
            // 
            // lbl_Status
            // 
            this.lbl_Status.AutoSize = true;
            this.lbl_Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbl_Status.Location = new System.Drawing.Point(499, 107);
            this.lbl_Status.Name = "lbl_Status";
            this.lbl_Status.Size = new System.Drawing.Size(140, 20);
            this.lbl_Status.TabIndex = 4;
            this.lbl_Status.Text = "Tool not trained.";
            // 
            // txt_TrainString
            // 
            this.txt_TrainString.Location = new System.Drawing.Point(156, 51);
            this.txt_TrainString.Name = "txt_TrainString";
            this.txt_TrainString.ReadOnly = true;
            this.txt_TrainString.Size = new System.Drawing.Size(484, 20);
            this.txt_TrainString.TabIndex = 3;
            // 
            // txt_FileName
            // 
            this.txt_FileName.Location = new System.Drawing.Point(42, 24);
            this.txt_FileName.Name = "txt_FileName";
            this.txt_FileName.ReadOnly = true;
            this.txt_FileName.Size = new System.Drawing.Size(475, 20);
            this.txt_FileName.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(298, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = ">";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(162, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = ">";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(6, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(500, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Segment the image, and match the previously known characters with the extracted i" +
    "mages:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(6, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Apriori known train string:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "File:";
            // 
            // btn_TrainTool
            // 
            this.btn_TrainTool.Enabled = false;
            this.btn_TrainTool.Location = new System.Drawing.Point(318, 104);
            this.btn_TrainTool.Name = "btn_TrainTool";
            this.btn_TrainTool.Size = new System.Drawing.Size(110, 23);
            this.btn_TrainTool.TabIndex = 0;
            this.btn_TrainTool.Text = "Train tool";
            this.btn_TrainTool.UseVisualStyleBackColor = true;
            this.btn_TrainTool.Click += new System.EventHandler(this.btn_TrainTool_Click);
            // 
            // btn_AddFonts
            // 
            this.btn_AddFonts.Enabled = false;
            this.btn_AddFonts.Location = new System.Drawing.Point(182, 104);
            this.btn_AddFonts.Name = "btn_AddFonts";
            this.btn_AddFonts.Size = new System.Drawing.Size(110, 23);
            this.btn_AddFonts.TabIndex = 0;
            this.btn_AddFonts.Text = "Add chars to font";
            this.btn_AddFonts.UseVisualStyleBackColor = true;
            this.btn_AddFonts.Click += new System.EventHandler(this.btn_AddFonts_Click);
            // 
            // btn_ExtractChars
            // 
            this.btn_ExtractChars.Enabled = false;
            this.btn_ExtractChars.Location = new System.Drawing.Point(46, 104);
            this.btn_ExtractChars.Name = "btn_ExtractChars";
            this.btn_ExtractChars.Size = new System.Drawing.Size(110, 23);
            this.btn_ExtractChars.TabIndex = 0;
            this.btn_ExtractChars.Text = "Segment";
            this.btn_ExtractChars.UseVisualStyleBackColor = true;
            this.btn_ExtractChars.Click += new System.EventHandler(this.btn_ExtractChars_Click);
            // 
            // btn_OpenFile
            // 
            this.btn_OpenFile.Location = new System.Drawing.Point(523, 22);
            this.btn_OpenFile.Name = "btn_OpenFile";
            this.btn_OpenFile.Size = new System.Drawing.Size(110, 23);
            this.btn_OpenFile.TabIndex = 0;
            this.btn_OpenFile.Text = "Open train image";
            this.btn_OpenFile.UseVisualStyleBackColor = true;
            this.btn_OpenFile.Click += new System.EventHandler(this.btn_OpenFile_Click);
            // 
            // cogRecordsDisplay
            // 
            this.cogRecordsDisplay.Location = new System.Drawing.Point(12, 232);
            this.cogRecordsDisplay.Name = "cogRecordsDisplay";
            this.cogRecordsDisplay.SelectedRecordKey = null;
            this.cogRecordsDisplay.ShowRecordsDropDown = true;
            this.cogRecordsDisplay.Size = new System.Drawing.Size(645, 233);
            this.cogRecordsDisplay.Subject = null;
            this.cogRecordsDisplay.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckb_DisplayAltChars);
            this.groupBox1.Controls.Add(this.txt_CharNum);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btn_RunTool);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.btn_LoadImgFile);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(12, 159);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(645, 67);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Multiline OCR";
            // 
            // ckb_DisplayAltChars
            // 
            this.ckb_DisplayAltChars.AutoSize = true;
            this.ckb_DisplayAltChars.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ckb_DisplayAltChars.Location = new System.Drawing.Point(610, 42);
            this.ckb_DisplayAltChars.Name = "ckb_DisplayAltChars";
            this.ckb_DisplayAltChars.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ckb_DisplayAltChars.Size = new System.Drawing.Size(15, 14);
            this.ckb_DisplayAltChars.TabIndex = 3;
            this.ckb_DisplayAltChars.UseVisualStyleBackColor = true;
            this.ckb_DisplayAltChars.CheckedChanged += new System.EventHandler(this.ckb_DisplayAltChars_CheckedChanged);
            // 
            // txt_CharNum
            // 
            this.txt_CharNum.Location = new System.Drawing.Point(603, 13);
            this.txt_CharNum.Name = "txt_CharNum";
            this.txt_CharNum.ReadOnly = true;
            this.txt_CharNum.Size = new System.Drawing.Size(30, 20);
            this.txt_CharNum.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label9.Location = new System.Drawing.Point(464, 41);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(133, 15);
            this.label9.TabIndex = 2;
            this.label9.Text = "Show alternative chars:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(472, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "Characters in the font:";
            // 
            // btn_RunTool
            // 
            this.btn_RunTool.Enabled = false;
            this.btn_RunTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn_RunTool.Location = new System.Drawing.Point(318, 24);
            this.btn_RunTool.Name = "btn_RunTool";
            this.btn_RunTool.Size = new System.Drawing.Size(110, 23);
            this.btn_RunTool.TabIndex = 0;
            this.btn_RunTool.Text = "Run tool";
            this.btn_RunTool.UseVisualStyleBackColor = true;
            this.btn_RunTool.Click += new System.EventHandler(this.btn_RunTool_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label8.Location = new System.Drawing.Point(298, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 15);
            this.label8.TabIndex = 2;
            this.label8.Text = ">";
            // 
            // btn_LoadImgFile
            // 
            this.btn_LoadImgFile.Enabled = false;
            this.btn_LoadImgFile.Location = new System.Drawing.Point(182, 24);
            this.btn_LoadImgFile.Name = "btn_LoadImgFile";
            this.btn_LoadImgFile.Size = new System.Drawing.Size(110, 23);
            this.btn_LoadImgFile.TabIndex = 0;
            this.btn_LoadImgFile.Text = "Open";
            this.btn_LoadImgFile.UseVisualStyleBackColor = true;
            this.btn_LoadImgFile.Click += new System.EventHandler(this.btn_LoadImgFile_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label7.Location = new System.Drawing.Point(6, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(159, 15);
            this.label7.TabIndex = 2;
            this.label7.Text = "Open image library and run:";
            // 
            // cogDisplay3
            // 
            this.cogDisplay3.Location = new System.Drawing.Point(523, 0);
            this.cogDisplay3.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay3.MouseWheelSensitivity = 1D;
            this.cogDisplay3.Name = "cogDisplay3";
            this.cogDisplay3.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay3.OcxState")));
            this.cogDisplay3.Size = new System.Drawing.Size(105, 124);
            this.cogDisplay3.TabIndex = 7;
            // 
            // pnl_DisplayPanel
            // 
            this.pnl_DisplayPanel.Controls.Add(this.btn_Up);
            this.pnl_DisplayPanel.Controls.Add(this.btn_Down);
            this.pnl_DisplayPanel.Controls.Add(this.label14);
            this.pnl_DisplayPanel.Controls.Add(this.label13);
            this.pnl_DisplayPanel.Controls.Add(this.label12);
            this.pnl_DisplayPanel.Controls.Add(this.label11);
            this.pnl_DisplayPanel.Controls.Add(this.txt_Score0);
            this.pnl_DisplayPanel.Controls.Add(this.txt_Char0);
            this.pnl_DisplayPanel.Controls.Add(this.txt_Char3);
            this.pnl_DisplayPanel.Controls.Add(this.txt_Char2);
            this.pnl_DisplayPanel.Controls.Add(this.txt_Char1);
            this.pnl_DisplayPanel.Controls.Add(this.txt_Score3);
            this.pnl_DisplayPanel.Controls.Add(this.txt_Score2);
            this.pnl_DisplayPanel.Controls.Add(this.txt_Score1);
            this.pnl_DisplayPanel.Controls.Add(this.lbl_AltChar1);
            this.pnl_DisplayPanel.Controls.Add(this.lbl_AltChar2);
            this.pnl_DisplayPanel.Controls.Add(this.lbl_AltChar3);
            this.pnl_DisplayPanel.Controls.Add(this.cogDisplay1);
            this.pnl_DisplayPanel.Controls.Add(this.cogDisplay2);
            this.pnl_DisplayPanel.Controls.Add(this.cogDisplay3);
            this.pnl_DisplayPanel.Controls.Add(this.label10);
            this.pnl_DisplayPanel.Location = new System.Drawing.Point(12, 471);
            this.pnl_DisplayPanel.Name = "pnl_DisplayPanel";
            this.pnl_DisplayPanel.Size = new System.Drawing.Size(645, 127);
            this.pnl_DisplayPanel.TabIndex = 8;
            this.pnl_DisplayPanel.Visible = false;
            // 
            // btn_Up
            // 
            this.btn_Up.Location = new System.Drawing.Point(54, 76);
            this.btn_Up.Name = "btn_Up";
            this.btn_Up.Size = new System.Drawing.Size(25, 23);
            this.btn_Up.TabIndex = 11;
            this.btn_Up.Text = ">";
            this.btn_Up.UseVisualStyleBackColor = true;
            this.btn_Up.Click += new System.EventHandler(this.btn_Up_Click);
            // 
            // btn_Down
            // 
            this.btn_Down.Location = new System.Drawing.Point(26, 76);
            this.btn_Down.Name = "btn_Down";
            this.btn_Down.Size = new System.Drawing.Size(25, 23);
            this.btn_Down.TabIndex = 11;
            this.btn_Down.Text = "<";
            this.btn_Down.UseVisualStyleBackColor = true;
            this.btn_Down.Click += new System.EventHandler(this.btn_Down_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(170, 4);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(35, 13);
            this.label14.TabIndex = 10;
            this.label14.Text = "Score";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(96, 4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 13);
            this.label13.TabIndex = 10;
            this.label13.Text = "Char";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(23, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Best match:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 53);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Alternatives:";
            // 
            // txt_Score0
            // 
            this.txt_Score0.BackColor = System.Drawing.SystemColors.Window;
            this.txt_Score0.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txt_Score0.Location = new System.Drawing.Point(152, 21);
            this.txt_Score0.Name = "txt_Score0";
            this.txt_Score0.ReadOnly = true;
            this.txt_Score0.Size = new System.Drawing.Size(75, 21);
            this.txt_Score0.TabIndex = 9;
            // 
            // txt_Char0
            // 
            this.txt_Char0.BackColor = System.Drawing.SystemColors.Window;
            this.txt_Char0.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txt_Char0.Location = new System.Drawing.Point(97, 20);
            this.txt_Char0.Name = "txt_Char0";
            this.txt_Char0.ReadOnly = true;
            this.txt_Char0.Size = new System.Drawing.Size(26, 21);
            this.txt_Char0.TabIndex = 9;
            // 
            // txt_Char3
            // 
            this.txt_Char3.BackColor = System.Drawing.SystemColors.Window;
            this.txt_Char3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txt_Char3.Location = new System.Drawing.Point(97, 102);
            this.txt_Char3.Name = "txt_Char3";
            this.txt_Char3.ReadOnly = true;
            this.txt_Char3.Size = new System.Drawing.Size(26, 21);
            this.txt_Char3.TabIndex = 9;
            // 
            // txt_Char2
            // 
            this.txt_Char2.BackColor = System.Drawing.SystemColors.Window;
            this.txt_Char2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txt_Char2.Location = new System.Drawing.Point(97, 76);
            this.txt_Char2.Name = "txt_Char2";
            this.txt_Char2.ReadOnly = true;
            this.txt_Char2.Size = new System.Drawing.Size(26, 21);
            this.txt_Char2.TabIndex = 9;
            // 
            // txt_Char1
            // 
            this.txt_Char1.BackColor = System.Drawing.SystemColors.Window;
            this.txt_Char1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txt_Char1.Location = new System.Drawing.Point(97, 50);
            this.txt_Char1.Name = "txt_Char1";
            this.txt_Char1.ReadOnly = true;
            this.txt_Char1.Size = new System.Drawing.Size(26, 21);
            this.txt_Char1.TabIndex = 9;
            // 
            // txt_Score3
            // 
            this.txt_Score3.BackColor = System.Drawing.SystemColors.Window;
            this.txt_Score3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txt_Score3.Location = new System.Drawing.Point(152, 102);
            this.txt_Score3.Name = "txt_Score3";
            this.txt_Score3.ReadOnly = true;
            this.txt_Score3.Size = new System.Drawing.Size(75, 21);
            this.txt_Score3.TabIndex = 9;
            // 
            // txt_Score2
            // 
            this.txt_Score2.BackColor = System.Drawing.SystemColors.Window;
            this.txt_Score2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txt_Score2.Location = new System.Drawing.Point(152, 76);
            this.txt_Score2.Name = "txt_Score2";
            this.txt_Score2.ReadOnly = true;
            this.txt_Score2.Size = new System.Drawing.Size(75, 21);
            this.txt_Score2.TabIndex = 9;
            // 
            // txt_Score1
            // 
            this.txt_Score1.BackColor = System.Drawing.SystemColors.Window;
            this.txt_Score1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txt_Score1.Location = new System.Drawing.Point(152, 50);
            this.txt_Score1.Name = "txt_Score1";
            this.txt_Score1.ReadOnly = true;
            this.txt_Score1.Size = new System.Drawing.Size(75, 21);
            this.txt_Score1.TabIndex = 9;
            // 
            // lbl_AltChar1
            // 
            this.lbl_AltChar1.AutoSize = true;
            this.lbl_AltChar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbl_AltChar1.Location = new System.Drawing.Point(238, 1);
            this.lbl_AltChar1.Name = "lbl_AltChar1";
            this.lbl_AltChar1.Size = new System.Drawing.Size(20, 16);
            this.lbl_AltChar1.TabIndex = 8;
            this.lbl_AltChar1.Text = "1.";
            // 
            // lbl_AltChar2
            // 
            this.lbl_AltChar2.AutoSize = true;
            this.lbl_AltChar2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbl_AltChar2.Location = new System.Drawing.Point(370, 1);
            this.lbl_AltChar2.Name = "lbl_AltChar2";
            this.lbl_AltChar2.Size = new System.Drawing.Size(20, 16);
            this.lbl_AltChar2.TabIndex = 8;
            this.lbl_AltChar2.Text = "2.";
            // 
            // lbl_AltChar3
            // 
            this.lbl_AltChar3.AutoSize = true;
            this.lbl_AltChar3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbl_AltChar3.Location = new System.Drawing.Point(502, 1);
            this.lbl_AltChar3.Name = "lbl_AltChar3";
            this.lbl_AltChar3.Size = new System.Drawing.Size(20, 16);
            this.lbl_AltChar3.TabIndex = 8;
            this.lbl_AltChar3.Text = "3.";
            // 
            // cogDisplay1
            // 
            this.cogDisplay1.Location = new System.Drawing.Point(259, 0);
            this.cogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay1.MouseWheelSensitivity = 1D;
            this.cogDisplay1.Name = "cogDisplay1";
            this.cogDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay1.OcxState")));
            this.cogDisplay1.Size = new System.Drawing.Size(105, 124);
            this.cogDisplay1.TabIndex = 7;
            // 
            // cogDisplay2
            // 
            this.cogDisplay2.Location = new System.Drawing.Point(391, 0);
            this.cogDisplay2.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay2.MouseWheelSensitivity = 1D;
            this.cogDisplay2.Name = "cogDisplay2";
            this.cogDisplay2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay2.OcxState")));
            this.cogDisplay2.Size = new System.Drawing.Size(105, 124);
            this.cogDisplay2.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label10.Location = new System.Drawing.Point(6, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 15);
            this.label10.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 610);
            this.Controls.Add(this.pnl_DisplayPanel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cogRecordsDisplay);
            this.Controls.Add(this.gbx_no1);
            this.Controls.Add(this.txt_Description);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Multiline OCR with Fixture - sample application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbx_no1.ResumeLayout(false);
            this.gbx_no1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay3)).EndInit();
            this.pnl_DisplayPanel.ResumeLayout(false);
            this.pnl_DisplayPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Description;
        private System.Windows.Forms.GroupBox gbx_no1;
        private System.Windows.Forms.Button btn_OpenFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_FileName;
        private System.Windows.Forms.Button btn_TrainTool;
        private System.Windows.Forms.Button btn_ExtractChars;
        private Cognex.VisionPro.CogRecordsDisplay cogRecordsDisplay;
        private System.Windows.Forms.TextBox txt_TrainString;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_AddFonts;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_Status;
        private System.Windows.Forms.TextBox txt_CharNum;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_LoadImgFile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_RunTool;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox ckb_DisplayAltChars;
        private System.Windows.Forms.Label label9;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay3;
        private System.Windows.Forms.Panel pnl_DisplayPanel;
        private System.Windows.Forms.Label lbl_AltChar3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_Score0;
        private System.Windows.Forms.TextBox txt_Char0;
        private System.Windows.Forms.TextBox txt_Char3;
        private System.Windows.Forms.TextBox txt_Char2;
        private System.Windows.Forms.TextBox txt_Char1;
        private System.Windows.Forms.TextBox txt_Score3;
        private System.Windows.Forms.TextBox txt_Score2;
        private System.Windows.Forms.TextBox txt_Score1;
        private System.Windows.Forms.Label lbl_AltChar1;
        private System.Windows.Forms.Label lbl_AltChar2;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay1;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay2;
        private System.Windows.Forms.Button btn_Up;
        private System.Windows.Forms.Button btn_Down;
    }
}

