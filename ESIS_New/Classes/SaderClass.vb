Imports System.Data.SqlClient
Imports System.IO

Public Class SaderClass

    Private activityLogClass As New ActivityLogClass()
    Private archiveClass As New ArchiveClass


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


    'Public Function UpdateArchiveIDByNumberBook(numberBook As String, archiveID As Integer) As Boolean
    '    Try
    '        If String.IsNullOrWhiteSpace(numberBook) OrElse archiveID <= 0 Then
    '            Return False
    '        End If

    '        Dim query As String = "UPDATE TbSader SET ArchiveID = @archiveID WHERE NumberBook = @numberBook"

    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()

    '            Using command As New SqlCommand(query, connection)
    '                command.Parameters.AddWithValue("@archiveID", archiveID)
    '                command.Parameters.AddWithValue("@numberBook", numberBook.Trim())

    '                Return command.ExecuteNonQuery() > 0
    '            End Using
    '        End Using

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في UpdateArchiveIDByNumberBook: {ex.Message}")
    '        Return False
    '    End Try
    'End Function


    ' دالة للحصول على اتصال قاعدة البيانات
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
                My.Settings.LastSaderNumber = nextNumber
                My.Settings.Save()
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



    'Public Function GetNextBookNumber() As String
    '    Try
    '        Dim query As String = "SELECT ISNULL(MAX(CAST(NumberBook AS BIGINT)), 251229999) + 1 FROM TbSader WHERE CAST(NumberBook AS BIGINT) >= 251230000"
    '        Dim result As Object = DatabaseHelper.ExecuteScalar(query)

    '        If result IsNot Nothing AndAlso Not IsDBNull(result) AndAlso IsNumeric(result) Then
    '            Dim nextNumber As String = result.ToString()
    '            ' حفظ الرقم الجديد في إعدادات النظام
    '            My.Settings.LastSaderNumber = nextNumber
    '            My.Settings.Save() ' ضروري لحفظ التغييرات على القرص
    '            Return nextNumber
    '        Else
    '            ' إذا فشل الاستعلام أو لم يُعثر على نتيجة صالحة، استخدم الرقم الافتراضي واحفظه أيضًا
    '            Dim defaultNumber As String = "251230000"
    '            My.Settings.LastSaderNumber = defaultNumber
    '            My.Settings.Save()
    '            Return defaultNumber
    '        End If
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في GetNextBookNumber: {ex.Message}")
    '        ' في حالة حدوث خطأ، استخدم الرقم الافتراضي وحاول حفظه
    '        Dim fallbackNumber As String = "251230000"
    '        My.Settings.LastSaderNumber = fallbackNumber
    '        My.Settings.Save()
    '        Return fallbackNumber
    '    End Try
    'End Function

    '' دالة للحصول على رقم الكتاب التالي
    'Public Function GetNextBookNumber() As String
    '    Try
    '        Dim query As String = "SELECT ISNULL(MAX(CAST(NumberBook AS BIGINT)), 251229999) + 1 FROM TbSader WHERE CAST(NumberBook AS BIGINT) >= 251230000"
    '        Dim result As Object = DatabaseHelper.ExecuteScalar(query)

    '        If result IsNot Nothing AndAlso Not IsDBNull(result) AndAlso IsNumeric(result) Then
    '            Return result.ToString()
    '        Else
    '            Return "251230000"
    '        End If
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في GetNextBookNumber: {ex.Message}")
    '        Return "251230000"
    '    End Try
    'End Function

    ' دالة للتحقق من وجود رقم كتاب
    Public Function IsBookNumberExists(bookNumber As String, Optional excludeId As Integer = 0) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM TbSader WHERE NumberBook = @NumberBook"
            If excludeId > 0 Then
                query &= " AND ID <> @ExcludeID"
            End If

            Dim parameters() As SqlParameter
            If excludeId > 0 Then
                parameters = {
                    DatabaseHelper.CreateParameter("@NumberBook", bookNumber),
                    DatabaseHelper.CreateParameter("@ExcludeID", excludeId)
                }
            Else
                parameters = {
                    DatabaseHelper.CreateParameter("@NumberBook", bookNumber)
                }
            End If

            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
            Return count > 0
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في IsBookNumberExists: {ex.Message}")
            Return False
        End Try
    End Function

    ' دالة للبحث عن بيانات الصادر بواسطة المعرف
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

    '' دالة للبحث عن بيانات الصادر بواسطة مصطلح بحث
    'Public Function SearchSader(searchTerm As String, searchColumn As String) As DataTable
    '    Try
    '        ' التحقق من صحة المعلمات
    '        If String.IsNullOrWhiteSpace(searchTerm) OrElse String.IsNullOrWhiteSpace(searchColumn) Then
    '            Return GetAllSader()
    '        End If

    '        ' تحقق من أن العمود المطلوب موجود
    '        Dim validColumns As New List(Of String) From {"NumberBook", "SubjBook", "WillMaster", "WillSub"}
    '        If Not validColumns.Contains(searchColumn) Then
    '            Return GetAllSader()
    '        End If

    '        Dim query As String = "SELECT * FROM TbSader WHERE " & searchColumn & " LIKE @SearchTerm ORDER BY DateBook , ID "
    '        Dim parameters() As SqlParameter = {
    '            DatabaseHelper.CreateParameter("@SearchTerm", "%" & searchTerm & "%")
    '        }

    '        Return DatabaseHelper.ExecuteQuery(query, parameters)
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في SearchSader: {ex.Message}")
    '        Return New DataTable()
    '    End Try
    'End Function

    ' دالة للبحث عن بيانات الصادر بواسطة نطاق تاريخ

    ''' <summary>
    ''' البحث في الصادرة
    ''' </summary>
    Public Function SearchSader(searchTerm As String, searchColumn As String) As DataTable
        Try
            ' التحقق من صحة المعلمات
            If String.IsNullOrWhiteSpace(searchTerm) OrElse String.IsNullOrWhiteSpace(searchColumn) Then
                Return GetAllSader()
            End If

            ' تحقق من أن العمود المطلوب موجود
            Dim validColumns As New List(Of String) From {"NumberBook", "SubjBook", "WillMaster", "WillSub"}
            If Not validColumns.Contains(searchColumn) Then
                Return GetAllSader()
            End If

            ' استخدام الإجراء المخزن "SearchSader"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@SearchTerm", searchTerm)
        }

            ' تنفيذ الإجراء المخزن وإرجاع النتائج
            Return DatabaseHelper.ExecuteStoredProcedure("SearchSader", parameters)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في SearchSader: {ex.Message}")
            Return New DataTable()
        End Try
    End Function

    ''' <summary>
    ''' البحث في الصادرة حسب النطاق الزمني
    ''' </summary>
    Public Function SearchSaderByDateRange(startDate As Date, endDate As Date) As DataTable
        Try
            ' استخدام الإجراء المخزن "SearchSaderByDateRange"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@StartDate", startDate),
            DatabaseHelper.CreateParameter("@EndDate", endDate)
        }

            ' تنفيذ الإجراء المخزن وإرجاع النتائج
            Return DatabaseHelper.ExecuteStoredProcedure("SearchSaderByDateRange", parameters)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في SearchSaderByDateRange: {ex.Message}")
            Return New DataTable()
        End Try
    End Function

    ''' <summary>
    ''' البحث عن الصادرة غير المرتبطة بالأرشيف
    ''' </summary>
    Public Function SearchUnarchivedSader(searchTerm As String) As DataTable
        Try
            ' استخدام الإجراء المخزن "SearchSader"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@SearchTerm", searchTerm)
        }

            ' تنفيذ الإجراء المخزن وإرجاع النتائج
            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("SearchSader", parameters)

            ' تصفية النتائج لعرض السجلات غير المرتبطة بالأرشيف (ArchiveID IS NULL)
            Dim filteredDt As DataTable = dt.Clone()

            For Each row As DataRow In dt.Rows
                If row("ArchiveID") Is DBNull.Value OrElse Convert.ToInt32(row("ArchiveID")) = 0 Then
                    filteredDt.ImportRow(row)
                End If
            Next

            Return filteredDt

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في SearchUnarchivedSader: {ex.Message}")
            Return New DataTable()
        End Try
    End Function

    'Public Function SearchSaderByDateRange(startDate As Date, endDate As Date) As DataTable
    '    Try
    '        Dim query As String = "SELECT * FROM TbSader WHERE DateBook BETWEEN @StartDate AND @EndDate ORDER BY DateBook DESC, ID DESC"
    '        Dim parameters() As SqlParameter = {
    '            DatabaseHelper.CreateParameter("@StartDate", startDate.Date),
    '            DatabaseHelper.CreateParameter("@EndDate", endDate.Date)
    '        }

    '        Return DatabaseHelper.ExecuteQuery(query, parameters)
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في SearchSaderByDateRange: {ex.Message}")
    '        Return New DataTable()
    '    End Try
    'End Function

    ' دالة لملء مربع النص بميزة الإكمال التلقائي
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


#Region " إضافة -  تعديل - حذف"

    ''' <summary>
    ''' إضافة صادر جديد وإرجاع معرفه
    ''' </summary>
    Public Function AddSader(dateBook As Date, subjBook As String, willMaster As String, willSub As String, bookCategory As String, userName As String) As Integer
        Try
            ' التحقق من صحة البيانات
            If String.IsNullOrWhiteSpace(subjBook) OrElse String.IsNullOrWhiteSpace(userName) Then
                Return 0
            End If

            ' استخدام الإجراء المخزن "AddSader"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@DateBook", dateBook),
            DatabaseHelper.CreateParameter("@SubjBook", subjBook.Trim()),
            DatabaseHelper.CreateParameter("@WillMaster", If(String.IsNullOrWhiteSpace(willMaster), DBNull.Value, willMaster.Trim())),
            DatabaseHelper.CreateParameter("@WillSub", If(String.IsNullOrWhiteSpace(willSub), DBNull.Value, willSub.Trim())),
            DatabaseHelper.CreateParameter("@bookCategory", bookCategory.Trim()),
            DatabaseHelper.CreateParameter("@UserName", userName.Trim()),
            DatabaseHelper.CreateParameter("@ArchiveID", DBNull.Value) ' افتراضيًا لا يوجد أرشيف مرتبط
        }

            ' تنفيذ الإجراء المخزن واحصل على رقم الكتاب الجديد
            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("AddSader", parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim numberBook As String = dt.Rows(0)("NumberBook").ToString()
                Dim newId As Integer = GetSaderIdByNumberBook(numberBook)

                ' تحديث عداد الصادرة
                DataHelper.UpdateSaderCount()

                ' تحديث عداد السجلات غير المرتبطة (يزداد العدد)
                DataHelper.UpdateUnarchivedSaderCount()

                ' ✅ إضافة تسجيل النشاط بعد نجاح الإضافة
                Try
                    activityLogClass.LogUserActivity(userName, "Add", "TbSader", newId, $"تمت إضافة كتاب صادر جديد برقم: {numberBook}, الموضوع: {subjBook}")
                Catch logEx As Exception
                    System.Diagnostics.Debug.WriteLine($"فشل في تسجيل نشاط المستخدم: {logEx.Message}")
                End Try

                Return newId
            Else
                activityLogClass.LogUserActivity(userName, "Failed_Add", "TbSader", -1, $"فشل في إضافة كتاب صادر. الموضوع: {subjBook}. السبب: استرجاع رقم كتاب غير صالح")
                Return 0
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في AddSader: {ex.Message}")
            Return 0
        End Try
    End Function

    Public Function GetSaderIdByNumberBook(numberBook As String) As Integer
        Dim query As String = "SELECT ID FROM TbSader WHERE NumberBook = @NumberBook"
        Dim parameters() As SqlParameter = {
        DatabaseHelper.CreateParameter("@NumberBook", numberBook)
    }
        Try
            Dim result = DatabaseHelper.ExecuteScalar(query, parameters)
            If result IsNot Nothing AndAlso IsNumeric(result) Then
                Return Convert.ToInt32(result)
            Else
                Return 0 ' أو -1 ليعني عدم العثور عليه
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetSaderIdByNumberBook: {ex.Message}")
            Return 0
        End Try
    End Function

    '''' <summary>
    '''' إضافة صادر جديد وإرجاع معرفه
    '''' </summary>
    'Public Function AddSader(dateBook As Date, subjBook As String, willMaster As String, willSub As String, bookCategory As String, userName As String) As Integer
    '    Try
    '        ' التحقق من صحة البيانات
    '        If String.IsNullOrWhiteSpace(subjBook) OrElse String.IsNullOrWhiteSpace(userName) Then
    '            Return 0
    '        End If

    '        ' الحصول على رقم الكتاب التالي
    '        Dim numberBook As String = GetNextBookNumber()
    '        If String.IsNullOrWhiteSpace(numberBook) Then
    '            Return 0
    '        End If

    '        ' تعديل الاستعلام لإرجاع المعرف الجديد
    '        Dim query As String = "INSERT INTO TbSader (NumberBook, DateBook, SubjBook, WillMaster, WillSub,bookCategory, UserName) " &
    '                              "VALUES (@numberBook, @dateBook, @subjBook, @willMaster, @willSub,@bookCategory, @userName); " &
    '                              "SELECT SCOPE_IDENTITY();"

    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()

    '            Using command As New SqlCommand(query, connection)
    '                ' إعداد المعلمات
    '                command.Parameters.AddWithValue("@numberBook", numberBook)
    '                command.Parameters.AddWithValue("@dateBook", dateBook.Date)
    '                command.Parameters.AddWithValue("@subjBook", subjBook.Trim())
    '                command.Parameters.AddWithValue("@willMaster", If(String.IsNullOrWhiteSpace(willMaster), DBNull.Value, willMaster.Trim()))
    '                command.Parameters.AddWithValue("@willSub", If(String.IsNullOrWhiteSpace(willSub), DBNull.Value, willSub.Trim()))
    '                command.Parameters.AddWithValue("@bookCategory", bookCategory.Trim())
    '                command.Parameters.AddWithValue("@userName", userName.Trim())

    '                ' الحصول على المعرف الجديد
    '                Dim newIdObj As Object = command.ExecuteScalar()

    '                ' التحقق من صحة النتيجة
    '                If newIdObj IsNot Nothing AndAlso IsNumeric(newIdObj) Then
    '                    Dim newSaderId As Integer = Convert.ToInt32(newIdObj)

    '                    ' تحديث عداد الصادرة
    '                    DataHelper.UpdateSaderCount()

    '                    ' تحديث عداد السجلات غير المرتبطة (يزداد العدد)
    '                    DataHelper.UpdateUnarchivedSaderCount()

    '                    ' ✅ إضافة تسجيل النشاط بعد نجاح الإضافة
    '                    Try

    '                        ' تسجيل نشاط إضافة الصادر
    '                        activityLogClass.LogUserActivity(userName, "Add", "TbSader", newSaderId, $"تمت إضافة كتاب صادر جديد برقم: {numberBook}, الموضوع: {subjBook}")
    '                    Catch logEx As Exception
    '                        ' لا نوقف العملية الرئيسية إذا فشل تسجيل النشاط
    '                        System.Diagnostics.Debug.WriteLine($"فشل في تسجيل نشاط المستخدم: {logEx.Message}")
    '                    End Try

    '                    Return newSaderId
    '                Else
    '                    activityLogClass.LogUserActivity(userName, "Failed_Add", "TbSader", -1, $"فشل في إضافة كتاب صادر. الموضوع: {subjBook}. السبب: استرجاع معرف غير صالح")
    '                    Return 0
    '                End If
    '            End Using
    '        End Using

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في AddSader: {ex.Message}")
    '        Return 0
    '    End Try
    'End Function

    '''' <summary>
    '''' تحديث صادر
    '''' </summary>
    'Public Function UpdateSader(id As Integer, numberBook As String, dateBook As Date, subjBook As String, willMaster As String, willSub As String, userName As String) As Boolean
    '    Try
    '        ' التحقق من صحة البيانات
    '        If id <= 0 OrElse String.IsNullOrWhiteSpace(numberBook) OrElse String.IsNullOrWhiteSpace(subjBook) OrElse String.IsNullOrWhiteSpace(userName) Then
    '            Return False
    '        End If

    '        ' التحقق من تكرار رقم الكتاب (باستثناء السجل الحالي)
    '        If IsBookNumberExists(numberBook, id) Then
    '            Return False
    '        End If

    '        Dim query As String = "UPDATE TbSader SET " &
    '                              "NumberBook = @numberBook, " &
    '                              "DateBook = @dateBook, " &
    '                              "SubjBook = @subjBook, " &
    '                              "WillMaster = @willMaster, " &
    '                              "WillSub = @willSub, " &
    '                              "UserName = @userName " &
    '                              "WHERE ID = @id"

    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()

    '            Using command As New SqlCommand(query, connection)
    '                ' إعداد المعلمات
    '                command.Parameters.AddWithValue("@id", id)
    '                command.Parameters.AddWithValue("@numberBook", numberBook)
    '                command.Parameters.AddWithValue("@dateBook", dateBook.Date)
    '                command.Parameters.AddWithValue("@subjBook", subjBook.Trim())
    '                command.Parameters.AddWithValue("@willMaster", If(String.IsNullOrWhiteSpace(willMaster), DBNull.Value, willMaster.Trim()))
    '                command.Parameters.AddWithValue("@willSub", If(String.IsNullOrWhiteSpace(willSub), DBNull.Value, willSub.Trim()))
    '                command.Parameters.AddWithValue("@userName", userName.Trim())

    '                ' تنفيذ الاستعلام
    '                Return command.ExecuteNonQuery() > 0

    '                ' تسجيل نشاط إضافة الصادر
    '                activityLogClass.LogUserActivity(userName, "Edit", "TbSader", id, $"تمت إضافة كتاب صادر جديد برقم: {numberBook}, الموضوع: {subjBook}")


    '            End Using
    '        End Using

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في UpdateSader: {ex.Message}")
    '        Return False
    '    End Try
    'End Function

    ''' <summary>
    ''' تحديث صادر
    ''' </summary>
    Public Function UpdateSader(id As Integer, numberBook As String, dateBook As Date, subjBook As String, willMaster As String, willSub As String, bookCategory As String, userName As String, ArchiveID As String) As Boolean
        Try
            ' التحقق من صحة البيانات
            If id <= 0 OrElse String.IsNullOrWhiteSpace(numberBook) OrElse String.IsNullOrWhiteSpace(subjBook) OrElse String.IsNullOrWhiteSpace(userName) Then
                Return False
            End If

            ' التحقق من تكرار رقم الكتاب (باستثناء السجل الحالي)
            If IsBookNumberExists(numberBook, id) Then
                Return False
            End If

            ' استخدام الإجراء المخزن "UpdateSader"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@ID", id),
            DatabaseHelper.CreateParameter("@NumberBook", numberBook),
            DatabaseHelper.CreateParameter("@DateBook", dateBook.Date),
            DatabaseHelper.CreateParameter("@SubjBook", subjBook.Trim()),
            DatabaseHelper.CreateParameter("@WillMaster", If(String.IsNullOrWhiteSpace(willMaster), DBNull.Value, willMaster.Trim())),
            DatabaseHelper.CreateParameter("@WillSub", If(String.IsNullOrWhiteSpace(willSub), DBNull.Value, willSub.Trim())),
            DatabaseHelper.CreateParameter("@bookCategory", bookCategory.Trim()),
            DatabaseHelper.CreateParameter("@UserName", userName.Trim()),
            DatabaseHelper.CreateParameter("@ArchiveID", If(String.IsNullOrWhiteSpace(ArchiveID), DBNull.Value, ArchiveID.Trim())) ' يمكن تعديله إذا كان هناك أرشيف مرتبط
        }

            ' تنفيذ الإجراء المخزن واحصل على عدد الصفوف المؤثرة
            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("UpdateSader", parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim rowsAffected As Integer = Convert.ToInt32(dt.Rows(0)("RowsAffected"))

                ' ✅ إذا تم تحديث السجل، قم بتحديث الأرشيف المرتبط
                If rowsAffected > 0 Then
                    ' تحديث الأرشيف المرتبط
                    Dim archiveUpdated As Boolean = UpdateArchiveFromSader(id)
                    If Not archiveUpdated Then
                        System.Diagnostics.Debug.WriteLine($"فشل في تحديث الأرشيف المرتبط بمعرف الصادر: {id}")
                    End If
                End If

                ' تسجيل نشاط إضافة الصادر
                activityLogClass.LogUserActivity(userName, "Edit", "TbSader", id, $"تمت إضافة كتاب صادر جديد برقم: {numberBook}, الموضوع: {subjBook}")

                Return rowsAffected > 0
            Else
                Return False
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateSader: {ex.Message}")
            Return False
        End Try
    End Function


    '''' <summary>
    '''' تحديث صادر
    '''' </summary>
    'Public Function UpdateSader(id As Integer, numberBook As String, dateBook As Date, subjBook As String, willMaster As String, willSub As String, bookCategory As String, userName As String) As Boolean
    '    Try
    '        ' التحقق من صحة البيانات
    '        If id <= 0 OrElse String.IsNullOrWhiteSpace(numberBook) OrElse String.IsNullOrWhiteSpace(subjBook) OrElse String.IsNullOrWhiteSpace(userName) Then
    '            Return False
    '        End If

    '        ' التحقق من تكرار رقم الكتاب (باستثناء السجل الحالي)
    '        If IsBookNumberExists(numberBook, id) Then
    '            Return False
    '        End If

    '        Dim query As String = "UPDATE TbSader SET " &
    '                          "NumberBook = @numberBook, " &
    '                          "DateBook = @dateBook, " &
    '                          "SubjBook = @subjBook, " &
    '                          "WillMaster = @willMaster, " &
    '                          "WillSub = @willSub, " &
    '                          "bookCategory = @bookCategory, " &
    '                          "UserName = @userName " &
    '                          "WHERE ID = @id"

    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()

    '            Using command As New SqlCommand(query, connection)
    '                ' إعداد المعلمات
    '                command.Parameters.AddWithValue("@id", id)
    '                command.Parameters.AddWithValue("@numberBook", numberBook)
    '                command.Parameters.AddWithValue("@dateBook", dateBook.Date)
    '                command.Parameters.AddWithValue("@subjBook", subjBook.Trim())
    '                command.Parameters.AddWithValue("@willMaster", If(String.IsNullOrWhiteSpace(willMaster), DBNull.Value, willMaster.Trim()))
    '                command.Parameters.AddWithValue("@willSub", If(String.IsNullOrWhiteSpace(willSub), DBNull.Value, willSub.Trim()))
    '                command.Parameters.AddWithValue("@bookCategory", bookCategory.Trim())
    '                command.Parameters.AddWithValue("@userName", userName.Trim())

    '                ' تنفيذ الاستعلام
    '                Dim rowsAffected As Integer = command.ExecuteNonQuery()

    '                ' ✅ إذا تم تحديث السجل، قم بتحديث الأرشيف المرتبط
    '                If rowsAffected > 0 Then
    '                    ' تحديث الأرشيف المرتبط
    '                    Dim archiveUpdated As Boolean = UpdateArchiveFromSader(id)
    '                    If Not archiveUpdated Then
    '                        ' يمكنك اختيار عدم إرجاع الخطأ هنا، أو تسجيله فقط
    '                        System.Diagnostics.Debug.WriteLine($"فشل في تحديث الأرشيف المرتبط بمعرف الصادر: {id}")
    '                    End If
    '                End If

    '                ' تسجيل نشاط إضافة الصادر
    '                activityLogClass.LogUserActivity(userName, "Edit", "TbSader", id, $"تمت إضافة كتاب صادر جديد برقم: {numberBook}, الموضوع: {subjBook}")

    '                Return rowsAffected > 0
    '            End Using
    '        End Using

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في UpdateSader: {ex.Message}")
    '        Return False
    '    End Try
    'End Function

    ''' <summary>
    ''' حذف صادر
    ''' </summary>
    Public Function DeleteSader(id As Integer) As Boolean
        Dim transaction As SqlTransaction = Nothing
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                transaction = connection.BeginTransaction()

                Try
                    ' الحصول على ArchiveID قبل الحذف
                    Dim saderData As DataTable = GetSaderById(id)
                    Dim archiveID As Integer = GetArchiveIDBySaderID(id)
                    Dim currentFilePath As String = ""
                    Dim numberBook As String = ""
                    Dim subjBook As String = ""

                    ' استخراج البيانات إذا كانت موجودة
                    If saderData IsNot Nothing AndAlso saderData.Rows.Count > 0 Then
                        numberBook = saderData.Rows(0)("NumberBook").ToString()
                        subjBook = saderData.Rows(0)("SubjBook").ToString()
                        archiveID = If(saderData.Rows(0)("ArchiveID") IsNot DBNull.Value,
                                Convert.ToInt32(saderData.Rows(0)("ArchiveID")), 0)
                    End If

                    ' إذا كان هناك سجل مرتبط في الأرشيف
                    If archiveID > 0 Then
                        ' الحصول على مسار الملف أولاً
                        currentFilePath = GetCurrentFilePathByArchiveID(archiveID)

                        ' حذف الملف من المسار الحالي أولاً
                        If Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath) Then
                            Try
                                MoveToDeletedFolder(currentFilePath)
                            Catch ex As Exception
                                MoveToDeletedFolder(currentFilePath)
                            End Try
                        End If
                    End If

                    ' استخدام الإجراء المخزن "DeleteSader"
                    Dim parameters() As SqlParameter = {
                    DatabaseHelper.CreateParameter("@ID", id)
                }

                    ' تنفيذ الإجراء المخزن واحصل على عدد الصفوف المؤثرة
                    Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("DeleteSader", parameters)

                    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                        Dim rowsAffected As Integer = Convert.ToInt32(dt.Rows(0)("RowsAffected"))

                        ' إذا كان هناك سجل مرتبط في الأرشيف
                        If archiveID > 0 Then
                            ' حذف السجل من جدول الأرشيف
                            Dim deleteArchiveQuery As String = "DELETE FROM TbArchive WHERE ID = @ArchiveID"
                            Using deleteArchiveCommand As New SqlCommand(deleteArchiveQuery, connection, transaction)
                                deleteArchiveCommand.Parameters.AddWithValue("@ArchiveID", archiveID)
                                deleteArchiveCommand.ExecuteNonQuery()
                            End Using
                        End If

                        ' تأكيد المعاملة
                        transaction.Commit()
                        ' تحديث عداد الصادرة
                        DataHelper.UpdateSaderCount()
                        ' تسجيل نشاط إضافة الصادر
                        activityLogClass.LogUserActivity(GlobalData.LoggedInUserName, "Delete", "TbSader", id, $"تم حذف كتاب صادر برقم: {numberBook}, الموضوع: {subjBook}" &
                        If(archiveID > 0, $", مع حذف الأرشيف المرتبط (معرّف: {archiveID})", "")
                    )

                        Return rowsAffected > 0
                    Else
                        Return False
                    End If

                Catch ex As Exception
                    ' التراجع عن المعاملة في حالة حدوث خطأ
                    If transaction IsNot Nothing Then
                        transaction.Rollback()
                    End If
                    Throw
                End Try
            End Using
        Catch ex As Exception
            Throw New Exception("خطأ في حذف الصادر: " & ex.Message)
        End Try
    End Function


    '''' <summary>
    '''' حذف صادر
    '''' </summary>
    'Public Function DeleteSader(id As Integer) As Boolean
    '    Dim transaction As SqlTransaction = Nothing
    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            transaction = connection.BeginTransaction()

    '            Try
    '                ' الحصول على ArchiveID قبل الحذف
    '                Dim saderData As DataTable = GetSaderById(id)
    '                Dim archiveID As Integer = GetArchiveIDBySaderID(id)
    '                Dim currentFilePath As String = ""
    '                Dim numberBook As String = ""
    '                Dim subjBook As String = ""

    '                ' استخراج البيانات إذا كانت موجودة
    '                If saderData IsNot Nothing AndAlso saderData.Rows.Count > 0 Then
    '                    numberBook = saderData.Rows(0)("NumberBook").ToString()
    '                    subjBook = saderData.Rows(0)("SubjBook").ToString()
    '                    archiveID = If(saderData.Rows(0)("ArchiveID") IsNot DBNull.Value,
    '                              Convert.ToInt32(saderData.Rows(0)("ArchiveID")), 0)
    '                End If

    '                ' إذا كان هناك سجل مرتبط في الأرشيف
    '                If archiveID > 0 Then
    '                    ' الحصول على مسار الملف أولاً
    '                    currentFilePath = GetCurrentFilePathByArchiveID(archiveID)

    '                    ' حذف الملف من المسار الحالي أولاً
    '                    If Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath) Then
    '                        Try
    '                            'File.Delete(currentFilePath)
    '                            MoveToDeletedFolder(currentFilePath)
    '                        Catch ex As Exception
    '                            MoveToDeletedFolder(currentFilePath)
    '                        End Try
    '                    End If
    '                End If


    '                ' حذف السجل من جدول الصادر
    '                Dim deleteSaderQuery As String = "DELETE FROM TbSader WHERE ID = @ID"
    '                Using deleteSaderCommand As New SqlCommand(deleteSaderQuery, connection, transaction)
    '                    deleteSaderCommand.Parameters.AddWithValue("@ID", id)
    '                    deleteSaderCommand.ExecuteNonQuery()
    '                End Using

    '                ' إذا كان هناك سجل مرتبط في الأرشيف
    '                If archiveID > 0 Then
    '                    ' حذف السجل من جدول الأرشيف
    '                    Dim deleteArchiveQuery As String = "DELETE FROM TbArchive WHERE ID = @ArchiveID"
    '                    Using deleteArchiveCommand As New SqlCommand(deleteArchiveQuery, connection, transaction)
    '                        deleteArchiveCommand.Parameters.AddWithValue("@ArchiveID", archiveID)
    '                        deleteArchiveCommand.ExecuteNonQuery()
    '                    End Using
    '                End If

    '                ' تأكيد المعاملة
    '                transaction.Commit()
    '                ' تحديث عداد الصادرة
    '                DataHelper.UpdateSaderCount()
    '                ' تسجيل نشاط إضافة الصادر

    '                activityLogClass.LogUserActivity(GlobalData.LoggedInUserName, "Delete", "TbSader", id, $"تم حذف كتاب صادر برقم: {numberBook}, الموضوع: {subjBook}" &
    '                    If(archiveID > 0, $", مع حذف الأرشيف المرتبط (معرّف: {archiveID})", "")
    '                )


    '                Return True
    '            Catch ex As Exception
    '                ' التراجع عن المعاملة في حالة حدوث خطأ
    '                If transaction IsNot Nothing Then
    '                    transaction.Rollback()
    '                End If
    '                Throw
    '            End Try
    '        End Using
    '    Catch ex As Exception
    '        Throw New Exception("خطأ في حذف الصادر: " & ex.Message)
    '    End Try
    'End Function





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
            Dim archiveData As DataTable = ArchiveClass.GetArchiveById(archiveID)
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
            Dim success As Boolean = ArchiveClass.UpdateArchive(
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

    'Public Function DeleteSader(id As Integer) As Boolean
    '    Try
    '        ' التحقق من صحة المعرف
    '        If id <= 0 Then
    '            Return False
    '        End If

    '        Dim query As String = "DELETE FROM TbSader WHERE ID = @ID"

    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()

    '            Using command As New SqlCommand(query, connection)
    '                ' إعداد المعلمات
    '                command.Parameters.AddWithValue("@ID", id)

    '                ' تنفيذ الاستعلام
    '                Dim rowsAffected As Integer = command.ExecuteNonQuery()

    '                ' إذا تم الحذف بنجاح، تحديث العدد
    '                If rowsAffected > 0 Then
    '                    ' تحديث عداد الصادرة
    '                    DataHelper.UpdateSaderCount()

    '                    ' التحقق مما إذا كان السجل مرتبطًا بالأرشيف
    '                    Dim archiveID As Integer = GetArchiveIDBySaderID(id)
    '                    If archiveID > 0 Then
    '                        ' إذا كان مرتبطًا، فسيتم حذف الارتباط
    '                        DataHelper.UpdateUnarchivedSaderCount()
    '                    End If
    '                End If

    '                Return rowsAffected > 0
    '            End Using
    '        End Using

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في DeleteSader: {ex.Message}")
    '        Return False
    '    End Try
    'End Function

    'Public Function GetArchiveIDBySaderID(saderID As Integer) As Integer
    '    Try
    '        Dim query As String = "SELECT ArchiveID FROM TbSader WHERE ID = @ID"
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            Using command As New SqlCommand(query, connection)
    '                command.Parameters.AddWithValue("@ID", saderID)
    '                Dim result As Object = command.ExecuteScalar()

    '                ' التحقق من أن النتيجة ليست Nothing أو DBNull
    '                If result IsNot Nothing AndAlso Not IsDBNull(result) AndAlso result IsNot DBNull.Value Then
    '                    Return Convert.ToInt32(result)
    '                End If
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في GetArchiveIDBySaderID: {ex.Message}")
    '    End Try

    '    ' إذا لم يتم العثور على السجل أو كانت قيمة ArchiveID فارغة
    '    Return 0
    'End Function
    'Public Function DeleteSader(id As Integer) As Boolean
    '    Dim transaction As SqlTransaction = Nothing
    '    Try
    '        Using connection As SqlConnection = GetConnection()
    '            connection.Open()
    '            transaction = connection.BeginTransaction()

    '            ' الحصول على ArchiveID قبل الحذف
    '            Dim archiveID As Integer = GetArchiveIDBySaderID(id)

    '            ' حذف السجل من جدول الصادر
    '            Dim deleteSaderQuery As String = "DELETE FROM TbSader WHERE ID = @ID"
    '            Using deleteSaderCommand As New SqlCommand(deleteSaderQuery, connection, transaction)
    '                deleteSaderCommand.Parameters.AddWithValue("@ID", id)
    '                deleteSaderCommand.ExecuteNonQuery()
    '            End Using

    '            ' إذا كان هناك سجل مرتبط في الأرشيف
    '            If archiveID > 0 Then
    '                ' حذف السجل من جدول الأرشيف
    '                Dim deleteArchiveQuery As String = "DELETE FROM TbArchive WHERE ID = @ArchiveID"
    '                Using deleteArchiveCommand As New SqlCommand(deleteArchiveQuery, connection, transaction)
    '                    deleteArchiveCommand.Parameters.AddWithValue("@ArchiveID", archiveID)
    '                    deleteArchiveCommand.ExecuteNonQuery()
    '                End Using

    '                ' الحصول على مسار الملف الحالي (المسار الذي تم نسخه إليه)
    '                Dim currentFilePath As String = GetCurrentFilePathByArchiveID(archiveID)

    '                ' حذف الملف من المسار الحالي فقط (وليس من المسار الأصلي)
    '                If Not String.IsNullOrEmpty(currentFilePath) AndAlso File.Exists(currentFilePath) Then
    '                    Try
    '                        File.Delete(currentFilePath)
    '                    Catch ex As Exception
    '                        ' في حالة فشل الحذف، نقل الملف إلى مجلد المحذوفات
    '                        MoveToDeletedFolder(currentFilePath)
    '                    End Try
    '                End If
    '            End If

    '            ' تأكيد المعاملة
    '            transaction.Commit()
    '            Return True
    '        End Using
    '    Catch ex As Exception
    '        ' التراجع عن المعاملة في حالة حدوث خطأ
    '        If transaction IsNot Nothing Then
    '            transaction.Rollback()
    '        End If
    '        Throw New Exception("خطأ في حذف الصادر: " & ex.Message)
    '    End Try
    'End Function

    ' دالة للحصول على ArchiveID من Sader
    'Private Function GetArchiveIDBySaderID(saderID As Integer) As Integer
    '    Try
    '        Dim query As String = "SELECT ArchiveID FROM TbSader WHERE ID = @ID"
    '        Using connection As SqlConnection = GetConnection()
    '            Using command As New SqlCommand(query, connection)
    '                command.Parameters.AddWithValue("@ID", saderID)
    '                connection.Open()
    '                Dim result As Object = command.ExecuteScalar()
    '                If result IsNot Nothing AndAlso Not IsDBNull(result) AndAlso Convert.ToInt32(result) > 0 Then
    '                    Return Convert.ToInt32(result)
    '                End If
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في GetArchiveIDBySaderID: {ex.Message}")
    '    End Try
    '    Return 0
    'End Function

    ' دالة للحصول على مسار الملف الحالي من جدول الأرشيف

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
            Dim deletedFolderPath As String = IO.Path.Combine(My.Settings.ARCHIVE_Default_PATH, "مجلد المحذوفات")
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


#Region " التقارير "

    ''' <summary>
    ''' الحصول على المرسلين المميزين
    ''' </summary>
    Public Function GetDistinctWillMasters() As List(Of String)
        Dim willMasters As New List(Of String)
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT DISTINCT WillMaster FROM TbSader WHERE WillMaster IS NOT NULL AND WillMaster <> '' ORDER BY WillMaster"
                Using command As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            If Not reader.IsDBNull(0) Then
                                willMasters.Add(reader.GetString(0))
                            End If
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على المرسلين: {ex.Message}")
        End Try
        Return willMasters
    End Function

    ''' <summary>
    ''' الحصول على المرسل إليهم المميزين
    ''' </summary>
    Public Function GetDistinctWillSubs() As List(Of String)
        Dim willSubs As New List(Of String)
        Try
            Using connection As SqlConnection = GetConnection()
                connection.Open()
                Dim query As String = "SELECT DISTINCT WillSub FROM TbSader WHERE WillSub IS NOT NULL AND WillSub <> '' ORDER BY WillSub"
                Using command As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            If Not reader.IsDBNull(0) Then
                                willSubs.Add(reader.GetString(0))
                            End If
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على المرسل إليهم: {ex.Message}")
        End Try
        Return willSubs
    End Function

#End Region

End Class





'Imports System.Data.SqlClient

'Public Class SaderClass

'    ''' <summary>
'    ''' جلب جميع بيانات الصادر مع فحص الصحة
'    ''' </summary>
'    ''' <returns>جدول بيانات منظف وآمن</returns>
'    Public Function GetAllSader() As DataTable
'        Try
'            Dim query As String = "SELECT * FROM TbSader ORDER BY NumberBook "

'            Using connection As SqlConnection = GetConnection()
'                connection.Open()

'                Using adapter As New SqlDataAdapter(query, connection)
'                    Dim dataTable As New DataTable()
'                    adapter.Fill(dataTable)

'                    ' تنظيف وفحص البيانات قبل الإرجاع
'                    Return ValidateAndCleanSaderData(dataTable)
'                End Using
'            End Using

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في GetAllSader: {ex.Message}")
'            Return CreateEmptySaderTable()
'        End Try
'    End Function

'    ''' <summary>
'    ''' البحث في بيانات الصادر مع فحص الصحة
'    ''' </summary>
'    ''' <param name="searchTerm">مصطلح البحث</param>
'    ''' <param name="searchColumn">العمود المراد البحث فيه</param>
'    ''' <returns>جدول بيانات منظف</returns>
'    Public Function SearchSader(searchTerm As String, searchColumn As String) As DataTable
'        Try
'            ' التحقق من صحة المدخلات
'            If String.IsNullOrWhiteSpace(searchTerm) OrElse String.IsNullOrWhiteSpace(searchColumn) Then
'                Return CreateEmptySaderTable()
'            End If

'            ' قائمة الأعمدة المسموح بالبحث فيها (لمنع SQL Injection)
'            Dim allowedColumns As String() = {"NumberBook", "SubjBook", "WillMaster", "WillSub", "UserName"}
'            If Not allowedColumns.Contains(searchColumn) Then
'                System.Diagnostics.Debug.WriteLine($"عمود البحث غير مسموح: {searchColumn}")
'                Return CreateEmptySaderTable()
'            End If

'            Dim query As String = $"SELECT ID, NumberBook, DateBook, SubjBook, WillMaster, WillSub, UserName FROM TbSader WHERE {searchColumn} LIKE @searchTerm ORDER BY ID DESC"

'            Using connection As SqlConnection = GetConnection()
'                connection.Open()

'                Using command As New SqlCommand(query, connection)
'                    command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%")

'                    Using adapter As New SqlDataAdapter(command)
'                        Dim dataTable As New DataTable()
'                        adapter.Fill(dataTable)

'                        Return ValidateAndCleanSaderData(dataTable)
'                    End Using
'                End Using
'            End Using

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في SearchSader: {ex.Message}")
'            Return CreateEmptySaderTable()
'        End Try
'    End Function

'    ''' <summary>
'    ''' البحث بالتاريخ مع فحص الصحة
'    ''' </summary>
'    ''' <param name="startDate">تاريخ البداية</param>
'    ''' <param name="endDate">تاريخ النهاية</param>
'    ''' <returns>جدول بيانات منظف</returns>
'    Public Function SearchSaderByDateRange(startDate As Date, endDate As Date) As DataTable
'        Try
'            ' التأكد من صحة التواريخ
'            If startDate > endDate Then
'                Dim temp As Date = startDate
'                startDate = endDate
'                endDate = temp
'            End If

'            Dim query As String = "SELECT ID, NumberBook, DateBook, SubjBook, WillMaster, WillSub, UserName FROM TbSader WHERE DateBook BETWEEN @startDate AND @endDate ORDER BY DateBook DESC"

'            Using connection As SqlConnection = GetConnection()
'                connection.Open()

'                Using command As New SqlCommand(query, connection)
'                    command.Parameters.AddWithValue("@startDate", startDate.Date)
'                    command.Parameters.AddWithValue("@endDate", endDate.Date.AddDays(1).AddSeconds(-1))

'                    Using adapter As New SqlDataAdapter(command)
'                        Dim dataTable As New DataTable()
'                        adapter.Fill(dataTable)

'                        Return ValidateAndCleanSaderData(dataTable)
'                    End Using
'                End Using
'            End Using

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في SearchSaderByDateRange: {ex.Message}")
'            Return CreateEmptySaderTable()
'        End Try
'    End Function

'    ''' <summary>
'    ''' جلب بيانات صادر محدد بالمعرف
'    ''' </summary>
'    ''' <param name="id">معرف الصادر</param>
'    ''' <returns>جدول بيانات منظف</returns>
'    Public Function GetSaderById(id As Integer) As DataTable
'        Try
'            If id <= 0 Then
'                Return CreateEmptySaderTable()
'            End If

'            Dim query As String = "SELECT * FROM TbSader WHERE ID = @id"

'            Using connection As SqlConnection = GetConnection()
'                connection.Open()

'                Using command As New SqlCommand(query, connection)
'                    command.Parameters.AddWithValue("@id", id)

'                    Using adapter As New SqlDataAdapter(command)
'                        Dim dataTable As New DataTable()
'                        adapter.Fill(dataTable)

'                        Return ValidateAndCleanSaderData(dataTable)
'                    End Using
'                End Using
'            End Using

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في GetSaderById: {ex.Message}")
'            Return CreateEmptySaderTable()
'        End Try
'    End Function

'    ''' <summary>
'    ''' التحقق من وجود رقم كتاب
'    ''' </summary>
'    ''' <param name="numberBook">رقم الكتاب</param>
'    ''' <param name="excludeId">معرف لاستثناؤه من البحث (للتحديث)</param>
'    ''' <returns>True إذا كان الرقم موجود</returns>
'    Public Function IsBookNumberExists(numberBook As String, Optional excludeId As Integer = 0) As Boolean
'        Try
'            If String.IsNullOrWhiteSpace(numberBook) Then
'                Return False
'            End If

'            Dim query As String = "SELECT COUNT(*) FROM TbSader WHERE NumberBook = @numberBook"
'            If excludeId > 0 Then
'                query += " AND ID <> @excludeId"
'            End If

'            Using connection As SqlConnection = GetConnection()
'                connection.Open()

'                Using command As New SqlCommand(query, connection)
'                    command.Parameters.AddWithValue("@numberBook", numberBook.Trim())
'                    If excludeId > 0 Then
'                        command.Parameters.AddWithValue("@excludeId", excludeId)
'                    End If

'                    Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
'                    Return count > 0
'                End Using
'            End Using

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في IsBookNumberExists: {ex.Message}")
'            Return False
'        End Try
'    End Function

'    '''' <summary>
'    '''' إضافة صادر جديد
'    '''' </summary>
'    'Public Function AddSader(dateBook As Date, subjBook As String, willMaster As String, willSub As String, userName As String, Optional archiveID As Integer? = Nothing) As Boolean
'    '    Try
'    '        ' التحقق من صحة البيانات
'    '        If String.IsNullOrWhiteSpace(subjBook) OrElse String.IsNullOrWhiteSpace(userName) Then
'    '            Return False
'    '        End If

'    '        ' الحصول على رقم الكتاب التالي
'    '        Dim numberBook As String = GetNextBookNumber()
'    '        If String.IsNullOrWhiteSpace(numberBook) Then
'    '            Return False
'    '        End If

'    '        Dim query As String = "INSERT INTO TbSader (NumberBook, DateBook, SubjBook, WillMaster, WillSub, UserName, ArchiveID) VALUES (@numberBook, @dateBook, @subjBook, @willMaster, @willSub, @userName, @archiveID)"

'    '        Using connection As SqlConnection = GetConnection()
'    '            connection.Open()

'    '            Using command As New SqlCommand(query, connection)
'    '                command.Parameters.AddWithValue("@numberBook", numberBook)
'    '                command.Parameters.AddWithValue("@dateBook", dateBook.Date)
'    '                command.Parameters.AddWithValue("@subjBook", subjBook.Trim())
'    '                command.Parameters.AddWithValue("@willMaster", If(String.IsNullOrWhiteSpace(willMaster), "", willMaster.Trim()))
'    '                command.Parameters.AddWithValue("@willSub", If(String.IsNullOrWhiteSpace(willSub), "", willSub.Trim()))
'    '                command.Parameters.AddWithValue("@userName", userName.Trim())
'    '                command.Parameters.AddWithValue("@archiveID", If(archiveID.HasValue, CObj(archiveID.Value), DBNull.Value))

'    '                Return command.ExecuteNonQuery() > 0
'    '            End Using
'    '        End Using

'    '    Catch ex As Exception
'    '        System.Diagnostics.Debug.WriteLine($"خطأ في AddSader: {ex.Message}")
'    '        Return False
'    '    Finally
'    '        ' 6. تحديث العدد بعد العملية الناجحة
'    '        SaderHelper.UpdateCount()
'    '    End Try
'    'End Function

'    ''' <summary>
'    ''' إضافة صادر جديد وإرجاع معرفه
'    ''' </summary>
'    Public Function AddSader(dateBook As Date, subjBook As String, willMaster As String, willSub As String, userName As String, Optional archiveID As Integer? = Nothing) As Integer
'        Try
'            ' التحقق من صحة البيانات
'            If String.IsNullOrWhiteSpace(subjBook) OrElse String.IsNullOrWhiteSpace(userName) Then
'                Return 0
'            End If

'            ' الحصول على رقم الكتاب التالي
'            Dim numberBook As String = GetNextBookNumber()
'            If String.IsNullOrWhiteSpace(numberBook) Then
'                Return 0
'            End If

'            ' تعديل الاستعلام لإرجاع المعرف الجديد
'            Dim query As String = "INSERT INTO TbSader (NumberBook, DateBook, SubjBook, WillMaster, WillSub, UserName) " &
'                              "VALUES (@numberBook, @dateBook, @subjBook, @willMaster, @willSub, @userName); " &
'                              "SELECT SCOPE_IDENTITY();"

'            Using connection As SqlConnection = GetConnection()
'                connection.Open()

'                Using command As New SqlCommand(query, connection)
'                    command.Parameters.AddWithValue("@numberBook", numberBook)
'                    command.Parameters.AddWithValue("@dateBook", dateBook.Date)
'                    command.Parameters.AddWithValue("@subjBook", subjBook.Trim())
'                    command.Parameters.AddWithValue("@willMaster", If(String.IsNullOrWhiteSpace(willMaster), DBNull.Value, willMaster.Trim()))
'                    command.Parameters.AddWithValue("@willSub", If(String.IsNullOrWhiteSpace(willSub), DBNull.Value, willSub.Trim()))
'                    command.Parameters.AddWithValue("@userName", userName.Trim())


'                    ' الحصول على المعرف الجديد
'                    Dim newIdObj As Object = command.ExecuteScalar()
'                    If newIdObj IsNot Nothing AndAlso IsNumeric(newIdObj) Then
'                        Dim newSaderId As Integer = Convert.ToInt32(newIdObj)

'                        ' 6. تحديث العدد بعد العملية الناجحة
'                        SaderHelper.UpdateCount()

'                        Return newSaderId
'                    Else
'                        Return 0
'                    End If
'                End Using
'            End Using

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في AddSader: {ex.Message}")
'            Return 0
'        End Try
'    End Function
'    ''' <summary>
'    ''' تحديث بيانات صادر
'    ''' </summary>
'    Public Function UpdateSader(id As Integer, numberBook As String, dateBook As Date, subjBook As String, willMaster As String, willSub As String, userName As String, Optional archiveID As Integer? = Nothing) As Boolean
'        Try
'            ' التحقق من صحة البيانات
'            If id <= 0 OrElse String.IsNullOrWhiteSpace(numberBook) OrElse String.IsNullOrWhiteSpace(subjBook) OrElse String.IsNullOrWhiteSpace(userName) Then
'                Return False
'            End If

'            Dim query As String = "UPDATE TbSader SET NumberBook = @numberBook, DateBook = @dateBook, SubjBook = @subjBook, WillMaster = @willMaster, WillSub = @willSub, UserName = @userName WHERE ID = @id"

'            Using connection As SqlConnection = GetConnection()
'                connection.Open()

'                Using command As New SqlCommand(query, connection)
'                    command.Parameters.AddWithValue("@id", id)
'                    command.Parameters.AddWithValue("@numberBook", numberBook.Trim())
'                    command.Parameters.AddWithValue("@dateBook", dateBook.Date)
'                    command.Parameters.AddWithValue("@subjBook", subjBook.Trim())
'                    command.Parameters.AddWithValue("@willMaster", If(String.IsNullOrWhiteSpace(willMaster), "", willMaster.Trim()))
'                    command.Parameters.AddWithValue("@willSub", If(String.IsNullOrWhiteSpace(willSub), "", willSub.Trim()))
'                    command.Parameters.AddWithValue("@userName", userName.Trim())
'                    command.Parameters.AddWithValue("@archiveID", If(archiveID.HasValue, CObj(archiveID.Value), DBNull.Value))

'                    Return command.ExecuteNonQuery() > 0
'                End Using
'            End Using

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateSader: {ex.Message}")
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' حذف صادر
'    ''' </summary>
'    Public Function DeleteSader(id As Integer) As Boolean
'        Try
'            If id <= 0 Then
'                Return False
'            End If

'            Dim query As String = "DELETE FROM TbSader WHERE ID = @id"

'            Using connection As SqlConnection = GetConnection()
'                connection.Open()

'                Using command As New SqlCommand(query, connection)
'                    command.Parameters.AddWithValue("@id", id)

'                    Return command.ExecuteNonQuery() > 0
'                End Using
'            End Using

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في DeleteSader: {ex.Message}")
'            Return False
'        Finally
'            ' 6. تحديث العدد بعد العملية الناجحة
'            SaderHelper.UpdateCount()
'        End Try
'    End Function

'    ''' <summary>
'    ''' الحصول على رقم الكتاب التالي
'    ''' </summary>
'    Public Function GetNextBookNumber() As String
'        Try
'            Dim query As String = "SELECT ISNULL(MAX(CAST(NumberBook AS BIGINT)), 251229999) + 1 FROM TbSader WHERE CAST(NumberBook AS BIGINT) >= 251230000"

'            Using connection As SqlConnection = GetConnection()
'                connection.Open()

'                Using command As New SqlCommand(query, connection)
'                    Dim result As Object = command.ExecuteScalar()
'                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
'                        Return result.ToString()
'                    Else
'                        Return "1"
'                    End If
'                End Using
'            End Using

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في GetNextBookNumber: {ex.Message}")
'            Return "1"
'        End Try
'    End Function

'    ''' <summary>
'    ''' تحديث معرف الأرشيف لصادر محدد
'    ''' </summary>
'    Public Function UpdateArchiveIDByNumberBook(numberBook As String, archiveID As Integer) As Boolean
'        Try
'            If String.IsNullOrWhiteSpace(numberBook) OrElse archiveID <= 0 Then
'                Return False
'            End If

'            Dim query As String = "UPDATE TbSader SET ArchiveID = @archiveID WHERE NumberBook = @numberBook"

'            Using connection As SqlConnection = GetConnection()
'                connection.Open()

'                Using command As New SqlCommand(query, connection)
'                    command.Parameters.AddWithValue("@archiveID", archiveID)
'                    command.Parameters.AddWithValue("@numberBook", numberBook.Trim())

'                    Return command.ExecuteNonQuery() > 0
'                End Using
'            End Using

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateArchiveIDByNumberBook: {ex.Message}")
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' التحقق من صحة وتنظيف بيانات الصادر
'    ''' </summary>
'    ''' <param name="dataTable">جدول البيانات الأصلي</param>
'    ''' <returns>جدول بيانات منظف</returns>
'    Private Function ValidateAndCleanSaderData(dataTable As DataTable) As DataTable
'        Try
'            If dataTable Is Nothing OrElse dataTable.Rows.Count = 0 Then
'                Return CreateEmptySaderTable()
'            End If

'            ' إنشاء جدول جديد منظف
'            Dim cleanTable As DataTable = CreateEmptySaderTable()

'            ' نسخ الصفوف مع التحقق من صحتها
'            For Each row As DataRow In dataTable.Rows
'                Try
'                    If row.RowState <> DataRowState.Deleted Then
'                        Dim newRow As DataRow = cleanTable.NewRow()

'                        ' نسخ البيانات مع معالجة القيم الفارغة
'                        newRow("ID") = If(IsDBNull(row("ID")), 0, Convert.ToInt32(row("ID")))
'                        newRow("NumberBook") = If(IsDBNull(row("NumberBook")), "", row("NumberBook").ToString().Trim())
'                        newRow("DateBook") = If(IsDBNull(row("DateBook")), DateTime.Now, Convert.ToDateTime(row("DateBook")))
'                        newRow("SubjBook") = If(IsDBNull(row("SubjBook")), "", row("SubjBook").ToString().Trim())
'                        newRow("WillMaster") = If(IsDBNull(row("WillMaster")), "", row("WillMaster").ToString().Trim())
'                        newRow("WillSub") = If(IsDBNull(row("WillSub")), "", row("WillSub").ToString().Trim())
'                        newRow("UserName") = If(IsDBNull(row("UserName")), "", row("UserName").ToString().Trim())
'                        newRow("ArchiveID") = If(IsDBNull(row("ArchiveID")), DBNull.Value, Convert.ToInt32(row("ArchiveID")))

'                        cleanTable.Rows.Add(newRow)
'                    End If
'                Catch ex As Exception
'                    System.Diagnostics.Debug.WriteLine($"خطأ في معالجة صف البيانات: {ex.Message}")
'                    Continue For
'                End Try
'            Next

'            Return cleanTable

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في ValidateAndCleanSaderData: {ex.Message}")
'            Return CreateEmptySaderTable()
'        End Try
'    End Function

'    ''' <summary>
'    ''' إنشاء جدول صادر فارغ بالهيكل الصحيح
'    ''' </summary>
'    ''' <returns>جدول بيانات فارغ</returns>
'    Private Function CreateEmptySaderTable() As DataTable
'        Try
'            Dim emptyTable As New DataTable()

'            emptyTable.Columns.Add("ID", GetType(Integer))
'            emptyTable.Columns.Add("NumberBook", GetType(String))
'            emptyTable.Columns.Add("DateBook", GetType(DateTime))
'            emptyTable.Columns.Add("SubjBook", GetType(String))
'            emptyTable.Columns.Add("WillMaster", GetType(String))
'            emptyTable.Columns.Add("WillSub", GetType(String))
'            emptyTable.Columns.Add("UserName", GetType(String))
'            emptyTable.Columns.Add("ArchiveID", GetType(Integer))

'            ' تعيين القيم الافتراضية
'            emptyTable.Columns("ID").DefaultValue = 0
'            emptyTable.Columns("NumberBook").DefaultValue = ""
'            emptyTable.Columns("DateBook").DefaultValue = DateTime.Now
'            emptyTable.Columns("SubjBook").DefaultValue = ""
'            emptyTable.Columns("WillMaster").DefaultValue = ""
'            emptyTable.Columns("WillSub").DefaultValue = ""
'            emptyTable.Columns("UserName").DefaultValue = ""
'            emptyTable.Columns("ArchiveID").DefaultValue = DBNull.Value

'            Return emptyTable

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء جدول فارغ: {ex.Message}")
'            Return New DataTable()
'        End Try
'    End Function

'    'الاكمال التلقائي للتكست بوكس
'    Public Sub AutoCompleteForTextBox(ByVal ColName As String, ByRef TableName As String, ByVal TXT As TextBox)
'        Try
'            If TXT.Text = Nothing Then
'                Exit Sub
'            Else
'                Dim DT As New DataTable
'                Dim da As SqlDataAdapter

'                da = New SqlDataAdapter("select " & ColName & " from " & TableName & " ", GetConnection)
'                da.Fill(DT)

'                Dim datasource As New AutoCompleteStringCollection
'                datasource.Clear()

'                For i As Integer = 0 To DT.Rows.Count - 1
'                    datasource.Add(DT.Rows(i)(ColName))
'                Next

'                TXT.AutoCompleteCustomSource = datasource
'                TXT.AutoCompleteMode = AutoCompleteMode.SuggestAppend
'                TXT.AutoCompleteSource = AutoCompleteSource.CustomSource
'            End If
'        Catch ex As Exception
'            Exit Sub
'        End Try
'    End Sub

'    Public Function AddSubjBook(subjBook As String) As Boolean
'        Try
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@Subject_Book", subjBook)
'            }

'            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("AddSubjBook", parameters)
'            Return rowsAffected > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في إضافة موضوع جديد: " & ex.Message)
'        End Try
'    End Function

'    ' دالة مساعدة لتحديد شركة الشحن بناءً على رقم البوليصة
'    Public Shared Function GetForwardingCompanyByBillOfLading(billOfLading As String) As String
'        Try
'            ' التحقق من أن الرقم يبدأ بـ 413
'            If billOfLading.StartsWith("413", StringComparison.OrdinalIgnoreCase) Then
'                Return "EXPEDITORS INTERNATIONAL | تصدير"
'            End If

'            ' يمكنك إضافة شروط أخرى هنا
'            ' If billOfLading.StartsWith("500") Then
'            '    Return "شركة أخرى | استيراد"
'            ' End If

'            ' القيمة الافتراضية (إذا لم ينطبق أي شرط)
'            Return String.Empty
'        Catch ex As Exception
'            Throw New Exception("خطأ في تحديد شركة الشحن: " & ex.Message)
'        End Try
'    End Function

'    ' (اختياري) دالة للتحقق من وجود Subject_Book في الجدول
'    Public Function GetSubjectBookDetails(subjectBookId As String) As DataTable
'        Try
'            Dim query As String = "SELECT Subject_Book FROM TbSubjBooks WHERE ID = @ID"

'            ' ✅ الحل الصحيح: إنشاء مصفوفة من SqlParameter '
'            Dim parameters() As SqlParameter = {
'            DatabaseHelper.CreateParameter("@ID", subjectBookId)
'        }

'            Return DatabaseHelper.ExecuteQuery(query, parameters) ' لا يوجد خطأ الآن
'        Catch ex As Exception
'            Throw New Exception("خطأ في جلب بيانات Subject_Book: " & ex.Message)
'        End Try
'    End Function

'End Class

'======================================================================================================******************************************************************************

'Imports System.Data.SqlClient

'Public Class SaderClass
'    Public Function AddSader(dateBook As Date, subjBook As String, willMaster As String, willSub As String, userName As String, archiveID As Integer?) As Boolean
'        Try
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@DateBook", dateBook),
'                DatabaseHelper.CreateParameter("@SubjBook", subjBook),
'                DatabaseHelper.CreateParameter("@WillMaster", willMaster),
'                DatabaseHelper.CreateParameter("@WillSub", willSub),
'                DatabaseHelper.CreateParameter("@UserName", userName),
'                DatabaseHelper.CreateParameter("@ArchiveID", If(archiveID.HasValue, archiveID.Value, DBNull.Value))
'            }

'            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("AddSader", parameters)
'            Return rowsAffected > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في إضافة الصادر: " & ex.Message)
'        End Try
'    End Function

'    Public Function UpdateSader(id As Integer, numberBook As String, dateBook As Date, subjBook As String, willMaster As String, willSub As String, userName As String, archiveID As Integer?) As Boolean
'        Try
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@ID", id),
'                DatabaseHelper.CreateParameter("@NumberBook", numberBook),
'                DatabaseHelper.CreateParameter("@DateBook", dateBook),
'                DatabaseHelper.CreateParameter("@SubjBook", subjBook),
'                DatabaseHelper.CreateParameter("@WillMaster", willMaster),
'                DatabaseHelper.CreateParameter("@WillSub", willSub),
'                DatabaseHelper.CreateParameter("@UserName", userName),
'                DatabaseHelper.CreateParameter("@ArchiveID", If(archiveID.HasValue, archiveID.Value, DBNull.Value))
'            }

'            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("UpdateSader", parameters)
'            Return rowsAffected > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في تحديث الصادر: " & ex.Message)
'        End Try
'    End Function

'    Public Function DeleteSader(id As Integer) As Boolean
'        Try
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@ID", id)
'            }

'            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("DeleteSader", parameters)
'            Return rowsAffected > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في حذف الصادر: " & ex.Message)
'        End Try
'    End Function

'    Public Function SearchSader(searchTerm As String, searchColumn As String) As DataTable
'        Try
'            Dim query As String = "SELECT ID, NumberBook, DateBook, SubjBook, WillMaster, WillSub, UserName, ArchiveID FROM TbSader WHERE " & searchColumn & " LIKE @SearchTerm ORDER BY NumberBook DESC"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@SearchTerm", "%" & searchTerm & "%")
'            }

'            Return DatabaseHelper.ExecuteQuery(query, parameters)
'        Catch ex As Exception
'            Throw New Exception("خطأ في البحث عن الصادر: " & ex.Message)
'        End Try
'    End Function

'    Public Function SearchSaderByDateRange(startDate As Date, endDate As Date) As DataTable
'        Try
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@StartDate", startDate),
'                DatabaseHelper.CreateParameter("@EndDate", endDate)
'            }

'            Return DatabaseHelper.ExecuteStoredProcedure("SearchSaderByDateRange", parameters)
'        Catch ex As Exception
'            Throw New Exception("خطأ في البحث عن الصادر بالتاريخ: " & ex.Message)
'        End Try
'    End Function

'    Public Function GetAllSader() As DataTable
'        Try
'            Dim query As String = "SELECT ID, NumberBook, DateBook, SubjBook, WillMaster, WillSub, UserName, ArchiveID FROM TbSader ORDER BY NumberBook DESC"
'            Return DatabaseHelper.ExecuteQuery(query)
'        Catch ex As Exception
'            Throw New Exception("خطأ في الحصول على الصادر: " & ex.Message)
'        End Try
'    End Function

'    Public Function GetSaderById(id As Integer) As DataTable
'        Try
'            Dim query As String = "SELECT ID, NumberBook, DateBook, SubjBook, WillMaster, WillSub, UserName, ArchiveID FROM TbSader WHERE ID = @ID"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@ID", id)
'            }

'            Return DatabaseHelper.ExecuteQuery(query, parameters)
'        Catch ex As Exception
'            Throw New Exception("خطأ في الحصول على بيانات الصادر: " & ex.Message)
'        End Try
'    End Function

'    Public Function GetBookNumberById(id As Integer) As String
'        Try
'            Dim query As String = "SELECT NumberBook FROM TbSader WHERE ID = @ID"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@ID", id)
'            }
'            Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
'            If Not IsDBNull(result) AndAlso Not result Is Nothing Then
'                Return result.ToString()
'            Else
'                Return String.Empty
'            End If
'        Catch ex As Exception
'            Throw New Exception("خطأ في الحصول على رقم الكتاب بواسطة المعرف: " & ex.Message)
'        End Try
'    End Function

'    Public Function GetNextBookNumber() As String
'        Try
'            Dim query As String = "SELECT ISNULL(MAX(CAST(NumberBook AS BIGINT)), 25122999) + 1 FROM TbSader WHERE CAST(NumberBook AS BIGINT) >= 251230000"
'            Dim result As Object = DatabaseHelper.ExecuteScalar(query)
'            Return result.ToString()
'        Catch ex As Exception
'            Throw New Exception("خطأ في الحصول على رقم الكتاب التالي: " & ex.Message)
'        End Try
'    End Function

'    Public Function IsBookNumberExists(numberBook As String, Optional excludeId As Integer = 0) As Boolean
'        Try
'            Dim query As String = "SELECT COUNT(*) FROM TbSader WHERE NumberBook = @NumberBook"
'            If excludeId > 0 Then
'                query += " AND ID <> @ExcludeId"
'            End If

'            Dim parameters As New List(Of SqlParameter) From {
'                DatabaseHelper.CreateParameter("@NumberBook", numberBook)
'            }

'            If excludeId > 0 Then
'                parameters.Add(DatabaseHelper.CreateParameter("@ExcludeId", excludeId))
'            End If

'            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters.ToArray()))
'            Return count > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في التحقق من رقم الكتاب: " & ex.Message)
'        End Try
'    End Function

'    ' دالة لتحديث معرف الأرشيف بناءً على رقم الكتاب
'    Public Function UpdateArchiveIDByNumberBook(numberBook As String, archiveID As Integer) As Boolean
'        Try
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@NumberBook", numberBook),
'                DatabaseHelper.CreateParameter("@ArchiveID", archiveID)
'            }

'            Dim query As String = "UPDATE TbSader SET ArchiveID = @ArchiveID WHERE NumberBook = @NumberBook"
'            Dim rowsAffected As Integer = DatabaseHelper.ExecuteNonQuery(query, parameters)
'            Return rowsAffected > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في تحديث معرف الأرشيف: " & ex.Message)
'        End Try
'    End Function

'    Public Function GetSaderStatistics() As DataTable
'        Try
'            Dim query As String = "
'                SELECT 
'                    COUNT(*) AS TotalCount,
'                    COUNT(CASE WHEN MONTH(DateBook) = MONTH(GETDATE()) AND YEAR(DateBook) = YEAR(GETDATE()) THEN 1 END) AS CurrentMonthCount,
'                    COUNT(CASE WHEN YEAR(DateBook) = YEAR(GETDATE()) THEN 1 END) AS CurrentYearCount
'                FROM TbSader"

'            Return DatabaseHelper.ExecuteQuery(query)
'        Catch ex As Exception
'            Throw New Exception("خطأ في الحصول على إحصائيات الصادر: " & ex.Message)
'        End Try
'    End Function

'    'الاكمال التلقائي للتكست بوكس
'    Public Sub AutoCompleteForTextBox(ByVal ColName As String, ByRef TableName As String, ByVal TXT As TextBox)
'        Try
'            If TXT.Text = Nothing Then
'                Exit Sub
'            Else
'                Dim DT As New DataTable
'                Dim da As SqlDataAdapter

'                da = New SqlDataAdapter("select " & ColName & " from " & TableName & " ", GetConnection)
'                da.Fill(DT)

'                Dim datasource As New AutoCompleteStringCollection
'                datasource.Clear()

'                For i As Integer = 0 To DT.Rows.Count - 1
'                    datasource.Add(DT.Rows(i)(ColName))
'                Next

'                TXT.AutoCompleteCustomSource = datasource
'                TXT.AutoCompleteMode = AutoCompleteMode.SuggestAppend
'                TXT.AutoCompleteSource = AutoCompleteSource.CustomSource
'            End If
'        Catch ex As Exception
'            Exit Sub
'        End Try
'    End Sub

'    Public Function AddSubjBook(subjBook As String) As Boolean
'        Try
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@Subject_Book", subjBook)
'            }

'            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("AddSubjBook", parameters)
'            Return rowsAffected > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في إضافة موضوع جديد: " & ex.Message)
'        End Try
'    End Function


'End Class


