'*******************************************************************************
'Copyright (C) 2008 Cognex Corporation

'Subject to Cognex Corporations terms and conditions and license agreement,
'you are authorized to use and modify this source code in any way you find
'useful, provided the Software and/or the modified Software is used solely in
'conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
'and agree that Cognex has no warranty, obligations or liability for your use
'of the Software.
'*******************************************************************************/
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

Public Class Form1
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler CogImageFileEditV21.Subject.Ran, New EventHandler(AddressOf ImageFile_Ran)

    End Sub

#Region " Image file tool 'Ran' event handler"
    Private Sub ImageFile_Ran(ByVal sender As Object, ByVal e As EventArgs)
        Try
            SimpleToolEditV21.Subject.InputImage = _
                CType(CogImageFileEditV21.Subject.OutputImage, Cognex.VisionPro.CogImage8Grey)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
#End Region

End Class
