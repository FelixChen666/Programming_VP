//*******************************************************************************
// Copyright (C) 2007 Cognex Corporation
//
// Subject to Cognex Corporation's terms and conditions and license agreement,
// you are authorized to use and modify this source code in any way you find
// useful, provided the Software and/or the modified Software is used solely in
// conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
// and agree that Cognex has no warranty, obligations or liability for your use
// of the Software.
//*******************************************************************************

using System.Threading;
using Cognex.VisionPro;
using Cognex.VisionPro.QuickBuild;

namespace QBInputSample
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
      if (QBdevice != null)
        QBdevice.MessageReceived -= new CogIOStreamMessageEventHandler(QBdevice_MessageReceived);
      cogJobManagerEdit1.Subject.Shutdown();
      Thread.Sleep(1000);
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
      this.cogJobManagerEdit1 = new Cognex.VisionPro.QuickBuild.CogJobManagerEdit();
      this.btnSend = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // cogJobManagerEdit1
      // 
      this.cogJobManagerEdit1.Location = new System.Drawing.Point(-2, 2);
      this.cogJobManagerEdit1.Name = "cogJobManagerEdit1";
      this.cogJobManagerEdit1.Size = new System.Drawing.Size(558, 351);
      this.cogJobManagerEdit1.Subject = null;
      this.cogJobManagerEdit1.TabIndex = 0;
      // 
      // btnSend
      // 
      this.btnSend.Location = new System.Drawing.Point(138, 395);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new System.Drawing.Size(201, 42);
      this.btnSend.TabIndex = 3;
      this.btnSend.Text = "Send RunOnce Command";
      this.btnSend.UseVisualStyleBackColor = true;
      this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(565, 476);
      this.Controls.Add(this.btnSend);
      this.Controls.Add(this.cogJobManagerEdit1);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private Cognex.VisionPro.QuickBuild.CogJobManagerEdit cogJobManagerEdit1;
    private System.Windows.Forms.Button btnSend;
  }
}

