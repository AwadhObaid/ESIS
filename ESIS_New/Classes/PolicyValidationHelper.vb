Imports System.Data.SqlClient
Imports System.Configuration

''' <summary>
''' كلاس مساعد للتحقق من وجود أرقام البوليصات في قاعدة البيانات
''' </summary>
Public Class PolicyValidationHelper

    ''' <summary>
    ''' الحصول على سلسلة الاتصال بقاعدة البيانات
    ''' </summary>
    Private Shared Function GetConnectionString() As String
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
    ''' التحقق من وجود رقم البوليصة الرئيسية في قاعدة البيانات
    ''' </summary>
    ''' <param name="masterPolicyNumber">رقم البوليصة الرئيسية</param>
    ''' <returns>True إذا كانت موجودة، False إذا لم تكن موجودة</returns>
    Public Shared Function CheckMasterPolicyExists(masterPolicyNumber As String) As Boolean
        Try
            If String.IsNullOrWhiteSpace(masterPolicyNumber) Then
                Return False
            End If

            Using connection As New SqlConnection(GetConnectionString())
                connection.Open()

                Dim query As String = "SELECT COUNT(*) FROM TbSader WHERE WillMaster = @masterPolicy"
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@masterPolicy", masterPolicyNumber.Trim())

                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count > 0
                End Using
            End Using

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من البوليصة الرئيسية: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق من وجود رقم البوليصة الفرعية في قاعدة البيانات
    ''' </summary>
    ''' <param name="subPolicyNumber">رقم البوليصة الفرعية</param>
    ''' <returns>True إذا كانت موجودة، False إذا لم تكن موجودة</returns>
    Public Shared Function CheckSubPolicyExists(subPolicyNumber As String) As Boolean
        Try
            If String.IsNullOrWhiteSpace(subPolicyNumber) Then
                Return False
            End If

            Using connection As New SqlConnection(GetConnectionString())
                connection.Open()

                Dim query As String = "SELECT COUNT(*) FROM TbSader WHERE WillSub = @subPolicy"
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@subPolicy", subPolicyNumber.Trim())

                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count > 0
                End Using
            End Using

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من البوليصة الفرعية: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' الحصول على معلومات البوليصة الرئيسية الموجودة
    ''' </summary>
    ''' <param name="masterPolicyNumber">رقم البوليصة الرئيسية</param>
    ''' <returns>معلومات البوليصة أو Nothing إذا لم توجد</returns>
    Public Shared Function GetMasterPolicyInfo(masterPolicyNumber As String) As PolicyInfo
        Try
            If String.IsNullOrWhiteSpace(masterPolicyNumber) Then
                Return Nothing
            End If

            Using connection As New SqlConnection(GetConnectionString())
                connection.Open()

                Dim query As String = "SELECT TOP 1 ID, NumberBook, SubjBook, DateBook FROM TbSader WHERE WillMaster = @masterPolicy ORDER BY ID DESC"
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@masterPolicy", masterPolicyNumber.Trim())

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Return New PolicyInfo() With {
                                .ID = Convert.ToInt32(reader("ID")),
                                .NumberBook = reader("NumberBook")?.ToString(),
                                .SubjBook = reader("SubjBook")?.ToString(),
                                .DateBook = If(IsDBNull(reader("DateBook")), Nothing, Convert.ToDateTime(reader("DateBook")))
                            }
                        End If
                    End Using
                End Using
            End Using

            Return Nothing

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على معلومات البوليصة الرئيسية: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' الحصول على معلومات البوليصة الفرعية الموجودة
    ''' </summary>
    ''' <param name="subPolicyNumber">رقم البوليصة الفرعية</param>
    ''' <returns>معلومات البوليصة أو Nothing إذا لم توجد</returns>
    Public Shared Function GetSubPolicyInfo(subPolicyNumber As String) As PolicyInfo
        Try
            If String.IsNullOrWhiteSpace(subPolicyNumber) Then
                Return Nothing
            End If

            Using connection As New SqlConnection(GetConnectionString())
                connection.Open()

                Dim query As String = "SELECT TOP 1 ID, NumberBook, SubjBook, DateBook FROM TbSader WHERE WillSub = @subPolicy ORDER BY ID DESC"
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@subPolicy", subPolicyNumber.Trim())

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Return New PolicyInfo() With {
                                .ID = Convert.ToInt32(reader("ID")),
                                .NumberBook = reader("NumberBook")?.ToString(),
                                .SubjBook = reader("SubjBook")?.ToString(),
                                .DateBook = If(IsDBNull(reader("DateBook")), Nothing, Convert.ToDateTime(reader("DateBook")))
                            }
                        End If
                    End Using
                End Using
            End Using

            Return Nothing

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على معلومات البوليصة الفرعية: {ex.Message}")
            Return Nothing
        End Try
    End Function

End Class

''' <summary>
''' كلاس لحفظ معلومات البوليصة
''' </summary>
Public Class PolicyInfo
    Public Property ID As Integer
    Public Property NumberBook As String
    Public Property SubjBook As String
    Public Property DateBook As DateTime?
End Class