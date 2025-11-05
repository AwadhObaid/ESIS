Imports System.Data.SqlClient
Imports System.IO
Imports ALquhaliLibrary.DBCLS

Public Class LoginForm
    ' متغيرات السحب والنقل للنافذة
    Private Drag As Boolean = False
    Private MouseX As Integer
    Private MouseY As Integer

    Dim userclass As New UsersClass

    ''' <summary>
    ''' التحقق مما إذا كان هذا هو التشغيل الأولي للبرنامج
    ''' </summary>
    Private Function IsFirstRun() As Boolean
        Return String.IsNullOrEmpty(My.Settings.ServerName) AndAlso
               String.IsNullOrEmpty(My.Settings.DataBaseName)
    End Function

#Region "الأحداث والتحميل"

    ''' <summary>
    ''' حدث تحميل النموذج
    ''' </summary>
    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' التحقق مما إذا كان هذا هو التشغيل الأولي
        If IsFirstRun() Then
            ' يجب فتح نموذج التثبيت أولاً
            Me.Close()
            Dim wizardForm As New InstallWizardForm()
            wizardForm.ShowDialog()
            Return
        End If

        Me.Text = "تسجيل الدخول - نظام ESIS"
        Me.RightToLeft = RightToLeft.Yes
        Me.RightToLeftLayout = True
        ComboUsername.Focus()
        fillCombo(ComboUsername, "SELECT UserName FROM TbUsers ORDER BY UserName", "UserName")
        LoadUserPermissions(GlobalData.LoggedInUserID)
    End Sub

    ''' <summary>
    ''' حدث الضغط على زر تسجيل الدخول
    ''' </summary>
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Try
            ' التحقق من المدخلات
            If Not ValidateInput() Then
                Return
            End If

            ' محاولة تسجيل الدخول
            Dim loginResult As LoginStatus = AttemptLogin()

            Select Case loginResult
                Case LoginStatus.Success

                    ' عرض رسالة الترحيب
                    Dim frm As New LoginMessegeForm
                    frm.lblUser.Text = GlobalData.LoggedInUserName
                    frm.ShowDialog()

                    ' إغلاق نموذج تسجيل الدخول
                    Me.Close()
                    ' تحميل صورة المستخدم بعد تسجيل الدخول الناجح
                    LoadCurrentUserImage()

                    ' البحث عن نموذج الرئيسي المفتوح
                    Dim mainForm As MainForm = TryCast(Application.OpenForms("MainForm"), MainForm)

                    If mainForm IsNot Nothing Then
                        ' إذا كان النموذج مفتوحًا، قم بتحديثه
                        mainForm.RefreshData()
                        mainForm.BringToFront()
                    Else
                        ' إذا كان النموذج مغلقًا، قم بفتحه
                        Dim newMainForm As New MainForm()
                        newMainForm.Show()
                    End If

                Case LoginStatus.InvalidCredentials
                    MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة", "بيانات خاطئة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtPassword.Clear()
                    txtPassword.Focus()

                Case LoginStatus.UserNotActive
                    MessageBox.Show("هذا الحساب غير نشط. يرجى الاتصال بمدير النظام.", "حساب غير نشط", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ComboUsername.SelectedIndex = -1
                    txtPassword.Clear()
                    ComboUsername.Focus()

                Case LoginStatus.UserNotFound
                    MessageBox.Show("اسم المستخدم غير موجود في النظام", "مستخدم غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    ComboUsername.SelectedIndex = -1
                    txtPassword.Clear()
                    ComboUsername.Focus()

                Case LoginStatus.DatabaseError
                    MessageBox.Show("حدث خطأ في قاعدة البيانات. يرجى المحاولة لاحقًا.", "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Case Else
                    MessageBox.Show("حدث خطأ غير متوقع أثناء تسجيل الدخول.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select

        Catch ex As Exception
            MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    'Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
    '    Try
    '        ' التحقق من المدخلات
    '        If Not ValidateInput() Then
    '            Return
    '        End If

    '        ' محاولة تسجيل الدخول
    '        Dim loginResult As LoginStatus = AttemptLogin()

    '        Select Case loginResult
    '            Case LoginStatus.Success
    '                Dim main As MainForm = TryCast(Application.OpenForms("MainForm"), MainForm)
    '                If main IsNot Nothing Then
    '                    Dim frm As New LoginMessegeForm
    '                    frm.lblUser.Text = GlobalData.LoggedInUserName
    '                    frm.ShowDialog()
    '                    ' تحميل صورة المستخدم بعد تسجيل الدخول الناجح
    '                    LoadCurrentUserImage()
    '                    DialogResult = DialogResult.OK
    '                    Me.Close()
    '                    main.MainForm_Load(Nothing, Nothing)
    '                Else
    '                    Dim frm As New LoginMessegeForm
    '                    frm.lblUser.Text = GlobalData.LoggedInUserName
    '                    frm.ShowDialog()
    '                    ' تحميل صورة المستخدم بعد تسجيل الدخول الناجح
    '                    LoadCurrentUserImage()
    '                    DialogResult = DialogResult.OK
    '                    Me.Close()
    '                End If


    '            Case LoginStatus.InvalidCredentials
    '                MessageBox.Show("اسم المستخدم أو كلمة المرور غير صحيحة", "بيانات خاطئة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '                txtPassword.Clear()
    '                txtPassword.Focus()

    '            Case LoginStatus.UserNotActive
    '                MessageBox.Show("هذا الحساب غير نشط. يرجى الاتصال بمدير النظام.", "حساب غير نشط", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '                ComboUsername.SelectedIndex = -1
    '                txtPassword.Clear()
    '                ComboUsername.Focus()

    '            Case LoginStatus.UserNotFound
    '                MessageBox.Show("اسم المستخدم غير موجود في النظام", "مستخدم غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '                ComboUsername.SelectedIndex = -1
    '                txtPassword.Clear()
    '                ComboUsername.Focus()

    '            Case LoginStatus.DatabaseError
    '                MessageBox.Show("حدث خطأ في قاعدة البيانات. يرجى المحاولة لاحقًا.", "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '            Case Else
    '                MessageBox.Show("حدث خطأ غير متوقع أثناء تسجيل الدخول.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Select

    '    Catch ex As Exception
    '        MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    ''' <summary>
    ''' حدث الضغط على زر إلغاء
    ''' </summary>
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Application.Exit()
    End Sub

#End Region

#Region "وظائف المصادقة والتحقق"

    ''' <summary>
    ''' مصادقة المستخدم باستخدام اسم المستخدم وكلمة المرور
    ''' </summary>
    Private Function AuthenticateUser(username As String, password As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM TbUsers 
                        WHERE UserName = @UserName AND Password = @Password"

        Dim params() As SqlParameter = {
            New SqlParameter("@UserName", username),
            New SqlParameter("@Password", password)
        }

        Dim result As Integer = CInt(DatabaseHelper.ExecuteScalar(query, params, False))
        Return result > 0
    End Function

    ''' <summary>
    ''' الحصول على معرف المستخدم بناءً على اسم المستخدم
    ''' </summary>
    Private Function GetUserID(username As String) As Integer
        Dim query As String = "SELECT ID FROM TbUsers WHERE UserName = @UserName"
        Dim params() As SqlParameter = {New SqlParameter("@UserName", username)}

        Dim result As Object = DatabaseHelper.ExecuteScalar(query, params, False)
        If result IsNot Nothing Then
            Return CInt(result)
        End If
        Return 0
    End Function

#End Region

#Region "أحداث لوحة المفاتيح وإدخال البيانات"

    ''' <summary>
    ''' حدث الضغط على مفتاح Enter في حقل اسم المستخدم
    ''' </summary>
    Private Sub ComboUsername_KeyDown(sender As Object, e As KeyEventArgs) Handles ComboUsername.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtPassword.Focus()
        End If
    End Sub

    ''' <summary>
    ''' حدث الضغط على مفتاح Enter في حقل كلمة المرور
    ''' </summary>
    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnLogin.PerformClick()
        End If
    End Sub

    ''' <summary>
    ''' عرض/إخفاء كلمة المرور عند تغيير حالة مربع الاختيار
    ''' </summary>
    Private Sub CbShowPass_CheckedChanged(sender As Object, e As EventArgs) Handles cbShowPass.CheckedChanged
        txtPassword.PasswordChar = If(cbShowPass.Checked, "", "*"c)
        cbShowPass.Text = If(cbShowPass.Checked, "إخفاء", "عرض")
    End Sub

    ''' <summary>
    ''' منع المسافات في كلمة المرور
    ''' </summary>
    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = " "c Then e.Handled = True
    End Sub

#End Region

#Region "أحداث التركيز والإظهار"

    ''' <summary>
    ''' حدث دخول تركيز على حقل اسم المستخدم
    ''' </summary>
    Private Sub ComboUsername_Enter(sender As Object, e As EventArgs) Handles ComboUsername.Enter
        If ComboUsername.Text = "إسم المستخدم" Then
            ComboUsername.ForeColor = Color.Red
            ComboUsername.Text = ""
        End If
    End Sub

    ''' <summary>
    ''' حدث مغادرة تركيز عن حقل اسم المستخدم
    ''' </summary>
    Private Sub ComboUsername_Leave(sender As Object, e As EventArgs) Handles ComboUsername.Leave
        If String.IsNullOrEmpty(ComboUsername.Text) Then
            ComboUsername.ForeColor = Color.Green
            ComboUsername.Text = "إسم المستخدم"
        End If
    End Sub

    ''' <summary>
    ''' حدث دخول تركيز على حقل كلمة المرور
    ''' </summary>
    Private Sub txtPassword_Enter(sender As Object, e As EventArgs) Handles txtPassword.Enter
        If txtPassword.Text = "كلمة المرور" Then
            txtPassword.ForeColor = Color.Green
            txtPassword.PasswordChar = "*"c
            txtPassword.Text = ""
        End If
    End Sub

    ''' <summary>
    ''' حدث مغادرة تركيز عن حقل كلمة المرور
    ''' </summary>
    Private Sub txtPassword_Leave(sender As Object, e As EventArgs) Handles txtPassword.Leave
        If String.IsNullOrEmpty(txtPassword.Text) Then
            txtPassword.ForeColor = Color.Green
            txtPassword.PasswordChar = ""
            txtPassword.Text = "كلمة المرور"
        End If
    End Sub

    ''' <summary>
    ''' عرض القائمة عند تمرير الماوس فوق حقل اسم المستخدم
    ''' </summary>
    Private Sub ComboUsername_MouseHover(sender As Object, e As EventArgs) Handles ComboUsername.MouseHover
        ComboUsername.DroppedDown = True
    End Sub

#End Region

#Region "أحداث السحب والنقل"

    ''' <summary>
    ''' بدء عملية السحب عند الضغط على رأس النافذة
    ''' </summary>
    Private Sub Panel3_MouseDown(sender As Object, e As MouseEventArgs) Handles Panel3.MouseDown
        Drag = True
        MouseX = Cursor.Position.X - Me.Left
        MouseY = Cursor.Position.Y - Me.Top
    End Sub

    ''' <summary>
    ''' تحديث موقع النافذة أثناء السحب
    ''' </summary>
    Private Sub Panel3_MouseMove(sender As Object, e As MouseEventArgs) Handles Panel3.MouseMove
        If Drag Then
            Me.Top = Cursor.Position.Y - MouseY
            Me.Left = Cursor.Position.X - MouseX
        End If
    End Sub

    ''' <summary>
    ''' إنهاء عملية السحب عند رفع الزر
    ''' </summary>
    Private Sub Panel3_MouseUp(sender As Object, e As MouseEventArgs) Handles Panel3.MouseUp
        Drag = False
    End Sub

#End Region

#Region "التحقق من المدخلات"

    ''' <summary>
    ''' التحقق من صحة المدخلات
    ''' </summary>
    Private Function ValidateInput() As Boolean
        If String.IsNullOrWhiteSpace(ComboUsername.Text) Then
            MessageBox.Show("يرجى إدخال اسم المستخدم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ComboUsername.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("يرجى إدخال كلمة المرور", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPassword.Focus()
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' محاولة تسجيل الدخول
    ''' </summary>
    Private Function AttemptLogin() As LoginStatus
        Try
            ' استخدام الإجراء المخزن "LoginUser"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@UserName", ComboUsername.Text.Trim()),
            DatabaseHelper.CreateParameter("@Password", Encrypt(txtPassword.Text.Trim()))
        }

            Dim result As DataTable = DatabaseHelper.ExecuteStoredProcedure("LoginUser", parameters)

            ' التحقق من نتيجة الاستعلام
            If result.Rows.Count = 0 Then
                Return LoginStatus.InvalidCredentials
            End If

            Dim userRow As DataRow = result.Rows(0)

            ' التحقق من حالة المستخدم (نشط أم لا)
            Dim isActive As Boolean = Convert.ToBoolean(userRow("IsActive"))
            If Not isActive Then
                Return LoginStatus.UserNotActive
            End If


            ' التحقق من أن المستخدم موجود
            If Convert.ToInt32(userRow("ID")) <= 0 Then
                Return LoginStatus.UserNotFound
            End If

            ' حفظ بيانات المستخدم في المتغيرات العالمية
            GlobalData.LoggedInUserID = Convert.ToInt32(userRow("ID"))
            GlobalData.LoggedInUserName = userRow("UserName").ToString()

            ' تطبيق الصلاحيات الأولية
            ApplyInitialPermissions()

            Return LoginStatus.Success

        Catch ex As SqlException
            System.Diagnostics.Debug.WriteLine($"خطأ في قاعدة البيانات عند تسجيل الدخول: {ex.Message}")
            Return LoginStatus.DatabaseError

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ عام عند تسجيل الدخول: {ex.Message}")
            Return LoginStatus.DatabaseError
        End Try
    End Function

    '''' <summary>
    '''' محاولة تسجيل الدخول
    '''' </summary>
    'Private Function AttemptLogin() As LoginStatus
    '    Try
    '        ' استعلام للتحقق من بيانات المستخدم وحالته
    '        Dim query As String = "
    '            SELECT ID, UserName, Password, IsAdmin, IsActive 
    '            FROM TbUsers 
    '            WHERE UserName = @UserName AND Password = @Password"

    '        Dim parameters() As SqlParameter = {
    '            DatabaseHelper.CreateParameter("@UserName", ComboUsername.Text.Trim()),
    '            DatabaseHelper.CreateParameter("@Password", userclass.EncryptPassword(txtPassword.Text.Trim()))
    '        }

    '        Dim result As DataTable = DatabaseHelper.ExecuteQuery(query, parameters)

    '        ' التحقق من نتيجة الاستعلام
    '        If result.Rows.Count = 0 Then
    '            Return LoginStatus.InvalidCredentials
    '        End If

    '        Dim userRow As DataRow = result.Rows(0)

    '        ' التحقق من حالة المستخدم (نشط أم لا)
    '        Dim isActive As Boolean = Convert.ToBoolean(userRow("IsActive"))
    '        If Not isActive Then
    '            Return LoginStatus.UserNotActive
    '        End If

    '        ' التحقق من أن المستخدم موجود
    '        If Convert.ToInt32(userRow("ID")) <= 0 Then
    '            Return LoginStatus.UserNotFound
    '        End If

    '        ' حفظ بيانات المستخدم في المتغيرات العالمية
    '        GlobalData.LoggedInUserID = Convert.ToInt32(userRow("ID"))
    '        GlobalData.LoggedInUserName = userRow("UserName").ToString()

    '        ' تطبيق الصلاحيات الأولية
    '        ApplyInitialPermissions()

    '        Return LoginStatus.Success

    '    Catch ex As SqlException
    '        System.Diagnostics.Debug.WriteLine($"خطأ في قاعدة البيانات عند تسجيل الدخول: {ex.Message}")
    '        Return LoginStatus.DatabaseError

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ عام عند تسجيل الدخول: {ex.Message}")
    '        Return LoginStatus.DatabaseError
    '    End Try
    'End Function

    ''' <summary>
    ''' تحميل صورة المستخدم الحالي
    ''' </summary>
    Private Sub LoadCurrentUserImage()
        Try
            Dim usersClass As New UsersClass()
            Dim imageData As Byte() = usersClass.GetUserImage(GlobalData.LoggedInUserID)

            If imageData IsNot Nothing AndAlso imageData.Length > 0 Then
                Using ms As New MemoryStream(imageData)
                    GlobalData.CurrentUserImage = System.Drawing.Image.FromStream(ms)
                End Using
            Else
                ' استخدام صورة افتراضية
                GlobalData.CurrentUserImage = My.Resources.UserDefault ' يمكنك تحديد صورة افتراضية من موارد المشروع
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل صورة المستخدم: {ex.Message}")
            ' استخدام صورة افتراضية في حالة الخطأ
            GlobalData.CurrentUserImage = My.Resources.UserDefault
        End Try
    End Sub

    ''' <summary>
    ''' نوع حالة تسجيل الدخول
    ''' </summary>
    Private Enum LoginStatus
        Success
        InvalidCredentials
        UserNotActive
        UserNotFound
        DatabaseError
    End Enum

    Private Sub btnConn_Click(sender As Object, e As EventArgs) Handles btnConn.Click
        Dim frm As New ServerConnectionForm
        frm.ShowDialog()
    End Sub

#End Region

#Region "الصلاحيات"

    ''' <summary>
    ''' تحميل صلاحيات المستخدم
    ''' </summary>
    Private Sub LoadUserPermissions(userID As Integer)
        Try
            ' تحميل الصلاحيات في مدير الصلاحيات الموحد
            PermissionsManager.Instance.LoadUserPermissions(userID)

            ' تعيين القيم العالمية للتوافق مع الكود القديم
            GlobalData.UserCanAdd = PermissionsManager.Instance.CanAdd(Me.GetType().Name)
            GlobalData.UserCanEdit = PermissionsManager.Instance.CanEdit(Me.GetType().Name)
            GlobalData.UserCanDelete = PermissionsManager.Instance.CanDelete(Me.GetType().Name)
            GlobalData.UserCanSearch = PermissionsManager.Instance.CanSearch(Me.GetType().Name)
            GlobalData.UserCanManageUsers = PermissionsManager.Instance.CanManageUsers()
            GlobalData.UserCanManagePermissions = PermissionsManager.Instance.CanManagePermissions()

        Catch ex As Exception
            ' في حالة حدوث خطأ، قم بتعيين القيم الافتراضية
            GlobalData.UserCanAdd = False
            GlobalData.UserCanEdit = False
            GlobalData.UserCanDelete = False
            GlobalData.UserCanSearch = True
            GlobalData.UserCanManageUsers = False
            GlobalData.UserCanManagePermissions = False

            ' تحميل الصلاحيات الافتراضية
            PermissionsManager.Instance.LoadUserPermissions(userID)
        End Try
    End Sub

    ''' <summary>
    ''' تطبيق الصلاحيات الأولية بعد تسجيل الدخول
    ''' </summary>
    Private Sub ApplyInitialPermissions()
        Try
            ' تطبيق الصلاحيات العامة
            If PermissionsManager.Instance.IsUserAdmin(GlobalData.LoggedInUserID) Then
                ' يمكن إضافة إجراءات خاصة للمديرين هنا
            End If

            ' تطبيق الصلاحيات على الأدوات المشتركة
            ApplyCommonToolPermissions()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق الصلاحيات الأولية: {ex.Message}")
        End Try
    End Sub


    ''' <summary>
    ''' تطبيق الصلاحيات على الأدوات المشتركة
    ''' </summary>
    Private Sub ApplyCommonToolPermissions()
        Try
            ' التحقق من وجود النماذج الرئيسية والتحكم في أدواتها
            For Each form As Form In Application.OpenForms
                If TypeOf form Is BaseFormWithPermissions Then
                    Dim baseForm As BaseFormWithPermissions = DirectCast(form, BaseFormWithPermissions)
                    baseForm.ApplyPermissions()
                End If
            Next

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق الصلاحيات على الأدوات المشتركة: {ex.Message}")
        End Try
    End Sub
#End Region

End Class

