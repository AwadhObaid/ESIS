Imports System.ComponentModel
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Threading
Imports MicroVisionSerial.Encryption.Activation
Imports Security.FingerPrint
''' <summary>
''' شاشة البداية مع التحقق من الاتصال بقاعدة البيانات
''' </summary>
Public Class SplashScreen

    ' متغيرات للتحكم في العملية
    Private _connectionTester As DatabaseConnectionTester
    Private _backgroundWorker As BackgroundWorker
    Private _startTime As DateTime
    Private _minimumDisplayTime As Integer = 3000 ' 3 ثوانٍ كحد أدنى لعرض الشاشة
    Private _isFormClosing As Boolean = False

    ' متغيرات السحب والنقل للنافذة
    Private Drag As Boolean = False
    Private MouseX As Integer
    Private MouseY As Integer

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        ' هذا السطر مطلوب لتهيئة النموذج
        InitializeComponent()

        ' إعداد النموذج
        SetupForm()

        ' إعداد BackgroundWorker
        SetupBackgroundWorker()

        ' إعداد كلاس اختبار الاتصال
        _connectionTester = New DatabaseConnectionTester()
    End Sub

    ''' <summary>
    ''' إعداد خصائص النموذج
    ''' </summary>
    Private Sub SetupForm()
        Try
            ' إعداد النموذج كشاشة بداية
            Me.FormBorderStyle = FormBorderStyle.None
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Size = New Size(600, 400)
            Me.BackColor = Color.FromArgb(225, 245, 254) ' لون خلفية داكن
            Me.ShowInTaskbar = False
            Me.TopMost = True

            ' إعداد ProgressBar
            progressBar.Style = ProgressBarStyle.Continuous
            progressBar.Minimum = 0
            progressBar.Maximum = 100
            progressBar.Value = 0

            ' تسجيل وقت البداية
            _startTime = DateTime.Now

        Catch ex As Exception
            HandleError("خطأ في إعداد شاشة البداية", ex)
        End Try
    End Sub

    ''' <summary>
    ''' إعداد BackgroundWorker للعمليات في الخلفية
    ''' </summary>
    Private Sub SetupBackgroundWorker()
        _backgroundWorker = New BackgroundWorker()
        _backgroundWorker.WorkerReportsProgress = True
        _backgroundWorker.WorkerSupportsCancellation = False

        AddHandler _backgroundWorker.DoWork, AddressOf BackgroundWorker_DoWork
        AddHandler _backgroundWorker.ProgressChanged, AddressOf BackgroundWorker_ProgressChanged
        AddHandler _backgroundWorker.RunWorkerCompleted, AddressOf BackgroundWorker_RunWorkerCompleted
    End Sub

    '''' <summary>
    '''' بدء عملية التحقق من الاتصال
    '''' </summary>
    'Private Async Sub SplashScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '    Try
    '        ' عرض معلومات التطبيق
    '        lblAppName.Text = "نظام ESIS"
    '        Dim x As String = GetRegValue()

    '        If x = String.Empty Then
    '            MsgBox("البرنامج غير مفعل")
    '            ActivationForm.ShowDialog()
    '        Else
    '            If CheckActivationKey(Value, x) Then

    '                ' جلب رقم الإصدار من AssemblyInfo
    '                Dim version As Version = My.Application.Info.Version
    '                lblVersion.Text = "الإصدار " & version.ToString()

    '                ' جلب حقوق الملكية من إعدادات المشروع
    '                lblCopyright.Text = My.Settings.CopyrightText

    '                lblStatus.Text = "جاري بدء التطبيق..."

    '                ' بدء عملية التحقق
    '                Await Task.Delay(100) ' تأخير بسيط لضمان تحميل الواجهة
    '                _backgroundWorker.RunWorkerAsync()
    '            Else
    '                MsgBox("توجد مشكلة في الترخيص")
    '            End If
    '        End If

    '    Catch ex As Exception
    '        HandleError("خطأ في تحميل شاشة البداية", ex)
    '    End Try
    'End Sub

    ''' <summary>
    ''' التحقق مما إذا كان هذا هو التشغيل الأولي للبرنامج
    ''' </summary>
    Private Function IsFirstRun() As Boolean
        Try
            ' التحقق من وجود إعدادات الاتصال
            Return String.IsNullOrEmpty(My.Settings.ServerName) OrElse
               String.IsNullOrEmpty(My.Settings.DataBaseName)
            ' التحقق من حالة التكوين
            Return Not My.Settings.IsConfigured
        Catch
            Return True
        End Try
    End Function


    ''' <summary>
    ''' بدء عملية التحقق من الاتصال وتحميل التطبيق
    ''' </summary>
    Private Async Sub SplashScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' عرض معلومات التطبيق
            lblAppName.Text = "نظام ESIS"
            Dim activationKey As String = GetRegValue()

            If String.IsNullOrEmpty(activationKey) Then
                ' التحقق مما إذا كان هذا هو التشغيل الأولي
                If IsFirstRun() Then
                    ' هذا التشغيل الأولي - فتح نموذج التثبيت
                    ShowInstallWizard()
                Else
                    ' البرنامج غير مفعل - فتح نموذج التفعيل
                    ActivationForm.ShowDialog()
                    ' إعادة تحميل مفتاح التفعيل
                    activationKey = GetRegValue()
                    If String.IsNullOrEmpty(activationKey) Then
                        Application.Exit()
                        Return
                    End If
                End If
            Else
                If CheckActivationKey(Value, activationKey) Then
                    ' جلب رقم الإصدار من AssemblyInfo
                    Dim version As Version = My.Application.Info.Version
                    lblVersion.Text = "الإصدار " & version.ToString()

                    ' جلب حقوق الملكية من إعدادات المشروع
                    lblCopyright.Text = My.Settings.CopyrightText

                    lblStatus.Text = "جاري بدء التطبيق..."

                    ' بدء عملية التحقق
                    Await Task.Delay(100) ' تأخير بسيط لضمان تحميل الواجهة
                    _backgroundWorker.RunWorkerAsync()
                Else
                    MsgBox("توجد مشكلة في الترخيص")
                End If
            End If

        Catch ex As Exception
            HandleError("خطأ في تحميل شاشة البداية", ex)
        End Try
        'Try
        '    ' عرض معلومات التطبيق
        '    lblAppName.Text = "نظام ESIS"
        '    lblStatus.Text = "جاري التحقق من حالة التفعيل..."

        '    ' التحقق من حالة التفعيل
        '    Dim activationKey As String = GetRegValue()
        '    Dim isActivated As Boolean = False

        '    ' التحقق مما إذا كان هناك مفتاح تفعيل مخزن
        '    If String.IsNullOrEmpty(activationKey) Then
        '        lblStatus.Text = "البرنامج غير مفعل، جاري فتح نموذج التفعيل..."

        '        ' عرض نموذج التفعيل
        '        Using activationForm As New ActivationForm()
        '            If activationForm.ShowDialog() = DialogResult.OK Then
        '                ' إعادة تحميل مفتاح التفعيل بعد التفعيل الناجح
        '                activationKey = GetRegValue()
        '                isActivated = CheckActivationKey(Value, activationKey)
        '            End If
        '        End Using
        '    Else
        '        ' التحقق من صحة مفتاح التفعيل
        '        isActivated = CheckActivationKey(Value, activationKey)
        '    End If

        '    ' معالجة حالة التفعيل
        '    If Not isActivated Then
        '        lblStatus.Text = "فشل التحقق من الترخيص، سيتم إغلاق البرنامج..."
        '        MessageBox.Show("البرنامج غير مفعل أو توجد مشكلة في الترخيص.", "خطأ في التفعيل",
        '                   MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        Application.Exit()
        '        Return
        '    End If

        '    ' عرض معلومات الإصدار
        '    Dim version As Version = My.Application.Info.Version
        '    lblVersion.Text = $"الإصدار {version.Major}.{version.Minor}.{version.Build}"
        '    lblCopyright.Text = My.Settings.CopyrightText
        '    lblStatus.Text = "جاري إعداد الاتصال بقاعدة البيانات..."

        '    ' تأخير بسيط لضمان تحميل الواجهة
        '    Await Task.Delay(100)

        '    ' بدء عملية التحقق من الاتصال
        '    lblStatus.Text = "جاري التحقق من اتصال قاعدة البيانات..."
        '    _backgroundWorker.RunWorkerAsync()

        'Catch ex As Exception
        '    HandleError("خطأ في تحميل شاشة البداية", ex)

        '    ' إظهار رسالة خطأ للمستخدم
        '    MessageBox.Show("حدث خطأ غير متوقع أثناء بدء تشغيل التطبيق. يرجى المحاولة مرة أخرى لاحقًا.",
        '               "خطأ في التطبيق", MessageBoxButtons.OK, MessageBoxIcon.Error)

        '    ' إغلاق التطبيق
        '    Application.Exit()
        'End Try
    End Sub

    ''' <summary>
    ''' العمليات التي تتم في الخلفية
    ''' </summary>
    Private Sub BackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs)
        Try
            Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

            ' المرحلة 1: التحقق من ملفات النظام
            worker.ReportProgress(10, "فحص ملفات النظام...")
            Thread.Sleep(200)

            ' المرحلة 2: تحميل الإعدادات
            worker.ReportProgress(25, "تحميل إعدادات التطبيق...")
            Thread.Sleep(200)

            ' المرحلة 3: التحقق من الاتصال بقاعدة البيانات
            worker.ReportProgress(40, "التحقق من الاتصال بقاعدة البيانات...")

            ' التحقق من صحة إعدادات الاتصال قبل المحاولة
            If Not ValidateConnectionSettings() Then
                e.Result = False
                e.Result = "إعدادات الاتصال غير مكتملة أو غير صحيحة"
                Return
            End If

            ' استخدام DatabaseHelper للتحقق من الاتصال
            Try
                If DatabaseHelper.TestConnection() Then
                    ' إذا نجح الاتصال، جرب استرجاع بيانات بسيطة للتأكد من أن الجدول موجود
                    Dim testQuery As String = "SELECT COUNT(*) FROM TbUsers"
                    Using connection As SqlConnection = DatabaseHelper.Con()
                        connection.Open()
                        Using command As New SqlCommand(testQuery, connection)
                            Dim userCount As Integer = Convert.ToInt32(command.ExecuteScalar())
                            System.Diagnostics.Debug.WriteLine($"عدد المستخدمين: {userCount}")
                        End Using
                    End Using

                    worker.ReportProgress(70, "تم الاتصال بقاعدة البيانات بنجاح")
                    Thread.Sleep(200)

                    ' المرحلة 4: تحميل البيانات الأساسية
                    worker.ReportProgress(85, "تحميل البيانات الأساسية...")
                    Thread.Sleep(200)

                    worker.ReportProgress(100, "تم بدء التطبيق بنجاح")
                    e.Result = True
                Else
                    Throw New Exception("فشل في الاتصال بقاعدة البيانات")
                End If

            Catch ex As SqlException
                System.Diagnostics.Debug.WriteLine($"SqlException: {ex.Message}")
                e.Result = $"خطأ في قاعدة البيانات: {ex.Message}"
            Catch ex As InvalidOperationException
                System.Diagnostics.Debug.WriteLine($"InvalidOperationException: {ex.Message}")
                e.Result = $"مشكلة في الإعدادات: {ex.Message}"
            Catch ex As ConfigurationErrorsException
                System.Diagnostics.Debug.WriteLine($"ConfigurationErrorsException: {ex.Message}")
                e.Result = $"خطأ في التهيئة: {ex.Message}"
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}")
                e.Result = $"خطأ عام: {ex.Message}"
            End Try

        Catch ex As OperationCanceledException
            e.Result = "تم إلغاء عملية الاتصال"
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في العملية: {ex.Message}")
            e.Result = ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' التحقق من صحة إعدادات الاتصال
    ''' </summary>
    Private Function ValidateConnectionSettings() As Boolean
        Try
            ' التحقق من وجود إعدادات أساسية
            If String.IsNullOrEmpty(My.Settings.ServerName) OrElse
           String.IsNullOrEmpty(My.Settings.DataBaseName) Then
                Return False
            End If

            ' التحقق من طريقة الدخول
            Select Case My.Settings.LoginMothed
                Case 0, 1 ' Windows Authentication أو SQL Server Authentication
                    Return Not String.IsNullOrEmpty(My.Settings.ServerName) AndAlso
                       Not String.IsNullOrEmpty(My.Settings.DataBaseName)

                Case 2 ' اتصال عبر الشبكة
                    Return Not String.IsNullOrEmpty(My.Settings.ServerIP) AndAlso
                       Not String.IsNullOrEmpty(My.Settings.LoginPort) AndAlso
                       Not String.IsNullOrEmpty(My.Settings.DataBaseName)

                Case 3 ' اتصال عبر WAN
                    Return Not String.IsNullOrEmpty(My.Settings.WANConString)

                Case Else
                    Return False
            End Select

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من إعدادات الاتصال: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' تحديث التقدم والحالة
    ''' </summary>
    Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs)
        Try
            ' تحديث ProgressBar
            If progressBar.InvokeRequired Then
                progressBar.Invoke(Sub()
                                       progressBar.Value = e.ProgressPercentage
                                   End Sub)
            Else
                progressBar.Value = e.ProgressPercentage
            End If

            ' تحديث نص الحالة
            If e.UserState IsNot Nothing Then
                If lblStatus.InvokeRequired Then
                    lblStatus.Invoke(Sub()
                                         lblStatus.Text = e.UserState.ToString()
                                     End Sub)
                Else
                    lblStatus.Text = e.UserState.ToString()
                End If
            End If

            ' تحديث النموذج
            Me.Refresh()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث التقدم: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' اكتمال العملية في الخلفية
    ''' </summary>
    Private Sub BackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        Try
            If _isFormClosing Then Return

            If e.Error IsNot Nothing Then
                ' حدث خطأ أثناء العملية
                HandleError("خطأ أثناء بدء التطبيق", e.Error)
                Return
            End If

            ' التحقق من نتيجة العملية
            If TypeOf e.Result Is Boolean Then
                If DirectCast(e.Result, Boolean) Then
                    ' نجح الاتصال - انتظار الحد الأدنى للوقت ثم إظهار شاشة تسجيل الدخول
                    EnsureMinimumDisplayTime(AddressOf ShowLoginForm)
                Else
                    ' فشل الاتصال - عرض رسالة خطأ
                    ShowConnectionError("فشل في الاتصال بقاعدة البيانات")
                End If
            ElseIf TypeOf e.Result Is String Then
                ' هناك خطأ مع رسالة محددة
                ShowConnectionError(DirectCast(e.Result, String))
            Else
                ' حالة غير متوقعة
                ShowConnectionError("حدث خطأ غير معروف أثناء الاتصال")
            End If

        Catch ex As Exception
            HandleError("خطأ في اكتمال العملية", ex)
        End Try
    End Sub

    ''' <summary>
    ''' عرض رسالة خطأ للاتصال
    ''' </summary>
    Private Sub ShowConnectionError(errorMessage As String)
        Try
            lblStatus.Text = "فشل الاتصال!"

            Dim message As String = $"تعذر الاتصال بقاعدة البيانات:{vbNewLine}{errorMessage}{vbNewLine}{vbNewLine}" &
                               "يرجى التحقق من:" & vbNewLine &
                               "- تشغيل خادم SQL Server" & vbNewLine &
                               "- صحة إعدادات الاتصال" & vbNewLine &
                               "- توفر الشبكة (إذا كان الاتصال عن بعد)"

            MessageBox.Show(message, "خطأ في الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ' تقديم خيار لتكوين قاعدة البيانات
            Dim result As DialogResult = MessageBox.Show("هل تريد تكوين إعدادات قاعدة البيانات الآن؟",
                                                   "تكوين قاعدة البيانات", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                Dim dbConfigForm As New ServerConnectionForm()
                If dbConfigForm.ShowDialog() = DialogResult.OK Then
                    Me.Hide()
                    ' أعد المحاولة بعد التكوين
                    _backgroundWorker.RunWorkerAsync()
                    Return
                End If
            End If

            Application.Exit()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في عرض رسالة الخطأ: {ex.Message}")
            Application.Exit()
        End Try
    End Sub


    ''' <summary>
    ''' معالجة إغلاق نموذج تسجيل الدخول
    ''' </summary>
    Private Sub Login_FormClosed(sender As Object, e As FormClosedEventArgs)
        Try
            ' إذا تم إغلاق نموذج تسجيل الدخول دون تسجيل دخول ناجح، أغلق التطبيق
            If GlobalData.LoggedInUserID <= 0 Then
                Application.Exit()
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالجة إغلاق نموذج تسجيل الدخول: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' ضمان الحد الأدنى لوقت عرض شاشة البداية
    ''' </summary>
    Private Sub EnsureMinimumDisplayTime(callback As Action)
        Try
            ' تحديد الوقت الأدنى لعرض شاشة البداية (مثلاً 2 ثانية)
            Dim minDisplayTime As Integer = 2000 ' 2000 ملي ثانية
            Dim elapsed As Long = DateTime.Now.Subtract(_startTime).TotalMilliseconds

            If elapsed < minDisplayTime Then
                Dim remainingTime As Integer = CInt(minDisplayTime - elapsed)
                Threading.Thread.Sleep(remainingTime)
            End If

            ' تنفيذ الوظيفة بعد التأخير
            If callback IsNot Nothing Then
                callback.Invoke()
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في ضمان وقت العرض: {ex.Message}")
            callback?.Invoke()
        End Try
    End Sub



    '''' <summary>
    '''' العمليات التي تتم في الخلفية
    '''' </summary>
    'Private Sub BackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs)
    '    Try
    '        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

    '        ' المرحلة 1: التحقق من ملفات النظام
    '        worker.ReportProgress(10, "فحص ملفات النظام...")
    '        Thread.Sleep(200)

    '        ' المرحلة 2: تحميل الإعدادات
    '        worker.ReportProgress(25, "تحميل إعدادات التطبيق...")
    '        Thread.Sleep(200)

    '        ' المرحلة 3: التحقق من الاتصال بقاعدة البيانات
    '        worker.ReportProgress(40, "التحقق من الاتصال بقاعدة البيانات...")

    '        Try
    '            ' استخدام DatabaseHelper للتحقق من الاتصال
    '            If DatabaseHelper.TestConnection() Then
    '                ' إذا نجح الاتصال، جرب استرجاع بيانات بسيطة للتأكد من أن الجدول موجود
    '                Dim testQuery As String = "SELECT COUNT(*) FROM TbUsers"
    '                Using connection As SqlConnection = DatabaseHelper.Con()
    '                    connection.Open()
    '                    Using command As New SqlCommand(testQuery, connection)
    '                        Dim userCount As Integer = Convert.ToInt32(command.ExecuteScalar())
    '                        System.Diagnostics.Debug.WriteLine($"عدد المستخدمين: {userCount}")
    '                    End Using
    '                End Using

    '                ' تعيين نتيجة النجاح
    '                e.Result = True
    '            Else
    '                Throw New Exception("فشل في الاتصال بقاعدة البيانات")
    '            End If

    '        Catch ex As SqlException
    '            ' خطأ خاص بـ SQL Server
    '            e.Result = $"خطأ في قاعدة البيانات: {ex.Message}"
    '            System.Diagnostics.Debug.WriteLine($"SqlException: {ex.Message}")
    '        Catch ex As InvalidOperationException
    '            ' مشكلة في التهيئة أو الإعدادات
    '            e.Result = $"مشكلة في الإعدادات: {ex.Message}"
    '            System.Diagnostics.Debug.WriteLine($"InvalidOperationException: {ex.Message}")
    '        Catch ex As ConfigurationErrorsException
    '            ' خطأ في ملف التكوين
    '            e.Result = $"خطأ في التهيئة: {ex.Message}"
    '            System.Diagnostics.Debug.WriteLine($"ConfigurationErrorsException: {ex.Message}")
    '        Catch ex As Exception
    '            ' أي خطأ عام
    '            e.Result = $"خطأ عام: {ex.Message}"
    '            System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}")
    '        End Try








    '        ' التحقق من صحة إعدادات الاتصال قبل المحاولة
    '        If Not ValidateConnectionSettings() Then
    '            e.Result = New DatabaseConnectionResult() With {
    '                .IsSuccessful = False,
    '                .ErrorMessage = "إعدادات الاتصال غير مكتملة أو غير صحيحة",
    '                .ConnectionString = ""
    '            }
    '            Return
    '        End If

    '        Dim connectionResult As DatabaseConnectionResult = _connectionTester.TestConnection()

    '        If connectionResult.IsSuccessful Then
    '            worker.ReportProgress(70, "تم الاتصال بقاعدة البيانات بنجاح")
    '            Thread.Sleep(200)

    '            ' المرحلة 4: تحميل البيانات الأساسية
    '            worker.ReportProgress(85, "تحميل البيانات الأساسية...")
    '            Thread.Sleep(200)

    '            worker.ReportProgress(100, "تم بدء التطبيق بنجاح")
    '            e.Result = connectionResult
    '        Else
    '            ' فشل الاتصال
    '            e.Result = connectionResult
    '        End If

    '    Catch ex As OperationCanceledException
    '        e.Result = New DatabaseConnectionResult() With {
    '            .IsSuccessful = False,
    '            .ErrorMessage = "تم إلغاء عملية الاتصال",
    '            .ConnectionString = ""
    '        }
    '    Catch ex As Exception
    '        ' إرجاع نتيجة فشل مع تفاصيل الخطأ
    '        e.Result = New DatabaseConnectionResult() With {
    '            .IsSuccessful = False,
    '            .ErrorMessage = ex.Message,
    '            .ConnectionString = ""
    '        }
    '    End Try
    'End Sub

    '''' <summary>
    '''' التحقق من صحة إعدادات الاتصال
    '''' </summary>
    'Private Function ValidateConnectionSettings() As Boolean
    '    Try
    '        Select Case My.Settings.LoginMothed
    '            Case 0, 1 ' Windows Authentication أو SQL Server Authentication
    '                Return Not String.IsNullOrEmpty(My.Settings.ServerName) AndAlso
    '                       Not String.IsNullOrEmpty(My.Settings.DataBaseName)

    '            Case 2 ' اتصال عبر الشبكة
    '                Return Not String.IsNullOrEmpty(My.Settings.ServerIP) AndAlso
    '                       Not String.IsNullOrEmpty(My.Settings.LoginPort) AndAlso
    '                       Not String.IsNullOrEmpty(My.Settings.DataBaseName)

    '            Case 3 ' اتصال عبر WAN
    '                Return Not String.IsNullOrEmpty(My.Settings.WANConString)

    '            Case Else
    '                Return False
    '        End Select

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من إعدادات الاتصال: {ex.Message}")
    '        Return False
    '    End Try
    'End Function

    '''' <summary>
    '''' تحديث التقدم والحالة
    '''' </summary>
    'Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs)
    '    Try
    '        ' تحديث ProgressBar
    '        If progressBar.InvokeRequired Then
    '            progressBar.Invoke(Sub()
    '                                   progressBar.Value = e.ProgressPercentage
    '                               End Sub)
    '        Else
    '            progressBar.Value = e.ProgressPercentage
    '        End If

    '        ' تحديث نص الحالة
    '        If e.UserState IsNot Nothing Then
    '            If lblStatus.InvokeRequired Then
    '                lblStatus.Invoke(Sub()
    '                                     lblStatus.Text = e.UserState.ToString()
    '                                 End Sub)
    '            Else
    '                lblStatus.Text = e.UserState.ToString()
    '            End If
    '        End If

    '        ' تحديث النموذج
    '        Me.Refresh()

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تحديث التقدم: {ex.Message}")
    '    End Try
    'End Sub

    '''' <summary>
    '''' اكتمال العملية في الخلفية
    '''' </summary>
    'Private Sub BackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
    '    Try
    '        If _isFormClosing Then Return

    '        If e.Error IsNot Nothing Then
    '            ' حدث خطأ أثناء العملية
    '            HandleError("خطأ أثناء بدء التطبيق", e.Error)
    '            Return
    '        End If

    '        Dim result As DatabaseConnectionResult = CType(e.Result, DatabaseConnectionResult)

    '        If result.IsSuccessful Then
    '            ' نجح الاتصال - انتظار الحد الأدنى للوقت ثم إظهار شاشة تسجيل الدخول
    '            EnsureMinimumDisplayTime(AddressOf ShowLoginForm)
    '        Else
    '            ' فشل الاتصال - عرض رسالة خطأ
    '            ShowConnectionError(result)
    '        End If

    '    Catch ex As Exception
    '        HandleError("خطأ في اكتمال العملية", ex)
    '    End Try
    'End Sub

    '''' <summary>
    '''' ضمان عرض الشاشة للحد الأدنى من الوقت
    '''' </summary>
    'Private Sub EnsureMinimumDisplayTime(callback As Action)
    '    Try
    '        Dim elapsedTime As Integer = CInt((DateTime.Now - _startTime).TotalMilliseconds)
    '        Dim remainingTime As Integer = Math.Max(0, _minimumDisplayTime - elapsedTime)

    '        If remainingTime > 0 Then
    '            ' انتظار الوقت المتبقي
    '            Dim timer As New System.Windows.Forms.Timer
    '            timer.Interval = remainingTime
    '            AddHandler timer.Tick, Sub(s, e)
    '                                       timer.Stop()
    '                                       timer.Dispose()
    '                                       callback.Invoke()
    '                                   End Sub
    '            timer.Start()
    '        Else
    '            ' تنفيذ الCallback مباشرة
    '            callback.Invoke()
    '        End If

    '    Catch ex As Exception
    '        ' في حالة حدوث خطأ، تنفيذ الCallback مباشرة
    '        callback.Invoke()
    '    End Try
    'End Sub

    ''' <summary>
    ''' إظهار شاشة تسجيل الدخول وإغلاق شاشة البداية
    ''' </summary>
    Private Sub ShowLoginForm()
        Try
            ' التحقق من أن النموذج لا يزال قيد التشغيل
            If _isFormClosing OrElse Me.IsDisposed OrElse Me.Disposing Then
                Return
            End If

            ' إخفاء شاشة البداية
            Me.Hide()

            ' التحقق مما إذا كان يجب عرض معالج التثبيت
            If ShouldShowInstallWizard() Then
                ShowInstallWizard()
                Return
            End If

            ' التحقق من اتصال قاعدة البيانات
            If Not TestDatabaseConnection() Then
                HandleDatabaseConnectionError()
                Return
            End If

            ' إظهار شاشة تسجيل الدخول
            Using loginForm As New LoginForm()
                If loginForm.ShowDialog() = DialogResult.OK Then
                    ' إظهار النموذج الرئيسي
                    Using mainForm As New MainForm()
                        mainForm.ShowDialog()
                    End Using
                End If
            End Using

            ' إغلاق شاشة البداية
            Me.Close()

        Catch ex As Exception
            HandleError("خطأ في إظهار شاشة تسجيل الدخول", ex)
        End Try
    End Sub

    ''' <summary>
    ''' التحقق مما إذا كان يجب عرض معالج التثبيت
    ''' </summary>
    Private Function ShouldShowInstallWizard() As Boolean
        Try
            ' التحقق من أن جميع إعدادات الاتصال الأساسية فارغة
            Return String.IsNullOrEmpty(My.Settings.ServerName) AndAlso
                   String.IsNullOrEmpty(My.Settings.ServerIP) AndAlso
                   String.IsNullOrEmpty(My.Settings.WANConString) AndAlso
                   (My.Settings.LoginMothed = -1 Or My.Settings.LoginMothed = 0)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من عرض المعالج: {ex.Message}")
            Return True ' في حالة الخطأ، اعرض المعالج كإجراء احترازي
        End Try
    End Function

    ''' <summary>
    ''' عرض معالج التثبيت
    ''' </summary>
    Private Sub ShowInstallWizard()
        Try
            ' إخفاء شاشة البداية
            Me.Hide()

            ' عرض معالج التثبيت
            Using wizardForm As New InstallWizardForm()
                wizardForm.ShowDialog()
            End Using

            ' بعد إغلاق المعالج، أغلق شاشة البداية
            Me.Close()

        Catch ex As Exception
            MessageBox.Show($"خطأ في فتح نموذج التثبيت: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            HandleError("فشل في عرض معالج التثبيت", ex)
            Application.Exit()
        End Try
    End Sub

    ''' <summary>
    ''' اختبار اتصال قاعدة البيانات
    ''' </summary>
    Private Function TestDatabaseConnection() As Boolean
        Try
            ' التحقق من صحة إعدادات الاتصال
            If Not ValidateConnectionSettings() Then
                Return False
            End If

            ' اختبار الاتصال الفعلي
            Return DatabaseHelper.TestConnection()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في اختبار الاتصال: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' التعامل مع أخطاء اتصال قاعدة البيانات
    ''' </summary>
    Private Sub HandleDatabaseConnectionError()
        Try
            Dim result As DialogResult = MessageBox.Show(
                "تعذر الاتصال بقاعدة البيانات. هل تريد تكوين إعدادات الاتصال الآن؟" & vbCrLf &
                "قد تكون إعدادات الاتصال غير صحيحة أو السيرفر غير متاح.",
                "اتصال قاعدة البيانات",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                ' عرض نموذج إعدادات الاتصال
                Using connectionForm As New ServerConnectionForm()
                    If connectionForm.ShowDialog() = DialogResult.OK Then
                        ' إعادة المحاولة بعد تحديث الإعدادات
                        If TestDatabaseConnection() Then
                            ' إعادة عرض نموذج تسجيل الدخول
                            ShowLoginForm()
                        Else
                            MessageBox.Show("لا يزال هناك مشكلة في الاتصال بقاعدة البيانات.", "اتصال فاشل", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Application.Exit()
                        End If
                    Else
                        Application.Exit()
                    End If
                End Using
            Else
                Application.Exit()
            End If

        Catch ex As Exception
            HandleError("فشل في التعامل مع خطأ الاتصال", ex)
            Application.Exit()
        End Try
    End Sub

    ''' <summary>
    ''' عرض خطأ الاتصال بقاعدة البيانات
    ''' </summary>
    Private Sub ShowConnectionError(result As DatabaseConnectionResult)
        Try
            lblStatus.Text = "فشل الاتصال بقاعدة البيانات"
            lblStatus.ForeColor = Color.Red
            progressBar.Value = 0

            Dim errorMessage As String = $"فشل في الاتصال بقاعدة البيانات:{vbNewLine}{vbNewLine}" &
                                       $"الخطأ: {result.ErrorMessage}{vbNewLine}{vbNewLine}" &
                                       "يرجى التحقق من:" & vbNewLine & vbNewLine &
                                       "• إعدادات الاتصال بقاعدة البيانات" & vbNewLine & vbNewLine &
                                       "• تشغيل خادم قاعدة البيانات" & vbNewLine & vbNewLine &
                                       "• صحة سلسلة الاتصال" & vbNewLine & vbNewLine &
                                       "هل تريد المحاولة مرة أخرى؟"

            Dim dialogResult As DialogResult = MessageBox.Show(
                errorMessage,
                "خطأ في الاتصال بقاعدة البيانات",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Error)

            If dialogResult = DialogResult.Yes Then
                ' إعادة المحاولة
                RestartConnectionTest()
            ElseIf dialogResult = DialogResult.No Then
                ' إغلاق التطبيق
                Application.Exit()
            Else
                Me.Hide()
                ' عرض نموذج إعدادات الاتصال
                Using connectionForm As New ServerConnectionForm()
                    If connectionForm.ShowDialog() = DialogResult.OK Then
                        ' إعادة المحاولة بعد تحديث الإعدادات
                        If TestDatabaseConnection() Then
                            ' إعادة عرض نموذج تسجيل الدخول
                            ShowLoginForm()
                        Else
                            MessageBox.Show("لا يزال هناك مشكلة في الاتصال بقاعدة البيانات.", "اتصال فاشل", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Application.Exit()
                        End If
                    Else
                        Application.Restart()
                    End If
                End Using
            End If

        Catch ex As Exception
            HandleError("خطأ في عرض رسالة الخطأ", ex)
        End Try
    End Sub

    ''' <summary>
    ''' إعادة بدء اختبار الاتصال
    ''' </summary>
    Private Sub RestartConnectionTest()
        Try
            ' إعادة تعيين الواجهة
            progressBar.Value = 0
            lblStatus.Text = "إعادة المحاولة..."
            lblStatus.ForeColor = Color.LightBlue

            ' إعادة تعيين وقت البداية
            _startTime = DateTime.Now

            ' بدء العملية مرة أخرى
            If Not _backgroundWorker.IsBusy Then
                _backgroundWorker.RunWorkerAsync()
            End If

        Catch ex As Exception
            HandleError("خطأ في إعادة بدء الاختبار", ex)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الأخطاء العامة
    ''' </summary>
    Private Sub HandleError(title As String, ex As Exception)
        Try
            Dim errorMessage As String = $"{title}:{vbNewLine}{vbNewLine}{ex.Message}"

            ' التحقق من الحاجة إلى Invoke
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              MessageBox.Show(errorMessage, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                              Application.Exit()
                          End Sub)
            Else
                MessageBox.Show(errorMessage, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Application.Exit()
            End If

        Catch
            ' في حالة فشل كل شيء، إغلاق التطبيق
            Application.Exit()
        End Try
    End Sub

    ''' <summary>
    ''' تنظيف الموارد عند إغلاق النموذج
    ''' </summary>
    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        Try
            _isFormClosing = True

            ' إيقاف BackgroundWorker إذا كان يعمل
            If _backgroundWorker IsNot Nothing AndAlso _backgroundWorker.IsBusy Then
                _backgroundWorker.CancelAsync()
            End If

            ' تنظيف الموارد
            If _backgroundWorker IsNot Nothing Then
                _backgroundWorker.Dispose()
            End If

            If _connectionTester IsNot Nothing Then
                _connectionTester.Dispose()
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تنظيف الموارد: {ex.Message}")
        Finally
            MyBase.OnFormClosed(e)
        End Try
    End Sub

    ''' <summary>
    ''' منع إغلاق النموذج بواسطة Alt+F4 أو X
    ''' </summary>
    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        ' السماح بالإغلاق فقط من الكود
        If e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
        End If

        MyBase.OnFormClosing(e)
    End Sub

    ' أحداث السحب والنقل للنافذة
    Private Sub panelMain_MouseDown(sender As Object, e As MouseEventArgs) Handles panelMain.MouseDown
        Drag = True
        MouseX = Cursor.Position.X - Me.Left
        MouseY = Cursor.Position.Y - Me.Top
    End Sub

    Private Sub panelMain_MouseMove(sender As Object, e As MouseEventArgs) Handles panelMain.MouseMove
        If Drag Then
            Me.Top = Cursor.Position.Y - MouseY
            Me.Left = Cursor.Position.X - MouseX
        End If
    End Sub

    Private Sub panelMain_MouseUp(sender As Object, e As MouseEventArgs) Handles panelMain.MouseUp
        Drag = False
    End Sub
End Class



'Imports System.ComponentModel
'Imports System.Threading
'Imports System.Data.SqlClient

'''' <summary>
'''' شاشة البداية مع التحقق من الاتصال بقاعدة البيانات
'''' </summary>
'Public Class SplashScreen

'    ' متغيرات للتحكم في العملية
'    Private _connectionTester As DatabaseConnectionTester
'    Private _backgroundWorker As BackgroundWorker
'    Private _startTime As DateTime
'    Private _minimumDisplayTime As Integer = 3000 ' 3 ثوانٍ كحد أدنى لعرض الشاشة


'    ' متغيرات السحب والنقل للنافذة
'    Private Drag As Boolean = False
'    Private MouseX As Integer
'    Private MouseY As Integer

'    ''' <summary>
'    ''' Constructor
'    ''' </summary>
'    Public Sub New()
'        ' هذا السطر مطلوب لتهيئة النموذج
'        InitializeComponent()

'        ' إعداد النموذج
'        SetupForm()

'        ' إعداد BackgroundWorker
'        SetupBackgroundWorker()

'        ' إعداد كلاس اختبار الاتصال
'        _connectionTester = New DatabaseConnectionTester()
'    End Sub

'    ''' <summary>
'    ''' إعداد خصائص النموذج
'    ''' </summary>
'    Private Sub SetupForm()
'        Try
'            ' إعداد النموذج كشاشة بداية
'            Me.FormBorderStyle = FormBorderStyle.None
'            Me.StartPosition = FormStartPosition.CenterScreen
'            Me.Size = New Size(600, 400)
'            Me.BackColor = Color.FromArgb(225, 245, 254) ' لون خلفية داكن
'            Me.ShowInTaskbar = False
'            Me.TopMost = True

'            ' إعداد النصوص والألوان
'            'lblAppName.ForeColor = Color.White
'            'lblVersion.ForeColor = Color.LightGray
'            'lblStatus.ForeColor = Color.LightBlue
'            'lblCopyright.ForeColor = Color.Gray

'            ' إعداد ProgressBar
'            progressBar.Style = ProgressBarStyle.Continuous
'            progressBar.Minimum = 0
'            progressBar.Maximum = 100
'            progressBar.Value = 0

'            ' تسجيل وقت البداية
'            _startTime = DateTime.Now

'        Catch ex As Exception
'            MessageBox.Show($"خطأ في إعداد شاشة البداية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' إعداد BackgroundWorker للعمليات في الخلفية
'    ''' </summary>
'    Private Sub SetupBackgroundWorker()
'        _backgroundWorker = New BackgroundWorker()
'        _backgroundWorker.WorkerReportsProgress = True
'        _backgroundWorker.WorkerSupportsCancellation = False

'        AddHandler _backgroundWorker.DoWork, AddressOf BackgroundWorker_DoWork
'        AddHandler _backgroundWorker.ProgressChanged, AddressOf BackgroundWorker_ProgressChanged
'        AddHandler _backgroundWorker.RunWorkerCompleted, AddressOf BackgroundWorker_RunWorkerCompleted
'    End Sub

'    ''' <summary>
'    ''' بدء عملية التحقق من الاتصال
'    ''' </summary>
'    Private Sub SplashScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        Try
'            ' عرض معلومات التطبيق
'            lblAppName.Text = "نظام ESIS"

'            ' جلب رقم الإصدار من AssemblyInfo
'            Dim version As Version = My.Application.Info.Version
'            lblVersion.Text = "الإصدار " & version.ToString()

'            ' جلب حقوق الملكية من إعدادات المشروع
'            lblCopyright.Text = My.Settings.CopyrightText



'            'lblVersion.Text = "الإصدار 2.0"
'            'lblCopyright.Text = "© 2024 جميع الحقوق محفوظة"
'            lblStatus.Text = "جاري بدء التطبيق..."

'            ' بدء عملية التحقق
'            _backgroundWorker.RunWorkerAsync()

'        Catch ex As Exception
'            HandleError("خطأ في تحميل شاشة البداية", ex)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' العمليات التي تتم في الخلفية
'    ''' </summary>
'    Private Sub BackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs)
'        Try
'            Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

'            ' المرحلة 1: التحقق من ملفات النظام
'            worker.ReportProgress(10, "فحص ملفات النظام...")
'            Thread.Sleep(500)

'            ' المرحلة 2: تحميل الإعدادات
'            worker.ReportProgress(25, "تحميل إعدادات التطبيق...")
'            Thread.Sleep(500)

'            ' المرحلة 3: التحقق من الاتصال بقاعدة البيانات
'            worker.ReportProgress(40, "التحقق من الاتصال بقاعدة البيانات...")

'            Dim connectionResult As DatabaseConnectionResult = _connectionTester.TestConnection()

'            If connectionResult.IsSuccessful Then
'                worker.ReportProgress(70, "تم الاتصال بقاعدة البيانات بنجاح")
'                Thread.Sleep(500)

'                ' المرحلة 4: تحميل البيانات الأساسية
'                worker.ReportProgress(85, "تحميل البيانات الأساسية...")
'                Thread.Sleep(500)

'                worker.ReportProgress(100, "تم بدء التطبيق بنجاح")
'                e.Result = connectionResult
'            Else
'                ' فشل الاتصال
'                e.Result = connectionResult
'            End If

'        Catch ex As Exception
'            ' إرجاع نتيجة فشل مع تفاصيل الخطأ
'            e.Result = New DatabaseConnectionResult() With {
'                .IsSuccessful = False,
'                .ErrorMessage = ex.Message,
'                .connectionString = ""
'            }
'        End Try
'    End Sub

'    ''' <summary>
'    ''' تحديث التقدم والحالة
'    ''' </summary>
'    Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs)
'        Try
'            ' تحديث ProgressBar
'            progressBar.Value = e.ProgressPercentage

'            ' تحديث نص الحالة
'            If e.UserState IsNot Nothing Then
'                lblStatus.Text = e.UserState.ToString()
'            End If

'            ' تحديث النموذج
'            Me.Refresh()

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث التقدم: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' اكتمال العملية في الخلفية
'    ''' </summary>
'    Private Sub BackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
'        Try
'            If e.Error IsNot Nothing Then
'                ' حدث خطأ أثناء العملية
'                HandleError("خطأ أثناء بدء التطبيق", e.Error)
'                Return
'            End If

'            Dim result As DatabaseConnectionResult = CType(e.Result, DatabaseConnectionResult)

'            If result.IsSuccessful Then
'                ' نجح الاتصال - انتظار الحد الأدنى للوقت ثم إظهار شاشة تسجيل الدخول
'                EnsureMinimumDisplayTime(Sub() ShowLoginForm())
'            Else
'                ' فشل الاتصال - عرض رسالة خطأ
'                ShowConnectionError(result)
'            End If

'        Catch ex As Exception
'            HandleError("خطأ في اكتمال العملية", ex)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' ضمان عرض الشاشة للحد الأدنى من الوقت
'    ''' </summary>
'    Private Sub EnsureMinimumDisplayTime(callback As Action)
'        Try
'            Dim elapsedTime As Integer = CInt((DateTime.Now - _startTime).TotalMilliseconds)
'            Dim remainingTime As Integer = Math.Max(0, _minimumDisplayTime - elapsedTime)

'            If remainingTime > 0 Then
'                ' انتظار الوقت المتبقي
'                Dim timer As New System.Windows.Forms.Timer
'                timer.Interval = remainingTime
'                AddHandler timer.Tick, Sub(s, e)
'                                           timer.Stop()
'                                           timer.Dispose()
'                                           callback.Invoke()
'                                       End Sub
'                timer.Start()
'            Else
'                ' تنفيذ الCallback مباشرة
'                callback.Invoke()
'            End If

'        Catch ex As Exception
'            ' في حالة حدوث خطأ، تنفيذ الCallback مباشرة
'            callback.Invoke()
'        End Try
'    End Sub

'    ''' <summary>
'    ''' إظهار شاشة تسجيل الدخول وإغلاق شاشة البداية
'    ''' </summary>
'    Private Sub ShowLoginForm()
'        Try
'            Me.Invoke(Sub()
'                          Try
'                              ' إخفاء شاشة البداية
'                              Me.Hide()

'                              'If String.IsNullOrEmpty(My.Settings.ServerName) And String.IsNullOrEmpty(My.Settings.ServerIP) And String.IsNullOrEmpty(My.Settings.WANConString) Then
'                              '    InstallWizardForm.Show()
'                              'Else
'                              ' إظهار شاشة تسجيل الدخول
'                              Dim loginForm As New LoginForm()

'                                  ' إذا تم تسجيل الدخول بنجاح، إظهار النموذج الرئيسي
'                                  If loginForm.ShowDialog() = DialogResult.OK Then
'                                      ' إظهار النموذج الرئيسي
'                                      Dim mainForm As New MainForm()
'                                      mainForm.Show()
'                                      'Application.Run(mainForm)
'                                  End If

'                                  ' إغلاق شاشة البداية
'                                  Me.Close()
'                              'End If

'                          Catch ex As Exception
'                              HandleError("خطأ في إظهار شاشة تسجيل الدخول", ex)
'                          End Try
'                      End Sub)

'        Catch ex As Exception
'            HandleError("خطأ في إظهار شاشة تسجيل الدخول", ex)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' عرض خطأ الاتصال بقاعدة البيانات
'    ''' </summary>
'    Private Sub ShowConnectionError(result As DatabaseConnectionResult)
'        Try
'            Me.Invoke(Sub()
'                          lblStatus.Text = "فشل الاتصال بقاعدة البيانات"
'                          lblStatus.ForeColor = Color.Red
'                          progressBar.Value = 0

'                          Dim errorMessage As String = $"فشل في الاتصال بقاعدة البيانات:{vbNewLine}{vbNewLine}" &
'                                                     $"الخطأ: {result.ErrorMessage}{vbNewLine}{vbNewLine}" &
'                                                     "يرجى التحقق من:" & vbNewLine & vbNewLine &
'                                                     "• إعدادات الاتصال بقاعدة البيانات" & vbNewLine & vbNewLine &
'                                                     "• تشغيل خادم قاعدة البيانات" & vbNewLine & vbNewLine &
'                                                     "• صحة سلسلة الاتصال" & vbNewLine & vbNewLine &
'                                                     "هل تريد المحاولة مرة أخرى؟"

'                          Dim dialogResult As DialogResult = MessageBox.Show(
'                              errorMessage,
'                              "خطأ في الاتصال بقاعدة البيانات",
'                              MessageBoxButtons.YesNo,
'                              MessageBoxIcon.Error)

'                          If dialogResult = DialogResult.Yes Then
'                              ' إعادة المحاولة
'                              RestartConnectionTest()
'                          Else
'                              ' إغلاق التطبيق
'                              Application.Exit()
'                          End If
'                      End Sub)

'        Catch ex As Exception
'            HandleError("خطأ في عرض رسالة الخطأ", ex)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' إعادة بدء اختبار الاتصال
'    ''' </summary>
'    Private Sub RestartConnectionTest()
'        Try
'            ' إعادة تعيين الواجهة
'            progressBar.Value = 0
'            lblStatus.Text = "إعادة المحاولة..."
'            lblStatus.ForeColor = Color.LightBlue

'            ' إعادة تعيين وقت البداية
'            _startTime = DateTime.Now

'            ' بدء العملية مرة أخرى
'            If Not _backgroundWorker.IsBusy Then
'                _backgroundWorker.RunWorkerAsync()
'            End If

'        Catch ex As Exception
'            HandleError("خطأ في إعادة بدء الاختبار", ex)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' معالجة الأخطاء العامة
'    ''' </summary>
'    Private Sub HandleError(title As String, ex As Exception)
'        Try
'            Dim errorMessage As String = $"{title}:{vbNewLine}{vbNewLine}{ex.Message}"

'            Me.Invoke(Sub()
'                          MessageBox.Show(errorMessage, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                          Application.Exit()
'                      End Sub)

'        Catch
'            ' في حالة فشل كل شيء، إغلاق التطبيق
'            Application.Exit()
'        End Try
'    End Sub

'    ''' <summary>
'    ''' تنظيف الموارد عند إغلاق النموذج
'    ''' </summary>
'    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
'        Try
'            ' إيقاف BackgroundWorker إذا كان يعمل
'            If _backgroundWorker IsNot Nothing AndAlso _backgroundWorker.IsBusy Then
'                _backgroundWorker.CancelAsync()
'            End If

'            ' تنظيف الموارد
'            If _backgroundWorker IsNot Nothing Then
'                _backgroundWorker.Dispose()
'            End If

'            If _connectionTester IsNot Nothing Then
'                _connectionTester.Dispose()
'            End If

'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في تنظيف الموارد: {ex.Message}")
'        Finally
'            MyBase.OnFormClosed(e)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' منع إغلاق النموذج بواسطة Alt+F4 أو X
'    ''' </summary>
'    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
'        ' السماح بالإغلاق فقط من الكود
'        If e.CloseReason = CloseReason.UserClosing Then
'            e.Cancel = True
'        End If

'        MyBase.OnFormClosing(e)
'    End Sub

'    ' أحداث السحب والنقل للنافذة
'    Private Sub panelMain_MouseDown(sender As Object, e As MouseEventArgs) Handles panelMain.MouseDown
'        Drag = True
'        MouseX = Cursor.Position.X - Me.Left
'        MouseY = Cursor.Position.Y - Me.Top
'    End Sub

'    Private Sub panelMain_MouseMove(sender As Object, e As MouseEventArgs) Handles panelMain.MouseMove
'        If Drag Then
'            Me.Top = Cursor.Position.Y - MouseY
'            Me.Left = Cursor.Position.X - MouseX
'        End If
'    End Sub

'    Private Sub panelMain_MouseUp(sender As Object, e As MouseEventArgs) Handles panelMain.MouseUp
'        Drag = False
'    End Sub


'End Class