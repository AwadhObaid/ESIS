

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PermissionsForm
    Inherits BaseFormWithPermissions

    'Form overrides dispose to clean up the list of components.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PermissionsForm))
        Me.PanelMain = New System.Windows.Forms.Panel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.PanelLeft = New System.Windows.Forms.Panel()
        Me.TreeViewUsers = New System.Windows.Forms.TreeView()
        Me.ImageListIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.PanelRight = New System.Windows.Forms.Panel()
        Me.GroupBoxPermissions = New System.Windows.Forms.GroupBox()
        Me.TreeViewPermissions = New System.Windows.Forms.TreeView()
        Me.PanelButtons = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.PanelTitle = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ContextMenuStripUser = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuExpandAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCollapseAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSelectAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDeselectAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimerRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.PanelMain.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.PanelLeft.SuspendLayout()
        Me.PanelRight.SuspendLayout()
        Me.GroupBoxPermissions.SuspendLayout()
        Me.PanelButtons.SuspendLayout()
        Me.PanelTitle.SuspendLayout()
        Me.ContextMenuStripUser.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelMain
        '
        Me.PanelMain.Controls.Add(Me.SplitContainer1)
        Me.PanelMain.Controls.Add(Me.PanelTitle)
        Me.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelMain.Location = New System.Drawing.Point(0, 0)
        Me.PanelMain.Name = "PanelMain"
        Me.PanelMain.Size = New System.Drawing.Size(1000, 700)
        Me.PanelMain.TabIndex = 0
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 50)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.PanelLeft)
        Me.SplitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.PanelRight)
        Me.SplitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.SplitContainer1.Size = New System.Drawing.Size(1000, 650)
        Me.SplitContainer1.SplitterDistance = 300
        Me.SplitContainer1.TabIndex = 2
        '
        'PanelLeft
        '
        Me.PanelLeft.Controls.Add(Me.TreeViewUsers)
        Me.PanelLeft.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelLeft.Location = New System.Drawing.Point(0, 0)
        Me.PanelLeft.Name = "PanelLeft"
        Me.PanelLeft.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelLeft.Size = New System.Drawing.Size(300, 650)
        Me.PanelLeft.TabIndex = 0
        '
        'TreeViewUsers
        '
        Me.TreeViewUsers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeViewUsers.Font = New System.Drawing.Font("Cairo SemiBold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TreeViewUsers.HideSelection = False
        Me.TreeViewUsers.ImageIndex = 0
        Me.TreeViewUsers.ImageList = Me.ImageListIcons
        Me.TreeViewUsers.Location = New System.Drawing.Point(10, 10)
        Me.TreeViewUsers.Name = "TreeViewUsers"
        Me.TreeViewUsers.RightToLeftLayout = True
        Me.TreeViewUsers.SelectedImageIndex = 0
        Me.TreeViewUsers.ShowNodeToolTips = True
        Me.TreeViewUsers.Size = New System.Drawing.Size(280, 630)
        Me.TreeViewUsers.TabIndex = 0
        '
        'ImageListIcons
        '
        Me.ImageListIcons.ImageStream = CType(resources.GetObject("ImageListIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListIcons.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListIcons.Images.SetKeyName(0, "User.png")
        Me.ImageListIcons.Images.SetKeyName(1, "admin.png")
        Me.ImageListIcons.Images.SetKeyName(2, "folder.png")
        Me.ImageListIcons.Images.SetKeyName(3, "form.png")
        '
        'PanelRight
        '
        Me.PanelRight.Controls.Add(Me.GroupBoxPermissions)
        Me.PanelRight.Controls.Add(Me.PanelButtons)
        Me.PanelRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelRight.Location = New System.Drawing.Point(0, 0)
        Me.PanelRight.Name = "PanelRight"
        Me.PanelRight.Size = New System.Drawing.Size(696, 650)
        Me.PanelRight.TabIndex = 1
        '
        'GroupBoxPermissions
        '
        Me.GroupBoxPermissions.Controls.Add(Me.TreeViewPermissions)
        Me.GroupBoxPermissions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxPermissions.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxPermissions.Location = New System.Drawing.Point(0, 0)
        Me.GroupBoxPermissions.Name = "GroupBoxPermissions"
        Me.GroupBoxPermissions.Padding = New System.Windows.Forms.Padding(10)
        Me.GroupBoxPermissions.Size = New System.Drawing.Size(696, 550)
        Me.GroupBoxPermissions.TabIndex = 0
        Me.GroupBoxPermissions.TabStop = False
        Me.GroupBoxPermissions.Text = "صلاحيات النماذج"
        '
        'TreeViewPermissions
        '
        Me.TreeViewPermissions.CheckBoxes = True
        Me.TreeViewPermissions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeViewPermissions.Font = New System.Drawing.Font("Cairo SemiBold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TreeViewPermissions.HideSelection = False
        Me.TreeViewPermissions.ImageIndex = 0
        Me.TreeViewPermissions.ImageList = Me.ImageListIcons
        Me.TreeViewPermissions.Location = New System.Drawing.Point(10, 28)
        Me.TreeViewPermissions.Name = "TreeViewPermissions"
        Me.TreeViewPermissions.RightToLeftLayout = True
        Me.TreeViewPermissions.SelectedImageIndex = 0
        Me.TreeViewPermissions.ShowNodeToolTips = True
        Me.TreeViewPermissions.Size = New System.Drawing.Size(676, 512)
        Me.TreeViewPermissions.TabIndex = 0
        '
        'PanelButtons
        '
        Me.PanelButtons.Controls.Add(Me.btnClose)
        Me.PanelButtons.Controls.Add(Me.btnRefresh)
        Me.PanelButtons.Controls.Add(Me.btnSave)
        Me.PanelButtons.Controls.Add(Me.btnCancel)
        Me.PanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelButtons.Location = New System.Drawing.Point(0, 550)
        Me.PanelButtons.Name = "PanelButtons"
        Me.PanelButtons.Size = New System.Drawing.Size(696, 100)
        Me.PanelButtons.TabIndex = 1
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.Red
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.White
        Me.btnClose.Location = New System.Drawing.Point(65, 30)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(120, 40)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "إغلاق"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnRefresh.FlatAppearance.BorderSize = 0
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRefresh.Location = New System.Drawing.Point(509, 30)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(120, 40)
        Me.btnRefresh.TabIndex = 2
        Me.btnRefresh.Text = "   تحديث"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnSave.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSave.FlatAppearance.BorderSize = 0
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.White
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(361, 30)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(120, 40)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "   حفظ"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnCancel.BackColor = System.Drawing.Color.Lime
        Me.btnCancel.FlatAppearance.BorderSize = 0
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Cairo", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(213, 30)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(120, 40)
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.Text = "   إلغاء"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'PanelTitle
        '
        Me.PanelTitle.BackColor = System.Drawing.Color.SteelBlue
        Me.PanelTitle.Controls.Add(Me.lblTitle)
        Me.PanelTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelTitle.Location = New System.Drawing.Point(0, 0)
        Me.PanelTitle.Name = "PanelTitle"
        Me.PanelTitle.Size = New System.Drawing.Size(1000, 50)
        Me.PanelTitle.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(434, 13)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(133, 25)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "إدارة الصلاحيات"
        '
        'ContextMenuStripUser
        '
        Me.ContextMenuStripUser.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripUser.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExpandAll, Me.mnuCollapseAll, Me.ToolStripSeparator1, Me.mnuSelectAll, Me.mnuDeselectAll})
        Me.ContextMenuStripUser.Name = "ContextMenuStripUser"
        Me.ContextMenuStripUser.Size = New System.Drawing.Size(134, 98)
        '
        'mnuExpandAll
        '
        Me.mnuExpandAll.Name = "mnuExpandAll"
        Me.mnuExpandAll.Size = New System.Drawing.Size(133, 22)
        Me.mnuExpandAll.Text = "توسيع الكل"
        '
        'mnuCollapseAll
        '
        Me.mnuCollapseAll.Name = "mnuCollapseAll"
        Me.mnuCollapseAll.Size = New System.Drawing.Size(133, 22)
        Me.mnuCollapseAll.Text = "طي الكل"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(130, 6)
        '
        'mnuSelectAll
        '
        Me.mnuSelectAll.Name = "mnuSelectAll"
        Me.mnuSelectAll.Size = New System.Drawing.Size(133, 22)
        Me.mnuSelectAll.Text = "تحديد الكل"
        '
        'mnuDeselectAll
        '
        Me.mnuDeselectAll.Name = "mnuDeselectAll"
        Me.mnuDeselectAll.Size = New System.Drawing.Size(133, 22)
        Me.mnuDeselectAll.Text = "إلغاء التحديد"
        '
        'TimerRefresh
        '
        Me.TimerRefresh.Interval = 30000
        '
        'PermissionsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1000, 700)
        Me.Controls.Add(Me.PanelMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "PermissionsForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PermissionsForm"
        Me.PanelMain.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.PanelLeft.ResumeLayout(False)
        Me.PanelRight.ResumeLayout(False)
        Me.GroupBoxPermissions.ResumeLayout(False)
        Me.PanelButtons.ResumeLayout(False)
        Me.PanelTitle.ResumeLayout(False)
        Me.PanelTitle.PerformLayout()
        Me.ContextMenuStripUser.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelMain As Panel
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents PanelLeft As Panel
    Friend WithEvents TreeViewUsers As TreeView
    Friend WithEvents PanelRight As Panel
    Friend WithEvents GroupBoxPermissions As GroupBox
    Friend WithEvents TreeViewPermissions As TreeView
    Friend WithEvents PanelButtons As Panel
    Friend WithEvents btnRefresh As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents PanelTitle As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents ImageListIcons As ImageList
    Friend WithEvents ContextMenuStripUser As ContextMenuStrip
    Friend WithEvents mnuExpandAll As ToolStripMenuItem
    Friend WithEvents mnuCollapseAll As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents mnuSelectAll As ToolStripMenuItem
    Friend WithEvents mnuDeselectAll As ToolStripMenuItem
    Friend WithEvents TimerRefresh As Timer
End Class