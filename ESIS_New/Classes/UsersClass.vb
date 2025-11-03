Imports System.Data.SqlClient
Imports ALquhaliLibrary.DBCLS
''' <summary>
''' كلاس لإدارة المستخدمين مع دعم تغيير كلمة المرور والتشفير
''' </summary>
Public Class UsersClass
    Private activityLogClass As New ActivityLogClass
    '''' <summary>
    '''' إضافة مستخدم جديد مع تشفير كلمة المرور
    '''' </summary>
    'Public Function AddUser(NumUser As Integer, UserName As String, Password As String, PicUser As Byte(), IsAdmin As Boolean, IsActive As Boolean) As Integer
    '    Try
    '        ' تشفير كلمة المرور قبل الإدخال
    '        Dim encryptedPassword As String = EncryptPassword(Password)

    '        Dim query As String = "INSERT INTO TbUsers (NumUser, UserName, Password, PicUser, IsAdmin, IsActive) VALUES (@NumUser, @UserName, @Password, @PicUser, @IsAdmin, @IsActive); SELECT SCOPE_IDENTITY();"

    '        ' إعداد المعلمات بشكل صحيح
    '        Dim parameters() As SqlParameter = {
    '            DatabaseHelper.CreateParameter("@NumUser", NumUser),
    '            DatabaseHelper.CreateParameter("@UserName", UserName),
    '            DatabaseHelper.CreateParameter("@Password", encryptedPassword),
    '            New SqlParameter("@PicUser", SqlDbType.VarBinary, -1) With {.Value = If(PicUser, DirectCast(DBNull.Value, Object))},
    '            DatabaseHelper.CreateParameter("@IsAdmin", IsAdmin),
    '            DatabaseHelper.CreateParameter("@IsActive", IsActive)
    '        }

    '        Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
    '        ' بعد النجاح
    '        activityLogClass.LogUserActivity(GlobalData.LoggedInUserName, "Add", "TbUsers", NumUser, $"تعديل مستخدم: {UserName}")
    '        Return Convert.ToInt32(result)

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في إضافة المستخدم: {ex.Message}")
    '        Throw
    '    End Try
    'End Function

    ''' <summary>
    ''' إضافة مستخدم جديد مع تشفير كلمة المرور
    ''' </summary>
    Public Function AddUser(NumUser As Integer, UserName As String, Password As String, PicUser As Byte(), IsAdmin As Boolean, IsActive As Boolean) As Integer
        Try
            ' تشفير كلمة المرور قبل الإدخال
            Dim encryptedPassword As String = Encrypt(Password)

            ' استخدام الإجراء المخزن "AddUser"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@NumUser", NumUser),
            DatabaseHelper.CreateParameter("@UserName", UserName),
            DatabaseHelper.CreateParameter("@Password", encryptedPassword),
            New SqlParameter("@PicUser", SqlDbType.VarBinary, -1) With {.Value = If(PicUser, DirectCast(DBNull.Value, Object))},
            DatabaseHelper.CreateParameter("@IsAdmin", IsAdmin),
            DatabaseHelper.CreateParameter("@IsActive", IsActive)
        }

            ' تنفيذ الإجراء المخزن واحصل على معرف المستخدم الجديد
            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("AddUser", parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim newUserId As Integer = Convert.ToInt32(dt.Rows(0)("UserID"))

                ' بعد النجاح
                activityLogClass.LogUserActivity(GlobalData.LoggedInUserName, "Add", "TbUsers", newUserId, $"تعديل مستخدم: {UserName}")
                Return newUserId
            Else
                Return 0
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إضافة المستخدم: {ex.Message}")
            Throw
        End Try
    End Function


    '''' <summary>
    '''' تحديث مستخدم مع تشفير كلمة المرور إذا تم تغييرها
    '''' </summary>
    'Public Function UpdateUser(ID As Integer, NumUser As Integer, UserName As String, Password As String, PicUser As Byte(), IsAdmin As Boolean, IsActive As Boolean) As Boolean
    '    Try
    '        Dim query As String = "UPDATE TbUsers SET NumUser = @NumUser, UserName = @UserName, "

    '        ' تحديد ما إذا تم تغيير كلمة المرور
    '        Dim passwordChanged As Boolean = Not String.IsNullOrEmpty(Password)

    '        If passwordChanged Then
    '            ' تشفير كلمة المرور الجديدة
    '            Dim encryptedPassword As String = EncryptPassword(Password)
    '            query &= "Password = @Password, "
    '        End If

    '        ' إضافة حقل الصورة فقط إذا كان هناك صورة جديدة
    '        If PicUser IsNot Nothing AndAlso PicUser.Length > 0 Then
    '            query &= "PicUser = @PicUser, "
    '        End If

    '        query &= "IsAdmin = @IsAdmin, IsActive = @IsActive WHERE ID = @ID;"

    '        ' إعداد المعلمات
    '        Dim parametersList As New List(Of SqlParameter) From {
    '            DatabaseHelper.CreateParameter("@ID", ID),
    '            DatabaseHelper.CreateParameter("@NumUser", NumUser),
    '            DatabaseHelper.CreateParameter("@UserName", UserName),
    '            DatabaseHelper.CreateParameter("@IsAdmin", IsAdmin),
    '            DatabaseHelper.CreateParameter("@IsActive", IsActive)
    '        }

    '        ' إضافة معلمة كلمة المرور فقط إذا تم تغييرها
    '        If passwordChanged Then
    '            parametersList.Add(DatabaseHelper.CreateParameter("@Password", EncryptPassword(Password)))
    '        End If

    '        ' إضافة معلمة الصورة فقط إذا كانت موجودة
    '        If PicUser IsNot Nothing AndAlso PicUser.Length > 0 Then
    '            parametersList.Add(New SqlParameter("@PicUser", SqlDbType.VarBinary, -1) With {.Value = PicUser})
    '        End If

    '        Dim result As Integer = DatabaseHelper.ExecuteNonQuery(query, parametersList.ToArray())
    '        ' بعد النجاح
    '        ActivityLogClass.LogUserActivity(GlobalData.LoggedInUserName, "Edit", "TbUsers", ID, $"تعديل مستخدم: {UserName}")
    '        Return result > 0

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تحديث المستخدم: {ex.Message}")
    '        Throw
    '    End Try
    'End Function

    ''' <summary>
    ''' تحديث مستخدم مع تشفير كلمة المرور إذا تم تغييرها
    ''' </summary>
    Public Function UpdateUser(ID As Integer, NumUser As Integer, UserName As String, Password As String, PicUser As Byte(), IsAdmin As Boolean, IsActive As Boolean) As Boolean
        Try
            ' تحديد ما إذا تم تغيير كلمة المرور
            Dim passwordChanged As Boolean = Not String.IsNullOrEmpty(Password)

            ' إعداد المعلمات
            Dim parametersList As New List(Of SqlParameter) From {
            DatabaseHelper.CreateParameter("@ID", ID),
            DatabaseHelper.CreateParameter("@NumUser", NumUser),
            DatabaseHelper.CreateParameter("@UserName", UserName),
            DatabaseHelper.CreateParameter("@IsAdmin", IsAdmin),
            DatabaseHelper.CreateParameter("@IsActive", IsActive)
        }

            ' إضافة معلمة كلمة المرور فقط إذا تم تغييرها
            If passwordChanged Then
                parametersList.Add(DatabaseHelper.CreateParameter("@Password", Encrypt(Password)))
            End If

            ' إضافة معلمة الصورة فقط إذا كانت موجودة
            If PicUser IsNot Nothing AndAlso PicUser.Length > 0 Then
                parametersList.Add(New SqlParameter("@PicUser", SqlDbType.VarBinary, -1) With {.Value = PicUser})
            End If

            ' استخدام الإجراء المخزن "UpdateUser"
            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("UpdateUser", parametersList.ToArray())

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim rowsAffected As Integer = Convert.ToInt32(dt.Rows(0)("RowsAffected"))

                ' بعد النجاح
                activityLogClass.LogUserActivity(GlobalData.LoggedInUserName, "Edit", "TbUsers", ID, $"تعديل مستخدم: {UserName}")
                Return rowsAffected > 0
            Else
                Return False
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث المستخدم: {ex.Message}")
            Throw
        End Try
    End Function


    ''' <summary>
    ''' تحديث كلمة مرور المستخدم بعد التحقق من كلمة المرور الحالية
    ''' </summary>
    Public Function ChangePassword(userID As Integer, currentPassword As String, newPassword As String) As Boolean
        Try
            ' التحقق من أن كلمات المرور ليست فارغة
            If String.IsNullOrEmpty(currentPassword) OrElse String.IsNullOrEmpty(newPassword) Then
                Return False
            End If

            ' تشفير كلمة المرور الجديدة
            Dim encryptedNewPassword As String = Encrypt(newPassword)

            ' استخدام الإجراء المخزن "ChangePassword"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@UserID", userID),
            DatabaseHelper.CreateParameter("@CurrentPassword", Encrypt(currentPassword)),
            DatabaseHelper.CreateParameter("@NewPassword", encryptedNewPassword)
        }

            ' تنفيذ الإجراء المخزن واحصل على عدد الصفوف المؤثرة
            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("ChangePassword", parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim rowsAffected As Integer = Convert.ToInt32(dt.Rows(0)("RowsAffected"))
                Return rowsAffected > 0
            Else
                Return False
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تغيير كلمة المرور: {ex.Message}")
            MessageBox.Show($"حدث خطأ أثناء تغيير كلمة المرور: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    '''' <summary>
    '''' تحديث كلمة مرور المستخدم بعد التحقق من كلمة المرور الحالية
    '''' </summary>
    'Public Function ChangePassword(userID As Integer, currentPassword As String, newPassword As String) As Boolean
    '    Try
    '        ' التحقق من أن كلمات المرور ليست فارغة
    '        If String.IsNullOrEmpty(currentPassword) OrElse String.IsNullOrEmpty(newPassword) Then
    '            Return False
    '        End If

    '        '' التحقق من طول كلمة المرور الجديدة
    '        'If newPassword.Length < 6 Then
    '        '    MessageBox.Show("يجب أن تكون كلمة المرور الجديدة على الأقل 6 أحرف.", "طول كلمة المرور", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        '    Return False
    '        'End If

    '        ' التحقق من كلمة المرور الحالية
    '        If Not VerifyPassword(userID, currentPassword) Then
    '            MessageBox.Show("كلمة المرور الحالية غير صحيحة.", "تحقق من كلمة المرور", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Return False
    '        End If

    '        ' التحقق مما إذا كانت كلمة المرور الجديدة مختلفة عن الحالية
    '        If VerifyPassword(userID, newPassword) Then
    '            MessageBox.Show("كلمة المرور الجديدة يجب أن تكون مختلفة عن كلمة المرور الحالية.", "تكرار كلمة المرور", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            Return False
    '        End If

    '        ' تشفير كلمة المرور الجديدة
    '        Dim encryptedNewPassword As String = EncryptPassword(newPassword)

    '        ' تحديث كلمة المرور
    '        Dim query As String = "UPDATE TbUsers SET Password = @NewPassword WHERE ID = @UserID"
    '        Dim parameters() As SqlParameter = {
    '            DatabaseHelper.CreateParameter("@UserID", userID),
    '            DatabaseHelper.CreateParameter("@NewPassword", encryptedNewPassword)
    '        }

    '        Dim result As Integer = DatabaseHelper.ExecuteNonQuery(query, parameters)
    '        Return result > 0

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تغيير كلمة المرور: {ex.Message}")
    '        MessageBox.Show($"حدث خطأ أثناء تغيير كلمة المرور: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        Return False
    '    End Try
    'End Function

    '''' <summary>
    '''' إعادة تعيين كلمة مرور المستخدم (للمديرين فقط)
    '''' </summary>
    'Public Function ResetPassword(userID As Integer, newPassword As String) As Boolean
    '    Try
    '        ' التحقق من أن كلمة المرور ليست فارغة
    '        If String.IsNullOrEmpty(newPassword) Then
    '            Return False
    '        End If

    '        '' التحقق من طول كلمة المرور الجديدة
    '        'If newPassword.Length < 6 Then
    '        '    MessageBox.Show("يجب أن تكون كلمة المرور الجديدة على الأقل 6 أحرف.", "طول كلمة المرور", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        '    Return False
    '        'End If

    '        ' تشفير كلمة المرور الجديدة
    '        Dim encryptedNewPassword As String = EncryptPassword(newPassword)

    '        ' تحديث كلمة المرور بدون الحاجة لكلمة المرور الحالية
    '        Dim query As String = "UPDATE TbUsers SET Password = @NewPassword WHERE ID = @UserID"
    '        Dim parameters() As SqlParameter = {
    '            DatabaseHelper.CreateParameter("@UserID", userID),
    '            DatabaseHelper.CreateParameter("@NewPassword", encryptedNewPassword)
    '        }

    '        Dim result As Integer = DatabaseHelper.ExecuteNonQuery(query, parameters)
    '        Return result > 0

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في إعادة تعيين كلمة المرور: {ex.Message}")
    '        MessageBox.Show($"حدث خطأ أثناء إعادة تعيين كلمة المرور: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        Return False
    '    End Try
    'End Function

    ''' <summary>
    ''' إعادة تعيين كلمة مرور المستخدم (للمديرين فقط)
    ''' </summary>
    Public Function ResetPassword(userID As Integer, newPassword As String) As Boolean
        Try
            ' التحقق من أن كلمة المرور ليست فارغة
            If String.IsNullOrEmpty(newPassword) Then
                Return False
            End If

            ' تشفير كلمة المرور الجديدة
            Dim encryptedNewPassword As String = Encrypt(newPassword)

            ' استخدام الإجراء المخزن "ResetPassword"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@UserID", userID),
            DatabaseHelper.CreateParameter("@NewPassword", encryptedNewPassword)
        }

            ' تنفيذ الإجراء المخزن واحصل على عدد الصفوف المؤثرة
            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("ResetPassword", parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim rowsAffected As Integer = Convert.ToInt32(dt.Rows(0)("RowsAffected"))
                Return rowsAffected > 0
            Else
                Return False
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إعادة تعيين كلمة المرور: {ex.Message}")
            MessageBox.Show($"حدث خطأ أثناء إعادة تعيين كلمة المرور: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    '''' <summary>
    '''' التحقق من كلمة المرور للمستخدم
    '''' </summary>
    'Public Function VerifyPassword(userID As Integer, password As String) As Boolean
    '    Try
    '        ' تشفير كلمة المرور للتحقق
    '        Dim encryptedPassword As String = EncryptPassword(password)

    '        ' استعلام للتحقق من كلمة المرور
    '        Dim query As String = "SELECT COUNT(*) FROM TbUsers WHERE ID = @UserID AND Password = @Password"
    '        Dim parameters() As SqlParameter = {
    '            DatabaseHelper.CreateParameter("@UserID", userID),
    '            DatabaseHelper.CreateParameter("@Password", encryptedPassword)
    '        }

    '        Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
    '        Return count > 0

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من كلمة المرور: {ex.Message}")
    '        Return False
    '    End Try
    'End Function

    ''' <summary>
    ''' التحقق من كلمة المرور للمستخدم
    ''' </summary>
    Public Function VerifyPassword(userID As Integer, password As String) As Boolean
        Try
            ' تشفير كلمة المرور للتحقق
            Dim encryptedPassword As String = Encrypt(password)

            ' استخدام الإجراء المخزن "VerifyPassword"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@UserID", userID),
            DatabaseHelper.CreateParameter("@Password", encryptedPassword)
        }

            ' تنفيذ الإجراء المخزن واحصل على نتيجة التحقق
            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("VerifyPassword", parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim isValid As Integer = Convert.ToInt32(dt.Rows(0)("IsValid"))
                Return isValid > 0
            Else
                Return False
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من كلمة المرور: {ex.Message}")
            Return False
        End Try
    End Function

    '''' <summary>
    '''' حذف مستخدم
    '''' </summary>
    'Public Function DeleteUser(ID As Integer) As Boolean
    '    Try
    '        Dim query As String = "DELETE FROM TbUsers WHERE ID = @ID;"
    '        Dim parameters() As SqlParameter = {
    '            DatabaseHelper.CreateParameter("@ID", ID)
    '        }

    '        Dim result As Integer = DatabaseHelper.ExecuteNonQuery(query, parameters)
    '        Return result > 0

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في حذف المستخدم: {ex.Message}")
    '        Return False
    '    End Try
    'End Function

    ''' <summary>
    ''' حذف مستخدم
    ''' </summary>
    Public Function DeleteUser(ID As Integer) As Boolean
        Try
            ' استخدام الإجراء المخزن "DeleteUser"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@ID", ID)
        }

            ' تنفيذ الإجراء المخزن واحصل على عدد الصفوف المؤثرة
            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("DeleteUser", parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim rowsAffected As Integer = Convert.ToInt32(dt.Rows(0)("RowsAffected"))
                Return rowsAffected > 0
            Else
                Return False
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حذف المستخدم: {ex.Message}")
            Return False
        End Try
    End Function


    '''' <summary>
    '''' جلب جميع المستخدمين
    '''' </summary>
    'Public Function GetAllUsers() As DataTable
    '    Try
    '        Dim query As String = "SELECT ID, NumUser, UserName, IsActive, IsAdmin FROM TbUsers ORDER BY UserName"
    '        Return DatabaseHelper.ExecuteQuery(query)

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في جلب المستخدمين: {ex.Message}")
    '        Throw
    '    End Try
    'End Function

    ''' <summary>
    ''' جلب جميع المستخدمين
    ''' </summary>
    Public Function GetAllUsers() As DataTable
        Try
            ' استخدام الإجراء المخزن "GetAllUsers"
            Return DatabaseHelper.ExecuteStoredProcedure("GetAllUsers")

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في جلب المستخدمين: {ex.Message}")
            Throw
        End Try
    End Function

    ''' <summary>
    ''' جلب مستخدم بواسطة المعرف
    ''' </summary>
    Public Function GetUserByID(ID As Integer) As DataTable
        Try
            ' استخدام الإجراء المخزن "GetUserByID"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@ID", ID)
        }

            Return DatabaseHelper.ExecuteStoredProcedure("GetUserByID", parameters)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في جلب المستخدم: {ex.Message}")
            Throw
        End Try
    End Function

    '''' <summary>
    '''' جلب مستخدم بواسطة المعرف
    '''' </summary>
    'Public Function GetUserByID(ID As Integer) As DataTable
    '    Try
    '        Dim query As String = "SELECT * FROM TbUsers WHERE ID = @ID"
    '        Dim parameters() As SqlParameter = {
    '            DatabaseHelper.CreateParameter("@ID", ID)
    '        }

    '        Return DatabaseHelper.ExecuteQuery(query, parameters)

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في جلب المستخدم: {ex.Message}")
    '        Throw
    '    End Try
    'End Function

    ''' <summary>
    ''' جلب صورة المستخدم
    ''' </summary>
    Public Function GetUserImage(userID As Integer) As Byte()
        Try
            ' استخدام الإجراء المخزن "GetUserImage"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@UserID", userID)
        }

            ' تنفيذ الإجراء المخزن واحصل على صورة المستخدم
            Dim dt As DataTable = DatabaseHelper.ExecuteStoredProcedure("GetUserImage", parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim result As Object = dt.Rows(0)("PicUser")
                If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                    Return DirectCast(result, Byte())
                End If
            End If

            Return Nothing

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في جلب صورة المستخدم: {ex.Message}")
            Return Nothing
        End Try
    End Function


    '''' <summary>
    '''' جلب مستخدم بواسطة رقم المستخدم
    '''' </summary>
    'Public Function GetUserByNum(NumUser As Integer) As DataTable
    '    Try
    '        Dim query As String = "SELECT * FROM TbUsers WHERE NumUser = @NumUser"
    '        Dim parameters() As SqlParameter = {
    '            DatabaseHelper.CreateParameter("@NumUser", NumUser)
    '        }

    '        Return DatabaseHelper.ExecuteQuery(query, parameters)

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في جلب المستخدم: {ex.Message}")
    '        Throw
    '    End Try
    'End Function

    ''' <summary>
    ''' جلب مستخدم بواسطة رقم المستخدم
    ''' </summary>
    Public Function GetUserByNum(NumUser As Integer) As DataTable
        Try
            ' استخدام الإجراء المخزن "GetUserByNum"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@NumUser", NumUser)
        }

            Return DatabaseHelper.ExecuteStoredProcedure("GetUserByNum", parameters)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في جلب المستخدم: {ex.Message}")
            Throw
        End Try
    End Function

    ''' <summary>
    ''' تشفير كلمة المرور باستخدام خوارزمية SHA256
    ''' </summary>
    Friend Function EncryptPassword(password As String) As String
        Try
            ' استخدام SHA256 لتشفير كلمة المرور
            Using sha256 As System.Security.Cryptography.SHA256 = System.Security.Cryptography.SHA256.Create()
                ' تحويل النص إلى بايت آري
                Dim bytes As Byte() = Text.Encoding.UTF8.GetBytes(password)

                ' تشفير البايت آري
                Dim hash As Byte() = sha256.ComputeHash(bytes)

                ' تحويل التوقيع الرقمي إلى سلسلة نصية
                Dim sb As New Text.StringBuilder()
                For Each b As Byte In hash
                    sb.Append(b.ToString("x2"))
                Next

                Return sb.ToString()
            End Using

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تشفير كلمة المرور: {ex.Message}")
            ' في حالة الخطأ، إعادة النص الأصلي (غير موصى به، فقط للتوضيح)
            Return password
        End Try
    End Function

    '''' <summary>
    '''' جلب صورة المستخدم
    '''' </summary>
    'Public Function GetUserImage(userID As Integer) As Byte()
    '    Try
    '        Dim query As String = "SELECT PicUser FROM TbUsers WHERE ID = @UserID"
    '        Dim parameters() As SqlParameter = {
    '            DatabaseHelper.CreateParameter("@UserID", userID)
    '        }

    '        Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
    '        If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
    '            Return DirectCast(result, Byte())
    '        End If

    '        Return Nothing

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في جلب صورة المستخدم: {ex.Message}")
    '        Return Nothing
    '    End Try
    'End Function
End Class



'Imports System.Data.SqlClient

'''' <summary>
'''' كلاس لإدارة المست	users مع دعم تشفير كلمات المرور
'''' </summary>
'Public Class UsersClass

'    ''' <summary>
'    ''' إضافة مستخدم جديد مع تشفير كلمة المرور
'    ''' </summary>
'    Public Function AddUser(NumUser As Integer, UserName As String, Password As String, PicUser As Byte(), IsAdmin As Boolean, IsActive As Boolean) As Integer
'        Try
'            ' تشفير كلمة المرور قبل الإدخال
'            Dim encryptedPassword As String = EncryptPassword(Password)

'            Dim query As String = "INSERT INTO TbUsers (NumUser, UserName, Password, PicUser, IsAdmin, IsActive) VALUES (@NumUser, @UserName, @Password, @PicUser, @IsAdmin, @IsActive); SELECT SCOPE_IDENTITY();"

'            ' إعداد المعلمات بشكل صحيح
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@NumUser", NumUser),
'                DatabaseHelper.CreateParameter("@UserName", UserName),
'                DatabaseHelper.CreateParameter("@Password", encryptedPassword),
'                New SqlParameter("@PicUser", SqlDbType.VarBinary, -1) With {.Value = If(PicUser, DirectCast(DBNull.Value, Object))},
'                DatabaseHelper.CreateParameter("@IsAdmin", IsAdmin),
'                DatabaseHelper.CreateParameter("@IsActive", IsActive)
'            }

'            Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
'            Return Convert.ToInt32(result)

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في إضافة المستخدم: {ex.Message}")
'            Throw
'        End Try
'    End Function

'    ''' <summary>
'    ''' تحديث مستخدم مع تشفير كلمة المرور إذا تم تغييرها
'    ''' </summary>
'    Public Function UpdateUser(ID As Integer, NumUser As Integer, UserName As String, Password As String, PicUser As Byte(), IsAdmin As Boolean, IsActive As Boolean) As Boolean
'        Try
'            Dim query As String = "UPDATE TbUsers SET NumUser = @NumUser, UserName = @UserName, "

'            ' تحديد ما إذا تم تغيير كلمة المرور
'            Dim passwordChanged As Boolean = Not String.IsNullOrEmpty(Password)

'            If passwordChanged Then
'                ' تشفير كلمة المرور الجديدة
'                Dim encryptedPassword As String = EncryptPassword(Password)
'                query &= "Password = @Password, "
'            End If

'            ' إضافة حقل الصورة فقط إذا كان هناك صورة جديدة
'            If PicUser IsNot Nothing AndAlso PicUser.Length > 0 Then
'                query &= "PicUser = @PicUser, "
'            End If

'            query &= "IsAdmin = @IsAdmin, IsActive = @IsActive WHERE ID = @ID;"

'            ' إعداد المعلمات
'            Dim parametersList As New List(Of SqlParameter) From {
'                DatabaseHelper.CreateParameter("@ID", ID),
'                DatabaseHelper.CreateParameter("@NumUser", NumUser),
'                DatabaseHelper.CreateParameter("@UserName", UserName),
'                DatabaseHelper.CreateParameter("@IsAdmin", IsAdmin),
'                DatabaseHelper.CreateParameter("@IsActive", IsActive)
'            }

'            ' إضافة معلمة كلمة المرور فقط إذا تم تغييرها
'            If passwordChanged Then
'                parametersList.Add(DatabaseHelper.CreateParameter("@Password", EncryptPassword(Password)))
'            End If

'            ' إضافة معلمة الصورة فقط إذا كانت موجودة
'            If PicUser IsNot Nothing AndAlso PicUser.Length > 0 Then
'                parametersList.Add(New SqlParameter("@PicUser", SqlDbType.VarBinary, -1) With {.Value = PicUser})
'            End If

'            Dim result As Integer = DatabaseHelper.ExecuteNonQuery(query, parametersList.ToArray())
'            Return result > 0

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث المستخدم: {ex.Message}")
'            Throw
'        End Try
'    End Function

'    ''' <summary>
'    ''' تحديث كلمة مرور المستخدم بعد التحقق من كلمة المرور الحالية
'    ''' </summary>
'    Public Function ChangePassword(userID As Integer, currentPassword As String, newPassword As String) As Boolean
'        Try
'            ' التحقق من أن كلمات المرور ليست فارغة
'            If String.IsNullOrEmpty(currentPassword) OrElse String.IsNullOrEmpty(newPassword) Then
'                Return False
'            End If

'            ' التحقق من طول كلمة المرور الجديدة
'            If newPassword.Length < 6 Then
'                MessageBox.Show("يجب أن تكون كلمة المرور الجديدة على الأقل 6 أحرف.", "طول كلمة المرور", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'                Return False
'            End If

'            ' التحقق من كلمة المرور الحالية
'            If Not VerifyPassword(userID, currentPassword) Then
'                MessageBox.Show("كلمة المرور الحالية غير صحيحة.", "تحقق من كلمة المرور", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                Return False
'            End If

'            ' التحقق مما إذا كانت كلمة المرور الجديدة مختلفة عن الحالية
'            If VerifyPassword(userID, newPassword) Then
'                MessageBox.Show("كلمة المرور الجديدة يجب أن تكون مختلفة عن كلمة المرور الحالية.", "تكرار كلمة المرور", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'                Return False
'            End If

'            ' تشفير كلمة المرور الجديدة
'            Dim encryptedNewPassword As String = EncryptPassword(newPassword)

'            ' تحديث كلمة المرور
'            Dim query As String = "UPDATE TbUsers SET Password = @NewPassword WHERE ID = @UserID"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@UserID", userID),
'                DatabaseHelper.CreateParameter("@NewPassword", encryptedNewPassword)
'            }

'            Dim result As Integer = DatabaseHelper.ExecuteNonQuery(query, parameters)
'            Return result > 0

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في تغيير كلمة المرور: {ex.Message}")
'            MessageBox.Show($"حدث خطأ أثناء تغيير كلمة المرور: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' التحقق من كلمة المرور للمستخدم
'    ''' </summary>
'    Public Function VerifyPassword(userID As Integer, password As String) As Boolean
'        Try
'            ' تشفير كلمة المرور للتحقق
'            Dim encryptedPassword As String = EncryptPassword(password)

'            ' استعلام للتحقق من كلمة المرور
'            Dim query As String = "SELECT COUNT(*) FROM TbUsers WHERE ID = @UserID AND Password = @Password"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@UserID", userID),
'                DatabaseHelper.CreateParameter("@Password", encryptedPassword)
'            }

'            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
'            Return count > 0

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من كلمة المرور: {ex.Message}")
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' حذف مستخدم
'    ''' </summary>
'    Public Function DeleteUser(ID As Integer) As Boolean
'        Try
'            Dim query As String = "DELETE FROM TbUsers WHERE ID = @ID;"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@ID", ID)
'            }

'            Dim result As Integer = DatabaseHelper.ExecuteNonQuery(query, parameters)
'            Return result > 0

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في حذف المستخدم: {ex.Message}")
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' جلب جميع المستخدمين
'    ''' </summary>
'    Public Function GetAllUsers() As DataTable
'        Try
'            Dim query As String = "SELECT ID, NumUser, UserName, IsActive, IsAdmin FROM TbUsers ORDER BY UserName"
'            Return DatabaseHelper.ExecuteQuery(query)

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في جلب المستخدمين: {ex.Message}")
'            Throw
'        End Try
'    End Function

'    ''' <summary>
'    ''' جلب مستخدم بواسطة المعرف
'    ''' </summary>
'    Public Function GetUserByID(ID As Integer) As DataTable
'        Try
'            Dim query As String = "SELECT * FROM TbUsers WHERE ID = @ID"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@ID", ID)
'            }

'            Return DatabaseHelper.ExecuteQuery(query, parameters)

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في جلب المستخدم: {ex.Message}")
'            Throw
'        End Try
'    End Function

'    ''' <summary>
'    ''' جلب مستخدم بواسطة رقم المستخدم
'    ''' </summary>
'    Public Function GetUserByNum(NumUser As Integer) As DataTable
'        Try
'            Dim query As String = "SELECT * FROM TbUsers WHERE NumUser = @NumUser"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@NumUser", NumUser)
'            }

'            Return DatabaseHelper.ExecuteQuery(query, parameters)

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في جلب المستخدم: {ex.Message}")
'            Throw
'        End Try
'    End Function

'    ''' <summary>
'    ''' تشفير كلمة المرور باستخدام خوارزمية SHA256
'    ''' </summary>
'    Friend Function EncryptPassword(password As String) As String
'        Try
'            ' استخدام SHA256 لتشفير كلمة المرور
'            Using sha256 As Security.Cryptography.SHA256 = Security.Cryptography.SHA256.Create()
'                ' تحويل النص إلى بايت آري
'                Dim bytes As Byte() = Text.Encoding.UTF8.GetBytes(password)

'                ' تشفير البايت آري
'                Dim hash As Byte() = sha256.ComputeHash(bytes)

'                ' تحويل التوقيع الرقمي إلى سلسلة نصية
'                Dim sb As New Text.StringBuilder()
'                For Each b As Byte In hash
'                    sb.Append(b.ToString("x2"))
'                Next

'                Return sb.ToString()
'            End Using

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في تشفير كلمة المرور: {ex.Message}")
'            ' في حالة الخطأ، إعادة النص الأصلي (غير موصى به، فقط للتوضيح)
'            Return password
'        End Try
'    End Function
'End Class

'''' <summary>
'''' كلاس لإدارة المستخدمين مع دعم الصور
'''' </summary>
'Public Class UsersClass

'    ''' <summary>
'    ''' إضافة مستخدم جديد مع صورة
'    ''' </summary>
'    Public Function AddUser(NumUser As Integer, UserName As String, Password As String, PicUser As Byte(), IsAdmin As Boolean, IsActive As Boolean) As Integer
'        Try
'            ' تشفير كلمة المرور قبل الإدخال
'            Dim encryptedPassword As String = EncryptPassword(Password)
'            Dim query As String = "INSERT INTO TbUsers (NumUser, UserName, Password, PicUser, IsAdmin, IsActive) VALUES (@NumUser, @UserName, @Password, @PicUser, @IsAdmin, @IsActive); SELECT SCOPE_IDENTITY();"

'            ' إعداد المعلمات بشكل صحيح
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@NumUser", NumUser),
'                DatabaseHelper.CreateParameter("@UserName", UserName),
'                DatabaseHelper.CreateParameter("@Password", Password),
'                New SqlParameter("@PicUser", SqlDbType.VarBinary, -1) With {.Value = If(PicUser, DirectCast(DBNull.Value, Object))},
'                DatabaseHelper.CreateParameter("@IsAdmin", IsAdmin),
'                DatabaseHelper.CreateParameter("@IsActive", IsActive)
'            }

'            Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
'            Return Convert.ToInt32(result)

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في إضافة المستخدم: {ex.Message}")
'            Throw
'        End Try
'    End Function

'    ''' <summary>
'    ''' تحديث مستخدم مع صورة
'    ''' </summary>
'    Public Function UpdateUser(ID As Integer, NumUser As Integer, UserName As String, Password As String, PicUser As Byte(), IsAdmin As Boolean, IsActive As Boolean) As Boolean
'        Try
'            Dim query As String = "UPDATE TbUsers SET NumUser = @NumUser, UserName = @UserName, Password = @Password, "

'            ' إضافة حقل الصورة فقط إذا كان هناك صورة جديدة
'            If PicUser IsNot Nothing AndAlso PicUser.Length > 0 Then
'                query &= "PicUser = @PicUser, "
'            End If

'            query &= "IsAdmin = @IsAdmin, IsActive = @IsActive WHERE ID = @ID;"

'            ' إعداد المعلمات
'            Dim parametersList As New List(Of SqlParameter) From {
'                DatabaseHelper.CreateParameter("@ID", ID),
'                DatabaseHelper.CreateParameter("@NumUser", NumUser),
'                DatabaseHelper.CreateParameter("@UserName", UserName),
'                DatabaseHelper.CreateParameter("@Password", Password),
'                DatabaseHelper.CreateParameter("@IsAdmin", IsAdmin),
'                DatabaseHelper.CreateParameter("@IsActive", IsActive)
'            }

'            ' إضافة معلمة الصورة فقط إذا كانت موجودة
'            If PicUser IsNot Nothing AndAlso PicUser.Length > 0 Then
'                parametersList.Add(New SqlParameter("@PicUser", SqlDbType.VarBinary, -1) With {.Value = PicUser})
'            End If

'            Dim result As Integer = DatabaseHelper.ExecuteNonQuery(query, parametersList.ToArray())
'            Return result > 0

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث المستخدم: {ex.Message}")
'            Throw
'        End Try
'    End Function

'    ''' <summary>
'    ''' جلب صورة المستخدم
'    ''' </summary>
'    Public Function GetUserImage(userID As Integer) As Byte()
'        Try
'            Dim query As String = "SELECT PicUser FROM TbUsers WHERE ID = @UserID"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@UserID", userID)
'            }

'            Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
'            If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
'                Return DirectCast(result, Byte())
'            End If

'            Return Nothing

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في جلب صورة المستخدم: {ex.Message}")
'            Return Nothing
'        End Try
'    End Function

'    ''' <summary>
'    ''' جلب جميع المستخدمين بدون الصورة (للجدول)
'    ''' </summary>
'    Public Function GetAllUsers() As DataTable
'        Try
'            Dim query As String = "SELECT ID, NumUser, UserName, IsActive, IsAdmin FROM TbUsers ORDER BY UserName"
'            Return DatabaseHelper.ExecuteQuery(query)

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في جلب المستخدمين: {ex.Message}")
'            Throw
'        End Try
'    End Function

'    ''' <summary>
'    ''' جلب بيانات المستخدم كاملة (بما في ذلك الصورة)
'    ''' </summary>
'    Public Function GetUserFullData(userID As Integer) As DataTable
'        Try
'            Dim query As String = "SELECT ID, NumUser, UserName, Password, PicUser, IsAdmin, IsActive FROM TbUsers WHERE ID = @UserID"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@UserID", userID)
'            }

'            Return DatabaseHelper.ExecuteQuery(query, parameters)

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في جلب بيانات المستخدم: {ex.Message}")
'            Throw
'        End Try
'    End Function

'    ''' <summary>
'    ''' التحقق من صحة بيانات تسجيل الدخول وإرجاع معرف المستخدم
'    ''' </summary>
'    ''' <param name="userName">اسم المستخدم</param>
'    ''' <param name="password">كلمة المرور</param>
'    ''' <returns>معرف المستخدم إذا كانت البيانات صحيحة، وإلا 0</returns>
'    Public Function ValidateUserAndGetID(userName As String, password As String) As Integer
'        Try
'            Dim query As String = "SELECT ID FROM TbUsers WHERE UserName = @UserName AND Password = @Password"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@UserName", userName),
'                DatabaseHelper.CreateParameter("@Password", password)
'            }

'            Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
'            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
'                Return Convert.ToInt32(result)
'            Else
'                Return 0
'            End If
'        Catch ex As Exception
'            Throw New Exception("خطأ في التحقق من بيانات المستخدم: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' حذف مستخدم
'    ''' </summary>
'    ''' <param name="id">معرف المستخدم</param>
'    ''' <returns>True إذا تم الحذف بنجاح</returns>
'    Public Function DeleteUser(id As Integer) As Boolean
'        Try
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@ID", id)
'            }

'            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("DeleteUser", parameters)
'            Return rowsAffected > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في حذف المستخدم: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' جلب مستخدم بواسطة رقم المستخدم
'    ''' </summary>
'    Public Function GetUserByNum(NumUser As Integer) As DataTable
'        Try
'            Dim query As String = "SELECT * FROM TbUsers WHERE NumUser = @NumUser"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@NumUser", NumUser)
'            }

'            Return DatabaseHelper.ExecuteQuery(query, parameters)

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في جلب المستخدم: {ex.Message}")
'            Throw
'        End Try
'    End Function
'End Class






'Imports System.Data.SqlClient

'Public Class UserClass

'#Region "دوال مساعدة"

'    ''' <summary>
'    ''' التحقق مما إذا كان المستخدم موجودًا
'    ''' </summary>
'    Public Function UserExists(username As String) As Boolean
'        Try
'            Dim parameters As New List(Of SqlParameter) From {
'                DatabaseHelper.CreateParameter("@UserName", username)
'            }
'            Dim result As DataTable = DatabaseHelper.ExecuteStoredProcedureQuery("UserExists", parameters.ToArray())
'            Return result.Rows.Count > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في التحقق من وجود المستخدم: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' جلب بيانات المستخدم بالاسم
'    ''' </summary>
'    Public Function GetUserByName(username As String) As DataTable
'        Try
'            Dim parameters As New List(Of SqlParameter) From {
'                DatabaseHelper.CreateParameter("@UserName", username)
'            }
'            Return DatabaseHelper.ExecuteStoredProcedureQuery("GetUserByName", parameters.ToArray())
'        Catch ex As Exception
'            Throw New Exception("خطأ في جلب بيانات المستخدم: " & ex.Message)
'        End Try
'    End Function


'    ''' <summary>
'    ''' إضافة مستخدم جديد
'    ''' </summary>
'    Public Function AddUser(username As String, password As String, isActive As Boolean) As Integer
'        Try
'            ' تشفير كلمة المرور
'            Dim hashedPassword As String = (password)

'            Dim parameters As New List(Of SqlParameter) From {
'                DatabaseHelper.CreateParameter("@UserName", username),
'                DatabaseHelper.CreateParameter("@Password", hashedPassword),
'                DatabaseHelper.CreateParameter("@IsActive", isActive)
'            }

'            ' تنفيذ الإجراء المخزن وإرجاع معرف المستخدم الجديد
'            Dim result As Object = DatabaseHelper.ExecuteScalar("AddUser", parameters.ToArray())
'            Return Convert.ToInt32(result)
'        Catch ex As Exception
'            Throw New Exception("خطأ في إضافة المستخدم: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' تحديث كلمة مرور المستخدم
'    ''' </summary>
'    Public Sub UpdateUserPassword(username As String, newPassword As String)
'        Try
'            ' تشفير كلمة المرور الجديدة
'            Dim hashedPassword As String = (newPassword)

'            Dim parameters As New List(Of SqlParameter) From {
'                DatabaseHelper.CreateParameter("@UserName", username),
'                DatabaseHelper.CreateParameter("@NewPassword", hashedPassword)
'            }

'            DatabaseHelper.ExecuteStoredProcedureNonQuery("UpdateUserPassword", parameters.ToArray())
'        Catch ex As Exception
'            Throw New Exception("خطأ في تحديث كلمة المرور: " & ex.Message)
'        End Try
'    End Sub

'#End Region


'    ''' <summary>
'    ''' إضافة مستخدم جديد
'    ''' </summary>
'    ''' <param name="numUser">رقم المستخدم</param>
'    ''' <param name="userName">اسم المستخدم</param>
'    ''' <param name="password">كلمة المرور</param>
'    ''' <param name="picUser">صورة المستخدم</param>
'    ''' <returns>True إذا تم الإضافة بنجاح</returns>
'    Public Function AddUser(numUser As Integer, userName As String, password As String, picUser As Byte()) As Boolean
'        Try
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@NumUser", numUser),
'                DatabaseHelper.CreateParameter("@UserName", userName),
'                DatabaseHelper.CreateParameter("@Password", password),
'                DatabaseHelper.CreateParameter("@PicUser", picUser, SqlDbType.VarBinary, -1)
'            }

'            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("AddUser", parameters)
'            Return rowsAffected > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في إضافة المستخدم: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' تحديث بيانات مستخدم
'    ''' </summary>
'    ''' <param name="id">معرف المستخدم</param>
'    ''' <param name="numUser">رقم المستخدم</param>
'    ''' <param name="userName">اسم المستخدم</param>
'    ''' <param name="password">كلمة المرور</param>
'    ''' <param name="picUser">صورة المستخدم</param>
'    ''' <returns>True إذا تم التحديث بنجاح</returns>
'    Public Function UpdateUser(id As Integer, numUser As Integer, userName As String, password As String, picUser As Byte()) As Boolean
'        Try
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@ID", id),
'                DatabaseHelper.CreateParameter("@NumUser", numUser),
'                DatabaseHelper.CreateParameter("@UserName", userName),
'                DatabaseHelper.CreateParameter("@Password", password),
'                DatabaseHelper.CreateParameter("@PicUser", picUser, SqlDbType.VarBinary, -1)
'            }

'            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("UpdateUser", parameters)
'            Return rowsAffected > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في تحديث المستخدم: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' حذف مستخدم
'    ''' </summary>
'    ''' <param name="id">معرف المستخدم</param>
'    ''' <returns>True إذا تم الحذف بنجاح</returns>
'    Public Function DeleteUser(id As Integer) As Boolean
'        Try
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@ID", id)
'            }

'            Dim rowsAffected As Integer = DatabaseHelper.ExecuteStoredProcedureNonQuery("DeleteUser", parameters)
'            Return rowsAffected > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في حذف المستخدم: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' البحث عن المستخدمين
'    ''' </summary>
'    ''' <param name="searchTerm">كلمة البحث</param>
'    ''' <returns>DataTable يحتوي على نتائج البحث</returns>
'    Public Function SearchUsers(searchTerm As String) As DataTable
'        Try
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@SearchTerm", searchTerm)
'            }

'            Return DatabaseHelper.ExecuteStoredProcedure("SearchUser", parameters)
'        Catch ex As Exception
'            Throw New Exception("خطأ في البحث عن المستخدمين: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' الحصول على جميع المستخدمين
'    ''' </summary>
'    ''' <returns>DataTable يحتوي على جميع المستخدمين</returns>
'    Public Function GetAllUsers() As DataTable
'        Try
'            Dim query As String = "SELECT ID, NumUser, UserName, PicUser FROM TbUsers ORDER BY NumUser"
'            Return DatabaseHelper.ExecuteQuery(query)
'        Catch ex As Exception
'            Throw New Exception("خطأ في الحصول على المستخدمين: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' التحقق من صحة بيانات تسجيل الدخول
'    ''' </summary>
'    ''' <param name="userName">اسم المستخدم</param>
'    ''' <param name="password">كلمة المرور</param>
'    ''' <returns>True إذا كانت البيانات صحيحة</returns>
'    Public Function ValidateUser(userName As String, password As String) As Boolean
'        Try
'            Dim query As String = "SELECT COUNT(*) FROM TbUsers WHERE UserName = @UserName AND Password = @Password"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@UserName", userName),
'                DatabaseHelper.CreateParameter("@Password", password)
'            }

'            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
'            Return count > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في التحقق من بيانات المستخدم: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' التحقق من صحة بيانات تسجيل الدخول وإرجاع معرف المستخدم
'    ''' </summary>
'    ''' <param name="userName">اسم المستخدم</param>
'    ''' <param name="password">كلمة المرور</param>
'    ''' <returns>معرف المستخدم إذا كانت البيانات صحيحة، وإلا 0</returns>
'    Public Function ValidateUserAndGetID(userName As String, password As String) As Integer
'        Try
'            Dim query As String = "SELECT ID FROM TbUsers WHERE UserName = @UserName AND Password = @Password"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@UserName", userName),
'                DatabaseHelper.CreateParameter("@Password", password)
'            }

'            Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
'            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
'                Return Convert.ToInt32(result)
'            Else
'                Return 0
'            End If
'        Catch ex As Exception
'            Throw New Exception("خطأ في التحقق من بيانات المستخدم: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' الحصول على بيانات مستخدم بواسطة المعرف
'    ''' </summary>
'    ''' <param name="id">معرف المستخدم</param>
'    ''' <returns>DataTable يحتوي على بيانات المستخدم</returns>
'    Public Function GetUserById(id As Integer) As DataTable
'        Try
'            Dim query As String = "SELECT * FROM TbUsers WHERE ID = @ID"
'            Dim parameters() As SqlParameter = {
'                DatabaseHelper.CreateParameter("@ID", id)
'            }

'            Return DatabaseHelper.ExecuteQuery(query, parameters)
'        Catch ex As Exception
'            Throw New Exception("خطأ في الحصول على بيانات المستخدم: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' التحقق من وجود رقم مستخدم
'    ''' </summary>
'    ''' <param name="numUser">رقم المستخدم</param>
'    ''' <param name="excludeId">معرف المستخدم المراد استثناؤه (للتحديث)</param>
'    ''' <returns>True إذا كان الرقم موجود</returns>
'    Public Function IsUserNumberExists(numUser As Integer, Optional excludeId As Integer = 0) As Boolean
'        Try
'            Dim query As String = "SELECT COUNT(*) FROM TbUsers WHERE NumUser = @NumUser"
'            If excludeId > 0 Then
'                query += " AND ID <> @ExcludeId"
'            End If

'            Dim parameters As New List(Of SqlParameter) From {
'                DatabaseHelper.CreateParameter("@NumUser", numUser)
'            }

'            If excludeId > 0 Then
'                parameters.Add(DatabaseHelper.CreateParameter("@ExcludeId", excludeId))
'            End If

'            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters.ToArray()))
'            Return count > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في التحقق من رقم المستخدم: " & ex.Message)
'        End Try
'    End Function

'    ''' <summary>
'    ''' التحقق من وجود اسم مستخدم
'    ''' </summary>
'    ''' <param name="userName">اسم المستخدم</param>
'    ''' <param name="excludeId">معرف المستخدم المراد استثناؤه (للتحديث)</param>
'    ''' <returns>True إذا كان الاسم موجود</returns>
'    Public Function IsUserNameExists(userName As String, Optional excludeId As Integer = 0) As Boolean
'        Try
'            Dim query As String = "SELECT COUNT(*) FROM TbUsers WHERE UserName = @UserName"
'            If excludeId > 0 Then
'                query += " AND ID <> @ExcludeId"
'            End If

'            Dim parameters As New List(Of SqlParameter) From {
'                DatabaseHelper.CreateParameter("@UserName", userName)
'            }

'            If excludeId > 0 Then
'                parameters.Add(DatabaseHelper.CreateParameter("@ExcludeId", excludeId))
'            End If

'            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters.ToArray()))
'            Return count > 0
'        Catch ex As Exception
'            Throw New Exception("خطأ في التحقق من اسم المستخدم: " & ex.Message)
'        End Try
'    End Function
'End Class

