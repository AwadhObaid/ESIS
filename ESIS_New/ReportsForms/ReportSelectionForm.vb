Public Class ReportSelectionForm
    ' تعريف الأحداث
    Public Event ReportSelected(reportType As ReportGenerator.ReportType)
    Public Event CancelReportSelection()

    ' دالة التحميل
    Private Sub ReportSelectionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' تهيئة العناصر
            InitializeUI()
        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل نموذج اختيار التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تهيئة واجهة المستخدم
    ''' </summary>
    Private Sub InitializeUI()
        ' إعداد القائمة
        cmbReportType.Items.Add("تقرير السجلات الصادرة")
        cmbReportType.Items.Add("تقرير السجلات المؤرشفة")
        cmbReportType.Items.Add("تقرير السجلات غير المؤرشفة")
        cmbReportType.Items.Add("تقرير السجلات المحذوفة")
        cmbReportType.Items.Add("تقرير مخصص")

        If cmbReportType.Items.Count > 0 Then
            cmbReportType.SelectedIndex = 0
        End If

        ' تعيين التاريخ الافتراضي
        dtpStartDate.Value = DateTime.Now.AddMonths(-1)
        dtpEndDate.Value = DateTime.Now
    End Sub

    ''' <summary>
    ''' معالجة اختيار نوع التقرير
    ''' </summary>
    Private Sub cmbReportType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbReportType.SelectedIndexChanged
        Try
            ' إظهار العناصر المناسبة حسب نوع التقرير
            pnlAdditionalCriteria.Visible = (cmbReportType.SelectedIndex = 0 OrElse cmbReportType.SelectedIndex = 2)
            pnlArchiveFolder.Visible = (cmbReportType.SelectedIndex = 1 OrElse cmbReportType.SelectedIndex = 3)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في معالجة اختيار نوع التقرير: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر المتابعة
    ''' </summary>
    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            ' التحقق من صحة المدخلات
            If Not ValidateInput() Then
                Return
            End If

            ' تحديد نوع التقرير
            Dim reportType As ReportGenerator.ReportType
            Select Case cmbReportType.SelectedIndex
                Case 0
                    reportType = ReportGenerator.ReportType.OutgoingRecords
                Case 1
                    reportType = ReportGenerator.ReportType.ArchivedRecords
                Case 2
                    reportType = ReportGenerator.ReportType.UnarchivedRecords
                Case 3
                    reportType = ReportGenerator.ReportType.DeletedRecords
                Case 4
                    reportType = ReportGenerator.ReportType.Custom
                Case Else
                    reportType = ReportGenerator.ReportType.OutgoingRecords
            End Select

            ' إطلاق الحدث
            RaiseEvent ReportSelected(reportType)

            ' إغلاق النموذج
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"خطأ في اختيار التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' التحقق من صحة المدخلات
    ''' </summary>
    Private Function ValidateInput() As Boolean
        ' التحقق من تواريخ صحيحة
        If dtpStartDate.Value > dtpEndDate.Value Then
            MessageBox.Show("يجب أن يكون تاريخ البدء قبل تاريخ الانتهاء", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            dtpStartDate.Focus()
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' معالجة الضغط على زر الإلغاء
    ''' </summary>
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            ' إطلاق الحدث
            RaiseEvent CancelReportSelection()

            ' إغلاق النموذج
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"خطأ في إلغاء اختيار التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class