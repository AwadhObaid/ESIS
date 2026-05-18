Imports PdfiumViewer

Public Class FrmReports

#Region "المتغيرات"

    Private scaler As ControlScaler

    Private reportHelper As New ReportsHelper()

    Private currentPdf As String = ""

#End Region

#Region "تهيئة النموذج"

    Public Sub New()
        ' هذا السطر مطلوب لتهيئة النموذج.
        InitializeComponent()
        ' تهيئة الكلاس مع النموذج الحالي
        scaler = New ControlScaler(Me)
        'InitializeDeleteButton()
    End Sub

    Private Sub SaderForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        ' التحقق من أن الكائن scaler مهيأ قبل استدعائه
        If scaler IsNot Nothing Then
            scaler.ScaleControls(Me)
        End If
    End Sub

#End Region

#Region "تحميل الفورم"

    Private Sub FrmReports_Load(
        sender As Object,
        e As EventArgs) Handles MyBase.Load

        ' أنواع التقارير
        cmbReportType.Items.Add("الصادر")
        cmbReportType.Items.Add("غير المؤرشف")   ' بدلاً من "الغير مؤرشف"
        cmbReportType.Items.Add("الأرشيف")
        cmbReportType.Items.Add("المحذوفات")


        cmbReportType.SelectedIndex = 0

        cmbYear.Items.Clear()

        Dim years = reportHelper.GetYears()

        For Each y In years
            cmbYear.Items.Add(y)
        Next

        If cmbYear.Items.Count > 0 Then
            cmbYear.SelectedIndex = 0
        End If

        ' التاريخ
        dtpFrom.Value = Date.Today.AddMonths(-1)
        dtpTo.Value = Date.Today

        ' الطابعات
        reportHelper.LoadPrinters(ListPrinter)

        ' تحميل المحفوظة
        LoadSavedPrinter()

    End Sub

    Private Sub ListPrinter_ItemCheck(
    sender As Object,
    e As ItemCheckEventArgs) Handles ListPrinter.ItemCheck

        For i = 0 To ListPrinter.Items.Count - 1

            If i <> e.Index Then
                ListPrinter.SetItemChecked(i, False)
            End If

        Next

        Me.BeginInvoke(Sub()

                           Dim printer =
                           ListPrinter.Items(e.Index).ToString()

                           reportHelper.LoadPrinterTrays(
                           cmbTrays,
                           printer)

                       End Sub)

        My.Settings.DefaultPrinterName =
        ListPrinter.Items(e.Index).ToString()

        My.Settings.Save()

    End Sub

    Private Sub LoadSavedPrinter()

        Try

            If ListPrinter.Items.Count = 0 Then Exit Sub

            Dim savedPrinter As String =
            My.Settings.DefaultPrinterName

            For i As Integer = 0 To ListPrinter.Items.Count - 1
                ListPrinter.SetItemChecked(i, False)
            Next

            If Not String.IsNullOrWhiteSpace(savedPrinter) Then

                Dim index As Integer =
                ListPrinter.Items.IndexOf(savedPrinter)

                If index <> -1 Then

                    ListPrinter.SetItemChecked(index, True)

                    reportHelper.LoadPrinterTrays(
                    cmbTrays,
                    savedPrinter)

                    Exit Sub

                End If

            End If

            ListPrinter.SetItemChecked(0, True)

            reportHelper.LoadPrinterTrays(
            cmbTrays,
            ListPrinter.Items(0).ToString())

        Catch ex As Exception

            MessageBox.Show(
            ex.Message,
            "خطأ",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btnResetFilters_Click(sender As Object, e As EventArgs) Handles btnResetFilters.Click
        Try
            ' تفريغ النصوص
            txtNumberBook.Clear()
            txtWillMaster.Clear()
            txtWillSub.Clear()
            txtSubjBook.Clear()

            ' إعادة تعيين القوائم المنسدلة
            If cmbYear.Items.Count > 0 Then cmbYear.SelectedIndex = 0
            If cmbReportType.Items.Count > 0 Then cmbReportType.SelectedIndex = 0

            ' إعادة تعيين إعدادات الطباعة (اختياري)
            nudCopies.Value = 1
            chkDuplex.Checked = False
            RBPrintAll.Checked = True
            If cmbTrays.Items.Count > 0 Then cmbTrays.SelectedIndex = 0

            ' إعادة تعيين نطاق التاريخ
            dtpFrom.Value = DateTime.Today.AddMonths(-1)
            dtpTo.Value = DateTime.Today
            ' 4. تفريغ التقرير الحالي من PdfViewer (إزالة البيانات)
            If pdfViewer IsNot Nothing Then

                ' إزالة المستند الحالي
                If pdfViewer.Document IsNot Nothing Then

                    pdfViewer.Document.Dispose()

                End If

                ' تفريغ العارض
                pdfViewer.Document = Nothing

                ' إعادة رسم الكنترول
                pdfViewer.Invalidate()

                pdfViewer.Refresh()

            End If

            ' إعادة تعيين الملف الحالي
            currentPdf = ""

            btnPrint.Enabled = False
            ' تحديث التقرير
            'btnPreview.PerformClick()  ' أو يمكن استدعاء Sub مخصص للتحديث مثل RefreshReport()
        Catch ex As Exception
            MessageBox.Show("خطأ في إعادة التعيين: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



#End Region

#Region "جلب البيانات"

    Private Function GetCurrentReportData() As DataTable

        Dim selectedYear As String = ""

        ' السنة المختارة
        If cmbYear.SelectedItem IsNot Nothing Then

            selectedYear =
            CType(cmbYear.SelectedItem,
                  YearItem).Year

        End If

        ' جلب البيانات
        Dim dt As DataTable =
        reportHelper.GetReportData(
            cmbReportType.Text,
            txtNumberBook.Text,
            selectedYear,
            txtWillMaster.Text,
            txtWillSub.Text,
            txtSubjBook.Text,
            dtpFrom.Value,
            dtpTo.Value)

        ' ====================================
        ' تحديد اسم الجدول للتقرير
        ' ====================================

        Select Case cmbReportType.Text

            Case "الصادر"

                dt.TableName = "TbSader"

            Case "غير المؤرشف"
                dt.TableName = "TbSader"

            Case "الأرشيف"

                dt.TableName = "TbArchive"

            Case "المحذوفات"

                dt.TableName = "TbArchive"

        End Select

        Return dt

    End Function

#End Region

#Region "مسار التقرير"

    Private Function GetReportPath() As String

        Select Case cmbReportType.Text

            Case "الصادر"

                Return IO.Path.Combine(
                    Application.StartupPath,
                    "Reports",
                    "Sader.frx")
            Case "غير المؤرشف"
                Return IO.Path.Combine(
                    Application.StartupPath,
                    "Reports",
                    "Sader.frx")  ' أو ملف مخصص للغير مؤرشف

            Case "الأرشيف"

                Return IO.Path.Combine(
                    Application.StartupPath,
                    "Reports",
                    "Archive.frx")

            Case "المحذوفات"

                Return IO.Path.Combine(
                    Application.StartupPath,
                    "Reports",
                    "Deleted.frx")

            Case Else
                Throw New ArgumentException($"نوع التقرير غير معروف: '{cmbReportType.Text}'")

        End Select

        Return ""

    End Function

#End Region

#Region "معاينة التقرير"

    Private Sub btnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click
        Try
            Dim dt As DataTable = GetCurrentReportData()
            If dt.Rows.Count = 0 Then
                MessageBox.Show("لا توجد بيانات للتقرير ⚠️", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Me.Cursor = Cursors.WaitCursor

            ' تحديد عنوان التقرير حسب النوع
            Dim titleText As String = GetReportTitle(cmbReportType.Text)

            ' استدعاء دالة ShowReport التي تقوم بالتصدير والمعاينة
            reportHelper.ShowReport(GetReportPath(), dt.TableName, dt, titleText)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Function GetReportTitle(reportType As String) As String
        Select Case reportType
            Case "الصادر"
                Return "تقرير الصادر"
            Case "غير المؤرشف"
                Return "تقرير الكتب غير المؤرشفة"
            Case "الأرشيف"
                Return "تقرير الأرشيف"
            Case "المحذوفات"
                Return "تقرير المحذوفات"
            Case Else
                Return "تقرير"
        End Select
    End Function

#End Region

#Region "تصدير PDF"

    Private Sub btnExportPdf_Click(sender As Object, e As EventArgs) Handles btnExportPdf.Click
        Try
            Dim dt As DataTable = GetCurrentReportData()
            If dt.Rows.Count = 0 Then
                MessageBox.Show("لا توجد بيانات ⚠️")
                Return
            End If

            ' استخدام الدالة المساعدة
            Dim titleText As String = GetReportTitle(cmbReportType.Text)

            currentPdf = reportHelper.ExportReportToPdf(GetReportPath(), dt, titleText)
            reportHelper.LoadPdf(pdfViewer, currentPdf)
            btnPrint.Enabled = True
            MessageBox.Show("تم إنشاء PDF بنجاح ✅")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
#End Region

#Region "طباعة"

    ' ====================================
    ' منع إدخال أي حرف غير مسموح في txtPages
    ' ====================================
    Private Sub txtPages_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPages.KeyPress
        ' المسموح: أرقام، شرطة، فاصلة، Backspace، Delete، Enter (يمكن التحكم)
        Dim allowedChars As String = "0123456789-,"

        ' إذا لم يكن الحرف في القائمة المسموحة وليس مفتاح تحكم (مثل Backspace)
        If Not allowedChars.Contains(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' منع الإدخال
        End If
    End Sub

    Private Sub txtPages_TextChanged(sender As Object, e As EventArgs) Handles txtPages.TextChanged
        ' إزالة أي أحرف غير مسموحة فوراً
        Dim original As String = txtPages.Text
        Dim filtered As New System.Text.StringBuilder()
        For Each ch As Char In original
            If "0123456789-,".Contains(ch) Then
                filtered.Append(ch)
            End If
        Next
        Dim newText As String = filtered.ToString()
        If original <> newText Then
            ' منع تكرار الحدث
            RemoveHandler txtPages.TextChanged, AddressOf txtPages_TextChanged
            txtPages.Text = newText
            txtPages.SelectionStart = newText.Length
            AddHandler txtPages.TextChanged, AddressOf txtPages_TextChanged
        End If
    End Sub

    Private Sub txtPages_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtPages.Validating
        ' خيارياً: تحقق من أن النص يتبع نمطاً معيناً مثلاً: أرقام وشرطة وفاصلة فقط
        Dim text As String = txtPages.Text.Trim()
        If Not String.IsNullOrEmpty(text) Then
            ' تحقق بسيط: يجب أن يتكون النص من أرقام أو - أو , فقط
            For Each ch As Char In text
                If Not "0123456789-,".Contains(ch) Then
                    MessageBox.Show("الرجاء إدخال أرقام وشرطة وفاصلة فقط.", "إدخال غير صالح")
                    e.Cancel = True
                    Exit Sub
                End If
            Next
        End If
    End Sub

    Private Sub txtPages_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPages.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.V Then
            ' السماح باللصق ولكن سيتم فحصه في Validating
            ' لا نمنعه هنا، بل نترك الفحص لـ Validating
        End If
    End Sub

    ' ====================================
    ' خيار الطباعة: عند تغيير الاختيار، يتم تحديث حالة txtPages (تمكين/تعطيل) ومحتواه بناءً على نوع الطباعة المختار
    ' ====================================
    Private Sub RBPrint_CheckedChanged(sender As Object, e As EventArgs) Handles RBPrintAll.CheckedChanged, RBPrintCurrent.CheckedChanged, RBPrintCustom.CheckedChanged
        GetPageRange()
    End Sub

    ' ====================================
    ' منع إدخال أي حرف غير رقمي في nudCopies
    ' ====================================
    Private Sub nudCopies_KeyPress(sender As Object, e As KeyPressEventArgs) Handles nudCopies.KeyPress
        ' السماح فقط بالأرقام ومفاتيح التحكم (Backspace, Delete, Enter...)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub nudCopies_KeyDown(sender As Object, e As KeyEventArgs) Handles nudCopies.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True ' منع اللصق
        End If
    End Sub

    Private Sub nudCopies_TextChanged(sender As Object, e As EventArgs) Handles nudCopies.TextChanged
        ' التأكد من أن القيمة عبارة عن أرقام فقط
        Dim val As String = nudCopies.Text
        If Not String.IsNullOrEmpty(val) AndAlso Not val.All(Function(c) Char.IsDigit(c)) Then
            nudCopies.Text = New String(val.Where(Function(c) Char.IsDigit(c)).ToArray())
            nudCopies.Select(nudCopies.Text.Length, 0)
        End If
    End Sub

    ' ====================================
    ' التحقق من اختيار نوع الطباعة وتحديث حالة txtPages بناءً على ذلك
    ' ====================================
    Private Function GetPageRange() As String

        If RBPrintAll.Checked Then
            txtPages.Text = ""
            txtPages.Enabled = False
            Return ""

        End If

        If RBPrintCurrent.Checked Then
            txtPages.Enabled = False
            txtPages.Text = (pdfViewer.Renderer.Page + 1).ToString()
            Return (pdfViewer.Renderer.Page + 1).ToString()

        End If

        If RBPrintCustom.Checked Then
            txtPages.Enabled = True
            Return txtPages.Text.Trim()

        End If

        Return ""

    End Function

    ' ====================================
    ' زر الطباعة: التحقق من وجود PDF، اختيار طابعة، ثم تنفيذ الطباعة باستخدام reportHelper مع تمرير الخيارات المختارة
    ' ====================================
    Private Sub btnPrint_Click(
    sender As Object,
    e As EventArgs) Handles btnPrint.Click

        Try

            If String.IsNullOrWhiteSpace(currentPdf) Then

                MessageBox.Show(
                    "قم بعرض التقرير أولاً ⚠️",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning)

                Return

            End If

            If ListPrinter.CheckedItems.Count = 0 Then

                MessageBox.Show(
                    "يرجى اختيار طابعة ⚠️",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning)

                Return

            End If

            Dim printerName As String =
                ListPrinter.CheckedItems(0).ToString()

            Dim trayName As String =
                If(cmbTrays.SelectedIndex <> -1,
                   cmbTrays.SelectedItem.ToString(),
                   String.Empty)

            reportHelper.PrintPdfAdvanced(
                currentPdf,
                printerName,
                CInt(nudCopies.Value),
                trayName,
                chkDuplex.Checked,
                GetPageRange())


            MessageBox.Show(
                "تمت الطباعة بنجاح ✅",
                "نجاح",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

        Catch ex As Exception

            MessageBox.Show(
                ex.Message,
                "خطأ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub

#End Region

End Class


