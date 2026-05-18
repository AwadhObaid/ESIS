Imports System.IO
Imports Microsoft.WindowsAPICodePack.Dialogs

Public Class ArchiveVariablesForm

    Private Sub ArchiveVariablesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' تحميل إعدادات المستخدم الحالي من الجدول
            ' يعتمد على GlobalData.LoggedInUserID
            ShowArchiveVariables(Me)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            ClearConfirmMsgTools(Me.lblConfirmMsg, Me.PicMsg, Me.Timer1)
        Catch
        End Try
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        ' تفعيل التابات وتفريغ الحقول (بدون تغيير التصميم)
        ClearContolOfFrmArchiveVariables(Me)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveArchiveVariables(Me)
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        UpdateArchiveVariables(Me)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        MessageBox.Show("معذرةً، لا يمكن حذف متغيرات الأرشيف. يمكن تعديلها فقط.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    '=========================
    ' أزرار اختيار المسارات
    '=========================
    Private Sub btnFolderBrows1_Click(sender As Object, e As EventArgs) Handles btnFolderBrows1.Click
        Dim dialog As New CommonOpenFileDialog()

        With dialog
            .IsFolderPicker = True          ' اختيار مجلد وليس ملف
            .Title = "اختر مسار حفظ الأرشيف الخارجي"
            .EnsurePathExists = True        ' التأكد من وجود المجلد
            .AllowNonFileSystemItems = False
            .Multiselect = False

            ' تحميل المسار الحالي (إن وجد)
            If Not String.IsNullOrWhiteSpace(ExternalSoredPath.Text) AndAlso Directory.Exists(ExternalSoredPath.Text) Then
                .InitialDirectory = ExternalSoredPath.Text
            End If
        End With

        ' عرض الحوار
        If dialog.ShowDialog() = CommonFileDialogResult.Ok Then
            ' تعيين المسار المختار إلى TextBox
            ExternalSoredPath.Text = dialog.FileName
        End If


        'Using f As New FolderBrowserDialog()
        '    f.Description = "اختر مسار حفظ الأرشيف الخارجي"
        '    If Directory.Exists(ExternalSoredPath.Text) Then f.SelectedPath = ExternalSoredPath.Text
        '    If f.ShowDialog(Me) = DialogResult.OK Then
        '        ExternalSoredPath.Text = f.SelectedPath
        '    End If
        'End Using
    End Sub

    Private Sub btnFolderBrows2_Click(sender As Object, e As EventArgs) Handles btnFolderBrows2.Click
        Dim dialog As New CommonOpenFileDialog()

        With dialog
            .IsFolderPicker = True          ' اختيار مجلد وليس ملف
            .Title = "اختر مسار حفظ صور المسح الضوئي"
            .EnsurePathExists = True        ' التأكد من وجود المجلد
            .AllowNonFileSystemItems = False
            .Multiselect = False

            ' تحميل المسار الحالي (إن وجد)
            If Not String.IsNullOrWhiteSpace(ScandImagePath.Text) AndAlso Directory.Exists(ScandImagePath.Text) Then
                .InitialDirectory = ScandImagePath.Text
            End If
        End With

        ' عرض الحوار
        If dialog.ShowDialog() = CommonFileDialogResult.Ok Then
            ' تعيين المسار المختار إلى TextBox
            ScandImagePath.Text = dialog.FileName
        End If

    End Sub

    Private Sub btnFolderBrows3_Click(sender As Object, e As EventArgs) Handles btnFolderBrows3.Click
        Dim dialog As New CommonOpenFileDialog()

        With dialog
            .IsFolderPicker = True          ' اختيار مجلد وليس ملف
            .Title = "اختر مسار حفظ ملفات PDF"
            .EnsurePathExists = True        ' التأكد من وجود المجلد
            .AllowNonFileSystemItems = False
            .Multiselect = False

            ' تحميل المسار الحالي (إن وجد)
            If Not String.IsNullOrWhiteSpace(PDFStoredPath.Text) AndAlso Directory.Exists(PDFStoredPath.Text) Then
                .InitialDirectory = PDFStoredPath.Text
            End If
        End With

        ' عرض الحوار
        If dialog.ShowDialog() = CommonFileDialogResult.Ok Then
            ' تعيين المسار المختار إلى TextBox
            PDFStoredPath.Text = dialog.FileName
        End If
    End Sub

    Private Sub btnBrowseSearchDefaultPath_Click(sender As Object, e As EventArgs) Handles btnBrowseSearchDefaultPath.Click
        Dim dialog As New CommonOpenFileDialog()

        With dialog
            .IsFolderPicker = True          ' اختيار مجلد وليس ملف
            .Title = "اختر مسار البحث الافتراضي"
            .EnsurePathExists = True        ' التأكد من وجود المجلد
            .AllowNonFileSystemItems = False
            .Multiselect = False

            ' تحميل المسار الحالي (إن وجد)
            If Not String.IsNullOrWhiteSpace(txtSearchDefaultPath.Text) AndAlso Directory.Exists(txtSearchDefaultPath.Text) Then
                .InitialDirectory = ExternalSoredPath.Text
            End If
        End With

        ' عرض الحوار
        If dialog.ShowDialog() = CommonFileDialogResult.Ok Then
            ' تعيين المسار المختار إلى TextBox
            txtSearchDefaultPath.Text = dialog.FileName
        End If

    End Sub

    '=========================
    ' إظهار/إخفاء كلمات المرور
    '=========================
    Private Sub CkshowHide_CheckedChanged(sender As Object, e As EventArgs) Handles CkshowHide.CheckedChanged
        EmailAppPassword.UseSystemPasswordChar = Not CkshowHide.Checked
        CkshowHide.Text = If(CkshowHide.Checked, "إخفاء", "إظهار")
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        txtPassCon.UseSystemPasswordChar = Not CheckBox1.Checked
        CheckBox1.Text = If(CheckBox1.Checked, "إخفاء", "إظهار")
    End Sub

    Private Sub btnYearManagementForm_Click(sender As Object, e As EventArgs) Handles btnYearManagementForm.Click
        Dim frm As New YearManagementForm
        frm.ShowDialog()
    End Sub

End Class




'Imports System.Data
'Imports System.Data.SqlClient
'Imports System.IO
'Imports ALquhaliLibrary.DBCLS

'Public Class ArchiveVariablesForm

'    '========================================================
'    '  إعدادات متعددة لكل مستخدم
'    '  الفكرة: نستخدم جدول ArchiveVariables نفسه لكن يكون:
'    '  ID = LoggedInUserID  (بدل ID = 1)
'    '  ملاحظة: لأن ID نوعه tinyint يجب أن يكون رقم المستخدم 0..255
'    '========================================================

'    Private Function GetUserKey() As Byte
'        ' LoggedInUserID متغير موجود عندك في النظام (يتم تعيينه عند تسجيل الدخول)
'        If LoggedInUserID < 0 OrElse LoggedInUserID > 255 Then
'            Throw New Exception("رقم المستخدم LoggedInUserID أكبر من 255 بينما عمود ID في ArchiveVariables نوعه tinyint. إمّا غيّر نوع ID إلى INT أو اجعل رقم المستخدم <= 255.")
'        End If
'        Return CByte(LoggedInUserID)
'    End Function

'    Private Function SafeGetString(row As DataRow, columnName As String) As String
'        If row Is Nothing OrElse row.IsNull(columnName) Then Return ""
'        Return row(columnName).ToString()
'    End Function

'    Private Function SafeGetInt(row As DataRow, columnName As String, defaultValue As Integer) As Integer
'        If row Is Nothing OrElse row.IsNull(columnName) Then Return defaultValue
'        Dim v As Integer
'        If Integer.TryParse(row(columnName).ToString(), v) Then Return v
'        Return defaultValue
'    End Function

'    Private Function SafeGetByte(row As DataRow, columnName As String, defaultValue As Byte) As Byte
'        If row Is Nothing OrElse row.IsNull(columnName) Then Return defaultValue
'        Dim v As Integer
'        If Integer.TryParse(row(columnName).ToString(), v) Then
'            If v < 0 Then v = 0
'            If v > 255 Then v = 255
'            Return CByte(v)
'        End If
'        Return defaultValue
'    End Function

'    Private Sub ApplyDefaults()
'        ' قيم افتراضية
'        If ImageExtension.Items.Count > 0 Then ImageExtension.SelectedIndex = 0
'        ScandImageName.Text = "ScanImage"
'        ScandImagePath.Text = Path.GetTempPath()
'        nudNotificationInterval.Value = 5
'        txtdaysToKeep.Text = "0"

'        btnSave.Enabled = True
'        btnUpdate.Enabled = False
'        btnDelete.Enabled = False
'        btnClose.Enabled = True
'    End Sub

'    Private Function EnsureOpen(cn As SqlConnection) As SqlConnection
'        If cn Is Nothing Then Throw New Exception("الاتصال بقاعدة البيانات غير متوفر.")
'        If cn.State <> ConnectionState.Open Then cn.Open()
'        Return cn
'    End Function

'    Private Function RecordExists(userKey As Byte) As Boolean
'        Dim cn As SqlConnection = EnsureOpen(Con())
'        Using cmd As New SqlCommand("SELECT COUNT(1) FROM ArchiveVariables WHERE ID=@ID", cn)
'            cmd.Parameters.AddWithValue("@ID", userKey)
'            Dim c = Convert.ToInt32(cmd.ExecuteScalar())
'            Return c > 0
'        End Using
'    End Function

'    '========================
'    '  تحميل الإعدادات عند فتح النموذج
'    '========================
'    Private Sub ArchiveVariablesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        Try
'            ShowArchiveVariables(Me)
'        Catch ex As Exception
'            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            ApplyDefaults()
'        End Try
'    End Sub

'    '========================
'    '  عرض الإعدادات (لكل مستخدم حسب LoggedInUserID)
'    '========================
'    Private Sub ShowArchiveVariables(ByVal xx As ArchiveVariablesForm)
'        Dim userKey As Byte = GetUserKey()

'        Dim dt As New DataTable
'        Dim cn As SqlConnection = EnsureOpen(Con())

'        Using cmd As New SqlCommand("SELECT * FROM ArchiveVariables WHERE ID=@ID", cn)
'            cmd.Parameters.AddWithValue("@ID", userKey)
'            Using da As New SqlDataAdapter(cmd)
'                da.Fill(dt)
'            End Using
'        End Using

'        If dt.Rows.Count = 0 Then
'            ApplyDefaults()
'            Return
'        End If

'        Dim r = dt.Rows(0)

'        ' النصوص
'        xx.ExternalSoredPath.Text = SafeGetString(r, "ExternalSoredPath")
'        xx.ScandImagePath.Text = SafeGetString(r, "ScandImagePath")
'        xx.PDFStoredPath.Text = SafeGetString(r, "PDFStoredPath")
'        xx.txtSearchDefaultPath.Text = SafeGetString(r, "PathSearch")
'        xx.txtlastSavedNumber.Text = SafeGetString(r, "LastSaderNumber")
'        xx.DefualtSenderEmail.Text = SafeGetString(r, "DefualtSenderEmail")
'        xx.EmailAppPassword.Text = SafeGetString(r, "EmailAppPassword")
'        xx.SMTPServer.Text = SafeGetString(r, "SMTPServer")
'        xx.PortNumber.Text = SafeGetString(r, "PortNumber")
'        xx.ScandImageName.Text = If(String.IsNullOrWhiteSpace(SafeGetString(r, "ScandImageName")), "ScanImage", SafeGetString(r, "ScandImageName"))

'        ' رقميات
'        xx.txtdaysToKeep.Text = SafeGetInt(r, "daysToKeep", 0).ToString()

'        Dim intervalVal As Integer = SafeGetInt(r, "NotificationsFormInterval", 5)
'        If intervalVal < CInt(nudNotificationInterval.Minimum) Then intervalVal = CInt(nudNotificationInterval.Minimum)
'        If intervalVal > CInt(nudNotificationInterval.Maximum) Then intervalVal = CInt(nudNotificationInterval.Maximum)
'        xx.nudNotificationInterval.Value = intervalVal

'        ' امتداد الصور
'        Dim imgExt As Integer = SafeGetInt(r, "ImageExtension", 0)
'        If imgExt >= 0 AndAlso imgExt < xx.ImageExtension.Items.Count Then
'            xx.ImageExtension.SelectedIndex = imgExt
'        ElseIf xx.ImageExtension.Items.Count > 0 Then
'            xx.ImageExtension.SelectedIndex = 0
'        End If

'        ' فك تشفير كلمة المرور (إن وجدت)
'        Try
'            Dim enc = SafeGetString(r, "FrmConStrPASS")
'            xx.txtPassCon.Text = If(String.IsNullOrWhiteSpace(enc), "", Decrypt(enc))
'        Catch
'            xx.txtPassCon.Text = ""
'        End Try

'        ' أزرار
'        xx.btnSave.Enabled = False
'        xx.btnUpdate.Enabled = True
'        xx.btnDelete.Enabled = True
'        xx.btnClose.Enabled = True
'    End Sub

'    '========================
'    '  حفظ (Insert/Update) — اسمها نفس اسم دالتك القديمة
'    '========================
'    Private Sub SaveArchiveVariables(ByVal xx As ArchiveVariablesForm)

'        Dim userKey As Byte = GetUserKey()
'        Dim cn As SqlConnection = EnsureOpen(Con())

'        Dim exists As Boolean = RecordExists(userKey)

'        Dim days As Integer = 0
'        Integer.TryParse(xx.txtdaysToKeep.Text.Trim(), days)
'        If days < 0 Then days = 0
'        If days > 255 Then days = 255

'        Dim intervalVal As Integer = CInt(xx.nudNotificationInterval.Value)
'        If intervalVal < 0 Then intervalVal = 0
'        If intervalVal > 255 Then intervalVal = 255

'        Dim imgExtIdx As Integer = xx.ImageExtension.SelectedIndex
'        If imgExtIdx < 0 Then imgExtIdx = 0

'        Dim encPass As Object = DBNull.Value
'        If Not String.IsNullOrWhiteSpace(xx.txtPassCon.Text) Then
'            encPass = Encrypt(xx.txtPassCon.Text.Trim())
'        End If

'        If Not exists Then
'            ' INSERT
'            Using cmd As New SqlCommand("
'INSERT INTO ArchiveVariables
'(ID, ExternalSoredPath, ScandImagePath, ScandImageName, ImageExtension, PDFStoredPath,
' DefualtSenderEmail, EmailAppPassword, SMTPServer, PortNumber, FrmConStrPASS,
' PathSearch, LastSaderNumber, daysToKeep, NotificationsFormInterval, AddUser, AddDate)
'VALUES
'(@ID, @ExternalSoredPath, @ScandImagePath, @ScandImageName, @ImageExtension, @PDFStoredPath,
' @DefualtSenderEmail, @EmailAppPassword, @SMTPServer, @PortNumber, @FrmConStrPASS,
' @PathSearch, @LastSaderNumber, @daysToKeep, @NotificationsFormInterval, @AddUser, GETDATE())", cn)

'                cmd.Parameters.AddWithValue("@ID", userKey)
'                cmd.Parameters.AddWithValue("@ExternalSoredPath", If(String.IsNullOrWhiteSpace(xx.ExternalSoredPath.Text), DBNull.Value, xx.ExternalSoredPath.Text.Trim()))
'                cmd.Parameters.AddWithValue("@ScandImagePath", If(String.IsNullOrWhiteSpace(xx.ScandImagePath.Text), DBNull.Value, xx.ScandImagePath.Text.Trim()))
'                cmd.Parameters.AddWithValue("@ScandImageName", If(String.IsNullOrWhiteSpace(xx.ScandImageName.Text), DBNull.Value, xx.ScandImageName.Text.Trim()))
'                cmd.Parameters.AddWithValue("@ImageExtension", imgExtIdx)
'                cmd.Parameters.AddWithValue("@PDFStoredPath", If(String.IsNullOrWhiteSpace(xx.PDFStoredPath.Text), DBNull.Value, xx.PDFStoredPath.Text.Trim()))

'                cmd.Parameters.AddWithValue("@DefualtSenderEmail", If(String.IsNullOrWhiteSpace(xx.DefualtSenderEmail.Text), DBNull.Value, xx.DefualtSenderEmail.Text.Trim()))
'                cmd.Parameters.AddWithValue("@EmailAppPassword", If(String.IsNullOrWhiteSpace(xx.EmailAppPassword.Text), DBNull.Value, xx.EmailAppPassword.Text))
'                cmd.Parameters.AddWithValue("@SMTPServer", If(String.IsNullOrWhiteSpace(xx.SMTPServer.Text), DBNull.Value, xx.SMTPServer.Text.Trim()))
'                cmd.Parameters.AddWithValue("@PortNumber", If(String.IsNullOrWhiteSpace(xx.PortNumber.Text), DBNull.Value, xx.PortNumber.Text.Trim()))
'                cmd.Parameters.AddWithValue("@FrmConStrPASS", encPass)

'                cmd.Parameters.AddWithValue("@PathSearch", If(String.IsNullOrWhiteSpace(xx.txtSearchDefaultPath.Text), DBNull.Value, xx.txtSearchDefaultPath.Text.Trim()))
'                cmd.Parameters.AddWithValue("@LastSaderNumber", If(String.IsNullOrWhiteSpace(xx.txtlastSavedNumber.Text), DBNull.Value, xx.txtlastSavedNumber.Text.Trim()))
'                cmd.Parameters.AddWithValue("@daysToKeep", CByte(days))
'                cmd.Parameters.AddWithValue("@NotificationsFormInterval", CByte(intervalVal))

'                cmd.Parameters.AddWithValue("@AddUser", userKey)

'                cmd.ExecuteNonQuery()
'            End Using
'        Else
'            ' UPDATE
'            Using cmd As New SqlCommand("
'UPDATE ArchiveVariables SET
'    ExternalSoredPath=@ExternalSoredPath,
'    ScandImagePath=@ScandImagePath,
'    ScandImageName=@ScandImageName,
'    ImageExtension=@ImageExtension,
'    PDFStoredPath=@PDFStoredPath,

'    DefualtSenderEmail=@DefualtSenderEmail,
'    EmailAppPassword=@EmailAppPassword,
'    SMTPServer=@SMTPServer,
'    PortNumber=@PortNumber,
'    FrmConStrPASS=@FrmConStrPASS,

'    PathSearch=@PathSearch,
'    LastSaderNumber=@LastSaderNumber,
'    daysToKeep=@daysToKeep,
'    NotificationsFormInterval=@NotificationsFormInterval,

'    EditUser=@EditUser,
'    EditDate=GETDATE()
'WHERE ID=@ID", cn)

'                cmd.Parameters.AddWithValue("@ID", userKey)
'                cmd.Parameters.AddWithValue("@ExternalSoredPath", If(String.IsNullOrWhiteSpace(xx.ExternalSoredPath.Text), DBNull.Value, xx.ExternalSoredPath.Text.Trim()))
'                cmd.Parameters.AddWithValue("@ScandImagePath", If(String.IsNullOrWhiteSpace(xx.ScandImagePath.Text), DBNull.Value, xx.ScandImagePath.Text.Trim()))
'                cmd.Parameters.AddWithValue("@ScandImageName", If(String.IsNullOrWhiteSpace(xx.ScandImageName.Text), DBNull.Value, xx.ScandImageName.Text.Trim()))
'                cmd.Parameters.AddWithValue("@ImageExtension", imgExtIdx)
'                cmd.Parameters.AddWithValue("@PDFStoredPath", If(String.IsNullOrWhiteSpace(xx.PDFStoredPath.Text), DBNull.Value, xx.PDFStoredPath.Text.Trim()))

'                cmd.Parameters.AddWithValue("@DefualtSenderEmail", If(String.IsNullOrWhiteSpace(xx.DefualtSenderEmail.Text), DBNull.Value, xx.DefualtSenderEmail.Text.Trim()))
'                cmd.Parameters.AddWithValue("@EmailAppPassword", If(String.IsNullOrWhiteSpace(xx.EmailAppPassword.Text), DBNull.Value, xx.EmailAppPassword.Text))
'                cmd.Parameters.AddWithValue("@SMTPServer", If(String.IsNullOrWhiteSpace(xx.SMTPServer.Text), DBNull.Value, xx.SMTPServer.Text.Trim()))
'                cmd.Parameters.AddWithValue("@PortNumber", If(String.IsNullOrWhiteSpace(xx.PortNumber.Text), DBNull.Value, xx.PortNumber.Text.Trim()))
'                cmd.Parameters.AddWithValue("@FrmConStrPASS", encPass)

'                cmd.Parameters.AddWithValue("@PathSearch", If(String.IsNullOrWhiteSpace(xx.txtSearchDefaultPath.Text), DBNull.Value, xx.txtSearchDefaultPath.Text.Trim()))
'                cmd.Parameters.AddWithValue("@LastSaderNumber", If(String.IsNullOrWhiteSpace(xx.txtlastSavedNumber.Text), DBNull.Value, xx.txtlastSavedNumber.Text.Trim()))
'                cmd.Parameters.AddWithValue("@daysToKeep", CByte(days))
'                cmd.Parameters.AddWithValue("@NotificationsFormInterval", CByte(intervalVal))

'                cmd.Parameters.AddWithValue("@EditUser", userKey)

'                cmd.ExecuteNonQuery()
'            End Using
'        End If

'        ' رسائل/أزرار
'        Try
'            SavedSuccessfully(xx.lblConfirmMsg, xx.PicMsg, xx.Timer1)
'        Catch
'            ' لو الدالة غير موجودة في مشروعك
'        End Try

'        xx.btnSave.Enabled = False
'        xx.btnUpdate.Enabled = True
'        xx.btnDelete.Enabled = True
'        xx.btnClose.Enabled = True

'    End Sub

'    '========================
'    '  تحديث — اسمها نفس اسم دالتك القديمة
'    '  (نعمل Upsert لتفادي مشاكل عدم وجود سجل)
'    '========================
'    Private Sub UpdateArchiveVariables(ByVal xx As ArchiveVariablesForm)
'        SaveArchiveVariables(xx)
'        Try
'            UpdetedSuccessfully(xx.lblConfirmMsg, xx.PicMsg, xx.Timer1)
'        Catch
'        End Try
'    End Sub

'    '========================================================
'    '  أحداث الأزرار الموجودة أصلاً (بدون تغيير التصميم)
'    '========================================================

'    Private Sub btnFolderBrows1_Click(sender As Object, e As EventArgs) Handles btnFolderBrows1.Click
'        Dim FBD As New FolderBrowserDialog
'        FBD.RootFolder = Environment.SpecialFolder.MyComputer
'        If FBD.ShowDialog = DialogResult.OK Then
'            Me.ExternalSoredPath.Text = FBD.SelectedPath
'        End If
'    End Sub

'    Private Sub btnFolderBrows2_Click(sender As Object, e As EventArgs) Handles btnFolderBrows2.Click
'        Dim FBD As New FolderBrowserDialog
'        FBD.RootFolder = Environment.SpecialFolder.MyComputer
'        If FBD.ShowDialog = DialogResult.OK Then
'            Me.ScandImagePath.Text = FBD.SelectedPath
'        End If
'    End Sub

'    Private Sub btnFolderBrows3_Click(sender As Object, e As EventArgs) Handles btnFolderBrows3.Click
'        Dim FBD As New FolderBrowserDialog
'        FBD.RootFolder = Environment.SpecialFolder.MyComputer
'        If FBD.ShowDialog = DialogResult.OK Then
'            Me.PDFStoredPath.Text = FBD.SelectedPath
'        End If
'    End Sub

'    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
'        SaveArchiveVariables(Me)
'    End Sub

'    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
'        UpdateArchiveVariables(Me)
'    End Sub

'    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
'        MessageBox.Show("معذرة ‘ لايمكن حذف متغيرات الإرشيف يمكن فقط تعديلها.", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information)
'    End Sub

'    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
'        Me.Close()
'    End Sub

'    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
'        If Me.CheckBox1.Checked = True Then
'            Me.txtPassCon.UseSystemPasswordChar = False
'            Me.CheckBox1.Text = "إخفاء"
'        Else
'            Me.txtPassCon.UseSystemPasswordChar = True
'            Me.CheckBox1.Text = "إظهار"
'        End If
'    End Sub

'    Private Sub btnBrowseSearchDefaultPath_Click(sender As Object, e As EventArgs) Handles btnBrowseSearchDefaultPath.Click
'        Dim FBD As New FolderBrowserDialog
'        FBD.RootFolder = Environment.SpecialFolder.MyComputer
'        If FBD.ShowDialog = DialogResult.OK Then
'            Me.txtSearchDefaultPath.Text = FBD.SelectedPath
'        End If
'    End Sub

'    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
'        Try
'            ' 1) تفعيل الأدوات كما كان سابقًا
'            TabControl1.Enabled = True
'            GroupBox1.Enabled = True

'            ' إذا عندك ToolStrip (غيّر الاسم حسب الموجود عندك)
'            ToolStrip1.Enabled = True

'            ' 2) تفريغ الحقول عبر دالتك القديمة (وهي تقوم بتفعيل التاب كنترول أيضًا)
'            Clear_Control_On_TabControls(Me.TabControl1)

'            ' 3) قيم افتراضية (مثل السابق)
'            ImageExtension.SelectedIndex = 0
'            ScandImageName.Text = "ScanImage"
'            ScandImagePath.Text = IO.Path.GetTempPath()

'            ' 4) ضبط الأزرار
'            btnSave.Enabled = True
'            btnUpdate.Enabled = False
'            btnDelete.Enabled = False
'            btnClose.Enabled = True

'        Catch ex As Exception
'            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'End Class
