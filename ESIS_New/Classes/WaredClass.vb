Imports System.Data.SqlClient

Public Class WaredClass
    ''' <summary>
    ''' إضافة وارد جديد
    ''' </summary>
    ''' <param name="numberBook">رقم الكتاب</param>
    ''' <param name="dateBook">تاريخ الكتاب</param>
    ''' <param name="subjBook">موضوع الكتاب</param>
    ''' <param name="willMaster">رقم البوليصة الرئيسية</param>
    ''' <param name="willSub">رقم البوليصة الفرعية</param>
    ''' <param name="userName">اسم المستخدم</param>
    ''' <returns>True إذا تم الإضافة بنجاح</returns>
    Public Function AddWared(numberBook As String, dateBook As Date, subjBook As String, willMaster As String, willSub As String, userName As String) As Boolean
        Try
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@NumberBook", numberBook),
                DatabaseHelper.CreateParameter("@DateBook", dateBook),
                DatabaseHelper.CreateParameter("@SubjBook", subjBook),
                DatabaseHelper.CreateParameter("@WillMaster", willMaster),
                DatabaseHelper.CreateParameter("@WillSub", willSub),
                DatabaseHelper.CreateParameter("@UserName", userName)
            }

            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("AddWared", parameters)
            Return rowsAffected > 0
        Catch ex As Exception
            Throw New Exception("خطأ في إضافة الوارد: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' تحديث بيانات وارد
    ''' </summary>
    ''' <param name="id">معرف الوارد</param>
    ''' <param name="numberBook">رقم الكتاب</param>
    ''' <param name="dateBook">تاريخ الكتاب</param>
    ''' <param name="subjBook">موضوع الكتاب</param>
    ''' <param name="willMaster">رقم البوليصة الرئيسية</param>
    ''' <param name="willSub">رقم البوليصة الفرعية</param>
    ''' <param name="userName">اسم المستخدم</param>
    ''' <returns>True إذا تم التحديث بنجاح</returns>
    Public Function UpdateWared(id As Integer, numberBook As String, dateBook As Date, subjBook As String, willMaster As String, willSub As String, userName As String) As Boolean
        Try
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@ID", id),
                DatabaseHelper.CreateParameter("@NumberBook", numberBook),
                DatabaseHelper.CreateParameter("@DateBook", dateBook),
                DatabaseHelper.CreateParameter("@SubjBook", subjBook),
                DatabaseHelper.CreateParameter("@WillMaster", willMaster),
                DatabaseHelper.CreateParameter("@WillSub", willSub),
                DatabaseHelper.CreateParameter("@UserName", userName)
            }

            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("UpdateWared", parameters)
            Return rowsAffected > 0
        Catch ex As Exception
            Throw New Exception("خطأ في تحديث الوارد: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' حذف وارد
    ''' </summary>
    ''' <param name="id">معرف الوارد</param>
    ''' <returns>True إذا تم الحذف بنجاح</returns>
    Public Function DeleteWared(id As Integer) As Boolean
        Try
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@ID", id)
            }

            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("DeleteWared", parameters)
            Return rowsAffected > 0
        Catch ex As Exception
            Throw New Exception("خطأ في حذف الوارد: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' البحث عن الوارد
    ''' </summary>
    ''' <param name="searchTerm">كلمة البحث</param>
    ''' <returns>DataTable يحتوي على نتائج البحث</returns>
    Public Function SearchWared(searchTerm As String) As DataTable
        Try
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@SearchTerm", searchTerm)
            }

            Return DatabaseHelper.ExecuteStoredProcedure("SearchWared", parameters)
        Catch ex As Exception
            Throw New Exception("خطأ في البحث عن الوارد: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' البحث عن الوارد بين تاريخين
    ''' </summary>
    ''' <param name="startDate">تاريخ البداية</param>
    ''' <param name="endDate">تاريخ النهاية</param>
    ''' <returns>DataTable يحتوي على نتائج البحث</returns>
    Public Function SearchWaredByDateRange(startDate As Date, endDate As Date) As DataTable
        Try
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@StartDate", startDate),
                DatabaseHelper.CreateParameter("@EndDate", endDate)
            }

            Return DatabaseHelper.ExecuteStoredProcedure("SearchWaredByDateRange", parameters)
        Catch ex As Exception
            Throw New Exception("خطأ في البحث عن الوارد بالتاريخ: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' الحصول على جميع الوارد
    ''' </summary>
    ''' <returns>DataTable يحتوي على جميع الوارد</returns>
    Public Function GetAllWared() As DataTable
        Try
            Dim query As String = "SELECT * FROM TbWared ORDER BY DateBook DESC"
            Return DatabaseHelper.ExecuteQuery(query)
        Catch ex As Exception
            Throw New Exception("خطأ في الحصول على الوارد: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' الحصول على بيانات وارد بواسطة المعرف
    ''' </summary>
    ''' <param name="id">معرف الوارد</param>
    ''' <returns>DataTable يحتوي على بيانات الوارد</returns>
    Public Function GetWaredById(id As Integer) As DataTable
        Try
            Dim query As String = "SELECT * FROM TbWared WHERE ID = @ID"
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@ID", id)
            }

            Return DatabaseHelper.ExecuteQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("خطأ في الحصول على بيانات الوارد: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' التحقق من وجود رقم كتاب
    ''' </summary>
    ''' <param name="numberBook">رقم الكتاب</param>
    ''' <param name="excludeId">معرف الوارد المراد استثناؤه (للتحديث)</param>
    ''' <returns>True إذا كان الرقم موجود</returns>
    Public Function IsBookNumberExists(numberBook As String, Optional excludeId As Integer = 0) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM TbWared WHERE NumberBook = @NumberBook"
            If excludeId > 0 Then
                query += " AND ID <> @ExcludeId"
            End If

            Dim parameters As New List(Of SqlParameter) From {
                DatabaseHelper.CreateParameter("@NumberBook", numberBook)
            }

            If excludeId > 0 Then
                parameters.Add(DatabaseHelper.CreateParameter("@ExcludeId", excludeId))
            End If

            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters.ToArray()))
            Return count > 0
        Catch ex As Exception
            Throw New Exception("خطأ في التحقق من رقم الكتاب: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' الحصول على إحصائيات الوارد
    ''' </summary>
    ''' <returns>DataTable يحتوي على الإحصائيات</returns>
    Public Function GetWaredStatistics() As DataTable
        Try
            Dim query As String = "
                SELECT 
                    COUNT(*) AS TotalCount,
                    COUNT(CASE WHEN MONTH(DateBook) = MONTH(GETDATE()) AND YEAR(DateBook) = YEAR(GETDATE()) THEN 1 END) AS CurrentMonthCount,
                    COUNT(CASE WHEN YEAR(DateBook) = YEAR(GETDATE()) THEN 1 END) AS CurrentYearCount
                FROM TbWared"

            Return DatabaseHelper.ExecuteQuery(query)
        Catch ex As Exception
            Throw New Exception("خطأ في الحصول على إحصائيات الوارد: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' الحصول على الوارد حسب المستخدم
    ''' </summary>
    ''' <param name="userName">اسم المستخدم</param>
    ''' <returns>DataTable يحتوي على الوارد الخاص بالمستخدم</returns>
    Public Function GetWaredByUser(userName As String) As DataTable
        Try
            Dim query As String = "SELECT * FROM TbWared WHERE UserName = @UserName ORDER BY DateBook DESC"
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@UserName", userName)
            }

            Return DatabaseHelper.ExecuteQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("خطأ في الحصول على الوارد حسب المستخدم: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' الحصول على الوارد حسب البوليصة الرئيسية
    ''' </summary>
    ''' <param name="willMaster">رقم البوليصة الرئيسية</param>
    ''' <returns>DataTable يحتوي على الوارد</returns>
    Public Function GetWaredByWillMaster(willMaster As String) As DataTable
        Try
            Dim query As String = "SELECT * FROM TbWared WHERE WillMaster = @WillMaster ORDER BY DateBook DESC"
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@WillMaster", willMaster)
            }

            Return DatabaseHelper.ExecuteQuery(query, parameters)
        Catch ex As Exception
            Throw New Exception("خطأ في الحصول على الوارد حسب البوليصة الرئيسية: " & ex.Message)
        End Try
    End Function
End Class

