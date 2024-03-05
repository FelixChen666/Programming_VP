/*******************************************************************************
Copyright (C) 2008 Cognex Corporation

Subject to Cognex Corporations terms and conditions and license agreement,
you are authorized to use and modify this source code in any way you find
useful, provided the Software and/or the modified Software is used solely in
conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
and agree that Cognex has no warranty, obligations or liability for your use
of the Software.
*******************************************************************************/
// This sample program is designed to illustrate certain VisionPro
// features or techniques in the simplest way possible. It is not
// intended as the framework for a complete application. In
// particular, the sample program may not provide proper error
// handling, event handling, cleanup, repeatability, and other
// mechanisms that a commercial quality application requires.

namespace TestApplication
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
            this.simpleToolEditV21 = new SimpleTool.SimpleToolEditV2();
            this.cogImageFileEditV21 = new Cognex.VisionPro.ImageFile.CogImageFileEditV2();
            ((System.ComponentModel.ISupportInitialize)(this.simpleToolEditV21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogImageFileEditV21)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleToolEditV21
            // 
            this.simpleToolEditV21.Location = new System.Drawing.Point(12, 325);
            this.simpleToolEditV21.MinimumSize = new System.Drawing.Size(489, 0);
            this.simpleToolEditV21.Name = "simpleToolEditV21";
            this.simpleToolEditV21.Size = new System.Drawing.Size(748, 286);
            this.simpleToolEditV21.SuspendElectricRuns = false;
            this.simpleToolEditV21.TabIndex = 0;
            // 
            // cogImageFileEditV21
            // 
            this.cogImageFileEditV21.AllowDrop = true;
            this.cogImageFileEditV21.Location = new System.Drawing.Point(12, 12);
            this.cogImageFileEditV21.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogImageFileEditV21.Name = "cogImageFileEditV21";
            this.cogImageFileEditV21.Size = new System.Drawing.Size(748, 286);
            this.cogImageFileEditV21.SuspendElectricRuns = false;
            this.cogImageFileEditV21.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 623);
            this.Controls.Add(this.cogImageFileEditV21);
            this.Controls.Add(this.simpleToolEditV21);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.simpleToolEditV21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogImageFileEditV21)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SimpleTool.SimpleToolEditV2 simpleToolEditV21;
        private Cognex.VisionPro.ImageFile.CogImageFileEditV2 cogImageFileEditV21;
    }
}

