<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RestoreForm
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
        Me.grpCurrentDatabaseInfo = New System.Windows.Forms.GroupBox()
        Me.lblCurrentLastBackup = New System.Windows.Forms.Label()
        Me.lblCurrentDatabaseSize = New System.Windows.Forms.Label()
        Me.lblCurrentDatabaseName = New System.Windows.Forms.Label()
        Me.grpBackupSelection = New System.Windows.Forms.GroupBox()
        Me.lblBackupsCount = New System.Windows.Forms.Label()
        Me.lstAvailableBackups = New System.Windows.Forms.ListView()
        Me.colFileName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colFileSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colCreationDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.txtBackupPath = New System.Windows.Forms.TextBox()
        Me.lblBackupPath = New System.Windows.Forms.Label()
        Me.grpSelectedBackupInfo = New System.Windows.Forms.GroupBox()
        Me.txtBackupFileDescription = New System.Windows.Forms.TextBox()
        Me.lblBackupFileDescription = New System.Windows.Forms.Label()
        Me.lblSelectedBackupDate = New System.Windows.Forms.Label()
        Me.lblSelectedBackupSize = New System.Windows.Forms.Label()
        Me.lblSelectedBackupFile = New System.Windows.Forms.Label()
        Me.grpRestoreSettings = New System.Windows.Forms.GroupBox()
        Me.txtRestoreDescription = New System.Windows.Forms.TextBox()
        Me.lblRestoreDescription = New System.Windows.Forms.Label()
        Me.chkCloseConnectionsBeforeRestore = New System.Windows.Forms.CheckBox()
        Me.chkCreateBackupBeforeRestore = New System.Windows.Forms.CheckBox()
        Me.grpRestoreProgress = New System.Windows.Forms.GroupBox()
        Me.lblRestoreStatus = New System.Windows.Forms.Label()
        Me.progressBarRestore = New System.Windows.Forms.ProgressBar()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnCancelRestore = New System.Windows.Forms.Button()
        Me.btnStartRestore = New System.Windows.Forms.Button()
        Me.btnRefreshBackupsList = New System.Windows.Forms.Button()
        Me.btnBrowseBackupFile = New System.Windows.Forms.Button()
        Me.btnBrowseBackupPath = New System.Windows.Forms.Button()
        Me.grpCurrentDatabaseInfo.SuspendLayout()
        Me.grpBackupSelection.SuspendLayout()
        Me.grpSelectedBackupInfo.SuspendLayout()
        Me.grpRestoreSettings.SuspendLayout()
        Me.grpRestoreProgress.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpCurrentDatabaseInfo
        '
        Me.grpCurrentDatabaseInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpCurrentDatabaseInfo.Controls.Add(Me.lblCurrentLastBackup)
        Me.grpCurrentDatabaseInfo.Controls.Add(Me.lblCurrentDatabaseSize)
        Me.grpCurrentDatabaseInfo.Controls.Add(Me.lblCurrentDatabaseName)
        Me.grpCurrentDatabaseInfo.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.grpCurrentDatabaseInfo.Location = New System.Drawing.Point(12, 11)
        Me.grpCurrentDatabaseInfo.Name = "grpCurrentDatabaseInfo"
        Me.grpCurrentDatabaseInfo.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.grpCurrentDatabaseInfo.Size = New System.Drawing.Size(760, 111)
        Me.grpCurrentDatabaseInfo.TabIndex = 0
        Me.grpCurrentDatabaseInfo.TabStop = False
        Me.grpCurrentDatabaseInfo.Text = "معلومات قاعدة البيانات الحالية"
        '
        'lblCurrentLastBackup
        '
        Me.lblCurrentLastBackup.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblCurrentLastBackup.Location = New System.Drawing.Point(149, 72)
        Me.lblCurrentLastBackup.Name = "lblCurrentLastBackup"
        Me.lblCurrentLastBackup.Size = New System.Drawing.Size(471, 23)
        Me.lblCurrentLastBackup.TabIndex = 2
        Me.lblCurrentLastBackup.Text = "آخر نسخة احتياطية: --"
        Me.lblCurrentLastBackup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCurrentDatabaseSize
        '
        Me.lblCurrentDatabaseSize.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblCurrentDatabaseSize.Location = New System.Drawing.Point(149, 47)
        Me.lblCurrentDatabaseSize.Name = "lblCurrentDatabaseSize"
        Me.lblCurrentDatabaseSize.Size = New System.Drawing.Size(471, 23)
        Me.lblCurrentDatabaseSize.TabIndex = 1
        Me.lblCurrentDatabaseSize.Text = "حجم قاعدة البيانات: --"
        Me.lblCurrentDatabaseSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCurrentDatabaseName
        '
        Me.lblCurrentDatabaseName.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblCurrentDatabaseName.Location = New System.Drawing.Point(149, 22)
        Me.lblCurrentDatabaseName.Name = "lblCurrentDatabaseName"
        Me.lblCurrentDatabaseName.Size = New System.Drawing.Size(471, 23)
        Me.lblCurrentDatabaseName.TabIndex = 0
        Me.lblCurrentDatabaseName.Text = "اسم قاعدة البيانات: --"
        Me.lblCurrentDatabaseName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpBackupSelection
        '
        Me.grpBackupSelection.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBackupSelection.Controls.Add(Me.btnRefreshBackupsList)
        Me.grpBackupSelection.Controls.Add(Me.btnBrowseBackupFile)
        Me.grpBackupSelection.Controls.Add(Me.lblBackupsCount)
        Me.grpBackupSelection.Controls.Add(Me.lstAvailableBackups)
        Me.grpBackupSelection.Controls.Add(Me.btnBrowseBackupPath)
        Me.grpBackupSelection.Controls.Add(Me.txtBackupPath)
        Me.grpBackupSelection.Controls.Add(Me.lblBackupPath)
        Me.grpBackupSelection.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.grpBackupSelection.Location = New System.Drawing.Point(12, 117)
        Me.grpBackupSelection.Name = "grpBackupSelection"
        Me.grpBackupSelection.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.grpBackupSelection.Size = New System.Drawing.Size(760, 268)
        Me.grpBackupSelection.TabIndex = 1
        Me.grpBackupSelection.TabStop = False
        Me.grpBackupSelection.Text = "اختيار النسخة الاحتياطية"
        '
        'lblBackupsCount
        '
        Me.lblBackupsCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblBackupsCount.AutoSize = True
        Me.lblBackupsCount.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblBackupsCount.Location = New System.Drawing.Point(15, 229)
        Me.lblBackupsCount.Name = "lblBackupsCount"
        Me.lblBackupsCount.Size = New System.Drawing.Size(195, 23)
        Me.lblBackupsCount.TabIndex = 4
        Me.lblBackupsCount.Text = "عدد النسخ الاحتياطية المتاحة: 0"
        '
        'lstAvailableBackups
        '
        Me.lstAvailableBackups.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstAvailableBackups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colFileName, Me.colFileSize, Me.colCreationDate})
        Me.lstAvailableBackups.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lstAvailableBackups.FullRowSelect = True
        Me.lstAvailableBackups.GridLines = True
        Me.lstAvailableBackups.HideSelection = False
        Me.lstAvailableBackups.Location = New System.Drawing.Point(15, 82)
        Me.lstAvailableBackups.MultiSelect = False
        Me.lstAvailableBackups.Name = "lstAvailableBackups"
        Me.lstAvailableBackups.Size = New System.Drawing.Size(730, 135)
        Me.lstAvailableBackups.TabIndex = 3
        Me.lstAvailableBackups.UseCompatibleStateImageBehavior = False
        Me.lstAvailableBackups.View = System.Windows.Forms.View.Details
        '
        'colFileName
        '
        Me.colFileName.Text = "اسم الملف"
        Me.colFileName.Width = 300
        '
        'colFileSize
        '
        Me.colFileSize.Text = "الحجم"
        Me.colFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colFileSize.Width = 120
        '
        'colCreationDate
        '
        Me.colCreationDate.Text = "تاريخ الإنشاء"
        Me.colCreationDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colCreationDate.Width = 150
        '
        'txtBackupPath
        '
        Me.txtBackupPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBackupPath.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.txtBackupPath.Location = New System.Drawing.Point(51, 46)
        Me.txtBackupPath.Name = "txtBackupPath"
        Me.txtBackupPath.Size = New System.Drawing.Size(694, 30)
        Me.txtBackupPath.TabIndex = 1
        Me.txtBackupPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblBackupPath
        '
        Me.lblBackupPath.AutoSize = True
        Me.lblBackupPath.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblBackupPath.Location = New System.Drawing.Point(606, 20)
        Me.lblBackupPath.Name = "lblBackupPath"
        Me.lblBackupPath.Size = New System.Drawing.Size(143, 23)
        Me.lblBackupPath.TabIndex = 0
        Me.lblBackupPath.Text = "مسار النسخ الاحتياطية:"
        '
        'grpSelectedBackupInfo
        '
        Me.grpSelectedBackupInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSelectedBackupInfo.Controls.Add(Me.txtBackupFileDescription)
        Me.grpSelectedBackupInfo.Controls.Add(Me.lblBackupFileDescription)
        Me.grpSelectedBackupInfo.Controls.Add(Me.lblSelectedBackupDate)
        Me.grpSelectedBackupInfo.Controls.Add(Me.lblSelectedBackupSize)
        Me.grpSelectedBackupInfo.Controls.Add(Me.lblSelectedBackupFile)
        Me.grpSelectedBackupInfo.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.grpSelectedBackupInfo.Location = New System.Drawing.Point(12, 391)
        Me.grpSelectedBackupInfo.Name = "grpSelectedBackupInfo"
        Me.grpSelectedBackupInfo.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.grpSelectedBackupInfo.Size = New System.Drawing.Size(760, 137)
        Me.grpSelectedBackupInfo.TabIndex = 2
        Me.grpSelectedBackupInfo.TabStop = False
        Me.grpSelectedBackupInfo.Text = "معلومات النسخة الاحتياطية المحددة"
        '
        'txtBackupFileDescription
        '
        Me.txtBackupFileDescription.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBackupFileDescription.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.txtBackupFileDescription.Location = New System.Drawing.Point(15, 95)
        Me.txtBackupFileDescription.Multiline = True
        Me.txtBackupFileDescription.Name = "txtBackupFileDescription"
        Me.txtBackupFileDescription.ReadOnly = True
        Me.txtBackupFileDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtBackupFileDescription.Size = New System.Drawing.Size(730, 32)
        Me.txtBackupFileDescription.TabIndex = 4
        '
        'lblBackupFileDescription
        '
        Me.lblBackupFileDescription.AutoSize = True
        Me.lblBackupFileDescription.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblBackupFileDescription.Location = New System.Drawing.Point(591, 69)
        Me.lblBackupFileDescription.Name = "lblBackupFileDescription"
        Me.lblBackupFileDescription.Size = New System.Drawing.Size(154, 23)
        Me.lblBackupFileDescription.TabIndex = 3
        Me.lblBackupFileDescription.Text = "وصف النسخة الاحتياطية:"
        '
        'lblSelectedBackupDate
        '
        Me.lblSelectedBackupDate.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblSelectedBackupDate.Location = New System.Drawing.Point(255, 44)
        Me.lblSelectedBackupDate.Name = "lblSelectedBackupDate"
        Me.lblSelectedBackupDate.Size = New System.Drawing.Size(499, 23)
        Me.lblSelectedBackupDate.TabIndex = 2
        Me.lblSelectedBackupDate.Text = "تاريخ الإنشاء: --"
        Me.lblSelectedBackupDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSelectedBackupSize
        '
        Me.lblSelectedBackupSize.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblSelectedBackupSize.Location = New System.Drawing.Point(6, 42)
        Me.lblSelectedBackupSize.Name = "lblSelectedBackupSize"
        Me.lblSelectedBackupSize.Size = New System.Drawing.Size(200, 34)
        Me.lblSelectedBackupSize.TabIndex = 1
        Me.lblSelectedBackupSize.Text = "الحجم: --"
        Me.lblSelectedBackupSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSelectedBackupFile
        '
        Me.lblSelectedBackupFile.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblSelectedBackupFile.Location = New System.Drawing.Point(259, 19)
        Me.lblSelectedBackupFile.Name = "lblSelectedBackupFile"
        Me.lblSelectedBackupFile.Size = New System.Drawing.Size(499, 23)
        Me.lblSelectedBackupFile.TabIndex = 0
        Me.lblSelectedBackupFile.Text = "لم يتم اختيار ملف"
        Me.lblSelectedBackupFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpRestoreSettings
        '
        Me.grpRestoreSettings.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpRestoreSettings.Controls.Add(Me.txtRestoreDescription)
        Me.grpRestoreSettings.Controls.Add(Me.lblRestoreDescription)
        Me.grpRestoreSettings.Controls.Add(Me.chkCloseConnectionsBeforeRestore)
        Me.grpRestoreSettings.Controls.Add(Me.chkCreateBackupBeforeRestore)
        Me.grpRestoreSettings.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.grpRestoreSettings.Location = New System.Drawing.Point(12, 524)
        Me.grpRestoreSettings.Name = "grpRestoreSettings"
        Me.grpRestoreSettings.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.grpRestoreSettings.Size = New System.Drawing.Size(760, 130)
        Me.grpRestoreSettings.TabIndex = 3
        Me.grpRestoreSettings.TabStop = False
        Me.grpRestoreSettings.Text = "إعدادات الاستعادة"
        '
        'txtRestoreDescription
        '
        Me.txtRestoreDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRestoreDescription.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRestoreDescription.Location = New System.Drawing.Point(15, 78)
        Me.txtRestoreDescription.Multiline = True
        Me.txtRestoreDescription.Name = "txtRestoreDescription"
        Me.txtRestoreDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtRestoreDescription.Size = New System.Drawing.Size(730, 46)
        Me.txtRestoreDescription.TabIndex = 3
        '
        'lblRestoreDescription
        '
        Me.lblRestoreDescription.AutoSize = True
        Me.lblRestoreDescription.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblRestoreDescription.Location = New System.Drawing.Point(636, 52)
        Me.lblRestoreDescription.Name = "lblRestoreDescription"
        Me.lblRestoreDescription.Size = New System.Drawing.Size(109, 23)
        Me.lblRestoreDescription.TabIndex = 2
        Me.lblRestoreDescription.Text = "وصف الاستعادة:"
        '
        'chkCloseConnectionsBeforeRestore
        '
        Me.chkCloseConnectionsBeforeRestore.AutoSize = True
        Me.chkCloseConnectionsBeforeRestore.Checked = True
        Me.chkCloseConnectionsBeforeRestore.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCloseConnectionsBeforeRestore.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.chkCloseConnectionsBeforeRestore.Location = New System.Drawing.Point(400, 25)
        Me.chkCloseConnectionsBeforeRestore.Name = "chkCloseConnectionsBeforeRestore"
        Me.chkCloseConnectionsBeforeRestore.Size = New System.Drawing.Size(206, 27)
        Me.chkCloseConnectionsBeforeRestore.TabIndex = 1
        Me.chkCloseConnectionsBeforeRestore.Text = "إغلاق الاتصالات قبل الاستعادة"
        Me.chkCloseConnectionsBeforeRestore.UseVisualStyleBackColor = True
        '
        'chkCreateBackupBeforeRestore
        '
        Me.chkCreateBackupBeforeRestore.AutoSize = True
        Me.chkCreateBackupBeforeRestore.Checked = True
        Me.chkCreateBackupBeforeRestore.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCreateBackupBeforeRestore.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.chkCreateBackupBeforeRestore.Location = New System.Drawing.Point(15, 25)
        Me.chkCreateBackupBeforeRestore.Name = "chkCreateBackupBeforeRestore"
        Me.chkCreateBackupBeforeRestore.Size = New System.Drawing.Size(238, 27)
        Me.chkCreateBackupBeforeRestore.TabIndex = 0
        Me.chkCreateBackupBeforeRestore.Text = "إنشاء نسخة احتياطية قبل الاستعادة"
        Me.chkCreateBackupBeforeRestore.UseVisualStyleBackColor = True
        '
        'grpRestoreProgress
        '
        Me.grpRestoreProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpRestoreProgress.Controls.Add(Me.lblRestoreStatus)
        Me.grpRestoreProgress.Controls.Add(Me.progressBarRestore)
        Me.grpRestoreProgress.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.grpRestoreProgress.Location = New System.Drawing.Point(12, 660)
        Me.grpRestoreProgress.Name = "grpRestoreProgress"
        Me.grpRestoreProgress.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.grpRestoreProgress.Size = New System.Drawing.Size(760, 80)
        Me.grpRestoreProgress.TabIndex = 4
        Me.grpRestoreProgress.TabStop = False
        Me.grpRestoreProgress.Text = "تقدم الاستعادة"
        '
        'lblRestoreStatus
        '
        Me.lblRestoreStatus.AutoSize = True
        Me.lblRestoreStatus.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.lblRestoreStatus.Location = New System.Drawing.Point(15, 50)
        Me.lblRestoreStatus.Name = "lblRestoreStatus"
        Me.lblRestoreStatus.Size = New System.Drawing.Size(51, 23)
        Me.lblRestoreStatus.TabIndex = 1
        Me.lblRestoreStatus.Text = "جاهز..."
        Me.lblRestoreStatus.Visible = False
        '
        'progressBarRestore
        '
        Me.progressBarRestore.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.progressBarRestore.Location = New System.Drawing.Point(15, 25)
        Me.progressBarRestore.Name = "progressBarRestore"
        Me.progressBarRestore.Size = New System.Drawing.Size(730, 20)
        Me.progressBarRestore.TabIndex = 0
        Me.progressBarRestore.Visible = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(12, 750)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(82, 35)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "إغلاق"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnCancelRestore
        '
        Me.btnCancelRestore.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelRestore.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.btnCancelRestore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelRestore.Location = New System.Drawing.Point(503, 750)
        Me.btnCancelRestore.Name = "btnCancelRestore"
        Me.btnCancelRestore.Size = New System.Drawing.Size(129, 35)
        Me.btnCancelRestore.TabIndex = 6
        Me.btnCancelRestore.Text = "إلغاء العملية"
        Me.btnCancelRestore.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelRestore.UseVisualStyleBackColor = True
        '
        'btnStartRestore
        '
        Me.btnStartRestore.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStartRestore.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.btnStartRestore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnStartRestore.Location = New System.Drawing.Point(648, 750)
        Me.btnStartRestore.Name = "btnStartRestore"
        Me.btnStartRestore.Size = New System.Drawing.Size(124, 35)
        Me.btnStartRestore.TabIndex = 5
        Me.btnStartRestore.Text = "بدء الاستعادة"
        Me.btnStartRestore.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnStartRestore.UseVisualStyleBackColor = True
        '
        'btnRefreshBackupsList
        '
        Me.btnRefreshBackupsList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefreshBackupsList.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.btnRefreshBackupsList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRefreshBackupsList.Location = New System.Drawing.Point(650, 224)
        Me.btnRefreshBackupsList.Name = "btnRefreshBackupsList"
        Me.btnRefreshBackupsList.Size = New System.Drawing.Size(95, 35)
        Me.btnRefreshBackupsList.TabIndex = 6
        Me.btnRefreshBackupsList.Text = "تحديث القائمة"
        Me.btnRefreshBackupsList.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRefreshBackupsList.UseVisualStyleBackColor = True
        '
        'btnBrowseBackupFile
        '
        Me.btnBrowseBackupFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseBackupFile.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.btnBrowseBackupFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBrowseBackupFile.Location = New System.Drawing.Point(537, 224)
        Me.btnBrowseBackupFile.Name = "btnBrowseBackupFile"
        Me.btnBrowseBackupFile.Size = New System.Drawing.Size(107, 35)
        Me.btnBrowseBackupFile.TabIndex = 5
        Me.btnBrowseBackupFile.Text = "تصفح ملف"
        Me.btnBrowseBackupFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBrowseBackupFile.UseVisualStyleBackColor = True
        '
        'btnBrowseBackupPath
        '
        Me.btnBrowseBackupPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseBackupPath.Font = New System.Drawing.Font("Cairo Medium", 8.999999!, System.Drawing.FontStyle.Bold)
        Me.btnBrowseBackupPath.Location = New System.Drawing.Point(15, 46)
        Me.btnBrowseBackupPath.Name = "btnBrowseBackupPath"
        Me.btnBrowseBackupPath.Size = New System.Drawing.Size(30, 30)
        Me.btnBrowseBackupPath.TabIndex = 2
        Me.btnBrowseBackupPath.UseVisualStyleBackColor = True
        '
        'RestoreForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(784, 797)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnCancelRestore)
        Me.Controls.Add(Me.btnStartRestore)
        Me.Controls.Add(Me.grpRestoreProgress)
        Me.Controls.Add(Me.grpRestoreSettings)
        Me.Controls.Add(Me.grpSelectedBackupInfo)
        Me.Controls.Add(Me.grpBackupSelection)
        Me.Controls.Add(Me.grpCurrentDatabaseInfo)
        Me.MaximumSize = New System.Drawing.Size(800, 836)
        Me.MinimumSize = New System.Drawing.Size(800, 836)
        Me.Name = "RestoreForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "استعادة قاعدة البيانات - نظام ESIS"
        Me.grpCurrentDatabaseInfo.ResumeLayout(False)
        Me.grpBackupSelection.ResumeLayout(False)
        Me.grpBackupSelection.PerformLayout()
        Me.grpSelectedBackupInfo.ResumeLayout(False)
        Me.grpSelectedBackupInfo.PerformLayout()
        Me.grpRestoreSettings.ResumeLayout(False)
        Me.grpRestoreSettings.PerformLayout()
        Me.grpRestoreProgress.ResumeLayout(False)
        Me.grpRestoreProgress.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpCurrentDatabaseInfo As GroupBox
    Friend WithEvents lblCurrentLastBackup As Label
    Friend WithEvents lblCurrentDatabaseSize As Label
    Friend WithEvents lblCurrentDatabaseName As Label
    Friend WithEvents grpBackupSelection As GroupBox
    Friend WithEvents btnBrowseBackupPath As Button
    Friend WithEvents txtBackupPath As TextBox
    Friend WithEvents lblBackupPath As Label
    Friend WithEvents lstAvailableBackups As ListView
    Friend WithEvents colFileName As ColumnHeader
    Friend WithEvents colFileSize As ColumnHeader
    Friend WithEvents colCreationDate As ColumnHeader
    Friend WithEvents lblBackupsCount As Label
    Friend WithEvents btnBrowseBackupFile As Button
    Friend WithEvents btnRefreshBackupsList As Button
    Friend WithEvents grpSelectedBackupInfo As GroupBox
    Friend WithEvents lblSelectedBackupFile As Label
    Friend WithEvents lblSelectedBackupSize As Label
    Friend WithEvents lblSelectedBackupDate As Label
    Friend WithEvents txtBackupFileDescription As TextBox
    Friend WithEvents lblBackupFileDescription As Label
    Friend WithEvents grpRestoreSettings As GroupBox
    Friend WithEvents chkCreateBackupBeforeRestore As CheckBox
    Friend WithEvents chkCloseConnectionsBeforeRestore As CheckBox
    Friend WithEvents txtRestoreDescription As TextBox
    Friend WithEvents lblRestoreDescription As Label
    Friend WithEvents grpRestoreProgress As GroupBox
    Friend WithEvents progressBarRestore As ProgressBar
    Friend WithEvents lblRestoreStatus As Label
    Friend WithEvents btnStartRestore As Button
    Friend WithEvents btnCancelRestore As Button
    Friend WithEvents btnClose As Button
End Class