Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports System.IO
Imports System.Threading
Imports FastReport
Imports FastReport.Export.PdfSimple

Public Class SaderForm : Inherits BaseFormWithPermissions
#Region "Variables and Initialization     المتغيرات والتهيئة"
    Private scaler As ControlScaler
    Private saderClass As New SaderClass()
    Private activityLogClass As New ActivityLogClass()

    Private bookCategories As List(Of String) = New List(Of String)()

    Private isLoadingData As Boolean = False ' متغير لتتبع حالة تحميل البيانات
    ' إضافة متغير للتحكم في التوقيت
    Private searchTimer As Timer
    Private lastSearchText As String = ""
    Private masterPolicyNumber As String = Nothing
    Private subPolicyNumber As String = Nothing
    Private dt_TbSader As New DataTable
    Private hasPendingPolicyChanges As Boolean = False ' تم تغيير الاسم هنا لتجنب التضارب
    Private DGV As New DataGridViewHelper
    Public Sub New()
        ' هذا السطر مطلوب لتهيئة النموذج.
        InitializeComponent()
        ' تهيئة الكلاس مع النموذج الحالي
        scaler = New ControlScaler(Me)
        'InitializeDeleteButton()
    End Sub
#End Region

#Region "Form Events     أحداث النماذج"
    Private Sub SaderForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        ' التحقق من أن الكائن scaler مهيأ قبل استدعائه
        If scaler IsNot Nothing Then
            scaler.ScaleControls(Me)
        End If
    End Sub

    ' معالج الحدث لتغيير العدد
    Private Sub HandleDataCountChanged(sender As Object, e As DataHelper.DataCountChangedEventArgs)
        ' تأكد أن التحديث لجدول الصادرة
        If e.TableName = "Sader" Then
            UpdateRecordCount()
        End If
    End Sub

    ' دالة لتحديث تسمية العدد
    Private Sub UpdateRecordCount()
        Dim count As Integer = DataHelper.GetCurrentSaderCount()
        lblRecordCount.Text = $"عدد السجلات الصادرة: {count}"
    End Sub

    Private Sub SaderForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' الاشتراك في الحدث
            AddHandler DataHelper.SaderCountChanged, AddressOf HandleDataCountChanged
            lblArchiveMsg.Visible = False
            ' تحديث العداد في الواجهة
            UpdateRecordCount()
            ' ربط تلقائي لأحداث التحقق من البوليصات
            PolicyValidationEvents.AttachPolicyValidationEvents(Me)

            ' إضافة معالجات الأحداث للحقول لتحديد حالة وجود بيانات غير محفوظة
            AddHandler txtWillMaster.TextChanged, AddressOf PolicyFields_TextChanged
            AddHandler txtWillSub.TextChanged, AddressOf PolicyFields_TextChanged
            'RBDirectPrint.Checked = True
            isLoadingData = True
            txtWillMaster.Clear()
            txtWillSub.Clear()
            ' إعداد معالج أخطاء DataGridView
            DataGridViewErrorHandler.SetupErrorHandling(dgvSader)
            ' تعطيل الأحداث قبل تحميل البيانات

            RemoveHandler dgvSader.CellClick, AddressOf dgvSader_CellClick
            ' إعداد DataGridView للتطبيقات العربية
            DataGridViewHelper.SetupArabicDataGridView(dgvSader)
            ClearFields()
            ' تعبئة ميزة الإكمال التلقائي في مربع النصوص
            saderClass.AutoCompleteForTextBox("WillMaster", "TbSader", txtWillMaster)
            saderClass.AutoCompleteForTextBox("WillSub", "TbSader", txtWillSub)
            saderClass.AutoCompleteForTextBox("NumberBook", "TbSader", txtSearch)
            ' تعبئة القائمة المنسدلة بالبيانات
            fillCombo(ComboSubjBook, "SELECT Subject_Book FROM TbSubjBooks ORDER BY Subject_Book", "Subject_Book")
            Me.RightToLeft = RightToLeft.Yes
            Me.RightToLeftLayout = True
            dtpDateBook.Value = DateTime.Now
            dtpStartDate.Value = DateTime.Now.AddDays(-30)
            dtpEndDate.Value = DateTime.Now
            SelectAll_TbSader(dgvSader)
            GenerateNextBookNumber()
            PopulatePrintersList()
            LoadSavedPrinter()
            LoadBookCategories() ' تحميل تصنيفات الكتاب
            HighlightUnarchivedRecords()
            txtUserName.Text = GlobalData.LoggedInUserName
            txtUserName.ReadOnly = True
            ClearFieldsOnLoad()
            txtSearch.Enabled = False
            'TimerRefrech.Start()
            ' إلغاء تحديد أي صفوف في dgvSader بعد تحميل البيانات
            dgvSader.ClearSelection()
            ' تعطيل أدوات التعديل في البداية
            EnableEditingTools(False)
            isLoadingData = False
            ' إعادة تسجيل الأحداث بعد تحميل البيانات

            AddHandler dgvSader.CellClick, AddressOf dgvSader_CellClick
            ComboSubjBook.SelectedIndex = -1
        Catch ex As Exception
            isLoadingData = False
            MessageBox.Show($"حدث خطأ أثناء تحميل النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub

    Private Sub SaderForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        btnNew.PerformClick()
        cmbBookCategory.DroppedDown = True
        cmbBookCategory.Focus()
        GenerateNextBookNumber()
        rbNumberBook.Checked = False
    End Sub

    ''' <summary>
    ''' إلغاء ربط الأحداث عند إغلاق النموذج (مهم جداً!)
    ''' </summary>
    Private Sub SaderForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            ' التحقق من وجود قيم في الحقول المهمة
            If hasPendingPolicyChanges Then
                ' بناء رسالة التنبيه
                Dim messageParts As New List(Of String)
                messageParts.Add("يوجد بيانات غير محفوظة في حقول البوليصات:")

                If Not String.IsNullOrWhiteSpace(txtWillMaster.Text) Then
                    messageParts.Add($" - البوليصة الرئيسية: {txtWillMaster.Text.Trim()}")
                End If

                If Not String.IsNullOrWhiteSpace(txtWillSub.Text) Then
                    messageParts.Add($" - البوليصة الفرعية: {txtWillSub.Text.Trim()}")
                End If

                messageParts.Add("")
                messageParts.Add("هل تريد المتابعة وإغلاق النموذج دون حفظ هذه البيانات؟")

                Dim message As String = String.Join(Environment.NewLine, messageParts)

                ' عرض نافذة التنبيه
                Dim result As DialogResult = MessageBox.Show(
        message,
        "تحذير: بيانات غير محفوظة",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning,
        MessageBoxDefaultButton.Button2
    )

                ' إذا اختار المستخدم "لا"، منع إغلاق النموذج
                If result = DialogResult.No Then
                    e.Cancel = True
                End If
            End If
            ' إلغاء ربط الأحداث لتجنب تسريب الذاكرة
            PolicyValidationEvents.DetachPolicyValidationEvents(Me)

            ' إزالة معالجات الأحداث المضافة لحقول البوليصات
            RemoveHandler txtWillMaster.TextChanged, AddressOf PolicyFields_TextChanged
            RemoveHandler txtWillSub.TextChanged, AddressOf PolicyFields_TextChanged
            ' إلغاء الاشتراك في الحدث
            RemoveHandler DataHelper.SaderCountChanged, AddressOf HandleDataCountChanged
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في إلغاء ربط الأحداث: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' التحقق من وجود بيانات غير محفوظة في حقول البوليصات
    ''' </summary>
    Private Function HasUnsavedPolicyData() As Boolean
        Try
            ' التحقق من وجود قيم في الحقول المهمة
            Dim hasMasterPolicy As Boolean = Not String.IsNullOrEmpty(txtWillMaster?.Text?.Trim())
            Dim hasSubPolicy As Boolean = Not String.IsNullOrEmpty(txtWillSub?.Text?.Trim())
            ' إذا كان أي من الحقلين يحتوي على قيمة، فهناك بيانات غير محفوظة
            Return hasMasterPolicy OrElse hasSubPolicy
        Catch ex As Exception
            ' في حالة الخطأ، افترض أن هناك بيانات غير محفوظة لتجنب فقدانها
            Debug.WriteLine($"خطأ في HasUnsavedPolicyData: {ex.Message}")
            Return True
        End Try
    End Function

#End Region

#Region "Data Loading and Display     تحميل البيانات وعرضها"
    Private Sub LoadSaderSafely()
        Try
            isLoadingData = True
            ' الحصول على البيانات من الكلاس المحسن
            Dim dataTable As DataTable = saderClass.GetAllSader()
            ' ربط البيانات بشكل آمن
            DataGridViewErrorHandler.SafeBindData(dgvSader, dataTable)

            ' التحقق من وجود سجلات غير مؤرشفة
            Dim nonArchivedCount As Integer = 0
            For Each row As DataRow In dataTable.Rows
                If row.IsNull("ArchiveID") OrElse row("ArchiveID") Is DBNull.Value Then
                    nonArchivedCount += 1
                End If
            Next

            HighlightUnarchivedRecords()

            ' تحديث عناوين الأعمدة إذا كانت موجودة
            UpdateColumnHeaders()
            ' تعطيل أدوات التعديل بعد تحميل البيانات الجديدة
            EnableEditingTools(False)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في LoadSaderSafely: {ex.Message}")
            ' في حالة الفشل، عرض رسالة للمستخدم وربط جدول فارغ
            MessageBox.Show("حدث خطأ أثناء تحميل البيانات. سيتم عرض قائمة فارغة.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            DataGridViewErrorHandler.SafeBindData(dgvSader, Nothing)
        Finally
            isLoadingData = False
        End Try
    End Sub

    Private Sub UpdateColumnHeaders()
        Try
            If dgvSader.Columns.Count > 0 Then
                ' التحقق من وجود كل عمود قبل تحديث العنوان
                If dgvSader.Columns.Contains("ID") Then
                    dgvSader.Columns("ID").HeaderText = "المعرف"
                    dgvSader.Columns("ID").Visible = False
                End If
                If dgvSader.Columns.Contains("NumberBook") Then
                    dgvSader.Columns("NumberBook").HeaderText = "الرقم"
                    dgvSader.Columns("NumberBook").Width = 90
                End If
                If dgvSader.Columns.Contains("DateBook") Then
                    dgvSader.Columns("DateBook").HeaderText = "التاريخ"
                    dgvSader.Columns("DateBook").Width = 90
                End If
                If dgvSader.Columns.Contains("SubjBook") Then
                    dgvSader.Columns("SubjBook").HeaderText = "الموضوع"
                    dgvSader.Columns("SubjBook").Width = 200
                End If
                If dgvSader.Columns.Contains("WillMaster") Then
                    dgvSader.Columns("WillMaster").HeaderText = "الرئيسية"
                    dgvSader.Columns("WillMaster").Width = 100
                End If
                If dgvSader.Columns.Contains("WillSub") Then
                    dgvSader.Columns("WillSub").HeaderText = "الفرعية"
                    dgvSader.Columns("WillSub").Width = 100
                End If
                If dgvSader.Columns.Contains("BookCategory") Then
                    dgvSader.Columns("BookCategory").HeaderText = "التصنيف"
                    dgvSader.Columns("BookCategory").Width = 100
                    dgvSader.Columns("BookCategory").Visible = False
                End If
                If dgvSader.Columns.Contains("UserName") Then
                    dgvSader.Columns("UserName").HeaderText = "المستخدم"
                    dgvSader.Columns("UserName").Width = 70
                End If
                If dgvSader.Columns.Contains("ArchiveID") Then
                    dgvSader.Columns("ArchiveID").HeaderText = "الإرشيف"
                    dgvSader.Columns("ArchiveID").Width = 60
                End If



                HighlightUnarchivedRecords()

            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث عناوين الأعمدة: {ex.Message}")
        End Try
    End Sub

    Private Sub EnableEditingTools(enable As Boolean)
        Try
            ' تفعيل/تعطيل أزرار التعديل والحذف حسب الصلاحيات والتحديد
            btnUpdate.Enabled = enable AndAlso PermissionsManager.Instance.HasPermission(GlobalData.LoggedInUserID, "SaderForm", "Edit")
            btnDelete.Enabled = enable AndAlso PermissionsManager.Instance.HasPermission(GlobalData.LoggedInUserID, "SaderForm", "Delete")
            btnPrint.Enabled = enable ' الطباعة متاحة دائماً إذا كان هناك تحديد
            ' تحديث شريط الحالة أو أي مؤشرات أخرى
            UpdateStatusIndicators(enable)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تفعيل أدوات التعديل: {ex.Message}")
        End Try
    End Sub

    Private Sub UpdateStatusIndicators(hasSelection As Boolean)
        Try
            If hasSelection Then
                ' مؤشر وجود تحديد
                dgvSader.BorderStyle = BorderStyle.Fixed3D
            Else
                ' مؤشر عدم وجود تحديد
                dgvSader.BorderStyle = BorderStyle.FixedSingle
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحديث مؤشرات الحالة: {ex.Message}")
        End Try
    End Sub

    Private Function IsValidRowSelected() As Boolean
        Return DataGridViewHelper.IsValidRowSelected(dgvSader, {"ID", "NumberBook"})
    End Function

    Private Function GetSelectedRowId() As Integer
        Return DataGridViewHelper.GetSelectedRowId(dgvSader)
    End Function

    Private Function GetSelectedRowStringValue(columnName As String, Optional defaultValue As String = "") As String
        Return DataGridViewHelper.GetSelectedRowValue(Of String)(dgvSader, columnName, defaultValue)
    End Function

    Private Function GetSelectedRowDateValue(columnName As String, Optional defaultValue As DateTime = Nothing) As DateTime
        If defaultValue = Nothing Then defaultValue = DateTime.Now
        Return DataGridViewHelper.GetSelectedRowValue(Of DateTime)(dgvSader, columnName, defaultValue)
    End Function

    Private Function GetSelectedRowObjectValue(columnName As String) As Object
        Try
            If Not IsValidRowSelected() Then
                Return Nothing
            End If
            Dim selectedRow As DataGridViewRow = dgvSader.SelectedRows(0)
            If Not dgvSader.Columns.Contains(columnName) Then
                Return Nothing
            End If
            Dim cellValue As Object = selectedRow.Cells(columnName).Value
            If cellValue Is Nothing OrElse IsDBNull(cellValue) Then
                Return Nothing
            End If
            Return cellValue
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في GetSelectedRowObjectValue: {ex.Message}")
            Return Nothing
        End Try
    End Function

    'دالة لتحديث البيانات في النموذج
    Public Sub RefreshData()
        LoadSaderSafely()
    End Sub

    ' دالة لتعيين معرف الأرشيف من فورم الأرشيف
    Public Sub SetArchiveIdFromArchiveForm(archiveId As Integer)
        txtArchiveID.Text = archiveId.ToString()
        'MessageBox.Show("تم ربط الأرشيف رقم " & archiveId.ToString() & " بنجاح مع فورم الصادر.", "ربط الأرشيف", MessageBoxButtons.OK, MessageBoxIcon.Information)
        btnNew.PerformClick()
    End Sub
#End Region

#Region "DataGridView Events     أحداث DataGridView"

    Public Sub SelectAll_TbSader(dgv As DataGridView)
        Dim da As SqlDataAdapter
        dt_TbSader.Clear()
        da = New SqlDataAdapter("select * from TbSader", GetConnection)
        da.Fill(dt_TbSader)
        dgv.DataSource = dt_TbSader.DefaultView
    End Sub

    Private Function GetConnection() As SqlConnection
        Return DatabaseHelper.GetConnection()
    End Function

    Private Sub dgvSader_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSader.CellClick
        Try
            ' تجاهل الحدث أثناء تحميل البيانات
            If isLoadingData Then Return

            ' التحقق من أن الفهرس صالح (ليس -1 للهيدر)
            If e.RowIndex < 0 Then
                Return ' الخروج إذا كان النقر على الهيدر
            End If

            ' التحقق من أن العمود صالح
            If e.ColumnIndex < 0 Then
                Return
            End If

            ' التحقق من وجود صفوف محددة
            If dgvSader.Rows.Count = 0 OrElse e.RowIndex >= dgvSader.Rows.Count Then
                Return
            End If

            With dgvSader
                txtNumberBook.Text = .CurrentRow.Cells("NumberBook").Value.ToString()
                dtpDateBook.Value = .CurrentRow.Cells("DateBook").Value
                ComboSubjBook.Text = .CurrentRow.Cells("SubjBook").Value.ToString()
                txtWillMaster.Text = .CurrentRow.Cells("WillMaster").Value.ToString()
                txtWillSub.Text = .CurrentRow.Cells("WillSub").Value.ToString()
                txtUserName.Text = .CurrentRow.Cells("UserName").Value.ToString()
                txtArchiveID.Text = .CurrentRow.Cells("ArchiveID").Value.ToString()

                ' ✅ تحديد التصنيف (BookCategory)
                Dim bookCategory As String = If(.CurrentRow.Cells("BookCategory").Value Is Nothing, "", .CurrentRow.Cells("BookCategory").Value.ToString())
                ' تعيين التصنيف في ComboBox
                If Not String.IsNullOrEmpty(bookCategory) Then
                    ' التحقق من وجود التصنيف في القائمة
                    If cmbBookCategory.Items.Contains(bookCategory) Then
                        cmbBookCategory.Text = bookCategory
                    Else
                        ' إذا لم يكن التصنيف موجودًا، أضفه مؤقتًا
                        cmbBookCategory.Items.Add(bookCategory)
                        cmbBookCategory.Text = bookCategory
                    End If
                Else
                    ' إذا كان التصنيف فارغًا، اترك الحقل فارغًا أو استخدم القيمة الافتراضية
                    cmbBookCategory.SelectedIndex = -1
                End If
            End With

            ' تعطيل أدوات التعديل في البداية
            btnDelete.Enabled = True
            btnUpdate.Enabled = True
            btnPrint.Enabled = True
            btnAdd.Enabled = False
            lblArchiveMsg.Visible = True
            lblArchiveMsg.Text = "إضغط على الجدول مرتين لارشفة الكتاب"
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub HighlightSelectedRow()
        Try
            If IsValidRowSelected() Then
                Dim selectedRow As DataGridViewRow = dgvSader.SelectedRows(0)
                selectedRow.DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue
                selectedRow.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkBlue
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في HighlightSelectedRow: {ex.Message}")
        End Try
    End Sub

    Public Sub dgvSader_RowsAdded(sender As Object, e As EventArgs) Handles dgvSader.RowsAdded
        ' تحديث عداد الصفوف أو أي عمليات أخرى
        lblRecordCount.Text = $"عدد السجلات الصادرة : {dgvSader.Rows.Count}"
    End Sub

    ''' <summary>
    ''' تمييز السجلات غير المؤرشفة في DataGridView
    ''' </summary>
    Public Sub HighlightUnarchivedRecords()
        Try
            ' تأكد من وجود البيانات
            If dgvSader.Rows.Count = 0 Then
                Return
            End If

            ' تعيين لون التمييز للسجلات غير المؤرشفة
            Dim unarchivedColor As System.Drawing.Color = System.Drawing.Color.FromArgb(255, 255, 200) ' لون أصفر فاتح

            For Each row As DataGridViewRow In dgvSader.Rows
                ' التحقق من أن السجل غير مؤرشف (ArchiveID فارغ أو 0)
                Dim archiveIdCell As Object = row.Cells("ArchiveID").Value
                Dim isUnarchived As Boolean = False

                If archiveIdCell Is Nothing OrElse archiveIdCell Is DBNull.Value Then
                    isUnarchived = True
                Else
                    Dim archiveId As Integer = 0
                    If Integer.TryParse(archiveIdCell.ToString(), archiveId) Then
                        isUnarchived = (archiveId = 0)
                    End If
                End If

                ' تطبيق لون التمييز
                If isUnarchived Then
                    row.DefaultCellStyle.BackColor = unarchivedColor
                    ' يمكن إضافة لون للخط إذا لزم
                    row.DefaultCellStyle.ForeColor = Color.DarkBlue
                Else
                    ' إعادة اللون الافتراضي للسجلات المؤرشفة
                    row.DefaultCellStyle.BackColor = dgvSader.DefaultCellStyle.BackColor
                End If
            Next
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تمييز السجلات غير المؤرشفة: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تمييز سجل محدد في DataGridView
    ''' </summary>
    Public Sub HighlightUnarchivedRecord(saderId As Integer, numberBook As String)
        Try
            ' تحديث البيانات أولاً
            RefreshData()

            ' تمييز جميع السجلات غير المؤرشفة
            HighlightUnarchivedRecords()

            ' البحث عن السجل المحدد
            Dim rowIndex As Integer = -1
            For i As Integer = 0 To dgvSader.Rows.Count - 1
                ' محاولة البحث باستخدام المعرف
                Dim idCell As Object = dgvSader.Rows(i).Cells("ID").Value
                Dim id As Integer = 0

                If idCell IsNot Nothing AndAlso Integer.TryParse(idCell.ToString(), id) AndAlso id = saderId Then
                    rowIndex = i
                    Exit For
                End If

                ' محاولة البحث باستخدام رقم الكتاب
                Dim numCell As Object = dgvSader.Rows(i).Cells("NumberBook").Value
                If numCell IsNot Nothing AndAlso numCell.ToString() = numberBook Then
                    rowIndex = i
                    Exit For
                End If
            Next

            ' تحديد السجل إذا تم العثور عليه
            If rowIndex >= 0 AndAlso rowIndex < dgvSader.Rows.Count Then
                dgvSader.ClearSelection()
                dgvSader.Rows(rowIndex).Selected = True

                ' التمرير إلى السجل إذا كان خارج النطاق المرئي
                If rowIndex >= dgvSader.FirstDisplayedScrollingRowIndex AndAlso
               rowIndex <= dgvSader.FirstDisplayedScrollingRowIndex + dgvSader.DisplayedRowCount(False) Then
                    dgvSader.FirstDisplayedScrollingRowIndex = rowIndex
                End If

                ' تطبيق تأثير بصري إضافي للسجّل المحدد
                dgvSader.Rows(rowIndex).DefaultCellStyle.Font = New Font(dgvSader.Font, FontStyle.Bold)
                dgvSader.Rows(rowIndex).DefaultCellStyle.BackColor = Color.LightBlue
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تمييز السجل المحدد: {ex.Message}")
        End Try
    End Sub

#End Region

#Region "CRUD Operations       عمليات CRUD"
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        'RBDirectPrint.Checked = True
        SelectAll_TbSader(dgvSader)
        Me.Refresh()
        ' 6. تحديث العدد بعد العملية الناجحة
        DataHelper.UpdateSaderCount()
        ' تعطيل أدوات التعديل عند إنشاء جديد
        EnableEditingTools(False)
        btnAdd.Enabled = True
        ' إلغاء تحديد الصفوف
        dgvSader.ClearSelection()
        ClearFields()
        ClearRb()
        ' التركيز على أول حقل
        txtWillMaster.Focus()
        UpdateColumnHeaders()
        GenerateNextBookNumber()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            ' التحقق من صلاحية الإضافة
            If Not CheckPermission(1, "إضافة ملف صادر") Then
                Return
            End If

            If ValidateInput() Then
                Dim dateBook As Date = dtpDateBook.Value.Date
                Dim subjBook As String = ComboSubjBook.Text.Trim()
                Dim willMaster As String = txtWillMaster.Text.Trim()
                Dim willSub As String = txtWillSub.Text.Trim()
                Dim bookCategory As String = cmbBookCategory.Text.Trim()
                Dim userName As String = txtUserName.Text.Trim()

                ' 1. الحصول على المعرف الجديد بعد الإضافة
                Dim newSaderId As Integer = saderClass.AddSader(dateBook, subjBook, willMaster, willSub, bookCategory, userName)

                If newSaderId > 0 Then
                    SavedSuccessfully(Me.lblMessage, Me.PicMsg, Me.TimerMsg)
                    'MessageBox.Show("تم إضافة الصادر بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ' تحديث العدد بعد الإضافة الناجحة
                    DataHelper.UpdateSaderCount()
                    ' 2. إعادة تحميل البيانات SelectAll_TbSader(dgvSader)
                    btnNew.PerformClick()

                    ' 3. تحديد السجل الجديد في الجدول
                    Dim rowIndex As Integer = DataGridViewHelper.FindRowByValue(dgvSader, "ID", newSaderId, True)

                    ' 4. إذا تم العثور على السجل وتحديده
                    If rowIndex >= 0 Then
                        ' 5. توليد رقم الكتاب التالي
                        GenerateNextBookNumber()

                        ' 6. طباعة السجل الجديد
                        HandlePrintAction()
                    Else
                        ' إذا فشل تحديد السجل الجديد، اعرض رسالة تحذير
                        MessageBox.Show("تم إضافة الصادر بنجاح، لكن فشل في تحديد السجل الجديد للطباعة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If

                    ' 7. مسح الحقول
                    ClearFields()
                Else
                    MessageBox.Show("فشل في إضافة الصادر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                ' 8. إنشاء سجل جديد
                btnNew.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            ' التحقق من صلاحية الإضافة
            If Not CheckPermission(2, "تعديل ملف صادر") Then
                Return
            End If

            ' التحقق المحسن من الصف المحدد
            If Not IsValidRowSelected() Then
                MessageBox.Show("يرجى اختيار صادر للتحديث من القائمة أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                ' التركيز على DataGridView لتوجيه المستخدم
                dgvSader.Focus()
                Return
            End If

            ' التحقق من صحة البيانات المدخلة
            If Not ValidateInput() Then
                Return
            End If

            Dim id As Integer = GetSelectedRowId()
            If id <= 0 Then
                MessageBox.Show("خطأ في الحصول على معرف الصادر المحدد.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim numberBook As String = txtNumberBook.Text.Trim()
            Dim dateBook As Date = dtpDateBook.Value.Date
            Dim subjBook As String = ComboSubjBook.Text.Trim()
            Dim willMaster As String = txtWillMaster.Text.Trim()
            Dim willSub As String = txtWillSub.Text.Trim()
            Dim bookCategory As String = cmbBookCategory.Text.Trim()
            Dim userName As String = txtUserName.Text.Trim()
            Dim ArchiveID As String = txtArchiveID.Text.Trim()

            If saderClass.UpdateSader(id, numberBook, dateBook, subjBook, willMaster, willSub, bookCategory, userName, ArchiveID) Then
                UpdetedSuccessfully(lblMessage, PicMsg, TimerMsg)
                'MessageBox.Show("تم تحديث الصادر بنجاح", "نجح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' حفظ معرف الصف المحدث للعودة إليه
                Dim updatedId As Integer = id
                ' إعادة تحديد الصف المحدث
                DataGridViewHelper.FindRowByValue(dgvSader, "ID", updatedId, True)
                btnNew.PerformClick()
                btnUpdate.Enabled = PermissionsManager.Instance.HasPermission(GlobalData.LoggedInUserID, "SaderForm", "Add")
            Else
                MessageBox.Show("فشل في تحديث الصادر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If dgvSader.SelectedRows.Count > 0 Then
                If MessageBox.Show("أنت على وشك حذف الصادر رقم [ " & Space(2) & Me.txtNumberBook.Text & "  ] هل تود الإستمرار ؟", "رسالة تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                    Dim id As Integer = Convert.ToInt32(dgvSader.SelectedRows(0).Cells("ID").Value)
                    Dim archiveID As Object = dgvSader.SelectedRows(0).Cells("ArchiveID").Value

                    ' إذا كان هناك سجل مرتبط في الأرشيف
                    If archiveID IsNot Nothing AndAlso Not IsDBNull(archiveID) AndAlso Convert.ToInt32(archiveID) > 0 Then
                        ' الحصول على مسار الملف من ArchiveClass
                        Dim archiveClass As New ArchiveClass()
                        Dim currentFilePath As String = archiveClass.GetFilePathByArchiveID(Convert.ToInt32(archiveID))

                        ' إغلاق الملف المفتوح في WebBrowser قبل محاولة حذفه
                        For Each form As Form In Application.OpenForms
                            If TypeOf form Is ArchiveForm Then
                                Dim archiveForm As ArchiveForm = CType(form, ArchiveForm)
                                If Not String.IsNullOrEmpty(currentFilePath) Then
                                    If archiveForm.WebBrowser1 IsNot Nothing Then
                                        archiveForm.WebBrowser1.Navigate("about:blank")
                                        Application.DoEvents()
                                        Thread.Sleep(200)
                                        ForceCloseFileHandles(currentFilePath)
                                        Thread.Sleep(500) ' انتظار قصير للتأكد من إغلاق الملف
                                    End If
                                End If
                            End If
                        Next
                    End If

                    ' حذف الصادر من قاعدة البيانات (سيتم حذف الأرشيف المرتبط تلقائيًا)
                    If saderClass.DeleteSader(id) Then
                        DeletedSaderSuccessfully(lblMessage, PicMsg, TimerMsg)
                        'MessageBox.Show("تم حذف الصادر والأرشيف المرتبط به بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' تحديث الواجهة
                        btnNew.PerformClick()
                        ' تحديث عداد الصادرة
                        DataHelper.UpdateSaderCount()
                        ' 5. تحديث عداد الأرشيف
                        DataHelper.UpdateArchiveCount()

                        GenerateNextBookNumber()

                        ' تحديث فورم الأرشيف إذا كان مفتوحًا
                        For Each form As Form In Application.OpenForms
                            If TypeOf form Is ArchiveForm Then
                                Dim archiveForm As ArchiveForm = CType(form, ArchiveForm)
                                archiveForm.LoadActiveArchive() ' تحديث قائمة الأرشيف
                                Exit For
                            End If
                        Next
                    Else
                        MessageBox.Show("فشل في حذف الصادر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            Else
                MessageBox.Show("يرجى اختيار صادر للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    'Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
    '    Try
    '        '' التحقق من صلاحية الإضافة
    '        'If Not CheckPermission(3, "حذف ملف صادر") Then
    '        '    Return
    '        'End If

    '        If dgvSader.SelectedRows.Count > 0 Then
    '            If MessageBox.Show("هل تريد حذف هذا الصادر؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
    '                Dim id As Integer = Convert.ToInt32(dgvSader.SelectedRows(0).Cells("ID").Value)
    '                Dim archiveID As Object = dgvSader.SelectedRows(0).Cells("ArchiveID").Value

    '                ' إذا كان هناك سجل مرتبط في الأرشيف
    '                If archiveID IsNot Nothing AndAlso Not IsDBNull(archiveID) AndAlso Convert.ToInt32(archiveID) > 0 Then
    '                    ' الحصول على مسار الملف من ArchiveClass
    '                    Dim archiveClass As New ArchiveClass()
    '                    Dim currentFilePath As String = archiveClass.GetFilePathByArchiveID(Convert.ToInt32(archiveID))
    '                    Dim frm As New ArchiveForm
    '                    ' إغلاق الملف المفتوح في WebBrowser قبل محاولة حذفه
    '                    If Not String.IsNullOrEmpty(currentFilePath) Then
    '                        If frm.WebBrowser1 IsNot Nothing Then
    '                            frm.WebBrowser1.Navigate("about:blank")
    '                            'frm.LoadActiveArchive()
    '                            Application.DoEvents()
    '                            Thread.Sleep(200)
    '                            ForceCloseFileHandles(currentFilePath)
    '                            Thread.Sleep(500) ' انتظار قصير للتأكد من إغلاق الملف
    '                        End If
    '                    End If
    '                End If


    '                ' حذف الصادر من قاعدة البيانات (سيتم حذف الأرشيف المرتبط تلقائيًا)
    '                If saderClass.DeleteSader(id) Then
    '                    MessageBox.Show("تم حذف الصادر والأرشيف المرتبط به بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

    '                    ' تحديث الواجهة
    '                    btnNew.PerformClick()
    '                    ' تحديث عداد الصادرة
    '                    DataHelper.UpdateSaderCount()
    '                    ' 5. تحديث عداد الأرشيف
    '                    DataHelper.UpdateArchiveCount()

    '                    GenerateNextBookNumber()

    '                    ' تحديث فورم الأرشيف إذا كان مفتوحًا
    '                    For Each form As Form In Application.OpenForms
    '                        If TypeOf form Is ArchiveForm Then
    '                            Dim archiveForm As ArchiveForm = CType(form, ArchiveForm)
    '                            archiveForm.LoadActiveArchive() ' تحديث قائمة الأرشيف
    '                            Exit For
    '                        End If
    '                    Next
    '                Else
    '                    MessageBox.Show("فشل في حذف الصادر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                End If
    '            End If
    '        Else
    '            MessageBox.Show("يرجى اختيار صادر للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

#End Region

#Region "Search Functionality     وظيفة البحث"

    Private Sub UpdateSearchFieldState(isEnabled As Boolean, Optional includeDatePicker As Boolean = False)
        txtSearch.Enabled = isEnabled
        If includeDatePicker Then
            dtpSearchDate.Enabled = isEnabled
        End If
        If isEnabled Then
            txtSearch.Clear()
            txtSearch.Focus()
        End If
    End Sub

    Private Sub btnSearchByDate_Click(sender As Object, e As EventArgs) Handles btnSearchByDate.Click
        Try

            Dim startDate As Date = dtpStartDate.Value.Date
            Dim endDate As Date = dtpEndDate.Value.Date
            ' استخدام الكلاس المحسن للبحث
            Dim dataTable As DataTable = saderClass.SearchSaderByDateRange(startDate, endDate)
            ' ربط البيانات بشكل آمن
            DataGridViewErrorHandler.SafeBindData(dgvSader, dataTable)
            UpdateColumnHeaders()
            ' تعطيل أدوات التعديل بعد البحث
            EnableEditingTools(False)
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء البحث بالتاريخ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        ' التحقق من صلاحية الإضافة
        If Not CheckPermission(1, "البحث عن الصادر") Then
            Return
        End If

        Try
            If String.IsNullOrWhiteSpace(txtSearch.Text) Then
                dgvSader.DataSource = Nothing
                RefreshData()
                Return
            End If

            Dim searchDate As DateTime
            ' التحقق مما إذا كان النص يمثل تاريخًا
            If DateTime.TryParse(txtSearch.Text, searchDate) Then
                ' استخدام الإجراء المخزن للبحث بالتاريخ
                Dim dt1 As DataTable = saderClass.SearchSaderByDateRange(searchDate.Date, searchDate.Date)
                dgvSader.DataSource = dt1
                Return
            End If

            ' إذا لم يكن نص البحث تاريخًا، استخدم البحث النصي
            Dim dt As DataTable = Nothing

            Select Case True
                Case rbNumberBook.Checked
                    dt = saderClass.SearchSader(txtSearch.Text.Trim(), "NumberBook")

                Case rbSubjBook.Checked
                    dt = saderClass.SearchSader(txtSearch.Text.Trim(), "SubjBook")

                Case rbWillMaster.Checked
                    dt = saderClass.SearchSader(txtSearch.Text.Trim(), "WillMaster")

                Case rbWillSub.Checked
                    dt = saderClass.SearchSader(txtSearch.Text.Trim(), "WillSub")

                Case RBDate.Checked
                    ' لا يمكن استخدام البحث بالنص لـ DateBook، لذا نستخدم التاريخ فقط
                    MessageBox.Show("يرجى إدخال تاريخ صحيح للبحث بحسب التاريخ.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtSearch.Clear()
                    txtSearch.Focus()
                    Return

                Case rbUnarchivedSader.Checked
                    ' استخدام الإجراء المخزن للبحث عن الصادرة غير المرتبطة بالأرشيف
                    dt = saderClass.SearchUnarchivedSader(txtSearch.Text.Trim())

                Case Else
                    Return
            End Select

            ' عرض النتائج
            dgvSader.DataSource = dt

        Catch ex As Exception
            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub rbNumberBook_CheckedChanged(sender As Object, e As EventArgs) Handles rbNumberBook.CheckedChanged
        UpdateSearchFieldState(rbNumberBook.Checked)
    End Sub

    Private Sub rbSubjBook_CheckedChanged(sender As Object, e As EventArgs) Handles rbSubjBook.CheckedChanged
        UpdateSearchFieldState(rbSubjBook.Checked)
    End Sub

    Private Sub rbWillMaster_CheckedChanged(sender As Object, e As EventArgs) Handles rbWillMaster.CheckedChanged
        UpdateSearchFieldState(rbWillMaster.Checked)
    End Sub

    Private Sub rbWillSub_CheckedChanged(sender As Object, e As EventArgs) Handles rbWillSub.CheckedChanged
        UpdateSearchFieldState(rbWillSub.Checked)
    End Sub

    Private Sub RBDate_CheckedChanged(sender As Object, e As EventArgs) Handles RBDate.CheckedChanged
        UpdateSearchFieldState(RBDate.Checked, True)
    End Sub


    Private Sub dtpSearchDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpSearchDate.ValueChanged
        txtSearch.Text = dtpSearchDate.Value.Date
    End Sub


    Private Sub rbUnarchivedSader_CheckedChanged(sender As Object, e As EventArgs) Handles rbUnarchivedSader.CheckedChanged
        UpdateSearchFieldState(rbUnarchivedSader.Checked, True)
    End Sub


#End Region

#Region "Printing Functionality    وظيفة الطباعة"
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        ' التحقق من الصلاحية
        If Not CheckPermission(1, "طباعة ملف صادر") Then
            Return
        End If
        HandlePrintAction()
    End Sub

    ''' <summary>
    ''' معالجة إجراء الطباعة مع تسجيل النشاط
    ''' </summary>
    Private Sub HandlePrintAction()
        Try
            ' 1. التحقق من وجود سجل محدد
            If Not IsValidRowSelected() Then
                MessageBox.Show("يرجى تحديد سجل صادر للطباعة من القائمة أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                dgvSader.Focus()

                ' تسجيل فشل العملية
                Try
                    Dim activityLogClass As New ActivityLogClass()
                    activityLogClass.LogUserActivity(
                    GlobalData.LoggedInUserName,
                    "Failed_Print",
                    "TbSader",
                    -1,
                    "محاولة طباعة دون تحديد سجل"
                )
                Catch logEx As Exception
                    System.Diagnostics.Debug.WriteLine($"فشل في تسجيل نشاط الطباعة: {logEx.Message}")
                End Try

                Return
            End If

            ' 2. الحصول على معرف السجل المحدد
            Dim selectedSaderId As Integer = GetSelectedRowId()
            If selectedSaderId <= 0 Then
                MessageBox.Show("معرف الصادر غير صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

                ' تسجيل فشل العملية
                Try
                    Dim activityLogClass As New ActivityLogClass()
                    activityLogClass.LogUserActivity(
                    GlobalData.LoggedInUserName,
                    "Failed_Print",
                    "TbSader",
                    -1,
                    $"محاولة طباعة سجل بمعرف غير صالح: {selectedSaderId}"
                )
                Catch logEx As Exception
                    System.Diagnostics.Debug.WriteLine($"فشل في تسجيل نشاط الطباعة: {logEx.Message}")
                End Try

                Return
            End If

            ' 3. جلب البيانات مع التحقق من وجودها
            Dim saderData As DataTable = saderClass.GetSaderById(selectedSaderId)

            ' 4. التحقق من البيانات
            If saderData Is Nothing Then
                MessageBox.Show("لم يتم العثور على بيانات الصادر للسجل المحدد.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

                ' تسجيل فشل العملية
                Try

                    activityLogClass.LogUserActivity(
                    GlobalData.LoggedInUserName,
                    "Failed_Print",
                    "TbSader",
                    selectedSaderId,
                    "فشل في جلب بيانات الصادر (البيانات فارغة)"
                )
                Catch logEx As Exception
                    System.Diagnostics.Debug.WriteLine($"فشل في تسجيل نشاط الطباعة: {logEx.Message}")
                End Try

                Return
            End If

            If saderData.Rows.Count = 0 Then
                MessageBox.Show("لا توجد بيانات للصادر المحدد.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

                ' تسجيل فشل العملية
                Try
                    activityLogClass.LogUserActivity(
                    GlobalData.LoggedInUserName,
                    "Failed_Print",
                    "TbSader",
                    selectedSaderId,
                    "فشل في جلب بيانات الصادر (لا توجد صفوف)"
                )
                Catch logEx As Exception
                    System.Diagnostics.Debug.WriteLine($"فشل في تسجيل نشاط الطباعة: {logEx.Message}")
                End Try

                Return
            End If

            ' 5. استخراج معلومات مهمة للتسجيل
            Dim numberBook As String = saderData.Rows(0)("NumberBook").ToString()
            Dim subjBook As String = saderData.Rows(0)("SubjBook").ToString()

            '6. طباعة التقرير
            PrintSaderReport(saderData)

            ' 7. تسجيل نجاح عملية الطباعة
            Try
                Dim activityLogClass As New ActivityLogClass()
                activityLogClass.LogUserActivity(
                GlobalData.LoggedInUserName,
                "Print",
                "TbSader",
                selectedSaderId,
                $"تم طباعة كتاب صادر برقم: {numberBook}, الموضوع: {subjBook}"
            )
            Catch logEx As Exception
                System.Diagnostics.Debug.WriteLine($"فشل في تسجيل نشاط الطباعة: {logEx.Message}")
            End Try

            ' 8. إنشاء سجل جديد بعد الطباعة
            btnNew.PerformClick()

        Catch ex As Exception
            ' تسجيل خطأ عام في الطباعة
            Try
                Dim activityLogClass As New ActivityLogClass()
                activityLogClass.LogUserActivity(
                GlobalData.LoggedInUserName,
                "Error_Print",
                "TbSader",
                If(IsValidRowSelected(), GetSelectedRowId(), -1),
                $"استثناء أثناء طباعة السجل. الخطأ: {ex.Message}"
            )
            Catch logEx As Exception
                System.Diagnostics.Debug.WriteLine($"فشل في تسجيل استثناء الطباعة: {logEx.Message}")
            End Try

            MessageBox.Show("حدث خطأ أثناء محاولة الطباعة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            btnAdd.Enabled = True
            btnPrint.Enabled = False
        End Try
    End Sub

    ''' <summary>
    ''' طباعة تقرير الصادر باستخدام FastReport.OpenSource واختيار الطابعة من CheckedListBox
    ''' </summary>
    ''' <param name="saderDataTable"></param>
    Private Sub PrintSaderReport(ByVal saderDataTable As DataTable)
            Dim report As Report = Nothing

            Try
                ' 🟡 التحقق من وجود بيانات
                If saderDataTable Is Nothing OrElse saderDataTable.Rows.Count = 0 Then
                    MessageBox.Show("لا توجد بيانات لطباعتها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' 🟡 التحقق من اختيار الطابعة من CheckedListBox
                If ListPrinter.CheckedItems.Count = 0 Then
                    MessageBox.Show("يرجى تحديد طابعة أولاً من القائمة.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' 🖨️ جلب اسم الطابعة المختارة (أول عنصر محدد)
                Dim selectedPrinter As String = ListPrinter.CheckedItems(0).ToString()

                Me.Cursor = Cursors.WaitCursor

                ' 🟢 إنشاء كائن التقرير
                report = New Report()

                ' 🟢 تحميل ملف التقرير
                Dim reportPath As String = Path.Combine(Application.StartupPath, "Reports", "SaderReport.frx")
                If Not File.Exists(reportPath) Then
                    MessageBox.Show($"لم يتم العثور على ملف التقرير: {reportPath}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                report.Load(reportPath)

            ' 🟢 ربط مصدر البيانات
            report.RegisterData(saderDataTable, "TbSader")
            report.GetDataSource("TbSader").Enabled = True

            ' 🟢 تحضير التقرير
            report.Prepare()

            ' 🟢 تصدير التقرير إلى PDF مؤقت
            Dim tempPdfPath As String = Path.Combine(Path.GetTempPath(), "SaderReport.pdf")
            Using pdfExport As New PDFSimpleExport()
                pdfExport.Export(report, tempPdfPath)

                'فتح التقرسر داخل فورم الصادر 
                AxAcroPDF1.src = tempPdfPath
            End Using

            ' 🖨️ طباعة ملف PDF باستخدام الطابعة المختارة
            PrintPDF(tempPdfPath, selectedPrinter)

            Catch ex As FileNotFoundException
                MessageBox.Show($"ملف التقرير غير موجود:{vbNewLine}{ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Catch ex As DirectoryNotFoundException
                MessageBox.Show($"مجلد التقارير غير موجود:{vbNewLine}{ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Catch ex As Exception
                MessageBox.Show($"خطأ في الطباعة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Finally
                ' 🧹 التأكد من التخلص من كائن التقرير
                If report IsNot Nothing Then
                    report.Dispose()
                End If
                Me.Cursor = Cursors.Default
            End Try
        End Sub

    ''' <summary>
    ''' فتح ملف PDF وإرساله للطباعة على الطابعة المحددة
    ''' </summary>
    Private Sub PrintPDF(ByVal filePath As String, ByVal printerName As String)
        Dim psi As New ProcessStartInfo()
        psi.FileName = filePath
        If RBDirectPrint.Checked Then
            psi.Verb = "Print"
        ElseIf RBPreviewBeforePrint.Checked Then
            psi.Verb = "Open"
        End If
        psi.CreateNoWindow = True
        psi.WindowStyle = ProcessWindowStyle.Hidden

        ' 🖨️ تحديد اسم الطابعة المختارة
        psi.Arguments = Chr(34) & printerName & Chr(34)

        Using p As New Process()
            p.StartInfo = psi
            p.Start()
            ' 🕒 انتظار قبل إنهاء العملية
            p.WaitForExit(5000)
        End Using
    End Sub

#End Region

#Region "Archive Functionality    وظيفة الأرشفة"

    Private Sub dgvSader_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSader.CellDoubleClick
        Try
            If e.RowIndex >= 0 Then
                Dim row As DataGridViewRow = dgvSader.Rows(e.RowIndex)
                ' استخراج البيانات من الصف المحدد مع التحقق من القيم الفارغة
                Dim numberBook As String = GetSelectedRowStringValue("NumberBook")
                Dim dateBook As Date = GetSelectedRowDateValue("DateBook")
                Dim subjBook As String = GetSelectedRowStringValue("SubjBook")
                Dim willMaster As String = GetSelectedRowStringValue("WillMaster")
                Dim willSub As String = GetSelectedRowStringValue("WillSub")
                Dim bookCategory As String = GetSelectedRowStringValue("bookCategory")

                ' البحث عن نموذج الأرشيف المفتوح
                Dim archiveForm As ArchiveForm = TryCast(Application.OpenForms("ArchiveForm"), ArchiveForm)

                If archiveForm IsNot Nothing Then
                    ' إذا كان النموذج مفتوحًا، قم بتحديثه بالبيانات الجديدة

                    archiveForm.txtArchiveNum.Text = numberBook
                    archiveForm.dtpArchiveDate.Value = dateBook
                    archiveForm.txtArchiveSubject.Text = subjBook
                    archiveForm.txtArchiveWillMaster.Text = willMaster
                    archiveForm.txtArchiveWillSub.Text = willSub
                    archiveForm.ComboArchiveFolder.Text = bookCategory
                    archiveForm.CalculateDestinationPath()
                    archiveForm.WebBrowser1.Navigate("about:blank")
                    archiveForm.btnBrowseFile.PerformClick()
                    archiveForm.BringToFront()
                Else
                    ' إذا كان النموذج مغلقًا، قم بفتحه
                    Dim main As MainForm = TryCast(Application.OpenForms("MainForm"), MainForm)
                    If main IsNot Nothing Then
                        Dim newArchiveForm As New ArchiveForm(numberBook, dateBook, subjBook, willMaster, willSub, bookCategory)
                        newArchiveForm.MdiParent = main
                        newArchiveForm.Show()
                        newArchiveForm.btnBrowseFile.PerformClick()
                    Else
                        MessageBox.Show("لا يمكن العثور على الفورم الرئيسي")
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء نقل البيانات إلى فورم الأرشيف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

#End Region

#Region "Utility Methods     طرق الأدوات المساعدة"

    Private Sub LoadBookCategories()
        Try
            ' تحديد مسار الملف (يمكنك تعديله حسب موقع الملف)
            Dim filePath As String = Path.Combine(Application.StartupPath, "BookCategories.txt")

            ' التحقق من وجود الملف
            If Not File.Exists(filePath) Then
                ' إذا لم يكن الملف موجودًا، قم بإنشائه مع القيم الافتراضية
                Dim defaultCategories As String() = {"DHL EXPRESS إفراج جمركي", "DHL EXPRESS تصدير", "DHL GLOBAL إفراج جمركي", "DHL GLOBAL تصدير",
                                                    "FEDEX EXPRESS إفراج جمركي", "FEDEX EXPRESS تصدير", "EXPEDITORS إفراج جمركي", "EXPEDITORS تصدير",
                                                    "إستلام مباشر", "شركات اخرى", "الجمارك العامة", "مذكرات", "تصدير", "إفراج جمركي"}

                File.WriteAllLines(filePath, defaultCategories)
            End If

            ' قراءة التصنيفات من الملف
            Dim categories As String() = File.ReadAllLines(filePath)

            ' تنظيف القائمة الحالية وإضافة التصنيفات الجديدة
            cmbBookCategory.Items.Clear()
            For Each category As String In categories
                ' تجاهل الأسطر الفارغة
                If Not String.IsNullOrWhiteSpace(category) Then
                    cmbBookCategory.Items.Add(category.Trim())
                End If
            Next

            ' تعيين القيمة الافتراضية (إذا كانت هناك تصنيفات)
            If cmbBookCategory.Items.Count > 0 Then
                cmbBookCategory.SelectedIndex = 0
            End If

            ' حفظ التصنيفات في المتغير
            bookCategories = categories.Where(Function(c) Not String.IsNullOrWhiteSpace(c)).Select(Function(c) c.Trim()).ToList()

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تحميل تصنيفات الكتاب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PopulatePrintersList()
        Try
            ListPrinter.Items.Clear()
            For Each printerName As String In PrinterSettings.InstalledPrinters
                ListPrinter.Items.Add(printerName)
            Next
        Catch ex As Exception
            MessageBox.Show("خطأ في جلب قائمة الطابعات: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadSavedPrinter()
        Try
            Dim savedPrinter As String = My.Settings.DefaultPrinterName
            If Not String.IsNullOrEmpty(savedPrinter) Then
                Dim index As Integer = ListPrinter.Items.IndexOf(savedPrinter)
                If index <> -1 Then
                    ListPrinter.SelectedIndex = index
                Else
                    If ListPrinter.Items.Count > 0 Then
                        ListPrinter.SelectedIndex = 0 ' Select first if saved printer not found
                    End If
                End If
            Else
                If ListPrinter.Items.Count > 0 Then
                    ListPrinter.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الطابعة المحفوظة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ListPrinter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListPrinter.SelectedIndexChanged
        If ListPrinter.SelectedIndex <> -1 Then
            Dim selectedPrinter As String = ListPrinter.SelectedItem.ToString()
            ' التحقق من أن القيمة ليست فارغة أو تحتوي على بيانات غير صالحة
            If Not String.IsNullOrWhiteSpace(selectedPrinter) Then
                My.Settings.DefaultPrinterName = selectedPrinter
                ' تحديث حالة التحديد للعناصر الأخرى
                For i As Integer = 0 To ListPrinter.Items.Count - 1
                    ListPrinter.SetItemChecked(i, False)
                Next
                ' تحديد العنصر المختار حاليًا
                ListPrinter.SetItemChecked(ListPrinter.SelectedIndex, True)
                ' حفظ الإعدادات
                My.Settings.Save()
            Else
                MessageBox.Show("العنصر المحدد غير صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
    End Sub



    Private Sub GenerateNextBookNumber()
        Try
            txtNumberBook.Text = saderClass.GetNextBookNumber()
            txtNumberBook.ReadOnly = True
        Catch ex As Exception
            MessageBox.Show("خطأ في توليد رقم الصادر: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function ValidateInput() As Boolean
        If String.IsNullOrEmpty(ComboSubjBook.Text.Trim()) Then
            MessageBox.Show("يرجى إدخال موضوع الكتاب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ComboSubjBook.Focus()
            Return False
        End If
        If String.IsNullOrEmpty(txtUserName.Text.Trim()) Then
            MessageBox.Show("يرجى إدخال اسم المستخدم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUserName.Focus()
            Return False
        End If

        ' تحقق من البوليصة الفرعية لتعبئة الكومبو بوكس تلقائيًا
        If Not String.IsNullOrEmpty(txtWillSub.Text.Trim()) AndAlso txtWillSub.Text.Trim().StartsWith("413", StringComparison.OrdinalIgnoreCase) Then
            ' تأكد من وجود القيمة في الكومبو بوكس
            If Not ComboSubjBook.Items.Contains("EXPEDITORS INTERNATIONAL | تصدير") Then
                ComboSubjBook.Items.Add("EXPEDITORS INTERNATIONAL | تصدير")
            End If
            ComboSubjBook.Text = "EXPEDITORS INTERNATIONAL | تصدير"
        End If

        Dim currentBookNumber As String = txtNumberBook.Text.Trim()
        Dim excludeId As Integer = 0
        If IsValidRowSelected() Then
            excludeId = GetSelectedRowId()
        End If
        If saderClass.IsBookNumberExists(currentBookNumber, excludeId) Then
            MessageBox.Show("رقم الكتاب موجود بالفعل. يرجى إدخال رقم كتاب فريد.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNumberBook.Focus()
            Return False
        End If
        Return True
    End Function

    Private Sub ClearRb()
        rbNumberBook.Checked = False
        rbSubjBook.Checked = False
        rbWillMaster.Checked = False
        rbWillSub.Checked = False
        RBDate.Checked = False
        rbUnarchivedSader.Checked = False
        txtSearch.Clear()
    End Sub

    Private Sub ClearFields()
        txtWillMaster.Text = ""
        txtWillSub.Text = ""
        txtSearch.Clear()
        txtArchiveID.Clear()
        dtpDateBook.Value = DateTime.Now
        If cmbBookCategory.Items.Count > 0 Then
            cmbBookCategory.SelectedIndex = -1 ' إعادة تعيين التصنيف للافتراضي
        End If
        ComboSubjBook.SelectedIndex = -1
        ComboSubjBook.Enabled = False
        txtPrintNumberBook.Clear()
        GenerateNextBookNumber()
        ' تعطيل أدوات التعديل عند مسح الحقول
        EnableEditingTools(False)
    End Sub

    Private Sub ClearFieldsOnLoad()
        txtWillMaster.Clear()
        txtWillSub.Clear()
        txtSearch.Clear()
        dtpDateBook.Value = DateTime.Now
    End Sub

    Private Sub txtPrintNumberBook_TextChanged(sender As Object, e As EventArgs) Handles txtPrintNumberBook.TextChanged
        Try
            Dim dv As DataView = dt_TbSader.DefaultView
            dv.RowFilter = " NumberBook LIKE '%" & txtPrintNumberBook.Text.Trim & "%' "
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ForceCloseFileHandles(filepath As String)
        Try
            ' محاولة فتح الملف بوضع حصري للتأكد من عدم استخدامه
            Using fs As New FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.None)
                ' إذا تم فتح الملف بنجاح، فهو غير مستخدم
            End Using
        Catch
            ' إذا فشل الفتح، فالملف مستخدم - قم بتحرير الذاكرة
            GC.Collect()
            GC.WaitForPendingFinalizers()
            Thread.Sleep(1000)
        End Try
    End Sub


#End Region

#Region "Button Events    أحداث الأزرار"
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnAddNewSubj_Click(sender As Object, e As EventArgs) Handles btnAddNewSubj.Click
        Dim Frm As New NewSubjForm
        Frm.ShowDialog(Me)
    End Sub

    Private Sub ComboSubjBook_KeyDown(sender As Object, e As KeyEventArgs) Handles ComboSubjBook.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnAdd.PerformClick()
        End If
    End Sub

    ''' <summary>
    ''' تتبع التغييرات في حقول البوليصات لتحديد وجود بيانات غير محفوظة
    ''' </summary>
    Private Sub PolicyFields_TextChanged(sender As Object, e As EventArgs)
        hasPendingPolicyChanges = HasUnsavedPolicyData()
    End Sub

    Private Sub btnShowUnarchivedOnly_Click(sender As Object, e As EventArgs) Handles btnShowUnarchivedOnly.Click
        Try
            Dim bindingSource As New BindingSource()
            bindingSource.DataSource = dgvSader.DataSource
            bindingSource.Filter = "ArchiveID IS NULL OR ArchiveID = 0"
            dgvSader.DataSource = bindingSource
            lblFilterStatus.Text = "عرض السجلات غير المؤرشفة فقط"
            HighlightUnarchivedRecords()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تصفية السجلات: {ex.Message}")
        End Try
    End Sub

    Private Sub dgvSader_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgvSader.RowsAdded

    End Sub

    Private Sub btnAddCategory_Click(sender As Object, e As EventArgs) Handles btnAddCategory.Click
        Dim frm As New ManageCategoriesForm
        frm.ShowDialog()

        LoadBookCategories()
    End Sub

    Private Sub cmbBookCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBookCategory.SelectedIndexChanged
        If cmbBookCategory.SelectedIndex >= 0 Then
            TLPControls.Enabled = True
            ComboSubjBook.Enabled = True
            txtWillMaster.Focus()
        Else
            TLPControls.Enabled = False
        End If
    End Sub

    Private Sub cmbBookCategory_MouseHover(sender As Object, e As EventArgs) Handles cmbBookCategory.MouseHover
        cmbBookCategory.DroppedDown = True
    End Sub


#End Region

#Region "الصلاحيات"
    ''' <summary>
    ''' تطبيق الصلاحيات على عناصر النموذج
    ''' </summary>
    Protected Overrides Sub ApplyCustomPermissions()
        Try
            Dim currentFormName As String = Me.GetType().Name

            ' التحقق أولاً مما إذا كان المستخدم هو المدير
            If IsCurrentUserAdmin() Then
                ' منح جميع الصلاحيات للمدير
                EnableAllEditingTools(True)
                lblWarning.Visible = False
                Return
            End If

            ' تطبيق الصلاحيات العادية فقط للمستخدمين غير الإداريين
            btnAdd.Enabled = PermissionsManager.Instance.CanAdd(currentFormName)
            btnUpdate.Enabled = PermissionsManager.Instance.CanEdit(currentFormName)
            btnDelete.Enabled = PermissionsManager.Instance.CanDelete(currentFormName)
            btnPrint.Enabled = PermissionsManager.Instance.CanSearch(currentFormName)

            ' عناصر إضافية خاصة بهذا النموذج
            ' ...

            ' عرض رسالة إذا لم تكن هناك صلاحيات كافية للتعديل
            If Not (btnAdd.Enabled OrElse btnUpdate.Enabled OrElse btnDelete.Enabled) Then
                lblWarning.Text = "ملاحظة: لا تملك صلاحيات كافية لتعديل البيانات"
                lblWarning.ForeColor = System.Drawing.Color.Red
                lblWarning.Visible = True
            Else
                lblWarning.Visible = False
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تطبيق الصلاحيات: {ex.Message}")
            ' في حالة الخطأ، تعطيل الأدوات الحساسة
            EnableAllEditingTools(False)
            lblWarning.Text = "حدث خطأ في تحميل الصلاحيات"
            lblWarning.ForeColor = System.Drawing.Color.Red
            lblWarning.Visible = True
        End Try
    End Sub

    ''' <summary>
    ''' التحقق مما إذا كان المستخدم الحالي هو مدير النظام
    ''' </summary>
    Private Function IsCurrentUserAdmin() As Boolean
        Try
            Dim query As String = "SELECT IsAdmin FROM TbUsers WHERE ID = @UserID AND IsActive = 1"
            Using connection As SqlConnection = DatabaseHelper.Con()
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@UserID", GlobalData.LoggedInUserID)
                    Dim result As Object = command.ExecuteScalar()
                    Return result IsNot Nothing AndAlso Convert.ToBoolean(result)
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في التحقق من صلاحية المدير: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' تم habilitation أو تعطيل جميع أدوات التحرير دفعة واحدة
    ''' </summary>
    Private Sub EnableAllEditingTools(enabled As Boolean)
        btnAdd.Enabled = enabled
        btnUpdate.Enabled = enabled
        btnDelete.Enabled = enabled
        btnPrint.Enabled = enabled
        ' يمكن إضافة المزيد من الأزرار حسب الحاجة
    End Sub

    Private Sub TimerMsg_Tick(sender As Object, e As EventArgs) Handles TimerMsg.Tick
        ClearConfirmMsgTools(Me.lblMessage, Me.PicMsg, Me.TimerMsg)

    End Sub

    Private Sub CbArchiveNum_CheckedChanged(sender As Object, e As EventArgs) Handles CbArchiveNum.CheckedChanged
        If CbArchiveNum.Checked Then
            txtArchiveID.ReadOnly = False

        Else
            txtArchiveID.ReadOnly = True
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblArchiveMsg.Visible = False
    End Sub



#End Region

#Region "أكواد واجراءات معطلة"
    'Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
    '    Try
    '        If Not PermissionHelper.CanDelete() Then
    '            PermissionHelper.ShowNoPermissionMessage("الحذف")
    '            Return
    '        End If

    '        If dgvSader.SelectedRows.Count > 0 Then
    '            If MessageBox.Show("هل تريد حذف هذا الصادر؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
    '                Dim id As Integer = Convert.ToInt32(dgvSader.SelectedRows(0).Cells("ID").Value)
    '                Dim archiveID As Object = dgvSader.SelectedRows(0).Cells("ArchiveID").Value

    '                ' حذف الصادر من قاعدة البيانات (سيتم حذف الأرشيف المرتبط تلقائيًا)
    '                If saderClass.DeleteSader(id) Then
    '                    MessageBox.Show("تم حذف الصادر والأرشيف المرتبط به بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

    '                    ' تحديث الواجهة
    '                    ClearFields()
    '                    'LoadDataToGrid(dgvSader, Function() saderClass.GetAllSader(), columns)
    '                    GenerateNextBookNumber()

    '                    ' تحديث فورم الأرشيف إذا كان مفتوحًا
    '                    For Each form As Form In Application.OpenForms
    '                        If TypeOf form Is ArchiveForm Then
    '                            Dim archiveForm As ArchiveForm = CType(form, ArchiveForm)
    '                            archiveForm.LoadArchive() ' تحديث قائمة الأرشيف
    '                            Exit For
    '                        End If
    '                    Next
    '                Else
    '                    MessageBox.Show("فشل في حذف الصادر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                End If
    '            End If
    '        Else
    '            MessageBox.Show("يرجى اختيار صادر للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    'Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
    '    Try
    '        ' التحقق من الصلاحية
    '        If Not PermissionHelper.CanDelete() Then
    '            MessageBox.Show("ليس لديك صلاحية الحذف. يرجى الاتصال بمدير النظام.", "صلاحيات محدودة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            Return
    '        End If

    '        ' التحقق المحسن من الصف المحدد
    '        If Not IsValidRowSelected() Then
    '            MessageBox.Show("يرجى اختيار صادر للحذف من القائمة أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            ' التركيز على DataGridView لتوجيه المستخدم
    '            dgvSader.Focus()
    '            Return
    '        End If

    '        ' الحصول على معلومات الصف المحدد
    '        Dim id As Integer = GetSelectedRowId()
    '        Dim numberBook As String = GetSelectedRowStringValue("NumberBook")

    '        ' تأكيد الحذف مع عرض معلومات الصادر
    '        Dim confirmMessage As String = $"هل تريد حذف الصادر رقم: {numberBook}؟"
    '        If MessageBox.Show(confirmMessage, "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
    '            ' حذف الصادر من قاعدة البيانات
    '            ' ملاحظة: الأرشيف المرتبط سيتم حذفه تلقائيًا بواسطة قاعدة البيانات (ON DELETE CASCADE)
    '            If saderClass.DeleteSader(id) Then
    '                MessageBox.Show("تم حذف الصادر والأرشيف المرتبط به بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '                btnNew.PerformClick()
    '                ' تحديث العدد بعد الحذف الناجح
    '                DataHelper.UpdateSaderCount()
    '                ' تحديث فورم الأرشيف إذا كان مفتوحًا
    '                For Each form As Form In Application.OpenForms
    '                    If TypeOf form Is ArchiveForm Then
    '                        Dim archiveForm As ArchiveForm = CType(form, ArchiveForm)
    '                        archiveForm.LoadArchive() ' تحديث قائمة الأرشيف
    '                        Exit For
    '                    End If
    '                Next
    '            Else
    '                MessageBox.Show("فشل في حذف الصادر", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show("حدث خطأ: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    'Private Sub dgvSader_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSader.SelectionChanged
    '    Try
    '        ' تجاهل الحدث أثناء تحميل البيانات
    '        If isLoadingData Then Return
    '        If IsValidRowSelected() Then
    '            ' تعبئة الحقول بالبيانات مع التحقق من الأمان
    '            txtNumberBook.Text = GetSelectedRowStringValue("NumberBook")
    '            dtpDateBook.Value = GetSelectedRowDateValue("DateBook")
    '            ComboSubjBook.Text = GetSelectedRowStringValue("SubjBook")
    '            txtWillMaster.Text = GetSelectedRowStringValue("WillMaster")
    '            txtWillSub.Text = GetSelectedRowStringValue("WillSub")
    '            txtUserName.Text = GetSelectedRowStringValue("UserName")
    '            txtNumberBook.ReadOnly = True
    '            ' تفعيل أدوات التعديل
    '            EnableEditingTools(True)
    '            btnAdd.Enabled = False
    '            ' إضافة تأثير بصري للصف المحدد
    '            HighlightSelectedRow()
    '        Else
    '            ' تعطيل أدوات التعديل إذا لم يكن هناك تحديد صالح
    '            EnableEditingTools(False)
    '            btnAdd.Enabled = clsPermissions.HasPermission(GlobalData.LoggedInUserID, "SaderForm", "Add")
    '        End If
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في dgvSader_SelectionChanged: {ex.Message}")
    '        EnableEditingTools(False)
    '        btnAdd.Enabled = clsPermissions.HasPermission(GlobalData.LoggedInUserID, "SaderForm", "Add")
    '    End Try
    'End Sub


    'Private Sub dgvSader_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSader.CellClick
    '    Try
    '        ' تجاهل الحدث أثناء تحميل البيانات
    '        If isLoadingData Then Return

    '        ' التحقق من أن الفهرس صالح (ليس -1 للهيدر)
    '        If e.RowIndex < 0 Then
    '            Return ' الخروج إذا كان النقر على الهيدر
    '        End If

    '        ' التحقق من أن العمود صالح
    '        If e.ColumnIndex < 0 Then
    '            Return
    '        End If

    '        ' التحقق من وجود صفوف محددة
    '        If dgvSader.Rows.Count = 0 OrElse e.RowIndex >= dgvSader.Rows.Count Then
    '            Return
    '        End If

    '        With dgvSader
    '            txtNumberBook.Text = .CurrentRow.Cells("NumberBook").Value.ToString()
    '            dtpDateBook.Value = .CurrentRow.Cells("DateBook").Value.ToString()
    '            ComboSubjBook.Text = .CurrentRow.Cells("SubjBook").Value.ToString()
    '            txtWillMaster.Text = .CurrentRow.Cells("WillMaster").Value.ToString()
    '            txtWillSub.Text = .CurrentRow.Cells("WillSub").Value.ToString()
    '            txtUserName.Text = .CurrentRow.Cells("UserName").Value.ToString()
    '            txtArchiveID.Text = .CurrentRow.Cells("ArchiveID").Value.ToString()
    '        End With
    '        ' تعطيل أدوات التعديل في البداية
    '        'EnableEditingTools(True)
    '        btnDelete.Enabled = True
    '        btnUpdate.Enabled = True
    '        btnAdd.Enabled = False
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message)
    '    End Try
    'End Sub

    'Private Sub PerformSearchSafely()
    '    Try
    '        Dim dataTable As DataTable = Nothing
    '        If RBDate.Checked Then
    '            ' قراءة التاريخ مباشرة من TextBox
    '            Dim searchDate As DateTime
    '            If DateTime.TryParse(txtSearch.Text, searchDate) Then
    '                ' استخدام الكلاس المحسن للبحث بالتاريخ
    '                dataTable = saderClass.SearchSaderByDateRange(searchDate.Date, searchDate.Date)
    '            Else
    '                dtpSearchDate.Focus()
    '                Return
    '            End If
    '        Else
    '            Dim searchTerm As String = txtSearch.Text.Trim()
    '            If Not String.IsNullOrEmpty(searchTerm) Then
    '                Dim searchColumn As String = ""
    '                If rbNumberBook.Checked Then
    '                    searchColumn = "NumberBook"
    '                ElseIf rbSubjBook.Checked Then
    '                    searchColumn = "SubjBook"
    '                ElseIf rbWillMaster.Checked Then
    '                    searchColumn = "WillMaster"
    '                ElseIf rbWillSub.Checked Then
    '                    searchColumn = "WillSub"
    '                Else
    '                    MessageBox.Show("يرجى تحديد خيار للبحث.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '                    Return
    '                End If
    '                ' استخدام الكلاس المحسن للبحث
    '                dataTable = saderClass.SearchSader(searchTerm, searchColumn)
    '            Else
    '                ' تحميل جميع البيانات
    '                dataTable = saderClass.GetAllSader()
    '            End If
    '        End If
    '        ' ربط البيانات بشكل آمن
    '        DataGridViewErrorHandler.SafeBindData(dgvSader, dataTable)
    '        UpdateColumnHeaders()
    '        ' تعطيل أدوات التعديل بعد البحث
    '        EnableEditingTools(False)
    '    Catch ex As Exception
    '        MessageBox.Show("حدث خطأ أثناء البحث: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        ' في حالة الخطأ، عرض جدول فارغ
    '        DataGridViewErrorHandler.SafeBindData(dgvSader, Nothing)
    '    End Try
    'End Sub

    '''' <summary>
    '''' تنفيذ عملية البحث بأمان مع تسجيل نشاط المستخدم
    '''' </summary>
    'Private Sub PerformSearchSafely()
    '    Try
    '        Dim dataTable As DataTable = Nothing
    '        Dim searchType As String = ""
    '        Dim searchTerm As String = ""

    '        If RBDate.Checked Then
    '            ' قراءة التاريخ مباشرة من TextBox
    '            Dim searchDate As DateTime
    '            If DateTime.TryParse(txtSearch.Text, searchDate) Then
    '                ' استخدام الكلاس المحسن للبحث بالتاريخ
    '                dataTable = saderClass.SearchSaderByDateRange(searchDate.Date, searchDate.Date)
    '                searchType = "Date"
    '                searchTerm = searchDate.ToString("yyyy-MM-dd")
    '            Else
    '                dtpSearchDate.Focus()

    '                ' تسجيل فشل البحث بالتاريخ
    '                Try

    '                    activityLogClass.LogUserActivity(
    '                    GlobalData.LoggedInUserName,
    '                    "Failed_Search",
    '                    "TbSader",
    '                    -1,
    '                    $"محاولة بحث بالتاريخ غير صالح: {txtSearch.Text}"
    '                )
    '                Catch logEx As Exception
    '                    System.Diagnostics.Debug.WriteLine($"فشل في تسجيل نشاط البحث: {logEx.Message}")
    '                End Try

    '                Return
    '            End If
    '        Else
    '            searchTerm = txtSearch.Text.Trim()

    '            If Not String.IsNullOrEmpty(searchTerm) Then
    '                Dim searchColumn As String = ""

    '                If rbNumberBook.Checked Then
    '                    searchColumn = "NumberBook"
    '                    searchType = "Number"
    '                ElseIf rbSubjBook.Checked Then
    '                    searchColumn = "SubjBook"
    '                    searchType = "Subject"
    '                ElseIf rbWillMaster.Checked Then
    '                    searchColumn = "WillMaster"
    '                    searchType = "WillMaster"
    '                ElseIf rbWillSub.Checked Then
    '                    searchColumn = "WillSub"
    '                    searchType = "WillSub"
    '                Else
    '                    MessageBox.Show("يرجى تحديد خيار للبحث.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)

    '                    ' تسجيل فشل اختيار عمود البحث
    '                    Try

    '                        activityLogClass.LogUserActivity(
    '                        GlobalData.LoggedInUserName,
    '                        "Failed_Search",
    '                        "TbSader",
    '                        -1,
    '                        "لم يتم تحديد عمود للبحث"
    '                    )
    '                    Catch logEx As Exception
    '                        System.Diagnostics.Debug.WriteLine($"فشل في تسجيل نشاط البحث: {logEx.Message}")
    '                    End Try

    '                    Return
    '                End If

    '                ' استخدام الكلاس المحسن للبحث
    '                dataTable = saderClass.SearchSader(searchTerm, searchColumn)
    '            Else
    '                ' تحميل جميع البيانات (عرض الكل)
    '                dataTable = saderClass.GetAllSader()
    '                searchType = "All"
    '                searchTerm = "عرض الكل"
    '            End If
    '        End If

    '        ' ربط البيانات بشكل آمن
    '        DataGridViewErrorHandler.SafeBindData(dgvSader, dataTable)
    '        UpdateColumnHeaders()

    '        ' تعطيل أدوات التعديل بعد البحث
    '        EnableEditingTools(False)

    '        ' ✅ تسجيل نجاح عملية البحث
    '        Try

    '            Dim recordCount As Integer = If(dataTable IsNot Nothing, dataTable.Rows.Count, 0)

    '            activityLogClass.LogUserActivity(
    '            GlobalData.LoggedInUserName,
    '            "Search",
    '            "TbSader",
    '            -1,
    '            $"بحث عن {searchType} بالقيمة: '{searchTerm}'. النتائج: {recordCount} سجل"
    '        )
    '        Catch logEx As Exception
    '            System.Diagnostics.Debug.WriteLine($"فشل في تسجيل نشاط البحث: {logEx.Message}")
    '        End Try

    '    Catch ex As Exception
    '        ' تسجيل خطأ عام في البحث
    '        Try

    '            activityLogClass.LogUserActivity(
    '            GlobalData.LoggedInUserName,
    '            "Error_Search",
    '            "TbSader",
    '            -1,
    '            $"استثناء أثناء البحث. نوع البحث: {If(RBDate.Checked, "Date", "Text")}, مصطلح: '{txtSearch.Text}', الخطأ: {ex.Message}"
    '        )
    '        Catch logEx As Exception
    '            System.Diagnostics.Debug.WriteLine($"فشل في تسجيل استثناء البحث: {logEx.Message}")
    '        End Try

    '        MessageBox.Show("حدث خطأ أثناء البحث: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        ' في حالة الخطأ، عرض جدول فارغ
    '        DataGridViewErrorHandler.SafeBindData(dgvSader, Nothing)
    '    End Try
    'End Sub

    'Private Sub DebugSearchResults()
    '    Try
    '        ' التحقق من عدد السجلات في جدول البيانات
    '        Dim rowCount As Integer = dgvSader.Rows.Count
    '        System.Diagnostics.Debug.WriteLine($"عدد السجلات في DataGridView: {rowCount}")

    '        ' التحقق من وجود سجلات غير مؤرشفة (ArchiveID IS NULL)
    '        Dim nonArchivedCount As Integer = 0
    '        For Each row As DataGridViewRow In dgvSader.Rows
    '            If row.Cells("ArchiveID").Value Is Nothing OrElse row.Cells("ArchiveID").Value = DBNull.Value Then
    '                nonArchivedCount += 1
    '            End If
    '        Next

    '        System.Diagnostics.Debug.WriteLine($"عدد السجلات غير المؤرشفة: {nonArchivedCount}")

    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في DebugSearchResults: {ex.Message}")
    '    End Try
    'End Sub

    'Private Sub dtpSearchDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpSearchDate.ValueChanged
    '    ' تعيين قيمة DateTimePicker إلى TextBox بتنسيق yyyy-MM-dd
    '    txtSearch.Text = dtpSearchDate.Value.ToString("yyyy-MM-dd")
    '    ' تنفيذ البحث تلقائيًا عند تغيير التاريخ
    '    PerformSearchSafely()
    'End Sub

    'Private Function GetSelectedRowIntegerValue(columnName As String) As Integer?
    '    Try
    '        Dim objValue As Object = GetSelectedRowObjectValue(columnName)
    '        If objValue Is Nothing Then
    '            Return Nothing
    '        End If
    '        Dim intValue As Integer
    '        If Integer.TryParse(objValue.ToString(), intValue) Then
    '            Return intValue
    '        End If
    '        Return Nothing
    '    Catch ex As Exception
    '        System.Diagnostics.Debug.WriteLine($"خطأ في GetSelectedRowIntegerValue: {ex.Message}")
    '        Return Nothing
    '    End Try
    'End Function

    '''' <summary>
    '''' فتح ملف PDF وإرساله للطباعة
    '''' </summary>
    'Private Sub PrintPDF(ByVal filePath As String)
    '    Dim psi As New ProcessStartInfo()
    '    psi.FileName = filePath
    '    If RBDirectPrint.Checked Then
    '        psi.Verb = "Print"
    '    ElseIf RBPreviewBeforePrint.Checked Then
    '        psi.Verb = "Open"
    '    End If

    '    psi.CreateNoWindow = True
    '    psi.WindowStyle = ProcessWindowStyle.Hidden

    '    Using p As New Process()
    '        p.StartInfo = psi
    '        p.Start()
    '        ' 🕒 انتظر قليلاً قبل إغلاق العملية (5 ثوانٍ)
    '        p.WaitForExit(5000)
    '    End Using




    'End Sub

    'Private Sub HandlePrintAction()
    '    Try
    '        ' 1. التحقق من وجود سجل محدد
    '        If Not IsValidRowSelected() Then
    '            MessageBox.Show("يرجى تحديد سجل صادر للطباعة من القائمة أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            dgvSader.Focus()
    '            Return
    '        End If

    '        ' 2. الحصول على معرف السجل المحدد
    '        Dim selectedSaderId As Integer = GetSelectedRowId()
    '        If selectedSaderId <= 0 Then
    '            MessageBox.Show("معرف الصادر غير صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Return
    '        End If

    '        ' 3. جلب البيانات مع التحقق من وجودها
    '        Dim saderData As DataTable = saderClass.GetSaderById(selectedSaderId)

    '        ' 4. التحقق من البيانات
    '        If saderData Is Nothing Then
    '            MessageBox.Show("لم يتم العثور على بيانات الصادر للسجل المحدد.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Return
    '        End If

    '        If saderData.Rows.Count = 0 Then
    '            MessageBox.Show("لا توجد بيانات للصادر المحدد.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Return
    '        End If

    '        ' 5. طباعة التقرير
    '        PrintSaderReport(saderData)
    '        ActivityLogClass.LogUserActivity(
    '                    GlobalData.LoggedInUserName,
    '                    "Delete",  ' ← تم التصحيح من "Add" إلى "Delete"
    '                    "TbSader",
    '                    id,
    '                    $"تم حذف كتاب صادر برقم: {numberBook}, الموضوع: {subjBook}" &
    '                    If(archiveID > 0, $", مع حذف الأرشيف المرتبط (معرّف: {archiveID})", "")
    '                )
    '        ' 6. إنشاء سجل جديد بعد الطباعة
    '        btnNew.PerformClick()
    '    Catch ex As Exception
    '        MessageBox.Show("حدث خطأ أثناء محاولة الطباعة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    'Private Sub PrintSaderReport(ByVal saderDataTable As DataTable)
    '    Try
    '        If ListPrinter.SelectedIndex = -1 Then
    '            MessageBox.Show("يرجى تحديد طابعة أولاً.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            Return
    '        End If
    '        Dim selectedPrinterName As String = ListPrinter.SelectedItem.ToString()
    '        Me.Cursor = Cursors.WaitCursor
    '        Dim cr As New CrystalReport1
    '        cr.SetDataSource(saderDataTable)
    '        ' تعيين التقرير في عنصر العرض
    '        CRV.ReportSource = cr
    '        CRV.Refresh()
    '        CRV.Zoom(60)
    '        ' تعيين الطابعة المحددة وطباعة التقرير
    '        cr.PrintOptions.PrinterName = selectedPrinterName
    '        cr.PrintToPrinter(1, False, 0, 0)
    '        'MessageBox.Show("تم إرسال التقرير إلى الطابعة بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Catch ex As Exception
    '        MessageBox.Show("خطأ في الطباعة: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        Me.Cursor = Cursors.Default
    '    End Try
    'End Sub


    '''' <summary>
    '''' طباعة تقرير الصادر باستخدام FastReport الإصدار المفتوح
    '''' </summary>
    'Private Sub PrintSaderReport(ByVal saderDataTable As DataTable)
    '    Dim report As FastReport.Report = Nothing
    '    Try
    '        'التحقق من وجود بيانات
    '        If saderDataTable Is Nothing OrElse saderDataTable.Rows.Count = 0 Then
    '            MessageBox.Show("لا توجد بيانات لطباعتها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            Return
    '        End If

    '        'التحقق من اختيار الطابعة
    '        If ListPrinter.SelectedIndex = -1 Then
    '            MessageBox.Show("يرجى تحديد طابعة أولاً.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            Return
    '        End If

    '        Me.Cursor = Cursors.WaitCursor

    '        'إنشاء كائن التقرير
    '        report = New FastReport.Report()

    '        'تحميل ملف التقرير
    '        Dim reportPath As String = Path.Combine(Application.StartupPath, "Reports", "SaderReport.frx")

    '        If Not File.Exists(reportPath) Then
    '            MessageBox.Show($"لم يتم العثور على ملف التقرير: {reportPath}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Return
    '        End If

    '        report.Load(reportPath)

    '        'ربط مصدر البيانات بالتقرير
    '        report.RegisterData(saderDataTable, "SaderData")
    '        report.GetDataSource("SaderData").Enabled = True

    '        '✅ التحضير للتقرير (ضروري قبل الطباعة)
    '        report.Prepare()

    '        '✅ تنفيذ الطباعة مباشرة باستخدام الطابعة المحددة
    '        report.Print(Nothing, False, ListPrinter.SelectedItem.ToString())



    '    Catch ex As FileNotFoundException
    '        MessageBox.Show($"ملف التقرير غير موجود:{vbNewLine}{ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    Catch ex As DirectoryNotFoundException
    '        MessageBox.Show($"مجلد التقارير غير موجود:{vbNewLine}{ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    Catch ex As Exception
    '        MessageBox.Show($"خطأ في الطباعة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    Finally
    '        'التأكد من التخلص من كائن التقرير
    '        If report IsNot Nothing Then
    '            report.Dispose()
    '        End If
    '        Me.Cursor = Cursors.Default
    '    End Try
    'End Sub

    '' <summary>
    '' طباعة تقرير الصادر باستخدام FastReport الإصدار المفتوح
    '' </summary>



    '''' <summary>
    '''' طباعة تقرير الصادر باستخدام FastReport OpenSource v2025.2.12
    '''' </summary>
    '''' <param name="saderDataTable">جدول البيانات المطلوب طباعته</param>
    'Private Sub PrintSaderReport(ByVal saderDataTable As DataTable)
    '    ' التحقق من وجود بيانات
    '    If saderDataTable Is Nothing OrElse saderDataTable.Rows.Count = 0 Then
    '        MessageBox.Show("لا توجد بيانات لطباعتها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        Return
    '    End If

    '    ' التحقق من اختيار الطابعة
    '    If ListPrinter.SelectedIndex = -1 Then
    '        MessageBox.Show("يرجى تحديد طابعة أولاً.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        Return
    '    End If

    '    Dim reportPath As String = Path.Combine(Application.StartupPath, "Reports", "SaderReport.frx")
    '    If Not File.Exists(reportPath) Then
    '        MessageBox.Show($"لم يتم العثور على ملف التقرير: {reportPath}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        Return
    '    End If

    '    Dim printerName As String = ListPrinter.SelectedItem.ToString()

    '    ' التحقق من أن الطابعة مثبتة
    '    Dim printerExists As Boolean = PrinterSettings.InstalledPrinters.Cast(Of String)().
    '    Any(Function(p) String.Equals(p, printerName, StringComparison.OrdinalIgnoreCase))
    '    If Not printerExists Then
    '        MessageBox.Show($"الطابعة المحددة غير مثبتة على النظام: {printerName}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        Return
    '    End If

    '    Me.Cursor = Cursors.WaitCursor

    '    Try
    '        Using report As New Report()
    '            ' تحميل التقرير
    '            report.Load(reportPath)

    '            ' ربط مصدر البيانات
    '            report.RegisterData(saderDataTable, "SaderData")
    '            Dim ds = report.GetDataSource("SaderData")
    '            If ds IsNot Nothing Then
    '                ds.Enabled = True
    '            End If

    '            ' تحضير التقرير للطباعة
    '            If Not report.Prepare() Then
    '                MessageBox.Show("فشل تحضير التقرير للطباعة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Return
    '            End If

    '            ' إعدادات الطباعة
    '            report.Sh
    '            report.PrintSettings.ShowDialog = False
    '            report.PrintSettings.Copies = 1
    '            report.PrintSettings.Collate = True

    '            ' ✅ الطباعة الفعلية
    '            report.PrintPrepare()
    '        End Using

    '    Catch ex As Exception
    '        MessageBox.Show($"حدث خطأ أثناء الطباعة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    Finally
    '        Me.Cursor = Cursors.Default
    '    End Try
    'End Sub




    'Private Sub PrintSaderReport(ByVal saderDataTable As DataTable)
    '    If saderDataTable Is Nothing OrElse saderDataTable.Rows.Count = 0 Then
    '        MessageBox.Show("لا توجد بيانات لطباعتها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        Return
    '    End If

    '    Dim reportPath As String = Path.Combine(Application.StartupPath, "Reports", "SaderReport.frx")
    '    If Not File.Exists(reportPath) Then
    '        MessageBox.Show($"لم يتم العثور على ملف التقرير: {reportPath}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        Return
    '    End If

    '    Dim tempPdf As String = Path.Combine(Path.GetTempPath(), "SaderReport.pdf")

    '    Try
    '        Using report As New Report()
    '            report.Load(reportPath)
    '            report.RegisterData(saderDataTable, "SaderData")
    '            Dim ds = report.GetDataSource("SaderData")
    '            If ds IsNot Nothing Then ds.Enabled = True

    '            report.Prepare()

    '            ' تصدير إلى PDF
    '            Dim pdfExport As New PDFSimpleExport()
    '            Using ms As New FileStream(tempPdf, FileMode.Create)
    '                pdfExport.Export(report, ms)
    '            End Using
    '        End Using

    '        ' طباعة PDF عبر عارض النظام الافتراضي (مثل Adobe Reader)
    '        Dim psi As New ProcessStartInfo()
    '        psi.FileName = tempPdf
    '        psi.Verb = "print"
    '        psi.CreateNoWindow = True
    '        psi.WindowStyle = ProcessWindowStyle.Hidden
    '        Process.Start(psi)

    '    Catch ex As Exception
    '        MessageBox.Show($"حدث خطأ أثناء الطباعة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub






    '''' <summary>
    '''' طباعة تقرير الصادر باستخدام FastReport.OpenSource
    '''' </summary>
    '''' <param name="saderDataTable"></param>
    'Private Sub PrintSaderReport(ByVal saderDataTable As DataTable)
    '    Dim report As Report = Nothing

    '    Try
    '        ' 🟡 التحقق من وجود بيانات
    '        If saderDataTable Is Nothing OrElse saderDataTable.Rows.Count = 0 Then
    '            MessageBox.Show("لا توجد بيانات لطباعتها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            Return
    '        End If

    '        ' 🟡 التحقق من اختيار الطابعة
    '        If ListPrinter.SelectedIndex = -1 Then
    '            MessageBox.Show("يرجى تحديد طابعة أولاً.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            Return
    '        End If

    '        Me.Cursor = Cursors.WaitCursor

    '        ' 🟢 إنشاء كائن التقرير
    '        report = New Report()

    '        ' 🟢 تحميل ملف التقرير
    '        Dim reportPath As String = Path.Combine(Application.StartupPath, "Reports", "SaderReport.frx")
    '        If Not File.Exists(reportPath) Then
    '            MessageBox.Show($"لم يتم العثور على ملف التقرير: {reportPath}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Return
    '        End If
    '        report.Load(reportPath)

    '        ' 🟢 ربط مصدر البيانات
    '        report.RegisterData(saderDataTable, "TbSader")
    '        report.GetDataSource("TbSader").Enabled = True

    '        ' 🟢 تحضير التقرير
    '        report.Prepare()

    '        ' 🟢 تصدير التقرير إلى PDF مؤقت
    '        Dim tempPdfPath As String = Path.Combine(Path.GetTempPath(), "SaderReport.pdf")
    '        Using pdfExport As New PDFSimpleExport()
    '            pdfExport.Export(report, tempPdfPath)

    '            'فتح التقرسر داخل فورم الصادر 
    '            AxAcroPDF1.src = tempPdfPath
    '        End Using

    '        ' 🖨️ طباعة ملف PDF باستخدام الطابعة الافتراضية
    '        PrintPDF(tempPdfPath)

    '    Catch ex As FileNotFoundException
    '        MessageBox.Show($"ملف التقرير غير موجود:{vbNewLine}{ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    Catch ex As DirectoryNotFoundException
    '        MessageBox.Show($"مجلد التقارير غير موجود:{vbNewLine}{ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    Catch ex As Exception
    '        MessageBox.Show($"خطأ في الطباعة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    Finally
    '        ' 🧹 التأكد من التخلص من كائن التقرير
    '        If report IsNot Nothing Then
    '            report.Dispose()
    '        End If
    '        AxAcroPDF1.src = Nothing
    '        Me.Cursor = Cursors.Default
    '    End Try
    'End Sub

    'Private Sub LoadBookCategories()
    '    Try
    '        ' إضافة التصنيفات إلى ComboBox
    '        cmbBookCategory.Items.Clear()
    '        cmbBookCategory.Items.Add("صادر")
    '        cmbBookCategory.Items.Add("وارد")
    '        cmbBookCategory.Items.Add("مذكرات")
    '        ' تعيين القيمة الافتراضية
    '        cmbBookCategory.SelectedIndex = 0 ' صادر كافتراضي
    '    Catch ex As Exception
    '        MessageBox.Show("حدث خطأ أثناء تحميل تصنيفات الكتاب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    'Private Sub LoadBookCategories()
    '    Try
    '        ' تحديد مسار الملف (يمكنك تعديله حسب موقع الملف)
    '        Dim filePath As String = Path.Combine(Application.StartupPath, "BookCategories.txt")

    '        ' التحقق من وجود الملف
    '        If Not File.Exists(filePath) Then
    '            ' إذا لم يكن الملف موجودًا، قم بإنشائه مع القيم الافتراضية
    '            Dim defaultCategories As String() = {"شركات اخرى", "الجمارك العامة", "مذكرات", "تصدير", "إفراج جمركي"}
    '            File.WriteAllLines(filePath, defaultCategories)
    '        End If

    '        ' قراءة التصنيفات من الملف
    '        Dim categories As String() = File.ReadAllLines(filePath)

    '        ' إضافة التصنيفات إلى ComboBox
    '        cmbBookCategory.Items.Clear()
    '        For Each category As String In categories
    '            ' تجاهل الأسطر الفارغة
    '            If Not String.IsNullOrWhiteSpace(category) Then
    '                cmbBookCategory.Items.Add(category.Trim())
    '            End If
    '        Next

    '        ' تعيين القيمة الافتراضية (إذا كانت هناك تصنيفات)
    '        If cmbBookCategory.Items.Count > 0 Then
    '            cmbBookCategory.SelectedIndex = 0
    '        End If

    '    Catch ex As Exception
    '        MessageBox.Show("حدث خطأ أثناء تحميل تصنيفات الكتاب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    'Private Sub CloseFileInWebBrowser(filepath As String)
    '    Try
    '        ' البحث عن جميع عناصر WebBrowser في الفورم
    '        For Each control As Control In Me.Controls
    '            If TypeOf control Is WebBrowser Then
    '                Dim webBrowser As WebBrowser = DirectCast(control, WebBrowser)
    '                webBrowser.Navigate("about:blank")
    '                Application.DoEvents()
    '                Thread.Sleep(200)
    '            End If
    '        Next

    '        ' البحث عن جميع عناصر WebBrowser في الفورمات الأخرى
    '        For Each form As Form In Application.OpenForms
    '            If form.Name <> Me.Name Then
    '                For Each control As Control In form.Controls
    '                    If TypeOf control Is WebBrowser Then
    '                        Dim webBrowser As WebBrowser = DirectCast(control, WebBrowser)
    '                        webBrowser.Navigate("about:blank")
    '                        Application.DoEvents()
    '                        Thread.Sleep(200)
    '                    End If
    '                Next
    '            End If
    '        Next
    '    Catch ex As Exception
    '        ' تجاهل الأخطاء في إغلاق WebBrowser
    '    End Try
    'End Sub

    'Private Sub InitializeDeleteButton()
    '    Try
    '        DGV.AddDeleteButtonColumn(
    '                    dataGridView:=dgvSader,
    '                    columnName:="DeleteButton",
    '                    headerText:="حذف",
    '                    buttonText:="حذف",
    '                    permissionForm:="SaderForm",
    '                    permissionAction:="Delete",
    '                    confirmMessage:="هل تريد حذف هذا الصادر؟",
    '                    successMessage:="تم حذف الصادر بنجاح",
    '                    deleteAction:=AddressOf DeleteSaderRecord
    '            )
    '    Catch ex As Exception
    '        MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    '''' <summary>
    '''' دالة حذف سجل الصادر
    '''' </summary>
    'Private Sub DeleteSaderRecord(id As Integer)
    '    Try
    '        Dim saderClass As New SaderClass()
    '        If saderClass.DeleteSader(id) Then
    '            ' تحديث عداد الصادرة
    '            DataHelper.UpdateSaderCount()
    '            ' تحديث عداد السجلات غير المرتبطة
    '            DataHelper.UpdateUnarchivedSaderCount()
    '        Else
    '            Throw New Exception("فشل في حذف الصادر")
    '        End If
    '    Catch ex As Exception
    '        Throw New Exception($"خطأ في حذف الصادر: {ex.Message}")
    '    End Try
    'End Sub

    'Private Sub TimerRefrech_Tick(sender As Object, e As EventArgs) Handles TimerRefrech.Tick
    '    Try

    '        GenerateNextBookNumber()
    '        SelectAll_TbSader(dgvSader)
    '        HighlightUnarchivedRecords()
    '        ' الاشتراك في الحدث
    '        AddHandler DataHelper.SaderCountChanged, AddressOf HandleDataCountChanged
    '        ' تحديث العداد في الواجهة
    '        UpdateRecordCount()
    '    Catch ex As Exception
    '        MessageBox.Show($"خطأ في الإتصال بالسيرفر: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try

    'End Sub
#End Region

End Class