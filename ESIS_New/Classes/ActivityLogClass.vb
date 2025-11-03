Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Sockets


''' <summary>
''' كلاس لإدارة سجلات حركة المستخدمين
''' </summary>
Public Class ActivityLogClass

    ''' <summary>
    ''' دالة لجلب جميع سجلات الحركة
    ''' </summary>
    Public Function GetActivityLogs() As DataTable
        Try
            Dim query As String = "
            SELECT 
                LogID,
                UserID,
                UserName,
                ActionType,
                TableName,
                TableNameArabic,
                RecordID,
                ActionDetails,
                IPAddress,
                ActionDate
            FROM UserActivityLog
            ORDER BY ActionDate"

            Return DatabaseHelper.ExecuteQuery(query)
        Catch ex As Exception
            Throw New Exception("خطأ في الحصول على سجلات الحركة: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' دالة لتصفية سجلات الحركة بناءً على معايير البحث
    ''' </summary>
    Public Function FilterActivityLogs(criteria As ActivitySearchCriteria) As DataTable
        Try
            Dim query As String = "
            SELECT 
                LogID,
                UserID,
                UserName,
                ActionType,
                TableName,
                TableNameArabic,
                RecordID,
                ActionDetails,
                IPAddress,
                ActionDate
            FROM UserActivityLog
            WHERE 1=1"

            Dim parameters As New List(Of SqlParameter)

            ' إضافة شروط البحث
            If criteria.StartDate.HasValue Then
                query &= " AND ActionDate >= @StartDate"
                parameters.Add(New SqlParameter("@StartDate", criteria.StartDate.Value))
            End If

            If criteria.EndDate.HasValue Then
                query &= " AND ActionDate <= @EndDate"
                parameters.Add(New SqlParameter("@EndDate", criteria.EndDate.Value))
            End If

            If Not String.IsNullOrEmpty(criteria.UserName) Then
                query &= " AND UserName LIKE @UserName"
                parameters.Add(New SqlParameter("@UserName", "%" & criteria.UserName & "%"))
            End If

            If Not String.IsNullOrEmpty(criteria.TableName) Then
                query &= " AND TableName LIKE @TableName"
                parameters.Add(New SqlParameter("@TableName", "%" & criteria.TableName & "%"))
            End If

            If criteria.RecordID.HasValue AndAlso criteria.RecordID.Value > 0 Then
                query &= " AND RecordID = @RecordID"
                parameters.Add(New SqlParameter("@RecordID", criteria.RecordID.Value))
            End If

            If Not String.IsNullOrEmpty(criteria.ActionType) Then
                query &= " AND ActionType = @ActionType"
                parameters.Add(New SqlParameter("@ActionType", criteria.ActionType))
            End If

            query &= " ORDER BY ActionDate DESC"

            Return DatabaseHelper.ExecuteQuery(query, parameters.ToArray())
        Catch ex As Exception
            Throw New Exception("خطأ في تصفية سجلات الحركة: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' دالة لإضافة سجل نشاط مستخدم
    ''' </summary>
    Public Function LogUserActivity(userName As String, actionType As String,
                              tableName As String, recordID As Integer, Optional details As String = "") As Boolean
        Try
            Using connection As SqlConnection = DatabaseHelper.Con() ' تأكد من استخدام نفس طريقة الاتصال
                connection.Open()
                Using command As New SqlCommand("INSERT INTO UserActivityLog ( UserName, ActionType, TableName, TableNameArabic, RecordID, ActionDetails, IPAddress, ActionDate) " &
                                          "VALUES ( @UserName, @ActionType, @TableName, @TableNameArabic, @RecordID, @ActionDetails, @IPAddress, GETDATE())", connection)


                    command.Parameters.AddWithValue("@UserName", userName)
                    command.Parameters.AddWithValue("@ActionType", actionType)
                    command.Parameters.AddWithValue("@TableName", tableName)
                    command.Parameters.AddWithValue("@TableNameArabic", GetArabicTableName(tableName))
                    command.Parameters.AddWithValue("@RecordID", If(recordID > 0, recordID, DBNull.Value))
                    command.Parameters.AddWithValue("@ActionDetails", If(String.IsNullOrEmpty(details), DBNull.Value, details))
                    command.Parameters.AddWithValue("@IPAddress", GetClientIP())

                    Return command.ExecuteNonQuery() > 0
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تسجيل نشاط المستخدم: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' دالة للحصول على اسم الجدول بالعربية
    ''' </summary>
    Private Function GetArabicTableName(tableName As String) As String
        Select Case tableName.ToUpper()
            Case "TBARCHIVE"
                Return "الأرشيف"
            Case "TBSADER"
                Return "الصادر"
            Case "TBWARED"
                Return "الوارد"
            Case "TBUSERS"
                Return "المستخدمين"
            Case "TBPERMISSIONS"
                Return "الصلاحيات"
            Case "USERACTIVITYLOG"
                Return "سجلات الحركة"
            Case Else
                Return tableName ' أو يمكن استخدام "جدول غير معروف"
        End Select
    End Function

    ''' <summary>
    ''' دالة للحصول على إحصائيات النشاط
    ''' </summary>
    Public Function GetActivityStatistics() As DataTable
        Try
            Dim query As String = "
            SELECT 
                UserName,
                COUNT(*) as TotalActions,
                SUM(CASE WHEN ActionType = 'Add' THEN 1 ELSE 0 END) as AddCount,
                SUM(CASE WHEN ActionType = 'Edit' THEN 1 ELSE 0 END) as EditCount,
                SUM(CASE WHEN ActionType = 'Delete' THEN 1 ELSE 0 END) as DeleteCount,
                SUM(CASE WHEN ActionType = 'Search' THEN 1 ELSE 0 END) as SearchCount,
                MIN(ActionDate) as FirstAction,
                MAX(ActionDate) as LastAction
            FROM UserActivityLog
            GROUP BY UserName
            ORDER BY TotalActions DESC"

            Return DatabaseHelper.ExecuteQuery(query)
        Catch ex As Exception
            Throw New Exception("خطأ في الحصول على إحصائيات النشاط: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' دالة للحصول على عنوان IP للعميل
    ''' </summary>
    Public Shared Function GetClientIP() As String
        Try
            ' محاولة الحصول على عنوان IP المحلي
            Dim hostEntry As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
            For Each address As IPAddress In hostEntry.AddressList
                ' التحقق مما إذا كان العنوان IPv4 وليس loopback
                If address.AddressFamily = AddressFamily.InterNetwork AndAlso Not IPAddress.IsLoopback(address) Then
                    Return address.ToString()
                End If
            Next

            ' إذا لم يتم العثور على عنوان، استخدم 127.0.0.1
            Return "127.0.0.1"
        Catch ex As Exception
            ' في حالة الخطأ، استخدم عنوان افتراضي
            Return "Unknown"
        End Try
    End Function
End Class

''' <summary>
''' كلاس لتحديد معايير بحث سجلات الحركة
''' </summary>
Public Class ActivitySearchCriteria
    Public Property StartDate As Date?
    Public Property EndDate As Date?
    Public Property UserName As String
    Public Property TableName As String
    Public Property RecordID As Integer?
    Public Property ActionType As String
End Class