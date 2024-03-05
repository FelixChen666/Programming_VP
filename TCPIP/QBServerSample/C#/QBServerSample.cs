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
// QuickBuild to be client as the sample creates a server socket.
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
// This program launches a thread when the user presses the Listen button. This 
// thread's responsibility is to look for all the clients that wish to communicate 
// with this sample application. When the user decides to stop listening, 
// the sample closes all client connections, and terminate the thread.
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


namespace QBServerSample
{
	/// <summary>
	/// After creating a QuickBuild TCP/IP client device, run this program
  /// to monitor the QuickBuild TCP/IP client device activities. This 
  /// program receives data from the QuickBuild client device and either
  /// displays it on the screen and/or saves to a file.
	/// </summary>
  public partial class QuickBuildServerSampleForm : Form
  {
    // Used for thread safe GUI update
    private delegate void UpdateString(string text);

    // _lock is used to synchronize multiple clients from trying to
    // to talk to the sample application. 
    private object _lock = new object();

    // _lock is used to collect NetworkStream objects. 
    private List<NetworkStream> _streams = new List<NetworkStream>();

    private List<TcpClient> _clients = new List<TcpClient>();
    private TcpListener _listener;

    // Thread that is responsible for identifying client connection requests.
    private Thread _connectionThread;
    private List<Thread> _threads = new List<Thread>();

    private long _totalBytes; // record the total number of bytes received

    // Synchronize multiple threads from writing to the same file stream.
    private object _filelock = new object();
    private StreamWriter _write;

    public QuickBuildServerSampleForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
      string vproPath = System.Environment.GetEnvironmentVariable("VPRO_ROOT");
      if (vproPath == null)
      {
        MessageBox.Show("VPRO_ROOT environment variable is not defined. "
         + "Run it again once the problem is fixed.", "QuickBuild Server Sample",
         MessageBoxButtons.OK);
        ListenButton.Enabled = false;
        ClearButton.Enabled = false;
        LoggingCheckBox.Enabled = false;
      }
      DescriptionTextBox.Text = "Makes a TCP/IP connection to an existing QuickBuild application "
        + "and receives the data it generates."
        + " Once this application is running, press the Listen button. "
        + "Next, launch QuickBuild and load QBClientSample.vpp from "
        + vproPath + @"Samples\Programming\TCPIP.  "
        + "Click the Online button to establish a TCP/IP link and then click the Run Continuous "
        + "button to start the data exchange.";
      HostNameTextBox.Text = Dns.GetHostName() + " (Monitors all network interfaces)";

      LoggingCheckBox.Checked = false;
    }

    // Display the received string on the textbox
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
        MessageBox.Show(this, message, "QuickBuild Server Sample");
      }
    }

    // This function is responsible for reading data from a client socket and 
    // display it on the output textbox and/or writing it to a designated file
    public void ReceiveDataFromClient(object clientObject)
    {
      TcpClient client = clientObject as TcpClient;
      NetworkStream netStream = null;

      try
      {
        netStream = client.GetStream();
      }
      catch (Exception ex)
      {
        // a bad connection, couldn't get the NetworkStream
        if (netStream != null) netStream.Close();
        DisplayError(ex.Message);
        return;
      }
      // Make sure we can read the data from the network stream
      if (netStream.CanRead)
      {
        _streams.Add(netStream);
        try
        {
          // Receive buffer -- should be large enough to reduce overhead.
          byte[] receiveBuffer = new byte[512]; 
          int bytesReceived; // Received byte count
          // Read data until client closes the connection.
          while ((bytesReceived = netStream.Read(receiveBuffer, 0, receiveBuffer.Length)) > 0)
          {
            // We need to synchronize multiple threads from writing to the same file stream.
            if (_write != null)
            {
              lock (_filelock)
              {
                _write.Write(Encoding.ASCII.GetString(receiveBuffer, 0, bytesReceived));
              }
            }
            UpdateGUI(Encoding.ASCII.GetString(receiveBuffer, 0, bytesReceived));
          }
        }
        catch(Exception ex)
        {
          // Ignore if the error is caused during shutdown
          // since the stream and client will be closed
          if (ListenButton.Text != "Listen")
            DisplayError(ex.Message);
        }
      }
    }

    // Create a file to save data and TcpListener to accept QuickBuild 
    // connection requests 
    private void ConnectToClient()
    {
      try
      {
        // Create TCPListener to start listening
        _listener = new TcpListener(IPAddress.Any, 
          Int32.Parse(PortNumberBox.Value.ToString()));

        // Initiates the underlying socket, binds it to the local endpoint, 
        // and begins listening for incoming connection attempts.
        _listener.Start();
      }
      catch (SocketException se)
      {
        DisplayError(se.ErrorCode.ToString() + ": " + se.Message);
        StopServer();
        return;
      }

      // Listens for client connection requests
      try
      {
        for (; ; )
        {
          // Wait for a client connection request
          TcpClient client = _listener.AcceptTcpClient(); 
          // Create a thread to start accepting client's data.
          Thread t = new Thread(new ParameterizedThreadStart(ReceiveDataFromClient));
          t.IsBackground = true;
          t.Priority = ThreadPriority.AboveNormal;
          t.Name = "Handle Client";
          t.Start(client);

          _threads.Add(t);
          _clients.Add(client);
        }
      }
      catch (SocketException ex)
      {
        // Display an error message unless we intentionally close server.
        if (ex.ErrorCode != (int)SocketError.Interrupted)
          DisplayError("Lost connection due to the following error: " + ex.Message);
      }
      catch (Exception ex)
      {
        DisplayError("Lost connection due to the following error: " + ex.Message);
      }
    }

    // Creates a thread to start listening for the client's connection
    // request
    private void ConnectToServer()
    {
      try
      {
        ListenButton.Text = "Stop";
        _totalBytes = 0;
        // There is only one connection thread that is used to connect clients.
        _connectionThread = new System.Threading.Thread(new ThreadStart(ConnectToClient));
        _connectionThread.IsBackground = true;
        _connectionThread.Start();
        PortNumberBox.Enabled = false;
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "QuickBuild Server Sample sample");
      }
    }

    // Called when the user wants to make a socket connection to QuickBuild
    // and start collecting data. The user must specify a filename to which 
    // the incoming data can be written; otherwise, an error message will be
    // displayed.
    private void ListenButton_Click(object sender, System.EventArgs e)
    {
      if (ListenButton.Text == "Listen")
      {
        ConnectToServer();
      }
      else
      {
        StopServer();
      }
    }

    // Close the server and all client connections.
    // Wait for all threads to terminate
    private void StopServer()
    {
      Cursor.Current = Cursors.WaitCursor;
      
      // Change to listen mode
      ListenButton.Text = "Listen";

      if (_listener != null)
      {
        // First, close TCPListener
        _listener.Stop();

        // Wait for the server thread to terminate.
        _connectionThread.Join();

        // close all client streams
        foreach (NetworkStream s in _streams)
          s.Close();
        _streams.Clear();

        // close the client connection
        foreach (TcpClient client in _clients)
          client.Close();
        _clients.Clear();

        // wait for all client threads to terminate
        foreach (Thread t in _threads)
          t.Join();
        _threads.Clear();
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
    private void QuickBuildServerSampleForm_FormClosing(object sender, 
                                                  FormClosingEventArgs e)
    {
      StopServer();

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
      saveFileDialog1.Title = "QuickBuild TCP/IP Sample Program - File Save";
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
          MessageBox.Show(this, ex.Message, "QuickBuild sample");
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
