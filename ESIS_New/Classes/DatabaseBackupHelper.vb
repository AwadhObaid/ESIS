Imports System.Data.SqlClient
Imports System.IO
Imports System.ComponentModel
Imports System.IO.Compression
Imports System.Configuration

''' <summary>
''' كلاس مساعد لعمليات النسخ الاحتياطي والاستعادة لقاعدة البيانات
''' </summary>
Public Class DatabaseBackupHelper

    ''' <summary>
    ''' الحصول على معلومات قاعدة البيانات
    ''' </summary>
    Public Function GetDatabaseInfo() As DatabaseInfo
        Try
            Dim dbInfo As New DatabaseInfo()

            Using connection As New SqlConnection(GetConnectionString())
                connection.Open()

                ' الحصول على اسم قاعدة البيانات
                Using cmd As New SqlCommand("SELECT DB_NAME()", connection)
                    dbInfo.DatabaseName = cmd.ExecuteScalar()?.ToString()
                End Using

                ' الحصول على حجم قاعدة البيانات
                Dim sizeQuery As String = "
                    SELECT 
                        SUM(CAST(FILEPROPERTY(name, 'SpaceUsed') AS bigint) * 8192) as DatabaseSize
                    FROM sys.database_files 
                    WHERE type IN (0,1)"

                Using cmd As New SqlCommand(sizeQuery, connection)
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        dbInfo.DatabaseSize = Convert.ToInt64(result)
                    End If
                End Using

                ' الحصول على تاريخ آخر نسخة احتياطية
                Dim lastBackupQuery As String = "
                    SELECT TOP 1 backup_finish_date 
                    FROM msdb.dbo.backupset 
                    WHERE database_name = DB_NAME() 
                    AND type = 'D' 
                    ORDER BY backup_finish_date DESC"

                Using cmd As New SqlCommand(lastBackupQuery, connection)
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        dbInfo.LastBackupDate = Convert.ToDateTime(result)
                    End If
                End Using
            End Using

            Return dbInfo

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetDatabaseInfo: {ex.Message}")
            Return New DatabaseInfo() With {
                .DatabaseName = "غير معروف",
                .DatabaseSize = 0,
                .LastBackupDate = Nothing
            }
        End Try
    End Function

    ''' <summary>
    ''' إنشاء نسخة احتياطية من قاعدة البيانات
    ''' </summary>
    Public Function CreateBackup(backupParams As BackupParameters, worker As BackgroundWorker) As BackupResult
        Dim result As New BackupResult()
        Dim startTime As DateTime = DateTime.Now

        Try
            worker?.ReportProgress(0, "بدء عملية النسخ الاحتياطي...")

            ' التحقق من المعاملات
            If Not ValidateBackupParameters(backupParams) Then
                result.Success = False
                result.ErrorMessage = "معاملات النسخ الاحتياطي غير صحيحة"
                Return result
            End If

            ' إنشاء اسم الملف
            Dim fileName As String = GenerateBackupFileName(backupParams.BackupDescription)
            Dim backupFilePath As String = Path.Combine(backupParams.BackupPath, fileName)

            worker?.ReportProgress(10, "إنشاء ملف النسخة الاحتياطية...")

            ' حذف النسخ القديمة إذا كان مطلوباً
            If backupParams.AutoDeleteOldBackups Then
                worker?.ReportProgress(15, "حذف النسخ الاحتياطية القديمة...")
                DeleteOldBackups(backupParams.BackupPath, backupParams.KeepBackupsForDays)
            End If

            ' إنشاء النسخة الاحتياطية
            worker?.ReportProgress(20, "جاري إنشاء النسخة الاحتياطية من قاعدة البيانات...")
            CreateDatabaseBackup(backupFilePath, backupParams, worker)

            ' ضغط الملف إذا كان مطلوباً
            If backupParams.CompressBackup Then
                worker?.ReportProgress(80, "جاري ضغط ملف النسخة الاحتياطية...")
                backupFilePath = CompressBackupFile(backupFilePath)
            End If

            ' نسخ ملفات الأرشيف إذا كان مطلوباً
            If backupParams.IncludeArchiveFiles Then
                worker?.ReportProgress(90, "جاري نسخ ملفات الأرشيف...")
                CopyArchiveFiles(backupParams.BackupPath, Path.GetFileNameWithoutExtension(backupFilePath))
            End If

            ' حساب النتائج النهائية
            worker?.ReportProgress(95, "إنهاء العملية...")

            If File.Exists(backupFilePath) Then
                Dim fileInfo As New FileInfo(backupFilePath)
                result.Success = True
                result.BackupFilePath = backupFilePath
                result.BackupFileSize = fileInfo.Length
                result.Duration = DateTime.Now - startTime

                ' حفظ معلومات النسخة الاحتياطية في قاعدة البيانات
                SaveBackupInfo(backupFilePath, backupParams.BackupDescription)
            Else
                result.Success = False
                result.ErrorMessage = "فشل في إنشاء ملف النسخة الاحتياطية"
            End If

            worker?.ReportProgress(100, "تم إنشاء النسخة الاحتياطية بنجاح")

        Catch ex As Exception
            result.Success = False
            result.ErrorMessage = ex.Message
            System.Diagnostics.Debug.WriteLine($"خطأ في CreateBackup: {ex.Message}")
        End Try

        Return result
    End Function

    ''' <summary>
    ''' استعادة قاعدة البيانات من نسخة احتياطية
    ''' </summary>
    Public Function RestoreDatabase(restoreParams As RestoreParameters, worker As BackgroundWorker) As RestoreResult
        Dim result As New RestoreResult()
        Dim startTime As DateTime = DateTime.Now

        Try
            worker?.ReportProgress(0, "بدء عملية الاستعادة...")

            ' التحقق من المعاملات
            If Not ValidateRestoreParameters(restoreParams) Then
                result.Success = False
                result.ErrorMessage = "معاملات الاستعادة غير صحيحة"
                Return result
            End If

            ' إنشاء نسخة احتياطية قبل الاستعادة إذا كان مطلوباً
            If restoreParams.CreateBackupBeforeRestore Then
                worker?.ReportProgress(10, "إنشاء نسخة احتياطية قبل الاستعادة...")

                Dim preRestoreBackupParams As New BackupParameters() With {
                    .BackupPath = Path.GetDirectoryName(restoreParams.BackupFilePath),
                    .BackupDescription = "نسخة احتياطية تلقائية قبل الاستعادة",
                    .CompressBackup = True,
                    .IncludeArchiveFiles = False,
                    .AutoDeleteOldBackups = False
                }

                Dim preBackupResult As BackupResult = CreateBackup(preRestoreBackupParams, Nothing)
                result.BackupCreatedBeforeRestore = preBackupResult.Success

                If Not preBackupResult.Success Then
                    result.Success = False
                    result.ErrorMessage = $"فشل في إنشاء النسخة الاحتياطية قبل الاستعادة: {preBackupResult.ErrorMessage}"
                    Return result
                End If
            End If

            ' إغلاق الاتصالات إذا كان مطلوباً
            If restoreParams.CloseConnectionsBeforeRestore Then
                worker?.ReportProgress(30, "إغلاق الاتصالات النشطة...")
                CloseActiveConnections()
            End If

            ' فك ضغط الملف إذا كان مضغوطاً
            Dim actualBackupFile As String = restoreParams.BackupFilePath
            If Path.GetExtension(restoreParams.BackupFilePath).ToLower() = ".zip" Then
                worker?.ReportProgress(40, "فك ضغط ملف النسخة الاحتياطية...")
                actualBackupFile = DecompressBackupFile(restoreParams.BackupFilePath)
            End If

            ' تنفيذ الاستعادة
            worker?.ReportProgress(50, "جاري استعادة قاعدة البيانات...")
            RestoreDatabaseFromFile(actualBackupFile, worker)

            ' تنظيف الملفات المؤقتة
            If actualBackupFile <> restoreParams.BackupFilePath Then
                Try
                    File.Delete(actualBackupFile)
                Catch
                    ' تجاهل أخطاء حذف الملفات المؤقتة
                End Try
            End If

            worker?.ReportProgress(95, "إنهاء العملية...")

            result.Success = True
            result.Duration = DateTime.Now - startTime

            ' حفظ معلومات الاستعادة
            SaveRestoreInfo(restoreParams.BackupFilePath, restoreParams.RestoreDescription)

            worker?.ReportProgress(100, "تم استعادة قاعدة البيانات بنجاح")

        Catch ex As Exception
            result.Success = False
            result.ErrorMessage = ex.Message
            System.Diagnostics.Debug.WriteLine($"خطأ في RestoreDatabase: {ex.Message}")
        End Try

        Return result
    End Function

    ''' <summary>
    ''' التحقق من صحة ملف النسخة الاحتياطية
    ''' </summary>
    Public Function ValidateBackupFile(backupFilePath As String) As Boolean
        Try
            If Not File.Exists(backupFilePath) Then
                Return False
            End If

            ' فحص امتداد الملف
            Dim extension As String = Path.GetExtension(backupFilePath).ToLower()
            If extension <> ".bak" AndAlso extension <> ".zip" Then
                Return False
            End If

            ' فحص حجم الملف (يجب أن يكون أكبر من 0)
            Dim fileInfo As New FileInfo(backupFilePath)
            If fileInfo.Length = 0 Then
                Return False
            End If

            ' فحص إضافي للملفات المضغوطة
            If extension = ".zip" Then
                Try
                    Using archive As ZipArchive = ZipFile.OpenRead(backupFilePath)
                        ' التحقق من وجود ملف .bak داخل الأرشيف
                        Dim bakEntry = archive.Entries.FirstOrDefault(Function(e) e.Name.EndsWith(".bak", StringComparison.OrdinalIgnoreCase))
                        Return bakEntry IsNot Nothing
                    End Using
                Catch
                    Return False
                End Try
            End If

            ' فحص بسيط لملفات .bak
            If extension = ".bak" Then
                Try
                    ' قراءة أول بضعة بايتات للتحقق من التوقيع
                    Using fs As New FileStream(backupFilePath, FileMode.Open, FileAccess.Read)
                        If fs.Length < 100 Then
                            Return False
                        End If

                        Dim buffer(99) As Byte
                        fs.Read(buffer, 0, 100)

                        ' البحث عن توقيع SQL Server backup
                        Dim signature As String = System.Text.Encoding.ASCII.GetString(buffer)
                        Return signature.Contains("TAPE") OrElse signature.Contains("DISK")
                    End Using
                Catch
                    Return False
                End Try
            End If

            Return True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في ValidateBackupFile: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' اختبار الاتصال بقاعدة البيانات
    ''' </summary>
    Public Function TestDatabaseConnection() As Boolean
        Try
            Using connection As New SqlConnection(GetConnectionString())
                connection.Open()
                Return connection.State = ConnectionState.Open
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في TestDatabaseConnection: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' الحصول على معلومات ملف النسخة الاحتياطية
    ''' </summary>
    Public Function GetBackupFileInfo(backupFilePath As String) As BackupFileInfo
        Try
            Dim info As New BackupFileInfo()

            If Not File.Exists(backupFilePath) Then
                Return Nothing
            End If

            Dim fileInfo As New FileInfo(backupFilePath)
            info.FileName = fileInfo.Name
            info.FilePath = backupFilePath
            info.FileSize = fileInfo.Length
            info.CreationDate = fileInfo.CreationTime

            ' محاولة قراءة الوصف من قاعدة البيانات
            Try
                Using connection As New SqlConnection(GetConnectionString())
                    connection.Open()

                    Dim query As String = "
                        SELECT TOP 1 description 
                        FROM msdb.dbo.backupset 
                        WHERE physical_device_name LIKE '%' + @fileName + '%' 
                        ORDER BY backup_finish_date DESC"

                    Using cmd As New SqlCommand(query, connection)
                        cmd.Parameters.AddWithValue("@fileName", fileInfo.Name)
                        Dim result As Object = cmd.ExecuteScalar()
                        If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                            info.Description = result.ToString()
                        End If
                    End Using
                End Using
            Catch
                ' تجاهل أخطاء قراءة الوصف
                info.Description = "لا يمكن قراءة الوصف"
            End Try

            Return info

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetBackupFileInfo: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' التحقق من صحة معاملات النسخ الاحتياطي
    ''' </summary>
    Private Function ValidateBackupParameters(backupParams As BackupParameters) As Boolean
        Try
            If backupParams Is Nothing Then
                Return False
            End If

            If String.IsNullOrEmpty(backupParams.BackupPath) Then
                Return False
            End If

            If Not Directory.Exists(backupParams.BackupPath) Then
                Directory.CreateDirectory(backupParams.BackupPath)
            End If

            Return True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في ValidateBackupParameters: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق من صحة معاملات الاستعادة
    ''' </summary>
    Private Function ValidateRestoreParameters(restoreParams As RestoreParameters) As Boolean
        Try
            If restoreParams Is Nothing Then
                Return False
            End If

            If String.IsNullOrEmpty(restoreParams.BackupFilePath) Then
                Return False
            End If

            If Not File.Exists(restoreParams.BackupFilePath) Then
                Return False
            End If

            Return ValidateBackupFile(restoreParams.BackupFilePath)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في ValidateRestoreParameters: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' توليد اسم ملف النسخة الاحتياطية
    ''' </summary>
    Private Function GenerateBackupFileName(description As String) As String
        Try
            Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
            Dim dbName As String = GetDatabaseName()

            Dim fileName As String = $"ESIS_{dbName}_{timestamp}"

            If Not String.IsNullOrEmpty(description) Then
                ' تنظيف الوصف من الأحرف غير المسموحة
                Dim cleanDescription As String = String.Join("_", description.Split(Path.GetInvalidFileNameChars()))
                If cleanDescription.Length > 50 Then
                    cleanDescription = cleanDescription.Substring(0, 50)
                End If
                fileName &= $"_{cleanDescription}"
            End If

            Return $"{fileName}.bak"

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GenerateBackupFileName: {ex.Message}")
            Return $"ESIS_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak"
        End Try
    End Function


    ''' <summary>
    ''' التحقق من دعم خيار الضغط في SQL Server
    ''' </summary>
    Public Function IsCompressionSupported() As Boolean
        Try
            Using connection As New SqlConnection(GetConnectionString())
                connection.Open()
                Dim query As String = "SELECT SERVERPROPERTY('Edition') AS Edition"
                Using cmd As New SqlCommand(query, connection)
                    Dim edition As String = cmd.ExecuteScalar()?.ToString()
                    Return Not edition.Contains("Express")
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من دعم الضغط: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' إنشاء النسخة الاحتياطية من قاعدة البيانات
    ''' </summary>


    Private Sub CreateDatabaseBackup(backupFilePath As String, backupParams As BackupParameters, worker As BackgroundWorker)
        Try
            Using connection As New SqlConnection(GetConnectionString())
                connection.Open()
                Dim dbName As String = GetDatabaseName()
                Dim backupQuery As String = $"
                BACKUP DATABASE [{dbName}] 
                TO DISK = @backupPath 
                WITH FORMAT, INIT, 
                NAME = @backupName, 
                DESCRIPTION = @description"

                ' إضافة خيار الضغط إذا كان مدعومًا وممكّنًا
                If backupParams.CompressBackup AndAlso IsCompressionSupported() Then
                    backupQuery &= ", COMPRESSION"
                End If

                Using cmd As New SqlCommand(backupQuery, connection)
                    cmd.CommandTimeout = 0 ' لا حد زمني للنسخ الاحتياطي
                    cmd.Parameters.AddWithValue("@backupPath", backupFilePath)
                    cmd.Parameters.AddWithValue("@backupName", $"ESIS Full Backup - {DateTime.Now:yyyy/MM/dd HH:mm}")
                    cmd.Parameters.AddWithValue("@description", backupParams.BackupDescription)

                    ' تنفيذ النسخ الاحتياطي مع تقرير التقدم
                    Dim startTime As DateTime = DateTime.Now
                    cmd.ExecuteNonQuery()

                    ' تحديث التقدم تدريجياً
                    For i As Integer = 30 To 75 Step 5
                        If worker?.CancellationPending Then
                            Exit For
                        End If
                        worker?.ReportProgress(i, "جاري إنشاء النسخة الاحتياطية...")
                        Threading.Thread.Sleep(500)
                    Next
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception($"فشل في إنشاء النسخة الاحتياطية: {ex.Message}")
        End Try
    End Sub


    'Private Sub CreateDatabaseBackup(backupFilePath As String, backupParams As BackupParameters, worker As BackgroundWorker)
    '    Try
    '        Using connection As New SqlConnection(GetConnectionString())
    '            connection.Open()

    '            Dim dbName As String = GetDatabaseName()
    '            Dim backupQuery As String = $"
    '                BACKUP DATABASE [{dbName}] 
    '                TO DISK = @backupPath 
    '                WITH FORMAT, INIT, 
    '                NAME = @backupName, 
    '                DESCRIPTION = @description,
    '                COMPRESSION"

    '            Using cmd As New SqlCommand(backupQuery, connection)
    '                cmd.CommandTimeout = 0 ' لا حد زمني للنسخ الاحتياطي
    '                cmd.Parameters.AddWithValue("@backupPath", backupFilePath)
    '                cmd.Parameters.AddWithValue("@backupName", $"ESIS Full Backup - {DateTime.Now:yyyy/MM/dd HH:mm}")
    '                cmd.Parameters.AddWithValue("@description", backupParams.BackupDescription)

    '                ' تنفيذ النسخ الاحتياطي مع تقرير التقدم
    '                Dim startTime As DateTime = DateTime.Now
    '                cmd.ExecuteNonQuery()

    '                ' تحديث التقدم تدريجياً
    '                For i As Integer = 30 To 75 Step 5
    '                    If worker?.CancellationPending Then
    '                        Exit For
    '                    End If
    '                    worker?.ReportProgress(i, "جاري إنشاء النسخة الاحتياطية...")
    '                    Threading.Thread.Sleep(500)
    '                Next
    '            End Using
    '        End Using

    '    Catch ex As Exception
    '        Throw New Exception($"فشل في إنشاء النسخة الاحتياطية: {ex.Message}")
    '    End Try
    'End Sub

    ''' <summary>
    ''' استعادة قاعدة البيانات من ملف
    ''' </summary>
    Private Sub RestoreDatabaseFromFile(backupFilePath As String, worker As BackgroundWorker)
        Try
            Using connection As New SqlConnection(GetMasterConnectionString())
                connection.Open()

                Dim dbName As String = GetDatabaseName()

                ' إغلاق الاتصالات النشطة
                Dim killConnectionsQuery As String = $"
                    ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE"

                Using cmd As New SqlCommand(killConnectionsQuery, connection)
                    cmd.ExecuteNonQuery()
                End Using

                worker?.ReportProgress(60, "جاري استعادة البيانات...")

                ' تنفيذ الاستعادة
                Dim restoreQuery As String = $"
                    RESTORE DATABASE [{dbName}] 
                    FROM DISK = @backupPath 
                    WITH REPLACE, RECOVERY"

                Using cmd As New SqlCommand(restoreQuery, connection)
                    cmd.CommandTimeout = 0 ' لا حد زمني للاستعادة
                    cmd.Parameters.AddWithValue("@backupPath", backupFilePath)
                    cmd.ExecuteNonQuery()
                End Using

                worker?.ReportProgress(85, "إعادة تفعيل قاعدة البيانات...")

                ' إعادة تفعيل الوصول المتعدد
                Dim enableMultiUserQuery As String = $"
                    ALTER DATABASE [{dbName}] SET MULTI_USER"

                Using cmd As New SqlCommand(enableMultiUserQuery, connection)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception($"فشل في استعادة قاعدة البيانات: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' ضغط ملف النسخة الاحتياطية
    ''' </summary>
    Private Function CompressBackupFile(backupFilePath As String) As String
        Try
            Dim compressedFilePath As String = Path.ChangeExtension(backupFilePath, ".zip")

            Using archive As ZipArchive = ZipFile.Open(compressedFilePath, ZipArchiveMode.Create)
                archive.CreateEntryFromFile(backupFilePath, Path.GetFileName(backupFilePath))
            End Using

            ' حذف الملف الأصلي غير المضغوط
            File.Delete(backupFilePath)

            Return compressedFilePath

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في ضغط الملف: {ex.Message}")
            Return backupFilePath ' إرجاع الملف الأصلي في حالة فشل الضغط
        End Try
    End Function

    ''' <summary>
    ''' فك ضغط ملف النسخة الاحتياطية
    ''' </summary>
    Private Function DecompressBackupFile(compressedFilePath As String) As String
        Try
            Dim tempDir As String = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())
            Directory.CreateDirectory(tempDir)

            Using archive As ZipArchive = ZipFile.OpenRead(compressedFilePath)
                Dim bakEntry = archive.Entries.FirstOrDefault(Function(e) e.Name.EndsWith(".bak", StringComparison.OrdinalIgnoreCase))
                If bakEntry IsNot Nothing Then
                    Dim extractedPath As String = Path.Combine(tempDir, bakEntry.Name)
                    bakEntry.ExtractToFile(extractedPath)
                    Return extractedPath
                End If
            End Using

            Throw New Exception("لم يتم العثور على ملف .bak داخل الأرشيف المضغوط")

        Catch ex As Exception
            Throw New Exception($"فشل في فك ضغط الملف: {ex.Message}")
        End Try
    End Function

    ''' <summary>
    ''' نسخ ملفات الأرشيف
    ''' </summary>
    Private Sub CopyArchiveFiles(backupPath As String, backupName As String)
        Try
            ' تحديد مجلد الأرشيف (يمكن تخصيصه حسب النظام)
            Dim archiveSourcePath As String = "C:\ESIS_Archive" ' مسار افتراضي

            If Directory.Exists(archiveSourcePath) Then
                Dim archiveBackupPath As String = Path.Combine(backupPath, $"{backupName}_Archive")
                Directory.CreateDirectory(archiveBackupPath)

                ' نسخ جميع الملفات
                For Each file As String In Directory.GetFiles(archiveSourcePath, "*.*", SearchOption.AllDirectories)
                    Dim relativePath As String = file.Substring(archiveSourcePath.Length + 1)
                    Dim destPath As String = Path.Combine(archiveBackupPath, relativePath)

                    Directory.CreateDirectory(Path.GetDirectoryName(destPath))
                    System.IO.File.Copy(file, destPath, True)
                Next
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في نسخ ملفات الأرشيف: {ex.Message}")
            ' لا نرمي استثناء هنا لأن نسخ ملفات الأرشيف اختياري
        End Try
    End Sub

    ''' <summary>
    ''' حذف النسخ الاحتياطية القديمة
    ''' </summary>
    Private Sub DeleteOldBackups(backupPath As String, keepDays As Integer)
        Try
            If Directory.Exists(backupPath) Then
                Dim cutoffDate As DateTime = DateTime.Now.AddDays(-keepDays)
                Dim backupFiles() As String = Directory.GetFiles(backupPath, "*.bak")
                backupFiles = backupFiles.Concat(Directory.GetFiles(backupPath, "*.zip")).ToArray()

                For Each file As String In backupFiles
                    Dim fileInfo As New FileInfo(file)
                    If fileInfo.CreationTime < cutoffDate Then
                        System.IO.File.Delete(file)
                        System.Diagnostics.Debug.WriteLine($"تم حذف النسخة الاحتياطية القديمة: {fileInfo.Name}")
                    End If
                Next
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حذف النسخ القديمة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' إغلاق الاتصالات النشطة
    ''' </summary>
    Private Sub CloseActiveConnections()
        Try
            Using connection As New SqlConnection(GetMasterConnectionString())
                connection.Open()

                Dim dbName As String = GetDatabaseName()
                Dim killQuery As String = $"
                    DECLARE @sql NVARCHAR(500) = ''
                    SELECT @sql = @sql + 'KILL ' + CONVERT(VARCHAR(5), session_id) + ';'
                    FROM sys.dm_exec_sessions 
                    WHERE database_id = DB_ID('{dbName}') 
                    AND session_id <> @@SPID
                    EXEC(@sql)"

                Using cmd As New SqlCommand(killQuery, connection)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إغلاق الاتصالات: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' حفظ معلومات النسخة الاحتياطية
    ''' </summary>
    Private Sub SaveBackupInfo(backupFilePath As String, description As String)
        Try
            ' يمكن حفظ معلومات إضافية في جدول مخصص إذا لزم الأمر
            System.Diagnostics.Debug.WriteLine($"تم إنشاء نسخة احتياطية: {backupFilePath}")
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حفظ معلومات النسخة الاحتياطية: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' حفظ معلومات الاستعادة
    ''' </summary>
    Private Sub SaveRestoreInfo(backupFilePath As String, description As String)
        Try
            ' يمكن حفظ معلومات إضافية في جدول مخصص إذا لزم الأمر
            System.Diagnostics.Debug.WriteLine($"تم استعادة قاعدة البيانات من: {backupFilePath}")
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حفظ معلومات الاستعادة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' الحصول على سلسلة الاتصال بقاعدة البيانات
    ''' </summary>
    Private Function GetConnectionString() As String
        Try
            ' محاولة الحصول على سلسلة الاتصال من الإعدادات
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection")?.ConnectionString

            If String.IsNullOrEmpty(connectionString) Then
                ' إذا لم توجد في الإعدادات، استخدم سلسلة افتراضية
                connectionString = "Data Source=.;Initial Catalog=ESIS;Integrated Security=True"
            End If

            Return connectionString

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetConnectionString: {ex.Message}")
            ' إرجاع سلسلة اتصال افتراضية
            Return "Data Source=.;Initial Catalog=ESIS;Integrated Security=True"
        End Try
    End Function

    ''' <summary>
    ''' الحصول على سلسلة الاتصال بقاعدة البيانات الرئيسية (master)
    ''' </summary>
    Private Function GetMasterConnectionString() As String
        Try
            Dim builder As New SqlConnectionStringBuilder(GetConnectionString())
            builder.InitialCatalog = "master"
            Return builder.ConnectionString
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetMasterConnectionString: {ex.Message}")
            Return GetConnectionString()
        End Try
    End Function

    ''' <summary>
    ''' الحصول على اسم قاعدة البيانات
    ''' </summary>
    Private Function GetDatabaseName() As String
        Try
            Dim builder As New SqlConnectionStringBuilder(GetConnectionString())
            Return If(String.IsNullOrEmpty(builder.InitialCatalog), "ESIS", builder.InitialCatalog)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetDatabaseName: {ex.Message}")
            Return "ESIS"
        End Try
    End Function

End Class

''' <summary>
''' معلومات قاعدة البيانات
''' </summary>
Public Class DatabaseInfo
    Public Property DatabaseName As String
    Public Property DatabaseSize As Long
    Public Property LastBackupDate As DateTime?
End Class

''' <summary>
''' معاملات النسخ الاحتياطي
''' </summary>
Public Class BackupParameters
    Public Property BackupPath As String
    Public Property BackupDescription As String
    Public Property CompressBackup As Boolean
    Public Property IncludeArchiveFiles As Boolean
    Public Property AutoDeleteOldBackups As Boolean
    Public Property KeepBackupsForDays As Integer
End Class

''' <summary>
''' نتيجة عملية النسخ الاحتياطي
''' </summary>
Public Class BackupResult
    Public Property Success As Boolean
    Public Property ErrorMessage As String
    Public Property BackupFilePath As String
    Public Property BackupFileSize As Long
    Public Property Duration As TimeSpan
End Class

''' <summary>
''' معاملات الاستعادة
''' </summary>
Public Class RestoreParameters
    Public Property BackupFilePath As String
    Public Property RestoreDescription As String
    Public Property CreateBackupBeforeRestore As Boolean
    Public Property CloseConnectionsBeforeRestore As Boolean
End Class

''' <summary>
''' نتيجة عملية الاستعادة
''' </summary>
Public Class RestoreResult
    Public Property Success As Boolean
    Public Property ErrorMessage As String
    Public Property BackupCreatedBeforeRestore As Boolean
    Public Property Duration As TimeSpan
End Class

''' <summary>
''' معلومات ملف النسخة الاحتياطية
''' </summary>
Public Class BackupFileInfo
    Public Property FileName As String
    Public Property FilePath As String
    Public Property FileSize As Long
    Public Property CreationDate As DateTime
    Public Property Description As String
End Class