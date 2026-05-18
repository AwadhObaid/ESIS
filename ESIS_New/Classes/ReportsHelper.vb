' ============================================
' ReportsHelper.vb
' ============================================

Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports FastReport
Imports FastReport.Export.PdfSimple
Imports PdfiumViewer

Public Class ReportsHelper

#Region "عرض التقرير"
    Public Sub ShowReport(reportPath As String, tableName As String, dt As DataTable, titleText As String)
        Dim pdfPath As String = ExportReportToPdf(reportPath, dt, titleText)

        If Not IO.File.Exists(pdfPath) Then
            MessageBox.Show("فشل إنشاء التقرير")
            Return
        End If

        Dim viewer As New PdfViewer With {.Dock = DockStyle.Fill}
        viewer.Document = PdfDocument.Load(pdfPath)

        Dim f As New Form With {
        .Text = "معاينة التقرير",
        .WindowState = FormWindowState.Maximized
    }
        f.Controls.Add(viewer)
        f.ShowDialog()

        ' حذف الملف المؤقت بعد الإغلاق (اختياري)
        Try : IO.File.Delete(pdfPath) : Catch : End Try
    End Sub
#End Region

#Region "جلب البيانات"

    Public Function GetReportData(
    reportType As String,
    numberBook As String,
    yearValue As String,
    willMaster As String,
    willSub As String,
    subjBook As String,
    dateFrom As Date,
    dateTo As Date
) As DataTable

        Dim dt As New DataTable()

        Using con As SqlConnection = DatabaseHelper.GetConnection()
            con.Open()

            Dim sql As String = ""

            Select Case reportType

                Case "الصادر"

                    sql = "
                        SELECT *
                        FROM TbSader
                        WHERE
                        (
                            @NumberBook = ''
                            OR NumberBook LIKE '%' + @NumberBook + '%'
                        )

                        AND
                        (
                            @Year = ''
                            OR Year = @Year
                        )

                        AND
                        (
                            @WillMaster = ''
                            OR WillMaster LIKE '%' + @WillMaster + '%'
                        )

                        AND
                        (
                            @WillSub = ''
                            OR WillSub LIKE '%' + @WillSub + '%'
                        )

                        AND
                        (
                            @SubjBook = ''
                            OR SubjBook LIKE '%' + @SubjBook + '%'
                        )

                        AND
                        (
                            CAST(DateBook AS DATE)
                            BETWEEN @DateFrom AND @DateTo
                        )

                        ORDER BY ID DESC"

                Case "الأرشيف"
                    sql = "
                        SELECT *
                        FROM TbArchive
                        WHERE IsDeleted = 0

                        AND
                        (
                            @NumberBook = ''
                            OR ArchiveNum LIKE '%' + @NumberBook + '%'
                        )

                        AND
                        (
                            @WillMaster = ''
                            OR ArchiveWillMaster LIKE '%' + @WillMaster + '%'
                        )

                        AND
                        (
                            @WillSub = ''
                            OR ArchiveWillSub LIKE '%' + @WillSub + '%'
                        )

                        AND
                        (
                            @SubjBook = ''
                            OR ArchiveSubject LIKE '%' + @SubjBook + '%'
                        )

                        AND
                        (
                            @Year = ''
                            OR YEAR(ArchiveDate) = @Year
                        )

                        AND
                        (
                            CAST(ArchiveDate AS DATE) BETWEEN @DateFrom AND @DateTo
                        )

                        ORDER BY ID DESC"

                Case "المحذوفات"
                    sql = "SELECT *
                       FROM TbArchive
                       WHERE IsDeleted = 1
                         AND (CAST(DeletedDate AS DATE) BETWEEN @DateFrom AND @DateTo)
                       ORDER BY ID DESC"

            ' ====================================
            ' الكتب غير المؤرشفة (الصادر غير المرتبط بأرشيف)
            ' ====================================
                Case "غير المؤرشف"
                    sql = "SELECT *
                       FROM TbSader
                       WHERE ArchiveID IS NULL
                         AND (@NumberBook = '' OR NumberBook LIKE '%' + @NumberBook + '%')
                         AND (@Year = '' OR Year = @Year)
                         AND (CAST(DateBook AS DATE) BETWEEN @DateFrom AND @DateTo)
                       ORDER BY ID DESC"

            End Select

            Using da As New SqlDataAdapter(sql, con)
                da.SelectCommand.Parameters.AddWithValue(
    "@NumberBook",
    numberBook)

                da.SelectCommand.Parameters.AddWithValue(
    "@Year",
    yearValue)

                da.SelectCommand.Parameters.AddWithValue(
    "@WillMaster",
    willMaster)

                da.SelectCommand.Parameters.AddWithValue(
    "@WillSub",
    willSub)

                da.SelectCommand.Parameters.AddWithValue(
    "@SubjBook",
    subjBook)

                da.SelectCommand.Parameters.AddWithValue(
    "@DateFrom",
    dateFrom.Date)

                da.SelectCommand.Parameters.AddWithValue(
    "@DateTo",
    dateTo.Date)
                da.Fill(dt)
            End Using
        End Using

        Return dt
    End Function

#End Region

#Region "تصدير PDF"

    Public Function ExportReportToPdf(
    reportPath As String,
    dt As DataTable,
    titleText As String
) As String

        If Not IO.File.Exists(reportPath) Then

            Throw New Exception("ملف التقرير غير موجود")

        End If

        Dim tableName As String = dt.TableName

        If String.IsNullOrWhiteSpace(tableName) Then

            Throw New Exception("اسم الجدول غير موجود")

        End If

        Dim report As New Report()

        Try

            ' تحميل التقرير
            report.Load(reportPath)


            ' تسجيل البيانات
            report.RegisterData(dt, tableName)

            ' تفعيل المصدر
            Dim ds = report.GetDataSource(tableName)

            If ds Is Nothing Then

                Throw New Exception(
            "مصدر البيانات غير موجود داخل التقرير: " &
            tableName)

            End If

            ds.Enabled = True
            ' بعد تحميل التقرير وقبل report.Prepare()
            report.SetParameterValue("ReportTitle", titleText)
            report.SetParameterValue("CreatedByUser", GlobalData.LoggedInUserName)
            ' تجهيز التقرير
            report.Prepare()

            ' إنشاء PDF مؤقت
            Dim tempPdf As String =
        IO.Path.Combine(
            IO.Path.GetTempPath(),
            Guid.NewGuid().ToString() & ".pdf")

            ' تصدير
            Using export As New PDFSimpleExport()

                report.Export(export, tempPdf)

            End Using

            Return tempPdf

        Catch ex As Exception

            Throw New Exception(
        "خطأ التقرير: " & ex.Message)

        Finally

            report.Dispose()

        End Try

    End Function

#End Region

#Region "تحميل السنوات"
    Public Function GetYears() As List(Of YearItem)

        Dim list As New List(Of YearItem)

        Using con As SqlConnection =
        DatabaseHelper.GetConnection()

            con.Open()

            Dim sql As String =
            "SELECT Year, IsClosed
             FROM TbYears
             ORDER BY Year DESC"

            Using cmd As New SqlCommand(sql, con)

                Using dr As SqlDataReader =
                cmd.ExecuteReader()

                    While dr.Read()

                        list.Add(New YearItem With {
                        .Year = dr("Year").ToString(),
                        .IsClosed = Convert.ToBoolean(dr("IsClosed"))
                    })

                    End While

                End Using

            End Using

        End Using

        Return list

    End Function
#End Region

#Region "تحميل PDF"

    Public Sub LoadPdf(
    viewer As PdfViewer,
    path As String)

        Try

            ' التحقق من العنصر
            If viewer Is Nothing Then

                Throw New Exception(
                "عنصر PdfViewer غير موجود داخل الفورم")

            End If

            ' التحقق من الملف
            If Not IO.File.Exists(path) Then

                Throw New Exception(
                "ملف PDF غير موجود")

            End If

            ' التخلص من الملف السابق
            If viewer.Document IsNot Nothing Then

                viewer.Document.Dispose()

            End If

            ' تحميل PDF
            viewer.Document =
            PdfDocument.Load(path)

        Catch ex As Exception

            MessageBox.Show(
            "فشل تحميل التقرير: " & ex.Message,
            "خطأ",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)

        End Try

    End Sub

#End Region

#Region "الطابعات"

    Public Sub LoadPrinters(
        list As CheckedListBox
    )

        list.Items.Clear()

        For Each p As String In PrinterSettings.InstalledPrinters

            list.Items.Add(p)

        Next

    End Sub

#End Region

#Region "الطباعة"

    Public Sub PrintPdf(
        pdfPath As String,
        printerName As String,
        copies As Integer,
        trayName As String
    )

        If Not IO.File.Exists(pdfPath) Then Exit Sub

        Using pdfDoc As PdfDocument =
            PdfDocument.Load(pdfPath)

            Using printDoc As PrintDocument =
                pdfDoc.CreatePrintDocument()

                printDoc.PrinterSettings.PrinterName =
                    printerName

                printDoc.PrinterSettings.Copies =
                    CShort(copies)

                For Each p As PaperSize In
                    printDoc.PrinterSettings.PaperSizes

                    If p.Kind = PaperKind.A4 Then

                        printDoc.DefaultPageSettings.PaperSize = p

                        Exit For

                    End If

                Next

                If Not String.IsNullOrWhiteSpace(trayName) Then

                    For Each src As PaperSource In
                        printDoc.PrinterSettings.PaperSources

                        If src.SourceName = trayName Then

                            printDoc.DefaultPageSettings.PaperSource = src

                            Exit For

                        End If

                    Next

                End If

                printDoc.PrintController =
                    New StandardPrintController()

                printDoc.Print()

            End Using

        End Using

    End Sub

#End Region

#Region "أدراج الطابعة"
    Public Sub PrintPdfAdvanced(
    pdfPath As String,
    printerName As String,
    copies As Integer,
    trayName As String,
    enableDuplex As Boolean,
    pageRange As String
)

        If Not IO.File.Exists(pdfPath) Then Exit Sub

        Using pdfDoc As PdfDocument =
        PdfDocument.Load(pdfPath)

            Using printDoc As PrintDocument =
            pdfDoc.CreatePrintDocument()

                ' ====================================
                ' إعدادات الطابعة
                ' ====================================

                printDoc.PrinterSettings.PrinterName =
                printerName

                printDoc.PrinterSettings.Copies =
                CShort(copies)

                ' ====================================
                ' حجم الورق A4
                ' ====================================

                For Each p As PaperSize In
                printDoc.PrinterSettings.PaperSizes

                    If p.Kind = PaperKind.A4 Then

                        printDoc.DefaultPageSettings.PaperSize = p

                        Exit For

                    End If

                Next

                ' ====================================
                ' درج الطابعة
                ' ====================================

                If Not String.IsNullOrWhiteSpace(trayName) Then

                    For Each src As PaperSource In
                    printDoc.PrinterSettings.PaperSources

                        If src.SourceName.Equals(
                        trayName,
                        StringComparison.OrdinalIgnoreCase) Then

                            printDoc.DefaultPageSettings.PaperSource = src

                            Exit For

                        End If

                    Next

                End If

                ' ====================================
                ' الطباعة على الوجهين
                ' ====================================

                If enableDuplex Then

                    If printDoc.PrinterSettings.CanDuplex Then

                        printDoc.PrinterSettings.Duplex =
                        Duplex.Default

                    Else

                        MessageBox.Show(
                        "هذه الطابعة لا تدعم الطباعة على الوجهين",
                        "تنبيه",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)

                    End If

                Else

                    printDoc.PrinterSettings.Duplex =
                    Duplex.Simplex

                End If

                ' ====================================
                ' اتجاه الصفحة
                ' ====================================

                printDoc.DefaultPageSettings.Landscape =
                False

                ' ====================================
                ' طباعة صامتة
                ' ====================================

                printDoc.PrintController =
                New StandardPrintController()

                ' ====================================
                ' الصفحات المخصصة
                ' ====================================

                If Not String.IsNullOrWhiteSpace(pageRange) Then

                    printDoc.PrinterSettings.PrintRange =
        PrintRange.SomePages

                    Dim parts() = pageRange.Split("-"c)

                    If parts.Length = 2 Then

                        printDoc.PrinterSettings.FromPage =
            Integer.Parse(parts(0))

                        printDoc.PrinterSettings.ToPage =
            Integer.Parse(parts(1))

                    Else

                        Dim singlePage As Integer =
            Integer.Parse(pageRange)

                        printDoc.PrinterSettings.FromPage =
            singlePage

                        printDoc.PrinterSettings.ToPage =
            singlePage

                    End If

                Else

                    printDoc.PrinterSettings.PrintRange =
        PrintRange.AllPages

                End If

                ' ====================================
                ' تنفيذ الطباعة
                ' ====================================

                printDoc.Print()

            End Using

        End Using

    End Sub

    Public Sub LoadPrinterTrays(
    cmb As ComboBox,
    printerName As String)

        cmb.Items.Clear()

        Dim pd As New PrintDocument()

        pd.PrinterSettings.PrinterName = printerName

        For Each src As PaperSource In
            pd.PrinterSettings.PaperSources

            cmb.Items.Add(src.SourceName)

        Next

        If cmb.Items.Count > 0 Then
            cmb.SelectedIndex = 0
        End If

    End Sub

#End Region

#Region "توليد PDF"

    Public Function GenerateReportPdf(
       reportPath As String,
       tableName As String,
       dt As DataTable
   ) As String

        dt.TableName = tableName

        Dim report As New Report()

        report.Load(reportPath)

        report.RegisterData(dt, tableName)

        report.GetDataSource(tableName).Enabled = True

        report.Prepare()

        Dim tempPdf As String =
            IO.Path.Combine(
                IO.Path.GetTempPath(),
                Guid.NewGuid().ToString() & ".pdf")

        Using export As New PDFSimpleExport()

            report.Export(export, tempPdf)

        End Using

        report.Dispose()

        Return tempPdf

    End Function

    Public Sub ShowPdfPreview(pdfPath As String)

        Using pdfDoc As PdfDocument =
            PdfDocument.Load(pdfPath)

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

#End Region

End Class

Public Class YearItem

    Public Property Year As String

    Public Property IsClosed As Boolean

    Public Overrides Function ToString() As String

        Return Year &
            If(IsClosed,
               " - مغلقة 🔒",
               " - مفتوحة ✅")

    End Function

End Class