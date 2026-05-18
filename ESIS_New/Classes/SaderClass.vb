Imports System.Data.SqlClient
Imports System.IO

Public Class SaderClass

    Private activityLogClass As New ActivityLogClass()
    Private archiveClass As New ArchiveClass


#Region " جلب المسار "
    Public Function GetUserSearchPath(userId As Integer) As String

        Dim path As String = ""

        Using con = DatabaseHelper.GetConnection()
            Using cmd As New SqlCommand("
            SELECT TOP 1 SearchPath 
            FROM UserSearchPaths 
            WHERE UserID = @UserID AND IsActive = 1", con)

                cmd.Parameters.AddWithValue("@UserID", userId)

                con.Open()

                Dim result = cmd.ExecuteScalar()

                If result IsNot Nothing Then
                    path = result.ToString()
                End If

            End Using
        End Using

        Return path

    End Function

    Public Sub SaveUserSearchPath(userId As Integer, path As String)

        Using con = DatabaseHelper.GetConnection()

            Using cmd As New SqlCommand("
        MERGE UserSearchPaths AS target
        USING (SELECT @UserID AS UserID) AS source
        ON target.UserID = source.UserID
        WHEN MATCHED THEN
            UPDATE SET SearchPath = @Path
        WHEN NOT MATCHED THEN
            INSERT (UserID, SearchPath) VALUES (@UserID, @Path);", con)

                cmd.Parameters.AddWithValue("@UserID", userId)
                cmd.Parameters.AddWithValue("@Path", path)

                con.Open()
                cmd.ExecuteNonQuery()

            End Using

        End Using

    End Sub

#End Region

#Region "📅 Years Filter"

    Public Function GetYears() As DataTable

        Dim dt As New DataTable()

        Using con As SqlConnection = DatabaseHelper.GetConnection()
            Using cmd As New SqlCommand("
            SELECT 
                Year,
                CASE 
                    WHEN IsClosed = 1 THEN N'مغلقة'
                    ELSE N'مفتوحة'
                END AS StatusText
            FROM TbYears
            ORDER BY Year DESC
        ", con)

                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)

            End Using
        End Using

        Return dt

    End Function

#End Region

#Region "🔍 Search | البحث"

    Public Function SearchSaderAdvanced(
    searchText As String,
    searchColumn As String,
    startDate As Date?,
    endDate As Date?,
    unarchivedOnly As Boolean,
    Optional year As Integer? = Nothing   ' 🔥 جديد
) As DataTable

        Dim dt As New DataTable()

        Try
            Using con As SqlConnection = DatabaseHelper.GetConnection()
                Using cmd As New SqlCommand("SearchSader", con)

                    cmd.CommandType = CommandType.StoredProcedure

                    ' =========================
                    ' 🔹 نص البحث
                    ' =========================
                    If String.IsNullOrWhiteSpace(searchText) Then
                        cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar).Value = searchText.Trim()
                    End If

                    ' =========================
                    ' 🔹 العمود
                    ' =========================
                    If String.IsNullOrWhiteSpace(searchColumn) Then
                        cmd.Parameters.Add("@SearchColumn", SqlDbType.NVarChar).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("@SearchColumn", SqlDbType.NVarChar).Value = searchColumn
                    End If

                    ' =========================
                    ' 🔹 التاريخ
                    ' =========================
                    If startDate.HasValue Then
                        cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = startDate.Value.Date
                    Else
                        cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = DBNull.Value
                    End If

                    If endDate.HasValue Then
                        cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value = endDate.Value.Date
                    Else
                        cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value = DBNull.Value
                    End If

                    ' =========================
                    ' 🔹 السنة 🔥 (الجديد)
                    ' =========================
                    If year.HasValue Then
                        cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year.Value
                    Else
                        cmd.Parameters.Add("@Year", SqlDbType.Int).Value = DBNull.Value
                    End If

                    ' =========================
                    ' 🔹 غير المؤرشف
                    ' =========================
                    cmd.Parameters.Add("@UnarchivedOnly", SqlDbType.Bit).Value = unarchivedOnly

                    ' =========================
                    ' 🔹 تنفيذ
                    ' =========================
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(dt)

                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("خطأ في البحث: " & ex.Message)
        End Try

        Return dt

    End Function

#End Region

#Region " إضافة -  تعديل - حذف"

    ''' <summary>
    ''' إضافة صادر جديد وإرجاع معرفه
    ''' </summary>
    Public Function AddSader(dateBook As Date, subjBook As String, willMaster As String, willSub As String, bookCategory As String, userName As String) As Integer

        Try
            If String.IsNullOrWhiteSpace(subjBook) OrElse String.IsNullOrWhiteSpace(userName) Then
                Return 0
            End If

            If IsYearClosed(dateBook.Year) Then
                MessageBox.Show("السنة مقفلة ولا يمكن الإضافة")
                Return 0
            End If

            Dim year As Integer = dateBook.Year

            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@DateBook", dateBook),
            DatabaseHelper.CreateParameter("@SubjBook", subjBook.Trim()),
            DatabaseHelper.CreateParameter("@WillMaster", If(String.IsNullOrWhiteSpace(willMaster), DBNull.Value, willMaster.Trim())),
            DatabaseHelper.CreateParameter("@WillSub", If(String.IsNullOrWhiteSpace(willSub), DBNull.Value, willSub.Trim())),
            DatabaseHelper.CreateParameter("@bookCategory", bookCategory.Trim()),
            DatabaseHelper.CreateParameter("@UserName", userName.Trim()),
            DatabaseHelper.CreateParameter("@ArchiveID", DBNull.Value)
        }

            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("AddSader", parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

                Dim newId As Integer = Convert.ToInt32(dt.Rows(0)("ID"))
                Dim numberBook As Integer = Convert.ToInt32(dt.Rows(0)("NumberBook"))

                DataHelper.UpdateSaderCount()
                DataHelper.UpdateUnarchivedSaderCount()

                '    activityLogClass.LogUserActivity(
                '    userName,
                '    "Add",
                '    "TbSader",
                '    newId,
                '    $"تمت إضافة صادر رقم: {year}/{numberBook} - الموضوع: {subjBook}"
                ')

                Return newId
            Else
                Return 0
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في AddSader: {ex.Message}")
            Return 0
        End Try

    End Function

    ''' <summary>
    ''' تحديث صادر
    ''' </summary>
    Public Function UpdateSader(id As Integer, numberBook As Integer, year As Integer, dateBook As Date, subjBook As String, willMaster As String, willSub As String, bookCategory As String, userName As String, ArchiveID As String) As Boolean

        Try
            If id <= 0 OrElse numberBook <= 0 OrElse String.IsNullOrWhiteSpace(subjBook) Then
                Return False
            End If

            If IsYearClosed(dateBook.Year) Then
                MessageBox.Show("السنة مقفلة ولا يمكن التعديل")
                Return False
            End If

            ' 🔥 تحقق من التكرار (سنة + رقم)
            If IsBookNumberExists(numberBook, year, id) Then
                Return False
            End If

            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@ID", id),
            DatabaseHelper.CreateParameter("@NumberBook", numberBook),
            DatabaseHelper.CreateParameter("@Year", year),
            DatabaseHelper.CreateParameter("@DateBook", dateBook),
            DatabaseHelper.CreateParameter("@SubjBook", subjBook.Trim()),
            DatabaseHelper.CreateParameter("@WillMaster", If(String.IsNullOrWhiteSpace(willMaster), DBNull.Value, willMaster.Trim())),
            DatabaseHelper.CreateParameter("@WillSub", If(String.IsNullOrWhiteSpace(willSub), DBNull.Value, willSub.Trim())),
            DatabaseHelper.CreateParameter("@bookCategory", bookCategory.Trim()),
            DatabaseHelper.CreateParameter("@UserName", userName.Trim()),
            DatabaseHelper.CreateParameter("@ArchiveID", If(String.IsNullOrWhiteSpace(ArchiveID), DBNull.Value, ArchiveID))
        }

            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("UpdateSader", parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

                Dim rowsAffected As Integer = Convert.ToInt32(dt.Rows(0)("RowsAffected"))

                If rowsAffected > 0 Then
                    UpdateArchiveFromSader(id)
                End If

                activityLogClass.LogUserActivity(
                userName,
                "Edit",
                "TbSader",
                id,
                $"تم تعديل صادر رقم: {year}/{numberBook}"
            )

                Return rowsAffected > 0
            End If

            Return False

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateSader: {ex.Message}")
            Return False
        End Try

    End Function

    ''' <summary>
    ''' الحصول على موضوع الصادر بواسطة المعرف
    ''' </summary>
    Public Function GetSaderSubjectById(id As Integer) As String
        Try
            Dim query As String = "SELECT SubjBook FROM TbSader WHERE ID = @ID"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@ID", id)
        }

            Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                Return result.ToString()
            Else
                Return ""
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على موضوع الصادر: {ex.Message}")
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' حذف صادر
    ''' </summary>
    Public Function DeleteSader(id As Integer) As Boolean
        Try
            ' استخدام الإجراء المخزن "DeleteSader"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@ID", id)
        }

            ' تنفيذ الإجراء المخزن واحصل على عدد الصفوف المؤثرة
            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("DeleteSader", parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim rowsAffected As Integer = Convert.ToInt32(dt.Rows(0)("RowsAffected"))

                ' تحديث عداد الصادرة
                DataHelper.UpdateSaderCount()
                ' تسجيل نشاط إضافة الصادر
                activityLogClass.LogUserActivity(GlobalData.LoggedInUserName, "Delete", "TbSader", id, $"تم حذف كتاب صادر برقم: {GetSaderById(id)}, الموضوع: {GetSaderSubjectById(id)}" &
                If(GetArchiveIDBySaderID(id) > 0, $", مع حذف الأرشيف المرتبط (معرّف: {GetArchiveIDBySaderID(id)})", "")
            )

                Return rowsAffected > 0
            Else
                Return False
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حذف الصادر: {ex.Message}")
            Throw
        End Try
    End Function

    ''' <summary>
    ''' تحديث سجل الأرشيف المرتبط بالصادر
    ''' </summary>
    Public Function UpdateArchiveFromSader(saderId As Integer) As Boolean
        Try
            ' الحصول على بيانات الصادر
            Dim saderData As DataTable = GetSaderById(saderId)
            If saderData.Rows.Count = 0 Then
                Return False
            End If

            ' الحصول على ArchiveID
            Dim archiveID As Integer = If(saderData.Rows(0)("ArchiveID") IsNot DBNull.Value,
                                    Convert.ToInt32(saderData.Rows(0)("ArchiveID")), 0)

            ' إذا لم يكن هناك أرشيف مرتبط، لا نفعل شيئًا
            If archiveID <= 0 Then
                Return True
            End If

            ' الحصول على بيانات الأرشيف
            Dim archiveData As DataTable = archiveClass.GetArchiveById(archiveID)
            If archiveData.Rows.Count = 0 Then
                Return False
            End If

            ' تحديث حقول الأرشيف بناءً على بيانات الصادر
            Dim archiveNum As String = saderData.Rows(0)("NumberBook").ToString()
            Dim archiveDate As Date = CDate(saderData.Rows(0)("DateBook"))
            Dim archiveSubject As String = saderData.Rows(0)("SubjBook").ToString()
            Dim archiveWillMaster As String = saderData.Rows(0)("WillMaster").ToString()
            Dim archiveWillSub As String = saderData.Rows(0)("WillSub").ToString()

            ' التحقق من وجود الملف
            Dim filePath As String = archiveData.Rows(0)("FilePath").ToString()
            If Not String.IsNullOrEmpty(filePath) AndAlso Not File.Exists(filePath) Then
                ' إذا لم يكن الملف موجودًا، لا نقوم بتحديثه
                MessageBox.Show($"الملف غير موجود: {filePath}", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return True
            End If

            ' تحديث الأرشيف
            Dim success As Boolean = archiveClass.UpdateArchive(
            archiveID,
            archiveNum,
            archiveSubject,
            archiveData.Rows(0)("ArchiveFolder").ToString(),
            archiveDate,
            archiveWillMaster,
            archiveWillSub,
            archiveData.Rows(0)("ArchiveNotes").ToString(),
            archiveData.Rows(0)("FileName").ToString(),
            archiveData.Rows(0)("FileType").ToString(),
            archiveData.Rows(0)("FilePath").ToString(),
            archiveData.Rows(0)("OriginalFilePath").ToString(),
            archiveData.Rows(0)("UserName").ToString()
        )
            Dim archiveForm As ArchiveForm = TryCast(Application.OpenForms("archiveForm"), ArchiveForm)
            If archiveForm IsNot Nothing Then
                archiveForm.LoadActiveArchive()
            End If

            Return success
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateArchiveFromSader: {ex.Message}")
            Return False
        End Try
    End Function

#End Region

#Region "📊 Counters & Utilities"

    ''' <summary>
    ''' الحصول على عدد السجلات غير المؤرشفة
    ''' </summary>
    Public Function GetUnarchivedSaderCount() As Integer
        Try
            Dim query As String = "SELECT COUNT(*) FROM TbSader WHERE ArchiveID IS NULL OR ArchiveID = 0"
            Dim result = DatabaseHelper.ExecuteScalar(query)
            Return Convert.ToInt32(result)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetUnarchivedSaderCount: {ex.Message}")
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' تحديث معرف الأرشيف لصادر محدد
    ''' </summary>
    Public Function UpdateArchiveIDByNumberBook(numberBook As String, archiveID As Integer) As Boolean
        Try
            If String.IsNullOrWhiteSpace(numberBook) OrElse archiveID <= 0 Then
                Return False
            End If

            Dim query As String = "UPDATE TbSader SET ArchiveID = @archiveID WHERE NumberBook = @numberBook AND (ArchiveID IS NULL OR ArchiveID = 0)"

            Using connection As SqlConnection = GetConnection()
                connection.Open()

                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@archiveID", archiveID)
                    command.Parameters.AddWithValue("@numberBook", numberBook.Trim())

                    Dim rowsAffected As Integer = command.ExecuteNonQuery()

                    ' تحديث عداد السجلات غير المرتبطة إذا تم تحديث سجل واحد على الأقل
                    If rowsAffected > 0 Then
                        ' تحديث العداد في نظام DataHelper
                        DataHelper.UpdateUnarchivedSaderCount()
                    End If

                    Return rowsAffected > 0
                End Using
            End Using

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateArchiveIDByNumberBook: {ex.Message}")
            Return False
        End Try
    End Function


    Private Function GetConnection() As SqlConnection
        Return DatabaseHelper.GetConnection()
    End Function

    Public Function GetNextBookNumber() As String
        Try
            ' استخدام الإجراء المخزن "GetNextSaderNumber"
            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("GetNextSaderNumber")

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim nextNumber As String = dt.Rows(0)("NextNumber").ToString()
                ' حفظ الرقم الجديد في إعدادات النظام
                'My.Settings.LastSaderNumber = nextNumber
                'My.Settings.Save()
                Return nextNumber
            Else
                ' إذا فشل الاستعلام أو لم يُعثر على نتيجة صالحة، استخدم الرقم الافتراضي واحفظه أيضًا
                Dim defaultNumber As String = "251230000"
                My.Settings.LastSaderNumber = defaultNumber
                My.Settings.Save()
                Return defaultNumber
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetNextBookNumber: {ex.Message}")
            ' في حالة حدوث خطأ، استخدم الرقم الافتراضي وحاول حفظه
            Dim fallbackNumber As String = "251230000"
            My.Settings.LastSaderNumber = fallbackNumber
            My.Settings.Save()
            Return fallbackNumber
        End Try
    End Function

    Public Function IsYearClosed(year As Integer) As Boolean

        Dim query As String = "SELECT IsClosed FROM TbYears WHERE Year = @Year"

        Dim parameters() As SqlParameter = {
        DatabaseHelper.CreateParameter("@Year", year)
    }

        Dim result = DatabaseHelper.ExecuteScalar(query, parameters)

        If result IsNot Nothing Then
            Return Convert.ToBoolean(result)
        End If

        Return False

    End Function

    Public Function IsBookNumberExists(numberBook As Integer, year As Integer, currentId As Integer) As Boolean

        Dim query As String =
    "SELECT COUNT(*) FROM TbSader WHERE NumberBook = @NumberBook AND Year = @Year AND ID <> @ID"

        Dim parameters() As SqlParameter = {
        DatabaseHelper.CreateParameter("@NumberBook", numberBook),
        DatabaseHelper.CreateParameter("@Year", year),
        DatabaseHelper.CreateParameter("@ID", currentId)
    }

        Dim count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))

        Return count > 0

    End Function

    Public Function GetSaderById(id As Integer) As DataTable
        Try
            ' التحقق من صحة المعرف
            If id <= 0 Then
                Return Nothing
            End If

            Dim query As String = "SELECT * FROM TbSader WHERE ID = @ID"
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@ID", id)
            }

            Dim dt As DataTable = DatabaseHelper.ExecuteQuery(query, parameters)

            ' التحقق من أن البيانات موجودة
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetSaderById: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ' دالة لجلب جميع بيانات الصادر
    Public Function GetAllSader() As DataTable
        Try
            Dim query As String = "SELECT * FROM TbSader ORDER BY DateBook , ID "
            Return DatabaseHelper.ExecuteQuery(query)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetAllSader: {ex.Message}")
            Return New DataTable()
        End Try
    End Function

    Public Sub AutoCompleteForTextBox(fieldName As String, tableName As String, textBox As TextBox)
        Try
            Dim query As String = $"SELECT DISTINCT {fieldName} FROM {tableName} WHERE {fieldName} IS NOT NULL AND {fieldName} <> '' ORDER BY {fieldName}"
            Dim dt As DataTable = DatabaseHelper.ExecuteQuery(query)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim source As New AutoCompleteStringCollection()
                For Each row As DataRow In dt.Rows
                    source.Add(row(fieldName).ToString())
                Next
                textBox.AutoCompleteCustomSource = source
                textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                textBox.AutoCompleteSource = AutoCompleteSource.CustomSource
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في AutoCompleteForTextBox: {ex.Message}")
        End Try
    End Sub

    Private Function GetArchiveIDBySaderID(saderID As Integer) As Integer
        Try
            ' التحقق من وجود العمود أولاً
            Dim columnExists As Boolean = CheckIfColumnExists("TbSader", "ArchiveID")

            If Not columnExists Then
                ' إذا لم يكن العمود موجودًا، نحاول إنشاءه
                Try
                    Using connection As SqlConnection = GetConnection()
                        connection.Open()
                        Dim alterQuery As String = "ALTER TABLE TbSader ADD ArchiveID INT NULL"
                        Using command As New SqlCommand(alterQuery, connection)
                            command.ExecuteNonQuery()

                            ' إضافة مفتاح خارجي
                            Dim fkQuery As String = "ALTER TABLE TbSader ADD CONSTRAINT FK_TbSader_ArchiveID FOREIGN KEY (ArchiveID) REFERENCES TbArchive(ID)"
                            Using fkCommand As New SqlCommand(fkQuery, connection)
                                fkCommand.ExecuteNonQuery()
                            End Using
                        End Using
                    End Using
                Catch ex As Exception
                    System.Diagnostics.Debug.WriteLine($"فشل في إنشاء عمود ArchiveID: {ex.Message}")
                    Return 0
                End Try
            End If

            Dim query As String = "SELECT ArchiveID FROM TbSader WHERE ID = @ID"
            Using connection As SqlConnection = GetConnection()
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ID", saderID)
                    connection.Open()
                    Dim result As Object = command.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) AndAlso Convert.ToInt32(result) > 0 Then
                        Return Convert.ToInt32(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetArchiveIDBySaderID: {ex.Message}")
        End Try
        Return 0
    End Function

    Private Function CheckIfColumnExists(tableName As String, columnName As String) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName"
            Using connection As SqlConnection = GetConnection()
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@TableName", tableName)
                    command.Parameters.AddWithValue("@ColumnName", columnName)
                    connection.Open()
                    Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من وجود العمود: {ex.Message}")
            Return False
        End Try
    End Function

    Private Function GetCurrentFilePathByArchiveID(archiveID As Integer) As String
        Try
            Dim query As String = "SELECT FilePath FROM TbArchive WHERE ID = @ID"
            Using connection As SqlConnection = GetConnection()
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ID", archiveID)
                    connection.Open()
                    Dim result As Object = command.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        Return result.ToString()
                    End If
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetCurrentFilePathByArchiveID: {ex.Message}")
        End Try
        Return String.Empty
    End Function

    ' دالة لنقل الملف إلى مجلد المحذوفات في حالة فشل الحذف
    Private Sub MoveToDeletedFolder(filePath As String)
        Try
            Dim deletedFolderPath As String = IO.Path.Combine(My.Settings.ARCHIVE_DEFAULT_PATH, "مجلد المحذوفات")
            If Not IO.Directory.Exists(deletedFolderPath) Then
                Directory.CreateDirectory(deletedFolderPath)
            End If

            Dim fileName As String = Path.GetFileName(filePath)
            Dim newFilePath As String = Path.Combine(deletedFolderPath, $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(fileName)}")
            File.Move(filePath, newFilePath)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"فشل في نقل الملف إلى مجلد المحذوفات: {ex.Message}")
        End Try
    End Sub

    Public Sub RefreshData()
        ' تشغيل الحدث عند تحديث البيانات
        SaderEvents.RaiseSaderDataChanged(SaderDataAction.Refreshed, 0, Nothing)
    End Sub

    Public Function AddSubjBook(subjBook As String) As Boolean
        Try
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@Subject_Book", subjBook)
            }

            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("AddSubjBook", parameters)
            Return rowsAffected > 0
        Catch ex As Exception
            Throw New Exception("خطأ في إضافة موضوع جديد: " & ex.Message)
        End Try
    End Function

#End Region

End Class