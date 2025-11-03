Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Windows.Forms
Imports ESIS_New.ESIS_New

''' <summary>
''' نموذج لعرض تقرير سجلات الحركة
''' </summary>
Public Class UserActivityLogReportForm
    Inherits BaseFormWithPermissions

    Private reportDocument As ReportDocument
    Private reportPath As String = Application.StartupPath & "\Reports\UserActivityLogReport.rpt"

    ''' <summary>
    ''' تحميل النموذج
    ''' </summary>
    Private Sub UserActivityLogReportForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' تطبيق الصلاحيات
            ApplyPermissions()

            ' تهيئة النموذج
            SetupForm()

            ' تحميل التقرير
            LoadReport()

        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل تقرير سجلات الحركة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تهيئة النموذج
    ''' </summary>
    Private Sub SetupForm()
        Try
            ' تعيين خصائص النموذج
            Me.Text = "تقرير سجلات الحركة"
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Normal

            ' تهيئة الأزرار
            btnPrint.Enabled = True

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تهيئة النموذج: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تحميل التقرير
    ''' </summary>
    Private Sub LoadReport()
        Try
            ' التحقق من وجود ملف التقرير
            If Not File.Exists(reportPath) Then
                Throw New FileNotFoundException($"لم يتم العثور على ملف التقرير: {reportPath}")
            End If

            ' تحميل التقرير
            reportDocument = New ReportDocument()
            reportDocument.Load(reportPath)

            ' تعيين مصدر البيانات للتقرير
            SetReportDataSource()

            ' عرض التقرير في العارض
            If UserActivityLogReportViewer IsNot Nothing Then
                UserActivityLogReportViewer.ReportSource = reportDocument
            End If

        Catch ex As Exception
            Throw New Exception($"فشل في تحميل التقرير: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' تعيين مصدر بيانات للتقرير
    ''' </summary>
    Private Sub SetReportDataSource()
        Try
            ' الحصول على بيانات سجلات الحركة
            Dim activityClass As New ActivityLogClass()
            Dim dataTable As DataTable = activityClass.GetActivityLogs()

            ' تعيين جدول البيانات كمصدر للبيانات
            If dataTable IsNot Nothing AndAlso dataTable.Rows.Count > 0 Then
                reportDocument.SetDataSource(dataTable)
            Else
                ' إنشاء جدول فارغ لتجنب الأخطاء
                Dim emptyTable As New DataTable()
                emptyTable.Columns.Add("LogID", GetType(Integer))
                emptyTable.Columns.Add("UserName", GetType(String))
                emptyTable.Columns.Add("ActionType", GetType(String))
                emptyTable.Columns.Add("TableNameArabic", GetType(String))
                emptyTable.Columns.Add("RecordID", GetType(Integer))
                emptyTable.Columns.Add("ActionDetails", GetType(String))
                emptyTable.Columns.Add("IPAddress", GetType(String))
                emptyTable.Columns.Add("ActionDate", GetType(DateTime))

                reportDocument.SetDataSource(emptyTable)
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تعيين مصدر البيانات: {ex.Message}")
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر الطباعة
    ''' </summary>
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click


        If CheckBoxDircet.Checked = False Then
            Try
                If reportDocument IsNot Nothing Then
                    ' عرض نافذة المعاينة قبل الطباعة
                    Dim printDialog As New PrintDialog()

                    '' إعداد خيارات الطباعة
                    'With printDialog.PrinterSettings
                    '    .AllowSomePages = True
                    '    .AllowCurrentPage = True
                    '    .AllowSelection = True
                    'End With

                    ' عرض مربع حوار المعاينة
                    If printDialog.ShowDialog() = DialogResult.OK Then
                        ' إذا وافق المستخدم، قم بالطباعة
                        reportDocument.PrintToPrinter(printDialog.PrinterSettings.Copies,
                                                    False,
                                                    printDialog.PrinterSettings.FromPage,
                                                    printDialog.PrinterSettings.ToPage)

                        MessageBox.Show("تم إرسال التقرير إلى الطابعة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    MessageBox.Show("لم يتم تحميل التقرير بعد", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

            Catch ex As Exception
                MessageBox.Show($"خطأ في الطباعة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            Try
                If reportDocument IsNot Nothing Then
                    ' عرض مربع حوار الطباعة
                    reportDocument.PrintToPrinter(1, False, 0, 0)

                    MessageBox.Show("تم إرسال التقرير إلى الطابعة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("لم يتم تحميل التقرير بعد", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

            Catch ex As Exception
                MessageBox.Show($"خطأ في الطباعة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If







    End Sub

    ''' <summary>
    ''' تنظيف الموارد عند إغلاق النموذج
    ''' </summary>
    Private Sub UserActivityLogReportForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            ' تفريغ تقرير كريستال
            If reportDocument IsNot Nothing Then
                reportDocument.Close()
                reportDocument.Dispose()
                reportDocument = Nothing
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تنظيف الموارد: {ex.Message}")
        End Try
    End Sub
End Class