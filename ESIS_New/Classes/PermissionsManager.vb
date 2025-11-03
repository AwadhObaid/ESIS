Imports System.Data.SqlClient

''' <summary>
''' كلاس موحد لإدارة الصلاحيات في نظام ESIS
''' </summary>
Public Class PermissionsManager
    ' نمط Singleton لضمان وجود نسخة واحدة فقط
    Private Shared _instance As PermissionsManager
    Private Shared ReadOnly _lockObject As New Object()

    ' متغير لتخزين صلاحيات المستخدم الحالي
    Private currentUserPermissions As Dictionary(Of String, Dictionary(Of String, Boolean))

    ''' <summary>
    ''' الحصول على النسخة الوحيدة من مدير الصلاحيات
    ''' </summary>
    Public Shared ReadOnly Property Instance As PermissionsManager
        Get
            If _instance Is Nothing Then
                SyncLock _lockObject
                    If _instance Is Nothing Then
                        _instance = New PermissionsManager()
                    End If
                End SyncLock
            End If
            Return _instance
        End Get
    End Property

    ''' <summary>
    ''' الباني الخاص
    ''' </summary>
    Private Sub New()
        currentUserPermissions = New Dictionary(Of String, Dictionary(Of String, Boolean))()
    End Sub

    ''' <summary>
    ''' تحميل صلاحيات المستخدم
    ''' </summary>
    Public Sub LoadUserPermissions(userId As Integer)
        Try
            currentUserPermissions.Clear()

            Dim query As String = "
            SELECT FormName, PermissionType 
            FROM TbPermissions 
            WHERE UserID = @UserID AND HasPermission = 1"

            Using connection As SqlConnection = DatabaseHelper.Con()
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@UserID", userId)

                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim formName As String = reader("FormName").ToString()
                            Dim permissionType As String = reader("PermissionType").ToString()

                            If Not currentUserPermissions.ContainsKey(formName) Then
                                currentUserPermissions(formName) = New Dictionary(Of String, Boolean)()
                            End If

                            currentUserPermissions(formName)(permissionType) = True
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل الصلاحيات: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' التحقق من صلاحية معينة
    ''' </summary>
    Public Function HasPermission(userId As Integer, formName As String, permissionType As String) As Boolean
        Try
            ' إذا لم يكن المستخدم الحالي هو نفس المستخدم، حمل الصلاحيات
            If userId <> GlobalData.LoggedInUserID Then
                LoadUserPermissions(userId)
            End If

            ' أولاً: التحقق مما إذا كان المستخدم هو المدير (ID = 1)
            If userId = 1 Then
                Return True
            End If

            ' ثانيًا: التحقق من وجود الصلاحيات
            If currentUserPermissions.ContainsKey(formName) Then
                Return currentUserPermissions(formName).ContainsKey(permissionType) AndAlso
                       currentUserPermissions(formName)(permissionType)
            Else
                Return False
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من الصلاحية: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التحقق من صلاحية الإضافة
    ''' </summary>
    Public Function CanAdd(formName As String) As Boolean
        Return HasPermission(GlobalData.LoggedInUserID, formName, "Add")
    End Function

    ''' <summary>
    ''' التحقق من صلاحية التعديل
    ''' </summary>
    Public Function CanEdit(formName As String) As Boolean
        Return HasPermission(GlobalData.LoggedInUserID, formName, "Edit")
    End Function

    ''' <summary>
    ''' التحقق من صلاحية الحذف
    ''' </summary>
    Public Function CanDelete(formName As String) As Boolean
        Return HasPermission(GlobalData.LoggedInUserID, formName, "Delete")
    End Function

    ''' <summary>
    ''' التحقق من صلاحية البحث
    ''' </summary>
    Public Function CanSearch(formName As String) As Boolean
        Return HasPermission(GlobalData.LoggedInUserID, formName, "Search")
    End Function

    ''' <summary>
    ''' التحقق من صلاحية الطباعة
    ''' </summary>
    Public Function CanPrint(formName As String) As Boolean
        Return HasPermission(GlobalData.LoggedInUserID, formName, "Print")
    End Function

    ''' <summary>
    ''' التحقق من صلاحية العرض
    ''' </summary>
    Public Function CanView(formName As String) As Boolean
        Return HasPermission(GlobalData.LoggedInUserID, formName, "View")
    End Function

    ''' <summary>
    ''' التحقق من صلاحية الأرشفة
    ''' </summary>
    Public Function CanArchive(formName As String) As Boolean
        Return HasPermission(GlobalData.LoggedInUserID, formName, "Archive")
    End Function

    ''' <summary>
    ''' التحقق من صلاحية إدارة المستخدمين
    ''' </summary>
    Public Function CanManageUsers() As Boolean
        Return HasPermission(GlobalData.LoggedInUserID, "*", "ManageUsers") OrElse
               HasPermission(GlobalData.LoggedInUserID, "UsersForm", "Edit")
    End Function

    ''' <summary>
    ''' التحقق من صلاحية إدارة الصلاحيات
    ''' </summary>
    Public Function CanManagePermissions() As Boolean
        Return HasPermission(GlobalData.LoggedInUserID, "*", "ManagePermissions") OrElse
               HasPermission(GlobalData.LoggedInUserID, "PermissionsForm", "Edit")
    End Function

    ''' <summary>
    ''' التحقق مما إذا كان المستخدم هو المدير
    ''' </summary>
    Public Function IsUserAdmin(userID As Integer) As Boolean
        Try
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

    ''' <summary>
    ''' تحديث صلاحيات المستخدم الحالي
    ''' </summary>
    Public Sub RefreshCurrentUserPermissions()
        LoadUserPermissions(GlobalData.LoggedInUserID)

        ' إرسال حدث لتحديث واجهة المستخدم
        PermissionEventManager.RaisePermissionsChanged()
    End Sub

    ''' <summary>
    ''' الحصول على جميع صلاحيات المستخدم
    ''' </summary>
    Public Function GetAllUserPermissions(userId As Integer) As DataTable
        Try
            ' تعديل الاستعلام ليعمل مع الهيكل الفعلي للقاعدة
            Dim query As String = "
        SELECT 
            p.ID, 
            p.UserID, 
            u.UserName, 
            p.FormName, 
            p.PermissionType, 
            p.HasPermission
        FROM TbPermissions p
        INNER JOIN TbUsers u ON p.UserID = u.ID
        WHERE p.UserID = @UserID
        ORDER BY p.FormName, p.PermissionType"

            Using connection As SqlConnection = DatabaseHelper.Con()
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@UserID", userId)

                    ' إنشاء جدول البيانات وإملاؤه مباشرة
                    Dim dataTable As New DataTable()
                    Using adapter As New SqlDataAdapter(command)
                        adapter.Fill(dataTable)
                    End Using

                    Return dataTable
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("خطأ في جلب صلاحيات المستخدم: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' حفظ صلاحية للمستخدم
    ''' </summary>
    Public Function SavePermission(userID As Integer, formName As String, permissionType As String, hasPermission As Boolean) As Boolean
        Try
            Dim query As String = "
            IF EXISTS (SELECT 1 FROM TbPermissions WHERE UserID = @UserID AND FormName = @FormName AND PermissionType = @PermissionType)
                UPDATE TbPermissions SET HasPermission = @HasPermission WHERE UserID = @UserID AND FormName = @FormName AND PermissionType = @PermissionType
            ELSE
                INSERT INTO TbPermissions (UserID, FormName, PermissionType, HasPermission) VALUES (@UserID, @FormName, @PermissionType, @HasPermission)"

            Using connection As SqlConnection = DatabaseHelper.Con()
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@UserID", userID)
                    command.Parameters.AddWithValue("@FormName", formName)
                    command.Parameters.AddWithValue("@PermissionType", permissionType)
                    command.Parameters.AddWithValue("@HasPermission", hasPermission)

                    Return command.ExecuteNonQuery() > 0
                End Using
            End Using

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حفظ الصلاحية: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' حذف جميع صلاحيات المستخدم
    ''' </summary>
    Public Function DeleteAllUserPermissions(userID As Integer) As Boolean
        Try
            Dim query As String = "DELETE FROM TbPermissions WHERE UserID = @UserID"
            Using connection As SqlConnection = DatabaseHelper.Con()
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@UserID", userID)
                    Return command.ExecuteNonQuery() >= 0
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حذف صلاحيات المستخدم: {ex.Message}")
            Return False
        End Try
    End Function
End Class




'Imports System.Data.SqlClient

'''' <summary>
'''' كلاس موحد لإدارة الصلاحيات في نظام ESIS
'''' </summary>
'Public Class PermissionsManager
'    ' نمط Singleton لضمان وجود نسخة واحدة فقط
'    Private Shared _instance As PermissionsManager
'    Private Shared ReadOnly _lockObject As New Object()

'    ' متغير لتخزين صلاحيات المستخدم الحالي
'    Private currentUserPermissions As Dictionary(Of String, Dictionary(Of String, Boolean))

'    ''' <summary>
'    ''' الحصول على النسخة الوحيدة من مدير الصلاحيات
'    ''' </summary>
'    Public Shared ReadOnly Property Instance As PermissionsManager
'        Get
'            If _instance Is Nothing Then
'                SyncLock _lockObject
'                    If _instance Is Nothing Then
'                        _instance = New PermissionsManager()
'                    End If
'                End SyncLock
'            End If
'            Return _instance
'        End Get
'    End Property

'    ''' <summary>
'    ''' الباني الخاص
'    ''' </summary>
'    Private Sub New()
'        currentUserPermissions = New Dictionary(Of String, Dictionary(Of String, Boolean))()
'    End Sub

'    ''' <summary>
'    ''' تحميل صلاحيات المستخدم
'    ''' </summary>
'    Public Sub LoadUserPermissions(userId As Integer)
'        Try
'            currentUserPermissions.Clear()

'            Dim query As String = "
'            SELECT FormName, PermissionType 
'            FROM TbPermissions 
'            WHERE UserID = @UserID AND IsActive = 1"

'            Using connection As SqlConnection = DatabaseHelper.Con()
'                connection.Open()
'                Using command As New SqlCommand(query, connection)
'                    command.Parameters.AddWithValue("@UserID", userId)

'                    Using reader As SqlDataReader = command.ExecuteReader()
'                        While reader.Read()
'                            Dim formName As String = reader("FormName").ToString()
'                            Dim permissionType As String = reader("PermissionType").ToString()

'                            If Not currentUserPermissions.ContainsKey(formName) Then
'                                currentUserPermissions(formName) = New Dictionary(Of String, Boolean)()
'                            End If

'                            currentUserPermissions(formName)(permissionType) = True
'                        End While
'                    End Using
'                End Using
'            End Using

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل الصلاحيات: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' التحقق من صلاحية معينة
'    ''' </summary>
'    Public Function HasPermission(userId As Integer, formName As String, permissionType As String) As Boolean
'        Try
'            ' إذا لم يكن المستخدم الحالي هو نفس المستخدم، حمل الصلاحيات
'            If userId <> GlobalData.LoggedInUserID Then
'                LoadUserPermissions(userId)
'            End If

'            ' تحقق من وجود الصلاحيات
'            If currentUserPermissions.ContainsKey(formName) Then
'                Return currentUserPermissions(formName).ContainsKey(permissionType) AndAlso
'                       currentUserPermissions(formName)(permissionType)
'            Else
'                Return False
'            End If

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من الصلاحية: {ex.Message}")
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' التحقق من صلاحية الإضافة
'    ''' </summary>
'    Public Function CanAdd(formName As String) As Boolean
'        Return HasPermission(GlobalData.LoggedInUserID, formName, "Add")
'    End Function

'    ''' <summary>
'    ''' التحقق من صلاحية التعديل
'    ''' </summary>
'    Public Function CanEdit(formName As String) As Boolean
'        Return HasPermission(GlobalData.LoggedInUserID, formName, "Edit")
'    End Function

'    ''' <summary>
'    ''' التحقق من صلاحية الحذف
'    ''' </summary>
'    Public Function CanDelete(formName As String) As Boolean
'        Return HasPermission(GlobalData.LoggedInUserID, formName, "Delete")
'    End Function

'    ''' <summary>
'    ''' التحقق من صلاحية البحث
'    ''' </summary>
'    Public Function CanSearch(formName As String) As Boolean
'        Return HasPermission(GlobalData.LoggedInUserID, formName, "Search")
'    End Function

'    ''' <summary>
'    ''' التحقق من صلاحية الطباعة
'    ''' </summary>
'    Public Function CanPrint(formName As String) As Boolean
'        Return HasPermission(GlobalData.LoggedInUserID, formName, "Print")
'    End Function

'    ''' <summary>
'    ''' التحقق من صلاحية الأرشفة
'    ''' </summary>
'    Public Function CanArchive(formName As String) As Boolean
'        Return HasPermission(GlobalData.LoggedInUserID, formName, "Archive")
'    End Function

'    ''' <summary>
'    ''' التحقق من صلاحية العرض
'    ''' </summary>
'    Public Function CanView(formName As String) As Boolean
'        Return HasPermission(GlobalData.LoggedInUserID, formName, "View")
'    End Function

'    ''' <summary>
'    ''' تحديث صلاحيات المستخدم الحالي
'    ''' </summary>
'    Public Sub RefreshCurrentUserPermissions()
'        LoadUserPermissions(GlobalData.LoggedInUserID)

'        ' إرسال حدث لتحديث واجهة المستخدم
'        PermissionEventManager.RaisePermissionsChanged()
'    End Sub

'    ''' <summary>
'    ''' الحصول على جميع صلاحيات المستخدم (لنموذج تسجيل الدخول)
'    ''' </summary>
'    Public Function GetPermissionByUserID(userID As Integer) As DataTable
'        Try
'            Dim query As String = "EXEC GetPermissionByUserID @UserID"

'            ' إنشاء مصفوفة من المعلمات
'            Dim parameters() As SqlParameter = {
'            New SqlParameter("@UserID", userID)
'        }

'            ' استخدام الدالة مع السلسلة النصية والمعلمات
'            Return DatabaseHelper.ExecuteQuery(query, parameters)

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في جلب صلاحيات المستخدم: {ex.Message}")
'            Return New DataTable() ' إرجاع جدول فارغ في حالة الخطأ
'        End Try
'    End Function

'    ''' <summary>
'    ''' التحقق مما إذا كان المستخدم مشرفًا
'    ''' </summary>
'    Public Function IsUserAdmin(userID As Integer) As Boolean
'        Try
'            ' التحقق من أن المستخدم لديه صلاحيات إدارة الصلاحيات أو المستخدمين
'            Dim hasManageUsers As Boolean = HasPermission(userID, "*", "ManageUsers") OrElse
'                                           HasPermission(userID, "UsersForm", "Edit")
'            Dim hasManagePermissions As Boolean = HasPermission(userID, "*", "ManagePermissions") OrElse
'                                                 HasPermission(userID, "PermissionsForm", "Edit")

'            Return hasManageUsers Or hasManagePermissions

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من صلاحية المشرف: {ex.Message}")
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' حفظ صلاحية للمستخدم
'    ''' </summary>
'    Public Function SavePermissions(userID As Integer, formName As String, permissionType As String, isActive As Boolean) As Boolean
'        Try
'            Dim query As String = "
'            IF EXISTS (SELECT 1 FROM TbPermissions WHERE UserID = @UserID AND FormName = @FormName AND PermissionType = @PermissionType)
'                UPDATE TbPermissions SET IsActive = @IsActive WHERE UserID = @UserID AND FormName = @FormName AND PermissionType = @PermissionType
'            ELSE
'                INSERT INTO TbPermissions (UserID, FormName, PermissionType, IsActive) VALUES (@UserID, @FormName, @PermissionType, @IsActive)"

'            Using connection As SqlConnection = DatabaseHelper.Con()
'                connection.Open()
'                Using command As New SqlCommand(query, connection)
'                    command.Parameters.AddWithValue("@UserID", userID)
'                    command.Parameters.AddWithValue("@FormName", formName)
'                    command.Parameters.AddWithValue("@PermissionType", permissionType)
'                    command.Parameters.AddWithValue("@IsActive", isActive)

'                    Return command.ExecuteNonQuery() > 0
'                End Using
'            End Using

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في حفظ الصلاحيات: {ex.Message}")
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' حذف جميع صلاحيات المستخدم
'    ''' </summary>
'    Public Function DeleteAllUserPermissions(userID As Integer) As Boolean
'        Try
'            Dim query As String = "DELETE FROM TbPermissions WHERE UserID = @UserID"
'            Using connection As SqlConnection = DatabaseHelper.Con()
'                connection.Open()
'                Using command As New SqlCommand(query, connection)
'                    command.Parameters.AddWithValue("@UserID", userID)
'                    Return command.ExecuteNonQuery() >= 0
'                End Using
'            End Using
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في حذف صلاحيات المستخدم: {ex.Message}")
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' الحصول على جميع صلاحيات المستخدم
'    ''' </summary>
'    Public Function GetAllUserPermissions(userId As Integer) As DataTable
'        Try
'            Dim query As String = "
'        SELECT p.ID, p.UserID, u.UserName, p.FormName, p.PermissionType, p.IsActive,
'               f.FormTitle, f.FormDescription
'        FROM TbPermissions p
'        INNER JOIN TbUsers u ON p.UserID = u.ID
'        LEFT JOIN TbForms f ON p.FormName = f.FormName
'        WHERE p.UserID = @UserID
'        ORDER BY p.FormName, p.PermissionType"

'            ' إنشاء مصفوفة من المعلمات
'            Dim parameters() As SqlParameter = {
'            New SqlParameter("@UserID", userId)
'        }

'            ' استخدام الدالة مع السلسلة النصية والمعلمات
'            Return DatabaseHelper.ExecuteQuery(query, parameters)

'        Catch ex As Exception
'            Throw New Exception("خطأ في جلب صلاحيات المستخدم: " & ex.Message)
'        End Try
'    End Function
'End Class
