Imports System.Data.SqlClient
Imports ALquhaliLibrary
Imports System.IO
Public Class InstallWizardForm

    Dim Clss As ServerConnectionManager
    Dim MyScriptFilePath As String = Application.StartupPath & "\SysAttach\ESIS.sql"

    Private Function CheckBeforeTest() As Boolean
        If MyCombTextEmpty(Me.CombServers, " اسم السيرفر") = True Then Return True
        If chkWindowsAuthentication.Checked = False Then
            If MyTextNull(Me.txtUserName, "اسم المستخدم") = True Then Return True
            If MyTextNull(Me.txtPassword, "الباسورد") = True Then Return True
        End If
        Return False
    End Function

    Private Sub TestSQLServerConnection(ByVal xx As InstallWizardForm)
        If CheckBeforeTest() = True Then Return
        Dim SqlConnStr As String = Nothing
        Dim SqlConn As New SqlConnection
        If chkWindowsAuthentication.Checked Then
            SqlConnStr = "Data Source=" & xx.CombServers.Text.Trim & ";Initial Catalog=master;Integrated Security=True"
        Else
            SqlConnStr = "Data Source=" & xx.CombServers.Text.Trim & ";Initial Catalog=master;User ID=" & xx.txtUserName.Text.Trim & ";Password=" & xx.txtPassword.Text.Trim & ""
        End If
        SqlConn.ConnectionString = SqlConnStr
        If SqlConn.State = ConnectionState.Closed Then
            Try
                SqlConn.Open()
                MessageBox.Show("تمت عملية الاتصال بنجاح", "فحص الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("فشل الاتصال بالسيرفر" + vbCrLf + Err.Description, "فحص الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub Save_Conection_Settings()
        My.Settings.ServerName = Me.CombServers.Text
        My.Settings.LoginMothed = 0
        My.Settings.DataBaseName = Me.txtDataBaseName.Text.Trim
        If chkWindowsAuthentication.Checked = False Then
            My.Settings.LoginID = Me.txtUserName.Text.Trim
            My.Settings.LogPassword = Me.txtPassword.Text.Trim
            My.Settings.LoginMothed = 1
        End If
        My.Settings.Save()
    End Sub

    Public Function GetConectionString() As String
        Dim SqlConnStr As String = Nothing
        If chkWindowsAuthentication.Checked = True Then
            SqlConnStr = "Data Source=" & Me.CombServers.Text & ";Initial Catalog=master;Integrated Security=True"
        Else
            SqlConnStr = "Data Source=" & Me.CombServers.Text & ";Initial Catalog=master;User ID=" & Me.txtUserName.Text.Trim & ";Password=" & Me.txtPassword.Text.Trim & ""
        End If
        Return SqlConnStr
    End Function

    Private Function IsFirstRun() As Boolean
        Return String.IsNullOrEmpty(My.Settings.ServerName) AndAlso
           String.IsNullOrEmpty(My.Settings.DataBaseName)
    End Function

    Private Sub LoadExistingConfiguration()

        ' عرض الإعدادات الحالية
        CombServers.Text = My.Settings.ServerName
        'txtServerName.Text = My.Settings.ServerName
        txtDataBaseName.Text = My.Settings.DataBaseName
        chkWindowsAuthentication.Checked = My.Settings.UseIntegratedSecurity
        ' إظهار باقي الحقول...
    End Sub




    Private Sub GoToLastStep()
        Me.Panel5.BringToFront()
        Me.btnFinish.Visible = True
        Me.btn_Next.Enabled = False
        Me.btnFinish.Enabled = True
    End Sub


    Private Sub FrmInstallWizard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' إذا كانت الإعدادات مُعدة مسبقًا، اعرض ملخص الإعدادات
        If Not IsFirstRun() Then
            LoadExistingConfiguration()
            'btnNext.Text = "إعادة التكوين"
            Me.Panel3.BringToFront()
            Me.btn_Next.Tag = 0
            Me.chkWindowsAuthentication.Checked = True
            Clss.LoadInstalledServers(Me.CombServers)
        Else
            ' هذا تثبيت جديد
            'lblTitle.Text = "مرحباً بك في نظام ESIS"
            'lblDescription.Text = "سيساعدك هذا النموذج في تكوين النظام للاستخدام الأولي"
        End If

    End Sub

    Private Sub btnTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTest.Click
        TestSQLServerConnection(Me)
    End Sub

    Private Sub chkWindowsAuthentication_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWindowsAuthentication.CheckedChanged
        If chkWindowsAuthentication.Checked Then
            Me.txtPassword.Enabled = False
            Me.txtUserName.Enabled = False
        Else
            Me.txtPassword.Enabled = True
            Me.txtUserName.Enabled = True
            Me.txtUserName.Focus()
        End If
    End Sub

    Private Sub CKShow1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CKShow1.CheckedChanged
        If CKShow1.Checked = True Then
            Me.txtPassword.UseSystemPasswordChar = False
            Me.CKShow1.Text = "Hide"
        Else
            Me.txtPassword.UseSystemPasswordChar = True
            Me.CKShow1.Text = "Show"
        End If
    End Sub

    Private Sub btnSkip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSkip.Click
        GoToLastStep()
    End Sub

    Private Sub btn_Next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Next.Click

        If Me.rbServer.Checked = False And Me.rbEndDevice.Checked = False And Me.btn_Next.Tag = 0 Then
            MessageBox.Show("فضلاً اختر نوع الإعداد للنسخه؟" & vbNewLine &
                            "إعداد النسخه للعمل كـ سيرفر" & vbNewLine &
                            "او إعداد النسخه للعمل كـ نقطه طرفيه", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf Me.rbServer.Checked = True And Me.btn_Next.Tag = 0 Then
            Me.Panel4.BringToFront()
            Me.btn_Next.Tag = 1
            Me.btn_Prev.Enabled = True
        ElseIf Me.rbEndDevice.Checked = True And Me.btn_Next.Tag = 0 Then
            ServerConnectionForm.Show()
            Me.Close()
        ElseIf Me.btn_Next.Tag = 1 Then
            ''create DataBase
            If CheckBeforeTest() = True Then Return
            If MyTextNull(Me.txtDataBaseName, "اسم قاعده البيانات") = True Then Return

            If DBCLS.CheckIFDataBaseExistOnSqlServe(Me.txtDataBaseName.Text.Trim, GetConectionString()) = True Then
                MessageBox.Show("ان قاعده البيانات" & vbNewLine & Me.txtDataBaseName.Text & vbNewLine &
                           "موجوده من قبل على السيرفر" & vbNewLine &
                           "فضلا قم بتعديل اسم قاعده البيانات او تخطى عمليه إنشاء قاعده البيانات", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            'If DBCLS.CheckIFDataBaseExistOnSqlServe("EArchivedFiles", GetConectionString()) = True Then
            '    If DBCLS.DropDatabaseOnSqlServe("EArchivedFiles", GetConectionString()) = True Then
            '        If DBCLS.CreateEmptyDatabaseOnSqlServe("EArchivedFiles", GetConectionString(), "Arabic_CI_AI") = True Then
            '            If DBCLS.EXCUTESQLSCRIPT("EArchivedFiles", Application.StartupPath & "\SysAttach\TS2.sql", GetConectionString()) = False Then Return
            '        End If
            '    End If
            'Else
            '    If DBCLS.CreateEmptyDatabaseOnSqlServe("EArchivedFiles", GetConectionString(), "Arabic_CI_AI ") = True Then
            '        If DBCLS.EXCUTESQLSCRIPT("EArchivedFiles", Application.StartupPath & "\SysAttach\TS2.sql", GetConectionString()) = False Then Return
            '    End If
            'End If


            If DBCLS.CreateEmptyDatabaseOnSqlServe(Me.txtDataBaseName.Text.Trim, GetConectionString(), "Arabic_CI_AI") = True Then
                If DBCLS.EXCUTESQLSCRIPT(Me.txtDataBaseName.Text.Trim, MyScriptFilePath, GetConectionString()) = True Then
                    Save_Conection_Settings()
                    MessageBox.Show("تم إنشاء قاعده البيانات بنجاح", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    GoToLastStep()
                End If
            End If
        End If
    End Sub

    Private Sub btn_Prev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Prev.Click
        If Me.btn_Next.Enabled = True Then
            Me.Panel3.BringToFront()
            Me.btn_Next.Tag = 0
            Me.btn_Prev.Enabled = False
        ElseIf Me.btn_Next.Enabled = False Then
            Me.Panel4.BringToFront()
            Me.btn_Next.Tag = 1
            Me.btn_Next.Enabled = True
            Me.btnFinish.Enabled = False
        End If
    End Sub
    ''' <summary>
    ''' حفظ إعدادات الاتصال إلى إعدادات المشروع
    ''' </summary>
    Private Sub SaveConfiguration()
        Try
            ' حفظ إعدادات الاتصال
            My.Settings.ServerName = CombServers.Text.Trim()
            My.Settings.DataBaseName = txtDataBaseName.Text.Trim()
            My.Settings.UserName = txtUserName.Text.Trim()
            My.Settings.Password = txtPassword.Text.Trim()
            My.Settings.UseIntegratedSecurity = chkWindowsAuthentication.Checked

            ' حفظ حالة التكوين
            My.Settings.IsConfigured = True
            My.Settings.Save()

            ' تأكد من حفظ الإعدادات
            If Not My.Settings.IsConfigured Then
                Throw New Exception("فشل في حفظ حالة التكوين")
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حفظ الإعدادات: {ex.Message}")
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' التحقق من صحة إعدادات الاتصال
    ''' </summary>
    Private Function ValidateConfiguration() As Boolean
        Try
            ' التحقق من إدخال اسم الخادم
            If String.IsNullOrWhiteSpace(CombServers.Text) Then
                MessageBox.Show("يرجى إدخال اسم الخادم", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                CombServers.Focus()
                Return False
            End If

            ' التحقق من إدخال اسم قاعدة البيانات
            If String.IsNullOrWhiteSpace(txtDataBaseName.Text) Then
                MessageBox.Show("يرجى إدخال اسم قاعدة البيانات", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtDataBaseName.Focus()
                Return False
            End If

            ' التحقق من طريقة الاتصال
            If Not chkWindowsAuthentication.Checked AndAlso String.IsNullOrWhiteSpace(txtUserName.Text) Then
                MessageBox.Show("يرجى إدخال اسم المستخدم", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtUserName.Focus()
                Return False
            End If

            ' إعدادات صالحة
            Return True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من الإعدادات: {ex.Message}")
            Return False
        End Try
    End Function
    Private Sub btnFinish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFinish.Click

        Try
            ' التحقق من صحة الإعدادات قبل الحفظ
            If ValidateConfiguration() Then


                ' حفظ إعدادات الاتصال
                SaveConfiguration()

                ' تحديد حالة النظام كـ "مُعد"
                My.Settings.IsConfigured = True
                My.Settings.Save()

                ' إغلاق النموذج
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show($"خطأ في حفظ الإعدادات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


        Dim frm As New LoginForm
        frm.Show()
        Me.Close()
    End Sub



End Class