Imports System.Data.SqlClient

''' <summary>
''' كلاس لإدارة صلاحيات المستخدمين في نظام ESIS
''' </summary>
Public Class clsPermissions
    ' قائمة بالدوال المسجلة لتلقي إشعارات تحديث الصلاحيات
    Private Shared _permissionUpdateHandlers As New List(Of Action)
    Private Shared _lockObject As New Object()

    ''' <summary>
    ''' تسجيل دالة للتنبيه عند تحديث الصلاحيات
    ''' </summary>
    Public Shared Sub RegisterForPermissionUpdates(handler As Action)
        SyncLock _lockObject
            If handler IsNot Nothing AndAlso Not _permissionUpdateHandlers.Contains(handler) Then
                _permissionUpdateHandlers.Add(handler)
            End If
        End SyncLock
    End Sub

    ''' <summary>
    ''' إلغاء تسجيل دالة من التنبيهات
    ''' </summary>
    Public Shared Sub UnregisterFromPermissionUpdates(handler As Action)
        SyncLock _lockObject
            If handler IsNot Nothing AndAlso _permissionUpdateHandlers.Contains(handler) Then
                _permissionUpdateHandlers.Remove(handler)
            End If
        End SyncLock
    End Sub

    ''' <summary>
    ''' إرسال إشعار بتحديث الصلاحيات
    ''' </summary>
    Public Shared Sub NotifyPermissionUpdates()
        SyncLock _lockObject
            ' إنشاء نسخة من القائمة لتجنب مشاكل التعديل أثناء التكرار
            Dim handlersToCall As New List(Of Action)(_permissionUpdateHandlers)

            ' استدعاء جميع الدوال المسجلة
            For Each handler In handlersToCall
                Try
                    handler.Invoke()
                Catch ex As Exception
                    System.Diagnostics.Debug.WriteLine($"خطأ في استدعاء معالج تحديث الصلاحيات: {ex.Message}")
                End Try
            Next
        End SyncLock
    End Sub

    ''' <summary>
    ''' التحقق من وجود صلاحية محددة للمستخدم
    ''' </summary>
    Public Shared Function HasPermission(userID As Integer, formName As String, permissionType As String) As Boolean
        Try
            ' التحقق من تسجيل الدخول
            If userID <= 0 Then
                Return False
            End If

            ' التحقق إذا كان المستخدم هو المدير
            If IsUserAdmin(userID) Then
                Return True
            End If

            ' التحقق إذا كان المستخدم نشطاً
            If Not IsUserActive(userID) Then
                Return False
            End If

            ' جلب الصلاحيات من قاعدة البيانات
            Dim query As String = "SELECT HasPermission FROM TbPermissions WHERE UserID = @UserID AND FormName = @FormName AND PermissionType = @PermissionType"
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@UserID", userID),
                DatabaseHelper.CreateParameter("@FormName", formName),
                DatabaseHelper.CreateParameter("@PermissionType", permissionType)
            }

            Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
            Return result IsNot Nothing AndAlso Convert.ToBoolean(result)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من الصلاحية: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق مما إذا كان المستخدم هو المدير
    ''' </summary>
    Public Shared Function IsUserAdmin(userID As Integer) As Boolean
        Try
            Dim query As String = "SELECT IsAdmin FROM TbUsers WHERE ID = @UserID"
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@UserID", userID)
            }

            Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
            Return result IsNot Nothing AndAlso Convert.ToBoolean(result)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من كون المستخدم مديراً: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق مما إذا كان المستخدم نشطاً
    ''' </summary>
    Public Shared Function IsUserActive(userID As Integer) As Boolean
        Try
            Dim query As String = "SELECT IsActive FROM TbUsers WHERE ID = @UserID"
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@UserID", userID)
            }

            Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
            Return result IsNot Nothing AndAlso Convert.ToBoolean(result)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من حالة المستخدم: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' الحصول على جميع صلاحيات المستخدم
    ''' </summary>
    Public Shared Function GetUserPermissions(userID As Integer) As DataTable
        Try
            ' التحقق من تسجيل الدخول
            If userID <= 0 Then
                Return New DataTable()
            End If

            ' إذا كان المستخدم هو المدير، إرجاع جميع الصلاحيات
            If IsUserAdmin(userID) Then
                Return GetAllPermissionsForAdmin()
            End If

            ' إذا كان المستخدم نشطاً، إرجاع صلاحياته
            If IsUserActive(userID) Then
                Dim query As String = "SELECT FormName, PermissionType, HasPermission FROM TbPermissions WHERE UserID = @UserID"
                Dim parameters() As SqlParameter = {
                    DatabaseHelper.CreateParameter("@UserID", userID)
                }

                Return DatabaseHelper.ExecuteQuery(query, parameters)
            End If

            ' إذا لم يكن نشطاً، إرجاع جدول فارغ
            Return New DataTable()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على صلاحيات المستخدم: {ex.Message}")
            Return New DataTable()
        End Try
    End Function

    ''' <summary>
    ''' إرجاع جميع الصلاحيات للمديرين
    ''' </summary>
    Private Shared Function GetAllPermissionsForAdmin() As DataTable
        Try
            Dim dt As New DataTable()

            ' إنشاء الأعمدة
            dt.Columns.Add("FormName", GetType(String))
            dt.Columns.Add("PermissionType", GetType(String))
            dt.Columns.Add("HasPermission", GetType(Boolean))

            ' إضافة جميع النماذج الممكنة وجميع أنواع الصلاحيات
            Dim forms As String() = {"ArchiveForm", "SaderForm", "WaredForm", "UsersForm", "PermissionsForm"}
            Dim permissions As String() = {"View", "Add", "Edit", "Delete", "Search", "Print", "Archive"}

            For Each formName As String In forms
                For Each permissionType As String In permissions
                    Dim row As DataRow = dt.NewRow()
                    row("FormName") = formName
                    row("PermissionType") = permissionType
                    row("HasPermission") = True
                    dt.Rows.Add(row)
                Next
            Next

            Return dt

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إنشاء صلاحيات المدير: {ex.Message}")
            Return New DataTable()
        End Try
    End Function

    ''' <summary>
    ''' تطبيق الصلاحيات على نموذج معين
    ''' </summary>
    Public Shared Sub ApplyPermissionsToForm(form As Form, userID As Integer, formName As String)
        Try
            ' التحقق من صحة المدخلات
            If form Is Nothing OrElse userID <= 0 Then
                Return
            End If

            ' التحقق إذا كان المستخدم هو المدير
            If IsUserAdmin(userID) Then
                EnableAllControls(form)
                Return
            End If

            ' التحقق إذا كان المستخدم نشطاً
            If Not IsUserActive(userID) Then
                DisableAllControls(form)
                Return
            End If

            ' تطبيق الصلاحيات على الأزرار القياسية
            ApplyStandardButtonPermissions(form, userID, formName)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق الصلاحيات على النموذج {form.Name}: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تطبيق الصلاحيات على الأزرار القياسية
    ''' </summary>
    Private Shared Sub ApplyStandardButtonPermissions(form As Form, userID As Integer, formName As String)
        Try
            ' التحقق من صلاحية الإضافة
            If form.Controls.ContainsKey("btnAdd") Then
                CType(form.Controls("btnAdd"), Button).Enabled = HasPermission(userID, formName, "Add")
            End If

            ' التحقق من صلاحية التعديل
            If form.Controls.ContainsKey("btnUpdate") Then
                CType(form.Controls("btnUpdate"), Button).Enabled = HasPermission(userID, formName, "Edit")
            End If

            ' التحقق من صلاحية الحذف
            If form.Controls.ContainsKey("btnDelete") Then
                CType(form.Controls("btnDelete"), Button).Enabled = HasPermission(userID, formName, "Delete")
            End If

            ' التحقق من صلاحية البحث
            If form.Controls.ContainsKey("btnSearch") Then
                CType(form.Controls("btnSearch"), Button).Enabled = HasPermission(userID, formName, "Search")
            End If

            ' التحقق من صلاحية الطباعة
            If form.Controls.ContainsKey("btnPrint") Then
                CType(form.Controls("btnPrint"), Button).Enabled = HasPermission(userID, formName, "Print")
            End If

            ' التحقق من صلاحية الأرشيف
            If form.Controls.ContainsKey("btnArchive") Then
                CType(form.Controls("btnArchive"), Button).Enabled = HasPermission(userID, formName, "Archive")
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق الصلاحيات على الأزرار: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تفعيل جميع العناصر في النموذج (للمديرين)
    ''' </summary>
    Private Shared Sub EnableAllControls(parent As Control)
        Try
            For Each ctrl As Control In parent.Controls
                ' تجاهل العناصر التي لا يمكن تعطيلها
                If ctrl.GetType().GetProperty("Enabled") IsNot Nothing Then
                    ctrl.Enabled = True
                End If

                ' تطبيق التفعيل على العناصر الفرعية
                If ctrl.HasChildren Then
                    EnableAllControls(ctrl)
                End If
            Next
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تفعيل جميع العناصر: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تعطيل جميع العناصر في النموذج (للمستخدمين غير النشطين)
    ''' </summary>
    Private Shared Sub DisableAllControls(parent As Control)
        Try
            For Each ctrl As Control In parent.Controls
                ' تجاهل العناصر التي لا يمكن تعطيلها
                If ctrl.GetType().GetProperty("Enabled") IsNot Nothing Then
                    ctrl.Enabled = False
                End If

                ' تطبيق التعطيل على العناصر الفرعية
                If ctrl.HasChildren Then
                    DisableAllControls(ctrl)
                End If
            Next
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تعطيل جميع العناصر: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' حفظ صلاحيات المستخدم
    ''' </summary>
    Public Shared Function SavePermissions(userID As Integer, formName As String, permissionType As String, hasPermission As Boolean) As Boolean
        Try
            ' التحقق من صحة المدخلات
            If userID <= 0 OrElse String.IsNullOrWhiteSpace(formName) OrElse String.IsNullOrWhiteSpace(permissionType) Then
                Return False
            End If

            ' استخدام MERGE لدمج الإدراج والتحديث
            Dim query As String = "
                MERGE INTO TbPermissions AS target 
                USING (VALUES (@UserID, @FormName, @PermissionType, @HasPermission)) 
                AS source (UserID, FormName, PermissionType, HasPermission) 
                ON (target.UserID = source.UserID AND target.FormName = source.FormName AND target.PermissionType = source.PermissionType) 
                WHEN MATCHED THEN 
                    UPDATE SET HasPermission = source.HasPermission 
                WHEN NOT MATCHED THEN 
                    INSERT (UserID, FormName, PermissionType, HasPermission) 
                    VALUES (source.UserID, source.FormName, source.PermissionType, source.HasPermission);"

            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@UserID", userID),
                DatabaseHelper.CreateParameter("@FormName", formName),
                DatabaseHelper.CreateParameter("@PermissionType", permissionType),
                DatabaseHelper.CreateParameter("@HasPermission", hasPermission)
            }

            ' تنفيذ الاستعلام
            DatabaseHelper.ExecuteNonQuery(query, parameters)

            ' إرسال إشعار بتحديث الصلاحيات
            NotifyPermissionUpdates()

            Return True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حفظ الصلاحية {permissionType} للنموذج {formName}: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' حذف جميع صلاحيات المستخدم
    ''' </summary>
    Public Shared Function DeleteUserPermissions(userID As Integer) As Boolean
        Try
            ' التحقق من صحة المدخلات
            If userID <= 0 Then
                Return False
            End If

            ' حذف جميع صلاحيات المستخدم
            Dim query As String = "DELETE FROM TbPermissions WHERE UserID = @UserID"
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@UserID", userID)
            }

            ' تنفيذ الاستعلام
            DatabaseHelper.ExecuteNonQuery(query, parameters)

            ' إرسال إشعار بتحديث الصلاحيات
            NotifyPermissionUpdates()

            Return True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حذف صلاحيات المستخدم {userID}: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' حذف صلاحيات نموذج معين للمستخدم
    ''' </summary>
    Public Shared Function DeleteFormPermissions(userID As Integer, formName As String) As Boolean
        Try
            ' التحقق من صحة المدخلات
            If userID <= 0 OrElse String.IsNullOrWhiteSpace(formName) Then
                Return False
            End If

            ' حذف صلاحيات النموذج
            Dim query As String = "DELETE FROM TbPermissions WHERE UserID = @UserID AND FormName = @FormName"
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@UserID", userID),
                DatabaseHelper.CreateParameter("@FormName", formName)
            }

            ' تنفيذ الاستعلام
            DatabaseHelper.ExecuteNonQuery(query, parameters)

            ' إرسال إشعار بتحديث الصلاحيات
            NotifyPermissionUpdates()

            Return True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حذف صلاحيات النموذج {formName}: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق من صلاحية معينة مع عرض رسالة إذا لزم الأمر
    ''' </summary>
    Public Shared Function CheckPermissionWithMessage(userID As Integer, formName As String, permissionType As String, actionName As String) As Boolean
        Try
            Dim hasPermissionresult As Boolean = HasPermission(userID, formName, permissionType)

            If Not hasPermissionresult Then
                MessageBox.Show($"ليس لديك صلاحية {actionName}. يرجى الاتصال بمدير النظام.",
                              "صلاحيات محدودة",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information)
            End If

            Return hasPermissionresult

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من الصلاحية {permissionType}: {ex.Message}")
            Return False
        End Try
    End Function
End Class