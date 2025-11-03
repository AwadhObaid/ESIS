Imports System.IO
Imports System.Data.SqlClient

''' <summary>
''' نموذج إدارة النسخ الاحتياطية - عرض وإدارة جميع النسخ الاحتياطية
''' </summary>
Public Class BackupManagerForm
    Inherits Form

    Private backupHelper As New DatabaseBackupHelper()
    Private currentBackupPath As String = ""

    Public Sub New()
        InitializeComponent()
        LoadSettings()
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
            currentBackupPath = defaultPath

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
            My.Settings.Save()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حفظ الإعدادات: {ex.Message}")
        End Try
    End Sub

    Private Sub BackupManagerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' إعداد النموذج
            Me.Text = "إدارة النسخ الاحتياطية - نظام ESIS"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            Me.StartPosition = FormStartPosition.CenterParent

            ' إعداد ListView
            SetupListView()

            ' تحديث معلومات قاعدة البيانات
            UpdateDatabaseInfo()

            ' تحديث قائمة النسخ الاحتياطية
            RefreshBackupsList()

            ' تعطيل أزرار الإجراءات في البداية
            btnDeleteBackup.Enabled = False
            btnRestoreBackup.Enabled = False
            btnViewBackupInfo.Enabled = False
            btnExportBackup.Enabled = False

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' إعداد ListView لعرض النسخ الاحتياطية
    ''' </summary>
    Private Sub SetupListView()
        Try
            lstBackups.View = View.Details
            lstBackups.FullRowSelect = True
            lstBackups.GridLines = True
            lstBackups.MultiSelect = True
            lstBackups.HideSelection = False

            ' إضافة الأعمدة
            lstBackups.Columns.Clear()
            lstBackups.Columns.Add("اسم الملف", 250)
            lstBackups.Columns.Add("الحجم", 100)
            lstBackups.Columns.Add("تاريخ الإنشاء", 150)
            lstBackups.Columns.Add("الوصف", 200)
            lstBackups.Columns.Add("النوع", 80)
            lstBackups.Columns.Add("المسار الكامل", 0) ' مخفي

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في SetupListView: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث معلومات قاعدة البيانات
    ''' </summary>
    Private Sub UpdateDatabaseInfo()
        Try
            Dim dbInfo As DatabaseInfo = backupHelper.GetDatabaseInfo()

            lblDatabaseName.Text = $"قاعدة البيانات: {dbInfo.DatabaseName}"
            lblDatabaseSize.Text = $"الحجم: {FormatFileSize(dbInfo.DatabaseSize)}"
            lblLastBackup.Text = $"آخر نسخة احتياطية: {If(dbInfo.LastBackupDate.HasValue, dbInfo.LastBackupDate.Value.ToString("yyyy/MM/dd HH:mm"), "لا توجد")}"

        Catch ex As Exception
            lblDatabaseName.Text = "خطأ في جلب معلومات قاعدة البيانات"
            lblDatabaseSize.Text = ""
            lblLastBackup.Text = ""
            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateDatabaseInfo: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث قائمة النسخ الاحتياطية
    ''' </summary>
    Private Sub RefreshBackupsList()
        Try
            lstBackups.Items.Clear()
            ClearSelection()

            currentBackupPath = txtBackupPath.Text.Trim()

            If Directory.Exists(currentBackupPath) Then
                ' جلب ملفات .bak
                Dim bakFiles() As String = Directory.GetFiles(currentBackupPath, "*.bak")
                ' جلب ملفات .zip
                Dim zipFiles() As String = Directory.GetFiles(currentBackupPath, "*.zip")

                ' دمج الملفات وترتيبها حسب تاريخ الإنشاء
                Dim allFiles() As String = bakFiles.Concat(zipFiles).OrderByDescending(Function(f) File.GetCreationTime(f)).ToArray()

                For Each file As String In allFiles
                    AddBackupFileToList(file)
                Next

                ' تحديث الإحصائيات
                UpdateStatistics(allFiles.Length, allFiles.Sum(Function(f) New FileInfo(f).Length))
            Else
                UpdateStatistics(0, 0)
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحديث قائمة النسخ الاحتياطية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' إضافة ملف نسخة احتياطية إلى القائمة
    ''' </summary>
    Private Sub AddBackupFileToList(filePath As String)
        Try
            Dim fileInfo As New FileInfo(filePath)
            Dim backupInfo As BackupFileInfo = backupHelper.GetBackupFileInfo(filePath)

            Dim item As New ListViewItem(fileInfo.Name)
            item.SubItems.Add(FormatFileSize(fileInfo.Length))
            item.SubItems.Add(fileInfo.CreationTime.ToString("yyyy/MM/dd HH:mm"))
            item.SubItems.Add(If(backupInfo?.Description, "لا يوجد وصف"))
            item.SubItems.Add(If(fileInfo.Extension.ToLower() = ".zip", "مضغوط", "عادي"))
            item.SubItems.Add(filePath) ' المسار الكامل

            ' تلوين الصفوف حسب العمر
            Dim daysSinceCreation As Integer = (DateTime.Now - fileInfo.CreationTime).Days
            If daysSinceCreation > 30 Then
                item.BackColor = Color.LightGray
                item.ForeColor = Color.DarkGray
            ElseIf daysSinceCreation > 7 Then
                item.BackColor = Color.LightYellow
            End If

            lstBackups.Items.Add(item)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في AddBackupFileToList: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث الإحصائيات
    ''' </summary>
    Private Sub UpdateStatistics(fileCount As Integer, totalSize As Long)
        Try
            lblBackupCount.Text = $"عدد النسخ الاحتياطية: {fileCount}"
            lblTotalSize.Text = $"الحجم الإجمالي: {FormatFileSize(totalSize)}"

            ' حساب المساحة المتاحة
            If Directory.Exists(currentBackupPath) Then
                Dim driveInfo As New DriveInfo(Path.GetPathRoot(currentBackupPath))
                lblAvailableSpace.Text = $"المساحة المتاحة: {FormatFileSize(driveInfo.AvailableFreeSpace)}"
            Else
                lblAvailableSpace.Text = "المساحة المتاحة: غير معروف"
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateStatistics: {ex.Message}")
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

    ''' <summary>
    ''' مسح التحديد وتعطيل الأزرار
    ''' </summary>
    Private Sub ClearSelection()
        Try
            btnDeleteBackup.Enabled = False
            btnRestoreBackup.Enabled = False
            btnViewBackupInfo.Enabled = False
            btnExportBackup.Enabled = False

            lblSelectedBackup.Text = "لم يتم اختيار نسخة احتياطية"

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في ClearSelection: {ex.Message}")
        End Try
    End Sub

    Private Sub txtBackupPath_TextChanged(sender As Object, e As EventArgs) Handles txtBackupPath.TextChanged
        ' تأخير التحديث لتجنب التحديث المستمر أثناء الكتابة
        timerRefresh.Stop()
        timerRefresh.Start()
    End Sub

    Private Sub timerRefresh_Tick(sender As Object, e As EventArgs) Handles timerRefresh.Tick
        timerRefresh.Stop()
        RefreshBackupsList()
    End Sub

    Private Sub btnBrowsePath_Click(sender As Object, e As EventArgs) Handles btnBrowsePath.Click
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

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        RefreshBackupsList()
        UpdateDatabaseInfo()
    End Sub

    Private Sub lstBackups_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstBackups.SelectedIndexChanged
        Try
            If lstBackups.SelectedItems.Count > 0 Then
                Dim selectedItem As ListViewItem = lstBackups.SelectedItems(0)
                Dim fileName As String = selectedItem.Text

                lblSelectedBackup.Text = $"المحدد: {fileName}"

                ' تفعيل الأزرار
                btnDeleteBackup.Enabled = True
                btnRestoreBackup.Enabled = True
                btnViewBackupInfo.Enabled = True
                btnExportBackup.Enabled = True
            Else
                ClearSelection()
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في lstBackups_SelectedIndexChanged: {ex.Message}")
        End Try
    End Sub

    Private Sub lstBackups_DoubleClick(sender As Object, e As EventArgs) Handles lstBackups.DoubleClick
        btnViewBackupInfo_Click(sender, e)
    End Sub

    Private Sub btnCreateBackup_Click(sender As Object, e As EventArgs) Handles btnCreateBackup.Click
        Try
            ' فتح نموذج النسخ الاحتياطي
            Using backupForm As New BackupForm()
                If backupForm.ShowDialog() = DialogResult.OK Then
                    ' تحديث القائمة بعد إنشاء نسخة احتياطية جديدة
                    RefreshBackupsList()
                    UpdateDatabaseInfo()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"خطأ في فتح نموذج النسخ الاحتياطي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnRestoreBackup_Click(sender As Object, e As EventArgs) Handles btnRestoreBackup.Click
        Try
            If lstBackups.SelectedItems.Count = 0 Then
                MessageBox.Show("يرجى اختيار نسخة احتياطية للاستعادة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim selectedItem As ListViewItem = lstBackups.SelectedItems(0)
            Dim backupFilePath As String = selectedItem.SubItems(5).Text

            ' فتح نموذج الاستعادة مع تمرير مسار الملف
            Using restoreForm As New RestoreForm()
                ' يمكن إضافة خاصية لتمرير مسار الملف المحدد
                If restoreForm.ShowDialog() = DialogResult.OK Then
                    ' تحديث المعلومات بعد الاستعادة
                    UpdateDatabaseInfo()
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show($"خطأ في فتح نموذج الاستعادة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDeleteBackup_Click(sender As Object, e As EventArgs) Handles btnDeleteBackup.Click
        Try
            If lstBackups.SelectedItems.Count = 0 Then
                MessageBox.Show("يرجى اختيار نسخة احتياطية للحذف.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim selectedItems As ListView.SelectedListViewItemCollection = lstBackups.SelectedItems
            Dim fileCount As Integer = selectedItems.Count
            Dim filesText As String = If(fileCount = 1, "الملف المحدد", $"{fileCount} ملفات")

            Dim result As DialogResult = MessageBox.Show(
                $"هل تريد حذف {filesText} نهائياً؟" & vbNewLine &
                "تحذير: هذه العملية لا يمكن التراجع عنها!",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2)

            If result = DialogResult.Yes Then
                Dim deletedCount As Integer = 0
                Dim errors As New List(Of String)

                For Each item As ListViewItem In selectedItems
                    Try
                        Dim filePath As String = item.SubItems(5).Text
                        If File.Exists(filePath) Then
                            File.Delete(filePath)
                            deletedCount += 1
                        End If
                    Catch ex As Exception
                        errors.Add($"{item.Text}: {ex.Message}")
                    End Try
                Next

                ' عرض النتائج
                If errors.Count = 0 Then
                    MessageBox.Show($"تم حذف {deletedCount} ملف بنجاح.", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Dim errorMessage As String = $"تم حذف {deletedCount} ملف." & vbNewLine & vbNewLine &
                                               "الأخطاء:" & vbNewLine & String.Join(vbNewLine, errors)
                    MessageBox.Show(errorMessage, "حذف مع أخطاء", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

                ' تحديث القائمة
                RefreshBackupsList()
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في حذف النسخ الاحتياطية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnViewBackupInfo_Click(sender As Object, e As EventArgs) Handles btnViewBackupInfo.Click
        Try
            If lstBackups.SelectedItems.Count = 0 Then
                MessageBox.Show("يرجى اختيار نسخة احتياطية لعرض معلوماتها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim selectedItem As ListViewItem = lstBackups.SelectedItems(0)
            Dim backupFilePath As String = selectedItem.SubItems(5).Text

            ShowBackupDetails(backupFilePath)

        Catch ex As Exception
            MessageBox.Show($"خطأ في عرض معلومات النسخة الاحتياطية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' عرض تفاصيل النسخة الاحتياطية
    ''' </summary>
    Private Sub ShowBackupDetails(backupFilePath As String)
        Try
            If Not File.Exists(backupFilePath) Then
                MessageBox.Show("الملف المحدد غير موجود.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim fileInfo As New FileInfo(backupFilePath)
            Dim backupInfo As BackupFileInfo = backupHelper.GetBackupFileInfo(backupFilePath)

            Dim details As String =
                $"اسم الملف: {fileInfo.Name}" & vbNewLine &
                $"المسار: {fileInfo.DirectoryName}" & vbNewLine &
                $"الحجم: {FormatFileSize(fileInfo.Length)}" & vbNewLine &
                $"تاريخ الإنشاء: {fileInfo.CreationTime:yyyy/MM/dd HH:mm:ss}" & vbNewLine &
                $"تاريخ التعديل: {fileInfo.LastWriteTime:yyyy/MM/dd HH:mm:ss}" & vbNewLine &
                $"النوع: {If(fileInfo.Extension.ToLower() = ".zip", "مضغوط", "عادي")}" & vbNewLine &
                $"الوصف: {If(backupInfo?.Description, "لا يوجد وصف")}" & vbNewLine & vbNewLine

            ' معلومات إضافية
            If backupHelper.ValidateBackupFile(backupFilePath) Then
                details &= "حالة الملف: صالح للاستعادة" & vbNewLine
            Else
                details &= "حالة الملف: تالف أو غير صالح" & vbNewLine
            End If

            MessageBox.Show(details, $"تفاصيل النسخة الاحتياطية - {fileInfo.Name}", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show($"خطأ في عرض التفاصيل: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExportBackup_Click(sender As Object, e As EventArgs) Handles btnExportBackup.Click
        Try
            If lstBackups.SelectedItems.Count = 0 Then
                MessageBox.Show("يرجى اختيار نسخة احتياطية للتصدير.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim selectedItem As ListViewItem = lstBackups.SelectedItems(0)
            Dim sourceFilePath As String = selectedItem.SubItems(5).Text

            Using saveDialog As New SaveFileDialog()
                saveDialog.Title = "تصدير النسخة الاحتياطية"
                saveDialog.FileName = Path.GetFileName(sourceFilePath)
                saveDialog.Filter = "ملفات النسخ الاحتياطية (*.bak;*.zip)|*.bak;*.zip|جميع الملفات (*.*)|*.*"

                If saveDialog.ShowDialog() = DialogResult.OK Then
                    File.Copy(sourceFilePath, saveDialog.FileName, True)
                    MessageBox.Show("تم تصدير النسخة الاحتياطية بنجاح.", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show($"خطأ في تصدير النسخة الاحتياطية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCleanOldBackups_Click(sender As Object, e As EventArgs) Handles btnCleanOldBackups.Click
        Try
            ' نموذج لاختيار عدد الأيام
            Dim daysInput As String = InputBox("كم يوماً تريد الاحتفاظ بالنسخ الاحتياطية؟" & vbNewLine & "(سيتم حذف النسخ الأقدم من هذا التاريخ)", "تنظيف النسخ القديمة", "30")

            If String.IsNullOrEmpty(daysInput) Then
                Return
            End If

            Dim days As Integer
            If Not Integer.TryParse(daysInput, days) OrElse days < 1 Then
                MessageBox.Show("يرجى إدخال رقم صحيح أكبر من 0.", "خطأ في الإدخال", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim cutoffDate As DateTime = DateTime.Now.AddDays(-days)
            Dim filesToDelete As New List(Of String)

            ' البحث عن الملفات القديمة
            If Directory.Exists(currentBackupPath) Then
                Dim allFiles() As String = Directory.GetFiles(currentBackupPath, "*.bak").Concat(Directory.GetFiles(currentBackupPath, "*.zip")).ToArray()

                For Each file As String In allFiles
                    Dim fileInfo As New FileInfo(file)
                    If fileInfo.CreationTime < cutoffDate Then
                        filesToDelete.Add(file)
                    End If
                Next
            End If

            If filesToDelete.Count = 0 Then
                MessageBox.Show($"لا توجد نسخ احتياطية أقدم من {days} يوم.", "لا توجد ملفات للحذف", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim result As DialogResult = MessageBox.Show(
                $"تم العثور على {filesToDelete.Count} نسخة احتياطية أقدم من {days} يوم." & vbNewLine &
                "هل تريد حذفها نهائياً؟",
                "تأكيد التنظيف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2)

            If result = DialogResult.Yes Then
                Dim deletedCount As Integer = 0
                Dim errors As New List(Of String)

                For Each file As String In filesToDelete
                    Try
                        System.IO.File.Delete(file)
                        deletedCount += 1
                    Catch ex As Exception
                        errors.Add($"{Path.GetFileName(file)}: {ex.Message}")
                    End Try
                Next

                ' عرض النتائج
                If errors.Count = 0 Then
                    MessageBox.Show($"تم حذف {deletedCount} نسخة احتياطية قديمة بنجاح.", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Dim errorMessage As String = $"تم حذف {deletedCount} نسخة احتياطية." & vbNewLine & vbNewLine &
                                               "الأخطاء:" & vbNewLine & String.Join(vbNewLine, errors.Take(10))
                    If errors.Count > 10 Then
                        errorMessage &= vbNewLine & $"... و {errors.Count - 10} أخطاء أخرى"
                    End If
                    MessageBox.Show(errorMessage, "تنظيف مع أخطاء", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

                ' تحديث القائمة
                RefreshBackupsList()
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في تنظيف النسخ القديمة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            SaveSettings()
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"خطأ في إغلاق النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BackupManagerForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        SaveSettings()
    End Sub

End Class