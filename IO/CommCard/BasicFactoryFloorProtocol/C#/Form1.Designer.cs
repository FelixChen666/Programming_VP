/*******************************************************************************
 Copyright (C) 2014 Cognex Corporation

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
 * 
 */
 namespace BasicFactoryFloorProtocol
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
      this.btnClearLog = new System.Windows.Forms.Button();
      this.lblLog = new System.Windows.Forms.Label();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.btnRunning = new System.Windows.Forms.Button();
      this.btnStopped = new System.Windows.Forms.Button();
      this.btnAcqStarted = new System.Windows.Forms.Button();
      this.btnUpdateSettings = new System.Windows.Forms.Button();
      this.lbl_HostName = new System.Windows.Forms.Label();
      this.ckb_DHCP = new System.Windows.Forms.CheckBox();
      this.lbl_Subnet = new System.Windows.Forms.Label();
      this.lbl_IPAddress = new System.Windows.Forms.Label();
      this.txt_Subnet = new System.Windows.Forms.TextBox();
      this.txt_IPAddress = new System.Windows.Forms.TextBox();
      this.txt_HostName = new System.Windows.Forms.TextBox();
      this.btnAcqComplete = new System.Windows.Forms.Button();
      this.btnAcqReady = new System.Windows.Forms.Button();
      this.btnAcqDisabled = new System.Windows.Forms.Button();
      this.btnAcqError = new System.Windows.Forms.Button();
      this.btnAcqMovePart = new System.Windows.Forms.Button();
      this.btnError = new System.Windows.Forms.Button();
      this.btnInspectionComplete = new System.Windows.Forms.Button();
      this.btnJobState = new System.Windows.Forms.Button();
      this.btnSystemStatus = new System.Windows.Forms.Button();
      this.btnReadUserData = new System.Windows.Forms.Button();
      this.grpNotify = new System.Windows.Forms.GroupBox();
      this.label3 = new System.Windows.Forms.Label();
      this.numErrorID = new System.Windows.Forms.NumericUpDown();
      this.label2 = new System.Windows.Forms.Label();
      this.numJobID = new System.Windows.Forms.NumericUpDown();
      this.chkBusy = new System.Windows.Forms.CheckBox();
      this.chkReady = new System.Windows.Forms.CheckBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.numAcqID = new System.Windows.Forms.NumericUpDown();
      this.label4 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.numCamIndex = new System.Windows.Forms.NumericUpDown();
      this.radEthernetIP = new System.Windows.Forms.RadioButton();
      this.radProfinet = new System.Windows.Forms.RadioButton();
      this.grpProtocol = new System.Windows.Forms.GroupBox();
      this.btnInitFFP = new System.Windows.Forms.Button();
      this.grpEthernetSettings = new System.Windows.Forms.GroupBox();
      this.grpActiveSettings = new System.Windows.Forms.GroupBox();
      this.txt_ActiveHost = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.txt_ActiveIP = new System.Windows.Forms.TextBox();
      this.txt_ActiveSubnet = new System.Windows.Forms.TextBox();
      this.lblActiveIPAddress = new System.Windows.Forms.Label();
      this.lblActiveSubnet = new System.Windows.Forms.Label();
      this.grpNotify.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numErrorID)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numJobID)).BeginInit();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numAcqID)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numCamIndex)).BeginInit();
      this.grpProtocol.SuspendLayout();
      this.grpEthernetSettings.SuspendLayout();
      this.grpActiveSettings.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnClearLog
      // 
      this.btnClearLog.Location = new System.Drawing.Point(652, 344);
      this.btnClearLog.Name = "btnClearLog";
      this.btnClearLog.Size = new System.Drawing.Size(75, 23);
      this.btnClearLog.TabIndex = 48;
      this.btnClearLog.Text = "Clear Log";
      this.btnClearLog.UseVisualStyleBackColor = true;
      this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
      // 
      // lblLog
      // 
      this.lblLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblLog.AutoSize = true;
      this.lblLog.Location = new System.Drawing.Point(9, 357);
      this.lblLog.Name = "lblLog";
      this.lblLog.Size = new System.Drawing.Size(50, 13);
      this.lblLog.TabIndex = 43;
      this.lblLog.Text = "FFP Log:";
      // 
      // textBox1
      // 
      this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.textBox1.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBox1.Location = new System.Drawing.Point(0, 373);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBox1.Size = new System.Drawing.Size(739, 226);
      this.textBox1.TabIndex = 42;
      // 
      // btnRunning
      // 
      this.btnRunning.Location = new System.Drawing.Point(15, 21);
      this.btnRunning.Name = "btnRunning";
      this.btnRunning.Size = new System.Drawing.Size(96, 23);
      this.btnRunning.TabIndex = 49;
      this.btnRunning.Text = "Running";
      this.btnRunning.UseVisualStyleBackColor = true;
      this.btnRunning.Click += new System.EventHandler(this.btnNotifyRunning_Click);
      // 
      // btnStopped
      // 
      this.btnStopped.Location = new System.Drawing.Point(15, 50);
      this.btnStopped.Name = "btnStopped";
      this.btnStopped.Size = new System.Drawing.Size(96, 23);
      this.btnStopped.TabIndex = 50;
      this.btnStopped.Text = "Stopped";
      this.btnStopped.UseVisualStyleBackColor = true;
      this.btnStopped.Click += new System.EventHandler(this.btnNotifyStopped_Click);
      // 
      // btnAcqStarted
      // 
      this.btnAcqStarted.Location = new System.Drawing.Point(6, 91);
      this.btnAcqStarted.Name = "btnAcqStarted";
      this.btnAcqStarted.Size = new System.Drawing.Size(96, 23);
      this.btnAcqStarted.TabIndex = 51;
      this.btnAcqStarted.Text = "Acq Started";
      this.btnAcqStarted.UseVisualStyleBackColor = true;
      this.btnAcqStarted.Click += new System.EventHandler(this.btnNotifyAcquisitionStarted_Click);
      // 
      // btnUpdateSettings
      // 
      this.btnUpdateSettings.Location = new System.Drawing.Point(115, 117);
      this.btnUpdateSettings.Name = "btnUpdateSettings";
      this.btnUpdateSettings.Size = new System.Drawing.Size(92, 23);
      this.btnUpdateSettings.TabIndex = 52;
      this.btnUpdateSettings.Text = "Apply";
      this.btnUpdateSettings.UseVisualStyleBackColor = true;
      this.btnUpdateSettings.Click += new System.EventHandler(this.btnUpdateSettings_Click);
      // 
      // lbl_HostName
      // 
      this.lbl_HostName.AutoSize = true;
      this.lbl_HostName.Location = new System.Drawing.Point(10, 94);
      this.lbl_HostName.Name = "lbl_HostName";
      this.lbl_HostName.Size = new System.Drawing.Size(58, 13);
      this.lbl_HostName.TabIndex = 62;
      this.lbl_HostName.Text = "Hostname:";
      // 
      // ckb_DHCP
      // 
      this.ckb_DHCP.AutoSize = true;
      this.ckb_DHCP.Location = new System.Drawing.Point(10, 22);
      this.ckb_DHCP.Name = "ckb_DHCP";
      this.ckb_DHCP.Size = new System.Drawing.Size(78, 17);
      this.ckb_DHCP.TabIndex = 59;
      this.ckb_DHCP.Text = "Use DHCP";
      this.ckb_DHCP.UseVisualStyleBackColor = true;
      // 
      // lbl_Subnet
      // 
      this.lbl_Subnet.AutoSize = true;
      this.lbl_Subnet.Location = new System.Drawing.Point(10, 68);
      this.lbl_Subnet.Name = "lbl_Subnet";
      this.lbl_Subnet.Size = new System.Drawing.Size(72, 13);
      this.lbl_Subnet.TabIndex = 58;
      this.lbl_Subnet.Text = "Subnet mask:";
      // 
      // lbl_IPAddress
      // 
      this.lbl_IPAddress.AutoSize = true;
      this.lbl_IPAddress.Location = new System.Drawing.Point(10, 42);
      this.lbl_IPAddress.Name = "lbl_IPAddress";
      this.lbl_IPAddress.Size = new System.Drawing.Size(61, 13);
      this.lbl_IPAddress.TabIndex = 57;
      this.lbl_IPAddress.Text = "IP Address:";
      // 
      // txt_Subnet
      // 
      this.txt_Subnet.Location = new System.Drawing.Point(106, 65);
      this.txt_Subnet.Name = "txt_Subnet";
      this.txt_Subnet.Size = new System.Drawing.Size(99, 20);
      this.txt_Subnet.TabIndex = 56;
      this.txt_Subnet.Text = "255.255.255.0";
      // 
      // txt_IPAddress
      // 
      this.txt_IPAddress.Location = new System.Drawing.Point(106, 40);
      this.txt_IPAddress.Name = "txt_IPAddress";
      this.txt_IPAddress.Size = new System.Drawing.Size(101, 20);
      this.txt_IPAddress.TabIndex = 55;
      this.txt_IPAddress.Text = "192.168.1.20";
      // 
      // txt_HostName
      // 
      this.txt_HostName.Location = new System.Drawing.Point(76, 91);
      this.txt_HostName.Name = "txt_HostName";
      this.txt_HostName.Size = new System.Drawing.Size(131, 20);
      this.txt_HostName.TabIndex = 63;
      this.txt_HostName.Text = "CommunicationCard";
      // 
      // btnAcqComplete
      // 
      this.btnAcqComplete.Location = new System.Drawing.Point(7, 141);
      this.btnAcqComplete.Name = "btnAcqComplete";
      this.btnAcqComplete.Size = new System.Drawing.Size(96, 23);
      this.btnAcqComplete.TabIndex = 66;
      this.btnAcqComplete.Text = "Acq Complete";
      this.btnAcqComplete.UseVisualStyleBackColor = true;
      this.btnAcqComplete.Click += new System.EventHandler(this.btnAcqComplete_Click);
      // 
      // btnAcqReady
      // 
      this.btnAcqReady.Location = new System.Drawing.Point(6, 191);
      this.btnAcqReady.Name = "btnAcqReady";
      this.btnAcqReady.Size = new System.Drawing.Size(96, 23);
      this.btnAcqReady.TabIndex = 67;
      this.btnAcqReady.Text = "Acq Ready";
      this.btnAcqReady.UseVisualStyleBackColor = true;
      this.btnAcqReady.Click += new System.EventHandler(this.btnAcqReady_Click);
      // 
      // btnAcqDisabled
      // 
      this.btnAcqDisabled.Location = new System.Drawing.Point(7, 216);
      this.btnAcqDisabled.Name = "btnAcqDisabled";
      this.btnAcqDisabled.Size = new System.Drawing.Size(96, 23);
      this.btnAcqDisabled.TabIndex = 68;
      this.btnAcqDisabled.Text = "Acq Disabled";
      this.btnAcqDisabled.UseVisualStyleBackColor = true;
      this.btnAcqDisabled.Click += new System.EventHandler(this.btnNotifyAcqDisabled_Click);
      // 
      // btnAcqError
      // 
      this.btnAcqError.Location = new System.Drawing.Point(6, 166);
      this.btnAcqError.Name = "btnAcqError";
      this.btnAcqError.Size = new System.Drawing.Size(96, 23);
      this.btnAcqError.TabIndex = 69;
      this.btnAcqError.Text = "Acq Error";
      this.btnAcqError.UseVisualStyleBackColor = true;
      this.btnAcqError.Click += new System.EventHandler(this.btnNotifyAcqError_Click);
      // 
      // btnAcqMovePart
      // 
      this.btnAcqMovePart.Location = new System.Drawing.Point(7, 116);
      this.btnAcqMovePart.Name = "btnAcqMovePart";
      this.btnAcqMovePart.Size = new System.Drawing.Size(96, 23);
      this.btnAcqMovePart.TabIndex = 70;
      this.btnAcqMovePart.Text = "Acq MovePart";
      this.btnAcqMovePart.UseVisualStyleBackColor = true;
      this.btnAcqMovePart.Click += new System.EventHandler(this.btnNotifyAcqMovePart_Click);
      // 
      // btnError
      // 
      this.btnError.Location = new System.Drawing.Point(276, 28);
      this.btnError.Name = "btnError";
      this.btnError.Size = new System.Drawing.Size(96, 23);
      this.btnError.TabIndex = 72;
      this.btnError.Text = "Error";
      this.btnError.UseVisualStyleBackColor = true;
      this.btnError.Click += new System.EventHandler(this.btnNotifyError_Click);
      // 
      // btnInspectionComplete
      // 
      this.btnInspectionComplete.Location = new System.Drawing.Point(6, 241);
      this.btnInspectionComplete.Name = "btnInspectionComplete";
      this.btnInspectionComplete.Size = new System.Drawing.Size(96, 40);
      this.btnInspectionComplete.TabIndex = 73;
      this.btnInspectionComplete.Text = "Inspection Complete";
      this.btnInspectionComplete.UseVisualStyleBackColor = true;
      this.btnInspectionComplete.Click += new System.EventHandler(this.btnNotifyInspectionComplete_Click);
      // 
      // btnJobState
      // 
      this.btnJobState.Location = new System.Drawing.Point(276, 114);
      this.btnJobState.Name = "btnJobState";
      this.btnJobState.Size = new System.Drawing.Size(96, 23);
      this.btnJobState.TabIndex = 74;
      this.btnJobState.Text = "Job State";
      this.btnJobState.UseVisualStyleBackColor = true;
      this.btnJobState.Click += new System.EventHandler(this.btnNotifyJobState_Click);
      // 
      // btnSystemStatus
      // 
      this.btnSystemStatus.Location = new System.Drawing.Point(276, 189);
      this.btnSystemStatus.Name = "btnSystemStatus";
      this.btnSystemStatus.Size = new System.Drawing.Size(96, 23);
      this.btnSystemStatus.TabIndex = 75;
      this.btnSystemStatus.Text = "System Status";
      this.btnSystemStatus.UseVisualStyleBackColor = true;
      this.btnSystemStatus.Click += new System.EventHandler(this.btnSystemStatus_Click);
      // 
      // btnReadUserData
      // 
      this.btnReadUserData.Location = new System.Drawing.Point(635, 12);
      this.btnReadUserData.Name = "btnReadUserData";
      this.btnReadUserData.Size = new System.Drawing.Size(92, 35);
      this.btnReadUserData.TabIndex = 76;
      this.btnReadUserData.Text = "Read User Data";
      this.btnReadUserData.UseVisualStyleBackColor = true;
      this.btnReadUserData.Click += new System.EventHandler(this.btnReadUserData_Click);
      // 
      // grpNotify
      // 
      this.grpNotify.Controls.Add(this.label3);
      this.grpNotify.Controls.Add(this.numErrorID);
      this.grpNotify.Controls.Add(this.label2);
      this.grpNotify.Controls.Add(this.numJobID);
      this.grpNotify.Controls.Add(this.chkBusy);
      this.grpNotify.Controls.Add(this.chkReady);
      this.grpNotify.Controls.Add(this.groupBox1);
      this.grpNotify.Controls.Add(this.btnRunning);
      this.grpNotify.Controls.Add(this.btnSystemStatus);
      this.grpNotify.Controls.Add(this.btnStopped);
      this.grpNotify.Controls.Add(this.btnJobState);
      this.grpNotify.Controls.Add(this.btnError);
      this.grpNotify.Location = new System.Drawing.Point(235, 3);
      this.grpNotify.Name = "grpNotify";
      this.grpNotify.Size = new System.Drawing.Size(394, 311);
      this.grpNotify.TabIndex = 77;
      this.grpNotify.TabStop = false;
      this.grpNotify.Text = "Notify Functions";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(292, 60);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(43, 13);
      this.label3.TabIndex = 83;
      this.label3.Text = "ErrorID:";
      // 
      // numErrorID
      // 
      this.numErrorID.Location = new System.Drawing.Point(336, 58);
      this.numErrorID.Name = "numErrorID";
      this.numErrorID.Size = new System.Drawing.Size(52, 20);
      this.numErrorID.TabIndex = 82;
      this.numErrorID.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(292, 145);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(38, 13);
      this.label2.TabIndex = 81;
      this.label2.Text = "JobID:";
      // 
      // numJobID
      // 
      this.numJobID.Location = new System.Drawing.Point(336, 143);
      this.numJobID.Name = "numJobID";
      this.numJobID.Size = new System.Drawing.Size(52, 20);
      this.numJobID.TabIndex = 80;
      this.numJobID.Value = new decimal(new int[] {
            22,
            0,
            0,
            0});
      // 
      // chkBusy
      // 
      this.chkBusy.AutoSize = true;
      this.chkBusy.Location = new System.Drawing.Point(314, 246);
      this.chkBusy.Name = "chkBusy";
      this.chkBusy.Size = new System.Drawing.Size(49, 17);
      this.chkBusy.TabIndex = 79;
      this.chkBusy.Text = "Busy";
      this.chkBusy.UseVisualStyleBackColor = true;
      // 
      // chkReady
      // 
      this.chkReady.AutoSize = true;
      this.chkReady.Location = new System.Drawing.Point(314, 223);
      this.chkReady.Name = "chkReady";
      this.chkReady.Size = new System.Drawing.Size(57, 17);
      this.chkReady.TabIndex = 78;
      this.chkReady.Text = "Ready";
      this.chkReady.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.numAcqID);
      this.groupBox1.Controls.Add(this.label4);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.numCamIndex);
      this.groupBox1.Controls.Add(this.btnAcqDisabled);
      this.groupBox1.Controls.Add(this.btnAcqMovePart);
      this.groupBox1.Controls.Add(this.btnAcqError);
      this.groupBox1.Controls.Add(this.btnAcqReady);
      this.groupBox1.Controls.Add(this.btnAcqComplete);
      this.groupBox1.Controls.Add(this.btnInspectionComplete);
      this.groupBox1.Controls.Add(this.btnAcqStarted);
      this.groupBox1.Location = new System.Drawing.Point(117, 18);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(153, 287);
      this.groupBox1.TabIndex = 77;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Acquisition ";
      // 
      // numAcqID
      // 
      this.numAcqID.Location = new System.Drawing.Point(95, 50);
      this.numAcqID.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
      this.numAcqID.Name = "numAcqID";
      this.numAcqID.Size = new System.Drawing.Size(52, 20);
      this.numAcqID.TabIndex = 79;
      this.numAcqID.Value = new decimal(new int[] {
            211,
            0,
            0,
            0});
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(14, 52);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(43, 13);
      this.label4.TabIndex = 78;
      this.label4.Text = "Acq ID:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(14, 26);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(60, 13);
      this.label1.TabIndex = 77;
      this.label1.Text = "Cam Index:";
      // 
      // numCamIndex
      // 
      this.numCamIndex.Location = new System.Drawing.Point(95, 24);
      this.numCamIndex.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
      this.numCamIndex.Name = "numCamIndex";
      this.numCamIndex.Size = new System.Drawing.Size(52, 20);
      this.numCamIndex.TabIndex = 76;
      // 
      // radEthernetIP
      // 
      this.radEthernetIP.AutoSize = true;
      this.radEthernetIP.Checked = true;
      this.radEthernetIP.Location = new System.Drawing.Point(6, 18);
      this.radEthernetIP.Name = "radEthernetIP";
      this.radEthernetIP.Size = new System.Drawing.Size(75, 17);
      this.radEthernetIP.TabIndex = 80;
      this.radEthernetIP.TabStop = true;
      this.radEthernetIP.Text = "EthernetIP";
      this.radEthernetIP.UseVisualStyleBackColor = true;
      // 
      // radProfinet
      // 
      this.radProfinet.AutoSize = true;
      this.radProfinet.Location = new System.Drawing.Point(98, 18);
      this.radProfinet.Name = "radProfinet";
      this.radProfinet.Size = new System.Drawing.Size(61, 17);
      this.radProfinet.TabIndex = 79;
      this.radProfinet.Text = "Profinet";
      this.radProfinet.UseVisualStyleBackColor = true;
      // 
      // grpProtocol
      // 
      this.grpProtocol.Controls.Add(this.btnInitFFP);
      this.grpProtocol.Controls.Add(this.radEthernetIP);
      this.grpProtocol.Controls.Add(this.radProfinet);
      this.grpProtocol.Location = new System.Drawing.Point(12, 283);
      this.grpProtocol.Name = "grpProtocol";
      this.grpProtocol.Size = new System.Drawing.Size(217, 71);
      this.grpProtocol.TabIndex = 81;
      this.grpProtocol.TabStop = false;
      this.grpProtocol.Text = "Protocol";
      // 
      // btnInitFFP
      // 
      this.btnInitFFP.Location = new System.Drawing.Point(98, 41);
      this.btnInitFFP.Name = "btnInitFFP";
      this.btnInitFFP.Size = new System.Drawing.Size(92, 23);
      this.btnInitFFP.TabIndex = 82;
      this.btnInitFFP.Text = "Init FFP";
      this.btnInitFFP.UseVisualStyleBackColor = true;
      this.btnInitFFP.Click += new System.EventHandler(this.btnInitFFP_Click);
      // 
      // grpEthernetSettings
      // 
      this.grpEthernetSettings.Controls.Add(this.btnUpdateSettings);
      this.grpEthernetSettings.Controls.Add(this.txt_IPAddress);
      this.grpEthernetSettings.Controls.Add(this.txt_Subnet);
      this.grpEthernetSettings.Controls.Add(this.lbl_IPAddress);
      this.grpEthernetSettings.Controls.Add(this.lbl_Subnet);
      this.grpEthernetSettings.Controls.Add(this.ckb_DHCP);
      this.grpEthernetSettings.Controls.Add(this.txt_HostName);
      this.grpEthernetSettings.Controls.Add(this.lbl_HostName);
      this.grpEthernetSettings.Location = new System.Drawing.Point(12, 12);
      this.grpEthernetSettings.Name = "grpEthernetSettings";
      this.grpEthernetSettings.Size = new System.Drawing.Size(217, 149);
      this.grpEthernetSettings.TabIndex = 83;
      this.grpEthernetSettings.TabStop = false;
      this.grpEthernetSettings.Text = "Stored Ethernet Settings";
      // 
      // grpActiveSettings
      // 
      this.grpActiveSettings.Controls.Add(this.txt_ActiveHost);
      this.grpActiveSettings.Controls.Add(this.label5);
      this.grpActiveSettings.Controls.Add(this.txt_ActiveIP);
      this.grpActiveSettings.Controls.Add(this.txt_ActiveSubnet);
      this.grpActiveSettings.Controls.Add(this.lblActiveIPAddress);
      this.grpActiveSettings.Controls.Add(this.lblActiveSubnet);
      this.grpActiveSettings.Location = new System.Drawing.Point(12, 167);
      this.grpActiveSettings.Name = "grpActiveSettings";
      this.grpActiveSettings.Size = new System.Drawing.Size(217, 110);
      this.grpActiveSettings.TabIndex = 87;
      this.grpActiveSettings.TabStop = false;
      this.grpActiveSettings.Text = "Active Ethernet Settings (DHCP)";
      // 
      // txt_ActiveHost
      // 
      this.txt_ActiveHost.Location = new System.Drawing.Point(67, 82);
      this.txt_ActiveHost.Name = "txt_ActiveHost";
      this.txt_ActiveHost.ReadOnly = true;
      this.txt_ActiveHost.Size = new System.Drawing.Size(131, 20);
      this.txt_ActiveHost.TabIndex = 65;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(3, 85);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(58, 13);
      this.label5.TabIndex = 64;
      this.label5.Text = "Hostname:";
      // 
      // txt_ActiveIP
      // 
      this.txt_ActiveIP.Location = new System.Drawing.Point(98, 28);
      this.txt_ActiveIP.Name = "txt_ActiveIP";
      this.txt_ActiveIP.ReadOnly = true;
      this.txt_ActiveIP.Size = new System.Drawing.Size(101, 20);
      this.txt_ActiveIP.TabIndex = 59;
      // 
      // txt_ActiveSubnet
      // 
      this.txt_ActiveSubnet.Location = new System.Drawing.Point(98, 53);
      this.txt_ActiveSubnet.Name = "txt_ActiveSubnet";
      this.txt_ActiveSubnet.ReadOnly = true;
      this.txt_ActiveSubnet.Size = new System.Drawing.Size(99, 20);
      this.txt_ActiveSubnet.TabIndex = 60;
      // 
      // lblActiveIPAddress
      // 
      this.lblActiveIPAddress.AutoSize = true;
      this.lblActiveIPAddress.Location = new System.Drawing.Point(2, 30);
      this.lblActiveIPAddress.Name = "lblActiveIPAddress";
      this.lblActiveIPAddress.Size = new System.Drawing.Size(61, 13);
      this.lblActiveIPAddress.TabIndex = 61;
      this.lblActiveIPAddress.Text = "IP Address:";
      // 
      // lblActiveSubnet
      // 
      this.lblActiveSubnet.AutoSize = true;
      this.lblActiveSubnet.Location = new System.Drawing.Point(2, 56);
      this.lblActiveSubnet.Name = "lblActiveSubnet";
      this.lblActiveSubnet.Size = new System.Drawing.Size(72, 13);
      this.lblActiveSubnet.TabIndex = 62;
      this.lblActiveSubnet.Text = "Subnet mask:";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(739, 599);
      this.Controls.Add(this.grpActiveSettings);
      this.Controls.Add(this.grpEthernetSettings);
      this.Controls.Add(this.grpProtocol);
      this.Controls.Add(this.grpNotify);
      this.Controls.Add(this.btnReadUserData);
      this.Controls.Add(this.btnClearLog);
      this.Controls.Add(this.lblLog);
      this.Controls.Add(this.textBox1);
      this.Name = "Form1";
      this.Text = "BasicFactoryFloorProtocol";
      this.grpNotify.ResumeLayout(false);
      this.grpNotify.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numErrorID)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numJobID)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numAcqID)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numCamIndex)).EndInit();
      this.grpProtocol.ResumeLayout(false);
      this.grpProtocol.PerformLayout();
      this.grpEthernetSettings.ResumeLayout(false);
      this.grpEthernetSettings.PerformLayout();
      this.grpActiveSettings.ResumeLayout(false);
      this.grpActiveSettings.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnClearLog;
    private System.Windows.Forms.Label lblLog;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Button btnRunning;
    private System.Windows.Forms.Button btnStopped;
    private System.Windows.Forms.Button btnAcqStarted;
    private System.Windows.Forms.Button btnUpdateSettings;
    private System.Windows.Forms.Label lbl_HostName;
    private System.Windows.Forms.CheckBox ckb_DHCP;
    private System.Windows.Forms.Label lbl_Subnet;
    private System.Windows.Forms.Label lbl_IPAddress;
    private System.Windows.Forms.TextBox txt_Subnet;
    private System.Windows.Forms.TextBox txt_IPAddress;
    private System.Windows.Forms.TextBox txt_HostName;
    private System.Windows.Forms.Button btnAcqComplete;
    private System.Windows.Forms.Button btnAcqReady;
    private System.Windows.Forms.Button btnAcqDisabled;
    private System.Windows.Forms.Button btnAcqError;
    private System.Windows.Forms.Button btnAcqMovePart;
    private System.Windows.Forms.Button btnError;
    private System.Windows.Forms.Button btnInspectionComplete;
    private System.Windows.Forms.Button btnJobState;
    private System.Windows.Forms.Button btnSystemStatus;
    private System.Windows.Forms.Button btnReadUserData;
    private System.Windows.Forms.GroupBox grpNotify;
    private System.Windows.Forms.RadioButton radEthernetIP;
    private System.Windows.Forms.RadioButton radProfinet;
    private System.Windows.Forms.GroupBox grpProtocol;
    private System.Windows.Forms.Button btnInitFFP;
    private System.Windows.Forms.GroupBox grpEthernetSettings;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown numCamIndex;
    private System.Windows.Forms.CheckBox chkBusy;
    private System.Windows.Forms.CheckBox chkReady;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.NumericUpDown numErrorID;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.NumericUpDown numJobID;
    private System.Windows.Forms.NumericUpDown numAcqID;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.GroupBox grpActiveSettings;
    private System.Windows.Forms.TextBox txt_ActiveHost;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txt_ActiveIP;
    private System.Windows.Forms.TextBox txt_ActiveSubnet;
    private System.Windows.Forms.Label lblActiveIPAddress;
    private System.Windows.Forms.Label lblActiveSubnet;
  }
}

