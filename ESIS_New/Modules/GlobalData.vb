Imports System.Data.SqlClient
Imports System.IO
Imports System.Security
Imports System.Security.AccessControl
Imports System.Threading
Imports Microsoft.Win32


Module GlobalData
    Public LoggedInUserName As String = ""
    Public LoggedInUserID As Integer = 0

    ' صلاحيات المستخدم الحالي
    Public UserCanAdd As Boolean = True
    Public UserCanEdit As Boolean = True
    Public UserCanDelete As Boolean = True
    Public UserCanSearch As Boolean = True ' البحث مسموح افتراضياً
    Public UserCanManageUsers As Boolean = True
    Public UserCanManagePermissions As Boolean = True
    Public UserCanPtint As Boolean = False
    Public CompanyName As String = "نظام إدارة الوثائق الإلكتروني"

    ' متغير لتخزين حالة المستخدم
    Public IsCurrentUserActive As Boolean = False
    Public IsCurrentUserAdmin As Boolean = False
    ' متغير لتخزين صورة المستخدم الحالي
    Public CurrentUserImage As Image = Nothing


#Region "التفعيل"

    Public Function WriteValue(key As String) As Boolean
        Dim baseKey As RegistryKey = Nothing
        Dim subKey As RegistryKey = Nothing
        Try
            ' فتح المفتاح الأساسي
            baseKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
            If baseKey Is Nothing Then
                Throw New Exception("تعذر فتح مسار SOFTWARE في التسجيل.")
            End If

            ' إنشاء المفتاح الفرعي إذا لم يكن موجودًا
            subKey = baseKey.CreateSubKey("ESIS")
            If subKey Is Nothing Then
                Throw New Exception("تعذر إنشاء مفتاح ESIS في التسجيل.")
            End If

            ' حفظ القيمة
            subKey.SetValue("System_key", key)
            Return True

        Catch ex As SecurityException
            MessageBox.Show("يحتاج البرنامج لصلاحيات مدير (Administrator) لتفعيله.",
                       "صلاحيات غير كافية", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في كتابة قيمة التسجيل: {ex.Message}")
            Return False
        Finally
            ' إغلاق المفاتيح بأمان
            If subKey IsNot Nothing Then
                Try
                    subKey.Close()
                Catch
                End Try
            End If
            If baseKey IsNot Nothing Then
                Try
                    baseKey.Close()
                Catch
                End Try
            End If
        End Try
    End Function







    '#Region "التفعيل"
    '    ''' <summary>
    '    ''' كتابة قيمة مفتاح التفعيل في تسجيل ويندوز
    '    ''' </summary>
    '    Public Function WriteValue(key As String) As Boolean
    '        Dim regKey As RegistryKey = Nothing
    '        Try
    '            ' فتح أو إنشاء مسار البرنامج في التسجيل
    '            regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", True)

    '            If regKey IsNot Nothing Then
    '                ' إنشاء المفتاح الفرعي إذا لم يكن موجودًا
    '                Dim subKey As RegistryKey = regKey.OpenSubKey("ESIS")
    '                If subKey Is Nothing Then
    '                    regKey.CreateSubKey("ESIS")
    '                    subKey = regKey.OpenSubKey("ESIS", True)
    '                Else
    '                    subKey = regKey.OpenSubKey("ESIS", True)
    '                End If

    '                If subKey IsNot Nothing Then
    '                    ' حفظ قيمة مفتاح النظام
    '                    subKey.SetValue("System_key", key)
    '                    subKey.Close()
    '                End If

    '                regKey.Close()
    '                Return True
    '            Else
    '                System.Diagnostics.Debug.WriteLine("فشل في فتح مسار SOFTWARE في التسجيل")
    '                Return False
    '            End If

    '        Catch ex As SecurityException
    '            System.Diagnostics.Debug.WriteLine($"مشكلة في الصلاحيات عند الكتابة في التسجيل: {ex.Message}")
    '            MessageBox.Show("يحتاج البرنامج لصلاحيات مدير (Administrator) لتشغيله بشكل صحيح.",
    '                       "صلاحيات غير كافية", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Return False

    '        Catch ex As Exception
    '            System.Diagnostics.Debug.WriteLine($"خطأ في كتابة قيمة التسجيل: {ex.Message}")
    '            Return False

    '        Finally
    '            ' التأكد من إغلاق المفتاح
    '            If regKey IsNot Nothing Then
    '                regKey.Close()
    '            End If
    '        End Try
    '    End Function

    ''' <summary>
    ''' قراءة قيمة مفتاح التفعيل من تسجيل ويندوز
    ''' </summary>
    Public Function GetRegValue() As String
        Dim regKey As RegistryKey = Nothing
        Try
            ' فتح مسار البرنامج في التسجيل
            regKey = Registry.LocalMachine.OpenSubKey("Software\ESIS", True) ' فقط للقراءة

            If regKey IsNot Nothing Then
                ' الحصول على قيمة مفتاح النظام
                Dim value As Object = regKey.GetValue("System_key")
                ' إغلاق المفتاح فورًا
                regKey.Close()

                ' التحقق من أن القيمة ليست Nothing أو فارغة
                If value IsNot Nothing AndAlso Not String.IsNullOrEmpty(value.ToString()) Then
                    Return value.ToString()
                Else
                    Return ""
                End If
            Else
                ' المفتاح غير موجود، ما يعني أن البرنامج غير مفعل
                System.Diagnostics.Debug.WriteLine("مفتاح ESIS غير موجود في التسجيل")
                ' إذا كان المفتاح غير موجود، نعيد سلسلة فارغة
                Return ""
            End If

        Catch ex As SecurityException
            System.Diagnostics.Debug.WriteLine($"مشكلة في الصلاحيات عند قراءة التسجيل: {ex.Message}")
            MessageBox.Show("يحتاج البرنامج لصلاحيات مدير (Administrator) للوصول إلى بيانات التسجيل.",
                       "صلاحيات غير كافية", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في قراءة قيمة التسجيل: {ex.Message}")
            Return ""

        Finally
            ' لا حاجة لإغلاقه هنا لأنه تم إغلاقه أعلاه
            ' أو إذا لم يتم إغلاقه، نتحقق منه
            If regKey IsNot Nothing Then
                Try
                    regKey.Close()
                Catch
                    ' تجاهل أي خطأ في الإغلاق
                End Try
            End If
        End Try
    End Function

    '''' <summary>
    '''' التحقق من صحة مفتاح التفعيل
    '''' </summary>
    'Public Function CheckActivationKey(inputKey As String, storedKey As String) As Boolean
    '    Try
    '        ' التحقق من أن المفاتيح ليست فارغة
    '        If String.IsNullOrWhiteSpace(inputKey) OrElse String.IsNullOrWhiteSpace(storedKey) Then
    '            Return False
    '        End If

    '        ' المقارنة الحساسة لحالة الأحرف
    '        Return inputKey.Trim() = storedKey.Trim()

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من مفتاح التفعيل: {ex.Message}")
    '        Return False
    '    End Try
    'End Function
    '#End Region






    '    Public Function WriteValue(key As String) As Boolean
    '        Dim regKey As RegistryKey
    '        Try
    '            regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
    '            regKey = Registry.LocalMachine.OpenSubKey("Software\ESIS", True)

    '            If regKey Is Nothing Then

    '                regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
    '                regKey.CreateSubKey("ESIS")

    '                regKey = Registry.LocalMachine.OpenSubKey("Software\ESIS", True)
    '                regKey.SetValue("System_key", key)

    '                regKey.Close()
    '            End If
    '            Return True
    '        Catch ex As Exception
    '            Return False
    '        End Try
    '    End Function

    '    Public Function GetRegValue() As String

    '        Try
    '            Dim regKey As RegistryKey
    '            Dim var As String
    '            regKey = Registry.LocalMachine.OpenSubKey("Software\ESIS", True)
    '            var = regKey.GetValue("System_key")
    '            Return var
    '        Catch ex As Exception
    '            Return ""
    '        End Try

    '    End Function
#End Region

    'Public Sub OpenMdiForm(Of T As {Form, New})(ByVal mdiParent As Form)
    '    Dim existingForm As T = mdiParent.MdiChildren.OfType(Of T)().FirstOrDefault()

    '    If existingForm IsNot Nothing Then
    '        existingForm.BringToFront()
    '        existingForm.Activate()
    '    Else
    '        Dim newForm As New T()
    '        newForm.MdiParent = mdiParent
    '        newForm.Show()
    '    End If
    'End Sub

    ''' <summary>
    ''' فتح نموذج كنافذة فرعية (MDI Child)
    ''' </summary>
    Public Sub OpenMdiForm(Of T As Form)(createInstance As Func(Of T), Optional ByVal mdiParent As Form = Nothing)
        Try
            ' إذا لم يتم تمرير النموذج الرئيسي، حاول العثور عليه تلقائيًا
            If mdiParent Is Nothing Then
                ' البحث عن النموذج الرئيسي (عادةً ما يكون النموذج الأول في قائمة النماذج المفتوحة)
                For Each form As Form In Application.OpenForms
                    If form.IsMdiContainer Then
                        mdiParent = form
                        Exit For
                    End If
                Next

                ' إذا لم يتم العثور على نموذج رئيسي، حاول استخدام النموذج الرئيسي المعروف
                If mdiParent Is Nothing Then
                    For Each form As Form In Application.OpenForms
                        If TypeOf form Is MainForm Then
                            mdiParent = form
                            Exit For
                        End If
                    Next
                End If
            End If

            ' إذا لم يتم العثور على النموذج الرئيسي بعد، أظهر خطأ
            If mdiParent Is Nothing Then
                MessageBox.Show("تعذر العثور على النموذج الرئيسي (MdiContainer).", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' البحث عن النموذج المطلوب في النماذج الفرعية
            Dim existingForm As T = Nothing
            For Each child As Form In mdiParent.MdiChildren
                If TypeOf child Is T Then
                    existingForm = DirectCast(child, T)
                    Exit For
                End If
            Next

            If existingForm IsNot Nothing Then
                ' إذا كان النموذج موجودًا، جلبه إلى المقدمة
                existingForm.BringToFront()
                existingForm.Activate()
            Else
                ' إذا لم يكن النموذج موجودًا، إنشاء نموذج جديد باستخدام الدالة الممررة
                Dim newForm As T = createInstance()
                newForm.MdiParent = mdiParent
                newForm.WindowState = FormWindowState.Maximized
                newForm.Show()
            End If
        Catch ex As Exception
            MessageBox.Show($"خطأ في فتح النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Sub OpenMdiForm(Of T As {Form, New})(Optional ByVal mdiParent As Form = Nothing)
        ' إذا لم يتم تمرير النموذج الرئيسي، حاول العثور عليه تلقائيًا
        If mdiParent Is Nothing Then
            ' البحث عن النموذج الرئيسي (عادةً ما يكون النموذج الأول في قائمة النماذج المفتوحة)
            For Each form As Form In Application.OpenForms
                If form.IsMdiContainer Then
                    mdiParent = form
                    Exit For
                End If
            Next

            ' إذا لم يتم العثور على نموذج رئيسي، حاول استخدام النموذج الرئيسي المعروف (مثل MainFrm)
            If mdiParent Is Nothing Then
                For Each form As Form In Application.OpenForms
                    If TypeOf form Is MainForm Then ' استبدل MainFrm باسم نموذجك الرئيسي
                        mdiParent = form
                        Exit For
                    End If
                Next
            End If
        End If

        ' إذا لم يتم العثور على النموذج الرئيسي بعد، أظهر خطأ
        If mdiParent Is Nothing Then
            MessageBox.Show("تعذر العثور على النموذج الرئيسي (MdiContainer).", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' البحث عن النموذج المطلوب في النماذج الفرعية
        Dim existingForm As T = mdiParent.MdiChildren.OfType(Of T)().FirstOrDefault()

        If existingForm IsNot Nothing Then
            ' إذا كان النموذج موجودًا، جلبه إلى المقدمة
            existingForm.BringToFront()
            existingForm.Activate()
        Else
            ' إذا لم يكن النموذج موجودًا، إنشاء نموذج جديد
            Dim newForm As New T()
            newForm.MdiParent = mdiParent
            newForm.Show()

            '' محاولة تعيين خصائص إضافية إذا لزم الأمر
            'Try
            '    ' إذا كان النموذج الجديد يحتوي على خصائص محددة يمكن تعيينها
            '    Dim formWithProperties As IHasAdditionalProperties = TryCast(newForm, IHasAdditionalProperties)
            '    If formWithProperties IsNot Nothing Then
            '        formWithProperties.SomeProperty = "قيمة محددة"
            '    End If
            'Catch
            '    ' تجاهل الأخطاء الطفيفة
            'End Try
        End If
    End Sub

    ''' <summary>
    ''' واجهة لتمكين تمرير البيانات عند فتح النماذج
    ''' </summary>
    Public Interface IFormWithParameters
        Sub InitializeWithData(data As Object)
    End Interface

#Region "دوال خاصة بالتاب كنترول "
    'Public Sub OpenOrSelectForm(ByVal formType As Type, ByVal formName As String, ByVal tabControl As TabControl)

    '    '    مثال استخدام الدالة
    '    '1. مع نموذج Frm_Sader_2025:
    '    'OpenOrSelectForm(GetType(Frm_Sader_2025), "Frm_Sader_2025", TabControlForms)

    '    '2. مع نموذج آخر:
    '    '    إذا كان لديك نموذج آخر يسمى OtherForm وتريد فتحه

    '    'OpenOrSelectForm(GetType(OtherForm), "OtherForm", TabControlForms)




    '    Try
    '        ' البحث عن النموذج إذا كان مفتوحًا بالفعل
    '        Dim frm As Form = CType(Application.OpenForms(formName), Form)
    '        If frm Is Nothing Then
    '            ' إذا لم يكن النموذج مفتوحًا، قم بإنشاء مثيل جديد منه
    '            frm = CType(Activator.CreateInstance(formType), Form)
    '            frm.Name = formName ' تعيين اسم النموذج

    '            ' إضافة النموذج إلى TabControl
    '            tabControl.TabPages.Add(frm)
    '        Else
    '            ' إذا كان النموذج مفتوحًا بالفعل، قم بتحديده في TabControl
    '            tabControl.TabPages(frm).Select()
    '        End If
    '    Catch ex As Exception
    '        ' معالجة الخطأ وإظهار رسالة للمستخدم
    '        MsgBox(ex.Message.ToString())
    '    End Try
    'End Sub

    'Public Sub OpenOrSelectForm(ByVal formType As Type, ByVal formName As String, ByVal tabControl As TabControl,
    '                            Optional ByVal formText As String = "", Optional ByVal formTag As Object = Nothing)

    '    '        مثال استخدام الدالة
    '    '    1. مع نموذج CustomSearch:
    '    '    OpenOrSelectForm(GetType(CustomSearch), "CustomSearch", TabControlForms, "البحث عن الملفات داخل جهاز الكمبيوتر", 1)

    '    '2. مع نموذج آخر:
    '    '    إذا كان لديك نموذج آخر يسمى OtherForm وتريد فتحه
    '    '    OpenOrSelectForm(GetType(OtherForm), "OtherForm", TabControlForms, "عنوان النموذج الآخر")


    '    Try
    '        ' البحث عن النموذج إذا كان مفتوحًا بالفعل
    '        Dim frm As Form = CType(Application.OpenForms(formName), Form)
    '        If frm Is Nothing Then
    '            ' إذا لم يكن النموذج مفتوحًا، قم بإنشاء مثيل جديد منه
    '            frm = CType(Activator.CreateInstance(formType), Form)
    '            frm.Name = formName ' تعيين اسم النموذج

    '            ' تعيين خصائص النموذج إذا تم تمريرها
    '            If Not String.IsNullOrEmpty(formText) Then
    '                frm.Text = formText
    '            End If
    '            If formTag IsNot Nothing Then
    '                frm.Tag = formTag
    '            End If

    '            ' إضافة النموذج إلى TabControl
    '            tabControl.TabPages.Add(frm)
    '        Else
    '            ' إذا كان النموذج مفتوحًا بالفعل، قم بتحديده في TabControl
    '            tabControl.TabPages(frm).Select()
    '        End If
    '    Catch ex As Exception
    '        ' معالجة الخطأ (يمكن تسجيل الخطأ أو إظهار رسالة للمستخدم)
    '        ' في هذا المثال، نقوم بإرجاع دون فعل أي شيء
    '        Return
    '    End Try
    'End Sub

    'Public Sub OpenOrSelectForm(ByVal formType As Type, ByVal formName As String, ByVal tabControl As MdiTabControl.TabControl)

    '    Try
    '        ' البحث عن علامة تبويب تحمل نفس اسم النموذج
    '        Dim existingPage As MdiTabControl.TabPage = Nothing
    '        For Each page As MdiTabControl.TabPage In tabControl.TabPages
    '            If page.Form.Name = formName Then
    '                existingPage = page
    '                Exit For
    '            End If
    '        Next

    '        If existingPage Is Nothing Then
    '            ' إذا لم يكن النموذج مفتوحًا، قم بإنشاء مثيل جديد منه
    '            Dim frm As Form = CType(Activator.CreateInstance(formType), Form)
    '            frm.Name = formName ' تعيين اسم النموذج

    '            ' إضافة النموذج باستخدام الدالة الخاصة بالمكتبة
    '            tabControl.TabPages.Add(frm)
    '        Else
    '            ' إذا كان النموذج مفتوحًا بالفعل، قم بتحديد علامة التبويب الخاصة به
    '            'tabControl.SelectedTab = existingPage
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.Message.ToString())
    '    End Try
    'End Sub

#End Region

    '''' <summary>
    '''' إغلاق جميع WebBrowser في جميع النماذج المفتوحة التي تحتوي على ملف محدد
    '''' </summary>
    '''' <param name="filePath">مسار الملف الذي يجب إغلاقه</param>
    'Public Sub CloseFileInAllWebBrowsers(filePath As String)
    '    Try
    '        ' البحث عن جميع النماذج المفتوحة
    '        For Each form As Form In Application.OpenForms
    '            ' البحث عن جميع عناصر التحكم في النموذج
    '            For Each control As Control In form.Controls
    '                If TypeOf control Is WebBrowser Then
    '                    Dim webBrowser As WebBrowser = DirectCast(control, WebBrowser)

    '                    ' التحقق من أن الملف المفتوح هو نفس الملف الذي نريد إغلاقه
    '                    If Not String.IsNullOrEmpty(webBrowser.Url?.LocalPath) AndAlso
    '                   String.Equals(webBrowser.Url.LocalPath, filePath, StringComparison.OrdinalIgnoreCase) Then
    '                        ' إغلاق الملف
    '                        webBrowser.Navigate("about:blank")
    '                        Application.DoEvents()
    '                        Thread.Sleep(200)

    '                        ' تفريغ الذاكرة
    '                        webBrowser.Dispose()
    '                    End If
    '                End If
    '            Next
    '        Next

    '        ' تفريغ جمع القمامة
    '        GC.Collect()
    '        GC.WaitForPendingFinalizers()
    '        Thread.Sleep(500)

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في إغلاق WebBrowser: {ex.Message}")
    '    End Try
    'End Sub

    ''' <summary>
    ''' إغلاق جميع WebBrowser في جميع النماذج المفتوحة التي تحتوي على ملف محدد
    ''' </summary>
    ''' <param name="filePath">مسار الملف الذي يجب إغلاقه</param>
    Public Sub CloseFileInAllWebBrowsers(filePath As String)
        Try
            ' البحث عن جميع النماذج المفتوحة
            For Each form As Form In Application.OpenForms
                ' البحث عن جميع عناصر التحكم في النموذج (بما في ذلك العناصر الفرعية)
                SearchControlsInForm(form, filePath)
            Next

            ' تفريغ جمع القمامة
            GC.Collect()
            GC.WaitForPendingFinalizers()
            Thread.Sleep(500)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إغلاق WebBrowser: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' البحث عن جميع عناصر التحكم في نموذج معين وإغلاق WebBrowser إذا كان يعرض الملف المحدد
    ''' </summary>
    ''' <param name="control">عنصر التحكم الحالي</param>
    ''' <param name="filePath">مسار الملف</param>
    Private Sub SearchControlsInForm(control As Control, filePath As String)
        Try
            ' التحقق من أن العنصر ليس فارغًا
            If control Is Nothing Then Return

            ' التحقق مما إذا كان العنصر هو WebBrowser
            If TypeOf control Is WebBrowser Then
                Dim webBrowser As WebBrowser = DirectCast(control, WebBrowser)

                ' التحقق من أن الملف المفتوح هو نفس الملف الذي نريد إغلاقه
                If Not String.IsNullOrEmpty(webBrowser.Url?.LocalPath) AndAlso
               String.Equals(webBrowser.Url.LocalPath, filePath, StringComparison.OrdinalIgnoreCase) Then
                    ' إغلاق الملف
                    webBrowser.Navigate("about:blank")
                    Application.DoEvents()
                    Thread.Sleep(200)

                    '' تفريغ الذاكرة
                    'webBrowser.Dispose()
                End If
            End If

            ' البحث في العناصر الفرعية إذا كان العنصر يحتوي على عناصر تحكم
            If control.HasChildren Then
                For Each childControl As Control In control.Controls
                    SearchControlsInForm(childControl, filePath)
                Next
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في SearchControlsInForm: {ex.Message}")
        End Try
    End Sub


    ''' <summary>
    ''' إجبار إغلاق مقابض الملف (لإنهاء أي عملية تستخدم الملف)
    ''' </summary>
    ''' <param name="filePath">مسار الملف</param>
    Public Sub ForceCloseFileHandles(filePath As String)
        Try
            ' المحاولة الأولى: استخدام GC و إغلاق الذاكرة
            GC.Collect()
            GC.WaitForPendingFinalizers()
            Thread.Sleep(500)

            ' المحاولة الثانية: محاولة فتح الملف بوضع حصري
            Try
                Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None)
                    ' إذا تم فتح الملف بنجاح، فهو غير مستخدم
                End Using
            Catch ex As IOException
                ' إذا فشل الفتح، حاول إغلاق الملف يدويًا
                Try
                    Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                        fs.Close()
                        fs.Dispose()
                    End Using
                Catch ex2 As Exception
                    System.Diagnostics.Debug.WriteLine($"فشل في إغلاق الملف: {ex2.Message}")
                End Try
            End Try
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في ForceCloseFileHandles: {ex.Message}")
        End Try
    End Sub

    'دالة عامة للتاكد من وجود البوليصة الفرعية
    Public Function CheckPolicyExists(ByVal tableName As String, ByVal fieldName As String, ByVal fieldValue As String, ByVal lblBills As Label, ByVal txtWillSub As TextBox, ByVal cmbBookCategory As ComboBox, ByVal ComboSubjBook As ComboBox) As Boolean
        ' استعلام SQL للتحقق من وجود البوليصة في قاعدة البيانات
        Dim query As String = $"SELECT * FROM {tableName} WHERE {fieldName} = @{fieldName}"

        Using adapter As New SqlDataAdapter(query, GetConnection())
            adapter.SelectCommand.Parameters.AddWithValue($"@{fieldName}", fieldValue)

            Dim dataSet As New DataSet()
            adapter.Fill(dataSet)
            Dim dataTable = dataSet.Tables(0)

            ' إذا كانت البوليصة موجودة مسبقًا
            If dataTable.Rows.Count > 0 Then
                Dim message As String = "البوليصة الرئيسية رقم " & vbNewLine & vbNewLine & fieldValue & vbNewLine & vbNewLine & "موجودة مسبقًا. هل ترغب في المواصلة؟"
                If MessageBox.Show(message, "تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                    Return False
                End If

                ' السؤال عن وجود بوليصة فرعية
                If MessageBox.Show("هل توجد بوليصة فرعية؟", "تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                    lblBills.Text = "رقم البوليصة موجود مسبقًا"
                    lblBills.ForeColor = Color.Red
                    cmbBookCategory.DroppedDown = True
                    ComboSubjBook.DroppedDown = True

                    Return True
                Else
                    txtWillSub.Focus()
                    Return True
                End If
            Else
                ' إذا كانت البوليصة غير موجودة
                lblBills.Text = "رقم البوليصة غير موجود"
                lblBills.ForeColor = Color.Green

                ' السؤال عن وجود بوليصة فرعية
                If MessageBox.Show("هل توجد بوليصة فرعية؟", "تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then

                    cmbBookCategory.DroppedDown = True
                    cmbBookCategory.Focus()
                    Return True
                Else
                    txtWillSub.Focus()
                    Return True
                End If
            End If
        End Using
    End Function

    'دالة عامة للتاكد من وجود البوليصة الرئيسية
    'Public Function CheckPolicyExists(ByVal tableName As String, ByVal fieldName As String, ByVal fieldValue As String, ByVal lblBills As Label, ByVal txtWillMaster As TextBox, ByVal txtWillSub As TextBox, ByVal ComboSubjBook As ComboBox) As Boolean
    '    ' استعلام SQL للتحقق من وجود البوليصة في قاعدة البيانات
    '    Dim query As String = $"SELECT * FROM {tableName} WHERE {fieldName} = @{fieldName}"

    '    Using adapter As New SqlDataAdapter(query, GetConnection())
    '        adapter.SelectCommand.Parameters.AddWithValue($"@{fieldName}", fieldValue)

    '        Dim dataSet As New DataSet()
    '        adapter.Fill(dataSet)
    '        Dim dataTable = dataSet.Tables(0)

    '        ' التحقق من وجود بيانات في النتيجة
    '        If dataTable.Rows.Count > 0 Then
    '            Dim message As String = "البوليصة الفرعية رقم " & vbNewLine & vbNewLine & fieldValue & vbNewLine & vbNewLine & "موجودة مسبقًا. هل ترغب في المواصلة؟"
    '            If MessageBox.Show(message, "تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
    '                Return False
    '            End If




    '            ' السؤال عن وجود بوليصة رئيسية
    '            If MessageBox.Show("هل توجد بوليصة رئيسية؟", "تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
    '                lblBills.Text = "رقم البوليصة موجود مسبقًا"
    '                lblBills.ForeColor = Color.Red
    '                txtWillMaster.Focus()

    '                Return True
    '            Else
    '                txtWillMaster.Focus()
    '                Return True
    '            End If

    '        ElseIf String.IsNullOrEmpty(txtWillMaster.Text) Then

    '            ' سؤال المستخدم إذا كانت هناك بوليصة رئيسية
    '            If MessageBox.Show("هل توجد بوليصة رئيسية؟", "تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
    '                ' إذا لم تكن هناك بوليصة رئيسية، تفعيل ComboBox
    '                ComboSubjBook.DroppedDown = True
    '                'EnableComboBox()
    '                Return True

    '            Else
    '                ' إذا كانت هناك بوليصة رئيسية، تفعيل CheckBox "cbBills_Master" والانتقال إلى الحقل التالي
    '                txtWillMaster.Focus()
    '                Return True
    '            End If

    '            ' إذا لم تكن البوليصة موجودة في قاعدة البيانات
    '            lblBills.Text = "رقم البوليصة غير موجود"
    '            lblBills.ForeColor = Color.Green

    '        Else
    '            ' إذا لم تكن هناك بوليصة رئيسية، تفعيل ComboBox
    '            ComboSubjBook.DroppedDown = True
    '            Return False
    '        End If

    '    End Using
    'End Function

    Public Function CheckSubPolicyExists(
    ByVal tableName As String,
    ByVal fieldName As String,
    ByVal fieldValue As String,
    ByVal lblBills As Label,
    ByVal txtWillSub As TextBox,
    ByVal ComboSubjBook As ComboBox) As Boolean

        ' 1. التحقق من أن حقل البوليصة الفرعية فارغ
        If String.IsNullOrEmpty(txtWillSub.Text.Trim()) Then
            ' سؤال المستخدم عن وجود بوليصة فرعية
            If Not AskUserAboutSubPolicy() Then
                ComboSubjBook.DroppedDown = True
                Return False
            Else
                txtWillSub.Focus()
                Return True
            End If
        End If

        ' 2. استعلام SQL للتحقق من وجود البوليصة الفرعية
        Dim query As String = $"SELECT * FROM {tableName} WHERE {fieldName} = @{fieldName}"

        Using adapter As New SqlDataAdapter(query, GetConnection())
            adapter.SelectCommand.Parameters.AddWithValue($"@{fieldName}", fieldValue)

            Dim dataSet As New DataSet()
            adapter.Fill(dataSet)
            Dim dataTable = dataSet.Tables(0)

            ' 3. إذا كانت البوليصة موجودة مسبقًا
            If dataTable.Rows.Count > 0 Then
                Dim message As String = $"البوليصة الفرعية رقم {fieldValue} موجودة مسبقًا. هل ترغب في المواصلة؟"
                If MessageBox.Show(message, "تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                    Return False
                End If

                ' التحقق من وجود بوليصة فرعية أخرى
                If Not AskUserAboutSubPolicy() Then
                    lblBills.Text = "رقم البوليصة موجود مسبقًا"
                    lblBills.ForeColor = Color.Red
                    ComboSubjBook.DroppedDown = True
                    Return True
                Else
                    txtWillSub.Focus()
                    Return True
                End If
            Else
                ' 4. إذا كانت البوليصة غير موجودة
                lblBills.Text = "رقم البوليصة غير موجود"
                lblBills.ForeColor = Color.Green

                ' التحقق من وجود بوليصة فرعية
                If Not AskUserAboutSubPolicy() Then
                    ComboSubjBook.DroppedDown = True
                    Return True
                Else
                    txtWillSub.Focus()
                    Return True
                End If
            End If
        End Using
    End Function

    'دالة مساعدة لسؤال المستخدم عن وجود بوليصة فرعية
    Private Function AskUserAboutSubPolicy() As Boolean
        Return MessageBox.Show(
        "هل توجد بوليصة فرعية؟",
        "تنبيه",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button2) = DialogResult.Yes
    End Function

    'Public Function CheckPolicyExists(ByVal tableName As String, ByVal fieldName As String, ByVal fieldValue As String, ByVal lblBills As Label, ByVal txtWillMaster As TextBox, ByVal txtWillSub As TextBox, ByVal ComboSubjBook As ComboBox) As Boolean
    '    ' التحقق من أن حقل txtWillMaster فارغ
    '    If String.IsNullOrEmpty(txtWillMaster.Text.Trim()) Then
    '        ' سؤال المستخدم إذا كانت هناك بوليصة رئيسية
    '        If Not AskUserAboutPrimaryPolicy() Then
    '            ' إذا لم تكن هناك بوليصة رئيسية، تفعيل ComboBox
    '            ComboSubjBook.DroppedDown = True
    '            Return False
    '        Else
    '            ' إذا كانت هناك بوليصة رئيسية، تفعيل التركيز على txtWillMaster
    '            txtWillMaster.Focus()
    '            Return True
    '        End If
    '    End If

    '    ' استعلام SQL للتحقق من وجود البوليصة في قاعدة البيانات
    '    Dim query As String = $"SELECT * FROM {tableName} WHERE {fieldName} = @{fieldName}"

    '    Using adapter As New SqlDataAdapter(query, GetConnection())
    '        adapter.SelectCommand.Parameters.AddWithValue($"@{fieldName}", fieldValue)

    '        Dim dataSet As New DataSet()
    '        adapter.Fill(dataSet)
    '        Dim dataTable = dataSet.Tables(0)

    '        ' التحقق من وجود بيانات في النتيجة
    '        If dataTable.Rows.Count > 0 Then
    '            ' إذا كانت البوليصة موجودة مسبقًا
    '            Dim message As String = "البوليصة الفرعية رقم " & vbNewLine & vbNewLine & fieldValue & vbNewLine & vbNewLine & "موجودة مسبقًا. هل ترغب في المواصلة؟"
    '            If MessageBox.Show(message, "تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
    '                Return False
    '            End If

    '            ' التحقق من وجود بوليصة رئيسية
    '            If Not AskUserAboutPrimaryPolicy() Then
    '                lblBills.Text = "رقم البوليصة موجود مسبقًا"
    '                lblBills.ForeColor = Color.Red
    '                txtWillMaster.Focus()
    '                Return True
    '            Else
    '                txtWillMaster.Focus()
    '                Return True
    '            End If
    '        Else
    '            ' إذا لم تكن البوليصة موجودة في قاعدة البيانات
    '            lblBills.Text = "رقم البوليصة غير موجود"
    '            lblBills.ForeColor = Color.Green

    '            ' التحقق من وجود بوليصة رئيسية
    '            If Not AskUserAboutPrimaryPolicy() Then
    '                ComboSubjBook.DroppedDown = True
    '                Return False
    '            Else
    '                txtWillMaster.Focus()

    '                Return True
    '            End If
    '        End If
    '    End Using
    'End Function

    '' دالة مساعدة لطرح السؤال حول وجود بوليصة رئيسية
    'Private Function AskUserAboutPrimaryPolicy() As Boolean
    '    Return MessageBox.Show("هل توجد بوليصة رئيسية؟", "تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes
    'End Function

#Region "حساب التاريخ"

    '''' <summary>
    '''' حساب الفرق بين تاريخين بدقة مع حساب أيام العمل والعطل
    '''' </summary>
    'Public Sub CalculateDateDifference(ByVal startDate As DateTime, ByVal endDate As DateTime,
    '                             ByRef years As Integer, ByRef months As Integer, ByRef days As Integer,
    '                             ByRef totalDays As Integer, ByRef workingDays As Integer,
    '                             ByRef fridayCount As Integer, ByRef saturdayCount As Integer,
    '                             ByRef holidayCount As Integer)
    '    Try
    '        ' تأكد من أن التواريخ مرتبة بشكل صحيح
    '        Dim startDt As DateTime = If(startDate <= endDate, startDate, endDate)
    '        Dim endDt As DateTime = If(startDate <= endDate, endDate, startDate)

    '        ' حساب الفرق بالسنوات
    '        years = endDt.Year - startDt.Year
    '        months = endDt.Month - startDt.Month
    '        days = endDt.Day - startDt.Day

    '        ' التصحيحات للفرق
    '        If days < 0 Then
    '            months -= 1
    '            days += DateTime.DaysInMonth(startDt.Year, startDt.Month)
    '        End If

    '        If months < 0 Then
    '            years -= 1
    '            months += 12
    '        End If

    '        ' ✅ الحساب الصحيح لإجمالي الأيام (بما في ذلك اليوم الأول واليوم الأخير)
    '        totalDays = (endDt - startDt).Days + 1

    '        ' تهيئة العدادات
    '        fridayCount = 0
    '        saturdayCount = 0
    '        workingDays = 0

    '        ' حساب أيام العطل وعمل بشكل دقيق
    '        Dim currentDate As DateTime = startDt.Date
    '        While currentDate <= endDt.Date
    '            Select Case currentDate.DayOfWeek
    '                Case DayOfWeek.Friday
    '                    fridayCount += 1
    '                Case DayOfWeek.Saturday
    '                    saturdayCount += 1
    '                Case Else
    '                    workingDays += 1
    '            End Select
    '            currentDate = currentDate.AddDays(1)
    '        End While

    '        ' ✅ الحساب الصحيح لأيام العطل
    '        holidayCount = fridayCount + saturdayCount

    '        ' التحقق من أن المجموع صحيح
    '        If workingDays + holidayCount <> totalDays Then
    '            ' تصحيح بسيط في حالة وجود اختلاف (نادر الحدوث)
    '            If workingDays + holidayCount < totalDays Then
    '                workingDays = totalDays - holidayCount
    '            End If
    '        End If

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في حساب الفرق بين التواريخ: {ex.Message}")
    '        ' إعادة تعيين القيم في حالة الخطأ
    '        years = 0
    '        months = 0
    '        days = 0
    '        totalDays = 0
    '        workingDays = 0
    '        fridayCount = 0
    '        saturdayCount = 0
    '        holidayCount = 0
    '    End Try
    'End Sub

    '''' <summary>
    '''' عرض نتيجة حساب الفرق بين التاريخين في Labels
    '''' </summary>
    'Public Sub ShowDateDifference(ByVal startDate As DateTime, ByVal endDate As DateTime,
    '                        ByVal lblYears As Label, ByVal lblMonths As Label, ByVal lblDays As Label,
    '                        ByVal lblTotalDays As Label, ByVal lblFridays As Label,
    '                        ByVal lblSaturdays As Label, ByVal lblHolidays As Label)
    '    Try
    '        ' تعريف المتغيرات لتخزين النتائج
    '        Dim years As Integer = 0
    '        Dim months As Integer = 0
    '        Dim days As Integer = 0
    '        Dim totalDays As Integer = 0
    '        Dim workingDays As Integer = 0
    '        Dim fridayCount As Integer = 0
    '        Dim saturdayCount As Integer = 0
    '        Dim holidayCount As Integer = 0

    '        ' حساب الفرق بين التاريخين مع عطل الأسبوع
    '        CalculateDateDifference(startDate, endDate, years, months, days, totalDays,
    '                          workingDays, fridayCount, saturdayCount, holidayCount)

    '        ' عرض النتائج في Labels
    '        lblYears.Text = "عدد السنوات: " & years
    '        lblMonths.Text = "عدد الأشهر: " & months
    '        lblDays.Text = "عدد الأيام: " & days

    '        ' ✅ عرض صافي الأيام بدون الجمعة والسبت في lblTotalDays
    '        lblTotalDays.Text = "صافي الأيام (بدون عطلة): " & workingDays

    '        ' ✅ عرض الإجمالي الكلي مع الجمعة والسبت في lblHolidays
    '        lblHolidays.Text = "الإجمالي مع العطل: " & totalDays

    '        ' عرض نتائج عطل الأسبوع
    '        lblFridays.Text = "أيام الجمعة: " & fridayCount
    '        lblSaturdays.Text = "أيام السبت: " & saturdayCount

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في عرض نتيجة حساب الفرق: {ex.Message}")
    '        ' تعيين قيم افتراضية في حالة الخطأ
    '        If lblYears IsNot Nothing Then lblYears.Text = "عدد السنوات: خطأ"
    '        If lblMonths IsNot Nothing Then lblMonths.Text = "عدد الأشهر: خطأ"
    '        If lblDays IsNot Nothing Then lblDays.Text = "عدد الأيام: خطأ"
    '        If lblTotalDays IsNot Nothing Then lblTotalDays.Text = "صافي الأيام: خطأ"
    '        If lblHolidays IsNot Nothing Then lblHolidays.Text = "الإجمالي مع العطل: خطأ"
    '        If lblFridays IsNot Nothing Then lblFridays.Text = "أيام الجمعة: خطأ"
    '        If lblSaturdays IsNot Nothing Then lblSaturdays.Text = "أيام السبت: خطأ"
    '    End Try
    'End Sub

    '''' <summary>
    '''' حساب الفرق بين تاريخين بالسنوات والأشهر والأيام بدقة مع حساب أيام الجمعة والسبت والعطل
    '''' </summary>
    'Public Sub CalculateDateDifference(ByVal startDate As DateTime, ByVal endDate As DateTime,
    '                             ByRef years As Integer, ByRef months As Integer, ByRef days As Integer,
    '                             ByRef totalDays As Integer, ByRef fridayCount As Integer,
    '                             ByRef saturdayCount As Integer, ByRef holidayCount As Integer)
    '    Try
    '        ' تأكد من أن التواريخ مرتبة بشكل صحيح
    '        If startDate > endDate Then
    '            Dim temp As DateTime = startDate
    '            startDate = endDate
    '            endDate = temp
    '        End If

    '        ' حساب الفرق بالسنوات
    '        years = endDate.Year - startDate.Year

    '        ' حساب الفرق بالأشهر
    '        months = endDate.Month - startDate.Month

    '        ' إذا كان شهر النهاية أقل من شهر البداية، نقوم بتعديل السنوات والأشهر
    '        If months < 0 Then
    '            years -= 1
    '            months += 12
    '        End If

    '        ' حساب الفرق بالأيام
    '        days = endDate.Day - startDate.Day

    '        ' إذا كان يوم النهاية أقل من يوم البداية، نقوم بتعديل الأشهر والأيام
    '        If days < 0 Then
    '            months -= 1
    '            days += DateTime.DaysInMonth(startDate.Year, startDate.Month)
    '        End If

    '        ' إذا كانت الأشهر سالبة بعد التعديل، نقوم بتعديل السنوات والأشهر
    '        If months < 0 Then
    '            years -= 1
    '            months += 12
    '        End If

    '        ' حساب إجمالي الفرق بالأيام
    '        totalDays = (endDate - startDate).Days

    '        ' تهيئة عدادات أيام العطل
    '        fridayCount = 0
    '        saturdayCount = 0
    '        holidayCount = 0

    '        ' حساب أيام الجمعة والسبت في الفترة
    '        Dim currentDate As DateTime = startDate.Date
    '        While currentDate <= endDate.Date
    '            Select Case currentDate.DayOfWeek
    '                Case DayOfWeek.Friday
    '                    fridayCount += 1
    '                Case DayOfWeek.Saturday
    '                    saturdayCount += 1
    '            End Select
    '            currentDate = currentDate.AddDays(1)
    '        End While

    '        ' حساب إجمالي أيام العطل (الجمعة + السبت)
    '        holidayCount = fridayCount + saturdayCount

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في حساب الفرق بين التواريخ: {ex.Message}")
    '        ' إعادة تعيين القيم في حالة الخطأ
    '        years = 0
    '        months = 0
    '        days = 0
    '        totalDays = 0
    '        fridayCount = 0
    '        saturdayCount = 0
    '        holidayCount = 0
    '    End Try
    'End Sub

    '''' <summary>
    '''' عرض نتيجة حساب الفرق بين التاريخين في Labels
    '''' </summary>
    'Public Sub ShowDateDifference(ByVal startDate As DateTime, ByVal endDate As DateTime,
    '                        ByVal lblYears As Label, ByVal lblMonths As Label, ByVal lblDays As Label,
    '                        ByVal lblTotalDays As Label, ByVal lblFridays As Label,
    '                        ByVal lblSaturdays As Label, ByVal lblHolidays As Label)
    '    Try
    '        ' تعريف المتغيرات لتخزين النتائج
    '        Dim years As Integer = 0
    '        Dim months As Integer = 0
    '        Dim days As Integer = 0
    '        Dim totalDays As Integer = 0
    '        Dim fridayCount As Integer = 0
    '        Dim saturdayCount As Integer = 0
    '        Dim holidayCount As Integer = 0

    '        ' حساب الفرق بين التاريخين مع عطل الأسبوع
    '        CalculateDateDifference(startDate, endDate, years, months, days, totalDays,
    '                          fridayCount, saturdayCount, holidayCount)

    '        ' عرض النتائج في Labels
    '        lblYears.Text = "عدد السنوات: " & years
    '        lblMonths.Text = "عدد الأشهر: " & months
    '        lblDays.Text = "عدد الأيام: " & days
    '        lblTotalDays.Text = "الإجمالي بالأيام: " & totalDays

    '        ' عرض نتائج عطل الأسبوع
    '        lblFridays.Text = "أيام الجمعة: " & fridayCount
    '        lblSaturdays.Text = "أيام السبت: " & saturdayCount
    '        lblHolidays.Text = "إجمالي العطل: " & holidayCount

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في عرض نتيجة حساب الفرق: {ex.Message}")
    '        ' تعيين قيم افتراضية في حالة الخطأ
    '        If lblYears IsNot Nothing Then lblYears.Text = "عدد السنوات: خطأ"
    '        If lblMonths IsNot Nothing Then lblMonths.Text = "عدد الأشهر: خطأ"
    '        If lblDays IsNot Nothing Then lblDays.Text = "عدد الأيام: خطأ"
    '        If lblTotalDays IsNot Nothing Then lblTotalDays.Text = "الإجمالي بالأيام: خطأ"
    '        If lblFridays IsNot Nothing Then lblFridays.Text = "أيام الجمعة: خطأ"
    '        If lblSaturdays IsNot Nothing Then lblSaturdays.Text = "أيام السبت: خطأ"
    '        If lblHolidays IsNot Nothing Then lblHolidays.Text = "إجمالي العطل: خطأ"
    '    End Try
    'End Sub


    '' دالة لحساب الفرق بين تاريخين بالسنوات والأشهر والأيام بدقة
    'Public Sub CalculateDateDifference(ByVal startDate As DateTime, ByVal endDate As DateTime, ByRef years As Integer, ByRef months As Integer, ByRef days As Integer, ByRef totalDays As Integer)
    '    ' حساب الفرق بالسنوات
    '    years = endDate.Year - startDate.Year

    '    ' حساب الفرق بالأشهر
    '    months = endDate.Month - startDate.Month

    '    ' إذا كان شهر النهاية أقل من شهر البداية، نقوم بتعديل السنوات والأشهر
    '    If months < 0 Then
    '        years -= 1
    '        months += 12
    '    End If

    '    ' حساب الفرق بالأيام
    '    days = endDate.Day - startDate.Day

    '    ' إذا كان يوم النهاية أقل من يوم البداية، نقوم بتعديل الأشهر والأيام
    '    If days < 0 Then
    '        months -= 1
    '        days += DateTime.DaysInMonth(startDate.Year, startDate.Month)
    '    End If

    '    ' إذا كانت الأشهر سالبة بعد التعديل، نقوم بتعديل السنوات والأشهر
    '    If months < 0 Then
    '        years -= 1
    '        months += 12
    '    End If

    '    ' حساب إجمالي الفرق بالأيام
    '    totalDays = (endDate - startDate).Days
    'End Sub

    '' دالة لعرض النتيجة في Labels
    'Public Sub ShowDateDifference(ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal lblYears As Label, ByVal lblMonths As Label, ByVal lblDays As Label, ByVal lblTotalDays As Label)
    '    ' تعريف المتغيرات لتخزين النتائج
    '    Dim years As Integer = 0
    '    Dim months As Integer = 0
    '    Dim days As Integer = 0
    '    Dim totalDays As Integer = 0

    '    ' حساب الفرق بين التاريخين
    '    CalculateDateDifference(startDate, endDate, years, months, days, totalDays)

    '    ' عرض النتائج في Labels
    '    lblYears.Text = "عدد السنوات: " & years
    '    lblMonths.Text = "عدد الأشهر: " & months
    '    lblDays.Text = "عدد الأيام: " & days
    '    lblTotalDays.Text = "الإجمالي بالأيام: " & totalDays
    'End Sub





    '' دالة لحساب الفرق بين تاريخين بالسنوات والأشهر والأيام بدقة
    'Public Sub CalculateDateDifference(ByVal startDate As DateTime, ByVal endDate As DateTime, ByRef years As Integer, ByRef months As Integer, ByRef days As Integer)
    '    ' حساب الفرق بالسنوات
    '    years = endDate.Year - startDate.Year

    '    ' حساب الفرق بالأشهر
    '    months = endDate.Month - startDate.Month

    '    ' إذا كان شهر النهاية أقل من شهر البداية، نقوم بتعديل السنوات والأشهر
    '    If months < 0 Then
    '        years -= 1
    '        months += 12
    '    End If

    '    ' حساب الفرق بالأيام
    '    days = endDate.Day - startDate.Day

    '    ' إذا كان يوم النهاية أقل من يوم البداية، نقوم بتعديل الأشهر والأيام
    '    If days < 0 Then
    '        months -= 1
    '        days += DateTime.DaysInMonth(startDate.Year, startDate.Month)
    '    End If

    '    ' إذا كانت الأشهر سالبة بعد التعديل، نقوم بتعديل السنوات والأشهر
    '    If months < 0 Then
    '        years -= 1
    '        months += 12
    '    End If
    'End Sub

    '' دالة لعرض النتيجة في Labels
    'Public Sub ShowDateDifference(ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal lblYears As Label, ByVal lblMonths As Label, ByVal lblDays As Label)
    '    ' تعريف المتغيرات لتخزين النتائج
    '    Dim years As Integer = 0
    '    Dim months As Integer = 0
    '    Dim days As Integer = 0

    '    ' حساب الفرق بين التاريخين
    '    CalculateDateDifference(startDate, endDate, years, months, days)

    '    ' عرض النتائج في Labels
    '    lblYears.Text = "عدد السنوات: " & years
    '    lblMonths.Text = "عدد الأشهر: " & months
    '    lblDays.Text = "عدد الأيام: " & days
    'End Sub






    '' دالة لحساب الفرق بين تاريخين بالسنوات والأشهر والأيام
    'Public Sub CalculateDateDifference(ByVal startDate As DateTime, ByVal endDate As DateTime, ByRef years As Integer, ByRef months As Integer, ByRef days As Integer)
    '    ' حساب الفرق بالسنوات
    '    years = DateDiff(DateInterval.Year, startDate, endDate)

    '    ' حساب الفرق بالأشهر
    '    months = DateDiff(DateInterval.Month, startDate, endDate) Mod 12

    '    ' حساب الفرق بالأيام
    '    days = DateDiff(DateInterval.Day, startDate, endDate) Mod 30 ' يمكن تعديله ليكون أكثر دقة
    'End Sub

    '' دالة لعرض النتيجة في Labels
    'Public Sub ShowDateDifference(ByVal startDate As DateTime, ByVal endDate As DateTime, ByVal lblYears As Label, ByVal lblMonths As Label, ByVal lblDays As Label)
    '    ' تعريف المتغيرات لتخزين النتائج
    '    Dim years As Integer = 0
    '    Dim months As Integer = 0
    '    Dim days As Integer = 0

    '    ' حساب الفرق بين التاريخين
    '    CalculateDateDifference(startDate, endDate, years, months, days)

    '    ' عرض النتائج في Labels
    '    lblYears.Text = "عدد السنوات: " & years
    '    lblMonths.Text = "عدد الأشهر: " & months
    '    lblDays.Text = "عدد الأيام: " & days
    'End Sub

    '''' <summary>
    '''' حساب الفرق بين تاريخين مع حساب صافي الأيام بدون الجمعة والسبت
    '''' </summary>
    'Public Sub CalculateDateDifference(ByVal startDate As DateTime, ByVal endDate As DateTime,
    '                             ByRef years As Integer, ByRef months As Integer, ByRef days As Integer,
    '                             ByRef totalDays As Integer, ByRef workingDays As Integer,
    '                             ByRef fridayCount As Integer, ByRef saturdayCount As Integer,
    '                             ByRef holidayCount As Integer)
    '    Try
    '        ' تأكد من أن التواريخ مرتبة بشكل صحيح
    '        If startDate > endDate Then
    '            Dim temp As DateTime = startDate
    '            startDate = endDate
    '            endDate = temp
    '        End If

    '        ' حساب الفرق بالسنوات
    '        years = endDate.Year - startDate.Year

    '        ' حساب الفرق بالأشهر
    '        months = endDate.Month - startDate.Month

    '        ' إذا كان شهر النهاية أقل من شهر البداية، نقوم بتعديل السنوات والأشهر
    '        If months < 0 Then
    '            years -= 1
    '            months += 12
    '        End If

    '        ' حساب الفرق بالأيام
    '        days = endDate.Day - startDate.Day

    '        ' إذا كان يوم النهاية أقل من يوم البداية، نقوم بتعديل الأشهر والأيام
    '        If days < 0 Then
    '            months -= 1
    '            days += DateTime.DaysInMonth(startDate.Year, startDate.Month)
    '        End If

    '        ' إذا كانت الأشهر سالبة بعد التعديل، نقوم بتعديل السنوات والأشهر
    '        If months < 0 Then
    '            years -= 1
    '            months += 12
    '        End If

    '        ' حساب إجمالي الفرق بالأيام
    '        totalDays = (endDate - startDate).Days

    '        ' تهيئة عدادات أيام العطل
    '        fridayCount = 0
    '        saturdayCount = 0
    '        holidayCount = 0
    '        workingDays = 0

    '        ' حساب أيام الجمعة والسبت في الفترة
    '        Dim currentDate As DateTime = startDate.Date
    '        While currentDate <= endDate.Date
    '            Select Case currentDate.DayOfWeek
    '                Case DayOfWeek.Friday
    '                    fridayCount += 1
    '                Case DayOfWeek.Saturday
    '                    saturdayCount += 1
    '                Case Else
    '                    workingDays += 1
    '            End Select

    '            currentDate = currentDate.AddDays(1)
    '        End While

    '        ' حساب إجمالي أيام العطل (الجمعة + السبت)
    '        holidayCount = fridayCount + saturdayCount

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في حساب الفرق بين التواريخ: {ex.Message}")
    '        ' إعادة تعيين القيم في حالة الخطأ
    '        years = 0
    '        months = 0
    '        days = 0
    '        totalDays = 0
    '        workingDays = 0
    '        fridayCount = 0
    '        saturdayCount = 0
    '        holidayCount = 0
    '    End Try
    'End Sub

    '''' <summary>
    '''' عرض نتيجة حساب الفرق بين التاريخين في Labels
    '''' </summary>
    'Public Sub ShowDateDifference(ByVal startDate As DateTime, ByVal endDate As DateTime,
    '                        ByVal lblYears As Label, ByVal lblMonths As Label, ByVal lblDays As Label,
    '                        ByVal lblTotalDays As Label, ByVal lblFridays As Label,
    '                        ByVal lblSaturdays As Label, ByVal lblHolidays As Label)
    '    Try
    '        ' تعريف المتغيرات لتخزين النتائج
    '        Dim years As Integer = 0
    '        Dim months As Integer = 0
    '        Dim days As Integer = 0
    '        Dim totalDays As Integer = 0
    '        Dim workingDays As Integer = 0
    '        Dim fridayCount As Integer = 0
    '        Dim saturdayCount As Integer = 0
    '        Dim holidayCount As Integer = 0

    '        ' حساب الفرق بين التاريخين مع عطل الأسبوع
    '        CalculateDateDifference(startDate, endDate, years, months, days, totalDays,
    '                          workingDays, fridayCount, saturdayCount, holidayCount)

    '        ' عرض النتائج في Labels
    '        lblYears.Text = "عدد السنوات: " & years
    '        lblMonths.Text = "عدد الأشهر: " & months
    '        lblDays.Text = "عدد الأيام: " & days

    '        ' عرض صافي الأيام بدون الجمعة والسبت في lblTotalDays
    '        lblTotalDays.Text = "صافي الأيام (بدون عطلة): " & workingDays

    '        ' عرض الإجمالي الكلي مع الجمعة والسبت في lblHolidays
    '        lblHolidays.Text = "الإجمالي مع العطل: " & totalDays

    '        ' عرض نتائج عطل الأسبوع
    '        lblFridays.Text = "أيام الجمعة: " & fridayCount
    '        lblSaturdays.Text = "أيام السبت: " & saturdayCount

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في عرض نتيجة حساب الفرق: {ex.Message}")
    '        ' تعيين قيم افتراضية في حالة الخطأ
    '        If lblYears IsNot Nothing Then lblYears.Text = "عدد السنوات: خطأ"
    '        If lblMonths IsNot Nothing Then lblMonths.Text = "عدد الأشهر: خطأ"
    '        If lblDays IsNot Nothing Then lblDays.Text = "عدد الأيام: خطأ"
    '        If lblTotalDays IsNot Nothing Then lblTotalDays.Text = "صافي الأيام: خطأ"
    '        If lblHolidays IsNot Nothing Then lblHolidays.Text = "الإجمالي مع العطل: خطأ"
    '        If lblFridays IsNot Nothing Then lblFridays.Text = "أيام الجمعة: خطأ"
    '        If lblSaturdays IsNot Nothing Then lblSaturdays.Text = "أيام السبت: خطأ"
    '    End Try
    'End Sub

    ''' <summary>
    ''' حساب الفرق بين تاريخين بدقة مع حساب أيام العمل والعطل (الجمعة فقط عطلة)
    ''' </summary>
    Public Sub CalculateDateDifference(ByVal startDate As DateTime, ByVal endDate As DateTime,
                             ByRef years As Integer, ByRef months As Integer, ByRef days As Integer,
                             ByRef totalDays As Integer, ByRef workingDays As Integer,
                             ByRef fridayCount As Integer, ByRef saturdayCount As Integer,
                             ByRef holidayCount As Integer)
        Try
            ' تأكد من أن التواريخ مرتبة بشكل صحيح
            Dim startDt As DateTime = If(startDate <= endDate, startDate, endDate)
            Dim endDt As DateTime = If(startDate <= endDate, endDate, startDate)

            ' حساب الفرق بالسنوات
            years = endDt.Year - startDt.Year
            months = endDt.Month - startDt.Month
            days = endDt.Day - startDt.Day

            ' التصحيحات للفرق
            If days < 0 Then
                months -= 1
                days += DateTime.DaysInMonth(startDt.Year, startDt.Month)
            End If

            If months < 0 Then
                years -= 1
                months += 12
            End If

            ' ✅ الحساب الصحيح لإجمالي الأيام (بما في ذلك اليوم الأول واليوم الأخير)
            totalDays = (endDt - startDt).Days + 1

            ' تهيئة العدادات
            fridayCount = 0
            saturdayCount = 0
            workingDays = 0

            ' حساب أيام العطل وعمل بشكل دقيق
            Dim currentDate As DateTime = startDt.Date
            While currentDate <= endDt.Date
                Select Case currentDate.DayOfWeek
                    Case DayOfWeek.Friday
                        fridayCount += 1
                    Case DayOfWeek.Saturday
                        saturdayCount += 1
                        ' ✅ السبت يعتبر يوم عمل (تمت إزالته من الحساب)
                    Case Else
                        workingDays += 1
                End Select
                currentDate = currentDate.AddDays(1)
            End While

            ' ✅ تعديل: إضافة السبت إلى أيام العمل
            workingDays += saturdayCount

            ' ✅ الحساب الصحيح لأيام العطل (الجمعة فقط)
            holidayCount = fridayCount

            ' التحقق من أن المجموع صحيح
            If workingDays + holidayCount <> totalDays Then
                ' تصحيح بسيط في حالة وجود اختلاف (نادر الحدوث)
                If workingDays + holidayCount < totalDays Then
                    workingDays = totalDays - holidayCount
                End If
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في حساب الفرق بين التواريخ: {ex.Message}")
            ' إعادة تعيين القيم في حالة الخطأ
            years = 0
            months = 0
            days = 0
            totalDays = 0
            workingDays = 0
            fridayCount = 0
            saturdayCount = 0
            holidayCount = 0
        End Try
    End Sub

    ''' <summary>
    ''' عرض نتيجة حساب الفرق بين التاريخين في Labels
    ''' </summary>
    Public Sub ShowDateDifference(startDate As DateTime, ByVal endDate As DateTime,
                        ByVal lblYears As Label, ByVal lblMonths As Label, ByVal lblDays As Label,
                        ByVal lblTotalDays As Label, ByVal lblFridays As Label,
                        ByVal lblSaturdays As Label, ByVal lblHolidays As Label)
        Try
            ' تعريف المتغيرات لتخزين النتائج
            Dim years As Integer = 0
            Dim months As Integer = 0
            Dim days As Integer = 0
            Dim totalDays As Integer = 0
            Dim workingDays As Integer = 0
            Dim fridayCount As Integer = 0
            Dim saturdayCount As Integer = 0
            Dim holidayCount As Integer = 0

            ' حساب الفرق بين التاريخين مع عطل الأسبوع
            CalculateDateDifference(startDate, endDate, years, months, days, totalDays,
                          workingDays, fridayCount, saturdayCount, holidayCount)

            ' عرض النتائج في Labels
            lblYears.Text = "عدد السنوات: " & years
            lblMonths.Text = "عدد الأشهر: " & months
            lblDays.Text = "عدد الأيام: " & days

            ' ✅ عرض صافي الأيام بدون الجمعة في lblTotalDays
            lblTotalDays.Text = "صافي الأيام (بدون عطلة): " & workingDays

            ' ✅ عرض الإجمالي الكلي مع الجمعة (وليس السبت) في lblHolidays
            lblHolidays.Text = "الإجمالي مع العطل: " & totalDays

            ' عرض نتائج عطل الأسبوع
            lblFridays.Text = "أيام الجمعة: " & fridayCount
            lblSaturdays.Text = "أيام السبت: " & saturdayCount & " (أيام راحة)"

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في عرض نتيجة حساب الفرق: {ex.Message}")
            ' تعيين قيم افتراضية في حالة الخطأ
            If lblYears IsNot Nothing Then lblYears.Text = "عدد السنوات: خطأ"
            If lblMonths IsNot Nothing Then lblMonths.Text = "عدد الأشهر: خطأ"
            If lblDays IsNot Nothing Then lblDays.Text = "عدد الأيام: خطأ"
            If lblTotalDays IsNot Nothing Then lblTotalDays.Text = "صافي الأيام: خطأ"
            If lblHolidays IsNot Nothing Then lblHolidays.Text = "الإجمالي مع العطل: خطأ"
            If lblFridays IsNot Nothing Then lblFridays.Text = "أيام الجمعة: خطأ"
            If lblSaturdays IsNot Nothing Then lblSaturdays.Text = "أيام السبت: خطأ"
        End Try
    End Sub


#End Region


#Region "====================( Checking Functions )=================="

    'دالة للتحقق من عنصر النص ليست فارغة
    Public Function MyTextNull(ByVal TXT As TextBox, ByVal Caption As String) As Boolean
        If String.IsNullOrEmpty(TXT.Text.Trim) = True Then
            MsgBox("حقل   '" & Caption & "'   " & "فارغا", MsgBoxStyle.Information, "تأكيد الإدخال")
            TXT.Text = Nothing
            TXT.Focus()
            Return True
        Else
            TXT.Text = TXT.Text.Trim
            Return False
        End If
    End Function
    ' التحقق مما إذا كان هناك عنصر محدد في الكوموبكس 
    Public Function MyCombIndexNull(ByVal Comb As ComboBox, ByVal CombCaption As String) As Boolean
        If Comb.SelectedIndex = -1 Then
            MsgBox("يرجى الإختيار   '" & Space(1) & "'" & CombCaption & "'", MsgBoxStyle.Information, "تأكيد الإدخال")

            Comb.Focus()
            Return True
        Else

            Return False
        End If
    End Function

    ' التحقق من وجود قيمة في الكوموبكس 
    Public Function MyCombValueNull(ByVal Comb As ComboBox, ByVal CombCaption As String) As Boolean
        If Comb.SelectedValue <= 0 Then
            MsgBox("يرجى إختيار   '" & Space(1) & "'" & CombCaption & "'", MsgBoxStyle.Information, "تأكيد الإدخال")

            Comb.Focus()
            Return True
        Else

            Return False
        End If
    End Function

    Public Function MyCombTextEmpty(ByVal Comb As ComboBox, ByVal CombCaption As String) As Boolean
        'If
        If Comb.Text.Trim = String.Empty Then
            MsgBox("يرجى الإختيار   '" & Space(1) & "'" & CombCaption & "'", MsgBoxStyle.Information, "تأكيد الإدخال")

            Comb.Focus()
            Return True
        Else

            Return False
        End If
    End Function


    'Function to check the RichTextBox element is not empty
    Public Function MyTextNull(ByVal TXT As RichTextBox, ByVal Caption As String) As Boolean
        If TXT.Text.Trim = Nothing Then
            MsgBox("حقل   '" & Caption & "'   " & "فارغا", MsgBoxStyle.Information, "تأكيد الإدخال")
            TXT.Text = Nothing
            TXT.Focus()
            Return True
        Else
            TXT.Text = TXT.Text.Trim
            Return False
        End If
    End Function

    ''' <summary>
    ''' التحقق من أن اسم المجلد من التصنيفات المسموحة
    ''' </summary>
    Public Function IsValidArchiveCategory(folderName As String) As Boolean
        ' التصنيفات المسموحة
        Dim validCategories As String() = {"شركات اخرى", "الجمارك العامة", "مذكرات", "تصدير", "إفراج جمركي"}

        ' التحقق من وجود الاسم في التصنيفات المسموحة
        For Each category As String In validCategories
            If String.Equals(folderName, category, StringComparison.OrdinalIgnoreCase) Then
                Return True
            End If
        Next

        Return False
    End Function


#End Region

#Region "================ Confirmation Messages ================"

    Public Sub SavedSuccessfully(ByVal lbl As Label, ByVal pic As PictureBox, ByVal t As Windows.Forms.Timer)
        lbl.Text = "تم الحفظ بنجاح"
        pic.Image = My.Resources.OK
        t.Start()
    End Sub

    Public Sub UpdetedSuccessfully(ByVal lbl As Label, ByVal pic As PictureBox, ByVal t As Windows.Forms.Timer)
        lbl.Text = "تم التعديل بنجاح"
        pic.Image = My.Resources.OK
        t.Start()
    End Sub

    Public Sub DeletedSuccessfully(ByVal lbl As Label, ByVal pic As PictureBox, ByVal t As Windows.Forms.Timer)
        lbl.Text = "تم الحذف بنجاح"
        pic.Image = My.Resources.OK
        t.Start()
    End Sub
    Public Sub DeletedSaderSuccessfully(ByVal lbl As Label, ByVal pic As PictureBox, ByVal t As Windows.Forms.Timer)
        lbl.Text = "تم حذف الصادر والأرشيف المرتبط به بنجاح"
        pic.Image = My.Resources.OK
        t.Start()
    End Sub

    Public Sub ConfirmMessage(ByVal lbl As Label, ByVal pic As PictureBox, ByVal t As Windows.Forms.Timer, ByVal msg As String)

        lbl.Text = msg
        pic.Image = My.Resources.OK
        t.Start()

    End Sub

    Public Sub ClearConfirmMsgTools(ByVal lblMsg As Label, ByVal picMsg As PictureBox, ByVal T As Windows.Forms.Timer)
        lblMsg.Text = Nothing
        lblMsg.BackColor = SystemColors.ControlLight
        picMsg.Image = Nothing
        picMsg.BackColor = SystemColors.ControlLight
        T.Stop()
    End Sub


#End Region


End Module
