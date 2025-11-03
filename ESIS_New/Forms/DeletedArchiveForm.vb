Public Class DeletedArchiveForm
    Private archiveClass As New ArchiveClass()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub DeletedArchiveForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' بدء العد التنازلي
        StartCountdownTimer()
        LoadDeletedArchives()
        txtDaysToKeep.Text = My.Settings.daysToKeep
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
    ''' تحميل البيانات إلى جدول العرض
    ''' </summary>

    Private Sub LoadDeletedArchives()
        Try
            dgvDeletedArchive.DataSource = archiveClass.GetDeletedArchives()

            ' تخصيص أسماء الأعمدة
            If dgvDeletedArchive.Columns.Contains("ID") Then dgvDeletedArchive.Columns("ID").HeaderText = "المعرف"
            If dgvDeletedArchive.Columns.Contains("ArchiveNum") Then dgvDeletedArchive.Columns("ArchiveNum").HeaderText = "رقم الأرشيف"
            If dgvDeletedArchive.Columns.Contains("ArchiveDate") Then dgvDeletedArchive.Columns("ArchiveDate").HeaderText = "تاريخ الأرشيف"
            If dgvDeletedArchive.Columns.Contains("ArchiveSubject") Then dgvDeletedArchive.Columns("ArchiveSubject").HeaderText = "موضوع الأرشيف"
            If dgvDeletedArchive.Columns.Contains("ArchiveFolder") Then dgvDeletedArchive.Columns("ArchiveFolder").HeaderText = "مجلد الأرشيف"
            If dgvDeletedArchive.Columns.Contains("FileName") Then dgvDeletedArchive.Columns("FileName").HeaderText = "اسم الملف"

            If dgvDeletedArchive.Columns.Contains("FilePath") Then dgvDeletedArchive.Columns("FilePath").HeaderText = "مسار الملف"
            If dgvDeletedArchive.Columns.Contains("ArchiveWillMaster") Then dgvDeletedArchive.Columns("ArchiveWillMaster").HeaderText = "البوليصة الرئيسية"
            If dgvDeletedArchive.Columns.Contains("ArchiveWillSub") Then dgvDeletedArchive.Columns("ArchiveWillSub").HeaderText = "البوليصة الفرعية"



            If dgvDeletedArchive.Columns.Contains("DeletedDate") Then dgvDeletedArchive.Columns("DeletedDate").HeaderText = "تاريخ الحذف"
            If dgvDeletedArchive.Columns.Contains("DeletedBy") Then dgvDeletedArchive.Columns("DeletedBy").HeaderText = "حذف بواسطة"


            ' إخفاء عمود المعرف ومسار الملف
            If dgvDeletedArchive.Columns.Contains("ID") Then dgvDeletedArchive.Columns("ID").Visible = False
            If dgvDeletedArchive.Columns.Contains("FilePath") Then dgvDeletedArchive.Columns("FilePath").Visible = False
            If dgvDeletedArchive.Columns.Contains("SaderID") Then dgvDeletedArchive.Columns("SaderID").Visible = False
            If dgvDeletedArchive.Columns.Contains("ArchiveNotes") Then dgvDeletedArchive.Columns("ArchiveNotes").Visible = False
            If dgvDeletedArchive.Columns.Contains("FileType") Then dgvDeletedArchive.Columns("FileType").Visible = False
            If dgvDeletedArchive.Columns.Contains("OriginalFilePath") Then dgvDeletedArchive.Columns("OriginalFilePath").Visible = False
            If dgvDeletedArchive.Columns.Contains("NumericID") Then dgvDeletedArchive.Columns("NumericID").Visible = False
            If dgvDeletedArchive.Columns.Contains("IsDeleted") Then dgvDeletedArchive.Columns("IsDeleted").Visible = False
            If dgvDeletedArchive.Columns.Contains("CreatedDate") Then dgvDeletedArchive.Columns("CreatedDate").Visible = False
            If dgvDeletedArchive.Columns.Contains("UserName") Then dgvDeletedArchive.Columns("UserName").Visible = False
            dgvDeletedArchive.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            UpdateRecordCount()

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الأرشيفات المحذوفة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateRecordCount()
        lblRecordCount.Text = $"عدد السجلات المحذوفة من الأرشيف: {dgvDeletedArchive.Rows.Count}"
    End Sub

    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        Try
            If dgvDeletedArchive.SelectedRows.Count > 0 Then
                Dim selectedId As Integer = Convert.ToInt32(dgvDeletedArchive.SelectedRows(0).Cells("ID").Value)
                
                Dim result As DialogResult = MessageBox.Show(
                    "هل أنت متأكد من رغبتك في استرجاع هذا الأرشيف؟",
                    "تأكيد الاسترجاع",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)

                If result = DialogResult.Yes Then
                    Dim success As Boolean = archiveClass.RestoreArchiveById(selectedId, GlobalData.LoggedInUserName)
                    
                    If success Then
                        LoadDeletedArchives()
                        ' تحديث النموذج الرئيسي إذا كان مفتوحًا
                        For Each form As Form In Application.OpenForms
                            If TypeOf form Is ArchiveForm Then
                                DirectCast(form, ArchiveForm).LoadActiveArchive()
                                Exit For
                            End If
                        Next
                    End If
                End If
            Else
                MessageBox.Show("يرجى اختيار أرشيف للاسترجاع", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    ' زر الحذف النهائي (اختياري)
    Private Sub btnPermanentDelete_Click(sender As Object, e As EventArgs) Handles btnPermanentDelete.Click
        Try
            ' التحقق من وجود صف محدد
            If dgvDeletedArchive.SelectedRows.Count = 0 Then
                MessageBox.Show("يرجى اختيار أرشيف للحذف النهائي", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التحقق من وجود خلية ID
            If Not dgvDeletedArchive.Columns.Contains("ID") Then
                MessageBox.Show("العمود المطلوب 'ID' غير موجود في جدول البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If


            ' تحويل المعرف مع التحقق من القيم الفارغة
            Dim idValue As Object = dgvDeletedArchive.SelectedRows(0).Cells("ID").Value
            Dim selectedId As Integer

            ' التحقق من أن القيمة ليست فارغة وقابلة للتحويل
            If idValue Is Nothing OrElse idValue Is DBNull.Value OrElse String.IsNullOrEmpty(idValue.ToString()) Then
                MessageBox.Show("قيمة المعرف غير صالحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' محاولة التحويل مع التعامل مع الأخطاء
            If Not Integer.TryParse(idValue.ToString(), selectedId) Then
                MessageBox.Show("قيمة المعرف ليست رقمية", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' الحصول على رقم الأرشيف مع التعامل مع القيم الفارغة
            Dim archiveNum As String = "المعرف " & selectedId

            ' التحقق من وجود خلية ArchiveNum
            If dgvDeletedArchive.Columns.Contains("ArchiveNum") Then
                Dim archiveNumValue As Object = dgvDeletedArchive.SelectedRows(0).Cells("ArchiveNum").Value

                ' التحقق من أن القيمة ليست فارغة
                If archiveNumValue IsNot Nothing AndAlso archiveNumValue IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(archiveNumValue.ToString()) Then
                    archiveNum = archiveNumValue.ToString()
                End If
            End If

            ' طلب التأكيد
            Dim result As DialogResult = MessageBox.Show(
                $"هل أنت متأكد من رغبتك في حذف أرشيف رقم {archiveNum} نهائيًا؟" & vbNewLine &
                "لن تتمكن من استرجاعه بعد هذا الحذف.",
                "تأكيد الحذف النهائي",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                ' في هذه الحالة، نقوم بحذف السجل نهائيًا
                ' ملاحظة: الدالة الصحيحة هي PermanentlyDeleteArchives مع تحديد الأيام
                ' لكننا نريد حذف سجل محدد، لذا سنستخدم دالة جديدة
                Dim success As Boolean = archiveClass.PermanentlyDeleteSingleArchive(selectedId)

                If success Then
                    LoadDeletedArchives()
                    ' تحديث النموذج الرئيسي إذا كان مفتوحًا
                    For Each form As Form In Application.OpenForms
                        If TypeOf form Is ArchiveForm Then
                            DirectCast(form, ArchiveForm).LoadActiveArchive()
                            Exit For
                        End If
                    Next
                Else
                    MessageBox.Show("فشل في حذف الأرشيف نهائيًا. يرجى المحاولة مرة أخرى.", "فشل", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Catch ex As Exception
            ' تضمين تفاصيل الخطأ في الرسالة
            MessageBox.Show("حدث خطأ غير متوقع أثناء محاولة حذف الأرشيف نهائيًا: " & ex.Message &
                            vbNewLine & "التفاصيل التقنية: " & ex.StackTrace,
                            "خطأ",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            System.Diagnostics.Debug.WriteLine($"خطأ في btnPermanentDelete_Click: {ex.Message}{vbNewLine}{ex.StackTrace}")
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Try
            If Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
                Dim dt As DataTable = archiveClass.SearchDeletedArchive(txtSearch.Text.Trim())
                dgvDeletedArchive.DataSource = dt
            Else
                LoadDeletedArchives()
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في البحث: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' زر بدء عملية التنظيف
    ''' </summary>
    Private Sub btnStartCleanup_Click(sender As Object, e As EventArgs) Handles btnStartCleanup.Click
        Try
            ' تحديد عدد الأيام التي يجب الاحتفاظ بها
            Dim daysToKeep As Integer = My.Settings.daysToKeep

            ' تنفيذ عملية الحذف النهائي
            Dim deletedCount As Integer = archiveClass.PermanentlyDeleteArchives(daysToKeep, True)

            If deletedCount > 0 Then
                LoadDeletedArchives()
                ' تحديث النموذج الرئيسي إذا كان مفتوحًا
                For Each form As Form In Application.OpenForms
                    If TypeOf form Is ArchiveForm Then
                        DirectCast(form, ArchiveForm).LoadActiveArchive()
                        Exit For
                    End If
                Next
                MessageBox.Show($"تم حذف {deletedCount} أرشيف محذوف نهائيًا.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("لم يتم العثور على أي أرشيف محذوف قديم للحذف.", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء التنظيف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' زر إعدادات افتراضية
    ''' </summary>
    Private Sub btnDefaultSettings_Click(sender As Object, e As EventArgs) Handles btnDefaultSettings.Click
        txtDaysToKeep.Text = My.Settings.daysToKeep
    End Sub

End Class