Imports System.Data.SqlClient
Imports System.Data

Public Class PermissionClass
    Private conn As SqlConnection

    Public Sub New()
        conn = DatabaseHelper.GetConnection()
    End Sub

    ''' <summary>
    ''' التحقق مما إذا كان المستخدم هو المدير (يملك جميع الصلاحيات)
    ''' </summary>
    Public Function IsUserAdmin(userID As Integer) As Boolean
        Try
            ' جلب صلاحيات المستخدم
            Dim dt As DataTable = GetPermissionByUserID(userID)

            ' إذا لم توجد صلاحيات
            If dt.Rows.Count = 0 Then
                Return False
            End If

            Dim row As DataRow = dt.Rows(0)

            ' التحقق مما إذا كانت جميع الصلاحيات ممنوحة
            Return Convert.ToBoolean(row("CanAdd")) AndAlso
                   Convert.ToBoolean(row("CanEdit")) AndAlso
                   Convert.ToBoolean(row("CanDelete")) AndAlso
                   Convert.ToBoolean(row("CanSearch")) AndAlso
                   Convert.ToBoolean(row("CanManageUsers")) AndAlso
                   Convert.ToBoolean(row("CanManagePermissions")) AndAlso
                   Convert.ToBoolean(row("CanPrint"))
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من كون المستخدم مديرًا: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' تعيين صلاحيات المدير للمستخدم (منح جميع الصلاحيات)
    ''' </summary>
    Public Sub SetAdminPermissions(userID As Integer)
        Try
            'محاولة تحديث الصلاحيات أولاً
            'If Not UpdatePermission(userID, True, True, True, True, True, True, True) Then
            '    ' إذا فشل التحديث، محاولة الإضافة
            '    AddPermission(userID, True, True, True, True, True, True, True)
            'End If
        Catch ex As Exception
            Throw New Exception("خطأ في تعيين صلاحيات المدير: " & ex.Message)
        End Try
    End Sub


    'Public Function AddPermission(userID As Integer, formName As String, canAdd As Boolean, canEdit As Boolean, canDelete As Boolean, canSearch As Boolean, canManageUsers As Boolean, canManagePermissions As Boolean, canPrint As Boolean) As Boolean
    '    Try
    '        Dim parameters As New List(Of SqlParameter) From {
    '            DatabaseHelper.CreateParameter("@UserID", userID),
    '            DatabaseHelper.CreateParameter("@FormName", formName),
    '            DatabaseHelper.CreateParameter("@CanAdd", canAdd),
    '            DatabaseHelper.CreateParameter("@CanEdit", canEdit),
    '            DatabaseHelper.CreateParameter("@CanDelete", canDelete),
    '            DatabaseHelper.CreateParameter("@CanSearch", canSearch),
    '            DatabaseHelper.CreateParameter("@CanManageUsers", canManageUsers),
    '            DatabaseHelper.CreateParameter("@CanManagePermissions", canManagePermissions),
    '            DatabaseHelper.CreateParameter("@CanPrint", canPrint)
    '        }
    '        Return DatabaseHelper.ExecuteStoredProcedureNonQuery("AddPermission", parameters.ToArray()) > 0
    '    Catch ex As Exception
    '        Throw New Exception("خطأ في إضافة الصلاحية: " & ex.Message)
    '    End Try
    'End Function

    'Public Function UpdatePermission(userID As Integer, formName As String, canAdd As Boolean, canEdit As Boolean, canDelete As Boolean, canSearch As Boolean, canManageUsers As Boolean, canManagePermissions As Boolean, canPrint As Boolean) As Boolean
    '    Try
    '        Dim parameters As New List(Of SqlParameter) From {
    '            DatabaseHelper.CreateParameter("@UserID", userID),
    '            DatabaseHelper.CreateParameter("@FormName", formName),
    '            DatabaseHelper.CreateParameter("@CanAdd", canAdd),
    '            DatabaseHelper.CreateParameter("@CanEdit", canEdit),
    '            DatabaseHelper.CreateParameter("@CanDelete", canDelete),
    '            DatabaseHelper.CreateParameter("@CanSearch", canSearch),
    '            DatabaseHelper.CreateParameter("@CanManageUsers", canManageUsers),
    '            DatabaseHelper.CreateParameter("@CanManagePermissions", canManagePermissions),
    '            DatabaseHelper.CreateParameter("@CanPrint", canPrint)
    '        }
    '        Return DatabaseHelper.ExecuteStoredProcedureNonQuery("UpdatePermission", parameters.ToArray()) > 0
    '    Catch ex As Exception
    '        Throw New Exception("خطأ في تحديث الصلاحية: " & ex.Message)
    '    End Try
    'End Function

    'Public Function AddPermission(userID As Integer, formName As String, canAdd As Boolean, canEdit As Boolean, canDelete As Boolean, canSearch As Boolean, canManageUsers As Boolean, canManagePermissions As Boolean, canPrint As Boolean) As Boolean
    '    Try
    '        Dim parameters As New List(Of SqlParameter) From {
    '        DatabaseHelper.CreateParameter("@UserID", userID),
    '        DatabaseHelper.CreateParameter("@FormName", formName),
    '        DatabaseHelper.CreateParameter("@CanAdd", canAdd),
    '        DatabaseHelper.CreateParameter("@CanEdit", canEdit),
    '        DatabaseHelper.CreateParameter("@CanDelete", canDelete),
    '        DatabaseHelper.CreateParameter("@CanSearch", canSearch),
    '        DatabaseHelper.CreateParameter("@CanManageUsers", canManageUsers),
    '        DatabaseHelper.CreateParameter("@CanManagePermissions", canManagePermissions),
    '        DatabaseHelper.CreateParameter("@CanPrint", canPrint)
    '    }
    '        Return DatabaseHelper.ExecuteStoredProcedureNonQuery("AddPermission", parameters.ToArray()) > 0
    '    Catch ex As Exception
    '        Throw New Exception("خطأ في إضافة الصلاحية: " & ex.Message)
    '    End Try
    'End Function

    'Public Function UpdatePermission(userID As Integer, formName As String, canAdd As Boolean, canEdit As Boolean, canDelete As Boolean, canSearch As Boolean, canManageUsers As Boolean, canManagePermissions As Boolean, canPrint As Boolean) As Boolean
    '    Try
    '        Dim parameters As New List(Of SqlParameter) From {
    '        DatabaseHelper.CreateParameter("@UserID", userID),
    '        DatabaseHelper.CreateParameter("@FormName", formName),
    '        DatabaseHelper.CreateParameter("@CanAdd", canAdd),
    '        DatabaseHelper.CreateParameter("@CanEdit", canEdit),
    '        DatabaseHelper.CreateParameter("@CanDelete", canDelete),
    '        DatabaseHelper.CreateParameter("@CanSearch", canSearch),
    '        DatabaseHelper.CreateParameter("@CanManageUsers", canManageUsers),
    '        DatabaseHelper.CreateParameter("@CanManagePermissions", canManagePermissions),
    '        DatabaseHelper.CreateParameter("@CanPrint", canPrint)
    '    }
    '        Return DatabaseHelper.ExecuteStoredProcedureNonQuery("UpdatePermission", parameters.ToArray()) > 0
    '    Catch ex As Exception
    '        Throw New Exception("خطأ في تحديث الصلاحية: " & ex.Message)
    '    End Try
    'End Function

    'Public Function GetPermissionByUserID(userID As Integer) As DataTable
    '    Try
    '        Dim parameters As New List(Of SqlParameter) From {
    '            DatabaseHelper.CreateParameter("@UserID", userID)
    '        }
    '        Return DatabaseHelper.ExecuteStoredProcedure("GetPermissionByUserID", parameters.ToArray())
    '    Catch ex As Exception
    '        Throw New Exception("خطأ في جلب صلاحيات المستخدم: " & ex.Message)
    '    End Try
    'End Function

    ''' <summary>
    ''' إضافة صلاحية جديدة مع تحديد النموذج
    ''' </summary>
    Public Function AddPermission(userID As Integer, formName As String, canAdd As Boolean, canEdit As Boolean, canDelete As Boolean, canSearch As Boolean, canManageUsers As Boolean, canManagePermissions As Boolean, canPrint As Boolean) As Boolean
        Try
            Dim parameters As New List(Of SqlParameter) From {
                DatabaseHelper.CreateParameter("@UserID", userID),
                DatabaseHelper.CreateParameter("@FormName", formName),
                DatabaseHelper.CreateParameter("@CanAdd", canAdd),
                DatabaseHelper.CreateParameter("@CanEdit", canEdit),
                DatabaseHelper.CreateParameter("@CanDelete", canDelete),
                DatabaseHelper.CreateParameter("@CanSearch", canSearch),
                DatabaseHelper.CreateParameter("@CanManageUsers", canManageUsers),
                DatabaseHelper.CreateParameter("@CanManagePermissions", canManagePermissions),
                DatabaseHelper.CreateParameter("@CanPrint", canPrint)
            }
            Return DatabaseHelper.ExecuteStoredProcedureNonQuery("AddPermission", parameters.ToArray()) > 0
        Catch ex As Exception
            Throw New Exception("خطأ في إضافة الصلاحية: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' تحديث صلاحية موجودة مع تحديد النموذج
    ''' </summary>
    Public Function UpdatePermission(userID As Integer, formName As String, canAdd As Boolean, canEdit As Boolean, canDelete As Boolean, canSearch As Boolean, canManageUsers As Boolean, canManagePermissions As Boolean, canPrint As Boolean) As Boolean
        Try
            Dim parameters As New List(Of SqlParameter) From {
                DatabaseHelper.CreateParameter("@UserID", userID),
                DatabaseHelper.CreateParameter("@FormName", formName),
                DatabaseHelper.CreateParameter("@CanAdd", canAdd),
                DatabaseHelper.CreateParameter("@CanEdit", canEdit),
                DatabaseHelper.CreateParameter("@CanDelete", canDelete),
                DatabaseHelper.CreateParameter("@CanSearch", canSearch),
                DatabaseHelper.CreateParameter("@CanManageUsers", canManageUsers),
                DatabaseHelper.CreateParameter("@CanManagePermissions", canManagePermissions),
                DatabaseHelper.CreateParameter("@CanPrint", canPrint)
            }
            Return DatabaseHelper.ExecuteStoredProcedureNonQuery("UpdatePermission", parameters.ToArray()) > 0
        Catch ex As Exception
            Throw New Exception("خطأ في تحديث الصلاحية: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' جلب جميع صلاحيات المستخدم (لجميع النماذج)
    ''' </summary>
    Public Function GetPermissionByUserID(userID As Integer) As DataTable
        Try
            Dim parameters As New List(Of SqlParameter) From {
                DatabaseHelper.CreateParameter("@UserID", userID)
            }
            Return DatabaseHelper.ExecuteStoredProcedureQuery("GetPermissionByUserID", parameters.ToArray())
        Catch ex As Exception
            Throw New Exception("خطأ في جلب الصلاحيات: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' جلب صلاحيات المستخدم لنموذج محدد
    ''' </summary>
    Public Function GetPermissionByUserIDAndForm(userID As Integer, formName As String) As DataTable
        Try
            Dim parameters As New List(Of SqlParameter) From {
                DatabaseHelper.CreateParameter("@UserID", userID),
                DatabaseHelper.CreateParameter("@FormName", formName)
            }
            Return DatabaseHelper.ExecuteStoredProcedureQuery("GetPermissionByUserIDAndForm", parameters.ToArray())
        Catch ex As Exception
            Throw New Exception("خطأ في جلب الصلاحيات: " & ex.Message)
        End Try
    End Function

    Public Function GetAllPermissions() As DataTable
        Try
            Return DatabaseHelper.ExecuteQuery("GetAllPermissions")
        Catch ex As Exception
            Throw New Exception("خطأ في جلب جميع الصلاحيات: " & ex.Message)
        End Try
    End Function
End Class


