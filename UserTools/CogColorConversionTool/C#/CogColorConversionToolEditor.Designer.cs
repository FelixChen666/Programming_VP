/*******************************************************************************
Copyright (C) 2008 Cognex Corporation

Subject to Cognex Corporations terms and conditions and license agreement,
you are authorized to use and modify this source code in any way you find
useful, provided the Software and/or the modified Software is used solely in
conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
and agree that Cognex has no warranty, obligations or liability for your use
of the Software.
*******************************************************************************
 This sample program is designed to illustrate certain VisionPro features or 
 techniques in the simplest way possible. It is not intended as the framework 
 for a complete application. In particular, the sample program may not provide
 proper error handling, event handling, cleanup, repeatability, and other 
 mechanisms that a commercial quality application requires.

This sample demonstrates how a user can create their own Vision Tools using 
base classes the Cognex provides.  The tool created in this class converts 3-plane
RGB images to individual R, G, and B planes, or converts a 3-plane RGB image to 
individual H, S, and I planes.

The tool created is called the CogColorConversionTool.  It makes use of an operator, which
does all the real work, called CogColorConversion.

*/

namespace CogColorConversionTool
{
    partial class CogColorConversionToolEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CogColorConversionToolEditor));
            this.tpgRegion = new System.Windows.Forms.TabPage();
            this.cboRegionShape = new System.Windows.Forms.ComboBox();
            this.lblRegionShape = new System.Windows.Forms.Label();
            this.chkConvertToHSI = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.sbpIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpStatusCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpStatusMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpProcessingTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpTotalTime)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tpgSettings.SuspendLayout();
            this.tpgRegion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tbrButtons
            // 
            this.tbrButtons.Size = new System.Drawing.Size(748, 28);
            // 
            // sbpStatusMessage
            // 
            this.sbpStatusMessage.Width = 502;
            // 
            // imlButtons
            // 
            this.imlButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlButtons.ImageStream")));
            this.imlButtons.Images.SetKeyName(0, "");
            this.imlButtons.Images.SetKeyName(1, "");
            this.imlButtons.Images.SetKeyName(2, "");
            this.imlButtons.Images.SetKeyName(3, "");
            this.imlButtons.Images.SetKeyName(4, "");
            this.imlButtons.Images.SetKeyName(5, "");
            this.imlButtons.Images.SetKeyName(6, "");
            this.imlButtons.Images.SetKeyName(7, "");
            this.imlButtons.Images.SetKeyName(8, "");
            this.imlButtons.Images.SetKeyName(9, "");
            this.imlButtons.Images.SetKeyName(10, "");
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpgRegion);
            this.tabControl.Size = new System.Drawing.Size(489, 484);
            this.tabControl.Controls.SetChildIndex(this.tpgRegion, 0);
            this.tabControl.Controls.SetChildIndex(this.tpgSettings, 0);
            // 
            // tpgSettings
            // 
            this.tpgSettings.Controls.Add(this.chkConvertToHSI);
            this.tpgSettings.Size = new System.Drawing.Size(481, 458);
            this.tpgSettings.UseVisualStyleBackColor = true;
            // 
            // lblControlState
            // 
            this.lblControlState.Location = new System.Drawing.Point(8, 487);
            // 
            // tpgRegion
            // 
            this.tpgRegion.Controls.Add(this.cboRegionShape);
            this.tpgRegion.Controls.Add(this.lblRegionShape);
            this.tpgRegion.Location = new System.Drawing.Point(4, 22);
            this.tpgRegion.Name = "tpgRegion";
            this.tpgRegion.Size = new System.Drawing.Size(481, 458);
            this.tpgRegion.TabIndex = 1;
            this.tpgRegion.Text = "Region";
            this.tpgRegion.UseVisualStyleBackColor = true;
            // 
            // cboRegionShape
            // 
            this.cboRegionShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRegionShape.FormattingEnabled = true;
            this.cboRegionShape.Items.AddRange(new object[] {
            "CogCircle",
            "CogRectangle",
            "<None = Use Entire Image>"});
            this.cboRegionShape.Location = new System.Drawing.Point(36, 58);
            this.cboRegionShape.Name = "cboRegionShape";
            this.cboRegionShape.Size = new System.Drawing.Size(206, 21);
            this.cboRegionShape.TabIndex = 1;
            this.cboRegionShape.SelectedIndexChanged += new System.EventHandler(this.cboRegionShape_SelectedIndexChanged);
            // 
            // lblRegionShape
            // 
            this.lblRegionShape.AutoSize = true;
            this.lblRegionShape.Location = new System.Drawing.Point(33, 24);
            this.lblRegionShape.Name = "lblRegionShape";
            this.lblRegionShape.Size = new System.Drawing.Size(75, 13);
            this.lblRegionShape.TabIndex = 0;
            this.lblRegionShape.Text = "Region Shape";
            // 
            // chkConvertToHSI
            // 
            this.chkConvertToHSI.AutoSize = true;
            this.chkConvertToHSI.Location = new System.Drawing.Point(28, 26);
            this.chkConvertToHSI.Name = "chkConvertToHSI";
            this.chkConvertToHSI.Size = new System.Drawing.Size(96, 17);
            this.chkConvertToHSI.TabIndex = 0;
            this.chkConvertToHSI.Text = "Convert to HSI";
            this.chkConvertToHSI.UseVisualStyleBackColor = true;
            this.chkConvertToHSI.CheckedChanged += new System.EventHandler(this.chkConvertToHSI_CheckedChanged);
            // 
            // CogColorConversionToolEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CogColorConversionToolEditor";
            ((System.ComponentModel.ISupportInitialize)(this.sbpIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpStatusCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpStatusMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpProcessingTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpTotalTime)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tpgSettings.ResumeLayout(false);
            this.tpgSettings.PerformLayout();
            this.tpgRegion.ResumeLayout(false);
            this.tpgRegion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tpgRegion;
        private System.Windows.Forms.CheckBox chkConvertToHSI;
        private System.Windows.Forms.ComboBox cboRegionShape;
        private System.Windows.Forms.Label lblRegionShape;
    }
}
