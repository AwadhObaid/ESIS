Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports System.IO
Imports System.Threading
Imports FastReport
Imports FastReport.Export.PdfSimple
Imports PdfiumViewer

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
    ' متغير لتخزين معرف سجل الصادر الحالي
    ' يفترض أن يتم تعيينه بعد حفظ سجل الصادر
    Private currentSaderID As Integer = 0
    Private _eventsWired As Boolean = False

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
        isLoadingData = True

        Try
            WireEventsOnce()

            PrepareUI()
            PrepareDefaults()

            PrepareGrid()

            LoadLookups()
            LoadDataAndState()

            FinishLoad()

        Catch ex As Exception
            MessageBox.Show($"حدث خطأ أثناء تحميل النموذج: {ex.Message}",
                        "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            isLoadingData = False
        End Try
    End Sub

    '========================
    ' 1) ربط الأحداث مرة واحدة
    '========================
    Private Sub WireEventsOnce()
        If _eventsWired Then Return
        _eventsWired = True

        AddHandler DataHelper.SaderCountChanged, AddressOf HandleDataCountChanged

        AddHandler txtWillMaster.TextChanged, AddressOf PolicyFields_TextChanged
        AddHandler txtWillSub.TextChanged, AddressOf PolicyFields_TextChanged

        AddHandler dgvSader.CellClick, AddressOf dgvSader_CellClick
    End Sub

    '========================
    ' 2) تجهيز الواجهة العامة
    '========================
    Private Sub PrepareUI()
        Me.RightToLeft = RightToLeft.Yes
        Me.RightToLeftLayout = True

        txtUserName.Text = GlobalData.LoggedInUserName
        txtUserName.ReadOnly = True

        txtSearch.Enabled = False

        EnableEditingTools(False)
    End Sub

    '========================
    ' 3) قيم افتراضية للتواريخ والحقول
    '========================
    Private Sub PrepareDefaults()
        txtWillMaster.Clear()
        txtWillSub.Clear()

        dtpDateBook.Value = DateTime.Now
        dtpStartDate.Value = DateTime.Now.AddDays(-30)
        dtpEndDate.Value = DateTime.Now

        ComboSubjBook.SelectedIndex = -1
    End Sub

    '========================
    ' 4) تجهيز الداتا جريد
    '========================
    Private Sub PrepareGrid()
        DataGridViewErrorHandler.SetupErrorHandling(dgvSader)
        DataGridViewHelper.SetupArabicDataGridView(dgvSader)

        dgvSader.ClearSelection()
    End Sub

    '========================
    ' 5) تحميل القوائم/الإكمال التلقائي
    '========================
    Private Sub LoadLookups()
        PolicyValidationEvents.AttachPolicyValidationEvents(Me)

        ClearFields()

        saderClass.AutoCompleteForTextBox("WillMaster", "TbSader", txtWillMaster)
        saderClass.AutoCompleteForTextBox("WillSub", "TbSader", txtWillSub)
        saderClass.AutoCompleteForTextBox("NumberBook", "TbSader", txtSearch)

        FillComboBox(ComboSubjBook,
                 "SELECT Subject_Book FROM TbSubjBooks ORDER BY Subject_Book",
                 "Subject_Book")

        LoadBookCategories()
        PopulatePrintersList()
        LoadSavedPrinter()
    End Sub

    '========================
    ' 6) تحميل البيانات وحالة النموذج
    '========================
    Private Sub LoadDataAndState()
        UpdateRecordCount()

        SelectAll_TbSader(dgvSader)
        GenerateNextBookNumber()

        HighlightUnarchivedRecords()
        ClearFieldsOnLoad()
    End Sub

    '========================
    ' 7) إنهاء التحميل
    '========================
    Private Sub FinishLoad()
        dgvSader.ClearSelection()
    End Sub


    'Private Sub SaderForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '    Try

    '        ' الاشتراك في الحدث
    '        AddHandler DataHelper.SaderCountChanged, AddressOf HandleDataCountChanged
    '        'lblArchiveMsg.Visible = False
    '        ' تحديث العداد في الواجهة
    '        UpdateRecordCount()
    '        ' ربط تلقائي لأحداث التحقق من البوليصات
    '        PolicyValidationEvents.AttachPolicyValidationEvents(Me)

    '        ' إضافة معالجات الأحداث للحقول لتحديد حالة وجود بيانات غير محفوظة
    '        AddHandler txtWillMaster.TextChanged, AddressOf PolicyFields_TextChanged
    '        AddHandler txtWillSub.TextChanged, AddressOf PolicyFields_TextChanged
    '        'RBDirectPrint.Checked = True
    '        isLoadingData = True
    '        txtWillMaster.Clear()
    '        txtWillSub.Clear()
    '        ' إعداد معالج أخطاء DataGridView
    '        DataGridViewErrorHandler.SetupErrorHandling(dgvSader)
    '        ' تعطيل الأحداث قبل تحميل البيانات

    '        RemoveHandler dgvSader.CellClick, AddressOf dgvSader_CellClick
    '        ' إعداد DataGridView للتطبيقات العربية
    '        DataGridViewHelper.SetupArabicDataGridView(dgvSader)
    '        ClearFields()
    '        ' تعبئة ميزة الإكمال التلقائي في مربع النصوص
    '        saderClass.AutoCompleteForTextBox("WillMaster", "TbSader", txtWillMaster)
    '        saderClass.AutoCompleteForTextBox("WillSub", "TbSader", txtWillSub)
    '        saderClass.AutoCompleteForTextBox("NumberBook", "TbSader", txtSearch)
    '        ' تعبئة القائمة المنسدلة بالبيانات
    '        FillComboBox(ComboSubjBook, "SELECT Subject_Book FROM TbSubjBooks ORDER BY Subject_Book", "Subject_Book")
    '        Me.RightToLeft = RightToLeft.Yes
    '        Me.RightToLeftLayout = True
    '        dtpDateBook.Value = DateTime.Now
    '        dtpStartDate.Value = DateTime.Now.AddDays(-30)
    '        dtpEndDate.Value = DateTime.Now
    '        SelectAll_TbSader(dgvSader)
    '        GenerateNextBookNumber()
    '        PopulatePrintersList()
    '        LoadSavedPrinter()
    '        LoadBookCategories() ' تحميل تصنيفات الكتاب
    '        HighlightUnarchivedRecords()
    '        txtUserName.Text = GlobalData.LoggedInUserName
    '        txtUserName.ReadOnly = True
    '        ClearFieldsOnLoad()
    '        txtSearch.Enabled = False
    '        ' إلغاء تحديد أي صفوف في dgvSader بعد تحميل البيانات
    '        dgvSader.ClearSelection()
    '        ' تعطيل أدوات التعديل في البداية
    '        EnableEditingTools(False)
    '        isLoadingData = False
    '        ' إعادة تسجيل الأحداث بعد تحميل البيانات
    '        AddHandler dgvSader.CellClick, AddressOf dgvSader_CellClick
    '        ComboSubjBook.SelectedIndex = -1
    '    Catch ex As Exception
    '        isLoadingData = False
    '        MessageBox.Show($"حدث خطأ أثناء تحميل النموذج: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try


    'End Sub

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
            Timer1.Start()
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

#Region "CRUD Operations  عمليات CRUD"
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        'RBDirectPrint.Checked = True
        SelectAll_TbSader(dgvSader)
        StopFlashingLabel()
        Me.Refresh()
        cmbBookCategory.DroppedDown = True
        cmbBookCategory.Focus()
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
                DataHelper.UpdateArchiveCount()
                DataHelper.UpdateUnarchivedSaderCount()
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
                                    If archiveForm.pdfViewer IsNot Nothing Then
                                        ' تفريغ العارض + تحرير قفل الملف
                                        ClearPdf(archiveForm.pdfViewer)

                                        Application.DoEvents()
                                        Thread.Sleep(200)

                                        ' (اختياري) لو عندك دالة ForceCloseFileHandles خَلّها كما هي
                                        ForceCloseFileHandles(currentFilePath)
                                        Thread.Sleep(300)
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

#Region "Printing & Printers | الطباعة والطابعات"

    '========================
    ' زر الطباعة الرئيسي
    '========================
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click

        ' التحقق من الصلاحية
        If Not CheckPermission(1, "طباعة ملف صادر") Then Return

        HandlePrintAction()

    End Sub


    '==================================================
    ' معالجة إجراء الطباعة + تسجيل النشاط
    '==================================================
    Private Sub HandlePrintAction()

        Try
            ' 1️⃣ التحقق من تحديد سجل
            If Not IsValidRowSelected() Then
                MessageBox.Show("يرجى تحديد سجل صادر للطباعة أولاً.", "تنبيه",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
                dgvSader.Focus()
                LogPrintActivity("Failed_Print", -1, "محاولة طباعة دون تحديد سجل")
                Return
            End If

            ' 2️⃣ جلب معرف السجل
            Dim selectedSaderId As Integer = GetSelectedRowId()
            If selectedSaderId <= 0 Then
                MessageBox.Show("معرف الصادر غير صالح.", "خطأ",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                LogPrintActivity("Failed_Print", -1, "معرف غير صالح")
                Return
            End If

            ' 3️⃣ جلب بيانات الصادر
            Dim saderData As DataTable = saderClass.GetSaderById(selectedSaderId)

            If saderData Is Nothing OrElse saderData.Rows.Count = 0 Then
                MessageBox.Show("لا توجد بيانات للصادر المحدد.", "خطأ",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                LogPrintActivity("Failed_Print", selectedSaderId, "لا توجد بيانات")
                Return
            End If

            ' 4️⃣ تنفيذ الطباعة
            PrintSaderReport(saderData)

            ' 5️⃣ تسجيل نجاح الطباعة
            LogPrintActivity(
            "Print",
            selectedSaderId,
            $"تم طباعة الصادر رقم {saderData.Rows(0)("NumberBook")}"
        )

            ' 6️⃣ إنشاء سجل جديد
            btnNew.PerformClick()

        Catch ex As Exception
            LogPrintActivity(
            "Error_Print",
            If(IsValidRowSelected(), GetSelectedRowId(), -1),
            ex.Message
        )
            MessageBox.Show("خطأ أثناء الطباعة: " & ex.Message, "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub


    '==================================================
    ' إنشاء تقرير الصادر + تصدير PDF + طباعة
    '==================================================
    Private Sub PrintSaderReport(ByVal saderDataTable As DataTable)

        Dim report As Report = Nothing

        Try
            ' التحقق من اختيار الطابعة
            If ListPrinter.CheckedItems.Count = 0 Then
                MessageBox.Show("يرجى اختيار طابعة أولاً.", "تنبيه",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim selectedPrinter As String = ListPrinter.CheckedItems(0).ToString()
            Me.Cursor = Cursors.WaitCursor

            ' إنشاء التقرير
            report = New Report()
            Dim reportPath As String =
            Path.Combine(Application.StartupPath, "Reports", "SaderReport.frx")

            If Not File.Exists(reportPath) Then
                MessageBox.Show("ملف التقرير غير موجود.", "خطأ",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            report.Load(reportPath)
            report.RegisterData(saderDataTable, "TbSader")
            report.GetDataSource("TbSader").Enabled = True
            report.Prepare()

            ' تصدير PDF مؤقت
            Dim tempPdfPath As String =
            Path.Combine(Path.GetTempPath(),
                         $"SaderReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf")

            Using pdfExport As New PDFSimpleExport()
                pdfExport.Export(report, tempPdfPath)
            End Using

            ' معاينة قبل الطباعة
            If RBPreviewBeforePrint.Checked Then
                ShowPdfPreview(tempPdfPath)
            End If

            ' الطباعة المباشرة
            If RBDirectPrint.Checked Then

                Dim copies As Integer = CInt(nudCopies.Value)

                Dim trayName As String = If(cmbTrays.SelectedIndex <> -1, cmbTrays.SelectedItem.ToString(), String.Empty)

                PrintPdfAdvanced(tempPdfPath, selectedPrinter, copies, trayName)

            End If

        Finally
            If report IsNot Nothing Then report.Dispose()
            Me.Cursor = Cursors.Default
        End Try

    End Sub


    '==================================================
    ' طباعة PDF مع A4 + Tray + Copies
    '==================================================
    Private Sub PrintPdfAdvanced(
    ByVal pdfPath As String,
    ByVal printerName As String,
    ByVal copies As Integer,
    ByVal trayName As String)

        If Not File.Exists(pdfPath) Then Exit Sub

        Using pdfDoc As PdfDocument = PdfDocument.Load(pdfPath)
            Using printDoc As PrintDocument = pdfDoc.CreatePrintDocument()

                printDoc.PrinterSettings.PrinterName = printerName
                printDoc.PrinterSettings.Copies = CShort(copies)

                ' تحديد حجم A4
                For Each p As PaperSize In printDoc.PrinterSettings.PaperSizes
                    If p.Kind = PaperKind.A4 Then
                        printDoc.DefaultPageSettings.PaperSize = p
                        Exit For
                    End If
                Next

                ' تحديد درج الطابعة
                If Not String.IsNullOrWhiteSpace(trayName) Then
                    For Each src As PaperSource In printDoc.PrinterSettings.PaperSources
                        If src.SourceName.Equals(trayName,
                        StringComparison.OrdinalIgnoreCase) Then
                            printDoc.DefaultPageSettings.PaperSource = src
                            Exit For
                        End If
                    Next
                End If

                printDoc.DefaultPageSettings.Landscape = False
                printDoc.PrintController = New StandardPrintController()
                printDoc.Print()

            End Using
        End Using

    End Sub


    '==================================================
    ' معاينة PDF
    '==================================================
    Private Sub ShowPdfPreview(ByVal pdfPath As String)

        Using pdfDoc As PdfDocument = PdfDocument.Load(pdfPath)
            Dim viewer As New PdfViewer With {
            .Dock = DockStyle.Fill,
            .Document = pdfDoc
        }

            Dim f As New Form With {
            .Text = "معاينة التقرير",
            .WindowState = FormWindowState.Maximized
        }
            f.Controls.Add(viewer)
            f.ShowDialog()
        End Using

    End Sub


    '==================================================
    ' تحميل الطابعات
    '==================================================
    Private Sub PopulatePrintersList()
        ListPrinter.Items.Clear()
        For Each p As String In PrinterSettings.InstalledPrinters
            ListPrinter.Items.Add(p)
        Next
    End Sub


    '==================================================
    ' تحميل أدراج الطابعة
    '==================================================
    Private Sub LoadPrinterTrays(ByVal printerName As String)

        cmbTrays.Items.Clear()
        Dim pd As New PrintDocument()
        pd.PrinterSettings.PrinterName = printerName

        For Each src As PaperSource In pd.PrinterSettings.PaperSources
            cmbTrays.Items.Add(src.SourceName)
        Next

        If cmbTrays.Items.Count > 0 Then cmbTrays.SelectedIndex = 0

    End Sub


    '==================================================
    ' السماح بطابعة واحدة فقط + تحميل الأدراج
    '==================================================
    Private Sub ListPrinter_ItemCheck(
    sender As Object,
    e As ItemCheckEventArgs) Handles ListPrinter.ItemCheck

        For i = 0 To ListPrinter.Items.Count - 1
            If i <> e.Index Then ListPrinter.SetItemChecked(i, False)
        Next

        Me.BeginInvoke(Sub()
                           LoadPrinterTrays(ListPrinter.Items(e.Index).ToString())
                       End Sub)
        ' حفظ الطابعة المختارة
        My.Settings.DefaultPrinterName = ListPrinter.Items(e.Index).ToString()
        My.Settings.Save()

    End Sub


    '==================================================
    ' تسجيل نشاط الطباعة
    '==================================================
    Private Sub LogPrintActivity(
    actionName As String,
    recordId As Integer,
    description As String)

        Try
            Dim log As New ActivityLogClass()
            log.LogUserActivity(
            GlobalData.LoggedInUserName,
            actionName,
            "TbSader",
            recordId,
            description)
        Catch
        End Try

    End Sub

    '==================================================
    ' حفظ الطابعة المحددة في اعدادات النظام
    '==================================================

    Private Sub LoadSavedPrinter()

        Try
            If ListPrinter.Items.Count = 0 Then Exit Sub

            Dim savedPrinter As String = My.Settings.DefaultPrinterName

            ' إلغاء أي تحديد سابق
            For i As Integer = 0 To ListPrinter.Items.Count - 1
                ListPrinter.SetItemChecked(i, False)
            Next

            ' في حال وجود طابعة محفوظة
            If Not String.IsNullOrWhiteSpace(savedPrinter) Then

                Dim index As Integer = ListPrinter.Items.IndexOf(savedPrinter)

                If index <> -1 Then
                    ListPrinter.SetItemChecked(index, True)

                    ' تحميل الأدراج للطابعة المحفوظة
                    LoadPrinterTrays(savedPrinter)
                    Exit Sub
                End If
            End If

            ' في حال عدم وجود طابعة محفوظة → اختر الأولى
            ListPrinter.SetItemChecked(0, True)
            LoadPrinterTrays(ListPrinter.Items(0).ToString())

        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل الطابعة المحفوظة: " & ex.Message,
                        "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

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
                    ClearPdf(archiveForm.pdfViewer)

                    archiveForm.btnBrowseFile.PerformClick()
                    archiveForm.BringToFront()
                Else
                    ' إذا كان النموذج مغلقًا، قم بفتحه
                    Dim main As MainForm = TryCast(Application.OpenForms("MainForm"), MainForm)
                    If main IsNot Nothing Then
                        Dim newArchiveForm As New ArchiveForm(numberBook, dateBook, subjBook, willMaster, willSub, bookCategory)
                        newArchiveForm.MdiParent = main
                        newArchiveForm.Show()
                        Select Case MessageBox.Show("هل تريد مسح الملف ضوئيا؟", "رسالة تنبيه", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                            Case DialogResult.Yes
                                Dim scanForm As New ScannerForm
                                scanForm.Show(Me)
                            Case DialogResult.No
                                newArchiveForm.btnBrowseFile.PerformClick()
                            Case DialogResult.Cancel
                                newArchiveForm.Close()
                        End Select

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
                                                    "الاستلام المباشر", "شركات اخرى", "الجمارك العامة", "مذكرات", "تصدير", "إفراج جمركي"}

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
        ' إنشاء لون عشوائي
        Dim random As New Random()
        Dim r As Integer = random.Next(0, 256) ' قيمة حمراء عشوائية بين 0 و 255
        Dim g As Integer = random.Next(0, 256) ' قيمة خضراء عشوائية
        Dim b As Integer = random.Next(0, 256) ' قيمة زرقاء عشوائية
        Dim randomColor As Color = Color.FromArgb(r, g, b)

        ' تغيير لون النص للون العشوائي
        lblArchiveMsg.ForeColor = randomColor

        ' جعل النص يلمع (يظهر ويختفي)
        ' إذا كان النص مرئيًا، اجعله غير مرئي والعكس صحيح
        lblArchiveMsg.Visible = Not lblArchiveMsg.Visible



        'lblArchiveMsg.Visible = False
    End Sub

    ' إذا أردت إيقاف التوهج لاحقًا (مثلاً عند ضغط زر)
    Private Sub StopFlashingLabel()
        Timer1.Stop()
        lblArchiveMsg.Visible = False ' تأكد من أن النص مرئي عند الإيقاف
        lblArchiveMsg.ForeColor = SystemColors.ControlText ' إعادة اللون الافتراضي
    End Sub

    Private Sub CbDateBook_CheckedChanged(sender As Object, e As EventArgs) Handles CbDateBook.CheckedChanged
        If CbDateBook.Checked Then
            dtpDateBook.Enabled = True

        Else
            dtpDateBook.Enabled = False
        End If
    End Sub











#End Region

End Class