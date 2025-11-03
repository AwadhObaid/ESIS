Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine

Public Class MainForm : Inherits BaseFormWithPermissions
    Private scaler As ControlScaler
    Private archiveClass As New ArchiveClass()
    Private activityLogClass As New ActivityLogClass()
    Private cleanupTimer As New System.Windows.Forms.Timer()

    ' تعريف المتغيرات في بداية النموذج الرئيسي
    Private _notificationsForm As NotificationsForm = Nothing
    Private _notificationBalloon As NotificationsForm = Nothing
    Private _notificationTaskbar As NotificationsForm = Nothing
    Public xx As Short = 0

#Region "التهيئة والتحميل"
    ''' <summary>
    ''' دالة البناء: تهيئة النموذج وإعداد مقياس التحكم
    ''' </summary>
    Public Sub New()
        ' هذا السطر مطلوب لتهيئة النموذج
        InitializeComponent()

        ' تهيئة كلاس التحجيم مع النموذج الحالي
        scaler = New ControlScaler(Me)
    End Sub

    ''' <summary>
    ''' بدء العد التنازلي للحذف التلقائي
    ''' </summary>
    Private Sub StartCountdownTimer()
        Try
            ' إنشاء مؤقت جديد
            Dim countdownTimer As New System.Windows.Forms.Timer()
            countdownTimer.Interval = 60000 ' كل دقيقة
            AddHandler countdownTimer.Tick, AddressOf CountdownTimer_Tick
            countdownTimer.Start()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في بدء العد التنازلي: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' حدث تكرار المؤقت للعد التنازلي
    ''' </summary>
    Private Sub CountdownTimer_Tick(sender As Object, e As EventArgs)
        Try
            ' حساب الأيام المتبقية
            Dim daysRemaining As Integer = archiveClass.GetDaysUntilDeletion()

            ' تحديث ليبل العد التنازلي
            If lblCountdown IsNot Nothing Then
                lblCountdown.Text = $"الحذف التلقائي بعد: {daysRemaining} يوم"
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في العد التنازلي: {ex.Message}")
        End Try
    End Sub


    ''' <summary>
    ''' تحميل النموذج الرئيسي وتهيئة الإعدادات الأولية
    ''' </summary>
    Public Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' إعدادات النموذج الأساسية
            Me.Text = "الواجهة الرئيسية - نظام ESIS"
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            SetBackColor()

            ' تعيين وقت التكرار (مثال: كل 24 ساعة)
            cleanupTimer.Interval = 24 * 60 * 60 * 1000 ' 24 ساعة بالمللي ثانية
            AddHandler cleanupTimer.Tick, AddressOf CleanupTimer_Tick
            cleanupTimer.Start()
            ' بدء العد التنازلي
            StartCountdownTimer()

            ' عرض اسم المستخدم في شريط الحالة
            lblUser.Text = GlobalData.LoggedInUserName
            lblUser2.Text = GlobalData.LoggedInUserName

            ' التحقق إذا كان المستخدم مسؤولاً
            Dim isAdmin As Boolean = CheckIfAdmin()
            If isAdmin Then
                lblUser.Text &= "  (مسؤول النظام)"
            End If
            ' تحميل صورة المستخدم
            RefreshUserImage()
            ' بدء مؤقت التحديث
            timerClock.Start()

            ' تحديث حالة الترخيص
            'RefreshLicenseStatus()

            ' تحديث التاريخ والوقت فور التحميل
            UpdateDateTime()

            Try
                ' تهيئة مراقب الشبكة
                _networkMonitor.Initialize()

                ' الاشتراك في أحداث تغيير الحالة
                AddHandler _networkMonitor.LocalNetworkStatusChanged, AddressOf OnLocalNetworkStatusChanged
                AddHandler _networkMonitor.InternetStatusChanged, AddressOf OnInternetStatusChanged

                ' التحقق من حالة الاتصال عند التحميل
                CheckConnectionStatus()
                ' السماح بتحديث واجهة المستخدم
                _isInitializing = False

            Catch ex As Exception
                MessageBox.Show($"خطأ في تهيئة مراقب الشبكة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try


            ' تهيئة NotifyIcon
            InitializeNotifyIcon()
            ' الاشتراك في أحداث تحديث العدادات
            AddHandler DataHelper.SaderCountChanged, AddressOf OnDataCountChanged
            AddHandler DataHelper.ArchiveCountChanged, AddressOf OnArchiveCountChanged
            AddHandler DataHelper.DeletedArchiveCountChanged, AddressOf OnDeletedArchiveCountChanged


            InitializeUnarchivedNotifications()
            InitializeTaskbarNotification()
            ' عرض الإشعارات إذا كانت هناك سجلات غير مؤرشفة
            If DataHelper.UnarchivedSaderCount > 0 Then
                ShowTemporaryNotification()
            End If
            InitializeUnarchivedCounter()
            ' بدء مراقبة السجلات غير المؤرشفة
            DataHelper.StartMonitoringUnarchivedRecords(300) ' كل 5 دقائق
            ' تحديث العدادات في الواجهة
            UpdateSaderCountLabel()
            UpdateArchiveCountLabel()
            UpdateDeletedArchiveCountLabel()
            '' تحديث تسمية العدد عند التحميل
            'UpdateUnarchivedCountLabel(DataHelper.UnarchivedSaderCount)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء تحميل النموذج الرئيسي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' حدث تكرار المهمة
    ''' </summary>
    Private Sub CleanupTimer_Tick(sender As Object, e As EventArgs)
        Try
            ' تنفيذ عملية التنظيف التلقائي (الحفاظ على الأرشيف المحذوف لمدة 30 يومًا)
            Dim deletedCount As Integer = archiveClass.PermanentlyDeleteArchives(30, False)

            ' تسجيل العملية في سجل الأحداث
            If deletedCount > 0 Then
                ActivityLogClass.LogUserActivity("System", "AutoCleanup", "TbArchive", -1, $"تم حذف {deletedCount} أرشيف محذوف نهائيًا.")
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التنظيف التلقائي: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تعيين لون خلفية النافذة الرئيسية وتوسيعها إلى الحجم الأقصى
    ''' </summary>
    Private Sub SetBackColor()
        Try
            Me.IsMdiContainer = True
            For Each Child As Control In Me.Controls
                If TypeOf Child Is MdiClient Then
                    Child.BackColor = Me.BackColor
                    Exit For
                End If
            Next
            Me.WindowState = FormWindowState.Maximized
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تعيين لون الخلفية: {ex.Message}")
        End Try
    End Sub
#End Region

#Region "الاشعارات"
    ' متغير لتخزين إشعار السجلات غير المؤرشفة
    ' متغير لتخزين نموذج الإشعارات
    'Private _notificationsForm As NotificationsForm = Nothing

    Private Sub InitializeUnarchivedNotifications()
        Try
            ' بدء المراقبة الدورية للسجلات غير المؤرشفة (كل 5 دقائق)
            DataHelper.StartMonitoringUnarchivedRecords(300)

            ' الاشتراك في حدث تغيير العدد
            AddHandler DataHelper.UnarchivedSaderCountChanged, AddressOf HandleUnarchivedCountChanged

            '' عرض الإشعار الأولي
            'ShowUnarchivedNotification()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تهيئة إشعارات السجلات غير المؤرشفة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' عرض نموذج الإشعارات (الشاشة الكاملة)
    ''' </summary>
    Private Sub ShowNotifications()
        Try
            ' التحقق من وجود النموذج
            If _notificationsForm Is Nothing OrElse _notificationsForm.IsDisposed Then
                ' التصحيح: تمرير نوع الإشعار كمعلمة
                _notificationsForm = New NotificationsForm(NotificationsForm.NotificationType.Popup)
                ' الاشتراك في حدث الإغلاق إذا لزم الأمر
                AddHandler _notificationsForm.FormClosed, AddressOf NotificationsForm_FormClosed
            End If

            ' عرض النموذج وجلبه إلى المقدمة
            If _notificationsForm.WindowState = FormWindowState.Minimized Then
                _notificationsForm.WindowState = FormWindowState.Normal
            End If

            _notificationsForm.MdiParent = Me
            _notificationsForm.Show()
            _notificationsForm.BringToFront()
            _notificationsForm.Focus()
        Catch ex As Exception
            MessageBox.Show($"خطأ في عرض نموذج الإشعارات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    '''' <summary>
    '''' عرض إشعار للسجلات غير المؤرشفة
    '''' </summary>
    'Private Sub ShowUnarchivedNotification()
    '    Try
    '        Dim count As Integer = DataHelper.UnarchivedSaderCount

    '        ' إظهار إشعار إذا كان هناك سجلات غير مؤرشفة
    '        If count > 0 Then
    '            ' التحقق من أن الإشعار ليس مفتوحًا بالفعل
    '            If _notificationsForm Is Nothing OrElse _notificationsForm.IsDisposed Then
    '                _notificationsForm = New NotificationsForm(NotificationsForm.NotificationType.Balloon)
    '            End If

    '            ' عرض الإشعار
    '            _notificationsForm.ShowUnarchivedRecords()
    '        Else
    '            ' إخفاء الإشعار إذا لم تكن هناك سجلات غير مؤرشفة
    '            If _notificationsForm IsNot Nothing AndAlso Not _notificationsForm.IsDisposed Then
    '                _notificationsForm.Close()
    '                _notificationsForm = Nothing
    '            End If
    '        End If
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في عرض إشعار السجلات غير المؤرشفة: {ex.Message}")
    '    End Try
    'End Sub

    ''' <summary>
    ''' عرض إشعار للسجلات غير المؤرشفة
    ''' </summary>
    Private Sub ShowUnarchivedNotification()
        Try
            Dim count As Integer = DataHelper.UnarchivedSaderCount

            ' إظهار إشعار إذا كان هناك سجلات غير مؤرشفة
            If count > 0 Then
                ' التحقق من أن الإشعار ليس مفتوحًا بالفعل
                If _notificationsForm Is Nothing OrElse _notificationsForm.IsDisposed Then
                    _notificationsForm = New NotificationsForm(NotificationsForm.NotificationType.Balloon)

                    ' تعيين خصائص النموذج لجعله يظهر في الزاوية السفلية اليسرى
                    With _notificationsForm
                        .StartPosition = FormStartPosition.Manual
                        .TopMost = True
                        .FormBorderStyle = FormBorderStyle.None
                        .Opacity = 0.95D ' شفافية جزئية
                    End With
                End If

                ' حساب الموضع الصحيح وعرض النموذج
                Dim position As Point = CalculateNotificationPosition()
                _notificationsForm.Location = position

                ' عرض الإشعار
                _notificationsForm.ShowUnarchivedRecords()
            Else
                ' إخفاء الإشعار إذا لم تكن هناك سجلات غير مؤرشفة
                If _notificationsForm IsNot Nothing AndAlso Not _notificationsForm.IsDisposed Then
                    _notificationsForm.Close()
                    _notificationsForm = Nothing
                End If
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في عرض إشعار السجلات غير المؤرشفة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' عرض إشعار مع إغلاق تلقائي بعد فترة
    ''' </summary>
    Private Sub ShowTemporaryNotification()
        Try
            Dim count As Integer = DataHelper.UnarchivedSaderCount

            If count > 0 Then
                ShowUnarchivedNotification()

                ' إعداد مؤقت لإغلاق تلقائي
                Dim closeTimer As New Timer()
                closeTimer.Interval = 10000 ' 10 ثوانٍ
                AddHandler closeTimer.Tick, Sub(sender As Object, e As EventArgs)
                                                closeTimer.Stop()
                                                closeTimer.Dispose()
                                                If _notificationsForm IsNot Nothing AndAlso Not _notificationsForm.IsDisposed Then
                                                    _notificationsForm.Close()
                                                    _notificationsForm = Nothing
                                                End If
                                            End Sub
                closeTimer.Start()
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في عرض الإشعار المؤقت: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' حساب موضع الزاوية السفلية اليسرى لإظهار نموذج الإشعار
    ''' </summary>
    Private Function CalculateNotificationPosition() As Point
        Try
            ' الحصول على موضع النموذج الرئيسي على الشاشة
            Dim mainFormScreenPoint As Point = Me.PointToScreen(Point.Empty)

            ' حساب الموضع المستهدف
            ' نريد أن يظهر في الزاوية السفلية اليسرى مع هامش صغير
            Dim xPosition As Integer = mainFormScreenPoint.Y + 20 ' هامش 20 بكسل من اليسار
            Dim yPosition As Integer = mainFormScreenPoint.X + Me.Height - 100 ' 100 بكسل من الحافة السفلية

            ' التأكد من أن الموضع ضمن حدود الشاشة
            Dim workingArea As Rectangle = Screen.GetWorkingArea(Me)
            xPosition = Math.Max(workingArea.Left, Math.Min(xPosition, workingArea.Right - 400))
            yPosition = Math.Max(workingArea.Top, Math.Min(yPosition, workingArea.Bottom - 180))

            Return New Point(xPosition, yPosition)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حساب موضع الإشعار: {ex.Message}")
            ' إذا فشل الحساب، استخدم الموضع الافتراضي
            Return New Point(Me.Location.X + 20, Me.Location.Y + Me.Height - 100)
        End Try
    End Function

    'Private Sub ShowUnarchivedNotification(count As Integer)
    '    Try
    '        If count > 0 Then
    '            ' إنشاء إشعار Toast حديث
    '            Dim toast As New ToastContentBuilder()
    '            toast.AddText("تنبيه: سجلات غير مؤرشفة")
    '            toast.AddText($"لديك {count} سجل غير مؤرشف. انقر لعرض التفاصيل.")
    '            toast.AddAppLogoOverride(New Uri("ms-appx:///Assets/AppLogo.png"), ToastImageCrop.Circle)

    '            ' إضافة زر للتفاعل مع الإشعار
    '            toast.AddButton("عرض التفاصيل", ToastActivationType.Foreground, "view_unarchived_records")

    '            ' عرض الإشعار
    '            toast.Show()
    '        Else
    '            ' إخفاء أي إشعارات قديمة
    '            ToastNotificationManagerCompat.History.Clear()
    '        End If
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في عرض إشعار السجلات غير المؤرشفة: {ex.Message}")

    '        ' كخيار بديل، استخدم الإشعار القديم
    '        FallbackToBalloonNotification(count)
    '    End Try
    'End Sub

    ''' <summary>
    ''' خيار بديل باستخدام الإشعار القديم
    ''' </summary>
    Private Sub FallbackToBalloonNotification(count As Integer)
        Try
            If notifyIcon Is Nothing Then
                notifyIcon = New NotifyIcon()
                notifyIcon.Icon = SystemIcons.Information
                notifyIcon.Text = "نظام الأرشيف - تنبيهات"
                notifyIcon.Visible = True
            End If

            notifyIcon.BalloonTipTitle = "تنبيه: سجلات غير مؤرشفة"
            notifyIcon.BalloonTipText = $"لديك {count} سجل غير مؤرشف."
            notifyIcon.BalloonTipIcon = ToolTipIcon.Warning
            notifyIcon.ShowBalloonTip(3000)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"فشل في عرض أي نوع من الإشعارات: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تهيئة NotifyIcon
    ''' </summary>
    Private Sub InitializeNotifyIcon()
        Try
            If notifyIcon Is Nothing Then
                notifyIcon = New NotifyIcon()
                notifyIcon.Icon = SystemIcons.Information
                notifyIcon.Text = "نظام الأرشيف - تنبيهات"
                notifyIcon.Visible = True

                ' إعداد القائمة السياقية
                Dim contextMenu As New ContextMenuStrip()
                Dim showItem As New ToolStripMenuItem("عرض السجلات غير المؤرشفة")
                AddHandler showItem.Click, AddressOf ShowUnarchivedRecords
                contextMenu.Items.Add(showItem)

                Dim exitItem As New ToolStripMenuItem("إغلاق")
                AddHandler exitItem.Click, Sub(sender As Object, e As EventArgs)
                                               notifyIcon.Visible = False
                                               notifyIcon.Dispose()
                                               Me.Close()
                                           End Sub
                contextMenu.Items.Add(exitItem)

                notifyIcon.ContextMenuStrip = contextMenu

                ' إعداد معالج الحدث للنقر
                AddHandler notifyIcon.MouseClick, AddressOf NotifyIcon_MouseClick
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تهيئة NotifyIcon: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' عرض نموذج السجلات غير المؤرشفة
    ''' </summary>
    Private Sub ShowUnarchivedRecords()
        Try
            Dim unarchivedForm As New UnarchivedRecordsForm()
            unarchivedForm.Show()
        Catch ex As Exception
            MessageBox.Show("خطأ في فتح نموذج السجلات غير المؤرشفة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Sub ShowUnarchivedNotification()
    '    Try
    '        Dim count As Integer = DataHelper.UnarchivedSaderCount

    '        ' إظهار إشعار إذا كان هناك سجلات غير مؤرشفة
    '        If count > 0 Then
    '            ' التحقق من أن الإشعار ليس مفتوحًا بالفعل
    '            If _notificationsForm Is Nothing OrElse _notificationsForm.IsDisposed Then
    '                _notificationsForm = New NotificationsForm(NotificationsForm.NotificationType.Balloon)
    '                _notificationsForm.ShowUnarchivedRecords()
    '            Else
    '                ' إذا كان الإشعار مفتوحًا بالفعل، قم بتحديثه فقط
    '                _notificationsForm.ShowUnarchivedRecords()
    '            End If
    '        Else
    '            ' إخفاء الإشعار إذا لم تكن هناك سجلات غير مؤرشفة
    '            If _notificationsForm IsNot Nothing AndAlso Not _notificationsForm.IsDisposed Then
    '                _notificationsForm.Close()
    '                _notificationsForm = Nothing
    '            End If
    '        End If
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في عرض إشعار السجلات غير المؤرشفة: {ex.Message}")
    '    End Try
    'End Sub

    ''' <summary>
    ''' معالجة إغلاق نموذج الإشعارات
    ''' </summary>
    Private Sub NotificationsForm_FormClosed(sender As Object, e As FormClosedEventArgs)
        Try
            ' تفريغ المتغير عند إغلاق النموذج
            _notificationsForm = Nothing
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالجة إغلاق نموذج الإشعارات: {ex.Message}")
        End Try
    End Sub

    ' دالة لفتح نموذج تفاصيل السجلات غير المؤرشفة
    Private Sub ShowUnarchivedRecordsDetails()
        Try
            Dim detailsForm As New UnarchivedRecordsForm()
            detailsForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("خطأ في عرض تفاصيل السجلات غير المؤرشفة: " & ex.Message,
                       "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' في ملف MainFrm.vb
    Private notifyIcon As New NotifyIcon()

    Private Sub InitializeTaskbarNotification()
        Try
            ' إعداد NotifyIcon
            notifyIcon.Icon = SystemIcons.Information
            notifyIcon.Text = "نظام الأرشيف - سجلات غير مؤرشفة"
            notifyIcon.Visible = True

            ' إعداد القائمة السياقية
            Dim contextMenu As New ContextMenuStrip()
            Dim menuItem As New ToolStripMenuItem("عرض السجلات غير المؤرشفة")
            AddHandler menuItem.Click, AddressOf ShowUnarchivedRecordsDetails
            contextMenu.Items.Add(menuItem)

            Dim exitItem As New ToolStripMenuItem("إغلاق")
            AddHandler exitItem.Click, Sub(sender As Object, e As EventArgs)
                                           notifyIcon.Visible = False
                                           notifyIcon.Dispose()
                                       End Sub
            contextMenu.Items.Add(exitItem)

            notifyIcon.ContextMenuStrip = contextMenu

            ' إعداد معالج الحدث للنقر
            AddHandler notifyIcon.MouseClick, AddressOf NotifyIcon_MouseClick
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تهيئة إشعار شريط المهام: {ex.Message}")
        End Try
    End Sub

    Private Sub NotifyIcon_MouseClick(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            ShowUnarchivedRecordsDetails()
        End If
    End Sub

#End Region

#Region "إدارة العدادات والعدادات"
    ''' <summary>
    ''' تهيئة عداد السجلات غير المرتبطة بالأرشيف
    ''' </summary>
    Private Sub InitializeUnarchivedCounter()
        Try
            ' الاشتراك في الحدث
            AddHandler DataHelper.UnarchivedSaderCountChanged, AddressOf HandleUnarchivedCountChanged
            ' تحديث العداد في الواجهة
            UpdateUnarchivedSaderCountLabel()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تهيئة عداد السجلات غير المرتبطة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة تغيير عدد السجلات غير المؤرشفة
    ''' </summary>
    Private Sub HandleUnarchivedCountChanged(sender As Object, e As DataHelper.DataCountChangedEventArgs)
        ' تأكد أن التحديث للسجلات غير المؤرشفة
        If e.TableName = "UnarchivedSader" Then
            ' تحديث تسمية العدد
            UpdateUnarchivedSaderCountLabel(e.NewCount)

            ' عرض إشعار إذا كان هناك سجلات غير مؤرشفة
            If e.NewCount > 0 Then
                ShowUnarchivedNotification()
            Else
                ' إخفاء الإشعار إذا لم تكن هناك سجلات غير مؤرشفة
                If _notificationsForm IsNot Nothing AndAlso Not _notificationsForm.IsDisposed Then
                    _notificationsForm.Close()
                    _notificationsForm = Nothing
                End If
            End If
        End If
    End Sub


    '''' <summary>
    '''' تحديث تسمية عدد السجلات غير المؤرشفة
    '''' </summary>
    '''' <param name="newCount">العدد الجديد للسجلات غير المؤرشفة</param>
    'Private Sub UpdateUnarchivedCountLabel(newCount As Integer)
    '    Try
    '        ' التحقق من الحاجة إلى Invoke (إذا تم الاستدعاء من خيط غير رئيسي)
    '        If Me.InvokeRequired Then
    '            Me.Invoke(Sub()
    '                          ' تحديث النص
    '                          lblUnarchivedCount.Text = $"غير مؤرشف: {newCount}"

    '                          ' تغيير اللون والخط حسب العدد
    '                          If newCount > 0 Then
    '                              lblUnarchivedCount.ForeColor = Color.Red
    '                              lblUnarchivedCount.Font = New Font(lblUnarchivedCount.Font, FontStyle.Bold)
    '                          Else
    '                              lblUnarchivedCount.ForeColor = Color.Green
    '                              lblUnarchivedCount.Font = New Font(lblUnarchivedCount.Font, FontStyle.Regular)
    '                          End If
    '                      End Sub)
    '        Else
    '            ' تحديث النص مباشرة (إذا تم الاستدعاء من الخيط الرئيسي)
    '            lblUnarchivedCount.Text = $"غير مؤرشف: {newCount}"

    '            ' تغيير اللون والخط حسب العدد
    '            If newCount > 0 Then
    '                lblUnarchivedCount.ForeColor = Color.Red
    '                lblUnarchivedCount.Font = New Font(lblUnarchivedCount.Font, FontStyle.Bold)
    '            Else
    '                lblUnarchivedCount.ForeColor = Color.Green
    '                lblUnarchivedCount.Font = New Font(lblUnarchivedCount.Font, FontStyle.Regular)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تحديث تسمية عداد السجلات غير المؤرشفة: {ex.Message}")
    '    End Try
    'End Sub

    ''' <summary>
    ''' تحديث تسمية العدد غير المرتبط بالأرشيف
    ''' </summary>
    Private Sub UpdateUnarchivedSaderCountLabel(Optional newCount As Integer? = Nothing)
        Try
            Dim count As Integer = If(newCount.HasValue, newCount.Value, DataHelper.GetCurrentUnarchivedSaderCount())
            lblUnarchivedCount.Text = $"  عدد السجلات غير المرتبطة بالأرشيف:   {count}  "

            ' تغيير لون التسمية بناءً على العدد
            If count = 0 Then
                lblUnarchivedCount.ForeColor = Color.Green
                lblUnarchivedCount.Text &= "  (تم ربط جميع السجلات)  "
            Else
                lblUnarchivedCount.ForeColor = Color.Red
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث تسمية العدد غير المرتبط: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث العرض عند تغيير عدد السجلات الصادرة
    ''' </summary>
    Private Sub OnDataCountChanged(sender As Object, e As DataHelper.DataCountChangedEventArgs)
        Try
            ' تأكد أن التحديث لجدول الصادرة
            If e.TableName = "Sader" Then
                ' تحديث العلامة في الخلفية (لتجنب مشاكل الثريد)
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  lblRecordCount.Text = $"عــــدد السجلات الصادرة :   {e.NewCount}"
                                  lblRecordCount.Tag = e.NewCount
                              End Sub)
                Else
                    lblRecordCount.Text = $"عــــدد السجلات الصادرة :   {e.NewCount}"
                    lblRecordCount.Tag = e.NewCount
                End If
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عداد الصادرة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' عرض العدد الأولي للسجلات الصادرة
    ''' </summary>
    Private Sub UpdateSaderCountLabel()
        Try
            Dim count As Integer = DataHelper.GetCurrentSaderCount()
            lblRecordCount.Text = $"عــــدد السجلات الصادرة  :   {count}"
            lblRecordCount.Tag = count
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عداد الصادرة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث العرض عند تغيير عداد الأرشيف
    ''' </summary>
    Private Sub OnArchiveCountChanged(sender As Object, e As DataHelper.DataCountChangedEventArgs)
        Try
            ' تأكد أن التحديث لجدول الأرشيف
            If e.TableName = "Archive" Then
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  lblArchiveCount.Text = $"عدد السجلات في الأرشيف :   {e.NewCount}"
                              End Sub)
                Else
                    lblArchiveCount.Text = $"عدد السجلات في الأرشيف :   {e.NewCount}"
                End If
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عداد الأرشيف: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث العرض عند تغيير عداد الأرشيفات المحذوفة
    ''' </summary>
    Private Sub OnDeletedArchiveCountChanged(sender As Object, e As DataHelper.DataCountChangedEventArgs)
        Try
            ' تأكد أن التحديث لجدول الأرشيفات المحذوفة
            If e.TableName = "DeletedArchive" Then
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  lblDeletedArchiveCount.Text = $"عدد السجلات المحذوفة من الأرشيف: {e.NewCount}"
                              End Sub)
                Else
                    lblDeletedArchiveCount.Text = $"عدد السجلات المحذوفة من الأرشيف: {e.NewCount}"
                End If
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عداد الأرشيفات المحذوفة: {ex.Message}")
        End Try
    End Sub
    Private Sub UpdateDeletedArchiveCountLabel()
        Try
            Dim count As Integer = DataHelper.GetCurrentDeletedArchiveCount()
            lblDeletedArchiveCount.Text = $"عـدد السجلات المحذوفة من الأرشيف  :   {count}"
            lblDeletedArchiveCount.Tag = count
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عداد المحذوفة من الأرشيف: {ex.Message}")
        End Try
    End Sub
    ''' <summary>
    ''' عرض العدد الأولي للأرشيف
    ''' </summary>
    Private Sub UpdateArchiveCountLabel()
        Try
            Dim count As Integer = DataHelper.GetCurrentArchiveCount()
            lblArchiveCount.Text = $"عدد السجلات في الأرشيف :   {count}"
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عداد الأرشيف: {ex.Message}")
        End Try
    End Sub
#End Region

#Region "التحقق من الصلاحيات"
    ''' <summary>
    ''' التحقق إذا كان المستخدم مسؤولاً
    ''' </summary>
    Private Function CheckIfAdmin() As Boolean
        Try
            Dim query As String = "SELECT IsAdmin FROM TbUsers WHERE ID = @UserID"
            Dim params() As SqlParameter = {New SqlParameter("@UserID", GlobalData.LoggedInUserID)}

            Dim result As Object = DatabaseHelper.ExecuteScalar(query, params, False)
            If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then
                Return CBool(result)
            Else
                Return False
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من صلاحيات المسؤول: {ex.Message}")
            Return False
        End Try
    End Function
#End Region

#Region "الأحداث والتعامل مع النوافذ"
    ''' <summary>
    ''' التعامل مع تغيير حجم النموذج
    ''' </summary>
    Private Sub MainForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        Try
            ' التحقق من أن الكائن scaler مهيأ قبل استدعائه
            If scaler IsNot Nothing Then
                scaler.ScaleControls(Me)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تغيير حجم النموذج: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' إغلاق النموذج الرئيسي
    ''' </summary>
    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            ' إلغاء الاشتراك في الأحداث
            RemoveHandler DataHelper.SaderCountChanged, AddressOf OnDataCountChanged
            RemoveHandler DataHelper.UnarchivedSaderCountChanged, AddressOf HandleUnarchivedCountChanged
            RemoveHandler DataHelper.ArchiveCountChanged, AddressOf OnArchiveCountChanged
            ' تنظيف الموارد
            If PictureBoxUser.Image IsNot Nothing AndAlso PictureBoxUser.Image IsNot My.Resources.UserDefault Then
                PictureBoxUser.Image.Dispose()
            End If

            If e.CloseReason = CloseReason.UserClosing Then
                If MessageBox.Show("هل تريد الخروج من البرنامج؟", "تأكيد الخروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                    e.Cancel = True
                Else
                    timerClock.Stop()
                    Application.Exit()
                End If
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إغلاق النموذج: {ex.Message}")
        End Try
    End Sub

    '' <summary>
    '' التحديث الدوري للتاريخ والوقت
    '' </summary>
    Private Sub timerClock_Tick(sender As Object, e As EventArgs) Handles timerClock.Tick
        Try
            UpdateDateTime()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث الوقت: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث التاريخ والوقت
    ''' </summary>
    Private Sub UpdateDateTime()
        Try
            ' الحصول على التاريخ والوقت الحاليين
            Dim currentDate As Date = Date.Now

            ' تنسيق التاريخ باللغة العربية
            lblDate.Text = currentDate.ToString("yyyy / MM / dd ")

            ' تنسيق الوقت
            lblTime.Text = currentDate.ToString("hh:mm:ss tt")
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث التاريخ والوقت: {ex.Message}")
        End Try
    End Sub
#End Region

#Region "الأحداث الخاصة بالأزرار"
    ''' <summary>
    ''' فتح نموذج إدارة المستخدمين
    ''' </summary>
    Private Sub btnUsers_Click(sender As Object, e As EventArgs) Handles btnUsers.Click
        Try

            Dim frm As New UsersForm()
            frm.MdiParent = Me
            frm.Show()
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح نموذج المستخدمين: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' فتح نموذج الصادر
    ''' </summary>
    Private Sub btnSader_Click(sender As Object, e As EventArgs) Handles btnSader.Click, btnSader2.Click

        Try
            '' التحقق من صلاحية عرض نموذج المستخدمين
            'If Not CheckPermission("View", "عرض نموذج إدارة الصادر") Then
            '    Return
            'End If
            Dim frm As New SaderForm()
            frm.MdiParent = Me
            frm.Show()
            'OpenMdiForm(Of SaderForm)(Me)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح نموذج الصادر: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' فتح نموذج الأرشيف
    ''' </summary>
    Private Sub btnArchive_Click(sender As Object, e As EventArgs) Handles btnArchive.Click, btnArchive2.Click
        Try
            '' التحقق من صلاحية عرض نموذج المستخدمين
            'If Not CheckPermission("View", "عرض نموذج إدارة الأرشيف") Then
            '    Return
            'End If
            OpenMdiForm(Of ArchiveForm)(Me)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح نموذج الأرشيف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    ''' <summary>
    ''' فتح نموذج البحث
    ''' </summary>
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try

            Dim frm As New CustomSearch()
            frm.Tag = 1
            frm.WindowState = FormWindowState.Maximized
            frm.MdiParent = Me
            frm.Show()
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح نموذج البحث: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' فتح نموذج الصلاحيات
    ''' </summary>
    Private Sub btnPermissions_Click(sender As Object, e As EventArgs) Handles btnPermissions.Click
        Try
            '' التحقق من صلاحية عرض نموذج المستخدمين
            'If Not CheckPermission("View", "عرض نموذج صلاحيات المستخدمين") Then
            '    Return
            'End If
            OpenMdiForm(Of PermissionsForm)(Me)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح نموذج الصلاحيات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' فتح نموذج الإعدادات
    ''' </summary>
    Private Sub btnSetting_Click(sender As Object, e As EventArgs) Handles btnSetting.Click
        Try
            OpenMdiForm(Of SettingsForm)(Me)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح نموذج الإعدادات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' فتح نموذج النسخ الاحتياطي
    ''' </summary>
    Private Sub btnBackup_Click(sender As Object, e As EventArgs) Handles btnBackup.Click
        Try
            '' التحقق من صلاحية عرض نموذج المستخدمين
            'If Not CheckPermission("View", "عرض نموذج النسخ الإحتياطي") Then
            '    Return
            'End If
            Dim frm As New BackupForm
            frm.Show(Me)
            'OpenMdiForm(Of BackupForm)(Function() New BackupForm(), Me)
            'OpenMdiForm(Of BackupForm)(Me)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح نموذج النسخ الاحتياطي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' فتح نموذج استعادة النسخة الاحتياطية
    ''' </summary>
    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        Try
            '' التحقق من صلاحية عرض نموذج المستخدمين
            'If Not CheckPermission("View", "عرض نموذج استعادة النسخ الاحتياطي") Then
            '    Return
            'End If
            Dim frm As New RestoreForm

            frm.Show(Me)
            'OpenMdiForm(Of RestoreForm)(Me)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح نموذج الاستعادة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' فتح نموذج إدارة النسخ الاحتياطية
    ''' </summary>
    Private Sub btnBackupManager_Click(sender As Object, e As EventArgs) Handles btnBackupManager.Click
        Try
            '' التحقق من صلاحية عرض نموذج المستخدمين
            'If Not CheckPermission("View", "عرض نموذج إدارة النسخ الإحتياطية") Then
            '    Return
            'End If
            Dim frm As New BackupManagerForm

            frm.Show(Me)
            'OpenMdiForm(Of BackupManagerForm)(Me)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح نموذج إدارة النسخ الاحتياطية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' زر الخروج من النظام
    ''' </summary>
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Try
            ConfirmExit()
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء محاولة الخروج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' زر تسجيل الخروج
    ''' </summary>
    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click, btnLogout2.Click
        Try
            If MessageBox.Show("هل تريد تسجيل الخروج؟", "تأكيد تسجيل الخروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim loginForm As New LoginForm()
                loginForm.Show()
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء تسجيل الخروج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' زر الوارد (قيد التطوير)
    ''' </summary>
    Private Sub btnWared_Click(sender As Object, e As EventArgs) Handles btnWared.Click
        Try
            MessageBox.Show("هذه الميزة قيد التطوير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    ''' <summary>
    ''' زر سلة المحذفات (قيد التطوير)
    ''' </summary>
    Private Sub btnDeletedForm_Click(sender As Object, e As EventArgs) Handles btnDeletedForm1.Click

    End Sub

    Private Sub btnDeletedForm2_Click(sender As Object, e As EventArgs) Handles btnDeletedForm2.Click
        Try
            '' التحقق من صلاحية عرض نموذج المستخدمين
            'If Not CheckPermission("View", "عرض نموذج سلة المحذوفات") Then
            '    Return
            'End If
            OpenMdiForm(Of DeletedArchiveForm)(Me)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح نموذج إدارة النسخ الاحتياطية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
#End Region

#Region "التعامل مع تسمية العدد غير المرتبط"
    ''' <summary>
    ''' فتح نموذج الصادر عند النقر على التسمية
    ''' </summary>
    Private Sub lblUnarchivedCount_Click(sender As Object, e As EventArgs) Handles lblUnarchivedCount.Click
        Try
            OpenMdiForm(Of SaderForm)(Me)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح نموذج الصادر: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تغيير لون التسمية عند المرور فوقها
    ''' </summary>
    Private Sub lblUnarchivedCount_MouseEnter(sender As Object, e As EventArgs) Handles lblUnarchivedCount.MouseEnter
        Try
            Me.lblUnarchivedCount.ForeColor = Color.Blue
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حدث MouseEnter: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' استعادة لون التسمية عند مغادرة المؤشر بناءً على تساوي القيم
    ''' </summary>
    Private Sub lblUnarchivedCount_MouseLeave(sender As Object, e As EventArgs) Handles lblUnarchivedCount.MouseLeave
        Try
            ' الحصول على القيم العددية من التسميات
            Dim recordCount As Integer = 0
            Dim archiveCount As Integer = 0

            ' استخراج الأعداد من نص التسميات
            Dim recordText As String = lblRecordCount.Text
            Dim archiveText As String = lblArchiveCount.Text

            ' استخدام تعبيرات منتظمة لاستخراج الأعداد من النص
            Dim recordMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(recordText, "\d+")
            Dim archiveMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(archiveText, "\d+")

            ' تحويل النتائج إلى أعداد
            If recordMatch.Success Then
                Integer.TryParse(recordMatch.Value, recordCount)
            End If

            If archiveMatch.Success Then
                Integer.TryParse(archiveMatch.Value, archiveCount)
            End If

            ' تغيير لون التسمية بناءً على تساوي القيم
            If recordCount = archiveCount AndAlso recordCount > 0 Then
                Me.lblUnarchivedCount.ForeColor = Color.Green
                Me.lblUnarchivedCount.Text = $"عدد السجلات غير المرتبطة بالأرشيف:   {DataHelper.GetCurrentUnarchivedSaderCount()}  "
            ElseIf recordCount = 0 AndAlso archiveCount = 0 Then
                Me.lblUnarchivedCount.ForeColor = Color.Green
                Me.lblUnarchivedCount.Text = "  عدد السجلات غير المرتبطة بالأرشيف:   0"
            Else
                Me.lblUnarchivedCount.ForeColor = Color.Red
                Me.lblUnarchivedCount.Text = $"عدد السجلات غير المرتبطة بالأرشيف:   {DataHelper.GetCurrentUnarchivedSaderCount()}  "
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حدث MouseLeave: {ex.Message}")
            ' في حالة الخطأ، استعادة اللون الأحمر كقيمة افتراضية
            Me.lblUnarchivedCount.ForeColor = Color.Red
        End Try
    End Sub

    '''' <summary>
    '''' استعادة لون التسمية عند مغادرة المؤشر
    '''' </summary>
    'Private Sub lblUnarchivedCount_MouseLeave(sender As Object, e As EventArgs) Handles lblUnarchivedCount.MouseLeave
    '    Try
    '        Dim count As Integer = If(newCount.HasValue, newCount.Value, DataHelper.GetCurrentUnarchivedSaderCount())
    '        If lblArchiveCount.Text = lblRecordCount.Text Then
    '            Me.lblUnarchivedCount.ForeColor = Color.Green
    '        Else
    '            Me.lblUnarchivedCount.ForeColor = Color.Red
    '        End If

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في حدث MouseLeave: {ex.Message}")
    '    End Try
    'End Sub
#End Region

#Region "الوظائف المساعدة"
    ''' <summary>
    ''' تأكيد الخروج من البرنامج
    ''' </summary>
    Private Sub ConfirmExit()
        Try
            If MessageBox.Show("هل تريد الخروج من البرنامج؟", "تأكيد الخروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Application.Exit()
            End If
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء تأكيد الخروج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تحديث بيانات النموذج الرئيسي
    ''' </summary>
    Public Sub RefreshData()
        Try
            ' تحديث عدادات السجلات
            DataHelper.UpdateSaderCount()
            DataHelper.UpdateUnarchivedSaderCount()
            DataHelper.UpdateArchiveCount()

            ' تحديث واجهة المستخدم
            UpdateRecordCounts()

            ' عرض اسم المستخدم في شريط الحالة
            lblUser.Text = GlobalData.LoggedInUserName
            lblUser2.Text = GlobalData.LoggedInUserName

            ' التحقق إذا كان المستخدم مسؤولاً
            Dim isAdmin As Boolean = CheckIfAdmin()
            If isAdmin Then
                lblUser.Text &= "  (مسؤول النظام)"
            End If
            ' تحميل صورة المستخدم
            RefreshUserImage()


            ' إعادة تحميل البيانات إذا لزم الأمر
            ' LoadData() ' إذا كنت تريد إعادة تحميل جميع البيانات

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في RefreshData: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحديث عدادات السجلات في واجهة المستخدم
    ''' </summary>
    Private Sub UpdateRecordCounts()
        Try
            ' تحديث عداد الصادرة
            lblRecordCount.Text = $"عدد الصادرة: {DataHelper.GetCurrentSaderCount()}"

            ' تحديث عداد الصادرة غير المرتبطة بالأرشيف
            lblUnarchivedCount.Text = $"الصادرة غير المرتبطة: {DataHelper.GetCurrentUnarchivedSaderCount()}"

            ' تحديث عداد الأرشيف
            lblArchiveCount.Text = $"عدد الأرشيف: {DataHelper.GetCurrentArchiveCount()}"

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في UpdateRecordCounts: {ex.Message}")
        End Try
    End Sub

    Private Sub menuUnarchivedRecords_Click(sender As Object, e As EventArgs) Handles menuUnarchivedRecords.Click
        ShowUnarchivedRecordsDetails()
    End Sub

    Private Sub BtnRefreshUnarchived_Click(sender As Object, e As EventArgs) Handles BtnRefreshUnarchived.Click
        ' تحديث العدد يدويًا
        DataHelper.UpdateUnarchivedSaderCount()

        '' تحديث التسمية
        'UpdateUnarchivedCountLabel(DataHelper.UnarchivedSaderCount)
    End Sub

    Private Sub BtnDateDeff_Click(sender As Object, e As EventArgs) Handles BtnDateDeff.Click
        Try
            OpenMdiForm(Of DateDifferenceForm)(Me)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح نموذج حساب بين تاريخين: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnReportCriteriaForm_Click(sender As Object, e As EventArgs) Handles BtnReportCriteriaForm.Click
        'OpenMdiForm(Of ReportCriteriaForm)()
    End Sub


    Private Sub BtnReportSelectionForm_Click(sender As Object, e As EventArgs) Handles BtnReportSelectionForm.Click
        Try
            ' فتح نموذج اختيار التقرير
            Dim reportSelectionForm As New ReportSelectionForm()
            AddHandler reportSelectionForm.ReportSelected, AddressOf OnReportSelected
            AddHandler reportSelectionForm.CancelReportSelection, AddressOf OnReportSelectionCancelled
            reportSelectionForm.ShowDialog(Me)
        Catch ex As Exception
            MessageBox.Show($"خطأ في فتح نافذة التقارير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    '''' <summary>
    '''' تحميل صورة المستخدم
    '''' </summary>
    'Private Sub LoadUserImage()
    '    Try
    '        ' التحقق مما إذا كانت الصورة قد تم تحميلها مسبقًا
    '        If GlobalData.CurrentUserImage IsNot Nothing Then
    '            PictureBoxUser.Image = GlobalData.CurrentUserImage
    '        Else
    '            ' محاولة تحميل الصورة من قاعدة البيانات
    '            Dim usersClass As New UsersClass()
    '            Dim imageData As Byte() = usersClass.GetUserImage(GlobalData.LoggedInUserID)

    '            If imageData IsNot Nothing AndAlso imageData.Length > 0 Then
    '                Using ms As New MemoryStream(imageData)
    '                    PictureBoxUser.Image = Image.FromStream(ms)
    '                    ' حفظ الصورة في المتغير العالمي
    '                    GlobalData.CurrentUserImage = PictureBoxUser.Image
    '                End Using
    '            Else
    '                ' استخدام صورة افتراضية
    '                PictureBoxUser.Image = My.Resources.UserDefault
    '                GlobalData.CurrentUserImage = My.Resources.UserDefault
    '            End If
    '        End If

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تحميل صورة المستخدم: {ex.Message}")
    '        ' استخدام صورة افتراضية في حالة الخطأ
    '        PictureBoxUser.Image = My.Resources.UserDefault
    '        GlobalData.CurrentUserImage = My.Resources.UserDefault
    '    End Try
    'End Sub


    ''' <summary>
    ''' تحميل صورة المستخدم
    ''' </summary>
    Private Sub LoadUserImage()
        Try
            ' التحقق مما إذا كانت الصورة قد تم تحميلها مسبقًا
            If GlobalData.CurrentUserImage IsNot Nothing Then
                PictureBoxUser.Image = GlobalData.CurrentUserImage
            Else
                ' محاولة تحميل الصورة من قاعدة البيانات
                Dim usersClass As New UsersClass()
                Dim imageData As Byte() = usersClass.GetUserImage(GlobalData.LoggedInUserID)

                If imageData IsNot Nothing AndAlso imageData.Length > 0 Then
                    Try
                        Using ms As New MemoryStream(imageData)
                            PictureBoxUser.Image = Image.FromStream(ms)
                            ' حفظ الصورة في المتغير العالمي
                            GlobalData.CurrentUserImage = PictureBoxUser.Image
                        End Using
                    Catch ex As Exception
                        System.Diagnostics.Debug.WriteLine($"خطأ في فتح الصورة من البايت آري: {ex.Message}")
                        ' استخدام صورة افتراضية في حالة الخطأ
                        PictureBoxUser.Image = My.Resources.UserDefault
                        GlobalData.CurrentUserImage = My.Resources.UserDefault
                    End Try
                Else
                    ' استخدام صورة افتراضية
                    PictureBoxUser.Image = My.Resources.UserDefault
                    GlobalData.CurrentUserImage = My.Resources.UserDefault
                End If
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل صورة المستخدم: {ex.Message}")
            ' استخدام صورة افتراضية في حالة الخطأ
            PictureBoxUser.Image = My.Resources.UserDefault
            GlobalData.CurrentUserImage = My.Resources.UserDefault
        End Try
    End Sub


    '''' <summary>
    '''' تحميل صورة المستخدم
    '''' </summary>
    'Private Sub LoadUserImage()
    '    Try
    '        ' التحقق مما إذا كانت الصورة قد تم تحميلها مسبقًا
    '        If GlobalData.CurrentUserImage IsNot Nothing Then
    '            PictureBoxUser.Image = GlobalData.CurrentUserImage
    '        Else
    '            ' محاولة تحميل الصورة من قاعدة البيانات
    '            Dim usersClass As New UsersClass()
    '            Dim imageData As Byte() = usersClass.GetUserImage(GlobalData.LoggedInUserID)

    '            If imageData IsNot Nothing AndAlso imageData.Length > 0 Then
    '                Using ms As New MemoryStream(imageData)
    '                    PictureBoxUser.Image = Image.FromStream(ms)
    '                    ' حفظ الصورة في المتغير العالمي
    '                    GlobalData.CurrentUserImage = PictureBoxUser.Image
    '                End Using
    '            Else
    '                ' استخدام صورة افتراضية
    '                PictureBoxUser.Image = My.Resources.UserDefault
    '                GlobalData.CurrentUserImage = My.Resources.UserDefault
    '            End If
    '        End If

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تحميل صورة المستخدم: {ex.Message}")
    '        ' استخدام صورة افتراضية في حالة الخطأ
    '        PictureBoxUser.Image = My.Resources.UserDefault
    '        GlobalData.CurrentUserImage = My.Resources.UserDefault
    '    End Try
    'End Sub

    ''' <summary>
    ''' تحديث صورة المستخدم (عند الحاجة)
    ''' </summary>
    Public Sub RefreshUserImage()
        Try
            ' مسح الصورة الحالية من الذاكرة
            If PictureBoxUser.Image IsNot Nothing AndAlso PictureBoxUser.Image IsNot My.Resources.UserDefault Then
                PictureBoxUser.Image.Dispose()
            End If

            ' إعادة تحميل الصورة
            GlobalData.CurrentUserImage = Nothing
            LoadUserImage()

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث صورة المستخدم: {ex.Message}")
        End Try
    End Sub

    Private Sub ShowOverlayForm(dialogForm As Form)
        ' إنشاء نموذج Overlay (شاشة سوداء شفافة)
        Dim overlayForm As New Form With {
        .FormBorderStyle = FormBorderStyle.None,
        .WindowState = FormWindowState.Maximized,
        .BackColor = Color.Black,
        .Opacity = 0.5,
        .ShowInTaskbar = False
    }

        ' عرض النموذج Overlay
        overlayForm.Show()

        ' عرض النموذج المطلوب (مثل Frm_About) كـ Dialog
        dialogForm.ShowDialog()

        ' إغلاق النموذج Overlay بعد إغلاق النموذج المطلوب
        overlayForm.Close()
    End Sub

#End Region

#Region "التقارير"

    ' في تعريف المتغيرات في النموذج الرئيسي (MainForm.vb)
    Private _reportGenerator As New ReportGenerator()
    Private _reportType As ReportGenerator.ReportType = ReportGenerator.ReportType.OutgoingRecords

    '''' <summary>
    '''' معالجة تأكيد معايير التقرير
    '''' </summary>
    'Private Sub OnCriteriaConfirmed(criteria As ReportGenerator.ReportCriteria)
    '    Try
    '        ' تعيين معايير التقرير
    '        _reportGenerator.SetReportType(_reportType) ' تم تعديل هذا السطر ليكون صحيحاً
    '        _reportGenerator.SetReportCriteria(criteria)

    '        ' إنشاء التقرير
    '        Dim reportDocument As ReportDocument = _reportGenerator.GenerateReport()

    '        ' فتح عارض التقرير
    '        Dim reportViewerForm As New ReportViewerForm(reportDocument, _reportType, criteria)
    '        AddHandler reportViewerForm.ReportClosed, AddressOf OnReportClosed
    '        reportViewerForm.MdiParent = Me
    '        reportViewerForm.Show()
    '    Catch ex As Exception
    '        MessageBox.Show($"خطأ في إنشاء التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' معالجة اختيار نوع التقرير
    '''' </summary>
    'Private Sub OnReportSelected(reportType As ReportGenerator.ReportType)
    '    Try
    '        ' فتح نموذج معايير التقرير
    '        Dim reportCriteriaForm As New ReportCriteriaForm(reportType)
    '        AddHandler reportCriteriaForm.CriteriaConfirmed, AddressOf OnCriteriaConfirmed
    '        AddHandler reportCriteriaForm.CancelCriteria, AddressOf OnCriteriaCancelled
    '        reportCriteriaForm.ShowDialog(Me)
    '    Catch ex As Exception
    '        MessageBox.Show($"خطأ في اختيار معايير التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    ''' <summary>
    ''' معالجة اختيار نوع التقرير
    ''' </summary>
    Private Sub OnReportSelected(reportType As ReportGenerator.ReportType)
        Try
            ' حفظ نوع التقرير
            _reportType = reportType

            ' فتح نموذج معايير التقرير
            Dim reportCriteriaForm As New ReportCriteriaForm(reportType)
            AddHandler reportCriteriaForm.CriteriaConfirmed, AddressOf OnCriteriaConfirmed
            AddHandler reportCriteriaForm.CancelCriteria, AddressOf OnCriteriaCancelled
            reportCriteriaForm.ShowDialog(Me)
        Catch ex As Exception
            MessageBox.Show($"خطأ في اختيار معايير التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة تأكيد معايير التقرير
    ''' </summary>
    Private Sub OnCriteriaConfirmed(criteria As ReportGenerator.ReportCriteria)
        Try
            ' تعيين معايير التقرير
            _reportGenerator.SetReportType(_reportType)
            _reportGenerator.SetReportCriteria(criteria)

            ' إنشاء التقرير
            Dim reportDocument As ReportDocument = _reportGenerator.GenerateReport()

            ' فتح عارض التقرير
            Dim reportViewerForm As New ReportViewerForm(reportDocument, _reportType, criteria)
            AddHandler reportViewerForm.ReportClosed, AddressOf OnReportClosed
            reportViewerForm.MdiParent = Me
            reportViewerForm.Show()
        Catch ex As Exception
            MessageBox.Show($"خطأ في إنشاء التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة إغلاق التقرير
    ''' </summary>
    Private Sub OnReportClosed()
        Try
            ' يمكن إضافة أي معالجة إضافية هنا
            System.Diagnostics.Debug.WriteLine("تم إغلاق التقرير")
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالجة إغلاق التقرير: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة إلغاء اختيار التقرير
    ''' </summary>
    Private Sub OnReportSelectionCancelled()
        Try
            ' يمكن إضافة أي معالجة إضافية هنا
            System.Diagnostics.Debug.WriteLine("تم إلغاء اختيار التقرير")
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالجة إلغاء اختيار التقرير: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة إلغاء معايير التقرير
    ''' </summary>
    Private Sub OnCriteriaCancelled()
        Try
            ' يمكن إضافة أي معالجة إضافية هنا
            System.Diagnostics.Debug.WriteLine("تم إلغاء معايير التقرير")
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالجة إلغاء معايير التقرير: {ex.Message}")
        End Try
    End Sub



#End Region

#Region "النماذج الالكترونية"

    ''' <summary>
    ''' فتح نموذج إلكتروني في نافذة فرعية (MDI Child)
    ''' </summary>
    Private Sub OpenElectronicForm(url As String, title As String)
        Try
            ' التحقق من وجود نموذج مفتوح بالفعل بنفس العنوان
            For Each form As Form In Me.MdiChildren
                If TypeOf form Is WebBrowserForm AndAlso form.Text.Contains(title) Then
                    form.BringToFront()
                    Return
                End If
            Next

            ' إنشاء النموذج الجديد
            Dim webForm As New WebBrowserForm(url, title)
            webForm.Text = title
            webForm.MdiParent = Me
            webForm.WindowState = FormWindowState.Normal

            ' تحميل الرابط
            Try
                'ghoooo
                ' التأكد من تنسيق الرابط الصحيح
                Dim uri As New Uri(url)
                webForm.webBrowser.Navigate(uri)
                webForm.txtUrl.Text = url
            Catch ex As UriFormatException
                Try
                    ' محاولة إضافة http:// إذا كان الرابط غير مكتمل
                    Dim safeUrl As String = "http://" & url
                    Dim uri As New Uri(safeUrl)
                    webForm.webBrowser.Navigate(uri)
                    webForm.txtUrl.Text = safeUrl
                Catch ex2 As Exception
                    MessageBox.Show($"الرابط غير صحيح: {url}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End Try
            End Try

            webForm.Show()
        Catch ex As Exception
            MessageBox.Show($"خطأ في فتح النموذج الإلكتروني: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' فتح نموذج إجازة وإقرار عودة
    ''' </summary>
    Private Sub leaveFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles leaveFormToolStripMenuItem.Click
        OpenElectronicForm(
            "https://www.mod.gov.kw/resources/skins/EformImages/000000/230000-12.htm",
            "نموذج إجازة وإقرار عودة")
    End Sub

    ''' <summary>
    ''' فتح نموذج طلب إستقالة / تقاعد
    ''' </summary>
    Private Sub resignationFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles resignationFormToolStripMenuItem.Click
        OpenElectronicForm(
            "https://www.mod.gov.kw/resources/skins/EformImages/000000/231000-06.htm",
            "نموذج طلب إستقالة / تقاعد")
    End Sub

    ''' <summary>
    ''' فتح طلب ترشيح لوظيفة إشرافية
    ''' </summary>
    Private Sub promotionFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles promotionFormToolStripMenuItem.Click
        OpenElectronicForm(
            "https://www.mod.gov.kw/resources/skins/EformImages/000000/230000-13.htm",
            "طلب ترشيح لوظيفة إشرافية")
    End Sub

    ''' <summary>
    ''' فتح نموذج نقل وندب داخلي للهيئة الإدارية والمالية
    ''' </summary>
    Private Sub transferFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles transferFormToolStripMenuItem.Click
        OpenElectronicForm(
            "https://www.mod.gov.kw/resources/skins/EformImages/000000/231000-27.htm",
            "نموذج نقل وندب داخلي للهيئة الإدارية والمالية")
    End Sub

    ''' <summary>
    ''' فتح نموذج استمارة كشف حضور وانصراف
    ''' </summary>
    Private Sub attendanceFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles attendanceFormToolStripMenuItem.Click
        OpenElectronicForm(
            "https://www.mod.gov.kw/resources/skins/EformImages/000000/231000-40.htm",
            "نموذج استمارة كشف حضور وانصراف للموظفين العاملين بالمواقع الميدانية")
    End Sub

    ''' <summary>
    ''' فتح نموذج طلب معاملة
    ''' </summary>
    Private Sub requestFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles requestFormToolStripMenuItem.Click
        OpenElectronicForm(
            "https://www.mod.gov.kw/resources/skins/EformImages/000000/230000-37.htm",
            "نموذج طلب معاملة")
    End Sub

    ''' <summary>
    ''' فتح نموذج طلب هوية
    ''' </summary>
    Private Sub idRequestFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles idRequestFormToolStripMenuItem.Click
        OpenElectronicForm(
            "https://www.mod.gov.kw/resources/skins/EformImages/000000/231000-12.htm",
            "نموذج طلب هوية")
    End Sub

    ''' <summary>
    ''' فتح نموذج استمارة صرف بدلات
    ''' </summary>
    Private Sub allowanceFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles allowanceFormToolStripMenuItem.Click
        OpenElectronicForm(
            "https://www.mod.gov.kw/resources/skins/EformImages/000000/230000-43.htm",
            "نموذج استمارة صرف بدلات")
    End Sub

    ''' <summary>
    ''' فتح تصريح خروج
    ''' </summary>
    Private Sub exitPermitFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles exitPermitFormToolStripMenuItem.Click
        OpenElectronicForm(
            "https://www.mod.gov.kw/resources/skins/EformImages/000000/231000-08%20.htm",
            "تصريح خروج")
    End Sub

    ''' <summary>
    ''' فتح إخلاء طرف عهدة شخصية / تنظيمية
    ''' </summary>
    Private Sub clearanceFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles clearanceFormToolStripMenuItem.Click
        OpenElectronicForm(
            "https://www.mod.gov.kw/resources/skins/EformImages/000000/240000-23.htm",
            "إخلاء طرف عهدة شخصية / تنظيمية")
    End Sub

    ''' <summary>
    ''' فتح براءة ذمة لموظفي العقود الخاصة
    ''' </summary>
    Private Sub clearanceContractFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles clearanceContractFormToolStripMenuItem.Click
        OpenElectronicForm(
            "https://www.mod.gov.kw/resources/skins/EformImages/000000/232000-08.htm",
            "براءة ذمة لموظفي العقود الخاصة")
    End Sub

    ''' <summary>
    ''' فتح نموذج علاج موظف
    ''' </summary>
    Private Sub medicalFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles medicalFormToolStripMenuItem.Click
        OpenElectronicForm(
            "https://www.mod.gov.kw/resources/skins/EformImages/000000/230000-41.htm",
            "نموذج علاج موظف")
    End Sub

    ''' <summary>
    ''' فتح اشعار اجازة طارئة
    ''' </summary>
    Private Sub emergencyLeaveFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles emergencyLeaveFormToolStripMenuItem.Click
        OpenElectronicForm(
            "https://www.mod.gov.kw/resources/skins/EformImages/000000/231000-26.htm",
            "اشعار اجازة طارئة")
    End Sub

#End Region


#Region "الشبكة المحلية والانترنت"

    ' في ملف MainForm.vb

    Private _networkMonitor As NetworkMonitor = NetworkMonitor.Instance
    Private _isInitializing As Boolean = True ' لمنع التحديث أثناء التهيئة

    ''' <summary>
    ''' معالجة تغيير حالة الاتصال بالشبكة المحلية
    ''' </summary>
    Private Sub OnLocalNetworkStatusChanged(sender As Object, e As ConnectionStatusChangedEventArgs)
        ' تحديث واجهة المستخدم
        If Me.InvokeRequired Then
            Me.Invoke(Sub()
                          UpdateConnectionUI(e.IsConnected, e.ConnectionType)
                      End Sub)
        Else
            UpdateConnectionUI(e.IsConnected, e.ConnectionType)
        End If
    End Sub

    ''' <summary>
    ''' معالجة تغيير حالة الاتصال بالإنترنت
    ''' </summary>
    Private Sub OnInternetStatusChanged(sender As Object, e As ConnectionStatusChangedEventArgs)
        Try
            ' تحديث واجهة المستخدم فقط إذا لم يكن النظام في مرحلة التهيئة
            If Not _isInitializing Then
                If Me.InvokeRequired Then
                    Me.Invoke(Sub()
                                  UpdateInternetDependentButtons(e.IsConnected)
                              End Sub)
                Else
                    UpdateInternetDependentButtons(e.IsConnected)
                End If
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالجة تغيير حالة الإنترنت: {ex.Message}")
        End Try
    End Sub
    ''' <summary>
    ''' تحديث الأزرار المعتمدة على الإنترنت
    ''' </summary>
    Private Sub UpdateInternetDependentButtons(isConnected As Boolean)
        Try
            ' تعطيل/تفعيل الأزرار التي تتطلب اتصالًا بالإنترنت
            btnOpenWebForms.Enabled = isConnected
            'btnOnlineBackup.Enabled = isConnected
            'btnCloudRestore.Enabled = isConnected
            'btnOnlineUpdate.Enabled = isConnected
            'btnSendEmail.Enabled = isConnected

            ' يمكن إضافة أزرار أخرى هنا
            ' myInternetButton.Enabled = isConnected

            ' تحديث تسمية الحالة
            lblInternetStatus.Text = If(isConnected, "  الإنترنت: متصل  ", "  الإنترنت: غير متصل  ")
            lblInternetStatus.ForeColor = If(isConnected, Color.Green, Color.Red)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث الأزرار: {ex.Message}")
        End Try
    End Sub


    ''' <summary>
    ''' تحديث واجهة المستخدم بناءً على حالة الاتصال
    ''' </summary>
    Private Sub UpdateConnectionUI(isConnected As Boolean, connectionType As ConnectionType)
        Try
            ' تحديث الأيقونات والنصوص
            Select Case connectionType
                Case ConnectionType.LocalNetwork
                    lblLocalNetworkStatus.Text = If(isConnected, "  الشبكة: متصل  ", "  الشبكة: غير متصل  ")
                    lblLocalNetworkStatus.ForeColor = If(isConnected, Color.Green, Color.Red)

                Case ConnectionType.Internet
                    btnOpenWebForms.Enabled = True
                    lblInternetStatus.Text = If(isConnected, "  الإنترنت: متصل  ", "  الإنترنت: غير متصل  ")
                    lblInternetStatus.ForeColor = If(isConnected, Color.Green, Color.Red)
            End Select

            ' تحديث المعلومات الإضافية
            txtIPAddress.Text = _networkMonitor.LocalIPAddress
            txtHostName.Text = _networkMonitor.HostName
            txtInternetSpeed.Text = $"  {_networkMonitor.InternetSpeed} بايت/ثانية  "
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث واجهة المستخدم: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' التحقق من حالة الاتصال
    ''' </summary>
    Private Sub CheckConnectionStatus()
        Try
            _networkMonitor.CheckAllConnections()

            ' تحديث واجهة المستخدم بناءً على حالة الاتصال الحالية
            UpdateInternetDependentButtons(_networkMonitor.IsConnectedToInternet)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من حالة الاتصال: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' التحقق من اتصال الخادم
    ''' </summary>
    Private Sub CheckServerConnection()
        Try
            Dim serverName As String = My.Settings.ServerName ' أو اسم الخادم الخاص بك
            Dim isConnected As Boolean = _networkMonitor.CheckServerConnection(serverName)

            MessageBox.Show($"اتصال الخادم: {If(isConnected, "ناجح", "فشل")}", "حالة الاتصال", MessageBoxButtons.OK,
                       If(isConnected, MessageBoxIcon.Information, MessageBoxIcon.Error))
        Catch ex As Exception
            MessageBox.Show($"خطأ في التحقق من اتصال الخادم: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' الحصول على معلومات الاتصال الكاملة
    ''' </summary>
    Private Sub GetConnectionInfo()
        Dim info As ConnectionInfo = _networkMonitor.GetConnectionInfo()

        Dim message As String = $"معلومات الاتصال:{vbNewLine}" &
                               $"الشبكة المحلية: {If(info.IsConnectedToLocalNetwork, "متصل", "غير متصل")}{vbNewLine}" &
                               $"الإنترنت: {If(info.IsConnectedToInternet, "متصل", "غير متصل")}{vbNewLine}" &
                               $"عنوان IP: {info.LocalIPAddress}{vbNewLine}" &
                               $"اسم المضيف: {info.HostName}{vbNewLine}" &
                               $"سرعة الإنترنت: {info.InternetSpeed} بايت/ثانية{vbNewLine}" &
                               $"آخر تحقق (الشبكة): {info.LastLocalCheck}{vbNewLine}" &
                               $"آخر تحقق (الإنترنت): {info.LastInternetCheck}"

        MessageBox.Show(message, "معلومات الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>
    ''' تنظيف الموارد عند إغلاق النموذج
    ''' </summary>
    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        Try
            ' التوقف عن المراقبة
            _networkMonitor.Dispose()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تنظيف موارد مراقب الشبكة: {ex.Message}")
        Finally
            MyBase.OnFormClosed(e)
        End Try
    End Sub

    Private Sub btnNotification_Click(sender As Object, e As EventArgs) Handles btnNotification.Click
        ShowUnarchivedNotification()
    End Sub





#End Region


#Region "الصلاحيات"
    ''' <summary>
    ''' التحقق من صلاحية المستخدم والوصول إلى النموذج
    ''' </summary>
    ''' <param name="roleColumn">اسم عمود الصلاحية في الجدول</param>
    ''' <param name="formInstance">النموذج المراد فتحه</param>
    Public Sub CheckUserPermission(roleColumn As String, formInstance As Form)
        Try
            ' التحقق من صحة المدخلات
            If String.IsNullOrWhiteSpace(roleColumn) Then
                Throw New ArgumentException("اسم العمود غير صالح")
            End If

            If formInstance Is Nothing Then
                Throw New ArgumentNullException("النموذج لا يمكن أن يكون فارغًا")
            End If

            ' التحقق من تسجيل الدخول
            If String.IsNullOrEmpty(GlobalData.LoggedInUserName) Then
                MessageBox.Show("يرجى تسجيل الدخول أولاً", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' استخدام استعلامات معلمة لمنع هجمات SQL Injection
            Using connection As SqlConnection = DatabaseHelper.GetConnection()
                connection.Open()

                Dim query As String = "SELECT [" & roleColumn & "] FROM Users_Table WHERE UserName = @UserName"

                Using command As New SqlCommand(query, connection)
                    ' إضافة المعلمة لمنع SQL Injection
                    command.Parameters.AddWithValue("@UserName", GlobalData.LoggedInUserName)

                    Dim result As Object = command.ExecuteScalar()

                    If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then
                        Dim hasPermission As Boolean = Convert.ToBoolean(result)

                        If hasPermission Then
                            ' التحقق من أن النموذج ليس مفتوحًا بالفعل
                            If Not IsFormOpen(formInstance.GetType()) Then
                                formInstance.MdiParent = Me
                                formInstance.Show()

                                ' تفعيل النموذج الجديد
                                formInstance.BringToFront()
                            Else
                                ' إذا كان مفتوحًا، جرّبه إلى المقدمة
                                ActivateExistingForm(formInstance.GetType())
                            End If
                        Else
                            MessageBox.Show($"ليس لديك الصلاحية للوصول إلى {formInstance.Text}. الرجاء مراجعة مدير النظام.",
                                      "صلاحيات محدودة", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    Else
                        MessageBox.Show("لم يتم العثور على بيانات المستخدم أو الصلاحية غير معرفة.",
                                  "خطأ في التحقق", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End Using
            End Using

        Catch ex As SqlException When ex.Number = 2
            MessageBox.Show("فشل الاتصال بقاعدة البيانات. تأكد من تشغيل السيرفر.",
                      "اتصال قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As SqlException
            System.Diagnostics.Debug.WriteLine($"خطأ SQL في التحقق من الصلاحيات: {ex.Message}")
            MessageBox.Show("حدث خطأ أثناء التحقق من الصلاحيات. الرجاء المحاولة لاحقًا.",
                      "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As InvalidOperationException
            MessageBox.Show("فشل في تنفيذ العملية. تأكد من اتصال قاعدة البيانات.",
                      "عملية غير صالحة", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ غير متوقع في التحقق من الصلاحيات: {ex.Message}")
            MessageBox.Show("حدث خطأ غير متوقع. الرجاء الاتصال بمدير النظام.",
                      "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' التحقق مما إذا كان نموذج معين مفتوحًا بالفعل
    ''' </summary>
    Private Function IsFormOpen(formType As Type) As Boolean
        For Each openForm As Form In Me.MdiChildren
            If openForm.GetType() = formType Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' تفعيل نموذج موجود
    ''' </summary>
    Private Sub ActivateExistingForm(formType As Type)
        For Each openForm As Form In Me.MdiChildren
            If openForm.GetType() = formType Then
                openForm.BringToFront()
                Exit For
            End If
        Next
    End Sub
    ''' <summary>
    ''' معالجة الضغط على زر لوحة التحكم
    ''' </summary>
    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click


        Try
            '' التحقق من صلاحية الوصول إلى لوحة التحكم
            'If Not CheckPermission("ViewDashboard", "عرض لوحة التحكم") Then
            '    MessageBox.Show("ليس لديك صلاحية الوصول إلى لوحة التحكم. يرجى الاتصال بمدير النظام.",
            '                  "صلاحيات محدودة",
            '                  MessageBoxButtons.OK,
            '                  MessageBoxIcon.Information)
            '    Return
            'End If

            ' التحقق مما إذا كان النموذج مفتوحًا بالفعل
            For Each form As Form In Me.MdiChildren
                If TypeOf form Is DashboardForm Then
                    form.BringToFront()
                    Return
                End If
            Next

            ' إنشاء نموذج لوحة التحكم
            Dim dashboardForm As New DashboardForm()
            dashboardForm.MdiParent = Me
            dashboardForm.WindowState = FormWindowState.Normal
            dashboardForm.Show()

        Catch ex As Exception
            MessageBox.Show($"خطأ في فتح لوحة التحكم: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnAbout_Click(sender As Object, e As EventArgs) Handles btnAbout.Click
        ShowOverlayForm(AboutForm)
    End Sub

    Private Sub btnActivityLog_Click(sender As Object, e As EventArgs) Handles btnActivityLog.Click
        Try
            '' التحقق من صلاحية عرض نموذج المستخدمين
            'If Not CheckPermission("ViewActivityLog", "عرض سجلات الحركة") Then
            '    Return
            'End If
            OpenMdiForm(Of UserActivityLogForm)(Me)
        Catch ex As Exception
            MessageBox.Show($"خطأ في فتح سجلات الحركة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnServerConnection_Click(sender As Object, e As EventArgs) Handles btnServerConnection.Click
        Dim frm As New ServerConnectionForm()
        frm.MdiParent = Me
        frm.Show()
    End Sub

    Private Sub btnDeletedForm_Click_1(sender As Object, e As EventArgs) Handles btnDeletedForm.Click
        Try
            '' التحقق من صلاحية عرض نموذج المستخدمين
            'If Not CheckPermission("View", "عرض نموذج سلة المحذوفات") Then
            '    Return
            'End If
            OpenMdiForm(Of DeletedArchiveForm)(Me)
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح نموذج إدارة النسخ الاحتياطية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub mnuCleanupArchives_Click(sender As Object, e As EventArgs) Handles MnuCleanupArchives.Click
        Try
            ' عرض مربع حوار للتأكيد
            Dim result As DialogResult = MessageBox.Show(
                "هل أنت متأكد من رغبتك في حذف جميع الأرشيفات المحذوفة التي تجاوزت 30 يومًا؟",
                "تنظيف الأرشيف المحذوف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                ' تنفيذ عملية الحذف النهائي
                Dim deletedCount As Integer = archiveClass.PermanentlyDeleteArchives(30, True)

                If deletedCount > 0 Then
                    MessageBox.Show($"تم حذف {deletedCount} أرشيف محذوف نهائيًا.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("لم يتم العثور على أي أرشيف محذوف قديم للحذف.", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء التنظيف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub




    '''' <summary>
    '''' تطبيق الصلاحيات على النموذج الرئيسي
    '''' </summary>
    'Private Sub ApplyPermissions()
    '    Try
    '        ' إخفاء أو تعطيل الأزرار بناءً على الصلاحيات
    '        btnUsers.Enabled = CheckPermission("View", "عرض نموذج المستخدمين")
    '        btnPermissions.Enabled = CheckPermission("View", "عرض نموذج الصلاحيات")
    '        btnArchive.Enabled = CheckPermission("View", "عرض نموذج الأرشيف")
    '        btnSader.Enabled = CheckPermission("View", "عرض نموذج الصادر")
    '        btnWared.Enabled = CheckPermission("View", "عرض نموذج الوارد")
    '        btnSearch.Enabled = True ' البحث متاح للجميع

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق الصلاحيات: {ex.Message}")
    '    End Try
    'End Sub

#End Region


#Region "التفعيل"

    '''' <summary>
    '''' تحديث حالة الترخيص في واجهة المستخدم
    '''' </summary>
    'Public Sub RefreshLicenseStatus()
    '    Try
    '        ' تحديث شريط الحالة
    '        If LicenseManager.IsActivated() Then
    '            tsslLicenseStatus.Text = "البرنامج مفعل"
    '            tsslLicenseStatus.ForeColor = Color.Green
    '        Else
    '            Dim frm As New ActivationForm
    '            Dim daysLeft As Integer = frm.GetTrialDaysLeft()
    '            If daysLeft > 0 Then
    '                tsslLicenseStatus.Text = $"التجربة: {daysLeft} يوم متبقٍ"
    '                tsslLicenseStatus.ForeColor = Color.Orange
    '            Else
    '                tsslLicenseStatus.Text = "انتهت فترة التجربة"
    '                tsslLicenseStatus.ForeColor = Color.Red
    '            End If
    '        End If

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في تحديث حالة الترخيص: {ex.Message}")
    '    End Try
    'End Sub



#End Region


End Class



'Imports System.Data.SqlClient
'Imports System.Drawing
'Public Class MainForm
'    Private scaler As ControlScaler

'    ''' <summary>
'    ''' هذه الدالة تقوم بتعيين لون خلفية النافذة الرئيسية وتوسيعها إلى الحجم الأقصى.
'    ''' </summary>
'    ''' 
'    Public xx As Short = 0


'    Public Sub New()
'        ' هذا السطر مطلوب لتهيئة النموذج.
'        InitializeComponent()

'        ' تهيئة الكلاس مع النموذج الحالي
'        scaler = New ControlScaler(Me)

'    End Sub

'    ' دالة لتهيئة عداد السجلات غير المرتبطة
'    Private Sub InitializeUnarchivedCounter()
'        ' الاشتراك في الحدث
'        AddHandler DataHelper.UnarchivedSaderCountChanged, AddressOf HandleUnarchivedCountChanged

'        ' تحديث العداد في الواجهة
'        UpdateUnarchivedSaderCountLabel()
'    End Sub

'    ' معالج الحدث لتغيير العدد غير المرتبط
'    Private Sub HandleUnarchivedCountChanged(sender As Object, e As DataHelper.DataCountChangedEventArgs)
'        ' التحقق من أن الحدث يتعلق بعدد السجلات غير المرتبطة
'        If e.TableName = "UnarchivedSader" Then
'            ' تحديث العلامة في الخلفية (لتجنب مشاكل الثريد)
'            If Me.InvokeRequired Then
'                Me.Invoke(Sub()
'                              UpdateUnarchivedSaderCountLabel(e.NewCount)
'                          End Sub)
'            Else
'                UpdateUnarchivedSaderCountLabel(e.NewCount)
'            End If
'        End If
'    End Sub

'    ' دالة لتحديث تسمية العدد غير المرتبط
'    Private Sub UpdateUnarchivedSaderCountLabel(Optional newCount As Integer? = Nothing)
'        Dim count As Integer = If(newCount.HasValue, newCount.Value, DataHelper.GetCurrentUnarchivedSaderCount())
'        lblUnarchivedCount.Text = $"عدد السجلات غير المرتبطة بالأرشيف: {count}"

'        ' تغيير لون التسمية بناءً على العدد
'        If count = 0 Then
'            lblUnarchivedCount.ForeColor = Color.Green
'            lblUnarchivedCount.Text &= " (تم ربط جميع السجلات)"
'        Else
'            lblUnarchivedCount.ForeColor = Color.Red
'        End If
'    End Sub

'    Private Sub SetBackColor()
'        Me.IsMdiContainer = True
'        Dim Child As Control
'        For Each Child In Me.Controls
'            If TypeOf Child Is MdiClient Then
'                Child.BackColor = Me.BackColor
'                Exit For
'            End If
'        Next
'        Me.WindowState = FormWindowState.Maximized
'    End Sub

'    Private Function CheckIfAdmin() As Boolean
'        Dim query As String = "SELECT IsAdmin FROM TbUsers WHERE ID = @UserID"
'        Dim params() As SqlParameter = {New SqlParameter("@UserID", GlobalData.LoggedInUserID)}

'        Dim result As Object = DatabaseHelper.ExecuteScalar(query, params, False)
'        If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then
'            Return CBool(result)
'        Else
'            Return False
'        End If
'    End Function

'    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        Me.Text = "الواجهة الرئيسية - نظام ESIS"
'        Me.RightToLeft = RightToLeft.Yes
'        Me.RightToLeftLayout = True
'        SetBackColor()

'        ' عرض اسم المستخدم في شريط الحالة
'        lblUser.Text = GlobalData.LoggedInUserName

'        ' التحقق إذا كان المستخدم مسؤولاً
'        Dim isAdmin As Boolean = CheckIfAdmin()
'        If isAdmin Then
'            lblUser.Text &= " (مسؤول النظام)"
'        End If


'        timerClock.Start()

'        ' تحديث البيانات فور التحميل
'        UpdateDateTime()

'        '' اعرض العدد عند التحميل
'        'UpdateSaderCountLabel()

'        ' الاشتراك في حدث تغيير العدد للصادرة
'        AddHandler DataHelper.SaderCountChanged, AddressOf OnDataCountChanged

'        ' إذا كنت بحاجة لعرض عداد الأرشيف أيضًا
'        AddHandler DataHelper.ArchiveCountChanged, AddressOf OnArchiveCountChanged

'        ' تحديث العداد في الواجهة
'        UpdateSaderCountLabel()
'        UpdateArchiveCountLabel() ' إذا كان مطلوبًا

'        InitializeUnarchivedCounter()

'    End Sub

'    ' 3. دالة لتحديث العرض عند تغيير العدد
'    Private Sub OnDataCountChanged(sender As Object, e As DataHelper.DataCountChangedEventArgs)
'        ' تأكد أن التحديث لجدول الصادرة
'        If e.TableName = "Sader" Then
'            ' تحديث العلامة في الخلفية (لتجنب مشاكل الثريد)
'            If Me.InvokeRequired Then
'                Me.Invoke(Sub()
'                              lblRecordCount.Text = $"عــــدد السجلات الصادرة :   {e.NewCount}"
'                              lblRecordCount.Tag = e.NewCount ' تخزين القيمة للرجوع إليها
'                          End Sub)
'            Else
'                lblRecordCount.Text = $"عــــدد السجلات الصادرة :   {e.NewCount}"
'                lblRecordCount.Tag = e.NewCount
'            End If
'        End If
'    End Sub

'    ' 4. دالة لعرض العدد الأولي
'    Private Sub UpdateSaderCountLabel()
'        Dim count As Integer = DataHelper.GetCurrentSaderCount()
'        lblRecordCount.Text = $"عــــدد السجلات الصادرة  :   {count}"
'        lblRecordCount.Tag = count
'    End Sub

'    ' 5. دالة لتحديث العرض عند تغيير عداد الأرشيف (إذا كان مطلوبًا)
'    Private Sub OnArchiveCountChanged(sender As Object, e As DataHelper.DataCountChangedEventArgs)
'        ' تأكد أن التحديث لجدول الأرشيف
'        If e.TableName = "Archive" Then
'            ' يمكنك استخدام هذا العداد إذا كان موجودًا في النموذج
'            If Me.InvokeRequired Then
'                Me.Invoke(Sub()
'                              'افترض أن لديك Label آخر للأرشيف
'                              lblArchiveCount.Text = $"عدد السجلات في الأرشيف :   {e.NewCount}"
'                          End Sub)
'            Else
'                lblArchiveCount.Text = $"عدد السجلات في الأرشيف :   {e.NewCount}"
'            End If
'        End If
'    End Sub

'    ' 6. دالة لعرض العدد الأولي للأرشيف (إذا كان مطلوبًا)
'    Private Sub UpdateArchiveCountLabel()
'        Dim count As Integer = DataHelper.GetCurrentArchiveCount()
'        ' افترض أن لديك Label آخر للأرشيف
'        lblArchiveCount.Text = $"عدد السجلات في الأرشيف :   {count}"
'    End Sub




'    '' 3. دالة لتحديث العرض عند تغيير العدد
'    'Private Sub OnSaderCountChanged(sender As Object, e As DataHelper.SaderCountChangedEventArgs)
'    '    ' تحديث العلامة في الخلفية (لتجنب مشاكل الثريد)
'    '    Me.Invoke(Sub()
'    '                  lblRecordCount.Text = $"عدد السجلات: {e.NewCount}"
'    '                  lblRecordCount.Tag = e.NewCount ' تخزين القيمة للرجوع إليها
'    '              End Sub)
'    'End Sub

'    '' 4. دالة لعرض العدد الأولي
'    'Private Sub UpdateSaderCountLabel()
'    '    Dim count As Integer = DataHelper.GetCurrentCount()
'    '    lblRecordCount.Text = $"عدد السجلات: {count}"
'    '    lblRecordCount.Tag = count
'    'End Sub


'    Private Sub btnUsers_Click(sender As Object, e As EventArgs) Handles btnUsers.Click
'        Dim frm As New UsersForm()
'        frm.MdiParent = Me
'        frm.Show()
'    End Sub

'    Private Sub btnSader_Click(sender As Object, e As EventArgs) Handles btnSader.Click
'        'Dim frm As New SaderForm()
'        'If frm IsNot Nothing Then
'        '    frm.MdiParent = Me
'        '    frm.Show()
'        'End If
'        ' فتح النموذج كـ MDI Child فقط
'        OpenMdiForm(Of SaderForm)(Me)

'    End Sub

'    Private Sub btnWared_Click(sender As Object, e As EventArgs) Handles btnWared.Click

'    End Sub

'    Private Sub btnArchive_Click(sender As Object, e As EventArgs) Handles btnArchive.Click
'        'Dim frm As New ArchiveForm()
'        'frm.MdiParent = Me
'        'frm.Show()
'        ' فتح النموذج كـ MDI Child فقط
'        OpenMdiForm(Of ArchiveForm)(Me)

'    End Sub
'    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
'        ConfirmExit()
'    End Sub

'    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
'        If MessageBox.Show("هل تريد تسجيل الخروج؟", "تأكيد تسجيل الخروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
'            Dim loginForm As New LoginForm()
'            loginForm.Show()
'            Me.Close()
'        End If
'    End Sub

'    Private Sub ConfirmExit()
'        If MessageBox.Show("هل تريد الخروج من البرنامج؟", "تأكيد الخروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
'            Application.Exit()
'        End If
'    End Sub

'    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
'        ' إلغاء الاشتراك في الحدث
'        RemoveHandler DataHelper.SaderCountChanged, AddressOf OnDataCountChanged

'        ' إلغاء الاشتراك في الحدث
'        RemoveHandler DataHelper.UnarchivedSaderCountChanged, AddressOf HandleUnarchivedCountChanged

'        ' إذا كنت قد اشتركت في حدث الأرشيف
'        RemoveHandler DataHelper.ArchiveCountChanged, AddressOf OnArchiveCountChanged

'        If e.CloseReason = CloseReason.UserClosing Then
'            If MessageBox.Show("هل تريد الخروج من البرنامج؟", "تأكيد الخروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
'                e.Cancel = True
'            Else
'                timerClock.Stop()
'                Application.Exit()  ' إنهاء التطبيق بالكامل
'            End If
'        End If
'    End Sub

'    Private Sub btnPermissions_Click(sender As Object, e As EventArgs) Handles btnPermissions.Click
'        'Dim frm As New PermissionsForm()
'        ''frm.MdiParent = Me
'        'frm.Show()
'        ' فتح النموذج كـ MDI Child فقط
'        OpenMdiForm(Of PermissionsForm)(Me)
'    End Sub

'    Private Sub btnSetting_Click(sender As Object, e As EventArgs) Handles btnSetting.Click
'        'Dim frm As New SettingsForm()
'        'frm.MdiParent = Me
'        'frm.Show()
'        ' فتح النموذج كـ MDI Child فقط
'        OpenMdiForm(Of SettingsForm)(Me)
'    End Sub

'    Private Sub MainForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
'        ' التحقق من أن الكائن scaler مهيأ قبل استدعائه
'        If scaler IsNot Nothing Then
'            scaler.ScaleControls(Me)
'        End If
'    End Sub

'    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

'        Dim frm As New CustomSearch()
'        frm.Tag = 1
'        frm.WindowState = FormWindowState.Maximized
'        frm.MdiParent = Me
'        frm.Show()


'    End Sub
'    Private Sub UpdateDateTime()
'        ' الحصول على التاريخ والوقت الحاليين
'        Dim currentDate As Date = Date.Now

'        ' تنسيق التاريخ باللغة العربية
'        lblDate.Text = currentDate.ToString("yyyy / MM / dd ")

'        ' تنسيق الوقت
'        lblTime.Text = currentDate.ToString("hh:mm:ss tt")
'    End Sub

'    Private Sub timerClock_Tick(sender As Object, e As EventArgs) Handles timerClock.Tick
'        ' تحديث التاريخ والوقت كل ثانية
'        UpdateDateTime()
'    End Sub

'    Private Sub btnBackup_Click(sender As Object, e As EventArgs) Handles btnBackup.Click
'        ' فتح النموذج كـ MDI Child فقط
'        OpenMdiForm(Of BackupForm)(Me)
'        'Dim frm As New BackupForm()
'        'frm.MdiParent = Me
'        'frm.Show()
'    End Sub

'    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
'        'Dim frm As New RestoreForm()
'        ''frm.MdiParent = Me
'        'frm.Show()
'        ' فتح النموذج كـ MDI Child فقط
'        OpenMdiForm(Of RestoreForm)(Me)
'    End Sub

'    Private Sub btnBackupManager_Click(sender As Object, e As EventArgs) Handles btnBackupManager.Click
'        'Dim frm As New BackupManagerForm()
'        ''frm.MdiParent = Me
'        'frm.Show()
'        ' فتح النموذج كـ MDI Child فقط
'        OpenMdiForm(Of BackupManagerForm)(Me)
'    End Sub

'    Private Sub lblUnarchivedCount_Click(sender As Object, e As EventArgs) Handles lblUnarchivedCount.Click
'        OpenMdiForm(Of SaderForm)(Me)
'    End Sub

'    Private Sub lblUnarchivedCount_MouseEnter(sender As Object, e As EventArgs) Handles lblUnarchivedCount.MouseEnter

'        Me.lblUnarchivedCount.ForeColor = Color.Blue
'    End Sub

'    Private Sub lblUnarchivedCount_MouseLeave(sender As Object, e As EventArgs) Handles lblUnarchivedCount.MouseLeave
'        Me.lblUnarchivedCount.ForeColor = Color.Red
'    End Sub
'End Class