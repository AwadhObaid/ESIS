Imports System.Data.SqlClient
Imports System.IO
Imports System.Security
Imports System.Threading
Imports ALquhaliLibrary.DBCLS
Imports Microsoft.Win32
Imports PdfiumViewer


Module GlobalData


#Region "====================( المتغيرات العامة )=================="
    ' متغير لتخزين حالة المستخدم
    Public IsCurrentUserActive As Boolean = False
    Public IsCurrentUserAdmin As Boolean = False
    ' متغير لتخزين صورة المستخدم الحالي
    Public CurrentUserImage As Image = Nothing

    ''' <summary>
    ''' متغير ثابت لتخزين المثيل الحالي لأي نموذج
    ''' </summary>
    Private currentInstance As Form = Nothing

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


#End Region

#Region "=======================( PdfViewer )============================ "
    Public Sub ShowPdf(viewer As PdfiumViewer.PdfViewer, pdfPath As String)
        If viewer Is Nothing Then Return

        ' اغلاق المستند السابق لتفادي قفل الملف
        Try
            If viewer.Document IsNot Nothing Then
                viewer.Document.Dispose()
                viewer.Document = Nothing
            End If
        Catch
        End Try

        If String.IsNullOrWhiteSpace(pdfPath) OrElse Not IO.File.Exists(pdfPath) Then Return

        ' تحميل الجديد
        viewer.Document = PdfiumViewer.PdfDocument.Load(pdfPath)
        viewer.ZoomMode = PdfViewerZoomMode.FitBest

    End Sub

    Public Sub ClearPdf(viewer As PdfiumViewer.PdfViewer)
        If viewer Is Nothing Then Return
        Try
            If viewer.Document IsNot Nothing Then
                viewer.Document.Dispose()
                viewer.Document = Nothing
            End If
        Catch
        End Try
    End Sub

#End Region


#Region "====================== ( Archive Veribles ) ==========================="

    'Clear Archive Veribles Controls
    'Clear Archive Veribles Controls  (NEW: Multi-user friendly)
    Public Sub ClearContolOfFrmArchiveVariables(ByVal xx As ArchiveVariablesForm)
        Try
            ' تفعيل واجهة الإدخال كما كان سابقاً
            xx.TabControl1.Enabled = True
            xx.GroupBox1.Enabled = True

            ' تفريغ الحقول عبر دالتك
            Clear_Control_On_TabControls(xx.TabControl1)

            ' قيم افتراضية
            If xx.ImageExtension.Items.Count > 0 Then
                xx.ImageExtension.SelectedIndex = 0
            End If
            xx.ScandImageName.Text = "ScanImage"
            xx.ScandImagePath.Text = Path.GetTempPath()

            ' أزرار
            xx.btnSave.Enabled = True
            xx.btnUpdate.Enabled = False
            xx.btnDelete.Enabled = False
            xx.btnClose.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    'Save Archive Variables
    'Save Archive Variables (INSERT for current user)
    Public Sub SaveArchiveVariables(ByVal xx As ArchiveVariablesForm)
        Try
            If LoggedInUserID <= 0 Then
                MessageBox.Show("لا يمكن حفظ الإعدادات لأن LoggedInUserID غير صحيح.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim dt As New DataTable

            Using cn As SqlConnection = Con()
                Using cmd As New SqlCommand("SELECT * FROM ArchiveVariables WHERE ID = @UserID", cn)
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = LoggedInUserID

                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)

                        If dt.Rows.Count = 0 Then
                            dt.Rows.Add()
                            Dim last As Integer = dt.Rows.Count - 1

                            With dt.Rows(last)
                                ' IMPORTANT: لكل مستخدم سجل مستقل
                                .Item("ID") = LoggedInUserID

                                .Item("ExternalSoredPath") = If(String.IsNullOrEmpty(xx.ExternalSoredPath.Text), DBNull.Value, xx.ExternalSoredPath.Text.Trim)
                                .Item("ScandImagePath") = If(String.IsNullOrEmpty(xx.ScandImagePath.Text), DBNull.Value, xx.ScandImagePath.Text.Trim)
                                .Item("ScandImageName") = If(String.IsNullOrEmpty(xx.ScandImageName.Text), DBNull.Value, xx.ScandImageName.Text)
                                .Item("ImageExtension") = If(xx.ImageExtension.SelectedIndex = -1, DBNull.Value, xx.ImageExtension.SelectedIndex)
                                .Item("PDFStoredPath") = If(String.IsNullOrEmpty(xx.PDFStoredPath.Text), DBNull.Value, xx.PDFStoredPath.Text.Trim)

                                .Item("DefualtSenderEmail") = If(String.IsNullOrEmpty(xx.DefualtSenderEmail.Text), DBNull.Value, xx.DefualtSenderEmail.Text)
                                .Item("EmailAppPassword") = If(String.IsNullOrEmpty(xx.EmailAppPassword.Text), DBNull.Value, xx.EmailAppPassword.Text)
                                .Item("SMTPServer") = If(String.IsNullOrEmpty(xx.SMTPServer.Text), DBNull.Value, xx.SMTPServer.Text)
                                .Item("PortNumber") = If(String.IsNullOrEmpty(xx.PortNumber.Text), DBNull.Value, xx.PortNumber.Text)
                                .Item("FrmConStrPASS") = If(String.IsNullOrEmpty(xx.txtPassCon.Text), DBNull.Value, Encrypt(xx.txtPassCon.Text.Trim))

                                .Item("PathSearch") = If(String.IsNullOrEmpty(xx.txtSearchDefaultPath.Text), DBNull.Value, xx.txtSearchDefaultPath.Text)
                                .Item("LastSaderNumber") = If(String.IsNullOrEmpty(xx.txtlastSavedNumber.Text), DBNull.Value, xx.txtlastSavedNumber.Text)
                                .Item("daysToKeep") = If(String.IsNullOrEmpty(xx.txtdaysToKeep.Text), DBNull.Value, CInt(xx.txtdaysToKeep.Text))
                                .Item("NotificationsFormInterval") = If(xx.nudNotificationInterval.Value = 5, DBNull.Value, CInt(xx.nudNotificationInterval.Value))

                                .Item("AddDate") = Now
                                .Item("AddUser") = LoggedInUserID
                            End With

                            ' حفظ
                            Dim builder As New SqlCommandBuilder(da)
                            da.Update(dt)
                            dt.AcceptChanges()

                            SaveVariablesAtSettings(xx)
                            GlobalData.SavedSuccessfully(xx.lblConfirmMsg, xx.PicMsg, xx.Timer1)

                            ' بعد الحفظ: اجعلها وضع "تعديل"
                            xx.btnSave.Enabled = False
                            xx.btnUpdate.Enabled = True
                            xx.btnDelete.Enabled = False
                            xx.btnClose.Enabled = True
                        Else
                            MessageBox.Show("يوجد إعدادات محفوظة لهذا المستخدم بالفعل. استخدم زر (تعديل).", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            xx.btnSave.Enabled = False
                            xx.btnUpdate.Enabled = True
                            xx.btnDelete.Enabled = False
                            xx.btnClose.Enabled = True
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub





    'Updat Archive Variables
    'Updat Archive Variables (UPDATE for current user)
    Public Sub UpdateArchiveVariables(ByVal xx As ArchiveVariablesForm)
        Try
            If LoggedInUserID <= 0 Then
                MessageBox.Show("لا يمكن تحديث الإعدادات لأن LoggedInUserID غير صحيح.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim dt As New DataTable

            Using cn As SqlConnection = Con()
                Using cmd As New SqlCommand("SELECT * FROM ArchiveVariables WHERE ID = @UserID", cn)
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = LoggedInUserID

                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)

                        If dt.Rows.Count > 0 Then
                            dt.Rows(0).Item("ExternalSoredPath") = xx.ExternalSoredPath.Text.Trim
                            dt.Rows(0).Item("ScandImagePath") = xx.ScandImagePath.Text.Trim
                            dt.Rows(0).Item("ScandImageName") = xx.ScandImageName.Text
                            dt.Rows(0).Item("ImageExtension") = xx.ImageExtension.SelectedIndex
                            dt.Rows(0).Item("PDFStoredPath") = xx.PDFStoredPath.Text.Trim

                            dt.Rows(0).Item("DefualtSenderEmail") = xx.DefualtSenderEmail.Text
                            dt.Rows(0).Item("EmailAppPassword") = xx.EmailAppPassword.Text
                            dt.Rows(0).Item("SMTPServer") = xx.SMTPServer.Text
                            dt.Rows(0).Item("PortNumber") = xx.PortNumber.Text
                            dt.Rows(0).Item("FrmConStrPASS") = Encrypt(xx.txtPassCon.Text.Trim)

                            dt.Rows(0).Item("PathSearch") = xx.txtSearchDefaultPath.Text
                            dt.Rows(0).Item("LastSaderNumber") = xx.txtlastSavedNumber.Text
                            dt.Rows(0).Item("daysToKeep") = CInt(xx.txtdaysToKeep.Text)
                            dt.Rows(0).Item("NotificationsFormInterval") = CInt(xx.nudNotificationInterval.Value)

                            dt.Rows(0).Item("EditDate") = Now
                            dt.Rows(0).Item("EditUser") = LoggedInUserID

                            Dim builder As New SqlCommandBuilder(da)
                            da.Update(dt)
                            dt.AcceptChanges()

                            SaveVariablesAtSettings(xx)
                            UpdetedSuccessfully(xx.lblConfirmMsg, xx.PicMsg, xx.Timer1)

                            xx.btnSave.Enabled = False
                            xx.btnUpdate.Enabled = True
                            xx.btnDelete.Enabled = False
                            xx.btnClose.Enabled = True
                        Else
                            MessageBox.Show("لا توجد إعدادات محفوظة لهذا المستخدم. استخدم زر (جديد) ثم (حفظ).", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            xx.btnSave.Enabled = True
                            xx.btnUpdate.Enabled = False
                            xx.btnDelete.Enabled = False
                            xx.btnClose.Enabled = True
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erorr Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    ''Show Archive Variables
    ''Show Archive Variables (LOAD for current user)
    Public Sub ShowArchiveVariables(ByVal xx As ArchiveVariablesForm)
        Try
            If LoggedInUserID <= 0 Then
                ' لا يوجد مستخدم مسجل
                xx.btnSave.Enabled = False
                xx.btnUpdate.Enabled = False
                xx.btnDelete.Enabled = False
                Return
            End If

            Dim DT As New DataTable

            Using cn As SqlConnection = Con()
                Using cmd As New SqlCommand("SELECT TOP 1 * FROM ArchiveVariables WHERE ID = @UserID", cn)
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = LoggedInUserID

                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(DT)
                    End Using
                End Using
            End Using

            ' تفعيل واجهة الإدخال
            xx.TabControl1.Enabled = True
            xx.GroupBox1.Enabled = True

            If DT.Rows.Count > 0 Then
                With DT.Rows(0)
                    ' الحقول النصية
                    xx.ExternalSoredPath.Text = SafeGetString(DT.Rows(0), "ExternalSoredPath")
                    xx.ScandImagePath.Text = SafeGetString(DT.Rows(0), "ScandImagePath")
                    xx.PDFStoredPath.Text = SafeGetString(DT.Rows(0), "PDFStoredPath")
                    xx.txtSearchDefaultPath.Text = SafeGetString(DT.Rows(0), "PathSearch")
                    xx.txtlastSavedNumber.Text = SafeGetString(DT.Rows(0), "LastSaderNumber")

                    ' الحقول الرقمية
                    xx.txtdaysToKeep.Text = If(IsDBNull(.Item("daysToKeep")), "0", .Item("daysToKeep").ToString())
                    xx.nudNotificationInterval.Value = If(IsDBNull(.Item("NotificationsFormInterval")), 5, CInt(.Item("NotificationsFormInterval")))

                    ' الحقول المتعلقة بالإعدادات البريدية
                    xx.DefualtSenderEmail.Text = SafeGetString(DT.Rows(0), "DefualtSenderEmail")
                    xx.EmailAppPassword.Text = SafeGetString(DT.Rows(0), "EmailAppPassword")
                    xx.SMTPServer.Text = SafeGetString(DT.Rows(0), "SMTPServer")
                    xx.PortNumber.Text = SafeGetString(DT.Rows(0), "PortNumber")

                    ' فك تشفير كلمة المرور
                    Dim encryptedPass = .Item("FrmConStrPASS")
                    xx.txtPassCon.Text = If(IsDBNull(encryptedPass), "", Decrypt(encryptedPass.ToString()))

                    ' امتداد الصورة
                    Dim imageExtValue = .Item("ImageExtension")
                    If IsDBNull(imageExtValue) Then
                        xx.ImageExtension.SelectedIndex = 0
                    Else
                        xx.ImageExtension.SelectedIndex = CInt(imageExtValue)
                    End If
                End With

                ' يوجد سجل -> تعديل
                xx.btnSave.Enabled = False
                xx.btnUpdate.Enabled = True
                xx.btnDelete.Enabled = False
                xx.btnClose.Enabled = True
            Else
                ' لا يوجد سجل -> حفظ جديد
                xx.btnSave.Enabled = True
                xx.btnUpdate.Enabled = False
                xx.btnDelete.Enabled = False
                xx.btnClose.Enabled = True
            End If

            DT.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SaveVariablesAtSettings(ByVal xx As ArchiveVariablesForm)

        My.Settings.ARCHIVE_DEFAULT_PATH = xx.ExternalSoredPath.Text.Trim
        My.Settings.ImgFolderPath = xx.ScandImagePath.Text.Trim
        My.Settings.ImageFormat = xx.ImageExtension.SelectedIndex
        My.Settings.DefualtImgName = xx.ScandImageName.Text
        My.Settings.PDFFolderPath = xx.PDFStoredPath.Text.Trim
        My.Settings.FrmConStrPASS = Encrypt(xx.txtPassCon.Text.Trim)

        My.Settings.DefaultPathSearch = xx.txtSearchDefaultPath.Text.Trim()
        My.Settings.LastSaderNumber = xx.txtlastSavedNumber.Text.Trim()
        My.Settings.daysToKeep = CInt(xx.txtdaysToKeep.Text.Trim())
        My.Settings.NotificationsFormInterval = CInt(xx.nudNotificationInterval.Value)

        My.Settings.Save()
    End Sub


    Public Sub RowInsert(ByVal DT As DataTable, ByVal da As SqlDataAdapter)

        Dim Save As New SqlCommandBuilder(da)
        da.Update(DT)
        DT.AcceptChanges()
        DT.Dispose()
        da.Dispose()

    End Sub

    Private Function SafeGetString(row As DataRow, columnName As String) As String
        If row.IsNull(columnName) Then
            Return ""
        Else
            Return row.Item(columnName).ToString()
        End If
    End Function


    'Public Sub ShowArchiveVariables(ByVal xx As ArchiveVariablesForm)
    '    Try
    '        Dim DT As New DataTable
    '        Dim sqlst As String
    '        sqlst = "Select * FROM ArchiveVariables where ID = 1 "
    '        FillDataTable(DT, sqlst)

    '        If DT.Rows.Count > 0 Then


    '            xx.ExternalSoredPath.Text = DT.Rows(0).Item("ExternalSoredPath").ToString()

    '            xx.ScandImagePath.Text = DT.Rows(0).Item("ScandImagePath").ToString()
    '            xx.ScandImageName.Text = DT.Rows(0).Item("ScandImageName").ToString()
    '            xx.ImageExtension.SelectedIndex = DT.Rows(0).Item("ImageExtension")
    '            xx.PDFStoredPath.Text = DT.Rows(0).Item("PDFStoredPath").ToString()


    '            xx.txtSearchDefaultPath.Text = DT.Rows(0).Item("PathSearch").ToString
    '            xx.txtlastSavedNumber.Text = DT.Rows(0).Item("LastSaderNumber").ToString
    '            xx.txtdaysToKeep.Text = CInt(DT.Rows(0).Item("daysToKeep"))
    '            xx.nudNotificationInterval.Value = CInt(DT.Rows(0).Item("NotificationsFormInterval"))


    '            xx.DefualtSenderEmail.Text = DT.Rows(0).Item("DefualtSenderEmail")
    '            xx.EmailAppPassword.Text = DT.Rows(0).Item("EmailAppPassword")
    '            xx.SMTPServer.Text = DT.Rows(0).Item("SMTPServer")
    '            xx.PortNumber.Text = DT.Rows(0).Item("PortNumber")
    '            xx.txtPassCon.Text = Decrypt(DT.Rows(0).Item("FrmConStrPASS").ToString())
    '            xx.btnSave.Enabled = False
    '            xx.btnUpdate.Enabled = True
    '            xx.btnDelete.Enabled = True
    '        End If
    '        DT.Dispose()
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "Erorr Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    'Clear Control On TabControls

    Public Sub Clear_Control_On_TabControls(ByVal TabControl As TabControl)

        TabControl.Enabled = True
        TabControl.SelectedTab = TabControl.TabPages(0)

        For i As Int16 = 0 To TabControl.TabPages.Count - 1

            For x As Int16 = 0 To TabControl.TabPages(i).Controls.Count - 1

                If TypeOf TabControl.TabPages(i).Controls(x) Is TextBox Then  ' clear TextBox Controls
                    Dim TXT As TextBox = TabControl.TabPages(i).Controls(x)
                    TXT.Clear()
                    TXT.Tag = Nothing
                End If
                If TypeOf TabControl.TabPages(i).Controls(x) Is ComboBox Then    ' Clear Combobox Control
                    Dim Combo As ComboBox = TabControl.TabPages(i).Controls(x)
                    Combo.SelectedValue = 0
                    Combo.SelectedIndex = -1
                    Combo.Text = Nothing
                    Combo.Tag = Nothing
                End If
                If TypeOf TabControl.TabPages(i).Controls(x) Is CheckBox Then   'Clear CheckBox Control
                    Dim Ck As CheckBox = TabControl.TabPages(i).Controls(x)
                    Ck.Checked = False
                End If

                If TypeOf TabControl.TabPages(i).Controls(x) Is DataGridView Then   'Clear DataGridView Control
                    Dim dgv As DataGridView = TabControl.TabPages(i).Controls(x)
                    dgv.Rows.Clear()
                End If

                For Each GP As GroupBox In TabControl.TabPages(i).Controls.OfType(Of GroupBox)() 'Clear Controls inside GroupBox
                    GP.Enabled = True
                    For Each txt As TextBox In GP.Controls.OfType(Of TextBox)()
                        txt.Tag = ""
                        txt.Clear()
                    Next

                    For Each combo As ComboBox In GP.Controls.OfType(Of ComboBox)()
                        combo.SelectedIndex = -1
                        combo.Text = Nothing
                    Next

                    For Each CK As CheckBox In GP.Controls.OfType(Of CheckBox)()
                        CK.Checked = False
                    Next
                Next

                For Each PL As Panel In TabControl.TabPages(i).Controls.OfType(Of Panel)() ' Clear Controls inside Panel
                    PL.Enabled = True
                    For Each txt As TextBox In PL.Controls.OfType(Of TextBox)()
                        txt.Tag = ""
                        txt.Clear()
                    Next

                    For Each combo As ComboBox In PL.Controls.OfType(Of ComboBox)()
                        combo.SelectedIndex = -1
                        combo.Text = Nothing
                    Next

                    For Each CK As CheckBox In PL.Controls.OfType(Of CheckBox)()
                        CK.Checked = False
                    Next

                    For Each dgv As DataGridView In PL.Controls.OfType(Of DataGridView)()
                        dgv.DataSource = Nothing
                        dgv.Rows.Clear()
                    Next

                    For Each PIC As PictureBox In PL.Controls.OfType(Of PictureBox)()   ' Clear PictureBox Control
                        PIC.Image = Nothing
                    Next
                    For Each DTP As DateTimePicker In PL.Controls.OfType(Of DateTimePicker)()   ' Clear DateTimePicker Control
                        DTP.Value = Now.Date.ToString("dd-MM-yyyy")
                    Next
                Next
            Next
        Next
    End Sub

    'Clear Controls Inside Panel
    Public Sub Clear_Controls_Inside_Panel(ByVal Panel As Panel)

        Panel.Enabled = True

        For Each txt As TextBox In Panel.Controls.OfType(Of TextBox)()  ' clear TextBox Controls
            txt.Clear()
            txt.Tag = String.Empty
        Next

        For Each combo As ComboBox In Panel.Controls.OfType(Of ComboBox)()    ' Clear Combobox Control
            combo.SelectedIndex = -1
            combo.Text = Nothing
        Next

        For Each Ck As CheckBox In Panel.Controls.OfType(Of CheckBox)()   'Clear CheckBox Control
            Ck.Checked = False
        Next

        For Each Rtxt As RichTextBox In Panel.Controls.OfType(Of RichTextBox)() ' Clear RichTextBox Control
            Rtxt.Clear()
            Rtxt.Tag = String.Empty
        Next

    End Sub

#End Region


#Region "================ Confirmation Messages ================"

    Public Sub SavedSuccessfully(ByVal lbl As Label, ByVal pic As PictureBox, ByVal t As System.Windows.Forms.Timer)
        lbl.Text = "تم الحفظ بنجاح"
        pic.Image = My.Resources.OK
        t.Start()
    End Sub

    Public Sub UpdetedSuccessfully(ByVal lbl As Label, ByVal pic As PictureBox, ByVal t As System.Windows.Forms.Timer)
        lbl.Text = "تم التعديل بنجاح"
        pic.Image = My.Resources.OK
        t.Start()
    End Sub

    Public Sub DeletedSuccessfully(ByVal lbl As Label, ByVal pic As PictureBox, ByVal t As System.Windows.Forms.Timer)
        lbl.Text = "تم الحذف بنجاح"
        pic.Image = My.Resources.OK
        t.Start()
    End Sub
    Public Sub DeletedSaderSuccessfully(ByVal lbl As Label, ByVal pic As PictureBox, ByVal t As System.Windows.Forms.Timer)
        lbl.Text = "تم حذف الصادر والأرشيف المرتبط به بنجاح"
        pic.Image = My.Resources.OK
        t.Start()
    End Sub

    Public Sub DeletedArchiveSuccessfully(ByVal lbl As Label, ByVal pic As PictureBox, ByVal t As System.Windows.Forms.Timer)
        lbl.Text = "تم ارسال الارشيف الى سلة المحذوفات بنجاح"
        pic.Image = My.Resources.OK
        t.Start()
    End Sub

    Public Sub ConfirmMessage(ByVal lbl As Label, ByVal pic As PictureBox, ByVal t As System.Windows.Forms.Timer, ByVal msg As String)

        lbl.Text = msg
        pic.Image = My.Resources.OK
        t.Start()

    End Sub

    Public Sub ClearConfirmMsgTools(ByVal lblMsg As Label, ByVal picMsg As PictureBox, ByVal T As System.Windows.Forms.Timer)
        lblMsg.Text = Nothing
        lblMsg.BackColor = SystemColors.ControlLight
        picMsg.Image = Nothing
        picMsg.BackColor = SystemColors.ControlLight
        T.Stop()
    End Sub


#End Region

#Region "====================( معالجة الملفات )=================="
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

    Private Function GetSelectedRowStringValue(DataGridViewName As DataGridView, columnName As String, Optional defaultValue As String = "") As String
        Return DataGridViewHelper.GetSelectedRowValue(Of String)(DataGridViewName, columnName, defaultValue)
    End Function

#End Region

#Region "====================( التحقق من البوالص )=================="

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

#End Region

#Region "====================( حساب التاريخ )=================="


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

#Region "====================== ( Password Management ) ==========================="
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
        Dim storedEncrypted As String = My.Settings.FrmConStrPASS

        ' إذا لم تكن هناك كلمة مرور مُعدة مسبقًا (القيمة فارغة)، فإن كلمة المرور الحالية فارغة
        If String.IsNullOrEmpty(storedEncrypted) Then
            If String.IsNullOrEmpty(inputPassword) Then
                Return True
            Else
                MessageBox.Show("كلمة المرور التي أدخلتها غير صحيحة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        End If

        Try
            Dim decryptedPassword As String = Decrypt(storedEncrypted)
            If String.Equals(inputPassword, decryptedPassword, StringComparison.Ordinal) Then
                Return True
            Else
                MessageBox.Show("كلمة المرور التي أدخلتها غير صحيحة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في التحقق من كلمة المرور. يرجى الاتصال بالمسؤول." & vbNewLine & "تفاصيل الخطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
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
        Dim encryptedPassword As String = Encrypt(newPassword)
        My.Settings.FrmConStrPASS = encryptedPassword
        My.Settings.Save()
        MessageBox.Show("تم تعديل كلمة المرور بنجاح.", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    '''' <summary>
    '''' تشفير نص كلمة المرور باستخدام DPAPI.
    '''' </summary>
    '''' <param name="input">نص كلمة المرور.</param>
    '''' <returns>النص المشفر بصيغة Base64.</returns>
    'Private Function EncryptString(input As String) As String
    '    If String.IsNullOrEmpty(input) Then Return String.Empty
    '    Dim data As Byte() = System.Text.Encoding.UTF8.GetBytes(input)
    '    Dim encryptedData As Byte() = ProtectedData.Protect(data, Nothing, DataProtectionScope.CurrentUser)
    '    Return Convert.ToBase64String(encryptedData)
    'End Function

    '''' <summary>
    '''' فك تشفير كلمة المرور المخزنة.
    '''' </summary>
    '''' <param name="encryptedBase64">النص المشفر بصيغة Base64.</param>
    '''' <returns>نص كلمة المرور الأصلي.</returns>
    'Private Function DecryptString(encryptedBase64 As String) As String
    '    If String.IsNullOrEmpty(encryptedBase64) Then Return String.Empty
    '    Dim encryptedData As Byte() = Convert.FromBase64String(encryptedBase64)
    '    Dim decryptedData As Byte() = ProtectedData.Unprotect(encryptedData, Nothing, DataProtectionScope.CurrentUser)
    '    Return System.Text.Encoding.UTF8.GetString(decryptedData)
    'End Function
#End Region

#Region "====================== ( Global Password Validation ) ==========================="
    ''' <summary>
    ''' التحقق من كلمة المرور باستخدام نموذج FrmPassword.
    ''' </summary>
    ''' <param name="promptMessage">رسالة تظهر للمستخدم داخل نموذج FrmPassword.</param>
    ''' <returns>قيمة Boolean تشير إلى ما إذا كانت كلمة المرور صحيحة أم لا.</returns>
    Public Function ValidatePassword(promptMessage As String) As Boolean
        Try
            ' إنشاء نموذج إدخال كلمة المرور
            Dim passwordForm As New PasswordForm()
            passwordForm.Label1.Text = promptMessage
            passwordForm.ShowDialog()

            ' التحقق من إلغاء العملية أو ترك الحقل فارغًا
            If String.IsNullOrEmpty(passwordForm.InputText) Then
                Return False
            End If

            ' فك تشفير كلمة المرور المخزنة ومقارنتها مع المدخلة
            Dim storedEncrypted As String = My.Settings.FrmConStrPASS
            If String.IsNullOrEmpty(storedEncrypted) Then
                MessageBox.Show("لم يتم تعيين كلمة مرور. يرجى الاتصال بالمسؤول.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Dim decryptedPassword As String = Decrypt(storedEncrypted)
            If String.Equals(passwordForm.InputText, decryptedPassword, StringComparison.Ordinal) Then
                Return True
            Else
                MessageBox.Show("معذرة، كلمة المرور الذي أدخلتها غير صحيحة.", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء التحقق من كلمة المرور: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

#End Region

#Region "***************** Change Bwtween Combuter Languages *****************"

    Public Sub ChangeToEnglish()
        Dim Lang As InputLanguage
        For Each Lang In InputLanguage.InstalledInputLanguages
            If Lang.Culture.EnglishName.ToUpper Like "*english*".ToUpper Then
                InputLanguage.CurrentInputLanguage = Lang
            End If
        Next
    End Sub

    Public Sub ChangeToArabic()
        Dim Lang As InputLanguage
        For Each Lang In InputLanguage.InstalledInputLanguages
            If Lang.Culture.EnglishName.ToUpper Like "*arabic*".ToUpper Then
                InputLanguage.CurrentInputLanguage = Lang
            End If
        Next
    End Sub

#End Region

#Region "====================( إدارة النماذج (MDI) )=================="
    '''' <summary>
    '''' فتح نموذج كنافذة فرعية (MDI Child)
    '''' </summary>
    'Public Sub OpenMdiForm(Of T As Form)(createInstance As Func(Of T), Optional ByVal mdiParent As Form = Nothing)
    '    Try
    '        ' إذا لم يتم تمرير النموذج الرئيسي، حاول العثور عليه تلقائيًا
    '        If mdiParent Is Nothing Then
    '            ' البحث عن النموذج الرئيسي (عادةً ما يكون النموذج الأول في قائمة النماذج المفتوحة)
    '            For Each form As Form In Application.OpenForms
    '                If form.IsMdiContainer Then
    '                    mdiParent = form
    '                    Exit For
    '                End If
    '            Next

    '            ' إذا لم يتم العثور على نموذج رئيسي، حاول استخدام النموذج الرئيسي المعروف
    '            If mdiParent Is Nothing Then
    '                For Each form As Form In Application.OpenForms
    '                    If TypeOf form Is MainForm Then
    '                        mdiParent = form
    '                        Exit For
    '                    End If
    '                Next
    '            End If
    '        End If

    '        ' إذا لم يتم العثور على النموذج الرئيسي بعد، أظهر خطأ
    '        If mdiParent Is Nothing Then
    '            MessageBox.Show("تعذر العثور على النموذج الرئيسي (MdiContainer).", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Return
    '        End If

    '        ' البحث عن النموذج المطلوب في النماذج الفرعية
    '        Dim existingForm As T = Nothing
    '        For Each child As Form In mdiParent.MdiChildren
    '            If TypeOf child Is T Then
    '                existingForm = DirectCast(child, T)
    '                Exit For
    '            End If
    '        Next

    '        If existingForm IsNot Nothing Then
    '            ' إذا كان النموذج موجودًا، جلبه إلى المقدمة
    '            existingForm.BringToFront()
    '            existingForm.Activate()
    '        Else
    '            ' إذا لم يكن النموذج موجودًا، إنشاء نموذج جديد باستخدام الدالة الممررة
    '            Dim newForm As T = createInstance()
    '            newForm.MdiParent = mdiParent
    '            newForm.WindowState = FormWindowState.Maximized
    '            newForm.Show()
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show($"خطأ في فتح النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub
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
        End If
    End Sub

    ''' <summary>
    ''' فتح نموذج واحد فقط (مثيل واحد) من نوع TForm
    ''' </summary>
    ''' <typeparam name="TForm">نوع النموذج المراد فتحه</typeparam>
    ''' <param name="createInstance">دالة لإنشاء مثيل جديد من النموذج</param>
    ''' <param name="onSuccess">دالة اختيارية لتنفيذها بعد إغلاق النموذج بنجاح</param>
    Public Sub OpenSingleInstance(Of TForm As {New, Form})(createInstance As Func(Of TForm), Optional onSuccess As Action = Nothing)
        Try
            ' البحث عن نموذج مفتوح بالفعل من النوع المطلوب
            Dim existingForm As TForm = Nothing
            For Each form As Form In Application.OpenForms
                If TypeOf form Is TForm Then
                    existingForm = DirectCast(form, TForm)
                    Exit For
                End If
            Next

            If existingForm IsNot Nothing AndAlso Not existingForm.IsDisposed Then
                ' إذا كان النموذج مفتوحًا، جلبه إلى المقدمة
                existingForm.BringToFront()
                existingForm.Activate()
            Else
                ' إذا لم يكن النموذج مفتوحًا، أنشئ مثيلًا جديدًا واحتفظ به
                currentInstance = createInstance()
                AddHandler currentInstance.FormClosed, AddressOf OnFormClosed

                ' عرض النموذج
                If currentInstance.ShowDialog() = DialogResult.OK Then
                    ' تنفيذ الدالة الاختيارية بعد الإغلاق بنجاح
                    onSuccess?.Invoke()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' فتح نموذج كنافذة فرعية (MDI Child)
    ''' </summary>
    ''' <typeparam name="TForm">نوع النموذج المراد فتحه</typeparam>
    ''' <param name="createInstance">دالة لإنشاء مثيل جديد من النموذج</param>
    ''' <param name="mdiParent">نموذج رئيسي (MdiContainer)</param>
    Public Sub OpenMdiForm(Of TForm As {New, Form})(createInstance As Func(Of TForm), Optional ByVal mdiParent As Form = Nothing)
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
            Dim existingForm As TForm = mdiParent.MdiChildren.OfType(Of TForm)().FirstOrDefault()

            If existingForm IsNot Nothing Then
                ' إذا كان النموذج موجودًا، جلبه إلى المقدمة
                existingForm.BringToFront()
                existingForm.Activate()
            Else
                ' إذا لم يكن النموذج موجودًا، إنشاء نموذج جديد
                Dim newForm As New TForm()
                newForm.MdiParent = mdiParent
                newForm.Show()
            End If
        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء فتح النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' حدث يتم استدعاؤه عند إغلاق النموذج
    ''' </summary>
    Private Sub OnFormClosed(sender As Object, e As FormClosedEventArgs)
        ' إعادة تعيين المتغير عندما يتم إغلاق النموذج
        currentInstance = Nothing
    End Sub

    ''' <summary>
    ''' واجهة لتمكين تمرير البيانات عند فتح النماذج
    ''' </summary>
    Public Interface IFormWithParameters
        Sub InitializeWithData(data As Object)
    End Interface


#End Region

#Region "====================( التفعيل )=================="

    ''' <summary>
    ''' كتابة قيمة مفتاح التفعيل في تسجيل ويندوز
    ''' </summary>
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
            MessageBox.Show("يحتاج البرنامج لصلاحيات مدير (Administrator) لتفعيله.", "صلاحيات غير كافية", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                Return CStr(value)
            Else
                Return ""
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في قراءة قيمة التسجيل: {ex.Message}")
            Return ""
        Finally
            If regKey IsNot Nothing Then
                Try
                    regKey.Close()
                Catch
                End Try
            End If
        End Try
    End Function

    ''' <summary>
    ''' التحقق من صحة مفتاح التفعيل
    ''' </summary>
    Public Function ValidateActivationKey(inputKey As String) As Boolean
        Try
            Dim storedKey As String = GetRegValue()
            If String.IsNullOrWhiteSpace(inputKey) OrElse String.IsNullOrWhiteSpace(storedKey) Then
                Return False
            End If
            ' المقارنة الحساسة لحالة الأحرف
            Return inputKey.Trim() = storedKey.Trim()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من مفتاح التفعيل: {ex.Message}")
            Return False
        End Try
    End Function

#End Region

End Module
