Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO

Public Class DatabaseConnectionForm
    Private _isFirstTime As Boolean = False
    Private _isTestingConnection As Boolean = False

    Public Event ConnectionConfigured()

    Public Sub New()
        InitializeComponent()
        InitializeUI()
    End Sub

    ''' <summary>
    ''' تهيئة واجهة المستخدم
    ''' </summary>
    Private Sub InitializeUI()
        ' التحقق مما إذا كان هذا التشغيل الأول
        _isFirstTime = Not My.MySettings.Default.IsConfigured

        ' تعيين النصوص حسب السياق
        Me.Text = If(_isFirstTime, "تهيئة النظام لأول مرة", "إعدادات الاتصال بقاعدة البيانات")
        lblTitle.Text = If(_isFirstTime, "مرحباً بك في نظام ESIS", "إعدادات الاتصال بقاعدة البيانات")
        lblDescription.Text = If(_isFirstTime,
                                "يرجى إدخال معلومات الاتصال بقاعدة البيانات لإكمال تهيئة النظام",
                                "يرجى إدخال معلومات الاتصال بقاعدة البيانات للنظام")

        ' تعيين الحالة الأولية
        btnTestConnection.Enabled = False
        btnConnect.Enabled = False

        ' تحميل إعدادات الاتصال الحالية
        LoadCurrentSettings()

        ' إعداد الحقول
        txtServerName.Focus()
    End Sub

    ''' <summary>
    ''' تحميل إعدادات الاتصال الحالية
    ''' </summary>
    Private Sub LoadCurrentSettings()
        Try
            ' تحميل الإعدادات الحالية
            txtServerName.Text = My.MySettings.Default.ServerName
            txtDatabaseName.Text = My.MySettings.Default.DataBaseName
            txtUsername.Text = My.MySettings.Default.DatabaseUser
            txtPassword.Text = My.MySettings.Default.DatabasePassword
            chkIntegratedSecurity.Checked = My.MySettings.Default.UseIntegratedSecurity

            ' تطبيق حالة المصادقة المتكاملة
            UpdateAuthenticationControls()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل إعدادات الاتصال: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث عناصر واجهة المستخدم بناءً على نوع المصادقة
    ''' </summary>
    Private Sub UpdateAuthenticationControls()
        ' إخفاء/إظهار حقول اسم المستخدم وكلمة المرور
        lblUsername.Visible = Not chkIntegratedSecurity.Checked
        txtUsername.Visible = Not chkIntegratedSecurity.Checked
        lblPassword.Visible = Not chkIntegratedSecurity.Checked
        txtPassword.Visible = Not chkIntegratedSecurity.Checked
        lblNote.Visible = chkIntegratedSecurity.Checked

        ' التحقق من إمكانية تمكين أزرار الاتصال
        CheckConnectionButtonsState()
    End Sub

    ''' <summary>
    ''' التحقق من حالة أزرار الاتصال
    ''' </summary>
    Private Sub CheckConnectionButtonsState()
        Dim canConnect As Boolean = Not String.IsNullOrEmpty(txtServerName.Text) AndAlso
                                   Not String.IsNullOrEmpty(txtDatabaseName.Text) AndAlso
                                   (chkIntegratedSecurity.Checked OrElse
                                    (Not String.IsNullOrEmpty(txtUsername.Text) AndAlso
                                     Not String.IsNullOrEmpty(txtPassword.Text)))

        btnTestConnection.Enabled = canConnect
        btnConnect.Enabled = canConnect
    End Sub

    ''' <summary>
    ''' معالجة تغيير مربع تحديد المصادقة المتكاملة
    ''' </summary>
    Private Sub chkIntegratedSecurity_CheckedChanged(sender As Object, e As EventArgs) Handles chkIntegratedSecurity.CheckedChanged
        UpdateAuthenticationControls()
    End Sub

    ''' <summary>
    ''' معالجة تغيير نص حقول الاتصال
    ''' </summary>
    Private Sub ConnectionFields_TextChanged(sender As Object, e As EventArgs) Handles txtServerName.TextChanged, txtDatabaseName.TextChanged, txtUsername.TextChanged, txtPassword.TextChanged
        CheckConnectionButtonsState()
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر اختبار الاتصال
    ''' </summary>
    Private Sub btnTestConnection_Click(sender As Object, e As EventArgs) Handles btnTestConnection.Click
        Try
            _isTestingConnection = True
            btnTestConnection.Enabled = False
            btnConnect.Enabled = False
            lblStatus.Text = "جاري اختبار الاتصال..."
            lblStatus.ForeColor = Color.Blue

            ' حفظ الإعدادات المؤقتة
            Dim serverName As String = txtServerName.Text
            Dim databaseName As String = txtDatabaseName.Text
            Dim useIntegratedSecurity As Boolean = chkIntegratedSecurity.Checked
            Dim userID As String = If(useIntegratedSecurity, String.Empty, txtUsername.Text)
            Dim password As String = If(useIntegratedSecurity, String.Empty, txtPassword.Text)

            ' اختبار الاتصال
            If TestConnection(serverName, databaseName, useIntegratedSecurity, userID, password) Then
                lblStatus.Text = "اتصال ناجح!"
                lblStatus.ForeColor = Color.Green
            Else
                lblStatus.Text = "فشل الاتصال. يرجى التحقق من الإعدادات."
                lblStatus.ForeColor = Color.Red
            End If
        Catch ex As Exception
            lblStatus.Text = $"فشل الاتصال: {ex.Message}"
            lblStatus.ForeColor = Color.Red
            System.Diagnostics.Debug.WriteLine($"خطأ في اختبار الاتصال: {ex.Message}")
        Finally
            _isTestingConnection = False
            btnTestConnection.Enabled = True
            CheckConnectionButtonsState()
        End Try
    End Sub

    ''' <summary>
    ''' اختبار اتصال قاعدة البيانات
    ''' </summary>
    Private Function TestConnection(serverName As String, databaseName As String, useIntegratedSecurity As Boolean, userID As String, password As String) As Boolean
        Dim connectionString As String

        ' بناء سلسلة الاتصال
        If useIntegratedSecurity Then
            connectionString = $"Server={serverName};Database={databaseName};Integrated Security=SSPI;"
        Else
            connectionString = $"Server={serverName};Database={databaseName};User ID={userID};Password={password};"
        End If

        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Return True
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الاتصال: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' معالجة الضغط على زر الاتصال
    ''' </summary>
    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        Try
            ' حفظ الإعدادات
            'SaveSettings()

            ' اختبار الاتصال
            If TestConnection(My.MySettings.Default.ServerName, My.MySettings.Default.DataBaseName,
                              My.MySettings.Default.UseIntegratedSecurity, My.MySettings.Default.DatabaseUser, My.MySettings.Default.DatabasePassword) Then

                ' حفظ حالة التهيئة
                'My.MySettings.Default.IsConfigured = True
                My.MySettings.Default.Save()

                ' إظهار رسالة نجاح
                If _isFirstTime Then
                    MessageBox.Show("تم تهيئة النظام بنجاح! يمكنك الآن استخدام النظام.", "تهيئة النظام",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("تم تحديث إعدادات الاتصال بنجاح!", "إعدادات الاتصال",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                ' إطلاق حدث الاتصال
                RaiseEvent ConnectionConfigured()

                ' إغلاق النموذج
                Me.Close()
            Else
                lblStatus.Text = "فشل الاتصال. يرجى التحقق من الإعدادات."
                lblStatus.ForeColor = Color.Red
                MessageBox.Show("فشل الاتصال بقاعدة البيانات. يرجى التحقق من صحة الإعدادات المدخلة.",
                                "فشل الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حفظ إعدادات الاتصال: {ex.Message}")
            MessageBox.Show($"حدث خطأ أثناء حفظ إعدادات الاتصال: {ex.Message}",
                            "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '''' <summary>
    '''' حفظ إعدادات الاتصال
    '''' </summary>
    'Private Sub SaveSettings()
    '    Try
    '        ' حفظ الإعدادات - استخدام My.Settings بدلاً من My.MySettings.Default
    '        My.Settings.ServerName = txtServerName.Text.Trim
    '        My.Settings.DataBaseName = txtDatabaseName.Text
    '        My.Settings.UseIntegratedSecurity = chkIntegratedSecurity.Checked
    '        My.Settings.DatabaseUser = If(chkIntegratedSecurity.Checked, String.Empty, txtUsername.Text)
    '        My.Settings.DatabasePassword = If(chkIntegratedSecurity.Checked, String.Empty, txtPassword.Text)

    '        ' حفظ حالة التهيئة
    '        My.Settings.IsConfigured = True

    '        ' بناء سلسلة الاتصال
    '        Dim connectionString As String
    '        If chkIntegratedSecurity.Checked Then
    '            connectionString = $"Server={txtServerName.Text};Database={txtDatabaseName.Text};Integrated Security=SSPI;"
    '        Else
    '            connectionString = $"Server={txtServerName.Text};Database={txtDatabaseName.Text};User ID={txtUsername.Text};Password={txtPassword.Text};"
    '        End If

    '        ' تحديث سلسلة الاتصال في ملف الإعدادات
    '        UpdateConnectionString(connectionString)

    '        ' حفظ الإعدادات
    '        My.Settings.Save()
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في حفظ إعدادات الاتصال: {ex.Message}")
    '        Throw
    '    End Try
    'End Sub

    ''' <summary>
    ''' تحديث سلسلة الاتصال في ملف الإعدادات
    ''' </summary>
    Private Sub UpdateConnectionString(newConnectionString As String)
        Try
            ' فتح ملف الإعدادات
            Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            Dim connectionStringsSection As ConnectionStringsSection = config.ConnectionStrings

            ' التحقق من وجود سلسلة الاتصال
            If connectionStringsSection.ConnectionStrings("ESIS") IsNot Nothing Then
                connectionStringsSection.ConnectionStrings("ESIS").ConnectionString = newConnectionString
            Else
                ' إضافة سلسلة اتصال جديدة
                connectionStringsSection.ConnectionStrings.Add(
                    New ConnectionStringSettings("ESIS", newConnectionString, "System.Data.SqlClient"))
            End If

            ' حفظ التغييرات
            config.Save(ConfigurationSaveMode.Modified)
            ConfigurationManager.RefreshSection("connectionStrings")
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث سلسلة الاتصال: {ex.Message}")
            ' يمكن محاولة الحل البديل
            Try
                ' حفظ سلسلة الاتصال مباشرة في إعدادات التطبيق
                My.MySettings.Default.Item("ConnectionString") = newConnectionString
                My.MySettings.Default.Save()
            Catch ex2 As Exception
                System.Diagnostics.Debug.WriteLine($"فشل الحل البديل: {ex2.Message}")
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر الإلغاء
    ''' </summary>
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ' إذا كان هذا التشغيل الأول، لا يمكن الإلغاء
        If _isFirstTime Then
            MessageBox.Show("يجب إكمال تهيئة النظام للبدء في استخدامه.", "تنبيه",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' التحقق من ما إذا تم تغيير الإعدادات
        If SettingsHaveChanged() Then
            Dim result As DialogResult = MessageBox.Show(
                "هل أنت متأكد من رغبتك في إلغاء التغييرات؟",
                "إلغاء التغييرات",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

            If result = DialogResult.No Then
                Return
            End If
        End If

        ' إغلاق النموذج
        Me.Close()
    End Sub

    ''' <summary>
    ''' التحقق مما إذا كانت الإعدادات قد تغيرت
    ''' </summary>
    Private Function SettingsHaveChanged() As Boolean
        Return txtServerName.Text <> My.MySettings.Default.ServerName OrElse
               txtDatabaseName.Text <> My.MySettings.Default.DataBaseName OrElse
               chkIntegratedSecurity.Checked <> My.MySettings.Default.UseIntegratedSecurity OrElse
               txtUsername.Text <> My.MySettings.Default.DatabaseUser OrElse
               txtPassword.Text <> My.MySettings.Default.DatabasePassword
    End Function

    ''' <summary>
    ''' معالجة إغلاق النموذج
    ''' </summary>
    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        ' إذا كان هذا التشغيل الأول، لا يمكن إغلاق النموذج دون إكمال التهيئة
        If _isFirstTime AndAlso Not My.MySettings.Default.IsConfigured Then
            e.Cancel = True
            MessageBox.Show("يجب إكمال تهيئة النظام للبدء في استخدامه.", "تنبيه",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' إذا تم تغيير الإعدادات دون حفظ
        If SettingsHaveChanged() AndAlso Not _isTestingConnection Then
            Dim result As DialogResult = MessageBox.Show(
                "هل أنت متأكد من رغبتك في إلغاء التغييرات؟",
                "إلغاء التغييرات",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

            If result = DialogResult.No Then
                e.Cancel = True
            End If
        End If

        MyBase.OnFormClosing(e)
    End Sub


End Class