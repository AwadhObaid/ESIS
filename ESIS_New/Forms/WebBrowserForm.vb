Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Win32
Public Class WebBrowserForm
    ' المتغيرات الأساسية
    Private _url As String = ""
    Private _title As String = "متصفح الويب"
    Private _isMdiChild As Boolean = False

    ' تعريف الأحداث
    Public Event DataSelected(filePath As String, fileName As String, fileExtension As String)

    ' دالة التحميل
    Public Sub New(url As String, Optional title As String = "متصفح الويب", Optional isMdiChild As Boolean = False)
        InitializeComponent()
        _url = url
        _title = title
        _isMdiChild = isMdiChild
    End Sub

    Private Sub WebBrowserForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' تعيين وضع المحاكاة قبل تحميل أي صفحات
            SetBrowserEmulationMode()
            ' تعيين العنوان
            Me.Text = _title
            ' استخدم الدالة الجديدة لتحميل الرابط بأمان
            If Not String.IsNullOrEmpty(_url) Then
                LoadUrlSafely(_url)
                txtUrl.Text = _url
            Else
                ' تحميل الصفحة الرئيسية الافتراضية
                Dim homeUrl As String = "https://www.mod.gov.kw"
                LoadUrlSafely(homeUrl)
                txtUrl.Text = homeUrl
            End If
            ' تحميل الصفحة
            If Not String.IsNullOrEmpty(_url) Then
                webBrowser.Navigate(_url)
                txtUrl.Text = _url
            End If

            ' تعيين خصائص النموذج
            If _isMdiChild Then
                Me.FormBorderStyle = FormBorderStyle.None
                Me.Dock = DockStyle.Fill
                Me.WindowState = FormWindowState.Normal
            Else
                Me.WindowState = FormWindowState.Normal
            End If

            ' تهيئة أزرار التنقل
            UpdateNavigationButtons()
        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل الصفحة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تحديث حالة أزرار التنقل بناءً على إمكانية التنقل
    ''' </summary>
    Private Sub UpdateNavigationButtons()
        Try
            btnBack.Enabled = webBrowser.CanGoBack
            btnForward.Enabled = webBrowser.CanGoForward
        Catch ex As Exception
            Debug.WriteLine($"خطأ في تحديث أزرار التنقل: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' الانتقال إلى الخلف
    ''' </summary>
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Try
            If webBrowser.CanGoBack Then
                webBrowser.GoBack()
                UpdateNavigationButtons()
            End If
        Catch ex As Exception
            MessageBox.Show($"خطأ في العودة للصفحة السابقة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' الانتقال إلى الأمام
    ''' </summary>
    Private Sub btnForward_Click(sender As Object, e As EventArgs) Handles btnForward.Click
        Try
            If webBrowser.CanGoForward Then
                webBrowser.GoForward()
                UpdateNavigationButtons()
            End If
        Catch ex As Exception
            MessageBox.Show($"خطأ في التقدم للصفحة التالية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تحديث الصفحة
    ''' </summary>
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            webBrowser.Refresh()
        Catch ex As Exception
            MessageBox.Show($"خطأ في تحديث الصفحة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' العودة للصفحة الرئيسية
    ''' </summary>
    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        Try
            ' يمكنك تعيين صفحة البداية حسب احتياجات النظام
            Dim homeUrl As String = "https://www.mod.gov.kw"
            webBrowser.Navigate(homeUrl)
            txtUrl.Text = homeUrl
        Catch ex As Exception
            MessageBox.Show($"خطأ في العودة للصفحة الرئيسية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' الانتقال إلى الرابط المدخل
    ''' </summary>
    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Try
            Dim url As String = txtUrl.Text.Trim()
            If Not String.IsNullOrEmpty(url) Then
                ' إضافة http:// إذا لم يكن موجودًا
                If Not url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) AndAlso
                   Not url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then
                    url = "http://" & url
                End If

                webBrowser.Navigate(url)
                txtUrl.Text = url
            End If
        Catch ex As Exception
            MessageBox.Show($"خطأ في التنقل إلى الصفحة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' طباعة الصفحة
    ''' </summary>
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Try
            webBrowser.ShowPrintPreviewDialog()
        Catch ex As Exception
            MessageBox.Show($"خطأ في طباعة الصفحة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' فتح الصفحة في المتصفح الافتراضي
    ''' </summary>
    Private Sub btnOpenInBrowser_Click(sender As Object, e As EventArgs) Handles btnOpenInBrowser.Click
        Try
            If Not String.IsNullOrEmpty(txtUrl.Text) Then
                Process.Start(txtUrl.Text)
            End If
        Catch ex As Exception
            MessageBox.Show($"خطأ في فتح الصفحة في المتصفح الافتراضي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' إغلاق النموذج
    ''' </summary>
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"خطأ في إغلاق النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة اكتمال تحميل الصفحة
    ''' </summary>
    Private Sub webBrowser_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles webBrowser.DocumentCompleted
        Try
            ' تحديث عنوان النموذج بعنوان الصفحة
            If _isMdiChild Then
                Me.Text = $"{_title} - {webBrowser.DocumentTitle}"
            Else
                Me.Text = $"{_title} - {webBrowser.DocumentTitle}"
            End If

            txtUrl.Text = webBrowser.Url.ToString()
            UpdateNavigationButtons()

            ' التأكد من استخدام أحدث إصدار من IE
            EnsureLatestIEVersion()
        Catch ex As Exception
            ' تجاهل الأخطاء البسيطة
            Debug.WriteLine($"خطأ في معالجة اكتمال تحميل الصفحة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة تغيير النص في مربع العنوان
    ''' </summary>
    Private Sub txtUrl_KeyDown(sender As Object, e As KeyEventArgs) Handles txtUrl.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnGo.PerformClick()
        End If
    End Sub

    ''' <summary>
    ''' معالجة فتح نافذة جديدة
    ''' </summary>
    Private Sub webBrowser_NewWindow(sender As Object, e As CancelEventArgs) Handles webBrowser.NewWindow
        Try
            ' فتح الروابط الجديدة في نافذة جديدة من نفس النوع
            Dim newUrl As String = webBrowser.StatusText
            If Not String.IsNullOrEmpty(newUrl) Then
                Dim newForm As New WebBrowserForm(newUrl, _title, _isMdiChild)

                ' جعل النموذج نافذة فرعية إذا كان هذا نموذجًا فرعيًا
                If _isMdiChild AndAlso Me.MdiParent IsNot Nothing Then
                    newForm.MdiParent = Me.MdiParent
                    newForm.WindowState = FormWindowState.Maximized
                End If

                newForm.Show()
                e.Cancel = True
            End If
        Catch ex As Exception
            Debug.WriteLine($"خطأ في فتح نافذة جديدة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة تحديد النص في مربع العنوان
    ''' </summary>
    Private Sub txtUrl_Enter(sender As Object, e As EventArgs) Handles txtUrl.Enter
        txtUrl.SelectAll()
    End Sub

    ''' <summary>
    ''' معالجة إغلاق النموذج
    ''' </summary>
    Private Sub WebBrowserForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            ' إذا كان هذا نموذجًا فرعيًا في نظام MDI، لا نريد إغلاق التطبيق بالكامل
            If _isMdiChild Then
                e.Cancel = False
            End If
        Catch ex As Exception
            Debug.WriteLine($"خطأ في معالجة إغلاق النموذج: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' دالة لتحميل رابط ويب في النموذج
    ''' </summary>
    Public Sub LoadUrl(url As String)
        Try
            ' حفظ الرابط والعنوان
            _url = url

            ' تحميل الصفحة
            If Not String.IsNullOrEmpty(_url) Then
                webBrowser.Navigate(_url)
                txtUrl.Text = _url
            End If
        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل الصفحة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' دالة لتعيين إصدار Internet Explorer المستخدم في WebBrowser
    Private Shared Sub SetBrowserEmulationMode()
        Try
            ' الحصول على اسم العملية
            Dim appName As String = Path.GetFileNameWithoutExtension(Application.ExecutablePath)

            ' مفتاح التسجيل لـ Internet Explorer
            Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(
                "Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", True)

                ' إذا لم يكن المفتاح موجودًا، قم بإنشائه
                If key Is Nothing Then
                    Registry.CurrentUser.CreateSubKey(
                        "Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION")
                Else
                    ' تعيين قيمة المفتاح
                    ' 11001 = IE11, 10001 = IE10, 9000 = IE9
                    ' يمكنك تغيير القيمة حسب إصدار IE المطلوب
                    key.SetValue(appName & ".exe", 11001, RegistryValueKind.DWord)
                End If
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تعيين وضع المحاكاة: {ex.Message}")
        End Try
    End Sub

    ' دالة لضمان تحميل المحتوى باستخدام أحدث إصدار
    Private Sub EnsureLatestIEVersion()
        Try
            ' إضافة تعريف X-UA-Compatible إلى الصفحة إذا لزم الأمر
            If webBrowser.Document IsNot Nothing Then
                Dim head As HtmlElement = webBrowser.Document.GetElementsByTagName("head")(0)
                If head IsNot Nothing Then
                    Dim meta As HtmlElement = webBrowser.Document.CreateElement("meta")
                    meta.SetAttribute("http-equiv", "X-UA-Compatible")
                    meta.SetAttribute("content", "IE=edge")
                    head.AppendChild(meta)
                End If
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إضافة X-UA-Compatible: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحميل الصفحة مع التأكد من استخدام أحدث إصدار
    ''' </summary>
    Private Sub LoadUrlSafely(url As String)
        Try
            ' محاولة تحميل الصفحة باستخدام JavaScript لإجبار استخدام أحدث إصدار
            Dim script As String = "document.documentMode || document.compatMode === 'CSS1Compat' && document.documentElement.offsetWidth || document.body.offsetWidth;"

            ' تحميل الصفحة
            webBrowser.Navigate(url)

            ' الانتظار حتى تحميل الصفحة
            AddHandler webBrowser.DocumentCompleted, Sub(sender As Object, e As WebBrowserDocumentCompletedEventArgs)
                                                         Try
                                                             ' إضافة تعريف X-UA-Compatible
                                                             Dim doc As HtmlDocument = webBrowser.Document
                                                             If doc IsNot Nothing Then
                                                                 Dim head As HtmlElement = doc.GetElementsByTagName("head")(0)
                                                                 If head IsNot Nothing Then
                                                                     Dim meta As HtmlElement = doc.CreateElement("meta")
                                                                     meta.SetAttribute("http-equiv", "X-UA-Compatible")
                                                                     meta.SetAttribute("content", "IE=edge")
                                                                     head.AppendChild(meta)
                                                                 End If
                                                             End If
                                                         Catch ex As Exception
                                                             Debug.WriteLine($"خطأ في تعديل الصفحة: {ex.Message}")
                                                         End Try
                                                     End Sub
        Catch ex As Exception
            Debug.WriteLine($"خطأ في تحميل الصفحة بأمان: {ex.Message}")
        End Try
    End Sub
End Class