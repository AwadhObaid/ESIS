Imports System.Reflection
Imports System.Configuration
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
    ' يخزن آخر نتيجة للنسخ الاحتياطي
    Private LastBackupResult As BackupResult = Nothing

    Private requestedBackupPath As String = ""
    Private useNetworkFallback As Boolean = False
    Private lastNetworkCopyWarning As String = ""
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
            ' التحقق من المساحة المتاحة (للمسارات المحلية فقط)
            Dim dbInfo As DatabaseInfo = backupHelper.GetDatabaseInfo()

            If Not backupPath.StartsWith("\") Then
                Dim root As String = Path.GetPathRoot(backupPath)
                If Not String.IsNullOrWhiteSpace(root) Then
                    Dim driveInfo As New DriveInfo(root)
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

                ' في حال وجود تحذير (مثلاً تم النسخ محليًا وتعذر النسخ للشبكة)
                If LastBackupResult IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(LastBackupResult.ErrorMessage) Then
                    MessageBox.Show(LastBackupResult.ErrorMessage, "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

                Return
            End If

            ' حفظ الإعدادات
            SaveSettings()


            requestedBackupPath = txtBackupPath.Text.Trim()
            useNetworkFallback = requestedBackupPath.StartsWith("\\")
            lastNetworkCopyWarning = ""

            ' تحضير معاملات النسخ الاحتياطي
            Dim backupParams As New BackupParameters() With {
                .BackupPath = If(useNetworkFallback, GetSqlDefaultBackupDirectorySafe(), requestedBackupPath),
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

            ' إذا كان المسار المطلوب مسار شبكة وفشل النسخ بسبب صلاحيات SQL Service،
            ' نقوم بعمل نسخة محلية على السيرفر ثم نحاول نسخها إلى مسار الشبكة بصلاحيات المستخدم.
            If (Not result.Success) AndAlso useNetworkFallback AndAlso result.ErrorMessage IsNot Nothing AndAlso
               (result.ErrorMessage.ToLower().Contains("access is denied") OrElse result.ErrorMessage.Contains("Operating system error 5")) Then

                Dim localDir As String = GetSqlDefaultBackupDirectorySafe()
                backupParams.BackupPath = localDir

                result = backupHelper.CreateBackup(backupParams, worker)
            End If

            ' إذا تم استخدام مسار محلي (Fallback) لوجهة شبكة، نحاول نسخ الناتج إلى مسار المستخدم
            If result.Success AndAlso useNetworkFallback Then
                Dim finalPath As String = result.BackupFilePath
                Dim warn As String = ""

                If TryCopyBackupToRequestedPath(result.BackupFilePath, requestedBackupPath, finalPath, warn) Then
                    result.BackupFilePath = finalPath
                Else
                    ' نُبقي النسخة ناجحة لكن نعرض تحذيرًا للمستخدم
                    result.ErrorMessage = warn
                End If
            End If

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


    ' =========================
    '  Network/SQL Helpers
    ' =========================

    Private Function TryGetSqlConnectionString() As String
        ' 1) Try public property on backupHelper
        Try
            Dim t = backupHelper.GetType()
            Dim p = t.GetProperty("ConnectionString", BindingFlags.Public Or BindingFlags.Instance)
            If p IsNot Nothing Then
                Dim v = TryCast(p.GetValue(backupHelper, Nothing), String)
                If Not String.IsNullOrWhiteSpace(v) Then Return v
            End If
        Catch
        End Try

        ' 2) Try private field on backupHelper
        Try
            Dim t = backupHelper.GetType()
            For Each f In t.GetFields(BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Public)
                If f.FieldType Is GetType(String) AndAlso f.Name.ToLower().Contains("connection") Then
                    Dim v = TryCast(f.GetValue(backupHelper), String)
                    If Not String.IsNullOrWhiteSpace(v) Then Return v
                End If
            Next
        Catch
        End Try

        ' 3) Try My.Settings (any string that looks like a connection string)
        Try
            For Each sp As SettingsProperty In My.Settings.Properties
                If sp.PropertyType Is GetType(String) Then
                    Dim v = TryCast(My.Settings(sp.Name), String)
                    If Not String.IsNullOrWhiteSpace(v) AndAlso v.ToLower().Contains("data source") Then
                        Return v
                    End If
                End If
            Next
        Catch
        End Try

        Return ""
    End Function

    Private Function GetSqlServerHostName() As String
        Try
            Dim cs = TryGetSqlConnectionString()
            If String.IsNullOrWhiteSpace(cs) Then Return ""
            Dim b As New SqlConnectionStringBuilder(cs)
            Dim ds As String = If(b.DataSource, "").Trim()
            If ds = "" Then Return ""
            Dim host = ds.Split("\"c)(0)
            host = host.Split(","c)(0)
            Return host.Trim()
        Catch
            Return ""
        End Try
    End Function

    Private Function GetSqlDefaultBackupDirectorySafe() As String
        Dim fallback As String = "C:\Windows\Temp"
        Try
            Dim cs = TryGetSqlConnectionString()
            If String.IsNullOrWhiteSpace(cs) Then Return fallback

            Using con As New SqlConnection(cs)
                con.Open()

                Using cmd As New SqlCommand("SELECT CAST(SERVERPROPERTY('InstanceDefaultBackupPath') AS nvarchar(4000));", con)
                    Dim v = TryCast(cmd.ExecuteScalar(), String)
                    If Not String.IsNullOrWhiteSpace(v) AndAlso Directory.Exists(v.Trim()) Then
                        Return v.Trim().TrimEnd("\"c)
                    End If
                End Using

                Using cmd As New SqlCommand("
DECLARE @path nvarchar(4000);
EXEC master.dbo.xp_instance_regread 
    N'HKEY_LOCAL_MACHINE',
    N'Software\Microsoft\MSSQLServer\MSSQLServer',
    N'BackupDirectory',
    @path OUTPUT;
SELECT @path;", con)
                    Dim v = TryCast(cmd.ExecuteScalar(), String)
                    If Not String.IsNullOrWhiteSpace(v) AndAlso Directory.Exists(v.Trim()) Then
                        Return v.Trim().TrimEnd("\"c)
                    End If
                End Using
            End Using
        Catch
        End Try

        Return fallback
    End Function

    Private Function BuildAdminUncFromLocalPath(serverHost As String, localPath As String) As String
        If String.IsNullOrWhiteSpace(serverHost) OrElse String.IsNullOrWhiteSpace(localPath) Then Return ""
        Try
            Dim root As String = Path.GetPathRoot(localPath)
            If String.IsNullOrWhiteSpace(root) OrElse root.Length < 2 Then Return ""
            Dim driveLetter As String = root.Substring(0, 1)
            Dim rest As String = localPath.Substring(root.Length).TrimStart("\"c)
            Return $"\\{serverHost}\{driveLetter}$\{rest}"
        Catch
            Return ""
        End Try
    End Function

    Private Function TryCopyBackupToRequestedPath(sourceLocalPathOnServer As String, requestedFolder As String, ByRef finalPath As String, ByRef warning As String) As Boolean
        finalPath = sourceLocalPathOnServer
        warning = ""

        If String.IsNullOrWhiteSpace(sourceLocalPathOnServer) OrElse String.IsNullOrWhiteSpace(requestedFolder) Then
            warning = "لا يمكن تحديد مسار النسخة أو مسار الوجهة."
            Return False
        End If

        Dim fileName As String = Path.GetFileName(sourceLocalPathOnServer)
        Dim destFile As String = Path.Combine(requestedFolder.TrimEnd("\"c), fileName)

        Try
            If Not Directory.Exists(requestedFolder) Then
                Directory.CreateDirectory(requestedFolder)
            End If
        Catch ex As Exception
            warning = "تعذر الوصول إلى مسار الشبكة أو إنشاؤه: " & ex.Message
            Return False
        End Try

        ' Try copy from direct local path (works if app runs on SQL server)
        Try
            If File.Exists(sourceLocalPathOnServer) Then
                File.Copy(sourceLocalPathOnServer, destFile, True)
                finalPath = destFile
                Return True
            End If
        Catch
        End Try

        ' Try copy via admin share to SQL server (requires admin rights)
        Dim serverHost As String = GetSqlServerHostName()
        Dim adminUnc As String = BuildAdminUncFromLocalPath(serverHost, sourceLocalPathOnServer)

        If adminUnc <> "" Then
            Try
                If File.Exists(adminUnc) Then
                    File.Copy(adminUnc, destFile, True)
                    finalPath = destFile
                    Return True
                End If
            Catch ex As Exception
                warning = "تم إنشاء النسخة على السيرفر لكن تعذر نسخها لمسار الشبكة. " &
                          "السبب: " & ex.Message & vbNewLine &
                          "الحل: شغّل البرنامج على السيرفر أو شارك مجلد النسخ على السيرفر أو امنح صلاحيات كافية."
                Return False
            End Try
        End If

        warning = "تم إنشاء النسخة على السيرفر لكن لم يتم العثور على الملف لنسخه لمسار الشبكة. " &
                  "الحل: شغّل العملية من السيرفر أو شارك مجلد النسخ على السيرفر."
        Return False
    End Function

End Class



'Imports System.IO
'Imports System.Data.SqlClient
'Imports System.Threading.Tasks
'Imports System.ComponentModel

'''' <summary>
'''' نموذج النسخ الاحتياطي لقاعدة البيانات
'''' </summary>
'Public Class BackupForm
'    Inherits Form

'    Private backupWorker As BackgroundWorker
'    Private backupHelper As New DatabaseBackupHelper()
'    Private isBackupInProgress As Boolean = False

'    Public Sub New()
'        InitializeComponent()
'        InitializeBackgroundWorker()
'        LoadSettings()
'    End Sub

'    ''' <summary>
'    ''' تهيئة BackgroundWorker للعمليات غير المتزامنة
'    ''' </summary>
'    Private Sub InitializeBackgroundWorker()
'        Try
'            backupWorker = New BackgroundWorker()
'            backupWorker.WorkerReportsProgress = True
'            backupWorker.WorkerSupportsCancellation = True

'            AddHandler backupWorker.DoWork, AddressOf BackupWorker_DoWork
'            AddHandler backupWorker.ProgressChanged, AddressOf BackupWorker_ProgressChanged
'            AddHandler backupWorker.RunWorkerCompleted, AddressOf BackupWorker_RunWorkerCompleted

'        Catch ex As Exception
'            MessageBox.Show($"خطأ في تهيئة BackgroundWorker: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' تحميل الإعدادات المحفوظة
'    ''' </summary>
'    Private Sub LoadSettings()
'        Try
'            ' تحميل المسار الافتراضي للنسخ الاحتياطية
'            Dim defaultPath As String = My.Settings.DefaultBackupPath
'            If String.IsNullOrEmpty(defaultPath) Then
'                defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ESIS_Backups")
'            End If

'            txtBackupPath.Text = defaultPath

'            ' تحميل إعدادات أخرى
'            chkCompressBackup.Checked = My.Settings.CompressBackup
'            chkIncludeArchiveFiles.Checked = My.Settings.IncludeArchiveFiles
'            chkAutoDeleteOldBackups.Checked = My.Settings.AutoDeleteOldBackups
'            nudKeepBackupsFor.Value = My.Settings.KeepBackupsForDays

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل الإعدادات: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' حفظ الإعدادات
'    ''' </summary>
'    Private Sub SaveSettings()
'        Try
'            My.Settings.DefaultBackupPath = txtBackupPath.Text.Trim()
'            My.Settings.CompressBackup = chkCompressBackup.Checked
'            My.Settings.IncludeArchiveFiles = chkIncludeArchiveFiles.Checked
'            My.Settings.AutoDeleteOldBackups = chkAutoDeleteOldBackups.Checked
'            My.Settings.KeepBackupsForDays = CInt(nudKeepBackupsFor.Value)
'            My.Settings.Save()
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في حفظ الإعدادات: {ex.Message}")
'        End Try
'    End Sub

'    Protected Overrides Sub OnHandleCreated(e As EventArgs)
'        MyBase.OnHandleCreated(e)
'        If Not Me.IsHandleCreated Then
'            Me.CreateHandle() ' إعادة إنشاء المقبض يدويًا
'        End If
'    End Sub
'    Private Sub BackupForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        Try
'            ' إعداد النموذج
'            Me.Text = "نسخ احتياطي لقاعدة البيانات - نظام ESIS"
'            Me.RightToLeft = RightToLeft.Yes
'            Me.RightToLeftLayout = True
'            Me.StartPosition = FormStartPosition.CenterScreen
'            ' التحقق من دعم خيار الضغط
'            If Not backupHelper.IsCompressionSupported() Then
'                chkCompressBackup.Enabled = False
'                chkCompressBackup.Checked = False
'                'ToolTip.SetToolTip(chkCompressBackup, "خيار الضغط غير مدعوم في إصدار SQL Server الحالي.")
'            End If
'            ' تحديث معلومات قاعدة البيانات
'            UpdateDatabaseInfo()

'            ' تحديث قائمة النسخ الاحتياطية الموجودة
'            RefreshBackupsList()

'            ' تعطيل أزرار الإلغاء في البداية
'            btnCancelBackup.Enabled = False

'        Catch ex As Exception
'            MessageBox.Show($"خطأ في تحميل النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' تحديث معلومات قاعدة البيانات
'    ''' </summary>
'    Public Sub UpdateDatabaseInfo()
'        Try
'            Dim dbInfo As DatabaseInfo = backupHelper.GetDatabaseInfo()

'            lblDatabaseName.Text = $"اسم قاعدة البيانات: {dbInfo.DatabaseName}"
'            lblDatabaseSize.Text = $"حجم قاعدة البيانات: {FormatFileSize(dbInfo.DatabaseSize)}"
'            lblLastBackup.Text = $"آخر نسخة احتياطية: {If(dbInfo.LastBackupDate.HasValue, dbInfo.LastBackupDate.Value.ToString("yyyy/MM/dd HH:mm"), "لا توجد")}"

'        Catch ex As Exception
'            lblDatabaseName.Text = "خطأ في جلب معلومات قاعدة البيانات"
'            lblDatabaseSize.Text = ""
'            lblLastBackup.Text = ""
'            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateDatabaseInfo: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' تحديث قائمة النسخ الاحتياطية الموجودة
'    ''' </summary>
'    Public Sub RefreshBackupsList()
'        Try
'            lstExistingBackups.Items.Clear()

'            Dim backupPath As String = txtBackupPath.Text.Trim()
'            If Directory.Exists(backupPath) Then
'                Dim backupFiles() As String = Directory.GetFiles(backupPath, "*.bak")

'                For Each file As String In backupFiles.OrderByDescending(Function(f) New FileInfo(f).CreationTime)
'                    Dim fileInfo As New FileInfo(file)
'                    Dim displayText As String = $"{fileInfo.Name} - {FormatFileSize(fileInfo.Length)} - {fileInfo.CreationTime:yyyy/MM/dd HH:mm}"
'                    lstExistingBackups.Items.Add(displayText)
'                Next

'                lblBackupsCount.Text = $"عدد النسخ الاحتياطية: {backupFiles.Length}"
'            Else
'                lblBackupsCount.Text = "عدد النسخ الاحتياطية: 0"
'            End If

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في RefreshBackupsList: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' تنسيق حجم الملف
'    ''' </summary>
'    Private Function FormatFileSize(sizeInBytes As Long) As String
'        Try
'            Dim sizes() As String = {"بايت", "كيلوبايت", "ميجابايت", "جيجابايت"}
'            Dim order As Integer = 0
'            Dim size As Double = sizeInBytes

'            While size >= 1024 AndAlso order < sizes.Length - 1
'                order += 1
'                size = size / 1024
'            End While

'            Return $"{size:F2} {sizes(order)}"
'        Catch ex As Exception
'            Return $"{sizeInBytes} بايت"
'        End Try
'    End Function

'    Private Sub btnBrowseBackupPath_Click(sender As Object, e As EventArgs) Handles btnBrowseBackupPath.Click
'        Try
'            Using folderDialog As New FolderBrowserDialog()
'                folderDialog.Description = "اختر مجلد حفظ النسخ الاحتياطية"
'                folderDialog.SelectedPath = txtBackupPath.Text.Trim()

'                If folderDialog.ShowDialog() = DialogResult.OK Then
'                    txtBackupPath.Text = folderDialog.SelectedPath
'                    RefreshBackupsList()
'                End If
'            End Using
'        Catch ex As Exception
'            MessageBox.Show($"خطأ في اختيار المجلد: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    Private Sub btnStartBackup_Click(sender As Object, e As EventArgs) Handles btnStartBackup.Click
'        Try
'            ' التحقق من صحة البيانات
'            If Not ValidateBackupSettings() Then
'                Return
'            End If

'            ' تأكيد بدء النسخ الاحتياطي
'            Dim result As DialogResult = MessageBox.Show(
'                "هل تريد بدء عملية النسخ الاحتياطي؟" & vbNewLine &
'                $"المسار: {txtBackupPath.Text}" & vbNewLine &
'                $"ضغط الملف: {If(chkCompressBackup.Checked, "نعم", "لا")}" & vbNewLine &
'                $"تضمين ملفات الأرشيف: {If(chkIncludeArchiveFiles.Checked, "نعم", "لا")}",
'                "تأكيد النسخ الاحتياطي",
'                MessageBoxButtons.YesNo,
'                MessageBoxIcon.Question)

'            If result = DialogResult.Yes Then
'                StartBackupProcess()
'            End If

'        Catch ex As Exception
'            MessageBox.Show($"خطأ في بدء النسخ الاحتياطي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' التحقق من صحة إعدادات النسخ الاحتياطي
'    ''' </summary>
'    Private Function ValidateBackupSettings() As Boolean
'        Try
'            ' التحقق من مسار النسخ الاحتياطي
'            Dim backupPath As String = txtBackupPath.Text.Trim()
'            If String.IsNullOrEmpty(backupPath) Then
'                MessageBox.Show("يرجى تحديد مسار حفظ النسخة الاحتياطية.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'                txtBackupPath.Focus()
'                Return False
'            End If

'            ' إنشاء المجلد إذا لم يكن موجوداً
'            If Not Directory.Exists(backupPath) Then
'                Try
'                    Directory.CreateDirectory(backupPath)
'                Catch ex As Exception
'                    MessageBox.Show($"لا يمكن إنشاء مجلد النسخ الاحتياطية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                    Return False
'                End Try
'            End If

'            ' التحقق من صلاحيات الكتابة
'            Try
'                Dim testFile As String = Path.Combine(backupPath, "test_write.tmp")
'                File.WriteAllText(testFile, "test")
'                File.Delete(testFile)
'            Catch ex As Exception
'                MessageBox.Show($"لا توجد صلاحيات كتابة في المجلد المحدد: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                Return False
'            End Try

'            ' التحقق من المساحة المتاحة
'            Dim driveInfo As New DriveInfo(Path.GetPathRoot(backupPath))
'            Dim dbInfo As DatabaseInfo = backupHelper.GetDatabaseInfo()

'            If driveInfo.AvailableFreeSpace < (dbInfo.DatabaseSize * 2) Then ' مساحة إضافية للأمان
'                Dim result As DialogResult = MessageBox.Show(
'                    "المساحة المتاحة قد لا تكون كافية للنسخة الاحتياطية." & vbNewLine &
'                    "هل تريد المتابعة؟",
'                    "تحذير المساحة",
'                    MessageBoxButtons.YesNo,
'                    MessageBoxIcon.Warning)

'                If result = DialogResult.No Then
'                    Return False
'                End If
'            End If

'            Return True

'        Catch ex As Exception
'            MessageBox.Show($"خطأ في التحقق من الإعدادات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' بدء عملية النسخ الاحتياطي
'    ''' </summary>
'    Private Sub StartBackupProcess()
'        Try
'            If isBackupInProgress Then
'                MessageBox.Show("عملية نسخ احتياطي جارية بالفعل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
'                Return
'            End If

'            ' حفظ الإعدادات
'            SaveSettings()

'            ' تحضير معاملات النسخ الاحتياطي
'            Dim backupParams As New BackupParameters() With {
'                .BackupPath = txtBackupPath.Text.Trim(),
'                .CompressBackup = chkCompressBackup.Checked,
'                .IncludeArchiveFiles = chkIncludeArchiveFiles.Checked,
'                .BackupDescription = txtBackupDescription.Text.Trim(),
'                .AutoDeleteOldBackups = chkAutoDeleteOldBackups.Checked,
'                .KeepBackupsForDays = CInt(nudKeepBackupsFor.Value)
'            }

'            ' تحديث واجهة المستخدم
'            SetBackupInProgress(True)

'            ' بدء العملية في الخلفية
'            backupWorker.RunWorkerAsync(backupParams)

'        Catch ex As Exception
'            SetBackupInProgress(False)
'            MessageBox.Show($"خطأ في بدء عملية النسخ الاحتياطي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' تحديث واجهة المستخدم أثناء النسخ الاحتياطي
'    ''' </summary>
'    Private Sub SetBackupInProgress(inProgress As Boolean)
'        Try
'            isBackupInProgress = inProgress

'            ' تفعيل/تعطيل العناصر
'            btnStartBackup.Enabled = Not inProgress
'            btnCancelBackup.Enabled = inProgress
'            txtBackupPath.Enabled = Not inProgress
'            btnBrowseBackupPath.Enabled = Not inProgress
'            chkCompressBackup.Enabled = Not inProgress
'            chkIncludeArchiveFiles.Enabled = Not inProgress
'            txtBackupDescription.Enabled = Not inProgress
'            chkAutoDeleteOldBackups.Enabled = Not inProgress
'            nudKeepBackupsFor.Enabled = Not inProgress

'            If inProgress Then
'                ' إعداد شريط التقدم
'                progressBarBackup.Value = 0
'                progressBarBackup.Visible = True
'                lblBackupStatus.Text = "جاري التحضير للنسخ الاحتياطي..."
'                lblBackupStatus.Visible = True

'                ' تغيير شكل المؤشر
'                Me.Cursor = Cursors.WaitCursor
'            Else
'                ' إخفاء شريط التقدم
'                progressBarBackup.Visible = False
'                lblBackupStatus.Visible = False

'                ' إعادة المؤشر العادي
'                Me.Cursor = Cursors.Default
'            End If

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في SetBackupInProgress: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' معالج حدث DoWork للـ BackgroundWorker
'    ''' </summary>
'    Private Sub BackupWorker_DoWork(sender As Object, e As DoWorkEventArgs)
'        Try
'            Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
'            Dim backupParams As BackupParameters = CType(e.Argument, BackupParameters)

'            ' تنفيذ النسخ الاحتياطي
'            Dim result As BackupResult = backupHelper.CreateBackup(backupParams, worker)

'            ' إرجاع النتيجة
'            e.Result = result

'        Catch ex As Exception
'            e.Result = New BackupResult() With {
'                .Success = False,
'                .ErrorMessage = ex.Message,
'                .BackupFilePath = ""
'            }
'        End Try
'    End Sub

'    ''' <summary>
'    ''' معالج حدث ProgressChanged للـ BackgroundWorker
'    ''' </summary>
'    Private Sub BackupWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs)
'        Try
'            ' تحديث شريط التقدم
'            progressBarBackup.Value = Math.Min(e.ProgressPercentage, 100)

'            ' تحديث رسالة الحالة
'            If e.UserState IsNot Nothing Then
'                lblBackupStatus.Text = e.UserState.ToString()
'            End If

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في BackupWorker_ProgressChanged: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' معالج حدث RunWorkerCompleted للـ BackgroundWorker
'    ''' </summary>
'    Private Sub BackupWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
'        Try
'            ' تحديث واجهة المستخدم
'            SetBackupInProgress(False)

'            If e.Cancelled Then
'                MessageBox.Show("تم إلغاء عملية النسخ الاحتياطي.", "إلغاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
'            ElseIf e.Error IsNot Nothing Then
'                MessageBox.Show($"حدث خطأ أثناء النسخ الاحتياطي: {e.Error.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            Else
'                ' النسخ الاحتياطي مكتمل
'                Dim result As BackupResult = CType(e.Result, BackupResult)

'                If result.Success Then
'                    MessageBox.Show(
'                        $"تم إنشاء النسخة الاحتياطية بنجاح!" & vbNewLine &
'                        $"المسار: {result.BackupFilePath}" & vbNewLine &
'                        $"الحجم: {FormatFileSize(result.BackupFileSize)}" & vbNewLine &
'                        $"الوقت المستغرق: {result.Duration.ToString("hh\:mm\:ss")}",
'                        "نجح",
'                        MessageBoxButtons.OK,
'                        MessageBoxIcon.Information)

'                    ' تحديث القوائم
'                    RefreshBackupsList()
'                    UpdateDatabaseInfo()

'                    ' مسح وصف النسخة الاحتياطية
'                    txtBackupDescription.Clear()
'                Else
'                    MessageBox.Show($"فشل في إنشاء النسخة الاحتياطية: {result.ErrorMessage}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                End If
'            End If

'        Catch ex As Exception
'            MessageBox.Show($"خطأ في معالجة نتيجة النسخ الاحتياطي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    Private Sub btnCancelBackup_Click(sender As Object, e As EventArgs) Handles btnCancelBackup.Click
'        Try
'            If isBackupInProgress AndAlso backupWorker.IsBusy Then
'                Dim result As DialogResult = MessageBox.Show(
'                    "هل تريد إلغاء عملية النسخ الاحتياطي الجارية؟",
'                    "تأكيد الإلغاء",
'                    MessageBoxButtons.YesNo,
'                    MessageBoxIcon.Question)

'                If result = DialogResult.Yes Then
'                    backupWorker.CancelAsync()
'                    lblBackupStatus.Text = "جاري إلغاء العملية..."
'                End If
'            End If
'        Catch ex As Exception
'            MessageBox.Show($"خطأ في إلغاء النسخ الاحتياطي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    Private Sub btnDeleteSelectedBackup_Click(sender As Object, e As EventArgs) Handles btnDeleteSelectedBackup.Click
'        Try
'            If lstExistingBackups.SelectedIndex = -1 Then
'                MessageBox.Show("يرجى اختيار نسخة احتياطية للحذف.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'                Return
'            End If

'            Dim selectedItem As String = lstExistingBackups.SelectedItem.ToString()
'            Dim fileName As String = selectedItem.Split(" - ")(0)
'            Dim filePath As String = Path.Combine(txtBackupPath.Text.Trim(), fileName)

'            Dim result As DialogResult = MessageBox.Show(
'                $"هل تريد حذف النسخة الاحتياطية؟" & vbNewLine &
'                $"الملف: {fileName}",
'                "تأكيد الحذف",
'                MessageBoxButtons.YesNo,
'                MessageBoxIcon.Question)

'            If result = DialogResult.Yes Then
'                File.Delete(filePath)
'                MessageBox.Show("تم حذف النسخة الاحتياطية بنجاح.", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
'                RefreshBackupsList()
'            End If

'        Catch ex As Exception
'            MessageBox.Show($"خطأ في حذف النسخة الاحتياطية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    Private Sub chkAutoDeleteOldBackups_CheckedChanged(sender As Object, e As EventArgs) Handles chkAutoDeleteOldBackups.CheckedChanged
'        nudKeepBackupsFor.Enabled = chkAutoDeleteOldBackups.Checked
'        lblKeepBackupsFor.Enabled = chkAutoDeleteOldBackups.Checked
'    End Sub

'    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
'        Try
'            If isBackupInProgress Then
'                MessageBox.Show("لا يمكن إغلاق النموذج أثناء عملية النسخ الاحتياطي.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'                Return
'            End If

'            SaveSettings()
'            Me.Close()
'        Catch ex As Exception
'            MessageBox.Show($"خطأ في إغلاق النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    Private Sub BackupForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
'        Try
'            If isBackupInProgress Then
'                e.Cancel = True
'                MessageBox.Show("لا يمكن إغلاق النموذج أثناء عملية النسخ الاحتياطي.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في BackupForm_FormClosing: {ex.Message}")
'        End Try
'    End Sub

'End Class
