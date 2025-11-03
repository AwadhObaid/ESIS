<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class BackupManagerForm
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
        Me.components = New System.ComponentModel.Container()
        Me.grpDatabaseInfo = New System.Windows.Forms.GroupBox()
        Me.lblLastBackup = New System.Windows.Forms.Label()
        Me.lblDatabaseSize = New System.Windows.Forms.Label()
        Me.lblDatabaseName = New System.Windows.Forms.Label()
        Me.grpBackupPath = New System.Windows.Forms.GroupBox()
        Me.btnBrowsePath = New System.Windows.Forms.Button()
        Me.txtBackupPath = New System.Windows.Forms.TextBox()
        Me.lblBackupPath = New System.Windows.Forms.Label()
        Me.grpBackupsList = New System.Windows.Forms.GroupBox()
        Me.lstBackups = New System.Windows.Forms.ListView()
        Me.colFileName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colFileSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colCreationDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colDescription = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.grpStatistics = New System.Windows.Forms.GroupBox()
        Me.lblAvailableSpace = New System.Windows.Forms.Label()
        Me.lblTotalSize = New System.Windows.Forms.Label()
        Me.lblBackupCount = New System.Windows.Forms.Label()
        Me.grpSelection = New System.Windows.Forms.GroupBox()
        Me.lblSelectedBackup = New System.Windows.Forms.Label()
        Me.grpActions = New System.Windows.Forms.GroupBox()
        Me.btnCleanOldBackups = New System.Windows.Forms.Button()
        Me.btnExportBackup = New System.Windows.Forms.Button()
        Me.btnViewBackupInfo = New System.Windows.Forms.Button()
        Me.btnDeleteBackup = New System.Windows.Forms.Button()
        Me.btnRestoreBackup = New System.Windows.Forms.Button()
        Me.btnCreateBackup = New System.Windows.Forms.Button()
        Me.timerRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.grpDatabaseInfo.SuspendLayout()
        Me.grpBackupPath.SuspendLayout()
        Me.grpBackupsList.SuspendLayout()
        Me.grpStatistics.SuspendLayout()
        Me.grpSelection.SuspendLayout()
        Me.grpActions.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpDatabaseInfo
        '
        Me.grpDatabaseInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDatabaseInfo.Controls.Add(Me.lblLastBackup)
        Me.grpDatabaseInfo.Controls.Add(Me.lblDatabaseSize)
        Me.grpDatabaseInfo.Controls.Add(Me.lblDatabaseName)
        Me.grpDatabaseInfo.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.grpDatabaseInfo.Location = New System.Drawing.Point(12, 12)
        Me.grpDatabaseInfo.Name = "grpDatabaseInfo"
        Me.grpDatabaseInfo.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.grpDatabaseInfo.Size = New System.Drawing.Size(1121, 117)
        Me.grpDatabaseInfo.TabIndex = 0
        Me.grpDatabaseInfo.TabStop = False
        Me.grpDatabaseInfo.Text = "معلومات قاعدة البيانات"
        '
        'lblLastBackup
        '
        Me.lblLastBackup.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblLastBackup.Location = New System.Drawing.Point(661, 64)
        Me.lblLastBackup.Name = "lblLastBackup"
        Me.lblLastBackup.Size = New System.Drawing.Size(329, 23)
        Me.lblLastBackup.TabIndex = 2
        Me.lblLastBackup.Text = "آخر نسخة احتياطية: --"
        Me.lblLastBackup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDatabaseSize
        '
        Me.lblDatabaseSize.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblDatabaseSize.Location = New System.Drawing.Point(131, 29)
        Me.lblDatabaseSize.Name = "lblDatabaseSize"
        Me.lblDatabaseSize.Size = New System.Drawing.Size(215, 42)
        Me.lblDatabaseSize.TabIndex = 1
        Me.lblDatabaseSize.Text = "الحجم: --"
        Me.lblDatabaseSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDatabaseName
        '
        Me.lblDatabaseName.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblDatabaseName.Location = New System.Drawing.Point(661, 29)
        Me.lblDatabaseName.Name = "lblDatabaseName"
        Me.lblDatabaseName.Size = New System.Drawing.Size(329, 23)
        Me.lblDatabaseName.TabIndex = 0
        Me.lblDatabaseName.Text = "قاعدة البيانات: --"
        Me.lblDatabaseName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpBackupPath
        '
        Me.grpBackupPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBackupPath.Controls.Add(Me.btnBrowsePath)
        Me.grpBackupPath.Controls.Add(Me.txtBackupPath)
        Me.grpBackupPath.Controls.Add(Me.lblBackupPath)
        Me.grpBackupPath.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.grpBackupPath.Location = New System.Drawing.Point(12, 149)
        Me.grpBackupPath.Name = "grpBackupPath"
        Me.grpBackupPath.Size = New System.Drawing.Size(833, 89)
        Me.grpBackupPath.TabIndex = 1
        Me.grpBackupPath.TabStop = False
        Me.grpBackupPath.Text = "مسار النسخ الاحتياطية"
        '
        'btnBrowsePath
        '
        Me.btnBrowsePath.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowsePath.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)

        Me.btnBrowsePath.Location = New System.Drawing.Point(12, 28)
        Me.btnBrowsePath.Name = "btnBrowsePath"
        Me.btnBrowsePath.Size = New System.Drawing.Size(35, 30)
        Me.btnBrowsePath.TabIndex = 2
        Me.btnBrowsePath.UseVisualStyleBackColor = True
        '
        'txtBackupPath
        '
        Me.txtBackupPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBackupPath.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.txtBackupPath.Location = New System.Drawing.Point(53, 29)
        Me.txtBackupPath.Name = "txtBackupPath"
        Me.txtBackupPath.Size = New System.Drawing.Size(618, 30)
        Me.txtBackupPath.TabIndex = 1
        Me.txtBackupPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblBackupPath
        '
        Me.lblBackupPath.AutoSize = True
        Me.lblBackupPath.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblBackupPath.Location = New System.Drawing.Point(677, 32)
        Me.lblBackupPath.Name = "lblBackupPath"
        Me.lblBackupPath.Size = New System.Drawing.Size(143, 23)
        Me.lblBackupPath.TabIndex = 0
        Me.lblBackupPath.Text = "مسار النسخ الاحتياطية:"
        Me.lblBackupPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpBackupsList
        '
        Me.grpBackupsList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBackupsList.Controls.Add(Me.lstBackups)
        Me.grpBackupsList.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.grpBackupsList.Location = New System.Drawing.Point(12, 244)
        Me.grpBackupsList.Name = "grpBackupsList"
        Me.grpBackupsList.Size = New System.Drawing.Size(833, 444)
        Me.grpBackupsList.TabIndex = 2
        Me.grpBackupsList.TabStop = False
        Me.grpBackupsList.Text = "قائمة النسخ الاحتياطية"
        '
        'lstBackups
        '
        Me.lstBackups.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstBackups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colFileName, Me.colFileSize, Me.colCreationDate, Me.colDescription, Me.colType})
        Me.lstBackups.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lstBackups.FullRowSelect = True
        Me.lstBackups.GridLines = True
        Me.lstBackups.HideSelection = False
        Me.lstBackups.Location = New System.Drawing.Point(15, 25)
        Me.lstBackups.Name = "lstBackups"
        Me.lstBackups.Size = New System.Drawing.Size(803, 404)
        Me.lstBackups.TabIndex = 0
        Me.lstBackups.UseCompatibleStateImageBehavior = False
        Me.lstBackups.View = System.Windows.Forms.View.Details
        '
        'colFileName
        '
        Me.colFileName.Text = "اسم الملف"
        Me.colFileName.Width = 250
        '
        'colFileSize
        '
        Me.colFileSize.Text = "الحجم"
        Me.colFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colFileSize.Width = 100
        '
        'colCreationDate
        '
        Me.colCreationDate.Text = "تاريخ الإنشاء"
        Me.colCreationDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colCreationDate.Width = 150
        '
        'colDescription
        '
        Me.colDescription.Text = "الوصف"
        Me.colDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colDescription.Width = 200
        '
        'colType
        '
        Me.colType.Text = "النوع"
        Me.colType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colType.Width = 80
        '
        'grpStatistics
        '
        Me.grpStatistics.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpStatistics.Controls.Add(Me.lblAvailableSpace)
        Me.grpStatistics.Controls.Add(Me.lblTotalSize)
        Me.grpStatistics.Controls.Add(Me.lblBackupCount)
        Me.grpStatistics.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.grpStatistics.Location = New System.Drawing.Point(851, 149)
        Me.grpStatistics.Name = "grpStatistics"
        Me.grpStatistics.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.grpStatistics.Size = New System.Drawing.Size(282, 115)
        Me.grpStatistics.TabIndex = 3
        Me.grpStatistics.TabStop = False
        Me.grpStatistics.Text = "الإحصائيات"
        '
        'lblAvailableSpace
        '
        Me.lblAvailableSpace.AutoSize = True
        Me.lblAvailableSpace.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblAvailableSpace.Location = New System.Drawing.Point(67, 83)
        Me.lblAvailableSpace.Name = "lblAvailableSpace"
        Me.lblAvailableSpace.Size = New System.Drawing.Size(126, 23)
        Me.lblAvailableSpace.TabIndex = 2
        Me.lblAvailableSpace.Text = "المساحة المتاحة: --"
        '
        'lblTotalSize
        '
        Me.lblTotalSize.AutoSize = True
        Me.lblTotalSize.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblTotalSize.Location = New System.Drawing.Point(67, 52)
        Me.lblTotalSize.Name = "lblTotalSize"
        Me.lblTotalSize.Size = New System.Drawing.Size(115, 23)
        Me.lblTotalSize.TabIndex = 1
        Me.lblTotalSize.Text = "الحجم الإجمالي: --"
        '
        'lblBackupCount
        '
        Me.lblBackupCount.AutoSize = True
        Me.lblBackupCount.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblBackupCount.Location = New System.Drawing.Point(67, 21)
        Me.lblBackupCount.Name = "lblBackupCount"
        Me.lblBackupCount.Size = New System.Drawing.Size(148, 23)
        Me.lblBackupCount.TabIndex = 0
        Me.lblBackupCount.Text = "عدد النسخ الاحتياطية: 0"
        '
        'grpSelection
        '
        Me.grpSelection.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSelection.Controls.Add(Me.lblSelectedBackup)
        Me.grpSelection.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.grpSelection.Location = New System.Drawing.Point(851, 270)
        Me.grpSelection.Name = "grpSelection"
        Me.grpSelection.Size = New System.Drawing.Size(282, 93)
        Me.grpSelection.TabIndex = 4
        Me.grpSelection.TabStop = False
        Me.grpSelection.Text = "المحدد حالياً"
        '
        'lblSelectedBackup
        '
        Me.lblSelectedBackup.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblSelectedBackup.Location = New System.Drawing.Point(10, 27)
        Me.lblSelectedBackup.Name = "lblSelectedBackup"
        Me.lblSelectedBackup.Size = New System.Drawing.Size(262, 59)
        Me.lblSelectedBackup.TabIndex = 0
        Me.lblSelectedBackup.Text = "لم يتم اختيار نسخة احتياطية"
        Me.lblSelectedBackup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpActions
        '
        Me.grpActions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpActions.Controls.Add(Me.btnCleanOldBackups)
        Me.grpActions.Controls.Add(Me.btnExportBackup)
        Me.grpActions.Controls.Add(Me.btnViewBackupInfo)
        Me.grpActions.Controls.Add(Me.btnDeleteBackup)
        Me.grpActions.Controls.Add(Me.btnRestoreBackup)
        Me.grpActions.Controls.Add(Me.btnCreateBackup)
        Me.grpActions.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.grpActions.Location = New System.Drawing.Point(851, 361)
        Me.grpActions.Name = "grpActions"
        Me.grpActions.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.grpActions.Size = New System.Drawing.Size(282, 327)
        Me.grpActions.TabIndex = 5
        Me.grpActions.TabStop = False
        Me.grpActions.Text = "الإجراءات"
        '
        'btnCleanOldBackups
        '
        Me.btnCleanOldBackups.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)

        Me.btnCleanOldBackups.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCleanOldBackups.Location = New System.Drawing.Point(10, 254)
        Me.btnCleanOldBackups.Name = "btnCleanOldBackups"
        Me.btnCleanOldBackups.Size = New System.Drawing.Size(262, 35)
        Me.btnCleanOldBackups.TabIndex = 5
        Me.btnCleanOldBackups.Text = "تنظيف النسخ القديمة"
        Me.btnCleanOldBackups.UseVisualStyleBackColor = True
        '
        'btnExportBackup
        '
        Me.btnExportBackup.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)

        Me.btnExportBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportBackup.Location = New System.Drawing.Point(10, 212)
        Me.btnExportBackup.Name = "btnExportBackup"
        Me.btnExportBackup.Size = New System.Drawing.Size(262, 35)
        Me.btnExportBackup.TabIndex = 4
        Me.btnExportBackup.Text = "تصدير النسخة"
        Me.btnExportBackup.UseVisualStyleBackColor = True
        '
        'btnViewBackupInfo
        '
        Me.btnViewBackupInfo.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)

        Me.btnViewBackupInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnViewBackupInfo.Location = New System.Drawing.Point(10, 170)
        Me.btnViewBackupInfo.Name = "btnViewBackupInfo"
        Me.btnViewBackupInfo.Size = New System.Drawing.Size(262, 35)
        Me.btnViewBackupInfo.TabIndex = 3
        Me.btnViewBackupInfo.Text = "عرض التفاصيل"
        Me.btnViewBackupInfo.UseVisualStyleBackColor = True
        '
        'btnDeleteBackup
        '
        Me.btnDeleteBackup.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)

        Me.btnDeleteBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDeleteBackup.Location = New System.Drawing.Point(10, 128)
        Me.btnDeleteBackup.Name = "btnDeleteBackup"
        Me.btnDeleteBackup.Size = New System.Drawing.Size(262, 35)
        Me.btnDeleteBackup.TabIndex = 2
        Me.btnDeleteBackup.Text = "حذف النسخة"
        Me.btnDeleteBackup.UseVisualStyleBackColor = True
        '
        'btnRestoreBackup
        '
        Me.btnRestoreBackup.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)

        Me.btnRestoreBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRestoreBackup.Location = New System.Drawing.Point(10, 86)
        Me.btnRestoreBackup.Name = "btnRestoreBackup"
        Me.btnRestoreBackup.Size = New System.Drawing.Size(262, 35)
        Me.btnRestoreBackup.TabIndex = 1
        Me.btnRestoreBackup.Text = "استعادة النسخة"
        Me.btnRestoreBackup.UseVisualStyleBackColor = True
        '
        'btnCreateBackup
        '
        Me.btnCreateBackup.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)

        Me.btnCreateBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCreateBackup.Location = New System.Drawing.Point(10, 44)
        Me.btnCreateBackup.Name = "btnCreateBackup"
        Me.btnCreateBackup.Size = New System.Drawing.Size(262, 35)
        Me.btnCreateBackup.TabIndex = 0
        Me.btnCreateBackup.Text = "إنشاء نسخة احتياطية"
        Me.btnCreateBackup.UseVisualStyleBackColor = True
        '
        'timerRefresh
        '
        Me.timerRefresh.Interval = 1000
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)

        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(12, 695)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(80, 38)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "إغلاق"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRefresh.Location = New System.Drawing.Point(1044, 695)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(89, 38)
        Me.btnRefresh.TabIndex = 6
        Me.btnRefresh.Text = "تحديث"
        Me.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'BackupManagerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1145, 741)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.grpActions)
        Me.Controls.Add(Me.grpSelection)
        Me.Controls.Add(Me.grpStatistics)
        Me.Controls.Add(Me.grpBackupsList)
        Me.Controls.Add(Me.grpBackupPath)
        Me.Controls.Add(Me.grpDatabaseInfo)
        Me.MaximumSize = New System.Drawing.Size(1161, 780)
        Me.MinimumSize = New System.Drawing.Size(1161, 780)
        Me.Name = "BackupManagerForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "إدارة النسخ الاحتياطية - نظام ESIS"
        Me.grpDatabaseInfo.ResumeLayout(False)
        Me.grpBackupPath.ResumeLayout(False)
        Me.grpBackupPath.PerformLayout()
        Me.grpBackupsList.ResumeLayout(False)
        Me.grpStatistics.ResumeLayout(False)
        Me.grpStatistics.PerformLayout()
        Me.grpSelection.ResumeLayout(False)
        Me.grpActions.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpDatabaseInfo As GroupBox
    Friend WithEvents lblLastBackup As Label
    Friend WithEvents lblDatabaseSize As Label
    Friend WithEvents lblDatabaseName As Label
    Friend WithEvents grpBackupPath As GroupBox
    Friend WithEvents btnBrowsePath As Button
    Friend WithEvents txtBackupPath As TextBox
    Friend WithEvents lblBackupPath As Label
    Friend WithEvents grpBackupsList As GroupBox
    Friend WithEvents lstBackups As ListView
    Friend WithEvents colFileName As ColumnHeader
    Friend WithEvents colFileSize As ColumnHeader
    Friend WithEvents colCreationDate As ColumnHeader
    Friend WithEvents colDescription As ColumnHeader
    Friend WithEvents colType As ColumnHeader
    Friend WithEvents grpStatistics As GroupBox
    Friend WithEvents lblAvailableSpace As Label
    Friend WithEvents lblTotalSize As Label
    Friend WithEvents lblBackupCount As Label
    Friend WithEvents grpSelection As GroupBox
    Friend WithEvents lblSelectedBackup As Label
    Friend WithEvents grpActions As GroupBox
    Friend WithEvents btnCreateBackup As Button
    Friend WithEvents btnRestoreBackup As Button
    Friend WithEvents btnDeleteBackup As Button
    Friend WithEvents btnViewBackupInfo As Button
    Friend WithEvents btnExportBackup As Button
    Friend WithEvents btnCleanOldBackups As Button
    Friend WithEvents btnRefresh As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents timerRefresh As Timer
End Class