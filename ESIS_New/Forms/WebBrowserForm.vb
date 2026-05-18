Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Win32
Public Class WebBrowserForm
    Private scaler As ControlScaler
    ' المتغيرات الأساسية
    Private _url As String = ""
    Private _title As String = "متصفح الويب"
    Private _isMdiChild As Boolean = False
    Private closeButtonRect As Rectangle
    Private hoverTabIndex As Integer = -1
    Private hoverCloseRect As Rectangle
    Public HistoryList As New List(Of String)
    Private FavoritesPath As String = "favorites.txt"
    Public isCreatingTab As Boolean = False
    ' تعريف الأحداث
    Public Event DataSelected(filePath As String, fileName As String, fileExtension As String)
#Region "إنشاء نافذة جديدة للتبويب"
    Private Function GetRoundedRect(rect As Rectangle, radius As Integer) As Drawing2D.GraphicsPath

        Dim path As New Drawing2D.GraphicsPath()

        Dim d As Integer = radius * 2

        path.AddArc(rect.X, rect.Y, d, d, 180, 90)
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90)
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90)
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90)

        path.CloseFigure()

        Return path

    End Function

    Private Sub TabControl1_DrawItem(sender As Object, e As DrawItemEventArgs) _
Handles TabControl1.DrawItem

        Dim tabRect As Rectangle = TabControl1.GetTabRect(e.Index)

        ' لون الخلفية
        Dim bgColor As Color = If(e.Index = TabControl1.SelectedIndex, Color.White, Color.LightGray)

        Using brush As New SolidBrush(bgColor)
            e.Graphics.FillRectangle(brush, tabRect)
        End Using

        ' النص
        TextRenderer.DrawText(
        e.Graphics,
        TabControl1.TabPages(e.Index).Text,
        e.Font,
        New Point(tabRect.X + 5, tabRect.Y + 4),
        Color.Black)

        ' ==============================
        ' زر الإغلاق (مربع بزوايا ناعمة)
        ' ==============================
        Dim size As Integer = 18

        Dim closeRect As New Rectangle(
    tabRect.Right - size - 6,
    tabRect.Top + (tabRect.Height - size) \ 2,
    size,
    size)

        Dim isHover As Boolean = (hoverTabIndex = e.Index)

        Dim backColor As Color
        Dim textColor As Color

        If isHover Then
            backColor = Color.FromArgb(220, 80, 80) ' Hover أحمر
            textColor = Color.White
        Else
            backColor = Color.FromArgb(230, 230, 230) ' رمادي ناعم
            textColor = Color.Black
        End If

        ' رسم Rounded Rectangle
        Using path As Drawing2D.GraphicsPath = GetRoundedRect(closeRect, 4)
            Using b As New SolidBrush(backColor)
                e.Graphics.FillPath(b, path)
            End Using
        End Using

        ' رسم علامة X
        TextRenderer.DrawText(
    e.Graphics,
    "✕",
    e.Font,
    closeRect,
    textColor,
    TextFormatFlags.HorizontalCenter Or
    TextFormatFlags.VerticalCenter Or
    TextFormatFlags.SingleLine)

    End Sub

    Private Sub TabControl1_MouseDown(sender As Object, e As MouseEventArgs) _
Handles TabControl1.MouseDown

        For i As Integer = 0 To TabControl1.TabPages.Count - 1

            Dim tabRect As Rectangle = TabControl1.GetTabRect(i)
            Dim size As Integer = 18

            Dim closeRect As New Rectangle(
    tabRect.Right - size - 6,
    tabRect.Top + (tabRect.Height - size) \ 2,
    size,
    size)

            If closeRect.Contains(e.Location) Then

                If TabControl1.TabPages.Count > 1 Then

                    TabControl1.TabPages.RemoveAt(i)

                    If i > 0 Then
                        TabControl1.SelectedIndex = i - 1
                    End If

                End If

                Exit For

            End If

        Next

    End Sub

    Private Sub TabControl1_MouseMove(sender As Object, e As MouseEventArgs) _
Handles TabControl1.MouseMove

        Dim foundIndex As Integer = -1

        For i As Integer = 0 To TabControl1.TabPages.Count - 1

            Dim tabRect As Rectangle = TabControl1.GetTabRect(i)
            Dim size As Integer = 18

            Dim closeRect As New Rectangle(
    tabRect.Right - size - 6,
    tabRect.Top + (tabRect.Height - size) \ 2,
    size,
    size)

            If closeRect.Contains(e.Location) Then
                foundIndex = i
                Exit For
            End If

        Next

        If hoverTabIndex <> foundIndex Then
            hoverTabIndex = foundIndex
            TabControl1.Invalidate()
        End If

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) _
Handles TabControl1.SelectedIndexChanged

        If isCreatingTab Then Exit Sub
        If TabControl1.SelectedTab Is Nothing Then Exit Sub

        ' تحديث URL
        Dim browser = GetCurrentBrowser()
        If browser IsNot Nothing AndAlso browser.Url IsNot Nothing Then
            txtUrl.Text = browser.Url.ToString()
        End If

        ' زر +
        If TabControl1.SelectedTab.Name = "plusTab" Then

            CreateNewTab("about:blank", TabControl1)

            TabControl1.SelectedIndex = TabControl1.TabPages.Count - 2

        End If

        'If TabControl1.SelectedTab IsNot Nothing Then

        '    Dim browser As WebBrowser =
        'CType(TabControl1.SelectedTab.Controls(0), WebBrowser)

        '    If browser.Url IsNot Nothing Then
        '        txtUrl.Text = browser.Url.ToString()
        '    End If

        'End If

    End Sub

    Public Function GetCurrentBrowser() As WebBrowser

        If TabControl1.SelectedTab Is Nothing Then Return Nothing

        For Each c As Control In TabControl1.SelectedTab.Controls
            If TypeOf c Is WebBrowser Then
                Return CType(c, WebBrowser)
            End If
        Next

        Return Nothing


        'If TabControl1.SelectedTab Is Nothing Then Return Nothing

        'Return CType(TabControl1.SelectedTab.Controls(0), WebBrowser)

    End Function

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        Select Case keyData

            Case Keys.Control Or Keys.T
                CreateNewTab("https://www.google.com", TabControl1)
                Return True

            Case Keys.Control Or Keys.W
                CloseCurrentTab()
                Return True

            Case Keys.Control Or Keys.H
                ShowHistory()
                Return True

            Case Keys.Control Or Keys.D
                AddToFavorites()
                Return True

            Case Keys.Control Or Keys.B
                ShowFavorites()
                Return True

            Case Keys.F8
                Dim browser = GetCurrentBrowser()
                If browser Is Nothing Then
                    Return False
                End If

                browser.ShowPrintPreviewDialog()
                Return True

        End Select

        Return MyBase.ProcessCmdKey(msg, keyData)

    End Function

    Private Sub CloseCurrentTab()

        If TabControl1.SelectedTab Is Nothing Then Exit Sub

        ' لا تغلق Tab +
        If TabControl1.SelectedTab.Name = "plusTab" Then Exit Sub

        If TabControl1.TabPages.Count > 1 Then

            Dim index As Integer = TabControl1.SelectedIndex

            TabControl1.TabPages.RemoveAt(index)

            If index > 0 Then
                TabControl1.SelectedIndex = index - 1
            End If

        End If

    End Sub

    Private Sub ShowHistory()

        If HistoryList.Count = 0 Then
            MessageBox.Show("لا يوجد سجل تصفح", "تنبيه")
            Exit Sub
        End If

        Dim frm As New Form With {
        .Text = "سجل التصفح",
        .Size = New Size(400, 500)
    }

        Dim list As New ListBox With {
        .Dock = DockStyle.Fill
    }

        list.Items.AddRange(HistoryList.ToArray())

        AddHandler list.DoubleClick,
    Sub()
        If list.SelectedItem IsNot Nothing Then
            CreateNewTab(list.SelectedItem.ToString(), TabControl1)
            frm.Close()
        End If
    End Sub

        frm.Controls.Add(list)
        frm.Show()

    End Sub

    Private Sub AddToFavorites()

        If String.IsNullOrEmpty(txtUrl.Text) Then Exit Sub

        Dim existing As New List(Of String)

        If File.Exists(FavoritesPath) Then
            existing = File.ReadAllLines(FavoritesPath).ToList()
        End If

        If existing.Contains(txtUrl.Text) Then
            MessageBox.Show("الموقع موجود مسبقًا")
            Exit Sub
        End If

        File.AppendAllText(FavoritesPath, txtUrl.Text & Environment.NewLine)

        MessageBox.Show("تمت الإضافة إلى المفضلة")

    End Sub

    Private Sub ShowFavorites()

        If Not File.Exists(FavoritesPath) Then
            MessageBox.Show("لا يوجد مفضلة")
            Exit Sub
        End If

        Dim data = File.ReadAllLines(FavoritesPath)

        If data.Length = 0 Then Exit Sub

        Dim frm As New Form With {
        .Text = "المفضلة",
        .Size = New Size(400, 500)
    }

        Dim list As New ListBox With {
        .Dock = DockStyle.Fill
    }

        list.Items.AddRange(data)

        AddHandler list.DoubleClick,
    Sub()
        If list.SelectedItem IsNot Nothing Then
            CreateNewTab(list.SelectedItem.ToString(), TabControl1)
            frm.Close()
        End If
    End Sub

        frm.Controls.Add(list)
        frm.Show()

    End Sub

    Private Sub btnAddToFavorites_Click(sender As Object, e As EventArgs) Handles btnAddToFavorites.Click
        AddToFavorites()
    End Sub

    Private Sub btnShowHistory_Click(sender As Object, e As EventArgs) Handles btnShowHistory.Click
        ShowHistory()
    End Sub

    Private Sub btnShowFavorites_Click(sender As Object, e As EventArgs) Handles btnShowFavorites.Click
        ShowFavorites()
    End Sub

#End Region


    ' دالة التحميل
    Public Sub New(Optional url As String = "", Optional title As String = "متصفح الويب", Optional isMdiChild As Boolean = False)
        InitializeComponent()
        _url = url
        _title = title
        _isMdiChild = isMdiChild
        ' عنوان الفورم
        Me.Text = _title

        ' تهيئة كلاس التحجيم مع النموذج الحالي
        scaler = New ControlScaler(Me)
    End Sub

    Private Sub WebBrowserForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        ' التحقق من أن الكائن scaler مهيأ قبل استدعائه
        If scaler IsNot Nothing Then
            scaler.ScaleControls(Me)
        End If
    End Sub

    Private Sub WebBrowserForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Try
        '    ' تعيين وضع المحاكاة قبل تحميل أي صفحات
        '    SetBrowserEmulationMode()
        '    ' تعيين العنوان
        '    Me.Text = _title
        '    ' استخدم الدالة الجديدة لتحميل الرابط بأمان
        '    If Not String.IsNullOrEmpty(_url) Then
        '        LoadUrlSafely(_url)
        '        txtUrl.Text = _url
        '    Else
        '        ' تحميل الصفحة الرئيسية الافتراضية
        '        Dim homeUrl As String = "https://www.mod.gov.kw"
        '        LoadUrlSafely(homeUrl)
        '        txtUrl.Text = homeUrl
        '    End If
        '    ' تحميل الصفحة
        '    If Not String.IsNullOrEmpty(_url) Then
        '        webBrowser.Navigate(_url)
        '        txtUrl.Text = _url
        '    End If

        '    ' تعيين خصائص النموذج
        '    If _isMdiChild Then
        '        Me.FormBorderStyle = FormBorderStyle.None
        '        Me.Dock = DockStyle.Fill
        '        Me.WindowState = FormWindowState.Normal
        '    Else
        '        Me.WindowState = FormWindowState.Normal
        '    End If

        '    ' تهيئة أزرار التنقل
        '    UpdateNavigationButtons()
        'Catch ex As Exception
        '    MessageBox.Show($"خطأ في تحميل الصفحة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try


        '' تهيئة CefSharp مرة واحدة
        'If Not Cef.IsInitialized Then
        '    Dim settings As New CefSettings()
        '    Cef.Initialize(settings)
        'End If
        Me.KeyPreview = True
        TabControl1.DrawMode = TabDrawMode.OwnerDrawFixed
        TabControl1.Padding = New Point(20, 4)
        TabControl1.TabPages.Add("+")
        '' أول تبويب

        If _url <> "" Then
            CreateNewTab(_url, TabControl1)
        End If


    End Sub

    ''' <summary>
    ''' تحديث حالة أزرار التنقل بناءً على إمكانية التنقل
    ''' </summary>

    Public Sub UpdateNavigationButtons()

        Dim browser = GetCurrentBrowser()

        If browser Is Nothing Then Exit Sub

        btnBack.Enabled = browser.CanGoBack
        btnForward.Enabled = browser.CanGoForward

    End Sub

    'Private Sub UpdateNavigationButtons()
    '    Try
    '        btnBack.Enabled = webBrowser.CanGoBack
    '        btnForward.Enabled = webBrowser.CanGoForward
    '    Catch ex As Exception
    '        Debug.WriteLine($"خطأ في تحديث أزرار التنقل: {ex.Message}")
    '    End Try
    'End Sub

    ''' <summary>
    ''' الانتقال إلى الخلف
    ''' </summary>
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Try

            Dim browser = GetCurrentBrowser()
            If browser Is Nothing Then Exit Sub

            If browser.CanGoBack Then
                browser.GoBack()
            End If

            UpdateNavigationButtons()

            'If webBrowser.CanGoBack Then
            '    webBrowser.GoBack()
            '    UpdateNavigationButtons()
            'End If
        Catch ex As Exception
            MessageBox.Show($"خطأ في العودة للصفحة السابقة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' الانتقال إلى الأمام
    ''' </summary>
    Private Sub btnForward_Click(sender As Object, e As EventArgs) Handles btnForward.Click
        Try
            Dim browser = GetCurrentBrowser()
            If browser Is Nothing Then Exit Sub

            If browser.CanGoForward Then
                browser.GoForward()
            End If

            UpdateNavigationButtons()
            'If webBrowser.CanGoForward Then
            '    webBrowser.GoForward()
            '    UpdateNavigationButtons()
            'End If
        Catch ex As Exception
            MessageBox.Show($"خطأ في التقدم للصفحة التالية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تحديث الصفحة
    ''' </summary>
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            Dim browser = GetCurrentBrowser()
            If browser Is Nothing Then Exit Sub

            browser.Refresh()

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحديث الصفحة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' العودة للصفحة الرئيسية
    ''' </summary>
    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        Try
            Dim browser = GetCurrentBrowser()
            If browser Is Nothing Then Exit Sub

            Dim homeUrl As String = "https://www.google.com"

            browser.Navigate(homeUrl)
            txtUrl.Text = homeUrl
        Catch ex As Exception
            MessageBox.Show($"خطأ في العودة للصفحة الرئيسية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '''' <summary>
    '''' الانتقال إلى الرابط المدخل
    '''' </summary>
    'Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
    '    Try
    '        Dim browser = GetCurrentBrowser()
    '        If browser Is Nothing Then Exit Sub

    '        Dim url As String = txtUrl.Text.Trim()

    '        If String.IsNullOrEmpty(url) Then Exit Sub
    '        If Uri.IsWellFormedUriString(url, UriKind.Absolute) = False Then
    '            url = "https://www.google.com"
    '        End If

    '        If Not url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) AndAlso
    '           Not url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then

    '            url = "http://" & url

    '        End If

    '        browser.Navigate(url)
    '    Catch ex As Exception
    '        MessageBox.Show($"خطأ في التنقل إلى الصفحة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click

        Try

            Dim url As String = txtUrl.Text.Trim()

            If String.IsNullOrEmpty(url) Then Exit Sub

            ' ==============================
            ' معالجة الرابط
            ' ==============================
            If Not url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) AndAlso
           Not url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then

                If url.Contains(".") Then
                    url = "http://" & url
                Else
                    url = "https://www.google.com"
                End If

            End If

            ' تحقق نهائي
            Dim uriResult As Uri = Nothing

            If Uri.TryCreate(url, UriKind.Absolute, uriResult) Then

                ' ==============================
                ' فتح تبويب جديد
                ' ==============================
                CreateNewTab(uriResult.ToString(), TabControl1)

            Else
                MessageBox.Show("الرابط غير صالح", "تنبيه")
            End If

        Catch ex As Exception
            MessageBox.Show("خطأ: " & ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' طباعة الصفحة
    ''' </summary>
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Try

            Dim browser = GetCurrentBrowser()
            If browser Is Nothing Then Exit Sub

            browser.ShowPrintPreviewDialog()
        Catch ex As Exception
            MessageBox.Show($"خطأ في طباعة الصفحة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' فتح الصفحة في المتصفح الافتراضي
    ''' </summary>
    Private Sub btnOpenInBrowser_Click(sender As Object, e As EventArgs) Handles btnOpenInBrowser.Click
        Try
            Dim url As String = txtUrl.Text.Trim()

            If String.IsNullOrEmpty(url) Then Exit Sub

            Process.Start(New ProcessStartInfo(url) With {
                .UseShellExecute = True
                })

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
    ''' معالجة تغيير النص في مربع العنوان
    ''' </summary>
    Private Sub txtUrl_KeyDown(sender As Object, e As KeyEventArgs) Handles txtUrl.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnGo.PerformClick()
        End If
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



    '''' <summary>
    '''' دالة لتحميل رابط ويب في النموذج
    '''' </summary>
    'Public Sub LoadUrl(url As String)
    '    Try
    '        ' حفظ الرابط والعنوان
    '        _url = url

    '        ' تحميل الصفحة
    '        If Not String.IsNullOrEmpty(_url) Then
    '            webBrowser.Navigate(_url)
    '            txtUrl.Text = _url
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show($"خطأ في تحميل الصفحة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

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



    '' دالة لضمان تحميل المحتوى باستخدام أحدث إصدار
    'Private Sub EnsureLatestIEVersion()
    '    Try
    '        ' إضافة تعريف X-UA-Compatible إلى الصفحة إذا لزم الأمر
    '        If webBrowser.Document IsNot Nothing Then
    '            Dim head As HtmlElement = webBrowser.Document.GetElementsByTagName("head")(0)
    '            If head IsNot Nothing Then
    '                Dim meta As HtmlElement = webBrowser.Document.CreateElement("meta")
    '                meta.SetAttribute("http-equiv", "X-UA-Compatible")
    '                meta.SetAttribute("content", "IE=edge")
    '                head.AppendChild(meta)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في إضافة X-UA-Compatible: {ex.Message}")
    '    End Try
    'End Sub

    '''' <summary>
    '''' تحميل الصفحة مع التأكد من استخدام أحدث إصدار
    '''' </summary>
    'Private Sub LoadUrlSafely(url As String)
    '    Try
    '        ' محاولة تحميل الصفحة باستخدام JavaScript لإجبار استخدام أحدث إصدار
    '        Dim script As String = "document.documentMode || document.compatMode === 'CSS1Compat' && document.documentElement.offsetWidth || document.body.offsetWidth;"

    '        ' تحميل الصفحة
    '        webBrowser.Navigate(url)

    '        ' الانتظار حتى تحميل الصفحة
    '        AddHandler webBrowser.DocumentCompleted, Sub(sender As Object, e As WebBrowserDocumentCompletedEventArgs)
    '                                                     Try
    '                                                         ' إضافة تعريف X-UA-Compatible
    '                                                         Dim doc As HtmlDocument = webBrowser.Document
    '                                                         If doc IsNot Nothing Then
    '                                                             Dim head As HtmlElement = doc.GetElementsByTagName("head")(0)
    '                                                             If head IsNot Nothing Then
    '                                                                 Dim meta As HtmlElement = doc.CreateElement("meta")
    '                                                                 meta.SetAttribute("http-equiv", "X-UA-Compatible")
    '                                                                 meta.SetAttribute("content", "IE=edge")
    '                                                                 head.AppendChild(meta)
    '                                                             End If
    '                                                         End If
    '                                                     Catch ex As Exception
    '                                                         Debug.WriteLine($"خطأ في تعديل الصفحة: {ex.Message}")
    '                                                     End Try
    '                                                 End Sub
    '    Catch ex As Exception
    '        Debug.WriteLine($"خطأ في تحميل الصفحة بأمان: {ex.Message}")
    '    End Try
    'End Sub


End Class