Imports System.IO
Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports System.ComponentModel

''' <summary>
''' نموذج استعادة قاعدة البيانات من النسخ الاحتياطية
''' </summary>
Public Class RestoreForm
    Inherits Form

    Private restoreWorker As BackgroundWorker
    Private backupHelper As New DatabaseBackupHelper()
    Private isRestoreInProgress As Boolean = False
    Private selectedBackupFile As String = ""

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
            restoreWorker = New BackgroundWorker()
            restoreWorker.WorkerReportsProgress = True
            restoreWorker.WorkerSupportsCancellation = True

            AddHandler restoreWorker.DoWork, AddressOf RestoreWorker_DoWork
            AddHandler restoreWorker.ProgressChanged, AddressOf RestoreWorker_ProgressChanged
            AddHandler restoreWorker.RunWorkerCompleted, AddressOf RestoreWorker_RunWorkerCompleted

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
            chkCreateBackupBeforeRestore.Checked = My.Settings.CreateBackupBeforeRestore
            chkCloseConnectionsBeforeRestore.Checked = My.Settings.CloseConnectionsBeforeRestore

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
            My.Settings.CreateBackupBeforeRestore = chkCreateBackupBeforeRestore.Checked
            My.Settings.CloseConnectionsBeforeRestore = chkCloseConnectionsBeforeRestore.Checked
            My.Settings.Save()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حفظ الإعدادات: {ex.Message}")
        End Try
    End Sub

    Private Sub RestoreForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' إعداد النموذج
            Me.Text = "استعادة قاعدة البيانات - نظام ESIS"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent

            ' تحديث معلومات قاعدة البيانات الحالية
            UpdateCurrentDatabaseInfo()

            ' تحديث قائمة النسخ الاحتياطية المتاحة
            RefreshBackupsList()

            ' تعطيل أزرار الإلغاء والاستعادة في البداية
            btnCancelRestore.Enabled = False
            btnStartRestore.Enabled = False

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تحديث معلومات قاعدة البيانات الحالية
    ''' </summary>
    Private Sub UpdateCurrentDatabaseInfo()
        Try
            Dim dbInfo As DatabaseInfo = backupHelper.GetDatabaseInfo()

            lblCurrentDatabaseName.Text = $"اسم قاعدة البيانات: {dbInfo.DatabaseName}"
            lblCurrentDatabaseSize.Text = $"حجم قاعدة البيانات: {FormatFileSize(dbInfo.DatabaseSize)}"
            lblCurrentLastBackup.Text = $"آخر نسخة احتياطية: {If(dbInfo.LastBackupDate.HasValue, dbInfo.LastBackupDate.Value.ToString("yyyy/MM/dd HH:mm"), "لا توجد")}"

        Catch ex As Exception
            lblCurrentDatabaseName.Text = "خطأ في جلب معلومات قاعدة البيانات"
            lblCurrentDatabaseSize.Text = ""
            lblCurrentLastBackup.Text = ""
            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateCurrentDatabaseInfo: {ex.Message}")
        End Try
    End Sub




    ''' <summary>
    ''' تحديث قائمة النسخ الاحتياطية المتاحة
    ''' </summary>
    Private Sub RefreshBackupsList()
        Try
            lstAvailableBackups.Items.Clear()
            ClearBackupFileInfo()

            Dim backupPath As String = txtBackupPath.Text.Trim()
            If Directory.Exists(backupPath) Then
                Dim backupFiles() As String = Directory.GetFiles(backupPath, "*.bak")

                For Each file As String In backupFiles.OrderByDescending(Function(f) New FileInfo(f).CreationTime)
                    Dim fileInfo As New FileInfo(file)
                    Dim displayText As String = $"{fileInfo.Name} - {FormatFileSize(fileInfo.Length)} - {fileInfo.CreationTime:yyyy/MM/dd HH:mm}"

                    Dim item As New ListViewItem(fileInfo.Name)
                    item.SubItems.Add(FormatFileSize(fileInfo.Length))
                    item.SubItems.Add(fileInfo.CreationTime.ToString("yyyy/MM/dd HH:mm"))
                    item.SubItems.Add(file) ' المسار الكامل

                    lstAvailableBackups.Items.Add(item)
                Next

                lblBackupsCount.Text = $"عدد النسخ الاحتياطية المتاحة: {backupFiles.Length}"
            Else
                lblBackupsCount.Text = "عدد النسخ الاحتياطية المتاحة: 0"
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في RefreshBackupsList: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' مسح معلومات ملف النسخة الاحتياطية
    ''' </summary>
    Private Sub ClearBackupFileInfo()
        Try
            lblSelectedBackupFile.Text = "لم يتم اختيار ملف"
            lblSelectedBackupSize.Text = ""
            lblSelectedBackupDate.Text = ""
            txtBackupFileDescription.Clear()
            btnStartRestore.Enabled = False
            selectedBackupFile = ""
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في ClearBackupFileInfo: {ex.Message}")
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
                folderDialog.Description = "اختر مجلد النسخ الاحتياطية"
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

    Private Sub btnBrowseBackupFile_Click(sender As Object, e As EventArgs) Handles btnBrowseBackupFile.Click
        Try
            Using openDialog As New OpenFileDialog()
                openDialog.Title = "اختر ملف النسخة الاحتياطية"
                openDialog.Filter = "ملفات النسخ الاحتياطية (*.bak)|*.bak|جميع الملفات (*.*)|*.*"
                openDialog.InitialDirectory = txtBackupPath.Text.Trim()

                If openDialog.ShowDialog() = DialogResult.OK Then
                    selectedBackupFile = openDialog.FileName
                    DisplayBackupFileInfo(selectedBackupFile)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"خطأ في اختيار الملف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' عرض معلومات ملف النسخة الاحتياطية المحدد
    ''' </summary>
    Private Sub DisplayBackupFileInfo(filePath As String)
        Try
            If File.Exists(filePath) Then
                Dim fileInfo As New FileInfo(filePath)

                lblSelectedBackupFile.Text = $"الملف المحدد: {fileInfo.Name}"
                lblSelectedBackupSize.Text = $"الحجم: {FormatFileSize(fileInfo.Length)}"
                lblSelectedBackupDate.Text = $"تاريخ الإنشاء: {fileInfo.CreationTime:yyyy/MM/dd HH:mm}"

                ' محاولة قراءة وصف النسخة الاحتياطية من قاعدة البيانات
                Try
                    Dim backupInfo As BackupFileInfo = backupHelper.GetBackupFileInfo(filePath)
                    If backupInfo IsNot Nothing Then
                        txtBackupFileDescription.Text = backupInfo.Description
                    End If
                Catch ex As Exception
                    txtBackupFileDescription.Text = "لا يمكن قراءة وصف النسخة الاحتياطية"
                End Try

                btnStartRestore.Enabled = True
            Else
                ClearBackupFileInfo()
                MessageBox.Show("الملف المحدد غير موجود.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            ClearBackupFileInfo()
            MessageBox.Show($"خطأ في قراءة معلومات الملف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub lstAvailableBackups_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstAvailableBackups.SelectedIndexChanged
        Try
            If lstAvailableBackups.SelectedItems.Count > 0 Then
                Dim selectedItem As ListViewItem = lstAvailableBackups.SelectedItems(0)
                selectedBackupFile = selectedItem.SubItems(3).Text ' المسار الكامل
                DisplayBackupFileInfo(selectedBackupFile)
                lblSelectedBackupFile.ForeColor = Color.Red
                lblSelectedBackupDate.ForeColor = Color.Red
                lblSelectedBackupSize.ForeColor = Color.Red
            Else
                ClearBackupFileInfo()
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في lstAvailableBackups_SelectedIndexChanged: {ex.Message}")
        End Try
    End Sub

    Private Sub btnStartRestore_Click(sender As Object, e As EventArgs) Handles btnStartRestore.Click
        Try
            ' التحقق من صحة البيانات
            If Not ValidateRestoreSettings() Then
                Return
            End If

            ' تأكيد بدء الاستعادة مع تحذير
            Dim warningMessage As String =
                "تحذير: عملية الاستعادة ستقوم بحذف جميع البيانات الحالية واستبدالها بالبيانات من النسخة الاحتياطية المحددة." & vbNewLine & vbNewLine &
                "هذه العملية لا يمكن التراجع عنها!" & vbNewLine & vbNewLine &
                $"الملف المحدد: {Path.GetFileName(selectedBackupFile)}" & vbNewLine &
                $"إنشاء نسخة احتياطية قبل الاستعادة: {If(chkCreateBackupBeforeRestore.Checked, "نعم", "لا")}" & vbNewLine & vbNewLine &
                "هل تريد المتابعة؟"

            Dim result As DialogResult = MessageBox.Show(
                warningMessage,
                "تأكيد الاستعادة - تحذير مهم",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2)

            If result = DialogResult.Yes Then
                ' تأكيد إضافي للأمان
                Dim finalConfirm As DialogResult = MessageBox.Show(
                    "هذا هو التأكيد الأخير." & vbNewLine &
                    "سيتم حذف جميع البيانات الحالية نهائياً." & vbNewLine & vbNewLine &
                    "هل أنت متأكد من المتابعة؟",
                    "التأكيد النهائي",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop,
                    MessageBoxDefaultButton.Button2)

                If finalConfirm = DialogResult.Yes Then
                    StartRestoreProcess()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في بدء الاستعادة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' التحقق من صحة إعدادات الاستعادة
    ''' </summary>
    Private Function ValidateRestoreSettings() As Boolean
        Try
            ' التحقق من اختيار ملف النسخة الاحتياطية
            If String.IsNullOrEmpty(selectedBackupFile) Then
                MessageBox.Show("يرجى اختيار ملف النسخة الاحتياطية للاستعادة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                lstAvailableBackups.Focus()
                Return False
            End If

            ' التحقق من وجود الملف
            If Not File.Exists(selectedBackupFile) Then
                MessageBox.Show("ملف النسخة الاحتياطية المحدد غير موجود.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                RefreshBackupsList()
                Return False
            End If

            ' التحقق من صحة ملف النسخة الاحتياطية
            Try
                Dim isValidBackup As Boolean = backupHelper.ValidateBackupFile(selectedBackupFile)
                If Not isValidBackup Then
                    MessageBox.Show("ملف النسخة الاحتياطية المحدد تالف أو غير صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Catch ex As Exception
                MessageBox.Show($"لا يمكن التحقق من صحة ملف النسخة الاحتياطية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try

            ' التحقق من الاتصال بقاعدة البيانات
            Try
                If Not backupHelper.TestDatabaseConnection() Then
                    MessageBox.Show("لا يمكن الاتصال بقاعدة البيانات.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Catch ex As Exception
                MessageBox.Show($"خطأ في الاتصال بقاعدة البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try

            Return True

        Catch ex As Exception
            MessageBox.Show($"خطأ في التحقق من الإعدادات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' بدء عملية الاستعادة
    ''' </summary>
    Private Sub StartRestoreProcess()
        Try
            If isRestoreInProgress Then
                MessageBox.Show("عملية استعادة جارية بالفعل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' حفظ الإعدادات
            SaveSettings()

            ' تحضير معاملات الاستعادة
            Dim restoreParams As New RestoreParameters() With {
                .BackupFilePath = selectedBackupFile,
                .CreateBackupBeforeRestore = chkCreateBackupBeforeRestore.Checked,
                .CloseConnectionsBeforeRestore = chkCloseConnectionsBeforeRestore.Checked,
                .RestoreDescription = txtRestoreDescription.Text.Trim()
            }

            ' تحديث واجهة المستخدم
            SetRestoreInProgress(True)

            ' بدء العملية في الخلفية
            restoreWorker.RunWorkerAsync(restoreParams)

        Catch ex As Exception
            SetRestoreInProgress(False)
            MessageBox.Show($"خطأ في بدء عملية الاستعادة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تحديث واجهة المستخدم أثناء الاستعادة
    ''' </summary>
    Private Sub SetRestoreInProgress(inProgress As Boolean)
        Try
            isRestoreInProgress = inProgress

            ' تفعيل/تعطيل العناصر
            btnStartRestore.Enabled = Not inProgress
            btnCancelRestore.Enabled = inProgress
            txtBackupPath.Enabled = Not inProgress
            btnBrowseBackupPath.Enabled = Not inProgress
            btnBrowseBackupFile.Enabled = Not inProgress
            lstAvailableBackups.Enabled = Not inProgress
            chkCreateBackupBeforeRestore.Enabled = Not inProgress
            chkCloseConnectionsBeforeRestore.Enabled = Not inProgress
            txtRestoreDescription.Enabled = Not inProgress

            If inProgress Then
                ' إعداد شريط التقدم
                progressBarRestore.Value = 0
                progressBarRestore.Visible = True
                lblRestoreStatus.Text = "جاري التحضير للاستعادة..."
                lblRestoreStatus.Visible = True

                ' تغيير شكل المؤشر
                Me.Cursor = Cursors.WaitCursor
            Else
                ' إخفاء شريط التقدم
                progressBarRestore.Visible = False
                lblRestoreStatus.Visible = False

                ' إعادة المؤشر العادي
                Me.Cursor = Cursors.Default

                ' إعادة تفعيل زر الاستعادة إذا كان هناك ملف محدد
                If Not String.IsNullOrEmpty(selectedBackupFile) AndAlso File.Exists(selectedBackupFile) Then
                    btnStartRestore.Enabled = True
                End If
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في SetRestoreInProgress: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالج حدث DoWork للـ BackgroundWorker
    ''' </summary>
    Private Sub RestoreWorker_DoWork(sender As Object, e As DoWorkEventArgs)
        Try
            Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
            Dim restoreParams As RestoreParameters = CType(e.Argument, RestoreParameters)

            ' تنفيذ الاستعادة
            Dim result As RestoreResult = backupHelper.RestoreDatabase(restoreParams, worker)

            ' إرجاع النتيجة
            e.Result = result

        Catch ex As Exception
            e.Result = New RestoreResult() With {
                .Success = False,
                .ErrorMessage = ex.Message,
                .BackupCreatedBeforeRestore = False
            }
        End Try
    End Sub

    ''' <summary>
    ''' معالج حدث ProgressChanged للـ BackgroundWorker
    ''' </summary>
    Private Sub RestoreWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs)
        Try
            ' تحديث شريط التقدم
            progressBarRestore.Value = Math.Min(e.ProgressPercentage, 100)

            ' تحديث رسالة الحالة
            If e.UserState IsNot Nothing Then
                lblRestoreStatus.Text = e.UserState.ToString()
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في RestoreWorker_ProgressChanged: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالج حدث RunWorkerCompleted للـ BackgroundWorker
    ''' </summary>
    Private Sub RestoreWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        Try
            ' تحديث واجهة المستخدم
            SetRestoreInProgress(False)

            If e.Cancelled Then
                MessageBox.Show("تم إلغاء عملية الاستعادة.", "إلغاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf e.Error IsNot Nothing Then
                MessageBox.Show($"حدث خطأ أثناء الاستعادة: {e.Error.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                ' الاستعادة مكتملة
                Dim result As RestoreResult = CType(e.Result, RestoreResult)

                If result.Success Then
                    Dim successMessage As String =
                        "تم استعادة قاعدة البيانات بنجاح!" & vbNewLine &
                        $"الوقت المستغرق: {result.Duration.ToString("hh\:mm\:ss")}" & vbNewLine

                    If result.BackupCreatedBeforeRestore Then
                        successMessage &= vbNewLine & "تم إنشاء نسخة احتياطية من البيانات السابقة قبل الاستعادة."
                    End If

                    successMessage &= vbNewLine & vbNewLine & "سيتم إعادة تشغيل التطبيق لتطبيق التغييرات."

                    MessageBox.Show(successMessage, "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' تحديث المعلومات
                    UpdateCurrentDatabaseInfo()

                    ' مسح وصف الاستعادة
                    txtRestoreDescription.Clear()

                    ' اقتراح إعادة تشغيل التطبيق
                    Dim restartResult As DialogResult = MessageBox.Show(
                        "يُنصح بإعادة تشغيل التطبيق لضمان عمل جميع الوظائف بشكل صحيح." & vbNewLine &
                        "هل تريد إعادة تشغيل التطبيق الآن؟",
                        "إعادة تشغيل التطبيق",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question)

                    If restartResult = DialogResult.Yes Then
                        Application.Restart()
                    End If

                Else
                    MessageBox.Show($"فشل في استعادة قاعدة البيانات: {result.ErrorMessage}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في معالجة نتيجة الاستعادة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelRestore_Click(sender As Object, e As EventArgs) Handles btnCancelRestore.Click
        Try
            If isRestoreInProgress AndAlso restoreWorker.IsBusy Then
                Dim result As DialogResult = MessageBox.Show(
                    "تحذير: إلغاء عملية الاستعادة قد يؤدي إلى تلف قاعدة البيانات!" & vbNewLine &
                    "هل تريد إلغاء العملية؟",
                    "تأكيد الإلغاء - تحذير",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2)

                If result = DialogResult.Yes Then
                    restoreWorker.CancelAsync()
                    lblRestoreStatus.Text = "جاري إلغاء العملية..."
                End If
            End If
        Catch ex As Exception
            MessageBox.Show($"خطأ في إلغاء الاستعادة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnRefreshBackupsList_Click(sender As Object, e As EventArgs) Handles btnRefreshBackupsList.Click
        RefreshBackupsList()
        lblSelectedBackupFile.ForeColor = SystemColors.ControlText
        lblSelectedBackupDate.ForeColor = SystemColors.ControlText
        lblSelectedBackupSize.ForeColor = SystemColors.ControlText
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            If isRestoreInProgress Then
                MessageBox.Show("لا يمكن إغلاق النموذج أثناء عملية الاستعادة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            SaveSettings()
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"خطأ في إغلاق النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RestoreForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If isRestoreInProgress Then
                e.Cancel = True
                MessageBox.Show("لا يمكن إغلاق النموذج أثناء عملية الاستعادة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في RestoreForm_FormClosing: {ex.Message}")
        End Try
    End Sub

End Class
