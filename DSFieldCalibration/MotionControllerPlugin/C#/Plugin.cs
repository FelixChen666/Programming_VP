//******************************************************************************
// Copyright (C) 2015 Cognex Corporation
//
// Subject to Cognex Corporation's terms and conditions and license agreement,
// you are authorized to use and modify this source code in any way you find
// useful, provided the Software and/or the modified Software is used solely in
// conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
// and agree that Cognex has no warranty, obligations or liability for your use
// of the Software.
//******************************************************************************

using System; // IDisposable
using Cognex.FieldCalibrationApplication.Plugin;  // IMotionControllerPlugin

/// Uncomment the following code for Zaber Linear Stage
/// Download the Zaber Core Serial Library in C# from http://www.zaber.com/wiki/Software
//using Zaber.Serial.Core;

namespace MotionControllerPlugin
{
  /// <summary>
  /// Sample motion controller plug-in.
  /// </summary>
  public class Plugin : IMotionControllerPlugin
  {
    bool mReversible = true;
    bool mDisposed = false;
    bool mInitialized = false;

    /// Uncomment the following code for Zaber Linear Stage
    //ZaberAsciiDevice mController = null;
    //String mPortString = "COM1";
    //int mAxis = 1;
    //double mStartPosition = 10000;
    //double mStopPosition = 140000;
    //int mMoveToStartVelocity = 40000;
    //int mMoveToStopVelocity = 2500;

    /// <summary>
    /// Construct plug-in
    /// </summary>
    public Plugin()
    {
      // Initialize member variables
    }

    /// <summary>
    /// C# destructor for finalization.
    /// This destructor will run only if the Dispose method does not get 
    /// called. Gives plug-in an opportunity to finalizes.
    /// </summary>
    ~Plugin()
    {
      this.Dispose(false);
    }

    /// <summary>
    /// Disposes unmanaged and managed resources.
    /// If called by finalizer, only unmanaged resources will be disposed.
    /// </summary>
    /// <param name="disposing">Whether the method has been called by user code</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!mDisposed)
      {
        if (disposing)
        {
          // Free managed resources.
        }
        // Free unmanaged resources. Close files and disconnect Ethernet or serial connections.

        /// Uncomment the following code for Zaber Linear Stage
        //if (mController != null)
        //  mController.Port.Close();

        mDisposed = true;
      }
    }

    /// <summary>
    /// Implement IDisposable.
    /// Do not make this method virtual.
    /// A derived class should not be able to override this method.
    /// Must be safe to call multiple times.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Returns whether the motion system is ready for use.
    /// </summary>
    /// <remarks>
    /// It should be false after instantiating the plug-in object from assembly.
    /// It should become and remain true after successfully calling <see cref="Initialize"/>. This signals the application that the plug-in is useable.
    /// </remarks>
    public bool Initialized
    {
      get { return mInitialized; }
    }

    /// <summary>
    /// Returns whether the motion system is capable of returning to the start position.
    /// </summary>
    /// <remarks>
    /// Applicable for conveyor systems that can only move in one direction.
    /// </remarks>
    public bool Reversible
    {
      // If unable to support MoveToStartPosition(), return false
      get { return mReversible; }
    }

    /// <summary>
    /// Returns the current motion status.
    /// </summary>
    /// <exception cref="MotionControllerPluginNotInitializedException">Thrown if the plug-in has not been successfully initialized.</exception>
    /// <exception cref="MotionControllerPluginOperationException">Thrown if unable to determine status.</exception>
    public MotionStatusConstants MotionStatus
    {
      get
      {
        if (mDisposed) throw new ObjectDisposedException("SamplePlugin");
        if (!mInitialized) throw new MotionControllerPluginNotInitializedException();
        // Check whether the motion system is stationary or moving

        /// Uncomment the following code for Zaber Linear Stage
        //try
        //{
        //  if (mController.GetAxis(mAxis).IsBusy())
        //    return MotionStatusConstants.Moving;
        //  else
        //    return MotionStatusConstants.Idle;
        //}
        //catch
        //{
        //  throw new MotionControllerPluginOperationException();
        //}
        throw new MotionControllerPluginOperationException();
        
        return MotionStatusConstants.Idle;
      }
    }

    /// <summary>
    /// Returns whether the motion system is currently at the start position.
    /// </summary>
    /// <remarks>
    /// The application will only check this property if <see cref="MotionStatus"/> is idle.
    /// For unidirectional motion system, always return true.
    /// </remarks>
    /// <exception cref="MotionControllerPluginNotInitializedException">Thrown if the plug-in has not been successfully initialized.</exception>
    /// <exception cref="MotionControllerPluginOperationException">Thrown if unable to determine position.</exception>
    public bool StartPosition
    {
      get
      {
        if (mDisposed) throw new ObjectDisposedException("SamplePlugin");
        if (!mInitialized) throw new MotionControllerPluginNotInitializedException();
        // Check whether the motion system is at the start position
        // If there are any issues, return false

        /// Uncomment the following code for Zaber Linear Stage
        //try
        //{
        //  if (mController.GetAxis(mAxis).GetPosition() == mStartPosition)
        //    return true;
        //  else
        //    return false;
        //}
        //catch
        //{
        //  throw new MotionControllerPluginOperationException();
        //}
        throw new MotionControllerPluginOperationException();

        return true;
      }
    }

    /// <summary>
    /// Returns whether the motion system is currently at the stop position.
    /// </summary>
    /// <remarks>
    /// The application will only check this property if <see cref="MotionStatus"/> is idle.
    /// For unidirectional motion system, always return true.
    /// </remarks>
    /// <exception cref="MotionControllerPluginNotInitializedException">Thrown if the plug-in has not been successfully initialized.</exception>
    /// <exception cref="MotionControllerPluginOperationException">Thrown if unable to determine position.</exception>
    public bool StopPosition
    {
      get
      {
        if (mDisposed) throw new ObjectDisposedException("SamplePlugin");
        if (!mInitialized) throw new MotionControllerPluginNotInitializedException();
        // Check whether the motion system is at the stop position
        // If there are any issues, return false

        /// Uncomment the following code for Zaber Linear Stage
        //try
        //{
        //  if (mController.GetAxis(mAxis).GetPosition() == mStopPosition)
        //    return true;
        //  else
        //    return false;
        //}
        //catch
        //{
        //  throw new MotionControllerPluginOperationException();
        //}
        throw new MotionControllerPluginOperationException();

        return true;
      }
    }

    /// <summary>
    /// Initialize the plug-in for use with the motion system.
    /// </summary>
    /// <remarks>
    /// It must be successfully called before the plug-in can be used.
    /// It should be a blocking function that only returns after executing the checklist that readies a motion system.
    /// This can include checking/loading dependencies, testing serial/Ethernet connections, and calling the motion system self-calibration process. 
    /// </remarks>
    /// <returns>True it was successfully initialized and is ready for use.</returns>
    public bool Initialize()
    {
      if (mDisposed) throw new ObjectDisposedException("SamplePlugin");
      // Setup or test connection to the motion system
      // Calibrate motion system
      // If there are any issues, return false

      /// Uncomment the following code for Zaber Linear Stage
      //try
      //{
      //  ZaberBinaryPort bPort = new ZaberBinaryPort(mPortString);
      //  try
      //  {
      //    bPort.Open();
      //    bPort.ReadTimeout = 1000;
      //    ZaberBinaryDevice bDevice = new ZaberBinaryDevice(bPort, 1);

      //    // Change *all* devices (device number 0) to ASCII, baud 115200.
      //    var changeProtocolCommand = new BinaryCommand(0, 124, 115200);
      //    bPort.Write(changeProtocolCommand);
      //    var bReply = bPort.Read();
      //    //Settle system so it can switch to ASCII mode
      //    System.Threading.Thread.Sleep(500);
      //    bPort.Close();
      //  }
      //  catch
      //  {
      //    //Close the port if there are any communication errors.  This can happen if the slide is already in ASCII mode or is running at a different Baud Rate than the default 9600 for Binary mode
      //    bPort.Close();
      //  }

      //  ZaberAsciiPort aPort = new ZaberAsciiPort(mPortString);
      //  aPort.Open();
      //  aPort.ReadTimeout = 1000;
      //  mController = new ZaberAsciiDevice(aPort, 1);

      //  AsciiReply reply;
      //  var maxStopCommand = new AsciiCommand(mController.GetAxis(mAxis).ParentAddress, mAxis, "get limit.max");
      //  mController.Port.Write(maxStopCommand.ToString());
      //  reply = mController.Port.Read();
      //  if (mStopPosition > Int32.Parse(reply.ResponseData)) throw new ArgumentOutOfRangeException("stopPosition");

      //  var minStartCommand = new AsciiCommand(mController.GetAxis(mAxis).ParentAddress, mAxis, "get limit.min");
      //  mController.Port.Write(minStartCommand.ToString());
      //  reply = mController.Port.Read();
      //  if (mStartPosition < Int32.Parse(reply.ResponseData)) throw new ArgumentOutOfRangeException("stopPosition");

      //  mController.GetAxis(mAxis).Home();
      //}
      //catch
      //{
      //  return false;
      //}

      mInitialized = true;
      return true;
    }

    /// <summary>
    /// Command the motion system to move to the start position.
    /// </summary>
    /// <remarks>
    /// The application will only call this function if <see cref="Reversible"/> is true.
    /// Can be a blocking call that waits until the move is finished or a non-blocking call that returns as soon as the command has been acknowledged.
    /// </remarks>
    /// <exception cref="MotionControllerPluginNotInitializedException">Thrown if the plug-in has not been successfully initialized.</exception>
    /// <exception cref="MotionControllerPluginNotSupportedException">Thrown if the plug-in is not reversible.</exception>
    /// <exception cref="MotionControllerPluginOperationException">Thrown if unable to move.</exception>
    public void MoveToStartPosition()
    {
      if (mDisposed) throw new ObjectDisposedException("SamplePlugin");
      if (!mInitialized) throw new MotionControllerPluginNotInitializedException();
      if (!mReversible) throw new MotionControllerPluginNotSupportedException();
      // Send command the motion system to the start position
      // Do not block function call unless necessary
      // If there are any issues, thrown an exception
      throw new MotionControllerPluginOperationException();
      /// Uncomment the following code for Zaber Linear Stage
      //this.MoveToPosition(mStartPosition, mMoveToStartVelocity);
    }

    /// <summary>
    /// Command the motion system to move to the stop position.
    /// </summary>
    /// <remarks>
    /// Can be a blocking call that waits until the move is finished or a non-blocking call that returns as soon as the command has been acknowledged.
    /// </remarks>
    /// <exception cref="MotionControllerPluginNotInitializedException">Thrown if the plug-in has not been successfully initialized.</exception>
    /// <exception cref="MotionControllerPluginOperationException">Thrown if unable to move.</exception>
    public void MoveToStopPosition()
    {
      if (mDisposed) throw new ObjectDisposedException("SamplePlugin");
      if (!mInitialized) throw new MotionControllerPluginNotInitializedException();
      // Send command the motion system to the stop position
      // Do not block function call unless necessary
      // If there are any issues, thrown an exception
      throw new MotionControllerPluginOperationException();
      /// Uncomment the following code for Zaber Linear Stage
      //this.MoveToPosition(mStopPosition, mMoveToStopVelocity);
    }

    /// Uncomment the following code for Zaber Linear Stage
    //public void MoveToPosition(double position, int velocity)
    //{
    //  if (mDisposed) throw new ObjectDisposedException("ZaberPlugin");
    //  if (!mInitialized) throw new MotionControllerPluginNotInitializedException();
    //  // Send command the motion system to the specified position at the specified velocity
    //  // Do not block function call unless necessary
    //  // If there are any issues, return false
    //  try
    //  {
    //    String moveVelocityString = "set maxspeed ";
    //    moveVelocityString += velocity.ToString();

    //    AsciiReply reply;
    //    var moveVelocityCommand = new AsciiCommand(mController.GetAxis(mAxis).ParentAddress, mAxis, moveVelocityString);
    //    mController.Port.Write(moveVelocityCommand.ToString());
    //    reply = mController.Port.Read();

    //    String movePositionString = "move abs ";
    //    movePositionString += position.ToString();
    //    var moveXCommand = new AsciiCommand(mController.GetAxis(mAxis).ParentAddress, mAxis, movePositionString);
    //    mController.Port.Write(moveXCommand.ToString());
    //    mController.Port.Read();
    //  }
    //  catch
    //  {
    //    throw new MotionControllerPluginOperationException();
    //  }
    //}
  }
}
