Imports System.Data.SqlClient
Imports System.Windows.Forms



'Namespace ElectronicSystemforIntegratedServices

''' <summary>
''' نموذج أساسي مع دعم الصلاحيات
''' </summary>
Public Class BaseFormWithPermissions
        Inherits Form

        ''' <summary>
        ''' تطبيق الصلاحيات العامة
        ''' </summary>
        Public Overridable Sub ApplyPermissions()
            Try
                ' تطبيق الصلاحيات الأساسية
                ApplyBasicPermissions()

                ' تطبيق الصلاحيات الخاصة بكل نموذج
                ApplyCustomPermissions()

                ' تسجيل معالج حدث تغيير الصلاحيات
                PermissionEventManager.RegisterPermissionsChangedHandler(AddressOf OnPermissionsChanged)

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق الصلاحيات: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' تطبيق الصلاحيات الأساسية
        ''' </summary>
        Protected Overridable Sub ApplyBasicPermissions()
            ' يمكن إضافة صلاحيات أساسية مشتركة هنا
        End Sub

        ''' <summary>
        ''' تطبيق الصلاحيات المخصصة لكل نموذج
        ''' </summary>
        Protected Overridable Sub ApplyCustomPermissions()
            ' يجب تنفيذها في الفئات المشتقة
        End Sub

        ''' <summary>
        ''' معالجة حدث تغيير الصلاحيات
        ''' </summary>
        Private Sub OnPermissionsChanged(sender As Object, e As EventArgs)
            Try
                ' تحديث صلاحيات المستخدم الحالي
                PermissionsManager.Instance.RefreshCurrentUserPermissions()

                ' إعادة تطبيق الصلاحيات
                ApplyPermissions()

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"خطأ في معالجة تغيير الصلاحيات: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' عند تحميل النموذج
        ''' </summary>
        Protected Overrides Sub OnLoad(e As EventArgs)
            MyBase.OnLoad(e)

            ' تطبيق الصلاحيات عند التحميل
            If Not DesignMode Then
                ApplyPermissions()

                ' تسجيل معالج حدث تغيير الصلاحيات
                PermissionEventManager.RegisterPermissionsChangedHandler(AddressOf OnPermissionsChanged)
            End If
        End Sub

        ''' <summary>
        ''' عند إغلاق النموذج
        ''' </summary>
        Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
            ' إلغاء تسجيل معالج الحدث
            Try
                PermissionEventManager.UnregisterPermissionsChangedHandler(AddressOf OnPermissionsChanged)
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"خطأ في إلغاء تسجيل معالج الحدث: {ex.Message}")
            End Try

            MyBase.OnFormClosing(e)
        End Sub

        ''' <summary>
        ''' التحقق من صلاحية معينة
        ''' </summary>
        Protected Function CheckPermission(permissionType As String, friendlyName As String) As Boolean
            Try
                ' أولاً: التحقق مما إذا كان المستخدم هو المدير
                Dim isAdmin As Boolean = False
                Try
                    Dim query As String = "SELECT IsAdmin FROM TbUsers WHERE ID = @UserID AND IsActive = 1"
                    Using connection As SqlConnection = DatabaseHelper.Con()
                        connection.Open()
                        Using command As New SqlCommand(query, connection)
                            command.Parameters.AddWithValue("@UserID", GlobalData.LoggedInUserID)
                            Dim result As Object = command.ExecuteScalar()
                            isAdmin = If(result IsNot Nothing, Convert.ToBoolean(result), False)
                        End Using
                    End Using
                Catch userEx As Exception
                    System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من كون المستخدم مديرًا: {userEx.Message}")
                    isAdmin = False ' الافتراضي
                End Try

                ' إذا كان المدير، لديه جميع الصلاحيات
                If isAdmin Then
                    Return True
                End If

                ' ثانيًا: التحقق من الصلاحية العادية
                Dim hasPermission As Boolean = False
                Try
                    Dim query As String = "
            SELECT HasPermission 
            FROM TbPermissions 
            WHERE UserID = @UserID AND FormName = @FormName AND PermissionType = @PermissionType"

                    Using connection As SqlConnection = DatabaseHelper.Con()
                        connection.Open()
                        Using command As New SqlCommand(query, connection)
                            command.Parameters.AddWithValue("@UserID", GlobalData.LoggedInUserID)
                            command.Parameters.AddWithValue("@FormName", Me.GetType().Name)
                            command.Parameters.AddWithValue("@PermissionType", permissionType)

                            Dim result As Object = command.ExecuteScalar()
                            hasPermission = If(result IsNot Nothing, Convert.ToBoolean(result), False)
                        End Using
                    End Using
                Catch permEx As Exception
                    System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من الصلاحية العادية: {permEx.Message}")
                    hasPermission = False ' الافتراضي
                End Try

                If Not hasPermission Then
                    MessageBox.Show($"ليس لديك صلاحية {friendlyName}. يرجى الاتصال بمدير النظام.",
                          "صلاحيات محدودة", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                Return hasPermission

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"خطأ عام في التحقق من الصلاحية: {ex.Message}")
                Return False
            End Try
        End Function




        '''' <summary>
        '''' التحقق من صلاحية معينة
        '''' </summary>
        'Protected Function CheckPermission(permissionType As String, friendlyName As String) As Boolean
        '    Try
        '        Dim hasPermission As Boolean = PermissionsManager.Instance.HasPermission(
        '        GlobalData.LoggedInUserID, Me.GetType().Name, permissionType)

        '        If Not hasPermission Then
        '            MessageBox.Show($"ليس لديك صلاحية {friendlyName}. يرجى الاتصال بمدير النظام.",
        '                      "صلاحيات محدودة", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '        End If

        '        Return hasPermission

        '    Catch ex As Exception
        '        System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من الصلاحية: {ex.Message}")
        '        Return False
        '    End Try
        'End Function

    End Class

'End Namespace