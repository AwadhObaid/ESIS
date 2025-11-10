Imports System.Data.SqlClient
Imports System.IO
Imports System.Management
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Public Class ArchiveClass
    ' تعريف الثوابت لتحسين الصيانة
    Private Const DELETED_FOLDER_NAME As String = "مجلد المحذوفات"
    'Private Const DELETED_FOLDER_NAME As String = "المحذوفات"
    Private Const MAX_RETRY_ATTEMPTS As Integer = 3
    Private Const RETRY_DELAY_MS As Integer = 500
    Private activityLogClass As New ActivityLogClass

    ' متغير لتخزين آخر خطأ
    Private _lastError As String = ""

    '''' <summary>
    '''' إضافة أرشيف جديد إلى قاعدة البيانات
    '''' </summary>
    'Public Function AddArchive(
    '    archiveNum As String,
    '    archiveSubject As String,
    '    archiveFolder As String,
    '    archiveDate As Date,
    '    archiveWillMaster As String,
    '    archiveWillSub As String,
    '    archiveNotes As String,
    '    filename As String,
    '    fileType As String,
    '    filePath As String,
    '    originalFilePath As String,
    '    userName As String) As Integer

    '    Dim transaction As SqlTransaction = Nothing
    '    Try
    '        ' تحليل مسار الأرشيف وإنشاء المجلدات المناسبة
    '        Dim finalArchivePath As String = GetFinalArchivePath(archiveFolder)
    '        If String.IsNullOrEmpty(finalArchivePath) Then
    '            Throw New Exception("فشل في تحديد مسار الأرشيف")
    '        End If

    '        ' التحقق من أن اسم الملف يحتوي على الامتداد الصحيح
    '        Dim fileExtension As String = Path.GetExtension(filePath) ' الحصول على الامتداد من المسار الكامل
    '        If Not String.IsNullOrEmpty(fileExtension) AndAlso Not filename.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) Then
    '            filename = Path.GetFileNameWithoutExtension(filename) & fileExtension
    '        End If

    '        ' تحديد المسار النهائي للملف (المسار الحالي بعد النسخ)
    '        Dim destinationPath As String = Path.Combine(finalArchivePath, filename)
    '        Dim actualOriginalFilePath As String = If(String.IsNullOrEmpty(originalFilePath), filePath, originalFilePath)
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            transaction = connection.BeginTransaction()

    '            Try
    '                ' التحقق من وجود الملف المصدر
    '                If File.Exists(actualOriginalFilePath) Then
    '                    ' نسخ الملف إلى المجلد المحدد
    '                    File.Copy(actualOriginalFilePath, destinationPath, True)
    '                Else
    '                    Throw New Exception($"الملف المصدر غير موجود: {actualOriginalFilePath}")
    '                End If

    '                ' إدراج السجل في قاعدة البيانات
    '                Dim query As String = "INSERT INTO TbArchive (ArchiveNum, ArchiveDate, ArchiveSubject,
    '                                        ArchiveNotes, ArchiveFolder, FileName, FileType, FilePath, OriginalFilePath, ArchiveWillMaster,
    '                                        ArchiveWillSub, UserName) VALUES (@ArchiveNum, @ArchiveDate, @ArchiveSubject, @ArchiveNotes,
    '                                        @ArchiveFolder, @FileName, @FileType, @FilePath, @OriginalFilePath,
    '                                        @ArchiveWillMaster, @ArchiveWillSub, @UserName); SELECT SCOPE_IDENTITY();"

    '                Using command As New SqlCommand(query, connection, transaction)
    '                    command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
    '                    command.Parameters.AddWithValue("@ArchiveDate", archiveDate)
    '                    command.Parameters.AddWithValue("@ArchiveSubject", archiveSubject)
    '                    command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
    '                    command.Parameters.AddWithValue("@ArchiveNotes", archiveNotes)
    '                    command.Parameters.AddWithValue("@FileName", filename)
    '                    command.Parameters.AddWithValue("@FileType", fileType)
    '                    command.Parameters.AddWithValue("@FilePath", destinationPath) ' المسار الحالي بعد النسخ
    '                    command.Parameters.AddWithValue("@OriginalFilePath", actualOriginalFilePath) ' المسار الأصلي للملف
    '                    command.Parameters.AddWithValue("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster))
    '                    command.Parameters.AddWithValue("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub))
    '                    command.Parameters.AddWithValue("@UserName", userName)

    '                    Dim result As Object = command.ExecuteScalar()
    '                    Dim newId As Integer

    '                    ' التحقق من أن القيمة يمكن تحويلها إلى رقم
    '                    If result IsNot Nothing AndAlso Integer.TryParse(result.ToString(), newId) Then
    '                        ' تأكيد المعاملة
    '                        transaction.Commit()
    '                        ' ✅ استدعاء دالة التسجيل من كلاس ActivityLogClass
    '                        activityLogClass.LogUserActivity(userName, "Add", "TbArchive", newId, $"تمت إضافة أرشيف جديد برقم: {archiveNum}, الموضوع: {archiveSubject}")

    '                        Return newId
    '                    Else
    '                        ' التراجع عن المعاملة في حالة فشل التحويل
    '                        transaction.Rollback()


    '                        Return -1
    '                    End If
    '                End Using
    '            Catch ex As Exception
    '                ' التراجع عن المعاملة في حالة حدوث خطأ
    '                transaction.Rollback()
    '                Throw
    '            End Try
    '        End Using

    '    Catch ex As Exception
    '        _lastError = "خطأ في إضافة الأرشيف: " & ex.Message
    '        Throw New Exception(_lastError)
    '    End Try
    'End Function

    '''' <summary>
    '''' إضافة أرشيف جديد إلى قاعدة البيانات مع تسجيل النشاط
    '''' </summary>
    'Public Function AddArchive(
    'archiveNum As String,
    'archiveSubject As String,
    'archiveFolder As String,
    'archiveDate As Date,
    'archiveWillMaster As String,
    'archiveWillSub As String,
    'archiveNotes As String,
    'filename As String,
    'fileType As String,
    'filePath As String,
    'originalFilePath As String,
    'userName As String) As Integer

    '    Dim transaction As SqlTransaction = Nothing
    '    Dim newId As Integer = 0 ' لتخزين معرف السجل الجديد
    '    Try
    '        ' تحليل مسار الأرشيف وإنشاء المجلدات المناسبة
    '        Dim finalArchivePath As String = GetFinalArchivePath(archiveFolder)
    '        If String.IsNullOrEmpty(finalArchivePath) Then
    '            Throw New Exception("فشل في تحديد مسار الأرشيف")
    '        End If

    '        ' التحقق من أن اسم الملف يحتوي على الامتداد الصحيح
    '        Dim fileExtension As String = Path.GetExtension(filePath) ' الحصول على الامتداد من المسار الكامل
    '        If Not String.IsNullOrEmpty(fileExtension) AndAlso Not filename.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) Then
    '            filename = Path.GetFileNameWithoutExtension(filename) & fileExtension
    '        End If

    '        ' تحديد المسار النهائي للملف (المسار الحالي بعد النسخ)
    '        Dim destinationPath As String = Path.Combine(finalArchivePath, filename)
    '        Dim actualOriginalFilePath As String = If(String.IsNullOrEmpty(originalFilePath), filePath, originalFilePath)

    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            transaction = connection.BeginTransaction()

    '            Try
    '                ' التحقق من وجود الملف المصدر
    '                If File.Exists(actualOriginalFilePath) Then
    '                    ' نسخ الملف إلى المجلد المحدد
    '                    File.Copy(actualOriginalFilePath, destinationPath, True)
    '                Else
    '                    Throw New Exception($"الملف المصدر غير موجود: {actualOriginalFilePath}")
    '                End If

    '                ' إدراج السجل في قاعدة البيانات
    '                Dim query As String = "INSERT INTO TbArchive (ArchiveNum, ArchiveDate, ArchiveSubject,
    '                                    ArchiveNotes, ArchiveFolder, FileName, FileType, FilePath, OriginalFilePath, ArchiveWillMaster,
    '                                    ArchiveWillSub, UserName) VALUES (@ArchiveNum, @ArchiveDate, @ArchiveSubject, @ArchiveNotes,
    '                                    @ArchiveFolder, @FileName, @FileType, @FilePath, @OriginalFilePath,
    '                                    @ArchiveWillMaster, @ArchiveWillSub, @UserName); SELECT SCOPE_IDENTITY();"

    '                Using command As New SqlCommand(query, connection, transaction)
    '                    command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
    '                    command.Parameters.AddWithValue("@ArchiveDate", archiveDate)
    '                    command.Parameters.AddWithValue("@ArchiveSubject", archiveSubject)
    '                    command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
    '                    command.Parameters.AddWithValue("@ArchiveNotes", archiveNotes)
    '                    command.Parameters.AddWithValue("@FileName", filename)
    '                    command.Parameters.AddWithValue("@FileType", fileType)
    '                    command.Parameters.AddWithValue("@FilePath", destinationPath) ' المسار الحالي بعد النسخ
    '                    command.Parameters.AddWithValue("@OriginalFilePath", actualOriginalFilePath) ' المسار الأصلي للملف
    '                    command.Parameters.AddWithValue("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster))
    '                    command.Parameters.AddWithValue("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub))
    '                    command.Parameters.AddWithValue("@UserName", userName)

    '                    Dim result As Object = command.ExecuteScalar()

    '                    ' التحقق من أن القيمة يمكن تحويلها إلى رقم
    '                    If result IsNot Nothing AndAlso Integer.TryParse(result.ToString(), newId) Then
    '                        ' تأكيد المعاملة
    '                        transaction.Commit()

    '                        ' ✅ استدعاء دالة التسجيل من كلاس ActivityLogClass
    '                        activityLogClass.LogUserActivity(userName, "Add", "TbArchive", newId, $"تمت إضافة أرشيف جديد برقم: {archiveNum}, الموضوع: {archiveSubject}")

    '                        Return newId
    '                    Else
    '                        ' التراجع عن المعاملة في حالة فشل التحويل
    '                        transaction.Rollback()
    '                        Return -1
    '                    End If
    '                End Using
    '            Catch exCopyOrInsert As Exception
    '                ' التراجع عن المعاملة في حالة حدوث خطأ
    '                transaction.Rollback()

    '                ' ✅ تسجيل فشل الإضافة باستخدام كلاس ActivityLogClass
    '                activityLogClass.LogUserActivity(userName, "Failed_Add", "TbArchive", -1,
    '                       $"فشل في إضافة الأرشيف برقم: {archiveNum}. الخطأ: {exCopyOrInsert.Message}")

    '                Throw
    '            End Try
    '        End Using

    '    Catch ex As Exception
    '        _lastError = "خطأ في إضافة الأرشيف: " & ex.Message
    '        Throw New Exception(_lastError)
    '    End Try
    'End Function

    '''' <summary>
    '''' إضافة أرشيف جديد إلى قاعدة البيانات مع تسجيل النشاط
    '''' </summary>
    'Public Function AddArchive(
    'archiveNum As String,
    'archiveSubject As String,
    'archiveFolder As String,
    'archiveDate As Date,
    'archiveWillMaster As String,
    'archiveWillSub As String,
    'archiveNotes As String,
    'filename As String,
    'fileType As String,
    'filePath As String,
    'originalFilePath As String,
    'userName As String) As Integer

    '    Dim transaction As SqlTransaction = Nothing
    '    Dim newId As Integer = 0
    '    Dim activityLogClass As New ActivityLogClass() ' إنشاء مثيل من كلاس ActivityLogClass
    '    Try
    '        ' تحليل مسار الأرشيف وإنشاء المجلدات المناسبة
    '        Dim finalArchivePath As String = GetFinalArchivePath(archiveFolder)
    '        If String.IsNullOrEmpty(finalArchivePath) Then
    '            Throw New Exception("فشل في تحديد مسار الأرشيف")
    '        End If

    '        ' التحقق من أن اسم الملف يحتوي على الامتداد الصحيح
    '        Dim fileExtension As String = Path.GetExtension(filePath)
    '        If Not String.IsNullOrEmpty(fileExtension) AndAlso Not filename.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) Then
    '            filename = Path.GetFileNameWithoutExtension(filename) & fileExtension
    '        End If

    '        ' تحديد المسار النهائي للملف (المسار الحالي بعد النسخ)
    '        Dim destinationPath As String = Path.Combine(finalArchivePath, filename)
    '        Dim actualOriginalFilePath As String = If(String.IsNullOrEmpty(originalFilePath), filePath, originalFilePath)

    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            transaction = connection.BeginTransaction()

    '            Try
    '                ' التحقق من وجود الملف المصدر
    '                If File.Exists(actualOriginalFilePath) Then
    '                    ' نسخ الملف إلى المجلد المحدد
    '                    File.Copy(actualOriginalFilePath, destinationPath, True)
    '                Else
    '                    Throw New Exception($"الملف المصدر غير موجود: {actualOriginalFilePath}")
    '                End If

    '                ' إدراج السجل في قاعدة البيانات
    '                Dim query As String = "INSERT INTO TbArchive (ArchiveNum, ArchiveDate, ArchiveSubject,
    '                                    ArchiveNotes, ArchiveFolder, FileName, FileType, FilePath, OriginalFilePath, ArchiveWillMaster,
    '                                    ArchiveWillSub, UserName) VALUES (@ArchiveNum, @ArchiveDate, @ArchiveSubject, @ArchiveNotes,
    '                                    @ArchiveFolder, @FileName, @FileType, @FilePath, @OriginalFilePath,
    '                                    @ArchiveWillMaster, @ArchiveWillSub, @UserName); SELECT SCOPE_IDENTITY();"

    '                Using command As New SqlCommand(query, connection, transaction)
    '                    command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
    '                    command.Parameters.AddWithValue("@ArchiveDate", archiveDate)
    '                    command.Parameters.AddWithValue("@ArchiveSubject", archiveSubject)
    '                    command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
    '                    command.Parameters.AddWithValue("@ArchiveNotes", archiveNotes)
    '                    command.Parameters.AddWithValue("@FileName", filename)
    '                    command.Parameters.AddWithValue("@FileType", fileType)
    '                    command.Parameters.AddWithValue("@FilePath", destinationPath)
    '                    command.Parameters.AddWithValue("@OriginalFilePath", actualOriginalFilePath)
    '                    command.Parameters.AddWithValue("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster))
    '                    command.Parameters.AddWithValue("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub))
    '                    command.Parameters.AddWithValue("@UserName", userName)

    '                    Dim result As Object = command.ExecuteScalar()

    '                    ' التحقق من أن القيمة يمكن تحويلها إلى رقم
    '                    If result IsNot Nothing AndAlso Integer.TryParse(result.ToString(), newId) Then
    '                        ' تأكيد المعاملة
    '                        transaction.Commit()

    '                        ' ✅ استدعاء دالة التسجيل من كلاس ActivityLogClass
    '                        activityLogClass.LogUserActivity(userName, "Add", "TbArchive", newId, $"تمت إضافة أرشيف جديد برقم: {archiveNum}, الموضوع: {archiveSubject}")

    '                        Return newId
    '                    Else
    '                        ' التراجع عن المعاملة في حالة فشل التحويل
    '                        transaction.Rollback()
    '                        Return -1
    '                    End If
    '                End Using
    '            Catch exCopyOrInsert As Exception
    '                ' التراجع عن المعاملة في حالة حدوث خطأ
    '                transaction.Rollback()

    '                ' ✅ تسجيل فشل الإضافة باستخدام كلاس ActivityLogClass
    '                activityLogClass.LogUserActivity(userName, "Failed_Add", "TbArchive", -1,
    '                       $"فشل في إضافة الأرشيف برقم: {archiveNum}. الخطأ: {exCopyOrInsert.Message}")

    '                Throw
    '            End Try
    '        End Using

    '    Catch ex As Exception
    '        _lastError = "خطأ في إضافة الأرشيف: " & ex.Message
    '        Throw New Exception(_lastError)
    '    End Try
    'End Function

#Region "الاضافة - التعديل  - الحذف"

    ''' <summary>
    ''' إضافة أرشيف جديد إلى قاعدة البيانات باستخدام الإجراء المخزن
    ''' </summary>
    Public Function AddArchive(
    archiveNum As String,
    archiveSubject As String,
    archiveFolder As String,
    archiveDate As Date,
    archiveWillMaster As String,
    archiveWillSub As String,
    archiveNotes As String,
    filename As String,
    fileType As String,
    filePath As String,
    originalFilePath As String,
    userName As String) As Integer

        Dim transaction As SqlTransaction = Nothing
        Try
            ' تحليل مسار الأرشيف وإنشاء المجلدات المناسبة
            Dim finalArchivePath As String = GetFinalArchivePath(archiveFolder)
            If String.IsNullOrEmpty(finalArchivePath) Then
                Throw New Exception("فشل في تحديد مسار الأرشيف")
            End If

            ' التحقق من أن اسم الملف يحتوي على الامتداد الصحيح
            Dim fileExtension As String = Path.GetExtension(filePath) ' الحصول على الامتداد من المسار الكامل
            If Not String.IsNullOrEmpty(fileExtension) AndAlso Not filename.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) Then
                filename = Path.GetFileNameWithoutExtension(filename) & fileExtension
            End If

            ' تحديد المسار النهائي للملف (المسار الحالي بعد النسخ)
            Dim destinationPath As String = Path.Combine(finalArchivePath, filename)
            Dim actualOriginalFilePath As String = If(String.IsNullOrEmpty(originalFilePath), filePath, originalFilePath)

            ' فتح الاتصال وبدء المعاملة
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                transaction = connection.BeginTransaction()

                Try
                    ' التحقق من وجود الملف المصدر
                    If File.Exists(actualOriginalFilePath) Then
                        ' نسخ الملف إلى المجلد المحدد
                        File.Copy(actualOriginalFilePath, destinationPath, True)
                    Else
                        Throw New Exception($"الملف المصدر غير موجود: {actualOriginalFilePath}")
                    End If

                    ' استخدام الإجراء المخزن "AddArchive"
                    Dim parameters() As SqlParameter = {
                    DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum),
                    DatabaseHelper.CreateParameter("@ArchiveSubject", archiveSubject),
                    DatabaseHelper.CreateParameter("@ArchiveFolder", archiveFolder),
                    DatabaseHelper.CreateParameter("@ArchiveDate", archiveDate),
                    DatabaseHelper.CreateParameter("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster)),
                    DatabaseHelper.CreateParameter("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub)),
                    DatabaseHelper.CreateParameter("@ArchiveNotes", archiveNotes),
                    DatabaseHelper.CreateParameter("@filename", filename),
                    DatabaseHelper.CreateParameter("@fileType", fileType),
                    DatabaseHelper.CreateParameter("@filepath", destinationPath), ' المسار الحالي بعد النسخ
                    DatabaseHelper.CreateParameter("@OriginalFilePath", actualOriginalFilePath), ' المسار الأصلي للملف
                    DatabaseHelper.CreateParameter("@UserName", userName)
                }

                    ' تنفيذ الإجراء المخزن واحصل على معرف السجل الجديد
                    Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("AddArchive", parameters, connection, transaction)

                    ' التحقق من أن الإجراء عاد بنتائج
                    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                        Dim newId As Integer = Convert.ToInt32(dt.Rows(0)("NewID"))

                        ' ✅ استدعاء دالة التسجيل من كلاس ActivityLogClass
                        activityLogClass.LogUserActivity(userName, "Add", "TbArchive", newId, $"تمت إضافة أرشيف جديد برقم: {archiveNum}, الموضوع: {archiveSubject}")

                        ' تأكيد المعاملة
                        transaction.Commit()

                        Return newId
                    Else
                        ' التراجع عن المعاملة إذا لم يتم إرجاع معرف جديد
                        transaction.Rollback()
                        Return -1
                    End If

                Catch ex As Exception
                    ' التراجع عن المعاملة في حالة حدوث خطأ
                    transaction.Rollback()
                    Throw
                End Try
            End Using

        Catch ex As Exception
            _lastError = "خطأ في إضافة الأرشيف: " & ex.Message
            Throw New Exception(_lastError)
        End Try
    End Function

    ''' <summary>
    ''' تعديل أرشيف موجود في قاعدة البيانات باستخدام الإجراء المخزن
    ''' </summary>
    Public Function UpdateArchive(
    id As Integer,
    archiveNum As String,
    archiveSubject As String,
    archiveFolder As String,
    archiveDate As Date,
    archiveWillMaster As String,
    archiveWillSub As String,
    archiveNotes As String,
    fileName As String,
    fileType As String,
    filePath As String,
    originalFilePath As String,
    userName As String
) As Boolean
        Dim transaction As SqlTransaction = Nothing
        Try
            ' تحليل مسار الأرشيف وإنشاء المجلدات المناسبة
            Dim finalArchivePath As String = GetFinalArchivePath(archiveFolder)
            If String.IsNullOrEmpty(finalArchivePath) Then
                Throw New Exception("فشل في تحديد مسار الأرشيف")
            End If

            ' التحقق من أن اسم الملف يحتوي على الامتداد الصحيح
            Dim fileExtension As String = Path.GetExtension(filePath)
            If Not String.IsNullOrEmpty(fileExtension) AndAlso Not fileName.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) Then
                fileName = Path.GetFileNameWithoutExtension(fileName) & fileExtension
            End If

            ' تحديد المسار النهائي للملف (المسار الحالي بعد النسخ)
            Dim destinationPath As String = Path.Combine(finalArchivePath, fileName)
            Dim actualOriginalFilePath As String = If(String.IsNullOrEmpty(originalFilePath), filePath, originalFilePath)

            Using connection As SqlConnection = GetConnection()
                connection.Open()
                transaction = connection.BeginTransaction()

                Try
                    ' الحصول على معلومات الأرشيف الحالية
                    Dim currentArchive As DataTable = GetArchiveById(id)
                    Dim currentFilePath As String = ""
                    Dim currentOriginalFilePath As String = ""

                    If currentArchive.Rows.Count > 0 Then
                        currentFilePath = If(currentArchive.Rows(0)("FilePath") IsNot DBNull.Value, currentArchive.Rows(0)("FilePath").ToString(), "")
                        currentOriginalFilePath = If(currentArchive.Rows(0)("OriginalFilePath") IsNot DBNull.Value, currentArchive.Rows(0)("OriginalFilePath").ToString(), "")
                    End If

                    ' إغلاق الملف في جميع WebBrowser
                    CloseFileInAllWebBrowsers(currentFilePath)
                    ForceCloseFileHandles(currentFilePath)
                    Thread.Sleep(500) ' انتظار قصير للتأكد من إغلاق الملف

                    ' التحقق من وجود الملف المصدر
                    If File.Exists(actualOriginalFilePath) Then
                        ' حذف الملف القديم من المسار الحالي إذا كان مختلفًا
                        If Not String.IsNullOrEmpty(currentFilePath) AndAlso currentFilePath <> destinationPath AndAlso File.Exists(currentFilePath) Then
                            Try
                                File.Delete(currentFilePath)
                            Catch ex As Exception
                                ' في حالة فشل الحذف، نقله إلى مجلد المحذوفات
                                'MoveToDeletedFolder(currentFilePath)
                            End Try
                        End If

                        ' نسخ الملف الجديد إلى المجلد المحدد
                        File.Copy(actualOriginalFilePath, destinationPath, True)
                    Else
                        Throw New Exception($"الملف المصدر غير موجود: {actualOriginalFilePath}")
                    End If

                    ' استخدام الإجراء المخزن "UpdateArchive"
                    Dim parameters() As SqlParameter = {
                    DatabaseHelper.CreateParameter("@ID", id),
                    DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum),
                    DatabaseHelper.CreateParameter("@ArchiveSubject", archiveSubject),
                    DatabaseHelper.CreateParameter("@ArchiveFolder", archiveFolder),
                    DatabaseHelper.CreateParameter("@ArchiveDate", archiveDate),
                    DatabaseHelper.CreateParameter("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster)),
                    DatabaseHelper.CreateParameter("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub)),
                    DatabaseHelper.CreateParameter("@ArchiveNotes", archiveNotes),
                    DatabaseHelper.CreateParameter("@filename", fileName),
                    DatabaseHelper.CreateParameter("@fileType", fileType),
                    DatabaseHelper.CreateParameter("@filepath", destinationPath), ' المسار الحالي بعد النسخ
                    DatabaseHelper.CreateParameter("@OriginalFilePath", actualOriginalFilePath), ' المسار الأصلي للملف
                    DatabaseHelper.CreateParameter("@UserName", userName)
                }

                    ' تنفيذ الإجراء المخزن واحصل على معرف السجل الجديد
                    Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("UpdateArchive", parameters, connection, transaction)

                    ' التحقق من أن الإجراء عاد بنتائج
                    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                        ' تأكيد المعاملة
                        transaction.Commit()

                        ' ✅ التحقق من نجاح التحديث باستخدام القيم الجاهزة
                        ' بدلًا من الاعتماد على rowsAffected، استخدم القيم التي جلبتها من قاعدة البيانات
                        If currentArchive.Rows.Count > 0 Then
                            ' التحقق مما إذا كانت القيم الجديدة مختلفة عن القديمة
                            Dim isChanged As Boolean = False
                            If currentArchive.Rows(0)("ArchiveNum").ToString() <> archiveNum Then isChanged = True
                            If currentArchive.Rows(0)("ArchiveSubject").ToString() <> archiveSubject Then isChanged = True
                            If currentArchive.Rows(0)("ArchiveFolder").ToString() <> archiveFolder Then isChanged = True
                            If Convert.ToDateTime(currentArchive.Rows(0)("ArchiveDate")) <> archiveDate Then isChanged = True
                            If currentArchive.Rows(0)("ArchiveWillMaster").ToString() <> archiveWillMaster Then isChanged = True
                            If currentArchive.Rows(0)("ArchiveWillSub").ToString() <> archiveWillSub Then isChanged = True
                            If currentArchive.Rows(0)("ArchiveNotes").ToString() <> archiveNotes Then isChanged = True
                            If currentArchive.Rows(0)("FileName").ToString() <> fileName Then isChanged = True
                            If currentArchive.Rows(0)("FileType").ToString() <> fileType Then isChanged = True
                            If currentArchive.Rows(0)("FilePath").ToString() <> destinationPath Then isChanged = True
                            If currentArchive.Rows(0)("OriginalFilePath").ToString() <> actualOriginalFilePath Then isChanged = True
                            If currentArchive.Rows(0)("UserName").ToString() <> userName Then isChanged = True

                            ' إذا كانت هناك أي تغييرات، فاعتبر العملية ناجحة
                            Return isChanged
                        Else

                            ' إذا لم يتم العثور على السجل، فاعتبر العملية فاشلة
                            Return False
                        End If

                        ' ✅ استدعاء دالة التسجيل من كلاس ActivityLogClass
                        activityLogClass.LogUserActivity(userName, "Edit", "TbArchive", id, $"تم تعديل الأرشيف رقم: {archiveNum}, الموضوع: {archiveSubject}")

                        Return True
                    Else
                        ' التراجع عن المعاملة إذا لم يتم إرجاع معرف جديد
                        transaction.Rollback()
                        Return False
                    End If

                Catch ex As Exception
                    ' التراجع عن المعاملة في حالة حدوث خطأ
                    transaction.Rollback()
                    Throw
                End Try
            End Using

        Catch ex As Exception
            _lastError = "خطأ في تعديل الأرشيف: " & ex.Message
            Throw New Exception(_lastError)
        End Try
    End Function

    ''' <summary>
    ''' حذف أرشيف (إخفاء منطقي) مع نقل الملف إلى مجلد المحذوفات
    ''' </summary>

    Public Function DeleteArchiveById(
    archiveId As Integer,
    userName As String,
    Optional showMessages As Boolean = True) As Boolean

        Dim transaction As SqlTransaction = Nothing
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                transaction = connection.BeginTransaction()

                ' 1. الحصول على معلومات الأرشيف قبل التعديل
                Dim archiveInfo As DataTable = GetArchiveById(archiveId)
                If archiveInfo.Rows.Count = 0 Then
                    If showMessages Then
                        MessageBox.Show("الأرشيف غير موجود", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                    Return False
                End If

                ' 2. الحصول على رقم الأرشيف وبيانات الارتباط
                Dim archiveNum As String = archiveInfo.Rows(0)("ArchiveNum").ToString()
                Dim filePath As String = If(archiveInfo.Rows(0)("FilePath") IsNot DBNull.Value,
                                  archiveInfo.Rows(0)("FilePath").ToString(), "")
                Dim originalFilePath As String = If(archiveInfo.Rows(0)("OriginalFilePath") IsNot DBNull.Value,
                                          archiveInfo.Rows(0)("OriginalFilePath").ToString(), "")

                ' 3. إغلاق الملف من جميع المصادر
                CloseFileInAllWebBrowsers(filePath)
                ForceCloseFileHandles(filePath)
                Thread.Sleep(500) ' انتظار قصير للتأكد من إغلاق الملف

                ' 4. نقل الملف إلى مجلد المحذوفات إذا كان هناك ملف
                If Not String.IsNullOrEmpty(filePath) AndAlso File.Exists(filePath) Then
                    Try
                        ' نقل الملف باستخدام الدالة المساعدة
                        MoveFileToDeletedFolder(filePath)

                        If showMessages Then
                            MessageBox.Show(
                        "تم إخفاء الأرشيف بنجاح. يمكن استرجاعه لاحقًا من قائمة المحذوفات.",
                        "نجاح",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information)
                        End If

                    Catch ex As Exception
                        ' إذا فشل نقل الملف، أعد تعيين حالة السجل
                        ' أو سجل الخطأ
                        System.Diagnostics.Debug.WriteLine($"فشل في نقل الملف: {ex.Message}")
                        ' يمكنك اختيار عدم إظهار رسالة خطأ للمستخدم هنا
                    End Try
                    'Else
                    '    ' إذا لم يكن هناك ملف، فقط عدّ العملية ناجحة
                    '    If showMessages Then
                    '        MessageBox.Show(
                    '    "تم إخفاء الأرشيف بنجاح. لا يوجد ملف مرتبط.",
                    '    "نجاح",
                    '    MessageBoxButtons.OK,
                    '    MessageBoxIcon.Information)
                    '    End If
                End If

                ' 5. إلغاء الارتباط من جدول الصادر (TbSader)
                Dim updateSaderQuery As String = "UPDATE TbSader SET ArchiveID = NULL WHERE ArchiveID = @ArchiveID"
                Using saderCommand As New SqlCommand(updateSaderQuery, connection, transaction)
                    saderCommand.Parameters.AddWithValue("@ArchiveID", archiveId)
                    saderCommand.ExecuteNonQuery()
                End Using

                ' 6. وضع علامة على السجل كمحذوف
                Dim updateQuery As String = "UPDATE TbArchive SET " &
                                "IsDeleted = 1, " &
                                "DeletedDate = @DeletedDate, " &
                                "DeletedBy = @DeletedBy " &
                                "WHERE ID = @ID"

                Using archiveCommand As New SqlCommand(updateQuery, connection, transaction)
                    archiveCommand.Parameters.AddWithValue("@DeletedDate", DateTime.Now)
                    archiveCommand.Parameters.AddWithValue("@DeletedBy", userName)
                    archiveCommand.Parameters.AddWithValue("@ID", archiveId)

                    Dim rowsAffected As Integer = archiveCommand.ExecuteNonQuery()

                    ' 7. تأكيد المعاملة
                    transaction.Commit()

                    ' 8. تحديث عداد الأرشيف
                    DataHelper.UpdateArchiveCount()

                    ' ✅ تسجيل النشاط
                    activityLogClass.LogUserActivity(userName, "Delete", "TbArchive", archiveId, $"تم حذف الأرشيف رقم: {archiveNum}, الموضوع: {archiveInfo.Rows(0)("ArchiveSubject").ToString()}")

                    Return rowsAffected > 0

                End Using

            End Using

        Catch ex As Exception
            _lastError = $"حدث خطأ أثناء إخفاء الأرشيف بمعرف {archiveId}: {ex.Message}"
            System.Diagnostics.Debug.WriteLine(_lastError)

            If showMessages Then
                MessageBox.Show(
        $"حدث خطأ أثناء إخفاء الأرشيف: {ex.Message}",
        "خطأ",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error)
            End If

            Return False
        End Try
    End Function

    'Public Function DeleteArchiveById(
    'archiveId As Integer,
    'userName As String,
    'Optional showMessages As Boolean = True) As Boolean

    '    Dim transaction As SqlTransaction = Nothing
    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            transaction = connection.BeginTransaction()

    '            ' 1. الحصول على معلومات الأرشيف قبل التعديل
    '            Dim archiveInfo As DataTable = GetArchiveById(archiveId)
    '            If archiveInfo.Rows.Count = 0 Then
    '                If showMessages Then
    '                    MessageBox.Show("الأرشيف غير موجود", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                End If
    '                Return False
    '            End If

    '            ' 2. الحصول على رقم الأرشيف وبيانات الارتباط
    '            Dim archiveNum As String = archiveInfo.Rows(0)("ArchiveNum").ToString()
    '            Dim filePath As String = If(archiveInfo.Rows(0)("FilePath") IsNot DBNull.Value,
    '                                  archiveInfo.Rows(0)("FilePath").ToString(), "")
    '            Dim originalFilePath As String = If(archiveInfo.Rows(0)("OriginalFilePath") IsNot DBNull.Value,
    '                                          archiveInfo.Rows(0)("OriginalFilePath").ToString(), "")

    '            ' 3. إغلاق الملف من جميع المصادر
    '            CloseFileInAllWebBrowsers(filePath)
    '            ForceCloseFileHandles(filePath)
    '            Thread.Sleep(500) ' انتظار قصير للتأكد من إغلاق الملف

    '            ' 4. نقل الملف إلى مجلد المحذوفات إذا كان هناك ملف
    '            If Not String.IsNullOrEmpty(filePath) AndAlso File.Exists(filePath) Then
    '                Try
    '                    ' تحديد مسار مجلد المحذوفات
    '                    Dim deletedFolderPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, "المحذوفات")

    '                    ' إنشاء مجلد المحذوفات إذا لم يكن موجودًا
    '                    If Not Directory.Exists(deletedFolderPath) Then
    '                        Directory.CreateDirectory(deletedFolderPath)
    '                    End If

    '                    ' تكوين اسم الملف الجديد (مع تاريخ ووقت لتجنب التكرار)
    '                    Dim fileName As String = Path.GetFileName(filePath)
    '                    Dim fileExtension As String = Path.GetExtension(filePath)
    '                    Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
    '                    Dim newFileName As String = $"{Path.GetFileNameWithoutExtension(fileName)}_deleted_{timestamp}{fileExtension}"
    '                    Dim newFilePath As String = Path.Combine(deletedFolderPath, newFileName)

    '                    ' نقل الملف
    '                    File.Move(, newFilePath)
    '                    MoveFileToDeletedFolder(filePath,)

    '                    If showMessages Then
    '                        MessageBox.Show(
    '                        "تم إخفاء الأرشيف بنجاح. يمكن استرجاعه لاحقًا من قائمة المحذوفات.",
    '                        "نجاح",
    '                        MessageBoxButtons.OK,
    '                        MessageBoxIcon.Information)
    '                    End If

    '                Catch ex As Exception
    '                    ' إذا فشل نقل الملف، أعد تعيين حالة السجل
    '                    ' أو سجل الخطأ
    '                    System.Diagnostics.Debug.WriteLine($"فشل في نقل الملف: {ex.Message}")
    '                    ' يمكنك اختيار عدم إظهار رسالة خطأ للمستخدم هنا
    '                End Try
    '            Else
    '                ' إذا لم يكن هناك ملف، فقط عدّ العملية ناجحة
    '                If showMessages Then
    '                    MessageBox.Show(
    '                    "تم إخفاء الأرشيف بنجاح. لا يوجد ملف مرتبط.",
    '                    "نجاح",
    '                    MessageBoxButtons.OK,
    '                    MessageBoxIcon.Information)
    '                End If
    '            End If

    '            ' 5. إلغاء الارتباط من جدول الصادر (TbSader)
    '            Dim updateSaderQuery As String = "UPDATE TbSader SET ArchiveID = NULL WHERE ArchiveID = @ArchiveID"
    '            Using saderCommand As New SqlCommand(updateSaderQuery, connection, transaction)
    '                saderCommand.Parameters.AddWithValue("@ArchiveID", archiveId)
    '                saderCommand.ExecuteNonQuery()
    '            End Using

    '            ' 6. وضع علامة على السجل كمحذوف
    '            Dim updateQuery As String = "UPDATE TbArchive SET " &
    '                                "IsDeleted = 1, " &
    '                                "DeletedDate = @DeletedDate, " &
    '                                "DeletedBy = @DeletedBy " &
    '                                "WHERE ID = @ID"

    '            Using archiveCommand As New SqlCommand(updateQuery, connection, transaction)
    '                archiveCommand.Parameters.AddWithValue("@DeletedDate", DateTime.Now)
    '                archiveCommand.Parameters.AddWithValue("@DeletedBy", userName)
    '                archiveCommand.Parameters.AddWithValue("@ID", archiveId)

    '                Dim rowsAffected As Integer = archiveCommand.ExecuteNonQuery()

    '                ' 7. تأكيد المعاملة
    '                transaction.Commit()

    '                ' 8. تحديث عداد الأرشيف
    '                DataHelper.UpdateArchiveCount()

    '                ' ✅ تسجيل النشاط
    '                activityLogClass.LogUserActivity(userName, "Delete", "TbArchive", archiveId, $"تم حذف الأرشيف رقم: {archiveNum}, الموضوع: {archiveInfo.Rows(0)("ArchiveSubject").ToString()}")

    '                Return rowsAffected > 0

    '            End Using

    '        End Using

    '    Catch ex As Exception
    '        _lastError = $"حدث خطأ أثناء إخفاء الأرشيف بمعرف {archiveId}: {ex.Message}"
    '        System.Diagnostics.Debug.WriteLine(_lastError)

    '        If showMessages Then
    '            MessageBox.Show(
    '        $"حدث خطأ أثناء إخفاء الأرشيف: {ex.Message}",
    '        "خطأ",
    '        MessageBoxButtons.OK,
    '        MessageBoxIcon.Error)
    '        End If

    '        Return False
    '    End Try
    'End Function

    'Public Function HideArchiveById(
    'ByVal archiveId As Integer,
    'ByVal userName As String,
    'Optional ByVal showMessages As Boolean = True) As Boolean

    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()

    '            ' 1. الحصول على معلومات الأرشيف قبل التعديل
    '            Dim archiveNum As String = ""
    '            Dim archiveSubject As String = ""
    '            Dim currentFilePath As String = ""

    '            Dim getInfoQuery As String = "SELECT ArchiveNum, ArchiveSubject, FilePath FROM TbArchive WHERE ID = @ID"
    '            Using getInfoCommand As New SqlCommand(getInfoQuery, connection)
    '                getInfoCommand.Parameters.AddWithValue("@ID", archiveId)
    '                Using reader As SqlDataReader = getInfoCommand.ExecuteReader()
    '                    If reader.Read() Then
    '                        archiveNum = If(reader("ArchiveNum") IsNot DBNull.Value, reader("ArchiveNum").ToString(), $"المعرف {archiveId}")
    '                        archiveSubject = If(reader("ArchiveSubject") IsNot DBNull.Value, reader("ArchiveSubject").ToString(), "موضوع غير محدد")
    '                        currentFilePath = If(reader("FilePath") IsNot DBNull.Value, reader("FilePath").ToString(), "")
    '                    Else
    '                        Throw New Exception($"الأرشيف بمعرف {archiveId} غير موجود")
    '                    End If
    '                End Using
    '            End Using

    '            ' 2. تنفيذ عملية الإخفاء (الحذف المنطقي)
    '            Dim updateQuery As String = "UPDATE TbArchive SET " &
    '                            "IsDeleted = 1, " &
    '                            "DeletedDate = @DeletedDate, " &
    '                            "DeletedBy = @DeletedBy " &
    '                            "WHERE ID = @ID"

    '            Using command As New SqlCommand(updateQuery, connection)
    '                command.Parameters.AddWithValue("@DeletedDate", DateTime.Now)
    '                command.Parameters.AddWithValue("@DeletedBy", userName)
    '                command.Parameters.AddWithValue("@ID", archiveId)

    '                Dim rowsAffected As Integer = command.ExecuteNonQuery()

    '                ' 3. نقل الملف إلى مجلد المحذوفات إذا كان هناك ملف
    '                If Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath) Then
    '                    Try
    '                        ' نقل الملف إلى مجلد المحذوفات
    '                        Dim newFilePath As String = MoveFileToDeletedFolder(currentFilePath, archiveNum, showMessages)

    '                        If showMessages Then
    '                            MessageBox.Show(
    '                            "تم إخفاء الأرشيف بنجاح. يمكن استرجاعه لاحقًا من قائمة المحذوفات.",
    '                            "نجاح",
    '                            MessageBoxButtons.OK,
    '                            MessageBoxIcon.Information)
    '                        End If

    '                    Catch ex As Exception
    '                        ' إذا فشل نقل الملف، أعد تعيين حالة السجل
    '                        ' أو سجل الخطأ
    '                        System.Diagnostics.Debug.WriteLine($"فشل في نقل الملف: {ex.Message}")
    '                        ' يمكنك اختيار عدم إظهار رسالة خطأ للمستخدم هنا
    '                    End Try
    '                Else
    '                    ' إذا لم يكن هناك ملف، فقط عدّ العملية ناجحة
    '                    If showMessages Then
    '                        MessageBox.Show(
    '                        "تم إخفاء الأرشيف بنجاح. لا يوجد ملف مرتبط.",
    '                        "نجاح",
    '                        MessageBoxButtons.OK,
    '                        MessageBoxIcon.Information)
    '                    End If
    '                End If

    '                ' تحديث عداد الأرشيف
    '                DataHelper.UpdateArchiveCount()

    '                ' ✅ تسجيل النشاط
    '                activityLogClass.LogUserActivity(userName, "Delete", "TbArchive", archiveId, $"تم حذف الأرشيف رقم: {archiveNum}, الموضوع: {archiveSubject}")

    '                Return rowsAffected > 0

    '            End Using
    '        End Using

    '    Catch ex As Exception
    '        _lastError = $"حدث خطأ أثناء إخفاء الأرشيف بمعرف {archiveId}: {ex.Message}"
    '        System.Diagnostics.Debug.WriteLine(_lastError)

    '        If showMessages Then
    '            MessageBox.Show(
    '            $"حدث خطأ أثناء إخفاء الأرشيف: {ex.Message}",
    '            "خطأ",
    '            MessageBoxButtons.OK,
    '            MessageBoxIcon.Error)
    '        End If

    '        Return False
    '    End Try
    'End Function


#End Region

    ''' <summary>
    ''' الحصول على المسار النهائي للمجلد بناءً على تصنيف الأرشيف
    ''' </summary>
    Public Function GetFinalArchivePath(archiveFolder As String) As String
        Try
            ' التحقق من وجود التصنيف
            If String.IsNullOrWhiteSpace(archiveFolder) Then
                Return String.Empty
            End If

            ' تحديد المسار الأساسي
            Dim archiveBasePath As String = My.Settings.ARCHIVE_Default_PATH

            ' التحقق من وجود مسار أساسي
            If String.IsNullOrWhiteSpace(archiveBasePath) Then
                Return String.Empty
            End If

            ' إنشاء المسار النهائي
            Dim finalPath As String = archiveBasePath

            ' إضافة التصنيف إلى المسار
            finalPath = Path.Combine(finalPath, archiveFolder)

            ' التأكد من إنشاء جميع المجلدات الفرعية
            If Not Directory.Exists(finalPath) Then
                Directory.CreateDirectory(finalPath)
            End If

            ' إذا كان المسار يحتوي على شرطة مائلة، تأكد من إنشاء جميع المجلدات الفرعية
            If archiveFolder.Contains("\") Then
                Dim parts() As String = archiveFolder.Split("\"c)
                Dim currentPath As String = archiveBasePath

                For Each part As String In parts
                    currentPath = Path.Combine(currentPath, part)
                    If Not Directory.Exists(currentPath) Then
                        Directory.CreateDirectory(currentPath)
                    End If
                Next
            End If

            Return finalPath
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على مسار الأرشيف: {ex.Message}")
            Return String.Empty
        End Try
    End Function

    '''' <summary>
    '''' حذف سجل الأرشيف مع نقل الملف إلى مجلد المحذوفات كأولوية
    '''' </summary>
    'Public Function DeleteArchiveById(
    '    ByVal archiveId As Integer,
    '    Optional ByVal showMessages As Boolean = True) As Boolean

    '    Dim transaction As SqlTransaction = Nothing
    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            transaction = connection.BeginTransaction()

    '            Try
    '                ' 1. الحصول على معلومات الأرشيف
    '                Dim dt As DataTable = GetArchiveById(archiveId)
    '                Dim currentFilePath As String = ""
    '                Dim archiveNum As String = ""

    '                If dt.Rows.Count > 0 Then
    '                    ' التأكد من استخدام الحقل الصحيح لمسار الملف
    '                    If dt.Columns.Contains("FilePath") AndAlso Not IsDBNull(dt.Rows(0)("FilePath")) Then
    '                        currentFilePath = dt.Rows(0)("FilePath").ToString()
    '                    ElseIf dt.Columns.Contains("filepath") AndAlso Not IsDBNull(dt.Rows(0)("filepath")) Then
    '                        currentFilePath = dt.Rows(0)("filepath").ToString()
    '                    End If

    '                    ' التأكد من أن المسار مُحدد بشكل صحيح
    '                    If Not String.IsNullOrEmpty(currentFilePath) Then
    '                        Try
    '                            currentFilePath = Path.GetFullPath(currentFilePath)
    '                        Catch ex As Exception
    '                            System.Diagnostics.Debug.WriteLine($"فشل في تحويل مسار الملف إلى مسار كامل: {ex.Message}")
    '                        End Try
    '                    End If

    '                    ' الحصول على رقم الأرشيف
    '                    archiveNum = If(Not IsDBNull(dt.Rows(0)("ArchiveNum")), dt.Rows(0)("ArchiveNum").ToString(), $"المعرف {archiveId}")
    '                Else
    '                    Throw New Exception($"الأرشيف بمعرف {archiveId} غير موجود")
    '                End If

    '                ' 2. تحديد ما إذا كان هناك ملف مرتبط
    '                Dim fileExists As Boolean = Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath)
    '                Dim fileMoved As Boolean = False
    '                Dim newFilePath As String = ""

    '                ' 3. محاولة نقل الملف إلى مجلد المحذوفات
    '                If fileExists Then
    '                    Try
    '                        newFilePath = MoveFileToDeletedFolder(currentFilePath, archiveNum, showMessages)
    '                        fileMoved = Not String.IsNullOrEmpty(newFilePath)
    '                    Catch ex As Exception
    '                        System.Diagnostics.Debug.WriteLine($"فشل في نقل الملف: {ex.Message}")
    '                    End Try
    '                End If

    '                ' 4. حذف السجل من قاعدة البيانات
    '                Dim deleteQuery As String = "DELETE FROM TbArchive WHERE ID = @ID"
    '                Using command As New SqlCommand(deleteQuery, connection, transaction)
    '                    command.Parameters.AddWithValue("@ID", archiveId)
    '                    Dim rowsAffected As Integer = command.ExecuteNonQuery()

    '                    ' 5. تأكيد المعاملة
    '                    transaction.Commit()

    '                    ' 5. تحديث عداد الأرشيف
    '                    DataHelper.UpdateArchiveCount()
    '                    ' 6. تحديث عداد المحذوفات
    '                    DataHelper.UpdateDeletedArchiveCount()

    '                    ' 7. إظهار رسالة النجاح المناسبة
    '                    If fileMoved Then
    '                        If showMessages Then
    '                            MessageBox.Show(
    '                                $"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات{vbNewLine}" &
    '                                $"ونقل الملف إلى مجلد المحذوفات بنجاح",
    '                                "نجاح",
    '                                MessageBoxButtons.OK,
    '                                MessageBoxIcon.Information)
    '                        End If
    '                    Else
    '                        If fileExists Then
    '                            Dim reason As String = GetFileMoveFailureReason(currentFilePath)
    '                            If showMessages Then
    '                                MessageBox.Show(
    '                                    $"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات{vbNewLine}" &
    '                                    $"ولكن لم يتم نقل الملف{vbNewLine}{vbNewLine}" &
    '                                    $"السبب: {reason}{vbNewLine}{vbNewLine}" &
    '                                    $"يرجى نقل الملف يدويًا إلى:{vbNewLine}" &
    '                                    $"{Path.GetFullPath(Path.Combine(My.Settings.ARCHIVE_Default_PATH, DELETED_FOLDER_NAME))}",
    '                                    "نجاح جزئي",
    '                                    MessageBoxButtons.OK,
    '                                    MessageBoxIcon.Warning)
    '                            End If
    '                        Else
    '                            If showMessages Then
    '                                MessageBox.Show(
    '                                    $"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات{vbNewLine}" &
    '                                    $"(لا يوجد ملف مرتبط بهذا الأرشيف)",
    '                                    "نجاح",
    '                                    MessageBoxButtons.OK,
    '                                    MessageBoxIcon.Information)
    '                            End If
    '                        End If
    '                    End If
    '                    ' ✅ استدعاء دالة التسجيل من كلاس ActivityLogClass
    '                    activityLogClass.LogUserActivity(GlobalData.LoggedInUserName, "Delete", "TbArchive", rowsAffected, $"تم حذف الأرشيف رقم: {archiveNum}, الموضوع: {archiveSubject}")

    '                    Return rowsAffected > 0
    '                End Using
    '            Catch ex As Exception
    '                ' التراجع عن المعاملة في حالة حدوث خطأ
    '                If transaction IsNot Nothing Then
    '                    transaction.Rollback()
    '                End If
    '                Throw
    '            End Try
    '        End Using
    '    Catch ex As Exception
    '        _lastError = $"حدث خطأ أثناء حذف الأرشيف بمعرف {archiveId}: {ex.Message}"
    '        System.Diagnostics.Debug.WriteLine(_lastError)

    '        If showMessages Then
    '            MessageBox.Show(
    '                $"حدث خطأ أثناء حذف الأرشيف: {ex.Message}",
    '                "خطأ",
    '                MessageBoxButtons.OK,
    '                MessageBoxIcon.Error)
    '        End If

    '        Return False
    '    End Try
    'End Function

    '''' <summary>
    '''' حذف سجل الأرشيف مع نقل الملف إلى مجلد المحذوفات كأولوية
    '''' </summary>
    'Public Function DeleteArchiveById(
    'ByVal archiveId As Integer,
    'Optional ByVal showMessages As Boolean = True) As Boolean

    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()

    '            ' 1. الحصول على معلومات الأرشيف قبل التعديل
    '            Dim archiveNum As String = ""
    '            Dim archiveSubject As String = ""
    '            Dim currentFilePath As String = ""

    '            Dim getInfoQuery As String = "SELECT ArchiveNum, ArchiveSubject, FilePath FROM TbArchive WHERE ID = @ID"
    '            Using getInfoCommand As New SqlCommand(getInfoQuery, connection)
    '                getInfoCommand.Parameters.AddWithValue("@ID", archiveId)
    '                Using reader As SqlDataReader = getInfoCommand.ExecuteReader()
    '                    If reader.Read() Then
    '                        archiveNum = If(reader("ArchiveNum") IsNot DBNull.Value, reader("ArchiveNum").ToString(), $"المعرف {archiveId}")
    '                        archiveSubject = If(reader("ArchiveSubject") IsNot DBNull.Value, reader("ArchiveSubject").ToString(), "موضوع غير محدد")
    '                        currentFilePath = If(reader("FilePath") IsNot DBNull.Value, reader("FilePath").ToString(), "")
    '                    Else
    '                        Throw New Exception($"الأرشيف بمعرف {archiveId} غير موجود")
    '                    End If
    '                End Using
    '            End Using

    '            ' 2. تنفيذ عملية الإخفاء (الحذف المنطقي)
    '            Dim updateQuery As String = "UPDATE TbArchive SET " &
    '                                "IsDeleted = 1, " &
    '                                "DeletedDate = @DeletedDate, " &
    '                                "DeletedBy = @DeletedBy " &
    '                                "WHERE ID = @ID"

    '            Using command As New SqlCommand(updateQuery, connection)
    '                command.Parameters.AddWithValue("@DeletedDate", DateTime.Now)
    '                command.Parameters.AddWithValue("@DeletedBy", GlobalData.LoggedInUserName)
    '                command.Parameters.AddWithValue("@ID", archiveId)

    '                Dim rowsAffected As Integer = command.ExecuteNonQuery()

    '                ' 3. نقل الملف إلى مجلد المحذوفات إذا كان هناك ملف
    '                If Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath) Then
    '                    Try
    '                        ' استخدام الدالة الجديدة لنقل الملف
    '                        Dim newFilePath As String = MoveFileToDeletedFolder(currentFilePath, archiveNum, showMessages)

    '                        If showMessages Then
    '                            MessageBox.Show(
    '                        "تم إخفاء الأرشيف بنجاح. يمكن استرجاعه لاحقًا من قائمة المحذوفات.",
    '                        "نجاح",
    '                        MessageBoxButtons.OK,
    '                        MessageBoxIcon.Information)
    '                        End If

    '                    Catch ex As Exception
    '                        ' إذا فشل نقل الملف، أعد تعيين حالة السجل
    '                        ' أو سجل الخطأ
    '                        System.Diagnostics.Debug.WriteLine($"فشل في نقل الملف: {ex.Message}")
    '                        ' يمكنك اختيار عدم إظهار رسالة خطأ للمستخدم هنا
    '                    End Try
    '                Else
    '                    ' إذا لم يكن هناك ملف، فقط عدّ العملية ناجحة
    '                    If showMessages Then
    '                        MessageBox.Show(
    '                    "تم إخفاء الأرشيف بنجاح. لا يوجد ملف مرتبط.",
    '                    "نجاح",
    '                    MessageBoxButtons.OK,
    '                    MessageBoxIcon.Information)
    '                    End If
    '                End If

    '                ' تحديث عداد الأرشيف
    '                DataHelper.UpdateArchiveCount()

    '                ' ✅ تسجيل النشاط
    '                activityLogClass.LogUserActivity(GlobalData.LoggedInUserName, "Delete", "TbArchive", archiveId, $"تم حذف الأرشيف رقم: {archiveNum}, الموضوع: {archiveSubject}")

    '                Return rowsAffected > 0

    '            End Using
    '        End Using

    '    Catch ex As Exception
    '        _lastError = $"حدث خطأ أثناء إخفاء الأرشيف بمعرف {archiveId}: {ex.Message}"
    '        System.Diagnostics.Debug.WriteLine(_lastError)

    '        If showMessages Then
    '            MessageBox.Show(
    '        $"حدث خطأ أثناء إخفاء الأرشيف: {ex.Message}",
    '        "خطأ",
    '        MessageBoxButtons.OK,
    '        MessageBoxIcon.Error)
    '        End If

    '        Return False
    '    End Try
    'End Function

    '''' <summary>
    '''' نقل الملف إلى مجلد المحذوفات مع تحسينات لضمان النجاح
    '''' </summary>
    'Public Function MoveFileToDeletedFolder(
    '    ByVal sourceFilePath As String,
    '    Optional ByVal archiveNum As String = "",
    '    Optional ByVal showMessages As Boolean = True) As String

    '    Try
    '        ' التحقق من وجود الملف
    '        If String.IsNullOrEmpty(sourceFilePath) Then
    '            System.Diagnostics.Debug.WriteLine("مسار الملف فارغ")
    '            Return ""
    '        End If

    '        ' التأكد من أن المسار كاملاً
    '        Dim fullPath As String = Path.GetFullPath(sourceFilePath)
    '        If Not File.Exists(fullPath) Then
    '            System.Diagnostics.Debug.WriteLine($"الملف غير موجود: {fullPath}")
    '            Return ""
    '        End If

    '        ' الحصول على المسار الأساسي للأرشيف
    '        Dim archiveBasePath As String = My.Settings.ARCHIVE_Default_PATH
    '        If String.IsNullOrEmpty(archiveBasePath) Then
    '            Throw New Exception("مسار الأرشيف الافتراضي غير معرف في الإعدادات")
    '        End If

    '        ' التأكد من أن المسار الأساسي موجود
    '        archiveBasePath = Path.GetFullPath(archiveBasePath)
    '        If Not Directory.Exists(archiveBasePath) Then
    '            Throw New Exception($"مسار الأرشيف الافتراضي غير موجود: {archiveBasePath}")
    '        End If

    '        ' تحديد مسار مجلد المحذوفات
    '        Dim deletedFolderPath As String = Path.Combine(archiveBasePath, DELETED_FOLDER_NAME)
    '        deletedFolderPath = Path.GetFullPath(deletedFolderPath)

    '        ' إنشاء مجلد المحذوفات إذا لم يكن موجودًا
    '        If Not Directory.Exists(deletedFolderPath) Then
    '            Directory.CreateDirectory(deletedFolderPath)
    '            System.Diagnostics.Debug.WriteLine($"تم إنشاء مجلد المحذوفات: {deletedFolderPath}")
    '        End If

    '        ' إنشاء اسم ملف فريد
    '        Dim fileName As String = Path.GetFileName(fullPath)
    '        Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(fileName)
    '        Dim fileExtension As String = Path.GetExtension(fileName)
    '        Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
    '        Dim newFileName As String = $"{fileNameWithoutExtension}_deleted_{timestamp}{fileExtension}"
    '        Dim destinationFilePath As String = Path.Combine(deletedFolderPath, newFileName)

    '        ' التحقق من أن المسار الوجهة مختلف عن المصدر
    '        If String.Equals(fullPath, destinationFilePath, StringComparison.OrdinalIgnoreCase) Then
    '            System.Diagnostics.Debug.WriteLine("مسار المصدر والوجهة متطابقان")
    '            Return ""
    '        End If

    '        ' محاولة إغلاق الملف إذا كان مفتوحًا
    '        CloseFileIfOpen(fullPath)

    '        ' محاولة نقل الملف مع إعادة المحاولة
    '        Dim maxRetries As Integer = 5 ' زيادة عدد المحاولات إلى 5
    '        Dim retryCount As Integer = 0
    '        Dim success As Boolean = False
    '        Dim lastException As Exception = Nothing

    '        While retryCount < maxRetries AndAlso Not success
    '            Try
    '                ' التحقق من أن الملف مغلق
    '                Using fs As FileStream = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.None)
    '                    ' إذا نجحت، فالمشكلة ليست في الاستخدام
    '                End Using
    '            Catch ex As IOException
    '                ' الملف قيد الاستخدام، الانتظار وإعادة المحاولة
    '                retryCount += 1
    '                Threading.Thread.Sleep(1000) ' زيادة الوقت بين المحاولات إلى 1000 مللي ثانية
    '                Continue While
    '            End Try

    '            Try
    '                ' محاولة نقل الملف
    '                File.Move(fullPath, destinationFilePath)
    '                success = True
    '            Catch ex As Exception
    '                lastException = ex
    '                retryCount += 1
    '                Threading.Thread.Sleep(1000)
    '            End Try
    '        End While

    '        If success Then
    '            System.Diagnostics.Debug.WriteLine($"تم نقل الملف إلى: {destinationFilePath}")
    '            Return destinationFilePath
    '        Else
    '            If lastException IsNot Nothing Then
    '                Throw lastException
    '            Else
    '                Throw New Exception("فشل في نقل الملف بعد عدة محاولات")
    '            End If
    '        End If
    '    Catch ex As IOException
    '        ' الملف قيد الاستخدام
    '        System.Diagnostics.Debug.WriteLine($"الملف قيد الاستخدام: {ex.Message}")
    '        Throw New Exception($"الملف قيد الاستخدام ولا يمكن نقله: {ex.Message}")
    '    Catch ex As UnauthorizedAccessException
    '        ' مشكلة في الصلاحيات
    '        System.Diagnostics.Debug.WriteLine($"مشكلة في الصلاحيات: {ex.Message}")
    '        Throw New Exception($"ليس لديك صلاحيات كافية لنقل الملف: {ex.Message}")
    '    Catch ex As Exception
    '        ' أخطاء أخرى
    '        System.Diagnostics.Debug.WriteLine($"خطأ عام في نقل الملف: {ex.Message}")
    '        Throw New Exception($"فشل في نقل الملف: {ex.Message}")
    '    End Try
    'End Function

    '''' <summary>
    '''' محاولة إغلاق الملف إذا كان مفتوحًا بواسطة عملية أخرى
    '''' </summary>
    'Private Sub CloseFileIfOpen(filePath As String)
    '    Try
    '        ' التحقق من أن الملف موجود
    '        If Not File.Exists(filePath) Then
    '            Return
    '        End If

    '        ' محاولة إغلاق الملف باستخدام تقنيات متقدمة
    '        Try
    '            ' محاولة 1: استخدام FileShare.None للكشف عن الملفات المفتوحة
    '            Using fs As FileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None)
    '                ' إذا نجحت، فالمشكلة ليست في الاستخدام
    '            End Using
    '        Catch ex As IOException
    '            System.Diagnostics.Debug.WriteLine($"الملف قيد الاستخدام، سيتم محاولة إغلاقه: {ex.Message}")

    '            Try
    '                ' محاولة 2: استخدام Process Explorer للكشف عن العمليات
    '                Dim processName As String = GetProcessUsingFile(filePath)
    '                If Not String.IsNullOrEmpty(processName) Then
    '                    System.Diagnostics.Debug.WriteLine($"الملف مفتوح بواسطة: {processName}")

    '                    ' إذا كانت العملية هي Adobe Reader، نحاول إغلاقها بحذر
    '                    If processName.IndexOf("AcroRd32", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
    '                       processName.IndexOf("Adobe", StringComparison.OrdinalIgnoreCase) >= 0 Then
    '                        Try
    '                            ForceCloseAdobeReader()
    '                        Catch
    '                            ' تجاهل الأخطاء، نحاول فقط
    '                        End Try
    '                    End If
    '                End If
    '            Catch ex2 As Exception
    '                System.Diagnostics.Debug.WriteLine($"فشل في تحديد العملية المستخدمة للملف: {ex2.Message}")
    '            End Try
    '        End Try
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في محاولة إغلاق الملف: {ex.Message}")
    '    End Try
    'End Sub

    '''' <summary>
    '''' الحصول على اسم العملية التي تستخدم الملف
    '''' </summary>
    'Private Function GetProcessUsingFile(filePath As String) As String
    '    Try
    '        Dim processName As String = ""
    '        For Each process As Process In Process.GetProcesses()
    '            Try
    '                If IsFileLockedByProcess(process, filePath) Then
    '                    processName = process.ProcessName
    '                    Exit For
    '                End If
    '            Catch
    '                ' تجاهل الأخطاء
    '            End Try
    '        Next
    '        Return processName
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تحديد العملية: {ex.Message}")
    '        Return ""
    '    End Try
    'End Function

    '''' <summary>
    '''' التحقق إذا كان الملف مقفل بواسطة عملية محددة
    '''' </summary>
    'Private Function IsFileLockedByProcess(process As Process, filePath As String) As Boolean
    '    Try
    '        ' هذه دالة مبسطة، في التطبيقات الحقيقية قد تحتاج إلى استخدام تقنيات أكثر تقدمًا
    '        ' مثل استخدام NtQuerySystemInformation أو أدوات خارجية
    '        Return False
    '    Catch
    '        Return False
    '    End Try
    'End Function

    '''' <summary>
    '''' محاولة إغلاق Adobe Reader بحذر
    '''' </summary>
    'Private Sub ForceCloseAdobeReader()
    '    Try
    '        For Each process As Process In Process.GetProcessesByName("AcroRd32")
    '            Try
    '                process.Kill()
    '                System.Diagnostics.Debug.WriteLine("تم إغلاق Adobe Reader")
    '                Threading.Thread.Sleep(500) ' انتظار قصير للتأكد من الإغلاق
    '            Catch ex As Exception
    '                System.Diagnostics.Debug.WriteLine($"فشل في إغلاق Adobe Reader: {ex.Message}")
    '            End Try
    '        Next
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"فشل في إغلاق Adobe Reader: {ex.Message}")
    '    End Try
    'End Sub


    ''' <summary>
    ''' الحصول على أرشيف بواسطة المعرف
    ''' </summary>
    Public Function GetArchiveById(archiveId As Integer) As DataTable
        Dim dt As New DataTable()
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT * FROM TbArchive WHERE ID = @ID"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ID", archiveId)
                    Using adapter As New SqlDataAdapter(command)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            _lastError = "خطأ في جلب الأرشيف: " & ex.Message
            Throw New Exception(_lastError)
        End Try
        Return dt
    End Function

    ''' <summary>
    ''' التحقق من تكرار رقم الأرشيف
    ''' </summary>
    Public Function CheckDuplicateArchiveNumber(archiveNum As String) As Boolean
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE ArchiveNum = @ArchiveNum"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
                    Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        Catch ex As Exception
            _lastError = "خطأ في التحقق من تكرار رقم الأرشيف: " & ex.Message
            Throw New Exception(_lastError)
        End Try
    End Function

    ''' <summary>
    ''' التحقق من تكرار رقم الأرشيف (للحالات التحديث)
    ''' </summary>
    Public Function CheckDuplicateArchiveNumberForUpdate(archiveNum As String, id As Integer) As Boolean
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE ArchiveNum = @ArchiveNum AND ID <> @ID"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
                    command.Parameters.AddWithValue("@ID", id)
                    Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        Catch ex As Exception
            _lastError = "خطأ في التحقق من تكرار رقم الأرشيف: " & ex.Message
            Throw New Exception(_lastError)
        End Try
    End Function

    ''' <summary>
    ''' التحقق من تكرار الملف
    ''' </summary>
    Public Function CheckDuplicateFile(fileName As String, fileType As String, archiveFolder As String) As Boolean
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE FileName = @FileName AND FileType = @FileType AND ArchiveFolder = @ArchiveFolder"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@FileName", fileName)
                    command.Parameters.AddWithValue("@FileType", fileType)
                    command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
                    Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        Catch ex As Exception
            _lastError = "خطأ في التحقق من تكرار الملف: " & ex.Message
            Throw New Exception(_lastError)
        End Try
    End Function

    ''' <summary>
    ''' الحصول على معلومات الملفات المكررة
    ''' </summary>
    Public Function GetDuplicateFileInfo(fileName As String, fileType As String, archiveFolder As String) As DataTable
        Dim dt As New DataTable()
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT * FROM TbArchive WHERE FileName = @FileName AND FileType = @FileType AND ArchiveFolder = @ArchiveFolder"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@FileName", fileName)
                    command.Parameters.AddWithValue("@FileType", fileType)
                    command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
                    Using adapter As New SqlDataAdapter(command)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            _lastError = "خطأ في جلب معلومات الملفات المكررة: " & ex.Message
            Throw New Exception(_lastError)
        End Try
        Return dt
    End Function

    ''' <summary>
    ''' الحصول على جميع أرشيفات
    ''' </summary>

    Public Function GetAllArchive() As DataTable
        Try
            Return DatabaseHelper.ExecuteStoredProcedure("GetAllArchive")
        Catch ex As Exception
            Throw New Exception("خطأ في الحصول على الأرشيف: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' البحث عن أرشيف باستخدام الإجراء المخزن
    ''' </summary>
    Public Function SearchArchive(searchTerm As String) As DataTable
        Try
            ' التحقق من صحة مصطلح البحث
            If String.IsNullOrWhiteSpace(searchTerm) Then
                Return New DataTable()
            End If

            ' استخدام الإجراء المخزن "SearchArchive"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@SearchTerm", searchTerm)
        }

            ' تنفيذ الإجراء المخزن وإرجاع النتائج
            Return DatabaseHelper.ExecuteStoredProcedure("SearchArchive", parameters)

        Catch ex As Exception
            _lastError = "خطأ في البحث في الأرشيف: " & ex.Message
            Throw New Exception(_lastError)
        End Try
    End Function

    'Public Function GetAllArchives() As DataTable
    '    Dim dt As New DataTable()
    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            Dim query As String = "SELECT * FROM TbArchive ORDER BY ID DESC"
    '            Using command As New SqlCommand(query, connection)
    '                Using adapter As New SqlDataAdapter(command)
    '                    adapter.Fill(dt)
    '                End Using
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        _lastError = "خطأ في جلب جميع الأرشيفات: " & ex.Message
    '        Throw New Exception(_lastError)
    '    End Try
    '    Return dt
    'End Function



    '''' <summary>
    '''' البحث في أرشيفات
    '''' </summary>
    'Public Function SearchArchive(searchTerm As String) As DataTable
    '    Dim dt As New DataTable()
    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            Dim query As String = "SELECT * FROM TbArchive WHERE " &
    '                                  "ArchiveNum LIKE @SearchTerm OR " &
    '                                  "ArchiveSubject LIKE @SearchTerm OR " &
    '                                  "ArchiveFolder LIKE @SearchTerm OR " &
    '                                  "FileName LIKE @SearchTerm"
    '            Using command As New SqlCommand(query, connection)
    '                command.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm & "%")
    '                Using adapter As New SqlDataAdapter(command)
    '                    adapter.Fill(dt)
    '                End Using
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        _lastError = "خطأ في البحث في الأرشيف: " & ex.Message
    '        Throw New Exception(_lastError)
    '    End Try
    '    Return dt
    'End Function

    ''' <summary>
    ''' البحث في أرشيفات حسب نوع الملف
    ''' </summary>
    Public Function GetArchiveByFileType(fileType As String) As DataTable
        Dim dt As New DataTable()
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT * FROM TbArchive WHERE FileType = @FileType ORDER BY ID DESC"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@FileType", fileType)
                    Using adapter As New SqlDataAdapter(command)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            _lastError = "خطأ في البحث في الأرشيف حسب نوع الملف: " & ex.Message
            Throw New Exception(_lastError)
        End Try
        Return dt
    End Function

    '''' <summary>
    '''' تحديث سجل الأرشيف
    '''' </summary>
    'Public Function UpdateArchive(id As Integer, archiveNum As String, archiveSubject As String, archiveFolder As String, archiveDate As Date,
    '                             archiveWillMaster As String, archiveWillSub As String, archiveNotes As String,
    '                             fileName As String, fileType As String, filePath As String, originalFilePath As String,
    '                             userName As String) As Boolean
    '    Dim transaction As SqlTransaction = Nothing
    '    Try
    '        ' تحليل مسار الأرشيف وإنشاء المجلدات المناسبة
    '        Dim finalArchivePath As String = ""

    '        If archiveFolder.Contains("\") Then
    '            ' إذا كان المسار يحتوي على تصنيف (مثل: صادر\2024-12)
    '            Dim parts() As String = archiveFolder.Split("\"c)
    '            If parts.Length >= 2 Then
    '                Dim category As String = parts(0)
    '                Dim subFolder As String = parts(1)
    '                finalArchivePath = CreateSubFolder(category, subFolder)
    '            Else
    '                ' مجرد مجلد فرعي
    '                finalArchivePath = Path.Combine(My.Settings.ARCHIVE_Default_PATH, archiveFolder)
    '                If Not Directory.Exists(finalArchivePath) Then
    '                    Directory.CreateDirectory(finalArchivePath)
    '                End If
    '            End If
    '        Else
    '            ' مجرد اسم مجلد بسيط
    '            finalArchivePath = Path.Combine(My.Settings.ARCHIVE_Default_PATH, archiveFolder)
    '            If Not Directory.Exists(finalArchivePath) Then
    '                Directory.CreateDirectory(finalArchivePath)
    '            End If
    '        End If

    '        ' التحقق من أن اسم الملف يحتوي على الامتداد الصحيح
    '        Dim fileExtension As String = Path.GetExtension(filePath) ' الحصول على الامتداد من المسار الكامل
    '        If Not String.IsNullOrEmpty(fileExtension) AndAlso Not fileName.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) Then
    '            fileName = Path.GetFileNameWithoutExtension(fileName) & fileExtension
    '        End If

    '        ' تحديد المسار النهائي للملف (المسار الحالي بعد النسخ)
    '        Dim destinationPath As String = Path.Combine(finalArchivePath, fileName)
    '        Dim actualOriginalFilePath As String = If(String.IsNullOrEmpty(originalFilePath), filePath, originalFilePath)
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            transaction = connection.BeginTransaction()

    '            Try
    '                ' الحصول على معلومات الأرشيف الحالية
    '                Dim currentArchive As DataTable = GetArchiveById(id)
    '                Dim currentFilePath As String = ""
    '                Dim currentOriginalFilePath As String = ""

    '                If currentArchive.Rows.Count > 0 Then
    '                    currentFilePath = If(currentArchive.Rows(0)("FilePath") IsNot DBNull.Value, currentArchive.Rows(0)("FilePath").ToString(), "")
    '                    currentOriginalFilePath = If(currentArchive.Rows(0)("OriginalFilePath") IsNot DBNull.Value, currentArchive.Rows(0)("OriginalFilePath").ToString(), "")
    '                End If

    '                ' التحقق من وجود الملف المصدر
    '                If File.Exists(filePath) Then
    '                    ' حذف الملف القديم من المسار الحالي إذا كان مختلفًا
    '                    If Not String.IsNullOrEmpty(currentFilePath) AndAlso currentFilePath <> destinationPath AndAlso File.Exists(currentFilePath) Then
    '                        Try
    '                            File.Delete(currentFilePath)
    '                        Catch ex As Exception
    '                            ' في حالة فشل الحذف، نقله إلى مجلد المحذوفات
    '                            'MoveToDeletedFolder(currentFilePath)
    '                        End Try
    '                    End If

    '                    ' نسخ الملف الجديد إلى المجلد المحدد
    '                    File.Copy(filePath, destinationPath, True)
    '                End If

    '                ' تحديث السجل في قاعدة البيانات
    '                Dim query As String = "UPDATE TbArchive SET ArchiveNum = @ArchiveNum, ArchiveDate = @ArchiveDate, ArchiveSubject = @ArchiveSubject, ArchiveNotes = @ArchiveNotes, ArchiveFolder = @ArchiveFolder, FileName = @FileName, FileType = @FileType, FilePath = @FilePath, OriginalFilePath = @OriginalFilePath, ArchiveWillMaster = @ArchiveWillMaster, ArchiveWillSub = @ArchiveWillSub, UserName = @UserName WHERE ID = @ID"

    '                Using command As New SqlCommand(query, connection, transaction)
    '                    command.Parameters.AddWithValue("@ID", id)
    '                    command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
    '                    command.Parameters.AddWithValue("@ArchiveDate", archiveDate)
    '                    command.Parameters.AddWithValue("@ArchiveSubject", archiveSubject)
    '                    command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
    '                    command.Parameters.AddWithValue("@ArchiveNotes", archiveNotes)
    '                    command.Parameters.AddWithValue("@FileName", fileName)
    '                    command.Parameters.AddWithValue("@FileType", fileType)
    '                    command.Parameters.AddWithValue("@FilePath", destinationPath) ' المسار الحالي بعد النسخ
    '                    command.Parameters.AddWithValue("@OriginalFilePath", actualOriginalFilePath) ' المسار الأصلي للملف
    '                    command.Parameters.AddWithValue("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster))
    '                    command.Parameters.AddWithValue("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub))
    '                    command.Parameters.AddWithValue("@UserName", userName)

    '                    Dim rowsAffected As Integer = command.ExecuteNonQuery()

    '                    ' تأكيد المعاملة
    '                    transaction.Commit()

    '                    '' ✅ استدعاء دالة التسجيل من كلاس ActivityLogClass
    '                    'activityLogClass.LogUserActivity(userName, "Edit", "TbArchive", rowsAffected, $"تم تعديل الأرشيف رقم: {archiveNum}, الموضوع: {archiveSubject}")


    '                    Return rowsAffected > 0
    '                End Using
    '            Catch ex As Exception
    '                ' التراجع عن المعاملة في حالة حدوث خطأ
    '                transaction.Rollback()
    '                Throw
    '            End Try
    '        End Using

    '    Catch ex As Exception
    '        _lastError = "خطأ في تعديل الأرشيف: " & ex.Message
    '        Throw New Exception(_lastError)
    '    End Try
    'End Function


    '''' <summary>
    '''' تحديث أرشيف
    '''' </summary>
    'Public Function UpdateArchive(
    '    id As Integer,
    '    archiveNum As String,
    '    archiveSubject As String,
    '    archiveFolder As String,
    '    archiveDate As Date,
    '    archiveWillMaster As String,
    '    archiveWillSub As String,
    '    archiveNotes As String,
    '    filename As String,
    '    fileType As String,
    '    filepath As String,
    '    userName As String) As Boolean

    '    Try
    '        ' التحقق من صحة البيانات
    '        If id <= 0 OrElse String.IsNullOrWhiteSpace(archiveNum) OrElse String.IsNullOrWhiteSpace(userName) Then
    '            Return False
    '        End If

    '        ' التحقق من تكرار رقم الأرشيف (باستثناء السجل الحالي)
    '        If CheckDuplicateArchiveNumberForUpdate(archiveNum, id) Then
    '            Return False
    '        End If

    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            Dim query As String = "UPDATE TbArchive SET " &
    '                                  "ArchiveNum = @ArchiveNum, " &
    '                                  "ArchiveSubject = @ArchiveSubject, " &
    '                                  "ArchiveFolder = @ArchiveFolder, " &
    '                                  "ArchiveDate = @ArchiveDate, " &
    '                                  "ArchiveWillMaster = @ArchiveWillMaster, " &
    '                                  "ArchiveWillSub = @ArchiveWillSub, " &
    '                                  "ArchiveNotes = @ArchiveNotes, " &
    '                                  "FileName = @FileName, " &
    '                                  "FileType = @FileType, " &
    '                                  "FilePath = @FilePath, " &
    '                                  "UserName = @UserName " &
    '                                  "WHERE ID = @ID"

    '            Using command As New SqlCommand(query, connection)
    '                command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
    '                command.Parameters.AddWithValue("@ArchiveSubject", archiveSubject)
    '                command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
    '                command.Parameters.AddWithValue("@ArchiveDate", archiveDate)
    '                command.Parameters.AddWithValue("@ArchiveWillMaster", archiveWillMaster)
    '                command.Parameters.AddWithValue("@ArchiveWillSub", archiveWillSub)
    '                command.Parameters.AddWithValue("@ArchiveNotes", archiveNotes)
    '                command.Parameters.AddWithValue("@FileName", filename)
    '                command.Parameters.AddWithValue("@FileType", fileType)
    '                command.Parameters.AddWithValue("@FilePath", filepath)
    '                command.Parameters.AddWithValue("@UserName", userName)
    '                command.Parameters.AddWithValue("@ID", id)

    '                Dim rowsAffected As Integer = command.ExecuteNonQuery()
    '                Return rowsAffected > 0
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        _lastError = "خطأ في تحديث الأرشيف: " & ex.Message
    '        Throw New Exception(_lastError)
    '    End Try
    'End Function

    '    ''' <summary>
    '    ''' تحديث سجل الأرشيف باستخدام الإجراء المخزن
    '    ''' </summary>
    '    Public Function UpdateArchive(
    '    id As Integer,
    '    archiveNum As String,
    '    archiveSubject As String,
    '    archiveFolder As String,
    '    archiveDate As Date,
    '    archiveWillMaster As String,
    '    archiveWillSub As String,
    '    archiveNotes As String,
    '    fileName As String,
    '    fileType As String,
    '    filePath As String,
    '    originalFilePath As String,
    '    userName As String
    ') As Boolean
    '        Dim transaction As SqlTransaction = Nothing
    '        Try
    '            ' تحليل مسار الأرشيف وإنشاء المجلدات المناسبة
    '            Dim finalArchivePath As String = ""

    '            If archiveFolder.Contains("\") Then
    '                ' إذا كان المسار يحتوي على تصنيف (مثل: صادر\2024-12)
    '                Dim parts() As String = archiveFolder.Split("\"c)
    '                If parts.Length >= 2 Then
    '                    Dim category As String = parts(0)
    '                    Dim subFolder As String = parts(1)
    '                    finalArchivePath = CreateSubFolder(category, subFolder)
    '                Else
    '                    ' مجرد مجلد فرعي
    '                    finalArchivePath = Path.Combine(My.Settings.ARCHIVE_Default_PATH, archiveFolder)
    '                    If Not Directory.Exists(finalArchivePath) Then
    '                        Directory.CreateDirectory(finalArchivePath)
    '                    End If
    '                End If
    '            Else
    '                ' مجرد اسم مجلد بسيط
    '                finalArchivePath = Path.Combine(My.Settings.ARCHIVE_Default_PATH, archiveFolder)
    '                If Not Directory.Exists(finalArchivePath) Then
    '                    Directory.CreateDirectory(finalArchivePath)
    '                End If
    '            End If

    '            ' التحقق من أن اسم الملف يحتوي على الامتداد الصحيح
    '            Dim fileExtension As String = Path.GetExtension(filePath) ' الحصول على الامتداد من المسار الكامل
    '            If Not String.IsNullOrEmpty(fileExtension) AndAlso Not fileName.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) Then
    '                fileName = Path.GetFileNameWithoutExtension(fileName) & fileExtension
    '            End If

    '            ' تحديد المسار النهائي للملف (المسار الحالي بعد النسخ)
    '            Dim destinationPath As String = Path.Combine(finalArchivePath, fileName)
    '            Dim actualOriginalFilePath As String = If(String.IsNullOrEmpty(originalFilePath), filePath, originalFilePath)

    '            Using connection As SqlConnection = GetConnection()
    '                connection.Open()
    '                transaction = connection.BeginTransaction()

    '                Try
    '                    ' الحصول على معلومات الأرشيف الحالية
    '                    Dim currentArchive As DataTable = GetArchiveById(id)
    '                    Dim currentFilePath As String = ""
    '                    Dim currentOriginalFilePath As String = ""

    '                    If currentArchive.Rows.Count > 0 Then
    '                        currentFilePath = If(currentArchive.Rows(0)("FilePath") IsNot DBNull.Value, currentArchive.Rows(0)("FilePath").ToString(), "")
    '                        currentOriginalFilePath = If(currentArchive.Rows(0)("OriginalFilePath") IsNot DBNull.Value, currentArchive.Rows(0)("OriginalFilePath").ToString(), "")
    '                    End If

    '                    ' التحقق من وجود الملف المصدر
    '                    If File.Exists(filePath) Then
    '                        ' حذف الملف القديم من المسار الحالي إذا كان مختلفًا
    '                        If Not String.IsNullOrEmpty(currentFilePath) AndAlso currentFilePath <> destinationPath AndAlso File.Exists(currentFilePath) Then
    '                            Try
    '                                File.Delete(currentFilePath)
    '                            Catch ex As Exception
    '                                ' في حالة فشل الحذف، نقله إلى مجلد المحذوفات
    '                                'MoveToDeletedFolder(currentFilePath)
    '                            End Try
    '                        End If

    '                        ' نسخ الملف الجديد إلى المجلد المحدد
    '                        'File.Copy(filePath, destinationPath, True)
    '                    End If

    '                    ' استخدام الإجراء المخزن "UpdateArchive"
    '                    Dim parameters() As SqlParameter = {
    '                    DatabaseHelper.CreateParameter("@ID", id),
    '                    DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum),
    '                    DatabaseHelper.CreateParameter("@ArchiveSubject", archiveSubject),
    '                    DatabaseHelper.CreateParameter("@ArchiveFolder", archiveFolder),
    '                    DatabaseHelper.CreateParameter("@ArchiveDate", archiveDate),
    '                    DatabaseHelper.CreateParameter("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster)),
    '                    DatabaseHelper.CreateParameter("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub)),
    '                    DatabaseHelper.CreateParameter("@ArchiveNotes", archiveNotes),
    '                    DatabaseHelper.CreateParameter("@filename", fileName),
    '                    DatabaseHelper.CreateParameter("@fileType", fileType),
    '                    DatabaseHelper.CreateParameter("@filepath", destinationPath), ' المسار الحالي بعد النسخ
    '                    DatabaseHelper.CreateParameter("@OriginalFilePath", actualOriginalFilePath), ' المسار الأصلي للملف
    '                    DatabaseHelper.CreateParameter("@UserName", userName)
    '                }

    '                    ' تنفيذ الإجراء المخزن واحصل على عدد الصفوف المؤثرة
    '                    Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("UpdateArchive", parameters, transaction)

    '                    ' تأكيد المعاملة
    '                    transaction.Commit()

    '                    '' ✅ استدعاء دالة التسجيل من كلاس ActivityLogClass
    '                    'activityLogClass.LogUserActivity(userName, "Edit", "TbArchive", id, $"تم تعديل الأرشيف رقم: {archiveNum}, الموضوع: {archiveSubject}")

    '                    Return rowsAffected > 0

    '                Catch ex As Exception
    '                    ' التراجع عن المعاملة في حالة حدوث خطأ
    '                    transaction.Rollback()
    '                    Throw
    '                End Try
    '            End Using

    '        Catch ex As Exception
    '            _lastError = "خطأ في تعديل الأرشيف: " & ex.Message
    '            Throw New Exception(_lastError)
    '        End Try
    '    End Function

    Public Function ArchiveExists(id As Integer) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE ID = @ID"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@ID", id)
        }
            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
            Return count > 0
        Catch ex As Exception
            Throw New Exception("خطأ في التحقق من وجود الأرشيف: " & ex.Message)
        End Try
    End Function

    Public Function GetFileTypes() As List(Of String)
        Dim fileTypes As New List(Of String)()
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT DISTINCT FileType FROM TbArchive ORDER BY FileType"
                Using command As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            If Not reader.IsDBNull(0) Then
                                fileTypes.Add(reader.GetString(0))
                            End If
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            _lastError = "خطأ في جلب أنواع الملفات: " & ex.Message
            Throw New Exception(_lastError)
        End Try
        Return fileTypes
    End Function

    ''' <summary>
    ''' إنشاء مجلدات التصنيف الأساسية
    ''' </summary>
    Public Sub CreateCategoryFolders()
        Try
            ' تحديد مسار الملف (يمكنك تعديله حسب موقع الملف)
            Dim filePath As String = Path.Combine(Application.StartupPath, "BookCategories.txt")

            ' التحقق من وجود الملف
            If Not File.Exists(filePath) Then
                ' إذا لم يكن الملف موجودًا، قم بإنشائه مع القيم الافتراضية
                Dim defaultCategories As String() = {"شركات اخرى", "الجمارك العامة", "مذكرات", "تصدير", "إفراج جمركي"}
                File.WriteAllLines(filePath, defaultCategories)
            End If

            ' قراءة التصنيفات من الملف
            Dim categories As String() = File.ReadAllLines(filePath)

            ' إنشاء المجلد الرئيسي إذا لم يكن موجودًا
            If Not Directory.Exists(My.Settings.ARCHIVE_Default_PATH) Then
                Directory.CreateDirectory(My.Settings.ARCHIVE_Default_PATH)
            End If

            ' إنشاء مجلد المحذوفات
            Dim deletedFolderPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, DELETED_FOLDER_NAME)
            If Not Directory.Exists(deletedFolderPath) Then
                Directory.CreateDirectory(deletedFolderPath)
                System.Diagnostics.Debug.WriteLine($"تم إنشاء مجلد المحذوفات: {deletedFolderPath}")
            End If

            ' إنشاء مجلدات التصنيفات
            For Each category As String In categories
                ' تجاهل الأسطر الفارغة
                If Not String.IsNullOrWhiteSpace(category) Then
                    Dim categoryPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, category.Trim())
                    If Not Directory.Exists(categoryPath) Then
                        Directory.CreateDirectory(categoryPath)
                    End If

                    ' إنشاء مجلدات فرعية حسب الشهر
                    Dim currentMonth As String = DateTime.Now.ToString("yyyy-MM")
                    Dim monthPath As String = Path.Combine(categoryPath, currentMonth)
                    If Not Directory.Exists(monthPath) Then
                        Directory.CreateDirectory(monthPath)
                    End If
                End If
            Next
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء مجلدات التصنيف: {ex.Message}")
        End Try
    End Sub


    'Public Sub CreateCategoryFolders()
    '    Try
    '        ' إنشاء المجلد الرئيسي إذا لم يكن موجودًا
    '        If Not Directory.Exists(My.Settings.ARCHIVE_Default_PATH) Then
    '            Directory.CreateDirectory(My.Settings.ARCHIVE_Default_PATH)
    '        End If

    '        ' إنشاء مجلد المحذوفات
    '        Dim deletedFolderPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, DELETED_FOLDER_NAME)
    '        If Not Directory.Exists(deletedFolderPath) Then
    '            Directory.CreateDirectory(deletedFolderPath)
    '            System.Diagnostics.Debug.WriteLine($"تم إنشاء مجلد المحذوفات: {deletedFolderPath}")
    '        End If

    '        ' إنشاء مجلدات التصنيفات
    '        Dim categories As String() = {"صادر", "وارد", "مذكرات", "قرارات", "تقارير"}
    '        For Each category As String In categories
    '            Dim categoryPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, category)
    '            If Not Directory.Exists(categoryPath) Then
    '                Directory.CreateDirectory(categoryPath)
    '            End If

    '            ' إنشاء مجلدات فرعية حسب الشهر
    '            Dim currentMonth As String = DateTime.Now.ToString("yyyy-MM")
    '            Dim monthPath As String = Path.Combine(categoryPath, currentMonth)
    '            If Not Directory.Exists(monthPath) Then
    '                Directory.CreateDirectory(monthPath)
    '            End If
    '        Next
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء مجلدات التصنيف: {ex.Message}")
    '    End Try
    'End Sub

    ''' <summary>
    ''' التحقق من وجود الملف في المسار قبل الإضافة مع منع التكرار
    ''' </summary>
    Public Function ValidateFileBeforeAddition(
        ByVal fileName As String,
        ByVal fileType As String,
        ByVal archiveFolder As String,
        ByVal originalFilePath As String,
        Optional ByVal currentFilePath As String = "") As Boolean
        Try
            ' التحقق من المدخلات الأساسية
            If String.IsNullOrWhiteSpace(fileName) Then
                MessageBox.Show("يرجى إدخال اسم الملف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            If String.IsNullOrWhiteSpace(fileType) Then
                MessageBox.Show("يرجى إدخال نوع الملف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            If String.IsNullOrWhiteSpace(archiveFolder) Then
                MessageBox.Show("يرجى إدخال مجلد الأرشيف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            ' تحليل مسار الأرشيف للحصول على المسار النهائي
            Dim finalArchivePath As String = GetFinalArchivePath(archiveFolder)
            If String.IsNullOrEmpty(finalArchivePath) Then
                MessageBox.Show("فشل في تحديد مسار الأرشيف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            ' تكوين اسم الملف الكامل
            Dim fileExtension As String = If(fileType.StartsWith("."), fileType, "." & fileType)
            Dim fullFileName As String = If(fileName.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase),
                                          fileName,
                                          fileName & fileExtension)
            Dim destinationPath As String = Path.Combine(finalArchivePath, fullFileName)

            ' استخدام المسار الحالي إذا كان متوفرًا، وإلا استخدام المسار الأصلي
            Dim filePathToCheck As String = String.Empty

            ' التحقق من وجود مسار الملف
            If Not String.IsNullOrWhiteSpace(currentFilePath) Then
                filePathToCheck = currentFilePath
            ElseIf Not String.IsNullOrWhiteSpace(originalFilePath) Then
                filePathToCheck = originalFilePath
            Else
                filePathToCheck = destinationPath
            End If

            ' التحقق من وجود الملف في المسار المحدد
            If File.Exists(destinationPath) Then
                ' التحقق من أن الملف ليس هو نفسه (لمنع التبديل عند التعديل)
                If Not File.Exists(filePathToCheck) OrElse
                   Path.GetFullPath(filePathToCheck).ToLower() <> Path.GetFullPath(destinationPath).ToLower() Then

                    Dim result As DialogResult = MessageBox.Show(
                        $"الملف '{fullFileName}' موجود مسبقًا في المسار: {finalArchivePath}{vbNewLine}{vbNewLine}" &
                        "يرجى تغيير اسم الملف أولاً لتجنب التكرار.{vbNewLine}" &
                        "هل تريد فتح مستعرض الملفات لتغيير الاسم؟",
                        "ملف موجود مسبقًا",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2)

                    Select Case result
                        Case DialogResult.Yes
                            ' فتح مستعرض الملفات للملف الموجود
                            Try
                                Process.Start("explorer.exe", $"/select,""{destinationPath}""")
                            Catch ex As Exception
                                MessageBox.Show($"فشل في فتح موقع الملف: {ex.Message}", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            End Try
                            Return False

                        Case DialogResult.No
                            ' محاولة تغيير الاسم تلقائيًا بإضافة رقم
                            Dim newFileName As String = GetUniqueFileName(finalArchivePath, fullFileName)
                            Dim newDestinationPath As String = Path.Combine(finalArchivePath, newFileName)

                            Dim confirm As DialogResult = MessageBox.Show(
                                $"سيتم تغيير اسم الملف تلقائيًا إلى: {newFileName}{vbNewLine}" &
                                "هل توافق على هذا التغيير؟",
                                "تغيير اسم الملف تلقائيًا",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question)

                            If confirm = DialogResult.Yes Then
                                ' تحديث حقول النموذج
                                UpdateFormFields(newFileName, newDestinationPath)
                                Return True
                            Else
                                Return False
                            End If

                        Case DialogResult.Cancel
                            Return False
                    End Select
                End If
            End If

            ' التحقق من وجود ملف بنفس الاسم في قاعدة البيانات (لتقليل التكرار)
            If CheckDuplicateFileInDatabase(fileName, fileType, archiveFolder) Then
                Dim result As DialogResult = MessageBox.Show(
                    $"يوجد ملف بنفس الاسم '{fullFileName}' في قاعدة البيانات.{vbNewLine}" &
                    "هل تريد المتابعة على الرغم من التكرار؟",
                    "تحذير التكرار",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)

                If result = DialogResult.No Then
                    Return False
                End If
            End If

            Return True

        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء التحقق من الملف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق من وجود ملف مكرر في قاعدة البيانات
    ''' </summary>
    Private Function CheckDuplicateFileInDatabase(fileName As String, fileType As String, archiveFolder As String) As Boolean
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE FileName = @FileName AND FileType = @FileType AND ArchiveFolder = @ArchiveFolder"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@FileName", fileName)
                    command.Parameters.AddWithValue("@FileType", fileType)
                    command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
                    Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من تكرار الملف في قاعدة البيانات: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' الحصول على اسم ملف فريد
    ''' </summary>

    Private Function GetUniqueFileName(folderPath As String, fileName As String) As String
        Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(fileName)
        Dim fileExtension As String = Path.GetExtension(fileName)
        Dim counter As Integer = 1
        Dim maxAttempts As Integer = 1000 ' تحديد حد أقصى للمحاولات

        ' محاولة إيجاد اسم فريد حتى الحد الأقصى للمحاولات
        While counter <= maxAttempts
            Dim newFileName As String = $"{fileNameWithoutExtension} ({counter}){fileExtension}"
            If Not File.Exists(Path.Combine(folderPath, newFileName)) Then
                Return newFileName
            End If
            counter += 1
        End While

        ' إذا فشلنا في العثور على اسم فريد بعد المحاولات القصوى
        ' نستخدم GUID لضمان الفريدة
        Dim randomSuffix As String = Guid.NewGuid().ToString("N").Substring(0, 8)
        Return $"{fileNameWithoutExtension}_{randomSuffix}{fileExtension}"
    End Function


    'Private Function GetUniqueFileName(folderPath As String, fileName As String) As String
    '    Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(fileName)
    '    Dim fileExtension As String = Path.GetExtension(fileName)
    '    Dim counter As Integer = 1

    '    While True
    '        Dim newFileName As String = $"{fileNameWithoutExtension} ({counter}){fileExtension}"
    '        If Not File.Exists(Path.Combine(folderPath, newFileName)) Then
    '            Return newFileName
    '        End If
    '        counter += 1
    '    End While
    'End Function

    'Public Function GetAllArchive() As DataTable
    '    Try
    '        Dim query As String = "SELECT *, TRY_CAST(ID AS INT) AS NumericID FROM TbArchive ORDER BY ArchiveDate DESC"
    '        Dim dt As DataTable = DatabaseHelper.ExecuteQuery(query)

    '        ' إذا كانت NumericID تحتوي على قيم غير صالحة، استبدلها بـ 0
    '        For Each row As DataRow In dt.Rows
    '            If row("NumericID") Is DBNull.Value Then
    '                row("NumericID") = 0
    '            End If
    '        Next

    '        Return dt
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على جميع الأرشيف: {ex.Message}")
    '        Return Nothing
    '    End Try
    'End Function


    ''' <summary>
    ''' جلب أنواع الملفات المميزة
    ''' </summary>
    Public Function GetDistinctFileTypes() As DataTable
        Try
            Dim query As String = "SELECT DISTINCT FileType FROM TbArchive WHERE FileType IS NOT NULL ORDER BY FileType"
            Return DatabaseHelper.ExecuteQuery(query)
        Catch ex As Exception
            _lastError = "خطأ في جلب أنواع الملفات: " & ex.Message
            Throw New Exception(_lastError)
        End Try
    End Function

    ''' <summary>
    ''' الحصول على معلومات الأرشيف المكرر بناءً على رقم الأرشيف
    ''' </summary>
    Public Function GetDuplicateArchiveInfo(archiveNum As String) As DataTable
        Try
            Dim query As String = "SELECT ArchiveNum, ArchiveSubject, ArchiveDate, ArchiveFolder, UserName FROM TbArchive WHERE ArchiveNum = @ArchiveNum"
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum)
            }
            Return DatabaseHelper.ExecuteQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("خطأ في جلب معلومات الأرشيف المكرر: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' فتح موقع الملف
    ''' </summary>
    Public Sub OpenFileLocation(filePath As String)
        Try
            If Not String.IsNullOrEmpty(filePath) AndAlso File.Exists(filePath) Then
                Process.Start("explorer.exe", $"/select,""{filePath}""")
            End If
        Catch ex As Exception
            MessageBox.Show($"فشل في فتح الموقع: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' البحث في الأرشيف حسب النطاق الزمني
    ''' </summary>
    Public Function SearchSaderByDateRange(startDate As Date, endDate As Date) As DataTable
        Try
            ' التأكد من صحة التواريخ
            If startDate > endDate Then
                Dim temp As Date = startDate
                startDate = endDate
                endDate = temp
            End If

            ' استخدام الإجراء المخزن "SearchArchiveByDateRange"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@StartDate", startDate),
            DatabaseHelper.CreateParameter("@EndDate", endDate)
        }

            ' تنفيذ الإجراء المخزن وإرجاع النتائج
            Return DatabaseHelper.ExecuteStoredProcedure("SearchArchiveByDateRange", parameters)

        Catch ex As Exception
            _lastError = "خطأ في البحث في الأرشيف حسب النطاق الزمني: " & ex.Message
            Throw New Exception(_lastError)
        End Try
    End Function


    ''' <summary>
    ''' إنشاء مجلد فرعي بناءً على التصنيف والمجلد الفرعي
    ''' </summary>
    ''' <param name="category">التصنيف (مثل: صادر، وارد)</param>
    ''' <param name="subFolder">اسم المجلد الفرعي (مثل: 2024-12)</param>
    ''' <returns>المسار الكامل للمجلد الفرعي</returns>
    Public Function CreateSubFolder(category As String, subFolder As String) As String
        Try
            ' إنشاء مسار المجلد الرئيسي
            Dim categoryPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, category)

            ' إنشاء مسار المجلد الفرعي
            Dim subFolderPath As String = Path.Combine(categoryPath, subFolder)

            ' إنشاء المجلدات إذا لم تكن موجودة
            If Not Directory.Exists(categoryPath) Then
                Directory.CreateDirectory(categoryPath)
            End If

            If Not Directory.Exists(subFolderPath) Then
                Directory.CreateDirectory(subFolderPath)
            End If

            Return subFolderPath
        Catch ex As Exception
            Throw New Exception("خطأ في إنشاء المجلد الفرعي: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' الحصول على مسار الملف من جدول الأرشيف
    ''' </summary>
    ''' <param name="archiveID">معرف الأرشيف</param>
    ''' <returns>مسار الملف أو سلسلة فارغة إذا لم يتم العثور على الأرشيف</returns>
    Public Function GetFilePathByArchiveID(archiveID As Integer) As String
        Try
            Dim dt As DataTable = GetArchiveById(archiveID)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                ' الحصول على مسار الملف الحالي (FilePath)
                If dt.Rows(0)("FilePath") IsNot DBNull.Value AndAlso
               Not String.IsNullOrEmpty(dt.Rows(0)("FilePath").ToString()) Then
                    Return dt.Rows(0)("FilePath").ToString()
                End If
            End If
            Return String.Empty
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetFilePathByArchiveID: {ex.Message}")
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' تحديث حقول النموذج
    ''' </summary>
    Private Sub UpdateFormFields(newFileName As String, newDestinationPath As String)
        ' سيتم تنفيذ هذه الدالة في النموذج الرئيسي
        ' هنا نضع الإطار العام فقط
    End Sub

    ''' <summary>
    ''' الحصول على آخر خطأ
    ''' </summary>
    Public Function GetLastError() As String
        Return _lastError
    End Function

#Region " التقارير"

    ''' <summary>
    ''' الحصول على تصنيفات الأرشيف
    ''' </summary>
    Public Function GetArchiveCategories() As List(Of String)
        Dim categories As New List(Of String)
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT DISTINCT BookCategory FROM TbSader ORDER BY BookCategory"
                Using command As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            If Not reader.IsDBNull(0) Then
                                categories.Add(reader.GetString(0))
                            End If
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على تصنيفات الأرشيف: {ex.Message}")
        End Try
        Return categories
    End Function

    ''' <summary>
    ''' الحصول على مجلدات الأرشيف
    ''' </summary>
    Public Function GetArchiveFolders() As List(Of String)
        Dim folders As New List(Of String)
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT DISTINCT ArchiveFolder FROM TbArchive ORDER BY ArchiveFolder"
                Using command As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            If Not reader.IsDBNull(0) Then
                                folders.Add(reader.GetString(0))
                            End If
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على مجلدات الأرشيف: {ex.Message}")
        End Try
        Return folders
    End Function

#End Region

#Region "الاكواد الجديدة  لتغيير استراتيجية الحذف ( الحذف المنطقي)"

    ''' <summary>
    ''' نقل الملف إلى مجلد المحذوفات مع تحسينات لضمان النجاح
    ''' </summary>
    Private Function MoveFileToDeletedFolder(
    ByVal sourceFilePath As String,
    Optional ByVal archiveNum As String = "",
    Optional ByVal showMessages As Boolean = True) As String

        Try
            ' التحقق من وجود الملف
            If String.IsNullOrEmpty(sourceFilePath) Then
                System.Diagnostics.Debug.WriteLine("مسار الملف فارغ")
                Return ""
            End If

            ' التأكد من أن المسار كاملاً
            Dim fullPath As String = Path.GetFullPath(sourceFilePath)
            If Not File.Exists(fullPath) Then
                System.Diagnostics.Debug.WriteLine($"الملف غير موجود: {fullPath}")
                Return ""
            End If

            ' الحصول على المسار الأساسي للأرشيف
            Dim archiveBasePath As String = IO.Path.Combine(My.Settings.ARCHIVE_Default_PATH, DELETED_FOLDER_NAME)
            If String.IsNullOrEmpty(archiveBasePath) Then
                Throw New Exception("مسار الأرشيف الافتراضي غير معرف في الإعدادات")
            End If

            ' التأكد من أن المسار الأساسي موجود
            archiveBasePath = Path.GetFullPath(archiveBasePath)
            If Not Directory.Exists(archiveBasePath) Then
                Throw New Exception($"مسار الأرشيف الافتراضي غير موجود: {archiveBasePath}")
            End If

            ' تحديد مسار مجلد المحذوفات
            Dim deletedFolderPath As String = Path.Combine(archiveBasePath)
            deletedFolderPath = Path.GetFullPath(deletedFolderPath)

            ' إنشاء مجلد المحذوفات إذا لم يكن موجودًا
            If Not Directory.Exists(deletedFolderPath) Then
                Directory.CreateDirectory(deletedFolderPath)
                System.Diagnostics.Debug.WriteLine($"تم إنشاء مجلد المحذوفات: {deletedFolderPath}")
            End If

            ' إنشاء اسم ملف فريد
            Dim fileName As String = Path.GetFileName(fullPath)
            Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(fileName)
            Dim fileExtension As String = Path.GetExtension(fileName)
            Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
            Dim newFileName As String = $"{fileNameWithoutExtension}_deleted_{timestamp}{fileExtension}"
            Dim destinationFilePath As String = Path.Combine(deletedFolderPath, newFileName)

            ' التحقق من أن المسار الوجهة مختلف عن المصدر
            If String.Equals(fullPath, destinationFilePath, StringComparison.OrdinalIgnoreCase) Then
                System.Diagnostics.Debug.WriteLine("مسار المصدر والوجهة متطابقان")
                Return ""
            End If

            ' محاولة إغلاق الملف إذا كان مفتوحًا
            CloseFileIfOpen(fullPath)

            ' محاولة نقل الملف مع إعادة المحاولة
            Dim maxRetries As Integer = 5 ' زيادة عدد المحاولات إلى 5
            Dim retryCount As Integer = 0
            Dim success As Boolean = False
            Dim lastException As Exception = Nothing

            While retryCount < maxRetries AndAlso Not success
                Try
                    ' التحقق من أن الملف مغلق
                    Using fs As FileStream = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.None)
                        ' إذا نجحت، فالمشكلة ليست في الاستخدام
                    End Using
                Catch ex As IOException
                    ' الملف قيد الاستخدام، الانتظار وإعادة المحاولة
                    retryCount += 1
                    Threading.Thread.Sleep(1000) ' زيادة الوقت بين المحاولات إلى 1000 مللي ثانية
                    Continue While
                End Try

                Try
                    ' محاولة نقل الملف
                    File.Move(fullPath, destinationFilePath)
                    success = True
                Catch ex As Exception
                    lastException = ex
                    retryCount += 1
                    Threading.Thread.Sleep(1000)
                End Try
            End While

            If success Then
                System.Diagnostics.Debug.WriteLine($"تم نقل الملف إلى: {destinationFilePath}")
                Return destinationFilePath
            Else
                If lastException IsNot Nothing Then

                    ' 🔥 هنا مكان استخدام دالة GetFileMoveFailureReason
                    ' إذا فشلت عملية النقل
                    Dim failureReason As String = GetFileMoveFailureReason(sourceFilePath)
                    If showMessages Then
                        MessageBox.Show(
                            $"فشل في نقل الملف إلى مجلد المحذوفات:{vbNewLine}" &
                            $"{failureReason}{vbNewLine}{vbNewLine}" &
                            "الملف لا يزال موجودًا في مساره الأصلي. يرجى معالجته يدويًا.",
                            "تحذير",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
                    End If

                    Throw lastException
                Else
                    Throw New Exception("فشل في نقل الملف بعد عدة محاولات")
                End If
            End If
        Catch ex As IOException
            ' الملف قيد الاستخدام
            System.Diagnostics.Debug.WriteLine($"الملف قيد الاستخدام: {ex.Message}")
            Throw New Exception($"الملف قيد الاستخدام ولا يمكن نقله: {ex.Message}")
        Catch ex As UnauthorizedAccessException
            ' مشكلة في الصلاحيات
            System.Diagnostics.Debug.WriteLine($"مشكلة في الصلاحيات: {ex.Message}")
            Throw New Exception($"ليس لديك صلاحيات كافية لنقل الملف: {ex.Message}")
        Catch ex As Exception
            ' أخطاء أخرى
            System.Diagnostics.Debug.WriteLine($"خطأ عام في نقل الملف: {ex.Message}")
            Throw New Exception($"فشل في نقل الملف: {ex.Message}")
        End Try
    End Function

    ''' <summary>
    ''' محاولة إغلاق الملف إذا كان مفتوحًا بواسطة عملية أخرى
    ''' </summary>
    Private Sub CloseFileIfOpen(filePath As String)
        Try
            ' التحقق من أن الملف موجود
            If Not File.Exists(filePath) Then
                Return
            End If

            ' محاولة إغلاق الملف باستخدام تقنيات متقدمة
            Try
                ' محاولة 1: استخدام FileShare.None للكشف عن الملفات المفتوحة
                Using fs As FileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None)
                    ' إذا نجحت، فالمشكلة ليست في الاستخدام
                End Using
            Catch ex As IOException
                System.Diagnostics.Debug.WriteLine($"الملف قيد الاستخدام، سيتم محاولة إغلاقه: {ex.Message}")

                Try
                    ' محاولة 2: استخدام Process Explorer للكشف عن العمليات
                    Dim processName As String = GetProcessUsingFile(filePath)
                    If Not String.IsNullOrEmpty(processName) Then
                        System.Diagnostics.Debug.WriteLine($"الملف مفتوح بواسطة: {processName}")

                        ' إذا كانت العملية هي Adobe Reader، نحاول إغلاقها بحذر
                        If processName.IndexOf("AcroRd32", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                       processName.IndexOf("Adobe", StringComparison.OrdinalIgnoreCase) >= 0 Then
                            Try
                                ForceCloseAdobeReader()
                            Catch
                                ' تجاهل الأخطاء، نحاول فقط
                            End Try
                        End If
                    End If
                Catch ex2 As Exception
                    System.Diagnostics.Debug.WriteLine($"فشل في تحديد العملية المستخدمة للملف: {ex2.Message}")
                End Try
            End Try
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في محاولة إغلاق الملف: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' ربط الأرشيف بالصادر
    ''' </summary>
    Public Function UpdateArchiveSaderId(archiveNum As String, saderId As Integer) As Boolean
        Try
            Dim query As String = "UPDATE TbArchive SET SaderID = @SaderID WHERE ArchiveNum = @ArchiveNum"
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
                    command.Parameters.AddWithValue("@SaderID", saderId)
                    Return command.ExecuteNonQuery() > 0
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateArchiveSaderId: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' الحصول على اسم العملية التي تستخدم الملف
    ''' </summary>
    Private Function GetProcessUsingFile(filePath As String) As String
        Try
            ' استخدام WMI للحصول على العمليات التي تستخدم الملف
            Dim wmiQuery As String = $"SELECT * FROM Win32_Process WHERE Handle LIKE '%{filePath}%'"
            Using searcher As New ManagementObjectSearcher(wmiQuery)
                For Each obj As ManagementObject In searcher.Get()
                    Return obj("Name").ToString()
                Next
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"فشل في الحصول على العملية المستخدمة للملف: {ex.Message}")
        End Try
        Return ""
    End Function

    ''' <summary>
    ''' إجبار إغلاق برنامج Adobe Reader (إذا كان مفتوحًا)
    ''' </summary>
    Private Sub ForceCloseAdobeReader()
        Try
            ' البحث عن عمليات Adobe Reader
            For Each process As Process In Process.GetProcessesByName("AcroRd32")
                ' محاولة إغلاق العملية
                process.Kill()
            Next
            For Each process As Process In Process.GetProcessesByName("Acrobat")
                ' محاولة إغلاق العملية
                process.Kill()
            Next
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"فشل في إغلاق Adobe Reader: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديد سبب فشل نقل الملف بدقة أكبر
    ''' </summary>
    Private Function GetFileMoveFailureReason(filePath As String) As String
        Try
            ' التحقق من وجود الملف
            If Not File.Exists(filePath) Then
                Return "الملف غير موجود في المسار المحدد"
            End If

            ' التحقق من أن الملف ليس قيد الاستخدام
            Try
                Using fs As FileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None)
                    ' إذا نجحت، فالمشكلة ليست في الاستخدام
                End Using
            Catch ex As IOException When ex.Message.Contains("process cannot access the file")
                Return "الملف قيد الاستخدام من قبل برنامج آخر (مثل Adobe Reader لملفات PDF)"
            Catch ex As IOException
                Return "الملف قيد الاستخدام من قبل عملية نظام"
            End Try

            ' التحقق من وجود مجلد المحذوفات
            Dim deletedFolderPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, DELETED_FOLDER_NAME)
            If Not Directory.Exists(deletedFolderPath) Then
                Try
                    Directory.CreateDirectory(deletedFolderPath)
                    System.Diagnostics.Debug.WriteLine($"تم إنشاء مجلد المحذوفات: {deletedFolderPath}")
                Catch ex As Exception
                    Return $"فشل في إنشاء مجلد المحذوفات: {ex.Message}"
                End Try
            End If

            ' التحقق من الصلاحيات
            Dim testPath As String = Path.Combine(deletedFolderPath, "permission_test.tmp")
            Try
                File.WriteAllText(testPath, "test")
                File.Delete(testPath)
            Catch ex As UnauthorizedAccessException
                Return $"صلاحيات غير كافية للكتابة في مجلد المحذوفات: {deletedFolderPath}"
            Catch ex As Exception
                Return $"خطأ في الوصول إلى مجلد المحذوفات: {ex.Message}"
            End Try

            ' التحقق من مساحة القرص
            Dim driveInfo As New DriveInfo(Path.GetPathRoot(filePath))
            If driveInfo.AvailableFreeSpace < 1024 * 1024 Then ' أقل من 1 ميجابايت
                Return "مساحة القرص غير كافية لنقل الملف"
            End If

            ' إذا لم تكن أي من الأسباب السابقة، فربما مشكلة أخرى
            Return "سبب غير معروف (قد يكون الملف قيد الاستخدام من قبل عملية نظام)"
        Catch ex As Exception
            Return $"خطأ غير متوقع: {ex.Message}"
        End Try
    End Function


    '''' <summary>
    '''' إخفاء الأرشيف (حذف منطقي) مع تحديث حالة السجل
    '''' </summary>
    'Public Function HideArchiveById(
    '            ByVal archiveId As Integer,
    '            ByVal userName As String,
    '            Optional ByVal showMessages As Boolean = True) As Boolean

    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()

    '            Dim query As String = "UPDATE TbArchive SET " &
    '                                  "IsDeleted = 1, " &
    '                                  "DeletedDate = @DeletedDate, " &
    '                                  "DeletedBy = @DeletedBy " &
    '                                  "WHERE ID = @ID"

    '            Using command As New SqlCommand(query, connection)
    '                command.Parameters.AddWithValue("@DeletedDate", DateTime.Now)
    '                command.Parameters.AddWithValue("@DeletedBy", userName)
    '                command.Parameters.AddWithValue("@ID", archiveId)

    '                Dim rowsAffected As Integer = command.ExecuteNonQuery()

    '                If showMessages AndAlso rowsAffected > 0 Then
    '                    MessageBox.Show(
    '                        "تم إخفاء الأرشيف بنجاح. يمكن استرجاعه لاحقًا من قائمة المحذوفات.",
    '                        "نجاح",
    '                        MessageBoxButtons.OK,
    '                        MessageBoxIcon.Information)
    '                End If

    '                ' تحديث عداد الأرشيف
    '                DataHelper.UpdateArchiveCount()

    '                ' ✅ الآن يمكن استخدام archiveSubject لأنه تم تعريفه
    '                activityLogClass.LogUserActivity(GlobalData.LoggedInUserName, "Delete", "TbArchive", archiveId, $"تم حذف الأرشيف رقم: {archiveNum}, الموضوع: {archiveSubject}")

    '                Return rowsAffected > 0
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        _lastError = $"حدث خطأ أثناء إخفاء الأرشيف بمعرف {archiveId}: {ex.Message}"
    '        System.Diagnostics.Debug.WriteLine(_lastError)

    '        If showMessages Then
    '            MessageBox.Show(
    '                $"حدث خطأ أثناء إخفاء الأرشيف: {ex.Message}",
    '                "خطأ",
    '                MessageBoxButtons.OK,
    '                MessageBoxIcon.Error)
    '        End If

    '        Return False
    '    End Try
    'End Function

    '''' <summary>
    '''' إخفاء الأرشيف (حذف منطقي) مع تحديث حالة السجل
    '''' </summary>
    'Public Function HideArchiveById(
    'ByVal archiveId As Integer,
    'ByVal userName As String,
    'Optional ByVal showMessages As Boolean = True) As Boolean

    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()

    '            ' 1. الحصول على معلومات الأرشيف قبل التعديل
    '            Dim archiveNum As String = ""
    '            Dim archiveSubject As String = ""
    '            Dim currentFilePath As String = ""

    '            Dim getInfoQuery As String = "SELECT ArchiveNum, ArchiveSubject, FilePath FROM TbArchive WHERE ID = @ID"
    '            Using getInfoCommand As New SqlCommand(getInfoQuery, connection)
    '                getInfoCommand.Parameters.AddWithValue("@ID", archiveId)
    '                Using reader As SqlDataReader = getInfoCommand.ExecuteReader()
    '                    If reader.Read() Then
    '                        archiveNum = If(reader("ArchiveNum") IsNot DBNull.Value, reader("ArchiveNum").ToString(), $"المعرف {archiveId}")
    '                        archiveSubject = If(reader("ArchiveSubject") IsNot DBNull.Value, reader("ArchiveSubject").ToString(), "موضوع غير محدد")
    '                        currentFilePath = If(reader("FilePath") IsNot DBNull.Value, reader("FilePath").ToString(), "")
    '                    Else
    '                        Throw New Exception($"الأرشيف بمعرف {archiveId} غير موجود")
    '                    End If
    '                End Using
    '            End Using

    '            ' 2. تنفيذ عملية الإخفاء (الحذف المنطقي)
    '            Dim updateQuery As String = "UPDATE TbArchive SET " &
    '                                "IsDeleted = 1, " &
    '                                "DeletedDate = @DeletedDate, " &
    '                                "DeletedBy = @DeletedBy " &
    '                                "WHERE ID = @ID"

    '            Using command As New SqlCommand(updateQuery, connection)
    '                command.Parameters.AddWithValue("@DeletedDate", DateTime.Now)
    '                command.Parameters.AddWithValue("@DeletedBy", userName)
    '                command.Parameters.AddWithValue("@ID", archiveId)

    '                Dim rowsAffected As Integer = command.ExecuteNonQuery()

    '                ' 3. نقل الملف إلى مجلد المحذوفات إذا كان هناك ملف
    '                If Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath) Then
    '                    Try
    '                        ' تحديد مسار مجلد المحذوفات
    '                        Dim deletedFolderPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, "المحذوفات")

    '                        ' إنشاء مجلد المحذوفات إذا لم يكن موجودًا
    '                        If Not Directory.Exists(deletedFolderPath) Then
    '                            Directory.CreateDirectory(deletedFolderPath)
    '                        End If

    '                        ' تكوين اسم الملف الجديد (مع تاريخ ووقت لتجنب التكرار)
    '                        Dim fileName As String = Path.GetFileName(currentFilePath)
    '                        Dim fileExtension As String = Path.GetExtension(currentFilePath)
    '                        Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
    '                        Dim newFileName As String = $"{Path.GetFileNameWithoutExtension(fileName)}_{timestamp}{fileExtension}"
    '                        Dim newFilePath As String = Path.Combine(deletedFolderPath, newFileName)

    '                        ' نقل الملف
    '                        File.Move(currentFilePath, newFilePath)

    '                        If showMessages Then
    '                            MessageBox.Show(
    '                        "تم إخفاء الأرشيف بنجاح. يمكن استرجاعه لاحقًا من قائمة المحذوفات.",
    '                        "نجاح",
    '                        MessageBoxButtons.OK,
    '                        MessageBoxIcon.Information)
    '                        End If

    '                    Catch ex As Exception
    '                        ' إذا فشل نقل الملف، أعد تعيين حالة السجل
    '                        ' أو سجل الخطأ
    '                        System.Diagnostics.Debug.WriteLine($"فشل في نقل الملف: {ex.Message}")
    '                        ' يمكنك اختيار عدم إظهار رسالة خطأ للمستخدم هنا
    '                    End Try
    '                Else
    '                    ' إذا لم يكن هناك ملف، فقط عدّ العملية ناجحة
    '                    If showMessages Then
    '                        MessageBox.Show(
    '                    "تم إخفاء الأرشيف بنجاح. لا يوجد ملف مرتبط.",
    '                    "نجاح",
    '                    MessageBoxButtons.OK,
    '                    MessageBoxIcon.Information)
    '                    End If
    '                End If

    '                ' تحديث عداد الأرشيف
    '                DataHelper.UpdateArchiveCount()

    '                ' ✅ تسجيل النشاط
    '                activityLogClass.LogUserActivity(userName, "Delete", "TbArchive", archiveId, $"تم حذف الأرشيف رقم: {archiveNum}, الموضوع: {archiveSubject}")

    '                Return rowsAffected > 0

    '            End Using
    '        End Using

    '    Catch ex As Exception
    '        _lastError = $"حدث خطأ أثناء إخفاء الأرشيف بمعرف {archiveId}: {ex.Message}"
    '        System.Diagnostics.Debug.WriteLine(_lastError)

    '        If showMessages Then
    '            MessageBox.Show(
    '        $"حدث خطأ أثناء إخفاء الأرشيف: {ex.Message}",
    '        "خطأ",
    '        MessageBoxButtons.OK,
    '        MessageBoxIcon.Error)
    '        End If

    '        Return False
    '    End Try
    'End Function



    'Public Function HideArchiveById(
    '        ByVal archiveId As Integer,
    '        ByVal userName As String,
    '        Optional ByVal showMessages As Boolean = True) As Boolean

    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()

    '            ' 1. الحصول على معلومات الأرشيف قبل التعديل
    '            Dim archiveNum As String = ""
    '            Dim archiveSubject As String = ""

    '            Dim getInfoQuery As String = "SELECT ArchiveNum, ArchiveSubject FROM TbArchive WHERE ID = @ID"
    '            Using getInfoCommand As New SqlCommand(getInfoQuery, connection)
    '                getInfoCommand.Parameters.AddWithValue("@ID", archiveId)
    '                Using reader As SqlDataReader = getInfoCommand.ExecuteReader()
    '                    If reader.Read() Then
    '                        archiveNum = If(reader("ArchiveNum") IsNot DBNull.Value, reader("ArchiveNum").ToString(), $"المعرف {archiveId}")
    '                        archiveSubject = If(reader("ArchiveSubject") IsNot DBNull.Value, reader("ArchiveSubject").ToString(), "موضوع غير محدد")
    '                    Else
    '                        Throw New Exception($"الأرشيف بمعرف {archiveId} غير موجود")
    '                    End If
    '                End Using
    '            End Using

    '            ' 2. تنفيذ عملية الإخفاء (الحذف المنطقي)
    '            Dim updateQuery As String = "UPDATE TbArchive SET " &
    '                                    "IsDeleted = 1, " &
    '                                    "DeletedDate = @DeletedDate, " &
    '                                    "DeletedBy = @DeletedBy " &
    '                                    "WHERE ID = @ID"

    '            Using command As New SqlCommand(updateQuery, connection)
    '                command.Parameters.AddWithValue("@DeletedDate", DateTime.Now)
    '                command.Parameters.AddWithValue("@DeletedBy", userName)
    '                command.Parameters.AddWithValue("@ID", archiveId)

    '                Dim rowsAffected As Integer = command.ExecuteNonQuery()

    '                If showMessages AndAlso rowsAffected > 0 Then
    '                    MessageBox.Show(
    '                    "تم إخفاء الأرشيف بنجاح. يمكن استرجاعه لاحقًا من قائمة المحذوفات.",
    '                    "نجاح",
    '                    MessageBoxButtons.OK,
    '                    MessageBoxIcon.Information)
    '                End If

    '                ' تحديث عداد الأرشيف
    '                DataHelper.UpdateArchiveCount()

    '                ' ✅ الآن يمكن استخدام archiveSubject و archiveNum لأنها تم تعريفها
    '                activityLogClass.LogUserActivity(userName, "Delete", "TbArchive", archiveId, $"تم حذف الأرشيف رقم: {archiveNum}, الموضوع: {archiveSubject}")

    '                Return rowsAffected > 0
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        _lastError = $"حدث خطأ أثناء إخفاء الأرشيف بمعرف {archiveId}: {ex.Message}"
    '        System.Diagnostics.Debug.WriteLine(_lastError)

    '        If showMessages Then
    '            MessageBox.Show(
    '            $"حدث خطأ أثناء إخفاء الأرشيف: {ex.Message}",
    '            "خطأ",
    '            MessageBoxButtons.OK,
    '            MessageBoxIcon.Error)
    '        End If

    '        Return False
    '    End Try
    'End Function

    ''' <summary>
    ''' جلب الأرشيفات النشطة (غير المحذوفة)
    ''' </summary>
    Public Function GetActiveArchives() As DataTable
        Try
            ' استخدام الإجراء المخزن "GetActiveArchives"
            Return DatabaseHelper.ExecuteStoredProcedure("GetActiveArchives")
        Catch ex As Exception
            _lastError = "خطأ في جلب الأرشيفات النشطة: " & ex.Message
            Throw New Exception(_lastError)
        End Try
    End Function
    '''' <summary>
    '''' جلب الأرشيفات النشطة (غير المحذوفة)
    '''' </summary>
    'Public Function GetActiveArchives() As DataTable
    '    Dim dt As New DataTable()
    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            Dim query As String = "SELECT * FROM TbArchive WHERE IsDeleted = 0 ORDER BY ID"
    '            Using command As New SqlCommand(query, connection)
    '                Using adapter As New SqlDataAdapter(command)
    '                    adapter.Fill(dt)
    '                End Using
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        _lastError = "خطأ في جلب الأرشيفات النشطة: " & ex.Message
    '        Throw New Exception(_lastError)
    '    End Try
    '    Return dt
    'End Function

    ''' <summary>
    ''' جلب الأرشيفات المحذوفة (المخفية)
    ''' </summary>
    Public Function GetDeletedArchives() As DataTable
        Dim dt As New DataTable()
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT * FROM TbArchive WHERE IsDeleted = 1 ORDER BY DeletedDate DESC"
                Using command As New SqlCommand(query, connection)
                    Using adapter As New SqlDataAdapter(command)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            _lastError = "خطأ في جلب الأرشيفات المحذوفة: " & ex.Message
            Throw New Exception(_lastError)
        End Try
        Return dt
    End Function

    ''' <summary>
    ''' استعادة أرشيف من سلة المحذوفات
    ''' </summary>
    Public Function RestoreArchiveById(
    ByVal archiveId As Integer,
    ByVal userName As String,
    Optional ByVal showMessages As Boolean = True) As Boolean

        Try
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@ID", archiveId),
            DatabaseHelper.CreateParameter("@RestoredBy", userName)
        }

            ' تنفيذ الإجراء المخزن واحصل على نتيجة الاستعادة
            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("RestoreArchive", parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                ' تحديث عداد الأرشيف
                DataHelper.UpdateArchiveCount()

                ' تسجيل نشاط إضافة المصدر
                activityLogClass.LogUserActivity(userName, "Restore", "TbArchive", archiveId, $"تم استعادة الأرشيف رقم: {archiveId}")

                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            _lastError = $"حدث خطأ أثناء استعادة الأرشيف بمعرف {archiveId}: {ex.Message}"
            System.Diagnostics.Debug.WriteLine(_lastError)

            If showMessages Then
                MessageBox.Show(
                $"حدث خطأ أثناء استعادة الأرشيف: {ex.Message}",
                "خطأ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
            End If

            Return False
        End Try
    End Function

    '''' <summary>
    '''' استعادة أرشيف من سلة المحذوفات
    '''' </summary>
    'Public Function RestoreArchiveById(
    'ByVal archiveId As Integer,
    'ByVal userName As String,
    'Optional ByVal showMessages As Boolean = True) As Boolean

    '    Try
    '        Dim parameters() As SqlParameter = {
    '        DatabaseHelper.CreateParameter("@ID", archiveId),
    '        DatabaseHelper.CreateParameter("@RestoredBy", userName)
    '    }

    '        ' تنفيذ الإجراء المخزن واحصل على نتيجة الاستعادة
    '        Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("RestoreArchive", parameters)

    '        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
    '            ' تحديث عداد الأرشيف
    '            DataHelper.UpdateArchiveCount()

    '            ' تسجيل نشاط إضافة المصدر
    '            activityLogClass.LogUserActivity(userName, "Restore", "TbArchive", archiveId, $"تم استعادة الأرشيف رقم: {archiveId}")

    '            Return True
    '        Else
    '            Return False
    '        End If

    '    Catch ex As Exception
    '        _lastError = $"حدث خطأ أثناء استعادة الأرشيف بمعرف {archiveId}: {ex.Message}"
    '        System.Diagnostics.Debug.WriteLine(_lastError)

    '        If showMessages Then
    '            MessageBox.Show(
    '            $"حدث خطأ أثناء استعادة الأرشيف: {ex.Message}",
    '            "خطأ",
    '            MessageBoxButtons.OK,
    '            MessageBoxIcon.Error)
    '        End If

    '        Return False
    '    End Try
    'End Function

    '''' <summary>
    '''' استرجاع الأرشيف (إلغاء الحذف المنطقي)
    '''' </summary>
    'Public Function RestoreArchiveById(
    '    ByVal archiveId As Integer,
    '    ByVal userName As String,
    '    Optional ByVal showMessages As Boolean = True) As Boolean

    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()

    '            Dim query As String = "UPDATE TbArchive SET " &
    '                                  "IsDeleted = 0, " &
    '                                  "DeletedDate = NULL, " &
    '                                  "DeletedBy = NULL " &
    '                                  "WHERE ID = @ID"

    '            Using command As New SqlCommand(query, connection)
    '                command.Parameters.AddWithValue("@ID", archiveId)

    '                Dim rowsAffected As Integer = command.ExecuteNonQuery()

    '                If showMessages AndAlso rowsAffected > 0 Then
    '                    MessageBox.Show(
    '                        "تم استرجاع الأرشيف بنجاح.",
    '                        "نجاح",
    '                        MessageBoxButtons.OK,
    '                        MessageBoxIcon.Information)
    '                End If

    '                ' تحديث عداد الأرشيف
    '                DataHelper.UpdateArchiveCount()

    '                ' تحديث عداد الأرشيفات المحذوفة
    '                DataHelper.UpdateDeletedArchiveCount()

    '                Return rowsAffected > 0
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        _lastError = $"حدث خطأ أثناء استرجاع الأرشيف بمعرف {archiveId}: {ex.Message}"
    '        System.Diagnostics.Debug.WriteLine(_lastError)

    '        If showMessages Then
    '            MessageBox.Show(
    '                $"حدث خطأ أثناء استرجاع الأرشيف: {ex.Message}",
    '                "خطأ",
    '                MessageBoxButtons.OK,
    '                MessageBoxIcon.Error)
    '        End If

    '        Return False
    '    End Try
    'End Function

    ''' <summary>
    ''' حذف الأرشيف نهائيًا بعد فترة محددة (للحذف الدائم)
    ''' </summary>
    Public Function PermanentlyDeleteArchives(
        ByVal daysToKeep As Integer,
        Optional ByVal showMessages As Boolean = True) As Integer

        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()

                Dim cutoffDate As DateTime = DateTime.Now.AddDays(-daysToKeep)
                Dim query As String = "DELETE FROM TbArchive WHERE IsDeleted = 1 AND DeletedDate < @CutoffDate"

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@CutoffDate", cutoffDate)
                    Dim rowsAffected As Integer = command.ExecuteNonQuery()

                    If showMessages AndAlso rowsAffected > 0 Then
                        MessageBox.Show(
                            $"تم حذف {rowsAffected} أرشيف محذوف نهائيًا (أقدم من {daysToKeep} يوم).",
                            "تنظيف تلقائي",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
                    End If

                    ' تحديث عداد الأرشيف
                    DataHelper.UpdateArchiveCount()

                    ' تحديث عداد الأرشيفات المحذوفة
                    DataHelper.UpdateDeletedArchiveCount()

                    Return rowsAffected
                End Using
            End Using
        Catch ex As Exception
            _lastError = $"حدث خطأ أثناء التنظيف التلقائي: {ex.Message}"
            System.Diagnostics.Debug.WriteLine(_lastError)

            If showMessages Then
                MessageBox.Show(
                    $"حدث خطأ أثناء التنظيف التلقائي: {ex.Message}",
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error)
            End If

            Return 0
        End Try
    End Function

    ''' <summary>
    ''' الحصول على عدد الأيام المتبقية قبل الحذف التلقائي
    ''' </summary>
    Public Function GetDaysUntilDeletion() As Integer
        Try
            Dim daysToKeep As Integer = My.Settings.daysToKeep ' يمكنك جعل هذا رقمًا قابلاً للتغيير من الإعدادات
            Dim cutoffDate As DateTime = DateTime.Now.AddDays(-daysToKeep)
            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE IsDeleted = 1 AND DeletedDate < @CutoffDate"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@CutoffDate", cutoffDate)
        }

            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
            Return count
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetDaysUntilDeletion: {ex.Message}")
            Return 0
        End Try
    End Function


    ''' <summary>
    ''' البحث في الأرشيفات النشطة (غير المحذوفة)
    ''' </summary>
    Public Function SearchActiveArchive(searchTerm As String) As DataTable
        Dim dt As New DataTable()
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT * FROM TbArchive WHERE IsDeleted = 0 AND " &
                                      "(ArchiveNum LIKE @SearchTerm OR " &
                                      "ArchiveSubject LIKE @SearchTerm OR " &
                                      "ArchiveFolder LIKE @SearchTerm OR " &
                                      "FileName LIKE @SearchTerm) " &
                                      "ORDER BY ArchiveNum"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm & "%")
                    Using adapter As New SqlDataAdapter(command)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            _lastError = "خطأ في البحث في الأرشيف: " & ex.Message
            Throw New Exception(_lastError)
        End Try
        Return dt
    End Function

    ''' <summary>
    ''' البحث في الأرشيفات غير النشطة ( المحذوفة)
    ''' </summary>
    Public Function SearchDeletedArchive(searchTerm As String) As DataTable
        Dim dt As New DataTable()
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT * FROM TbArchive WHERE IsDeleted = 1 AND " &
                                      "(ArchiveNum LIKE @SearchTerm OR " &
                                      "ArchiveSubject LIKE @SearchTerm OR " &
                                      "ArchiveFolder LIKE @SearchTerm OR " &
                                      "FileName LIKE @SearchTerm) " &
                                      "ORDER BY ArchiveNum"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm & "%")
                    Using adapter As New SqlDataAdapter(command)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            _lastError = "خطأ في البحث في الأرشيف: " & ex.Message
            Throw New Exception(_lastError)
        End Try
        Return dt
    End Function

    ''' <summary>
    ''' حذف أرشيف واحد نهائيًا (بدون الاعتماد على الأيام)
    ''' </summary>
    Public Function PermanentlyDeleteSingleArchive(
        ByVal archiveId As Integer,
        Optional ByVal showMessages As Boolean = True) As Boolean

        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()

                ' حذف السجل نهائيًا
                Dim query As String = "DELETE FROM TbArchive WHERE ID = @ID"

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ID", archiveId)
                    Dim rowsAffected As Integer = command.ExecuteNonQuery()

                    If showMessages AndAlso rowsAffected > 0 Then
                        MessageBox.Show(
                            "تم حذف الأرشيف نهائيًا. لا يمكن استرجاعه.",
                            "حذف نهائي",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
                    End If

                    ' تحديث عداد الأرشيف
                    DataHelper.UpdateArchiveCount()

                    Return rowsAffected > 0
                End Using
            End Using
        Catch ex As Exception
            _lastError = $"حدث خطأ أثناء الحذف النهائي للأرشيف بمعرف {archiveId}: {ex.Message}"
            System.Diagnostics.Debug.WriteLine(_lastError)

            If showMessages Then
                MessageBox.Show(
                    $"حدث خطأ أثناء الحذف النهائي: {ex.Message}",
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error)
            End If

            Return False
        End Try
    End Function

#End Region

#Region "الاشعارات"

    ''' <summary>
    ''' جلب عدد السجلات غير المؤرشفة (الصادر بدون أرشيف)
    ''' </summary>
    Public Function GetUnarchivedSaderCount() As Integer
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                ' استعلام لجلب السجلات الصادرة التي لا تحتوي على معرف أرشيف
                Dim query As String = "SELECT COUNT(*) FROM TbSader WHERE ArchiveID IS NULL OR ArchiveID = 0"
                Using command As New SqlCommand(query, connection)
                    Return Convert.ToInt32(command.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            _lastError = "خطأ في جلب عدد السجلات غير المؤرشفة: " & ex.Message
            Throw New Exception(_lastError)
        End Try
    End Function

    ''' <summary>
    ''' جلب السجلات غير المؤرشفة (الصادر بدون أرشيف)
    ''' </summary>
    Public Function GetUnarchivedSaderRecords() As DataTable
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                ' استعلام لجلب السجلات الصادرة التي لا تحتوي على معرف أرشيف
                Dim query As String = "SELECT ID, NumberBook, DateBook, SubjBook, WillMaster, WillSub " &
                                      "FROM TbSader WHERE ArchiveID IS NULL OR ArchiveID = 0 " &
                                      "ORDER BY DateBook DESC"
                Using command As New SqlCommand(query, connection)
                    Using adapter As New SqlDataAdapter(command)
                        Dim dt As New DataTable()
                        adapter.Fill(dt)

                        ' التحقق من أسماء الأعمدة الفعلية
                        ValidateAndFixColumnNames(dt)

                        Return dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            _lastError = "خطأ في جلب السجلات غير المؤرشفة: " & ex.Message
            Throw New Exception(_lastError)
        End Try
    End Function

    ''' <summary>
    ''' التحقق من أسماء الأعمدة وتصحيحها إذا لزم الأمر
    ''' </summary>
    Private Sub ValidateAndFixColumnNames(ByRef dt As DataTable)
        ' القائمة بالترتيب الصحيح كما في قاعدة البيانات
        Dim expectedColumns As String() = {"ID", "NumberBook", "DateBook", "SubjBook", "WillMaster", "WillSub"}

        ' التحقق من وجود الأعمدة بالترتيب الصحيح
        For i As Integer = 0 To expectedColumns.Length - 1
            If i >= dt.Columns.Count Then
                Exit For
            End If

            ' إذا كان الاسم غير مطابق، قم بإصلاحه
            If dt.Columns(i).ColumnName <> expectedColumns(i) Then
                ' التحقق من وجود العمود بالاسم الصحيح في مكان آخر
                Dim columnIndex As Integer = dt.Columns.IndexOf(expectedColumns(i))
                If columnIndex <> -1 Then
                    ' تبديل الأعمدة
                    Dim tempColumnName As String = dt.Columns(i).ColumnName
                    dt.Columns(i).ColumnName = expectedColumns(i)
                    dt.Columns(columnIndex).ColumnName = tempColumnName
                Else
                    ' إذا لم يكن العمود موجودًا، أضفه
                    If Not dt.Columns.Contains(expectedColumns(i)) Then
                        dt.Columns.Add(expectedColumns(i))
                    End If
                End If
            End If
        Next
    End Sub

    '''' <summary>
    '''' جلب السجلات غير المؤرشفة (الصادر بدون أرشيف)
    '''' </summary>
    'Public Function GetUnarchivedSaderRecords() As DataTable
    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            ' استعلام لجلب السجلات الصادرة التي لا تحتوي على معرف أرشيف
    '            Dim query As String = "SELECT ID, NumberBook, DateBook, SubjBook, WillMaster, WillSub " &
    '                                  "FROM TbSader WHERE ArchiveID IS NULL OR ArchiveID = 0 " &
    '                                  "ORDER BY DateBook DESC"
    '            Using command As New SqlCommand(query, connection)
    '                Using adapter As New SqlDataAdapter(command)
    '                    Dim dt As New DataTable()
    '                    adapter.Fill(dt)
    '                    Return dt
    '                End Using
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        _lastError = "خطأ في جلب السجلات غير المؤرشفة: " & ex.Message
    '        Throw New Exception(_lastError)
    '    End Try
    'End Function

#End Region

#Region "الداش بورد | لوحة التحكم"

    ''' <summary>
    ''' دالة للحصول على إحصائيات الأرشيف
    ''' </summary>
    Public Function GetArchiveStatistics() As DataTable
        Try
            Dim query As String = "
        SELECT 
            (SELECT COUNT(*) FROM TbArchive) AS TotalCount,
            (SELECT COUNT(*) FROM TbArchive WHERE ArchiveDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)) AS CurrentMonthCount,
            (SELECT COUNT(*) FROM TbArchive) AS ArchivedCount,
            (SELECT COUNT(*) FROM TbSader WHERE ArchiveID IS NULL OR ArchiveID = 0) AS UnarchivedCount,
            (SELECT COUNT(*) FROM TbSader) AS OutgoingCount,
            (SELECT COUNT(*) FROM TbArchive WHERE IsDeleted = 1) AS DeletedCount"

            Return DatabaseHelper.ExecuteQuery(query)
        Catch ex As Exception
            Throw New Exception("خطأ في الحصول على الإحصائيات: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' دالة للحصول على توزيع الأرشيف حسب المجلدات
    ''' </summary>
    Public Function GetArchiveByFolder() As DataTable
        Try
            Dim query As String = "
        SELECT ArchiveFolder, COUNT(*) AS Count 
        FROM TbArchive 
        WHERE ArchiveFolder IS NOT NULL AND IsDeleted = 0
        GROUP BY ArchiveFolder 
        ORDER BY Count DESC"

            Return DatabaseHelper.ExecuteQuery(query)
        Catch ex As Exception
            Throw New Exception("خطأ في الحصول على توزيع المجلدات: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' دالة للحصول على الاتجاه الشهري للأرشيف
    ''' </summary>
    Public Function GetMonthlyArchiveTrend() As DataTable
        Try
            Dim query As String = "
        SELECT 
            FORMAT(ArchiveDate, 'MMM') AS MonthName,
            COUNT(*) AS Count
        FROM TbArchive
        WHERE ArchiveDate >= DATEADD(MONTH, -6, GETDATE())
        GROUP BY FORMAT(ArchiveDate, 'MMM'), MONTH(ArchiveDate)
        ORDER BY MONTH(ArchiveDate)"

            Return DatabaseHelper.ExecuteQuery(query)
        Catch ex As Exception
            Throw New Exception("خطأ في الحصول على الاتجاه الشهري: " & ex.Message)
        End Try
    End Function

#End Region

#Region "تسجيل حركة المستخدمين"

    ''' <summary>
    ''' تسجيل نشاط المستخدم في جدول UserActivityLog
    ''' </summary>
    Private Sub LogActivity(userName As String, actionType As String, tableName As String, recordID As Integer, details As String)
        Try
            ' استخدام نفس اتصال قاعدة البيانات أو اتصال منفصل حسب الحاجة
            Using connection As SqlConnection = DatabaseHelper.Con()
                connection.Open()
                Using command As New SqlCommand("INSERT INTO UserActivityLog (UserName, ActionType, TableName, RecordID, ActionDetails, IPAddress, ActionDate) " &
                                              "VALUES (@UserName, @ActionType, @TableName, @RecordID, @ActionDetails, @IPAddress, GETDATE())", connection)

                    command.Parameters.AddWithValue("@UserName", userName)
                    command.Parameters.AddWithValue("@ActionType", actionType)
                    command.Parameters.AddWithValue("@TableName", tableName)
                    command.Parameters.AddWithValue("@RecordID", If(recordID > 0, recordID, DBNull.Value))
                    command.Parameters.AddWithValue("@ActionDetails", If(String.IsNullOrEmpty(details), DBNull.Value, details))
                    command.Parameters.AddWithValue("@IPAddress", GetClientIP())

                    command.ExecuteNonQuery()
                End Using
            End Using
        Catch logEx As Exception
            ' من الأفضل عدم رمي استثناء هنا حتى لا يعطل العملية الرئيسية
            System.Diagnostics.Debug.WriteLine($"فشل في تسجيل النشاط: {logEx.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' دالة للحصول على عنوان IP للعميل
    ''' </summary>
    Private Function GetClientIP() As String
        Try
            Dim hostEntry As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
            For Each address As IPAddress In hostEntry.AddressList
                If address.AddressFamily = AddressFamily.InterNetwork AndAlso Not IPAddress.IsLoopback(address) Then
                    Return address.ToString()
                End If
            Next
            Return "127.0.0.1"
        Catch ex As Exception
            Return "Unknown"
        End Try
    End Function

#End Region
End Class
















''Imports System.IO
''Imports System.Data
''Imports System.Data.SqlClient
''Imports System.Drawing
''Imports System.Windows.Forms

''Public Class ArchiveClass
''#Region "المتغيرات والتعريفات الأساسية"
''    ' تعريف المسار الأساسي للأرشيف
''    ' إضافة هذه المتغيرات في بداية ArchiveClass.vb
''    Private Const DELETED_FOLDER_NAME As String = "المحذوفات"
''    Private Const MAX_RETRY_ATTEMPTS As Integer = 3
''    Private Const RETRY_DELAY_MS As Integer = 500

''    ' متغيرات لتخزين حالة العملية
''    Private _lastError As String = String.Empty

''    ' هيكل لتخزين معلومات الأرشيف
''    Public Structure ArchiveInfo
''        Public ID As Integer
''        Public ArchiveNum As String
''        Public ArchiveDate As Date
''        Public ArchiveSubject As String
''        Public ArchiveFolder As String
''        Public FileName As String
''        Public FileType As String
''        Public FilePath As String
''        Public OriginalFilePath As String
''        Public ArchiveWillMaster As String
''        Public ArchiveWillSub As String
''        Public UserName As String
''        Public ArchiveNotes As String
''    End Structure
''#End Region

''#Region "الدوال الأساسية لإدارة الأرشيف"
''    ''' <summary>
''    ''' إنشاء مجلدات التصنيف الرئيسية
''    ''' </summary>
''    Public Function CreateCategoryFolders() As Boolean
''        Try
''            ' إنشاء المجلد الرئيسي إذا لم يكن موجودًا
''            If Not Directory.Exists(My.Settings.ARCHIVE_Default_PATH) Then
''                Directory.CreateDirectory(My.Settings.ARCHIVE_Default_PATH)
''            End If

''            ' إنشاء مجلدات التصنيفات
''            Dim incomingPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, "الوارد")
''            Dim outgoingPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, "الصادر")
''            Dim memosPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, "المذكرات")

''            If Not Directory.Exists(incomingPath) Then
''                Directory.CreateDirectory(incomingPath)
''            End If

''            If Not Directory.Exists(outgoingPath) Then
''                Directory.CreateDirectory(outgoingPath)
''            End If

''            If Not Directory.Exists(memosPath) Then
''                Directory.CreateDirectory(memosPath)
''            End If

''            Return True
''        Catch ex As Exception
''            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء مجلدات التصنيف: {ex.Message}")
''            Return False
''        End Try
''    End Function

''    ''' <summary>
''    ''' إنشاء مجلد فرعي بناءً على التصنيف والمجلد الفرعي
''    ''' </summary>
''    ''' <param name="category">التصنيف (مثل: صادر، وارد)</param>
''    ''' <param name="subFolder">اسم المجلد الفرعي (مثل: 2024-12)</param>
''    ''' <returns>المسار الكامل للمجلد الفرعي</returns>
''    Public Function CreateSubFolder(category As String, subFolder As String) As String
''        Try
''            ' إنشاء مسار المجلد الرئيسي
''            Dim categoryPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, category)

''            ' إنشاء مسار المجلد الفرعي
''            Dim subFolderPath As String = Path.Combine(categoryPath, subFolder)

''            ' إنشاء المجلدات إذا لم تكن موجودة
''            If Not Directory.Exists(categoryPath) Then
''                Directory.CreateDirectory(categoryPath)
''            End If

''            If Not Directory.Exists(subFolderPath) Then
''                Directory.CreateDirectory(subFolderPath)
''            End If

''            Return subFolderPath
''        Catch ex As Exception
''            Throw New Exception("خطأ في إنشاء المجلد الفرعي: " & ex.Message)
''        End Try
''    End Function

''    ''' <summary>
''    ''' إضافة أرشيف جديد
''    ''' </summary>
''    Public Function AddArchive(
''    archiveNum As String,
''    archiveSubject As String,
''    archiveFolder As String,
''    archiveDate As Date,
''    archiveWillMaster As String,
''    archiveWillSub As String,
''    archiveNotes As String,
''    filename As String,
''    fileType As String,
''    filePath As String,
''    originalFilePath As String,
''    userName As String) As Integer

''        Dim transaction As SqlTransaction = Nothing
''        Try
''            ' تحليل مسار الأرشيف وإنشاء المجلدات المناسبة
''            Dim finalArchivePath As String = GetFinalArchivePath(archiveFolder)
''            If String.IsNullOrEmpty(finalArchivePath) Then
''                Throw New Exception("فشل في تحديد مسار الأرشيف")
''            End If

''            ' التحقق من أن اسم الملف يحتوي على الامتداد الصحيح
''            Dim fileExtension As String = Path.GetExtension(filePath) ' الحصول على الامتداد من المسار الكامل
''            If Not String.IsNullOrEmpty(fileExtension) AndAlso Not filename.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) Then
''                filename = Path.GetFileNameWithoutExtension(filename) & fileExtension
''            End If

''            ' تحديد المسار النهائي للملف (المسار الحالي بعد النسخ)
''            Dim destinationPath As String = Path.Combine(finalArchivePath, filename)
''            Dim actualOriginalFilePath As String = If(String.IsNullOrEmpty(originalFilePath), filePath, originalFilePath)
''            Using connection As SqlConnection = GetConnection()
''                connection.Open()
''                transaction = connection.BeginTransaction()

''                Try
''                    ' التحقق من وجود الملف المصدر
''                    If File.Exists(actualOriginalFilePath) Then
''                        ' نسخ الملف إلى المجلد المحدد
''                        File.Copy(actualOriginalFilePath, destinationPath, True)
''                    Else
''                        Throw New Exception($"الملف المصدر غير موجود: {actualOriginalFilePath}")
''                    End If

''                    ' إدراج السجل في قاعدة البيانات
''                    Dim query As String = "INSERT INTO TbArchive (ArchiveNum, ArchiveDate, ArchiveSubject,
''                                        ArchiveNotes, ArchiveFolder, FileName, FileType, FilePath, OriginalFilePath, ArchiveWillMaster,
''                                        ArchiveWillSub, UserName) VALUES (@ArchiveNum, @ArchiveDate, @ArchiveSubject, @ArchiveNotes,
''                                        @ArchiveFolder, @FileName, @FileType, @FilePath, @OriginalFilePath,
''                                        @ArchiveWillMaster, @ArchiveWillSub, @UserName); SELECT SCOPE_IDENTITY();"

''                    Using command As New SqlCommand(query, connection, transaction)
''                        command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
''                        command.Parameters.AddWithValue("@ArchiveDate", archiveDate)
''                        command.Parameters.AddWithValue("@ArchiveSubject", archiveSubject)
''                        command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
''                        command.Parameters.AddWithValue("@ArchiveNotes", archiveNotes)
''                        command.Parameters.AddWithValue("@FileName", filename)
''                        command.Parameters.AddWithValue("@FileType", fileType)
''                        command.Parameters.AddWithValue("@FilePath", destinationPath) ' المسار الحالي بعد النسخ
''                        command.Parameters.AddWithValue("@OriginalFilePath", actualOriginalFilePath) ' المسار الأصلي للملف
''                        command.Parameters.AddWithValue("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster))
''                        command.Parameters.AddWithValue("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub))
''                        command.Parameters.AddWithValue("@UserName", userName)

''                        Dim result As Object = command.ExecuteScalar()
''                        Dim newId As Integer

''                        ' التحقق من أن القيمة يمكن تحويلها إلى رقم
''                        If result IsNot Nothing AndAlso Integer.TryParse(result.ToString(), newId) Then
''                            ' تأكيد المعاملة
''                            transaction.Commit()
''                            Return newId
''                        Else
''                            ' التراجع عن المعاملة في حالة فشل التحويل
''                            transaction.Rollback()
''                            Return -1
''                        End If
''                    End Using
''                Catch ex As Exception
''                    ' التراجع عن المعاملة في حالة حدوث خطأ
''                    transaction.Rollback()
''                    Throw
''                End Try
''            End Using

''        Catch ex As Exception
''            _lastError = "خطأ في إضافة الأرشيف: " & ex.Message
''            Throw New Exception(_lastError)
''        End Try
''    End Function
''    ''' <summary>
''    ''' الحصول على المسار النهائي للمجلد بناءً على تصنيف الأرشيف
''    ''' </summary>
''    Public Function GetFinalArchivePath(archiveFolder As String) As String
''        Try
''            ' التحقق من وجود التصنيف
''            If String.IsNullOrWhiteSpace(archiveFolder) Then
''                Return String.Empty
''            End If

''            ' تحديد المسار الأساسي
''            Dim archiveBasePath As String = My.Settings.ARCHIVE_Default_PATH

''            ' التحقق من وجود مسار أساسي
''            If String.IsNullOrWhiteSpace(archiveBasePath) Then
''                Return String.Empty
''            End If

''            ' إنشاء المسار النهائي
''            Dim finalPath As String = archiveBasePath

''            ' إضافة التصنيف إلى المسار
''            finalPath = Path.Combine(finalPath, archiveFolder)

''            ' التأكد من إنشاء جميع المجلدات الفرعية
''            If Not Directory.Exists(finalPath) Then
''                Directory.CreateDirectory(finalPath)
''            End If

''            ' إذا كان المسار يحتوي على شرطة مائلة، تأكد من إنشاء جميع المجلدات الفرعية
''            If archiveFolder.Contains("\") Then
''                Dim parts() As String = archiveFolder.Split("\"c)
''                Dim currentPath As String = archiveBasePath

''                For Each part As String In parts
''                    currentPath = Path.Combine(currentPath, part)
''                    If Not Directory.Exists(currentPath) Then
''                        Directory.CreateDirectory(currentPath)
''                    End If
''                Next
''            End If

''            Return finalPath
''        Catch ex As Exception
''            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على مسار الأرشيف: {ex.Message}")
''            Return String.Empty
''        End Try
''    End Function

''    'Public Function AddArchive(
''    '                          archiveNum As String,
''    '                          archiveSubject As String,
''    '                          archiveFolder As String,
''    '                          archiveDate As Date,
''    '                          archiveWillMaster As String,
''    '                          archiveWillSub As String,
''    '                          archiveNotes As String,
''    '                          fileName As String,
''    '                          fileType As String,
''    '                          filePath As String,
''    '                          originalFilePath As String,
''    '                          userName As String) As Integer
''    '    Dim transaction As SqlTransaction = Nothing
''    '    Try
''    '        ' تحليل مسار الأرشيف وإنشاء المجلدات المناسبة
''    '        Dim finalArchivePath As String = ""

''    '        If archiveFolder.Contains("\") Then
''    '            ' إذا كان المسار يحتوي على تصنيف (مثل: صادر\2024-12)
''    '            Dim parts() As String = archiveFolder.Split("\"c)
''    '            If parts.Length >= 2 Then
''    '                Dim category As String = parts(0)
''    '                Dim subFolder As String = parts(1)
''    '                finalArchivePath = CreateSubFolder(category, subFolder)
''    '            Else
''    '                ' مجرد مجلد فرعي
''    '                finalArchivePath = Path.Combine(My.Settings.ARCHIVE_Default_PATH, archiveFolder)
''    '                If Not Directory.Exists(finalArchivePath) Then
''    '                    Directory.CreateDirectory(finalArchivePath)
''    '                End If
''    '            End If
''    '        Else
''    '            ' مجرد اسم مجلد بسيط
''    '            finalArchivePath = Path.Combine(My.Settings.ARCHIVE_Default_PATH, archiveFolder)
''    '            If Not Directory.Exists(finalArchivePath) Then
''    '                Directory.CreateDirectory(finalArchivePath)
''    '            End If
''    '        End If

''    '        ' التحقق من أن اسم الملف يحتوي على الامتداد الصحيح
''    '        Dim fileExtension As String = Path.GetExtension(filePath) ' الحصول على الامتداد من المسار الكامل
''    '        If Not String.IsNullOrEmpty(fileExtension) AndAlso Not fileName.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) Then
''    '            fileName = Path.GetFileNameWithoutExtension(fileName) & fileExtension
''    '        End If

''    '        ' تحديد المسار النهائي للملف (المسار الحالي بعد النسخ)
''    '        Dim destinationPath As String = Path.Combine(finalArchivePath, fileName)
''    '        Dim actualOriginalFilePath As String = If(String.IsNullOrEmpty(originalFilePath), filePath, originalFilePath)
''    '        Using connection As SqlConnection = GetConnection()
''    '            connection.Open()
''    '            transaction = connection.BeginTransaction()

''    '            Try
''    '                ' التحقق من وجود الملف المصدر
''    '                If File.Exists(filePath) Then
''    '                    ' نسخ الملف إلى المجلد المحدد
''    '                    File.Copy(filePath, destinationPath, True)
''    '                End If

''    '                ' إدراج السجل في قاعدة البيانات
''    '                Dim query As String = "INSERT INTO TbArchive (ArchiveNum, ArchiveDate, ArchiveSubject,
''    '                                        ArchiveNotes, ArchiveFolder, FileName, FileType, FilePath, OriginalFilePath, ArchiveWillMaster,
''    '                                        ArchiveWillSub, UserName) VALUES (@ArchiveNum, @ArchiveDate, @ArchiveSubject, @ArchiveNotes,
''    '                                        @ArchiveFolder, @FileName, @FileType, @FilePath, @OriginalFilePath,
''    '                                        @ArchiveWillMaster, @ArchiveWillSub, @UserName); SELECT SCOPE_IDENTITY();"

''    '                Using command As New SqlCommand(query, connection, transaction)
''    '                    command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
''    '                    command.Parameters.AddWithValue("@ArchiveDate", archiveDate)
''    '                    command.Parameters.AddWithValue("@ArchiveSubject", archiveSubject)
''    '                    command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
''    '                    command.Parameters.AddWithValue("@ArchiveNotes", archiveNotes)
''    '                    command.Parameters.AddWithValue("@FileName", fileName)
''    '                    command.Parameters.AddWithValue("@FileType", fileType)
''    '                    command.Parameters.AddWithValue("@FilePath", destinationPath) ' المسار الحالي بعد النسخ
''    '                    command.Parameters.AddWithValue("@OriginalFilePath", actualOriginalFilePath) ' المسار الأصلي للملف
''    '                    command.Parameters.AddWithValue("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster))
''    '                    command.Parameters.AddWithValue("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub))
''    '                    command.Parameters.AddWithValue("@UserName", userName)

''    '                    Dim result As Object = command.ExecuteScalar()
''    '                    Dim newId As Integer

''    '                    If result IsNot Nothing AndAlso Integer.TryParse(result.ToString(), newId) Then
''    '                        ' تأكيد المعاملة
''    '                        transaction.Commit()
''    '                        Return newId
''    '                    Else
''    '                        transaction.Rollback()
''    '                        Return -1
''    '                    End If

''    '                End Using
''    '            Catch ex As Exception
''    '                ' التراجع عن المعاملة في حالة حدوث خطأ
''    '                transaction.Rollback()
''    '                Throw
''    '            End Try
''    '        End Using

''    '    Catch ex As Exception
''    '        _lastError = "خطأ في إضافة الأرشيف: " & ex.Message
''    '        Throw New Exception(_lastError)
''    '    End Try
''    'End Function

''    ''' <summary>
''    ''' تحديث سجل الأرشيف
''    ''' </summary>
''    Public Function UpdateArchive(id As Integer, archiveNum As String, archiveSubject As String, archiveFolder As String, archiveDate As Date,
''                                 archiveWillMaster As String, archiveWillSub As String, archiveNotes As String,
''                                 fileName As String, fileType As String, filePath As String, originalFilePath As String,
''                                 userName As String) As Boolean
''        Dim transaction As SqlTransaction = Nothing
''        Try
''            ' تحليل مسار الأرشيف وإنشاء المجلدات المناسبة
''            Dim finalArchivePath As String = ""

''            If archiveFolder.Contains("\") Then
''                ' إذا كان المسار يحتوي على تصنيف (مثل: صادر\2024-12)
''                Dim parts() As String = archiveFolder.Split("\"c)
''                If parts.Length >= 2 Then
''                    Dim category As String = parts(0)
''                    Dim subFolder As String = parts(1)
''                    finalArchivePath = CreateSubFolder(category, subFolder)
''                Else
''                    ' مجرد مجلد فرعي
''                    finalArchivePath = Path.Combine(My.Settings.ARCHIVE_Default_PATH, archiveFolder)
''                    If Not Directory.Exists(finalArchivePath) Then
''                        Directory.CreateDirectory(finalArchivePath)
''                    End If
''                End If
''            Else
''                ' مجرد اسم مجلد بسيط
''                finalArchivePath = Path.Combine(My.Settings.ARCHIVE_Default_PATH, archiveFolder)
''                If Not Directory.Exists(finalArchivePath) Then
''                    Directory.CreateDirectory(finalArchivePath)
''                End If
''            End If

''            ' التحقق من أن اسم الملف يحتوي على الامتداد الصحيح
''            Dim fileExtension As String = Path.GetExtension(filePath) ' الحصول على الامتداد من المسار الكامل
''            If Not String.IsNullOrEmpty(fileExtension) AndAlso Not fileName.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) Then
''                fileName = Path.GetFileNameWithoutExtension(fileName) & fileExtension
''            End If

''            ' تحديد المسار النهائي للملف (المسار الحالي بعد النسخ)
''            Dim destinationPath As String = Path.Combine(finalArchivePath, fileName)
''            Dim actualOriginalFilePath As String = If(String.IsNullOrEmpty(originalFilePath), filePath, originalFilePath)
''            Using connection As SqlConnection = GetConnection()
''                connection.Open()
''                transaction = connection.BeginTransaction()

''                Try
''                    ' الحصول على معلومات الأرشيف الحالية
''                    Dim currentArchive As DataTable = GetArchiveById(id)
''                    Dim currentFilePath As String = ""
''                    Dim currentOriginalFilePath As String = ""

''                    If currentArchive.Rows.Count > 0 Then
''                        currentFilePath = If(currentArchive.Rows(0)("FilePath") IsNot DBNull.Value, currentArchive.Rows(0)("FilePath").ToString(), "")
''                        currentOriginalFilePath = If(currentArchive.Rows(0)("OriginalFilePath") IsNot DBNull.Value, currentArchive.Rows(0)("OriginalFilePath").ToString(), "")
''                    End If

''                    ' التحقق من وجود الملف المصدر
''                    If File.Exists(filePath) Then
''                        ' حذف الملف القديم من المسار الحالي إذا كان مختلفًا
''                        If Not String.IsNullOrEmpty(currentFilePath) AndAlso currentFilePath <> destinationPath AndAlso File.Exists(currentFilePath) Then
''                            Try
''                                File.Delete(currentFilePath)
''                            Catch ex As Exception
''                                ' في حالة فشل الحذف، نقله إلى مجلد المحذوفات
''                                MoveToDeletedFolder(currentFilePath)
''                            End Try
''                        End If

''                        ' نسخ الملف الجديد إلى المجلد المحدد
''                        File.Copy(filePath, destinationPath, True)
''                    End If

''                    ' تحديث السجل في قاعدة البيانات
''                    Dim query As String = "UPDATE TbArchive SET ArchiveNum = @ArchiveNum, ArchiveDate = @ArchiveDate, ArchiveSubject = @ArchiveSubject, ArchiveNotes = @ArchiveNotes, ArchiveFolder = @ArchiveFolder, FileName = @FileName, FileType = @FileType, FilePath = @FilePath, OriginalFilePath = @OriginalFilePath, ArchiveWillMaster = @ArchiveWillMaster, ArchiveWillSub = @ArchiveWillSub, UserName = @UserName WHERE ID = @ID"

''                    Using command As New SqlCommand(query, connection, transaction)
''                        command.Parameters.AddWithValue("@ID", id)
''                        command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
''                        command.Parameters.AddWithValue("@ArchiveDate", archiveDate)
''                        command.Parameters.AddWithValue("@ArchiveSubject", archiveSubject)
''                        command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
''                        command.Parameters.AddWithValue("@ArchiveNotes", archiveNotes)
''                        command.Parameters.AddWithValue("@FileName", fileName)
''                        command.Parameters.AddWithValue("@FileType", fileType)
''                        command.Parameters.AddWithValue("@FilePath", destinationPath) ' المسار الحالي بعد النسخ
''                        command.Parameters.AddWithValue("@OriginalFilePath", actualOriginalFilePath) ' المسار الأصلي للملف
''                        command.Parameters.AddWithValue("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster))
''                        command.Parameters.AddWithValue("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub))
''                        command.Parameters.AddWithValue("@UserName", userName)

''                        Dim rowsAffected As Integer = command.ExecuteNonQuery()

''                        ' تأكيد المعاملة
''                        transaction.Commit()
''                        Return rowsAffected > 0
''                    End Using
''                Catch ex As Exception
''                    ' التراجع عن المعاملة في حالة حدوث خطأ
''                    transaction.Rollback()
''                    Throw
''                End Try
''            End Using

''        Catch ex As Exception
''            _lastError = "خطأ في تعديل الأرشيف: " & ex.Message
''            Throw New Exception(_lastError)
''        End Try
''    End Function

''    '''' <summary>
''    '''' حذف سجل الأرشيف مع نقل الملف إلى مجلد المحذوفات كأولوية
''    '''' </summary>
''    '''' <param name="archiveId">معرف الأرشيف</param>
''    '''' <param name="showMessages">إظهار رسائل للمستخدم</param>
''    '''' <returns>صحيح إذا تم الحذف بنجاح</returns>

''    'Public Function DeleteArchiveById(archiveId As Integer, Optional showMessages As Boolean = True) As Boolean
''    '    Dim transaction As SqlTransaction = Nothing
''    '    Try
''    '        Using connection As SqlConnection = GetConnection()
''    '            connection.Open()
''    '            transaction = connection.BeginTransaction()

''    '            Try
''    '                ' 1. الحصول على معلومات الأرشيف
''    '                Dim dt As DataTable = GetArchiveById(archiveId)
''    '                Dim currentFilePath As String = ""
''    '                Dim archiveNum As String = ""

''    '                If dt.Rows.Count > 0 Then
''    '                    currentFilePath = If(dt.Rows(0)("FilePath") IsNot DBNull.Value, dt.Rows(0)("FilePath").ToString(), "")
''    '                    archiveNum = If(dt.Rows(0)("ArchiveNum") IsNot DBNull.Value, dt.Rows(0)("ArchiveNum").ToString(), $"المعرف {archiveId}")
''    '                End If

''    '                ' 2. حذف السجل من قاعدة البيانات أولاً
''    '                Dim deleteQuery As String = "DELETE FROM TbArchive WHERE ID = @ID"
''    '                Using command As New SqlCommand(deleteQuery, connection, transaction)
''    '                    command.Parameters.AddWithValue("@ID", archiveId)
''    '                    Dim result As Boolean = command.ExecuteNonQuery() > 0

''    '                    ' 3. تأكيد المعاملة
''    '                    transaction.Commit()

''    '                    ' 4. نقل الملف إلى مجلد المحذوفات بعد حذف السجل
''    '                    If result AndAlso Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath) Then
''    '                        Try
''    '                            ' نقل الملف إلى مجلد المحذوفات
''    '                            MoveToDeletedFolder(currentFilePath)

''    '                            If showMessages Then
''    '                                MessageBox.Show("تم حذف الأرشيف من قاعدة البيانات ونقل الملف إلى مجلد المحذوفات", "نجح جزئي", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''    '                            End If
''    '                        Catch ex As Exception
''    '                            If showMessages Then
''    '                                MessageBox.Show("تم حذف الأرشيف من قاعدة البيانات ولكن فشل في نقل الملف", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''    '                            End If
''    '                        End Try
''    '                    ElseIf result Then
''    '                        If showMessages Then
''    '                            MessageBox.Show("تم حذف الأرشيف بنجاح (لا يوجد مسار ملف مرتبط)", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
''    '                        End If
''    '                    End If

''    '                    ' 5. تحديث عداد الأرشيف
''    '                    DataHelper.UpdateArchiveCount()

''    '                    Return result
''    '                End Using
''    '            Catch ex As Exception
''    '                ' التراجع عن المعاملة في حالة حدوث خطأ
''    '                If transaction IsNot Nothing Then
''    '                    transaction.Rollback()
''    '                End If
''    '                Throw
''    '            End Try
''    '        End Using
''    '    Catch ex As Exception
''    '        _lastError = $"حدث خطأ أثناء حذف الأرشيف بمعرف {archiveId}: {ex.Message}"
''    '        If showMessages Then
''    '            MessageBox.Show(_lastError, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
''    '        End If
''    '        Return False
''    '    End Try
''    'End Function



''    'Public Function DeleteArchiveById(archiveId As Integer, Optional showMessages As Boolean = True) As Boolean
''    '    Dim transaction As SqlTransaction = Nothing
''    '    Try
''    '        Using connection As SqlConnection = GetConnection()
''    '            connection.Open()
''    '            transaction = connection.BeginTransaction()

''    '            Try
''    '                ' 1. الحصول على معلومات الأرشيف
''    '                Dim dt As DataTable = GetArchiveById(archiveId)
''    '                Dim currentFilePath As String = ""
''    '                Dim archiveNum As String = ""

''    '                If dt.Rows.Count > 0 Then
''    '                    currentFilePath = If(dt.Rows(0)("FilePath") IsNot DBNull.Value, dt.Rows(0)("FilePath").ToString(), "")
''    '                    archiveNum = If(dt.Rows(0)("ArchiveNum") IsNot DBNull.Value, dt.Rows(0)("ArchiveNum").ToString(), $"المعرف {archiveId}")
''    '                End If

''    '                ' 2. أولوية نقل الملف إلى مجلد المحذوفات
''    '                Dim fileOperationSuccess As Boolean = False
''    '                Dim fileOperationMessage As String = ""

''    '                If Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath) Then
''    '                    ' محاولة نقل الملف إلى مجلد المحذوفات أولاً (الأولوية)
''    '                    Try
''    '                        MoveToDeletedFolder(currentFilePath)
''    '                        fileOperationSuccess = True
''    '                        fileOperationMessage = "تم نقل الملف إلى مجلد المحذوفات"

''    '                        If showMessages Then
''    '                            MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات ونقل الملف إلى مجلد المحذوفات",
''    '                                      "نجاح جزئي", MessageBoxButtons.OK, MessageBoxIcon.Information)
''    '                        End If
''    '                    Catch moveEx As Exception
''    '                        ' إذا فشل النقل، حاول حذف الملف
''    '                        Try
''    '                            File.Delete(currentFilePath)
''    '                            fileOperationSuccess = True
''    '                            fileOperationMessage = "تم حذف الملف بنجاح"

''    '                            If showMessages Then
''    '                                MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} والملف المرتبط به بنجاح",
''    '                                          "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
''    '                            End If
''    '                        Catch deleteEx As Exception
''    '                            ' إذا فشل كلا الأمرين
''    '                            fileOperationMessage = $"فشل في نقل أو حذف الملف: {deleteEx.Message}"

''    '                            If showMessages Then
''    '                                MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات فقط.{vbNewLine}" &
''    '                                          $"{fileOperationMessage}{vbNewLine}" &
''    '                                          $"يرجى معالجة الملف يدويًا من المسار: {currentFilePath}",
''    '                                          "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''    '                            End If
''    '                        End Try
''    '                    End Try
''    '                Else
''    '                    fileOperationSuccess = True
''    '                    fileOperationMessage = "لا يوجد ملف مرتبط"

''    '                    If showMessages Then
''    '                        MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات (لا يوجد مسار ملف مرتبط)",
''    '                                  "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
''    '                    End If
''    '                End If

''    '                ' 3. حذف السجل من قاعدة البيانات
''    '                Dim deleteQuery As String = "DELETE FROM TbArchive WHERE ID = @ID"
''    '                Using command As New SqlCommand(deleteQuery, connection, transaction)
''    '                    command.Parameters.AddWithValue("@ID", archiveId)
''    '                    Dim rowsAffected As Integer = command.ExecuteNonQuery()

''    '                    ' 4. تأكيد المعاملة
''    '                    transaction.Commit()

''    '                    ' 5. تحديث عداد الأرشيف
''    '                    DataHelper.UpdateArchiveCount()

''    '                    Return rowsAffected > 0
''    '                End Using
''    '            Catch ex As Exception
''    '                ' التراجع عن المعاملة في حالة حدوث خطأ
''    '                If transaction IsNot Nothing Then
''    '                    transaction.Rollback()
''    '                End If
''    '                Throw
''    '            End Try
''    '        End Using
''    '    Catch ex As Exception
''    '        _lastError = $"حدث خطأ أثناء حذف الأرشيف بمعرف {archiveId}: {ex.Message}"
''    '        If showMessages Then
''    '            MessageBox.Show(_lastError, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
''    '        End If
''    '        Return False
''    '    End Try
''    'End Function


''    'Public Function DeleteArchiveById(archiveId As Integer, Optional showMessages As Boolean = True) As Boolean
''    '    Dim transaction As SqlTransaction = Nothing
''    '    Try
''    '        Using connection As SqlConnection = GetConnection()
''    '            connection.Open()
''    '            transaction = connection.BeginTransaction()

''    '            Try
''    '                ' 1. الحصول على معلومات الأرشيف أولاً
''    '                Dim dt As DataTable = GetArchiveById(archiveId)
''    '                Dim currentFilePath As String = ""
''    '                Dim currentOriginalFilePath As String = ""
''    '                Dim archiveNum As String = ""

''    '                If dt.Rows.Count > 0 Then
''    '                    currentFilePath = If(dt.Rows(0)("FilePath") IsNot DBNull.Value, dt.Rows(0)("FilePath").ToString(), "")
''    '                    currentOriginalFilePath = If(dt.Rows(0)("OriginalFilePath") IsNot DBNull.Value, dt.Rows(0)("OriginalFilePath").ToString(), "")
''    '                    archiveNum = If(dt.Rows(0)("ArchiveNum") IsNot DBNull.Value, dt.Rows(0)("ArchiveNum").ToString(), $"المعرف {archiveId}")
''    '                End If

''    '                ' 2. محاولة حذف الملف أولاً (قبل حذف السجل من قاعدة البيانات)
''    '                Dim fileDeleted As Boolean = False
''    '                Dim fileMoved As Boolean = False

''    '                If Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath) Then
''    '                    Try
''    '                        ' محاولة حذف الملف
''    '                        File.Delete(currentFilePath)
''    '                        fileDeleted = True

''    '                        If showMessages Then
''    '                            MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} والملف المرتبط به بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
''    '                        End If
''    '                    Catch ex As Exception
''    '                        Try
''    '                            ' في حالة فشل الحذف، نقل الملف إلى مجلد المحذوفات
''    '                            MoveToDeletedFolder(currentFilePath)
''    '                            fileMoved = True

''    '                            If showMessages Then
''    '                                MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات ونقل الملف إلى مجلد المحذوفات", "نجح جزئي", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''    '                            End If
''    '                        Catch moveEx As Exception
''    '                            ' إذا فشل النقل أيضًا
''    '                            If showMessages Then
''    '                                MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات ولكن فشل في حذف الملف: {currentFilePath}{vbNewLine}السبب: {ex.Message}{vbNewLine}يرجى إغلاق أي برامج تستخدم هذا الملف وحذفه يدوياً.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''    '                            End If
''    '                        End Try
''    '                    End Try
''    '                End If

''    '                ' 3. الآن حذف السجل من قاعدة البيانات
''    '                Dim deleteQuery As String = "DELETE FROM TbArchive WHERE ID = @ID"
''    '                Using command As New SqlCommand(deleteQuery, connection, transaction)
''    '                    command.Parameters.AddWithValue("@ID", archiveId)
''    '                    Dim rowsAffected As Integer = command.ExecuteNonQuery()

''    '                    ' 4. تأكيد المعاملة
''    '                    transaction.Commit()

''    '                    ' 5. تحديث عداد الأرشيف
''    '                    DataHelper.UpdateArchiveCount()

''    '                    ' 6. إرجاع النتيجة بناءً على نجاح حذف السجل
''    '                    Return rowsAffected > 0
''    '                End Using
''    '            Catch ex As Exception
''    '                ' التراجع عن المعاملة في حالة حدوث خطأ
''    '                If transaction IsNot Nothing Then
''    '                    transaction.Rollback()
''    '                End If
''    '                Throw
''    '            End Try
''    '        End Using
''    '    Catch ex As Exception
''    '        _lastError = $"حدث خطأ أثناء حذف الأرشيف بمعرف {archiveId}: {ex.Message}"
''    '        If showMessages Then
''    '            MessageBox.Show(_lastError, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
''    '        End If
''    '        Return False
''    '    End Try
''    'End Function



''    'Public Function DeleteArchiveById(archiveId As Integer, Optional showMessages As Boolean = True) As Boolean
''    '    Dim transaction As SqlTransaction = Nothing
''    '    Try
''    '        Using connection As SqlConnection = GetConnection()
''    '            connection.Open()
''    '            transaction = connection.BeginTransaction()

''    '            Try
''    '                ' الحصول على معلومات الأرشيف
''    '                Dim dt As DataTable = GetArchiveById(archiveId)
''    '                Dim currentFilePath As String = ""
''    '                Dim currentOriginalFilePath As String = ""

''    '                If dt.Rows.Count > 0 Then
''    '                    currentFilePath = If(dt.Rows(0)("FilePath") IsNot DBNull.Value, dt.Rows(0)("FilePath").ToString(), "")
''    '                    currentOriginalFilePath = If(dt.Rows(0)("OriginalFilePath") IsNot DBNull.Value, dt.Rows(0)("OriginalFilePath").ToString(), "")
''    '                End If

''    '                ' حذف السجل من قاعدة البيانات
''    '                Dim deleteQuery As String = "DELETE FROM TbArchive WHERE ID = @ID"
''    '                Using command As New SqlCommand(deleteQuery, connection, transaction)
''    '                    command.Parameters.AddWithValue("@ID", archiveId)
''    '                    Dim result As Boolean = command.ExecuteNonQuery() > 0

''    '                    ' حذف الملف من المسار الحالي فقط (وليس من المسار الأصلي)
''    '                    If result AndAlso Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath) Then
''    '                        Try
''    '                            File.Delete(currentFilePath)
''    '                            If showMessages Then
''    '                                MessageBox.Show("تم حذف الأرشيف والملف المرتبط بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
''    '                            End If
''    '                        Catch
''    '                            ' في حالة فشل حذف الملف، نقله إلى مجلد المحذوفات
''    '                            MoveToDeletedFolder(currentFilePath)
''    '                            If showMessages Then
''    '                                MessageBox.Show("تم حذف الأرشيف من قاعدة البيانات ونقل الملف إلى مجلد المحذوفات", "نجح جزئي", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''    '                            End If
''    '                        End Try
''    '                    ElseIf result Then
''    '                        If showMessages Then
''    '                            MessageBox.Show("تم حذف الأرشيف بنجاح (لا يوجد مسار ملف مرتبط)", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
''    '                        End If
''    '                    End If

''    '                    ' تأكيد المعاملة
''    '                    transaction.Commit()
''    '                    Return result
''    '                End Using
''    '            Catch ex As Exception
''    '                ' التراجع عن المعاملة في حالة حدوث خطأ
''    '                If transaction IsNot Nothing Then
''    '                    transaction.Rollback()
''    '                End If
''    '                Throw
''    '            End Try
''    '        End Using
''    '    Catch ex As Exception
''    '        _lastError = $"حدث خطأ أثناء حذف الأرشيف بمعرف {archiveId}: {ex.Message}"
''    '        If showMessages Then
''    '            MessageBox.Show(_lastError, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
''    '        End If
''    '        Return False
''    '    End Try
''    'End Function

''    'Public Function DeleteArchiveById(archiveId As Integer, Optional showMessages As Boolean = True) As Boolean
''    '    Dim transaction As SqlTransaction = Nothing
''    '    Try
''    '        Using connection As SqlConnection = GetConnection()
''    '            connection.Open()
''    '            transaction = connection.BeginTransaction()

''    '            ' الحصول على معلومات الأرشيف
''    '            Dim dt As DataTable = GetArchiveById(archiveId)
''    '            Dim currentFilePath As String = ""
''    '            Dim currentOriginalFilePath As String = ""

''    '            If dt.Rows.Count > 0 Then
''    '                currentFilePath = If(dt.Rows(0)("FilePath") IsNot DBNull.Value, dt.Rows(0)("FilePath").ToString(), "")
''    '                currentOriginalFilePath = If(dt.Rows(0)("OriginalFilePath") IsNot DBNull.Value, dt.Rows(0)("OriginalFilePath").ToString(), "")
''    '            End If

''    '            ' حذف السجل من قاعدة البيانات
''    '            Dim deleteQuery As String = "DELETE FROM TbArchive WHERE ID = @ID"
''    '            Using command As New SqlCommand(deleteQuery, connection, transaction)
''    '                command.Parameters.AddWithValue("@ID", archiveId)
''    '                Dim result As Boolean = command.ExecuteNonQuery() > 0

''    '                ' حذف الملف من المسار الحالي فقط (وليس من المسار الأصلي)
''    '                If result AndAlso Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath) Then
''    '                    Try
''    '                        File.Delete(currentFilePath)
''    '                        If showMessages Then
''    '                            MessageBox.Show("تم حذف الأرشيف والملف المرتبط بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
''    '                        End If
''    '                    Catch
''    '                        ' في حالة فشل حذف الملف، نقله إلى مجلد المحذوفات
''    '                        MoveToDeletedFolder(currentFilePath)
''    '                        If showMessages Then
''    '                            MessageBox.Show("تم حذف الأرشيف من قاعدة البيانات ونقل الملف إلى مجلد المحذوفات", "نجح جزئي", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''    '                        End If
''    '                    End Try
''    '                ElseIf result Then
''    '                    If showMessages Then
''    '                        MessageBox.Show("تم حذف الأرشيف بنجاح (لا يوجد مسار ملف مرتبط)", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
''    '                    End If
''    '                End If

''    '                ' تأكيد المعاملة
''    '                transaction.Commit()
''    '                Return result
''    '            End Using
''    '        End Using
''    '    Catch ex As Exception
''    '        ' التراجع عن المعاملة في حالة حدوث خطأ
''    '        If transaction IsNot Nothing Then
''    '            transaction.Rollback()
''    '        End If

''    '        _lastError = $"حدث خطأ أثناء حذف الأرشيف بمعرف {archiveId}: {ex.Message}"
''    '        If showMessages Then
''    '            MessageBox.Show(_lastError, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
''    '        End If
''    '        Return False
''    '    End Try
''    'End Function

''    '''' <summary>
''    '''' نقل الملف إلى مجلد المحذوفات عند فشل حذفه
''    '''' </summary>
''    '''' <param name="filePath">مسار الملف الذي فشل حذفه</param>
''    'Private Sub MoveToDeletedFolder(filePath As String)
''    '    Try
''    '        ' تحديد مسار مجلد المحذوفات
''    '        Dim deletedFolderPath As String = Path.Combine(ARCHIVE_BASE_PATH, "مجلد المحذوفات")

''    '        ' إنشاء مجلد المحذوفات إذا لم يكن موجودًا
''    '        If Not Directory.Exists(deletedFolderPath) Then
''    '            Directory.CreateDirectory(deletedFolderPath)
''    '        End If

''    '        ' استخراج اسم الملف والامتداد
''    '        Dim fileName As String = Path.GetFileName(filePath)
''    '        Dim extension As String = Path.GetExtension(filePath)
''    '        Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(filePath)

''    '        ' إنشاء اسم ملف فريد باستخدام التاريخ والوقت
''    '        Dim newFileName As String = $"{fileNameWithoutExtension}_{DateTime.Now:yyyyMMddHHmmss}{extension}"
''    '        Dim newFilePath As String = Path.Combine(deletedFolderPath, newFileName)

''    '        ' نقل الملف إلى مجلد المحذوفات
''    '        File.Move(filePath, newFilePath)

''    '        System.Diagnostics.Debug.WriteLine($"تم نقل الملف إلى مجلد المحذوفات: {newFilePath}")
''    '    Catch ex As Exception
''    '        ' تسجيل الخطأ إذا فشل النقل أيضًا
''    '        System.Diagnostics.Debug.WriteLine($"فشل في نقل الملف إلى مجلد المحذوفات: {ex.Message}")

''    '        ' محاولة حذف الملف مرة أخرى كحل أخير
''    '        Try
''    '            If File.Exists(filePath) Then
''    '                File.Delete(filePath)
''    '            End If
''    '        Catch
''    '            ' إذا فشل كل شيء، نسجل الخطأ
''    '            System.Diagnostics.Debug.WriteLine("فشل في حذف الملف نهائياً")
''    '        End Try
''    '    End Try
''    'End Sub

''    ''' <summary>
''    ''' نقل الملف إلى مجلد المحذوفات مع معالجة أفضل للأخطاء
''    ''' </summary>
''    ''' <param name="filePath">مسار الملف الذي يجب نقله</param>
''    Private Sub MoveToDeletedFolder(filePath As String)
''        Try
''            ' التحقق من وجود الملف
''            If String.IsNullOrEmpty(filePath) OrElse Not File.Exists(filePath) Then
''                Return
''            End If

''            ' تحديد مسار مجلد المحذوفات - استخدام الاسم الصحيح "المحذوفات" وليس "مجلد المحذوفات"
''            Dim deletedFolderPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, "المحذوفات")

''            ' إنشاء مجلد المحذوفات إذا لم يكن موجودًا
''            If Not Directory.Exists(deletedFolderPath) Then
''                Directory.CreateDirectory(deletedFolderPath)
''            End If

''            ' إنشاء اسم ملف فريد
''            Dim fileName As String = Path.GetFileName(filePath)
''            Dim extension As String = Path.GetExtension(filePath)
''            Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(fileName)
''            Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
''            Dim newFileName As String = $"{fileNameWithoutExtension}_deleted_{timestamp}{extension}"
''            Dim newFilePath As String = Path.Combine(deletedFolderPath, newFileName)

''            ' محاولة نقل الملف مع إعادة المحاولة
''            Dim maxRetries As Integer = 3
''            Dim retryCount As Integer = 0
''            Dim success As Boolean = False

''            While retryCount < maxRetries
''                Try
''                    ' التحقق من أن الملف مغلق
''                    Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None)
''                        ' إذا نجحت، فالمشكلة ليست في الاستخدام
''                    End Using
''                Catch ex As IOException
''                    ' الملف قيد الاستخدام، الانتظار وإعادة المحاولة
''                    Threading.Thread.Sleep(500)
''                    retryCount += 1
''                    Continue While
''                End Try

''                Try
''                    ' محاولة نقل الملف
''                    File.Move(filePath, newFilePath)
''                    success = True
''                    Exit While
''                Catch ex As Exception
''                    ' الانتظار وإعادة المحاولة
''                    Threading.Thread.Sleep(500)
''                    retryCount += 1
''                End Try
''            End While

''            If Not success Then
''                ' إذا فشلت جميع المحاولات، حاول حذف الملف
''                Try
''                    'File.Delete(filePath)
''                    System.Diagnostics.Debug.WriteLine("فشل نقل الملف، تم حذفه بدلاً من ذلك")
''                Catch
''                    System.Diagnostics.Debug.WriteLine("فشل نقل الملف وحذفه")
''                End Try
''            End If

''            System.Diagnostics.Debug.WriteLine($"تم نقل الملف إلى مجلد المحذوفات: {newFilePath}")
''        Catch ex As Exception
''            System.Diagnostics.Debug.WriteLine($"فشل في نقل الملف إلى مجلد المحذوفات: {ex.Message}")
''        End Try
''    End Sub

''    '=======================================================================================================

''    ''' <summary>
''    ''' حذف سجل الأرشيف مع نقل الملف إلى مجلد المحذوفات كأولوية
''    ''' </summary>
''    ''' <param name="archiveId">معرف الأرشيف</param>
''    ''' <param name="showMessages">إظهار رسائل للمستخدم</param>
''    ''' <returns>صحيح إذا تم الحذف بنجاح</returns>
''    Public Function DeleteArchiveById(
''    ByVal archiveId As Integer,
''    Optional ByVal showMessages As Boolean = True) As Boolean

''        Dim transaction As SqlTransaction = Nothing
''        Try
''            Using connection As SqlConnection = GetConnection()
''                connection.Open()
''                transaction = connection.BeginTransaction()

''                Try
''                    ' 1. الحصول على معلومات الأرشيف
''                    Dim dt As DataTable = GetArchiveById(archiveId)
''                    Dim currentFilePath As String = ""
''                    Dim archiveNum As String = ""

''                    If dt.Rows.Count > 0 Then
''                        ' التأكد من استخدام الحقل الصحيح لمسار الملف
''                        If dt.Columns.Contains("FilePath") AndAlso Not IsDBNull(dt.Rows(0)("FilePath")) Then
''                            currentFilePath = dt.Rows(0)("FilePath").ToString()
''                        ElseIf dt.Columns.Contains("filepath") AndAlso Not IsDBNull(dt.Rows(0)("filepath")) Then
''                            currentFilePath = dt.Rows(0)("filepath").ToString()
''                        End If

''                        ' التأكد من أن المسار مُحدد بشكل صحيح
''                        If Not String.IsNullOrEmpty(currentFilePath) Then
''                            currentFilePath = Path.GetFullPath(currentFilePath)
''                        End If

''                        ' الحصول على رقم الأرشيف
''                        archiveNum = If(Not IsDBNull(dt.Rows(0)("ArchiveNum")), dt.Rows(0)("ArchiveNum").ToString(), $"المعرف {archiveId}")
''                    Else
''                        Throw New Exception($"الأرشيف بمعرف {archiveId} غير موجود")
''                    End If

''                    ' 2. أولوية نقل الملف إلى مجلد المحذوفات
''                    Dim fileMoved As Boolean = False
''                    Dim newFilePath As String = ""

''                    If Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath) Then
''                        ' محاولة نقل الملف إلى مجلد المحذوفات
''                        newFilePath = MoveFileToDeletedFolder(currentFilePath, archiveNum, showMessages)
''                        fileMoved = Not String.IsNullOrEmpty(newFilePath)
''                    End If

''                    ' 3. حذف السجل من قاعدة البيانات
''                    Dim deleteQuery As String = "DELETE FROM TbArchive WHERE ID = @ID"
''                    Using command As New SqlCommand(deleteQuery, connection, transaction)
''                        command.Parameters.AddWithValue("@ID", archiveId)
''                        Dim rowsAffected As Integer = command.ExecuteNonQuery()

''                        ' 4. تأكيد المعاملة
''                        transaction.Commit()

''                        ' 5. تحديث عداد الأرشيف
''                        DataHelper.UpdateArchiveCount()

''                        ' 6. إظهار رسالة النجاح المناسبة
''                        If fileMoved Then
''                            If showMessages Then
''                                MessageBox.Show(
''                                $"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات{vbNewLine}" &
''                                $"ونقل الملف إلى مجلد المحذوفات بنجاح",
''                                "نجاح",
''                                MessageBoxButtons.OK,
''                                MessageBoxIcon.Information)
''                            End If
''                        Else
''                            If Not String.IsNullOrEmpty(currentFilePath) Then
''                                If showMessages Then
''                                    MessageBox.Show(
''                                    $"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات{vbNewLine}" &
''                                    $"ولكن لم يتم نقل الملف (قد يكون قيد الاستخدام){vbNewLine}{vbNewLine}" &
''                                    $"يرجى نقل الملف يدويًا إلى:{vbNewLine}" &
''                                    $"{Path.Combine(My.Settings.ARCHIVE_Default_PATH, DELETED_FOLDER_NAME)}",
''                                    "نجاح جزئي",
''                                    MessageBoxButtons.OK,
''                                    MessageBoxIcon.Warning)
''                                End If
''                            Else
''                                If showMessages Then
''                                    MessageBox.Show(
''                                    $"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات{vbNewLine}" &
''                                    $"(لا يوجد ملف مرتبط بهذا الأرشيف)",
''                                    "نجاح",
''                                    MessageBoxButtons.OK,
''                                    MessageBoxIcon.Information)
''                                End If
''                            End If
''                        End If

''                        Return rowsAffected > 0
''                    End Using
''                Catch ex As Exception
''                    ' التراجع عن المعاملة في حالة حدوث خطأ
''                    If transaction IsNot Nothing Then
''                        transaction.Rollback()
''                    End If
''                    Throw
''                End Try
''            End Using
''        Catch ex As Exception
''            _lastError = $"حدث خطأ أثناء حذف الأرشيف بمعرف {archiveId}: {ex.Message}"
''            System.Diagnostics.Debug.WriteLine(_lastError)

''            If showMessages Then
''                MessageBox.Show(
''                $"حدث خطأ أثناء حذف الأرشيف: {ex.Message}",
''                "خطأ",
''                MessageBoxButtons.OK,
''                MessageBoxIcon.Error)
''            End If

''            Return False
''        End Try
''    End Function

''    ''' <summary>
''    ''' نقل الملف إلى مجلد المحذوفات مع إعادة المحاولة
''    ''' </summary>
''    ''' <param name="sourceFilePath">مسار الملف المصدر</param>
''    ''' <param name="archiveNum">رقم الأرشيف (للاستخدام في الرسائل)</param>
''    ''' <param name="showMessages">إظهار الرسائل للمستخدم</param>
''    ''' <returns>المسار الجديد للملف أو فارغ إذا فشل النقل</returns>
''    Public Function MoveFileToDeletedFolder(
''    ByVal sourceFilePath As String,
''    Optional ByVal archiveNum As String = "",
''    Optional ByVal showMessages As Boolean = True) As String

''        Try
''            ' التحقق من وجود الملف
''            If String.IsNullOrEmpty(sourceFilePath) OrElse Not File.Exists(sourceFilePath) Then
''                System.Diagnostics.Debug.WriteLine("لم يتم العثور على الملف للنقل")
''                Return ""
''            End If

''            ' الحصول على المسار الأساسي للأرشيف
''            Dim archiveBasePath As String = My.Settings.ARCHIVE_Default_PATH
''            If String.IsNullOrEmpty(archiveBasePath) OrElse Not Directory.Exists(archiveBasePath) Then
''                Throw New Exception("مسار الأرشيف الافتراضي غير معرف أو غير موجود")
''            End If

''            ' تحديد مسار مجلد المحذوفات
''            Dim deletedFolderPath As String = Path.Combine(archiveBasePath, DELETED_FOLDER_NAME)
''            If Not Directory.Exists(deletedFolderPath) Then
''                Directory.CreateDirectory(deletedFolderPath)
''            End If

''            ' إنشاء اسم ملف فريد
''            Dim fileName As String = Path.GetFileName(sourceFilePath)
''            Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(fileName)
''            Dim fileExtension As String = Path.GetExtension(fileName)
''            Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
''            Dim newFileName As String = $"{fileNameWithoutExtension}_deleted_{timestamp}{fileExtension}"
''            Dim destinationFilePath As String = Path.Combine(deletedFolderPath, newFileName)

''            ' محاولة نقل الملف مع إعادة المحاولة
''            Dim success As Boolean = False
''            Dim errorMessage As String = ""

''            For retryCount As Integer = 1 To MAX_RETRY_ATTEMPTS
''                Try
''                    ' التأكد من إغلاق الملف
''                    CloseFileIfOpen(sourceFilePath)

''                    ' محاولة النقل
''                    File.Move(sourceFilePath, destinationFilePath)
''                    success = True
''                    Exit For
''                Catch ex As IOException
''                    ' الملف قيد الاستخدام، الانتظار وإعادة المحاولة
''                    errorMessage = ex.Message
''                    Threading.Thread.Sleep(RETRY_DELAY_MS)
''                Catch ex As Exception
''                    errorMessage = ex.Message
''                    Exit For
''                End Try
''            Next

''            If success Then
''                System.Diagnostics.Debug.WriteLine($"تم نقل الملف إلى مجلد المحذوفات: {destinationFilePath}")
''                Return destinationFilePath
''            Else
''                Dim logMessage As String = $"فشل في نقل الملف {sourceFilePath} إلى مجلد المحذوفات بعد {MAX_RETRY_ATTEMPTS} محاولة: {errorMessage}"
''                System.Diagnostics.Debug.WriteLine(logMessage)

''                If showMessages Then
''                    MessageBox.Show(
''                    $"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات{vbNewLine}" &
''                    $"لكن فشل في نقل الملف إلى مجلد المحذوفات.{vbNewLine}{vbNewLine}" &
''                    $"يرجى نقل الملف يدويًا إلى:{vbNewLine}{deletedFolderPath}",
''                    "تحذير",
''                    MessageBoxButtons.OK,
''                    MessageBoxIcon.Warning)
''                End If

''                Return ""
''            End If
''        Catch ex As Exception
''            System.Diagnostics.Debug.WriteLine($"خطأ في نقل الملف إلى مجلد المحذوفات: {ex.Message}")
''            Return ""
''        End Try
''    End Function

''    ''' <summary>
''    ''' محاولة إغلاق الملف إذا كان مفتوحًا بواسطة عملية أخرى
''    ''' </summary>
''    ''' <param name="filePath">مسار الملف</param>
''    Private Sub CloseFileIfOpen(filePath As String)
''        Try
''            ' التحقق من أن الملف موجود
''            If Not File.Exists(filePath) Then
''                Return
''            End If

''            ' محاولة فتح الملف للقراءة لاختبار إغلاقه
''            Using fs As FileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None)
''                ' إذا تم تنفيذ هذا السطر، فهذا يعني أن الملف مغلق بالفعل
''            End Using
''        Catch ex As IOException
''            ' إذا كان الملف مفتوحًا، سيتم إلقاء استثناء هنا
''            ' لا حاجة للقيام بشيء، سيتم محاولة النقل مرة أخرى
''        Catch ex As Exception
''            ' معالجة الأخطاء الأخرى
''            System.Diagnostics.Debug.WriteLine($"خطأ في محاولة إغلاق الملف: {ex.Message}")
''        End Try
''    End Sub


''    '================================================================================================

''    'Private Sub MoveToDeletedFolder(filePath As String)
''    '    Try
''    '        ' التحقق من وجود الملف
''    '        If String.IsNullOrEmpty(filePath) OrElse Not File.Exists(filePath) Then
''    '            Return
''    '        End If

''    '        ' تحديد مسار مجلد المحذوفات
''    '        Dim deletedFolderPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, "مجلد المحذوفات")

''    '        ' إنشاء مجلد المحذوفات إذا لم يكن موجودًا
''    '        If Not Directory.Exists(deletedFolderPath) Then
''    '            Directory.CreateDirectory(deletedFolderPath)
''    '        End If

''    '        ' التحقق من أن الملف مغلق
''    '        For i As Integer = 0 To 2
''    '            Try
''    '                ' التحقق من أن الملف مغلق
''    '                Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None)
''    '                    ' إذا نجحت، فالمشكلة ليست في الاستخدام
''    '                End Using
''    '                Exit For
''    '            Catch ex As IOException
''    '                Threading.Thread.Sleep(500)
''    '            End Try
''    '        Next

''    '        ' إنشاء اسم ملف فريد
''    '        Dim fileName As String = Path.GetFileName(filePath)
''    '        Dim extension As String = Path.GetExtension(filePath)
''    '        Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(fileName)
''    '        Dim newFileName As String = $"{fileNameWithoutExtension}_{DateTime.Now:yyyyMMddHHmmss}{extension}"
''    '        Dim newFilePath As String = Path.Combine(deletedFolderPath, newFileName)

''    '        ' محاولة نقل الملف
''    '        File.Move(filePath, newFilePath)

''    '        System.Diagnostics.Debug.WriteLine($"تم نقل الملف إلى مجلد المحذوفات: {newFilePath}")
''    '    Catch ex As Exception
''    '        System.Diagnostics.Debug.WriteLine($"فشل في نقل الملف إلى مجلد المحذوفات: {ex.Message}")

''    '        ' محاولة حذف الملف كحل أخير
''    '        Try
''    '            If File.Exists(filePath) Then
''    '                File.Delete(filePath)
''    '            End If
''    '        Catch
''    '            System.Diagnostics.Debug.WriteLine("فشل في حذف الملف نهائياً")
''    '        End Try
''    '    End Try
''    'End Sub

''    'Public Function DeleteArchive(id As Integer) As Boolean
''    '    Try
''    '        ' جلب مسار الملف قبل الحذف من قاعدة البيانات
''    '        Dim dt As DataTable = GetArchiveById(id)
''    '        Dim filePathToDelete As String = String.Empty
''    '        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
''    '            filePathToDelete = dt.Rows(0)("filepath").ToString()
''    '        End If

''    '        Dim parameters() As SqlParameter = {
''    '            DatabaseHelper.CreateParameter("@ID", id)
''    '        }

''    '        Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("DeleteArchive", parameters)

''    '        If rowsAffected > 0 Then
''    '            ' محاولة حذف الملف من نظام الملفات بعد الحذف من قاعدة البيانات
''    '            If File.Exists(filePathToDelete) Then
''    '                Try
''    '                    ' محاولة حذف الملف مع إعادة المحاولة
''    '                    DeleteFileWithRetry(filePathToDelete, 3)
''    '                Catch fileEx As Exception
''    '                    ' إذا فشل حذف الملف، نقله إلى مجلد المحذوفات
''    '                    Try
''    '                        MoveFileToDeletedFolder(filePathToDelete)
''    '                    Catch moveEx As Exception
''    '                        ' إذا فشل النقل أيضًا، اترك الملف واعرض تحذير
''    '                        Throw New Exception($"تم حذف السجل من قاعدة البيانات ولكن فشل في حذف الملف: {filePathToDelete}{vbNewLine} السبب: {fileEx.Message}{vbNewLine}يرجى إغلاق أي برامج تستخدم هذا الملف وحذفه يدوياً.")
''    '                    End Try
''    '                End Try
''    '            End If
''    '            Return True
''    '        Else
''    '            Return False
''    '        End If
''    '    Catch ex As Exception
''    '        Throw New Exception("خطأ في حذف الأرشيف: " & ex.Message)
''    '    End Try
''    'End Function


''    ''' <summary>
''    ''' تحديد سبب فشل حذف الملف
''    ''' </summary>
''    Private Function GetFileDeleteFailureReason(filePath As String) As String
''        Try
''            ' التحقق من وجود الملف
''            If Not File.Exists(filePath) Then
''                Return "الملف غير موجود"
''            End If

''            ' محاولة فتح الملف للقراءة (للتحقق مما إذا كان قيد الاستخدام)
''            Try
''                Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None)
''                    ' إذا نجحت هذه السطر، فالمشكلة ليست في الاستخدام
''                End Using
''            Catch ex As IOException When ex.Message.Contains("process cannot access the file")
''                Return "الملف قيد الاستخدام من قبل برنامج آخر"
''            Catch ex As UnauthorizedAccessException
''                Return "صلاحيات غير كافية لحذف الملف"
''            Catch
''                ' تجاهل الأخطاء الأخرى
''            End Try

''            ' التحقق من الصلاحيات
''            Try
''                Dim permissionTestPath As String = Path.Combine(Path.GetDirectoryName(filePath), "permission_test.tmp")
''                File.WriteAllText(permissionTestPath, "test")
''                File.Delete(permissionTestPath)
''            Catch
''                Return "مشكلة في الصلاحيات - لا يمكن الكتابة في هذا المجلد"
''            End Try

''            ' التحقق من خصائص الملف
''            Dim fileInfo As New FileInfo(filePath)
''            If fileInfo.IsReadOnly Then
''                Return "الملف محدد كـ 'للقراءة فقط'"
''            End If

''            ' إذا لم تكن أي من الأسباب السابقة، فربما مشكلة أخرى
''            Return "سبب غير معروف (قد يكون الملف قيد الاستخدام من قبل عملية نظام)"
''        Catch ex As Exception
''            Return $"خطأ غير متوقع: {ex.Message}"
''        End Try
''    End Function
''#End Region

''#Region "الدوال المساعدة لاسترداد البيانات"
''    ''' <summary>
''    ''' جلب جميع السجلات من جدول الأرشيف
''    ''' </summary>

''    Public Function GetAllArchive() As DataTable
''        Try
''            Dim query As String = "SELECT *, TRY_CAST(ID AS INT) AS NumericID FROM TbArchive ORDER BY ArchiveDate DESC"
''            Dim dt As DataTable = DatabaseHelper.ExecuteQuery(query)

''            ' إذا كانت NumericID تحتوي على قيم غير صالحة، استبدلها بـ 0
''            For Each row As DataRow In dt.Rows
''                If row("NumericID") Is DBNull.Value Then
''                    row("NumericID") = 0
''                End If
''            Next

''            Return dt
''        Catch ex As Exception
''            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على جميع الأرشيف: {ex.Message}")
''            Return Nothing
''        End Try
''    End Function


''    'Public Function GetAllArchive() As DataTable
''    '    Try
''    '        Dim query As String = "SELECT * FROM TbArchive ORDER BY ArchiveDate DESC"
''    '        Return DatabaseHelper.ExecuteQuery(query)
''    '    Catch ex As Exception
''    '        _lastError = "خطأ في جلب بيانات الأرشيف: " & ex.Message
''    '        Throw New Exception(_lastError)
''    '    End Try
''    'End Function

''    ''' <summary>
''    ''' جلب سجل الأرشيف بالمعرف
''    ''' </summary>
''    Public Function GetArchiveById(archiveId As Integer) As DataTable
''        Try
''            Dim query As String = "SELECT * FROM TbArchive WHERE ID = @ID"
''            Using connection As SqlConnection = GetConnection()
''                Using command As New SqlCommand(query, connection)
''                    command.Parameters.AddWithValue("@ID", archiveId)
''                    Using adapter As New SqlDataAdapter(command)
''                        Dim dt As New DataTable()
''                        adapter.Fill(dt)
''                        Return dt
''                    End Using
''                End Using
''            End Using
''        Catch ex As Exception
''            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على الأرشيف بالمعرف: {ex.Message}")
''            Return Nothing
''        End Try
''    End Function

''    ''' <summary>
''    ''' جلب أنواع الملفات المميزة
''    ''' </summary>
''    Public Function GetDistinctFileTypes() As DataTable
''        Try
''            Dim query As String = "SELECT DISTINCT FileType FROM TbArchive WHERE FileType IS NOT NULL ORDER BY FileType"
''            Return DatabaseHelper.ExecuteQuery(query)
''        Catch ex As Exception
''            _lastError = "خطأ في جلب أنواع الملفات: " & ex.Message
''            Throw New Exception(_lastError)
''        End Try
''    End Function

''    ''' <summary>
''    ''' جلب الأرشيف حسب نوع الملف
''    ''' </summary>
''    Public Function GetArchiveByFileType(fileType As String) As DataTable
''        Try
''            Dim query As String = "SELECT * FROM TbArchive WHERE FileType = @FileType ORDER BY ArchiveDate DESC"
''            Dim parameters() As SqlParameter = {
''                DatabaseHelper.CreateParameter("@FileType", fileType)
''            }
''            Return DatabaseHelper.ExecuteQuery(query, parameters)
''        Catch ex As Exception
''            _lastError = "خطأ في جلب الأرشيف حسب نوع الملف: " & ex.Message
''            Throw New Exception(_lastError)
''        End Try
''    End Function

''    ''' <summary>
''    ''' البحث في الأرشيف
''    ''' </summary>
''    Public Function SearchArchive(searchTerm As String) As DataTable
''        Try
''            Dim query As String = "SELECT * FROM TbArchive WHERE " &
''                                 "ArchiveNum LIKE @SearchTerm OR " &
''                                 "ArchiveSubject LIKE @SearchTerm OR " &
''                                 "FileName LIKE @SearchTerm OR " &
''                                 "ArchiveFolder LIKE @SearchTerm OR " &
''                                 "ArchiveWillMaster LIKE @SearchTerm OR " &
''                                 "ArchiveWillSub LIKE @SearchTerm " &
''                                 "ORDER BY ArchiveDate DESC"
''            Dim parameters() As SqlParameter = {
''                DatabaseHelper.CreateParameter("@SearchTerm", "%" & searchTerm & "%")
''            }
''            Return DatabaseHelper.ExecuteQuery(query, parameters)
''        Catch ex As Exception
''            _lastError = "خطأ في البحث في الأرشيف: " & ex.Message
''            Throw New Exception(_lastError)
''        End Try
''    End Function

''    ''' <summary>
''    ''' البحث في الأرشيف حسب النطاق الزمني
''    ''' </summary>
''    Public Function SearchSaderByDateRange(startDate As Date, endDate As Date) As DataTable
''        Try
''            ' التأكد من صحة التواريخ
''            If startDate > endDate Then
''                Dim temp As Date = startDate
''                startDate = endDate
''                endDate = temp
''            End If

''            Dim query As String = "SELECT ID, ArchiveNum, ArchiveDate, ArchiveSubject, ArchiveWillMaster, ArchiveWillSub, UserName FROM TbArchive WHERE ArchiveDate BETWEEN @startDate AND @endDate ORDER BY ArchiveDate DESC"
''            Using connection As SqlConnection = GetConnection()
''                connection.Open()
''                Using command As New SqlCommand(query, connection)
''                    command.Parameters.AddWithValue("@startDate", startDate.Date)
''                    command.Parameters.AddWithValue("@endDate", endDate.Date.AddDays(1).AddSeconds(-1))

''                    Using adapter As New SqlDataAdapter(command)
''                        Dim dt As New DataTable()
''                        adapter.Fill(dt)
''                        Return dt
''                    End Using
''                End Using
''            End Using
''        Catch ex As Exception
''            _lastError = "خطأ في البحث في الأرشيف حسب النطاق الزمني: " & ex.Message
''            Throw New Exception(_lastError)
''        End Try
''    End Function

''    ''' <summary>
''    ''' البحث في الأرشيف حسب النطاق الزمني
''    ''' </summary>
''    ''' <param name="startDate">تاريخ البدء</param>
''    ''' <param name="endDate">تاريخ الانتهاء</param>
''    ''' <returns>جدول بيانات بالنتائج</returns>

''    Public Function SearchArchiveByDateRange(startDate As Date, endDate As Date) As DataTable
''        Try
''            Dim query As String = "SELECT * FROM TbArchive WHERE ArchiveDate BETWEEN @StartDate AND @EndDate ORDER BY ArchiveDate DESC, ID DESC"
''            Dim parameters() As SqlParameter = {
''                DatabaseHelper.CreateParameter("@StartDate", startDate.Date),
''                DatabaseHelper.CreateParameter("@EndDate", endDate.Date)
''            }

''            Return DatabaseHelper.ExecuteQuery(query, parameters)
''        Catch ex As Exception
''            System.Diagnostics.Debug.WriteLine($"خطأ في SearchSaderByDateRange: {ex.Message}")
''            Return New DataTable()
''        End Try
''    End Function

''    'Public Function SearchArchiveByDateRange(startDate As Date, endDate As Date) As DataTable
''    '    Try
''    '        ' التأكد من صحة التواريخ
''    '        If startDate > endDate Then
''    '            Dim temp As Date = startDate
''    '            startDate = endDate
''    '            endDate = temp
''    '        End If

''    '        ' تعديل التواريخ لتغطية اليوم الكامل
''    '        ' تحويل تاريخ البدء إلى بداية اليوم
''    '        Dim searchStartDate As Date = startDate.Date
''    '        ' تحويل تاريخ الانتهاء إلى نهاية اليوم
''    '        Dim searchEndDate As Date = endDate.Date.AddDays(1).AddSeconds(-1)

''    '        Dim query As String = "SELECT ID, ArchiveNum, ArchiveDate, ArchiveSubject, ArchiveWillMaster, ArchiveWillSub, UserName FROM TbArchive WHERE ArchiveDate BETWEEN @startDate AND @endDate ORDER BY ArchiveDate DESC"

''    '        Using connection As SqlConnection = GetConnection()
''    '            connection.Open()
''    '            Using command As New SqlCommand(query, connection)
''    '                command.Parameters.AddWithValue("@startDate", searchStartDate)
''    '                command.Parameters.AddWithValue("@endDate", searchEndDate)

''    '                Using adapter As New SqlDataAdapter(command)
''    '                    Dim dt As New DataTable()
''    '                    adapter.Fill(dt)
''    '                    Return dt
''    '                End Using
''    '            End Using
''    '        End Using
''    '    Catch ex As Exception
''    '        _lastError = "خطأ في البحث في الأرشيف حسب النطاق الزمني: " & ex.Message
''    '        Throw New Exception(_lastError)
''    '    End Try
''    'End Function



''    ''' <summary>
''    ''' الحصول على مسار الملف من جدول الأرشيف
''    ''' </summary>
''    ''' <param name="archiveID">معرف الأرشيف</param>
''    ''' <returns>مسار الملف أو سلسلة فارغة إذا لم يتم العثور على الأرشيف</returns>
''    Public Function GetFilePathByArchiveID(archiveID As Integer) As String
''        Try
''            Dim dt As DataTable = GetArchiveById(archiveID)
''            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
''                ' الحصول على مسار الملف الحالي (FilePath)
''                If dt.Rows(0)("FilePath") IsNot DBNull.Value AndAlso
''               Not String.IsNullOrEmpty(dt.Rows(0)("FilePath").ToString()) Then
''                    Return dt.Rows(0)("FilePath").ToString()
''                End If
''            End If
''            Return String.Empty
''        Catch ex As Exception
''            System.Diagnostics.Debug.WriteLine($"خطأ في GetFilePathByArchiveID: {ex.Message}")
''            Return String.Empty
''        End Try
''    End Function
''#End Region

''#Region "التحقق من التكرار"


''    ''' <summary>
''    ''' التحقق من تكرار رقم الأرشيف
''    ''' </summary>
''    Public Function CheckDuplicateArchiveNumber(archiveNum As String) As Boolean
''        Try
''            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE ArchiveNum = @ArchiveNum"
''            Dim parameters() As SqlParameter = {
''                DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum)
''            }
''            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
''            Return count > 0
''        Catch ex As Exception
''            Throw New Exception("خطأ في التحقق من تكرار رقم الأرشيف: " & ex.Message)
''        End Try
''    End Function

''    ''' <summary>
''    ''' التحقق من تكرار رقم الأرشيف عند التحديث (استثناء السجل الحالي)
''    ''' </summary>
''    Public Function CheckDuplicateArchiveNumberForUpdate(archiveNum As String, currentId As Integer) As Boolean
''        Try
''            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE ArchiveNum = @ArchiveNum AND ID <> @CurrentId"
''            Dim parameters() As SqlParameter = {
''                DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum),
''                DatabaseHelper.CreateParameter("@CurrentId", currentId)
''            }
''            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
''            Return count > 0
''        Catch ex As Exception
''            Throw New Exception("خطأ في التحقق من تكرار رقم الأرشيف عند التحديث: " & ex.Message)
''        End Try
''    End Function

''    ''' <summary>
''    ''' الحصول على معلومات الأرشيف المكرر بناءً على رقم الأرشيف
''    ''' </summary>
''    Public Function GetDuplicateArchiveInfo(archiveNum As String) As DataTable
''        Try
''            Dim query As String = "SELECT ArchiveNum, ArchiveSubject, ArchiveDate, ArchiveFolder, UserName FROM TbArchive WHERE ArchiveNum = @ArchiveNum"
''            Dim parameters() As SqlParameter = {
''                DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum)
''            }
''            Return DatabaseHelper.ExecuteQuery(query, parameters)
''        Catch ex As Exception
''            Throw New Exception("خطأ في جلب معلومات الأرشيف المكرر: " & ex.Message)
''        End Try
''    End Function

''    ''' <summary>
''    ''' التحقق من تكرار الملف
''    ''' </summary>
''    Public Function CheckDuplicateFile(fileName As String, fileType As String, archiveFolder As String) As Boolean
''        Try
''            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE FileName = @FileName AND FileType = @FileType AND ArchiveFolder = @ArchiveFolder"
''            Dim parameters() As SqlParameter = {
''                DatabaseHelper.CreateParameter("@FileName", fileName),
''                DatabaseHelper.CreateParameter("@FileType", fileType),
''                DatabaseHelper.CreateParameter("@ArchiveFolder", archiveFolder)
''            }
''            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
''            Return count > 0
''        Catch ex As Exception
''            Throw New Exception("خطأ في التحقق من تكرار الملف: " & ex.Message)
''        End Try
''    End Function

''    ''' <summary>
''    ''' الحصول على معلومات الملف المكرر
''    ''' </summary>
''    Public Function GetDuplicateFileInfo(fileName As String, fileType As String, archiveFolder As String) As DataTable
''        Try
''            Dim query As String = "SELECT ArchiveNum, ArchiveSubject, ArchiveDate, UserName FROM TbArchive WHERE FileName = @FileName AND FileType = @FileType AND ArchiveFolder = @ArchiveFolder"
''            Dim parameters() As SqlParameter = {
''                DatabaseHelper.CreateParameter("@FileName", fileName),
''                DatabaseHelper.CreateParameter("@FileType", fileType),
''                DatabaseHelper.CreateParameter("@ArchiveFolder", archiveFolder)
''            }
''            Return DatabaseHelper.ExecuteQuery(query, parameters)
''        Catch ex As Exception
''            Throw New Exception("خطأ في جلب معلومات الملف المكرر: " & ex.Message)
''        End Try
''    End Function
''#End Region

''#Region "الكود القديم - للرجوع فقط"
''    '
''    '' الكود القديم الذي تم استبداله، تم الاحتفاظ به لأغراض الرجوع
''    '
''    '' دالة إضافة الأرشيف القديمة
''    ''Public Function AddArchive(archiveNum As String, archiveDate As Date, archiveSubject As String, archiveFolder As String, archiveNotes As String, fileName As String, fileType As String, filePath As String, archiveWillMaster As String, archiveWillSub As String, userName As String) As Integer
''    ''    Try
''    ''        ' تحليل مسار الأرشيف وإنشاء المجلدات المناسبة
''    ''        Dim finalArchivePath As String = ""
''    ''
''    ''        If archiveFolder.Contains("\") Then
''    ''            Dim parts() As String = archiveFolder.Split("\"c)
''    ''            If parts.Length >= 2 Then
''    ''                Dim category As String = parts(0)
''    ''                Dim subFolder As String = parts(1)
''    ''                finalArchivePath = CreateSubFolder(category, subFolder)
''    ''            Else
''    ''                finalArchivePath = Path.Combine(ARCHIVE_BASE_PATH, archiveFolder)
''    ''                If Not Directory.Exists(finalArchivePath) Then
''    ''                    Directory.CreateDirectory(finalArchivePath)
''    ''                End If
''    ''            End If
''    ''        Else
''    ''            finalArchivePath = Path.Combine(ARCHIVE_BASE_PATH, archiveFolder)
''    ''            If Not Directory.Exists(finalArchivePath) Then
''    ''                Directory.CreateDirectory(finalArchivePath)
''    ''            End If
''    ''        End If
''    ''
''    ''        ' التحقق من أن اسم الملف يحتوي على الامتداد الصحيح
''    ''        Dim fileExtension As String = Path.GetExtension(filePath) ' الحصول على الامتداد من المسار الكامل
''    ''        If Not String.IsNullOrEmpty(fileExtension) AndAlso Not fileName.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) Then
''    ''            fileName = Path.GetFileNameWithoutExtension(fileName) & fileExtension
''    ''        End If
''    ''
''    ''        ' تحديد المسار النهائي للملف
''    ''        Dim destinationPath As String = Path.Combine(finalArchivePath, fileName)
''    ''
''    ''        ' التحقق من وجود الملف المصدر
''    ''        If File.Exists(filePath) Then
''    ''            ' نسخ الملف إلى المجلد المحدد
''    ''            File.Copy(filePath, destinationPath, True)
''    ''        End If
''    ''
''    ''        ' إدراج السجل في قاعدة البيانات
''    ''        Dim query As String = "INSERT INTO TbArchive (ArchiveNum, ArchiveDate, ArchiveSubject, ArchiveNotes, ArchiveFolder, FileName, FileType, FilePath, ArchiveWillMaster, ArchiveWillSub, UserName) VALUES (@ArchiveNum, @ArchiveDate, @ArchiveSubject, @ArchiveNotes, @ArchiveFolder, @FileName, @FileType, @FilePath, @ArchiveWillMaster, @ArchiveWillSub, @UserName); SELECT SCOPE_IDENTITY();"
''    ''
''    ''        Using connection As SqlConnection = GetConnection()
''    ''            Using command As New SqlCommand(query, connection)
''    ''                command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
''    ''                command.Parameters.AddWithValue("@ArchiveDate", archiveDate)
''    ''                command.Parameters.AddWithValue("@ArchiveSubject", archiveSubject)
''    ''                command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
''    ''                command.Parameters.AddWithValue("@ArchiveNotes", archiveNotes)
''    ''                command.Parameters.AddWithValue("@FileName", fileName)
''    ''                command.Parameters.AddWithValue("@FileType", fileType)
''    ''                command.Parameters.AddWithValue("@FilePath", destinationPath)
''    ''                command.Parameters.AddWithValue("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster))
''    ''                command.Parameters.AddWithValue("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub))
''    ''                command.Parameters.AddWithValue("@UserName", userName)
''    ''
''    ''                Dim result As Object = command.ExecuteScalar()
''    ''                Return Convert.ToInt32(result)
''    ''            End Using
''    ''        End Using
''    ''    Catch ex As Exception
''    ''        Throw New Exception("خطأ في إضافة الأرشيف: " & ex.Message)
''    ''    End Try
''    ''End Function
''    '
''    '' دالة حذف الأرشيف القديمة
''    ''Public Function DeleteArchiveById(archiveId As Integer, Optional showMessages As Boolean = True) As Boolean
''    ''    Try
''    ''        ' التحقق من وجود الأرشيف
''    ''        If Not ArchiveExists(archiveId) Then
''    ''            If showMessages Then
''    ''                MessageBox.Show($"الأرشيف بمعرف {archiveId} غير موجود أو تم حذفه مسبقًا", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
''    ''            End If
''    ''            Return True
''    ''        End If
''    ''
''    ''        ' الحصول على معلومات الأرشيف
''    ''        Dim dt As DataTable = GetArchiveById(archiveId)
''    ''        Dim filePathToDelete As String = ""
''    ''
''    ''        If dt.Rows.Count > 0 Then
''    ''            filePathToDelete = If(dt.Rows(0)("FilePath") IsNot DBNull.Value, dt.Rows(0)("FilePath").ToString(), "")
''    ''        End If
''    ''
''    ''        ' حذف السجل من قاعدة البيانات
''    ''        Dim query As String = "DELETE FROM TbArchive WHERE ID = @ID"
''    ''
''    ''        Using connection As SqlConnection = GetConnection()
''    ''            connection.Open()
''    ''            Using command As New SqlCommand(query, connection)
''    ''                command.Parameters.AddWithValue("@ID", archiveId)
''    ''                Dim result As Boolean = command.ExecuteNonQuery() > 0
''    ''
''    ''                ' حذف الملف من النظام إذا تم حذف السجل بنجاح
''    ''                If result AndAlso Not String.IsNullOrEmpty(filePathToDelete) AndAlso File.Exists(filePathToDelete) Then
''    ''                    Try
''    ''                        File.Delete(filePathToDelete)
''    ''                        If showMessages Then
''    ''                            MessageBox.Show("تم حذف الأرشيف والملف المرتبط بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
''    ''                        End If
''    ''                    Catch
''    ''                        ' في حالة فشل حذف الملف، نقله إلى مجلد المحذوفات
''    ''                        MoveToDeletedFolder(filePathToDelete)
''    ''                        If showMessages Then
''    ''                            MessageBox.Show("تم حذف الأرشيف من قاعدة البيانات ونقل الملف إلى مجلد المحذوفات", "نجح جزئي", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''    ''                        End If
''    ''                    End Try
''    ''                ElseIf result Then
''    ''                    If showMessages Then
''    ''                        MessageBox.Show("تم حذف الأرشيف بنجاح (لا يوجد مسار ملف مرتبط)", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
''    ''                    End If
''    ''                End If
''    ''
''    ''                Return result
''    ''            End Using
''    ''        End Using
''    ''
''    ''    Catch ex As Exception
''    ''        If showMessages Then
''    ''            MessageBox.Show($"حدث خطأ أثناء حذف الأرشيف بمعرف {archiveId}: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
''    ''        End If
''    ''        Return False
''    ''    End Try
''    ''End Function
''    '
''    '' دالة التعديل القديمة
''    ''Public Function UpdateArchive(id As Integer, archiveNum As String, archiveDate As Date, archiveSubject As String, archiveFolder As String, archiveNotes As String, fileName As String, fileType As String, filePath As String, archiveWillMaster As String, archiveWillSub As String, userName As String) As Boolean
''    ''    Try
''    ''        ' تحليل مسار الأرشيف وإنشاء المجلدات المناسبة
''    ''        Dim finalArchivePath As String = ""
''    ''
''    ''        If archiveFolder.Contains("\") Then
''    ''            Dim parts() As String = archiveFolder.Split("\"c)
''    ''            If parts.Length >= 2 Then
''    ''                Dim category As String = parts(0)
''    ''                Dim subFolder As String = parts(1)
''    ''                finalArchivePath = CreateSubFolder(category, subFolder)
''    ''            Else
''    ''                finalArchivePath = Path.Combine(ARCHIVE_BASE_PATH, archiveFolder)
''    ''                If Not Directory.Exists(finalArchivePath) Then
''    ''                    Directory.CreateDirectory(finalArchivePath)
''    ''                End If
''    ''            End If
''    ''        Else
''    ''            finalArchivePath = Path.Combine(ARCHIVE_BASE_PATH, archiveFolder)
''    ''            If Not Directory.Exists(finalArchivePath) Then
''    ''                Directory.CreateDirectory(finalArchivePath)
''    ''            End If
''    ''        End If
''    ''
''    ''        ' التحقق من أن اسم الملف يحتوي على الامتداد الصحيح
''    ''        Dim fileExtension As String = Path.GetExtension(filePath) ' الحصول على الامتداد من المسار الكامل
''    ''        If Not String.IsNullOrEmpty(fileExtension) AndAlso Not fileName.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) Then
''    ''            fileName = Path.GetFileNameWithoutExtension(fileName) & fileExtension
''    ''        End If
''    ''
''    ''        ' تحديد المسار النهائي للملف
''    ''        Dim destinationPath As String = Path.Combine(finalArchivePath, fileName)
''    ''
''    ''        ' التحقق من وجود الملف المصدر
''    ''        If File.Exists(filePath) Then
''    ''            ' نسخ الملف إلى المجلد المحدد
''    ''            File.Copy(filePath, destinationPath, True)
''    ''        End If
''    ''
''    ''        ' تحديث السجل في قاعدة البيانات
''    ''        Dim query As String = "UPDATE TbArchive SET ArchiveNum = @ArchiveNum, ArchiveDate = @ArchiveDate, ArchiveSubject = @ArchiveSubject, ArchiveNotes = @ArchiveNotes, ArchiveFolder = @ArchiveFolder, FileName = @FileName, FileType = @FileType, FilePath = @FilePath, ArchiveWillMaster = @ArchiveWillMaster, ArchiveWillSub = @ArchiveWillSub, UserName = @UserName WHERE ID = @ID"
''    ''
''    ''        Using connection As SqlConnection = GetConnection()
''    ''            Using command As New SqlCommand(query, connection)
''    ''                command.Parameters.AddWithValue("@ID", id)
''    ''                command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
''    ''                command.Parameters.AddWithValue("@ArchiveDate", archiveDate)
''    ''                command.Parameters.AddWithValue("@ArchiveSubject", archiveSubject)
''    ''                command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
''    ''                command.Parameters.AddWithValue("@ArchiveNotes", archiveNotes)
''    ''                command.Parameters.AddWithValue("@FileName", fileName)
''    ''                command.Parameters.AddWithValue("@FileType", fileType)
''    ''                command.Parameters.AddWithValue("@FilePath", destinationPath)
''    ''                command.Parameters.AddWithValue("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster))
''    ''                command.Parameters.AddWithValue("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub))
''    ''                command.Parameters.AddWithValue("@UserName", userName)
''    ''
''    ''                Dim rowsAffected As Integer = command.ExecuteNonQuery()
''    ''
''    ''                Return rowsAffected > 0
''    ''            End Using
''    ''        End Using
''    ''
''    ''    Catch ex As Exception
''    ''        Throw New Exception("خطأ في تعديل الأرشيف: " & ex.Message)
''    ''    End Try
''    ''End Function
''    '
''    '' دالة مساعدة قديمة
''    ''Private Function ArchiveExists(archiveId As Integer) As Boolean
''    ''    Try
''    ''        Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE ID = @ID"
''    ''        Dim parameters() As SqlParameter = {
''    ''            DatabaseHelper.CreateParameter("@ID", archiveId)
''    ''        }
''    ''        Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
''    ''        Return count > 0
''    ''    Catch ex As Exception
''    ''        Return False
''    ''    End Try
''    ''End Function
''    '
''    '' دالة مساعدة قديمة
''    ''Public Function GenerateNextArchiveNumber() As String
''    ''    Try
''    ''        Dim query As String = "SELECT MAX(CAST(ArchiveNum AS INT)) FROM TbArchive WHERE ISNUMERIC(ArchiveNum) = 1"
''    ''        Dim result As Object = DatabaseHelper.ExecuteScalar(query)
''    ''
''    ''        Dim nextNumber As Integer = 1
''    ''        If result IsNot Nothing AndAlso Not IsDBNull(result) Then
''    ''            nextNumber = Convert.ToInt32(result) + 1
''    ''        End If
''    ''
''    ''        Return nextNumber.ToString("D6")
''    ''    Catch ex As Exception
''    ''        Return "000001"
''    ''    End Try
''    ''End Function
''    '
''#End Region

''#Region "الدوال الإضافية"
''    ''' <summary>
''    ''' فتح موقع الملف
''    ''' </summary>
''    Public Sub OpenFileLocation(filePath As String)
''        Try
''            If Not String.IsNullOrEmpty(filePath) AndAlso File.Exists(filePath) Then
''                Process.Start("explorer.exe", $"/select,""{filePath}""")
''            End If
''        Catch ex As Exception
''            MessageBox.Show($"فشل في فتح الموقع: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
''        End Try
''    End Sub

''    ''' <summary>
''    ''' التحقق من وجود الأرشيف
''    ''' </summary>
''    Private Function ArchiveExists(archiveId As Integer) As Boolean
''        Try
''            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE ID = @ID"
''            Dim parameters() As SqlParameter = {
''                DatabaseHelper.CreateParameter("@ID", archiveId)
''            }
''            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
''            Return count > 0
''        Catch ex As Exception
''            Return False
''        End Try
''    End Function

''    ''' <summary>
''    ''' توليد رقم أرشيف تالي
''    ''' </summary>
''    Public Function GenerateNextArchiveNumber() As String
''        Try
''            Dim query As String = "SELECT MAX(CAST(ArchiveNum AS INT)) FROM TbArchive WHERE ISNUMERIC(ArchiveNum) = 1"
''            Dim result As Object = DatabaseHelper.ExecuteScalar(query)

''            Dim nextNumber As Integer = 1
''            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
''                nextNumber = Convert.ToInt32(result) + 1
''            End If

''            Return nextNumber.ToString("D6")
''        Catch ex As Exception
''            Return "000001"
''        End Try
''    End Function

''    ''' <summary>
''    ''' الحصول على معرف الأرشيف من معرف الصادر
''    ''' </summary>
''    Public Function GetArchiveIDBySaderID(saderID As Integer) As Integer
''        Try
''            Dim query As String = "SELECT ArchiveID FROM TbSader WHERE ID = @ID"
''            Using connection As SqlConnection = GetConnection()
''                Using command As New SqlCommand(query, connection)
''                    command.Parameters.AddWithValue("@ID", saderID)
''                    connection.Open()
''                    Dim result As Object = command.ExecuteScalar()
''                    If result IsNot Nothing AndAlso Not IsDBNull(result) AndAlso Convert.ToInt32(result) > 0 Then
''                        Return Convert.ToInt32(result)
''                    End If
''                End Using
''            End Using
''        Catch ex As Exception
''            System.Diagnostics.Debug.WriteLine($"خطأ في GetArchiveIDBySaderID: {ex.Message}")
''        End Try
''        Return 0
''    End Function

''    ''' <summary>
''    ''' التحقق من أن الملف مغلق وليس قيد الاستخدام
''    ''' </summary>
''    ''' <param name="filePath">مسار الملف المراد التحقق منه</param>
''    ''' <returns>صحيح إذا كان الملف مغلقًا أو غير موجود</returns>
''    Public Function IsFileClosed(filePath As String) As Boolean
''        If String.IsNullOrEmpty(filePath) Then
''            Return True
''        End If

''        Try
''            ' التحقق من وجود الملف
''            If Not File.Exists(filePath) Then
''                Return True
''            End If

''            ' محاولة فتح الملف بشكل حصري
''            Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None)
''                ' إذا نجحت، فهذا يعني أن الملف لم يعد قيد الاستخدام
''                Return True
''            End Using
''        Catch ex As IOException
''            ' إذا فشلت بسبب أن الملف قيد الاستخدام
''            If ex.Message.Contains("process cannot access the file") OrElse
''               ex.Message.Contains("العملية لا يمكنها الوصول إلى الملف") Then
''                Return False
''            Else
''                Return True ' أخطاء أخرى قد لا تعني أن الملف قيد الاستخدام
''            End If
''        Catch ex As UnauthorizedAccessException
''            ' صلاحيات غير كافية للوصول إلى الملف
''            Return False
''        Catch
''            Return False
''        End Try
''    End Function
''#End Region
''End Class





''''Imports System.Data.SqlClient
''''Imports System.IO
''''Imports System.Data

''''Public Class ArchiveClass

''''    Private ReadOnly ARCHIVE_BASE_PATH As String = My.Settings.ARCHIVE_Default_PATH ' مسار الأرشيف الأساسي على جهاز الكمبيوتر

'''''    ' دالة لإنشاء مجلدات التصنيف الرئيسية
''''    Public Function CreateCategoryFolders() As Boolean
''''        Try
''''            ' إنشاء المجلد الرئيسي إذا لم يكن موجودًا
''''            If Not Directory.Exists(ARCHIVE_BASE_PATH) Then
''''                Directory.CreateDirectory(ARCHIVE_BASE_PATH)
''''            End If

''''            ' إنشاء مجلدات التصنيفات
''''            Dim incomingPath As String = Path.Combine(ARCHIVE_BASE_PATH, "الوارد")
''''            Dim outgoingPath As String = Path.Combine(ARCHIVE_BASE_PATH, "الصادر")
''''            Dim memosPath As String = Path.Combine(ARCHIVE_BASE_PATH, "المذكرات")

''''            If Not Directory.Exists(incomingPath) Then
''''                Directory.CreateDirectory(incomingPath)
''''            End If

''''            If Not Directory.Exists(outgoingPath) Then
''''                Directory.CreateDirectory(outgoingPath)
''''            End If

''''            If Not Directory.Exists(memosPath) Then
''''                Directory.CreateDirectory(memosPath)
''''            End If

''''            Return True
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في إنشاء مجلدات التصنيف: " & ex.Message)
''''        End Try
''''    End Function

''''    Public Function GetAllArchive() As DataTable
''''        Try
''''            Dim query As String = "SELECT * FROM TbArchive ORDER BY ArchiveDate"
''''            Return DatabaseHelper.ExecuteQuery(query)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على الأرشيف: " & ex.Message)
''''        End Try
''''    End Function


''''    ' دالة للحصول على اتصال قاعدة البيانات
''''    Private Function GetConnection() As SqlConnection
''''        Return DatabaseHelper.GetConnection()
''''    End Function

''''    ' دالة للتحقق من وجود الأرشيف
''''    Public Function ArchiveExists(archiveId As Integer) As Boolean
''''        Try
''''            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE ID = @ID"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@ID", archiveId)
''''            }
''''            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
''''            Return count > 0
''''        Catch ex As Exception
''''            System.Diagnostics.Debug.WriteLine($"خطأ في ArchiveExists: {ex.Message}")
''''            Return False
''''        End Try
''''    End Function

''''    ' دالة للتحقق من تكرار رقم الأرشيف
''''    Public Function CheckDuplicateArchiveNumber(archiveNum As String) As Boolean
''''        Try
''''            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE ArchiveNum = @ArchiveNum"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum)
''''            }

''''            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
''''            Return count > 0
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في التحقق من تكرار رقم الأرشيف: " & ex.Message)
''''        End Try
''''    End Function

''''    ' دالة للتحقق من تكرار رقم الأرشيف عند التحديث (استثناء السجل الحالي)
''''    Public Function CheckDuplicateArchiveNumberForUpdate(archiveNum As String, currentId As Integer) As Boolean
''''        Try
''''            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE ArchiveNum = @ArchiveNum AND ID <> @CurrentId"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum),
''''                DatabaseHelper.CreateParameter("@CurrentId", currentId)
''''            }

''''            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
''''            Return count > 0
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في التحقق من تكرار رقم الأرشيف عند التحديث: " & ex.Message)
''''        End Try
''''    End Function

''''    ' دالة للحصول على معلومات الأرشيف المكرر بناءً على رقم الأرشيف
''''    Public Function GetDuplicateArchiveInfo(archiveNum As String) As DataTable
''''        Try
''''            Dim query As String = "SELECT ArchiveNum, ArchiveSubject, ArchiveDate, ArchiveFolder, UserName FROM TbArchive WHERE ArchiveNum = @ArchiveNum"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum)
''''            }

''''            Return DatabaseHelper.ExecuteQuery(query, parameters)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على معلومات الأرشيف المكرر: " & ex.Message)
''''        End Try
''''    End Function

''''    ' دالة للحصول على سجل الأرشيف بواسطة المعرف
''''    'Public Function GetArchiveById(id As Integer) As DataTable
''''    '    Try
''''    '        Dim query As String = "SELECT * FROM TbArchive WHERE ID = @ID"
''''    '        Dim parameters() As SqlParameter = {
''''    '            DatabaseHelper.CreateParameter("@ID", id)
''''    '        }
''''    '        Return DatabaseHelper.ExecuteQuery(query, parameters)
''''    '    Catch ex As Exception
''''    '        System.Diagnostics.Debug.WriteLine($"خطأ في GetArchiveById: {ex.Message}")
''''    '        Return Nothing
''''    '    End Try
''''    'End Function

''''    ''' <summary>
''''    ''' إضافة أرشيف جديد
''''    ''' </summary>

''''    Public Function AddArchive(archiveNum As String, archiveDate As Date, archiveSubject As String, archiveFolder As String, archiveNotes As String, fileName As String, fileType As String, filePath As String, archiveWillMaster As String, archiveWillSub As String, userName As String) As Integer
''''        Try

''''            ' التحقق من صحة البيانات
''''            If String.IsNullOrWhiteSpace(archiveNum) OrElse String.IsNullOrWhiteSpace(userName) Then
''''                Return 0
''''            End If

''''            ' التحقق من تكرار رقم الأرشيف
''''            If CheckDuplicateArchiveNumber(archiveNum) Then
''''                Return -1 ' رمز خاص لرقم أرشيف مكرر
''''            End If

''''            ' تحليل مسار الأرشيف وإنشاء المجلدات المناسبة
''''            Dim finalArchivePath As String = ""
''''            ' في دالة AddArchive
''''            If archiveFolder.Contains("\") Then
''''                ' إذا كان المسار يحتوي على تصنيف (مثل: صادر\2024-12)
''''                Dim parts() As String = archiveFolder.Split("\"c)
''''                If parts.Length >= 2 Then
''''                    Dim category As String = parts(0)
''''                    Dim subFolder As String = parts(1)
''''                    finalArchivePath = CreateSubFolder(category, subFolder)
''''                Else
''''                    ' مجرد مجلد فرعي
''''                    finalArchivePath = Path.Combine(ARCHIVE_BASE_PATH, archiveFolder)
''''                    If Not Directory.Exists(finalArchivePath) Then
''''                        Directory.CreateDirectory(finalArchivePath)
''''                    End If
''''                End If
''''            Else
''''                ' مجرد اسم مجلد بسيط
''''                finalArchivePath = Path.Combine(ARCHIVE_BASE_PATH, archiveFolder)
''''                If Not Directory.Exists(finalArchivePath) Then
''''                    Directory.CreateDirectory(finalArchivePath)
''''                End If
''''            End If


''''            'If archiveFolder.Contains("\") Then
''''            '    Dim parts() As String = archiveFolder.Split("\"c)
''''            '    If parts.Length >= 2 Then
''''            '        Dim category As String = parts(0)
''''            '        Dim subFolder As String = parts(1)
''''            '        finalArchivePath = CreateSubFolder(category, subFolder)
''''            '    Else
''''            '        finalArchivePath = Path.Combine(ARCHIVE_BASE_PATH, archiveFolder)
''''            '        If Not Directory.Exists(finalArchivePath) Then
''''            '            Directory.CreateDirectory(finalArchivePath)
''''            '        End If
''''            '    End If
''''            'Else
''''            '    finalArchivePath = Path.Combine(ARCHIVE_BASE_PATH, archiveFolder)
''''            '    If Not Directory.Exists(finalArchivePath) Then
''''            '        Directory.CreateDirectory(finalArchivePath)
''''            '    End If
''''            'End If

''''            ' التحقق من أن اسم الملف يحتوي على الامتداد الصحيح
''''            Dim fileExtension As String = Path.GetExtension(filePath)
''''            If Not String.IsNullOrEmpty(fileExtension) AndAlso Not fileName.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) Then
''''                fileName = Path.GetFileNameWithoutExtension(fileName) & fileExtension
''''            End If

''''            ' تحديد المسار النهائي للملف
''''            Dim destinationPath As String = Path.Combine(finalArchivePath, fileName)

''''            ' التحقق من وجود الملف المصدر
''''            If File.Exists(filePath) Then
''''                ' نسخ الملف إلى المجلد المحدد
''''                File.Copy(filePath, destinationPath, True)
''''            End If

''''            ' إدراج السجل في قاعدة البيانات
''''            Dim query As String = "INSERT INTO TbArchive (ArchiveNum, ArchiveDate, ArchiveSubject, ArchiveNotes, ArchiveFolder, FileName, FileType, FilePath, OriginalFilePath, ArchiveWillMaster, ArchiveWillSub, UserName) VALUES (@ArchiveNum, @ArchiveDate, @ArchiveSubject, @ArchiveNotes, @ArchiveFolder, @FileName, @FileType, @FilePath, @OriginalFilePath, @ArchiveWillMaster, @ArchiveWillSub, @UserName); SELECT SCOPE_IDENTITY();"

''''            Using connection As SqlConnection = GetConnection()
''''                connection.Open()
''''                Using command As New SqlCommand(query, connection)
''''                    command.Parameters.AddWithValue("@ArchiveNum", archiveNum)
''''                    command.Parameters.AddWithValue("@ArchiveDate", archiveDate)
''''                    command.Parameters.AddWithValue("@ArchiveSubject", archiveSubject)
''''                    command.Parameters.AddWithValue("@ArchiveFolder", archiveFolder)
''''                    command.Parameters.AddWithValue("@ArchiveNotes", archiveNotes)
''''                    command.Parameters.AddWithValue("@FileName", fileName)
''''                    command.Parameters.AddWithValue("@FileType", fileType)
''''                    command.Parameters.AddWithValue("@FilePath", destinationPath)
''''                    command.Parameters.AddWithValue("@OriginalFilePath", filePath) ' حفظ المسار الأصلي
''''                    command.Parameters.AddWithValue("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster))
''''                    command.Parameters.AddWithValue("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub))
''''                    command.Parameters.AddWithValue("@UserName", userName)

''''                    Dim result As Object = command.ExecuteScalar()
''''                    ' التحقق من صحة النتيجة
''''                    If result IsNot Nothing AndAlso IsNumeric(result) Then
''''                        Return Convert.ToInt32(result)
''''                        ' تحديث عداد الأرشيف
''''                        DataHelper.UpdateArchiveCount()
''''                    Else
''''                        Return 0
''''                    End If
''''                End Using
''''            End Using
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في إضافة الأرشيف: " & ex.Message)
''''        End Try
''''    End Function

''''#Region "إدارة المجلدات"
''''    ''' <summary>
''''    ''' إنشاء مجلد فرعي بناءً على التصنيف والمجلد الفرعي
''''    ''' </summary>
''''    ''' <param name="category">التصنيف (مثل: صادر، وارد)</param>
''''    ''' <param name="subFolder">اسم المجلد الفرعي (مثل: 2024-12)</param>
''''    ''' <returns>المسار الكامل للمجلد الفرعي</returns>
''''    Public Function CreateSubFolder(category As String, subFolder As String) As String
''''        Try
''''            ' إنشاء مسار المجلد الرئيسي
''''            Dim categoryPath As String = Path.Combine(ARCHIVE_BASE_PATH, category)

''''            ' إنشاء مسار المجلد الفرعي
''''            Dim subFolderPath As String = Path.Combine(categoryPath, subFolder)

''''            ' إنشاء المجلدات إذا لم تكن موجودة
''''            If Not Directory.Exists(categoryPath) Then
''''                Directory.CreateDirectory(categoryPath)
''''            End If

''''            If Not Directory.Exists(subFolderPath) Then
''''                Directory.CreateDirectory(subFolderPath)
''''            End If

''''            Return subFolderPath
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في إنشاء المجلد الفرعي: " & ex.Message)
''''        End Try
''''    End Function
''''#End Region


''''    'Public Function AddArchive(archiveNum As String, archiveSubject As String, archiveFolder As String, archiveDate As Date,
''''    '                         archiveWillMaster As String, archiveWillSub As String, archiveNotes As String,
''''    '                         filename As String, fileType As String, filepath As String, userName As String) As Integer
''''    '    Try
''''    '        ' التحقق من صحة البيانات
''''    '        If String.IsNullOrWhiteSpace(archiveNum) OrElse String.IsNullOrWhiteSpace(userName) Then
''''    '            Return 0
''''    '        End If

''''    '        ' التحقق من تكرار رقم الأرشيف
''''    '        If CheckDuplicateArchiveNumber(archiveNum) Then
''''    '            Return -1 ' رمز خاص لرقم أرشيف مكرر
''''    '        End If

''''    '        Dim query As String = "INSERT INTO TbArchive (ArchiveNum, ArchiveSubject, ArchiveFolder, ArchiveDate, " &
''''    '                              "ArchiveWillMaster, ArchiveWillSub, ArchiveNotes, filename, fileType, filepath, UserName) " &
''''    '                              "VALUES (@ArchiveNum, @ArchiveSubject, @ArchiveFolder, @ArchiveDate, " &
''''    '                              "@ArchiveWillMaster, @ArchiveWillSub, @ArchiveNotes, @filename, @fileType, @filepath, @UserName); " &
''''    '                              "SELECT SCOPE_IDENTITY();"

''''    '        Using connection As SqlConnection = GetConnection()
''''    '            connection.Open()

''''    '            Using command As New SqlCommand(query, connection)
''''    '                ' إعداد المعلمات
''''    '                command.Parameters.AddWithValue("@ArchiveNum", archiveNum.Trim())
''''    '                command.Parameters.AddWithValue("@ArchiveSubject", If(String.IsNullOrEmpty(archiveSubject), DBNull.Value, archiveSubject.Trim()))
''''    '                command.Parameters.AddWithValue("@ArchiveFolder", If(String.IsNullOrEmpty(archiveFolder), DBNull.Value, archiveFolder.Trim()))
''''    '                command.Parameters.AddWithValue("@ArchiveDate", archiveDate.Date)
''''    '                command.Parameters.AddWithValue("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster.Trim()))
''''    '                command.Parameters.AddWithValue("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub.Trim()))
''''    '                command.Parameters.AddWithValue("@ArchiveNotes", If(String.IsNullOrEmpty(archiveNotes), DBNull.Value, archiveNotes.Trim()))
''''    '                command.Parameters.AddWithValue("@filename", If(String.IsNullOrEmpty(filename), DBNull.Value, filename.Trim()))
''''    '                command.Parameters.AddWithValue("@fileType", If(String.IsNullOrEmpty(fileType), DBNull.Value, fileType.Trim()))
''''    '                command.Parameters.AddWithValue("@filepath", If(String.IsNullOrEmpty(filepath), DBNull.Value, filepath.Trim()))
''''    '                command.Parameters.AddWithValue("@UserName", userName.Trim())


''''    '                ' تنفيذ الاستعلام والحصول على معرف الأرشيف الجديد
''''    '                Dim result As Object = command.ExecuteScalar()

''''    '                ' التحقق من صحة النتيجة
''''    '                If result IsNot Nothing AndAlso IsNumeric(result) Then
''''    '                    Return Convert.ToInt32(result)
''''    '                    ' تحديث عداد الأرشيف
''''    '                    DataHelper.UpdateArchiveCount()
''''    '                Else
''''    '                    Return 0
''''    '                End If
''''    '            End Using
''''    '        End Using

''''    '    Catch ex As Exception
''''    '        System.Diagnostics.Debug.WriteLine($"خطأ في AddArchive: {ex.Message}")
''''    '        Return 0
''''    '    End Try
''''    'End Function

''''    ''' <summary>
''''    ''' تحديث سجل الأرشيف
''''    ''' </summary>
''''    Public Function UpdateArchive(id As Integer, archiveNum As String, archiveSubject As String, archiveFolder As String, archiveDate As Date,
''''                                 archiveWillMaster As String, archiveWillSub As String, archiveNotes As String,
''''                                 filename As String, fileType As String, filepath As String, userName As String) As Boolean
''''        Try
''''            ' التحقق من صحة البيانات
''''            If id <= 0 OrElse String.IsNullOrWhiteSpace(archiveNum) OrElse String.IsNullOrWhiteSpace(userName) Then
''''                Return False
''''            End If

''''            ' التحقق من تكرار رقم الأرشيف (باستثناء السجل الحالي)
''''            If CheckDuplicateArchiveNumberForUpdate(archiveNum, id) Then
''''                Return False
''''            End If

''''            Dim query As String = "UPDATE TbArchive SET " &
''''                                  "ArchiveNum = @ArchiveNum, " &
''''                                  "ArchiveSubject = @ArchiveSubject, " &
''''                                  "ArchiveFolder = @ArchiveFolder, " &
''''                                  "ArchiveDate = @ArchiveDate, " &
''''                                  "ArchiveWillMaster = @ArchiveWillMaster, " &
''''                                  "ArchiveWillSub = @ArchiveWillSub, " &
''''                                  "ArchiveNotes = @ArchiveNotes, " &
''''                                  "filename = @filename, " &
''''                                  "fileType = @fileType, " &
''''                                  "filepath = @filepath, " &
''''                                  "UserName = @UserName " &
''''                                  "WHERE ID = @ID"

''''            Using connection As SqlConnection = GetConnection()
''''                connection.Open()

''''                Using command As New SqlCommand(query, connection)
''''                    ' إعداد المعلمات
''''                    command.Parameters.AddWithValue("@ID", id)
''''                    command.Parameters.AddWithValue("@ArchiveNum", archiveNum.Trim())
''''                    command.Parameters.AddWithValue("@ArchiveSubject", If(String.IsNullOrEmpty(archiveSubject), DBNull.Value, archiveSubject.Trim()))
''''                    command.Parameters.AddWithValue("@ArchiveFolder", If(String.IsNullOrEmpty(archiveFolder), DBNull.Value, archiveFolder.Trim()))
''''                    command.Parameters.AddWithValue("@ArchiveDate", archiveDate.Date)
''''                    command.Parameters.AddWithValue("@ArchiveWillMaster", If(String.IsNullOrEmpty(archiveWillMaster), DBNull.Value, archiveWillMaster.Trim()))
''''                    command.Parameters.AddWithValue("@ArchiveWillSub", If(String.IsNullOrEmpty(archiveWillSub), DBNull.Value, archiveWillSub.Trim()))
''''                    command.Parameters.AddWithValue("@ArchiveNotes", If(String.IsNullOrEmpty(archiveNotes), DBNull.Value, archiveNotes.Trim()))
''''                    command.Parameters.AddWithValue("@filename", If(String.IsNullOrEmpty(filename), DBNull.Value, filename.Trim()))
''''                    command.Parameters.AddWithValue("@fileType", If(String.IsNullOrEmpty(fileType), DBNull.Value, fileType.Trim()))
''''                    command.Parameters.AddWithValue("@filepath", If(String.IsNullOrEmpty(filepath), DBNull.Value, filepath.Trim()))
''''                    command.Parameters.AddWithValue("@UserName", userName.Trim())

''''                    ' تنفيذ الاستعلام
''''                    Return command.ExecuteNonQuery() > 0
''''                End Using
''''            End Using

''''        Catch ex As Exception
''''            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateArchive: {ex.Message}")
''''            Return False
''''        End Try
''''    End Function

''''    ''' <summary>
''''    ''' ربط الأرشيف بالصادر
''''    ''' </summary>
''''    Public Function UpdateArchiveSaderId(archiveNum As String, saderId As Integer) As Boolean
''''        Try
''''            ' التحقق من صحة البيانات
''''            If String.IsNullOrWhiteSpace(archiveNum) OrElse saderId <= 0 Then
''''                Return False
''''            End If

''''            Dim query As String = "UPDATE TbArchive SET SaderID = @SaderID WHERE ArchiveNum = @ArchiveNum"

''''            Using connection As SqlConnection = GetConnection()
''''                connection.Open()

''''                Using command As New SqlCommand(query, connection)
''''                    ' إعداد المعلمات
''''                    command.Parameters.AddWithValue("@ArchiveNum", archiveNum.Trim())
''''                    command.Parameters.AddWithValue("@SaderID", saderId)

''''                    ' تنفيذ الاستعلام
''''                    Return command.ExecuteNonQuery() > 0
''''                End Using
''''            End Using

''''        Catch ex As Exception
''''            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateArchiveSaderId: {ex.Message}")
''''            Return False
''''        End Try
''''    End Function

''''    Public Function DeleteArchive(id As Integer) As Boolean
''''        Try
''''            ' جلب مسار الملف قبل الحذف من قاعدة البيانات
''''            Dim dt As DataTable = GetArchiveById(id)
''''            Dim filePathToDelete As String = String.Empty
''''            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
''''                filePathToDelete = dt.Rows(0)("filepath").ToString()
''''            End If

''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@ID", id)
''''            }



''''            '' تنفيذ الاستعلام
''''            'Dim rowsAffected As Integer = Command.ExecuteNonQuery()

''''            '' إذا تم الحذف بنجاح، تحديث العدد
''''            'If rowsAffected > 0 Then
''''            '    SaderHelper.UpdateCount()
''''            'End If

''''            'Return rowsAffected > 0

''''            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("DeleteArchive", parameters)

''''            If rowsAffected > 0 Then
''''                ' محاولة حذف الملف من نظام الملفات بعد الحذف من قاعدة البيانات
''''                If File.Exists(filePathToDelete) Then
''''                    Try
''''                        MoveFileToDeletedFolder(filePathToDelete)
''''                        ' تحديث عداد الأرشيف
''''                        DataHelper.UpdateArchiveCount()
''''                        ' محاولة حذف الملف مع إعادة المحاولة
''''                        'DeleteFileWithRetry(filePathToDelete, 3)
''''                    Catch fileEx As Exception
''''                        ' إذا فشل حذف الملف، نقله إلى مجلد المحذوفات
''''                        Try
''''                            MoveFileToDeletedFolder(filePathToDelete)

''''                        Catch moveEx As Exception
''''                            ' إذا فشل النقل أيضًا، اترك الملف واعرض تحذير
''''                            Throw New Exception($"تم حذف السجل من قاعدة البيانات ولكن فشل في حذف الملف: {filePathToDelete}{vbNewLine} السبب: {fileEx.Message}{vbNewLine}يرجى إغلاق أي برامج تستخدم هذا الملف وحذفه يدوياً.")
''''                        End Try
''''                    End Try
''''                End If
''''                Return True
''''            Else
''''                Return False
''''            End If
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في حذف الأرشيف: " & ex.Message)
''''        End Try
''''    End Function
''''    ' دالة لنقل الملف إلى مجلد المحذوفات
''''    Private Sub MoveFileToDeletedFolder(filePath As String)
''''        Try
''''            ' إنشاء مجلد المحذوفات
''''            Dim deletedFolderPath As String = Path.Combine(ARCHIVE_BASE_PATH, "المحذوفات")
''''            If Not Directory.Exists(deletedFolderPath) Then
''''                Directory.CreateDirectory(deletedFolderPath)
''''            End If

''''            ' إنشاء اسم ملف فريد في مجلد المحذوفات
''''            Dim fileName As String = Path.GetFileName(filePath)
''''            Dim fileNameWithoutExt As String = Path.GetFileNameWithoutExtension(filePath)
''''            Dim fileExt As String = Path.GetExtension(filePath)
''''            Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
''''            Dim newFileName As String = $"{fileNameWithoutExt}_deleted_{timestamp}{fileExt}"
''''            Dim newFilePath As String = Path.Combine(deletedFolderPath, newFileName)

''''            ' نقل الملف
''''            File.Move(filePath, newFilePath)
''''        Catch ex As Exception
''''            Throw New Exception($"فشل في نقل الملف إلى مجلد المحذوفات: {ex.Message}")
''''        End Try
''''    End Sub

''''    ''' <summary>
''''    ''' حذف الأرشيف
''''    ''' </summary>
''''    Public Function DeleteArchiveById(archiveId As Integer, Optional showMessages As Boolean = True) As Boolean
''''        Dim transaction As SqlTransaction = Nothing
''''        Try
''''            Using connection As SqlConnection = GetConnection()
''''                connection.Open()
''''                transaction = connection.BeginTransaction()

''''                ' الحصول على معلومات الأرشيف
''''                Dim dt As DataTable = GetArchiveById(archiveId)
''''                Dim currentFilePath As String = ""
''''                Dim originalFilePath As String = ""

''''                If dt.Rows.Count > 0 Then
''''                    currentFilePath = If(dt.Rows(0)("FilePath") IsNot DBNull.Value, dt.Rows(0)("FilePath").ToString(), "")
''''                    originalFilePath = If(dt.Rows(0)("OriginalFilePath") IsNot DBNull.Value, dt.Rows(0)("OriginalFilePath").ToString(), "")
''''                End If

''''                ' حذف السجل من قاعدة البيانات
''''                Dim deleteQuery As String = "DELETE FROM TbArchive WHERE ID = @ID"
''''                Using command As New SqlCommand(deleteQuery, connection, transaction)
''''                    command.Parameters.AddWithValue("@ID", archiveId)
''''                    Dim result As Boolean = command.ExecuteNonQuery() > 0

''''                    ' حذف الملف من المسار الحالي فقط (وليس من المسار الأصلي)
''''                    If result AndAlso Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath) Then
''''                        Try
''''                            File.Delete(currentFilePath)
''''                            If showMessages Then
''''                                MessageBox.Show("تم حذف الأرشيف والملف المرتبط بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''                            End If
''''                        Catch
''''                            ' في حالة فشل حذف الملف، نقله إلى مجلد المحذوفات
''''                            MoveToDeletedFolder(currentFilePath)
''''                            If showMessages Then
''''                                MessageBox.Show("تم حذف الأرشيف من قاعدة البيانات ونقل الملف إلى مجلد المحذوفات", "نجح جزئي", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''''                            End If
''''                        End Try
''''                    ElseIf result Then
''''                        If showMessages Then
''''                            MessageBox.Show("تم حذف الأرشيف بنجاح (لا يوجد مسار ملف مرتبط)", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''                        End If
''''                    End If

''''                    ' تأكيد المعاملة
''''                    transaction.Commit()
''''                    Return result
''''                End Using
''''            End Using
''''        Catch ex As Exception
''''            ' التراجع عن المعاملة في حالة حدوث خطأ
''''            If transaction IsNot Nothing Then
''''                transaction.Rollback()
''''            End If

''''            If showMessages Then
''''                MessageBox.Show($"حدث خطأ أثناء حذف الأرشيف بمعرف {archiveId}: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
''''            End If
''''            Return False
''''        End Try
''''    End Function

''''    ' دالة للحصول على بيانات الأرشيف بالمعرف
''''    Public Function GetArchiveById(archiveId As Integer) As DataTable
''''        Try
''''            Dim query As String = "SELECT * FROM TbArchive WHERE ID = @ID"

''''            Using connection As SqlConnection = GetConnection()
''''                Using command As New SqlCommand(query, connection)
''''                    command.Parameters.AddWithValue("@ID", archiveId)

''''                    Using adapter As New SqlDataAdapter(command)
''''                        Dim dt As New DataTable()
''''                        adapter.Fill(dt)
''''                        Return dt
''''                    End Using
''''                End Using
''''            End Using
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على الأرشيف بالمعرف: " & ex.Message)
''''        End Try
''''    End Function

''''    ''' <summary>
''''    ''' نقل الملف إلى مجلد المحذوفات عند فشل حذفه
''''    ''' </summary>
''''    ''' <param name="filePath">مسار الملف الذي فشل حذفه</param>
''''    Private Sub MoveToDeletedFolder(filePath As String)
''''        Try
''''            ' تحديد مسار مجلد المحذوفات
''''            Dim deletedFolderPath As String = Path.Combine(My.Settings.ARCHIVE_Default_PATH, "Deleted")

''''            ' إنشاء مجلد المحذوفات إذا لم يكن موجودًا
''''            If Not Directory.Exists(deletedFolderPath) Then
''''                Directory.CreateDirectory(deletedFolderPath)
''''            End If

''''            ' استخراج اسم الملف والامتداد
''''            Dim fileName As String = Path.GetFileName(filePath)
''''            Dim extension As String = Path.GetExtension(filePath)
''''            Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(filePath)

''''            ' إنشاء اسم ملف فريد باستخدام التاريخ والوقت
''''            Dim newFileName As String = $"{fileNameWithoutExtension}_{DateTime.Now:yyyyMMddHHmmss}{extension}"
''''            Dim newFilePath As String = Path.Combine(deletedFolderPath, newFileName)

''''            ' نقل الملف إلى مجلد المحذوفات
''''            File.Move(filePath, newFilePath)

''''            System.Diagnostics.Debug.WriteLine($"تم نقل الملف إلى مجلد المحذوفات: {newFilePath}")
''''        Catch ex As Exception
''''            ' تسجيل الخطأ إذا فشل النقل أيضًا
''''            System.Diagnostics.Debug.WriteLine($"فشل في نقل الملف إلى مجلد المحذوفات: {ex.Message}")

''''            ' محاولة حذف الملف مرة أخرى كحل أخير
''''            Try
''''                If File.Exists(filePath) Then
''''                    File.Delete(filePath)
''''                End If
''''            Catch
''''                ' إذا فشل كل شيء، نسجل الخطأ
''''                System.Diagnostics.Debug.WriteLine("فشل في حذف الملف نهائياً")
''''            End Try
''''        End Try
''''    End Sub

''''    'Public Function DeleteArchiveByArchiveId(archiveId As Integer, Optional showMessages As Boolean = False) As Boolean
''''    '    Try
''''    '        ' التحقق من وجود الأرشيف أولاً
''''    '        If Not ArchiveExists(archiveId) Then
''''    '            If showMessages Then
''''    '                MessageBox.Show($"الأرشيف بمعرف {archiveId} غير موجود أو تم حذفه مسبقًا", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '            End If
''''    '            Return True ' اعتبر العملية ناجحة إذا كان الأرشيف غير موجود أصلاً
''''    '        End If

''''    '        ' جلب مسار الملف قبل الحذف من قاعدة البيانات
''''    '        Dim dt As DataTable = GetArchiveById(archiveId)
''''    '        Dim filePathToDelete As String = String.Empty
''''    '        Dim archiveNum As String = String.Empty

''''    '        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
''''    '            ' تصحيح المسار باستخدام الدالة الجديدة
''''    '            filePathToDelete = NormalizeFilePath(dt.Rows(0)("filepath"))
''''    '            archiveNum = If(dt.Rows(0)("ArchiveNum") IsNot Nothing, dt.Rows(0)("ArchiveNum").ToString(), $"معرف {archiveId}")
''''    '        End If

''''    '        ' حذف السجل من قاعدة البيانات أولاً
''''    '        Dim parameters() As SqlParameter = {
''''    '            DatabaseHelper.CreateParameter("@ID", archiveId)
''''    '        }

''''    '        Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("DeleteArchive", parameters)

''''    '        If rowsAffected > 0 Then
''''    '            ' محاولة حذف الملف من نظام الملفات بعد الحذف من قاعدة البيانات
''''    '            If Not String.IsNullOrEmpty(filePathToDelete) Then
''''    '                Try
''''    '                    ' التحقق من وجود الملف أولاً
''''    '                    If File.Exists(filePathToDelete) Then
''''    '                        ' محاولة حذف الملف مع إعادة المحاولة
''''    '                        If DeleteFileWithRetry(filePathToDelete, 3) Then
''''    '                            If showMessages Then
''''    '                                MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} والملف المرتبط به بنجاح", "حذف الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '                            End If
''''    '                        Else
''''    '                            ' إذا فشل الحذف، حاول معرفة السبب
''''    '                            Dim reason As String = GetFileDeleteFailureReason(filePathToDelete)
''''    '                            If showMessages Then
''''    '                                MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات ولكن فشل في حذف الملف: {filePathToDelete}{vbNewLine}السبب: {reason}{vbNewLine}يرجى إغلاق أي برامج تستخدم هذا الملف وحذفه يدوياً.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''''    '                            End If
''''    '                        End If
''''    '                    Else
''''    '                        ' الملف غير موجود أصلاً
''''    '                        If showMessages Then
''''    '                            MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات (الملف غير موجود في المسار)", "حذف الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '                        End If
''''    '                    End If
''''    '                Catch ex As Exception
''''    '                    ' معالجة الأخطاء غير المتوقعة
''''    '                    Dim errorMessage As String = $"فشل في معالجة الملف: {ex.Message}"
''''    '                    If showMessages Then
''''    '                        MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات ولكن حدث خطأ في معالجة الملف:{vbNewLine}{errorMessage}", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''''    '                    End If
''''    '                End Try
''''    '            Else
''''    '                If showMessages Then
''''    '                    MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات (لا يوجد مسار ملف مرتبط)", "حذف الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '                End If
''''    '            End If
''''    '            ' تحديث عداد الأرشيف
''''    '            DataHelper.UpdateArchiveCount()

''''    '            Return True
''''    '        Else
''''    '            If showMessages Then
''''    '                MessageBox.Show($"لم يتم العثور على الأرشيف بمعرف {archiveId} في قاعدة البيانات", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '            End If
''''    '            Return True
''''    '        End If
''''    '    Catch ex As Exception
''''    '        If showMessages Then
''''    '            MessageBox.Show($"حدث خطأ أثناء حذف الأرشيف بمعرف {archiveId}: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
''''    '        End If
''''    '        Return False
''''    '    End Try
''''    'End Function

''''    ''' <summary>
''''    ''' تصحيح مسار الملف (إزالة المسافات الزائدة، تحويل الشرطات)
''''    ''' </summary>
''''    Private Function NormalizeFilePath(filePath As Object) As String
''''        If filePath Is Nothing OrElse filePath Is DBNull.Value Then
''''            Return String.Empty
''''        End If

''''        Dim path As String = filePath.ToString().Trim()

''''        ' استبدال الشرطات المائلة العكسية بشرطات مائلة إذا لزم
''''        path = path.Replace("/", "\")

''''        ' إزالة الشرطات الزائدة في النهاية
''''        While path.EndsWith("\")
''''            path = path.Substring(0, path.Length - 1)
''''        End While

''''        Return path
''''    End Function

''''    ''' <summary>
''''    ''' محاولة حذف الملف مع إعادة المحاولة
''''    ''' </summary>
''''    Private Function DeleteFileWithRetry(filePath As String, maxRetries As Integer) As Boolean
''''        Dim retryCount As Integer = 0

''''        While retryCount < maxRetries
''''            Try
''''                ' التحقق من وجود الملف
''''                If Not File.Exists(filePath) Then
''''                    Return True ' اعتبره محذوفًا إذا كان غير موجود
''''                End If

''''                ' إزالة خاصية "للقراءة فقط" إذا وجدت
''''                Dim fileInfo As New IO.FileInfo(filePath)
''''                fileInfo.IsReadOnly = False

''''                ' محاولة الحذف
''''                File.Delete(filePath)

''''                ' التحقق من أن الملف تم حذفه
''''                If Not File.Exists(filePath) Then
''''                    Return True
''''                End If

''''            Catch ex As Exception
''''                ' الانتظار قبل إعادة المحاولة
''''                Threading.Thread.Sleep(500)
''''                retryCount += 1
''''            End Try
''''        End While

''''        Return False
''''    End Function

''''    ''' <summary>
''''    ''' تحديد سبب فشل حذف الملف
''''    ''' </summary>
''''    Private Function GetFileDeleteFailureReason(filePath As String) As String
''''        Try
''''            ' التحقق من وجود الملف
''''            If Not File.Exists(filePath) Then
''''                Return "الملف غير موجود"
''''            End If

''''            ' محاولة فتح الملف للقراءة (للتحقق مما إذا كان قيد الاستخدام)
''''            Using fs As New IO.FileStream(filePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.None)
''''                ' إذا نجحت هذه السطر، فالمشكلة ليست في الاستخدام
''''            End Using

''''            ' التحقق من الصلاحيات
''''            Dim permissionTestPath As String = Path.Combine(Path.GetDirectoryName(filePath), "permission_test.tmp")
''''            Try
''''                File.WriteAllText(permissionTestPath, "test")
''''                File.Delete(permissionTestPath)
''''            Catch
''''                Return "مشكلة في الصلاحيات - لا يمكن الكتابة في هذا المجلد"
''''            End Try

''''            ' إذا لم تكن أي من الأسباب السابقة، فربما مشكلة أخرى
''''            Return "سبب غير معروف (قد يكون الملف قيد الاستخدام من قبل عملية نظام)"

''''        Catch ex As UnauthorizedAccessException
''''            Return "صلاحيات غير كافية لحذف الملف"
''''        Catch ex As IOException When ex.Message.Contains("process cannot access the file")
''''            Return "الملف قيد الاستخدام من قبل برنامج آخر"
''''        Catch ex As Exception
''''            Return $"خطأ غير متوقع: {ex.Message}"
''''        End Try
''''    End Function


'''''    Public Function GetDistinctFileTypes() As DataTable
'''''        Try
'''''            Dim query As String = "SELECT DISTINCT FileType FROM TbArchive WHERE FileType IS NOT NULL AND FileType <> '' ORDER BY FileType"

''''            Using connection As SqlConnection = GetConnection()
''''                Using adapter As New SqlDataAdapter(query, connection)
''''                    Dim dt As New DataTable()
''''                    adapter.Fill(dt)
''''                    Return dt
''''                End Using
''''            End Using

''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على أنواع الملفات: " & ex.Message)
''''        End Try
''''    End Function

''''    ' دالة للتحقق من وجود ملف مكرر في نفس المسار
''''    Public Function CheckDuplicateFile(filename As String, fileType As String, archiveFolder As String) As Boolean
''''        Try
''''            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE filename = @Filename AND fileType = @FileType AND ArchiveFolder = @ArchiveFolder"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@Filename", filename),
''''                DatabaseHelper.CreateParameter("@FileType", fileType),
''''                DatabaseHelper.CreateParameter("@ArchiveFolder", archiveFolder)
''''            }

''''            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
''''            Return count > 0
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في التحقق من تكرار الملف: " & ex.Message)
''''        End Try
''''    End Function

''''    'دالة للحصول على معلومات الملف المكرر
''''    Public Function GetDuplicateFileInfo(filename As String, fileType As String, archiveFolder As String) As DataTable
''''        Try
''''            Dim query As String = "SELECT ArchiveNum, ArchiveSubject, ArchiveDate, UserName FROM TbArchive WHERE filename = @Filename AND fileType = @FileType AND ArchiveFolder = @ArchiveFolder"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@Filename", filename),
''''                DatabaseHelper.CreateParameter("@FileType", fileType),
''''                DatabaseHelper.CreateParameter("@ArchiveFolder", archiveFolder)
''''            }

''''            Return DatabaseHelper.ExecuteQuery(query, parameters)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على معلومات الملف المكرر: " & ex.Message)
''''        End Try
''''    End Function

''''    Public Function GetArchiveByFileType(fileType As String) As DataTable
''''        Try
''''            Dim query As String = "SELECT * FROM TbArchive WHERE fileType = @FileType ORDER BY ArchiveDate"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@FileType", fileType)
''''            }

''''            Return DatabaseHelper.ExecuteQuery(query, parameters)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على الأرشيف حسب نوع الملف: " & ex.Message)
''''        End Try
''''    End Function


''''    Public Sub OpenFileLocation(filePath As String)
''''        If String.IsNullOrWhiteSpace(filePath) Then
''''            Throw New ArgumentException("مسار الملف غير صالح")
''''            Return
''''        End If

''''        ' التحقق من وجود الملف فقط (بدون التحقق من المجلد)
''''        If File.Exists(filePath) Then
''''            Process.Start("explorer.exe", $"/select, ""{filePath}""")
''''        Else
''''            ' إذا لم يوجد الملف، نفتح المجلد فقط
''''            Dim directory = Path.GetDirectoryName(filePath)
''''            If Not String.IsNullOrEmpty(directory) Then
''''                Process.Start("explorer.exe", $"""{directory}""")
''''            Else
''''                Throw New DirectoryNotFoundException("المجلد غير موجود")
''''            End If
''''        End If
''''    End Sub

''''    Public Function SearchSaderByDateRange(startDate As Date, endDate As Date) As DataTable
''''        Try
''''            ' التأكد من صحة التواريخ
''''            If startDate > endDate Then
''''                Dim temp As Date = startDate
''''                startDate = endDate
''''                endDate = temp
''''            End If

''''            Dim query As String = "SELECT ID, ArchiveNum, ArchiveDate, ArchiveSubject, ArchiveWillMaster, ArchiveWillSub, UserName FROM TbArchive WHERE ArchiveDate BETWEEN @startDate AND @endDate ORDER BY ArchiveDate DESC"

''''            Using connection As SqlConnection = GetConnection()
''''                connection.Open()

''''                Using command As New SqlCommand(query, connection)
''''                    command.Parameters.AddWithValue("@startDate", startDate.Date)
''''                    command.Parameters.AddWithValue("@endDate", endDate.Date.AddDays(1).AddSeconds(-1))

''''                    Using adapter As New SqlDataAdapter(command)
''''                        Dim dataTable As New DataTable()
''''                        adapter.Fill(dataTable)

''''                        Return ValidateAndCleanSaderData(dataTable)
''''                    End Using
''''                End Using
''''            End Using

''''        Catch ex As Exception
''''            System.Diagnostics.Debug.WriteLine($"خطأ في SearchSaderByDateRange: {ex.Message}")
''''            Return CreateEmptySaderTable()
''''        End Try
''''    End Function

''''    ''' <summary>
''''    ''' التحقق من صحة وتنظيف بيانات الصادر
''''    ''' </summary>
''''    ''' <param name="dataTable">جدول البيانات الأصلي</param>
''''    ''' <returns>جدول بيانات منظف</returns>
''''    Private Function ValidateAndCleanSaderData(dataTable As DataTable) As DataTable
''''        Try
''''            If dataTable Is Nothing OrElse dataTable.Rows.Count = 0 Then
''''                Return CreateEmptySaderTable()
''''            End If

''''            ' إنشاء جدول جديد منظف
''''            Dim cleanTable As DataTable = CreateEmptySaderTable()

''''            ' نسخ الصفوف مع التحقق من صحتها
''''            For Each row As DataRow In dataTable.Rows
''''                Try
''''                    If row.RowState <> DataRowState.Deleted Then
''''                        Dim newRow As DataRow = cleanTable.NewRow()

''''                        ' نسخ البيانات مع معالجة القيم الفارغة
''''                        newRow("ID") = If(IsDBNull(row("ID")), 0, Convert.ToInt32(row("ID")))
''''                        newRow("ArchiveNum") = If(IsDBNull(row("ArchiveNum")), "", row("ArchiveNum").ToString().Trim())
''''                        newRow("ArchiveDate") = If(IsDBNull(row("ArchiveDate")), DateTime.Now, Convert.ToDateTime(row("ArchiveDate")))
''''                        newRow("ArchiveSubject") = If(IsDBNull(row("ArchiveSubject")), "", row("ArchiveSubject").ToString().Trim())
''''                        newRow("ArchiveFolder") = If(IsDBNull(row("ArchiveFolder")), "", row("ArchiveFolder").ToString().Trim())
''''                        newRow("ArchiveWillMaster") = If(IsDBNull(row("ArchiveWillMaster")), "", row("ArchiveWillMaster").ToString().Trim())
''''                        newRow("ArchiveWillSub") = If(IsDBNull(row("ArchiveWillSub")), "", row("ArchiveWillSub").ToString().Trim())
''''                        newRow("ArchiveNotes") = If(IsDBNull(row("ArchiveNotes")), "", row("ArchiveNotes").ToString().Trim())
''''                        newRow("filename") = If(IsDBNull(row("filename")), "", row("filename").ToString().Trim())
''''                        newRow("fileType") = If(IsDBNull(row("fileType")), "", row("fileType").ToString().Trim())
''''                        newRow("filepath") = If(IsDBNull(row("filepath")), "", row("filepath").ToString().Trim())
''''                        newRow("UserName") = If(IsDBNull(row("UserName")), "", row("UserName").ToString().Trim())


''''                        cleanTable.Rows.Add(newRow)
''''                    End If
''''                Catch ex As Exception
''''                    System.Diagnostics.Debug.WriteLine($"خطأ في معالجة صف البيانات: {ex.Message}")
''''                    Continue For
''''                End Try
''''            Next

''''            Return cleanTable

''''        Catch ex As Exception
''''            System.Diagnostics.Debug.WriteLine($"خطأ في ValidateAndCleanSaderData: {ex.Message}")
''''            Return CreateEmptySaderTable()
''''        End Try
''''    End Function

''''    ''' <summary>
''''    ''' إنشاء جدول صادر فارغ بالهيكل الصحيح
''''    ''' </summary>
''''    ''' <returns>جدول بيانات فارغ</returns>
''''    Private Function CreateEmptySaderTable() As DataTable
''''        Try
''''            Dim emptyTable As New DataTable()

''''            emptyTable.Columns.Add("ID", GetType(Integer))
''''            emptyTable.Columns.Add("ArchiveNum", GetType(String))
''''            emptyTable.Columns.Add("ArchiveDate", GetType(DateTime))
''''            emptyTable.Columns.Add("ArchiveSubject", GetType(String))
''''            emptyTable.Columns.Add("ArchiveFolder", GetType(String))
''''            emptyTable.Columns.Add("ArchiveWillMaster", GetType(String))
''''            emptyTable.Columns.Add("ArchiveWillSub", GetType(String))
''''            emptyTable.Columns.Add("ArchiveNotes", GetType(String))
''''            emptyTable.Columns.Add("UserName", GetType(String))


''''            ' تعيين القيم الافتراضية
''''            emptyTable.Columns("ID").DefaultValue = 0
''''            emptyTable.Columns("ArchiveNum").DefaultValue = ""
''''            emptyTable.Columns("ArchiveDate").DefaultValue = DateTime.Now
''''            emptyTable.Columns("ArchiveSubject").DefaultValue = ""
''''            emptyTable.Columns("ArchiveFolder").DefaultValue = ""
''''            emptyTable.Columns("ArchiveWillMaster").DefaultValue = ""
''''            emptyTable.Columns("ArchiveWillSub").DefaultValue = ""
''''            emptyTable.Columns("ArchiveNotes").DefaultValue = ""
''''            emptyTable.Columns("UserName").DefaultValue = ""


''''            Return emptyTable

''''        Catch ex As Exception
''''            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء جدول فارغ: {ex.Message}")
''''            Return New DataTable()
''''        End Try
''''    End Function

''''    Public Function SearchArchive(searchTerm As String) As DataTable
''''        Try
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@SearchTerm", searchTerm)
''''            }

''''            Return DatabaseHelper.ExecuteStoredProcedure("SearchArchive", parameters)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في البحث عن الأرشيف: " & ex.Message)
''''        End Try
''''    End Function
''''End Class





''''Imports System.Data.SqlClient
''''Imports System.IO

''''Public Class ArchiveClass
''''    Private ReadOnly ARCHIVE_BASE_PATH As String = My.Settings.ARCHIVE_Default_PATH ' مسار الأرشيف الأساسي على جهاز الكمبيوتر

''''    ' تصنيفات الأرشيف
''''    Public Enum ArchiveCategory
''''        Incoming = 1    ' وارد
''''        Outgoing = 2    ' صادر
''''        Memos = 3       ' مذكرات
''''    End Enum

''''    ' دالة لإنشاء مجلدات التصنيف الرئيسية
''''    Public Function CreateCategoryFolders() As Boolean
''''        Try
''''            ' إنشاء المجلد الرئيسي إذا لم يكن موجودًا
''''            If Not Directory.Exists(ARCHIVE_BASE_PATH) Then
''''                Directory.CreateDirectory(ARCHIVE_BASE_PATH)
''''            End If

''''            ' إنشاء مجلدات التصنيفات
''''            Dim incomingPath As String = Path.Combine(ARCHIVE_BASE_PATH, "الوارد")
''''            Dim outgoingPath As String = Path.Combine(ARCHIVE_BASE_PATH, "الصادر")
''''            Dim memosPath As String = Path.Combine(ARCHIVE_BASE_PATH, "المذكرات")

''''            If Not Directory.Exists(incomingPath) Then
''''                Directory.CreateDirectory(incomingPath)
''''            End If

''''            If Not Directory.Exists(outgoingPath) Then
''''                Directory.CreateDirectory(outgoingPath)
''''            End If

''''            If Not Directory.Exists(memosPath) Then
''''                Directory.CreateDirectory(memosPath)
''''            End If

''''            Return True
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في إنشاء مجلدات التصنيف: " & ex.Message)
''''        End Try
''''    End Function

''''    ' دالة للحصول على مسار المجلد بناءً على التصنيف
''''    Public Function GetCategoryPath(category As String) As String
''''        Try
''''            ' إنشاء مجلدات التصنيف إذا لم تكن موجودة
''''            CreateCategoryFolders()

''''            Select Case category.ToLower().Trim()
''''                Case "وارد", "الوارد", "incoming"
''''                    Return Path.Combine(ARCHIVE_BASE_PATH, "الوارد")
''''                Case "صادر", "الصادر", "outgoing"
''''                    Return Path.Combine(ARCHIVE_BASE_PATH, "الصادر")
''''                Case "مذكرات", "المذكرات", "مذكرة", "memos"
''''                    Return Path.Combine(ARCHIVE_BASE_PATH, "المذكرات")
''''                Case Else
''''                    ' إذا لم يكن التصنيف معروفًا، استخدم مجلد "متنوع"
''''                    Dim miscPath As String = Path.Combine(ARCHIVE_BASE_PATH, "متنوع")
''''                    If Not Directory.Exists(miscPath) Then
''''                        Directory.CreateDirectory(miscPath)
''''                    End If
''''                    Return miscPath
''''            End Select
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على مسار التصنيف: " & ex.Message)
''''        End Try
''''    End Function

''''    ' دالة لإنشاء مجلد فرعي داخل تصنيف معين
''''    Public Function CreateSubFolder(category As String, subFolderName As String) As String
''''        Try
''''            Dim categoryPath As String = GetCategoryPath(category)
''''            Dim subFolderPath As String = Path.Combine(categoryPath, subFolderName)

''''            If Not Directory.Exists(subFolderPath) Then
''''                Directory.CreateDirectory(subFolderPath)
''''            End If

''''            Return subFolderPath
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في إنشاء المجلد الفرعي: " & ex.Message)
''''        End Try
''''    End Function

''''    Public Function AddArchive(archiveNum As String, archiveSubject As String, archiveFolder As String, archiveDate As Date, archiveWillMaster As String,
''''                               archiveWillSub As String, archiveNotes As String, filename As String, fileType As String, originalFilepath As String, userName As String) As Integer
''''        Try
''''            ' تحديد التصنيف من archiveFolder (أول جزء قبل أي فاصل)
''''            Dim category As String = ""
''''            Dim subFolder As String = ""

''''            ' تحليل archiveFolder لاستخراج التصنيف والمجلد الفرعي
''''            If archiveFolder.Contains("\") Then
''''                Dim parts() As String = archiveFolder.Split("\"c)
''''                category = parts(0)
''''                subFolder = String.Join("\", parts.Skip(1))
''''            ElseIf archiveFolder.Contains("/") Then
''''                Dim parts() As String = archiveFolder.Split("/"c)
''''                category = parts(0)
''''                subFolder = String.Join("/", parts.Skip(1))
''''            Else
''''                category = archiveFolder
''''                subFolder = DateTime.Now.ToString("yyyy-MM") ' مجلد فرعي افتراضي بالشهر الحالي
''''            End If

''''            ' الحصول على مسار التصنيف الصحيح
''''            Dim categoryPath As String = GetCategoryPath(category)

''''            ' إنشاء المجلد الفرعي إذا كان محددًا
''''            Dim destinationFolder As String
''''            If Not String.IsNullOrEmpty(subFolder) Then
''''                destinationFolder = CreateSubFolder(category, subFolder)
''''            Else
''''                destinationFolder = categoryPath
''''            End If

''''            Dim newFilepath As String = Path.Combine(destinationFolder, filename & "." & fileType)

''''            ' نسخ الملف إلى مسار الأرشيف
''''            If File.Exists(originalFilepath) Then
''''                ' التحقق من وجود الملف في المسار الجديد
''''                If File.Exists(newFilepath) Then
''''                    ' إنشاء نسخة احتياطية من الملف الموجود
''''                    Dim backupPath As String = Path.Combine(destinationFolder, filename & "_backup_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & "." & fileType)
''''                    File.Move(newFilepath, backupPath)
''''                End If

''''                File.Copy(originalFilepath, newFilepath, True)
''''            Else
''''                Throw New FileNotFoundException("الملف الأصلي غير موجود: " & originalFilepath)
''''            End If

''''            ' حفظ المسار النسبي في قاعدة البيانات (بدءًا من مجلد التصنيف)
''''            Dim relativePath As String = Path.Combine(Path.GetFileName(categoryPath),
''''                                                     If(String.IsNullOrEmpty(subFolder), "", subFolder),
''''                                                     filename & "." & fileType)

''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum),
''''                DatabaseHelper.CreateParameter("@ArchiveSubject", archiveSubject),
''''                DatabaseHelper.CreateParameter("@ArchiveFolder", archiveFolder), ' حفظ التصنيف الأصلي
''''                DatabaseHelper.CreateParameter("@ArchiveDate", archiveDate),
''''                DatabaseHelper.CreateParameter("@ArchiveWillMaster", archiveWillMaster),
''''                DatabaseHelper.CreateParameter("@ArchiveWillSub", archiveWillSub),
''''                DatabaseHelper.CreateParameter("@ArchiveNotes", archiveNotes),
''''                DatabaseHelper.CreateParameter("@filename", filename),
''''                DatabaseHelper.CreateParameter("@fileType", fileType),
''''                DatabaseHelper.CreateParameter("@filepath", newFilepath), ' حفظ المسار الكامل للملف المؤرشف
''''                DatabaseHelper.CreateParameter("@UserName", userName)
''''            }

''''            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("AddArchive", parameters)
''''            If dt.Rows.Count > 0 Then
''''                Return Convert.ToInt32(dt.Rows(0)("ID"))
''''            End If
''''            Return 0
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في إضافة الأرشيف: " & ex.Message)
''''        End Try
''''    End Function

''''    Public Function UpdateArchive(id As Integer, archiveNum As String, archiveSubject As String, archiveFolder As String, archiveDate As Date, archiveWillMaster As String, archiveWillSub As String, archiveNotes As String, filename As String, fileType As String, originalFilepath As String, userName As String) As Boolean
''''        Try
''''            ' إنشاء مسار الأرشيف الكامل للملف
''''            Dim destinationFolder As String = Path.Combine(ARCHIVE_BASE_PATH, archiveFolder)
''''            If Not Directory.Exists(destinationFolder) Then
''''                Directory.CreateDirectory(destinationFolder)
''''            End If

''''            Dim newFilepath As String = Path.Combine(destinationFolder, filename & "." & fileType)

''''            ' إذا تغير مسار الملف الأصلي، قم بنسخ الملف الجديد
''''            If originalFilepath <> newFilepath Then
''''                If File.Exists(originalFilepath) Then
''''                    File.Copy(originalFilepath, newFilepath, True)
''''                Else
''''                    ' إذا لم يتم العثور على الملف الأصلي، يمكن التعامل مع هذا الخطأ أو تجاهله حسب الحاجة
''''                    ' حاليًا، سنسمح بالتحديث حتى لو لم يتم العثور على الملف الأصلي، ولكن لن يتم نسخ ملف جديد
''''                End If
''''            End If

''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@ID", id),
''''                DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum),
''''                DatabaseHelper.CreateParameter("@ArchiveSubject", archiveSubject),
''''                DatabaseHelper.CreateParameter("@ArchiveFolder", archiveFolder),
''''                DatabaseHelper.CreateParameter("@ArchiveDate", archiveDate),
''''                DatabaseHelper.CreateParameter("@ArchiveWillMaster", archiveWillMaster),
''''                DatabaseHelper.CreateParameter("@ArchiveWillSub", archiveWillSub),
''''                DatabaseHelper.CreateParameter("@ArchiveNotes", archiveNotes),
''''                DatabaseHelper.CreateParameter("@filename", filename),
''''                DatabaseHelper.CreateParameter("@fileType", fileType),
''''                DatabaseHelper.CreateParameter("@filepath", newFilepath), ' حفظ المسار الجديد للملف المؤرشف
''''                DatabaseHelper.CreateParameter("@UserName", userName)
''''            }

''''            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("UpdateArchive", parameters)
''''            Return rowsAffected > 0
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في تحديث الأرشيف: " & ex.Message)
''''        End Try
''''    End Function

''''    Public Function DeleteArchive(id As Integer) As Boolean
''''        Try
''''            ' جلب مسار الملف قبل الحذف من قاعدة البيانات
''''            Dim dt As DataTable = GetArchiveById(id)
''''            Dim filePathToDelete As String = String.Empty
''''            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
''''                filePathToDelete = dt.Rows(0)("filepath").ToString()
''''            End If

''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@ID", id)
''''            }

''''            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("DeleteArchive", parameters)

''''            If rowsAffected > 0 Then
''''                ' محاولة حذف الملف من نظام الملفات بعد الحذف من قاعدة البيانات
''''                If File.Exists(filePathToDelete) Then
''''                    Try
''''                        ' محاولة حذف الملف مع إعادة المحاولة
''''                        DeleteFileWithRetry(filePathToDelete, 3)
''''                    Catch fileEx As Exception
''''                        ' إذا فشل حذف الملف، نقله إلى مجلد المحذوفات
''''                        Try
''''                            MoveFileToDeletedFolder(filePathToDelete)
''''                        Catch moveEx As Exception
''''                            ' إذا فشل النقل أيضًا، اترك الملف واعرض تحذير
''''                            Throw New Exception($"تم حذف السجل من قاعدة البيانات ولكن فشل في حذف الملف: {filePathToDelete}{vbNewLine} السبب: {fileEx.Message}{vbNewLine}يرجى إغلاق أي برامج تستخدم هذا الملف وحذفه يدوياً.")
''''                        End Try
''''                    End Try
''''                End If
''''                Return True
''''            Else
''''                Return False
''''            End If
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في حذف الأرشيف: " & ex.Message)
''''        End Try
''''    End Function

''''    ''' <summary>
''''    ''' محاولة حذف الملف مع إعادة المحاولة
''''    ''' </summary>
''''    Private Function DeleteFileWithRetry(filePath As String, maxRetries As Integer) As Boolean
''''        Dim retryCount As Integer = 0

''''        While retryCount < maxRetries
''''            Try
''''                ' التحقق من وجود الملف
''''                If Not File.Exists(filePath) Then
''''                    Return True ' اعتبره محذوفًا إذا كان غير موجود
''''                End If

''''                ' إزالة خاصية "للقراءة فقط" إذا وجدت
''''                Dim fileInfo As New IO.FileInfo(filePath)
''''                fileInfo.IsReadOnly = False

''''                ' محاولة الحذف
''''                File.Delete(filePath)

''''                ' التحقق من أن الملف تم حذفه
''''                If Not File.Exists(filePath) Then
''''                    Return True
''''                End If

''''            Catch ex As Exception
''''                ' الانتظار قبل إعادة المحاولة
''''                Threading.Thread.Sleep(500)
''''                retryCount += 1
''''            End Try
''''        End While

''''        Return False
''''    End Function
''''    ' دالة لنقل الملف إلى مجلد المحذوفات
''''    Private Sub MoveFileToDeletedFolder(filePath As String)
''''        Try
''''            ' إنشاء مجلد المحذوفات
''''            Dim deletedFolderPath As String = Path.Combine(ARCHIVE_BASE_PATH, "المحذوفات")
''''            If Not Directory.Exists(deletedFolderPath) Then
''''                Directory.CreateDirectory(deletedFolderPath)
''''            End If

''''            ' إنشاء اسم ملف فريد في مجلد المحذوفات
''''            Dim fileName As String = Path.GetFileName(filePath)
''''            Dim fileNameWithoutExt As String = Path.GetFileNameWithoutExtension(filePath)
''''            Dim fileExt As String = Path.GetExtension(filePath)
''''            Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
''''            Dim newFileName As String = $"{fileNameWithoutExt}_deleted_{timestamp}{fileExt}"
''''            Dim newFilePath As String = Path.Combine(deletedFolderPath, newFileName)

''''            ' نقل الملف
''''            File.Move(filePath, newFilePath)
''''        Catch ex As Exception
''''            Throw New Exception($"فشل في نقل الملف إلى مجلد المحذوفات: {ex.Message}")
''''        End Try
''''    End Sub

''''    ' دالة للتحقق من وجود أرشيف بمعرف معين
''''    Public Function ArchiveExists(archiveId As Integer) As Boolean
''''        Try
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@ID", archiveId)
''''            }

''''            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("GetArchiveById", parameters)
''''            Return dt IsNot Nothing AndAlso dt.Rows.Count > 0
''''        Catch ex As Exception
''''            Return False
''''        End Try
''''    End Function

''''    ' دالة للحذف التلقائي للأرشيف المرتبط بالصادر

''''    ' ملاحظة: هذا الكود يجب إضافته إلى ArchiveClass.vb، وليس إلى SaderForm.vb
''''    ' لكن سأضعه هنا للإشارة إلى التعديل المطلوب
''''    Public Function DeleteArchiveByArchiveId(archiveId As Integer, Optional showMessages As Boolean = False) As Boolean
''''        Try
''''            ' التحقق من وجود الأرشيف أولاً
''''            If Not ArchiveExists(archiveId) Then
''''                If showMessages Then
''''                    MessageBox.Show($"الأرشيف بمعرف {archiveId} غير موجود أو تم حذفه مسبقًا", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''                End If
''''                Return True ' اعتبر العملية ناجحة إذا كان الأرشيف غير موجود أصلاً
''''            End If

''''            ' جلب مسار الملف قبل الحذف من قاعدة البيانات
''''            Dim dt As DataTable = GetArchiveById(archiveId)
''''            Dim filePathToDelete As String = String.Empty
''''            Dim archiveNum As String = String.Empty

''''            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
''''                ' تصحيح المسار باستخدام الدالة الجديدة
''''                filePathToDelete = NormalizeFilePath(dt.Rows(0)("filepath"))
''''                archiveNum = If(dt.Rows(0)("ArchiveNum") IsNot Nothing, dt.Rows(0)("ArchiveNum").ToString(), $"معرف {archiveId}")
''''            End If

''''            ' حذف السجل من قاعدة البيانات أولاً
''''            Dim parameters() As SqlParameter = {
''''            DatabaseHelper.CreateParameter("@ID", archiveId)
''''        }

''''            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("DeleteArchive", parameters)

''''            If rowsAffected > 0 Then
''''                ' محاولة حذف الملف من نظام الملفات بعد الحذف من قاعدة البيانات
''''                If Not String.IsNullOrEmpty(filePathToDelete) Then
''''                    Try
''''                        ' التحقق من وجود الملف أولاً
''''                        If File.Exists(filePathToDelete) Then
''''                            ' محاولة حذف الملف مع إعادة المحاولة
''''                            If DeleteFileWithRetry(filePathToDelete, 3) Then
''''                                If showMessages Then
''''                                    MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} والملف المرتبط به بنجاح", "حذف الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''                                End If
''''                            Else
''''                                ' إذا فشل الحذف، حاول معرفة السبب
''''                                Dim reason As String = GetFileDeleteFailureReason(filePathToDelete)
''''                                If showMessages Then
''''                                    MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات ولكن فشل في حذف الملف: {filePathToDelete}{vbNewLine}السبب: {reason}{vbNewLine}يرجى إغلاق أي برامج تستخدم هذا الملف وحذفه يدوياً.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''''                                End If
''''                            End If
''''                        Else
''''                            ' الملف غير موجود أصلاً
''''                            If showMessages Then
''''                                MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات (الملف غير موجود في المسار)", "حذف الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''                            End If
''''                        End If
''''                    Catch ex As Exception
''''                        ' معالجة الأخطاء غير المتوقعة
''''                        Dim errorMessage As String = $"فشل في معالجة الملف: {ex.Message}"
''''                        If showMessages Then
''''                            MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات ولكن حدث خطأ في معالجة الملف:{vbNewLine}{errorMessage}", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''''                        End If
''''                    End Try
''''                Else
''''                    If showMessages Then
''''                        MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات (لا يوجد مسار ملف مرتبط)", "حذف الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''                    End If
''''                End If
''''                Return True
''''            Else
''''                If showMessages Then
''''                    MessageBox.Show($"لم يتم العثور على الأرشيف بمعرف {archiveId} في قاعدة البيانات", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''                End If
''''                Return True
''''            End If
''''        Catch ex As Exception
''''            If showMessages Then
''''                MessageBox.Show($"حدث خطأ أثناء حذف الأرشيف بمعرف {archiveId}: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
''''            End If
''''            Return False
''''        End Try
''''    End Function


''''    'Public Function DeleteArchiveByArchiveId(archiveId As Integer, Optional showMessages As Boolean = False) As Boolean
''''    '    Try
''''    '        ' التحقق من وجود الأرشيف أولاً
''''    '        If Not ArchiveExists(archiveId) Then
''''    '            If showMessages Then
''''    '                MessageBox.Show($"الأرشيف بمعرف {archiveId} غير موجود أو تم حذفه مسبقًا", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '            End If
''''    '            Return True
''''    '        End If

''''    '        ' جلب مسار الملف قبل الحذف من قاعدة البيانات
''''    '        Dim dt As DataTable = GetArchiveById(archiveId)
''''    '        Dim filePathToDelete As String = String.Empty
''''    '        Dim archiveNum As String = String.Empty

''''    '        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
''''    '            ' تصحيح المسار (إزالة المسافات الزائدة، تصحيح الشرطات)
''''    '            filePathToDelete = NormalizeFilePath(dt.Rows(0)("filepath"))
''''    '            archiveNum = If(dt.Rows(0)("ArchiveNum") IsNot Nothing, dt.Rows(0)("ArchiveNum").ToString(), $"معرف {archiveId}")
''''    '        End If

''''    '        ' حذف السجل من قاعدة البيانات أولاً
''''    '        Dim parameters() As SqlParameter = {
''''    '        DatabaseHelper.CreateParameter("@ID", archiveId)
''''    '    }

''''    '        Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("DeleteArchive", parameters)

''''    '        If rowsAffected > 0 Then
''''    '            ' محاولة حذف الملف من نظام الملفات بعد الحذف من قاعدة البيانات
''''    '            If Not String.IsNullOrEmpty(filePathToDelete) Then
''''    '                Try
''''    '                    ' التحقق من وجود الملف أولاً
''''    '                    If File.Exists(filePathToDelete) Then
''''    '                        ' محاولة حذف الملف مع إعادة المحاولة
''''    '                        If DeleteFileWithRetry(filePathToDelete, 3) Then
''''    '                            If showMessages Then
''''    '                                MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} والملف المرتبط به بنجاح", "حذف الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '                            End If
''''    '                        Else
''''    '                            ' إذا فشل الحذف، حاول معرفة السبب
''''    '                            Dim reason As String = GetFileDeleteFailureReason(filePathToDelete)
''''    '                            If showMessages Then
''''    '                                MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات ولكن فشل في حذف الملف: {filePathToDelete}{vbNewLine}السبب: {reason}{vbNewLine}يرجى إغلاق أي برامج تستخدم هذا الملف وحذفه يدوياً.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''''    '                            End If
''''    '                        End If
''''    '                    Else
''''    '                        ' الملف غير موجود أصلاً
''''    '                        If showMessages Then
''''    '                            MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات (الملف غير موجود في المسار)", "حذف الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '                        End If
''''    '                    End If
''''    '                Catch ex As Exception
''''    '                    ' معالجة الأخطاء غير المتوقعة
''''    '                    Dim errorMessage As String = $"فشل في معالجة الملف: {ex.Message}"
''''    '                    If showMessages Then
''''    '                        MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات ولكن حدث خطأ في معالجة الملف:{vbNewLine}{errorMessage}", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''''    '                    End If
''''    '                End Try
''''    '            Else
''''    '                If showMessages Then
''''    '                    MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات (لا يوجد مسار ملف مرتبط)", "حذف الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '                End If
''''    '            End If
''''    '            Return True
''''    '        Else
''''    '            If showMessages Then
''''    '                MessageBox.Show($"لم يتم العثور على الأرشيف بمعرف {archiveId} في قاعدة البيانات", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '            End If
''''    '            Return True
''''    '        End If
''''    '    Catch ex As Exception
''''    '        If showMessages Then
''''    '            MessageBox.Show($"حدث خطأ أثناء حذف الأرشيف بمعرف {archiveId}: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
''''    '        End If
''''    '        Return False
''''    '    End Try
''''    'End Function


''''    ''' <summary>
''''    ''' تصحيح مسار الملف (إزالة المسافات الزائدة، تحويل الشرطات)
''''    ''' </summary>
''''    Private Function NormalizeFilePath(filePath As Object) As String
''''        If filePath Is Nothing OrElse filePath Is DBNull.Value Then
''''            Return String.Empty
''''        End If

''''        Dim path As String = filePath.ToString().Trim()

''''        ' استبدال الشرطات المائلة العكسية بشرطات مائلة إذا لزم
''''        path = path.Replace("/", "\")

''''        ' إزالة الشرطات الزائدة في النهاية
''''        While path.EndsWith("\")
''''            path = path.Substring(0, path.Length - 1)
''''        End While

''''        Return path
''''    End Function


''''    ''' <summary>
''''    ''' تحديد سبب فشل حذف الملف
''''    ''' </summary>
''''    Private Function GetFileDeleteFailureReason(filePath As String) As String
''''        Try
''''            ' التحقق من وجود الملف
''''            If Not File.Exists(filePath) Then
''''                Return "الملف غير موجود"
''''            End If

''''            ' محاولة فتح الملف للقراءة (للتحقق مما إذا كان قيد الاستخدام)
''''            Using fs As New IO.FileStream(filePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.None)
''''                ' إذا نجحت هذه السطر، فالمشكلة ليست في الاستخدام
''''            End Using

''''            ' التحقق من الصلاحيات
''''            Dim permissionTestPath As String = Path.Combine(Path.GetDirectoryName(filePath), "permission_test.tmp")
''''            Try
''''                File.WriteAllText(permissionTestPath, "test")
''''                File.Delete(permissionTestPath)
''''            Catch
''''                Return "مشكلة في الصلاحيات - لا يمكن الكتابة في هذا المجلد"
''''            End Try

''''            ' إذا لم تكن أي من الأسباب السابقة، فربما مشكلة أخرى
''''            Return "سبب غير معروف (قد يكون الملف قيد الاستخدام من قبل عملية نظام)"

''''        Catch ex As UnauthorizedAccessException
''''            Return "صلاحيات غير كافية لحذف الملف"
''''        Catch ex As IOException When ex.Message.Contains("process cannot access the file")
''''            Return "الملف قيد الاستخدام من قبل برنامج آخر"
''''        Catch ex As Exception
''''            Return $"خطأ غير متوقع: {ex.Message}"
''''        End Try
''''    End Function

''''    'Public Function DeleteArchiveByArchiveId(archiveId As Integer, Optional showMessages As Boolean = False) As Boolean
''''    '    Try
''''    '        ' التحقق من وجود الأرشيف أولاً
''''    '        If Not ArchiveExists(archiveId) Then
''''    '            If showMessages Then
''''    '                MessageBox.Show($"الأرشيف بمعرف {archiveId} غير موجود أو تم حذفه مسبقًا", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '            End If
''''    '            Return True ' اعتبر العملية ناجحة إذا كان الأرشيف غير موجود أصلاً
''''    '        End If

''''    '        ' جلب مسار الملف قبل الحذف من قاعدة البيانات
''''    '        Dim dt As DataTable = GetArchiveById(archiveId)
''''    '        Dim filePathToDelete As String = String.Empty
''''    '        Dim archiveNum As String = String.Empty

''''    '        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
''''    '            filePathToDelete = If(dt.Rows(0)("filepath") IsNot Nothing, dt.Rows(0)("filepath").ToString(), "")
''''    '            archiveNum = If(dt.Rows(0)("ArchiveNum") IsNot Nothing, dt.Rows(0)("ArchiveNum").ToString(), $"معرف {archiveId}")
''''    '        End If

''''    '        ' حذف السجل من قاعدة البيانات أولاً
''''    '        Dim parameters() As SqlParameter = {
''''    '            DatabaseHelper.CreateParameter("@ID", archiveId)
''''    '        }

''''    '        Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("DeleteArchive", parameters)

''''    '        If rowsAffected > 0 Then
''''    '            ' محاولة حذف الملف من نظام الملفات بعد الحذف من قاعدة البيانات
''''    '            If Not String.IsNullOrEmpty(filePathToDelete) AndAlso File.Exists(filePathToDelete) Then
''''    '                Try
''''    '                    ' محاولة حذف الملف مع إعادة المحاولة
''''    '                    DeleteFileWithRetry(filePathToDelete, 3)
''''    '                    If showMessages Then
''''    '                        MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} والملف المرتبط به بنجاح", "حذف الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '                    End If
''''    '                Catch fileEx As Exception
''''    '                    ' إذا فشل حذف الملف، نقله إلى مجلد المحذوفات
''''    '                    Try
''''    '                        MoveFileToDeletedFolder(filePathToDelete)
''''    '                        If showMessages Then
''''    '                            MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات ونقل الملف إلى مجلد المحذوفات", "حذف الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '                        End If
''''    '                    Catch moveEx As Exception
''''    '                        ' إذا فشل النقل أيضًا، اترك الملف واعرض تحذير
''''    '                        If showMessages Then
''''    '                            MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات ولكن فشل في حذف الملف: {filePathToDelete}{vbNewLine}السبب: {fileEx.Message}{vbNewLine}يرجى إغلاق أي برامج تستخدم هذا الملف وحذفه يدوياً.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
''''    '                        End If
''''    '                    End Try
''''    '                End Try
''''    '            Else
''''    '                If showMessages Then
''''    '                    If String.IsNullOrEmpty(filePathToDelete) Then
''''    '                        MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات (لا يوجد مسار ملف مرتبط)", "حذف الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '                    Else
''''    '                        MessageBox.Show($"تم حذف الأرشيف رقم {archiveNum} من قاعدة البيانات (الملف غير موجود في المسار)", "حذف الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '                    End If
''''    '                End If
''''    '            End If
''''    '            Return True
''''    '        Else
''''    '            If showMessages Then
''''    '                MessageBox.Show($"لم يتم العثور على الأرشيف بمعرف {archiveId} في قاعدة البيانات", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
''''    '            End If
''''    '            Return True ' اعتبر العملية ناجحة حتى لو لم يتم حذف أي شيء
''''    '        End If
''''    '    Catch ex As Exception
''''    '        If showMessages Then
''''    '            MessageBox.Show($"حدث خطأ أثناء حذف الأرشيف بمعرف {archiveId}: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
''''    '        End If
''''    '        Return False
''''    '    End Try
''''    'End Function

''''    Public Function SearchArchive(searchTerm As String) As DataTable
''''        Try
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@SearchTerm", searchTerm)
''''            }

''''            Return DatabaseHelper.ExecuteStoredProcedure("SearchArchive", parameters)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في البحث عن الأرشيف: " & ex.Message)
''''        End Try
''''    End Function


''''    Public Function SearchSaderByDateRange(startDate As Date, endDate As Date) As DataTable
''''        Try
''''            ' التأكد من صحة التواريخ
''''            If startDate > endDate Then
''''                Dim temp As Date = startDate
''''                startDate = endDate
''''                endDate = temp
''''            End If

''''            Dim query As String = "SELECT ID, ArchiveNum, ArchiveDate, ArchiveSubject, ArchiveWillMaster, ArchiveWillSub, UserName FROM TbArchive WHERE ArchiveDate BETWEEN @startDate AND @endDate ORDER BY ArchiveDate DESC"

''''            Using connection As SqlConnection = GetConnection()
''''                connection.Open()

''''                Using command As New SqlCommand(query, connection)
''''                    command.Parameters.AddWithValue("@startDate", startDate.Date)
''''                    command.Parameters.AddWithValue("@endDate", endDate.Date.AddDays(1).AddSeconds(-1))

''''                    Using adapter As New SqlDataAdapter(command)
''''                        Dim dataTable As New DataTable()
''''                        adapter.Fill(dataTable)

''''                        Return ValidateAndCleanSaderData(dataTable)
''''                    End Using
''''                End Using
''''            End Using

''''        Catch ex As Exception
''''            System.Diagnostics.Debug.WriteLine($"خطأ في SearchSaderByDateRange: {ex.Message}")
''''            Return CreateEmptySaderTable()
''''        End Try
''''    End Function

''''    ''' <summary>
''''    ''' التحقق من صحة وتنظيف بيانات الصادر
''''    ''' </summary>
''''    ''' <param name="dataTable">جدول البيانات الأصلي</param>
''''    ''' <returns>جدول بيانات منظف</returns>
''''    Private Function ValidateAndCleanSaderData(dataTable As DataTable) As DataTable
''''        Try
''''            If dataTable Is Nothing OrElse dataTable.Rows.Count = 0 Then
''''                Return CreateEmptySaderTable()
''''            End If

''''            ' إنشاء جدول جديد منظف
''''            Dim cleanTable As DataTable = CreateEmptySaderTable()

''''            ' نسخ الصفوف مع التحقق من صحتها
''''            For Each row As DataRow In dataTable.Rows
''''                Try
''''                    If row.RowState <> DataRowState.Deleted Then
''''                        Dim newRow As DataRow = cleanTable.NewRow()

''''                        ' نسخ البيانات مع معالجة القيم الفارغة
''''                        newRow("ID") = If(IsDBNull(row("ID")), 0, Convert.ToInt32(row("ID")))
''''                        newRow("ArchiveNum") = If(IsDBNull(row("ArchiveNum")), "", row("ArchiveNum").ToString().Trim())
''''                        newRow("ArchiveDate") = If(IsDBNull(row("ArchiveDate")), DateTime.Now, Convert.ToDateTime(row("ArchiveDate")))
''''                        newRow("ArchiveSubject") = If(IsDBNull(row("ArchiveSubject")), "", row("ArchiveSubject").ToString().Trim())
''''                        newRow("ArchiveFolder") = If(IsDBNull(row("ArchiveFolder")), "", row("ArchiveFolder").ToString().Trim())
''''                        newRow("ArchiveWillMaster") = If(IsDBNull(row("ArchiveWillMaster")), "", row("ArchiveWillMaster").ToString().Trim())
''''                        newRow("ArchiveWillSub") = If(IsDBNull(row("ArchiveWillSub")), "", row("ArchiveWillSub").ToString().Trim())
''''                        newRow("ArchiveNotes") = If(IsDBNull(row("ArchiveNotes")), "", row("ArchiveNotes").ToString().Trim())
''''                        newRow("filename") = If(IsDBNull(row("filename")), "", row("filename").ToString().Trim())
''''                        newRow("fileType") = If(IsDBNull(row("fileType")), "", row("fileType").ToString().Trim())
''''                        newRow("filepath") = If(IsDBNull(row("filepath")), "", row("filepath").ToString().Trim())
''''                        newRow("UserName") = If(IsDBNull(row("UserName")), "", row("UserName").ToString().Trim())


''''                        cleanTable.Rows.Add(newRow)
''''                    End If
''''                Catch ex As Exception
''''                    System.Diagnostics.Debug.WriteLine($"خطأ في معالجة صف البيانات: {ex.Message}")
''''                    Continue For
''''                End Try
''''            Next

''''            Return cleanTable

''''        Catch ex As Exception
''''            System.Diagnostics.Debug.WriteLine($"خطأ في ValidateAndCleanSaderData: {ex.Message}")
''''            Return CreateEmptySaderTable()
''''        End Try
''''    End Function

''''    ''' <summary>
''''    ''' إنشاء جدول صادر فارغ بالهيكل الصحيح
''''    ''' </summary>
''''    ''' <returns>جدول بيانات فارغ</returns>
''''    Private Function CreateEmptySaderTable() As DataTable
''''        Try
''''            Dim emptyTable As New DataTable()

''''            emptyTable.Columns.Add("ID", GetType(Integer))
''''            emptyTable.Columns.Add("ArchiveNum", GetType(String))
''''            emptyTable.Columns.Add("ArchiveDate", GetType(DateTime))
''''            emptyTable.Columns.Add("ArchiveSubject", GetType(String))
''''            emptyTable.Columns.Add("ArchiveFolder", GetType(String))
''''            emptyTable.Columns.Add("ArchiveWillMaster", GetType(String))
''''            emptyTable.Columns.Add("ArchiveWillSub", GetType(String))
''''            emptyTable.Columns.Add("ArchiveNotes", GetType(String))
''''            emptyTable.Columns.Add("UserName", GetType(String))


''''            ' تعيين القيم الافتراضية
''''            emptyTable.Columns("ID").DefaultValue = 0
''''            emptyTable.Columns("ArchiveNum").DefaultValue = ""
''''            emptyTable.Columns("ArchiveDate").DefaultValue = DateTime.Now
''''            emptyTable.Columns("ArchiveSubject").DefaultValue = ""
''''            emptyTable.Columns("ArchiveFolder").DefaultValue = ""
''''            emptyTable.Columns("ArchiveWillMaster").DefaultValue = ""
''''            emptyTable.Columns("ArchiveWillSub").DefaultValue = ""
''''            emptyTable.Columns("ArchiveNotes").DefaultValue = ""
''''            emptyTable.Columns("UserName").DefaultValue = ""


''''            Return emptyTable

''''        Catch ex As Exception
''''            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء جدول فارغ: {ex.Message}")
''''            Return New DataTable()
''''        End Try
''''    End Function


''''    'Public Function SearchArchiveByDateRange(startDate As Date, endDate As Date) As DataTable
''''    '    Try
''''    '        Dim parameters() As SqlParameter = {
''''    '            DatabaseHelper.CreateParameter("@StartDate", startDate),
''''    '            DatabaseHelper.CreateParameter("@EndDate", endDate)
''''    '        }

''''    '        Return DatabaseHelper.ExecuteStoredProcedure("SearchArchiveByDateRange", parameters)
''''    '    Catch ex As Exception
''''    '        Throw New Exception("خطأ في البحث عن الأرشيف بالتاريخ: " & ex.Message)
''''    '    End Try
''''    'End Function

''''    Public Function GetAllArchive() As DataTable
''''        Try
''''            Dim query As String = "SELECT * FROM TbArchive ORDER BY ArchiveDate"
''''            Return DatabaseHelper.ExecuteQuery(query)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على الأرشيف: " & ex.Message)
''''        End Try
''''    End Function

''''    Public Function GetArchiveById(id As Integer) As DataTable
''''        Try
''''            Dim query As String = "SELECT * FROM TbArchive WHERE ID = @ID"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@ID", id)
''''            }

''''            Return DatabaseHelper.ExecuteQuery(query, parameters)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على بيانات الأرشيف: " & ex.Message)
''''        End Try
''''    End Function

''''    Public Function IsArchiveNumberExists(archiveNum As String, Optional excludeId As Integer = 0) As Boolean
''''        Try
''''            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE ArchiveNum = @ArchiveNum"
''''            If excludeId > 0 Then
''''                query += " AND ID <> @ExcludeId"
''''            End If

''''            Dim parameters As New List(Of SqlParameter) From {
''''                DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum)
''''            }

''''            If excludeId > 0 Then
''''                parameters.Add(DatabaseHelper.CreateParameter("@ExcludeId", excludeId))
''''            End If

''''            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters.ToArray()))
''''            Return count > 0
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في التحقق من رقم الأرشيف: " & ex.Message)
''''        End Try
''''    End Function

''''    Public Function GetArchiveStatistics() As DataTable
''''        Try
''''            Dim query As String = "
''''                SELECT 
''''                    COUNT(*) AS TotalCount,
''''                    COUNT(CASE WHEN MONTH(ArchiveDate) = MONTH(GETDATE()) AND YEAR(ArchiveDate) = YEAR(GETDATE()) THEN 1 END) AS CurrentMonthCount,
''''                    COUNT(CASE WHEN YEAR(ArchiveDate) = YEAR(GETDATE()) THEN 1 END) AS CurrentYearCount,
''''                    COUNT(DISTINCT ArchiveFolder) AS FoldersCount
''''                FROM TbArchive"

''''            Return DatabaseHelper.ExecuteQuery(query)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على إحصائيات الأرشيف: " & ex.Message)
''''        End Try
''''    End Function

''''    Public Function GetArchiveByFolder(folderName As String) As DataTable
''''        Try
''''            Dim query As String = "SELECT * FROM TbArchive WHERE ArchiveFolder = @FolderName ORDER BY ArchiveDate"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@FolderName", folderName)
''''            }

''''            Return DatabaseHelper.ExecuteQuery(query, parameters)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على الأرشيف حسب المجلد: " & ex.Message)
''''        End Try
''''    End Function

''''    Public Function GetFoldersList() As DataTable
''''        Try
''''            Dim query As String = "SELECT DISTINCT ArchiveFolder FROM TbArchive WHERE ArchiveFolder IS NOT NULL ORDER BY ArchiveFolder"
''''            Return DatabaseHelper.ExecuteQuery(query)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على قائمة المجلدات: " & ex.Message)
''''        End Try
''''    End Function

''''    Public Function GetArchiveByFileType(fileType As String) As DataTable
''''        Try
''''            Dim query As String = "SELECT * FROM TbArchive WHERE fileType = @FileType ORDER BY ArchiveDate"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@FileType", fileType)
''''            }

''''            Return DatabaseHelper.ExecuteQuery(query, parameters)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على الأرشيف حسب نوع الملف: " & ex.Message)
''''        End Try
''''    End Function

''''    ' دالة للتحقق من وجود ملف مكرر في نفس المسار
''''    Public Function CheckDuplicateFile(filename As String, fileType As String, archiveFolder As String) As Boolean
''''        Try
''''            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE filename = @Filename AND fileType = @FileType AND ArchiveFolder = @ArchiveFolder"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@Filename", filename),
''''                DatabaseHelper.CreateParameter("@FileType", fileType),
''''                DatabaseHelper.CreateParameter("@ArchiveFolder", archiveFolder)
''''            }

''''            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
''''            Return count > 0
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في التحقق من تكرار الملف: " & ex.Message)
''''        End Try
''''    End Function

''''    ' دالة للتحقق من تكرار رقم الأرشيف
''''    Public Function CheckDuplicateArchiveNumber(archiveNum As String) As Boolean
''''        Try
''''            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE ArchiveNum = @ArchiveNum"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum)
''''            }

''''            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
''''            Return count > 0
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في التحقق من تكرار رقم الأرشيف: " & ex.Message)
''''        End Try
''''    End Function

''''    ' دالة للتحقق من تكرار رقم الأرشيف عند التحديث (استثناء السجل الحالي)
''''    Public Function CheckDuplicateArchiveNumberForUpdate(archiveNum As String, currentId As Integer) As Boolean
''''        Try
''''            Dim query As String = "SELECT COUNT(*) FROM TbArchive WHERE ArchiveNum = @ArchiveNum AND ID <> @CurrentId"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum),
''''                DatabaseHelper.CreateParameter("@CurrentId", currentId)
''''            }

''''            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
''''            Return count > 0
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في التحقق من تكرار رقم الأرشيف عند التحديث: " & ex.Message)
''''        End Try
''''    End Function

''''    ' دالة للحصول على معلومات الأرشيف المكرر بناءً على رقم الأرشيف
''''    Public Function GetDuplicateArchiveInfo(archiveNum As String) As DataTable
''''        Try
''''            Dim query As String = "SELECT ArchiveNum, ArchiveSubject, ArchiveDate, ArchiveFolder, UserName FROM TbArchive WHERE ArchiveNum = @ArchiveNum"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@ArchiveNum", archiveNum)
''''            }

''''            Return DatabaseHelper.ExecuteQuery(query, parameters)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على معلومات الأرشيف المكرر: " & ex.Message)
''''        End Try
''''    End Function

''''    ' دالة للحصول على معلومات الملف المكرر
''''    Public Function GetDuplicateFileInfo(filename As String, fileType As String, archiveFolder As String) As DataTable
''''        Try
''''            Dim query As String = "SELECT ArchiveNum, ArchiveSubject, ArchiveDate, UserName FROM TbArchive WHERE filename = @Filename AND fileType = @FileType AND ArchiveFolder = @ArchiveFolder"
''''            Dim parameters() As SqlParameter = {
''''                DatabaseHelper.CreateParameter("@Filename", filename),
''''                DatabaseHelper.CreateParameter("@FileType", fileType),
''''                DatabaseHelper.CreateParameter("@ArchiveFolder", archiveFolder)
''''            }

''''            Return DatabaseHelper.ExecuteQuery(query, parameters)
''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على معلومات الملف المكرر: " & ex.Message)
''''        End Try
''''    End Function

''''    Public Function GetDistinctFileTypes() As DataTable
''''        Try
''''            Dim query As String = "SELECT DISTINCT FileType FROM TbArchive WHERE FileType IS NOT NULL AND FileType <> '' ORDER BY FileType"

''''            Using connection As SqlConnection = GetConnection()
''''                Using adapter As New SqlDataAdapter(query, connection)
''''                    Dim dt As New DataTable()
''''                    adapter.Fill(dt)
''''                    Return dt
''''                End Using
''''            End Using

''''        Catch ex As Exception
''''            Throw New Exception("خطأ في الحصول على أنواع الملفات: " & ex.Message)
''''        End Try
''''    End Function

''''    Public Sub OpenFileLocation(filePath As String)
''''        If String.IsNullOrWhiteSpace(filePath) Then
''''            Throw New ArgumentException("مسار الملف غير صالح")
''''            Return
''''        End If

''''        ' التحقق من وجود الملف فقط (بدون التحقق من المجلد)
''''        If File.Exists(filePath) Then
''''            Process.Start("explorer.exe", $"/select, ""{filePath}""")
''''        Else
''''            ' إذا لم يوجد الملف، نفتح المجلد فقط
''''            Dim directory = Path.GetDirectoryName(filePath)
''''            If Not String.IsNullOrEmpty(directory) Then
''''                Process.Start("explorer.exe", $"""{directory}""")
''''            Else
''''                Throw New DirectoryNotFoundException("المجلد غير موجود")
''''            End If
''''        End If
''''    End Sub

''''    'Public Function GetArchiveByFileType(fileType As String) As DataTable
''''    '    Try
''''    '        Dim query As String = "SELECT * FROM TbArchive WHERE FileType = @FileType ORDER BY ArchiveDate DESC"

''''    '        Using connection As SqlConnection = GetConnection()
''''    '            Using command As New SqlCommand(query, connection)
''''    '                command.Parameters.AddWithValue("@FileType", fileType)

''''    '                Using adapter As New SqlDataAdapter(command)
''''    '                    Dim dt As New DataTable()
''''    '                    adapter.Fill(dt)
''''    '                    Return dt
''''    '                End Using
''''    '            End Using
''''    '        End Using

''''    '    Catch ex As Exception
''''    '        Throw New Exception("خطأ في الحصول على الأرشيف حسب نوع الملف: " & ex.Message)
''''    '    End Try
''''    'End Function

''''End Class