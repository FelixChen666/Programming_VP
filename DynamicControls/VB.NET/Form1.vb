'*******************************************************************************
' Copyright (C) 2005 Cognex Corporation
'
' Subject to Cognex Corporation's terms and conditions and license agreement,
' you are authorized to use and modify this source code in any way you find
' useful, provided the Software and/or the modified Software is used solely in
' conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
' and agree that Cognex has no warranty, obligations or liability for your use
' of the Software.
'*******************************************************************************
' This sample program is designed to illustrate certain VisionPro features or 
' techniques in the simplest way possible. It is not intended as the framework 
' for a complete application. In particular, the sample program may not provide
' proper error handling, event handling, cleanup, repeatability, and other 
' mechanisms that a commercial quality application requires.

' In the Windows operating system, a limited number of GDI and USER handles can be
' claimed by each application. Adding a tool edit control to an application
' consumes both GDI and USER handles. These handles may run out if you add
' too many tool edit controls to an application. However, if you create
' a tool edit control only when you need it, you can control this problem.
' This sample demonstrates how to create a Cognex Tool Edit control and add it
' to a TabControl which has four initial tabs. Each tool edit control will be placed
' into a predefined tab position. The CogBlobEdit control will be placed onto
' the first tab, the CogPMAlignEdit control will be placed onto the second tab,
' the CogCaliperEdit control will be placed onto the third tab and lastly
' the CogCNLSearchEdit control will be placed onto the fourth tab.
'
' When the tool edit control is created dynamically, it ignores the AutoCreateTool
' property value. Thus, the user must create a tool and assign it to the tool
' edit control. 
'
' This program assumes that you have good knowledge of VB.NET and VisionPro
' programming.
'
' 

Imports Cognex.VisionPro
Imports Cognex.VisionPro.PMAlign
Imports Cognex.VisionPro.Blob
Imports Cognex.VisionPro.Caliper
Imports Cognex.VisionPro.CNLSearch

Public Class Form1
    Inherits System.Windows.Forms.Form


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents BlobCheck As System.Windows.Forms.CheckBox
    Friend WithEvents PMAlignCheck As System.Windows.Forms.CheckBox
    Friend WithEvents CaliperCheck As System.Windows.Forms.CheckBox
    Friend WithEvents CnlSearchCheck As System.Windows.Forms.CheckBox
    Friend WithEvents BlobTabPage As System.Windows.Forms.TabPage
    Friend WithEvents PMAlignTabPage As System.Windows.Forms.TabPage
    Friend WithEvents CaliperTabPage As System.Windows.Forms.TabPage
    Friend WithEvents CNLSearchTabPage As System.Windows.Forms.TabPage
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.BlobCheck = New System.Windows.Forms.CheckBox
        Me.PMAlignCheck = New System.Windows.Forms.CheckBox
        Me.CaliperCheck = New System.Windows.Forms.CheckBox
        Me.CnlSearchCheck = New System.Windows.Forms.CheckBox
        Me.BlobTabPage = New System.Windows.Forms.TabPage
        Me.PMAlignTabPage = New System.Windows.Forms.TabPage
        Me.CaliperTabPage = New System.Windows.Forms.TabPage
        Me.CNLSearchTabPage = New System.Windows.Forms.TabPage
        Me.TabControl1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.BlobTabPage)
        Me.TabControl1.Controls.Add(Me.PMAlignTabPage)
        Me.TabControl1.Controls.Add(Me.CaliperTabPage)
        Me.TabControl1.Controls.Add(Me.CNLSearchTabPage)
        Me.TabControl1.Location = New System.Drawing.Point(16, 16)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(828, 500)
        Me.TabControl1.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CnlSearchCheck)
        Me.GroupBox1.Controls.Add(Me.CaliperCheck)
        Me.GroupBox1.Controls.Add(Me.PMAlignCheck)
        Me.GroupBox1.Controls.Add(Me.BlobCheck)
        Me.GroupBox1.Location = New System.Drawing.Point(860, 80)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(160, 232)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Controls"
        '
        'BlobCheck
        '
        Me.BlobCheck.Location = New System.Drawing.Point(24, 32)
        Me.BlobCheck.Name = "BlobCheck"
        Me.BlobCheck.Size = New System.Drawing.Size(112, 24)
        Me.BlobCheck.TabIndex = 0
        Me.BlobCheck.Text = "Blob Control"
        '
        'PMAlignCheck
        '
        Me.PMAlignCheck.Location = New System.Drawing.Point(24, 56)
        Me.PMAlignCheck.Name = "PMAlignCheck"
        Me.PMAlignCheck.Size = New System.Drawing.Size(112, 24)
        Me.PMAlignCheck.TabIndex = 1
        Me.PMAlignCheck.Text = "PMAlign Control"
        '
        'CaliperCheck
        '
        Me.CaliperCheck.Location = New System.Drawing.Point(24, 80)
        Me.CaliperCheck.Name = "CaliperCheck"
        Me.CaliperCheck.TabIndex = 2
        Me.CaliperCheck.Text = "Caliper Control"
        '
        'CnlSearchCheck
        '
        Me.CnlSearchCheck.Location = New System.Drawing.Point(24, 104)
        Me.CnlSearchCheck.Name = "CnlSearchCheck"
        Me.CnlSearchCheck.Size = New System.Drawing.Size(104, 40)
        Me.CnlSearchCheck.TabIndex = 3
        Me.CnlSearchCheck.Text = "CNLSearch Control"
        '
        'BlobTabPage
        '
        Me.BlobTabPage.Location = New System.Drawing.Point(4, 22)
        Me.BlobTabPage.Name = "BlobTabPage"
        Me.BlobTabPage.Size = New System.Drawing.Size(820, 474)
        Me.BlobTabPage.TabIndex = 0
        Me.BlobTabPage.Text = "Blob"
        '
        'PMAlignTabPage
        '
        Me.PMAlignTabPage.Location = New System.Drawing.Point(4, 22)
        Me.PMAlignTabPage.Name = "PMAlignTabPage"
        Me.PMAlignTabPage.Size = New System.Drawing.Size(656, 438)
        Me.PMAlignTabPage.TabIndex = 1
        Me.PMAlignTabPage.Text = "PMAlign"
        '
        'CaliperTabPage
        '
        Me.CaliperTabPage.Location = New System.Drawing.Point(4, 22)
        Me.CaliperTabPage.Name = "CaliperTabPage"
        Me.CaliperTabPage.Size = New System.Drawing.Size(656, 438)
        Me.CaliperTabPage.TabIndex = 2
        Me.CaliperTabPage.Text = "Caliper"
        '
        'CNLSearchTabPage
        '
        Me.CNLSearchTabPage.Location = New System.Drawing.Point(4, 22)
        Me.CNLSearchTabPage.Name = "CNLSearchTabPage"
        Me.CNLSearchTabPage.Size = New System.Drawing.Size(656, 438)
        Me.CNLSearchTabPage.TabIndex = 3
        Me.CNLSearchTabPage.Text = "CNLSearch"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(1092, 526)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "Form1"
        Me.Text = "Dynamic Controls Sample"
        Me.TabControl1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    ' Tool and edit control variables
    Private mPMAlignTool As CogPMAlignTool
    Private mPMAlignEdit As CogPMAlignEditV2
    Private mBlobTool As CogBlobTool
    Private mBlobEdit As CogBlobEditV2
    Private mCaliperTool As CogCaliperTool
    Private mCaliperEdit As CogCaliperEditV2
    Private mCnlSearchTool As CogCNLSearchTool
    Private mCnlSearchEdit As CogCNLSearchEditV2

    ' Each of the following "CheckedChanged" subroutines executes the same steps. Because of this, only
    ' the BlobCheck_Changed subroutine is documented.

#Region "BlobCheck"
    Private Sub BlobCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlobCheck.CheckedChanged
        If BlobCheck.Checked Then ' Add the edit control to the tab
            ' Change the cursor to the hourglass
            Me.Cursor = Cursors.WaitCursor
            ' Build a blob tool if necessary
            If mBlobTool Is Nothing Then mBlobTool = New CogBlobTool
            ' Create a new blob edit control
            mBlobEdit = New CogBlobEditV2
            ' Begin initializing the blob edit control
            CType(mBlobEdit, System.ComponentModel.ISupportInitialize).BeginInit()
            ' Make sure it is enabled
            mBlobEdit.Enabled = True
            ' Set it's location to the upper left corner of the tab
            mBlobEdit.Location = New System.Drawing.Point(0, 0)
            ' Assign it a name.
            mBlobEdit.Name = "CogBlobEdit1"
            ' Define the blob edit control size.
            mBlobEdit.Size = New System.Drawing.Size(800, 450)
            mBlobEdit.TabIndex = 0
            ' Set the tab to display to tab 0, which is the BlobTabPage
            Me.TabControl1.SelectedIndex = 0
            'Add the blob edit control to the BlobTabPage
            Me.BlobTabPage.Controls.Add(mBlobEdit)
            ' All done initializing
            CType(mBlobEdit, System.ComponentModel.ISupportInitialize).EndInit()
            ' Assign a tool to the edit control
            mBlobEdit.Subject = mBlobTool
            ' Change the cursor back to the default pointer
            Me.Cursor = Cursors.Default
        Else  'Remove the edit control from the tab
            ' Change the cursor to the hourglass
            Me.Cursor = Cursors.WaitCursor
            ' Remove the blob edit control from the BlobTabPage
            Me.BlobTabPage.Controls.Remove(mBlobEdit)
            ' Dispose of the BlobEditControl. This gets rid of its resources 
            '(GDI objects and user handles)
            mBlobEdit.Dispose()
            ' Change the cursor back to the default pointer
            Me.Cursor = Cursors.Default
        End If
    End Sub
#End Region

#Region "PMAlignCheck"
    Private Sub PMAlignCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PMAlignCheck.CheckedChanged
        If PMAlignCheck.Checked Then
            Me.Cursor = Cursors.WaitCursor
            If mPMAlignTool Is Nothing Then mPMAlignTool = New CogPMAlignTool
            mPMAlignEdit = New CogPMAlignEditV2
            CType(mPMAlignEdit, System.ComponentModel.ISupportInitialize).BeginInit()
            mPMAlignEdit.Enabled = True
            mPMAlignEdit.Location = New System.Drawing.Point(0, 0)
            mPMAlignEdit.Name = "CogPMAlignEdit1"
            mPMAlignEdit.Size = New System.Drawing.Size(800, 450)
            mPMAlignEdit.TabIndex = 0
            Me.TabControl1.SelectedIndex = 1
            Me.PMAlignTabPage.Controls.Add(mPMAlignEdit)
            CType(mPMAlignEdit, System.ComponentModel.ISupportInitialize).EndInit()
            mPMAlignEdit.Subject = mPMAlignTool
            Me.Cursor = Cursors.Default
        Else
            Me.Cursor = Cursors.WaitCursor
            Me.PMAlignTabPage.Controls.Remove(mPMAlignEdit)
            mPMAlignEdit.Dispose()
            Me.Cursor = Cursors.Default
        End If

    End Sub
#End Region

#Region "CaliperCheck"
    Private Sub CaliperCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CaliperCheck.CheckedChanged
        If CaliperCheck.Checked Then
            Me.Cursor = Cursors.WaitCursor
            If mCaliperTool Is Nothing Then mCaliperTool = New CogCaliperTool
            mCaliperEdit = New CogCaliperEditV2
            CType(mCaliperEdit, System.ComponentModel.ISupportInitialize).BeginInit()
            mCaliperEdit.Enabled = True
            mCaliperEdit.Location = New System.Drawing.Point(0, 0)
            mCaliperEdit.Name = "CogCaliperEdit1"
            mCaliperEdit.Size = New System.Drawing.Size(800, 450)
            mCaliperEdit.TabIndex = 0
            Me.TabControl1.SelectedIndex = 2
            Me.CaliperTabPage.Controls.Add(mCaliperEdit)
            CType(mCaliperEdit, System.ComponentModel.ISupportInitialize).EndInit()
            mCaliperEdit.Subject = mCaliperTool
            Me.Cursor = Cursors.Default
        Else
            Me.Cursor = Cursors.WaitCursor
            Me.CaliperTabPage.Controls.Remove(mCaliperEdit)
            mCaliperEdit.Dispose()
            Me.Cursor = Cursors.Default
        End If

    End Sub
#End Region

#Region "CnlSearchCheck"
    Private Sub CnlSearchCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CnlSearchCheck.CheckedChanged
        If CnlSearchCheck.Checked Then
            Me.Cursor = Cursors.WaitCursor
            If mCnlSearchTool Is Nothing Then mCnlSearchTool = New CogCNLSearchTool
            mCnlSearchEdit = New CogCNLSearchEditV2
            CType(mCnlSearchEdit, System.ComponentModel.ISupportInitialize).BeginInit()
            mCnlSearchEdit.Enabled = True
            mCnlSearchEdit.Location = New System.Drawing.Point(0, 0)
            mCnlSearchEdit.Name = "CogCNLSearchEdit1"
            mCnlSearchEdit.Size = New System.Drawing.Size(800, 450)
            mCnlSearchEdit.TabIndex = 0
            Me.TabControl1.SelectedIndex = 3
            Me.CNLSearchTabPage.Controls.Add(mCnlSearchEdit)
            CType(mCnlSearchEdit, System.ComponentModel.ISupportInitialize).EndInit()
            mCnlSearchEdit.Subject = mCnlSearchTool
            Me.Cursor = Cursors.Default
        Else
            Me.Cursor = Cursors.WaitCursor
            Me.CNLSearchTabPage.Controls.Remove(mCnlSearchEdit)
            mCnlSearchEdit.Dispose()
            Me.Cursor = Cursors.Default
        End If

    End Sub
#End Region

 End Class
