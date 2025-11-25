#Region "Imports"
Imports Microsoft.Win32
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports ALquhaliLibrary.DBCLS
#End Region


''' <summary>
''' فئة لإدارة اتصالات SQL Server، بما في ذلك تحميل السيرفرات المثبتة، تعبئة قواعد البيانات، اختبار الاتصال، وحفظ الإعدادات.
''' </summary>

Public Class ServerConnectionManager

#Region "Server Loading Methods"
    ''' <summary>
    ''' تحميل أسماء السيرفرات المثبتة (الطريقة القديمة).
    ''' </summary>
    ''' <param name="Comb">ComboBox لتعبئة أسماء السيرفرات المثبتة.</param>
    Public Sub LoadInstalledServers_Old(ByVal Comb As ComboBox)
        Try
            ' فتح الريجستري لنظام 64 بت للوصول إلى نسخ SQL Server المثبتة
            Dim RegKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
            Dim SubRegKey = RegKey.OpenSubKey("SOFTWARE\Microsoft\Microsoft SQL Server")
            Dim Instances = CType(SubRegKey.GetValue("InstalledInstances"), String())
            If Instances.Length > 0 Then
                For Each element As String In Instances
                    If element = "MSSQLSERVER" Then
                        Comb.Items.Add(Environment.MachineName) ' النسخة الافتراضية
                    Else
                        Comb.Items.Add(Environment.MachineName + "\" + element) ' نسخة مخصصة
                    End If
                Next element
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    ''' <summary>
    ''' تحميل نسخ SQL Server المثبتة في ComboBox (الطريقة المحدثة).
    ''' </summary>
    ''' <param name="comboBox">ComboBox لتعبئة أسماء السيرفرات المثبتة.</param>
    Public Sub LoadInstalledServers(ByVal comboBox As ComboBox)
        Try
            ' تحديد نوع النظام (32 بت أو 64 بت) لفتح الريجستري المناسب
            Dim regView As RegistryView = If(Is64BitOperatingSystem(), RegistryView.Registry64, RegistryView.Registry32)

            ' فتح الريجستري للوصول إلى نسخ SQL Server المثبتة
            Using localMachine As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, regView)
                Using rk As RegistryKey = localMachine.OpenSubKey("SOFTWARE\Microsoft\Microsoft SQL Server")
                    If rk Is Nothing Then
                        MessageBox.Show("لم يتم العثور على أي نسخ من SQL Server.", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If

                    Dim instances As String() = TryCast(rk.GetValue("InstalledInstances"), String())

                    If instances Is Nothing OrElse instances.Length = 0 Then
                        MessageBox.Show("لم يتم العثور على أي نسخ من SQL Server.", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If

                    ' مسح ComboBox وإضافة أسماء النسخ المثبتة
                    comboBox.Items.Clear()
                    For Each instance As String In instances
                        Dim serverName As String
                        If instance = "MSSQLSERVER" Then
                            serverName = Environment.MachineName ' النسخة الافتراضية
                        Else
                            serverName = String.Format("{0}\{1}", Environment.MachineName, instance) ' نسخة مخصصة
                        End If
                        comboBox.Items.Add(serverName)
                    Next
                End Using
            End Using

        Catch ex As System.Security.SecurityException
            MessageBox.Show("تم رفض الوصول إلى الريجستري. يرجى تشغيل التطبيق بصلاحيات مدير.", "رفض الوصول", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Catch ex As Exception
            MessageBox.Show(String.Format("حدث خطأ أثناء تحميل السيرفرات: {0}", ex.Message), "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    ''' <summary>
    ''' تحديد ما إذا كان النظام يعمل بنظام 64 بت.
    ''' </summary>
    ''' <returns>قيمة Boolean تشير إلى ما إذا كان النظام 64 بت.</returns>
    Private Function Is64BitOperatingSystem() As Boolean
        Return Environment.Is64BitOperatingSystem
    End Function

#End Region

#Region "Database Loading Methods"
    ''' <summary>
    ''' تعبئة ComboBox بأسماء قواعد البيانات (الطريقة القديمة).
    ''' </summary>
    ''' <param name="ServerName">اسم السيرفر.</param>
    ''' <param name="ComboDBs">ComboBox لتعبئة أسماء قواعد البيانات.</param>
    Public Sub FillCombByDataBases_Old(ByVal ServerName As String, ByVal ComboDBs As ComboBox)
        Try
            If String.IsNullOrEmpty(ServerName) = False Then
                ComboDBs.Items.Clear()

                Dim SqlConnStr As String = Nothing
                If My.Settings.LoginMothed = 0 Then
                    SqlConnStr = "Data Source=" & ServerName & ";Initial Catalog=master;Integrated Security=True"
                ElseIf My.Settings.LoginMothed = 1 Then
                    SqlConnStr = "Data Source=" & ServerName & ";Initial Catalog=master;User ID=" & My.Settings.LoginID & ";Password=" & My.Settings.LogPassword & ""
                ElseIf My.Settings.LoginMothed = 2 Then
                    SqlConnStr = "Data Source=" & My.Settings.ServerIP & "," & My.Settings.LoginPort & ";Network Library=DBMSSOCN;Initial Catalog=master;User ID=" & My.Settings.LoginID & ";Password=" & My.Settings.LogPassword & ""
                ElseIf My.Settings.LoginMothed = 3 Then
                    SqlConnStr = My.Settings.WANConString
                End If

                Dim cn As New SqlConnection(SqlConnStr)
                Dim da As New SqlDataAdapter("select name from sys.databases ", cn)
                Dim dt As New DataTable
                da.Fill(dt)

                For i As Short = 0 To dt.Rows.Count - 1
                    ComboDBs.Items.Add(dt.Rows(i).Item("name").ToString())
                Next
                cn.Dispose()
                da.Dispose()
                dt.Dispose()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    ''' <summary>
    ''' تعبئة ComboBox بأسماء قواعد البيانات (الطريقة المحدثة).
    ''' </summary>
    ''' <param name="serverName">اسم السيرفر.</param>
    ''' <param name="combo">ComboBox لتعبئة أسماء قواعد البيانات.</param>
    Public Sub FillCombByDataBases(ByVal serverName As String, ByVal combo As ComboBox)
        Try
            If Not String.IsNullOrEmpty(serverName) Then
                combo.Items.Clear()
                Dim connectionString As String = GetConnectionString(serverName, "master")
                Using connection As New SqlConnection(connectionString)
                    Dim adapter As New SqlDataAdapter("SELECT name FROM sys.databases", connection)
                    Dim dataTable As New DataTable
                    adapter.Fill(dataTable)
                    For Each row As DataRow In dataTable.Rows
                        combo.Items.Add(row("name").ToString())
                    Next
                End Using
            End If
        Catch ex As Exception
            MessageBox.Show(String.Format("حدث خطأ أثناء استرداد قواعد البيانات: {0}", ex.Message), "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' إنشاء سلسلة الاتصال بناءً على نوع المصادقة.
    ''' </summary>
    ''' <param name="ServerName">اسم السيرفر.</param>
    ''' <param name="DbName">اسم قاعدة البيانات.</param>
    ''' <returns>سلسلة الاتصال.</returns>
    Private Function GetConnectionString(ByVal ServerName As String, ByVal DbName As String) As String
        Try
            Select Case My.Settings.LoginMothed
                Case 0 ' مصادقة ويندوز
                    Return String.Format("Data Source={0};Initial Catalog={1};Integrated Security=True",
                                        ServerName, DbName)
                Case 1 ' مصادقة SQL Server
                    Return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}",
                                               ServerName, DbName, My.Settings.LoginID, My.Settings.LogPassword)
                Case 2 ' مصادقة الشبكة
                    Return String.Format("Data Source={0},{1};Network Library=DBMSSOCN;Initial Catalog={2};User ID={3};Password={4}",
                                               ServerName, My.Settings.LoginPort, DbName, My.Settings.LoginID, My.Settings.LogPassword)
                Case Else
                    Throw New InvalidOperationException("طريقة تسجيل الدخول غير صالحة.")
            End Select
        Catch ex As Exception
            Throw New Exception(String.Format("فشل في إنشاء سلسلة الاتصال: {0}", ex.Message))
        End Try
    End Function
#End Region

#Region "Settings Management"
    ''' <summary>
    ''' عرض الإعدادات المحفوظة.
    ''' </summary>
    ''' <param name="xx">نافذة FrmServerConnection لعرض الإعدادات.</param>
    Public Sub ShowSavedSettings(ByVal xx As ServerConnectionForm)
        If My.Settings.LoginMothed = 0 Then
            xx.rbLocal.Checked = True
            xx.CombServers.Text = My.Settings.ServerName
            xx.CombAuth.SelectedIndex = 0
            xx.UserId.Clear()
            xx.UserPassword.Clear()
        ElseIf My.Settings.LoginMothed = 1 Then
            xx.rbLocal.Checked = True
            xx.CombServers.Text = My.Settings.ServerName
            xx.CombAuth.SelectedIndex = 1
            xx.UserId.Text = My.Settings.LoginID
            xx.UserPassword.Text = My.Settings.LogPassword
        ElseIf My.Settings.LoginMothed = 2 Then
            xx.rbNetwork.Checked = True
            xx.txtIPAddress.Text = My.Settings.ServerIP
            xx.txtPort.Text = My.Settings.LoginPort
            xx.txtUserName.Text = My.Settings.LoginID
            xx.txtPassword.Text = My.Settings.LogPassword
        ElseIf My.Settings.LoginMothed = 3 Then
            xx.rbWAN.Checked = True
            xx.WANConString.Text = My.Settings.WANConString
        End If
        xx.ComboDBs.Text = My.Settings.DataBaseName
    End Sub

    ''' <summary>
    ''' إعادة تعيين الإعدادات المحفوظة.
    ''' </summary>
    ''' <param name="xx">نافذة FrmServerConnection لإعادة تعيين الإعدادات.</param>
    Public Sub ResetSavedSettingsData(ByVal xx As ServerConnectionForm)
        If MessageBox.Show("أنت على وشك إلغاء إعدادات الاتصال بالسيرفر. هل تريد الاستمرار؟", "رسالة تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
            My.Settings.ServerName = Nothing
            My.Settings.DataBaseName = Nothing
            My.Settings.ServerIP = Nothing
            My.Settings.LoginID = Nothing
            My.Settings.LogPassword = Nothing
            My.Settings.WANConString = Nothing
            My.Settings.LoginMothed = 0
            xx.rbLocal.Checked = True
            xx.CombAuth.SelectedIndex = 0
            My.Settings.Save()
            'ConfirmMessage(xx.lblConfirmMsg, xx.PicMsg, xx.Timer1, "... تم إلغاء إعدادات الاتصال بالسيرفر بنجاح")
        End If
    End Sub

    ''' <summary>
    ''' التحقق من الإعدادات قبل الاختبار أو الحفظ.
    ''' </summary>
    ''' <param name="xx">نافذة FrmServerConnection للتحقق من الإعدادات.</param>
    ''' <returns>قيمة Boolean تشير إلى ما إذا كانت الإعدادات صالحة.</returns>
    Private Function CheckBeforTestSaveSettings(ByVal xx As ServerConnectionForm) As Boolean
        If xx.rbLocal.Checked = True Then
            'If MyCombTextEmpty(xx.CombServers, "اسم السيرفر") Then Return True
            If xx.CombAuth.SelectedIndex = 1 Then
                If MyTextNull(xx.UserId, "اسم المستخدم") = True Then Return True
                If MyTextNull(xx.UserPassword, "كلمة المرور") = True Then Return True
            End If
        ElseIf xx.rbNetwork.Checked = True Then
            If MyTextNull(xx.txtIPAddress, "عنوان أو اسم السيرفر") = True Then Return True
            If MyTextNull(xx.txtPort, "رقم البورت") = True Then Return True
            If MyTextNull(xx.txtUserName, "اسم المستخدم") = True Then Return True
            If MyTextNull(xx.txtPassword, "كلمة المرور") = True Then Return True
        ElseIf xx.rbWAN.Checked = True Then
            If MyTextNull(xx.WANConString, "سلسلة الاتصال") = True Then Return True
        End If
        If MyCombTextEmpty(xx.ComboDBs, "اسم قاعدة البيانات") = True Then Return True
        Return False
    End Function
#End Region

#Region "Connection Testing and Saving"
    ''' <summary>
    ''' اختبار اتصال SQL Server.
    ''' </summary>
    ''' <param name="xx">نافذة FrmServerConnection لاختبار الاتصال.</param>
    Public Sub TestSQLServerConnection(ByVal xx As ServerConnectionForm)
        If CheckBeforTestSaveSettings(xx) = True Then Return
        Dim SqlConnStr As String = Nothing
        Dim SqlConn As New SqlConnection

        If xx.CombAuth.SelectedIndex = 0 And xx.rbLocal.Checked = True Then
            SqlConnStr = "Data Source=" & xx.CombServers.Text.Trim & ";Initial Catalog=master;Integrated Security=True"
        ElseIf xx.CombAuth.SelectedIndex = 1 And xx.rbLocal.Checked = True Then
            SqlConnStr = "Data Source=" & xx.CombServers.Text.Trim & ";Initial Catalog=master;User ID=" & xx.UserId.Text.Trim & ";Password=" & xx.UserPassword.Text.Trim & ""
        ElseIf xx.rbNetwork.Checked = True Then
            SqlConnStr = "Data Source=" & xx.txtIPAddress.Text.Trim & "," & xx.txtPort.Text.Trim & ";Network Library=DBMSSOCN;Initial Catalog=master;User ID=" & xx.txtUserName.Text.Trim & ";Password=" & xx.txtPassword.Text.Trim & ""
        ElseIf xx.rbWAN.Checked = True Then
            SqlConnStr = xx.WANConString.Text.Trim
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

    ''' <summary>
    ''' حفظ إعدادات الاتصال.
    ''' </summary>
    ''' <param name="xx">نافذة FrmServerConnection لحفظ الإعدادات.</param>
    Public Sub Save_Conection_Settings(ByVal xx As ServerConnectionForm)
        If MessageBox.Show("هل تريد حفظ إعدادات الاتصال بالسيرفر؟", "رسالة تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
            If CheckBeforTestSaveSettings(xx) = True Then Return
            If xx.rbLocal.Checked = True Then
                My.Settings.ServerName = xx.CombServers.Text.Trim
                If xx.CombAuth.SelectedIndex = 0 Then
                    My.Settings.LoginMothed = 0
                ElseIf xx.CombAuth.SelectedIndex = 1 Then
                    My.Settings.LoginMothed = 1
                    My.Settings.LoginID = xx.UserId.Text.Trim
                    My.Settings.LogPassword = xx.UserPassword.Text.Trim
                End If
            ElseIf xx.rbNetwork.Checked = True Then
                My.Settings.LoginMothed = 2
                My.Settings.ServerIP = xx.txtIPAddress.Text
                My.Settings.LoginPort = xx.txtPort.Text.Trim
                My.Settings.LoginID = xx.txtUserName.Text.Trim
                My.Settings.LogPassword = xx.txtPassword.Text.Trim
            ElseIf xx.rbWAN.Checked = True Then
                My.Settings.WANConString = xx.WANConString.Text.Trim
                My.Settings.LoginMothed = 3
            End If
            My.Settings.DataBaseName = xx.ComboDBs.Text.Trim
            My.Settings.Save()
            'ConfirmMessage(xx.lblConfirmMsg, xx.PicMsg, xx.Timer1, "... تم حفظ إعدادات الاتصال بالسيرفر بنجاح")
        End If
    End Sub
#End Region

#Region "Password Management"
    ''' <summary>
    ''' تغيير كلمة مرور الوصول إلى الشاشة.
    ''' </summary>
    Public Sub ChangeAccessFrmServerConnectionPassword()
        Try
            ' الحصول على كلمة المرور الحالية
            Dim currentPassword As String = OutofInputBox("لتغيير كلمة المرور، يرجى " & vbNewLine &
                                                          " إدخال كلمة المرور الحالية لمتابعة عملية التعديل",
                                                          "تعديل كلمة المرور للوصول إلى الشاشة")

            ' التحقق من إلغاء العملية أو ترك الحقل فارغًا
            If String.IsNullOrEmpty(currentPassword) Then
                Return
            End If

            ' التحقق من صحة كلمة المرور الحالية
            If Not ValidateCurrentPassword(currentPassword) Then Return

            ' الحصول على كلمة المرور الجديدة
            Dim newPassword As String = OutofInputBox("فضلاً أدخل كلمة المرور الجديدة:", "تعديل كلمة المرور")
            If String.IsNullOrEmpty(newPassword) Then
                MessageBox.Show("تم إلغاء العملية. لم يتم تعديل كلمة المرور.", "إلغاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' التحقق من صحة كلمة المرور الجديدة
            If Not ValidateNewPassword(currentPassword, newPassword) Then Return

            ' حفظ كلمة المرور الجديدة
            SaveNewPassword(newPassword)

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تعديل كلمة المرور. الرجاء المحاولة مرة أخرى." & vbNewLine & "تفاصيل الخطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' عرض نافذة إدخال كلمة المرور.
    ''' </summary>
    ''' <param name="promptMessage">الرسالة التي تظهر للمستخدم.</param>
    ''' <param name="title">عنوان النافذة.</param>
    ''' <returns>كلمة المرور التي أدخلها المستخدم.</returns>
    Private Function OutofInputBox(ByVal promptMessage As String, ByVal title As String) As String
        Return InputBox(promptMessage, title).Trim()
    End Function

    ''' <summary>
    ''' التحقق من صحة كلمة المرور الحالية.
    ''' </summary>
    ''' <param name="inputPassword">كلمة المرور التي أدخلها المستخدم.</param>
    ''' <returns>قيمة Boolean تشير إلى ما إذا كانت كلمة المرور صحيحة.</returns>
    Private Function ValidateCurrentPassword(ByVal inputPassword As String) As Boolean
        If Not String.Equals(inputPassword, My.Settings.FrmConStrPASS, StringComparison.Ordinal) Then
            MessageBox.Show("كلمة المرور التي أدخلتها غير صحيحة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' التحقق من صحة كلمة المرور الجديدة.
    ''' </summary>
    ''' <param name="currentPassword">كلمة المرور الحالية.</param>
    ''' <param name="newPassword">كلمة المرور الجديدة.</param>
    ''' <returns>قيمة Boolean تشير إلى ما إذا كانت كلمة المرور الجديدة صالحة.</returns>
    Private Function ValidateNewPassword(ByVal currentPassword As String, ByVal newPassword As String) As Boolean
        If newPassword.Length < 5 Then
            MessageBox.Show("كلمة المرور الجديدة يجب أن تحتوي على 5 أحرف أو أرقام على الأقل.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If String.Equals(currentPassword, newPassword, StringComparison.Ordinal) Then
            MessageBox.Show("كلمة المرور الجديدة مطابقة للقديمة.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' حفظ كلمة المرور الجديدة.
    ''' </summary>
    ''' <param name="newPassword">كلمة المرور الجديدة.</param>
    Private Sub SaveNewPassword(ByVal newPassword As String)
        My.Settings.FrmConStrPASS = Encrypt(newPassword)
        My.Settings.Save()
        MessageBox.Show("تم تعديل كلمة المرور بنجاح.", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

End Class