
Public Class NotificationsForm : Inherits System.Windows.Forms.Form
        Private _archiveClass As New ArchiveClass()
        Private _notificationType As NotificationType = NotificationType.Balloon
        Private _isDragging As Boolean = False
        Private _dragCursorPoint As Point
        Private _dragFormPoint As Point

        ' أنواع الإشعارات
        Public Enum NotificationType
            Balloon ' إشعار بجوار الساعة
            Popup ' نافذة منبثقة
            Taskbar ' إشعار في شريط المهام
        End Enum

        Public Sub New(notificationType As NotificationType)
            InitializeComponent()
            _notificationType = notificationType
            SetupUI()
        End Sub

        Private Sub SetupUI()
            ' إعداد الواجهة حسب نوع الإشعار
            Select Case _notificationType
                Case NotificationType.Balloon
                    Me.FormBorderStyle = FormBorderStyle.None
                    Me.BackColor = Color.White
                    Me.TransparencyKey = Color.White
                    pnlHeader.BackColor = Color.FromArgb(41, 128, 185)
                    lblTitle.Text = "سجلات غير مؤرشفة"
                    lblTitle.ForeColor = Color.White
                    lblMessage.ForeColor = Color.Black
                    Me.Size = New Size(300, 150)

                Case NotificationType.Popup
                    Me.FormBorderStyle = FormBorderStyle.FixedSingle
                    Me.Size = New Size(400, 300)
                    pnlHeader.BackColor = Color.FromArgb(41, 128, 185)
                    lblTitle.Text = "سجلات غير مؤرشفة"
                    lblTitle.ForeColor = Color.White
                    lblMessage.ForeColor = Color.Black
                    btnArchiveAll.Visible = True
                    btnClose.Visible = True

                Case NotificationType.Taskbar
                    Me.FormBorderStyle = FormBorderStyle.None
                    Me.BackColor = Color.White
                    Me.TransparencyKey = Color.White
                    pnlHeader.BackColor = Color.FromArgb(231, 76, 60)
                    lblTitle.Text = "تحذير: سجلات غير مؤرشفة"
                    lblTitle.ForeColor = Color.White
                    lblMessage.ForeColor = Color.Black
                    Me.Size = New Size(250, 100)
            End Select
        End Sub

        Public Sub ShowUnarchivedRecords()
            Try
                Dim count As Integer = DataHelper.UnarchivedSaderCount

                If count > 0 Then
                    Dim message As String = $"لديك {count} سجل غير مؤرشف."
                    If count = 1 Then
                        message = "لديك سجل واحد غير مؤرشف."
                    End If

                    lblMessage.Text = message & vbNewLine & "انقر لعرض التفاصيل."

                    Me.Show()
                    Me.TopMost = True

                    ' في حالة إشعار البالون، عرضه كإشعار بجوار الساعة
                    If _notificationType = NotificationType.Balloon Then
                        ShowBalloonNotification()
                    End If
                Else
                    Me.Hide()
                End If
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"خطأ في عرض الإشعار: {ex.Message}")
            End Try
        End Sub

        Private Sub ShowBalloonNotification()
            Try
                ' إنشاء إشعار بجوار الساعة
                Dim notifyIcon As New NotifyIcon()
                notifyIcon.Icon = SystemIcons.Information
                notifyIcon.Visible = True

                ' تحديد النصوص
                notifyIcon.BalloonTipTitle = "سجلات غير مؤرشفة"
                notifyIcon.BalloonTipText = $"لديك {DataHelper.UnarchivedSaderCount} سجل غير مؤرشف."
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info

                ' عرض الإشعار
                notifyIcon.ShowBalloonTip(3000)

                ' إعداد معالج الحدث للنقر على الإشعار
                AddHandler notifyIcon.MouseClick, AddressOf NotifyIcon_MouseClick

                ' إعداد مؤقت لإخفاء الإشعار بعد فترة
                Dim hideTimer As New Timer()
                hideTimer.Interval = 5000 ' 5 ثوانٍ
                AddHandler hideTimer.Tick, Sub(sender As Object, e As EventArgs)
                                               notifyIcon.Visible = False
                                               notifyIcon.Dispose()
                                               hideTimer.Stop()
                                               hideTimer.Dispose()
                                           End Sub
                hideTimer.Start()
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"خطأ في عرض إشعار البالون: {ex.Message}")
            End Try
        End Sub

        Private Sub NotifyIcon_MouseClick(sender As Object, e As MouseEventArgs)
            If e.Button = MouseButtons.Left Then
                ShowUnarchivedRecordsDetails()
            End If
        End Sub

        Private Sub ShowUnarchivedRecordsDetails()
            Try
                ' إخفاء الإشعار الحالي
                Me.Hide()

                ' فتح نموذج تفاصيل السجلات غير المؤرشفة
                Dim detailsForm As New UnarchivedRecordsForm()
                detailsForm.ShowDialog()

                ' تحديث العدد بعد العودة
                DataHelper.UpdateUnarchivedSaderCount()
            Catch ex As Exception
                MessageBox.Show("خطأ في عرض تفاصيل السجلات غير المؤرشفة: " & ex.Message,
                               "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub pnlHeader_MouseDown(sender As Object, e As MouseEventArgs) Handles pnlHeader.MouseDown
            If e.Button = MouseButtons.Left Then
                _isDragging = True
                _dragCursorPoint = Cursor.Position
                _dragFormPoint = Me.Location
            End If
        End Sub

        Private Sub pnlHeader_MouseMove(sender As Object, e As MouseEventArgs) Handles pnlHeader.MouseMove
            If _isDragging Then
                Dim diff As Size = Point.Subtract(Cursor.Position, _dragCursorPoint)
                Me.Location = Point.Add(_dragFormPoint, New Size(diff.Width, diff.Height))
            End If
        End Sub

        Private Sub pnlHeader_MouseUp(sender As Object, e As MouseEventArgs) Handles pnlHeader.MouseUp
            _isDragging = False
        End Sub

        Private Sub lblMessage_Click(sender As Object, e As EventArgs) Handles lblMessage.Click
            ShowUnarchivedRecordsDetails()
        End Sub

        Private Sub btnArchiveAll_Click(sender As Object, e As EventArgs) Handles btnArchiveAll.Click
            ShowUnarchivedRecordsDetails()
        End Sub

        Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
            Me.Close()
        End Sub

    Private Sub NotificationsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        ShowBalloonNotification()
        ' بدء المؤقت مع المدة المحددة من الإعدادات
        Timer1.Interval = My.Settings.NotificationsFormInterval * 1000 ' تحويل الثواني إلى مللي ثانية
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' إيقاف المؤقت أولاً
        Timer1.Stop()

        ' إغلاق النموذج
        Me.Close()
    End Sub
End Class

