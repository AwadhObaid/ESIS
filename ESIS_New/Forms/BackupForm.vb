Imports System.IO
Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports System.ComponentModel

''' <summary>
''' نموذج النسخ الاحتياطي لقاعدة البيانات
''' </summary>
Public Class BackupForm
    Inherits Form

    Private backupWorker As BackgroundWorker
    Private backupHelper As New DatabaseBackupHelper()
    Private isBackupInProgress As Boolean = False

    Public Sub New()
        InitializeComponent()
        InitializeBackgroundWorker()
        LoadSettings()
    End Sub

    ''' <summary>
    ''' تهيئة BackgroundWorker للعمليات غير المتزامنة
    ''' </summary>
    Private Sub InitializeBackgroundWorker()
        Try
            backupWorker = New BackgroundWorker()
            backupWorker.WorkerReportsProgress = True
            backupWorker.WorkerSupportsCancellation = True

            AddHandler backupWorker.DoWork, AddressOf BackupWorker_DoWork
            AddHandler backupWorker.ProgressChanged, AddressOf BackupWorker_ProgressChanged
            AddHandler backupWorker.RunWorkerCompleted, AddressOf BackupWorker_RunWorkerCompleted

        Catch ex As Exception
            MessageBox.Show($"خطأ في تهيئة BackgroundWorker: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تحميل الإعدادات المحفوظة
    ''' </summary>
    Private Sub LoadSettings()
        Try
            ' تحميل المسار الافتراضي للنسخ الاحتياطية
            Dim defaultPath As String = My.Settings.DefaultBackupPath
            If String.IsNullOrEmpty(defaultPath) Then
                defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ESIS_Backups")
            End If

            txtBackupPath.Text = defaultPath

            ' تحميل إعدادات أخرى
            chkCompressBackup.Checked = My.Settings.CompressBackup
            chkIncludeArchiveFiles.Checked = My.Settings.IncludeArchiveFiles
            chkAutoDeleteOldBackups.Checked = My.Settings.AutoDeleteOldBackups
            nudKeepBackupsFor.Value = My.Settings.KeepBackupsForDays

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل الإعدادات: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' حفظ الإعدادات
    ''' </summary>
    Private Sub SaveSettings()
        Try
            My.Settings.DefaultBackupPath = txtBackupPath.Text.Trim()
            My.Settings.CompressBackup = chkCompressBackup.Checked
            My.Settings.IncludeArchiveFiles = chkIncludeArchiveFiles.Checked
            My.Settings.AutoDeleteOldBackups = chkAutoDeleteOldBackups.Checked
            My.Settings.KeepBackupsForDays = CInt(nudKeepBackupsFor.Value)
            My.Settings.Save()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حفظ الإعدادات: {ex.Message}")
        End Try
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        If Not Me.IsHandleCreated Then
            Me.CreateHandle() ' إعادة إنشاء المقبض يدويًا
        End If
    End Sub
    Private Sub BackupForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' إعداد النموذج
            Me.Text = "نسخ احتياطي لقاعدة البيانات - نظام ESIS"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterScreen
            ' التحقق من دعم خيار الضغط
            If Not backupHelper.IsCompressionSupported() Then
                chkCompressBackup.Enabled = False
                chkCompressBackup.Checked = False
                'ToolTip.SetToolTip(chkCompressBackup, "خيار الضغط غير مدعوم في إصدار SQL Server الحالي.")
            End If
            ' تحديث معلومات قاعدة البيانات
            UpdateDatabaseInfo()

            ' تحديث قائمة النسخ الاحتياطية الموجودة
            RefreshBackupsList()

            ' تعطيل أزرار الإلغاء في البداية
            btnCancelBackup.Enabled = False

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تحديث معلومات قاعدة البيانات
    ''' </summary>
    Public Sub UpdateDatabaseInfo()
        Try
            Dim dbInfo As DatabaseInfo = backupHelper.GetDatabaseInfo()

            lblDatabaseName.Text = $"اسم قاعدة البيانات: {dbInfo.DatabaseName}"
            lblDatabaseSize.Text = $"حجم قاعدة البيانات: {FormatFileSize(dbInfo.DatabaseSize)}"
            lblLastBackup.Text = $"آخر نسخة احتياطية: {If(dbInfo.LastBackupDate.HasValue, dbInfo.LastBackupDate.Value.ToString("yyyy/MM/dd HH:mm"), "لا توجد")}"

        Catch ex As Exception
            lblDatabaseName.Text = "خطأ في جلب معلومات قاعدة البيانات"
            lblDatabaseSize.Text = ""
            lblLastBackup.Text = ""
            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateDatabaseInfo: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث قائمة النسخ الاحتياطية الموجودة
    ''' </summary>
    Public Sub RefreshBackupsList()
        Try
            lstExistingBackups.Items.Clear()

            Dim backupPath As String = txtBackupPath.Text.Trim()
            If Directory.Exists(backupPath) Then
                Dim backupFiles() As String = Directory.GetFiles(backupPath, "*.bak")

                For Each file As String In backupFiles.OrderByDescending(Function(f) New FileInfo(f).CreationTime)
                    Dim fileInfo As New FileInfo(file)
                    Dim displayText As String = $"{fileInfo.Name} - {FormatFileSize(fileInfo.Length)} - {fileInfo.CreationTime:yyyy/MM/dd HH:mm}"
                    lstExistingBackups.Items.Add(displayText)
                Next

                lblBackupsCount.Text = $"عدد النسخ الاحتياطية: {backupFiles.Length}"
            Else
                lblBackupsCount.Text = "عدد النسخ الاحتياطية: 0"
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في RefreshBackupsList: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تنسيق حجم الملف
    ''' </summary>
    Private Function FormatFileSize(sizeInBytes As Long) As String
        Try
            Dim sizes() As String = {"بايت", "كيلوبايت", "ميجابايت", "جيجابايت"}
            Dim order As Integer = 0
            Dim size As Double = sizeInBytes

            While size >= 1024 AndAlso order < sizes.Length - 1
                order += 1
                size = size / 1024
            End While

            Return $"{size:F2} {sizes(order)}"
        Catch ex As Exception
            Return $"{sizeInBytes} بايت"
        End Try
    End Function

    Private Sub btnBrowseBackupPath_Click(sender As Object, e As EventArgs) Handles btnBrowseBackupPath.Click
        Try
            Using folderDialog As New FolderBrowserDialog()
                folderDialog.Description = "اختر مجلد حفظ النسخ الاحتياطية"
                folderDialog.SelectedPath = txtBackupPath.Text.Trim()

                If folderDialog.ShowDialog() = DialogResult.OK Then
                    txtBackupPath.Text = folderDialog.SelectedPath
                    RefreshBackupsList()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"خطأ في اختيار المجلد: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnStartBackup_Click(sender As Object, e As EventArgs) Handles btnStartBackup.Click
        Try
            ' التحقق من صحة البيانات
            If Not ValidateBackupSettings() Then
                Return
            End If

            ' تأكيد بدء النسخ الاحتياطي
            Dim result As DialogResult = MessageBox.Show(
                "هل تريد بدء عملية النسخ الاحتياطي؟" & vbNewLine &
                $"المسار: {txtBackupPath.Text}" & vbNewLine &
                $"ضغط الملف: {If(chkCompressBackup.Checked, "نعم", "لا")}" & vbNewLine &
                $"تضمين ملفات الأرشيف: {If(chkIncludeArchiveFiles.Checked, "نعم", "لا")}",
                "تأكيد النسخ الاحتياطي",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                StartBackupProcess()
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في بدء النسخ الاحتياطي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' التحقق من صحة إعدادات النسخ الاحتياطي
    ''' </summary>
    Private Function ValidateBackupSettings() As Boolean
        Try
            ' التحقق من مسار النسخ الاحتياطي
            Dim backupPath As String = txtBackupPath.Text.Trim()
            If String.IsNullOrEmpty(backupPath) Then
                MessageBox.Show("يرجى تحديد مسار حفظ النسخة الاحتياطية.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtBackupPath.Focus()
                Return False
            End If

            ' إنشاء المجلد إذا لم يكن موجوداً
            If Not Directory.Exists(backupPath) Then
                Try
                    Directory.CreateDirectory(backupPath)
                Catch ex As Exception
                    MessageBox.Show($"لا يمكن إنشاء مجلد النسخ الاحتياطية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End Try
            End If

            ' التحقق من صلاحيات الكتابة
            Try
                Dim testFile As String = Path.Combine(backupPath, "test_write.tmp")
                File.WriteAllText(testFile, "test")
                File.Delete(testFile)
            Catch ex As Exception
                MessageBox.Show($"لا توجد صلاحيات كتابة في المجلد المحدد: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try

            ' التحقق من المساحة المتاحة
            Dim driveInfo As New DriveInfo(Path.GetPathRoot(backupPath))
            Dim dbInfo As DatabaseInfo = backupHelper.GetDatabaseInfo()

            If driveInfo.AvailableFreeSpace < (dbInfo.DatabaseSize * 2) Then ' مساحة إضافية للأمان
                Dim result As DialogResult = MessageBox.Show(
                    "المساحة المتاحة قد لا تكون كافية للنسخة الاحتياطية." & vbNewLine &
                    "هل تريد المتابعة؟",
                    "تحذير المساحة",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning)

                If result = DialogResult.No Then
                    Return False
                End If
            End If

            Return True

        Catch ex As Exception
            MessageBox.Show($"خطأ في التحقق من الإعدادات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' بدء عملية النسخ الاحتياطي
    ''' </summary>
    Private Sub StartBackupProcess()
        Try
            If isBackupInProgress Then
                MessageBox.Show("عملية نسخ احتياطي جارية بالفعل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' حفظ الإعدادات
            SaveSettings()

            ' تحضير معاملات النسخ الاحتياطي
            Dim backupParams As New BackupParameters() With {
                .BackupPath = txtBackupPath.Text.Trim(),
                .CompressBackup = chkCompressBackup.Checked,
                .IncludeArchiveFiles = chkIncludeArchiveFiles.Checked,
                .BackupDescription = txtBackupDescription.Text.Trim(),
                .AutoDeleteOldBackups = chkAutoDeleteOldBackups.Checked,
                .KeepBackupsForDays = CInt(nudKeepBackupsFor.Value)
            }

            ' تحديث واجهة المستخدم
            SetBackupInProgress(True)

            ' بدء العملية في الخلفية
            backupWorker.RunWorkerAsync(backupParams)

        Catch ex As Exception
            SetBackupInProgress(False)
            MessageBox.Show($"خطأ في بدء عملية النسخ الاحتياطي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تحديث واجهة المستخدم أثناء النسخ الاحتياطي
    ''' </summary>
    Private Sub SetBackupInProgress(inProgress As Boolean)
        Try
            isBackupInProgress = inProgress

            ' تفعيل/تعطيل العناصر
            btnStartBackup.Enabled = Not inProgress
            btnCancelBackup.Enabled = inProgress
            txtBackupPath.Enabled = Not inProgress
            btnBrowseBackupPath.Enabled = Not inProgress
            chkCompressBackup.Enabled = Not inProgress
            chkIncludeArchiveFiles.Enabled = Not inProgress
            txtBackupDescription.Enabled = Not inProgress
            chkAutoDeleteOldBackups.Enabled = Not inProgress
            nudKeepBackupsFor.Enabled = Not inProgress

            If inProgress Then
                ' إعداد شريط التقدم
                progressBarBackup.Value = 0
                progressBarBackup.Visible = True
                lblBackupStatus.Text = "جاري التحضير للنسخ الاحتياطي..."
                lblBackupStatus.Visible = True

                ' تغيير شكل المؤشر
                Me.Cursor = Cursors.WaitCursor
            Else
                ' إخفاء شريط التقدم
                progressBarBackup.Visible = False
                lblBackupStatus.Visible = False

                ' إعادة المؤشر العادي
                Me.Cursor = Cursors.Default
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في SetBackupInProgress: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالج حدث DoWork للـ BackgroundWorker
    ''' </summary>
    Private Sub BackupWorker_DoWork(sender As Object, e As DoWorkEventArgs)
        Try
            Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
            Dim backupParams As BackupParameters = CType(e.Argument, BackupParameters)

            ' تنفيذ النسخ الاحتياطي
            Dim result As BackupResult = backupHelper.CreateBackup(backupParams, worker)

            ' إرجاع النتيجة
            e.Result = result

        Catch ex As Exception
            e.Result = New BackupResult() With {
                .Success = False,
                .ErrorMessage = ex.Message,
                .BackupFilePath = ""
            }
        End Try
    End Sub

    ''' <summary>
    ''' معالج حدث ProgressChanged للـ BackgroundWorker
    ''' </summary>
    Private Sub BackupWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs)
        Try
            ' تحديث شريط التقدم
            progressBarBackup.Value = Math.Min(e.ProgressPercentage, 100)

            ' تحديث رسالة الحالة
            If e.UserState IsNot Nothing Then
                lblBackupStatus.Text = e.UserState.ToString()
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في BackupWorker_ProgressChanged: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالج حدث RunWorkerCompleted للـ BackgroundWorker
    ''' </summary>
    Private Sub BackupWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        Try
            ' تحديث واجهة المستخدم
            SetBackupInProgress(False)

            If e.Cancelled Then
                MessageBox.Show("تم إلغاء عملية النسخ الاحتياطي.", "إلغاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf e.Error IsNot Nothing Then
                MessageBox.Show($"حدث خطأ أثناء النسخ الاحتياطي: {e.Error.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                ' النسخ الاحتياطي مكتمل
                Dim result As BackupResult = CType(e.Result, BackupResult)

                If result.Success Then
                    MessageBox.Show(
                        $"تم إنشاء النسخة الاحتياطية بنجاح!" & vbNewLine &
                        $"المسار: {result.BackupFilePath}" & vbNewLine &
                        $"الحجم: {FormatFileSize(result.BackupFileSize)}" & vbNewLine &
                        $"الوقت المستغرق: {result.Duration.ToString("hh\:mm\:ss")}",
                        "نجح",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information)

                    ' تحديث القوائم
                    RefreshBackupsList()
                    UpdateDatabaseInfo()

                    ' مسح وصف النسخة الاحتياطية
                    txtBackupDescription.Clear()
                Else
                    MessageBox.Show($"فشل في إنشاء النسخة الاحتياطية: {result.ErrorMessage}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في معالجة نتيجة النسخ الاحتياطي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelBackup_Click(sender As Object, e As EventArgs) Handles btnCancelBackup.Click
        Try
            If isBackupInProgress AndAlso backupWorker.IsBusy Then
                Dim result As DialogResult = MessageBox.Show(
                    "هل تريد إلغاء عملية النسخ الاحتياطي الجارية؟",
                    "تأكيد الإلغاء",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)

                If result = DialogResult.Yes Then
                    backupWorker.CancelAsync()
                    lblBackupStatus.Text = "جاري إلغاء العملية..."
                End If
            End If
        Catch ex As Exception
            MessageBox.Show($"خطأ في إلغاء النسخ الاحتياطي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDeleteSelectedBackup_Click(sender As Object, e As EventArgs) Handles btnDeleteSelectedBackup.Click
        Try
            If lstExistingBackups.SelectedIndex = -1 Then
                MessageBox.Show("يرجى اختيار نسخة احتياطية للحذف.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim selectedItem As String = lstExistingBackups.SelectedItem.ToString()
            Dim fileName As String = selectedItem.Split(" - ")(0)
            Dim filePath As String = Path.Combine(txtBackupPath.Text.Trim(), fileName)

            Dim result As DialogResult = MessageBox.Show(
                $"هل تريد حذف النسخة الاحتياطية؟" & vbNewLine &
                $"الملف: {fileName}",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                File.Delete(filePath)
                MessageBox.Show("تم حذف النسخة الاحتياطية بنجاح.", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                RefreshBackupsList()
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في حذف النسخة الاحتياطية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub chkAutoDeleteOldBackups_CheckedChanged(sender As Object, e As EventArgs) Handles chkAutoDeleteOldBackups.CheckedChanged
        nudKeepBackupsFor.Enabled = chkAutoDeleteOldBackups.Checked
        lblKeepBackupsFor.Enabled = chkAutoDeleteOldBackups.Checked
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            If isBackupInProgress Then
                MessageBox.Show("لا يمكن إغلاق النموذج أثناء عملية النسخ الاحتياطي.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            SaveSettings()
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"خطأ في إغلاق النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BackupForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If isBackupInProgress Then
                e.Cancel = True
                MessageBox.Show("لا يمكن إغلاق النموذج أثناء عملية النسخ الاحتياطي.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في BackupForm_FormClosing: {ex.Message}")
        End Try
    End Sub

End Class
