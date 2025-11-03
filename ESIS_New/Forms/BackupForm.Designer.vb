<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class BackupForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.grpDatabaseInfo = New System.Windows.Forms.GroupBox()
        Me.lblLastBackup = New System.Windows.Forms.Label()
        Me.lblDatabaseSize = New System.Windows.Forms.Label()
        Me.lblDatabaseName = New System.Windows.Forms.Label()
        Me.grpBackupSettings = New System.Windows.Forms.GroupBox()
        Me.nudKeepBackupsFor = New System.Windows.Forms.NumericUpDown()
        Me.lblKeepBackupsFor = New System.Windows.Forms.Label()
        Me.chkAutoDeleteOldBackups = New System.Windows.Forms.CheckBox()
        Me.txtBackupDescription = New System.Windows.Forms.TextBox()
        Me.lblBackupDescription = New System.Windows.Forms.Label()
        Me.chkIncludeArchiveFiles = New System.Windows.Forms.CheckBox()
        Me.chkCompressBackup = New System.Windows.Forms.CheckBox()
        Me.txtBackupPath = New System.Windows.Forms.TextBox()
        Me.lblBackupPath = New System.Windows.Forms.Label()
        Me.grpBackupProgress = New System.Windows.Forms.GroupBox()
        Me.lblBackupStatus = New System.Windows.Forms.Label()
        Me.progressBarBackup = New System.Windows.Forms.ProgressBar()
        Me.grpExistingBackups = New System.Windows.Forms.GroupBox()
        Me.lblBackupsCount = New System.Windows.Forms.Label()
        Me.lstExistingBackups = New System.Windows.Forms.ListBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnCancelBackup = New System.Windows.Forms.Button()
        Me.btnStartBackup = New System.Windows.Forms.Button()
        Me.btnDeleteSelectedBackup = New System.Windows.Forms.Button()
        Me.btnBrowseBackupPath = New System.Windows.Forms.Button()
        Me.grpDatabaseInfo.SuspendLayout()
        Me.grpBackupSettings.SuspendLayout()
        CType(Me.nudKeepBackupsFor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpBackupProgress.SuspendLayout()
        Me.grpExistingBackups.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpDatabaseInfo
        '
        Me.grpDatabaseInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDatabaseInfo.Controls.Add(Me.lblLastBackup)
        Me.grpDatabaseInfo.Controls.Add(Me.lblDatabaseSize)
        Me.grpDatabaseInfo.Controls.Add(Me.lblDatabaseName)
        Me.grpDatabaseInfo.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpDatabaseInfo.Location = New System.Drawing.Point(12, 5)
        Me.grpDatabaseInfo.Name = "grpDatabaseInfo"
        Me.grpDatabaseInfo.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.grpDatabaseInfo.Size = New System.Drawing.Size(660, 107)
        Me.grpDatabaseInfo.TabIndex = 0
        Me.grpDatabaseInfo.TabStop = False
        Me.grpDatabaseInfo.Text = "معلومات قاعدة البيانات"
        '
        'lblLastBackup
        '
        Me.lblLastBackup.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastBackup.Location = New System.Drawing.Point(47, 72)
        Me.lblLastBackup.Name = "lblLastBackup"
        Me.lblLastBackup.Size = New System.Drawing.Size(562, 23)
        Me.lblLastBackup.TabIndex = 2
        Me.lblLastBackup.Text = "آخر نسخة احتياطية: --"
        Me.lblLastBackup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDatabaseSize
        '
        Me.lblDatabaseSize.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDatabaseSize.Location = New System.Drawing.Point(47, 47)
        Me.lblDatabaseSize.Name = "lblDatabaseSize"
        Me.lblDatabaseSize.Size = New System.Drawing.Size(562, 23)
        Me.lblDatabaseSize.TabIndex = 1
        Me.lblDatabaseSize.Text = "حجم قاعدة البيانات: --"
        Me.lblDatabaseSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDatabaseName
        '
        Me.lblDatabaseName.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDatabaseName.Location = New System.Drawing.Point(47, 22)
        Me.lblDatabaseName.Name = "lblDatabaseName"
        Me.lblDatabaseName.Size = New System.Drawing.Size(566, 23)
        Me.lblDatabaseName.TabIndex = 0
        Me.lblDatabaseName.Text = "اسم قاعدة البيانات: --"
        Me.lblDatabaseName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpBackupSettings
        '
        Me.grpBackupSettings.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBackupSettings.Controls.Add(Me.nudKeepBackupsFor)
        Me.grpBackupSettings.Controls.Add(Me.lblKeepBackupsFor)
        Me.grpBackupSettings.Controls.Add(Me.chkAutoDeleteOldBackups)
        Me.grpBackupSettings.Controls.Add(Me.txtBackupDescription)
        Me.grpBackupSettings.Controls.Add(Me.lblBackupDescription)
        Me.grpBackupSettings.Controls.Add(Me.chkIncludeArchiveFiles)
        Me.grpBackupSettings.Controls.Add(Me.chkCompressBackup)
        Me.grpBackupSettings.Controls.Add(Me.btnBrowseBackupPath)
        Me.grpBackupSettings.Controls.Add(Me.txtBackupPath)
        Me.grpBackupSettings.Controls.Add(Me.lblBackupPath)
        Me.grpBackupSettings.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpBackupSettings.Location = New System.Drawing.Point(12, 118)
        Me.grpBackupSettings.Name = "grpBackupSettings"
        Me.grpBackupSettings.Size = New System.Drawing.Size(660, 264)
        Me.grpBackupSettings.TabIndex = 1
        Me.grpBackupSettings.TabStop = False
        Me.grpBackupSettings.Text = "إعدادات النسخ الاحتياطي"
        '
        'nudKeepBackupsFor
        '
        Me.nudKeepBackupsFor.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudKeepBackupsFor.Location = New System.Drawing.Point(15, 223)
        Me.nudKeepBackupsFor.Maximum = New Decimal(New Integer() {365, 0, 0, 0})
        Me.nudKeepBackupsFor.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudKeepBackupsFor.Name = "nudKeepBackupsFor"
        Me.nudKeepBackupsFor.Size = New System.Drawing.Size(80, 30)
        Me.nudKeepBackupsFor.TabIndex = 9
        Me.nudKeepBackupsFor.Value = New Decimal(New Integer() {30, 0, 0, 0})
        '
        'lblKeepBackupsFor
        '
        Me.lblKeepBackupsFor.AutoSize = True
        Me.lblKeepBackupsFor.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblKeepBackupsFor.Location = New System.Drawing.Point(101, 225)
        Me.lblKeepBackupsFor.Name = "lblKeepBackupsFor"
        Me.lblKeepBackupsFor.Size = New System.Drawing.Size(28, 23)
        Me.lblKeepBackupsFor.TabIndex = 8
        Me.lblKeepBackupsFor.Text = "يوم"
        '
        'chkAutoDeleteOldBackups
        '
        Me.chkAutoDeleteOldBackups.AutoSize = True
        Me.chkAutoDeleteOldBackups.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoDeleteOldBackups.Location = New System.Drawing.Point(431, 203)
        Me.chkAutoDeleteOldBackups.Name = "chkAutoDeleteOldBackups"
        Me.chkAutoDeleteOldBackups.Size = New System.Drawing.Size(212, 27)
        Me.chkAutoDeleteOldBackups.TabIndex = 7
        Me.chkAutoDeleteOldBackups.Text = "حذف النسخ الاحتياطية القديمة تلقائياً"
        Me.chkAutoDeleteOldBackups.UseVisualStyleBackColor = True
        '
        'txtBackupDescription
        '
        Me.txtBackupDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBackupDescription.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBackupDescription.Location = New System.Drawing.Point(15, 152)
        Me.txtBackupDescription.Multiline = True
        Me.txtBackupDescription.Name = "txtBackupDescription"
        Me.txtBackupDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtBackupDescription.Size = New System.Drawing.Size(630, 45)
        Me.txtBackupDescription.TabIndex = 6
        '
        'lblBackupDescription
        '
        Me.lblBackupDescription.AutoSize = True
        Me.lblBackupDescription.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBackupDescription.Location = New System.Drawing.Point(510, 126)
        Me.lblBackupDescription.Name = "lblBackupDescription"
        Me.lblBackupDescription.Size = New System.Drawing.Size(133, 23)
        Me.lblBackupDescription.TabIndex = 5
        Me.lblBackupDescription.Text = "وصف النسخة الاحتياطية:"
        '
        'chkIncludeArchiveFiles
        '
        Me.chkIncludeArchiveFiles.AutoSize = True
        Me.chkIncludeArchiveFiles.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncludeArchiveFiles.Location = New System.Drawing.Point(501, 89)
        Me.chkIncludeArchiveFiles.Name = "chkIncludeArchiveFiles"
        Me.chkIncludeArchiveFiles.Size = New System.Drawing.Size(142, 27)
        Me.chkIncludeArchiveFiles.TabIndex = 4
        Me.chkIncludeArchiveFiles.Text = "تضمين ملفات الأرشيف"
        Me.chkIncludeArchiveFiles.UseVisualStyleBackColor = True
        '
        'chkCompressBackup
        '
        Me.chkCompressBackup.AutoSize = True
        Me.chkCompressBackup.Checked = True
        Me.chkCompressBackup.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCompressBackup.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCompressBackup.Location = New System.Drawing.Point(15, 89)
        Me.chkCompressBackup.Name = "chkCompressBackup"
        Me.chkCompressBackup.Size = New System.Drawing.Size(147, 27)
        Me.chkCompressBackup.TabIndex = 3
        Me.chkCompressBackup.Text = "ضغط النسخة الاحتياطية"
        Me.chkCompressBackup.UseVisualStyleBackColor = True
        '
        'txtBackupPath
        '
        Me.txtBackupPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBackupPath.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBackupPath.Location = New System.Drawing.Point(49, 45)
        Me.txtBackupPath.Name = "txtBackupPath"
        Me.txtBackupPath.Size = New System.Drawing.Size(594, 30)
        Me.txtBackupPath.TabIndex = 1
        Me.txtBackupPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblBackupPath
        '
        Me.lblBackupPath.AutoSize = True
        Me.lblBackupPath.Font = New System.Drawing.Font("Cairo", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBackupPath.Location = New System.Drawing.Point(489, 19)
        Me.lblBackupPath.Name = "lblBackupPath"
        Me.lblBackupPath.Size = New System.Drawing.Size(156, 23)
        Me.lblBackupPath.TabIndex = 0
        Me.lblBackupPath.Text = "مسار حفظ النسخة الاحتياطية:"
        '
        'grpBackupProgress
        '
        Me.grpBackupProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBackupProgress.Controls.Add(Me.lblBackupStatus)
        Me.grpBackupProgress.Controls.Add(Me.progressBarBackup)
        Me.grpBackupProgress.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpBackupProgress.Location = New System.Drawing.Point(12, 388)
        Me.grpBackupProgress.Name = "grpBackupProgress"
        Me.grpBackupProgress.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.grpBackupProgress.Size = New System.Drawing.Size(660, 80)
        Me.grpBackupProgress.TabIndex = 2
        Me.grpBackupProgress.TabStop = False
        Me.grpBackupProgress.Text = "تقدم النسخ الاحتياطي"
        '
        'lblBackupStatus
        '
        Me.lblBackupStatus.AutoSize = True
        Me.lblBackupStatus.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblBackupStatus.Location = New System.Drawing.Point(15, 50)
        Me.lblBackupStatus.Name = "lblBackupStatus"
        Me.lblBackupStatus.Size = New System.Drawing.Size(51, 23)
        Me.lblBackupStatus.TabIndex = 1
        Me.lblBackupStatus.Text = "جاهز..."
        Me.lblBackupStatus.Visible = False
        '
        'progressBarBackup
        '
        Me.progressBarBackup.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.progressBarBackup.Location = New System.Drawing.Point(15, 25)
        Me.progressBarBackup.Name = "progressBarBackup"
        Me.progressBarBackup.Size = New System.Drawing.Size(630, 20)
        Me.progressBarBackup.TabIndex = 0
        Me.progressBarBackup.Visible = False
        '
        'grpExistingBackups
        '
        Me.grpExistingBackups.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpExistingBackups.Controls.Add(Me.btnDeleteSelectedBackup)
        Me.grpExistingBackups.Controls.Add(Me.lblBackupsCount)
        Me.grpExistingBackups.Controls.Add(Me.lstExistingBackups)
        Me.grpExistingBackups.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpExistingBackups.Location = New System.Drawing.Point(12, 476)
        Me.grpExistingBackups.Name = "grpExistingBackups"
        Me.grpExistingBackups.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.grpExistingBackups.Size = New System.Drawing.Size(660, 180)
        Me.grpExistingBackups.TabIndex = 3
        Me.grpExistingBackups.TabStop = False
        Me.grpExistingBackups.Text = "النسخ الاحتياطية الموجودة"
        '
        'lblBackupsCount
        '
        Me.lblBackupsCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblBackupsCount.AutoSize = True
        Me.lblBackupsCount.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblBackupsCount.Location = New System.Drawing.Point(15, 150)
        Me.lblBackupsCount.Name = "lblBackupsCount"
        Me.lblBackupsCount.Size = New System.Drawing.Size(148, 23)
        Me.lblBackupsCount.TabIndex = 1
        Me.lblBackupsCount.Text = "عدد النسخ الاحتياطية: 0"
        '
        'lstExistingBackups
        '
        Me.lstExistingBackups.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstExistingBackups.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lstExistingBackups.FormattingEnabled = True
        Me.lstExistingBackups.ItemHeight = 23
        Me.lstExistingBackups.Location = New System.Drawing.Point(15, 30)
        Me.lstExistingBackups.Name = "lstExistingBackups"
        Me.lstExistingBackups.Size = New System.Drawing.Size(630, 96)
        Me.lstExistingBackups.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.Location = New System.Drawing.Point(12, 665)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(80, 35)
        Me.btnClose.TabIndex = 6
        Me.btnClose.Text = "إغلاق"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnCancelBackup
        '
        Me.btnCancelBackup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelBackup.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        Me.btnCancelBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelBackup.Location = New System.Drawing.Point(388, 665)
        Me.btnCancelBackup.Name = "btnCancelBackup"
        Me.btnCancelBackup.Size = New System.Drawing.Size(110, 35)
        Me.btnCancelBackup.TabIndex = 5
        Me.btnCancelBackup.Text = "إلغاء العملية"
        Me.btnCancelBackup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelBackup.UseVisualStyleBackColor = True
        '
        'btnStartBackup
        '
        Me.btnStartBackup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStartBackup.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        Me.btnStartBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnStartBackup.Location = New System.Drawing.Point(504, 665)
        Me.btnStartBackup.Name = "btnStartBackup"
        Me.btnStartBackup.Size = New System.Drawing.Size(168, 35)
        Me.btnStartBackup.TabIndex = 4
        Me.btnStartBackup.Text = "بدء النسخ الاحتياطي"
        Me.btnStartBackup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnStartBackup.UseVisualStyleBackColor = True
        '
        'btnDeleteSelectedBackup
        '
        Me.btnDeleteSelectedBackup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteSelectedBackup.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)

        Me.btnDeleteSelectedBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDeleteSelectedBackup.Location = New System.Drawing.Point(531, 144)
        Me.btnDeleteSelectedBackup.Name = "btnDeleteSelectedBackup"
        Me.btnDeleteSelectedBackup.Size = New System.Drawing.Size(114, 31)
        Me.btnDeleteSelectedBackup.TabIndex = 2
        Me.btnDeleteSelectedBackup.Text = "حذف المحدد"
        Me.btnDeleteSelectedBackup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDeleteSelectedBackup.UseVisualStyleBackColor = True
        '
        'btnBrowseBackupPath
        '
        Me.btnBrowseBackupPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseBackupPath.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        Me.btnBrowseBackupPath.Location = New System.Drawing.Point(6, 44)
        Me.btnBrowseBackupPath.Name = "btnBrowseBackupPath"
        Me.btnBrowseBackupPath.Size = New System.Drawing.Size(39, 30)
        Me.btnBrowseBackupPath.TabIndex = 2
        Me.btnBrowseBackupPath.UseVisualStyleBackColor = True
        '
        'BackupForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(684, 712)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnCancelBackup)
        Me.Controls.Add(Me.btnStartBackup)
        Me.Controls.Add(Me.grpExistingBackups)
        Me.Controls.Add(Me.grpBackupProgress)
        Me.Controls.Add(Me.grpBackupSettings)
        Me.Controls.Add(Me.grpDatabaseInfo)
        Me.MaximumSize = New System.Drawing.Size(700, 751)
        Me.MinimumSize = New System.Drawing.Size(700, 751)
        Me.Name = "BackupForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "نسخ احتياطي لقاعدة البيانات - نظام ESIS"
        Me.grpDatabaseInfo.ResumeLayout(False)
        Me.grpBackupSettings.ResumeLayout(False)
        Me.grpBackupSettings.PerformLayout()
        CType(Me.nudKeepBackupsFor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpBackupProgress.ResumeLayout(False)
        Me.grpBackupProgress.PerformLayout()
        Me.grpExistingBackups.ResumeLayout(False)
        Me.grpExistingBackups.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpDatabaseInfo As GroupBox
    Friend WithEvents lblLastBackup As Label
    Friend WithEvents lblDatabaseSize As Label
    Friend WithEvents lblDatabaseName As Label
    Friend WithEvents grpBackupSettings As GroupBox
    Friend WithEvents btnBrowseBackupPath As Button
    Friend WithEvents txtBackupPath As TextBox
    Friend WithEvents lblBackupPath As Label
    Friend WithEvents chkCompressBackup As CheckBox
    Friend WithEvents chkIncludeArchiveFiles As CheckBox
    Friend WithEvents txtBackupDescription As TextBox
    Friend WithEvents lblBackupDescription As Label
    Friend WithEvents chkAutoDeleteOldBackups As CheckBox
    Friend WithEvents nudKeepBackupsFor As NumericUpDown
    Friend WithEvents lblKeepBackupsFor As Label
    Friend WithEvents grpBackupProgress As GroupBox
    Friend WithEvents progressBarBackup As ProgressBar
    Friend WithEvents lblBackupStatus As Label
    Friend WithEvents grpExistingBackups As GroupBox
    Friend WithEvents lstExistingBackups As ListBox
    Friend WithEvents lblBackupsCount As Label
    Friend WithEvents btnDeleteSelectedBackup As Button
    Friend WithEvents btnStartBackup As Button
    Friend WithEvents btnCancelBackup As Button
    Friend WithEvents btnClose As Button
End Class