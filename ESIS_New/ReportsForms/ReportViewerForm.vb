Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Windows.Forms

Public Class ReportViewerForm
    Private _reportDocument As ReportDocument
    Private _reportType As ReportGenerator.ReportType
    Private _criteria As ReportGenerator.ReportCriteria
    Private _isExporting As Boolean = False

    ' تعريف الأحداث
    Public Event ReportClosed()

    Public Sub New(reportDocument As ReportDocument, reportType As ReportGenerator.ReportType, criteria As ReportGenerator.ReportCriteria)
        InitializeComponent()
        _reportDocument = reportDocument
        _reportType = reportType
        _criteria = criteria
    End Sub

    Private Sub ReportViewerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' تهيئة واجهة المستخدم
            InitializeUI()

            ' عرض التقرير
            DisplayReport()
        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل عارض التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تهيئة واجهة المستخدم
    ''' </summary>
    Private Sub InitializeUI()
        ' تعيين العنوان
        Me.Text = GetReportTitle(_reportType)

        ' إعداد شريط الأدوات
        SetupToolbar()
    End Sub

    ''' <summary>
    ''' الحصول على عنوان التقرير
    ''' </summary>
    Private Function GetReportTitle(reportType As ReportGenerator.ReportType) As String
        Select Case reportType
            Case ReportGenerator.ReportType.OutgoingRecords
                Return "تقرير السجلات الصادرة"
            Case ReportGenerator.ReportType.ArchivedRecords
                Return "تقرير السجلات المؤرشفة"
            Case ReportGenerator.ReportType.UnarchivedRecords
                Return "تقرير السجلات غير المؤرشفة"
            Case ReportGenerator.ReportType.DeletedRecords
                Return "تقرير السجلات المحذوفة"
            Case ReportGenerator.ReportType.Custom
                Return "تقرير مخصص"
            Case Else
                Return "تقرير"
        End Select
    End Function

    ''' <summary>
    ''' إعداد شريط الأدوات
    ''' </summary>
    Private Sub SetupToolbar()
        ' إعداد شريط الأدوات
        tsPrint.Image = My.Resources.printer
        tsPrint.Text = "طباعة"
        tsExportPDF.Image = My.Resources.file
        tsExportPDF.Text = "تصدير إلى PDF"
        tsClose.Image = My.Resources.close_50px
        tsClose.Text = "إغلاق"
    End Sub

    ''' <summary>
    ''' عرض التقرير
    ''' </summary>
    Private Sub DisplayReport()
        Try
            ' عرض التقرير في عنصر التحكم
            crystalReportViewer.ReportSource = _reportDocument
            crystalReportViewer.Refresh()
        Catch ex As Exception
            MessageBox.Show($"خطأ في عرض التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر الطباعة
    ''' </summary>
    Private Sub tsPrint_Click(sender As Object, e As EventArgs) Handles tsPrint.Click
        Try
            ' طباعة التقرير
            _reportDocument.PrintToPrinter(1, False, 0, 0)
        Catch ex As Exception
            MessageBox.Show($"خطأ في طباعة التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' معالجة الضغط على زر التصدير إلى PDF
    ''' </summary>
    Private Sub tsExportPDF_Click(sender As Object, e As EventArgs) Handles tsExportPDF.Click
        Try
            ' اختيار مسار الحفظ
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "ملفات PDF|*.pdf"
            saveFileDialog.Title = "حفظ التقرير كـ PDF"
            saveFileDialog.FileName = $"{GetReportFileName()}.pdf"

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                _isExporting = True

                ' تصدير التقرير إلى PDF
                Dim reportGenerator As New ReportGenerator()
                If reportGenerator.ExportToPDF(_reportDocument, saveFileDialog.FileName) Then
                    MessageBox.Show("تم تصدير التقرير بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' سؤال المستخدم إذا أراد فتح الملف
                    Dim result As DialogResult = MessageBox.Show("هل تريد فتح ملف PDF الآن؟", "فتح ملف", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If result = DialogResult.Yes Then
                        Process.Start(saveFileDialog.FileName)
                    End If
                Else
                    MessageBox.Show("فشل في تصدير التقرير إلى PDF", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show($"خطأ في تصدير التقرير إلى PDF: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _isExporting = False
        End Try
    End Sub

    ''' <summary>
    ''' الحصول على اسم ملف التقرير
    ''' </summary>
    Private Function GetReportFileName() As String
        Dim reportName As String = String.Empty

        Select Case _reportType
            Case ReportGenerator.ReportType.OutgoingRecords
                reportName = "OutgoingRecords"
            Case ReportGenerator.ReportType.ArchivedRecords
                reportName = "ArchivedRecords"
            Case ReportGenerator.ReportType.UnarchivedRecords
                reportName = "UnarchivedRecords"
            Case ReportGenerator.ReportType.DeletedRecords
                reportName = "DeletedRecords"
            Case ReportGenerator.ReportType.Custom
                reportName = "CustomReport"
            Case Else
                reportName = "Report"
        End Select

        Return $"{reportName}_{DateTime.Now:yyyyMMdd_HHmmss}"
    End Function

    ''' <summary>
    ''' معالجة الضغط على زر الإغلاق
    ''' </summary>
    Private Sub tsClose_Click(sender As Object, e As EventArgs) Handles tsClose.Click
        Try
            ' إطلاق الحدث
            RaiseEvent ReportClosed()

            ' إغلاق النموذج
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"خطأ في إغلاق عارض التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' تنظيف الموارد عند الإغلاق
    ''' </summary>
    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        Try
            ' تفريغ التقرير
            If _reportDocument IsNot Nothing Then
                _reportDocument.Close()
                _reportDocument.Dispose()
                _reportDocument = Nothing
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"خطأ في تنظيف موارد التقرير: {ex.Message}")
        Finally
            MyBase.OnFormClosing(e)
        End Try
    End Sub
End Class