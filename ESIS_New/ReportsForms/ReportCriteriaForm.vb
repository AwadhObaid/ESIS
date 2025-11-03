Public Class ReportCriteriaForm
    Private _reportType As ReportGenerator.ReportType
    Private _criteria As ReportGenerator.ReportCriteria

    ' تعريف الأحداث
    Public Event CriteriaConfirmed(criteria As ReportGenerator.ReportCriteria)
    Public Event CancelCriteria()

    Public Sub New(reportType As ReportGenerator.ReportType)
        InitializeComponent()
        _reportType = reportType
        _criteria = New ReportGenerator.ReportCriteria()
    End Sub

    Private Sub ReportCriteriaForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' تهيئة واجهة المستخدم
            InitializeUI()
        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل نموذج معايير التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تهيئة واجهة المستخدم
    ''' </summary>
    Private Sub InitializeUI()
        ' تعيين العنوان حسب نوع التقرير
        Me.Text = GetReportTitle(_reportType)

        ' تعيين التاريخ الافتراضي
        dtpStartDate.Value = DateTime.Now.AddMonths(-1)
        dtpEndDate.Value = DateTime.Now

        ' إعداد العناصر بناءً على نوع التقرير
        SetupCriteriaControls()
    End Sub

    ''' <summary>
    ''' الحصول على عنوان التقرير
    ''' </summary>
    Private Function GetReportTitle(reportType As ReportGenerator.ReportType) As String
        Select Case reportType
            Case ReportGenerator.ReportType.OutgoingRecords
                Return "معاير تقرير السجلات الصادرة"
            Case ReportGenerator.ReportType.ArchivedRecords
                Return "معاير تقرير السجلات المؤرشفة"
            Case ReportGenerator.ReportType.UnarchivedRecords
                Return "معاير تقرير السجلات غير المؤرشفة"
            Case ReportGenerator.ReportType.DeletedRecords
                Return "معاير تقرير السجلات المحذوفة"
            Case ReportGenerator.ReportType.Custom
                Return "معاير تقرير مخصص"
            Case Else
                Return "معاير التقرير"
        End Select
    End Function

    ''' <summary>
    ''' إعداد عناصر واجهة المستخدم بناءً على نوع التقرير
    ''' </summary>
    Private Sub SetupCriteriaControls()
        ' إخفاء جميع العناصر أولاً
        pnlBookCategory.Visible = False
        pnlWillMaster.Visible = False
        pnlWillSub.Visible = False
        pnlArchiveFolder.Visible = False

        ' عرض العناصر المناسبة
        Select Case _reportType
            Case ReportGenerator.ReportType.OutgoingRecords, ReportGenerator.ReportType.UnarchivedRecords
                ' تحميل التصنيفات
                LoadBookCategories()
                pnlBookCategory.Visible = True

                ' تحميل المرسلين
                LoadWillMasters()
                pnlWillMaster.Visible = True

                ' تحميل المرسل إليهم
                LoadWillSubs()
                pnlWillSub.Visible = True

            Case ReportGenerator.ReportType.ArchivedRecords, ReportGenerator.ReportType.DeletedRecords
                ' تحميل مجلدات الأرشيف
                LoadArchiveFolders()
                pnlArchiveFolder.Visible = True
        End Select
    End Sub

    ''' <summary>
    ''' تحميل تصنيفات الكتب
    ''' </summary>
    Private Sub LoadBookCategories()
        Try
            ' استخدام ArchiveClass للحصول على التصنيفات
            Dim archiveClass As New ArchiveClass()
            Dim categories As List(Of String) = archiveClass.GetArchiveCategories()

            ' إضافة التصنيفات إلى القائمة
            cmbBookCategory.Items.Clear()
            cmbBookCategory.Items.Add("الكل")
            For Each category As String In categories
                cmbBookCategory.Items.Add(category)
            Next

            If cmbBookCategory.Items.Count > 0 Then
                cmbBookCategory.SelectedIndex = 0
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل تصنيفات الكتب: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحميل المرسلين
    ''' </summary>
    Private Sub LoadWillMasters()
        Try
            ' استخدام SaderClass للحصول على المرسلين
            Dim saderClass As New SaderClass()
            Dim willMasters As List(Of String) = saderClass.GetDistinctWillMasters()

            ' إضافة المرسلين إلى القائمة
            cmbWillMaster.Items.Clear()
            cmbWillMaster.Items.Add("الكل")
            For Each willMaster As String In willMasters
                cmbWillMaster.Items.Add(willMaster)
            Next

            If cmbWillMaster.Items.Count > 0 Then
                cmbWillMaster.SelectedIndex = 0
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل المرسلين: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحميل المرسل إليهم
    ''' </summary>
    Private Sub LoadWillSubs()
        Try
            ' استخدام SaderClass للحصول على المرسل إليهم
            Dim saderClass As New SaderClass()
            Dim willSubs As List(Of String) = saderClass.GetDistinctWillSubs()

            ' إضافة المرسل إليهم إلى القائمة
            cmbWillSub.Items.Clear()
            cmbWillSub.Items.Add("الكل")
            For Each willSub As String In willSubs
                cmbWillSub.Items.Add(willSub)
            Next

            If cmbWillSub.Items.Count > 0 Then
                cmbWillSub.SelectedIndex = 0
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل المرسل إليهم: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحميل مجلدات الأرشيف
    ''' </summary>
    Private Sub LoadArchiveFolders()
        Try
            ' استخدام ArchiveClass للحصول على المجلدات
            Dim archiveClass As New ArchiveClass()
            Dim folders As List(Of String) = archiveClass.GetArchiveFolders()

            ' إضافة المجلدات إلى القائمة
            cmbArchiveFolder.Items.Clear()
            cmbArchiveFolder.Items.Add("الكل")
            For Each folder As String In folders
                cmbArchiveFolder.Items.Add(folder)
            Next

            If cmbArchiveFolder.Items.Count > 0 Then
                cmbArchiveFolder.SelectedIndex = 0
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تحميل مجلدات الأرشيف: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر تأكيد المعايير
    ''' </summary>
    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        Try
            ' تجميع المعايير
            _criteria.StartDate = dtpStartDate.Value
            _criteria.EndDate = dtpEndDate.Value
            _criteria.ReportTitle = txtReportTitle.Text

            ' إعداد معايير إضافية حسب نوع التقرير
            Select Case _reportType
                Case ReportGenerator.ReportType.OutgoingRecords, ReportGenerator.ReportType.UnarchivedRecords
                    If cmbBookCategory.SelectedIndex > 0 Then
                        _criteria.BookCategory = cmbBookCategory.SelectedItem.ToString()
                    End If
                    If cmbWillMaster.SelectedIndex > 0 Then
                        _criteria.WillMaster = cmbWillMaster.SelectedItem.ToString()
                    End If
                    If cmbWillSub.SelectedIndex > 0 Then
                        _criteria.WillSub = cmbWillSub.SelectedItem.ToString()
                    End If

                Case ReportGenerator.ReportType.ArchivedRecords, ReportGenerator.ReportType.DeletedRecords
                    If cmbArchiveFolder.SelectedIndex > 0 Then
                        _criteria.ArchiveFolder = cmbArchiveFolder.SelectedItem.ToString()
                    End If
            End Select

            ' إطلاق الحدث
            RaiseEvent CriteriaConfirmed(_criteria)

            ' إغلاق النموذج
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"خطأ في تأكيد معايير التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر الإلغاء
    ''' </summary>
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            ' إطلاق الحدث
            RaiseEvent CancelCriteria()

            ' إغلاق النموذج
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"خطأ في إلغاء معايير التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class