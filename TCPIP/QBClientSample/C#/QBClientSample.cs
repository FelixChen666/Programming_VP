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
//
// The purpose of this sample is to demonstrate how to listen for the socket 
// connection request and how to collect and display data produced by QuickBuild.
// QuickBuild can be either client or server, and this sample expects
// QuickBuild to be server as the sample creates a client socket.
//
// As soon as the socket connection is made, the sample launches a thread whose 
// responsibility is to start collecting and displaying data.
// 
// .NET provides two classes that assist socket communications--TCPListener and 
// TCPClient. The TcpListener class provides simple methods that listen for and 
// accept incoming connection requests in blocking synchronous mode. The TcpClient 
// class provides simple methods for connecting, and receiving stream data over 
// a network in synchronous blocking mode. This sample program only deals with 
// synchronous blocking mode.
//
// This program launches a thread when the user presses the Listen button, and
// it will try to make a connection to the server from this thread. The user must
// start QuickBuild server before attempting to make a connection to the server. 
// This is important because unless TcpClient can make a successful connection 
// to the server, it will not receive any data.
//
// Note: This program's focus is not showing how to write a multi-threaded 
// application, but simply how to listen for the socket connection request 
// and collect and display QuickBuild data.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace QuickBuildClientSample
{
  public partial class QBClientSampleForm : Form
  {
    // Used for thread safe GUI update
    private delegate void UpdateString(string text);

    private TcpClient _client;

    // Thread that is responsible for identifying client connection requests.
    private Thread _connectionThread;
    private long _totalBytes; // record the total number of bytes received

    // Used to log the received data from server
    private StreamWriter _write;

    // When the server is not running from the same machine,
    // change Dns.GetHostName() to the target server name.
    private string hostname = Dns.GetHostName();

    public QBClientSampleForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

      string vproPath = System.Environment.GetEnvironmentVariable("VPRO_ROOT");
      if (vproPath == null)
      {
        MessageBox.Show("VPRO_ROOT environment variable is not defined.. "
         + "Please fix the problem and run it again.", "QuickBuild Client Sample", 
         MessageBoxButtons.OK);
        ConnectButton.Enabled = false;
        ClearButton.Enabled = false;
        LoggingCheckBox.Enabled = false;
      }
      txtDescription.Text = "Makes a TCP/IP connection to an existing QuickBuild application "
        + "and receives the data it generates."
        + " and wait for the data produced by QuickBuild server. "
        + "First, launch QuickBuild and load QBServerSample.vpp from "
        + vproPath + @"Samples\Programming\TCPIP.  "
        + "Next, click the Online button and the Run Continuous button in the QuickBuild "
        + "button bar. Finally, run this application and click Connect to establish a TCP/IP"
        + " link and start the data exchange.";
      HostNameTextBox.Text = hostname;

      LoggingCheckBox.Checked = false;
    }

    // Display the received data on the textbox
    private void UpdateGUI(string s)
    {
      if (OutputTextBox.InvokeRequired)
        OutputTextBox.BeginInvoke(new UpdateString(UpdateGUI), new object[] { s });
      else
      {
        if (OutputTextBox.TextLength >= OutputTextBox.MaxLength)
          OutputTextBox.Text = "";
        OutputTextBox.AppendText(s);
        _totalBytes += s.Length;
        TotalBytesLabel.Text = _totalBytes.ToString();
      }
    }

    // Display an error message
    private void DisplayError(string message)
    {
      if (this.InvokeRequired)
        this.BeginInvoke(new UpdateString(DisplayError), new object[] { message });
      else
      {
        MessageBox.Show(this, message, "QuickBuild Client Sample");
        StopClient();
      }
    }

    // This function is responsible for reading data from QuickBuild server and 
    // display it on the GUI and/or writing it to a designated file.
    private void ReceiveDataFromServer()
    {
      try
      {
        // Create TcpClient to initiate the connection to the server.
        _client = new TcpClient(hostname, 
          Int32.Parse(PortNumberBox.Value.ToString()));
      }
      catch (SocketException ex)
      {
        DisplayError(ex.Message);
        return;
      }

      NetworkStream netStream = null;

      try
      {
        netStream = _client.GetStream();
      }
      catch (Exception ex)
      {
        // a bad connection, couldn't get the NetworkStream
        DisplayError(ex.Message);
        return;
      }
      // Make sure we can read the data from the network stream
      if (netStream.CanRead)
      {
        try
        {
          // Receive buffer -- should be large enough to reduce overhead.
          byte[] receiveBuffer = new byte[512]; 
          int bytesReceived;                    // Received byte count
          // Read data until server closes the connection.
          while ((bytesReceived = netStream.Read(receiveBuffer, 0, receiveBuffer.Length)) > 0)
          {
            if (_write != null)
              _write.Write(Encoding.ASCII.GetString(receiveBuffer, 0, bytesReceived));

            UpdateGUI(Encoding.ASCII.GetString(receiveBuffer, 0, bytesReceived));
          }
        }
        catch (Exception ex)
        {
          // Ignore if the error is caused during shutdown
          // since the stream and client will be closed
          if (ConnectButton.Text != "Connect")
            DisplayError(ex.Message);
        }
      }

      StopClient();
    }

    // Creates a thread to make a connection to server
    // and start receiving data.
    private void ConnectToServer()
    {
      try
      {
        ConnectButton.Text = "Stop";
        _totalBytes = 0;
        // There is only one connection thread that is used to connect clients.
        _connectionThread = new System.Threading.Thread(new ThreadStart(ReceiveDataFromServer));
        _connectionThread.IsBackground = true;
        _connectionThread.Priority = ThreadPriority.AboveNormal;
        _connectionThread.Name = "Handle Server";
        _connectionThread.Start();
        PortNumberBox.Enabled = false;
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "QuickBuild Client Sample");
      }
    }

    // Called when the user wants to make a socket connection to QuickBuild
    // and start collecting data. The user must specify a filename to which 
    // the incoming data can be written; otherwise, an error message will be displayed.
    private void btnConnect_Click(object sender, System.EventArgs e)
    {
      if (ConnectButton.Text == "Connect")
      {
        ConnectToServer();
      }
      else
      {
        StopClient();
      }
    }

    // Close the connection and stop receiving data
    private void StopClient()
    {
      if (this.InvokeRequired)
      {
        // StopClient can be called after
        // client is closed
        if (ConnectButton.Text != "Connect")
          this.BeginInvoke(new MethodInvoker(StopClient));
        return;
      }
      Cursor.Current = Cursors.WaitCursor;

      // Change to listen mode
      ConnectButton.Text = "Connect";

      if (_client != null)
      {
        // Close the connection
        _client.Close();

        // Wait for the thread to terminate.
        _connectionThread.Join();
      }

      PortNumberBox.Enabled = true;

      Cursor.Current = Cursors.Default;
    }

    // Clear the contents of the output textbox and resets the byte counter.
    private void ClearButton_Click(object sender, System.EventArgs e)
    {
      OutputTextBox.Clear();
      TotalBytesLabel.Text = "0";
    }

    // The application is about to terminate
    private void QBClientSampleForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      StopClient();

      // Close the open file stream
      CloseFile();
    }

    #region Data Logging
    // Close the data logging file
    private void CloseFile()
    {
      if (_write != null)
      {
        _write.Flush();
        _write.Close();
        _write = null;
      }
    }

    // Create a data logging file. Warns the user if the file
    // already exists.
    private void CreateFile(bool checkFileExist)
    {
      // First, close the open file stream
      CloseFile();

      // Create a text file for saving data
      FileInfo file = new FileInfo(FileNameTextBox.Text);

      if (checkFileExist && file.Exists)
      {
        if (MessageBox.Show(this, FileNameTextBox.Text +
          " already exists. Would you like to overwrite it?",
          "QuickBuild sample", MessageBoxButtons.YesNo) == DialogResult.No)
        {
          return;
        }
      }

      // Create StreamWriter for writing
      _write = file.CreateText();
    }

    private void LoggingCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      FileNameLabel.Enabled = LoggingCheckBox.Checked;
      FileNameTextBox.Enabled = LoggingCheckBox.Checked;
      FileSaveButton.Enabled = LoggingCheckBox.Checked;

      // If data logging is enabled, then create a file
      if (LoggingCheckBox.Checked)
        CreateFile(true);
    }

    private void FileSaveButton_Click(object sender, EventArgs e)
    {
      // Open a file for logging incoming data.
      saveFileDialog1.Filter = "Log File (*.txt)|*.txt;";
      saveFileDialog1.AddExtension = true;
      saveFileDialog1.Title = "QuickBuild TCP/IP Client Sample - File Save";
      saveFileDialog1.FileName = FileNameTextBox.Text;
      if (saveFileDialog1.ShowDialog() == DialogResult.OK)
      {
        try
        {
          FileNameTextBox.Text = saveFileDialog1.FileName;
          CreateFile(false);
        }
        catch (Exception ex)
        {
          MessageBox.Show(this, ex.Message, "QuickBuild Client Sample");
        }
      }
    }

    private void FileNameTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        if (!FileNameTextBox.Text.EndsWith(".txt"))
        {
          FileNameTextBox.Text += ".txt";
        }
        CreateFile(true);
      }
    }
    #endregion
  }
}