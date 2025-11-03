Imports System.Data.SqlClient
Imports System.IO

Public Class UnarchivedRecordsForm
    Private _archiveClass As New ArchiveClass()
    Private _selectedRowIndex As Integer = -1

    Public Sub New()
        InitializeComponent()
        SetupUI()
        LoadUnarchivedRecords()
    End Sub

    Private Sub SetupUI()
        ' تهيئة DataGridView
        dgvUnarchivedRecords.AutoGenerateColumns = False
        dgvUnarchivedRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvUnarchivedRecords.AllowUserToAddRows = False
        dgvUnarchivedRecords.AllowUserToDeleteRows = False
        dgvUnarchivedRecords.ReadOnly = True

        ' إضافة الأعمدة
        AddColumnsToDataGridView()

        ' تعيين عنوان النموذج
        Me.Text = "السجلات غير المؤرشفة"

        ' تعيين الأيقونة
        Me.Icon = SystemIcons.Information

        ' تعيين حجم النموذج
        Me.Size = New Size(700, 400)
    End Sub

    Private Sub AddColumnsToDataGridView()
        ' مسح الأعمدة الحالية
        dgvUnarchivedRecords.Columns.Clear()

        ' إضافة الأعمدة
        Dim colID As New DataGridViewTextBoxColumn()
        colID.HeaderText = "المعرف"
        colID.DataPropertyName = "ID"
        colID.Visible = False

        Dim colNumberBook As New DataGridViewTextBoxColumn()
        colNumberBook.HeaderText = "رقم الكتاب"
        colNumberBook.DataPropertyName = "NumberBook"
        colNumberBook.Width = 100

        Dim colDateBook As New DataGridViewTextBoxColumn()
        colDateBook.HeaderText = "التاريخ"
        colDateBook.DataPropertyName = "DateBook"
        colDateBook.Width = 100

        Dim colSubjBook As New DataGridViewTextBoxColumn()
        colSubjBook.HeaderText = "الموضوع"
        colSubjBook.DataPropertyName = "SubjBook"
        colSubjBook.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

        Dim colWillMaster As New DataGridViewTextBoxColumn()
        colWillMaster.HeaderText = "البوليصة الرئيسية"
        colWillMaster.DataPropertyName = "WillMaster"
        colWillMaster.Width = 80

        Dim colWillSub As New DataGridViewTextBoxColumn()
        colWillSub.HeaderText = "البوليصة الفرعية"
        colWillSub.DataPropertyName = "WillSub"
        colWillSub.Width = 80

        Dim colBookCategory As New DataGridViewTextBoxColumn()
        colBookCategory.HeaderText = "التصنيف"
        colBookCategory.DataPropertyName = "BookCategory"
        colBookCategory.Width = 80

        ' إضافة الأعمدة إلى DataGridView
        dgvUnarchivedRecords.Columns.Add(colID)
        dgvUnarchivedRecords.Columns.Add(colNumberBook)
        dgvUnarchivedRecords.Columns.Add(colDateBook)
        dgvUnarchivedRecords.Columns.Add(colSubjBook)
        dgvUnarchivedRecords.Columns.Add(colWillMaster)
        dgvUnarchivedRecords.Columns.Add(colWillSub)
        dgvUnarchivedRecords.Columns.Add(colBookCategory)
    End Sub

    Private Sub LoadUnarchivedRecords()
        Try
            dgvUnarchivedRecords.DataSource = _archiveClass.GetUnarchivedSaderRecords()
            lblRecordCount.Text = $"عدد السجلات غير المؤرشفة: {dgvUnarchivedRecords.Rows.Count}"
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل السجلات غير المؤرشفة: " & ex.Message,
                           "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvUnarchivedRecords_SelectionChanged(sender As Object, e As EventArgs) Handles dgvUnarchivedRecords.SelectionChanged
        If dgvUnarchivedRecords.SelectedRows.Count > 0 Then
            _selectedRowIndex = dgvUnarchivedRecords.SelectedRows(0).Index
        End If
    End Sub

    'Private Sub btnArchiveSelected_Click(sender As Object, e As EventArgs) Handles btnArchiveSelected.Click
    '    If _selectedRowIndex >= 0 AndAlso _selectedRowIndex < dgvUnarchivedRecords.Rows.Count Then
    '        Try
    '            ' الحصول على صف السجل المحدد
    '            Dim row As DataGridViewRow = dgvUnarchivedRecords.Rows(_selectedRowIndex)

    '            ' التحقق من أسماء الأعمدة قبل استخدامها
    '            Dim numberBook As String = SafeGetString(GetCellValue(row, "NumberBook"))
    '            Dim dateBook As Date = SafeGetDate(GetCellValue(row, "DateBook"))
    '            Dim subjBook As String = SafeGetString(GetCellValue(row, "SubjBook"))
    '            Dim willMaster As String = SafeGetString(GetCellValue(row, "WillMaster"))
    '            Dim willSub As String = SafeGetString(GetCellValue(row, "WillSub"))
    '            Dim saderId As Integer = SafeGetInteger(GetCellValue(row, "ID"))

    '            ' التحقق من أن القيم تم استخراجها بشكل صحيح
    '            If String.IsNullOrEmpty(numberBook) Then
    '                MessageBox.Show("لا يمكن استخراج رقم الكتاب من السجل. تحقق من هيكل جدول TbSader.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Return
    '            End If

    '            ' فتح نموذج الأرشيف مع البيانات
    '            Dim archiveForm As New ArchiveForm()

    '            ' تعبئة الحقول
    '            archiveForm.txtArchiveNum.Text = numberBook
    '            archiveForm.dtpArchiveDate.Value = dateBook
    '            archiveForm.txtArchiveSubject.Text = subjBook
    '            archiveForm.txtArchiveWillMaster.Text = willMaster
    '            archiveForm.txtArchiveWillSub.Text = willSub

    '            ' اقتراح مجلد الأرشيف بناءً على التاريخ
    '            Dim currentMonth As String = DateTime.Now.ToString("yyyy-MM")
    '            archiveForm.ComboArchiveFolder.Text = $"صادر\{currentMonth}"

    '            ' تعيين أن البيانات من السجلات غير المؤرشفة
    '            archiveForm.Tag = "FromUnarchived"

    '            ' حفظ معرف السجل للربط لاحقًا
    '            archiveForm.SelectedArchiveID = saderId

    '            ' عرض النموذج
    '            archiveForm.Show()

    '            ' إغلاق نموذج السجلات غير المؤرشفة
    '            Me.Close()
    '        Catch ex As Exception
    '            MessageBox.Show($"خطأ في فتح نموذج الأرشيف: {ex.Message}" & vbNewLine &
    '                       "التفاصيل التقنية: " & ex.StackTrace,
    '                       "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Else
    '        MessageBox.Show("يرجى تحديد سجل للإرشفة", "تنبيه",
    '                   MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '    End If
    'End Sub

    Private Sub btnArchiveSelected_Click(sender As Object, e As EventArgs) Handles btnArchiveSelected.Click
        If _selectedRowIndex >= 0 AndAlso _selectedRowIndex < dgvUnarchivedRecords.Rows.Count Then
            Try
                ' الحصول على صف السجل المحدد
                Dim row As DataGridViewRow = dgvUnarchivedRecords.Rows(_selectedRowIndex)

                ' استخراج القيم مع التحقق من القيم الفارغة
                Dim numberBook As String = SafeGetString(GetCellValue(row, "NumberBook"))
                Dim saderId As Integer = SafeGetInteger(GetCellValue(row, "ID"))

                ' التحقق من أن القيم تم استخراجها بشكل صحيح
                If String.IsNullOrEmpty(numberBook) Then
                    numberBook = $"SADER-{saderId}"
                End If

                ' فتح نموذج الصادر مع تمييز السجل
                HighlightUnarchivedRecordInSaderForm(saderId, numberBook)

                ' إغلاق نموذج السجلات غير المؤرشفة
                Me.Close()
            Catch ex As Exception
                MessageBox.Show($"خطأ في فتح نموذج الأرشيف: {ex.Message}" & vbNewLine &
                       "التفاصيل التقنية: " & ex.StackTrace,
                       "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("يرجى تحديد سجل للإرشفة", "تنبيه",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ''' <summary>
    ''' فتح نموذج الصادر مع تمييز السجل غير المؤرشف
    ''' </summary>
    Private Sub HighlightUnarchivedRecordInSaderForm(saderId As Integer, numberBook As String)
        Try
            Dim saderForm As SaderForm = Nothing
            Dim formFound As Boolean = False

            ' البحث عن نموذج الصادر المفتوح
            For Each form As Form In Application.OpenForms
                If TypeOf form Is SaderForm Then
                    saderForm = DirectCast(form, SaderForm)
                    formFound = True
                    Exit For
                End If
            Next

            ' إذا لم يكن مفتوحًا، قم بفتحه
            If Not formFound Then
                saderForm = New SaderForm()
                saderForm.Show()
            Else
                ' جلب النموذج إلى المقدمة
                saderForm.BringToFront()
                saderForm.Focus()
            End If

            ' تمييز السجل في نموذج الصادر
            saderForm.HighlightUnarchivedRecord(saderId, numberBook)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تمييز السجل في نموذج الصادر: {ex.Message}")
            MessageBox.Show("خطأ في فتح نموذج الصادر: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    ''' <summary>
    ''' الحصول على قيمة الخلية مع التحقق من وجود العمود
    ''' </summary>
    Private Function GetCellValue(row As DataGridViewRow, columnName As String) As Object
        Try
            ' التحقق من وجود العمود باستخدام IndexOf
            Dim columnIndex As Integer = row.DataGridView.Columns.IndexOf(row.DataGridView.Columns(columnName))
            If columnIndex <> -1 AndAlso columnIndex < row.Cells.Count Then
                Return row.Cells(columnName).Value
            Else
                ' البحث عن العمود بالاسم
                For Each cell As DataGridViewCell In row.Cells
                    If String.Equals(cell.OwningColumn.Name, columnName, StringComparison.OrdinalIgnoreCase) Then
                        Return cell.Value
                    End If
                Next
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في الحصول على قيمة الخلية {columnName}: {ex.Message}")
        End Try
        Return Nothing
    End Function


    'Private Sub btnArchiveSelected_Click(sender As Object, e As EventArgs) Handles btnArchiveSelected.Click
    '    If _selectedRowIndex >= 0 AndAlso _selectedRowIndex < dgvUnarchivedRecords.Rows.Count Then
    '        Try
    '            ' الحصول على صف السجل المحدد
    '            Dim row As DataGridViewRow = dgvUnarchivedRecords.Rows(_selectedRowIndex)

    '            ' استخراج القيم مع التحقق من القيم الفارغة
    '            Dim numberBook As String = SafeGetString(row.Cells("NumberBook").Value)
    '            Dim dateBook As Date = SafeGetDate(row.Cells("DateBook").Value)
    '            Dim subjBook As String = SafeGetString(row.Cells("SubjBook").Value)
    '            Dim willMaster As String = SafeGetString(row.Cells("WillMaster").Value)
    '            Dim willSub As String = SafeGetString(row.Cells("WillSub").Value)
    '            Dim bookCategory As String = SafeGetString(row.Cells("BookCategory").Value)
    '            Dim saderId As Integer = SafeGetInteger(row.Cells("ID").Value)

    '            ' فتح نموذج الأرشيف مع البيانات
    '            Dim archiveForm As New ArchiveForm()

    '            ' تعبئة الحقول
    '            archiveForm.txtArchiveNum.Text = numberBook
    '            archiveForm.dtpArchiveDate.Value = dateBook
    '            archiveForm.txtArchiveSubject.Text = subjBook
    '            archiveForm.txtArchiveWillMaster.Text = willMaster
    '            archiveForm.txtArchiveWillSub.Text = willSub

    '            ' اقتراح مجلد الأرشيف بناءً على التصنيف
    '            Dim currentMonth As String = DateTime.Now.ToString("yyyy-MM")
    '            Select Case bookCategory.ToLower().Trim()
    '                Case "صادر", "outgoing"
    '                    archiveForm.ComboArchiveFolder.Text = $"صادر\{currentMonth}"
    '                Case "وارد", "incoming"
    '                    archiveForm.ComboArchiveFolder.Text = $"وارد\{currentMonth}"
    '                Case "مذكرات", "memos"
    '                    archiveForm.ComboArchiveFolder.Text = $"مذكرات\{currentMonth}"
    '                Case Else
    '                    archiveForm.ComboArchiveFolder.Text = $"صادر\{currentMonth}" ' افتراضي
    '            End Select

    '            ' تعيين أن البيانات من السجلات غير المؤرشفة
    '            archiveForm.Tag = "FromUnarchived"

    '            ' حفظ معرف السجل للربط لاحقًا
    '            archiveForm.SelectedArchiveID = saderId

    '            ' عرض النموذج
    '            archiveForm.Show()

    '            ' إغلاق نموذج السجلات غير المؤرشفة
    '            Me.Close()
    '        Catch ex As Exception
    '            MessageBox.Show("خطأ في فتح نموذج الأرشيف: " & ex.Message,
    '                       "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End Try
    '    Else
    '        MessageBox.Show("يرجى تحديد سجل للإرشفة", "تنبيه",
    '                   MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '    End If
    'End Sub

    Private Sub btnArchiveAll_Click(sender As Object, e As EventArgs)
        If dgvUnarchivedRecords.Rows.Count > 0 Then
            MessageBox.Show("هذه الميزة قيد التطوير. يمكنك أرشفة السجلات واحدة تلو الأخرى.",
                           "ملاحظة", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("لا توجد سجلات غير مؤرشفة.", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadUnarchivedRecords()
    End Sub

    ''' <summary>
    ''' تحويل كائن إلى سلسلة نصية بأمان
    ''' </summary>
    Private Function SafeGetString(value As Object) As String
        If value Is Nothing OrElse value Is DBNull.Value Then
            Return ""
        Else
            Return value.ToString()
        End If
    End Function

    ''' <summary>
    ''' تحويل كائن إلى تاريخ بأمان
    ''' </summary>
    Private Function SafeGetDate(value As Object) As Date
        If value Is Nothing OrElse value Is DBNull.Value Then
            Return DateTime.Now
        Else
            Try
                Return Convert.ToDateTime(value)
            Catch
                Return DateTime.Now
            End Try
        End If
    End Function

    ''' <summary>
    ''' تحويل كائن إلى عدد صحيح بأمان
    ''' </summary>
    Private Function SafeGetInteger(value As Object) As Integer
        If value Is Nothing OrElse value Is DBNull.Value Then
            Return 0
        Else
            Try
                Return Convert.ToInt32(value)
            Catch
                Return 0
            End Try
        End If
    End Function

    Private Sub UnarchivedRecordsForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' التحقق من أن النموذج لا يغلق إذا كان هناك عمليات جارية
        ' يمكن إضافة تحقق إضافي هنا إذا لزم الأمر
    End Sub
End Class