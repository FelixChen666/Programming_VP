'*******************************************************************************
' Copyright (C) 2010 Cognex Corporation

' Subject to Cognex Corporations terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
'*******************************************************************************/
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

 
 
Option Explicit On 

Public Class CImageRoot
    ' This interface is used by the CogImage8Root object. When the
    ' last reference to the CogImage8Root object is released, i.e. when
    ' it is no longer being used by any image, it calls the Dispose function
    ' on this interface. This class can then free the memory or take
    ' whatever action is appropriate.
    Implements IDisposable
    Private m_PixelMemory As IntPtr ' Address of allocated pixel memory

    ' These are Win32 functions for allocating and freeing memory.
    ' In an actual application the memory may have already been allocated
    ' by other software, in which case the Allocate function below can be
    ' modified to accept an existing memory buffer.
    Private Declare Auto Function GetProcessHeap Lib "kernel32" () As IntPtr
    Private Declare Auto Function HeapAlloc Lib "kernel32" _
     (ByVal hHeap As IntPtr, ByVal dwFlags As IntPtr, ByVal dwBytes As IntPtr) As IntPtr
    Private Declare Auto Function HeapFree Lib "kernel32" _
     (ByVal hHeap As IntPtr, ByVal dwFlags As IntPtr, ByVal lpMem As IntPtr) As IntPtr

    Public Function CogImage8RootFromMemory(ByVal Width As Integer, ByVal Height As Integer) As CogImage8Root
        ' If memory is already allocated, it's an error. You need to create
        ' a new instance of this class each time you want a new block of memory.
        If m_PixelMemory <> 0 Then
            Throw New Exception("Memory is already allocated")
        End If

        ' Allocate a block of memory from a heap.
        m_PixelMemory = HeapAlloc(GetProcessHeap, 0, Width * Height)

        ' Create a root buffer to pass to CogImage8Grey.SetRoot
        ' Buffer will hold raw 8-bit pixel data of an image.
        Dim Buffer As ICogImage8RootBuffer
        Buffer = New CogImage8Root

        ' Intialize the buffer, giving it the image dimensions and a reference
        ' back to this object so it can call Dispose when it's done with
        ' the pixel memory.
        Buffer.Initialize(Width, Height, m_PixelMemory, Width, Me)

        ' Return the buffer to the caller. Note that it's important NOT to store
        ' a reference to the Buffer in this class, because the Buffer already
        ' has a reference to this class's IDisposable interface. If a reference
        ' to the Buffer were stored in this class it would create a circular
        ' reference and the two objects would never get freed up.
        Return Buffer

    End Function
  Public Overridable Sub Dispose() Implements IDisposable.Dispose
    If m_PixelMemory <> 0 Then
      HeapFree(GetProcessHeap, 0, m_PixelMemory)
      m_PixelMemory = 0
    End If

  End Sub
End Class
