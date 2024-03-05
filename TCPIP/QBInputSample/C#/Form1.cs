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

// This sample demonstrates the following
// - How to hook the TCPIP Input Event Handler
// - How to Send a command
// - How to Process the command

// The sample used a saved application with a QB server device defined 
// that we can setup the input event handler against

// The sample loads the application, enable IO, hook the event
// When you click on the Send RunOnce button, the application will run a single time.
// The event handler will receive the message, decode it and run job manager.

// For more sophisticated commands you will need to do more thread safe coding
// such as queue the commands and then process these commands when it is safe to do so.
// Another way is to make sure you always stop QB before you process the commands and then restarted.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Cognex.VisionPro;
using Cognex.VisionPro.QuickBuild;
using System.Net;
using System.Threading;
using System.Net.Sockets;


namespace QBInputSample
{
  public partial class Form1 : Form
  {
    ICogIOTCPIP QBdevice = null;
    public Form1()
    {
      InitializeComponent();
      cogJobManagerEdit1.Subject = CogSerializer.LoadObjectFromFile(Environment.GetEnvironmentVariable("VPRO_ROOT") + @"\samples\programming\tcpip\QBInputSample.vpp") as CogJobManager;
      cogJobManagerEdit1.Subject.IOEnable = true;
      QBdevice = cogJobManagerEdit1.Subject.StreamInput("localhost", 5001, false);
      if (QBdevice == null)
      {
        MessageBox.Show("Failed to locate the localhost server device.", "QBInputSample");
        this.Close();
        return;
      }
      QBdevice.MessageReceived += new CogIOStreamMessageEventHandler(QBdevice_MessageReceived);

    }
    void QBdevice_MessageReceived(object sender, CogIOStreamMessageEventArgs eventArgs)
    {
      String msg = eventArgs.DecodedMessage;
      if (msg == "RunOnce")
        cogJobManagerEdit1.Subject.Run();
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      TcpClient myTcpClient = new System.Net.Sockets.TcpClient();
      myTcpClient.Connect(Dns.GetHostName(), 5001);
      if (myTcpClient != null)
      {
        NetworkStream myStream = myTcpClient.GetStream();
        if (myStream.CanWrite)
        {
          byte[] bytes = new byte[100];
          bytes = Encoding.ASCII.GetBytes("RunOnce");
          myStream.Write(bytes, 0, bytes.Length);
        }
      }
      myTcpClient.Close();

    }
  }
}