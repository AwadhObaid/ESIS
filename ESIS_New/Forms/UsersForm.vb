Imports System.Data.SqlClient
Imports System.IO
Imports ALquhaliLibrary.DBCLS
''' <summary>
''' نموذج إدارة المستخدمين مع دعم تغيير كلمة المرور
''' </summary>
Public Class UsersForm
    Inherits BaseFormWithPermissions

    Private usersClass As New UsersClass()
    Private currentUserID As Integer = 0
    Private currentUserImage As Byte() = Nothing ' لتخزين صورة المستخدم الحالية
    Private isLoading As Boolean = False ' متغير للتحكم في حالة التحميل
    ''' <summary>
    ''' تحميل النموذج
    ''' </summary>
    Private Sub UsersForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' تعيين الحالة إلى جاري التحميل
            isLoading = True
            '' إلغاء تحديد جدول المستخدمين
            'If dgvUsers IsNot Nothing Then
            '    dgvUsers.ClearSelection()
            'End If
            txtConfirmPassword.Enabled = False
            txtCurrentPassword.Enabled = False
            txtNewPassword.Enabled = False
            ClearFields()
            LoadUsers()
            ' انتهاء عملية التحميل
            isLoading = False

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' انتهاء عملية التحميل
            isLoading = False
        End Try
    End Sub

    ''' <summary>
    ''' تحميل قائمة المستخدمين
    ''' </summary>
    Private Sub LoadUsers()
        Try
            ' التحقق من أن عنصر DataGridView تم إنشاؤه
            If dgvUsers Is Nothing Then
                MessageBox.Show("عنصر عرض المستخدمين غير مهيأ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim dt As DataTable = usersClass.GetAllUsers()

            ' التحقق من أن الجدول ليس فارغًا
            If dt.Rows.Count = 0 Then
                MessageBox.Show("لا يوجد مستخدمون في قاعدة البيانات", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' ربط البيانات بجدول العرض
            dgvUsers.DataSource = dt

            ' تنسيق الأعمدة
            If dgvUsers.Columns.Contains("ID") Then
                dgvUsers.Columns("ID").Visible = False
            End If

            If dgvUsers.Columns.Contains("NumUser") Then
                dgvUsers.Columns("NumUser").HeaderText = "رقم المستخدم"
            End If

            If dgvUsers.Columns.Contains("UserName") Then
                dgvUsers.Columns("UserName").HeaderText = "اسم المستخدم"
            End If

            If dgvUsers.Columns.Contains("IsActive") Then
                dgvUsers.Columns("IsActive").HeaderText = "نشط"
                dgvUsers.Columns("IsActive").Width = 60
            End If

            If dgvUsers.Columns.Contains("IsAdmin") Then
                dgvUsers.Columns("IsAdmin").HeaderText = "مسؤول"
                dgvUsers.Columns("IsAdmin").Width = 60
            End If

            ' تحسين مظهر جدول العرض
            dgvUsers.AutoResizeColumns()
            dgvUsers.ClearSelection()

        Catch ex As NullReferenceException
            MessageBox.Show($"عنصر عرض المستخدمين غير مهيأ. تأكد من أن عنصر DataGridView معرف بشكل صحيح في ملف المصمم.",
                          "خطأ في التهيئة",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        Catch ex As SqlException
            MessageBox.Show($"خطأ في قاعدة البيانات: {ex.Message}", "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show($"فشل في تحميل المستخدمين: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Sub dgvUsers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUsers.CellClick

    'End Sub

    ''' <summary>
    ''' معالجة تحديد صف في جدول المستخدمين
    ''' </summary>
    Private Sub dgvUsers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUsers.CellClick
        Try
            ' تجاهل الحدث إذا كان أثناء التحميل
            If isLoading Then
                Return
            End If
            If dgvUsers.SelectedRows.Count > 0 AndAlso dgvUsers.SelectedRows(0).Index >= 0 Then
                Dim selectedRow As DataGridViewRow = dgvUsers.SelectedRows(0)

                ' الحصول على معرف المستخدم
                currentUserID = Convert.ToInt32(selectedRow.Cells("ID").Value)

                ' جلب بيانات المستخدم كاملة (بما في ذلك الصورة)
                Dim userData As DataTable = usersClass.GetUserByID(currentUserID)

                If userData.Rows.Count > 0 Then
                    Dim row As DataRow = userData.Rows(0)

                    ' تعبئة الحقول بالبيانات
                    txtNumUser.Text = row("NumUser").ToString()
                    txtUserName.Text = row("UserName").ToString()
                    txtPassword.Text = GetCurrentPassword(currentUserID)
                    chkIsAdmin.Checked = row("IsAdmin").ToString
                    chkIsActive.Checked = row("IsActive").ToString

                    ' تحميل صورة المستخدم
                    LoadUserImage(row("PicUser"))
                End If

                ' تمكين أزرار التعديل والحذف
                btnUpdate.Enabled = True
                btnDelete.Enabled = True
                btnChangePassword.Enabled = True
                btnResetPassword.Enabled = PermissionsManager.Instance.IsUserAdmin(GlobalData.LoggedInUserID) ' فقط للمديرين
            Else
                ' تعطيل أزرار التعديل والحذف
                btnUpdate.Enabled = False
                btnDelete.Enabled = False
                btnChangePassword.Enabled = False
                btnResetPassword.Enabled = False
                ClearImage()
            End If
        Catch ex As Exception
            MessageBox.Show($"خطأ في تحديد المستخدم: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تحميل صورة المستخدم
    ''' </summary>
    Private Sub LoadUserImage(imageData As Object)
        Try
            ' مسح الصورة الحالية
            ClearImage()

            ' التحقق من وجود بيانات صورة
            If imageData IsNot Nothing AndAlso Not DBNull.Value.Equals(imageData) Then
                Dim imageBytes As Byte() = DirectCast(imageData, Byte())

                If imageBytes.Length > 0 Then
                    Using ms As New MemoryStream(imageBytes)
                        PictureBox1.Image = Image.FromStream(ms)
                    End Using

                    ' حفظ بيانات الصورة للتحديث لاحقًا
                    currentUserImage = imageBytes
                End If
            Else
                ' استخدام صورة افتراضية
                PictureBox1.Image = My.Resources.UserDefault ' يمكنك تحديد صورة افتراضية من موارد المشروع
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل صورة المستخدم: {ex.Message}")
            PictureBox1.Image = My.Resources.UserDefault ' صورة افتراضية
        End Try
    End Sub

    ''' <summary>
    ''' مسح الصورة
    ''' </summary>
    Private Sub ClearImage()
        PictureBox1.Image = Nothing
        currentUserImage = Nothing
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر اختيار الصورة
    ''' </summary>
    Private Sub btnBrowseImage_Click(sender As Object, e As EventArgs) Handles btnBrowseImage.Click
        Try
            ' إعداد مربع الحوار لاختيار الصورة
            Using openFileDialog As New OpenFileDialog()
                openFileDialog.Filter = "ملفات الصور|*.jpg;*.jpeg;*.png;*.bmp;*.gif|جميع الملفات|*.*"
                openFileDialog.Title = "اختر صورة المستخدم"
                openFileDialog.Multiselect = False

                If openFileDialog.ShowDialog() = DialogResult.OK Then
                    ' قراءة ملف الصورة إلى بايت آري
                    currentUserImage = File.ReadAllBytes(openFileDialog.FileName)

                    ' عرض الصورة في PictureBox
                    PictureBox1.Image = Image.FromFile(openFileDialog.FileName)
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل الصورة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر الإضافة
    ''' </summary>
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            ' التحقق من صلاحية الإضافة
            If Not CheckPermission("Add", "إضافة مستخدم") Then
                Return
            End If

            ' التحقق من المدخلات
            If String.IsNullOrWhiteSpace(txtNumUser.Text) OrElse String.IsNullOrWhiteSpace(txtUserName.Text) Then
                MessageBox.Show("يرجى إدخال رقم المستخدم واسم المستخدم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التحقق من تكرار رقم المستخدم
            Dim existingUser As DataTable = usersClass.GetUserByNum(Convert.ToInt32(txtNumUser.Text))
            If existingUser.Rows.Count > 0 Then
                MessageBox.Show("رقم المستخدم موجود مسبقًا، يرجى استخدام رقم مختلف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التحقق من كلمة المرور
            If String.IsNullOrWhiteSpace(txtPassword.Text) Then
                MessageBox.Show("يرجى إدخال كلمة المرور", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPassword.Focus()
                Return
            End If

            '' التحقق من طول كلمة المرور
            'If txtPassword.Text.Length < 6 Then
            '    MessageBox.Show("يجب أن تكون كلمة المرور على الأقل 6 أحرف.", "طول كلمة المرور", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            '    txtPassword.Focus()
            '    Return
            'End If

            ' إضافة المستخدم مع الصورة
            Dim result As Integer = usersClass.AddUser(
                Convert.ToInt32(txtNumUser.Text),
                txtUserName.Text,
                txtPassword.Text,
                currentUserImage, ' استخدام صورة المستخدم أو Nothing
                False,   ' ليس مديرًا
                True     ' نشط
            )

            If result > 0 Then
                MessageBox.Show("تمت إضافة المستخدم بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadUsers()
                ClearFields()
            Else
                MessageBox.Show("فشل في إضافة المستخدم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في إضافة المستخدم: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر التحديث
    ''' </summary>
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            '' التحقق من صلاحية التعديل
            'If Not CheckPermission("Edit", "تعديل مستخدم") Then
            '    Return
            'End If

            ' التحقق من تحديد مستخدم
            If dgvUsers.SelectedRows.Count = 0 Then
                MessageBox.Show("يرجى تحديد مستخدم للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التحقق من المدخلات
            If String.IsNullOrWhiteSpace(txtNumUser.Text) OrElse String.IsNullOrWhiteSpace(txtUserName.Text) Then
                MessageBox.Show("يرجى إدخال رقم المستخدم واسم المستخدم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            ' التحقق من تكرار رقم المستخدم (باستثناء الحالي)
            If CheckDuplicateUserNumber(Convert.ToInt32(txtNumUser.Text), currentUserID) Then
                MessageBox.Show("رقم المستخدم موجود مسبقًا، يرجى استخدام رقم مختلف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' الحصول على كلمة المرور الحالية من قاعدة البيانات
            Dim currentPassword As String = GetCurrentPassword(currentUserID)

            ' تحديث المستخدم مع الصورة
            Dim result As Boolean = usersClass.UpdateUser(
            currentUserID,
            Convert.ToInt32(txtNumUser.Text),
            txtUserName.Text,
            currentPassword, ' ← مرر كلمة المرور الحالية إذا لم يتم تغييرها
            currentUserImage, ' استخدام صورة المستخدم الحالية أو الجديدة
            chkIsAdmin.Checked,   ' ← من واجهة المستخدم
            chkIsActive.Checked   ' ← من واجهة المستخدم
        )

            If result Then
                MessageBox.Show("تم تحديث المستخدم بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadUsers()
                ClearFields()
            Else
                MessageBox.Show("فشل في تحديث المستخدم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحديث المستخدم: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '''' <summary>
    '''' معالجة الضغط على زر التحديث
    '''' </summary>
    'Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
    '    Try
    '        ' التحقق من صلاحية التعديل
    '        If Not CheckPermission("Edit", "تعديل مستخدم") Then
    '            Return
    '        End If

    '        ' التحقق من تحديد مستخدم
    '        If dgvUsers.SelectedRows.Count = 0 Then
    '            MessageBox.Show("يرجى تحديد مستخدم للتعديل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            Return
    '        End If

    '        ' التحقق من المدخلات
    '        If String.IsNullOrWhiteSpace(txtNumUser.Text) OrElse String.IsNullOrWhiteSpace(txtUserName.Text) Then
    '            MessageBox.Show("يرجى إدخال رقم المستخدم واسم المستخدم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            Return
    '        End If
    '        ' التحقق من تكرار رقم المستخدم (باستثناء الحالي)
    '        If CheckDuplicateUserNumber(Convert.ToInt32(txtNumUser.Text), currentUserID) Then
    '            MessageBox.Show("رقم المستخدم موجود مسبقًا، يرجى استخدام رقم مختلف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            Return
    '        End If
    '        ' تحديث المستخدم مع الصورة
    '        Dim result As Boolean = usersClass.UpdateUser(
    '            currentUserID,
    '            Convert.ToInt32(txtNumUser.Text),
    '            txtUserName.Text,
    '            "", ' لا تغيير في كلمة المرور هنا (سيتم تغييرها من خلال زر آخر)
    '            currentUserImage, ' استخدام صورة المستخدم الحالية أو الجديدة
    '            chkIsAdmin.Checked,   ' ← من واجهة المستخدم
    '            chkIsActive.Checked   ' ← من واجهة المستخدم
    '        )

    '        If result Then
    '            MessageBox.Show("تم تحديث المستخدم بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            LoadUsers()
    '            ClearFields()
    '        Else
    '            MessageBox.Show("فشل في تحديث المستخدم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End If

    '    Catch ex As Exception
    '        MessageBox.Show($"خطأ في تحديث المستخدم: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    ''' <summary>
    ''' التحقق من تكرار رقم المستخدم (باستثناء المستخدم الحالي)
    ''' </summary>
    Private Function CheckDuplicateUserNumber(numUser As Integer, currentId As Integer) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM TbUsers WHERE NumUser = @NumUser AND ID <> @CurrentId"
            Dim parameters() As SqlParameter = {
            DatabaseHelper.CreateParameter("@NumUser", numUser),
            DatabaseHelper.CreateParameter("@CurrentId", currentId)
        }
            Dim count As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters))
            Return count > 0
        Catch ex As Exception
            Throw New Exception("خطأ في التحقق من تكرار رقم المستخدم: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' معالجة الضغط على زر تغيير كلمة المرور
    ''' </summary>
    Private Sub btnChangePassword_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        Try
            ' التحقق من صلاحية تغيير كلمة المرور
            If Not CheckPermission("ChangePassword", "تغيير كلمة المرور") Then
                Return
            End If

            ' التحقق من تحديد مستخدم
            If dgvUsers.SelectedRows.Count = 0 Then
                MessageBox.Show("يرجى تحديد مستخدم لتغيير كلمة المرور", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التحقق من المدخلات
            If String.IsNullOrWhiteSpace(txtCurrentPassword.Text) OrElse
               String.IsNullOrWhiteSpace(txtNewPassword.Text) OrElse
               String.IsNullOrWhiteSpace(txtConfirmPassword.Text) Then
                MessageBox.Show("يرجى إدخال جميع حقول كلمة المرور", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التحقق من تطابق كلمتي المرور الجديدتين
            If txtNewPassword.Text <> txtConfirmPassword.Text Then
                MessageBox.Show("كلمتا المرور الجديدتان غير متطابقتين", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtNewPassword.Clear()
                txtConfirmPassword.Clear()
                txtNewPassword.Focus()
                Return
            End If

            ' تغيير كلمة المرور
            Dim success As Boolean = usersClass.ChangePassword(
                currentUserID,
                txtCurrentPassword.Text,
                txtNewPassword.Text
            )

            If success Then
                MessageBox.Show("تم تغيير كلمة المرور بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' مسح حقول كلمة المرور بعد النجاح
                txtCurrentPassword.Clear()
                txtNewPassword.Clear()
                txtConfirmPassword.Clear()
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في تغيير كلمة المرور: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر الحذف
    ''' </summary>
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            ' التحقق من صلاحية الحذف
            If Not CheckPermission("Delete", "حذف مستخدم") Then
                Return
            End If

            ' التحقق من تحديد مستخدم
            If dgvUsers.SelectedRows.Count = 0 Then
                MessageBox.Show("يرجى تحديد مستخدم للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التأكيد على الحذف
            Dim result As DialogResult = MessageBox.Show("هل أنت متأكد من حذف هذا المستخدم؟",
                                                        "تأكيد الحذف",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question)

            If result = DialogResult.No Then
                Return
            End If

            ' حذف المستخدم
            Dim success As Boolean = usersClass.DeleteUser(currentUserID)

            If success Then
                MessageBox.Show("تم حذف المستخدم بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadUsers()
                ClearFields()
            Else
                MessageBox.Show("فشل في حذف المستخدم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في حذف المستخدم: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر الإلغاء
    ''' </summary>
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' مسح الحقول
    ''' </summary>
    Private Sub ClearFields()
        txtNumUser.Text = ""
        txtUserName.Text = ""
        txtPassword.Text = ""
        txtCurrentPassword.Clear()
        txtNewPassword.Clear()
        txtConfirmPassword.Clear()
        ClearImage()

        '' إلغاء تحديد جدول المستخدمين
        'If dgvUsers IsNot Nothing Then
        dgvUsers.ClearSelection()
        'End If
        btnUpdate.Enabled = False
        btnDelete.Enabled = False
        btnChangePassword.Enabled = False
        btnResetPassword.Enabled = False
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        ClearFields()
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر إعادة تعيين كلمة المرور (للمديرين فقط)
    ''' </summary>
    Private Sub btnResetPassword_Click(sender As Object, e As EventArgs) Handles btnResetPassword.Click
        Try
            ' التحقق من أن المستخدم الحالي هو مدير
            If Not PermissionsManager.Instance.IsUserAdmin(GlobalData.LoggedInUserID) Then
                MessageBox.Show("هذه الوظيفة متاحة فقط للمديرين.", "صلاحية محدودة", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' التحقق من تحديد مستخدم
            If dgvUsers.SelectedRows.Count = 0 Then
                MessageBox.Show("يرجى تحديد مستخدم لإعادة تعيين كلمة المرور", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التحقق من أن المستخدم الذي يتم تعديله ليس نفسه (لمنع إغلاق الحساب)
            If currentUserID = GlobalData.LoggedInUserID Then
                MessageBox.Show("لا يمكنك إعادة تعيين كلمة المرور لنفسك.", "عملية غير مسموحة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التحقق من المدخلات
            If String.IsNullOrWhiteSpace(txtNewPassword.Text) OrElse
               String.IsNullOrWhiteSpace(txtConfirmPassword.Text) Then
                MessageBox.Show("يرجى إدخال كلمة المرور الجديدة وتأكيدها", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التحقق من تطابق كلمتي المرور
            If txtNewPassword.Text <> txtConfirmPassword.Text Then
                MessageBox.Show("كلمتا المرور غير متطابقتين", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtNewPassword.Clear()
                txtConfirmPassword.Clear()
                txtNewPassword.Focus()
                Return
            End If

            '' التحقق من طول كلمة المرور
            'If txtNewPassword.Text.Length < 6 Then
            '    MessageBox.Show("يجب أن تكون كلمة المرور على الأقل 6 أحرف.", "طول كلمة المرور", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            '    txtNewPassword.Focus()
            '    Return
            'End If

            ' عرض رسالة تأكيد
            Dim result As DialogResult = MessageBox.Show(
                "هل أنت متأكد من رغبتك في إعادة تعيين كلمة مرور هذا المستخدم؟" & vbCrLf &
                "لن يكون لدى المستخدم أي فكرة عن كلمة المرور الجديدة." & vbCrLf &
                "يجب عليك إبلاغه بكلمة المرور الجديدة بشكل آمن.",
                "تأكيد إعادة تعيين كلمة المرور",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

            If result = DialogResult.No Then
                Return
            End If

            ' إعادة تعيين كلمة المرور
            Dim success As Boolean = usersClass.ResetPassword(
                currentUserID,
                txtNewPassword.Text
            )

            If success Then
                MessageBox.Show(
                    "تم إعادة تعيين كلمة المرور بنجاح." & vbCrLf &
                    "يجب الآن إبلاغ المستخدم بكلمة المرور الجديدة بشكل آمن.",
                    "نجاح",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information)

                ' مسح حقول كلمة المرور بعد النجاح
                txtNewPassword.Clear()
                txtConfirmPassword.Clear()
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في إعادة تعيين كلمة المرور: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' الحصول على كلمة المرور الحالية للمستخدم
    ''' </summary>
    Private Function GetCurrentPassword(userID As Integer) As String
        Try
            Dim query As String = "SELECT Password FROM TbUsers WHERE ID = @UserID"
            Dim parameters() As SqlParameter = {
                DatabaseHelper.CreateParameter("@UserID", userID)
            }

            Dim result As Object = DatabaseHelper.ExecuteScalar(query, parameters)
            If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                Return result.ToString()
            End If

            Return ""
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على كلمة المرور: {ex.Message}")
            Return ""
        End Try
    End Function

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            txtConfirmPassword.Enabled = True
            txtCurrentPassword.Enabled = True
            txtNewPassword.Enabled = True
        Else
            txtConfirmPassword.Enabled = False
            txtCurrentPassword.Enabled = False
            txtNewPassword.Enabled = False
        End If
    End Sub


End Class






'Imports System.Data.SqlClient
'Imports System.IO

'Public Class UsersForm : Inherits ESIS_New.BaseFormWithPermissions
'    Private currentUserID As Integer = 0
'    Private isEditing As Boolean = False
'    Private userImageBytes As Byte() = Nothing
'    'Private _canAccess As Boolean = False

'    ''' <summary>
'    ''' Constructor
'    ''' </summary>
'    Public Sub New()
'        MyBase.New()

'        '' التحقق من الصلاحية قبل تحميل أي شيء
'        '_canAccess = clsPermissions.HasPermission(GlobalData.LoggedInUserID, Me.Name, "View")

'        'If Not _canAccess Then
'        '    ' عدم تحميل العناصر غير الضرورية
'        '    Me.StartPosition = FormStartPosition.Manual
'        '    Me.Size = New Size(0, 0)
'        '    Me.Location = New Point(-1000, -1000) ' خارج الشاشة
'        'End If
'    End Sub

'    Private Sub frmUsers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        Try
'            ' التحقق من صلاحية عرض نموذج المستخدمين
'            If Not CheckPermission(0, "عرض نموذج المستخدمين") Then
'                ' استخدام Timer لإغلاق النموذج بعد اكتمال إنشاء العناصر
'                Dim closeTimer As New Timer()
'                closeTimer.Interval = 100 ' 100 ملي ثانية
'                AddHandler closeTimer.Tick, Sub(senderTimer As Object, eTimer As EventArgs)
'                                                closeTimer.Stop()
'                                                closeTimer.Dispose()
'                                                Me.Close()
'                                            End Sub
'                closeTimer.Start()
'                Return
'            End If
'            LoadUsers()
'            ApplyPermissions()
'            ResetForm()


'        Catch ex As Exception
'            MessageBox.Show($"خطأ في تحميل النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            Me.Close()
'        End Try



'        ' تسجيل النموذج للحصول على إشعارات تحديث الصلاحيات
'        PermissionEventManager.Instance.RegisterPermissionChangedHandler(AddressOf RefreshPermissionsHandler)
'    End Sub
'    Private Sub RefreshPermissionsHandler()
'        ' تحديث الصلاحيات في النموذج
'        'ApplyCustomPermissions()
'    End Sub

'    'Protected Overrides Sub ApplyCustomPermissions()
'    '    Try
'    '        ' تطبيق الصلاحيات على عناصر النموذج
'    '        btnAdd.Enabled = PermissionHelperDeleted.CanAdd()
'    '        btnUpdate.Enabled = PermissionHelperDeleted.CanEdit()
'    '        btnDelete.Enabled = PermissionHelperDeleted.CanDelete()
'    '        btnSearch.Enabled = PermissionHelperDeleted.CanSearch()
'    '        'btnPrint.Enabled = PermissionHelper.CanPrint()
'    '        'btnManageUsers.Enabled = PermissionHelper.CanManageUsers()

'    '        '' عناصر إضافية خاصة بهذا النموذج
'    '        'cmbDepartment.Enabled = PermissionHelper.CanEdit()
'    '        'chkIsActive.Enabled = PermissionHelper.CanEdit()

'    '        ' عرض رسالة إذا لم تكن هناك صلاحيات كافية
'    '        If Not (PermissionHelperDeleted.CanAdd() OrElse PermissionHelperDeleted.CanEdit() OrElse PermissionHelperDeleted.CanDelete()) Then
'    '            lblWarning.Text = "ملاحظة: لا تملك صلاحيات كافية لتعديل البيانات"
'    '            lblWarning.ForeColor = Color.Red
'    '            lblWarning.Visible = True
'    '        Else
'    '            lblWarning.Visible = False
'    '        End If
'    '    Catch ex As Exception
'    '        System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق الصلاحيات في UsersForm: {ex.Message}")
'    '    End Try
'    'End Sub


'    Private Sub LoadUsers()
'        Try
'            Dim query As String = "SELECT * FROM TbUsers"
'            Dim dt As DataTable = DatabaseHelper.GetData(query, Nothing, False)

'            dgvUsers.DataSource = dt

'            ' تنسيق الأعمدة
'            dgvUsers.Columns("ID").Visible = False
'            dgvUsers.Columns("NumUser").HeaderText = "رقم المستخدم"
'            dgvUsers.Columns("UserName").HeaderText = "اسم المستخدم"
'            dgvUsers.Columns("IsActive").HeaderText = "نشط"
'            dgvUsers.Columns("IsAdmin").HeaderText = "مسؤول"

'        Catch ex As Exception
'            MessageBox.Show("فشل في تحميل المستخدمين: " & ex.Message)
'        End Try
'    End Sub

'    Private Sub ApplyPermissions()
'        Try
'            Dim currentUserID As Integer = GlobalData.LoggedInUserID
'            Dim isAdmin As Boolean = CheckIfAdmin(currentUserID)

'            ' المسؤول لديه جميع الصلاحيات
'            btnAdd.Enabled = isAdmin Or clsPermissions.HasPermission(currentUserID, "UsersForm", "Add")
'            btnUpdate.Enabled = isAdmin Or clsPermissions.HasPermission(currentUserID, "UsersForm", "Edit")
'            btnDelete.Enabled = isAdmin Or clsPermissions.HasPermission(currentUserID, "UsersForm", "Delete")
'            'btnSearch.Enabled = isAdmin Or clsPermissions.HasPermission(currentUserID, "UsersForm", "Search")
'            'btnResetPassword.Enabled = isAdmin

'            ' غير المسؤولين لا يمكنهم تعديل المسؤولين
'            If Not isAdmin Then
'                dgvUsers.ReadOnly = True
'            End If

'        Catch ex As Exception
'            MessageBox.Show("فشل في تطبيق الصلاحيات: " & ex.Message)
'        End Try
'    End Sub

'    Private Function CheckIfAdmin(userID As Integer) As Boolean
'        Try
'            Dim query As String = "SELECT IsAdmin FROM TbUsers WHERE ID = @UserID"
'            Dim params() As SqlParameter = {New SqlParameter("@UserID", userID)}

'            Dim result As Object = DatabaseHelper.ExecuteScalar(query, params, False)
'            Return result IsNot Nothing AndAlso CBool(result)
'        Catch ex As Exception
'            Return False
'        End Try
'    End Function

'    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
'        ' التحقق من صلاحية إضافة مستخدم
'        If Not CheckPermission(1, "إضافة مستخدم") Then
'            Return
'        End If

'        If ValidateInput() Then
'            Try
'                AddUser()
'                LoadUsers()
'                ResetForm()
'                MessageBox.Show("تمت إضافة المستخدم بنجاح")
'            Catch ex As Exception
'                MessageBox.Show("فشل في إضافة المستخدم: " & ex.Message)
'            End Try
'        End If
'    End Sub

'    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
'        'التحقق من صلاحية تعديل مستخدم
'        If Not CheckPermission(2, "تعديل مستخدم") Then
'            Return
'        End If

'        If currentUserID = 0 Then
'            MessageBox.Show("يرجى تحديد مستخدم للتعديل")
'            Return
'        End If

'        If ValidateInput() Then
'            Try
'                UpdateUser()
'                LoadUsers()
'                ResetForm()
'                MessageBox.Show("تم تحديث بيانات المستخدم بنجاح")
'            Catch ex As Exception
'                MessageBox.Show("فشل في تحديث المستخدم: " & ex.Message)
'            End Try
'        End If
'    End Sub

'    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
'        ' التحقق من صلاحية حذف مستخدم
'        If Not CheckPermission(3, "حذف مستخدم") Then
'            Return
'        End If

'        If currentUserID = 0 Then
'            MessageBox.Show("يرجى تحديد مستخدم للحذف")
'            Return
'        End If

'        If MessageBox.Show("هل أنت متأكد من حذف المستخدم؟", "تأكيد الحذف",
'                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
'            Try
'                DeleteUser()
'                LoadUsers()
'                ResetForm()
'                MessageBox.Show("تم حذف المستخدم بنجاح")
'            Catch ex As Exception
'                MessageBox.Show("فشل في حذف المستخدم: " & ex.Message)
'            End Try
'        End If
'    End Sub

'    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
'        Try
'            Dim field As String = cboSearchField.Text
'            Dim value As String = txtSearch.Text

'            Dim query As String = ""

'            Select Case field
'                Case "رقم المستخدم"
'                    query = "SELECT * FROM TbUsers WHERE NumUser LIKE '%' + @Value + '%'"
'                Case "اسم المستخدم"
'                    query = "SELECT * FROM TbUsers WHERE UserName LIKE '%' + @Value + '%'"
'                Case Else
'                    query = "SELECT * FROM TbUsers"
'            End Select

'            Dim params() As SqlParameter = {New SqlParameter("@Value", value)}
'            Dim dt As DataTable = DatabaseHelper.GetData(query, params, False)

'            dgvUsers.DataSource = dt

'        Catch ex As Exception
'            MessageBox.Show("فشل في البحث: " & ex.Message)
'        End Try
'    End Sub

'    Private Sub btnBrowsePic_Click(sender As Object, e As EventArgs) Handles btnBrowsePic.Click
'        Using ofd As New OpenFileDialog
'            ofd.Filter = "الصور|*.jpg;*.jpeg;*.png;*.bmp|جميع الملفات|*.*"
'            ofd.Title = "اختيار صورة المستخدم"

'            If ofd.ShowDialog() = DialogResult.OK Then
'                Try
'                    ' تحميل الصورة وعرضها
'                    picUser.Image = Image.FromFile(ofd.FileName)

'                    ' تحويل الصورة إلى مصفوفة بايت
'                    userImageBytes = File.ReadAllBytes(ofd.FileName)

'                Catch ex As Exception
'                    MessageBox.Show("فشل في تحميل الصورة: " & ex.Message)
'                End Try
'            End If
'        End Using
'    End Sub

'    Private Sub btnResetPassword_Click(sender As Object, e As EventArgs) Handles btnResetPassword.Click
'        If currentUserID = 0 Then
'            MessageBox.Show("يرجى تحديد مستخدم لإعادة تعيين كلمة المرور")
'            Return
'        End If

'        If txtPassword.Text <> txtConfirmPassword.Text Then
'            MessageBox.Show("كلمتا المرور غير متطابقتين")
'            Return
'        End If

'        Try
'            ResetPassword()
'            MessageBox.Show("تمت إعادة تعيين كلمة المرور بنجاح")
'        Catch ex As Exception
'            MessageBox.Show("فشل في إعادة تعيين كلمة المرور: " & ex.Message)
'        End Try
'    End Sub

'    Private Sub dgvUsers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUsers.CellClick
'        ' التحقق من النقر على صف وليس على الهيدر
'        If e.RowIndex >= 0 Then
'            Dim row As DataGridViewRow = dgvUsers.Rows(e.RowIndex)

'            ' التحقق من القيم قبل التحويل
'            currentUserID = If(Not IsDBNull(row.Cells("ID").Value), CInt(row.Cells("ID").Value), -1)

'            txtNumUser.Text = If(Not IsDBNull(row.Cells("NumUser").Value),
'                            row.Cells("NumUser").Value.ToString(),
'                            String.Empty)

'            txtUserName.Text = If(Not IsDBNull(row.Cells("UserName").Value),
'                             row.Cells("UserName").Value.ToString(),
'                             String.Empty)

'            ' استخدام الدالة المساعدة للتحويل الآمن للقيم المنطقية
'            chkIsUser.Checked = GetSafeBoolean(row.Cells("IsActive").Value)
'            chkIsAdmin.Checked = GetSafeBoolean(row.Cells("IsAdmin").Value)

'            ' جلب صورة المستخدم
'            LoadUserImage()
'        End If
'    End Sub

'    ' دالة مساعدة للتحويل الآمن للقيم المنطقية
'    Private Function GetSafeBoolean(value As Object) As Boolean
'        If IsDBNull(value) OrElse value Is Nothing Then
'            Return False
'        End If
'        Return CBool(value)
'    End Function



'    'Private Sub dgvUsers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUsers.CellClick
'    '    If e.RowIndex >= 0 Then
'    '        Dim row As DataGridViewRow = dgvUsers.Rows(e.RowIndex)

'    '        currentUserID = CInt(row.Cells("ID").Value)
'    '        txtNumUser.Text = row.Cells("NumUser").Value.ToString()
'    '        txtUserName.Text = row.Cells("UserName").Value.ToString()
'    '        chkIsUser.Checked = CBool(row.Cells("IsActive").Value)
'    '        chkIsAdmin.Checked = CBool(row.Cells("IsAdmin").Value)

'    '        ' جلب صورة المستخدم
'    '        LoadUserImage()
'    '    End If
'    'End Sub

'    ' ============== دالات العمل مع قاعدة البيانات ==============

'    Private Sub AddUser()
'        Dim query As String = "INSERT INTO TbUsers (NumUser, UserName, Password, IsActive, IsAdmin, PicUser) 
'                              VALUES (@NumUser, @UserName, @Password, @IsActive, @IsAdmin, @PicUser)"

'        Dim params() As SqlParameter = {
'            New SqlParameter("@NumUser", txtNumUser.Text),
'            New SqlParameter("@UserName", txtUserName.Text),
'            New SqlParameter("@Password", txtPassword.Text),
'            New SqlParameter("@IsActive", chkIsUser.Checked),
'            New SqlParameter("@IsAdmin", chkIsAdmin.Checked),
'            New SqlParameter("@PicUser", If(userImageBytes, DBNull.Value))
'        }

'        DatabaseHelper.ExecuteNonQuery(query, params, False)
'    End Sub

'    Private Sub UpdateUser()
'        Dim query As String = "UPDATE TbUsers SET 
'                              NumUser = @NumUser, 
'                              UserName = @UserName, 
'                              IsActive = @IsActive, 
'                              IsAdmin = @IsAdmin, 
'                              PicUser = @PicUser 
'                              WHERE ID = @ID"

'        Dim params() As SqlParameter = {
'            New SqlParameter("@NumUser", txtNumUser.Text),
'            New SqlParameter("@UserName", txtUserName.Text),
'            New SqlParameter("@IsActive", chkIsUser.Checked),
'            New SqlParameter("@IsAdmin", chkIsAdmin.Checked),
'            New SqlParameter("@PicUser", If(userImageBytes, DBNull.Value)),
'            New SqlParameter("@ID", currentUserID)
'        }

'        DatabaseHelper.ExecuteNonQuery(query, params, False)
'    End Sub

'    Private Sub DeleteUser()
'        Dim query As String = "DELETE FROM TbUsers WHERE ID = @ID"
'        Dim params() As SqlParameter = {New SqlParameter("@ID", currentUserID)}
'        DatabaseHelper.ExecuteNonQuery(query, params, False)
'    End Sub

'    Private Sub ResetPassword()
'        Dim query As String = "UPDATE TbUsers SET Password = @Password WHERE ID = @ID"
'        Dim params() As SqlParameter = {
'            New SqlParameter("@Password", txtPassword.Text),
'            New SqlParameter("@ID", currentUserID)
'        }

'        DatabaseHelper.ExecuteNonQuery(query, params, False)
'    End Sub

'    Private Sub LoadUserImage()
'        Try
'            Dim query As String = "SELECT PicUser FROM TbUsers WHERE ID = @ID"
'            Dim params() As SqlParameter = {New SqlParameter("@ID", currentUserID)}
'            Dim result As Object = DatabaseHelper.ExecuteScalar(query, params, False)

'            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
'                Dim imageBytes As Byte() = CType(result, Byte())
'                Using ms As New IO.MemoryStream(imageBytes)
'                    picUser.Image = Image.FromStream(ms)
'                End Using
'                userImageBytes = imageBytes
'            Else
'                picUser.Image = Nothing
'                userImageBytes = Nothing
'            End If
'        Catch ex As Exception
'            MessageBox.Show("فشل في تحميل صورة المستخدم: " & ex.Message)
'        End Try
'    End Sub

'    ' ============== الدالات المساعدة ==============

'    Private Function ValidateInput() As Boolean
'        If String.IsNullOrWhiteSpace(txtNumUser.Text) Then
'            MessageBox.Show("يرجى إدخال رقم المستخدم")
'            txtNumUser.Focus()
'            Return False
'        End If

'        If String.IsNullOrWhiteSpace(txtUserName.Text) Then
'            MessageBox.Show("يرجى إدخال اسم المستخدم")
'            txtUserName.Focus()
'            Return False
'        End If

'        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
'            MessageBox.Show("يرجى إدخال كلمة المرور")
'            txtPassword.Focus()
'            Return False
'        End If

'        If txtPassword.Text <> txtConfirmPassword.Text Then
'            MessageBox.Show("كلمتا المرور غير متطابقتين")
'            txtConfirmPassword.Focus()
'            Return False
'        End If

'        Return True
'    End Function

'    Private Sub ResetForm()
'        currentUserID = 0
'        txtNumUser.Clear()
'        txtUserName.Clear()
'        txtPassword.Clear()
'        txtConfirmPassword.Clear()
'        chkIsUser.Checked = True
'        chkIsAdmin.Checked = False
'        picUser.Image = Nothing
'        userImageBytes = Nothing
'    End Sub

'    Private Sub btnChengePassword_Click(sender As Object, e As EventArgs) Handles btnChengePassword.Click
'        Dim frm As New frmChangePassword
'        frm.ShowDialog()
'    End Sub

'    Private Sub UsersForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
'        ' إلغاء التسجيل عند إغلاق النموذج
'        PermissionEventManager.Instance.UnregisterPermissionChangedHandler(AddressOf RefreshPermissionsHandler)
'    End Sub
'End Class