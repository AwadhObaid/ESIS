Imports System.Data.SqlClient

''' <summary>
''' كلاس مساعد لتوفير واجهة بسيطة للصلاحيات
''' </summary>
Public Class PermissionHelper

    ''' <summary>
    ''' التحقق من صلاحية الإضافة
    ''' </summary>
    Public Shared Function CanAdd() As Boolean
        Return PermissionsManager.Instance.CanAdd(GetCurrentFormName())
    End Function

    ''' <summary>
    ''' التحقق من صلاحية التعديل
    ''' </summary>
    Public Shared Function CanEdit() As Boolean
        Return PermissionsManager.Instance.CanEdit(GetCurrentFormName())
    End Function

    ''' <summary>
    ''' التحقق من صلاحية الحذف
    ''' </summary>
    Public Shared Function CanDelete() As Boolean
        Return PermissionsManager.Instance.CanDelete(GetCurrentFormName())
    End Function

    ''' <summary>
    ''' التحقق من صلاحية البحث
    ''' </summary>
    Public Shared Function CanSearch() As Boolean
        Return PermissionsManager.Instance.CanSearch(GetCurrentFormName())
    End Function

    ''' <summary>
    ''' التحقق من صلاحية الطباعة
    ''' </summary>
    Public Shared Function CanPrint() As Boolean
        Return PermissionsManager.Instance.CanPrint(GetCurrentFormName())
    End Function

    ''' <summary>
    ''' التحقق من صلاحية إدارة المستخدمين
    ''' </summary>
    Public Shared Function CanManageUsers() As Boolean
        Return PermissionsManager.Instance.HasPermission(GlobalData.LoggedInUserID, "UsersForm", "Edit")
    End Function

    ''' <summary>
    ''' التحقق من صلاحية عرض لوحة التحكم
    ''' </summary>
    Public Shared Function CanViewDashboard() As Boolean
        Return PermissionsManager.Instance.HasPermission(GlobalData.LoggedInUserID, "DashboardForm", "View")
    End Function

    ''' <summary>
    ''' التحقق من صلاحية عرض التقارير
    ''' </summary>
    Public Shared Function CanViewReports() As Boolean
        Return PermissionsManager.Instance.HasPermission(GlobalData.LoggedInUserID, "ReportsForm", "View")
    End Function

    ''' <summary>
    ''' الحصول على اسم النموذج الحالي
    ''' </summary>
    Private Shared Function GetCurrentFormName() As String
        Try
            If Application.OpenForms.Count > 0 Then
                Return Application.OpenForms(Application.OpenForms.Count - 1).GetType().Name
            End If
            Return ""
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على اسم النموذج: {ex.Message}")
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' عرض رسالة عدم الصلاحية
    ''' </summary>
    Public Shared Sub ShowNoPermissionMessage(action As String)
        MessageBox.Show($"ليس لديك صلاحية {action}. يرجى الاتصال بمدير النظام.",
                      "صلاحيات محدودة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    ''' <summary>
    ''' التحقق من صلاحية مخصصة
    ''' </summary>
    Public Shared Function HasCustomPermission(formName As String, permissionType As String) As Boolean
        Return PermissionsManager.Instance.HasPermission(GlobalData.LoggedInUserID, formName, permissionType)
    End Function

    ''' <summary>
    ''' التحقق مما إذا كان المستخدم هو المدير
    ''' </summary>
    Public Shared Function IsUserAdmin(userID As Integer) As Boolean
        Try
            ' التحقق من أن المستخدم لديه صلاحية الإدارة من جدول TbUsers
            Dim query As String = "SELECT IsAdmin FROM TbUsers WHERE ID = @UserID AND IsActive = 1"
            Using connection As SqlConnection = DatabaseHelper.Con()
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@UserID", userID)
                    Dim result As Object = command.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return Convert.ToBoolean(result)
                    End If
                End Using
            End Using

            Return False
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من صلاحيات المدير: {ex.Message}")
            Return False
        End Try
    End Function


End Class



'Imports System.Data.SqlClient

'Public Class PermissionHelper

'    ''' <summary>
'    ''' التحقق من صلاحية الإضافة
'    ''' </summary>
'    Public Shared Function CanAdd() As Boolean
'        Return clsPermissions.HasPermission(GlobalData.LoggedInUserID, "SaderForm", "Add")
'    End Function

'    ''' <summary>
'    ''' التحقق من صلاحية التعديل
'    ''' </summary>
'    Public Shared Function CanEdit() As Boolean
'        Return clsPermissions.HasPermission(GlobalData.LoggedInUserID, "SaderForm", "Edit")
'    End Function

'    ''' <summary>
'    ''' التحقق من صلاحية الحذف
'    ''' </summary>
'    Public Shared Function CanDelete() As Boolean
'        Return clsPermissions.HasPermission(GlobalData.LoggedInUserID, "SaderForm", "Delete")
'    End Function

'    ''' <summary>
'    ''' التحقق من صلاحية البحث
'    ''' </summary>
'    Public Shared Function CanSearch() As Boolean
'        Return clsPermissions.HasPermission(GlobalData.LoggedInUserID, "SaderForm", "Search")
'    End Function

'    ''' <summary>
'    ''' التحقق من صلاحية الطباعة
'    ''' </summary>
'    Public Shared Function CanPrint() As Boolean
'        Return clsPermissions.HasPermission(GlobalData.LoggedInUserID, "SaderForm", "Print")
'    End Function

'    ''' <summary>
'    ''' عرض رسالة عدم الصلاحية
'    ''' </summary>
'    Public Shared Sub ShowNoPermissionMessage(action As String)
'        MessageBox.Show($"ليس لديك صلاحية {action}. يرجى الاتصال بمدير النظام.",
'                      "صلاحيات محدودة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'    End Sub

'    ''' <summary>
'    ''' تحديث صلاحيات المستخدم الحالي
'    ''' </summary>
'    Public Shared Sub RefreshCurrentUserPermissions()
'        Try
'            ' يمكنك إضافة أي منطق لتحديث الصلاحيات هنا
'            ' مثل إعادة جلب الصلاحيات من قاعدة البيانات
'            ' أو تحديث ذاكرة التخزين المؤقت للصلاحيات

'            ' مثال:
'            ' clsPermissions.ClearCacheForUser(GlobalData.LoggedInUserID)

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في RefreshCurrentUserPermissions: {ex.Message}")
'        End Try
'    End Sub


'    ''' <summary>
'    ''' التحقق مما إذا كان المستخدم هو المدير
'    ''' </summary>
'    Public Shared Function IsAdmin(userID As Integer) As Boolean
'        Try
'            Dim query As String = "SELECT IsAdmin FROM TbUsers WHERE ID = @UserID"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@UserID", userID)
'            }

'            Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
'            Return result IsNot Nothing AndAlso Convert.ToBoolean(result)

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من كون المستخدم مديراً: {ex.Message}")
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' التحقق من صلاحية معينة
'    ''' </summary>
'    Public Shared Function HasPermission(userID As Integer, formName As String, permissionIndex As Integer) As Boolean
'        Try
'            ' إذا كان المستخدم مديراً، لديه جميع الصلاحيات
'            If IsAdmin(userID) Then
'                Return True
'            End If

'            ' التحقق من صلاحية العرض أولاً (الرقم الأول)
'            If permissionIndex > 0 Then
'                If Not HasPermission(userID, formName, 0) Then
'                    Return False
'                End If
'            End If

'            ' جلب الصلاحيات
'            Dim query As String = "SELECT Permissions FROM UserPermissions WHERE UserID = @UserID AND FormName = @FormName"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@UserID", userID),
'                DatabaseHelper.CreateParameter("@FormName", formName)
'            }

'            Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
'            If result Is Nothing Then
'                Return False
'            End If

'            ' التحقق من الصلاحية المطلوبة
'            Dim permissions As String = result.ToString()
'            If permissionIndex < permissions.Length Then
'                Return permissions(permissionIndex) = "1"
'            End If

'            Return False

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من الصلاحية: {ex.Message}")
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' تطبيق الصلاحيات على النموذج
'    ''' </summary>
'    Public Shared Sub ApplyPermissionsToForm(form As Form, userID As Integer, formName As String)
'        Try
'            ' إذا كان المستخدم مديراً، تفعيل جميع العناصر
'            If IsAdmin(userID) Then
'                EnableAllControls(form)
'                Return
'            End If

'            ' تطبيق الصلاحيات على الأزرار القياسية
'            ApplyStandardPermissions(form, userID, formName)

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق الصلاحيات: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' تفعيل جميع العناصر (للمديرين)
'    ''' </summary>
'    Private Shared Sub EnableAllControls(parent As Control)
'        For Each ctrl As Control In parent.Controls
'            ' تجاهل العناصر التي لا يمكن تعطيلها
'            If ctrl.GetType().GetProperty("Enabled") IsNot Nothing Then
'                ctrl.Enabled = True
'            End If

'            ' تطبيق التفعيل على العناصر الفرعية
'            If ctrl.HasChildren Then
'                EnableAllControls(ctrl)
'            End If
'        Next
'    End Sub

'    ''' <summary>
'    ''' تطبيق الصلاحيات على الأزرار القياسية
'    ''' </summary>
'    Private Shared Sub ApplyStandardPermissions(form As Form, userID As Integer, formName As String)
'        ' التحقق من صلاحية الإضافة (الرقم الثاني)
'        If form.Controls.ContainsKey("btnAdd") Then
'            CType(form.Controls("btnAdd"), Button).Enabled = HasPermission(userID, formName, 1)
'        End If

'        ' التحقق من صلاحية التعديل (الرقم الثالث)
'        If form.Controls.ContainsKey("btnUpdate") Then
'            CType(form.Controls("btnUpdate"), Button).Enabled = HasPermission(userID, formName, 2)
'        End If

'        ' التحقق من صلاحية الحذف (الرقم الرابع)
'        If form.Controls.ContainsKey("btnDelete") Then
'            CType(form.Controls("btnDelete"), Button).Enabled = HasPermission(userID, formName, 3)
'        End If

'        ' التحقق من صلاحية البحث (الرقم الخامس)
'        If form.Controls.ContainsKey("btnSearch") Then
'            CType(form.Controls("btnSearch"), Button).Enabled = HasPermission(userID, formName, 4)
'        End If

'        ' التحقق من صلاحية الطباعة (الرقم السادس)
'        If form.Controls.ContainsKey("btnPrint") Then
'            CType(form.Controls("btnPrint"), Button).Enabled = HasPermission(userID, formName, 5)
'        End If
'    End Sub

'    ''' <summary>
'    ''' حفظ صلاحيات المستخدم
'    ''' </summary>
'    Public Shared Function SavePermissions(userID As Integer, formName As String, permissions As String) As Boolean
'        Try
'            ' التأكد من أن الصلاحيات بطول 6 أرقام
'            If permissions.Length < 6 Then
'                permissions = permissions.PadRight(6, "0")
'            End If

'            ' حفظ الصلاحيات
'            Dim query As String = "MERGE INTO UserPermissions AS target " &
'                                  "USING (VALUES (@UserID, @FormName, @Permissions)) " &
'                                  "AS source (UserID, FormName, Permissions) " &
'                                  "ON (target.UserID = source.UserID AND target.FormName = source.FormName) " &
'                                  "WHEN MATCHED THEN " &
'                                  "UPDATE SET Permissions = source.Permissions " &
'                                  "WHEN NOT MATCHED THEN " &
'                                  "INSERT (UserID, FormName, Permissions) " &
'                                  "VALUES (source.UserID, source.FormName, source.Permissions);"

'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@UserID", userID),
'                DatabaseHelper.CreateParameter("@FormName", formName),
'                DatabaseHelper.CreateParameter("@Permissions", permissions.Substring(0, 6))
'            }

'            DatabaseHelper.ExecuteNonQuery(query, parameters)
'            Return True

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في حفظ الصلاحيات: {ex.Message}")
'            Return False
'        End Try
'    End Function
'End Class
