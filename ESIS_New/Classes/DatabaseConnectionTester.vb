Imports System.Data.SqlClient
Imports System.Configuration

''' <summary>
''' كلاس للتحقق من الاتصال بقاعدة البيانات
''' </summary>
Public Class DatabaseConnectionTester
    Implements IDisposable

    Private _disposed As Boolean = False
    Private _connectionString As String
    Private _connectionTimeout As Integer = 30 ' 30 ثانية timeout

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        Try
            ' الحصول على سلسلة الاتصال من الإعدادات أو استخدام القيمة الافتراضية
            _connectionString = GetConnectionString()
        Catch ex As Exception
            Throw New Exception($"خطأ في تهيئة كلاس اختبار الاتصال: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' Constructor مع سلسلة اتصال مخصصة
    ''' </summary>
    ''' <param name="connectionString">سلسلة الاتصال</param>
    Public Sub New(connectionString As String)
        If String.IsNullOrWhiteSpace(connectionString) Then
            Throw New ArgumentException("سلسلة الاتصال لا يمكن أن تكون فارغة")
        End If

        _connectionString = connectionString
    End Sub

    ''' <summary>
    ''' اختبار الاتصال بقاعدة البيانات
    ''' </summary>
    ''' <returns>نتيجة اختبار الاتصال</returns>
    Public Function TestConnection() As DatabaseConnectionResult
        Dim result As New DatabaseConnectionResult()

        Try
            result.ConnectionString = _connectionString
            result.TestStartTime = DateTime.Now

            ' اختبار الاتصال الأساسي
            Using connection As New SqlConnection(_connectionString)
                'connection.ConnectionTimeout = _connectionTimeout
                connection.Open()

                ' اختبار تنفيذ استعلام بسيط
                Using command As New SqlCommand("SELECT 1", connection)
                    command.CommandTimeout = _connectionTimeout
                    Dim testResult As Object = command.ExecuteScalar()

                    If testResult IsNot Nothing AndAlso Convert.ToInt32(testResult) = 1 Then
                        result.IsSuccessful = True
                        result.ServerVersion = connection.ServerVersion
                        result.DatabaseName = connection.Database
                        result.DataSource = connection.DataSource
                    Else
                        result.IsSuccessful = False
                        result.ErrorMessage = "فشل في تنفيذ الاستعلام التجريبي"
                    End If
                End Using
            End Using

            result.TestEndTime = DateTime.Now
            result.ResponseTime = CInt((result.TestEndTime - result.TestStartTime).TotalMilliseconds)

            ' اختبارات إضافية إذا نجح الاتصال الأساسي
            If result.IsSuccessful Then
                PerformAdditionalTests(result)
            End If

        Catch ex As SqlException
            result.IsSuccessful = False
            result.ErrorMessage = GetSqlErrorMessage(ex)
            result.ErrorCode = ex.Number
            result.TestEndTime = DateTime.Now

        Catch ex As Exception
            result.IsSuccessful = False
            result.ErrorMessage = ex.Message
            result.TestEndTime = DateTime.Now
        End Try

        Return result
    End Function

    ''' <summary>
    ''' اختبار الاتصال بشكل غير متزامن
    ''' </summary>
    ''' <returns>نتيجة اختبار الاتصال</returns>
    Public Async Function TestConnectionAsync() As Threading.Tasks.Task(Of DatabaseConnectionResult)
        Return Await Threading.Tasks.Task.Run(Function() TestConnection())
    End Function

    ''' <summary>
    ''' تنفيذ اختبارات إضافية
    ''' </summary>
    ''' <param name="result">نتيجة الاختبار</param>
    Private Sub PerformAdditionalTests(result As DatabaseConnectionResult)
        Try
            Using connection As New SqlConnection(_connectionString)
                connection.Open()

                ' اختبار وجود الجداول الأساسية
                Dim tablesExist As Boolean = CheckEssentialTables(connection)
                result.EssentialTablesExist = tablesExist

                If Not tablesExist Then
                    result.Warnings.Add("بعض الجداول الأساسية غير موجودة")
                End If

                ' اختبار الصلاحيات
                Dim hasPermissions As Boolean = CheckDatabasePermissions(connection)
                result.HasRequiredPermissions = hasPermissions

                If Not hasPermissions Then
                    result.Warnings.Add("قد تكون هناك مشاكل في صلاحيات قاعدة البيانات")
                End If

            End Using

        Catch ex As Exception
            result.Warnings.Add($"خطأ في الاختبارات الإضافية: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' التحقق من وجود الجداول الأساسية
    ''' </summary>
    ''' <param name="connection">اتصال قاعدة البيانات</param>
    ''' <returns>True إذا كانت الجداول موجودة</returns>
    Private Function CheckEssentialTables(connection As SqlConnection) As Boolean
        Try
            Dim essentialTables() As String = {"TbUsers", "TbPermissions", "TbSader", "TbArchive"}

            For Each tableName As String In essentialTables
                Using command As New SqlCommand($"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'", connection)
                    Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                    If count = 0 Then
                        Return False
                    End If
                End Using
            Next

            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق من صلاحيات قاعدة البيانات
    ''' </summary>
    ''' <param name="connection">اتصال قاعدة البيانات</param>
    ''' <returns>True إذا كانت الصلاحيات متوفرة</returns>
    Private Function CheckDatabasePermissions(connection As SqlConnection) As Boolean
        Try
            ' اختبار صلاحية SELECT
            Using command As New SqlCommand("SELECT HAS_PERMS_BY_NAME(NULL, NULL, 'SELECT')", connection)
                Dim selectPermission As Object = command.ExecuteScalar()
                If selectPermission Is Nothing OrElse Not Convert.ToBoolean(selectPermission) Then
                    Return False
                End If
            End Using

            ' اختبار صلاحية INSERT
            Using command As New SqlCommand("SELECT HAS_PERMS_BY_NAME(NULL, NULL, 'INSERT')", connection)
                Dim insertPermission As Object = command.ExecuteScalar()
                If insertPermission Is Nothing OrElse Not Convert.ToBoolean(insertPermission) Then
                    Return False
                End If
            End Using

            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' الحصول على سلسلة الاتصال
    ''' </summary>
    ''' <returns>سلسلة الاتصال</returns>
    Private Function GetConnectionString() As String
        Try
            ' بناء سلسلة الاتصال مع تضمين Connect Timeout
            Dim connectionString As String = "Server=" & My.Settings.ServerName &
                                         ";Database=" & My.Settings.DataBaseName &
                                         ";Trusted_Connection=True;" &
                                         "Connect Timeout=" & _connectionTimeout.ToString() & ";"

            If Not String.IsNullOrWhiteSpace(connectionString) Then
                Return connectionString
            End If

            Return GetDefaultConnectionString()
        Catch ex As Exception
            Return GetDefaultConnectionString()
        End Try
    End Function

    ''' <summary>
    ''' الحصول على سلسلة الاتصال الافتراضية
    ''' </summary>
    ''' <returns>سلسلة الاتصال الافتراضية</returns>
    Private Function GetDefaultConnectionString() As String
        Return "Data Source=localhost;Initial Catalog=ESIS;Integrated Security=True;Connect Timeout=30;"
    End Function

    ''' <summary>
    ''' الحصول على رسالة خطأ SQL مفهومة
    ''' </summary>
    ''' <param name="ex">استثناء SQL</param>
    ''' <returns>رسالة خطأ مفهومة</returns>
    Private Function GetSqlErrorMessage(ex As SqlException) As String
        Select Case ex.Number
            Case 2
                Return "لا يمكن الوصول إلى خادم قاعدة البيانات. تأكد من أن الخادم يعمل وأن اسم الخادم صحيح."
            Case 18456
                Return "فشل في تسجيل الدخول. تأكد من صحة اسم المستخدم وكلمة المرور."
            Case 4060
                Return "لا يمكن فتح قاعدة البيانات المطلوبة. تأكد من وجود قاعدة البيانات."
            Case 53
                Return "لا يمكن العثور على خادم قاعدة البيانات أو الوصول إليه."
            Case 233
                Return "تم رفض الاتصال من قبل الخادم."
            Case 2146893022
                Return "مشكلة في شهادة SSL أو التشفير."
            Case Else
                Return $"خطأ في قاعدة البيانات (رقم الخطأ: {ex.Number}): {ex.Message}"
        End Select
    End Function

    ''' <summary>
    ''' تنظيف الموارد
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    ''' <summary>
    ''' تنظيف الموارد المحمي
    ''' </summary>
    ''' <param name="disposing">هل يتم التنظيف</param>
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _disposed Then
            If disposing Then
                ' تنظيف الموارد المدارة هنا إذا لزم الأمر
            End If
            _disposed = True
        End If
    End Sub

End Class

''' <summary>
''' نتيجة اختبار الاتصال بقاعدة البيانات
''' </summary>
Public Class DatabaseConnectionResult

    ''' <summary>
    ''' هل نجح الاتصال
    ''' </summary>
    Public Property IsSuccessful As Boolean = False

    ''' <summary>
    ''' رسالة الخطأ إن وجدت
    ''' </summary>
    Public Property ErrorMessage As String = ""

    ''' <summary>
    ''' رقم الخطأ
    ''' </summary>
    Public Property ErrorCode As Integer = 0

    ''' <summary>
    ''' سلسلة الاتصال المستخدمة
    ''' </summary>
    Public Property ConnectionString As String = ""

    ''' <summary>
    ''' وقت بداية الاختبار
    ''' </summary>
    Public Property TestStartTime As DateTime

    ''' <summary>
    ''' وقت انتهاء الاختبار
    ''' </summary>
    Public Property TestEndTime As DateTime

    ''' <summary>
    ''' وقت الاستجابة بالميلي ثانية
    ''' </summary>
    Public Property ResponseTime As Integer = 0

    ''' <summary>
    ''' إصدار الخادم
    ''' </summary>
    Public Property ServerVersion As String = ""

    ''' <summary>
    ''' اسم قاعدة البيانات
    ''' </summary>
    Public Property DatabaseName As String = ""

    ''' <summary>
    ''' مصدر البيانات
    ''' </summary>
    Public Property DataSource As String = ""

    ''' <summary>
    ''' هل الجداول الأساسية موجودة
    ''' </summary>
    Public Property EssentialTablesExist As Boolean = False

    ''' <summary>
    ''' هل الصلاحيات المطلوبة متوفرة
    ''' </summary>
    Public Property HasRequiredPermissions As Boolean = False

    ''' <summary>
    ''' قائمة التحذيرات
    ''' </summary>
    Public Property Warnings As New List(Of String)

    ''' <summary>
    ''' الحصول على ملخص النتيجة
    ''' </summary>
    ''' <returns>ملخص النتيجة</returns>
    Public Function GetSummary() As String
        Dim summary As New System.Text.StringBuilder()

        summary.AppendLine($"حالة الاتصال: {If(IsSuccessful, "نجح", "فشل")}")

        If IsSuccessful Then
            summary.AppendLine($"خادم قاعدة البيانات: {DataSource}")
            summary.AppendLine($"اسم قاعدة البيانات: {DatabaseName}")
            summary.AppendLine($"إصدار الخادم: {ServerVersion}")
            summary.AppendLine($"وقت الاستجابة: {ResponseTime} مللي ثانية")
            summary.AppendLine($"الجداول الأساسية: {If(EssentialTablesExist, "موجودة", "غير موجودة")}")
            summary.AppendLine($"الصلاحيات: {If(HasRequiredPermissions, "متوفرة", "غير متوفرة")}")
        Else
            summary.AppendLine($"سبب الفشل: {ErrorMessage}")
            If ErrorCode > 0 Then
                summary.AppendLine($"رقم الخطأ: {ErrorCode}")
            End If
        End If

        If Warnings.Count > 0 Then
            summary.AppendLine("التحذيرات:")
            For Each warning As String In Warnings
                summary.AppendLine($"- {warning}")
            Next
        End If

        Return summary.ToString()
    End Function

End Class
